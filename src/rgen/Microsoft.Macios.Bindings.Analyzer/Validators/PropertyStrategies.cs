// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

static class PropertyStrategies {

	/// <summary>
	/// Validates that the field property and its accessors are available on the current platform.
	/// This method checks platform availability for the property itself, its getter (if present), and its setter (if present).
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property or its accessors are not available on the current platform; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property and all its accessors are available on the current platform; otherwise, <c>false</c>.</returns>
	internal static bool IsValidPlatform (Property property, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;

		// there are several locations in which we need to check if the field is valid for the platform
		// 1. the property itself
		// 2. the getter if present
		// 3. the setter if present
		if (!SupportedPlatformStrategies.IsValidPlatform (property.SymbolAvailability,
				context, out diagnostics, property.Name, property.Location)) {
			return false;
		}

		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		// if we have a getter or a setter, we want to validate them as well but we will return those merged in the
		// diagnostics rather than returning false if one of them is invalid
		var getter = property.GetAccessor (AccessorKind.Getter);
		if (!getter.IsNullOrDefault) {
			if (!SupportedPlatformStrategies.IsValidPlatform (getter.SymbolAvailability, context,
					out var getterDiagnostics,
					$"{property.Name}.get", getter.Location)) {
				builder.AddRange (getterDiagnostics);
			}
		}

		var setter = property.GetAccessor (AccessorKind.Setter);
		if (!setter.IsNullOrDefault) {
			if (!SupportedPlatformStrategies.IsValidPlatform (setter.SymbolAvailability, context,
					out var setterDiagnostics,
					$"{property.Name}.set", setter.Location)) {
				builder.AddRange (setterDiagnostics);
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that a field property has the partial modifier.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property has the partial modifier; otherwise, <c>false</c>.</returns>
	internal static bool IsPartial (Property property, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsPartial (property.Modifiers, RBI0031, out diagnostics, location, property.Name);

	/// <summary>
	/// Validates that a field property has the static modifier.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property has the static modifier; otherwise, <c>false</c>.</returns>
	internal static bool IsStatic (Property property, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsStatic (property.Modifiers, RBI0030, out diagnostics, location, property.Name);

}
