// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct Parameter {

	/// <summary>
	/// Returns the bind from data if present in the binding.
	/// </summary>
	public BindFromData? BindAs { get; init; }

	/// <summary>
	/// Returns the forced type data if present in the binding.
	/// </summary>
	public ForcedTypeData? ForcedType { get; init; }

	/// <summary>
	/// Returns if the parameter needs a null check when the code is generated.
	/// </summary>
	public bool NeedsNullCheck {
		get {
			if (ReferenceKind != ReferenceKind.None)
				return false;
			if (Type.IsNullable)
				return false;
			return Type.IsWrapped || Type.IsReferenceType;
		}
	}

	public static bool TryCreate (IParameterSymbol symbol, ParameterSyntax declaration, RootContext context,
		[NotNullWhen (true)] out Parameter? parameter)
	{
		parameter = new (symbol.Ordinal, new (symbol.Type, context.Compilation), symbol.GetSafeName ()) {
			BindAs = symbol.GetBindFromData (),
			ForcedType = symbol.GetForceTypeData (),
			IsOptional = symbol.IsOptional,
			IsParams = symbol.IsParams,
			IsThis = symbol.IsThis,
			DefaultValue = (symbol.HasExplicitDefaultValue) ? symbol.ExplicitDefaultValue?.ToString () : null,
			ReferenceKind = symbol.RefKind.ToReferenceKind (),
			Attributes = declaration.GetAttributeCodeChanges (context.SemanticModel),
		};
		return true;
	}
}
