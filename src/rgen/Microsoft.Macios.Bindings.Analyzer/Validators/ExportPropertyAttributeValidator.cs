// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validates <see cref="ExportData{T}"/> for properties.
/// </summary>
class ExportPropertyAttributeValidator : Validator<ExportData<Property>> {

	/// <summary>
	/// Diagnostic descriptor for when a property export is missing a selector.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0018 = new (
		"RBI0018",
		new LocalizableResourceString (nameof (Resources.RBI0018Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0018MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0018Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a property export selector contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0019 = new (
		"RBI0019",
		new LocalizableResourceString (nameof (Resources.RBI0019Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0018MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0019Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Validates that the selector is not null.
	/// </summary>
	/// <param name="selector">The data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool SelectorIsNotNull (string? selector, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> Selector.IsNotNull (
			selector: selector,
			descriptor: RBI0018, // A export property must have a selector defined
			diagnostics: out diagnostics,
			location: location);

	/// <summary>
	/// Validates that the selector does not contain any whitespace.
	/// </summary>
	/// <param name="selector">The data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool SelectorHasNoWhitespace (string? selector, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> Selector.HasNoWhitespace (
			selector: selector,
			descriptor: RBI0019, // A export property selector must not contain any whitespace.
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Initializes a new instance of the <see cref="ExportPropertyAttributeValidator"/> class.
	/// </summary>
	public ExportPropertyAttributeValidator ()
	{
		// add the default rules for this validator
		AddStrategy (d => d.Selector, RBI0018, SelectorIsNotNull);
		AddStrategy (d => d.Selector, RBI0019, SelectorHasNoWhitespace);
		// only with methods
		RestrictToFlagType (
			d => d.ResultType,
			d => d.Flags,
			d => d.IsNullOrDefault,
			typeof (Method)
		);
		// only with async methods
		RestrictToFlagType (
			d => d.MethodName,
			d => d.Flags,
			typeof (Method)
		);

		// only with async methods
		RestrictToFlagType (
			d => d.ResultTypeName,
			d => d.Flags,
			typeof (Method)
		);

		// only with async methods
		RestrictToFlagType (
			d => d.PostNonResultSnippet,
			d => d.Flags,
			typeof (Method)
		);

		// only with strong dictionary keys
		RestrictToFlagType (
			d => d.StrongDictionaryKeyClass,
			d => d.Flags,
			d => d.IsNullOrDefault,
			typeof (StrongDictionaryKeys)
		);

		// only with methods
		RestrictToFlagType (
			d => d.EventArgsType,
			d => d.Flags,
			d => d.IsNullOrDefault,
			typeof (Method)
		);

		// only with methods
		RestrictToFlagType (
			d => d.EventArgsTypeName,
			d => d.Flags,
			typeof (Method)
		);

	}
}
