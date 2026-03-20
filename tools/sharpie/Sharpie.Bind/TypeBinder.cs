// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Types;

namespace Sharpie.Bind;

public class TypeBinder : AstVisitor {
	readonly Stack<TypedefType> typedefs = new Stack<TypedefType> ();
	readonly Stack<AstType> resolvedTypes = new Stack<AstType> ();

	public AstType? Type { get; private set; }
	public AstType? ParameterType { get; private set; }
	public ParameterModifier ParameterModifier { get; private set; }

	public TypeBinder (BindingResult bindingResult)
		: base (bindingResult)
	{
	}

	public void Resolve ()
	{
		Type = null;
		ParameterModifier = ParameterModifier.None;

		if (resolvedTypes.Count == 0)
			return;

		if (resolvedTypes.Count > 1) {
			string? stack = null;
			var i = 0;
			foreach (var type in resolvedTypes)
				stack += String.Format ("\n    {0}: {1} -> {2}", i++, type.GetType (), type);

			throw new InvalidOperationException ("should not be reached: " +
				"there should be exactly one type remaining on the resolve stack:" + stack);
		}

		Type = resolvedTypes.Pop ();

		var composedType = Type as ComposedType;
		if (composedType is not null &&
			composedType.PointerRank == 1 &&
			composedType.BaseType.Annotation<ObjCObjectType> () is not null) {
			ParameterType = composedType.BaseType.Clone ();
			ParameterModifier = ParameterModifier.Out;
		} else {
			ParameterType = Type.Clone ();
		}
	}

	void ResolvePrimitive (string keyword, ClangSharp.Type nativeType)
	{
		Resolve (new PrimitiveType (keyword), nativeType);
	}

	void Resolve (AstType astType, ClangSharp.Type nativeType)
	{
		typedefs.Clear ();

		resolvedTypes.Push (astType);
		if (nativeType is not null)
			astType.AddAnnotation (nativeType);
	}

	public override void VisitType (ClangSharp.Type type)
	{
		Resolve (new UnsupportedType (type), type);
	}

	public override void VisitBuiltinType (BuiltinType type)
	{
		switch (type.Kind) {
		case CXTypeKind.CXType_Void:
			Resolve (new VoidType (), type);
			break;
		case CXTypeKind.CXType_Bool:
			ResolvePrimitive ("bool", type);
			break;
		case CXTypeKind.CXType_Char_S:
		case CXTypeKind.CXType_SChar:
			ResolvePrimitive ("sbyte", type);
			break;
		case CXTypeKind.CXType_UChar:
			ResolvePrimitive ("byte", type);
			break;
		case CXTypeKind.CXType_Short:
			ResolvePrimitive ("short", type);
			break;
		case CXTypeKind.CXType_UShort:
			ResolvePrimitive ("ushort", type);
			break;
		case CXTypeKind.CXType_Int:
			ResolvePrimitive ("int", type);
			break;
		case CXTypeKind.CXType_UInt:
			ResolvePrimitive ("uint", type);
			break;
		case CXTypeKind.CXType_Long:
			Resolve (new NIntType (), type);
			break;
		case CXTypeKind.CXType_ULong:
			Resolve (new NUIntType (), type);
			break;
		case CXTypeKind.CXType_LongLong:
			ResolvePrimitive ("long", type);
			break;
		case CXTypeKind.CXType_ULongLong:
			ResolvePrimitive ("ulong", type);
			break;
		case CXTypeKind.CXType_Float:
		case CXTypeKind.CXType_Double:
			if (typedefs.Count == 1 && typedefs.Peek ().Decl.Name == "CGFloat")
				Resolve (new NFloatType (), type);
			else if (type.Kind == CXTypeKind.CXType_Float)
				ResolvePrimitive ("float", type);
			else
				ResolvePrimitive ("double", type);
			break;
		case CXTypeKind.CXType_LongDouble:
			ResolvePrimitive ("decimal", type);
			break;
		case CXTypeKind.CXType_ObjCId:
			Resolve (new ObjCIdType (), type);
			break;
		case CXTypeKind.CXType_ObjCClass:
			Resolve (new ObjCClassType (), type);
			break;
		case CXTypeKind.CXType_ObjCSel:
			Resolve (new ObjCSelectorType (), type);
			break;
		default:
			VisitType (type); // unsupported
			break;
		}
	}

