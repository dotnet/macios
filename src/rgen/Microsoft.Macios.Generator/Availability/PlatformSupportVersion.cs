// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Macios.Generator.Availability;

/// <summary>
/// Represents a platform support version, combining a version number and a support kind.
/// </summary>
public readonly record struct PlatformSupportVersion : IComparable<PlatformSupportVersion> {
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
	/// Initializes a new instance of the <see cref="PlatformSupportVersion"/> struct.
	/// </summary>
	/// <param name="version">The version number.</param>
	/// <param name="kind">The kind of support.</param>
	public PlatformSupportVersion (Version version, SupportKind kind)
	{
		Version = version;
		Kind = kind;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PlatformSupportVersion"/> struct with an explicit support kind.
	/// </summary>
	/// <param name="version">The version number.</param>
	public PlatformSupportVersion (Version version) : this (version, SupportKind.Explicit) { }

	/// <summary>
	/// Returns the platform support version with the highest precedence.
	/// </summary>
	/// <param name="v1">The first platform support version to compare.</param>
	/// <param name="v2">The second platform support version to compare.</param>
	/// <returns>
	/// The platform support version with the highest precedence. If the kinds are the same, it returns the one with the greater version.
	/// If the kinds are different, it returns the one with the higher kind value.
	/// </returns>
	public static PlatformSupportVersion? Max (PlatformSupportVersion? v1, PlatformSupportVersion? v2)
	{
		if (v1 is null)
			return v2;
		if (v2 is null)
			return v1;

		if (v1.Value.Kind == v2.Value.Kind) {
			return v1.Value.Version >= v2.Value.Version ? v1 : v2;
		}
		return (int) v1.Value.Kind > (int) v2.Value.Kind ? v1 : v2;
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
	public static PlatformSupportVersion? Min (PlatformSupportVersion? v1, PlatformSupportVersion? v2)
	{
		if (v1 is null)
			return v2;
		if (v2 is null)
			return v1;
		if (v1.Value.Kind == v2.Value.Kind) {
			return v1.Value.Version <= v2.Value.Version ? v1 : v2;
		}
		return (int) v1.Value.Kind > (int) v2.Value.Kind ? v1 : v2;
	}

	/// <inheritdoc />
	public int CompareTo (PlatformSupportVersion other)
	{
		var versionComparison = Version.CompareTo (other.Version);
		if (versionComparison != 0)
			return versionComparison;
		return Kind.CompareTo (other.Kind);
	}

	/// <summary>
	/// Compares two <see cref="PlatformSupportVersion"/> instances to determine if the left is less than the right.
	/// </summary>
	/// <param name="left">The first <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <param name="right">The second <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <returns><c>true</c> if the left instance is less than the right instance; otherwise, <c>false</c>.</returns>
	public static bool operator < (PlatformSupportVersion left, PlatformSupportVersion right)
	{
		return left.CompareTo (right) < 0;
	}

	/// <summary>
	/// Compares two <see cref="PlatformSupportVersion"/> instances to determine if the left is greater than the right.
	/// </summary>
	/// <param name="left">The first <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <param name="right">The second <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <returns><c>true</c> if the left instance is greater than the right instance; otherwise, <c>false</c>.</returns>
	public static bool operator > (PlatformSupportVersion left, PlatformSupportVersion right)
	{
		return left.CompareTo (right) > 0;
	}

	/// <summary>
	/// Compares two <see cref="PlatformSupportVersion"/> instances to determine if the left is less than or equal to the right.
	/// </summary>
	/// <param name="left">The first <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <param name="right">The second <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <returns><c>true</c> if the left instance is less than or equal to the right instance; otherwise, <c>false</c>.</returns>
	public static bool operator <= (PlatformSupportVersion left, PlatformSupportVersion right)
	{
		return left.CompareTo (right) <= 0;
	}

	/// <summary>
	/// Compares two <see cref="PlatformSupportVersion"/> instances to determine if the left is greater than or equal to the right.
	/// </summary>
	/// <param name="left">The first <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <param name="right">The second <see cref="PlatformSupportVersion"/> to compare.</param>
	/// <returns><c>true</c> if the left instance is greater than or equal to the right instance; otherwise, <c>false</c>.</returns>
	public static bool operator >= (PlatformSupportVersion left, PlatformSupportVersion right)
	{
		return left.CompareTo (right) >= 0;
	}
}
