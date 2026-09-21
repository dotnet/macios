#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using NUnit.Framework;

namespace Xamarin.Tests {
	[TestFixture]
	public class ResourcePrefixTest : TestBaseClass {
		[Test]
		public void ResourcePrefix_DefaultValues ()
		{
			// Arrange
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				// Act & Assert
				var defaultValue = GetResourcePrefix (platform.AsString ().ToLower ());
				Assert.That (defaultValue, Is.EqualTo ("Resources"), $"Default value for {platform} should be 'Resources'");
			}
		}

		[Test]
		public void ResourcePrefix_AppBundleResourcePrefix ()
		{
			// Arrange
			var platforms = Configuration.GetIncludedPlatforms ();
			var customPrefix = "CustomResources";

			foreach (var platform in platforms) {
				// Act
				var value = GetResourcePrefix (platform.AsString ().ToLower (), ("AppBundleResourcePrefix", customPrefix));

				// Assert
				Assert.That (value, Is.EqualTo (customPrefix), $"{platform}: AppBundleResourcePrefix should be used");
			}
		}

		[Test]
		public void ResourcePrefix_PlatformSpecific ()
		{
			// Arrange
			var customPrefix = "CustomResources";

			// Act & Assert

			// iOS and tvOS use IPhoneResourcePrefix
			if (Configuration.include_ios) {
				var iOSValue = GetResourcePrefix ("ios", ("IPhoneResourcePrefix", customPrefix));
				Assert.That (iOSValue, Is.EqualTo (customPrefix), "iOS should use IPhoneResourcePrefix");
			}

			if (Configuration.include_tvos) {
				var tvOSValue = GetResourcePrefix ("tvos", ("IPhoneResourcePrefix", customPrefix));
				Assert.That (tvOSValue, Is.EqualTo (customPrefix), "tvOS should use IPhoneResourcePrefix");
			}

			// Mac Catalyst uses IPhoneResourcePrefix
			if (Configuration.include_maccatalyst) {
				var macCatalystValue = GetResourcePrefix ("maccatalyst", ("IPhoneResourcePrefix", customPrefix));
				Assert.That (macCatalystValue, Is.EqualTo (customPrefix), "Mac Catalyst should use IPhoneResourcePrefix");
			}

			// macOS can use either XamMacResourcePrefix or MonoMacResourcePrefix
			if (Configuration.include_mac) {
				var macOSXamValue = GetResourcePrefix ("macos", ("XamMacResourcePrefix", customPrefix));
				Assert.That (macOSXamValue, Is.EqualTo (customPrefix), "macOS should use XamMacResourcePrefix");

				var macOSMonoValue = GetResourcePrefix ("macos", ("MonoMacResourcePrefix", customPrefix));
				Assert.That (macOSMonoValue, Is.EqualTo (customPrefix), "macOS should use MonoMacResourcePrefix");
			}
		}

