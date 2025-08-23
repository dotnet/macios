// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

public static class StringStrategies {
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
	/// <param name="stringValue">The data to validate.</param>
	/// <param name="descriptor">The descriptor to use for the error.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <param name="messageArgs">Extra args for the diagnostic message.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool HasNoWhitespace (string? stringValue, DiagnosticDescriptor descriptor, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null, params object [] messageArgs)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (stringValue is null || !stringValue.Any (char.IsWhiteSpace))
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: descriptor, // A export property selector must not contain any whitespace.
				location: location,
				messageArgs: messageArgs)
		];
		return false;
	}

	/// <summary>
	/// Validates that a native name (prefix or suffix) does not contain any whitespace.
	/// </summary>
	/// <param name="suffix">The native name to validate.</param>
	/// <param name="fieldName">The name of the field being validated.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool NativeNameHasNoWhitespace (string? suffix, string fieldName, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> HasNoWhitespace (
			stringValue: suffix,
			descriptor: RBI0024,
			diagnostics: out diagnostics,
			location: location,
			messageArgs: fieldName
		);

	/// <summary>
	/// Validates that a type name does not contain any whitespace.
	/// </summary>
	/// <param name="typeName">The type name to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <param name="messageArgs">Extra args for the diagnostic message.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool TypeNameHasNoWhitespace (string? typeName, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null, params object [] messageArgs)
		=> HasNoWhitespace (
			stringValue: typeName,
			descriptor: RBI0025,
			diagnostics: out diagnostics,
			location: location,
			messageArgs: messageArgs
		);
}
