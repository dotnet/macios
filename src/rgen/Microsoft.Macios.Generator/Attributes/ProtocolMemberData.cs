// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Attributes;

/// <summary>
/// Represents the metadata for a member (method or property) within a protocol.
/// </summary>
readonly struct ProtocolMemberData : IEquatable<ProtocolMemberData> {

	// general properties

	/// <summary>
	/// Gets a value indicating whether the protocol member is required.
	/// </summary>
	public bool IsRequired { get; init; }
	/// <summary>
	/// Gets a value indicating whether the protocol member is a property.
	/// </summary>
	public bool IsProperty { get; init; }
	/// <summary>
	/// Gets a value indicating whether the protocol member is static.
	/// </summary>
	public bool IsStatic { get; init; }
	/// <summary>
	/// Gets the name of the protocol member.
	/// </summary>
	public string Name { get; init; }
	/// <summary>
	/// Gets the selector of the protocol member.
	/// </summary>
	public string? Selector { get; init; }

	// method properties

	/// <summary>
	/// Gets the return type of the method. Null for properties.
	/// </summary>
	public TypeInfo? ReturnType { get; init; }
	/// <summary>
	/// Gets the delegate proxy for the return type of the method. Null for properties.
	/// </summary>
	public TypeInfo? ReturnTypeDelegateProxy { get; init; }
	/// <summary>
	/// Gets the parameter types of the method. Empty for properties.
	/// </summary>
	public ImmutableArray<TypeInfo> ParameterType { get; init; } = [];
	/// <summary>
	/// Gets a value indicating whether each parameter is passed by reference. Empty for properties.
	/// </summary>
	public ImmutableArray<bool> ParameterByRef { get; init; } = [];
	/// <summary>
	/// Gets the block proxy for each parameter. Empty for properties.
	/// </summary>
	public ImmutableArray<TypeInfo?> ParameterBlockProxy { get; init; } = [];
	/// <summary>
	/// Gets a value indicating whether the method is variadic. False for properties.
	/// </summary>
	public bool IsVariadic { get; init; }

	// property properties
	/// <summary>
	/// Gets the type of the property. Null for methods.
	/// </summary>
	public TypeInfo? PropertyType { get; init; }
	/// <summary>
	/// Gets the selector for the property's getter. Null for methods.
	/// </summary>
	public string? GetterSelector { get; init; }
	/// <summary>
	/// Gets the selector for the property's setter. Null for methods or read-only properties.
	/// </summary>
	public string? SetterSelector { get; init; }
	/// <summary>
	/// Gets the argument semantic for the property. None for methods.
	/// </summary>
	public ArgumentSemantic ArgumentSemantic { get; init; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ProtocolMemberData"/> struct.
	/// </summary>
	/// <param name="isRequired">A value indicating whether the protocol member is required.</param>
	/// <param name="isProperty">A value indicating whether the protocol member is a property.</param>
	/// <param name="isStatic">A value indicating whether the protocol member is static.</param>
	/// <param name="name">The name of the protocol member.</param>
	/// <param name="selector">The selector of the protocol member.</param>
	public ProtocolMemberData (bool isRequired, bool isProperty, bool isStatic, string name, string? selector)
	{
		IsRequired = isRequired;
		IsProperty = isProperty;
		IsStatic = isStatic;
		Name = name;
		Selector = selector;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ProtocolMemberData"/> struct for a property.
	/// </summary>
	/// <param name="property">The property metadata.</param>
	/// <param name="getter">The getter method metadata.</param>
	/// <param name="setter">The setter method metadata.</param>
	public ProtocolMemberData (in Property property, in Method? getter, in Method? setter)
		: this (isRequired: !property.IsOptional, isProperty: true, isStatic: property.IsStatic, name: property.Name, selector: property.Selector)
	{
		// set the property specific data
		PropertyType = property.ReturnType.WithNullable (isNullable: false);
		GetterSelector = getter?.Selector;
		SetterSelector = setter?.Selector;
		ArgumentSemantic = property.ExportPropertyData?.ArgumentSemantic ?? ArgumentSemantic.None;
		ReturnTypeDelegateProxy = property.ReturnType.IsDelegate
			? TypeInfo.CreateDelegateProxy (property.ReturnType)
			: null;
		ParameterBlockProxy = property.ReturnType.IsDelegate
			? [TypeInfo.CreateDelegateProxy (property.ReturnType)]
			: [];
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ProtocolMemberData"/> struct for a method.
	/// </summary>
	/// <param name="method">The method metadata.</param>
	public ProtocolMemberData (in Method method)
		: this (isRequired: !method.IsOptional, isProperty: false, isStatic: method.IsStatic, name: method.Name, selector: method.Selector)
	{
		// set the method specific data
		ReturnType = method.ReturnType.WithNullable (isNullable: false);
		ReturnTypeDelegateProxy = method.ReturnType.IsDelegate
			? TypeInfo.CreateDelegateProxy (method.ReturnType)
			: null;
		ParameterType = [.. method.Parameters.Select (p => p.Type.WithNullable (isNullable: false))];
		ParameterByRef = [.. method.Parameters.Select (p => p.IsByRef)];
		ParameterBlockProxy = [.. method.Parameters.Select (p => {
			return (TypeInfo?) (p.Type.IsDelegate ? TypeInfo.CreateDelegateProxy (p.Type) : null);
		})];
		IsVariadic = method.IsVariadic;
	}

	/// <inheritdoc />
	public bool Equals (ProtocolMemberData other)
	{
		if (IsRequired != other.IsRequired)
			return false;
		if (IsProperty != other.IsProperty)
			return false;
		if (IsStatic != other.IsStatic)
			return false;
		if (Name != other.Name)
			return false;
		if (Selector != other.Selector)
			return false;
		if (!Nullable.Equals (ReturnType, other.ReturnType))
			return false;
		if (!Nullable.Equals (ReturnTypeDelegateProxy, other.ReturnTypeDelegateProxy))
			return false;
		if (!ParameterType.SequenceEqual (other.ParameterType))
			return false;
		if (!ParameterByRef.SequenceEqual (other.ParameterByRef))
			return false;
		if (!ParameterBlockProxy.SequenceEqual (other.ParameterBlockProxy))
			return false;
		if (IsVariadic != other.IsVariadic)
			return false;
		if (!Nullable.Equals (PropertyType, other.PropertyType))
			return false;
		if (GetterSelector != other.GetterSelector)
			return false;
		if (SetterSelector != other.SetterSelector)
			return false;
		if (ArgumentSemantic != other.ArgumentSemantic)
			return false;
		return true;
	}


	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is ProtocolMemberData other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (IsRequired);
		hashCode.Add (IsProperty);
		hashCode.Add (IsStatic);
		hashCode.Add (Name);
		hashCode.Add (Selector);
		hashCode.Add (ReturnType);
		hashCode.Add (ReturnTypeDelegateProxy);
		hashCode.Add (ParameterType);
		hashCode.Add (ParameterByRef);
		hashCode.Add (ParameterBlockProxy);
		hashCode.Add (IsVariadic);
		hashCode.Add (PropertyType);
		hashCode.Add (GetterSelector);
		hashCode.Add (SetterSelector);
		hashCode.Add ((int) ArgumentSemantic);
		return hashCode.ToHashCode ();
	}

	/// <summary>
	/// Compares two <see cref="ProtocolMemberData"/> instances for equality.
	/// </summary>
	/// <param name="left">The first instance to compare.</param>
	/// <param name="right">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (ProtocolMemberData left, ProtocolMemberData right)
	{
		return left.Equals (right);
	}

	/// <summary>
	/// Compares two <see cref="ProtocolMemberData"/> instances for inequality.
	/// </summary>
	/// <param name="left">The first instance to compare.</param>
	/// <param name="right">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (ProtocolMemberData left, ProtocolMemberData right)
	{
		return !left.Equals (right);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		var sb = new StringBuilder ("{ IsRequired: '");
		sb.Append (IsRequired);
		sb.Append ("', IsProperty: '");
		sb.Append (IsProperty);
		sb.Append ("', IsStatic: '");
		sb.Append (IsStatic);
		sb.Append ("', Name: '");
		sb.Append (Name ?? "null");
		sb.Append ("', Selector: '");
		sb.Append (Selector ?? "null");
		sb.Append ("', ReturnType: '");
		sb.Append (ReturnType?.FullyQualifiedName ?? "null");
		sb.Append ("', ReturnTypeDelegateProxy: '");
		sb.Append (ReturnTypeDelegateProxy?.FullyQualifiedName ?? "null");
		sb.Append ("', ParameterType: '");
		sb.Append (ParameterType.IsDefaultOrEmpty ? "null" : string.Join (", ", ParameterType.Select (p => p.FullyQualifiedName)));
		sb.Append ("', ParameterByRef: '");
		sb.Append (ParameterByRef.IsDefaultOrEmpty ? "null" : string.Join (", ", ParameterByRef));
		sb.Append ("', ParameterBlockProxy: '");
		sb.Append (ParameterBlockProxy.IsDefaultOrEmpty ? "null" : string.Join (", ", ParameterBlockProxy.Select (p => p?.FullyQualifiedName ?? "null")));
		sb.Append ("', IsVariadic: '");
		sb.Append (IsVariadic);
		sb.Append ("', PropertyType: '");
		sb.Append (PropertyType?.FullyQualifiedName ?? "null");
		sb.Append ("', GetterSelector: '");
		sb.Append (GetterSelector ?? "null");
		sb.Append ("', SetterSelector: '");
		sb.Append (SetterSelector ?? "null");
		sb.Append ("', ArgumentSemantic: '");
		sb.Append (ArgumentSemantic);
		sb.Append ("' }");
		return sb.ToString ();
	}
}
