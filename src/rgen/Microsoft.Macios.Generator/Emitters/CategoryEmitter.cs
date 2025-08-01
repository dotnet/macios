// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
using ObjCBindings;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter for Objective-C categories.
/// </summary>
class CategoryEmitter : IClassEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding) => binding.Name;
	/// <inheritdoc />
	public IEnumerable<string> UsingStatements => [
		"System",
		"System.Drawing",
		"System.Diagnostics",
		"System.ComponentModel",
		"System.Runtime.Versioning",
		"System.Runtime.InteropServices",
		"System.Diagnostics.CodeAnalysis",
		"ObjCRuntime",
	];

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.Category) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		var bindingData = (BindingTypeData<Category>) bindingContext.Changes.BindingInfo;
		if (bindingData.CategoryType.IsNullOrDefault) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}
		var registrationName = bindingData.CategoryType.Name;

		// namespace declaration
		this.EmitNamespace (bindingContext);

		using (var _ = this.EmitOuterClasses (bindingContext, out var builder)) {
			// append the class availability, this will add the necessary attributes to the class
			builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);

			var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
			// class declaration, the analyzer should ensure that the class is static, otherwise it will fail to compile with an error.
			using (var classBlock = builder.CreateBlock (
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name}",
					   true)) {
				// emit the fields for the selectors before we register the class or anything
				this.EmitSelectorFields (bindingContext, classBlock);

				classBlock.WriteLine (
					$"static readonly {NativeHandle} {ClassPtr} = {BindingSyntaxFactory.Class}.GetHandle (\"{registrationName}\");");

				// categories only have methods since we cannot have extensions properties in C#.
				this.EmitMethods (bindingContext, classBlock);
			}

			return true;
		}
	}
}
