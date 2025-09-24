// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Macios.Generator.Availability;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.Macios.Generator.Tests;

/// <summary>
/// Provides a set of custom assertion methods that can be used with xUnit, including support for multiple assertion scopes.
/// </summary>
public class AssertEx {

	/// <summary>
	/// Verifies that two <see cref="PlatformAvailability"/> instances are equal by comparing their properties within a multiple assertion scope.
	/// </summary>
	/// <param name="expected">The expected platform availability.</param>
	/// <param name="actual">The actual platform availability.</param>
	internal static void Equal (PlatformAvailability? expected, PlatformAvailability? actual)
	{
		var obsoleteComparer = new DictionaryComparer<Version, (string?, string?)> ();
		var unsupportedComparer = new DictionaryComparer<Version, string?> ();
		Assert.Multiple (
			() => Assert.Equal (expected?.Platform, actual?.Platform),
			() => Assert.Equal (expected?.SupportedVersion, actual?.SupportedVersion),
			() => Assert.True (unsupportedComparer.Equals (expected?.UnsupportedVersions, actual?.UnsupportedVersions)),
			() => Assert.True (obsoleteComparer.Equals (expected?.ObsoletedVersions, actual?.ObsoletedVersions))
		);
	}

	/// <summary>
	/// Verifies that two <see cref="SymbolAvailability"/> instances are equal by comparing the availability for each supported platform within a multiple assertion scope.
	/// </summary>
	/// <param name="expected">The expected symbol availability.</param>
	/// <param name="actual">The actual symbol availability.</param>
	internal static void Equal (SymbolAvailability expected, SymbolAvailability actual)
	{
		var platformActions = new List<Action> ();
		// compare each of the platforms individually
		foreach (var platform in SymbolAvailability.SupportedPlatforms) {
			platformActions.Add (() => Equal (expected [platform], actual [platform]));
		}

		Assert.Multiple (platformActions.ToArray ());
	}
}
