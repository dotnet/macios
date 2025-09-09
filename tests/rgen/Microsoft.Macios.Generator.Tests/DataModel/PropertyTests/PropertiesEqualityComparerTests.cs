// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.DataModel.PropertyTests;

public class PropertiesEqualityComparerTests {
	readonly PropertiesEqualityComparer equalityComparer = new ();

	[Fact]
	public void CompareEmptyArrays ()
	{
		ImmutableArray<Property> x = [];
		ImmutableArray<Property> y = [];

		Assert.True (equalityComparer.Equals (x, y));
	}

	[Fact]
	public void CompareDifferentSize ()
	{
		ImmutableArray<Property> x = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
			new (
				name: "SecondProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];
		ImmutableArray<Property> y = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];

		Assert.False (equalityComparer.Equals (x, y));
	}

	[Fact]
	public void CompareSameSizeDiffProperties ()
	{
		ImmutableArray<Property> x = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];
		ImmutableArray<Property> y = [
			new (
				name: "FirstProperty",
				returnType: new ("AVFoundation.AVVideo"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];

		Assert.False (equalityComparer.Equals (x, y));
	}

	[Fact]
	public void CompareSameSizeSameProperties ()
	{
		ImmutableArray<Property> x = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];
		ImmutableArray<Property> y = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];

		Assert.True (equalityComparer.Equals (x, y));
	}

	[Fact]
	public void CompareDiffSmartEnum ()
	{
		ImmutableArray<Property> x = [
			new (
				name: "FirstProperty",
				returnType: new ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];
		ImmutableArray<Property> y = [
			new (
				name: "FirstProperty",
				returnType: new ("string", isSmartEnum: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [
					SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					)
				]),
		];

		Assert.False (equalityComparer.Equals (x, y));
	}
}
