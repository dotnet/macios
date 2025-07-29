// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Availability;

namespace Microsoft.Macios.Generator.DataModel;

[StructLayout (LayoutKind.Auto)]
readonly partial struct Accessor : IEquatable<Accessor> {

	/// <summary>
	/// The initialization state of the struct.
	/// </summary>
	StructState State { get; init; } = StructState.Default;

	/// <summary>
	/// Gets the default, uninitialized instance of <see cref="Accessor"/>.
	/// </summary>
	public static Accessor Default { get; } = new (StructState.Default);

	/// <summary>
	/// Gets a value indicating whether the instance is the default, uninitialized instance.
	/// </summary>
	public bool IsNullOrDefault => State == StructState.Default;

	/// <summary>
	/// The kind of accessor.
	/// </summary>
	public AccessorKind Kind { get; }

	/// <summary>
	/// The platform availability of the enum value.
	/// </summary>
	public SymbolAvailability SymbolAvailability { get; }

	/// <summary>
	/// List of attribute code changes of the accessor.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; } = [];

	/// <summary>
	/// List of modifiers of the accessor.
	/// </summary>
	public ImmutableArray<SyntaxToken> Modifiers { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Accessor"/> struct with a specific state.
	/// Used to create the default, uninitialized instance.
	/// </summary>
	/// <param name="state">The initialization state of the struct.</param>
	Accessor (StructState state)
	{
		State = state;
	}

	/// <inheritdoc />
	public bool Equals (Accessor other)
	{
		if (State == StructState.Default && other.State == StructState.Default)
			return true;
		if (Kind != other.Kind)
			return false;
		if (SymbolAvailability != other.SymbolAvailability)
			return false;
		if (ExportPropertyData != other.ExportPropertyData)
			return false;

		var attrsComparer = new AttributesEqualityComparer ();
		if (!attrsComparer.Equals (Attributes, other.Attributes))
			return false;
		var modifiersComparer = new ModifiersEqualityComparer ();
		return modifiersComparer.Equals (Modifiers, other.Modifiers);
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is Accessor other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine ((int) Kind, SymbolAvailability, ExportPropertyData, Attributes, Modifiers);
	}

	public static bool operator == (Accessor left, Accessor right)
	{
		return left.Equals (right);
	}

	public static bool operator != (Accessor left, Accessor right)
	{
		return !left.Equals (right);
	}
}
