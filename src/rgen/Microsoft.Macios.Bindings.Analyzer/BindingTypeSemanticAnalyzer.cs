// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Macios.Bindings.Analyzer.Extensions;
using Microsoft.Macios.Bindings.Analyzer.Validators;
using Microsoft.Macios.Generator;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.RgenDiagnostics;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Analyzer that ensures that the types that have been declared as binding types are partial and follow the correct
/// pattern.
/// </summary>
[DiagnosticAnalyzer (LanguageNames.CSharp)]
class BindingTypeSemanticAnalyzer : DiagnosticAnalyzer, IBindingTypeAnalyzer<BaseTypeDeclarationSyntax> {

	// create a validator for each binding type
	readonly ClassValidator classValidator = new ();
	readonly CategoryValidator categoryValidator = new ();
	readonly ProtocolValidator protocolValidator = new ();
	readonly SmartEnumValidator smartEnumValidator = new ();
	readonly StrongDictionaryValidator strongDictionaryValidator = new ();

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics {
		get {
			// use a hashset to avoid duplicates to add all the descriptors that we use in this analyzer + all
			// the ones from the validators. This initial set has be done manually, all others can be added by the validators
			var descriptors = new HashSet<DiagnosticDescriptor> {
				RBI0000,
				RBI0002,
				RBI0003,
				RBI0005,
				RBI0006,
				RBI0007,
			};

			foreach (var validator in new BindingValidator [] {classValidator,
						 categoryValidator, protocolValidator, smartEnumValidator, strongDictionaryValidator}) {
				foreach (var diagnosticDescriptor in validator.Descriptors) {
					descriptors.Add (diagnosticDescriptor);
				}
			}
			return [.. descriptors];
		}
	}

	public override void Initialize (AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis (GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution ();
		context.RegisterSyntaxNodeAction (AnalysisContext,
			SyntaxKind.ClassDeclaration,
			SyntaxKind.InterfaceDeclaration,
			SyntaxKind.EnumDeclaration);
	}

	void AnalysisContext (SyntaxNodeAnalysisContext context)
		=> this.AnalyzeBindingType (context);

	static readonly HashSet<string> attributes = new (AttributesNames.BindingTypes);
	public IReadOnlySet<string> AttributeNames => attributes;

	static ImmutableArray<Diagnostic> Validate<T> (BindingValidator validator, RootContext context, DiagnosticDescriptor wrongTypeDescriptor,
		BaseTypeDeclarationSyntax declarationNode, INamedTypeSymbol symbol) where T : BaseTypeDeclarationSyntax
	{
		var bucket = ImmutableArray.CreateBuilder<Diagnostic> ();
		if (declarationNode is T baseTypeDeclarationSyntax) {
			var binding = Binding.FromDeclaration (baseTypeDeclarationSyntax, context);
			if (binding is null) {
				// add a diagnostic if the binding could not be created
				bucket.Add (Diagnostic.Create (
					RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
					location: baseTypeDeclarationSyntax.GetLocation (),
					messageArgs: symbol.ToDisplayString ()));
			} else {
				var allErrors = validator.ValidateAll (binding.Value, context);
				// flatten the errors and add them to the bucket
				foreach (var (_, errors) in allErrors) {
					bucket.AddRange (errors);
				}
			}
		} else {
			var notAInterfaceDiagnostic = Diagnostic.Create (
				descriptor: wrongTypeDescriptor, // custom error message for a specific binding type
				location: declarationNode.Identifier.GetLocation (),
				messageArgs: symbol.ToDisplayString ());
			bucket.Add (notAInterfaceDiagnostic);
		}

		return bucket.ToImmutable ();
	}

	public ImmutableArray<Diagnostic> Analyze (string matchedAttribute, RootContext context,
		BaseTypeDeclarationSyntax declarationNode, INamedTypeSymbol symbol)
		=> matchedAttribute switch {
			AttributesNames.ClassAttribute
				=> Validate<ClassDeclarationSyntax> (classValidator, context, RBI0002, declarationNode, symbol),
			AttributesNames.CategoryAttribute
				=> Validate<ClassDeclarationSyntax> (categoryValidator, context, RBI0003, declarationNode, symbol),
			AttributesNames.ProtocolAttribute
				=> Validate<InterfaceDeclarationSyntax> (protocolValidator, context, RBI0005, declarationNode, symbol),
			AttributesNames.SmartEnumAttribute
				=> Validate<EnumDeclarationSyntax> (smartEnumValidator, context, RBI0006, declarationNode, symbol),
			AttributesNames.StrongDictionaryAttribute
				=> Validate<ClassDeclarationSyntax> (strongDictionaryValidator, context, RBI0007, declarationNode, symbol),
			_ => throw new InvalidOperationException ($"Not recognized attribute {matchedAttribute}.")
		};
}
