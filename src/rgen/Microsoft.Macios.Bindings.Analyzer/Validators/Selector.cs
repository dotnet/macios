// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

public static class Selector {
	/// <summary>
	/// Validates that the selector is not null.
	/// </summary>
	/// <param name="selector">The data to validate.</param>
	/// <param name="descriptor">The descriptor to use for the error.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool IsNotNull (string? selector, DiagnosticDescriptor descriptor, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (selector is not null)
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: descriptor, // A export property must have a selector defined
				location: location)
		];
		return false;
	}

	/// <summary>
	/// Validates that the selector does not contain any whitespace.
	/// </summary>
	/// <param name="selector">The data to validate.</param>
	/// <param name="descriptor">The descriptor to use for the error.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool HasNoWhitespace (string? selector, DiagnosticDescriptor descriptor, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (selector is null || !selector.Any (char.IsWhiteSpace))
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: descriptor, // A export property selector must not contain any whitespace.
				location: location)
		];
		return false;
	}
}
