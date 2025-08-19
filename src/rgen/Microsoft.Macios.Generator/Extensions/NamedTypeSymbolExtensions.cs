// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;

namespace Microsoft.Macios.Generator.Extensions;

static class NamedTypeSymbolExtensions {
	public static bool TryGetEnumFields (this INamedTypeSymbol enumSymbol,
		[NotNullWhen (true)]
		out ImmutableArray<(IFieldSymbol Symbol, FieldData<EnumValue> FieldData)>? fields,
		[NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		fields = null;
		diagnostics = null;

		// we can only return fields for enums
		if (enumSymbol.TypeKind != TypeKind.Enum) {
			diagnostics = [Diagnostic.Create (RgenDiagnostics.RBI0000, // An unexpected error ocurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				enumSymbol.Locations [0], enumSymbol.ToDisplayString ().Trim ())];
			return false;
		}

		// because we are dealing with an enum, we need to get all the fields from the symbol but we need to
		// keep the order in which they are defined in the source code.

		var fieldBucket =
			ImmutableArray.CreateBuilder<(IFieldSymbol Symbol, FieldData<EnumValue> FieldData)> ();

		var members = enumSymbol.GetMembers ().OfType<IFieldSymbol> ().ToArray ();
		foreach (var fieldSymbol in members) {
			var attributes = fieldSymbol.GetAttributeData ();
			if (attributes.Count == 0)
				continue;

			// Get all the FieldAttribute, parse it and add the data to the result
			if (attributes.TryGetValue (AttributesNames.EnumFieldAttribute, out var fieldAttrDataList)) {
				if (fieldAttrDataList.Count != 1) {
					// FieldAttribute restrictions does not allow it to appear more than once
					diagnostics = [Diagnostic.Create (RgenDiagnostics.RBI0000, // An unexpected error ocurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
						enumSymbol.Locations [0], fieldSymbol.ToDisplayString ().Trim ())];
					return false;
				}

				var fieldAttrData = fieldAttrDataList [0];
				var fieldSyntax = fieldAttrData.ApplicationSyntaxReference?.GetSyntax ();
				if (fieldSyntax is null)
					continue;

				if (FieldData<EnumValue>.TryParse (fieldAttrData, out var fieldData)) {
					fieldBucket.Add ((Symbol: fieldSymbol, FieldData: fieldData.Value));
				} else {
					diagnostics = [Diagnostic.Create (RgenDiagnostics.RBI0000, // An unexpected error ocurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
						enumSymbol.Locations [0], fieldSymbol.ToDisplayString ().Trim ())];
					return false;
				}
			}
		}

		fields = fieldBucket.ToImmutable ();
		return true;
	}

	/// <summary>
	/// Gets all members from all interfaces implemented by the specified symbol.
	/// </summary>
	/// <param name="symbol">The symbol whose interface members are to be retrieved.</param>
	/// <returns>An enumerable collection of all members from all implemented interfaces.</returns>
	public static IEnumerable<ISymbol> GetAllInterfaceMembers (this INamedTypeSymbol symbol)
	{
		foreach (var i in symbol.AllInterfaces) {
			foreach (var member in i.GetMembers ()) {
				yield return member;
			}
		}
	}
}
