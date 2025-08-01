// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
using ObjCBindings;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emits the wrapper class for a protocol.
/// </summary>
class ProtocolWrapperEmitter : ICodeEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding) => Nomenclator.GetProtocolWrapperName (binding.Name);

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements { get; } = [];

	/// <summary>
	/// Emits the default constructors for the protocol wrapper class.
	/// </summary>
	/// <param name="bindingContext">The binding context.</param>
	/// <param name="classBlock">The writer for the class block.</param>
	void EmitDefaultConstructors (in BindingContext bindingContext, TabbedWriter<StringWriter> classBlock)
	{
		classBlock.WriteLine ("// Implement default constructor");
		classBlock.WriteLine ();
	}

	/// <summary>
	/// Emits the properties for the protocol wrapper class.
	/// </summary>
	/// <param name="context">The binding context.</param>
	/// <param name="classBlock">The writer for the class block.</param>
	void EmitProperties (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var allProperties = context.Changes.Properties
			.Concat (context.Changes.ParentProtocolProperties)
			.OrderBy (p => p.Name);

		foreach (var property in allProperties) {
			classBlock.WriteLine ($"// Implement property: {property.Name}");
			classBlock.WriteLine ();
		}
	}

	/// <summary>
	/// Emits the methods for the protocol wrapper class.
	/// </summary>
	/// <param name="context">The binding context.</param>
	/// <param name="classBlock">The writer for the class block.</param>
	void EmitMethods (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var allMethods = context.Changes.Methods
			.Concat (context.Changes.ParentProtocolMethods)
			.OrderBy (m => m.Name);

		foreach (var method in allMethods) {
			classBlock.WriteLine ($"// Implement method: {method.Name}");
			classBlock.WriteLine ();
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		var bindingData = (BindingTypeData<Protocol>) bindingContext.Changes.BindingInfo;
		var protocolName = bindingData.Name ?? bindingContext.Changes.Name [1..];
		var wrapperName = Nomenclator.GetProtocolWrapperName (protocolName);

		// we do not emit outer classes for protocol wrappers
		using (var classBlock = bindingContext.Builder.CreateBlock (
				   $"internal unsafe sealed class {wrapperName} : BaseWrapper, {bindingContext.Changes.Name}",
				   true)) {
			EmitDefaultConstructors (bindingContext, classBlock);
			EmitProperties (bindingContext, classBlock);
			EmitMethods (bindingContext, classBlock);
		}

		return true;
	}

}
