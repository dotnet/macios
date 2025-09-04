// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Bindings.Analyzer.Extensions;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using Xamarin.Utils;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Provides validation strategies for verifying platform support compatibility.
/// </summary>
static class SupportedPlatformStrategies {

	/// <summary>
	/// Validates that a symbol is available on the current platform.
	/// </summary>
	/// <param name="availability">The symbol availability information.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the platform is not supported; otherwise, an empty array.</param>
	/// <param name="symbolName">The name of the symbol being validated.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the symbol is available on the current platform; otherwise, <c>false</c>.</returns>
	internal static bool IsValidPlatform (SymbolAvailability availability, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, string symbolName, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		// retrieve the availability platform of the current platform found in the root context, if the platform is 
		// not supported, we return false
		var platform = context.CurrentPlatform.ToApplePlatform ();
		if (platform == ApplePlatform.None)
			return true;

		var currentPlatformAvailability = availability [platform];
		if (currentPlatformAvailability is { IsSupported: true })
			return true;
		diagnostics = [
			Diagnostic.Create (
				descriptor: RBI0027,
				location: location,
				symbolName, platform
			)
		];
		return false;

	}
}
