#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
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
				ClassicAssert.AreEqual ("Resources", defaultValue, $"Default value for {platform} should be 'Resources'");
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
				ClassicAssert.AreEqual (customPrefix, value, $"{platform}: AppBundleResourcePrefix should be used");
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
				ClassicAssert.AreEqual (customPrefix, iOSValue, "iOS should use IPhoneResourcePrefix");
			}

			if (Configuration.include_tvos) {
				var tvOSValue = GetResourcePrefix ("tvos", ("IPhoneResourcePrefix", customPrefix));
				ClassicAssert.AreEqual (customPrefix, tvOSValue, "tvOS should use IPhoneResourcePrefix");
			}

			// Mac Catalyst uses IPhoneResourcePrefix
			if (Configuration.include_maccatalyst) {
				var macCatalystValue = GetResourcePrefix ("maccatalyst", ("IPhoneResourcePrefix", customPrefix));
				ClassicAssert.AreEqual (customPrefix, macCatalystValue, "Mac Catalyst should use IPhoneResourcePrefix");
			}

			// macOS can use either XamMacResourcePrefix or MonoMacResourcePrefix
			if (Configuration.include_mac) {
				var macOSXamValue = GetResourcePrefix ("macos", ("XamMacResourcePrefix", customPrefix));
				ClassicAssert.AreEqual (customPrefix, macOSXamValue, "macOS should use XamMacResourcePrefix");

				var macOSMonoValue = GetResourcePrefix ("macos", ("MonoMacResourcePrefix", customPrefix));
				ClassicAssert.AreEqual (customPrefix, macOSMonoValue, "macOS should use MonoMacResourcePrefix");
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
				ClassicAssert.AreEqual (appBundlePrefix, iOSValue, "iOS should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// tvOS - AppBundleResourcePrefix should take precedence over IPhoneResourcePrefix
			if (Configuration.include_tvos) {
				var tvOSValue = GetResourcePrefix ("tvos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("IPhoneResourcePrefix", platformPrefix));
				ClassicAssert.AreEqual (appBundlePrefix, tvOSValue, "tvOS should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// Mac Catalyst - AppBundleResourcePrefix should take precedence over IPhoneResourcePrefix
			if (Configuration.include_maccatalyst) {
				var macCatalystValue = GetResourcePrefix ("maccatalyst",
					("AppBundleResourcePrefix", appBundlePrefix),
					("IPhoneResourcePrefix", platformPrefix));
				ClassicAssert.AreEqual (appBundlePrefix, macCatalystValue, "Mac Catalyst should prioritize AppBundleResourcePrefix over IPhoneResourcePrefix");
			}

			// macOS - AppBundleResourcePrefix should take precedence over XamMacResourcePrefix
			if (Configuration.include_mac) {
				var macOSXamValue = GetResourcePrefix ("macos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("XamMacResourcePrefix", platformPrefix));
				ClassicAssert.AreEqual (appBundlePrefix, macOSXamValue, "macOS should prioritize AppBundleResourcePrefix over XamMacResourcePrefix");

				// macOS - AppBundleResourcePrefix should take precedence over MonoMacResourcePrefix
				var macOSMonoValue = GetResourcePrefix ("macos",
					("AppBundleResourcePrefix", appBundlePrefix),
					("MonoMacResourcePrefix", platformPrefix));
				ClassicAssert.AreEqual (appBundlePrefix, macOSMonoValue, "macOS should prioritize AppBundleResourcePrefix over MonoMacResourcePrefix");
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
	}
}
