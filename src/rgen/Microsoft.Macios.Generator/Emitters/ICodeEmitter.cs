// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Interface to be implemented by all those classes that know how to emit code for a binding.
/// </summary>
interface ICodeEmitter {
	/// <summary>
	/// Gets the name of the symbol to be emitted. This is used to generate the file name.
	/// </summary>
	/// <param name="binding">The binding information.</param>
	/// <returns>The name of the symbol.</returns>
	string GetSymbolName (in Binding binding);

	/// <summary>
	/// Tries to emit the code for the given binding context.
	/// </summary>
	/// <param name="bindingContext">The context for the binding.</param>
	/// <param name="diagnostics">An array of diagnostics if the emission fails.</param>
	/// <returns><c>true</c> if the code was emitted successfully; otherwise, <c>false</c>.</returns>
	bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics);

	/// <summary>
	/// Gets the collection of using statements required by the emitted code.
	/// </summary>
	IEnumerable<string> UsingStatements { get; }
}
