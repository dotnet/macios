// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Extensions;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests;

/// <summary>
/// Base class that allows to test the generator.
/// </summary>
public class BaseGeneratorTestClass {

	// list of the defines for each platform, this is passed to the parser to ensure that
	// we are testing the platforms as if they were being compiled.
	readonly Dictionary<TargetFramework, string []> platformDefines = new () {
		{ TargetFramework.DotNet_iOS, new [] { "__IOS__" } },
		{ TargetFramework.DotNet_tvOS, new [] { "__TVOS__" } },
		{ TargetFramework.DotNet_macOS, new [] { "__MACOS__" } },
		{ TargetFramework.DotNet_MacCatalyst, new [] { "__MACCATALYST__" } },
	};

	/// <summary>
	/// Returns the name of the class in the global namespace if the generator configuration has set to do so. If
	/// not, the same string will be returned.
	/// </summary>
	/// <param name="className">The class that should have global prepend to it.</param>
	/// <param name="isGlobal">If global should be used.</param>
	/// <returns>A modified class name with the global alias if needed.</returns>
	public static string Global (string className, bool isGlobal = GeneratorConfiguration.UseGlobalNamespace)
	{
		return isGlobal ? $"global::{className}" : className;
	}

	protected Compilation RunGeneratorsAndUpdateCompilation (CSharpGeneratorDriver driver, Compilation compilation, out ImmutableArray<Diagnostic> diagnostics)
	{
		driver.RunGeneratorsAndUpdateCompilation (compilation, out var updatedCompilation, out diagnostics);
		return updatedCompilation;
	}

	GeneratorDriverRunResult RunGenerators (CSharpGeneratorDriver driver, Compilation compilation)
		=> driver.RunGenerators (compilation).GetRunResult ();

	protected IEnumerable<string> GetPlatformDefines (TargetFramework targetFramework)
	{
		if (Configuration.TryGetPlatformPreprocessorSymbolsRsp (targetFramework, out var rspFile)) {
			var args = new [] { $"@{rspFile}" };
			var workingDirectory = Path.Combine (Configuration.SourceRoot, "src");
			var parseResult = CSharpCommandLineParser.Default.Parse (
				args, null, null);
			var frameworkDefines = parseResult.ParseOptions.PreprocessorSymbolNames.ToList ();
			// add the platform ones that are not in this rsp
			frameworkDefines.AddRange (platformDefines [targetFramework]);
			return frameworkDefines;
		}

		return [];
	}

	protected CompilationResult CreateCompilation (ApplePlatform platform, [CallerMemberName] string name = "", params string [] sources)
	{
		// get the dotnet bcl and fully load it for the test.
		var references = Directory.GetFiles (Configuration.DotNetBclDir, "*.dll")
			.Select (assembly => MetadataReference.CreateFromFile (assembly)).ToList ();
		// get the dll for the current platform
		var targetFramework = TargetFramework.GetTargetFramework (platform);
		// get the platform definitions
		var preprocessorSymbols = GetPlatformDefines (targetFramework);
		var platformDll = Configuration.GetBaseLibrary (targetFramework);
		if (!string.IsNullOrEmpty (platformDll)) {
			references.Add (MetadataReference.CreateFromFile (platformDll));
		} else {
			throw new InvalidOperationException ($"Could not find platform dll for {platform}");
		}

		var parseOptions = new CSharpParseOptions (LanguageVersion.Latest, DocumentationMode.None, preprocessorSymbols: preprocessorSymbols);
		var trees = sources.Select (s => CSharpSyntaxTree.ParseText (s, parseOptions)).ToImmutableArray ();

		var options = new CSharpCompilationOptions (OutputKind.NetModule)
			.WithAllowUnsafe (true);

		return new (CSharpCompilation.Create (name, trees, references, options), trees);
	}

	readonly static Lock uiNamespaceLock = new ();
	protected void CompareGeneratedCode (GenerationTestData testData)
	{
		lock (uiNamespaceLock) {
			var driver = CSharpGeneratorDriver.Create (new BindingSourceGeneratorGenerator ());
			// We need to create a compilation with the required source code.
			var (compilation, _) = CreateCompilation (testData.Platform, sources: testData.InputText);
			// for the refresh of the namespaces, this is needed to make sure that the generator does not get confused
			// when several compilations are running
			compilation.GetUINamespaces (force: true);

			// Run generators and retrieve all results.
			var runResult = RunGenerators (driver, compilation);

			// All generated files can be found in 'RunResults.GeneratedTrees'.
			var generatedFileSyntax = runResult.GeneratedTrees.Where (t => t.FilePath.EndsWith ($"{testData.ClassName}.g.cs")).ToArray ();
			Assert.Single (generatedFileSyntax);

			// Complex generators should be tested using text comparison.
			Assert.Equal (testData.ExpectedOutputText, generatedFileSyntax [0].GetText ().ToString (),
				ignoreLineEndingDifferences: true);

			if (testData.ExpectedLibraryText is not null) {
				// validate that Library.g.cs was created by the LibraryEmitter and matches the expectation
				var generatedLibSyntax = runResult.GeneratedTrees.Single (t => t.FilePath.EndsWith ("Libraries.g.cs"));
				Assert.Equal (testData.ExpectedLibraryText, generatedLibSyntax.GetText ().ToString ());
			}

			if (testData.ExpectedTrampolineText is not null) {
				// validate that Library.g.cs was created by the LibraryEmitter and matches the expectation
				var generatedLibSyntax = runResult.GeneratedTrees.Single (t => t.FilePath.EndsWith ("Trampolines.g.cs"));
				Assert.Equal (testData.ExpectedTrampolineText, generatedLibSyntax.GetText ().ToString ());
			}
		}

	}
}
