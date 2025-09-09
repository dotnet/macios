// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for strong dictionary bindings.
/// </summary>
sealed class StrongDictionaryValidator : BindingValidator {
	/// <summary>
	/// Initializes a new instance of the <see cref="StrongDictionaryValidator"/> class.
	/// </summary>
	public StrongDictionaryValidator ()
	{
		AddGlobalStrategy (RgenDiagnostics.RBI0001, IsPartial);
	}
}
