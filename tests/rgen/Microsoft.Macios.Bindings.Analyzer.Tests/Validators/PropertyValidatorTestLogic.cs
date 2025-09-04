// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

/// <summary>
/// Contains shared test logic for property validators.
/// </summary>
class PropertyValidatorTestLogic {

	readonly RootContext context;
	readonly Validator<Property> validator;

	public PropertyValidatorTestLogic (Validator<Property> validator)
	{
		this.validator = validator;

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

	/// <summary>
	/// Creates a property for testing purposes.
	/// </summary>
	Property CreateProperty (string name = "TestProperty",
		bool isPartial = true,
		string? propertySelector = "testProperty",
		ObjCBindings.Property propertyFlags = ObjCBindings.Property.Default,
		ArgumentSemantic argumentSemantic = ArgumentSemantic.None,
		string? getterSelector = null,
		string? setterSelector = null,
		bool hasGetter = true,
		bool hasSetter = true)
	{
		var modifiers = ImmutableArray.CreateBuilder<SyntaxToken> ();
		modifiers.Add (SyntaxFactory.Token (SyntaxKind.PublicKeyword));
		if (isPartial)
			modifiers.Add (SyntaxFactory.Token (SyntaxKind.PartialKeyword));

		var accessors = ImmutableArray.CreateBuilder<Accessor> ();

		if (hasGetter) {
			getterSelector ??= "sampleGetter";
			var getterExportData = new ExportData<ObjCBindings.Property> (getterSelector,
				argumentSemantic, ObjCBindings.Property.Default);

			accessors.Add (new Accessor (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new SymbolAvailability.Builder ().ToImmutable (),
				exportPropertyData: getterExportData,
				attributes: [],
				modifiers: []
			));
		}

		if (hasSetter) {
			setterSelector ??= "sampleSetter:";
			var setterExportData = new ExportData<ObjCBindings.Property> (setterSelector,
				argumentSemantic, ObjCBindings.Property.Default);

			accessors.Add (new Accessor (
				accessorKind: AccessorKind.Setter,
				symbolAvailability: new SymbolAvailability.Builder ().ToImmutable (),
				exportPropertyData: setterExportData,
				attributes: [],
				modifiers: []
			));
		}

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
			accessors: accessors.ToImmutable ()
		) {
			ExportPropertyData = new ExportData<ObjCBindings.Property> (propertySelector,
				argumentSemantic, propertyFlags)
		};
	}

	/// <summary>
	/// Creates a field property for testing purposes.
	/// </summary>
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

