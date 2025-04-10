// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Transformer.Attributes;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct DelegateParameter {

	/// <summary>
	/// Returns the forced type data if present in the binding.
	/// </summary>
	public ForcedTypeData? ForcedType { get; init; }

	public static bool TryCreate (IParameterSymbol symbol,
		[NotNullWhen (true)] out DelegateParameter? parameter)
	{
		parameter = new (symbol.Ordinal, new (symbol.Type), symbol.Name) {
			IsOptional = symbol.IsOptional,
			IsParams = symbol.IsParams,
			IsThis = symbol.IsThis,
			ReferenceKind = symbol.RefKind.ToReferenceKind (),
		};
		return true;
	}
}
