using System;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Microsoft.Build.Logging.StructuredLogger;

namespace Xamarin.Tests {
	public class XcodeProjectTests : TestBaseClass {

		readonly string XCodeTestProjectDir = Path.Combine (Configuration.SourceRoot, "tests", "common", "TestProjects", "Xcode");

		void AddXcodeProjectItem (string project, string path, Dictionary<string, string> metadata)
		{
			string xcProjItem = $@"
<ItemGroup>
  <XcodeProject Include=""{path}"">
    {string.Join ("\n", metadata.Select (v => $"<{v.Key}>{v.Value}</{v.Key}>"))}
  </XcodeProject>
</ItemGroup>
";
			var existingProjContent = File.ReadAllText (project);
			var newProjContent = existingProjContent.Replace ("</Project>", xcProjItem + "</Project>");
			File.WriteAllText (project, newProjContent);
		}

		void AssertXcFrameworkOutput (ApplePlatform platform, string testDir, string xcodeProjName, string config = "Debug")
		{
			if (!UsesCompressedBindingResourcePackage (platform)) {
				var expectedXcodeFxOutput = Path.Combine (testDir, "bin", config, platform.ToFramework (), $"{TestName}.resources", $"{xcodeProjName}{platform.AsString ()}.xcframework");
				Assert.That (expectedXcodeFxOutput, Does.Exist, $"Expected xcframework output '{expectedXcodeFxOutput}' did not exist.");
			} else {
				var resourcesZip = Path.Combine (testDir, "bin", config, platform.ToFramework (), $"{TestName}.resources.zip");
				Assert.That (ZipHelpers.List (resourcesZip), Does.Contain ($"{xcodeProjName}{platform.AsString ()}.xcframework/Info.plist"), $"Expected xcframework output was not found in '{resourcesZip}'.");
			}
		}


