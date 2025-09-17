// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Availability;

namespace Microsoft.Macios.Generator.DataModel;

[StructLayout (LayoutKind.Auto)]
readonly partial struct Constructor : IEquatable<Constructor> {

	/// <summary>
	/// The initialization state of the struct.
	/// </summary>
	StructState State { get; init; } = StructState.Default;

	/// <summary>
	/// Gets the default, uninitialized instance of <see cref="Constructor"/>.
	/// </summary>
	public static Constructor Default { get; } = new (StructState.Default);

	/// <summary>
	/// Gets a value indicating whether the instance is the default, uninitialized instance.
	/// </summary>
	public bool IsNullOrDefault => State == StructState.Default;

	/// <summary>
	/// Gets or sets a value indicating whether the constructor comes from a protocol factory method.
	/// </summary>
	public bool IsProtocolConstructor { get; init; }

	/// <summary>
	/// Gets the name of the protocol that this constructor originates from, if it is a protocol constructor.
	/// </summary>
	[MemberNotNullWhen (true, nameof (IsProtocolConstructor))]
	public string? ProtocolType { get; init; }

	/// <summary>
	/// Type name that owns the constructor.
	/// </summary>
	public string Type { get; }

	/// <summary>
	/// The platform availability of the constructor.
	/// </summary>
	public SymbolAvailability SymbolAvailability { get; }

	/// <summary>
	/// Get the attributes added to the constructor.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; } = [];

	/// <summary>
	/// Modifiers list.
	/// </summary>
	public ImmutableArray<SyntaxToken> Modifiers { get; init; } = [];

	/// <summary>
	/// Parameters list.
	/// </summary>
	public ImmutableArray<Parameter> Parameters { get; init; } = [];

	Constructor (StructState state)
	{
		State = state;
		Type = string.Empty;
	}

	public Constructor (string type,
		SymbolAvailability symbolAvailability,
		ImmutableArray<AttributeCodeChange> attributes,
		ImmutableArray<SyntaxToken> modifiers,
		ImmutableArray<Parameter> parameters) : this (StructState.Initialized)
	{
		Type = type;
		SymbolAvailability = symbolAvailability;
		Attributes = attributes;
		Modifiers = modifiers;
		Parameters = parameters;
	}

	/// <inheritdoc/>
	public bool Equals (Constructor other)
	{
		if (Type != other.Type)
			return false;
		if (ExportMethodData != other.ExportMethodData)
			return false;
		if (SymbolAvailability != other.SymbolAvailability)
			return false;

		var attrsComparer = new AttributesEqualityComparer ();
		if (!attrsComparer.Equals (Attributes, other.Attributes))
			return false;
		var modifiersComparer = new ModifiersEqualityComparer ();
		if (!modifiersComparer.Equals (Modifiers, other.Modifiers))
			return false;

		var paramComparer = new MethodParameterEqualityComparer ();
		return paramComparer.Equals (Parameters, other.Parameters);
	}

	/// <inheritdoc/>
	public override bool Equals (object? obj)
	{
		return obj is Constructor other && Equals (other);
	}

	/// <inheritdoc/>
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (Type);
		hashCode.Add (SymbolAvailability);
		foreach (var modifier in Modifiers) {
			hashCode.Add (modifier);
		}

		foreach (var attr in Attributes) {
			hashCode.Add (attr);
		}

		foreach (var parameter in Parameters) {
			hashCode.Add (parameter);
		}

		return hashCode.ToHashCode ();
	}

	public static bool operator == (Constructor left, Constructor right)
	{
		return left.Equals (right);
	}

	public static bool operator != (Constructor left, Constructor right)
	{
		return !left.Equals (right);
	}

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ($"{{ Ctr: Type: {Type}, ");
		sb.Append ($"ExportMethodData: {ExportMethodData}, ");
		sb.Append ($"SymbolAvailability: {SymbolAvailability}, ");
		sb.Append ("Attributes: [");
		sb.AppendJoin (", ", Attributes);
		sb.Append ("], Modifiers: [");
		sb.AppendJoin (", ", Modifiers.Select (x => x.Text));
		sb.Append ("], Parameters: [");
		sb.AppendJoin (", ", Parameters);
		sb.Append ("] }}");
		return sb.ToString ();
	}
}
