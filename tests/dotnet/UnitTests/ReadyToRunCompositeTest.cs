#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class ReadyToRunCompositeTest : TestBaseClass {
		const string EmptyAppManifest =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
<key>CFBundleIdentifier</key>
<string>ID</string>
</dict>
</plist>";

		const string EmptyMainFile =
@"using System;
using Foundation;

class MainClass {
	static void Main ()
	{
		Console.WriteLine (typeof (NSObject));
	}
}
";

		// Verify that Debug builds of CoreCLR iOS/tvOS/MacCatalyst apps default to a
		// composite R2R image rooted only on System.Private.CoreLib.dll, so the app
		// bundle contains exactly one per-app composite framework rather than the
		// upstream Microsoft.NETCore.App.r2r.* set or per-module .r2r.framework files.
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Debug_DefaultsToCoreLibOnlyCompositeRoot (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var configuration = "Debug";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["UseMonoRuntime"] = "false";

			var rv = DotNet.AssertBuild (project_path, properties);

			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_ConfigureCoreLibOnlyCompositeRoots", "default Debug CoreCLR build");
			AssertTargetExecuted (allTargets, "_DedupUnrootedReadyToRunPublish", "default Debug CoreCLR build");

			AssertCoreLibOnlyBundleComposition (platform, appPath, project);
		}

		// Verify the policy target skips itself when the user has already set
		// PublishReadyToRunCompositeRoots via their csproj/Directory.Build.props.
		// The dedup target still fires (it is decoupled from the policy decision
		// and exists purely to work around the NETSDK1152 asymmetry in
		// Microsoft.NET.CrossGen.targets).
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Debug_UserSetCompositeRoots_OptsOutOfDefault (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var csproj = $@"<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{platform.ToFramework ()}</TargetFramework>
		<OutputType>Exe</OutputType>
	</PropertyGroup>
	<ItemGroup>
		<PublishReadyToRunCompositeRoots Include=""System.Private.CoreLib.dll"" KeepDuplicates=""false"" />
		<PublishReadyToRunCompositeRoots Include=""System.Linq.dll"" KeepDuplicates=""false"" />
	</ItemGroup>
</Project>";

			var project_path = Path.Combine (tmpdir, "TestProject.csproj");
			File.WriteAllText (project_path, csproj);
			File.WriteAllText (Path.Combine (tmpdir, "Info.plist"), EmptyAppManifest);
			File.WriteAllText (Path.Combine (tmpdir, "Main.cs"), EmptyMainFile);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = "Debug";
			properties ["UseMonoRuntime"] = "false";

			var rv = DotNet.AssertBuild (project_path, properties);

			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetNotExecuted (allTargets, "_ConfigureCoreLibOnlyCompositeRoots", "user-set composite roots opt out");
			AssertTargetExecuted (allTargets, "_DedupUnrootedReadyToRunPublish", "dedup target stays active whenever composite roots are set");
		}

		// Verify Release CoreCLR builds are unchanged: the CoreLib-only policy target
		// is gated to Debug and must not fire in Release, where the full per-module
		// composite is still the intended behavior.
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Release_DoesNotRestrictCompositeRoots (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var configuration = "Release";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["UseMonoRuntime"] = "false";

			var rv = DotNet.AssertBuild (project_path, properties);

			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetNotExecuted (allTargets, "_ConfigureCoreLibOnlyCompositeRoots", "Release CoreCLR build must keep full per-module composite");
		}

		void AssertCoreLibOnlyBundleComposition (ApplePlatform platform, string appPath, string applicationName)
		{
			Assert.That (appPath, Does.Exist, "App bundle directory");

			// iOS/tvOS package the per-app composite as a <App>.r2r.framework under
			// Frameworks/; Mac Catalyst packages it as a <App>.r2r.dylib under
			// Contents/MonoBundle/ (see _CreateR2RModuleFrameworks vs
			// _CreateR2RModuleDylibs in Microsoft.Sdk.R2R.targets, and the
			// <Platform>-CoreCLR-R2R-size.txt baselines).
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				var frameworksDir = Path.Combine (appPath, GetFrameworksRelativePath (platform));
				Assert.That (frameworksDir, Does.Exist, "Frameworks directory");

				var r2rFrameworks = Directory.GetDirectories (frameworksDir, "*.r2r.framework");
				Assert.That (r2rFrameworks.Length, Is.EqualTo (1),
					$"Expected exactly one .r2r.framework (the per-app composite), found:\n  {string.Join ("\n  ", r2rFrameworks)}");

				var perAppFramework = applicationName + ".r2r.framework";
				Assert.That (Path.GetFileName (r2rFrameworks [0]), Is.EqualTo (perAppFramework),
					$"The single .r2r.framework should be the per-app composite ({perAppFramework})");

				// The upstream Microsoft.NETCore.App.r2r.framework must not be carried
				// into the bundle: every non-CoreLib BCL assembly was re-headered to
				// point at the per-app composite instead.
				var leakedNetCoreFrameworks = Directory.GetDirectories (frameworksDir, "Microsoft.NETCore.App.r2r*");
				Assert.That (leakedNetCoreFrameworks, Is.Empty,
					$"Bundle must not contain upstream Microsoft.NETCore.App.r2r framework(s):\n  {string.Join ("\n  ", leakedNetCoreFrameworks)}");

				// On iOS/tvOS the composite ships as a framework whose binary is
				// <App>.r2r (no extension), so no *.r2r.dylib should be present.
				var leakedR2RDylibs = Directory.GetFiles (appPath, "*.r2r.dylib", SearchOption.AllDirectories);
				Assert.That (leakedR2RDylibs, Is.Empty,
					$"Bundle must not contain any *.r2r.dylib files on iOS/tvOS:\n  {string.Join ("\n  ", leakedR2RDylibs)}");
				break;
			case ApplePlatform.MacCatalyst:
				var monoBundleDir = Path.Combine (appPath, "Contents", "MonoBundle");
				Assert.That (monoBundleDir, Does.Exist, "MonoBundle directory");

				var r2rDylibs = Directory.GetFiles (monoBundleDir, "*.r2r.dylib");
				Assert.That (r2rDylibs.Length, Is.EqualTo (1),
					$"Expected exactly one .r2r.dylib (the per-app composite), found:\n  {string.Join ("\n  ", r2rDylibs)}");

				var perAppDylib = applicationName + ".r2r.dylib";
				Assert.That (Path.GetFileName (r2rDylibs [0]), Is.EqualTo (perAppDylib),
					$"The single .r2r.dylib should be the per-app composite ({perAppDylib})");

				// The upstream Microsoft.NETCore.App.r2r.dylib must not be carried into
				// the bundle: every non-CoreLib BCL assembly was re-headered to point at
				// the per-app composite instead.
				var leakedNetCoreDylibs = Directory.GetFiles (monoBundleDir, "Microsoft.NETCore.App.r2r*");
				Assert.That (leakedNetCoreDylibs, Is.Empty,
					$"Bundle must not contain upstream Microsoft.NETCore.App.r2r dylib(s):\n  {string.Join ("\n  ", leakedNetCoreDylibs)}");
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (platform), platform, "Unsupported platform for CoreLib-only R2R bundle composition");
			}
		}
	}
}
