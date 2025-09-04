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

public class FieldValidatorTests {

	readonly RootContext context;

	public FieldValidatorTests ()
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
	[InlineData (true, true, 0)] // Valid field: static and partial
	[InlineData (false, true, 1)] // Invalid: not static
	[InlineData (true, false, 1)] // Invalid: not partial
	[InlineData (false, false, 2)] // Invalid: neither static nor partial
	public void ValidateFieldModifiersTests (bool isStatic, bool isPartial, int expectedDiagnosticsCount)
	{
		// Arrange
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (isStatic: isStatic, isPartial: isPartial);

		// Act
		var result = validator.ValidateAll (property, context);

		// Assert
		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	[Theory]
	[InlineData (ObjCBindings.Property.Default, 0)]
	[InlineData (ObjCBindings.Property.IsThreadStatic, 1)]
	[InlineData (ObjCBindings.Property.MarshalNativeExceptions, 1)]
	[InlineData (ObjCBindings.Property.CustomMarshalDirective, 1)]
	[InlineData (ObjCBindings.Property.DisableZeroCopy, 1)]
	[InlineData (ObjCBindings.Property.IsThreadSafe, 1)]
	[InlineData (ObjCBindings.Property.Transient, 1)]
	[InlineData (ObjCBindings.Property.PlainString, 1)]
	[InlineData (ObjCBindings.Property.CoreImageFilterProperty, 1)]
	[InlineData (ObjCBindings.Property.AutoRelease, 1)]
	[InlineData (ObjCBindings.Property.RetainReturnValue, 1)]
	[InlineData (ObjCBindings.Property.ReleaseReturnValue, 1)]
	[InlineData (ObjCBindings.Property.Proxy, 1)]
	[InlineData (ObjCBindings.Property.WeakDelegate, 1)]
	[InlineData (ObjCBindings.Property.Optional, 1)]
	[InlineData (ObjCBindings.Property.CreateEvents, 1)]
	public void ValidateIgnoredFlagsTests (ObjCBindings.Property flags, int expectedDiagnosticsCount)
	{
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (flags: flags);

		var result = new List<Diagnostic> ();
		foreach (var (_, value) in validator.ValidateAll (property, context)) {
			result.AddRange (value);
		}

		if (expectedDiagnosticsCount > 0) {
			//Assert.True (result.ContainsKey ("FlagsAreValid"));
			Assert.Equal (expectedDiagnosticsCount, result.Count);
			Assert.All (result, d => Assert.Equal ("RBI0028", d.Id));
		} else {
			Assert.Empty (result);
		}
	}

	[Theory]
	[InlineData (ObjCBindings.Property.IsThreadStatic | ObjCBindings.Property.MarshalNativeExceptions, 2)]
	[InlineData (ObjCBindings.Property.CustomMarshalDirective | ObjCBindings.Property.DisableZeroCopy | ObjCBindings.Property.IsThreadSafe, 3)]
	[InlineData (ObjCBindings.Property.Transient | ObjCBindings.Property.PlainString | ObjCBindings.Property.CoreImageFilterProperty | ObjCBindings.Property.AutoRelease, 4)]
	public void ValidateMultipleIgnoredFlagsTests (ObjCBindings.Property flags, int expectedDiagnosticsCount)
	{
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (flags: flags);

		var result = new List<Diagnostic> ();
		foreach (var (_, value) in validator.ValidateAll (property, context)) {
			result.AddRange (value);
		}

		Assert.NotEmpty (result);
		Assert.Equal (expectedDiagnosticsCount, result.Count);
		Assert.All (result, d => Assert.Equal ("RBI0028", d.Id));
	}

	[Fact]
	public void ValidateValidFieldPropertyTests ()
	{
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (); // Default: static, partial, no invalid flags

		var result = validator.ValidateAll (property, context);
		Assert.Empty (result);
	}

	[Theory]
	[InlineData ("validSelector", true, 0)]
	[InlineData ("", true, 0)]
	[InlineData (null, false, 1)]
	[InlineData ("another_valid_selector", true, 0)]
	[InlineData ("valid123", true, 0)]
	public void SelectorIsNotNullOrEmptyTests (string? selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (selector: selector);

		var result = validator.ValidateAll (property, context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var selectorDiagnostics = diagnostics.Where (d => d.Id == "RBI0018").ToList ();

		Assert.Equal (expectedDiagnosticsCount, selectorDiagnostics.Count);
		if (!expectedResult) {
			Assert.All (selectorDiagnostics, d => Assert.Equal ("RBI0018", d.Id));
		}
	}

	[Theory]
	[InlineData ("validSelector", true, 0)]
	[InlineData ("invalid selector", false, 1)]
	[InlineData ("selector\twith\ttab", false, 1)]
	[InlineData ("selector\nwith\nnewline", false, 1)]
	[InlineData ("selector with space", false, 1)]
	[InlineData ("valid_selector", true, 0)]
	[InlineData ("validSelector123", true, 0)]
	[InlineData (" leadingSpace", false, 1)]
	[InlineData ("trailingSpace ", false, 1)]
	public void SelectorHasNoWhitespaceTests (string selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var validator = new FieldValidator ();
		var property = CreateFieldProperty (selector: selector);

		var result = validator.ValidateAll (property, context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var whitespaceDiagnostics = diagnostics.Where (d => d.Id == "RBI0019").ToList ();

		Assert.Equal (expectedDiagnosticsCount, whitespaceDiagnostics.Count);
		if (!expectedResult) {
			Assert.All (whitespaceDiagnostics, d => Assert.Equal ("RBI0019", d.Id));
		}
	}
}
