// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Macios.Generator.Availability;

/// <summary>
/// Compares two <see cref="PlatformSupportVersion"/> instances for sorting.
/// </summary>
public class PlatformSupportVersionComparer : IComparer<PlatformSupportVersion> {

	/// <summary>
	/// Compares two platform support versions and returns a value indicating whether one is less than, equal to, or greater than the other.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>A signed integer that indicates the relative values of x and y.</returns>
	public int Compare (PlatformSupportVersion x, PlatformSupportVersion y)
	{
		int versionComparison = x.Version.CompareTo (y.Version);
		if (versionComparison != 0) return versionComparison;
		return x.Kind.CompareTo (y.Kind);
	}
}
