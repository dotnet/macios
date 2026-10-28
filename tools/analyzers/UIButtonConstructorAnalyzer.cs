// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.Macios.Analyzers {
	[DiagnosticAnalyzer (LanguageNames.CSharp)]
	public sealed class UIButtonConstructorAnalyzer : DiagnosticAnalyzer {
		public const string DiagnosticId = "APL1000";

		static readonly ResourceManager resources = new ResourceManager ("Microsoft.Macios.Analyzers.Resources", typeof (UIButtonConstructorAnalyzer).Assembly);
		static readonly DiagnosticDescriptor rule = new DiagnosticDescriptor (
			DiagnosticId,
			new LocalizableResourceString ("UIButtonConstructorTitle", resources, typeof (UIButtonConstructorAnalyzer)),
			new LocalizableResourceString ("UIButtonConstructorMessage", resources, typeof (UIButtonConstructorAnalyzer)),
			"Usage",
			DiagnosticSeverity.Warning,
			isEnabledByDefault: true,
			description: new LocalizableResourceString ("UIButtonConstructorDescription", resources, typeof (UIButtonConstructorAnalyzer)),
			helpLinkUri: "https://github.com/dotnet/macios/blob/main/docs/diagnostics/APL1000.md");

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create (rule);

		public override void Initialize (AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis (GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution ();
			context.RegisterCompilationStartAction (startContext => {
				var button = startContext.Compilation.GetTypeByMetadataName ("UIKit.UIButton");
				var buttonType = startContext.Compilation.GetTypeByMetadataName ("UIKit.UIButtonType");
				if (button is null || buttonType is null)
					return;

				startContext.RegisterSyntaxNodeAction (nodeContext => {
					var constructor = nodeContext.SemanticModel.GetSymbolInfo (nodeContext.Node, nodeContext.CancellationToken).Symbol as IMethodSymbol;
					if (constructor is null ||
						!SymbolEqualityComparer.Default.Equals (constructor.ContainingType, button) ||
						constructor.Parameters.Length != 1 ||
						!SymbolEqualityComparer.Default.Equals (constructor.Parameters [0].Type, buttonType))
						return;

					nodeContext.ReportDiagnostic (Diagnostic.Create (rule, nodeContext.Node.GetLocation ()));
				}, SyntaxKind.BaseConstructorInitializer, SyntaxKind.PrimaryConstructorBaseType);
			});
		}
	}
}
