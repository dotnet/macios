// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Abstract base class for comparing `ImmutableArray` instances.
/// </summary>
/// <typeparam name="T">The type of the elements in the array.</typeparam>
/// <typeparam name="TR">The type of the elements in the sorted array.</typeparam>
abstract class ImmutableArrayEqualityComparer<T, TR> : EqualityComparer<ImmutableArray<T>>
	where T : IEquatable<T>
	where TR : IEquatable<TR> {

	/// <summary>
	/// Sorts the provided array.
	/// </summary>
	/// <param name="array">The array to sort.</param>
	/// <returns>A sorted array.</returns>
	protected abstract TR [] Sort (ImmutableArray<T> array);

	/// <inheritdoc/>
	public override bool Equals (ImmutableArray<T> x, ImmutableArray<T> y)
	{
		if (x.Length != y.Length)
			return false;
		var xOrdered = Sort (x);
		var yOrdered = Sort (y);
		for (int i = 0; i < x.Length; i++) {
			if (!xOrdered [i].Equals (yOrdered [i]))
				return false;
		}
		return true;
	}

	/// <inheritdoc/>
	public override int GetHashCode (ImmutableArray<T> obj)
	{
		var hash = new HashCode ();
		foreach (var change in obj) {
			hash.Add (change.GetHashCode ());
		}
		return hash.ToHashCode ();
	}
}
