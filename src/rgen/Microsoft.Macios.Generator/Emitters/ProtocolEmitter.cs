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
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using Property = Microsoft.Macios.Generator.DataModel.Property;
using Method = Microsoft.Macios.Generator.DataModel.Method;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter responsible for generating protocol interfaces.
/// Generates C# interfaces that represent Objective-C protocols with proper protocol member attributes.
/// </summary>
class ProtocolEmitter : ICodeEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding) => binding.Name;

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements => [];

	/// <summary>
	/// Emits the default constructors and dynamic dependency attributes for the protocol interface.
	/// This includes DynamicDependencyAttribute for each property and a static constructor with GC.KeepAlive.
	/// </summary>
	/// <param name="bindingContext">The binding context containing protocol information.</param>
	/// <param name="interfaceBlock">The writer for the interface block.</param>
	void EmitDefaultConstructors (in BindingContext bindingContext, TabbedWriter<StringWriter> interfaceBlock)
	{
		// emit the DynamicDependencyAttribute per property
		foreach (var property in bindingContext.Changes.Properties.OrderBy (p => p.Name)) {
			interfaceBlock.AppendDynamicDependencyAttribute (property.Name);
		}
		interfaceBlock.AppendGeneratedCodeAttribute ();
		interfaceBlock.WriteRaw (
$@"static {bindingContext.Changes.Name} ()
{{
	{GC}.KeepAlive (null);
}}
");
	}

	/// <summary>
	/// Gets the properties from the binding context and their corresponding extension methods.
	/// Returns a collection of tuples containing the property and its optional getter/setter methods.
	/// </summary>
	/// <param name="bindingContext">The binding context containing the properties.</param>
	/// <returns>An immutable array of tuples containing properties and their extension methods.</returns>
	static ImmutableArray<(Property Property, Method? Getter, Method? Setter)> GetProperties (in BindingContext bindingContext)
	{
		// collect all properties and generate the extension methods, this will be used to generate the protocol
		// member data and later the extension methods.
		var propertiesBucket = ImmutableArray.CreateBuilder<(Property Property, Method? Getter, Method? Setter)> (bindingContext.Changes.Properties.Length);
		foreach (var property in bindingContext.Changes.Properties.OrderBy (p => p.Name)) {
			var (getter, setter) = property.ToExtensionMethods (new (bindingContext.Changes.Name, SpecialType.None));
			propertiesBucket.Add ((property, getter, setter));
		}
		var properties = propertiesBucket.ToImmutable ();
		return properties;
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.Protocol) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		var bindingData = (BindingTypeData<Protocol>) bindingContext.Changes.BindingInfo;
		// namespace declaration
		this.EmitNamespace (bindingContext);

		using (var _ = this.EmitOuterClasses (bindingContext, out var builder)) {
			// append the class availability, this will add the necessary attributes to the class
			builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);

			// Protocol registration
			var protocolName = bindingData.Name ?? bindingContext.Changes.Name [1..];
			builder.AppendProtocolAttribute (protocolName, Nomenclator.GetProtocolWrapperName (protocolName));

			// we need to collect the properties extension methods, we do that with a helper method
			// that will return the properties and their getters/setters.
			var properties = GetProperties (bindingContext);

			// append the properties to the protocol member data
			foreach (var (property, getter, setter) in properties) {
				var protocolMember = new ProtocolMemberData (property, getter, setter);
				builder.AppendProtocolMemberData (protocolMember);
			}

			var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
			// class declaration, the analyzer should ensure that the class is static, otherwise it will fail to compile with an error.
			using (var interfaceBlock = builder.CreateBlock (
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}interface {bindingContext.Changes.Name} : INativeObject, IDisposable",
					   true)) {
				// space for readability
				interfaceBlock.WriteLine ();

				// emit static constructor
				EmitDefaultConstructors (in bindingContext, interfaceBlock);
			}
		}
		return true;
	}
}