	public override void VisitTypedefType (TypedefType type)
	{
		if (type.IsObjCInstanceType) {
			Resolve (new ObjCInstanceTypeType (), type);
			return;
		}

		var canonicalType = type.Decl.UnderlyingType;
		var declName = type.Decl.Name;
		switch (declName) {
		case "BOOL":
		case "GLboolean":
			if (canonicalType is ElaboratedType elaboratedType)
				canonicalType = elaboratedType.CanonicalType;
			switch ((canonicalType as BuiltinType)?.Kind) {
			case CXTypeKind.CXType_Bool:
			case CXTypeKind.CXType_SChar:
			case CXTypeKind.CXType_UChar:
			case CXTypeKind.CXType_Char_S:
				ResolvePrimitive ("bool", type);
				return;
			}
			BindingResult.ReportUnsupportedConstruct (type.Decl.Location, $"Typedef '{declName}' does not map to a known underlying type");
			break;
		case "intptr_t":
		case "uintptr_t":
		case "ptrdiff_t": {
			if (canonicalType is ElaboratedType elaboratedType2)
				canonicalType = elaboratedType2.CanonicalType;

			if (canonicalType is BuiltinType builtinType) {
				switch (builtinType.Kind) {
				case CXTypeKind.CXType_ULong:
					Resolve (new UIntPtrType (), type);
					return;
				case CXTypeKind.CXType_Long:
					Resolve (new IntPtrType (), type);
					return;
				}
			}
			BindingResult.ReportUnsupportedConstruct (type.Decl.Location, $"Typedef '{declName}' does not map to a known underlying type");
			break;
		}
		case "dispatch_object_t":
			Resolve (AstType.Create ("CoreFoundation.DispatchObject"), type);
			return;
		case "dispatch_queue_t":
			Resolve (AstType.Create ("CoreFoundation.DispatchQueue"), type);
			return;
		case "dispatch_group_t":
			Resolve (AstType.Create ("CoreFoundation.DispatchGroup"), type);
			return;
		case "dispatch_data_t":
			Resolve (AstType.Create ("CoreFoundation.DispatchData"), type);
			return;
		case "sec_trust_t":
			Resolve (AstType.Create ($"Security.SecTrust2"), type);
			return;
		case "sec_identity_t":
			Resolve (AstType.Create ($"Security.SecIdentity2"), type);
			return;
		case "sec_protocol_options_t":
			Resolve (AstType.Create ($"Security.SecProtocolOptions"), type);
			return;
		case "sec_protocol_metadata_t":
			Resolve (AstType.Create ($"Security.SecProtocolMetadata"), type);
			return;
		case "MIDIEndpointRef":
			Resolve (AstType.Create ($"CoreMidi.MidiEndpoint"), type);
			return;
		case "CGColorRef":
		case "CGPathRef":
		case "CGGradientRef":
		case "CGContextRef":
		case "CGImageRef":
		case "CGColorSpaceRef":
		case "CGPDFDocumentRef":
		case "CGPDFPageRef":
		case "CGLayerRef":
			Resolve (AstType.Create ($"CoreGraphics.{RemoveRefSuffix (declName)}"), type);
			return;
		case "CGImageSourceRef":
			Resolve (AstType.Create ($"ImageIO.{RemoveRefSuffix (declName)}"), type);
			return;
		case "CFRunLoopRef":
			Resolve (AstType.Create ($"CoreFoundation.{RemoveRefSuffix (declName)}"), type);
			return;
		case "CMTimebaseRef":
		case "CMClockRef":
		case "CMSampleBufferRef":
		case "CMFormatDescriptionRef":
		case "CMAudioFormatDescriptionRef":
		case "CMVideoFormatDescriptionRef":
			Resolve (AstType.Create ($"CoreMedia.{RemoveRefSuffix (declName)}"), type);
			return;
		case "CVImageBufferRef":
		case "CVPixelBufferRef":
			Resolve (AstType.Create ($"CoreVideo.{RemoveRefSuffix (declName)}"), type);
			return;
		case "SecIdentityRef":
		case "SecTrustRef":
		case "SecAccessControlRef":
			Resolve (AstType.Create ($"Security.{RemoveRefSuffix (declName)}"), type);
			return;
		case "NSPoint":
		case "NSRect":
		case "NSSize":
			// bind to the canonical CoreGraphics type instead of the AppKit typedef
			// alias since the Xamarin bindings do not provide the NS aliases.
			var underlyingRecord = type.Decl.UnderlyingType.CanonicalType as RecordType;
			if (underlyingRecord?.Decl.Name?.Replace ("CG", "NS") == declName) {
				type.Decl.UnderlyingType.Accept (this);
				return;
			}
			break;
		}

		typedefs.Push (type);
		type.Decl.UnderlyingType.Accept (this);

		// Turns CGColorSpaceRef into CGColorSpace
		string RemoveRefSuffix (string refName) => refName.Substring (0, refName.Length - 3);
	}

