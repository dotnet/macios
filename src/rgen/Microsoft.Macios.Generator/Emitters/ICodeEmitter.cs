// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;

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

static class ICodeEmitterExtensions {

	/// <summary>
	/// Helper class to manage the disposal of the writers for the outer classes.
	/// </summary>
	class Blocks (LinkedList<TabbedWriter<StringWriter>> blocks) : IDisposable {
		public void Dispose ()
		{
			// close the outer classes since we used a LinkedList to store them and insert them at the head, we can
			// simply dispose them in current order, which is the reverse of the creation order
			foreach (var outerClassesBuilder in blocks) {
				outerClassesBuilder.Dispose ();
			}
		}
	}

	/// <summary>
	/// Emits the namespace declaration for the binding.
	/// </summary>
	/// <param name="self">The code emitter.</param>
	/// <param name="bindingContext">The context for the binding.</param>
	public static void EmitNamespace (this ICodeEmitter self, in BindingContext bindingContext)
	{
		bindingContext.Builder.WriteLine ();
		bindingContext.Builder.WriteLine ($"namespace {string.Join (".", bindingContext.Changes.Namespace)};");
		bindingContext.Builder.WriteLine ();
	}

	/// <summary>
	/// Emits the outer classes for the binding.
	/// </summary>
	/// <param name="self">The code emitter.</param>
	/// <param name="bindingContext">The context for the binding.</param>
	/// <param name="builder">The tabbed writer for the innermost class.</param>
	/// <returns>An <see cref="IDisposable"/> that will close the outer classes blocks when disposed.</returns>
	public static IDisposable EmitOuterClasses (this ICodeEmitter self, in BindingContext bindingContext,
		out TabbedWriter<StringWriter> builder)
	{
		// if the type of the change contains outer classes, we need to emit the outer classes first
		builder = bindingContext.Builder;
		// create a list for the outer classes builders so that we can dispose them later, we are using a linked list
		// because AddFirst is O(1) and we need to dispose them in the reverse order of creation
		var outerClassesBuilders = new LinkedList<TabbedWriter<StringWriter>> ();
		if (bindingContext.Changes.OuterClasses.Length > 0) {
			// emit the outer classes and set the builder to the innermost class
			foreach (var outerClass in bindingContext.Changes.OuterClasses) {
				var outerModifiers = $"{string.Join (' ', outerClass.Modifiers)} ";
				builder = builder.CreateBlock ($"{(string.IsNullOrWhiteSpace (outerModifiers) ? string.Empty : outerModifiers)}class {outerClass.Name}", true);
				// add the current builder to the list so that we can dispose it later, we add it to the head of the
				// list so that we can dispose it in the reverse order of creation
				outerClassesBuilders.AddFirst (builder);
			}
		}
		return new Blocks (outerClassesBuilders);
	}
}
