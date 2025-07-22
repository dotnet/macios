// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter for Objective-C strong dictionaries.
/// </summary>
class StrongDictionaryEmitter : IClassEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding) => binding.Name;

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements { get; } = [];

	void EmitDefaultConstructors (in BindingContext bindingContext, TabbedWriter<StringWriter> classBlock)
	{
		// Emit default constructors, which are required for strong dictionaries. And their needed docs
		classBlock.WriteLine ();
		classBlock.WriteDocumentation (Documentation.StrongDictionary.EmptyConstructor (bindingContext.Changes.Name));
		classBlock.AppendPreserveAttribute (conditional: true);
		classBlock.WriteLine ($"public {bindingContext.Changes.Name} () : base (new {NSMutableDictionary} ()) {{}}");
		classBlock.WriteLine ();
		classBlock.WriteDocumentation (Documentation.StrongDictionary.InitWithDictionary (bindingContext.Changes.Name));
		classBlock.AppendPreserveAttribute (conditional: true);
		classBlock.WriteLine ($"public {bindingContext.Changes.Name} ({NSDictionary}? dictionary) : base (dictionary) {{}}");
	}

	void EmitProperties (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		classBlock.WriteLine ();
		classBlock.WriteLine ("// TODO: implement properties.");
		classBlock.WriteLine ();

		foreach (var property in context.Changes.StrongDictionaryProperties) {
			classBlock.WriteLine ();
			classBlock.WriteLine ($"// Emit code for property: {property.Name}");
			classBlock.WriteLine ();
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.StrongDictionary) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		// namespace declaration
		bindingContext.Builder.WriteLine ();
		bindingContext.Builder.WriteLine ($"namespace {string.Join (".", bindingContext.Changes.Namespace)};");
		bindingContext.Builder.WriteLine ();

		var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
		using (var classBlock = bindingContext.Builder.CreateBlock (
				   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name} : DictionaryContainer",
				   true)) {
			// we care about two specific things, the constructors and the strong dictionary properties
			EmitDefaultConstructors (bindingContext, classBlock);
			EmitProperties (bindingContext, classBlock);
		}

		return true;
	}
}
