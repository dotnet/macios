using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

using Xamarin.Utils;
using Xamarin.Tests;

namespace Xamarin.MMP.Tests {
	public class CodeStrippingTests {
		static Func<string, bool> LipoStripConditional = s => s.Contains ("lipo") && s.Contains ("-extract_family");
		static Func<string, bool> LipoStripSkipPosixConditional = s => LipoStripConditional (s) && !s.Contains ("libMonoPosixHelper.dylib");

		static bool DidAnyLipoStripSkipPosix (BuildResult buildResult)
		{
			return buildResult.BuildOutputLines.Any (LipoStripSkipPosixConditional);
		}

		static bool DidAnyLipoStrip (BuildResult buildResult)
		{
			return buildResult.BuildOutputLines.Any (LipoStripConditional);
		}

		static TI.UnifiedTestConfig CreateStripTestConfig (bool? strip, string tmpDir, string additionalMMPArgs = "")
		{
			TI.UnifiedTestConfig test = new TI.UnifiedTestConfig (tmpDir) { };

			if (strip.HasValue)
				test.CSProjConfig = $"<MonoBundlingExtraArgs>--optimize={(strip.Value ? "+" : "-")}trim-architectures {additionalMMPArgs}</MonoBundlingExtraArgs>";
			else if (!string.IsNullOrEmpty (additionalMMPArgs))
				test.CSProjConfig = $"<MonoBundlingExtraArgs>{additionalMMPArgs}</MonoBundlingExtraArgs>";

			return test;
		}

		void AssertStrip (string libPath, bool shouldStrip)
		{
			var archsFound = MachO.GetArchitectures (libPath);
			if (shouldStrip) {
				Assert.That (archsFound.Count, Is.EqualTo (1), "Did not contain one archs");
				Assert.That (archsFound.Contains (Abi.x86_64), Is.True, "Did not contain x86_64");
			} else {
				Assert.That (archsFound.Count, Is.GreaterThanOrEqualTo (2), "Did not contain two or more archs");
				Assert.That (archsFound.Contains (Abi.i386) || archsFound.Contains (Abi.ARM64), Is.True, "Did not contain i386 nor arm64");
				Assert.That (archsFound.Contains (Abi.x86_64), Is.True, "Did not contain x86_64");
			}
		}

		void StripTestCore (TI.UnifiedTestConfig test, bool debugStrips, bool releaseStrips, string libPath, bool shouldWarn)
		{
			var testResult = TI.TestUnifiedExecutable (test);
			Assert.That (DidAnyLipoStrip (testResult.BuildResult), Is.EqualTo (debugStrips), "Debug lipo usage did not match expectations");
			AssertStrip (Path.Combine (test.TmpDir, "bin/Debug/UnifiedExample.app/", libPath), shouldStrip: debugStrips);
			Assert.That (testResult.BuildResult.HasMessage (2108), Is.EqualTo (shouldWarn && debugStrips), "Debug warning did not match expectations");

			test.Release = true;
			testResult = TI.TestUnifiedExecutable (test);
			Assert.That (DidAnyLipoStrip (testResult.BuildResult), Is.EqualTo (releaseStrips), "Release lipo usage did not match expectations");
			AssertStrip (Path.Combine (test.TmpDir, "bin/Release/UnifiedExample.app/", libPath), shouldStrip: releaseStrips);
			Assert.That (testResult.BuildResult.HasMessage (2108), Is.EqualTo (shouldWarn && releaseStrips), "Release warning did not match expectations");
		}

		[TestCase (null, false, true)]
		[TestCase (true, true, true)]
		[TestCase (false, false, false)]
		public void ShouldStripMonoPosixHelper (bool? strip, bool debugStrips, bool releaseStrips)
		{
			var posixHelper = Path.Combine (Configuration.SdkRootXM, "SDKs", "Xamarin.macOS.sdk", "lib", "libMonoPosixHelper.dylib");
			if (Xamarin.MachO.GetArchitectures (posixHelper).Count < 2)
				Assert.Ignore ($"libMonoPosixHelper.dylib is not a fat library.");

			MMPTests.RunMMPTest (tmpDir => {
				TI.UnifiedTestConfig test = CreateStripTestConfig (strip, tmpDir);
				// Mono's linker is smart enough to remove libMonoPosixHelper unless used (DeflateStream uses it)
				test.TestCode = "using (var ms = new System.IO.MemoryStream ()) { using (var gz = new System.IO.Compression.DeflateStream (ms, System.IO.Compression.CompressionMode.Compress)) { }}";
				StripTestCore (test, debugStrips, releaseStrips, "Contents/MonoBundle/libMonoPosixHelper.dylib", shouldWarn: false);
			});
		}

		[TestCase (null, false, true)]
		[TestCase (true, true, true)]
		[TestCase (false, false, false)]
		public void ShouldStripUserFramework (bool? strip, bool debugStrips, bool releaseStrips)
		{
			Configuration.AssertDotNetAvailable (); // not really a .NET test, but the logic that builds our native test frameworks is (unintentionally, but untrivially unravelable) .NET-only
			MMPTests.RunMMPTest (tmpDir => {
				var frameworkPath = FrameworkBuilder.CreateFatFramework (tmpDir);
				TI.UnifiedTestConfig test = CreateStripTestConfig (strip, tmpDir, $"--native-reference={frameworkPath}");

				var frameworkName = Path.GetFileNameWithoutExtension (frameworkPath);
				StripTestCore (test, debugStrips, releaseStrips, $"Contents/Frameworks/{frameworkName}.framework/{frameworkName}", shouldWarn: true);
			});
		}

