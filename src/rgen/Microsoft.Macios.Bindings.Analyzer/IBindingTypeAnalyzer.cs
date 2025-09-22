// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Context;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Interface to be implemented by those analyzer that will be looking at BindingTypes.
/// </summary>
interface IBindingTypeAnalyzer<T> where T : BaseTypeDeclarationSyntax {
	IReadOnlySet<string> AttributeNames { get; }

	ImmutableArray<Diagnostic> Analyze (string matchedAttribute, RootContext context, T declarationNode, INamedTypeSymbol symbol);
}
