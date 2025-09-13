// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.DataModel;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Extensions;

static partial class SemanticModelExtensions {

	/// <summary>
	/// Extracts symbol data from a type declaration including binding information, type details, and availability.
	/// </summary>
	/// <param name="self">The semantic model to analyze.</param>
	/// <param name="declaration">The type declaration syntax to process.</param>
	/// <param name="bindingType">The type of binding to extract data for.</param>
	/// <param name="name">When this method returns, contains the name of the symbol.</param>
	/// <param name="baseClass">When this method returns, contains the base class name if present; otherwise, null.</param>
	/// <param name="typeInfo">When this method returns, contains the type information for named types; otherwise, default.</param>
	/// <param name="interfaces">When this method returns, contains the implemented interface names.</param>
	/// <param name="outerClasses">When this method returns, contains the outer class hierarchy.</param>
	/// <param name="namespaces">When this method returns, contains the namespace hierarchy.</param>
	/// <param name="symbolAvailability">When this method returns, contains the platform availability information.</param>
	/// <param name="bindingInfo">When this method returns, contains the binding-specific information based on the binding type.</param>
	public static void GetSymbolData (this SemanticModel self, BaseTypeDeclarationSyntax declaration,
		BindingType bindingType,
		out string name,
		out string? baseClass,
		out TypeInfo typeInfo,
		out ImmutableArray<string> interfaces,
		out ImmutableArray<OuterClass> outerClasses,
		out ImmutableArray<string> namespaces,
		out SymbolAvailability symbolAvailability,
		out BindingInfo bindingInfo)
	{
		var symbol = self.GetDeclaredSymbol (declaration);
		// only named types have type info
		typeInfo = (symbol is INamedTypeSymbol namedTypeSymbol) ? new (namedTypeSymbol) : TypeInfo.Default;
		GetSymbolData (symbol, out name, out baseClass, out interfaces, out outerClasses, out namespaces, out symbolAvailability);
		if (symbol is null)
			bindingInfo = default;
		else {
			bindingInfo = bindingType switch {
				BindingType.Category => new BindingInfo (symbol.GetBindingData<ObjCBindings.Category> ()),
				BindingType.Class => new BindingInfo (symbol.GetBindingData<ObjCBindings.Class> ()),
				BindingType.Protocol => new BindingInfo (symbol.GetBindingData<ObjCBindings.Protocol> ()),
				BindingType.SmartEnum => new BindingInfo (symbol.GetBindingData<ObjCBindings.SmartEnum> ()),
				BindingType.StrongDictionary => new BindingInfo (symbol.GetBindingData<ObjCBindings.StrongDictionary> ()),
				BindingType.StrongDictionaryKeys => new BindingInfo (symbol.GetBindingData<ObjCBindings.StrongDictionaryKeys> ()),
				_ => default,
			};
		}
	}

}
