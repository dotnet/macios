// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter responsible for generating strong dictionary keys classes.
/// </summary>
class StrongDictionaryKeysEmitter : IClassEmitter {

	/// <inheritdoc />
	public string GetSymbolName (in Binding binding)
	{
		var outerClasses = binding.OuterClasses.Select (x => x.Name);
		var prefix = string.Join ('.', outerClasses);
		return string.IsNullOrEmpty (prefix)
			? binding.Name
			: $"{prefix}.{binding.Name}";
	}

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements { get; } = [];

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.StrongDictionaryKeys) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		// namespace declaration
		this.EmitNamespace (bindingContext);

		using (var _ = this.EmitOuterClasses (bindingContext, out var builder)) {
			builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);
			var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
			using (var classBlock = builder.CreateBlock (
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name}",
					   true)) {
				// the only thing we care about is the keys, so we emit the keys
				this.EmitFields (bindingContext.Changes.Name, bindingContext.Changes.Properties, classBlock,
					out var _);
			}
		}
		return true;
	}

}
