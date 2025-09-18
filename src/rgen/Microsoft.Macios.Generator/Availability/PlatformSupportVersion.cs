// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Macios.Generator.Availability;

/// <summary>
/// Represents a platform support version, combining a version number and a support kind.
/// </summary>
public readonly record struct PlatformSupportVersion {
	/// <summary>
	/// Gets the version number.
	/// </summary>
	public Version Version { get; init; }
	/// <summary>
	/// Gets the kind of support (e.g., explicit, implicit).
	/// </summary>
	public SupportKind Kind { get; init; }

	/// <summary>
	/// Gets a default platform support version with an implicit kind.
	/// </summary>
	public static PlatformSupportVersion ImplicitDefault { get; } = new () {
		Version = new (),
		Kind = SupportKind.Implicit
	};

	/// <summary>
	/// Gets a default platform support version with an explicit kind.
	/// </summary>
	public static PlatformSupportVersion ExplicitDefault { get; } = new () {
		Version = new (),
		Kind = SupportKind.Explicit
	};

	/// <summary>
	/// Returns the platform support version with the highest precedence.
	/// </summary>
	/// <param name="v1">The first platform support version to compare.</param>
	/// <param name="v2">The second platform support version to compare.</param>
	/// <returns>
	/// The platform support version with the highest precedence. If the kinds are the same, it returns the one with the greater version.
	/// If the kinds are different, it returns the one with the higher kind value.
	/// </returns>
	public static PlatformSupportVersion Max (PlatformSupportVersion v1, PlatformSupportVersion v2)
	{
		if (v1.Kind == v2.Kind) {
			return v1.Version >= v2.Version ? v1 : v2;
		}
		return (int) v1.Kind > (int) v2.Kind ? v1 : v2;
	}

	/// <summary>
	/// Returns the platform support version with the lowest version if the kinds are the same, otherwise returns the one with the highest precedence kind.
	/// </summary>
	/// <param name="v1">The first platform support version to compare.</param>
	/// <param name="v2">The second platform support version to compare.</param>
	/// <returns>
	/// The platform support version with the lowest version if the kinds are the same.
	/// If the kinds are different, it returns the one with the higher kind value.
	/// </returns>
	public static PlatformSupportVersion Min (PlatformSupportVersion v1, PlatformSupportVersion v2)
	{
		if (v1.Kind == v2.Kind) {
			return v1.Version <= v2.Version ? v1 : v2;
		}
		return (int) v1.Kind > (int) v2.Kind ? v1 : v2;
	}
}
