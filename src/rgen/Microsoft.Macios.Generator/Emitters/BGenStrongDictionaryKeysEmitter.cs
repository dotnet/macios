// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter responsible for generating strong dictionary keys classes compatible with bgen output.
/// This emitter mimics the behavior of bgen for strong dictionary keys generation.
/// </summary>
class BGenStrongDictionaryKeysEmitter : IClassEmitter {

	/// <inheritdoc />
	public string GetSymbolName (in Binding binding)
	{
		// in this case the symbol name is the name passed to the binding type attr
		var bindingTypeData = (BindingTypeData<ObjCBindings.StrongDictionaryKeys>) binding.BindingInfo;
		return bindingTypeData.Name ?? string.Empty;
	}

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements { get; } = [];

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.StrongDictionaryKeys) {
			diagnostics = [Diagnostic.Create (
				RgenDiagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}
		// emit a preprocessor directive so that we do not include the file if we are in XAMCORE_5
		bindingContext.Builder.WriteLine ("#if !XAMCORE_5_0");

		// this code emitter is trying to copy what bgen generates, we will use the namespace of the type, but because
		// bgen does not have the keys as a nester class, we are going to ignore the outer classes.
		this.EmitNamespace (bindingContext);

		// add an obsolete attribute to the class so that it is clear that this is a bgen generated class and the new nested
		// strong dictionary keys should be used instead.
		var outerClasses = string.Join ('.', bindingContext.Changes.OuterClasses.Select (x => x.Name));
		bindingContext.Builder.WriteLine ($"[Obsolete (\"Use '{outerClasses}.{bindingContext.Changes.Name}' class instead.\", false)]");
		bindingContext.Builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);
		var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
		var bindingTypeData = (BindingTypeData<ObjCBindings.StrongDictionaryKeys>) bindingContext.Changes.BindingInfo;
		using (var classBlock = bindingContext.Builder.CreateBlock (
				   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingTypeData.Name}",
				   true)) {
			// the only thing we care about is the keys, so we emit the keys
			this.EmitFields (bindingContext.Changes.Name, bindingContext.Changes.Properties, classBlock,
				out var _);
		}

		bindingContext.Builder.WriteLine ("#endif");
		return true;
	}

}
