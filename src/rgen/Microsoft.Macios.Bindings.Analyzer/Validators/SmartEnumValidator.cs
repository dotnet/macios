// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Bindings.Analyzer.Extensions;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for smart enum bindings.
/// </summary>
sealed class SmartEnumValidator : BindingValidator {

	/// <summary>
	/// Validates that enum fields have unique backing field selectors.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all field selectors are unique; otherwise, <c>false</c>.</returns>
	internal static bool FieldsAreUnique (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!IsValidBindingType (binding, BindingType.SmartEnum, out diagnostics, location)) {
			return false;
		}
		// build a dict that will contain the field names and the enum values, this way we can check if a field was 
		// defined more than once
		var fieldNames = new Dictionary<string, List<EnumMember>> ();
		foreach (var enumMember in binding.EnumMembers) {
			if (enumMember.Selector is null) {
				// the current strategy does not take care of missing selectors, so we skip them
				continue;
			}

			if (fieldNames.TryGetValue (enumMember.Selector, out var previousValues)) {
				previousValues.Add (enumMember);
			} else {
				fieldNames.Add (enumMember.Selector, [enumMember]);
			}
		}
		// get all the fileds that have more than one value
		var duplicates = fieldNames.Where (x => x.Value.Count > 1).ToList ();
		if (duplicates.Count == 0) {
			// no duplicates found
			return true;
		}
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		var bindingNamespace = string.Join ('.', binding.Namespace);
		// build the diagnostics for the duplicates
		foreach (var (field, enums) in duplicates) {
			for (var index = 1; index < enums.Count; index++) { // skip the first one, it is the original
				builder.Add (Diagnostic.Create (
					RBI0009, // The backing field '{0}' for the enum value '{1}' is already in use for the enum value '{2}'
					location: enums [index].Location,
					$"{bindingNamespace}.{field}",
					$"{binding.Name}.{enums [index].Name}",
					$"{binding.Name}.{enums [0].Name}"));
			}
		}
		diagnostics = builder.ToImmutable ();
		return false;
	}

	/// <summary>
	/// Validates that enum member field names are valid C# identifiers.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all field identifiers are valid; otherwise, <c>false</c>.</returns>
	internal static bool FieldsHaveValidIdentifier (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!IsValidBindingType (binding, BindingType.SmartEnum, out diagnostics, location)) {
			return false;
		}

		// loop over all the enum values and check if the field names are valid identifiers
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var enumMember in binding.EnumMembers) {
			if (enumMember.FieldInfo is null)
				continue;
			if (!enumMember.FieldInfo.Value.FieldData.SymbolName.IsValidIdentifier ()) {
				builder.Add (Diagnostic.Create (
					RBI0010, // The enum value '{0}' has an invalid identifier '{1}'. Identifiers must be valid C# identifiers.
					enumMember.Location,
					$"{binding.FullyQualifiedSymbol}.{enumMember.Name}",
					enumMember.FieldInfo.Value.FieldData.SymbolName));
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that enum members have appropriate library values based on whether they belong to Apple frameworks.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all library values are valid; otherwise, <c>false</c>.</returns>
	internal static bool ValidateLibraryPathValue (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!IsValidBindingType (binding, BindingType.SmartEnum, out diagnostics, location)) {
			return false;
		}

		var platformName = context.Compilation.GetCurrentPlatform ();
		if (platformName == PlatformName.None) {
			// we could not identify the platform, we have a bug 
			diagnostics = [Diagnostic.Create (
				RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				location: location,
				messageArgs: binding.FullyQualifiedSymbol
			)];
			return false;
		}

		var appleFrameworks = Frameworks.GetFrameworks (platformName.ToApplePlatform (), false);
		if (appleFrameworks is null) {
			// we could not get the frameworks, we have a bug
			// we could not identify the platform, we have a bug 
			diagnostics = [Diagnostic.Create (
				RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				location: location,
				messageArgs: binding.FullyQualifiedSymbol
			)];
			return false;
		}

		var bindingNamespace = string.Join ('.', binding.Namespace);
		var isAppleFramework = appleFrameworks.Find (bindingNamespace) is not null;
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		// loop over all the enum values and check if the library name is set depending if we are
		// dealing with an apple framework or not
		foreach (var enumMember in binding.EnumMembers) {
			if (enumMember.FieldInfo is null)
				continue;
			if (!isAppleFramework) {
				// If the Field attribute is not from a known apple library, the library name is set
				if (string.IsNullOrWhiteSpace (enumMember.FieldInfo.Value.LibraryPath)) {
					builder.Add (Diagnostic.Create (
						RBI0011, // Non Apple framework bindings must provide a library name.
						enumMember.Location,
						$"{binding.FullyQualifiedSymbol}.{enumMember.Name}"));
				}
			} else {
				// If the Field attribute is from a known apple library, the lib should be null
				if (enumMember.FieldInfo.Value.LibraryPath is not null) {
					builder.Add (Diagnostic.Create (
						RBI0012, // Do not provide the LibraryName for known Apple frameworks.
						enumMember.Location,
						$"{binding.FullyQualifiedSymbol}.{enumMember.Name}"));
				}
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that the enum has at least one member.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the enum has at least one member; otherwise, <c>false</c>.</returns>
	internal static bool AtLeastOneMember (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!IsValidBindingType (binding, BindingType.SmartEnum, out diagnostics, location)) {
			return false;
		}

		if (binding.EnumMembers.Length != 0)
			return true;
		// return an error stating that at least one enum member is required
		diagnostics = [Diagnostic.Create (
			RBI0008, // The enum '{0}' must have at least one member tagged with a Field<EnumValue> attribute 
			location: binding.Location,
			binding.FullyQualifiedSymbol
		)];
		return false;
	}

	/// <summary>
	/// Validates that the enum and its members are available on the current platform.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the data is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the enum and its members are valid for the current platform; otherwise, <c>false</c>.</returns>
	internal static bool IsValidPlatform (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		// we have to do several checks for the IsValidPlatform method:
		// 1. Check if the binding is a smart enum
		// 2. Check if the current platform is valid
		// 3. Check if the enum members are valid for the current platform
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!IsValidBindingType (binding, BindingType.SmartEnum, out diagnostics, location)) {
			return false;
		}

		if (!SupportedPlatformStrategies.IsValidPlatform (binding.SymbolAvailability,
				context, out diagnostics, binding.FullyQualifiedSymbol, binding.Location)) {
			return false;
		}

		// current smart enum is valid, loop to check its values
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var enumMember in binding.EnumMembers) {
			if (!SupportedPlatformStrategies.IsValidPlatform (
					enumMember.SymbolAvailability, context,
					out var valueDiagnostics,
					$"{binding.FullyQualifiedSymbol}.{enumMember.Name}",
					enumMember.Location)) {
				builder.AddRange (valueDiagnostics);
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SmartEnumValidator"/> class.
	/// </summary>
	public SmartEnumValidator ()
	{
		AddGlobalStrategy ([RBI0000, RBI0008], AtLeastOneMember);
		AddGlobalStrategy ([RBI0000, RBI0009], FieldsAreUnique);
		AddGlobalStrategy ([RBI0000, RBI0010], FieldsHaveValidIdentifier);
		AddGlobalStrategy ([RBI0000, RBI0011, RBI0012], ValidateLibraryPathValue);
		AddGlobalStrategy ([RBI0027], IsValidPlatform);
	}

}
