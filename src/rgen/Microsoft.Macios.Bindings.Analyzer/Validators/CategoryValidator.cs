// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator;
using static Microsoft.Macios.Generator.RgenDiagnostics;

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
		AddGlobalStrategy (RBI0001, IsPartial);
		// categories must be static
		AddGlobalStrategy (RBI0004, IsStatic);
	}
}
