// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Base validator class for validating binding definitions.
/// </summary>
abstract class BindingValidator : Validator<Binding> {

	/// <summary>
	/// Validates that the binding type matches the expected binding type.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="expectedBindingType">The expected binding type.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the binding type is invalid; otherwise, an empty array.</param>
	/// <param name="location">The optional location for the diagnostic.</param>
	/// <returns><c>true</c> if the binding type matches the expected type; otherwise, <c>false</c>.</returns>
	internal static bool IsValidBindingType (Binding binding, BindingType expectedBindingType,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		// if the binding type is not a smart enum, add a diagnostic for an unexpected binding type
		if (binding.BindingType == expectedBindingType)
			return true;
		diagnostics = [Diagnostic.Create (
			RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
			location: location,
			messageArgs: binding.FullyQualifiedSymbol
		)];
		return false;
	}

	/// <summary>
	/// Validates that a binding has the partial modifier.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">The collection of diagnostics returned when validation fails.</param>
	/// <param name="location">The optional location for the diagnostic.</param>
	/// <returns>True if the binding has the partial modifier; otherwise, false.</returns>
	internal static bool IsPartial (Binding binding, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsPartial (binding.Modifiers, RBI0001, out diagnostics, location, binding.FullyQualifiedSymbol);

	/// <summary>
	/// Validates that a binding has the static modifier.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">The collection of diagnostics returned when validation fails.</param>
	/// <param name="location">The optional location for the diagnostic.</param>
	/// <returns>True if the binding has the static modifier; otherwise, false.</returns>
	internal static bool IsStatic (Binding binding, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsStatic (binding.Modifiers, RBI0004, out diagnostics, location, binding.FullyQualifiedSymbol);

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingValidator"/> class.
	/// </summary>
	public BindingValidator () : base (b => b.Location) { }
}
