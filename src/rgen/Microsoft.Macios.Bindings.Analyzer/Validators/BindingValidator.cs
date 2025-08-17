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
