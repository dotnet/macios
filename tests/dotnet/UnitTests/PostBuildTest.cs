using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Mono.Cecil;

namespace Xamarin.Tests {
	[TestFixture]
	public class PostBuildTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void ArchiveTest (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["ArchiveOnBuild"] = "true";
			properties ["Configuration"] = configuration;

			var result = DotNet.AssertBuild (project_path, properties);
			var recordArgs = BinLog.ReadBuildEvents (result.BinLogPath).ToList ();
			var findString = "Output Property: ArchiveDir";
			var findString2 = "TaskOutput: ArchiveDir";
			var archiveDirs = recordArgs.
				Select (v => v?.Message).
				Select (v => {
					if (v is null)
						return null;
					if (v.Contains (findString))
						return v.Substring (findString.Length + 1);
					if (v.Contains (findString2))
						return v.Substring (findString2.Length + 1);
					return null;
				}).
				Where (v => v is not null).
				ToList ();
			Assert.That (archiveDirs.Count, Is.GreaterThan (0), "ArchiveDir");
			var archiveDir = archiveDirs [0]!.Trim ();
			Assert.That (archiveDir, Does.Exist, "Archive directory existence");
			AssertDSymDirectory (appPath);
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		public void BuildIpaTest (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["BuildIpa"] = "true";
			properties ["Configuration"] = configuration;

			DotNet.AssertBuild (project_path, properties);

			var pkgPath = Path.Combine (appPath, "..", $"{project}.ipa");
			Assert.That (pkgPath, Does.Exist, "pkg creation");

			AssertBundleAssembliesStripStatus (appPath, true);
			AssertDSymDirectory (appPath);
		}

		[Test]
		[TestCase ("MySimpleApp", ApplePlatform.iOS, "ios-arm64", true)]
		[TestCase ("MySimpleApp", ApplePlatform.iOS, "ios-arm64", false)]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.iOS, "ios-arm64", true)]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.iOS, "ios-arm64", false)]
		public void AssemblyStripping (string project, ApplePlatform platform, string runtimeIdentifiers, bool shouldStrip)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			// Force EnableAssemblyILStripping since we are building debug which never will by default
			properties ["EnableAssemblyILStripping"] = shouldStrip ? "true" : "false";

			DotNet.AssertBuild (project_path, properties);

			AssertBundleAssembliesStripStatus (appPath, shouldStrip);
			Assert.That (Path.Combine (appPath, $"{project}.dll"), Does.Exist, "Application Assembly");
			Assert.That (Path.Combine (appPath, "Microsoft.iOS.dll"), Does.Exist, "Platform Assembly");
		}

		[Test]
		[TestCase ("MySimpleApp", ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		public void DefaultAssemblyStripping (string project, ApplePlatform platform, string runtimeIdentifiers)
		{
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			// Verify value defaults to false when not set
			properties ["Configuration"] = configuration;

			DotNet.AssertBuild (project_path, properties);

			AssertBundleAssembliesStripStatus (appPath, false);
		}

		[Test]
		[TestCase ("MySimpleApp", ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase ("MySimpleApp", ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		[TestCase ("MySimpleApp", ApplePlatform.MacOSX, "osx-x64")]
		[TestCase ("MySimpleApp", ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.MacOSX, "osx-x64")]
		[TestCase ("MySimpleAppWithSatelliteReference", ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void BuildPackageTest (string project, ApplePlatform platform, string runtimeIdentifiers)
		{
			var projectVersion = "3.14";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["CreatePackage"] = "true";

			DotNet.AssertBuild (project_path, properties);

			var pkgPath = Path.Combine (appPath, "..", $"{project}-{projectVersion}.pkg");
			Assert.That (pkgPath, Does.Exist, "pkg creation");
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64;maccatalyst-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void PublishTest (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			string packageExtension;
			string pathVariable;
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				packageExtension = "ipa";
				pathVariable = "IpaPackagePath";
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				packageExtension = "pkg";
				pathVariable = "PkgPackagePath";
				break;
			default:
				throw new ArgumentOutOfRangeException ($"Unknown platform: {platform}");
			}
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var pkgPath = Path.Combine (tmpdir, $"MyPackage.{packageExtension}");

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties [pathVariable] = pkgPath;

			DotNet.AssertPublish (project_path, properties);

			Assert.That (pkgPath, Does.Exist, "ipa/pkg creation");
		}


		[TestCase (ApplePlatform.iOS, "iossimulator-x64")]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64;iossimulator-x64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-x64")]
		public void PublishFailureTest (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			string packageExtension;
			string pathVariable;
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				packageExtension = "ipa";
				pathVariable = "IpaPackagePath";
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				packageExtension = "pkg";
				pathVariable = "PkgPackagePath";
				break;
			default:
				throw new ArgumentOutOfRangeException ($"Unknown platform: {platform}");
			}
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var pkgPath = Path.Combine (tmpdir, $"MyPackage.{packageExtension}");

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties [pathVariable] = pkgPath;

			var rv = DotNet.AssertPublishFailure (project_path, properties);
			var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).ToArray ();
			Assert.AreEqual (1, errors.Length, "Error Count");
			string expectedErrorMessage;
			if (runtimeIdentifiers.IndexOf (';') >= 0) {
				expectedErrorMessage = $"A runtime identifier for a device architecture must be specified in order to publish this project. '{runtimeIdentifiers}' are simulator architectures.";
			} else {
				expectedErrorMessage = $"A runtime identifier for a device architecture must be specified in order to publish this project. '{runtimeIdentifiers}' is a simulator architecture.";
			}
			Assert.AreEqual (expectedErrorMessage, errors [0].Message, "Error Message");

			Assert.That (pkgPath, Does.Not.Exist, "ipa/pkg creation");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void DylibPostProcessingItems (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "NativeDynamicLibraryReferencesApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			var result = DotNet.AssertBuild (project_path, properties);
			var postProcessingItems = GetPostProcessingItems (result.BinLogPath);

			// Find the user's dylib item (not SDK runtime dylibs)
			var dylibItems = postProcessingItems.Where (i => i.ItemSpec.Contains ("libframework.dylib")).ToList ();
			Assert.That (dylibItems.Count, Is.EqualTo (1), $"Expected 1 libframework.dylib post-processing item, got {dylibItems.Count}. All items:\n\t{string.Join ("\n\t", postProcessingItems.Select (i => i.ItemSpec))}");
			var dylibItem = dylibItems [0];

			// Verify the path does NOT contain ".framework/" (the bug was that dylibs were treated as frameworks)
			Assert.That (dylibItem.ItemSpec, Does.Not.Contain (".framework/"), "Dylib path should not contain .framework/");

			// Verify the path contains the full dylib filename
			Assert.That (dylibItem.ItemSpec, Does.Contain ("libframework.dylib"), "Dylib path should contain the full dylib filename");

			// Verify the DSymName is correct for a dylib (should be "libframework.dSYM", not "libframework.dylib.dSYM")
			var dSymName = dylibItem.GetMetadata ("DSymName");
			Assert.That (dSymName, Is.EqualTo ("libframework.dSYM"), "DSymName for dylib");

			// Verify dSYMSourcePath points to where a pre-existing dSYM would be for the dylib.
			// For a dylib at /path/to/libfoo.dylib, the dSYMSourcePath should be /path/to/libfoo.dylib.dSYM
			var dSYMSourcePath = dylibItem.GetMetadata ("dSYMSourcePath");
			var itemSourcePath = dylibItem.GetMetadata ("ItemSourcePath");
			Assert.That (dSYMSourcePath, Is.EqualTo (itemSourcePath + ".dSYM"), "dSYMSourcePath for dylib");

			// Debug builds don't generate dSYMs, verify none exist
			var appContainerDir = Path.GetDirectoryName (appPath)!;
			var dSymDirs = Directory.GetDirectories (appContainerDir, "*.dSYM");
			Assert.That (dSymDirs, Is.Empty, "No dSYMs should exist for Debug builds");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void FrameworkPostProcessingItems (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "NativeFrameworkReferencesApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers, platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			var result = DotNet.AssertBuild (project_path, properties);
			var postProcessingItems = GetPostProcessingItems (result.BinLogPath);

			// Find the framework item (XTest.framework is the dynamic framework)
			var frameworkItems = postProcessingItems.Where (i => i.ItemSpec.Contains ("XTest.framework/XTest")).ToList ();
			Assert.That (frameworkItems.Count, Is.EqualTo (1), $"Expected 1 XTest framework post-processing item, got {frameworkItems.Count}. All items:\n\t{string.Join ("\n\t", postProcessingItems.Select (i => i.ItemSpec))}");
			var frameworkItem = frameworkItems [0];

			// Verify the DSymName is correct for a framework (should be "XTest.framework.dSYM")
			var dSymName = frameworkItem.GetMetadata ("DSymName");
			Assert.That (dSymName, Is.EqualTo ("XTest.framework.dSYM"), "DSymName for framework");

			// Verify dSYMSourcePath points to where a pre-existing dSYM would be for the framework.
			// For a framework at /path/to/XTest.framework/XTest, the dSYMSourcePath should be /path/to/XTest.framework.dSYM
			var dSYMSourcePath = frameworkItem.GetMetadata ("dSYMSourcePath");
			var itemSourcePath = frameworkItem.GetMetadata ("ItemSourcePath");
			Assert.That (dSYMSourcePath, Is.EqualTo (Path.GetDirectoryName (itemSourcePath) + ".dSYM"), "dSYMSourcePath for framework");

			// Debug builds don't generate dSYMs, verify none exist
			var appContainerDir = Path.GetDirectoryName (appPath)!;
			var dSymDirs = Directory.GetDirectories (appContainerDir, "*.dSYM");
			Assert.That (dSymDirs, Is.Empty, "No dSYMs should exist for Debug builds");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64", "Release")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "Release")]
		public void BundleStructureDSyms (ApplePlatform platform, string runtimeIdentifiers, string configuration)
		{
			var project = "BundleStructure";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["_IsAppSigned"] = "true";
			// macOS and Mac Catalyst default to NoDSymUtil=true (dSYMs only generated when archiving),
			// so explicitly disable it to test dSYM generation.
			properties ["NoDSymUtil"] = "false";

			DotNet.AssertBuild (project_path, properties);

			AssertExpectedDSyms (platform, appPath);
		}

		static List<ITaskItem> GetPostProcessingItems (string binLogPath)
		{
			var items = new Dictionary<string, ITaskItem> ();
			foreach (var args in BinLog.ReadBuildEvents (binLogPath)) {
				if (args is not TaskParameterEventArgs tpea)
					continue;
				if (tpea.Kind != TaskParameterMessageKind.AddItem)
					continue;
				if (tpea.ItemType != "_PostProcessingItem")
					continue;
				foreach (var item in tpea.Items) {
					if (item is ITaskItem taskItem)
						items [taskItem.ItemSpec] = taskItem;
				}
			}
			return items.Values.ToList ();
		}
	}
}
