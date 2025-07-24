// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Compares two `ImmutableArray&lt;SyntaxToken&gt;` instances representing modifiers.
/// </summary>
class ModifiersEqualityComparer : ImmutableArrayEqualityComparer<SyntaxToken, string> {

	/// <inheritdoc/>
	protected override string [] Sort (ImmutableArray<SyntaxToken> array)
		=> array.Select (t => t.Text).Order ().ToArray ();
}
