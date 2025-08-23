// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
		foreach (var property in context.Changes.StrongDictionaryProperties.OrderBy (p => p.Name)) {
			// make sure that the user did not forget to add the getter
			var getter = property.GetAccessor (AccessorKind.Getter);
			if (getter.IsNullOrDefault)
				continue;

			classBlock.WriteLine ();
			classBlock.AppendMemberAvailability (property.SymbolAvailability);
			var (getCall, setCall) = GetStrongDictionaryInvocations (property);
			using (var propertyBlock = classBlock.CreateBlock (property.ToDeclaration ().ToString (), block: true)) {

				propertyBlock.AppendMemberAvailability (getter.SymbolAvailability);
				using (var getterBlock = propertyBlock.CreateBlock ("get", block: true)) {
					getterBlock.WriteLine ($"return {ExpressionStatement (getCall)}");
				}

				var setter = property.GetAccessor (AccessorKind.Setter);
				if (setter.IsNullOrDefault)
					// we are done with the current property
					continue;

				propertyBlock.WriteLine (); // add space between getter and setter since we have the attrs
				propertyBlock.AppendMemberAvailability (setter.SymbolAvailability);
				using (var setterBlock = propertyBlock.CreateBlock ("set", block: true)) {
					setterBlock.WriteLine ($"{ExpressionStatement (setCall)}");
				}
			}
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.StrongDictionary) {
			diagnostics = [Diagnostic.Create (
				RgenDiagnostics
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
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name} : DictionaryContainer",
					   true)) {
				// we care about two specific things, the constructors and the strong dictionary properties
				EmitDefaultConstructors (bindingContext, classBlock);
				EmitProperties (bindingContext, classBlock);
			}

			return true;
		}
	}
}