	public override void VisitAttributedType (AttributedType type)
	{
		type.ModifiedType.Accept (this);
		Resolve (resolvedTypes.Pop (), type);
	}

	public override void VisitParenType (ParenType type)
	{
		type.InnerType.Accept (this);
	}

	public override void VisitPointerType (PointerType type)
	{
		// regular pointers we keep track of on the Resolve () stack
		type.PointeeType.Accept (this);

		// much like with Objective-C object types, the first rank pointer is
		// implied in the binding, so we drop it. SEL is actually a pointer
		// to a struct (typedef struct objc_selector *SEL), so drop the pointer
		var resolved = resolvedTypes.Pop ();
		if ((type.PointeeType as BuiltinType)?.Kind != CXTypeKind.CXType_ObjCSel)
			resolved = resolved.MakePointerType ();

		Resolve (resolved, type);
	}

	public override void VisitBlockPointerType (BlockPointerType type)
	{
		// do not Resolve () the pointer as it's special
		type.PointeeType.Accept (this);
	}

	public override void VisitObjCObjectPointerType (ObjCObjectPointerType type)
	{
		// do not Resolve () the pointer as it's special
		type.PointeeType.Accept (this);
	}

	public override void VisitObjCObjectType (ObjCObjectType type)
	{
		AstType astType;

		if (type.Interface is null) {
			switch ((type.BaseType as BuiltinType)?.Kind) {
			case CXTypeKind.CXType_ObjCClass:
				astType = new ObjCClassType ();
				break;
			case CXTypeKind.CXType_ObjCId:
				astType = new ObjCIdType ();
				break;
			default:
				Debug.Assert (false, "should not be reached");
				VisitType (type); // unsupported
				return;
			}
		} else {
			astType = AstTypeWithDecl (type.Interface, type.Interface.Name);
		}

		switch (type.Protocols.Count) {
		case 0:
			break;
		case 1:
			astType = AstType.Create (type.Protocols.First ().Name);
			break;
		default:
			var protocolPrefix = type.Interface is not null && type.Interface.TypeParamList.Any ()
				? "PROTOCOL: "
				: String.Empty;
			astType.GetChildrenByRole<AstType> (Roles.TypeArgument).AddRange (
				type.Protocols.Select (proto => {
					var protoType = AstType.Create (protocolPrefix + proto.Name);
					protoType.AddAnnotation (proto);
					return protoType;
				})
			);
			break;
		}

		foreach (var typeArg in type.TypeArgs) {
			typeArg.Accept (this);
			astType.GetChildrenByRole<AstType> (Roles.TypeArgument).Add (resolvedTypes.Pop ());
		}

		Resolve (astType, type);
	}

	public override void VisitObjCTypeParamType (ObjCTypeParamType type)
	{
		type.Decl.UnderlyingType.Accept (this);
	}

	public override void VisitEnumType (EnumType type)
	{
		BindEnumOrRecordType (type, type.Decl);
	}

	public override void VisitRecordType (RecordType type)
	{
		BindEnumOrRecordType (type, type.Decl);
	}

	void BindEnumOrRecordType (ClangSharp.Type type, NamedDecl decl)
	{
		// use the outer-most typedef alias for structs/unions/enums
		var name = decl.Name;
		while (typedefs.Count > 0)
			name = typedefs.Pop ().Decl.Name;

		if (decl.GetIsUnnamedOrAnonymous (BindingResult))
			Resolve (new AnonymousType (type), type);
		else
			Resolve (AstTypeWithDecl (decl, name), type);
	}

