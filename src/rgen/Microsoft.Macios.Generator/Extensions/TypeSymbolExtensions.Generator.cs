// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;

namespace Microsoft.Macios.Generator.Extensions;

static partial class TypeSymbolExtensions {

	/// <summary>
	/// Return the symbol availability WITHOUT taking into account the parent symbols availability.
	/// </summary>
	/// <param name="symbol">The symbols whose availability attributes we want to retrieve.</param>
	/// <returns>The symbol availability WITHOUT taking into account the parent symbols.</returns>
	/// <remarks>This is a helper method, you probably don't want to use it.</remarks>
	static SymbolAvailability GetAvailabilityForSymbol (this ISymbol symbol)
	{
		//get the attribute of the symbol and look for the Supported and Unsupported attributes and
		// add the different platforms to the result hashsets
		var builder = SymbolAvailability.CreateBuilder ();
		var boundAttributes = symbol.GetAttributes ();
		if (boundAttributes.Length == 0) {
			// no attrs in the symbol, therefore the symbol is supported in all platforms
			return builder.ToImmutable ();
		}

		foreach (var attributeData in boundAttributes) {
			var attrName = attributeData.AttributeClass?.ToDisplayString ();
			if (string.IsNullOrEmpty (attrName))
				continue;
			// we only care in this case about the support and unsupported attrs, ignore any other
			switch (attrName) {
			case AttributesNames.SupportedOSPlatformAttribute:
				if (SupportedOSPlatformData.TryParse (attributeData, out var supportedPlatform)) {
					builder.Add (supportedPlatform.Value);
				}

				break;
			case AttributesNames.UnsupportedOSPlatformAttribute:
				if (UnsupportedOSPlatformData.TryParse (attributeData, out var unsupportedPlatform)) {
					builder.Add (unsupportedPlatform.Value);
				}

				break;
			case AttributesNames.ObsoletedOSPlatformAttribute:
				if (ObsoletedOSPlatformData.TryParse (attributeData, out var obsoletedOsPlatform)) {
					builder.Add (obsoletedOsPlatform.Value);
				}

				break;
			default:
				continue;
			}
		}

		return builder.ToImmutable ();
	}

	/// <summary>
	/// Test if a symbol represents a smart enum.
	/// </summary>
	/// <param name="symbol">The symbol under query.</param>
	/// <returns>True if the symbol is a smart enum.</returns>
	public static bool IsSmartEnum (this ITypeSymbol symbol)
	{
		// a type is a smart enum if its type is a enum one AND it was decorated with the
		// binding type attribute
		return symbol.TypeKind == TypeKind.Enum
			   && symbol.HasAttribute (AttributesNames.SmartEnumAttribute);
	}

	/// <summary>
	/// Returns if a symbol represents a protocol.
	/// </summary>
	/// <param name="symbol">The symbol under query.</param>
	/// <returns>True if the symbol represents a protocol, false otherwise.</returns>
	public static bool IsProtocol (this ITypeSymbol symbol)
		=> symbol is INamedTypeSymbol { TypeKind: TypeKind.Interface } && symbol.HasAttribute (AttributesNames.ProtocolAttribute);

	/// <summary>
	/// Retrieves the binding type data from a symbol that represents a binding type.
	/// </summary>
	/// <param name="symbol">The symbol under query.</param>
	/// <typeparam name="T">The flag that identifies the type of binding.</typeparam>
	/// <returns>The binding type data for the binding that matches the flat T.</returns>
	public static BindingTypeData<T> GetBindingData<T> (this ISymbol symbol) where T : Enum
		=> GetAttribute<BindingTypeData<T>> (symbol, AttributesNames.GetBindingTypeAttributeName<T>, BindingTypeData<T>.TryParse) ?? default;

	/// <summary>
	/// Retrieve the data of an export attribute on a symbol.
	/// </summary>
	/// <param name="symbol">The tagged symbol.</param>
	/// <param name="context"></param>
	/// <typeparam name="T">Enum type used in the attribute.</typeparam>
	/// <returns>The data of the export attribute if present or null if it was not found.</returns>
	/// <remarks>If the passed enum is unknown or not supported as an enum for the export attribute, null will be
	/// returned.</remarks>
	public static ExportData<T>? GetExportData<T> (this ISymbol symbol, RootContext context) where T : Enum
	{
		return GetAttribute<ExportData<T>> (symbol, AttributesNames.GetExportAttributeName<T>, TryParseWrapped);

		// helper that will trap the context so that we can reuse the GetAttribute method
		bool TryParseWrapped (AttributeData attributeData, [NotNullWhen (true)] out ExportData<T>? data)
		{
			return ExportData<T>.TryParse (attributeData, context, out data);
		}
	}

	/// <summary>
	/// Retrieve the data of a field attribute on a symbol, usually a property.
	/// </summary>
	/// <param name="symbol">The tagged symbol.</param>
	/// <typeparam name="T">Enum type used in the attribute.</typeparam>
	/// <returns>The data of the export attribute if present or null if it was not found.</returns>
	/// <remarks>If the passed enum is unknown or not supported as an enum for the field attribute, null will be
	/// returned.</remarks>
	public static FieldData<T>? GetFieldData<T> (this ISymbol symbol) where T : Enum
		=> GetAttribute<FieldData<T>> (symbol, AttributesNames.GetFieldAttributeName<T>, FieldData<T>.TryParse);

	/// <summary>
	/// Retrieve the data from the bind from attribute on a symbol.
	/// </summary>
	/// <param name="symbol">The tagged symbol.</param>
	/// <returns>The data of a BindFromAttribute that was added to the symbol or null if it was not found.</returns>
	public static BindFromData? GetBindFromData (this ISymbol symbol)
		=> GetAttribute<BindFromData> (symbol, AttributesNames.BindFromAttribute, BindFromData.TryParse);

	public static ForcedTypeData? GetForceTypeData (this ISymbol symbol)
		=> GetAttribute<ForcedTypeData> (symbol, AttributesNames.ForcedTypeAttribute, ForcedTypeData.TryParse);

	public static bool X86_64NeedStret (ITypeSymbol returnType)
	{
		if (!returnType.IsValueType || returnType.SpecialType == SpecialType.System_Enum ||
			returnType.TryGetBuiltInTypeSize ())
			return false;

		var fieldTypes = new List<ITypeSymbol> ();
		return GetValueTypeSize (returnType, fieldTypes) > 16;
	}

	/// <summary>
	/// Return if a given ITypeSymbol requires to use the objc_MsgSend_stret variants.
	/// </summary>
	/// <param name="returnType">The type we are testing.</param>
	/// <param name="compilation">The current compilation, used to determine the target platform.</param>
	/// <returns>If the type represented by the symtol needs a stret call variant.</returns>
	public static bool NeedsStret (this ITypeSymbol returnType, Compilation compilation)
	{
		// pointers do not need stret
		if (returnType is IPointerTypeSymbol)
			return false;

		return X86_64NeedStret (returnType);
	}

}
