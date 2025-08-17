// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;
using ExportMethod = ExportData<Method>;

/// <summary>
/// Validates <see cref="ExportData{T}"/> for methods.
/// </summary>
class ExportMethodAttributeValidator : Validator<ExportMethod> {

	/// <summary>
	/// Diagnostic descriptor for when a named parameter is used with an incorrect flag.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0020 = new (
		"RBI0020",
		new LocalizableResourceString (nameof (Resources.RBI0020Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0020MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0020Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when an invalid combination of flags is used.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0021 = new (
		"RBI0021",
		new LocalizableResourceString (nameof (Resources.RBI0021Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0021MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0021Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a method export is missing a selector.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0022 = new (
		"RBI0022",
		new LocalizableResourceString (nameof (Resources.RBI0022Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0022MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0022Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a method export selector contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0023 = new (
		"RBI0023",
		new LocalizableResourceString (nameof (Resources.RBI0023Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0023MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0023Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when an async method name contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0026 = new (
		"RBI0026",
		new LocalizableResourceString (nameof (Resources.RBI0026Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0026MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0026Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Validates that a string field is only used when a specific flag is present.
	/// </summary>
	/// <param name="fieldName">The name of the field being validated.</param>
	/// <param name="value">The value of the field.</param>
	/// <param name="flags">The flags present on the export.</param>
	/// <param name="expectedFlag">The flag required for the field to be valid.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool StringFieldIsAllowed (string fieldName, string? value, Method flags, Method expectedFlag, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = [];
		if (string.IsNullOrEmpty (value)) {
			return true; // if is null or empty, it's allowed
		}

		if (!flags.HasFlag (expectedFlag)) {
			// we are missing the async flag, so ResultTypeName is not allowed
			diagnostics = [
				Diagnostic.Create (
					descriptor: RBI0020, // The '{0}' named parameter is only allowed with the {1} flag.
					location: location,
					fieldName,
					$"ObjCBindings.Method.{nameof (expectedFlag)}")
			];
			return false;
		}
		return true;
	}

	/// <summary>
	/// Validates that a TypeInfo field is only used when a specific flag is present.
	/// </summary>
	/// <param name="fieldName">The name of the field being validated.</param>
	/// <param name="value">The value of the field.</param>
	/// <param name="flags">The flags present on the export.</param>
	/// <param name="expectedFlag">The flag required for the field to be valid.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool TypeInfoFieldIsAllowed (string fieldName, TypeInfo value, Method flags, Method expectedFlag, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = [];
		if (value.IsNullOrDefault) {
			return true; // if is null or empty, it's allowed
		}

		if (!flags.HasFlag (expectedFlag)) {
			// we are missing the async flag, so ResultTypeName is not allowed
			diagnostics = [
				Diagnostic.Create (
					descriptor: RBI0020, // The '{0}' named parameter is only allowed with the {1} flag.
					location: location,
					fieldName,
					$"ObjCBindings.Method.{nameof (expectedFlag)}")
			];
			return false;
		}
		return true;
	}

	/// <summary>
	/// Validates that the ResultTypeName field is only used with the Async flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool ResultTypeNameIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringFieldIsAllowed (
			fieldName: nameof (exportData.ResultTypeName),
			value: exportData.ResultTypeName,
			flags: exportData.Flags,
			expectedFlag: Method.Async,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the ResultType field is only used with the Async flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool ResultTypeIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> TypeInfoFieldIsAllowed (
			fieldName: nameof (exportData.ResultType),
			value: exportData.ResultType,
			flags: exportData.Flags,
			expectedFlag: Method.Async,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the MethodName field is only used with the Async flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool MethodNameIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringFieldIsAllowed (
			fieldName: nameof (exportData.MethodName),
			value: exportData.MethodName,
			flags: exportData.Flags,
			expectedFlag: Method.Async,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the PostNonResultSnippet field is only used with the Async flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool PostNonResultSnippetIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringFieldIsAllowed (
			fieldName: nameof (exportData.PostNonResultSnippet),
			value: exportData.PostNonResultSnippet,
			flags: exportData.Flags,
			expectedFlag: Method.Async,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the EventArgsType field is only used with the Event flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool EventArgsTypeIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> TypeInfoFieldIsAllowed (
			fieldName: nameof (exportData.EventArgsType),
			value: exportData.EventArgsType,
			flags: exportData.Flags,
			expectedFlag: Method.Event,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the EventArgsTypeName field is only used with the Event flag.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool EventArgsTypeNameIsAllowed (ExportMethod exportData, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringFieldIsAllowed (
			fieldName: nameof (exportData.EventArgsTypeName),
			value: exportData.EventArgsTypeName,
			flags: exportData.Flags,
			expectedFlag: Method.Event,
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that the combination of flags is allowed.
	/// </summary>
	/// <param name="exportData">The export data to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool FlagsAreValid (ExportMethod exportData,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = [];
		// the only combination of flags that is disallowed is Async and Event
		if (exportData.Flags.HasFlag (Method.Event) && exportData.Flags.HasFlag (Method.Async)) {
			diagnostics = [
				Diagnostic.Create (
					descriptor: RBI0021, // The combination of flags {0} is not allowed.
					location: location,
					$"({string.Join (" | ", nameof (Method.Async), nameof (Method.Event))})")
			];
			return false;
		}
		return true;
	}

	/// <summary>
	/// Validates that the selector is not null.
	/// </summary>
	/// <param name="selector">The selector to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool SelectorIsNotNull (string? selector, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringStrategies.IsNotNull (
			selector: selector,
			descriptor: RBI0022, // A export property must have a selector defined
			diagnostics: out diagnostics,
			location: location);

	/// <summary>
	/// Validates that the selector does not contain any whitespace.
	/// </summary>
	/// <param name="selector">The selector to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool SelectorHasNoWhitespace (string? selector, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringStrategies.HasNoWhitespace (
			stringValue: selector,
			descriptor: RBI0023, // A export property selector must not contain any whitespace.
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that an async method name does not contain any whitespace.
	/// </summary>
	/// <param name="selector">The method name to validate.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the data is valid; otherwise, <c>false</c>.</returns>
	internal static bool AsyncMethodNameHasNoWhitespace (string? selector, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringStrategies.HasNoWhitespace (
			stringValue: selector,
			descriptor: RBI0026, // An async method name must not contain any whitespace.
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Initializes a new instance of the <see cref="ExportMethodAttributeValidator"/> class.
	/// </summary>
	public ExportMethodAttributeValidator ()
	{
		// validate the flags values
		AddGlobalStrategy (RBI0021, FlagsAreValid);

		// validate the selector
		AddStrategy (d => d.Selector, RBI0022, SelectorIsNotNull);
		AddStrategy (d => d.Selector, RBI0023, SelectorHasNoWhitespace);

		// prefix and suffix cannot have whitespaces
		AddStrategy (
			selector: d => d.NativePrefix,
			descriptor: StringStrategies.RBI0024,
			validation: (string? data, out ImmutableArray<Diagnostic> diagnostics, Location? location)
				=> StringStrategies.NativeNameHasNoWhitespace (
					data,
					nameof (ExportMethod.NativePrefix),
					out diagnostics,
					location)
		);

		AddStrategy (
			selector: d => d.NativeSuffix,
			descriptor: StringStrategies.RBI0024,
			validation: (string? data, out ImmutableArray<Diagnostic> diagnostics, Location? location)
				=> StringStrategies.NativeNameHasNoWhitespace (
					data,
					nameof (ExportMethod.NativeSuffix),
					out diagnostics,
					location)
		);

		// if async is set, either ResultType or ResultTypeName must be set
		// but not both
		AddConditionalMutuallyExclusive (x => x.Flags,
			exactlyOne: false, // we allow both to be null
			requireAllFlags: false,
			requiredFlags: [ObjCBindings.Method.Async],
			new FieldNullCheck<ExportMethod, TypeInfo> (data => data.ResultType, data => data.IsNullOrDefault),
			new FieldNullCheck<ExportMethod, string?> (data => data.ResultTypeName, string.IsNullOrWhiteSpace)
			);

		// ensure that the asycn fields are only used with methods with the async flag
		AddGlobalStrategy (RBI0020, ResultTypeNameIsAllowed);
		AddGlobalStrategy (RBI0020, ResultTypeIsAllowed);
		AddGlobalStrategy (RBI0020, MethodNameIsAllowed);
		AddGlobalStrategy (RBI0020, PostNonResultSnippetIsAllowed);

		// ensure that of the async fields, we do not have spaces in the name
		AddStrategy (
			selector: d => d.ResultTypeName,
			descriptor: StringStrategies.RBI0025,
			validation: (string? data, out ImmutableArray<Diagnostic> diagnostics, Location? location)
				=> StringStrategies.TypeNameHasNoWhitespace (
					data,
					out diagnostics,
					location,
					nameof (ExportMethod.ResultTypeName))
		);
		AddStrategy (d => d.MethodName, RBI0026, AsyncMethodNameHasNoWhitespace);

		// Strong delegate fields are only allowed in properties
		RestrictToFlagType (
			d => d.StrongDelegateType,
			d => d.Flags,
			d => d.IsNullOrDefault,
			typeof (Property)
		);

		RestrictToFlagType (
			d => d.StrongDelegateName,
			d => d.Flags,
			typeof (Property)
		);

		// only with strong dictionary keys
		RestrictToFlagType (
			d => d.StrongDictionaryKeyClass,
			d => d.Flags,
			d => d.IsNullOrDefault,
			typeof (StrongDictionaryKeys)
		);

		// ensure that the event fields are used with methods with the event flag
		AddGlobalStrategy (RBI0020, EventArgsTypeIsAllowed);
		AddGlobalStrategy (RBI0020, EventArgsTypeNameIsAllowed);

		// ensure that of the event fields, we do not have spaces in the name
		AddStrategy (
			selector: d => d.EventArgsTypeName,
			descriptor: StringStrategies.RBI0025,
			validation: (string? data, out ImmutableArray<Diagnostic> diagnostics, Location? location)
				=> StringStrategies.TypeNameHasNoWhitespace (
					data,
					out diagnostics,
					location,
					nameof (ExportMethod.EventArgsTypeName))
		);
	}
}
