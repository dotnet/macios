// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

public abstract class BaseClass {
	public void AssertPrepare (AssemblyPreparer preparer)
	{
		if (!preparer.Prepare (out var exceptions)) {
			foreach (var ex in exceptions) {
				Console.WriteLine (ex.ToString ());
				if (ex.InnerException is not null)
					Console.WriteLine ($"  Inner: {ex.InnerException}");
			}
			Assert.Fail ($"Prepare failed, exceptions:\n\t{string.Join ("\n\t", exceptions.Select (v => v.ToString ()))}");
		}
		Assert.That (exceptions, Is.Empty, "Exceptions");
	}

	public bool AssertPrepare (ApplePlatform platform, bool isCoreCLR, string code, out AssemblyDefinition assemblyDefinition)
	{
		return AssertPrepare (platform, isCoreCLR, RegistrarMode.Dynamic, code, out assemblyDefinition);
	}

	// returns true if the test assembly was modified
	public bool AssertPrepare (ApplePlatform platform, bool isCoreCLR, RegistrarMode registrar, string code, out AssemblyDefinition assemblyDefinition)
	{
		AssemblyPreparer? preparer = null;
		var rv = AssertPrepareCode (platform, isCoreCLR, p => {
			p.Registrar = registrar;
			preparer = p;
		}, code, out string outputPath);
		var resolver = new DefaultAssemblyResolver ();
		var dirs = preparer!.Assemblies.Select (v => Path.GetDirectoryName (v.OutputPath)).Distinct ().ToList ();
		dirs.ForEach (v => resolver.AddSearchDirectory (v));
		var readerParameters = new ReaderParameters {
			ReadSymbols = true,
			AssemblyResolver = resolver,
		};
		assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath, readerParameters);
		return rv;
	}

	// returns true if the test assembly was modified
	public bool AssertPrepareCode (ApplePlatform platform, bool isCoreCLR, Action<AssemblyPreparer>? configure, string code, out string outputPath)
	{
		Configuration.IgnoreIfIgnoredPlatform (platform);

		var csproj = $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>net$(BundledNETCoreAppTargetFrameworkVersion)-{platform.AsString ().ToLower ()}</TargetFramework>
        <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
		<UseFloatingTargetPlatformVersion>true</UseFloatingTargetPlatformVersion>
	</PropertyGroup>
</Project>
    ";

		var tmpdir = Xamarin.Cache.CreateTemporaryDirectory ();

		var config = $@"
		AreAnyAssembliesTrimmed=true
		PublishTrimmed=true
		IntermediateOutputPath={Path.Combine (tmpdir, "intermediate")}
		Platform={platform.AsString ()}
		PlatformAssembly=Microsoft.{platform.AsString ()}.dll
		SdkDevPath={Configuration.XcodeLocation}
		SdkVersion={Configuration.GetSdkVersion (platform)}
		TargetFramework={TargetFramework.GetTargetFramework (platform)}
		";
		var configpath = Path.Combine (tmpdir, "config.txt");
		File.WriteAllText (configpath, config);

		File.WriteAllText (Path.Combine (tmpdir, "Test.cs"), code);
		var csprojPath = Path.Combine (tmpdir, "Test.csproj");
		File.WriteAllText (csprojPath, csproj);
		var properties = new Dictionary<string, string> {
			{ "TreatWarningsAsErrors", "false" },
		};
		DotNet.AssertBuild (csprojPath, properties);
		var assemblyDir = Path.Combine (tmpdir, "bin", "Debug");

		var assemblies = Configuration.GetImplementationAssemblies (platform, isCoreCLR);
		assemblies.Add (Path.Combine (assemblyDir, "Test.dll"));
		var infos = assemblies.Select (v => new AssemblyPreparerInfo (v, Path.Combine (assemblyDir, "out", Path.GetFileName (v)), true, "link")).ToArray ();
		var logger = new TestLogger () { Platform = platform };
		var preparer = new AssemblyPreparer (logger, infos, configpath);
		if (configure is not null)
			configure (preparer);
		AssertPrepare (preparer);

		var testInfo = infos.Single (v => Path.GetFileNameWithoutExtension (v.InputPath) == "Test");
		outputPath = testInfo.OutputPath;
		Console.WriteLine ("Output assembly: " + outputPath);
		preparer.Dispose ();
		return testInfo.InputPath != testInfo.OutputPath;
	}
}
