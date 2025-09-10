// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using ObjCBindings;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for category bindings.
/// </summary>
sealed class CategoryValidator : BindingValidator {

	/// <summary>
	/// Validates that all methods in a category binding are properly declared as partial extension methods
	/// with the correct first parameter type matching the category's extended type.
	/// </summary>
	/// <param name="binding">The category binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any invalid methods; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all methods are valid; otherwise, <c>false</c>.</returns>
	bool ValidMethods (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		// we need to make sure that all the methods of the category are:
		// 1. partial
		// 2. static
		// 3. Have the correct first parameter type (the type that the category is extending)
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var extensionMethod in binding.Methods) {
			if (!ModifiersStrategies.IsPartial (extensionMethod.Modifiers, RBI0042,
					out var partialDiagnostics, extensionMethod.Location, extensionMethod.Name)) {
				builder.AddRange (partialDiagnostics);
			}
			// check that it is an extension method
			var bindingData = (BindingTypeData<Category>) binding.BindingInfo;
			if (extensionMethod.IsExtension) {
				// ensure that the first parameter type matches the type that the category is extending
				if (extensionMethod.Parameters [0].Type.FullyQualifiedName !=
					bindingData.CategoryType.FullyQualifiedName) {
					// we do not allow to mix types in the extension methods in a category
					builder.Add (Diagnostic.Create (
						RBI0044, // Extension methods in a category must have the first parameter type match the category's extended type.
						extensionMethod.Location,
						extensionMethod.Name,
						binding.Name,
						bindingData.CategoryType.FullyQualifiedName,
						extensionMethod.Parameters [0].Type.FullyQualifiedName));
				}
			} else {
				// method should be an extension
				builder.Add (Diagnostic.Create (
					RBI0043, // Extension methods in a category must be declared as extension methods.
					extensionMethod.Location,
					extensionMethod.Name,
					binding.Name,
					bindingData.CategoryType.FullyQualifiedName
					));
			}
		}

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryValidator"/> class.
	/// </summary>
	public CategoryValidator ()
	{
		// all bindings must be partial
		AddGlobalStrategy (RBI0001, IsPartial);
		// categories must be static
		AddGlobalStrategy (RBI0004, IsStatic);
		// validate all methods in the category binding
		AddGlobalStrategy ([RBI0042, RBI0043, RBI0044], ValidMethods);
	}
}
