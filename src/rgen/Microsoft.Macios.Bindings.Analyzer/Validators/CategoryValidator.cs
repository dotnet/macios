// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for category bindings.
/// </summary>
sealed class CategoryValidator : BindingValidator {

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryValidator"/> class.
	/// </summary>
	public CategoryValidator ()
	{

		// all bindings must be partial
		AddGlobalStrategy (RgenDiagnostics.RBI0001, IsPartial);
		// categories must be static
		AddGlobalStrategy (RgenDiagnostics.RBI0004, IsStatic);
	}
}
