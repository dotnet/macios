// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for class bindings.
/// </summary>
sealed class ClassValidator : BindingValidator {
	/// <summary>
	/// Initializes a new instance of the <see cref="ClassValidator"/> class.
	/// </summary>
	public ClassValidator ()
	{
		// class bindings must be partial
		AddGlobalStrategy (RgenDiagnostics.RBI0001, IsPartial);
	}
}