		const string MonoPosixOffset = "Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/libMonoPosixHelper.dylib";

		[TestCase (true, true)]
		[TestCase (false, false)]
		public void ExplictStripOption_ThirdPartyLibrary_AndWarnsIfSo (bool? strip, bool shouldStrip)
		{
			MMPTests.RunMMPTest (tmpDir => {
				string originalLocation = Path.Combine (Configuration.SourceRoot, "tests", "test-libraries", "libtest-fat.macos.dylib");
				string newLibraryLocation = Path.Combine (tmpDir, "libTest.dylib");
				File.Copy (originalLocation, newLibraryLocation);

				TI.UnifiedTestConfig test = CreateStripTestConfig (strip, tmpDir, $" --native-reference=\"{newLibraryLocation}\"");
				test.Release = true;

				var testResult = TI.TestUnifiedExecutable (test);
				var bundleDylib = Path.Combine (test.BundlePath, "Contents", "MonoBundle", "libTest.dylib");
				Assert.That (bundleDylib, Does.Exist, "libTest.dylib presence in app bundle");

				var architectures = MachO.GetArchitectures (bundleDylib);
				if (shouldStrip) {
					Assert.That (architectures.Count, Is.EqualTo (1), "libTest.dylib should only contain 1 architecture");
					Assert.That (architectures [0], Is.EqualTo (Abi.x86_64), "libTest.dylib should be x86_64");
					testResult.Messages.AssertWarning (2108, "libTest.dylib was stripped of architectures except x86_64 to comply with App Store restrictions. This could break existing codesigning signatures. Consider stripping the library with lipo or disabling with --optimize=-trim-architectures");
				} else {
					Assert.That (architectures.Count, Is.EqualTo (2), "libTest.dylib should contain 2+ architectures");
					Assert.That (architectures, Is.EquivalentTo (new Abi [] { Abi.i386, Abi.x86_64 }), "libTest.dylib should be x86_64 + i386");
					testResult.Messages.AssertWarningCount (0);
				}
			});
		}

		void AssertNoLipoOrWarning (BuildResult buildOutput, string context)
		{
			Assert.That (DidAnyLipoStrip (buildOutput), Is.False, "lipo incorrectly run in context: " + context);
			Assert.That (buildOutput.HasMessage (2108), Is.False, "MM2108 incorrectly given in in context: " + context);
		}

		void AssertLipoOnlyMonoPosix (BuildResult buildOutput, string context)
		{
			Assert.That (DidAnyLipoStripSkipPosix (buildOutput), Is.False, "lipo incorrectly run in context outside of libMonoPosixHelper/libmono-native: " + context);
			Assert.That (buildOutput.HasMessage (2108), Is.False, "MM2108 incorrectly given in in context: " + context);
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ThirdPartyLibrary_WithIncorrectBitness_ShouldWarnOnRelease (bool sixtyFourBits)
		{
			Configuration.AssertDotNetAvailable (); // not really a .NET test, but the logic that builds our native test frameworks is (unintentionally, but untrivially unravelable) .NET-only
			MMPTests.RunMMPTest (tmpDir => {
				var frameworkPath = FrameworkBuilder.CreateFatFramework (tmpDir);

				TI.UnifiedTestConfig test = CreateStripTestConfig (null, tmpDir, $" --native-reference=\"{frameworkPath}\"");

				// Should always skip lipo/warning in Debug
				var testResult = TI.TestUnifiedExecutable (test);
				AssertNoLipoOrWarning (testResult.BuildResult, "Debug");

				// Should always lipo/warn in Release
				test.Release = true;
				testResult = TI.TestUnifiedExecutable (test);
				Assert.That (DidAnyLipoStrip (testResult.BuildResult), Is.True, $"lipo did not run in release");
				testResult.BuildResult.Messages.AssertError (2108, $"{frameworkPath} was stripped of architectures except x86_64 to comply with App Store restrictions. This could break existing codesigning signatures. Consider stripping the library with lipo or disabling with --optimize=-trim-architectures");
				// Assert.That (testResult.Contains ("MM2108"), Is.True, $"MM2108 not given in release");

			});
		}

		[TestCase]
		public void ThirdPartyLibrary_WithCorrectBitness_ShouldNotStripOrWarn ()
		{
			Configuration.AssertDotNetAvailable (); // not really a .NET test, but the logic that builds our native test frameworks is (unintentionally, but untrivially unravelable) .NET-only
			MMPTests.RunMMPTest (tmpDir => {
				var frameworkPath = FrameworkBuilder.CreateThinFramework (tmpDir);

				TI.UnifiedTestConfig test = CreateStripTestConfig (null, tmpDir, $" --native-reference=\"{frameworkPath}\"");

				var testResult = TI.TestUnifiedExecutable (test);
				AssertNoLipoOrWarning (testResult.BuildResult, "Debug");

				test.Release = true;
				testResult = TI.TestUnifiedExecutable (test);
				AssertLipoOnlyMonoPosix (testResult.BuildResult, "Release"); // libMonoPosixHelper.dylib and libmono-native.dylib will lipo in Release
			});
		}
	}
}
