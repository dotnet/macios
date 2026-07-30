#nullable enable

namespace Xamarin.Tests {
	public class PartialAppManifestTest : TestBaseClass {

		[Test]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		public void Build (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MyPartialAppManifestApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			DotNet.AssertBuild (project_path, properties);

			var infoPlistPath = GetInfoPListPath (platform, appPath);
			var infoPlist = PDictionary.OpenFile (infoPlistPath);
			Assert.That (infoPlist.GetString ("CFBundleIdentifier").Value, Is.EqualTo ("com.xamarin.mypartialappmanifestapp"), "CFBundleIdentifier");
			Assert.That (infoPlist.GetString ("CFBundleDisplayName").Value, Is.EqualTo ("MyPartialAppManifestApp"), "CFBundleDisplayName");
			Assert.That (infoPlist.GetString ("CFBundleVersion").Value, Is.EqualTo ("3.14"), "CFBundleVersion");
			Assert.That (infoPlist.GetString ("CFBundleShortVersionString").Value, Is.EqualTo ("3.14"), "CFBundleShortVersionString");
			Assert.That (infoPlist.GetString ("Something").Value, Is.EqualTo ("SomeValue"), "Something");

			var partialAppManifestPath = Path.Combine (Path.GetDirectoryName (project_path)!, "..", "Partial.plist");
			Configuration.Touch (partialAppManifestPath);
			var rv = DotNet.AssertBuild (project_path, GetDefaultProperties (runtimeIdentifiers));
			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_CompileAppManifest", "_CompileAppManifest rebuild");

			rv = DotNet.AssertBuild (project_path, GetDefaultProperties (runtimeIdentifiers));
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetNotExecuted (allTargets, "_CompileAppManifest", "_CompileAppManifest rebuild 2");
		}

		[Test]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		public void AppWithLibraryWithResourcesReference (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "AppWithLibraryWithResourcesReference";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			var library = "LibraryWithResources";
			var library_project_path = GetProjectPath (library, platform: platform);
			Clean (library_project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["BundleOriginalResources"] = "true";

			DotNet.AssertBuild (project_path, properties);

			var infoPlistPath = GetInfoPListPath (platform, appPath);
			var infoPlist = PDictionary.OpenFile (infoPlistPath);
			Assert.That (infoPlist.GetString ("LibraryWithResources").Value, Is.EqualTo ("Here I am"), "LibraryWithResources 1");

			// build again, nothing should change
			DotNet.AssertBuild (project_path, properties);

			infoPlist = PDictionary.OpenFile (infoPlistPath);
			Assert.That (infoPlist.GetString ("LibraryWithResources").Value, Is.EqualTo ("Here I am"), "LibraryWithResources 2");
		}
	}
}
