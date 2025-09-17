// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using ObjCBindings;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Structure that represents a change that was made by the user on enum members that has to be
/// reflected in the generated code.
/// </summary>
[StructLayout (LayoutKind.Auto)]
readonly partial struct EnumMember : IEquatable<EnumMember> {

	/// <summary>
	/// The initialization state of the struct.
	/// </summary>
	StructState State { get; init; } = StructState.Default;

	/// <summary>
	/// Gets the default, uninitialized instance of <see cref="EnumMember"/>.
	/// </summary>
	public static EnumMember Default { get; } = new (StructState.Default, string.Empty);

	/// <summary>
	/// Gets a value indicating whether the instance is the default, uninitialized instance.
	/// </summary>
	public bool IsNullOrDefault => State == StructState.Default;

	/// <summary>
	/// Get the name of the member.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Gets the index of the enum member.
	/// </summary>
	public uint Index { get; init; }

	/// <summary>
	/// The platform availability of the enum value.
	/// </summary>
	public SymbolAvailability SymbolAvailability { get; }

	/// <summary>
	/// Get the attributes added to the member.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; } = [];

	/// <inheritdoc />
	public bool Equals (EnumMember other)
	{
		if (Name != other.Name)
			return false;
		if (Index != other.Index)
			return false;
		if (SymbolAvailability != other.SymbolAvailability)
			return false;
		if (FieldInfo != other.FieldInfo)
			return false;

		var attrComparer = new AttributesEqualityComparer ();
		return attrComparer.Equals (Attributes, other.Attributes);
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is EnumMember other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Name, Index, SymbolAvailability, FieldInfo, Attributes);
	}

	/// <summary>
	/// Compares two <see cref="EnumMember"/> instances for equality.
	/// </summary>
	/// <param name="x">The first <see cref="EnumMember"/> to compare.</param>
	/// <param name="y">The second <see cref="EnumMember"/> to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (EnumMember x, EnumMember y)
	{
		return x.Equals (y);
	}

	/// <summary>
	/// Compares two <see cref="EnumMember"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first <see cref="EnumMember"/> to compare.</param>
	/// <param name="y">The second <see cref="EnumMember"/> to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (EnumMember x, EnumMember y)
	{
		return !(x == y);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EnumMember"/> struct.
	/// </summary>
	/// <param name="state">The initialization state of the struct.</param>
	/// <param name="name">The name of the enum member.</param>
	public EnumMember (StructState state, string name)
	{
		State = state;
		Name = name;
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		var sb = new StringBuilder (
			$"{{ Name: '{Name}', Index: {Index}, SymbolAvailability: {SymbolAvailability} FieldInfo: {FieldInfo} Attributes: [");
		sb.AppendJoin (", ", Attributes);
		sb.Append ("] }");
		return sb.ToString ();
	}
}