	// Property test implementations
	public void ValidatePropertyModifiersTestsImpl (bool isPartial, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (isPartial: isPartial);
		var result = validator.ValidateAll (property, context);
		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	public void PropertySelectorIsNotNullTestsImpl (string? selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (propertySelector: selector);
		var result = validator.ValidateAll (property, context);
		Assert.Equal (expectedResult, result.Count == 0);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var selectorDiagnostics = diagnostics.Where (d => d.Id == "RBI0018").ToList ();
		Assert.Equal (expectedDiagnosticsCount, selectorDiagnostics.Count);
	}

	public void PropertySelectorHasNoWhitespaceTestsImpl (string selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (propertySelector: selector);
		var result = validator.ValidateAll (property, context);
		Assert.Equal (expectedResult, result.Count == 0);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var whitespaceDiagnostics = diagnostics.Where (d => d.Id == "RBI0019").ToList ();
		Assert.Equal (expectedDiagnosticsCount, whitespaceDiagnostics.Count);
	}

	public void AccessorSelectorValidationTestsImpl (string? getterSelector, string? setterSelector, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (
			getterSelector: getterSelector,
			setterSelector: setterSelector
		);

		var result = validator.ValidateAll (property, context);
		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var selectorDiagnostics = diagnostics.Where (d => d.Id == "RBI0018").ToList ();
		Assert.Equal (expectedDiagnosticsCount, selectorDiagnostics.Count);
	}

	public void AccessorSelectorWhitespaceTestsImpl (string getterSelector, string setterSelector, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (
			getterSelector: getterSelector,
			setterSelector: setterSelector
		);

		var result = validator.ValidateAll (property, context);
		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var whitespaceDiagnostics = diagnostics.Where (d => d.Id == "RBI0019").ToList ();
		Assert.Equal (expectedDiagnosticsCount, whitespaceDiagnostics.Count);
	}

	public void AccessorSelectorArgCountTestsImpl (string getterSelector, string setterSelector, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (
			getterSelector: getterSelector,
			setterSelector: setterSelector
		);

		var result = validator.ValidateAll (property, context);
		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var argCountDiagnostics = diagnostics.Where (d => d.Id == "RBI0029").ToList ();
		Assert.Equal (expectedDiagnosticsCount, argCountDiagnostics.Count);
	}

	public void PropertyAccessorPresenceTestsImpl (bool hasGetter, bool hasSetter, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (
			hasGetter: hasGetter,
			hasSetter: hasSetter
		);

		var result = validator.ValidateAll (property, context);
		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	public void CombinedPropertyValidationTestsImpl (bool isPartial, string? getterSelector, string? setterSelector, int expectedDiagnosticsCount)
	{
		var property = CreateProperty (
			isPartial: isPartial,
			getterSelector: getterSelector,
			setterSelector: setterSelector
		);

		var result = validator.ValidateAll (property, context);
		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	public void ValidateValidPropertyTestsImpl ()
	{
		var property = CreateProperty (); // Default: partial, valid selectors
		var result = validator.ValidateAll (property, context);
		Assert.Empty (result);
	}

	// Field test implementations
	public void ValidateFieldModifiersTestsImpl (bool isStatic, bool isPartial, int expectedDiagnosticsCount)
	{
		var property = CreateFieldProperty (isStatic: isStatic, isPartial: isPartial);
		var result = validator.ValidateAll (property, context);
		var totalDiagnostics = result.Values.Sum (x => x.Count);
		Assert.Equal (expectedDiagnosticsCount, totalDiagnostics);
	}

	public void ValidateIgnoredFlagsTestsImpl (ObjCBindings.Property flags, int expectedDiagnosticsCount)
	{
		var property = CreateFieldProperty (flags: flags);
		var result = new List<Diagnostic> ();
		foreach (var (_, value) in validator.ValidateAll (property, context)) {
			result.AddRange (value);
		}

		if (expectedDiagnosticsCount > 0) {
			Assert.Equal (expectedDiagnosticsCount, result.Count);
			Assert.All (result, d => Assert.Equal ("RBI0028", d.Id));
		} else {
			Assert.Empty (result);
		}
	}

	public void ValidateMultipleIgnoredFlagsTestsImpl (ObjCBindings.Property flags, int expectedDiagnosticsCount)
	{
		var property = CreateFieldProperty (flags: flags);
		var result = new List<Diagnostic> ();
		foreach (var (_, value) in validator.ValidateAll (property, context)) {
			result.AddRange (value);
		}

		Assert.NotEmpty (result);
		Assert.Equal (expectedDiagnosticsCount, result.Count);
		Assert.All (result, d => Assert.Equal ("RBI0028", d.Id));
	}

	public void FieldSelectorIsNotNullOrEmptyTestsImpl (string? selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var property = CreateFieldProperty (selector: selector);
		var result = validator.ValidateAll (property, context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var selectorDiagnostics = diagnostics.Where (d => d.Id == "RBI0018").ToList ();

		Assert.Equal (expectedDiagnosticsCount, selectorDiagnostics.Count);
		if (!expectedResult) {
			Assert.All (selectorDiagnostics, d => Assert.Equal ("RBI0018", d.Id));
		}
	}

	public void FieldSelectorHasNoWhitespaceTestsImpl (string selector, bool expectedResult, int expectedDiagnosticsCount)
	{
		var property = CreateFieldProperty (selector: selector);
		var result = validator.ValidateAll (property, context);

		var diagnostics = result.Values.SelectMany (x => x).ToList ();
		var whitespaceDiagnostics = diagnostics.Where (d => d.Id == "RBI0019").ToList ();

		Assert.Equal (expectedDiagnosticsCount, whitespaceDiagnostics.Count);
		if (!expectedResult) {
			Assert.All (whitespaceDiagnostics, d => Assert.Equal ("RBI0019", d.Id));
		}
	}

	public void ValidateValidFieldPropertyTestsImpl ()
	{
		var property = CreateFieldProperty (); // Default: static, partial, no invalid flags
		var result = validator.ValidateAll (property, context);
		Assert.Empty (result);
	}
}

