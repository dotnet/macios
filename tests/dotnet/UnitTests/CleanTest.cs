// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Xamarin.Tests {
	[TestFixture]
	public class CleanTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64;osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64;maccatalyst-arm64")]
		public void App (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GenerateProject (platform, TestName, runtimeIdentifiers, out var appPath);
			var projectDirectory = Path.GetDirectoryName (projectPath) ?? throw new InvalidOperationException (projectPath);
			File.WriteAllText (Path.Combine (projectDirectory, "Main.cs"), "class MainClass { static int Main () => 0; }\n");
			File.WriteAllText (projectPath, File.ReadAllText (projectPath).Replace ("</Project>", """
				<Target Name="WriteLateCleanFile" AfterTargets="_CreateAppBundle;_CreateMergedAppBundle">
					<WriteLinesToFile File="$(OutputPath)late-clean.cache" Lines="Native output" Overwrite="true" WriteOnlyWhenDifferent="true" />
					<ItemGroup>
						<FileWrites Include="$(OutputPath)late-clean.cache" />
					</ItemGroup>
				</Target>
				</Project>
				""") + "\n");
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCodeSigning"] = "false";
			if (platform == ApplePlatform.MacOSX) {
				properties ["CreatePackage"] = "true";
				properties ["EnablePackageSigning"] = "false";
			}

			DotNet.AssertBuild (projectPath, properties);
			Assert.That (appPath, Does.Exist, "App bundle before clean");

			var binDirectory = GetBinDir (projectPath, platform, runtimeIdentifiers);
			var objDirectory = GetObjDir (projectPath, platform, runtimeIdentifiers);
			if (platform == ApplePlatform.MacOSX)
				Assert.That (Directory.GetFiles (binDirectory, "*.pkg"), Is.Not.Empty, "Installer package before clean");
			var nativeCache = Path.Combine (binDirectory, runtimeIdentifiers.Contains (';') ? runtimeIdentifiers.Split (';') [0] : "", "NativeLink.cache");
			Assert.That (nativeCache, Does.Exist, "Native link cache");
			var lateCleanFile = Path.Combine (binDirectory, "late-clean.cache");
			Assert.That (lateCleanFile, Does.Exist, "Late native output");
			var cacheTimestamp = new DateTime (2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			File.SetLastWriteTimeUtc (lateCleanFile, cacheTimestamp);
			DotNet.AssertBuild (projectPath, properties);
			Assert.That (File.GetLastWriteTimeUtc (lateCleanFile), Is.EqualTo (cacheTimestamp), "Incremental build preserves late FileWrites");
			var cleanFile = Path.Combine (objDirectory, Path.GetFileName (projectPath) + ".FileListAbsolute.txt");
			Assert.That (File.ReadAllLines (cleanFile), Does.Contain (lateCleanFile), "Late FileWrites are recorded for clean");

			var preservedFiles = new [] {
				Path.Combine (projectDirectory, "obj", "project.assets.json"),
				Path.Combine (projectDirectory, "bin", "Release", platform.ToFramework (), "preserve.txt"),
				Path.Combine (projectDirectory, "obj", "Release", platform.ToFramework (), "preserve.txt"),
				Path.Combine (projectDirectory, "bin", "Debug", "net10.0", "preserve.txt"),
				Path.Combine (projectDirectory, "obj", "Debug", "net10.0", "preserve.txt"),
			};
			foreach (var file in preservedFiles.Skip (1)) {
				Directory.CreateDirectory (Path.GetDirectoryName (file) ?? throw new InvalidOperationException (file));
				File.WriteAllText (file, "Preserve other configurations and target frameworks.\n");
			}

			DotNet.Execute ("clean", projectPath, properties);
			AssertCleanDirectory (binDirectory);
			AssertCleanDirectory (objDirectory);
			foreach (var file in preservedFiles)
				Assert.That (file, Does.Exist, "Clean preserves " + file);

			DotNet.Execute ("clean", projectPath, properties);
			AssertCleanDirectory (binDirectory);
			AssertCleanDirectory (objDirectory);
			DotNet.AssertBuild (projectPath, properties);
			Assert.That (appPath, Does.Exist, "Build after clean");
			if (runtimeIdentifiers.Contains (';')) {
				properties.Remove ("RuntimeIdentifiers");
				DotNet.Execute ("clean", projectPath, properties);
				AssertCleanDirectory (binDirectory);
				AssertCleanDirectory (objDirectory);
			}
		}

		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.TVOS, true)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.MacOSX, true)]
		[TestCase (ApplePlatform.MacCatalyst, false)]
		[TestCase (ApplePlatform.MacCatalyst, true)]
		public void BindingResourcePackage (ApplePlatform platform, bool compressed)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			var projectPath = GetProjectPath ("BindingWithCompressedXCFramework", platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties ();
			properties ["CompressBindingResourcePackage"] = compressed ? "true" : "false";
			DotNet.AssertBuild (projectPath, properties);
			var binDirectory = GetBinDir (projectPath, platform, "");
			var objDirectory = GetObjDir (projectPath, platform, "");
			var resourcePath = Path.Combine (binDirectory, "BindingWithCompressedXCFramework.resources");
			Assert.That (compressed ? resourcePath + ".zip" : resourcePath, Does.Exist, "Binding resources before clean");
			DotNet.Execute ("clean", projectPath, properties);
			AssertCleanDirectory (binDirectory);
			AssertCleanDirectory (objDirectory);
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64", false)]
		[TestCase (ApplePlatform.iOS, "ios-arm64", true)]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64", false)]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64", true)]
		[TestCase (ApplePlatform.MacOSX, "osx-x64", false)]
		[TestCase (ApplePlatform.MacOSX, "osx-x64", true)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64", false)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64", true)]
		public void DefaultRuntimeIdentifier (ApplePlatform platform, string runtimeIdentifier, bool buildingInsideVisualStudio)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			if (runtimeIdentifier == GetDefaultRuntimeIdentifier (platform))
				runtimeIdentifier = runtimeIdentifier.Replace ("-x64", "-arm64");
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GenerateProject (platform, TestName, runtimeIdentifier, out _);
			File.WriteAllText (projectPath, File.ReadAllText (projectPath).Replace ("</Project>", """
				<Target Name="CheckBeforeClean" BeforeTargets="BeforeClean" Condition="'$(_CleaningRidSpecificBuild)' != 'true'">
					<Error Condition="!Exists('$(CleanRegressionOutputFile)')" Text="RID-specific cleanup ran before BeforeClean." />
				</Target>
				</Project>
				""") + "\n");
			var properties = GetDefaultProperties (runtimeIdentifier);
			var binDirectory = GetBinDir (projectPath, platform, runtimeIdentifier);
			var objDirectory = GetObjDir (projectPath, platform, runtimeIdentifier);
			Directory.CreateDirectory (binDirectory);
			Directory.CreateDirectory (objDirectory);
			var outputFile = Path.Combine (binDirectory, "native-output.cache");
			var intermediateFile = Path.Combine (objDirectory, "native-object.o");
			properties ["CleanRegressionOutputFile"] = outputFile;
			File.WriteAllText (outputFile, "Native output\n");
			File.WriteAllText (intermediateFile, "Native intermediate\n");
			File.WriteAllText (Path.Combine (objDirectory, Path.GetFileName (projectPath) + ".FileListAbsolute.txt"), outputFile + "\n" + intermediateFile + "\n");

			properties ["RuntimeIdentifier"] = GetDefaultRuntimeIdentifier (platform);
			DotNet.Execute ("clean", projectPath, properties);
			Assert.That (outputFile, Does.Exist, "Explicit RID clean preserves other runtime outputs");
			Assert.That (intermediateFile, Does.Exist, "Explicit RID clean preserves other runtime intermediates");

			properties.Remove ("RuntimeIdentifier");
			properties ["BuildingInsideVisualStudio"] = buildingInsideVisualStudio ? "true" : "false";
			if (buildingInsideVisualStudio) {
				var platformName = platform switch {
					ApplePlatform.iOS => "iOS",
					ApplePlatform.TVOS => "tvOS",
					ApplePlatform.MacOSX => "macOS",
					ApplePlatform.MacCatalyst => "MacCatalyst",
					_ => throw new ArgumentOutOfRangeException (nameof (platform)),
				};
				properties [$"_{platformName}RuntimeIdentifier"] = GetDefaultRuntimeIdentifier (platform);
			}
			DotNet.Execute ("clean", projectPath, properties);
			AssertCleanDirectory (binDirectory);
			AssertCleanDirectory (objDirectory);
		}

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void UnrecordedNativeOutputs (ApplePlatform platform, string runtimeIdentifier)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);
			var projectPath = GenerateProject (platform, TestName, runtimeIdentifier, out _);
			var binDirectory = GetBinDir (projectPath, platform, runtimeIdentifier);
			Directory.CreateDirectory (binDirectory);
			foreach (var file in new [] { "LinkerInputs.cache", "LinkerInputs.cache.xml", "AOTCompileInputs.cache", "AOTCompileInputs.cache.uptodate", "NativeLink.cache", "NativeLink.cache.uptodate" })
				File.WriteAllText (Path.Combine (binDirectory, file), "Unrecorded native cache\n");
			File.WriteAllText (Path.Combine (binDirectory, platform == ApplePlatform.iOS || platform == ApplePlatform.TVOS ? "previous-build.ipa" : "previous-build.pkg"), "Unrecorded package\n");
			DotNet.Execute ("clean", projectPath, GetDefaultProperties (runtimeIdentifier));
			AssertCleanDirectory (binDirectory);
			AssertCleanDirectory (GetObjDir (projectPath, platform, runtimeIdentifier));
		}

		static void AssertCleanDirectory (string directory)
		{
			if (Directory.Exists (directory))
				Assert.That (Directory.GetFileSystemEntries (directory, "*", SearchOption.AllDirectories).Where (File.Exists), Is.Empty, directory);
		}
	}
}
