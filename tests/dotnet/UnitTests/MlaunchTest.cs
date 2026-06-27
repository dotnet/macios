using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using Mono.Cecil;

using Xamarin.Tests;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class MlaunchTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvos-arm64")]
		public void GetMlaunchInstallArguments (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var outputPath = Path.Combine (Cache.CreateTemporaryDirectory (), "install.sh");
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCodeSigning"] = "false"; // Skip code signing, since that would require making sure we have code signing configured on bots.

			// Create the app manifest first, since it's required to compute the mlaunch install arguments
			DotNet.Execute ("build", project_path, properties, target: "_DetectSdkLocations;_DetectAppManifest;_CompileAppManifest;_WriteAppManifest");

			properties ["MlaunchInstallScript"] = outputPath;
			var rv = DotNet.Execute ("build", project_path, properties, assert_success: false, target: "ComputeMlaunchInstallArguments");

			if (rv.ExitCode != 0) {
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).Select (v => v.Message).OfType<string> ().ToArray ();
				Assert.That (string.Join ("\n", errors), Does.Contain ("No applicable and available devices found."));
				return;
			}

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchInstallArguments", out var mlaunchInstallArguments))
				Assert.Fail ("Could not find the property 'MlaunchInstallArguments' in the binlog.");

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchPath", out var mlaunchPath))
				Assert.Fail ("Could not find the property 'MlaunchPath' in the binlog.");
			Assert.That (mlaunchPath, Does.Exist, "mlaunch existence");

			var expectedArguments = new StringBuilder ();
			expectedArguments.Append ("--installdev ");
			expectedArguments.Append (appPath.Substring (Path.GetDirectoryName (project_path)!.Length + 1)).Append ('/');
			expectedArguments.Append ($" --wait-for-exit:false");
			Assert.That (mlaunchInstallArguments, Is.EqualTo (expectedArguments.ToString ()));

			var scriptContents = File.ReadAllText (outputPath).Trim ('\n');
			var expectedScriptContents = $"'{mlaunchPath}' " + expectedArguments.ToString ();
			Assert.That (scriptContents, Is.EqualTo (expectedScriptContents), "Script contents");
		}

		public static object [] GetMlaunchRunArgumentsTestCases ()
		{
			return new object [] {
				new object [] {ApplePlatform.iOS, "iossimulator-x64;iossimulator-arm64", @":v2:udid=[A-F0-9-]+" },
				new object [] {ApplePlatform.iOS, "ios-arm64", "" },
				new object [] {ApplePlatform.TVOS, "tvossimulator-arm64", @":v2:udid=[A-F0-9-]+" },
			};
		}

		[Test]
		[TestCaseSource (nameof (GetMlaunchRunArgumentsTestCases))]
		public void GetMlaunchRunArguments (ApplePlatform platform, string runtimeIdentifiers, string device)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var outputPath = Path.Combine (Cache.CreateTemporaryDirectory (), "launch.sh");
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCodeSigning"] = "false"; // Skip code signing, since that would require making sure we have code signing configured on bots.

			// Create the app manifest first, since it's required to compute the mlaunch run arguments
			DotNet.Execute ("build", project_path, properties, target: "_DetectSdkLocations;_DetectAppManifest;_CompileAppManifest;_WriteAppManifest");

			properties ["MlaunchRunScript"] = outputPath;
			var rv = DotNet.Execute ("build", project_path, properties, assert_success: false, target: "ComputeMlaunchRunArguments");

			if (rv.ExitCode != 0) {
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).Select (v => v.Message).OfType<string> ().ToArray ();
				Assert.That (string.Join ("\n", errors), Does.Contain ("No applicable and available devices found."));
				return;
			}

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchRunArguments", out var mlaunchRunArguments))
				Assert.Fail ("Could not find the property 'MlaunchRunArguments' in the binlog.");

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchPath", out var mlaunchPath))
				Assert.Fail ("Could not find the property 'MlaunchPath' in the binlog.");
			Assert.That (mlaunchPath, Does.Exist, "mlaunch existence");

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "RunCommand", out var runCommand))
				Assert.Fail ("Could not find the property 'RunCommand' in the binlog.");
			Assert.That (runCommand, Is.EqualTo ($"{mlaunchPath}"), "Run command");

			var expectedArguments = new StringBuilder ();
			var isSim = runtimeIdentifiers.Contains ("simulator");
			expectedArguments.Append (isSim ? "--launchsim " : "--launchdev ");
			expectedArguments.Append (appPath.Substring (Path.GetDirectoryName (project_path)!.Length + 1)).Append ('/');
			if (isSim) {
				expectedArguments.Append (" --device ");
				expectedArguments.Append (device);
			}
			expectedArguments.Append ($" --wait-for-exit:true");
			Assert.That (mlaunchRunArguments, Does.Match (expectedArguments.ToString ()), "arguments");

			var scriptContents = File.ReadAllText (outputPath).Trim ('\n');
			var expectedScriptContents = $"'{mlaunchPath}' " + expectedArguments.ToString ();
			Assert.That (scriptContents, Does.Match (expectedScriptContents), "Script contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64;iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		public void GetMlaunchRunArguments_NewProperties (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var outputPath = Path.Combine (tmpdir, "launch.sh");
			var stdoutPath = Path.Combine (tmpdir, "stdout.txt");
			var stderrPath = Path.Combine (tmpdir, "stderr.txt");
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCodeSigning"] = "false";

			// Create the app manifest first, since it's required to compute the mlaunch run arguments
			DotNet.Execute ("build", project_path, properties, target: "_DetectSdkLocations;_DetectAppManifest;_CompileAppManifest;_WriteAppManifest");

			// Set the new properties (these should take precedence over the _Mlaunch-prefixed ones)
			properties ["MlaunchRunScript"] = outputPath;
			properties ["WaitForExit"] = "false";
			properties ["StandardOutputPath"] = stdoutPath;
			properties ["StandardErrorPath"] = stderrPath;
			var rv = DotNet.Execute ("build", project_path, properties, assert_success: false, target: "ComputeMlaunchRunArguments");

			if (rv.ExitCode != 0) {
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).Select (v => v.Message).OfType<string> ().ToArray ();
				Assert.That (string.Join ("\n", errors), Does.Contain ("No applicable and available devices found."));
				return;
			}

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchRunArguments", out var mlaunchRunArguments))
				Assert.Fail ("Could not find the property 'MlaunchRunArguments' in the binlog.");

			Assert.Multiple (() => {
				Assert.That (mlaunchRunArguments, Does.Contain ("--wait-for-exit:false"), "WaitForExit should be false");
				Assert.That (mlaunchRunArguments, Does.Contain ($"--stdout {stdoutPath}"), "StandardOutputPath");
				Assert.That (mlaunchRunArguments, Does.Contain ($"--stderr {stderrPath}"), "StandardErrorPath");
			});
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64;iossimulator-arm64")]
		public void GetMlaunchRunArguments_NewPropertiesTakePrecedence (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var outputPath = Path.Combine (tmpdir, "launch.sh");
			var stdoutNew = Path.Combine (tmpdir, "stdout-new.txt");
			var stderrNew = Path.Combine (tmpdir, "stderr-new.txt");
			var stdoutOld = Path.Combine (tmpdir, "stdout-old.txt");
			var stderrOld = Path.Combine (tmpdir, "stderr-old.txt");
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["EnableCodeSigning"] = "false";

			// Create the app manifest first, since it's required to compute the mlaunch run arguments
			DotNet.Execute ("build", project_path, properties, target: "_DetectSdkLocations;_DetectAppManifest;_CompileAppManifest;_WriteAppManifest");

			// Set both old and new properties - new ones should take precedence
			properties ["MlaunchRunScript"] = outputPath;
			properties ["WaitForExit"] = "false";
			properties ["_MlaunchWaitForExit"] = "true";
			properties ["StandardOutputPath"] = stdoutNew;
			properties ["_MlaunchStandardOutputPath"] = stdoutOld;
			properties ["StandardErrorPath"] = stderrNew;
			properties ["_MlaunchStandardErrorPath"] = stderrOld;
			var rv = DotNet.Execute ("build", project_path, properties, assert_success: false, target: "ComputeMlaunchRunArguments");

			if (rv.ExitCode != 0) {
				var errors = BinLog.GetBuildLogErrors (rv.BinLogPath).Select (v => v.Message).OfType<string> ().ToArray ();
				Assert.That (string.Join ("\n", errors), Does.Contain ("No applicable and available devices found."));
				return;
			}

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "MlaunchRunArguments", out var mlaunchRunArguments))
				Assert.Fail ("Could not find the property 'MlaunchRunArguments' in the binlog.");

			Assert.Multiple (() => {
				Assert.That (mlaunchRunArguments, Does.Contain ("--wait-for-exit:false"), "WaitForExit (new) should take precedence");
				Assert.That (mlaunchRunArguments, Does.Contain ($"--stdout {stdoutNew}"), "StandardOutputPath (new) should take precedence");
				Assert.That (mlaunchRunArguments, Does.Not.Contain (stdoutOld), "Old StandardOutputPath should not be used");
				Assert.That (mlaunchRunArguments, Does.Contain ($"--stderr {stderrNew}"), "StandardErrorPath (new) should take precedence");
				Assert.That (mlaunchRunArguments, Does.Not.Contain (stderrOld), "Old StandardErrorPath should not be used");
			});
		}

		[Test]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void GetDesktopRunArguments_WaitForExitTakesPrecedence (ApplePlatform platform)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, platform: platform);
			var properties = GetDefaultProperties ();
			properties ["EnableCodeSigning"] = "false";

			// Build first so that _AppBundleName and other properties are available
			DotNet.AssertBuild (project_path, properties);

			// Set WaitForExit=true, which should take precedence over OpenWaitForExit=false
			properties ["WaitForExit"] = "true";
			properties ["OpenWaitForExit"] = "false";
			var rv = DotNet.Execute ("build", project_path, properties, target: "_PrepareRunDesktop");

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "RunArguments", out var runArguments))
				Assert.Fail ("Could not find the property 'RunArguments' in the binlog.");

			Assert.That (runArguments, Does.Contain ("-W"), "WaitForExit=true should result in -W in RunArguments");
		}

		[Test]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void GetDesktopRunArguments_WaitForExitFalse (ApplePlatform platform)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, platform: platform);
			var properties = GetDefaultProperties ();
			properties ["EnableCodeSigning"] = "false";

			// Build first so that _AppBundleName and other properties are available
			DotNet.AssertBuild (project_path, properties);

			// Set WaitForExit=false, which should take precedence over OpenWaitForExit=true
			properties ["WaitForExit"] = "false";
			properties ["OpenWaitForExit"] = "true";
			var rv = DotNet.Execute ("build", project_path, properties, target: "_PrepareRunDesktop");

			if (!BinLog.TryFindPropertyValue (rv.BinLogPath, "RunArguments", out var runArguments))
				Assert.Fail ("Could not find the property 'RunArguments' in the binlog.");

			Assert.That (runArguments, Does.Not.Contain ("-W"), "WaitForExit=false should result in no -W in RunArguments");
		}
	}
}
