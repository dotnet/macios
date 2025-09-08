// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Defines a contract for a validator that can validate an object and report diagnostics.
/// </summary>
interface IValidator {
	/// <summary>
	/// Validates the specified data object.
	/// </summary>
	/// <param name="data">The object to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <returns>A dictionary where the key is the name of the invalid field and the value is a list of diagnostics.</returns>
	Dictionary<string, List<Diagnostic>> ValidateAll (object data, RootContext context);
}

/// <summary>
/// A generic validator for a specific type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the object to validate.</typeparam>
partial class Validator<T> : IValidator {
	readonly Dictionary<string, List<IFieldValidationStrategy>> strategies = new ();
	List<IFieldValidationStrategy>? globalStrategies;
	readonly Dictionary<string, IValidator> nestedValidators = new ();
	Func<T, Location?>? getLocation;

	public Validator (Expression<Func<T, Location?>>? locationSelector = null)
	{
		if (locationSelector is not null) {
			getLocation = locationSelector.Compile ();
		}
	}

	/// <summary>
	/// Gets all the diagnostic descriptors that this validator and its nested validators can produce.
	/// </summary>
	public ImmutableArray<DiagnosticDescriptor> Descriptors {
		get {
			// Combine all descriptors from strategies and global strategies
			var allDescriptors = new HashSet<DiagnosticDescriptor> ();
			foreach (var (_, checks) in strategies) {
				foreach (var check in checks) {
					allDescriptors.UnionWith (check.Descriptors);
				}
			}

			if (globalStrategies is not null) {
				foreach (var check in globalStrategies) {
					allDescriptors.UnionWith (check.Descriptors);
				}
			}

			return [.. allDescriptors];
		}
	}

	/// <summary>
	/// Gets the name of the property from a selector expression.
	/// </summary>
	/// <typeparam name="TField">The type of the field.</typeparam>
	/// <param name="selector">The selector expression.</param>
	/// <returns>The name of the property.</returns>
	/// <exception cref="ArgumentException">Thrown when the selector is not a valid property selector.</exception>
	static string GetPropertyName<TField> (Expression<Func<T, TField>> selector)
	{
		return selector.Body switch {
			MemberExpression memberExpr => memberExpr.Member.Name,
			UnaryExpression { Operand: MemberExpression member } => member.Member.Name,
			_ => throw new ArgumentException ("Invalid property selector expression")
		};
	}

	/// <summary>
	/// Gets the value of a field or property from an object using reflection.
	/// </summary>
	/// <param name="data">The object to get the value from.</param>
	/// <param name="fieldName">The name of the field or property.</param>
	/// <returns>The value of the field or property, or null if it doesn't exist.</returns>
	static object? GetFieldValue (T data, string fieldName)
	{
		var prop = typeof (T).GetProperty (fieldName);
		if (prop is not null)
			return prop.GetValue (data);

		var field = typeof (T).GetField (fieldName);
		return field?.GetValue (data);
	}

	/// <summary>
	/// Adds a validation strategy for a specific field.
	/// </summary>
	/// <typeparam name="TField">The type of the field.</typeparam>
	/// <param name="selector">An expression to select the field to validate.</param>
	/// <param name="descriptor">The diagnostic descriptors to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	/// <param name="propertyName">The field name under test. If not present it will be deduced from the selector.</param>
	public void AddStrategy<TField> (
		Expression<Func<T, TField>> selector,
		ImmutableArray<DiagnosticDescriptor> descriptor,
		LambdaFieldValidationStrategy<T, TField>.ValidationFunc validation,
		string? propertyName = null)
	{
		var fieldName = propertyName ?? GetPropertyName (selector);

		if (!strategies.ContainsKey (fieldName))
			strategies [fieldName] = new List<IFieldValidationStrategy> ();

		strategies [fieldName].Add (new LambdaFieldValidationStrategy<T, TField> (descriptor, selector, validation));
	}

