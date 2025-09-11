// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Bindings.Analyzer.Validators;

/// <summary>
/// Helper equality comparer used only within this file to key the async methods dictionary.
/// Compares tuples of (method name, ordered parameter types) to detect duplicate async method names.
/// Being declared with the 'file' modifier keeps the helper local to this source file.
/// </summary>
file class AsyncNameComparer : IEqualityComparer<(string, TypeInfo [])> {
	/// <summary>
	/// Determines equality by requiring the same method name and identical ordered parameter type sequence.
	/// </summary>
	public bool Equals ((string, TypeInfo []) x, (string, TypeInfo []) y)
	{
		// has to be the same name and with the same parameter type in the same order to be considered equal
		return x.Item1 == y.Item1 && x.Item2.SequenceEqual (y.Item2);
	}

	/// <summary>
	/// Computes a hash code combining the method name and ordered parameter types.
	/// </summary>
	public int GetHashCode ((string, TypeInfo []) obj)
	{
		int hash = obj.Item1.GetHashCode ();
		foreach (var t in obj.Item2)
			hash = hash * 31 + (t.GetHashCode ());
		return hash;
	}
}

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
	/// Validates async methods in a binding. This includes checking for methods that should be async, methods marked as
	/// async that are invalid, and ensuring that generated async method names are unique.
	/// </summary>
	/// <param name="binding">The binding to validate.</param>
	/// <param name="context">The root context for validation.</param>
	/// <param name="diagnostics">When this method returns, contains diagnostics for any async method issues; otherwise, an empty array.</param>
	/// <param name="location">The code location to be used for the diagnostics.</param>
	/// <returns><c>true</c> if all async methods are valid; otherwise, <c>false</c>.</returns>
	bool ValidAsyncMethods (Binding binding, RootContext context,
		out ImmutableArray<Diagnostic> diagnostics, Location? location = null)
	{
		// there are several things we need to validate with async methods:
		// 1. If a method parameter is a callback delegate and the method is not marked as async, we have to report a
		//    diagnostic as a warning.
		// 2. I a method is marked as async yet it does not have a callback delegate parameter, we have to report a
		//    diagnostic as an error.
		// 3. Collect all the names of async methods and ensure that there are no duplicates across the binding.
		//	  If there are duplicates, we have to report a diagnostic as an error.
		diagnostics = ImmutableArray<Diagnostic>.Empty;
		var builder = ImmutableArray.CreateBuilder<Diagnostic> ();
		// create a dictionary with a custom comparer that checks the method name and the parameter types
		var asyncMethodNames = new Dictionary<(string Name, TypeInfo [] Arguments), List<(string SymbolName, Location? Location)>> (
			new AsyncNameComparer ());

		foreach (var currentMethod in binding.Methods) {
			if (currentMethod.IsAsync) {
				if (!currentMethod.ReturnType.IsVoid) {
					builder.Add (Diagnostic.Create (
						descriptor: RBI0035, // The method '{0}' was marked as async but its return type is not void
						location: currentMethod.Location,
						messageArgs: [
							currentMethod.Name,
						]));
					continue;
				}
				// if it was marked as async, we need to ensure that it has at least a parameter, if there are not
				// params, the method can't be async
				if (currentMethod.Parameters.Length == 0) {
					// error, we need at least one parameter for the method to be async, report a diagnostic
					builder.Add (Diagnostic.Create (
						descriptor: RBI0036, // The method '{0}' was marked as async but has 0 parameters when at least a single delegate parameter is required
						location: currentMethod.Location,
						messageArgs: [
							currentMethod.Name,
						]));
					continue;
				}

				if (!currentMethod.Parameters [^1].Type.IsDelegate) {
					// error, we need the last parameter to be a delegate type for the method to be async, report a diagnostic	
					builder.Add (Diagnostic.Create (
						descriptor: RBI0037,
						location: currentMethod.Location,
						messageArgs: [
							currentMethod.Name,
						]));
				} else {
					// parameters are valid, but we want to get the async method name to ensure that the name is unique
					// across the binding for async methods. The async method name + parameter count has to be unique.
					var asyncMethod = currentMethod.ToAsync ();
					var asyncMethodKey = (
						asyncMethod.Name,
						asyncMethod.Parameters.Select (x => x.Type).ToArray ());
					if (asyncMethodNames.TryGetValue (asyncMethodKey, out var list)) {
						list.Add ((currentMethod.Name, currentMethod.Location));
					} else {
						// add a new list with the current property
						asyncMethodNames.Add (asyncMethodKey, [(currentMethod.Name, currentMethod.Location)]);
					}

					// add a extra validation, if current method has more than one parameter + the delegate and does 
					// not provide a return type, create a warning for the user to avoid the usage of a nameless
					// tuple
					if (currentMethod.Parameters.Length > 2 // more than one parameter + the delegate
						&& currentMethod.ExportMethodData.ResultType.IsNullOrDefault // does not provide a return type
						&& currentMethod.ExportMethodData.ResultTypeName is null // does not provide a return type name to generate
						) {
						// create warning to inform that a nameless tuple will be generated for the async method
						// and that it is better to provide a return type name
						builder.Add (Diagnostic.Create (
							descriptor: RBI0040, // The method '{0}' was marked as async and has multiple parameters but does not provide a return type name, a nameless tuple will be generated for the async method
							location: currentMethod.Location,
							messageArgs: [
								currentMethod.Name,
							]));
					}
				}
			} else {
				// this is not an async method, but we need to check if it has a callback delegate parameter,
				// if it does, we need to report a warning, not an error because maybe the user does not want to use
				// the async feature
				if (currentMethod.ReturnType.IsVoid && currentMethod.Parameters [^1].Type.IsDelegate) {
					// report a warning
					builder.Add (Diagnostic.Create (
						descriptor: RBI0038, // The method '{0}' was not marked as async but it can be
						location: currentMethod.Location,
						messageArgs: [
							currentMethod.Name,
						]));
				}
			}
		}

		// we have gone through all the methods, now we need to check if there are any duplicate async method names
		var duplicates = asyncMethodNames.Where (x => x.Value.Count > 1).ToImmutableArray ();
		if (duplicates.Length > 0) {
			foreach (var duplicate in duplicates) {
				var firstSymbol = duplicate.Value.First ();
				for (var index = 1; index < duplicate.Value.Count; index++) {
					var dupSymbol = duplicate.Value [index]; // used for the msg and the location
					builder.Add (Diagnostic.Create (
						descriptor: RBI0039,
						location: dupSymbol.Location,
						messageArgs: [
							duplicate.Key.Name,
							dupSymbol.SymbolName,
							firstSymbol.SymbolName
						]));
				}
			}
		}

		diagnostics = builder.ToImmutable ();
		return diagnostics.Length == 0;
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

		// validate async methods. This is a global strategy because it needs to look at all the methods in the binding
		// are validated together so that async methods do not have the same names
		AddGlobalStrategy ([RBI0035, RBI0036, RBI0037, RBI0038, RBI0039, RBI0040], ValidAsyncMethods);

		// validate that strong delegates are not duplicated, this is only for weak properties
		AddStrategy (
			b => b.Properties.Where (p => p.IsWeakDelegate).ToImmutableArray (),
			[RBI0033],
			StrongDelegatesAreUnique, "WeakDelegates");
	}
}
