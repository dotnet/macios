// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Compares two `ImmutableArray&lt;OuterClass&gt;` instances.
/// </summary>
class OuterClassEqualityComparer : ImmutableArrayEqualityComparer<OuterClass, OuterClass> {

	/// <inheritdoc/>
	protected override OuterClass [] Sort (ImmutableArray<OuterClass> array)
		=> array.OrderBy (x => x.Name).ToArray ();
}
