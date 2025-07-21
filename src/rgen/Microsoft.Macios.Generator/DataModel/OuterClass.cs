// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.DataModel;

readonly struct OuterClass : IEquatable<OuterClass> {

	/// <summary>
	/// Modifiers list.
	/// </summary>
	public ImmutableArray<SyntaxToken> Modifiers { init; get; } = [];

	/// <summary>
	/// The name of the named type that generated the code change.
	/// </summary>
	public string Name { init; get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="OuterClass"/> struct.
	/// </summary>
	/// <param name="name">The name of the outer class.</param>
	public OuterClass (string name)
	{
		Name = name;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OuterClass"/> struct.
	/// </summary>
	/// <param name="name">The name of the outer class.</param>
	/// <param name="modifiers">The modifiers of the outer class.</param>
	public OuterClass (string name, ImmutableArray<SyntaxToken> modifiers) : this (name)
	{
		Modifiers = modifiers;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OuterClass"/> struct from a <see cref="INamedTypeSymbol"/>.
	/// </summary>
	/// <param name="type">The named type symbol.</param>
	public OuterClass (INamedTypeSymbol type) : this (type.Name)
	{
		var bucket = ImmutableArray.CreateBuilder<SyntaxToken> ();
		bucket.Add (type.DeclaredAccessibility switch {
			Accessibility.Private => Token (SyntaxKind.PrivateKeyword),
			Accessibility.Internal => Token (SyntaxKind.InternalKeyword),
			_ => Token (SyntaxKind.PublicKeyword)
		});

		if (type.IsStatic) {
			bucket.Add (Token (SyntaxKind.StaticKeyword));
		}

		if (type.IsAbstract) {
			bucket.Add (Token (SyntaxKind.AbstractKeyword));
		}

		// always add the partial modifier
		bucket.Add (Token (SyntaxKind.PartialKeyword));
		Modifiers = bucket.ToImmutable ();
	}

	/// <inheritdoc />
	public bool Equals (OuterClass other)
	{
		if (Name != other.Name)
			return false;

		var modifiersComparer = new ModifiersEqualityComparer ();
		return modifiersComparer.Equals (Modifiers, other.Modifiers);
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is OuterClass other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		var hash = new HashCode ();
		hash.Add (Name);
		foreach (var modifier in Modifiers) {
			hash.Add (modifier.Kind ());
		}
		return hash.ToHashCode ();
	}

	/// <summary>
	/// Compares two <see cref="OuterClass"/> instances for equality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (OuterClass x, OuterClass y) => x.Equals (y);

	/// <summary>
	/// Compares two <see cref="OuterClass"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (OuterClass x, OuterClass y) => !x.Equals (y);

	/// <inheritdoc />
	public override string ToString ()
	{
		var sb = new StringBuilder ("{");
		sb.Append ($"Name: '{Name}', ");
		sb.Append ("Modifiers: [");
		sb.AppendJoin (", ", Modifiers.Select (m => $"'{m}'"));
		sb.Append ("]}");
		return sb.ToString ();
	}
}
