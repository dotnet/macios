// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests;

/// <summary>
/// Custom equality comparer for <see cref="TypeInfo"/> used in tests.
/// Performs comparison based on the fully qualified names of types rather than reference equality.
/// </summary>
class TypeInfoCustomEqualityComparer : IEqualityComparer<TypeInfo?> {

	/// <summary>
	/// Determines whether the specified <see cref="TypeInfo"/> objects are equal.
	/// </summary>
	/// <param name="x">The first <see cref="TypeInfo"/> to compare.</param>
	/// <param name="y">The second <see cref="TypeInfo"/> to compare.</param>
	/// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
	public bool Equals (TypeInfo? x, TypeInfo? y)
	{
		if (x is null)
			return y is null;
		if (y is null)
			return false;
		if (x.Value.SpecialType == SpecialType.System_Void)
			return y.Value.SpecialType == SpecialType.System_Void;

		return x?.FullyQualifiedName == y?.FullyQualifiedName;
	}

	/// <summary>
	/// Returns a hash code for the specified <see cref="TypeInfo"/>.
	/// </summary>
	/// <param name="obj">The <see cref="TypeInfo"/> for which a hash code is to be returned.</param>
	/// <returns>A hash code for the specified object.</returns>
	public int GetHashCode ([DisallowNull] TypeInfo? obj)
	{
		if (obj.Value.SpecialType == SpecialType.System_Void)
			return SpecialType.System_Void.GetHashCode ();

		return obj.Value.FullyQualifiedName?.GetHashCode () ?? 0;
	}
}
