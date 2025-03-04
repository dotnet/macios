// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct TypeInfo {

	/// <summary>
	/// True if the type needs to use a stret call.
	/// </summary>
	public bool NeedsStret { get; init; }

	/// <summary>
	/// Get the name of the variable for the type when it is used as a return value.
	/// </summary>
	public string ReturnVariableName => "ret"; // nothing fancy for now

	internal TypeInfo (ITypeSymbol symbol, Compilation compilation) : this (symbol)
	{
		IsNativeEnum = symbol.HasAttribute (AttributesNames.NativeEnumAttribute);
		NeedsStret = symbol.NeedsStret (compilation);
	}

}
