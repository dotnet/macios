// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using ObjCBindings;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for category bindings.
/// </summary>
sealed class CategoryValidator : BindingValidator {

	/// <summary>
	/// Validates that all methods in a category binding are properly declared as partial extension methods
	/// with the correct first parameter type matching the category's extended type.
	/// </summary>
	/// <param name="binding">The category binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any invalid methods; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all methods are valid; otherwise, <c>false</c>.</returns>
	bool ValidMethods (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		// we need to make sure that all the methods of the category are:
		// 1. partial
		// 2. static
		// 3. Have the correct first parameter type (the type that the category is extending)
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var extensionMethod in binding.Methods) {
			if (!ModifiersStrategies.IsPartial (extensionMethod.Modifiers, RBI0042,
					out var partialDiagnostics, extensionMethod.Location, extensionMethod.Name)) {
				builder.AddRange (partialDiagnostics);
			}
			// check that it is an extension method
			var bindingData = (BindingTypeData<Category>) binding.BindingInfo;
			if (extensionMethod.IsExtension) {
				// ensure that the first parameter type matches the type that the category is extending
				if (extensionMethod.Parameters [0].Type.FullyQualifiedName !=
					bindingData.CategoryType.FullyQualifiedName) {
					// we do not allow to mix types in the extension methods in a category
					builder.Add (Diagnostic.Create (
						RBI0044, // Extension methods in a category must have the first parameter type match the category's extended type.
						extensionMethod.Location,
						extensionMethod.Name,
						binding.Name,
						bindingData.CategoryType.FullyQualifiedName,
						extensionMethod.Parameters [0].Type.FullyQualifiedName));
				}
			} else {
				// method should be an extension
				builder.Add (Diagnostic.Create (
					RBI0043, // Extension methods in a category must be declared as extension methods.
					extensionMethod.Location,
					extensionMethod.Name,
					binding.Name,
					bindingData.CategoryType.FullyQualifiedName
					));
			}
		}

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that categories do not contain unsupported members such as events, constructors, or properties.
	/// </summary>
	/// <param name="binding">The category binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any unsupported members; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if no unsupported members are present; otherwise, <c>false</c>.</returns>
	bool ValidMembers (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();

		if (binding.Events.Length > 0) {
			builder.Add (Diagnostic.Create (
				RBI0045, // Categories cannot contain events.
				location,
				binding.Name,
				binding.Events.Length));
		}

		if (binding.Constructors.Length > 0) {
			builder.Add (Diagnostic.Create (
				RBI0046, // Categories cannot contain constructors.
				location,
				binding.Name,
				binding.Constructors.Length));
		}

		if (binding.Properties.Length > 0) {
			builder.Add (Diagnostic.Create (
				RBI0047, // Categories cannot contain properties.
				location,
				binding.Name,
				binding.Properties.Length));
		}

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates the Export attribute data for a category binding, ensuring proper naming conventions,
	/// type constraints, and constructor visibility settings.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any validation failures; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the Export attribute data is valid; otherwise, <c>false</c>.</returns>
	bool ValidateExportData (Binding binding, RootContext context, out ImmutableArray<Diagnostic> diagnostics, Location? location)
	{
		var data = (BindingTypeData<Category>) binding.BindingInfo;
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();

		// validate the name if specified
		if (data.Name is not null) {
			// validate that we do not have any whitespaces in the name
			if (string.IsNullOrWhiteSpace (data.Name) || data.Name.Contains (' ')) {
				// the name is not valid
				builder.Add (Diagnostic.Create (
					RBI0048, // Category '{0}' name '{1}' is empty an empty string or has white spaces
					location,
					binding.Name,
					data.Name));
			}
		}
		// the category types must be a INativeObject or a NSObject
		if (!data.CategoryType.IsINativeObject) {
			// the type is not valid
			builder.Add (Diagnostic.Create (
				RBI0049, // Category '{0}' type '{1}' does not implement INativeObject
				location,
				binding.Name,
				data.CategoryType.FullyQualifiedName));
		}
		// the default ctor visibility must be the default value, else throw a warning
		if (data.DefaultCtorVisibility != MethodAttributes.Public) {
			// warning for the user
			builder.Add (Diagnostic.Create (
				RBI0050, // Category '{0}' has DefaultCtorVisibility set to '{1}' but it will be ignored
				location,
				binding.Name,
				data.DefaultCtorVisibility.ToString ()));
		}

		if (data.ErrorDomain is not null) {
			// warning for the user
			builder.Add (Diagnostic.Create (
				RBI0051, // Category '{0}' has set ErrorDomain to '{1}' but it will be ignored
				location,
				binding.Name,
				data.ErrorDomain));
		}

		// intptr ctor visibility must be private scope since it will be ignored
		if (data.IntPtrCtorVisibility != MethodAttributes.PrivateScope) {
			// warning for the user
			builder.Add (Diagnostic.Create (
				RBI0052, // The IntPtr constructor visibility for a category must be PrivateScope.
				location,
				binding.Name,
				data.IntPtrCtorVisibility.ToString ()));
		}

		if (data.ModelName is not null) {
			// warning for the user
			builder.Add (Diagnostic.Create (
				RBI0053, // Category '{0}' has set ModelName to '{1}' but it will be ignored
				location,
				binding.Name,
				data.ModelName));
		}

		// string ctor visibility must be private scope since it will be ignored
		if (data.StringCtorVisibility != MethodAttributes.PrivateScope) {
			// warning for the user
			builder.Add (Diagnostic.Create (
				RBI0054, // Category '{0}' has set StringCtorVisibility to '{1}' but it will be ignored
				location,
				binding.Name,
				data.StringCtorVisibility.ToString ()));
		}

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryValidator"/> class.
	/// </summary>
	public CategoryValidator ()
	{
		// all bindings must be partial
		AddGlobalStrategy (RBI0001, IsPartial);
		// categories must be static
		AddGlobalStrategy (RBI0004, IsStatic);
		// validate the export attr of the category
		AddGlobalStrategy (
			descriptor: [RBI0048, RBI0049, RBI0050, RBI0051, RBI0052, RBI0053, RBI0054],
			validation: ValidateExportData);
		// validate all methods in the category binding
		AddGlobalStrategy ([RBI0042, RBI0043, RBI0044], ValidMethods);
		// make sure that we do not have constructors, properties, fields or events
		AddGlobalStrategy ([RBI0045, RBI0046, RBI0047], ValidMembers);
	}

}
