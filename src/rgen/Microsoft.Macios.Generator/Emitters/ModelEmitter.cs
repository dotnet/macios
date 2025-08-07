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
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using ObjCBindings;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emits the model class for a protocol.
/// </summary>
class ModelEmitter : IClassEmitter {

	/// <inheritdoc />
	public string GetSymbolName (in Binding binding)
	{
		var bindingData = (BindingTypeData<Protocol>) binding.BindingInfo;
		// for the model we will always use the name of the class, not the register protocol
		return bindingData.ModelName ?? binding.Name [1..];
	}

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements { get; } = [];

	/// <summary>
	/// Emits the properties for the model class.
	/// </summary>
	/// <param name="context">The binding context.</param>
	/// <param name="classBlock">The writer for the class block.</param>
	void EmitProperties (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var allProperties = context.Changes.Properties
			.Concat (context.Changes.ParentProtocolProperties)
			.OrderBy (p => p.Name);

		foreach (var property in allProperties) {
			classBlock.AppendMemberAvailability (property.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute ();
			classBlock.AppendExportAttribute (property.ExportPropertyData);
			using (var propertyBlock = classBlock.CreateBlock (property.ToDeclaration ().ToString (), block: true)) {
				var getter = property.GetAccessor (AccessorKind.Getter);
				if (!getter.IsNullOrDefault) {
					propertyBlock.AppendMemberAvailability (getter.SymbolAvailability);
					propertyBlock.AppendGeneratedCodeAttribute ();
					if (!getter.ExportPropertyData.IsNullOrDefault)
						propertyBlock.AppendExportAttribute (getter.ExportPropertyData);
					using (var getterBlock = propertyBlock.CreateBlock ("get", true)) {
						getterBlock.WriteLine ($"throw new {ModelNotImplementedException} ();");
					}
				}

				var setter = property.GetAccessor (AccessorKind.Setter);
				if (!setter.IsNullOrDefault) {
					propertyBlock.AppendMemberAvailability (setter.SymbolAvailability);
					propertyBlock.AppendGeneratedCodeAttribute ();
					if (!getter.ExportPropertyData.IsNullOrDefault)
						propertyBlock.AppendExportAttribute (setter.ExportPropertyData);
					using (var setterBlock = propertyBlock.CreateBlock ("set", true)) {
						setterBlock.WriteLine ($"throw new {ModelNotImplementedException} ();");
					}
				}

			}

			classBlock.WriteLine ();
		}
	}

	/// <summary>
	/// Emits the methods for the model class.
	/// </summary>
	/// <param name="context">The binding context.</param>
	/// <param name="classBlock">The writer for the class block.</param>
	void EmitMethods (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var allMethods = context.Changes.Methods
			.Concat (context.Changes.ParentProtocolMethods)
			.OrderBy (m => m.Name);

		foreach (var method in allMethods) {
			classBlock.AppendMemberAvailability (method.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute ();
			classBlock.AppendExportAttribute (method.ExportMethodData);
			using (var methodBlock = classBlock.CreateBlock (method.ToDeclaration ().ToString (), block: true)) {
				methodBlock.WriteLine ($"throw new {ModelNotImplementedException} ();");
			}
			classBlock.WriteLine ();
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		var modelName = GetSymbolName (bindingContext.Changes);
		var registrationName =
			Nomenclator.GetProtocolModelRegistrationName (bindingContext.Changes.Namespace, modelName);
		bindingContext.Builder.AppendRegisterAttribute (registrationName, false);
		bindingContext.Builder.WriteLine ("[Model]");
		bindingContext.Builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);

		using (var classBlock = bindingContext.Builder.CreateBlock (
				   $"public unsafe abstract partial class {modelName} : NSObject, {bindingContext.Changes.Name}",
				   true)) {

			this.EmitDefaultNSObjectConstructors (
				className: modelName, classBlock: classBlock, disableDefaultCtor: false);
			EmitProperties (bindingContext, classBlock);
			EmitMethods (bindingContext, classBlock);
		}

		return true;
	}

}
