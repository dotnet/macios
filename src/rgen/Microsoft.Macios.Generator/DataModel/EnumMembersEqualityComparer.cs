// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Compares two `ImmutableArray&lt;EnumMember&gt;` instances.
/// </summary>
class EnumMembersEqualityComparer : ImmutableArrayEqualityComparer<EnumMember, EnumMember> {

	/// <inheritdoc/>
	protected override EnumMember [] Sort (ImmutableArray<EnumMember> array)
		=> array.OrderBy (x => x.Name).ToArray ();
}
