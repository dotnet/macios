using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;
using Xamarin;
using Xamarin.MacDev.Tasks;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class FilterStaticFrameworksTaskTest : TestBase {

		string tempDir = "";

		[SetUp]
		public void Setup ()
		{
			tempDir = Cache.CreateTemporaryDirectory ();
		}

		[TearDown]
		public void TearDown ()
		{
			// Cache.CreateTemporaryDirectory() handles cleanup automatically
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "libavcodec.framework", "libavcodec.dylib")]
		[TestCase (ApplePlatform.TVOS, "libavcodec.framework", "libavcodec.dylib")]
		[TestCase (ApplePlatform.MacOSX, "libavcodec.framework", "libavcodec.dylib")]
		[TestCase (ApplePlatform.MacCatalyst, "libavcodec.framework", "libavcodec.dylib")]
		public void TestCustomFrameworkExecutablePath (ApplePlatform platform, string frameworkName, string executableName)
		{
			TestCustomFrameworkExecutablePathForPlatform (platform, frameworkName, executableName);
		}

		void TestCustomFrameworkExecutablePathForPlatform (ApplePlatform platform, string frameworkName, string executableName)
		{
			bool usesVersionsStructure = platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst;
			
			// Arrange: Create a mock framework with custom CFBundleExecutable
			var frameworkDir = Path.Combine (tempDir, platform.AsString (), frameworkName);
			Directory.CreateDirectory (frameworkDir);

			string infoPlistPath;
			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure: Framework.framework/Versions/A/Resources/Info.plist
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				infoPlistPath = Path.Combine (resourcesDir, "Info.plist");
				
				// Create symlinks as they exist in real frameworks
				Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
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

			// Act: Create and execute the task
			var task = CreateTask<FilterStaticFrameworks> ();
			task.FrameworkToPublish = new ITaskItem [] { new TaskItem (frameworkDir) };
			task.OnlyFilterFrameworks = true;

			// Execute the task
			ExecuteTask (task);

			// Assert: Task should succeed and the framework should be processed correctly
			Assert.That (task.FrameworkToPublish, Is.Not.Null);
			Assert.That (task.FrameworkToPublish.Length, Is.EqualTo (1), $"Framework should be included for {platform}");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "TestFramework.framework")]
		[TestCase (ApplePlatform.TVOS, "TestFramework.framework")]
		[TestCase (ApplePlatform.MacOSX, "TestFramework.framework")]
		[TestCase (ApplePlatform.MacCatalyst, "TestFramework.framework")]
		public void TestDefaultFrameworkExecutablePath (ApplePlatform platform, string frameworkName)
		{
			TestDefaultFrameworkExecutablePathForPlatform (platform, frameworkName);
		}

		void TestDefaultFrameworkExecutablePathForPlatform (ApplePlatform platform, string frameworkName)
		{
			bool usesVersionsStructure = platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst;
			
			// Arrange: Create a framework without Info.plist (or with default CFBundleExecutable)
			var frameworkDir = Path.Combine (tempDir, platform.AsString (), frameworkName);
			Directory.CreateDirectory (frameworkDir);

			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				
				// Create symlinks as they exist in real frameworks
				Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
			}

			var expectedExecutable = Path.Combine (frameworkDir, "TestFramework");
			File.WriteAllText (expectedExecutable, "mock executable");

			// Act: Create and execute the task
			var task = CreateTask<FilterStaticFrameworks> ();
			task.FrameworkToPublish = new ITaskItem [] { new TaskItem (frameworkDir) };
			task.OnlyFilterFrameworks = true;

			// Execute the task
			ExecuteTask (task);

			// Assert: Task should succeed and use default framework executable path
			Assert.That (task.FrameworkToPublish, Is.Not.Null);
			Assert.That (task.FrameworkToPublish.Length, Is.EqualTo (1), $"Framework should be included for {platform}");
		}

		[Test]
		public void TestNonFrameworkPath ()
		{
			// Arrange: Use a non-framework path
			var nonFrameworkPath = Path.Combine (tempDir, "regular_file.dylib");
			File.WriteAllText (nonFrameworkPath, "mock dylib");

			// Act: Create and execute the task
			var task = CreateTask<FilterStaticFrameworks> ();
			task.FrameworkToPublish = new ITaskItem [] { new TaskItem (nonFrameworkPath) };
			task.OnlyFilterFrameworks = false; // Don't filter non-frameworks

			// Execute the task
			ExecuteTask (task);

			// Assert: Non-framework paths should be processed unchanged
			Assert.That (task.FrameworkToPublish, Is.Not.Null);
			Assert.That (task.FrameworkToPublish.Length, Is.EqualTo (1), "Non-framework file should be included");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "BadFramework.framework")]
		[TestCase (ApplePlatform.TVOS, "BadFramework.framework")]
		[TestCase (ApplePlatform.MacOSX, "BadFramework.framework")]
		[TestCase (ApplePlatform.MacCatalyst, "BadFramework.framework")]
		public void TestMalformedInfoPlist (ApplePlatform platform, string frameworkName)
		{
			TestMalformedInfoPlistForPlatform (platform, frameworkName);
		}

		void TestMalformedInfoPlistForPlatform (ApplePlatform platform, string frameworkName)
		{
			bool usesVersionsStructure = platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst;
			
			// Arrange: Create a framework with malformed Info.plist
			var frameworkDir = Path.Combine (tempDir, platform.AsString (), frameworkName);
			Directory.CreateDirectory (frameworkDir);

			string infoPlistPath;
			if (usesVersionsStructure) {
				// macOS and MacCatalyst structure: Framework.framework/Versions/A/Resources/Info.plist
				var versionsDir = Path.Combine (frameworkDir, "Versions", "A");
				var resourcesDir = Path.Combine (versionsDir, "Resources");
				Directory.CreateDirectory (resourcesDir);
				infoPlistPath = Path.Combine (resourcesDir, "Info.plist");
				
				// Create symlinks as they exist in real frameworks
				Directory.CreateSymbolicLink (Path.Combine (frameworkDir, "Resources"), "Versions/A/Resources");
			} else {
				// iOS and tvOS structure: Framework.framework/Info.plist
				infoPlistPath = Path.Combine (frameworkDir, "Info.plist");
			}

			// Create malformed Info.plist
			File.WriteAllText (infoPlistPath, "This is not a valid plist file");

			var expectedExecutable = Path.Combine (frameworkDir, "BadFramework");
			File.WriteAllText (expectedExecutable, "mock executable");

			// Act: Create and execute the task
			var task = CreateTask<FilterStaticFrameworks> ();
			task.FrameworkToPublish = new ITaskItem [] { new TaskItem (frameworkDir) };
			task.OnlyFilterFrameworks = true;

			// Execute the task - should handle malformed plist gracefully
			ExecuteTask (task);

			// Assert: Should fall back to default behavior and succeed
			Assert.That (task.FrameworkToPublish, Is.Not.Null);
			Assert.That (task.FrameworkToPublish.Length, Is.EqualTo (1), $"Framework should be included despite malformed plist for {platform}");
		}
	}
}
