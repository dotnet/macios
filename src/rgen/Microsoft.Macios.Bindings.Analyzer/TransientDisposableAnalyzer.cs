// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Analyzer to ensure that transient disposable types (TransientString, TransientCFString, TransientCFObject)
/// are always declared with the 'using' keyword to guarantee proper disposal of native resources.
/// </summary>
[DiagnosticAnalyzer (LanguageNames.CSharp)]
public class TransientDisposableAnalyzer : DiagnosticAnalyzer {

	static readonly ImmutableHashSet<string> transientTypeFullNames = ImmutableHashSet.Create (
		"ObjCRuntime.TransientString",
		"ObjCRuntime.TransientCFString",
		"ObjCRuntime.TransientCFObject"
	);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create (RBI0042);

	public override void Initialize (AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis (GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
		context.EnableConcurrentExecution ();
		context.RegisterSyntaxNodeAction (AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
	}

	void AnalyzeNode (SyntaxNodeAnalysisContext context)
	{
		if (context.Node is not LocalDeclarationStatementSyntax localDeclaration)
			return;

		// If the declaration already has 'using', it's fine.
		if (localDeclaration.UsingKeyword != default)
			return;

		foreach (var variable in localDeclaration.Declaration.Variables) {
			var symbol = context.SemanticModel.GetDeclaredSymbol (variable);
			if (symbol is not ILocalSymbol localSymbol)
				continue;

			var typeDisplayName = localSymbol.Type.ToDisplayString ();
			if (!transientTypeFullNames.Contains (typeDisplayName))
				continue;

			var diagnostic = Diagnostic.Create (RBI0042, variable.GetLocation (), variable.Identifier.Text, localSymbol.Type.Name);
			context.ReportDiagnostic (diagnostic);
		}
	}
}
