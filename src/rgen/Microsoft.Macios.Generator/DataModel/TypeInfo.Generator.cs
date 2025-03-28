// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct TypeInfo {

	/// <summary>
	/// True if the type needs to use a stret call.
	/// </summary>
	public bool NeedsStret { get; init; }

	internal TypeInfo (ITypeSymbol symbol, Compilation compilation) : this (symbol)
	{
		NeedsStret = symbol.NeedsStret (compilation);
	}

}
