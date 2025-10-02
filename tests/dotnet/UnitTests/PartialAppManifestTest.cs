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
			var infoPlist = PDictionary.FromFile (infoPlistPath)!;
			Assert.AreEqual ("com.xamarin.mypartialappmanifestapp", infoPlist.GetString ("CFBundleIdentifier").Value, "CFBundleIdentifier");
			Assert.AreEqual ("MyPartialAppManifestApp", infoPlist.GetString ("CFBundleDisplayName").Value, "CFBundleDisplayName");
			Assert.AreEqual ("3.14", infoPlist.GetString ("CFBundleVersion").Value, "CFBundleVersion");
			Assert.AreEqual ("3.14", infoPlist.GetString ("CFBundleShortVersionString").Value, "CFBundleShortVersionString");
			Assert.AreEqual ("SomeValue", infoPlist.GetString ("Something").Value, "Something");
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
			var infoPlist = PDictionary.FromFile (infoPlistPath)!;
			Assert.AreEqual ("Here I am", infoPlist.GetString ("LibraryWithResources").Value, "LibraryWithResources 1");

			// build again, nothing should change
			DotNet.AssertBuild (project_path, properties);

			infoPlist = PDictionary.FromFile (infoPlistPath)!;
			Assert.AreEqual ("Here I am", infoPlist.GetString ("LibraryWithResources").Value, "LibraryWithResources 2");
		}
	}
}
