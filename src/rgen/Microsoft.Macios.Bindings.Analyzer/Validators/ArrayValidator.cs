// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for arrays of items, which validates each item in the array using an inner validator.
/// </summary>
/// <typeparam name="T">The type of items in the array to validate.</typeparam>
class ArrayValidator<T> : Validator<ImmutableArray<T>> {
	/// <summary>
	/// Validates that all items in the array are valid using the inner validator.
	/// </summary>
	/// <param name="property">The array of items to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if any items are invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all items in the array are valid; otherwise, <c>false</c>.</returns>
	bool AllItemsAreValid (ImmutableArray<T> property, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		// loop over all the variables, validate them and collect the diagnostics
		foreach (var prop in property) {
			var errors = validator.ValidateAll (prop, context);
			if (errors.Count > 0) {
				// flatten and add all the errors in the builder
				builder.AddRange (errors.SelectMany (x => x.Value));
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// The validator used internally to validate each of the items in the array.
	/// </summary>
	readonly Validator<T> validator;

	/// <summary>
	/// Initializes a new instance of the <see cref="ArrayValidator{T}"/> class.
	/// </summary>
	/// <param name="innerValidator">The validator to use for validating each item in the array.</param>
	public ArrayValidator (Validator<T> innerValidator)
	{
		validator = innerValidator;
		// add a global strategy that will validate each of the properties in the array
		AddGlobalStrategy (validator.Descriptors, AllItemsAreValid);
	}
}
