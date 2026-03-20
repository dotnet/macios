// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;
using ClangSharp.Interop;
using NUnit.Framework.Constraints;
using Xamarin;
using Xamarin.Tests;

namespace Sharpie.Bind.Tests;

public class OnDiskTests {
	static readonly string testsDir;

	static OnDiskTests ()
	{
		// from there we walk up the tree until we find the Tests directory
		testsDir = Environment.CurrentDirectory;

		while (!string.IsNullOrEmpty (testsDir)) {
			if (Directory.Exists (Path.Combine (testsDir, ".git")))
				break;

			var subdir = Path.Combine (testsDir, "Tests");
			if (Directory.Exists (subdir)) {
				testsDir = subdir;
				break;
			}

			testsDir = Path.GetDirectoryName (testsDir)!;
		}
	}

	public static IEnumerable<object []> GetRunTests ()
	{
		return GetTests (true);
	}

	public static IEnumerable<object []> GetNoRunTests ()
	{
		return GetTests (false);
	}


	public static IEnumerable<object []> GetTests (bool runTests = true)
	{
		var testsToRun = new HashSet<string> (Environment
			.GetEnvironmentVariable ("SHARPIE_PARSEBIND_TESTS")
			?.Split (new [] { ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
				?? Array.Empty<string> ());

		foreach (var file in Directory.EnumerateFiles (testsDir, "*.h", SearchOption.AllDirectories).OrderBy (v => v)) {
			var testsDirLength = testsDir.Length;
			var relativePath = file.Substring (testsDirLength + 1);

			var testCategory = Path.GetDirectoryName (relativePath);
			var testName = Path.GetFileNameWithoutExtension (relativePath);
			if (!string.IsNullOrEmpty (testCategory))
				testName = Path.Combine (testCategory, testName);

			if (testsToRun.Count > 0 && !testsToRun.Contains (testName))
				continue;

			var isRunTest = false;
			foreach (var line in File.ReadLines (file)) {
				var match = Regex.Match (line, @"\s*//\s*RUN(\s+[^:]+)?:(.*)$");
				isRunTest |= match.Success;
				if (match.Success && runTests)
					yield return new object [] {
						relativePath,
						match.Groups [1].Value.Trim (), // name
                        match.Groups [2].Value // bind arguments
                    };
			}

			if (runTests)
				continue;

			if (!runTests && isRunTest)
				continue;

			yield return new object []
			{
				relativePath,
				"",
			};
		}
	}

	[TestCaseSource (nameof (GetNoRunTests))]
	public void ParseNoRunTest (string path, string bindArguments)
	{
		ParseBindTestImpl (path, "", "-x objective-c");
	}

	[TestCaseSource (nameof (GetRunTests))]
	public void ParseRunTest (string path, string variant, string bindArguments)
	{
		ParseBindTestImpl (path, variant, bindArguments);
	}

	void ParseBindTestImpl (string path, string variant, string bindArguments)
	{
		var inputPath = Path.Combine (testsDir, path);
		var expectedOutputPath = Path.Combine (Path.GetDirectoryName (inputPath)!, Path.GetFileNameWithoutExtension (path));

		if (!string.IsNullOrEmpty (variant))
			expectedOutputPath += "." + variant.Replace (' ', '_');

		expectedOutputPath += ".cs";

		var binder = new BindTool ();
		binder.SplitDocuments = false;
		binder.DirectoriesInScope.Add (testsDir);

		var clangArguments = new List<string> ();
		var bindArgs = bindArguments.Split (' ', StringSplitOptions.RemoveEmptyEntries); // FIXME: better splitting
		for (var i = 0; i < bindArgs.Length; i++) {
			switch (bindArgs [i]) {
			case "-sdk":
				binder.Sdk = bindArgs [i + 1];
				i++;
				break;
			case "-namespace":
				binder.Namespace = bindArgs [i + 1];
				i++;
				break;
			case string s when s.StartsWith ("-massage="):
				var massagerName = s.Substring ("-massage=".Length);
				var enable = massagerName [0] == '+';
				binder.Massagers.Add ((massagerName.TrimStart ('+', '-'), enable));
				break;
			case "-massage":
				massagerName = bindArgs [i + 1];
				enable = massagerName.StartsWith ("+");
				binder.Massagers.Add ((massagerName.TrimStart ('+', '-'), enable));
				i++;
				break;
			default:
				clangArguments.Add (bindArgs [i]);
				break;
			}
		}

		var outputdir = Cache.CreateTemporaryDirectory ();
		var output = File.ReadAllText (expectedOutputPath);

		binder.OutputDirectory = outputdir;
		binder.SourceFile = inputPath;
		binder.ClangArguments.AddRange (clangArguments);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		var result = binder.BindInOrOut ();

		var tmpdir = Cache.CreateTemporaryDirectory ();
		var csharpCode = Path.Combine (tmpdir, "ApiDefinition.cs");
		File.WriteAllText (csharpCode, result.BindingCode);

		result.AssertSuccess (output,
			$"Input file: {inputPath}",
			$"Expected Output file: {expectedOutputPath}",
			$"Actual Output file: {binder.OutputApiDefinitionPath}");
		result.AssertNoWarnings ();
	}
}
