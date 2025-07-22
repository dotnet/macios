// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	internal static (ExpressionSyntax, ExpressionSyntax) GetStrongDictionaryInvocations (in Property property)
	{
		return (ThrowNotImplementedException (), ThrowNotImplementedException ());
	}
}
