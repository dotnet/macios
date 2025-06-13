using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;
using Xamarin.MacDev.Tasks;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class FilterStaticFrameworksTaskTest {

		string tempDir = "";

		[SetUp]
		public void Setup ()
		{
			tempDir = Path.Combine (Path.GetTempPath (), Guid.NewGuid ().ToString ());
			Directory.CreateDirectory (tempDir);
		}

		[TearDown]
		public void TearDown ()
		{
			if (Directory.Exists (tempDir))
				Directory.Delete (tempDir, true);
		}

		[Test]
		public void TestCustomFrameworkExecutablePath ()
		{
			// Arrange: Create a mock framework with custom CFBundleExecutable
			var frameworkDir = Path.Combine (tempDir, "libavcodec.framework");
			Directory.CreateDirectory (frameworkDir);

			// Create Info.plist with custom CFBundleExecutable
			var infoPlistContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>CFBundleExecutable</key>
	<string>libavcodec.dylib</string>
	<key>CFBundleIdentifier</key>
	<string>com.ffmpeg.libavcodec</string>
</dict>
</plist>";
			File.WriteAllText (Path.Combine (frameworkDir, "Info.plist"), infoPlistContent);

			// Create the custom executable file 
			var customExecutablePath = Path.Combine (frameworkDir, "libavcodec.dylib");
			File.WriteAllText (customExecutablePath, "mock executable");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath", 
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object[] { frameworkDir }) as string;

			// Assert: Should return the custom executable path from CFBundleExecutable
			Assert.That (result, Is.EqualTo (customExecutablePath), "Should use CFBundleExecutable from Info.plist");
		}

		[Test]
		public void TestDefaultFrameworkExecutablePath ()
		{
			// Arrange: Create a framework without Info.plist (or with default CFBundleExecutable)
			var frameworkDir = Path.Combine (tempDir, "TestFramework.framework");
			Directory.CreateDirectory (frameworkDir);

			var expectedPath = Path.Combine (frameworkDir, "TestFramework");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath", 
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object[] { frameworkDir }) as string;

			// Assert: Should return the default framework executable path
			Assert.That (result, Is.EqualTo (expectedPath), "Should use default framework executable path");
		}

		[Test]
		public void TestNonFrameworkPath ()
		{
			// Arrange: Use a non-framework path
			var nonFrameworkPath = Path.Combine (tempDir, "regular_file.dylib");

			// Act: Use reflection to test the helper method
			var method = typeof (FilterStaticFrameworks).GetMethod ("GetFrameworkExecutablePath", 
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var result = method?.Invoke (null, new object[] { nonFrameworkPath }) as string;

			// Assert: Should return the path unchanged
			Assert.That (result, Is.EqualTo (nonFrameworkPath), "Should return non-framework paths unchanged");
		}
	}
}