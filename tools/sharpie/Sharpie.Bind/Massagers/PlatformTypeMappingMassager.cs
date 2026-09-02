// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

[RegisterBefore (typeof (GenerateUsingStatementsMassager))]
public sealed class PlatformTypeMappingMassager : Massager<PlatformTypeMappingMassager> {
	readonly Dictionary<string, (string FullName, string Namespace, string Name)> typeMap = new ();
	readonly HashSet<string> protocolEntries = new ();
	readonly Stack<bool> ignoreType = new Stack<bool> ();

	public PlatformTypeMappingMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override bool Initialize ()
	{
		typeMap.Clear ();
		protocolEntries.Clear ();

		var path = base.Binder.PlatformAssembly;
		var decoder = new TypelessDecoder ();

		using var fs = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using var peReader = new PEReader (fs);
		var mr = peReader.GetMetadataReader ();
		foreach (var etHandle in mr.TypeDefinitions) {
			// Look for non-nested public types with a [Register] or [Protocol] attribute
			var et = mr.GetTypeDefinition (etHandle);
			var isPublic = et.Attributes.HasFlag (TypeAttributes.Public);
			var isNested = (et.Attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public;
			if (!isPublic || isNested)
				continue;

			var foundAttribute = false;
			var nativeName = (string?) null;
			bool isRegisterAttribute = false;
			bool isProtocolAttribute = false;
			foreach (var attribHandle in et.GetCustomAttributes ()) {
				var attrib = mr.GetCustomAttribute (attribHandle);
				isRegisterAttribute = IsDeclaringType (mr, attrib.Constructor, "Foundation", "RegisterAttribute");
				isProtocolAttribute = !isRegisterAttribute && IsDeclaringType (mr, attrib.Constructor, "Foundation", "ProtocolAttribute");
				if (!(isRegisterAttribute || isProtocolAttribute))
					continue;

				var ctor = mr.GetMethodDefinition ((MethodDefinitionHandle) attrib.Constructor);
				foundAttribute = true;

				var decoded = attrib.DecodeValue (decoder);
				if (isRegisterAttribute) {
					if (decoded.FixedArguments.Length > 0)
						nativeName = decoded.FixedArguments [0].Value as string;
				} else if (isProtocolAttribute) {
					var args = decoded.NamedArguments
						.Where (na => na.Name == "Name");
					if (args.Count () == 1) {
						var arg = args.First ();
						nativeName = arg.Value as string;
					}
				}
				break;
			}
			if (!foundAttribute)
				continue;

			var etNamespace = mr.GetString (et.Namespace);
			var etName = mr.GetString (et.Name);
			nativeName ??= etName;

			var entry = (etNamespace + "." + etName, etNamespace, etName);
			if (typeMap.ContainsKey (nativeName)) {
				// When two types map to the same native name, prefer the
				// standard protocol interface (named "I" + nativeName, e.g.
				// INSCopying for "NSCopying") over a [Model] class or a
				// non-standard protocol. If neither or both follow the
				// convention, drop both (genuine ambiguity).
				bool newIsStandardProtocol = isProtocolAttribute && etName == "I" + nativeName;
				bool existingIsStandardProtocol = protocolEntries.Contains (nativeName);
				if (newIsStandardProtocol && !existingIsStandardProtocol) {
					typeMap [nativeName] = entry;
					protocolEntries.Add (nativeName);
				} else if (!newIsStandardProtocol && existingIsStandardProtocol) {
					// existing is the standard protocol, keep it
				} else {
					// genuine collision, drop both
					typeMap.Remove (nativeName);
					protocolEntries.Remove (nativeName);
				}
			} else {
				typeMap.Add (nativeName, entry);
				if (isProtocolAttribute && etName == "I" + nativeName)
					protocolEntries.Add (nativeName);
			}
		}

		return typeMap.Count > 0;
	}

	// This is a very minimal implementation of ICustomAttributeTypeProvider
	// that only supports the operations we need to decode the RegisterAttribute
	// constructor arguments.
	class TypelessDecoder : ICustomAttributeTypeProvider<object> {
		public object GetPrimitiveType (PrimitiveTypeCode typeCode)
		{
			if (typeCode == PrimitiveTypeCode.String)
				return typeof (string);
			else if (typeCode == PrimitiveTypeCode.Boolean)
				return typeof (bool);
			else if (typeCode == PrimitiveTypeCode.Int32)
				return typeof (int);

			throw new InvalidOperationException (string.Format ("TypelessDecoder: Unsupported action: GetPrimitiveType ({0})", typeCode));
		}

		public object GetSystemType ()
		{
			return typeof (System.Type);
		}

		public object GetSZArrayType (object elementType)
		{
			throw new InvalidOperationException (string.Format ("TypelessDecoder: Unsupported action: GetSZArrayType ({0})", elementType));
		}

		public object GetTypeFromDefinition (MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
		{
			throw new InvalidOperationException ("TypelessDecoder: Unsupported action: GetTypeFromDefinition");
		}

		public object GetTypeFromReference (MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
		{
			throw new InvalidOperationException ("TypelessDecoder: Unsupported action: GetTypeFromReference");
		}

		public object GetTypeFromSerializedName (string name)
		{
			return new TypeInAttribute { TypeName = name };
		}

		public PrimitiveTypeCode GetUnderlyingEnumType (object type)
		{
			throw new InvalidOperationException ("TypelessDecoder: Unsupported action: GetUnderlyingEnumType");
		}

		public bool IsSystemType (object type)
		{
			throw new InvalidOperationException ("TypelessDecoder: Unsupported action: IsSystemType");
		}

		class TypeInAttribute {
			public required string TypeName;
		}
	}

	public static bool IsDeclaringType (MetadataReader reader, EntityHandle method, string ns, string name)
	{
		switch (method.Kind) {
		case HandleKind.MethodDefinition:
			var mdh = (MethodDefinitionHandle) method;
			var md = reader.GetMethodDefinition (mdh);
			return IsType (md.GetDeclaringType (), reader, ns, name);
		case HandleKind.MemberReference:
			var mref = reader.GetMemberReference ((MemberReferenceHandle) method);
			switch (mref.Parent.Kind) {
			case HandleKind.TypeReference:
				return IsType ((TypeReferenceHandle) mref.Parent, reader, ns, name);
			case HandleKind.TypeDefinition:
				return IsType ((TypeDefinitionHandle) mref.Parent, reader, ns, name);
			case HandleKind.TypeSpecification:
				// This can never happen for the types we're interested in (Foundation.ProtocolAttribute)
				return false;
			}
			throw new InvalidOperationException (string.Format ("Unsupported parent kind '{0}' for the attribute's constructor.", mref.Parent.Kind));
		default:
			throw new InvalidOperationException (string.Format ("Unsupported kind '{0}' for the attribute's constructor.", method.Kind));
		}
	}

	static bool IsType (TypeDefinitionHandle typeHandle, MetadataReader reader, string ns, string name)
	{
		var type = reader.GetTypeDefinition (typeHandle);
		if (reader.GetString (type.Name) != name)
			return false;
		if (reader.GetString (type.Namespace) != ns)
			return false;
		return true;
	}

	static bool IsType (TypeReferenceHandle typeHandle, MetadataReader reader, string ns, string name)
	{
		var type = reader.GetTypeReference (typeHandle);
		if (reader.GetString (type.Name) != name)
			return false;
		if (reader.GetString (type.Namespace) != ns)
			return false;
		return true;
	}

	public override void VisitAttribute (ICSharpCode.NRefactory.CSharp.Attribute attribute)
	{
		// types in attributes are well known as the infrastructure for defining
		// the binding itself and are not to be mapped as native types, with the
		// exception of types inside a typeof() expression (handled below).
		ignoreType.Push (true);
		base.VisitAttribute (attribute);
		ignoreType.Pop ();
	}

	public override void VisitTypeOfExpression (TypeOfExpression typeOfExpression)
	{
		// invert the ignore state when visiting typeof(). For example, in:
		// [BaseType (typeof (NSURL))] we ignore BaseType since we're inside an
		// attribute, but invert to map NSURL inside the typeof(), which is also
		// inside the attribute.
		ignoreType.Push (false);
		base.VisitTypeOfExpression (typeOfExpression);
		ignoreType.Pop ();
	}

	public override void VisitMemberType (MemberType memberType)
	{
		// Visit children first so that type arguments (e.g. inside Action<NSData, NSURLResponse>)
		// are mapped before the parent type is processed.
		base.VisitMemberType (memberType);
		VisitType (memberType, memberType.MemberName);
	}

	public override void VisitSimpleType (SimpleType simpleType)
	{
		base.VisitSimpleType (simpleType);
		VisitType (simpleType, simpleType.Identifier);
	}

	void VisitType (AstType type, string nativeTypeName)
	{
		if (HasVisited (type))
			return;

		MarkVisited (type);

		if (typeMap.Count == 0)
			return;

		if (ignoreType.TryPeek (out var topIgnoreType) && topIgnoreType)
			return;

		// if the type was bound from an ObjCProtocol in the context of creating
		// the bound interface, it will be prefixed with "I" as per the new
		// protocol support; we need to strip that off to look up the native name
		var proto = type.Annotation<ObjCProtocolDecl> ();
		if (proto is not null && type.Parent is TypeDeclaration) {
			if (!nativeTypeName.StartsWith ('I'))
				throw new InvalidOperationException ();
			nativeTypeName = nativeTypeName.Substring (1);
		}

		if (typeMap.TryGetValue (nativeTypeName, out var xType)) {
			var xName = xType.FullName;
			var backtickIndex = xName.IndexOf ('`');
			// if it's a generic type, strip off the `1 suffix
			if (backtickIndex > 0)
				xName = xName [..backtickIndex];

			var newType = AstType.Create (xName);

			// copy over the annotations from the old type since we'll be replacing it
			foreach (var annotation in type.Annotations)
				newType.AddAnnotation (annotation);

			type.GetChildrenByRole (Roles.TypeArgument).MoveTo (newType.GetChildrenByRole (Roles.TypeArgument));

			type.CopyAnnotationsTo (newType);
			type.ReplaceWith (newType);
		}
	}
}