	/// <summary>
	/// Adds a validation strategy for a specific field.
	/// </summary>
	/// <typeparam name="TField">The type of the field.</typeparam>
	/// <param name="selector">An expression to select the field to validate.</param>
	/// <param name="descriptor">The diagnostic descriptor to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	/// <param name="propertyName">The field name under test. If not present it will be deduced from the selector.</param>
	public void AddStrategy<TField> (
		Expression<Func<T, TField>> selector,
		DiagnosticDescriptor descriptor,
		LambdaFieldValidationStrategy<T, TField>.ValidationFunc validation,
		string? propertyName = null)
		=> AddStrategy (selector, [descriptor], validation, propertyName);

	/// <summary>
	/// Adds a validation strategy for a specific nullable struct field.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field to validate.</param>
	/// <param name="descriptor">The diagnostic descriptors to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	/// <param name="propertyName">The field name under test. If not present it will be deduced from the selector.</param>
	public void AddStrategy<TField> (
		Expression<Func<T, TField?>> selector,
		ImmutableArray<DiagnosticDescriptor> descriptor,
		LambdaFieldValidationStrategy<T, TField?>.ValidationFunc validation,
		string? propertyName = null) where TField : struct
	{
		var fieldName = propertyName ?? GetPropertyName (selector);

		if (!strategies.ContainsKey (fieldName))
			strategies [fieldName] = new List<IFieldValidationStrategy> ();

		strategies [fieldName].Add (new LambdaFieldValidationStrategy<T, TField?> (descriptor, selector, validation));
	}

	/// <summary>
	/// Adds a validation strategy for a specific nullable struct field.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field to validate.</param>
	/// <param name="descriptor">The diagnostic descriptor to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	/// <param name="propertyName">The field name under test. If not present it will be deduced from the selector.</param>
	public void AddStrategy<TField> (
		Expression<Func<T, TField?>> selector,
		DiagnosticDescriptor descriptor,
		LambdaFieldValidationStrategy<T, TField?>.ValidationFunc validation,
		string? propertyName = null) where TField : struct
		=> AddStrategy (selector, [descriptor], validation, propertyName);

	/// <summary>
	/// Adds a validation strategy for a specific field.
	/// </summary>
	/// <param name="fieldName">The name of the field.</param>
	/// <param name="strategy">The validation strategy.</param>
	void AddStrategy (string fieldName, IFieldValidationStrategy strategy)
	{
		if (!strategies.ContainsKey (fieldName))
			strategies [fieldName] = new List<IFieldValidationStrategy> ();

		strategies [fieldName].Add (strategy);
	}

	/// <summary>
	/// Adds a nested validator for a complex field.
	/// </summary>
	/// <typeparam name="TField">The type of the field.</typeparam>
	/// <param name="selector">An expression to select the field.</param>
	/// <param name="nestedValidator">The validator for the field's type.</param>
	public void AddNestedValidator<TField> (
		Expression<Func<T, TField?>> selector,
		Validator<TField> nestedValidator)
	{
		var fieldName = GetPropertyName (selector);
		nestedValidators [fieldName] = nestedValidator;
	}

	/// <summary>
	/// Adds a nested validator for a nullable struct field.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field.</param>
	/// <param name="nestedValidator">The validator for the struct's type.</param>
	public void AddNestedValidator<TField> (
		Expression<Func<T, TField?>> selector,
		Validator<TField> nestedValidator) where TField : struct
	{
		var fieldName = GetPropertyName (selector);

		nestedValidators [fieldName] = nestedValidator;

		AddStrategy (selector, nestedValidator.Descriptors, NestedValidation);

		bool NestedValidation (TField? data, RootContext context, out ImmutableArray<Diagnostic> diagnostic, Location? location = null)
		{
			diagnostic = [];
			if (data is null)
				return true; // null nested = valid

			var nestedErrors = nestedValidator.ValidateAll (data.Value, context);
			// flatten the diagnostics
			diagnostic = [.. nestedErrors.SelectMany (x => x.Value)];
			return nestedErrors.Count == 0;
		}
	}

