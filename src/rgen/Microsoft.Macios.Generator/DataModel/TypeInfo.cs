// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Readonly structure that represents a change in a method return type.
/// </summary>
[StructLayout (LayoutKind.Auto)]
readonly partial struct TypeInfo : IEquatable<TypeInfo> {

	public static TypeInfo Void = new ("void", SpecialType.System_Void) { Parents = ["System.ValueType", "object"], };

	/// <summary>
	/// The fully qualified name of the type.
	/// </summary>
	public string FullyQualifiedName { get; init; } = string.Empty;

	/// <summary>
	/// Type name.
	/// </summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// The namespace of the type, split by '.'.
	/// </summary>
	public ImmutableArray<string> Namespace { get; init; } = [];

	/// <summary>
	/// The metadata name of the type. This is normally the same as name except
	/// when the SpecialType is not None.
	/// </summary>
	public string? MetadataName { get; init; }

	/// <summary>
	/// If the type is an enum, it returns the special type of the underlying type.
	/// </summary>
	public SpecialType? EnumUnderlyingType { get; init; }

	/// <summary>
	/// If the type is an enum type.
	/// </summary>
	[MemberNotNullWhen (true, nameof (EnumUnderlyingType))]
	public bool IsEnum => EnumUnderlyingType is not null;

	/// <summary>
	/// The special type enum of the type info. This is used to differentiate nint from IntPtr and other.
	/// </summary>
	public SpecialType SpecialType { get; init; } = SpecialType.None;

	/// <summary>
	/// True if the parameter is nullable.
	/// </summary>
	public bool IsNullable { get; init; }

	/// <summary>
	/// True if the parameter type is blittable.
	/// </summary>
	public bool IsBlittable { get; }

	/// <summary>
	/// Returns if the return type is a smart enum.
	/// </summary>
	public bool IsSmartEnum { get; }

	/// <summary>
	/// If the type is an array, it returns the special type of the underlying type.
	/// </summary>
	public SpecialType? ArrayElementType { get; init; }

	/// <summary>
	/// Returns if the return type is an array type.
	/// </summary>
	[MemberNotNullWhen (true, nameof (ArrayElementType))]
	public bool IsArray { get; init; }

	/// <summary>
	/// Returns if the return type is a reference type.
	/// </summary>
	public bool IsReferenceType { get; }

	/// <summary>
	/// Returns if the type is a struct.
	/// </summary>
	public bool IsStruct { get; }

	/// <summary>
	/// Returns if the return type is void.
	/// </summary>
	public bool IsVoid => SpecialType == SpecialType.System_Void;

	/// <summary>
	/// True if the type is for an interface.
	/// </summary>
	public bool IsInterface { get; init; }

	/// <summary>
	/// True if the type represents an integer that was built using one of the keywords, like byte, int, nint etc.
	///
	/// This can be used to decide if we should use the name of the metadata name to cast the value.
	/// </summary>
	public bool IsNativeIntegerType { get; init; }

	/// <summary>
	/// True if an enumerator was marked with the NativeAttribute.
	/// </summary>
	public bool IsNativeEnum { get; init; }

	/// <summary>
	/// Return if the type represents a wrapped object from the objc world.
	/// </summary>
	public bool IsWrapped { get; init; }

	/// <summary>
	/// Returns, if the type is an array, if its elements are a wrapped object from the objc world.
	/// </summary>
	public bool ArrayElementTypeIsWrapped { get; init; }

	/// <summary>
	/// Returns, if the type is an array, if its elements implement the INativeObject interface.
	/// </summary>
	public bool ArrayElementIsINativeObject { get; init; }

	readonly bool isNSObject = false;

	/// <summary>
	/// True if the type represents a NSObject.
	/// </summary>
	public bool IsNSObject {
		get => isNSObject;
		init => isNSObject = value;
	}

	readonly bool isINativeObject = false;

	/// <summary>
	/// True if the type implements INativeObject.
	/// </summary>
	public bool IsINativeObject {
		get => isINativeObject;
		init => isINativeObject = value;
	}

	readonly bool isDictionaryContainer = false;

	/// <summary>
	/// True if the type inherits from the DictionaryContainer class.
	/// </summary>
	public bool IsDictionaryContainer {
		get => isDictionaryContainer;
		init => isDictionaryContainer = value;
	}

	/// <summary>
	/// True if the type represents a delegate.
	/// </summary>
	[MemberNotNullWhen (true, nameof (Delegate))]
	public bool IsDelegate { get; init; }

	/// <summary>
	/// If the parameter is a delegate. The method information of the invoke.
	/// </summary>
	public DelegateInfo? Delegate { get; init; } = null;

	/// <summary>
	/// If the type is a pointer type.
	/// </summary>
	public bool IsPointer { get; init; }

	/// <summary>
	/// True if the symbol represents a generic type.
	/// </summary>
	public bool IsGenericType { get; init; }

	/// <summary>
	/// True if the type represents a ObjC protocol.
	/// </summary>
	public bool IsProtocol { get; init; }

	readonly ImmutableArray<string> parents = [];
	/// <summary>
	/// Array of the parent types of the type.
	/// </summary>
	public ImmutableArray<string> Parents {
		get => parents;
		init => parents = value;
	}

	readonly ImmutableArray<string> interfaces = [];

	/// <summary>
	/// Array of the implemented interfaces by the type.
	/// </summary>
	public ImmutableArray<string> Interfaces {
		get => interfaces;
		init => interfaces = value;
	}


	/// <summary>
	/// The type arguments of the generic type.
	/// </summary>
	public ImmutableArray<string> TypeArguments { get; init; } = [];

	internal TypeInfo (string name, SpecialType specialType)
	{
		FullyQualifiedName = name;
		SpecialType = specialType;

		// calculate the name and namespace based on the name. This is not the most efficient way to do it and it is
		// a internal constructor for testing purposes only.
		// there are few things to consider when setting the name of the class, first we need to
		// to make the diff between a generic class and a non generic class
		var nonGenericName = FullyQualifiedName.Contains ('<')
			? FullyQualifiedName.Substring (0, FullyQualifiedName.IndexOf ('<'))
			: FullyQualifiedName;
		var index = nonGenericName.LastIndexOf ('.');
		Name = index != -1
			? nonGenericName.Substring (index + 1)
			: nonGenericName;
		// based on the name, calculate the name space for the class
		if (Name.Length == nonGenericName.Length)
			Namespace = [];
		else
			// remove the name + 1 for the dot
			Namespace = [.. nonGenericName.Remove (nonGenericName.Length - (Name.Length + 1)).Split ('.')];
	}

	internal TypeInfo (string name,
		SpecialType specialType = SpecialType.None,
		bool isNullable = false,
		bool isBlittable = false,
		bool isSmartEnum = false,
		bool isArray = false,
		bool isReferenceType = false,
		bool isStruct = false) : this (name, specialType)
	{
		IsNullable = isNullable;
		IsBlittable = isBlittable;
		IsSmartEnum = isSmartEnum;
		IsArray = isArray;
		IsReferenceType = isReferenceType;
		IsStruct = isStruct;
	}

	static (string Name, ImmutableArray<string> Namespace) GetTypeNameAndNamespace (ITypeSymbol symbol)
	{
		if (symbol.SpecialType == SpecialType.None)
			return (symbol.Name, GetNamespaceComponents (symbol));

		var token = symbol.SpecialType.GetKeyword ();
		var name = string.IsNullOrEmpty (token) ? symbol.Name : token;
		// if we are dealing with int, uint etc.. we will ignore the namespace since it is not needed
		return (name, []);
	}

	/// <summary>
	/// Returns the namespace components of the type symbol.
	/// </summary>
	/// <param name="symbol">The symbol we are interested in.</param>
	/// <returns>An immutable array with the namespace components.</returns>
	static ImmutableArray<string> GetNamespaceComponents (ITypeSymbol symbol)
	{
		var namespaceSymbol = symbol.ContainingSymbol;
		var components = ImmutableArray.CreateBuilder<string> ();
		while (namespaceSymbol is not null) {
			components.Insert (0, namespaceSymbol.Name);
			namespaceSymbol = namespaceSymbol.ContainingSymbol;
			if (namespaceSymbol is INamespaceSymbol { IsGlobalNamespace: true })
				break;
		}
		return components.ToImmutableArray ();
	}

	internal TypeInfo (ITypeSymbol symbol)
	{
		// general case, get the name and namespace. If we are dealing with a generic type or an array type
		// the name will be later overwritten with the generic name or the array name
		(Name, Namespace) = GetTypeNameAndNamespace (symbol);
		SpecialType = symbol.SpecialType;
		FullyQualifiedName = symbol.ToDisplayString ().Trim ('?', '[', ']');
		IsNullable = symbol.NullableAnnotation == NullableAnnotation.Annotated;
		IsBlittable = symbol.IsBlittable ();
		IsSmartEnum = symbol.IsSmartEnum ();
		IsReferenceType = symbol.IsReferenceType;
		IsStruct = symbol.TypeKind == TypeKind.Struct;
		IsInterface = symbol.TypeKind == TypeKind.Interface;
		IsDelegate = symbol.TypeKind == TypeKind.Delegate;
		IsNativeIntegerType = symbol.IsNativeIntegerType;
		IsNativeEnum = symbol.HasAttribute (AttributesNames.NativeAttribute);
		IsProtocol = symbol.HasAttribute (AttributesNames.ProtocolAttribute);

		// data that we can get from the symbol without being INamedType
		symbol.GetInheritance (
			isNSObject: out isNSObject,
			isNativeObject: out isINativeObject,
			isDictionaryContainer: out isDictionaryContainer,
			parents: out parents,
			interfaces: out interfaces);

		IsWrapped = symbol.IsWrapped (isNSObject);
		if (symbol is IArrayTypeSymbol arraySymbol) {
			// override the name and namespace with the array name
			(Name, Namespace) = GetTypeNameAndNamespace (arraySymbol.ElementType);
			FullyQualifiedName = arraySymbol.ElementType.ToDisplayString ();
			IsArray = true;
			ArrayElementType = arraySymbol.ElementType.SpecialType;
			ArrayElementTypeIsWrapped = arraySymbol.ElementType.IsWrapped ();
			ArrayElementIsINativeObject = arraySymbol.ElementType.IsINativeObject ();
		}

		// try to get the named type symbol to have more educated decisions
		var namedTypeSymbol = symbol as INamedTypeSymbol;

		// store the enum special type, useful when generate code that needs to cast
		EnumUnderlyingType = namedTypeSymbol?.EnumUnderlyingType?.SpecialType;
		if (namedTypeSymbol is not null) {
			IsGenericType = namedTypeSymbol.IsGenericType;
			var typeArgumentsBucket = ImmutableArray.CreateBuilder<string> (namedTypeSymbol.TypeArguments.Length);
			foreach (var typeArgument in namedTypeSymbol.TypeArguments) {
				// rather than use the display name, which could be a generic name, we will create a struct for the 
				// type and use our type formater
				var info = new TypeInfo (typeArgument);
				var syntax = info.GetIdentifierSyntax ();
				typeArgumentsBucket.Add (syntax.ToString ());
			}
			TypeArguments = typeArgumentsBucket.ToImmutable ();

			if (namedTypeSymbol.DelegateInvokeMethod is not null &&
				DelegateInfo.TryCreate (namedTypeSymbol, out var delegateInfo))
				Delegate = delegateInfo;
		}

		if (!IsReferenceType && IsNullable && namedTypeSymbol is not null) {
			// get the type argument for nullable, which we know is the data that was boxed and use it to 
			// overwrite the SpecialType 
			var typeArgument = namedTypeSymbol.TypeArguments [0];
			// we need to update the name and namespace with the type argument
			(Name, Namespace) = GetTypeNameAndNamespace (typeArgument);
			// we need to decide if is generic based on the inner type
			if (typeArgument is INamedTypeSymbol innerType) {
				IsGenericType = innerType.IsGenericType;
			}
			SpecialType = typeArgument.SpecialType;
			MetadataName = SpecialType is SpecialType.None or SpecialType.System_Void
				? null : typeArgument.MetadataName;
		} else {
			MetadataName = SpecialType is SpecialType.None or SpecialType.System_Void
				? null : symbol.MetadataName;
		}

		if (symbol is IPointerTypeSymbol pointerTypeSymbol) {
			IsPointer = true;
			(Name, Namespace) = GetTypeNameAndNamespace (pointerTypeSymbol.PointedAtType);
		}
	}

	/// <inheritdoc/>
	public bool Equals (TypeInfo other)
	{
		if (FullyQualifiedName != other.FullyQualifiedName)
			return false;
		if (SpecialType != other.SpecialType)
			return false;
		if (MetadataName != other.MetadataName)
			return false;
		if (IsNullable != other.IsNullable)
			return false;
		if (IsBlittable != other.IsBlittable)
			return false;
		if (IsSmartEnum != other.IsSmartEnum)
			return false;
		if (IsArray != other.IsArray)
			return false;
		if (IsReferenceType != other.IsReferenceType)
			return false;
		if (IsStruct != other.IsStruct)
			return false;
		if (IsVoid != other.IsVoid)
			return false;
		if (EnumUnderlyingType != other.EnumUnderlyingType)
			return false;
		if (IsInterface != other.IsInterface)
			return false;
		if (IsNativeIntegerType != other.IsNativeIntegerType)
			return false;
		if (IsNativeEnum != other.IsNativeEnum)
			return false;
		if (Delegate != other.Delegate)
			return false;
		if (IsProtocol != other.IsProtocol)
			return false;

		// compare base classes and interfaces, order does not matter at all
		var listComparer = new CollectionComparer<string> ();
		if (!listComparer.Equals (parents, other.Parents))
			return false;
		if (!listComparer.Equals (interfaces, other.Interfaces))
			return false;

		return true;
	}

	/// <inheritdoc/>
	public override bool Equals (object? obj)
	{
		return obj is TypeInfo other && Equals (other);
	}

	/// <inheritdoc/>
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (FullyQualifiedName);
		hashCode.Add (SpecialType);
		hashCode.Add (MetadataName);
		hashCode.Add (IsNullable);
		hashCode.Add (IsBlittable);
		hashCode.Add (IsSmartEnum);
		hashCode.Add (IsArray);
		hashCode.Add (IsReferenceType);
		hashCode.Add (IsStruct);
		hashCode.Add (IsVoid);
		hashCode.Add (EnumUnderlyingType);
		hashCode.Add (IsInterface);
		hashCode.Add (IsNativeIntegerType);
		hashCode.Add (IsNativeEnum);
		hashCode.Add (Delegate);
		hashCode.Add (IsProtocol);
		foreach (var parent in parents) {
			hashCode.Add (parent);
		}

		foreach (var @interface in interfaces) {
			hashCode.Add (@interface);
		}

		return hashCode.ToHashCode ();
	}

	public static bool operator == (TypeInfo left, TypeInfo right)
	{
		return left.Equals (right);
	}

	public static bool operator != (TypeInfo left, TypeInfo right)
	{
		return !left.Equals (right);
	}

	const string NativeHandle = "NativeHandle";
	const string IntPtr = "IntPtr";
	const string UIntPtr = "UIntPtr";

	public string? ToMarshallType ()
	{
#pragma warning disable format
		var type = this switch {
			// arrays
			{ IsArray: true } => NativeHandle,
			
			// special cases based on name
			{ Name: "nfloat" or "NFloat" } => "nfloat", 
			{ Name: "nint" or "nuint" } => MetadataName,
			// special string case
			{ SpecialType: SpecialType.System_String } => NativeHandle, // use a NSString when we get a string

			// NSObject should use the native handle
			{ IsNSObject: true } => NativeHandle, 
			{ IsINativeObject: true } => NativeHandle,

			// structs will use their name
			{ IsStruct: true, SpecialType: SpecialType.System_Double } => "Double", 
			{ IsStruct: true } => Name,
			
			// enums:
			// IsSmartEnum: We are using a nsstring, so it should be a native handle.
			// IsNativeEnum: Depends on the enum backing field kind.
			// GeneralEnum: Depends on the EnumUnderlyingType

			{ IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_Int64 } => IntPtr, 
			{ IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_UInt64 } => UIntPtr, 
			{ IsSmartEnum: true } => NativeHandle, 
			{ IsEnum: true, EnumUnderlyingType: not null } => EnumUnderlyingType.GetKeyword (),

			// special type that is a keyword (none would be a ref type)
			{ SpecialType: SpecialType.System_Void } => SpecialType.GetKeyword (),

			// This should not happen in bindings because all of the types should either be native objects
			// nsobjects, or structs 
			{ IsReferenceType: false } => Name,
			
			// delegates will use the native handle
			{ IsDelegate: true} => NativeHandle,

			_ => null,
		};
#pragma warning restore format
		return type;
	}

	public TypeInfo ToArrayElementType ()
	{
		if (!IsArray)
			return this;
		// copy all the elements from the current array type and set the array type to false
		return this with {
			IsArray = false,
			SpecialType = ArrayElementType ?? SpecialType.None,
		};
	}

	public TypeInfo ToNonNullable ()
	{
		if (!IsNullable)
			return this;
		// copy all the elements from the current array type and set the array type to false
		return this with {
			IsNullable = false,
		};
	}

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ("{");
		sb.Append ($"Name: '{FullyQualifiedName}', ");
		sb.Append ($"MetadataName: '{MetadataName}', ");
		sb.Append ($"SpecialType: '{SpecialType}', ");
		sb.Append ($"IsNullable: {IsNullable}, ");
		sb.Append ($"IsBlittable: {IsBlittable}, ");
		sb.Append ($"IsSmartEnum: {IsSmartEnum}, ");
		sb.Append ($"IsArray: {IsArray}, ");
		sb.Append ($"IsReferenceType: {IsReferenceType}, ");
		sb.Append ($"IsStruct: {IsStruct}, ");
		sb.Append ($"IsVoid: {IsVoid}, ");
		sb.Append ($"IsNSObject: {IsNSObject}, ");
		sb.Append ($"IsDictionaryContainer: {IsDictionaryContainer}, ");
		sb.Append ($"IsNativeObject: {IsINativeObject}, ");
		sb.Append ($"IsInterface: {IsInterface}, ");
		sb.Append ($"IsNativeIntegerType: {IsNativeIntegerType}, ");
		sb.Append ($"IsNativeEnum: {IsNativeEnum}, ");
		sb.Append ($"IsProtocol: {IsProtocol}, ");
		sb.Append ($"Delegate: {Delegate?.ToString () ?? "null"}, ");
		sb.Append ($"EnumUnderlyingType: '{EnumUnderlyingType?.ToString () ?? "null"}', ");
		sb.Append ("Parents: [");
		sb.AppendJoin (", ", parents);
		sb.Append ("], Interfaces: [");
		sb.AppendJoin (", ", interfaces);
		sb.Append ("]}");
		return sb.ToString ();
	}
}
