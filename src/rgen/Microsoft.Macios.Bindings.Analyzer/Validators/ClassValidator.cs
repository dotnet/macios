// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Validator for class bindings.
/// </summary>
sealed class ClassValidator : BindingValidator {

	readonly ArrayValidator<Property> propertiesValidator = new (new PropertyOrFieldValidator ());

	/// <summary>
	/// Validates that strong delegate names are unique across all properties.
	/// </summary>
	/// <param name="properties">The properties to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any duplicate strong delegate names; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all strong delegate names are unique; otherwise, <c>false</c>.</returns>
	bool StrongDelegatesAreUnique (ImmutableArray<Property> properties, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		// use a dictionary to track all the strong names and the properties that use them
		var strongNames = new Dictionary<string, List<Property>> ();
		foreach (var p in properties) {
			var strongDelegate = p.ToStrongDelegate ();
			if (strongNames.TryGetValue (strongDelegate.Name, out var list)) {
				list.Add (p);
			} else {
				// add list with the current property since we want to use is as a ref
				strongNames.Add (strongDelegate.Name, [p]);
			}
		}
		// get all the strong names that have more than one property using them
		var duplicates = strongNames.Where (x => x.Value.Count > 1).ToImmutableArray ();
		if (duplicates.Length == 0) {
			// no duplicates, we are good
			return true;
		}
		// build the diagnostics
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		foreach (var duplicate in duplicates) {
			// add a diagnostic for each duplicate strong delegate using the first one as a reference and the second
			// one as the location of the error. We use the first one as a reference because we have to choose one and
			// is the one on top of the file
			var firstProperty = duplicate.Value.First ();
			for (var index = 1; index < duplicate.Value.Count; index++) {
				var dupProperty = duplicate.Value [index]; // used for the msg and the location
				builder.Add (Diagnostic.Create (
					descriptor: RBI0033,
					location: dupProperty.Location,
					messageArgs: [
						dupProperty.Name,
						duplicate.Key,
						firstProperty.Name
					]));
			}
		}
		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
	}

	/// <summary>
	/// Validates that selectors are unique across all properties and methods in a binding.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any duplicate selectors; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all selectors are unique; otherwise, <c>false</c>.</returns>
	bool SelectorsAreUnique (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();

		// the logic is as follows:
		// 1. Collect all selectors that we have decided to register. Those are the ones in properties and methods that
		// do not have the SkipRegister attribute.
		// 2. Collect the selectors based on them being static or instance selectors. We can have the same selector
		// for static and instance methods, but not for two static or two instance methods.

		var instanceSelectors = new Dictionary<string, List<(string SymbolName, Location? Location)>> ();
		var staticSelectors = new Dictionary<string, List<(string SymbolName, Location? Location)>> ();
		// collect property selectors
		foreach (var property in binding.Properties) {
			if (string.IsNullOrEmpty (property.Selector))
				continue;
			if (property.SkipRegistration)
				// user has decided to skip registration for this property, so we don't need to validate it
				continue;
			// decide which dictionary to use based on the property being static or instance
			var selectors = property.IsStatic ? staticSelectors : instanceSelectors;
			if (selectors.TryGetValue (property.Selector, out var list)) {
				list.Add ((property.Name, property.Location));
			} else {
				// add a new list with the current property
				selectors.Add (property.Selector, [(property.Name, property.Location)]);
			}
		}

		// collect method selectors
		foreach (var method in binding.Methods) {
			if (string.IsNullOrEmpty (method.Selector))
				continue;
			if (method.SkipRegistration)
				// user has decided to skip registration for this method, so we don't need to validate it
				continue;
			var selectors = method.IsStatic ? staticSelectors : instanceSelectors;
			if (selectors.TryGetValue (method.Selector, out var list)) {
				list.Add ((method.Name, method.Location));
			} else {
				// add a new list with the current property
				selectors.Add (method.Selector, [(method.Name, method.Location)]);
			}
		}
		// get all the selectors that have more than one property or method
		var instanceDuplicates = instanceSelectors.Where (x => x.Value.Count > 1).ToImmutableArray ();
		var staticDuplicates = staticSelectors.Where (x => x.Value.Count > 1).ToImmutableArray ();

		if (instanceDuplicates.Length == 0 && staticDuplicates.Length == 0) {
			// no duplicates, we are good
			return true;
		}
		// loop over each of the duplicates and create diagnostics for them, we do this separately for instance and
		// static selectors to make it easier to read the code and to avoid mixing selectors and getting confused about
		// which one is which.
		BuildDiagnostics (instanceDuplicates, builder);
		BuildDiagnostics (staticDuplicates, builder);

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;

		void BuildDiagnostics (ImmutableArray<KeyValuePair<string, List<(string SymbolName, Location? Location)>>> keyValuePairs,
			ImmutableArray<Diagnostic>.Builder builder1)
		{
			foreach (var duplicate in keyValuePairs) {
				var firstSymbol = duplicate.Value.First ();
				for (var index = 1; index < duplicate.Value.Count; index++) {
					var dupSymbol = duplicate.Value [index]; // used for the msg and the location
					builder1.Add (Diagnostic.Create (
						descriptor: RBI0034,
						location: dupSymbol.Location,
						messageArgs: [
							duplicate.Key,
							dupSymbol.SymbolName,
							firstSymbol.SymbolName
						]));
				}
			}
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ClassValidator"/> class.
	/// </summary>
	public ClassValidator ()
	{
		// class bindings must be partial
		AddGlobalStrategy (RBI0001, IsPartial);

		// use a nested validator to validate the properties and fields individually
		AddNestedValidator (b => b.Properties, propertiesValidator);

		// validate that the selectors are not duplicated, this includes properties and methods
		AddGlobalStrategy ([RBI0034], SelectorsAreUnique);

		// validate that strong delegates are not duplicated, this is only for weak properties
		AddStrategy (
			b => b.Properties.Where (p => p.IsWeakDelegate).ToImmutableArray (),
			[RBI0033],
			StrongDelegatesAreUnique, "WeakDelegates");
	}
}
