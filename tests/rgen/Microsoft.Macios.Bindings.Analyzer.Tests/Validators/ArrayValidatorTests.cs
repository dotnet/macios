// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Bindings.Analyzer.Validators;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

public class ArrayValidatorTests {

	readonly RootContext context;

	public ArrayValidatorTests ()
	{
		// Create a dummy compilation to get a semantic model and RootContext
		var syntaxTree = CSharpSyntaxTree.ParseText ("namespace Test { }");
		var compilation = CSharpCompilation.Create (
			"TestAssembly",
			[syntaxTree],
			references: [],
			options: new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary)
		);
		var semanticModel = compilation.GetSemanticModel (syntaxTree);
		context = new RootContext (semanticModel);
	}

	Property CreateFieldProperty (string name = "TestProperty",
		bool isStatic = true,
		bool isPartial = true,
		ObjCBindings.Property flags = ObjCBindings.Property.Default,
		string? selector = "testSelector")
	{
		var modifiers = ImmutableArray.CreateBuilder<SyntaxToken> ();
		modifiers.Add (SyntaxFactory.Token (SyntaxKind.PublicKeyword));
		if (isStatic)
			modifiers.Add (SyntaxFactory.Token (SyntaxKind.StaticKeyword));
		if (isPartial)
			modifiers.Add (SyntaxFactory.Token (SyntaxKind.PartialKeyword));

		var exportFieldData = new FieldInfo<ObjCBindings.Property> (
			new FieldData<ObjCBindings.Property> (selector!, flags), "Test");

		var availability = new SymbolAvailability.Builder ();
		availability.Add (new SupportedOSPlatformData ("ios"));
		availability.Add (new SupportedOSPlatformData ("tvos"));
		availability.Add (new SupportedOSPlatformData ("macos"));
		availability.Add (new SupportedOSPlatformData ("maccatalyst"));

		return new Property (
			name: name,
			returnType: ReturnTypeForString (),
			symbolAvailability: availability.ToImmutable (),
			attributes: [],
			modifiers: modifiers.ToImmutable (),
			accessors: []
		) {
			ExportFieldData = exportFieldData,
		};
	}

	[Theory]
	[InlineData (new [] { true }, new [] { true }, 0)] // One valid field
	[InlineData (new [] { false }, new [] { true }, 1)] // One invalid field (not static)
	[InlineData (new [] { true }, new [] { false }, 1)] // One invalid field (not partial)
	[InlineData (new [] { false }, new [] { false }, 2)] // One invalid field (neither static nor partial)
	[InlineData (new [] { true, true }, new [] { true, true }, 0)] // Two valid fields
	[InlineData (new [] { false, true }, new [] { true, true }, 1)] // One invalid, one valid
	[InlineData (new [] { false, false }, new [] { true, true }, 2)] // Two invalid (not static)
	[InlineData (new [] { true, true }, new [] { false, false }, 2)] // Two invalid (not partial)
	[InlineData (new [] { false, false }, new [] { false, false }, 4)] // Two invalid (neither static nor partial)
	public void ValidateArrayFieldModifiersTests (bool [] isStaticArray, bool [] isPartialArray, int expectedDiagnosticsCount)
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.CreateBuilder<Property> ();

		for (int i = 0; i < isStaticArray.Length; i++) {
			properties.Add (CreateFieldProperty (
				name: $"TestProperty{i}",
				isStatic: isStaticArray [i],
				isPartial: isPartialArray [i]
			));
		}

		var result = validator.ValidateAll (properties.ToImmutable (), context);

		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	[Theory]
	[InlineData (new [] { ObjCBindings.Property.Default }, 0)]
	[InlineData (new [] { ObjCBindings.Property.IsThreadStatic }, 1)]
	[InlineData (new [] { ObjCBindings.Property.Default, ObjCBindings.Property.Default }, 0)]
	[InlineData (new [] { ObjCBindings.Property.IsThreadStatic, ObjCBindings.Property.Default }, 1)]
	[InlineData (new [] { ObjCBindings.Property.IsThreadStatic, ObjCBindings.Property.MarshalNativeExceptions }, 2)]
	[InlineData (new [] { ObjCBindings.Property.CustomMarshalDirective, ObjCBindings.Property.DisableZeroCopy, ObjCBindings.Property.IsThreadSafe }, 3)]
	public void ValidateArrayIgnoredFlagsTests (ObjCBindings.Property [] flagsArray, int expectedDiagnosticsCount)
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.CreateBuilder<Property> ();

		for (int i = 0; i < flagsArray.Length; i++) {
			properties.Add (CreateFieldProperty (
				name: $"TestProperty{i}",
				flags: flagsArray [i]
			));
		}

		var result = validator.ValidateAll (properties.ToImmutable (), context);

		var diagnostics = new List<Diagnostic> ();
		foreach (var (_, value) in result) {
			diagnostics.AddRange (value);
		}

		if (expectedDiagnosticsCount > 0) {
			Assert.Equal (expectedDiagnosticsCount, diagnostics.Count);
			Assert.All (diagnostics, d => Assert.Equal ("RBI0028", d.Id));
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (new [] { "validSelector" }, 0)]
	[InlineData (new [] { "" }, 1)]
	[InlineData (new string? [] { null }, 1)]
	[InlineData (new [] { "validSelector", "anotherValid" }, 0)]
	[InlineData (new [] { "validSelector", null }, 1)]
	[InlineData (new string? [] { null, null }, 2)]
	public void ArraySelectorIsNotNullTests (string? [] selectors, int expectedDiagnosticsCount)
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.CreateBuilder<Property> ();

		for (int i = 0; i < selectors.Length; i++) {
			properties.Add (CreateFieldProperty (
				name: $"TestProperty{i}",
				selector: selectors [i]
			));
		}

		var result = validator.ValidateAll (properties.ToImmutable (), context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var selectorDiagnostics = diagnostics.Where (d => d.Id == "RBI0018").ToList ();

		Assert.Equal (expectedDiagnosticsCount, selectorDiagnostics.Count);
	}

	[Theory]
	[InlineData (new [] { "validSelector" }, 0)]
	[InlineData (new [] { "invalid selector" }, 1)]
	[InlineData (new [] { "validSelector", "anotherValid" }, 0)]
	[InlineData (new [] { "validSelector", "invalid selector" }, 1)]
	[InlineData (new [] { "invalid selector", "another invalid" }, 2)]
	[InlineData (new [] { "selector\twith\ttab", "selector\nwith\nnewline", " leadingSpace" }, 3)]
	public void ArraySelectorHasNoWhitespaceTests (string [] selectors, int expectedDiagnosticsCount)
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.CreateBuilder<Property> ();

		for (int i = 0; i < selectors.Length; i++) {
			properties.Add (CreateFieldProperty (
				name: $"TestProperty{i}",
				selector: selectors [i]
			));
		}

		var result = validator.ValidateAll (properties.ToImmutable (), context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var whitespaceDiagnostics = diagnostics.Where (d => d.Id == "RBI0019").ToList ();

		Assert.Equal (expectedDiagnosticsCount, whitespaceDiagnostics.Count);
	}

	[Fact]
	public void ValidateEmptyArrayTests ()
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray<Property>.Empty;

		var result = validator.ValidateAll (properties, context);

		Assert.Empty (result);
	}

	[Fact]
	public void ValidateArrayOfValidFieldsTests ()
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.Create (
			CreateFieldProperty (name: "Property1"),
			CreateFieldProperty (name: "Property2"),
			CreateFieldProperty (name: "Property3")
		);

		var result = validator.ValidateAll (properties, context);

		Assert.Empty (result);
	}

	[Theory]
	[InlineData (1)]
	[InlineData (3)]
	[InlineData (5)]
	public void ValidateArrayWithMixedValidityTests (int arraySize)
	{
		var validator = new ArrayValidator<Property> (new FieldValidator ());
		var properties = ImmutableArray.CreateBuilder<Property> ();

		for (int i = 0; i < arraySize; i++) {
			// Make every other property invalid (not static)
			properties.Add (CreateFieldProperty (
				name: $"TestProperty{i}",
				isStatic: i % 2 == 0
			));
		}

		var result = validator.ValidateAll (properties.ToImmutable (), context);

		var totalDiagnostics = result.Values.Sum (x => x.Count);
		var expectedInvalidCount = arraySize / 2; // Every other property is invalid
		Assert.Equal (expectedInvalidCount, totalDiagnostics);
	}
}
