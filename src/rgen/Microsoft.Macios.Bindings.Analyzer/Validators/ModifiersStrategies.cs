// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Provides validation strategies for C# type and member modifiers.
/// </summary>
public static class ModifiersStrategies {

	/// <summary>
	/// Validates that the given modifiers include the partial keyword.
	/// </summary>
	/// <param name="modifiers">The collection of syntax tokens representing modifiers.</param>
	/// <param name="descriptor">The diagnostic descriptor to use when validation fails.</param>
	/// <param name="diagnostics">The collection of diagnostics returned when validation fails.</param>
	/// <param name="location">The optional location for the diagnostic.</param>
	/// <param name="msgArgs">The optional message arguments for the diagnostic.</param>
	/// <returns>True if the modifiers contain the partial keyword; otherwise, false.</returns>
	internal static bool IsPartial (ImmutableArray<SyntaxToken> modifiers, DiagnosticDescriptor descriptor, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null, params object [] msgArgs)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (modifiers.Any (x => x.IsKind (SyntaxKind.PartialKeyword)))
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: descriptor, // has to be partial, e.g. "RBI0001: All binding types should be partial."
				location: location,
				messageArgs: msgArgs)
		];
		return false;
	}

	/// <summary>
	/// Validates that the given modifiers include the static keyword.
	/// </summary>
	/// <param name="modifiers">The collection of syntax tokens representing modifiers.</param>
	/// <param name="descriptor">The diagnostic descriptor to use when validation fails.</param>
	/// <param name="diagnostics">The collection of diagnostics returned when validation fails.</param>
	/// <param name="location">The optional location for the diagnostic.</param>
	/// <param name="msgArgs">The optional message arguments for the diagnostic.</param>
	/// <returns>True if the modifiers contain the static keyword; otherwise, false.</returns>
	internal static bool IsStatic (ImmutableArray<SyntaxToken> modifiers, DiagnosticDescriptor descriptor, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null, params object [] msgArgs)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (modifiers.Any (x => x.IsKind (SyntaxKind.StaticKeyword)))
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: descriptor, // has to be partial, e.g. "RBI0001: All binding types should be partial."
				location: location,
				messageArgs: msgArgs)
		];
		return false;
	}
}