		[Test]
		public void ResourcePrefix_Precedence ()
		{
			// Arrange
			var appBundlePrefix = "AppPrefix";
			var platformPrefix = "PlatformPrefix";

			// Act & Assert

			// iOS - AppBundleResourcePrefix should take precedence over IPhoneResourcePrefix
			if (Configuration.include_ios) {
				var iOSValue = GetResourcePrefix ("ios",
					("AppBundleResourcePrefix", appBundlePrefix),
					("IPhoneResourcePrefix", platformPrefix));
				Assert.That (iOSValue, Is.EqualTo (appBundlePrefix), "iOS should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// tvOS - AppBundleResourcePrefix should take precedence over IPhoneResourcePrefix
			if (Configuration.include_tvos) {
				var tvOSValue = GetResourcePrefix ("tvos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("IPhoneResourcePrefix", platformPrefix));
				Assert.That (tvOSValue, Is.EqualTo (appBundlePrefix), "tvOS should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// Mac Catalyst - AppBundleResourcePrefix should take precedence over IPhoneResourcePrefix
			if (Configuration.include_maccatalyst) {
				var macCatalystValue = GetResourcePrefix ("maccatalyst",
					("AppBundleResourcePrefix", appBundlePrefix),
					("IPhoneResourcePrefix", platformPrefix));
				Assert.That (macCatalystValue, Is.EqualTo (appBundlePrefix), "Mac Catalyst should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// macOS - AppBundleResourcePrefix should take precedence over XamMacResourcePrefix
			if (Configuration.include_mac) {
				var macOSXamValue = GetResourcePrefix ("macos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("XamMacResourcePrefix", platformPrefix));
				Assert.That (macOSXamValue, Is.EqualTo (appBundlePrefix), "macOS should prioritize AppBundleResourcePrefix over XamMacResourcePrefix");

				// macOS - AppBundleResourcePrefix should take precedence over MonoMacResourcePrefix
				var macOSMonoValue = GetResourcePrefix ("macos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("MonoMacResourcePrefix", platformPrefix));
				Assert.That (macOSMonoValue, Is.EqualTo (appBundlePrefix), "macOS should prioritize AppBundleResourcePrefix over MonoMacResourcePrefix");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_PropertyValue ()
		{
			// Verify that _ResourcePrefix correctly carries multiple semicolon-separated values
			var multiplePrefixes = "Resources;Platforms/iOS/Resources";

			if (Configuration.include_ios) {
				var value = GetResourcePrefix ("ios", ("IPhoneResourcePrefix", multiplePrefixes));
				Assert.That (value, Is.EqualTo (multiplePrefixes), "iOS: _ResourcePrefix should contain both prefixes separated by semicolons");
			}

			if (Configuration.include_tvos) {
				var value = GetResourcePrefix ("tvos", ("IPhoneResourcePrefix", multiplePrefixes));
				Assert.That (value, Is.EqualTo (multiplePrefixes), "tvOS: _ResourcePrefix should contain both prefixes separated by semicolons");
			}

			if (Configuration.include_maccatalyst) {
				var value = GetResourcePrefix ("maccatalyst", ("IPhoneResourcePrefix", multiplePrefixes));
				Assert.That (value, Is.EqualTo (multiplePrefixes), "Mac Catalyst: _ResourcePrefix should contain both prefixes separated by semicolons");
			}

			if (Configuration.include_mac) {
				var macMultiple = "Resources;Platforms/macOS/Resources";
				var value = GetResourcePrefix ("macos", ("XamMacResourcePrefix", macMultiple));
				Assert.That (value, Is.EqualTo (macMultiple), "macOS: _ResourcePrefix should contain both prefixes separated by semicolons");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_AppBundleResourcePrefix ()
		{
			// Verify that AppBundleResourcePrefix also works with multiple semicolon-separated values
			var multiplePrefixes = "Resources;SharedResources;Platforms/iOS/Resources";

			if (Configuration.include_ios) {
				var value = GetResourcePrefix ("ios", ("AppBundleResourcePrefix", multiplePrefixes));
				Assert.That (value, Is.EqualTo (multiplePrefixes), "iOS: AppBundleResourcePrefix should support multiple prefixes");
			}

			if (Configuration.include_mac) {
				var value = GetResourcePrefix ("macos", ("AppBundleResourcePrefix", multiplePrefixes));
				Assert.That (value, Is.EqualTo (multiplePrefixes), "macOS: AppBundleResourcePrefix should support multiple prefixes");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_BundleResourceItems_SinglePrefix ()
		{
			// Verify that files in a single resource prefix directory are picked up as BundleResource items
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
				var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

				// Create a resource directory with a file
				var resourceDir = Path.Combine (testDirectory, "Resources");
				Directory.CreateDirectory (resourceDir);
				File.WriteAllText (Path.Combine (resourceDir, "image.png"), "fake-png-data");

				// Create project with default resource prefix
				File.WriteAllText (projectPath, GetTestProjectContent (platform.AsString ().ToLower ()));

				// Get BundleResource items
				var bundleResources = GetBundleResourceIdentities (projectPath);

				Assert.That (bundleResources, Has.Some.EndsWith ("image.png"),
					$"{platform}: image.png in Resources/ should be included as a BundleResource");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_BundleResourceItems_MultiplePrefixes ()
		{
			// Verify that files in multiple resource prefix directories are all picked up as BundleResource items
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
				var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

				// Create both resource directories with files
				var resourceDir1 = Path.Combine (testDirectory, "Resources");
				Directory.CreateDirectory (resourceDir1);
				File.WriteAllText (Path.Combine (resourceDir1, "shared.png"), "fake-png-data");

				var resourceDir2 = Path.Combine (testDirectory, "Platforms", platform.AsString (), "Resources");
				Directory.CreateDirectory (resourceDir2);
				File.WriteAllText (Path.Combine (resourceDir2, "platform.png"), "fake-png-data");

				// Create project with multiple resource prefixes
				var platformStr = platform.AsString ().ToLower ();
				var prefixProperty = platform == ApplePlatform.MacOSX ? "XamMacResourcePrefix" : "IPhoneResourcePrefix";
				var prefixes = $"Resources;Platforms/{platform.AsString ()}/Resources";

				File.WriteAllText (projectPath, GetTestProjectContent (platformStr, (prefixProperty, prefixes)));

				// Get BundleResource items (need to run the target to expand multi-prefix globs)
				var bundleResources = GetBundleResourceIdentities (projectPath);

				Assert.That (bundleResources, Has.Some.EndsWith ("shared.png"),
					$"{platform}: shared.png in Resources/ should be included as a BundleResource with multiple prefixes");
				Assert.That (bundleResources, Has.Some.EndsWith ("platform.png"),
					$"{platform}: platform.png in Platforms/{platform.AsString ()}/Resources/ should be included as a BundleResource with multiple prefixes");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_BundleResourceItems_OnlySecondPrefixHasFiles ()
		{
			// Verify that BundleResource items are picked up even if only the second prefix directory has files
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
				var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

				// Only create the second resource directory
				var resourceDir2 = Path.Combine (testDirectory, "PlatformResources");
				Directory.CreateDirectory (resourceDir2);
				File.WriteAllText (Path.Combine (resourceDir2, "only-here.png"), "fake-png-data");

				// Create project with multiple resource prefixes (first doesn't exist)
				var platformStr = platform.AsString ().ToLower ();
				var prefixProperty = platform == ApplePlatform.MacOSX ? "XamMacResourcePrefix" : "IPhoneResourcePrefix";
				var prefixes = "Resources;PlatformResources";

				File.WriteAllText (projectPath, GetTestProjectContent (platformStr, (prefixProperty, prefixes)));

				// Get BundleResource items (need to run the target to expand multi-prefix globs)
				var bundleResources = GetBundleResourceIdentities (projectPath);

				Assert.That (bundleResources, Has.Some.EndsWith ("only-here.png"),
					$"{platform}: only-here.png in PlatformResources/ should be included as a BundleResource even when first prefix directory doesn't exist");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_BundleResourceItems_SubDirectories ()
		{
			// Verify that files in subdirectories of multiple prefixes are picked up
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
				var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

				// Create resource directories with subdirectories
				var resourceDir1 = Path.Combine (testDirectory, "Resources", "Images");
				Directory.CreateDirectory (resourceDir1);
				File.WriteAllText (Path.Combine (resourceDir1, "icon.png"), "fake-png-data");

				var resourceDir2 = Path.Combine (testDirectory, "Extra", "Sounds");
				Directory.CreateDirectory (resourceDir2);
				File.WriteAllText (Path.Combine (resourceDir2, "beep.wav"), "fake-wav-data");

				// Create project with multiple resource prefixes
				var platformStr = platform.AsString ().ToLower ();
				var prefixProperty = platform == ApplePlatform.MacOSX ? "XamMacResourcePrefix" : "IPhoneResourcePrefix";
				var prefixes = "Resources;Extra";

				File.WriteAllText (projectPath, GetTestProjectContent (platformStr, (prefixProperty, prefixes)));

				// Get BundleResource items (need to run the target to expand multi-prefix globs)
				var bundleResources = GetBundleResourceIdentities (projectPath);

				Assert.That (bundleResources, Has.Some.EndsWith ("icon.png"),
					$"{platform}: icon.png in Resources/Images/ should be included as a BundleResource");
				Assert.That (bundleResources, Has.Some.EndsWith ("beep.wav"),
					$"{platform}: beep.wav in Extra/Sounds/ should be included as a BundleResource");
			}
		}

		[Test]
		public void ResourcePrefix_MultiplePrefixes_BundleResourceItems_ThreePrefixes ()
		{
			// Verify that three semicolon-separated prefixes all work
			var platforms = Configuration.GetIncludedPlatforms ();

			foreach (var platform in platforms) {
				var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
				var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

				// Create three resource directories
				var resourceDir1 = Path.Combine (testDirectory, "Resources");
				Directory.CreateDirectory (resourceDir1);
				File.WriteAllText (Path.Combine (resourceDir1, "first.png"), "fake-png-data");

				var resourceDir2 = Path.Combine (testDirectory, "SharedResources");
				Directory.CreateDirectory (resourceDir2);
				File.WriteAllText (Path.Combine (resourceDir2, "second.txt"), "text-data");

				var resourceDir3 = Path.Combine (testDirectory, "PlatformResources");
				Directory.CreateDirectory (resourceDir3);
				File.WriteAllText (Path.Combine (resourceDir3, "third.json"), "{}");

				// Create project with three resource prefixes
				var platformStr = platform.AsString ().ToLower ();
				var prefixProperty = platform == ApplePlatform.MacOSX ? "XamMacResourcePrefix" : "IPhoneResourcePrefix";
				var prefixes = "Resources;SharedResources;PlatformResources";

				File.WriteAllText (projectPath, GetTestProjectContent (platformStr, (prefixProperty, prefixes)));

				// Get BundleResource items (need to run the target to expand multi-prefix globs)
				var bundleResources = GetBundleResourceIdentities (projectPath);

				Assert.That (bundleResources, Has.Some.EndsWith ("first.png"),
					$"{platform}: first.png from first prefix should be included");
				Assert.That (bundleResources, Has.Some.EndsWith ("second.txt"),
					$"{platform}: second.txt from second prefix should be included");
				Assert.That (bundleResources, Has.Some.EndsWith ("third.json"),
					$"{platform}: third.json from third prefix should be included");
			}
		}

		private string GetResourcePrefix (string platform, params (string Property, string Value) [] properties)
		{
			// Create a temporary test project
			var testDirectory = Xamarin.Cache.CreateTemporaryDirectory ();
			var projectPath = Path.Combine (testDirectory, "TestApp.csproj");

			// Create project file with specified properties
			File.WriteAllText (projectPath, GetTestProjectContent (platform, properties));

			// Use dotnet build with getProperty to get _ResourcePrefix value
			return DotNet.GetProperty (projectPath, "_ResourcePrefix", (Dictionary<string, string>?) null);
		}

		private List<string> GetBundleResourceIdentities (string projectPath, string? target = null)
		{
			var json = DotNet.GetItems (projectPath, "BundleResource", target: target);
			using var doc = JsonDocument.Parse (json);
			var items = new List<string> ();
			if (doc.RootElement.TryGetProperty ("Items", out var itemsObj) &&
				itemsObj.TryGetProperty ("BundleResource", out var bundleResources)) {
				foreach (var item in bundleResources.EnumerateArray ()) {
					if (item.TryGetProperty ("Identity", out var identity)) {
						var value = identity.GetString ();
						if (value is not null)
							items.Add (value);
					}
				}
			}
			return items;
		}

		private string GetTestProjectContent (string platform, params (string Property, string Value) [] properties)
		{
			// Create project property group with specified properties
			var propertyGroup = "";
			foreach (var (property, value) in properties) {
				propertyGroup += $"    <{property}>{value}</{property}>\n";
			}

			// Generate the project file content
			return $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net$(BundledNETCoreAppTargetFrameworkVersion)-{platform}</TargetFramework>
    <OutputType>Exe</OutputType>
{propertyGroup}
  </PropertyGroup>
</Project>";
		}

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64;osx-x64")]
		public void AppWithMultipleResourcePrefixes (ApplePlatform platform, string runtimeIdentifiers)
		{
			// End-to-end test: build an on-disk project that uses multiple resource prefixes,
			// and verify that the resulting app bundle contains resources from all prefix directories.
			var project = "AppWithMultipleResourcePrefixes";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			DotNet.AssertBuild (project_path, GetDefaultProperties (runtimeIdentifiers));

			var resourcesDirectory = GetResourcesDirectory (platform, appPath);

			// Verify resources from the first prefix directory (Resources/) are in the app bundle
			var sharedResource = Path.Combine (resourcesDirectory, "SharedResource.txt");
			Assert.That (sharedResource, Does.Exist, $"{platform}: SharedResource.txt from Resources/ prefix should be in the app bundle");

			// Verify resources from the second prefix directory (PlatformResources/) are in the app bundle
			var platformResource = Path.Combine (resourcesDirectory, "PlatformResource.txt");
			Assert.That (platformResource, Does.Exist, $"{platform}: PlatformResource.txt from PlatformResources/ prefix should be in the app bundle");

			// Verify resources in subdirectories of the second prefix are also included
			var subDirResource = Path.Combine (resourcesDirectory, "SubDir", "SubDirResource.txt");
			Assert.That (subDirResource, Does.Exist, $"{platform}: SubDir/SubDirResource.txt from PlatformResources/ prefix should be in the app bundle");
		}
	}
}
