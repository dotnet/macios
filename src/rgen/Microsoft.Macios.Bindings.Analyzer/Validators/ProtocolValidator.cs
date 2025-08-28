// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for protocol bindings.
/// </summary>
sealed class ProtocolValidator : BindingValidator {

	/// <summary>
	/// Initializes a new instance of the <see cref="ProtocolValidator"/> class.
	/// </summary>
	public ProtocolValidator ()
	{
		AddGlobalStrategy (RgenDiagnostics.RBI0001, IsPartial);
	}
}