	/// <summary>
	/// Adds a validation strategy that applies to the entire object.
	/// </summary>
	/// <param name="descriptor">The diagnostic descriptors to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	public void AddGlobalStrategy (ImmutableArray<DiagnosticDescriptor> descriptor,
		LambdaFieldValidationStrategy<T, T>.ValidationFunc validation)
	{
		globalStrategies ??= new List<IFieldValidationStrategy> ();
		globalStrategies.Add (new LambdaFieldValidationStrategy<T, T> (descriptor, x => x, validation));
	}

	/// <summary>
	/// Adds a validation strategy that applies to the entire object.
	/// </summary>
	/// <param name="descriptor">The diagnostic descriptor to use if validation fails.</param>
	/// <param name="validation">The validation logic.</param>
	public void AddGlobalStrategy (DiagnosticDescriptor descriptor,
		LambdaFieldValidationStrategy<T, T>.ValidationFunc validation)
		=> AddGlobalStrategy ([descriptor], validation);

	/// <summary>
	/// Adds a validation rule to ensure that a group of fields are mutually exclusive.
	/// </summary>
	/// <param name="exactlyOne">If true, exactly one of the fields must be set. If false, at most one can be set.</param>
	/// <param name="selectors">The fields to check for mutual exclusivity.</param>
	public void MutuallyExclusive (bool exactlyOne = false, params Expression<Func<T, object?>> [] selectors)
	{
		if (selectors.Length < 2)
			throw new ArgumentException ("MutuallyExclusive requires at least 2 fields.");

		var compiledSelectors = selectors.Select (s => s.Compile ()).ToArray ();

		AddGlobalStrategy (RBI0016, CheckSelectors);

		bool CheckSelectors (T data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			int setCount = compiledSelectors.Count (sel => {
				var value = sel (data);
				return value is not null;
			});

			var valid = exactlyOne ? setCount == 1 : setCount <= 1;
			if (!valid) {
				var fieldNames = selectors.Select (GetPropertyName).ToList ();
				// we will only return a single diagnostic for the whole group
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0016, // Fields '{0}' must be mutually exclusive. At most one field can be set.
						location: location,
						messageArgs: fieldNames)
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a validation rule that requires a field to be set when certain flags are present in another field.
	/// </summary>
	/// <typeparam name="TField">The type of the field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the field that is conditionally required.</param>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for the field to be required. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the requirement.</param>
	public void RequireWhen<TField, TFlag> (
		Expression<Func<T, TField>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		bool requireAllFlags = false,
		params TFlag [] requiredFlags)
		where TFlag : Enum
	{
		Expression<Func<T, (TField FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		var fieldName = GetPropertyName (selector);

		var strategy = new LambdaFieldValidationStrategy<T, (TField FieldData, TFlag FlagData)> (
			descriptor: [RBI0015],
			selector: tupleSelector,
			validationFunc: CheckFlags);

		AddStrategy (fieldName, strategy);

		bool CheckFlags ((TField FieldData, TFlag FlagData) data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			// we do know that we can only get the anonymous type with two properties
			diagnostics = [];
			var flagValue = data.Item2;

			bool shouldBePresent = requireAllFlags
				? requiredFlags.All (@enum => flagValue.HasFlag (@enum))
				: requiredFlags.Any (@enum => flagValue.HasFlag (@enum));

			var fieldValue = data.Item1;
			var valid = !shouldBePresent || fieldValue is not null;
			if (!valid) {
				var flagNames = requiredFlags.Select (x => x.ToString ()).ToList ();
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0015, // Field '{0}' is required when one of the following flags is set: '{1}'.
						location: location,
						messageArgs: [GetPropertyName (selector), flagNames])
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a conditional validation strategy that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TField">The type of the field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="descriptor">The diagnostic descriptors to use if validation fails.</param>
	/// <param name="validation">The validation logic to execute when flags are present.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	public void AddConditionalStrategy<TField, TFlag> (
		Expression<Func<T, TField>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		ImmutableArray<DiagnosticDescriptor> descriptor,
		LambdaFieldValidationStrategy<T, TField>.ValidationFunc validation,
		bool requireAllFlags = false,
		params TFlag [] requiredFlags)
		where TFlag : Enum
	{
		Expression<Func<T, (TField FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		var fieldName = GetPropertyName (selector);

		var strategy = new LambdaFieldValidationStrategy<T, (TField FieldData, TFlag FlagData)> (
			descriptor: [RBI0015],
			selector: tupleSelector,
			validationFunc: ConditionalValidation);

		AddStrategy (fieldName, strategy);

		bool ConditionalValidation ((TField FieldData, TFlag FlagData) data, RootContext context, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var flagValue = data.FlagData;

			bool shouldValidate = requireAllFlags
				? requiredFlags.All (@enum => flagValue.HasFlag (@enum))
				: requiredFlags.Any (@enum => flagValue.HasFlag (@enum));

			// If flags are not set, validation passes (no validation needed)
			if (!shouldValidate)
				return true;

			// Execute the actual validation logic
			return validation (data.FieldData, context, out diagnostics, location);
		}
	}

	/// <summary>
	/// Adds a conditional validation strategy that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TField">The type of the field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="descriptor">The diagnostic descriptor to use if validation fails.</param>
	/// <param name="validation">The validation logic to execute when flags are present.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	public void AddConditionalStrategy<TField, TFlag> (
		Expression<Func<T, TField>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		DiagnosticDescriptor descriptor,
		LambdaFieldValidationStrategy<T, TField>.ValidationFunc validation,
		bool requireAllFlags = false,
		params TFlag [] requiredFlags)
		where TFlag : Enum
		=> AddConditionalStrategy (selector, flagSelector, [descriptor], validation, requireAllFlags, requiredFlags);

	/// <summary>
	/// Adds a conditional validation strategy that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="descriptor">The diagnostic descriptors to use if validation fails.</param>
	/// <param name="validation">The validation logic to execute when flags are present.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	public void AddConditionalStrategy<TField, TFlag> (
		Expression<Func<T, TField?>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		ImmutableArray<DiagnosticDescriptor> descriptor,
		LambdaFieldValidationStrategy<T, TField?>.ValidationFunc validation,
		bool requireAllFlags = false,
		params TFlag [] requiredFlags)
		where TField : struct
		where TFlag : Enum
	{

		Expression<Func<T, (TField? FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		var fieldName = GetPropertyName (selector);

		var strategy = new LambdaFieldValidationStrategy<T, (TField? FieldData, TFlag FlagData)> (
			descriptor: [RBI0015],
			selector: tupleSelector,
			validationFunc: ConditionalValidation);

		AddStrategy (fieldName, strategy);

		bool ConditionalValidation ((TField? FieldData, TFlag FlagData) data, RootContext context, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var flagValue = data.FlagData;

			bool shouldValidate = requireAllFlags
				? requiredFlags.All (@enum => flagValue.HasFlag (@enum))
				: requiredFlags.Any (@enum => flagValue.HasFlag (@enum));

			// If flags are not set, validation passes (no validation needed)
			if (!shouldValidate)
				return true;

			// Execute the actual validation logic
			return validation (data.FieldData, context, out diagnostics, location);
		}
	}

	/// <summary>
	/// Adds a conditional validation strategy that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="descriptor">The diagnostic descriptor to use if validation fails.</param>
	/// <param name="validation">The validation logic to execute when flags are present.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	public void AddConditionalStrategy<TField, TFlag> (
		Expression<Func<T, TField?>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		DiagnosticDescriptor descriptor,
		LambdaFieldValidationStrategy<T, TField?>.ValidationFunc validation,
		bool requireAllFlags = false,
		params TFlag [] requiredFlags)
		where TField : struct
		where TFlag : Enum
		=> AddConditionalStrategy (selector, flagSelector, [descriptor], validation, requireAllFlags, requiredFlags);

	/// <summary>
	/// Adds a conditional mutually exclusive validation rule that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="exactlyOne">If true, exactly one of the fields must be set when flags are present. If false, at most one can be set.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	/// <param name="selectors">The fields to check for mutual exclusivity.</param>
	public void AddConditionalMutuallyExclusive<TFlag> (
		Expression<Func<T, TFlag>> flagSelector,
		bool exactlyOne = false,
		bool requireAllFlags = false,
		TFlag []? requiredFlags = null,
		params IFieldNullCheck<T> [] selectors)
		where TFlag : Enum
	{
		if (selectors.Length < 2)
			throw new ArgumentException ("AddConditionalMutuallyExclusive requires at least 2 fields.");

		if (requiredFlags is null || requiredFlags.Length == 0)
			throw new ArgumentException ("requiredFlags cannot be null or empty.");

		var flagSelectorCompiled = flagSelector.Compile ();

		AddGlobalStrategy (RBI0016, CheckSelectors);

		bool CheckSelectors (T data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var flagValue = flagSelectorCompiled (data);

			bool shouldValidate = requireAllFlags
				? requiredFlags.All (@enum => flagValue.HasFlag (@enum))
				: requiredFlags.Any (@enum => flagValue.HasFlag (@enum));

			// If flags are not set, validation passes (no validation needed)
			if (!shouldValidate)
				return true;

			int setCount = selectors.Count (sel => !sel.IsNull (data));

			var valid = exactlyOne ? setCount == 1 : setCount <= 1;
			if (!valid) {
				var fieldNames = selectors.Select (s => s.FieldName).ToList ();
				// we will only return a single diagnostic for the whole group
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0016, // Fields '{0}' must be mutually exclusive. At most one field can be set.
						location: location,
						messageArgs: string.Join (", ", fieldNames))
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a conditional mutually exclusive validation rule that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="exactlyOne">If true, exactly one of the fields must be set when flags are present. If false, at most one can be set.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	/// <param name="selectors">The fields to check for mutual exclusivity.</param>
	public void AddConditionalMutuallyExclusive<TFlag> (
		Expression<Func<T, TFlag>> flagSelector,
		bool exactlyOne = false,
		bool requireAllFlags = false,
		TFlag []? requiredFlags = null,
		params Expression<Func<T, object?>> [] selectors)
		where TFlag : Enum
	{
		// build IFieldNullCheck<T> from the selectors
		var fieldChecks = new IFieldNullCheck<T> [selectors.Length];
		for (int i = 0; i < selectors.Length; i++) {
			fieldChecks [i] = new FieldNullCheck<T, object?> (
				selector: selectors [i],
				isDefaultValue: x => x is null,
				fieldName: GetPropertyName (selectors [i]));
		}
		AddConditionalMutuallyExclusive (flagSelector, exactlyOne, requireAllFlags, requiredFlags, fieldChecks);
	}

	/// <summary>
	/// Adds a conditional mutually exclusive validation rule that only executes when specific flags are present in another field.
	/// </summary>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="flagSelector">An expression to select the enum field that contains the flags.</param>
	/// <param name="exactlyOne">If true, exactly one of the fields must be set when flags are present. If false, at most one can be set.</param>
	/// <param name="requireAllFlags">If true, all flags in <paramref name="requiredFlags"/> must be present for validation to execute. If false, any flag is sufficient.</param>
	/// <param name="requiredFlags">The enum flags that trigger the validation.</param>
	/// <param name="selectors">The nullable struct fields to check for mutual exclusivity.</param>
	public void AddConditionalMutuallyExclusive<TFlag> (
		Expression<Func<T, TFlag>> flagSelector,
		bool exactlyOne = false,
		bool requireAllFlags = false,
		TFlag []? requiredFlags = null,
		params Expression<Func<T, ValueType?>> [] selectors)
		where TFlag : Enum
	{
		if (selectors.Length < 2)
			throw new ArgumentException ("AddConditionalMutuallyExclusive requires at least 2 fields.");

		if (requiredFlags is null || requiredFlags.Length == 0)
			throw new ArgumentException ("requiredFlags cannot be null or empty.");

		var compiledSelectors = selectors.Select (s => s.Compile ()).ToArray ();
		var flagSelectorCompiled = flagSelector.Compile ();

		AddGlobalStrategy (RBI0016, CheckSelectors);

		bool CheckSelectors (T data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var flagValue = flagSelectorCompiled (data);

			bool shouldValidate = requireAllFlags
				? requiredFlags.All (@enum => flagValue.HasFlag (@enum))
				: requiredFlags.Any (@enum => flagValue.HasFlag (@enum));

			// If flags are not set, validation passes (no validation needed)
			if (!shouldValidate)
				return true;

			int setCount = compiledSelectors.Count (sel => {
				var value = sel (data);
				return value is not null;
			});

			var valid = exactlyOne ? setCount == 1 : setCount <= 1;
			if (!valid) {
				var fieldNames = selectors.Select (GetPropertyName).ToList ();
				// we will only return a single diagnostic for the whole group
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0016, // Fields '{0}' must be mutually exclusive. At most one field can be set.
						location: location,
						messageArgs: fieldNames)
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a validation rule that restricts a field to only be set when the flag is of a specific type.
	/// </summary>
	/// <typeparam name="TField">The type of the field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field.</param>
	/// <param name="expectedFlagType">The expected flag type that allows the field to be set.</param>
	public void RestrictToFlagType<TField, TFlag> (
		Expression<Func<T, TField?>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		Type expectedFlagType)
		where TFlag : Enum
	{
		Expression<Func<T, (TField? FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		AddStrategy (tupleSelector, RBI0017, CheckFlagType, GetPropertyName (selector));

		bool CheckFlagType ((TField? FieldData, TFlag FlagData) data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var fieldValue = data.FieldData;

			// If field is null, validation passes
			if (fieldValue is null)
				return true;

			// Check if the flag type matches the expected type
			var flagType = typeof (TFlag);
			var valid = flagType == expectedFlagType;

			if (!valid) {
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0017,
						location: location,
						messageArgs: [
							GetPropertyName (selector),
							expectedFlagType.Name,
							flagType.Name
						])
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a validation rule that restricts a nullable struct field to only be set when the flag is of a specific type.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the nullable struct field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field.</param>
	/// <param name="expectedFlagType">The expected flag type that allows the field to be set.</param>
	public void RestrictToFlagType<TField, TFlag> (
		Expression<Func<T, TField?>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		Type expectedFlagType)
		where TField : struct
		where TFlag : Enum
	{

		Expression<Func<T, (TField? FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		AddStrategy (tupleSelector, RBI0017, CheckFlagType, GetPropertyName (selector));

		bool CheckFlagType ((TField? FieldData, TFlag FlagData) data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var fieldValue = data.FieldData;

			// If field is null, validation passes
			if (fieldValue is null)
				return true;

			// Check if the flag type matches the expected type
			var flagType = typeof (TFlag);
			var valid = flagType == expectedFlagType;

			if (!valid) {
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0017,
						location: location,
						messageArgs: [
							GetPropertyName (selector),
							expectedFlagType.Name,
							flagType.Name
						])
				];
			}

			return valid;
		}
	}

	/// <summary>
	/// Adds a validation rule that restricts a non-nullable struct field to only be set when the flag is of a specific type.
	/// Uses a lambda to determine if the struct is in its default state.
	/// </summary>
	/// <typeparam name="TField">The type of the struct field to validate.</typeparam>
	/// <typeparam name="TFlag">The type of the enum flag.</typeparam>
	/// <param name="selector">An expression to select the struct field to validate.</param>
	/// <param name="flagSelector">An expression to select the enum field.</param>
	/// <param name="isDefaultValue">A function that returns true if the struct field is in its default state.</param>
	/// <param name="expectedFlagType">The expected flag type that allows the field to be set.</param>
	public void RestrictToFlagType<TField, TFlag> (
		Expression<Func<T, TField>> selector,
		Expression<Func<T, TFlag>> flagSelector,
		Func<TField, bool> isDefaultValue,
		Type expectedFlagType)
		where TField : struct
		where TFlag : Enum
	{
		Expression<Func<T, (TField FieldData, TFlag FlagData)>> tupleSelector =
			y => new (selector.Compile () (y), flagSelector.Compile () (y));

		AddStrategy (tupleSelector, RBI0017, CheckFlagType, GetPropertyName (selector));

		bool CheckFlagType ((TField FieldData, TFlag FlagData) data, RootContext _, out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
		{
			diagnostics = [];
			var fieldValue = data.FieldData;

			// If field is in default state, validation passes
			if (isDefaultValue (fieldValue))
				return true;

			// Check if the flag type matches the expected type
			var flagType = typeof (TFlag);
			var valid = flagType == expectedFlagType;

			if (!valid) {
				diagnostics = [
					Diagnostic.Create (
						descriptor: RBI0017,
						location: location,
						messageArgs: [
							GetPropertyName (selector),
							expectedFlagType.Name,
							flagType.Name
						])
				];
			}

			return valid;
		}
	}

	/// <inheritdoc />
	public Dictionary<string, List<Diagnostic>> ValidateAll (object data, RootContext rootContext)
		=> data is not T validData ? [] : ValidateAll (validData, rootContext);

	/// <summary>
	/// Validates the given data object against all registered strategies.
	/// </summary>
	/// <param name="data">The data object to validate.</param>
	/// <param name="rootContext">The root context for validation.</param>
	/// <returns>A dictionary of validation errors, where the key is the field name and the value is a list of diagnostics.</returns>
	public Dictionary<string, List<Diagnostic>> ValidateAll (T data, RootContext rootContext)
	{
		// if the location is null BUT we have a func that can retrieve it from the data, use data
		var location = getLocation?.Invoke (data);

		var errors = new Dictionary<string, List<Diagnostic>> ();

		// Own field strategies
		foreach (var kvp in strategies) {
			foreach (var strategy in kvp.Value) {
				if (strategy.IsValid (data, rootContext, out var diagnostics, location))
					continue;

				if (!errors.ContainsKey (kvp.Key))
					errors [kvp.Key] = [];

				errors [kvp.Key].AddRange (diagnostics);
			}
		}

		// Global strategies
		if (globalStrategies is not null) {
			foreach (var strategy in globalStrategies) {
				if (!strategy.IsValid (data, rootContext, out var diagnostics, location)) {
					if (!errors.ContainsKey (string.Empty))
						errors [string.Empty] = [];

					errors [string.Empty].AddRange (diagnostics);
				}
			}
		}

		// Nested validators recursively
		foreach (var nestedEntry in nestedValidators) {
			var fieldName = nestedEntry.Key;
			var nestedValidatorObj = nestedEntry.Value;

			var nestedValue = GetFieldValue (data, fieldName);

			if (nestedValue is null)
				continue;

			var nestedErrors = nestedValidatorObj.ValidateAll (nestedValue, rootContext);
			foreach (var ne in nestedErrors) {
				var compositeKey = string.IsNullOrEmpty (ne.Key)
					? fieldName
					: $"{fieldName}.{ne.Key}";

				if (!errors.ContainsKey (compositeKey))
					errors [compositeKey] = new ();

				errors [compositeKey].AddRange (ne.Value);
			}
		}

		return errors;
	}
}