		[Test]
		[TestCase ("Debug", "iossimulator-arm64")]
		[TestCase ("Release", "ios-arm64")]
		public void BuildAppiOS (string projConfig, string rid)
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "ios");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var projProps = GetDefaultProperties (runtimeIdentifiers: rid);
			projProps ["Configuration"] = projConfig;
			DotNet.AssertBuild (proj, properties: projProps);
			var appDir = Path.Combine (testDir, "bin", projConfig, platform.ToFramework (), rid, $"{TestName}.app");
			Assert.That (appDir, Does.Exist, $"Expected app dir '{appDir}' did not exist.");
			var appContent = Directory.GetFiles (appDir, "*", SearchOption.AllDirectories);
			var expectedAppOutput = Path.Combine (testDir, "bin", projConfig, platform.ToFramework (), rid, $"{TestName}.app", "Frameworks", $"{xcodeProjName}.framework", "Info.plist");
			Assert.That (appContent, Does.Contain (expectedAppOutput), $"Expected framework output '{expectedAppOutput}' did not exist.");

		}

		[Test]
		[TestCase ("Release", "Release", false)]
		[TestCase ("Debug", "Relase", true)]
		[TestCase ("Release", "Debug", true)]
		public void BuildBindingiOS (string projConfig, string fxConfig, bool createNativeReference)
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "iosbinding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "Configuration", fxConfig },
					{ "CreateNativeReference", createNativeReference.ToString () },
					{ "SchemeName", xcodeProjName },
				});

			var projProps = GetDefaultProperties ();
			projProps ["Configuration"] = projConfig;
			DotNet.AssertBuild (proj, properties: projProps);
			string expectedXcodeFxOutput;
			if (UsesCompressedBindingResourcePackage (platform)) {
				expectedXcodeFxOutput = Path.Combine (testDir, "bin", projConfig, platform.ToFramework (), $"{TestName}.resources.zip");
			} else {
				expectedXcodeFxOutput = Path.Combine (testDir, "bin", projConfig, platform.ToFramework (), $"{TestName}.resources", $"{xcodeProjName}{platform.AsString ()}.xcframework");
			}
			Assert.That (expectedXcodeFxOutput, createNativeReference ? Does.Exist : Does.Not.Exist, $"Expected xcframework output '{expectedXcodeFxOutput}' to exist when CreateNativeReference=true.");
		}

		[Test]
		[TestCase (true)]
		[TestCase (false)]
		public void WindowsRemoteBuildSelection (bool hasXcodeProject)
		{
			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, "RemoteBuildSelection.proj");
			File.WriteAllText (proj, $@"
<Project>
  <PropertyGroup>
    <_TaskAssemblyName>$(MSBuildToolsPath)/Microsoft.Build.Tasks.Core.dll</_TaskAssemblyName>
    <_TaskAssemblyFileNameWindows>$(_TaskAssemblyName)</_TaskAssemblyFileNameWindows>
    <BundleOriginalResources>true</BundleOriginalResources>
    <_BundleOriginalResources>true</_BundleOriginalResources>
    <_CanOutputAppBundle>false</_CanOutputAppBundle>
    <OutputType>Library</OutputType>
  </PropertyGroup>
  <ItemGroup Condition=""'$(HasXcodeProject)' == 'true'"">
    <XcodeProject Include=""TemplateFx.xcodeproj"" />
  </ItemGroup>
  <Import Project=""{Configuration.SourceRoot}/msbuild/Xamarin.Shared/Xamarin.Shared.targets"" />
  <Import Project=""{Configuration.SourceRoot}/msbuild/Xamarin.iOS.Tasks.Windows/Xamarin.iOS.Common.After.targets"" />
  <Target Name=""_SayHello"">
    <PropertyGroup>
      <_ConnectionAttempted>true</_ConnectionAttempted>
    </PropertyGroup>
  </Target>
  <Target Name=""Verify"" DependsOnTargets=""EvaluateRemoteBuild;_DetectBuildType;EnsureMacConnection"">
    <Error Condition=""'$(IsRemoteBuild)' != '$(HasXcodeProject)'"" Text=""Unexpected remote build selection: $(IsRemoteBuild)"" />
    <Error Condition=""'$(HasXcodeProject)' == 'true' And '$(_ConnectionAttempted)' != 'true'"" Text=""Missing connection attempt."" />
    <Error Condition=""'$(HasXcodeProject)' == 'false' And '$(_ConnectionAttempted)' == 'true'"" Text=""Unexpected connection attempt."" />
  </Target>
</Project>
");
			var properties = GetDefaultProperties (includeRemoteProperties: false);
			properties ["HasXcodeProject"] = hasXcodeProject.ToString ();
			DotNet.AssertBuild (proj, properties, target: "Verify");
		}

		[Test]
		[Category ("RemoteWindows")]
		public void BuildBindingFromWindows ()
		{
			Configuration.IgnoreIfNotOnWindows ();
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "iosbinding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDir = Path.Combine (testDir, "Xcode project");
			FileHelpers.CopyDirectory (Path.Combine (XCodeTestProjectDir, xcodeProjName), xcodeProjDir);
			AddXcodeProjectItem (proj, Path.Combine ("Xcode project", $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var properties = GetDefaultProperties (includeRemoteProperties: true);
			properties ["CompressBindingResourcePackage"] = "true";
			var rv = DotNet.AssertBuild (proj, properties);
			AssertTargetExecuted (BinLog.GetAllTargets (rv.BinLogPath), "_BuildXcodeProjects", "Initial Xcode build");
			AssertXcFrameworkOutput (ApplePlatform.iOS, testDir, xcodeProjName);
			var resourcePackage = Path.Combine (testDir, "bin", "Debug", ApplePlatform.iOS.ToFramework (), $"{TestName}.resources.zip");
			using (var archive = System.IO.Compression.ZipFile.OpenRead (resourcePackage)) {
				var binary = archive.GetEntry ($"{xcodeProjName}ios.xcframework/ios-arm64/{xcodeProjName}.framework/{xcodeProjName}");
				Assert.That (binary, Is.Not.Null, "Framework binary copied back to Windows");
				Assert.That (binary?.Length, Is.GreaterThan (0), "Framework binary must not be an empty placeholder");
			}

			rv = DotNet.AssertBuild (proj, properties);
			AssertTargetNotExecuted (BinLog.GetAllTargets (rv.BinLogPath), "_BuildXcodeProjects", "Unchanged Xcode project");
			AssertXcFrameworkOutput (ApplePlatform.iOS, testDir, xcodeProjName);

			var source = Directory.GetFiles (xcodeProjDir, "*.swift", SearchOption.AllDirectories).First ();
			File.AppendAllText (source, "\n");
			rv = DotNet.AssertBuild (proj, properties);
			var targets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (targets, "_BuildXcodeProjects", "Changed Xcode source");
			AssertTargetExecuted (targets, "_CreateBindingResourcePackage", "Changed binding resources");
			AssertXcFrameworkOutput (ApplePlatform.iOS, testDir, xcodeProjName);

			rv = DotNet.AssertBuild (proj, properties, target: "Clean");
			AssertTargetExecuted (BinLog.GetAllTargets (rv.BinLogPath), "_CleanXcodeProjects", "Clean Xcode project");
			Assert.That (resourcePackage, Does.Not.Exist, "Clean binding resources");

			DotNet.AssertBuild (proj, properties);
			AssertXcFrameworkOutput (ApplePlatform.iOS, testDir, xcodeProjName);
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void BuildBinding (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, $"{platform.AsString ().ToLower ()}binding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			// Add API definition to expose the API from the xcodeproj
			var apiDefinition = @"
using Foundation;
namespace XcProjBinding;
[BaseType (typeof(NSObject))]
interface SwiftBindTest {
	[Static,Export (""getStringWithMyString:"")]
	string GetString (string myString);
}";
			File.WriteAllText (Path.Combine (testDir, "ApiDefinition.cs"), apiDefinition);

			// Add class file to ensure ensure the xcframework from our xcodeproj was built and bound
			string classContent = @"
public class Binding
{
	public void BindTest () { Console.WriteLine (XcProjBinding.SwiftBindTest.GetString(""test"")); }
}
";
			File.WriteAllText (Path.Combine (testDir, "Binding.cs"), classContent);

			var properties = GetDefaultProperties ();
			var rv = DotNet.AssertBuild (proj, properties);
			var warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath).FilterWarnings (platform).Select (v => v.Message);
			Assert.That (warnings, Is.Empty, $"Build warnings:\n\t{string.Join ("\n\t", warnings)}");
			AssertXcFrameworkOutput (platform, testDir, xcodeProjName);
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void PackBinding (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, $"{platform.AsString ().ToLower ()}binding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var properties = GetDefaultProperties ();
			DotNet.AssertPack (proj, properties);
			var expectedNupkgOutput = Path.Combine (testDir, "bin", "Release", $"{TestName}.1.0.0.nupkg");
			Assert.That (expectedNupkgOutput, Does.Exist, $"Expected pack output '{expectedNupkgOutput}' did not exist.");

			List<string> zipContent = ZipHelpers.List (expectedNupkgOutput);
			var tfm = platform.ToFrameworkWithPlatformVersion (isExecutable: false);
			var expectedFxPath = $"lib/{tfm}/{TestName}.resources/{xcodeProjName}{platform.AsString ()}.xcframework/Info.plist";
			if (UsesCompressedBindingResourcePackage (platform)) {
				zipContent = ZipHelpers.ListInnerZip (expectedNupkgOutput, $"lib/{tfm}/{TestName}.resources.zip");
				expectedFxPath = $"{xcodeProjName}{platform.AsString ()}.xcframework/Info.plist";
			}
			Assert.That (zipContent, Does.Contain (expectedFxPath), $"Expected xcframework output was not found in '{expectedNupkgOutput}'.");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void BuildIncremental (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, $"{platform.AsString ().ToLower ()}binding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			var xcodeProjPath = Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, xcodeProjPath,
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			// Build the first time
			var properties = GetDefaultProperties ();
			var rv = DotNet.AssertBuild (proj, properties);
			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_BuildXcodeProjects", "First _BuildXcodeProjects");
			var expectedXcodeFxOutput = Path.Combine (testDir, "bin", "Debug", platform.ToFramework (), $"{TestName}.resources", $"{xcodeProjName}{platform.AsString ().ToLower ()}.xcframework");
			if (UsesCompressedBindingResourcePackage (platform))
				expectedXcodeFxOutput = Path.Combine (testDir, "bin", "Debug", platform.ToFramework (), $"{TestName}.resources.zip");
			Assert.That (expectedXcodeFxOutput, Does.Exist, $"Expected xcframework output '{expectedXcodeFxOutput}' did not exist.");
			var outputFxFirstWriteTime = File.GetLastWriteTime (expectedXcodeFxOutput);

			// Build again, _BuildXcodeProjects should be skipped and outputs should not be updated
			rv = DotNet.AssertBuild (proj, properties);
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetNotExecuted (allTargets, "_BuildXcodeProjects", "Second _BuildXcodeProjects");
			Assert.That (expectedXcodeFxOutput, Does.Exist, $"Expected xcframework output '{expectedXcodeFxOutput}' did not exist.");
			var outputFxSecondWriteTime = File.GetLastWriteTime (expectedXcodeFxOutput);
			Assert.That (outputFxFirstWriteTime, Is.EqualTo (outputFxSecondWriteTime), $"Expected '{expectedXcodeFxOutput}' write time to be '{outputFxFirstWriteTime}', but was '{outputFxSecondWriteTime}'");

			// Update xcode project, _BuildXcodeProjects should run and outputs should be updated
			File.SetLastWriteTime (Path.Combine (xcodeProjPath, "project.pbxproj"), DateTime.Now);
			rv = DotNet.AssertBuild (proj, properties);
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_BuildXcodeProjects", "Third _BuildXcodeProjects");
			Assert.That (expectedXcodeFxOutput, Does.Exist, $"Expected xcframework output '{expectedXcodeFxOutput}' did not exist.");
			var outputFxThirdWriteTime = File.GetLastWriteTime (expectedXcodeFxOutput);
			Assert.That (outputFxThirdWriteTime, Is.GreaterThan (outputFxFirstWriteTime), $"Expected '{expectedXcodeFxOutput}' third write time '{outputFxThirdWriteTime}' to be greater than first write '{outputFxFirstWriteTime}'");
		}

		[Test]
		public void BuildMultipleSchemesiOS ()
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "iosbinding");

			var xcodeProjName = "TwoSchemeFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var secondSchemeName = "iOSFx";
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", secondSchemeName },
				});

			var properties = GetDefaultProperties ();
			DotNet.AssertBuild (proj, properties);
			AssertXcFrameworkOutput (platform, testDir, xcodeProjName);
			AssertXcFrameworkOutput (platform, testDir, secondSchemeName);
		}

		[Test]
		public void BuildMultipleProjectsiOS ()
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "iosbinding");

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var xcodeProjName2 = "TwoSchemeFx";
			var xcodeProjDirSrc2 = Path.Combine (XCodeTestProjectDir, xcodeProjName2);
			var xcodeProjDirDest2 = Cache.CreateTemporaryDirectory ($"{TestName}XcProjTwo");
			FileHelpers.CopyDirectory (xcodeProjDirSrc2, xcodeProjDirDest2);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest2, $"{xcodeProjName2}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName2 },
				});

			var properties = GetDefaultProperties ();
			DotNet.AssertBuild (proj, properties);
			AssertXcFrameworkOutput (platform, testDir, xcodeProjName);
			AssertXcFrameworkOutput (platform, testDir, xcodeProjName2);
		}

		[Test]
		[Category ("Multiplatform")]
		public void BuildMultiTargeting ()
		{
			var enabledPlatforms = Configuration.GetIncludedPlatforms ();
			var templatePlatform = enabledPlatforms.First ();
			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, $"{templatePlatform.AsString ().ToLower ()}binding");

			var tfxs = $"<TargetFrameworks>{string.Join (";", enabledPlatforms.Select (p => p.ToFramework ()))}</TargetFrameworks>";
			var existingProjContent = File.ReadAllText (proj);
			var newProjContent = existingProjContent.Replace ($"<TargetFramework>{templatePlatform.ToFramework ()}</TargetFramework>", tfxs);
			File.WriteAllText (proj, newProjContent);
			Assert.That (File.ReadAllText (proj), Does.Contain (tfxs));

			var xcodeProjName = "TemplateFx";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);
			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var properties = GetDefaultProperties ();
			DotNet.AssertBuild (proj, properties);
			foreach (var platform in enabledPlatforms) {
				AssertXcFrameworkOutput (platform, testDir, xcodeProjName);
			}
		}

		[Test]
		public void InvalidItemErroriOS ()
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "ioslib");

			var xcodeProjName = "TemplateFx";
			var invalidXcodeProjPath = Path.Combine (testDir, "invalid", "path", $"{xcodeProjName}.xcodeproj");
			AddXcodeProjectItem (proj, invalidXcodeProjPath,
				new Dictionary<string, string> {
					{ "SchemeName", xcodeProjName },
				});

			var properties = GetDefaultProperties ();
			var rv = DotNet.AssertBuildFailure (proj, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			var expectedError = $"The Xcode project item: '{invalidXcodeProjPath}' could not be found. Please update the 'Include' value to a path containing a valid '.xcodeproj' file.";
			AssertErrorMessages (errors, expectedError);
		}

		[Test]
		public void InvalidSchemeErroriOS ()
		{
			var platform = ApplePlatform.iOS;
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var testDir = Cache.CreateTemporaryDirectory (TestName);
			var proj = Path.Combine (testDir, $"{TestName}.csproj");
			DotNet.AssertNew (testDir, "ioslib");

			var xcodeProjName = "TemplateFx";
			var invaldSchemeName = "InvalidScheme";
			var xcodeProjDirSrc = Path.Combine (XCodeTestProjectDir, xcodeProjName);
			var xcodeProjDirDest = Cache.CreateTemporaryDirectory ($"{TestName}XcProj");
			FileHelpers.CopyDirectory (xcodeProjDirSrc, xcodeProjDirDest);

			AddXcodeProjectItem (proj, Path.Combine (xcodeProjDirDest, $"{xcodeProjName}.xcodeproj"),
				new Dictionary<string, string> {
					{ "SchemeName", invaldSchemeName },
				});

			var properties = GetDefaultProperties ();
			var rv = DotNet.AssertBuildFailure (proj, properties);
			var expectedErrorContent = $"xcodebuild: error: The project named \"{xcodeProjName}\" does not contain a scheme named \"{invaldSchemeName}\".";
			var errors = BinLog.GetBuildMessages (rv.BinLogPath).ToArray ();
			var anyExpectedError = errors.Where (v => v.Message?.Contains (expectedErrorContent) == true);
			Assert.That (anyExpectedError.Any (), Is.True, "Expected error");
		}

	}
}
