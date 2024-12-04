using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Interface to be implemented by those analyzer that will be looking at BindingTypes.
/// </summary>
public interface IBindingTypeAnalyzer<T> where T : BaseTypeDeclarationSyntax {
	ImmutableArray<Diagnostic> Analyze (PlatformName platformName, T declarationNode, INamedTypeSymbol symbol);
}