	public override void VisitElaboratedType (ElaboratedType type)
	{
		if (type.IsSugared) {
			// the elaborated type might have sugar, this means that we need to remove
			// the sugar and visit the type again. This is needed for example for
			// elaborated types that are pointers to anonymous structs, which are
			// represented as elaborated types with sugar.

			var desugar = type.Desugar;
			if (desugar is not null) {
				switch (desugar) {
				case ComplexType complexType:
					VisitComplexType (complexType);
					return;
				case TypedefType typedefType:
					VisitTypedefType (typedefType);
					return;
				case PointerType pointerType:
					VisitPointerType (pointerType);
					return;
				case RecordType recordType:
					VisitRecordType (recordType);
					return;
				case EnumType enumType:
					VisitEnumType (enumType);
					return;
				case ElaboratedType elaboratedType:
					VisitElaboratedType (elaboratedType);
					return;
				case BuiltinType builtinType:
					VisitBuiltinType (builtinType);
					return;
				case BlockPointerType blockPointerType:
					VisitBlockPointerType (blockPointerType);
					return;
				case ObjCObjectPointerType objcObjectPointerType:
					VisitObjCObjectPointerType (objcObjectPointerType);
					return;
				case ObjCObjectType objcObjectType:
					VisitObjCObjectType (objcObjectType);
					return;
				default:
					Console.Error.WriteLine ($"UNSUPPORTED SUGAR: {desugar.GetType ()} {desugar}");
					VisitType (type); // unsupported
					return;
				}
			} else {
				VisitType (type); // unsupported
			}
		} else {
			if (type.NamedType is EnumType enumType) {
				enumType.Accept (this);
			} else if (type.NamedType is RecordType recordType) {
				recordType.Accept (this);
			} else {
				VisitType (type); // unsupported
			}
		}
	}

	public override void VisitMacroQualifiedType (MacroQualifiedType type)
	{
		type.ModifiedType.Accept (this);
	}

	public override void VisitFunctionType (FunctionType type)
	{
		if (typedefs.Count > 0) {
			Resolve (new CustomDelegateType (typedefs.Peek ().Decl.Name), type);
			return;
		}

		var delegateArgTypes = new List<AstType> ();

		var proto = type as FunctionProtoType;
		if (proto is not null) {
			foreach (var paramType in proto.ParamTypes) {
				paramType.Accept (this);
				delegateArgTypes.Add (resolvedTypes.Pop ());
			}

			if (proto.IsVariadic)
				delegateArgTypes.Add (new IntPtrType ());
		}

		type.ReturnType.Accept (this);
		var resolvedReturnType = resolvedTypes.Pop ();

		if (resolvedReturnType is VoidType) {
			Resolve (new ActionType (delegateArgTypes), type);
			return;
		}

		delegateArgTypes.Add (resolvedReturnType);
		Resolve (new FuncType (delegateArgTypes), type);
	}

	public override void VisitArrayType (ArrayType type)
	{
		type.ElementType.Accept (this);
		Resolve (resolvedTypes.Pop ().MakeArrayType (), type);
	}

	public override void VisitDecayedType (DecayedType type)
	{
		type.OriginalType.Accept (this);
	}

	public override void VisitComplexType (ComplexType type)
	{
		Resolve (AstType.Create ("System.Numerics.Complex"), type);
	}

	static AstType AstTypeWithDecl (Decl decl, string name)
	{
		var module = GetTopLevelModuleName (decl);
		if (module is null)
			return AstType.Create (name);

		return AstType.Create (module + "." + name);
	}

	static string? GetTopLevelModuleName (Decl decl)
	{
		if (decl is null)
			return null;

		if (!decl.TryGetPresumedLoc (out var presumedLoc))
			return null;

		var path = presumedLoc?.FileName;
		if (path is null)
			return null;

		var ofs = path.IndexOf (".framework/", StringComparison.Ordinal);
		if (ofs < 0)
			return null;

		return Path.GetFileName (path.Substring (0, ofs));
	}
}
