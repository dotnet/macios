// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;

namespace Microsoft.Macios.Bindings.Analyzer;

interface IFieldValidationStrategy {

	/// <summary>
	/// Gets the diagnostic descriptors that this validation strategy can produce.
	/// </summary>
	ImmutableArray<DiagnosticDescriptor> Descriptors { get; }

	/// <summary>
	/// Validates the given data.
	/// </summary>
	/// <param name="data">The data to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostic">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	bool IsValid (object? data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location = null);
}

/// <summary>
/// Defines a strategy for validating a field of a given type.
/// </summary>
/// <typeparam name="T">The type of the data to validate.</typeparam>
interface IFieldValidationStrategy<in T> : IFieldValidationStrategy {

	/// <summary>
	/// Validates the given data.
	/// </summary>
	/// <param name="data">The data to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostic">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	bool IsValid (T data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location = null);

	bool IFieldValidationStrategy.IsValid (object? data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location)
	{
		if (data is T typedData)
			return IsValid (typedData, context, out diagnostic, location);

		diagnostic = ImmutableArray<Diagnostic>.Empty;
		return false;
	}

}
