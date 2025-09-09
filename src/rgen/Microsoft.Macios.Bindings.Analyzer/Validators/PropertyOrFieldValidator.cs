// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator that validates both properties and fields using the appropriate inner validator based on the property type.
/// </summary>
class PropertyOrFieldValidator : Validator<Property> {

	readonly FieldValidator fieldValidator = new ();
	readonly PropertyValidator propertyValidator = new ();

	/// <summary>
	/// Validates a property or field by delegating to the appropriate validator based on whether it's a field or property.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context containing compilation information.</param>
	/// <param name="diagnostics">The collection of diagnostics produced during validation.</param>
	/// <param name="location">The optional location for diagnostic reporting.</param>
	/// <returns>True if validation passes, false otherwise.</returns>
	bool ValidatePropertyOrField (Property property, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		Dictionary<string, List<Diagnostic>> errors = property.IsField
			? fieldValidator.ValidateAll (property, context)
			: propertyValidator.ValidateAll (property, context);
		if (errors.Count == 0)
			return true;
		// select all the errors from the inner validator
		diagnostics = [.. errors.SelectMany (x => x.Value)];
		return false;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PropertyOrFieldValidator"/> class.
	/// Sets up validation strategies that delegate to field or property validators based on the property type.
	/// </summary>
	public PropertyOrFieldValidator () : base (p => p.Location)
	{
		// add a global strategy based on the type of property (IsField or IsProperty) will call the appropriate strategy
		var errors = fieldValidator.Descriptors.AddRange (propertyValidator.Descriptors);
		AddGlobalStrategy (errors, ValidatePropertyOrField);
	}
}
