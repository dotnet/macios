// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;
using PropertyFlag = ObjCBindings.Property;

/// <summary>
/// Validator for field properties in bindings.
/// </summary>
class FieldValidator : Validator<Property> {

	/// <summary>
	/// Validates that the selector is not null or empty.
	/// </summary>
	/// <param name="selector">The selector to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the selector is null or empty; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the selector is not null or empty; otherwise, <c>false</c>.</returns>
	internal static bool SelectorIsNotNull (string? selector, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringStrategies.IsNotNull (
			selector: selector,
			descriptor: RBI0018, // A export property must have a selector defined
			diagnostics: out diagnostics,
			location: location);

	/// <summary>
	/// Validates that the selector does not contain any whitespace.
	/// </summary>
	/// <param name="selector">The selector to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the selector contains whitespace; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the selector does not contain whitespace; otherwise, <c>false</c>.</returns>
	internal static bool SelectorHasNoWhitespace (string? selector, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> StringStrategies.HasNoWhitespace (
			stringValue: selector,
			descriptor: RBI0019, // A export property selector must not contain any whitespace.
			diagnostics: out diagnostics,
			location: location
		);

	/// <summary>
	/// Validates that a field property has the partial modifier.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property has the partial modifier; otherwise, <c>false</c>.</returns>
	internal static bool IsPartial (Property property, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsPartial (property.Modifiers, RBI0004, out diagnostics, location, property.Name);

	/// <summary>
	/// Validates that a field property has the static modifier.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property is invalid; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property has the static modifier; otherwise, <c>false</c>.</returns>
	internal static bool IsStatic (Property property, RootContext context, out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
		=> ModifiersStrategies.IsStatic (property.Modifiers, RBI0004, out diagnostics, location, property.Name);

	/// <summary>
	/// Validates that field property flags are appropriate for field properties.
	/// Many property flags are ignored when used on fields, and this method validates that only valid flags are used.
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if invalid flags are used; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all flags are valid for field properties; otherwise, <c>false</c>.</returns>
	internal static bool FlagsAreValid (Property property, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics,
		Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!property.IsField) {
			// this is a bug, return the diagnostic for it
			diagnostics = [Diagnostic.Create (
				RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				location: location,
				messageArgs: property.Name
			)];
			return false;
		}

		// there are a number of flags that have no effect in a field property, we are not raise a warning for each
		// of them since they are ignored by rgen. The user that has warnings as errors will have to deal with them.
		var ignoredFlags = new [] {
			PropertyFlag.IsThreadStatic,
			PropertyFlag.MarshalNativeExceptions,
			PropertyFlag.CustomMarshalDirective,
			PropertyFlag.DisableZeroCopy,
			PropertyFlag.IsThreadSafe,
			PropertyFlag.Transient,
			PropertyFlag.PlainString,
			PropertyFlag.CoreImageFilterProperty,
			PropertyFlag.AutoRelease,
			PropertyFlag.RetainReturnValue,
			PropertyFlag.ReleaseReturnValue,
			PropertyFlag.Proxy,
			PropertyFlag.WeakDelegate,
			PropertyFlag.Optional,
			PropertyFlag.CreateEvents,
		};

		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var flag in ignoredFlags) {
			if (property.ExportFieldData.FieldData.Flags.HasFlag (flag)) {
				builder.Add (
					Diagnostic.Create (
						descriptor: RBI0028, // The flag '{0}' is no supported on '{1}' and will be ignored</value>
						location: property.ExportFieldData.FieldData.Location,
						$"ObjCBindings.Property.{flag}", "Field<Property>"
					)
				);
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that the field property and its accessors are available on the current platform.
	/// This method checks platform availability for the property itself, its getter (if present), and its setter (if present).
	/// </summary>
	/// <param name="property">The property to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains an array of diagnostics if the property or its accessors are not available on the current platform; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if the property and all its accessors are available on the current platform; otherwise, <c>false</c>.</returns>
	internal static bool IsValidPlatform (Property property, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		if (!property.IsField) {
			// this is a bug, return the diagnostic for it
			diagnostics = [Diagnostic.Create (
				RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				location: location,
				messageArgs: property.Name
			)];
			return false;
		}

		// there are several locations in which we need to check if the field is valid for the platform
		// 1. the property itself
		// 2. the getter if present
		// 3. the setter if present
		if (!SupportedPlatformStrategies.IsValidPlatform (property.SymbolAvailability,
				context, out diagnostics, property.Name, property.Location)) {
			return false;
		}

		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		// if we have a getter or a setter, we want to validate them as well but we will return those merged in the
		// diagnostics rather than returning false if one of them is invalid
		var getter = property.GetAccessor (AccessorKind.Getter);
		if (!getter.IsNullOrDefault) {
			if (!SupportedPlatformStrategies.IsValidPlatform (getter.SymbolAvailability, context,
					out var getterDiagnostics,
					$"{property.Name}.get", getter.Location)) {
				builder.AddRange (getterDiagnostics);
			}
		}

		var setter = property.GetAccessor (AccessorKind.Setter);
		if (!setter.IsNullOrDefault) {
			if (!SupportedPlatformStrategies.IsValidPlatform (setter.SymbolAvailability, context,
					out var setterDiagnostics,
					$"{property.Name}.set", setter.Location)) {
				builder.AddRange (setterDiagnostics);
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FieldValidator"/> class.
	/// </summary>
	public FieldValidator () : base (p => p.Location)
	{
		// check the selector, the selector property in the struct does the correct thing
		AddStrategy (p => p.Selector, RBI0018, SelectorIsNotNull);
		AddStrategy (p => p.Selector, RBI0019, SelectorHasNoWhitespace);

		// fields have to be partial
		AddGlobalStrategy (RBI0001, IsPartial);
		// fields have to be static	
		AddGlobalStrategy (RBI0004, IsStatic);

		// ensure that the flags are valid
		AddGlobalStrategy ([RBI0000, RBI0028], FlagsAreValid);

		// check platform validity for the property and its accessors
		AddGlobalStrategy ([RBI0000, RBI0027], IsValidPlatform);
	}
}
