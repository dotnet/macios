using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;
using Xamarin;
using Xamarin.MacDev.Tasks;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class FilterStaticFrameworksTaskTest {

		string tempDir = "";

		[SetUp]
		public void Setup ()
		{
			tempDir = Cache.CreateTemporaryDirectory ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (Directory.Exists (tempDir))
				Directory.Delete (tempDir, true);
		}

		[Test]
		public void TestCustomFrameworkExecutablePath_iOS ()
		{
			TestCustomFrameworkExecutablePathForPlatform ("iOS", "libavcodec.framework", "libavcodec.dylib", false);
		}

		[Test]
		public void TestCustomFrameworkExecutablePath_tvOS ()
		{
			TestCustomFrameworkExecutablePathForPlatform ("tvOS", "libavcodec.framework", "libavcodec.dylib", false);
		}

		[Test]
		public void TestCustomFrameworkExecutablePath_macOS ()
		{
			TestCustomFrameworkExecutablePathForPlatform ("macOS", "libavcodec.framework", "libavcodec.dylib", true);
		}

		[Test]
		public void TestCustomFrameworkExecutablePath_MacCatalyst ()
		{
			TestCustomFrameworkExecutablePathForPlatform ("MacCatalyst", "libavcodec.framework", "libavcodec.dylib", true);
		}

		void TestCustomFrameworkExecutablePathForPlatform (string platform, string frameworkName, string executableName, bool usesVersionsStructure)
		{
			// Arrange: Create a mock framework with custom CFBundleExecutable
			var frameworkDir = Path.Combine (tempDir, platform, frameworkName);
			Directory.CreateDirectory (frameworkDir);

			string infoPlistPath;
			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure: Framework.framework/Versions/A/Resources/Info.plist
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				infoPlistPath = Path.Combine (resourcesDir, "Info.plist");
				
				// Create symlinks as they exist in real frameworks
				if (!Directory.Exists (Path.Combine (frameworkDir, "Resources"))) {
					Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
				}
			} else {
				// iOS and tvOS structure: Framework.framework/Info.plist
				infoPlistPath = Path.Combine (frameworkDir, "Info.plist");
			}

			// Create Info.plist with custom CFBundleExecutable
			var infoPlistContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>CFBundleExecutable</key>
	<string>" + executableName + @"</string>
	<key>CFBundleIdentifier</key>
	<string>com.ffmpeg.libavcodec</string>
</dict>
</plist>";
			File.WriteAllText (infoPlistPath, infoPlistContent);

			// Create the custom executable file 
			var customExecutablePath = Path.Combine (frameworkDir, executableName);
			File.WriteAllText (customExecutablePath, "mock executable");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object [] { frameworkDir }) as string;

			// Assert: Should return the custom executable path from CFBundleExecutable
			Assert.That (result, Is.EqualTo (customExecutablePath), $"Should use CFBundleExecutable from Info.plist for {platform}");
		}

		[Test]
		public void TestDefaultFrameworkExecutablePath_iOS ()
		{
			TestDefaultFrameworkExecutablePathForPlatform ("iOS", "TestFramework.framework", false);
		}

		[Test]
		public void TestDefaultFrameworkExecutablePath_tvOS ()
		{
			TestDefaultFrameworkExecutablePathForPlatform ("tvOS", "TestFramework.framework", false);
		}

		[Test]
		public void TestDefaultFrameworkExecutablePath_macOS ()
		{
			TestDefaultFrameworkExecutablePathForPlatform ("macOS", "TestFramework.framework", true);
		}

		[Test]
		public void TestDefaultFrameworkExecutablePath_MacCatalyst ()
		{
			TestDefaultFrameworkExecutablePathForPlatform ("MacCatalyst", "TestFramework.framework", true);
		}

		void TestDefaultFrameworkExecutablePathForPlatform (string platform, string frameworkName, bool usesVersionsStructure)
		{
			// Arrange: Create a framework without Info.plist (or with default CFBundleExecutable)
			var frameworkDir = Path.Combine (tempDir, platform, frameworkName);
			Directory.CreateDirectory (frameworkDir);

			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				
				// Create symlinks as they exist in real frameworks
				if (!Directory.Exists (Path.Combine (frameworkDir, "Resources"))) {
					Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
				}
			}

			var expectedPath = Path.Combine (frameworkDir, "TestFramework");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object [] { frameworkDir }) as string;

			// Assert: Should return the default framework executable path
			Assert.That (result, Is.EqualTo (expectedPath), $"Should use default framework executable path for {platform}");
		}

		[Test]
		public void TestNonFrameworkPath ()
		{
			// Arrange: Use a non-framework path
			var nonFrameworkPath = Path.Combine (tempDir, "regular_file.dylib");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object [] { nonFrameworkPath }) as string;

			// Assert: Should return the path unchanged
			Assert.That (result, Is.EqualTo (nonFrameworkPath), "Should return non-framework paths unchanged");
		}

		[Test]
		public void TestMalformedInfoPlist_iOS ()
		{
			TestMalformedInfoPlistForPlatform ("iOS", "BadFramework.framework", false);
		}

		[Test]
		public void TestMalformedInfoPlist_tvOS ()
		{
			TestMalformedInfoPlistForPlatform ("tvOS", "BadFramework.framework", false);
		}

		[Test]
		public void TestMalformedInfoPlist_macOS ()
		{
			TestMalformedInfoPlistForPlatform ("macOS", "BadFramework.framework", true);
		}

		[Test]
		public void TestMalformedInfoPlist_MacCatalyst ()
		{
			TestMalformedInfoPlistForPlatform ("MacCatalyst", "BadFramework.framework", true);
		}

		void TestMalformedInfoPlistForPlatform (string platform, string frameworkName, bool usesVersionsStructure)
		{
			// Arrange: Create a framework with malformed Info.plist
			var frameworkDir = Path.Combine (tempDir, platform, frameworkName);
			Directory.CreateDirectory (frameworkDir);

			string infoPlistPath;
			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure: Framework.framework/Versions/A/Resources/Info.plist
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				infoPlistPath = Path.Combine (resourcesDir, "Info.plist");
				
				// Create symlinks as they exist in real frameworks
				if (!Directory.Exists (Path.Combine (frameworkDir, "Resources"))) {
					Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
				}
			} else {
				// iOS and tvOS structure: Framework.framework/Info.plist
				infoPlistPath = Path.Combine (frameworkDir, "Info.plist");
			}

			// Create malformed Info.plist
			File.WriteAllText (infoPlistPath, "This is not a valid plist file");

			var expectedPath = Path.Combine (frameworkDir, "BadFramework");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

			// Assert: Should either throw an exception or fall back to default path
			// The exact behavior depends on the implementation - if we remove try-catch,
			// this should throw an exception that gets caught by the caller
			try {
				var result = method?.Invoke (null, new object [] { frameworkDir }) as string;
				// If no exception, should fall back to default
				Assert.That (result, Is.EqualTo (expectedPath), $"Should fall back to default path for malformed plist on {platform}");
			} catch (System.Reflection.TargetInvocationException ex) {
				// If exception is thrown, that's also acceptable - it will be caught by the caller
				Assert.That (ex.InnerException, Is.Not.Null, $"Should have an inner exception for malformed plist on {platform}");
			}
		}
	}
}
