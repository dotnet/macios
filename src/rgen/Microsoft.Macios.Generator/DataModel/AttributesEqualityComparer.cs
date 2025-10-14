// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Macios.Generator.DataModel;

class AttributeComparer : IComparer<AttributeCodeChange> {
	public int Compare (AttributeCodeChange x, AttributeCodeChange y)
	{
		// return the order based on the following
		// 1. Attribute name
		// 2. Attribute param count
		// 3. Attribute values
		var nameComparison = String.Compare (x.Name, y.Name, StringComparison.Ordinal);
		if (nameComparison != 0)
			return nameComparison;
		var lengthComparison = x.Arguments.Length.CompareTo (y.Arguments.Length);
		if (lengthComparison != 0)
			return lengthComparison;
		// argument order is important, we do know that we already have the same length, loop and return if diff
		for (int index = 0; index < x.Arguments.Length; index++) {
			var xArgument = x.Arguments [index];
			var yArgument = y.Arguments [index];
			var compare = String.Compare (xArgument, yArgument, StringComparison.Ordinal);
			if (compare != 0)
				return compare;
		}

		return 0;
	}
}

/// <summary>
/// Compares two `ImmutableArray&lt;AttributeCodeChange&gt;` instances.
/// </summary>
class AttributesEqualityComparer : ImmutableArrayEqualityComparer<AttributeCodeChange, AttributeCodeChange> {
	readonly AttributeComparer comparer = new ();

	/// <inheritdoc/>
	protected override AttributeCodeChange [] Sort (ImmutableArray<AttributeCodeChange> array)
		=> array.Sort (comparer).ToArray ();
}
