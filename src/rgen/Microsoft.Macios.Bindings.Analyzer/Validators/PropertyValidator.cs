// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;
using static Microsoft.Macios.Bindings.Analyzer.Validators.PropertyStrategies;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for property bindings.
/// </summary>
class PropertyValidator : Validator<Property> {
	readonly ExportPropertyAttributeValidator exportPropertyValidator = new ();

	/// <summary>
	/// Validates that a property accessor has valid export attributes when present.
	/// This method checks that exported accessors have non-null selectors, no whitespace in selectors,
	/// and that the selector argument count matches the expected count (0 for getters, 1 for setters).
	/// </summary>
	/// <param name="data">A tuple containing the property name and accessor to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the accessor is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the accessor is valid or has no export attributes; otherwise, <c>false</c>.</returns>
	internal static bool AccessorIsValid ((string PropertyName, Accessor Accessor) data, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (data.Accessor.IsNullOrDefault) {
			// nothing to check, it is valid
			return true;
		}

		// if the accessor is present AND only if the export property data is not null, we need to validate it
		if (data.Accessor.ExportPropertyData.IsNullOrDefault) {
			// no need to check, it was not exported
			return true;
		}

		// validate the export property data, in this case we are just going to validate the selector:

		// 1. Is not null
		if (!StringStrategies.IsNotNullOrEmpty (
			selector: data.Accessor.ExportPropertyData.Selector,
			descriptor: RBI0018, // A export property must have a selector defined
			diagnostics: out diagnostics,
			location: location)) {
			return false;
		}

		// 2. Is not white spaces
		if (!StringStrategies.HasNoWhitespace (
				stringValue: data.Accessor.ExportPropertyData.Selector,
				descriptor: RBI0019, // A export property selector must not contain any whitespace.
				diagnostics: out diagnostics,
				location: location)) {
			return false;
		}

		// 3. selector argument count
		return StringStrategies.MatchingSelectorArgCount (
			data.PropertyName,
			data.Accessor.ExportPropertyData.Selector,
			data.Accessor.Kind == AccessorKind.Getter ? 0 : 1,
			out diagnostics);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PropertyValidator"/> class.
	/// </summary>
	public PropertyValidator () : base (p => p.Location)
	{
		// validate the export property data of the current property
		AddNestedValidator (p => p.ExportPropertyData, exportPropertyValidator);

		// properties must be partial 
		AddGlobalStrategy (RBI0031, IsPartial);

		// check platform validity for the property and its accessors
		AddGlobalStrategy ([RBI0000, RBI0027], IsValidPlatform);

		// ensure that if the accessors have an export attribute, that the values are correct
		AddStrategy<(string PropertyName, Accessor accessor)> (
			selector: p => new (p.Name, p.GetAccessor (AccessorKind.Getter)),
			descriptor: [RBI0018, RBI0019, RBI0029],
			validation: AccessorIsValid,
			propertyName: "getter");

		AddStrategy<(string PropertyName, Accessor accessor)> (
			selector: p => new (p.Name, p.GetAccessor (AccessorKind.Setter)),
			descriptor: [RBI0018, RBI0019, RBI0029],
			validation: AccessorIsValid,
			propertyName: "setter");
	}
}
