// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// An implementation of <see cref="IFieldValidationStrategy{T}"/> that uses a lambda function for validation.
/// </summary>
/// <typeparam name="T">The type of the data to validate.</typeparam>
/// <typeparam name="TField">The type of the field to validate.</typeparam>
/// <param name="descriptor">The diagnostic descriptors that this validation strategy can produce.</param>
/// <param name="validationFunc">The function to use for validation.</param>
class LambdaFieldValidationStrategy<T, TField> (
	ImmutableArray<DiagnosticDescriptor> descriptor,
	Expression<Func<T, TField>> selector,
	LambdaFieldValidationStrategy<T, TField>.ValidationFunc validationFunc)
	: IFieldValidationStrategy<T> {

	// cache the compilation of the selector expression
	readonly Func<T, TField> selectorFunc = selector.Compile ();

	/// <summary>
	/// Represents the method that will handle the validation of the data.
	/// </summary>
	/// <param name="data">The data to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostic">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	public delegate bool ValidationFunc (TField data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location = null);

	/// <inheritdoc />
	public ImmutableArray<DiagnosticDescriptor> Descriptors { get; } = descriptor;

	/// <inheritdoc />
	public bool IsValid (T data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location = null)
	{
		// use the selector to get the field value
		var fieldValue = selectorFunc (data);
		return validationFunc (fieldValue, context, out diagnostic, location);
	}
}
