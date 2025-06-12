using Microsoft.Build.Logging.StructuredLogger;

#nullable enable

namespace Xamarin.Tests {
	public class IncrementalBuildTest : TestBaseClass {
		[Test]
		// this test is fairly slow, so execute on one arch only
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Link (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "IncrementalTestApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			properties ["UseInterpreter"] = "true"; // this makes the test faster

			// Build the first time
			DotNet.AssertBuild (project_path, properties);

			// Make sure it runs successfully (if on desktop)
			var appExecutable = GetNativeExecutable (platform, appPath);
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Must not link with the frameworks from the nuget
			var lc_load_dylib = GetLoadCommands (appExecutable).ToArray ();
			Assert.That (lc_load_dylib, Does.Not.Contain ("@rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1"), "A: Should not link with @rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1");
			Assert.That (lc_load_dylib, Does.Not.Contain ("@rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2"), "A: Should not link with @rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2");

			// Capture when executable was created
			var appExecutableTimestamp = File.GetLastWriteTimeUtc (appExecutable);

			// Build again, adding a package with frameworks
			properties ["IncludeFwInRuntimesNativeDirectory"] = "true";
			DotNet.AssertBuild (project_path, properties);

			// Executing should work just fine
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Must link with the frameworks from the nuget
			lc_load_dylib = GetLoadCommands (appExecutable).ToArray ();
			Assert.That (lc_load_dylib, Does.Contain ("@rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1"), "B: Should link with @rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1");
			Assert.That (lc_load_dylib, Does.Contain ("@rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2"), "B: Should link with @rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2");

			// The main executable must be modified
			Assert.That (File.GetLastWriteTimeUtc (appExecutable), Is.GreaterThan (appExecutableTimestamp), "Modified B");

			// Capture when executable was rebuilt
			appExecutableTimestamp = File.GetLastWriteTimeUtc (appExecutable);

			// Build again, not doing anything
			DotNet.AssertBuild (project_path, properties);

			// Executing should work just fine
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Must still link with the frameworks from the nuget
			lc_load_dylib = GetLoadCommands (appExecutable).ToArray ();
			Assert.That (lc_load_dylib, Does.Contain ("@rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1"), "C: Should link with @rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1");
			Assert.That (lc_load_dylib, Does.Contain ("@rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2"), "C: Should link with @rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2");

			// The main executable must not be modified
			Assert.That (File.GetLastWriteTimeUtc (appExecutable), Is.EqualTo (appExecutableTimestamp), "Modified C");

			// Build yet again, now removing the package
			properties.Remove ("IncludeFwInRuntimesNativeDirectory");
			DotNet.AssertBuild (project_path, properties);

			// Executing should work just fine
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Must not link with the frameworks from the nuget anymore
			lc_load_dylib = GetLoadCommands (appExecutable).ToArray ();
			Assert.That (lc_load_dylib, Does.Not.Contain ("@rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1"), "D: Should not link with @rpath/FrameworksInRuntimesNativeDirectory1.framework/FrameworksInRuntimesNativeDirectory1");
			Assert.That (lc_load_dylib, Does.Not.Contain ("@rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2"), "D: Should not link with @rpath/FrameworksInRuntimesNativeDirectory2.framework/FrameworksInRuntimesNativeDirectory2");

			// The main executable must be modified
			Assert.That (File.GetLastWriteTimeUtc (appExecutable), Is.GreaterThan (appExecutableTimestamp), "Modified D");
		}

		static IEnumerable<string> GetLoadCommands (string dylib)
		{
			var file = MachO.Read (dylib).Single ();
			foreach (var lc in file.load_commands) {
				if (lc is DylibLoadCommand dlc)
					yield return dlc.name;
			}
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void NativeLink (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GenerateProject (platform, name: nameof (NativeLink), runtimeIdentifiers: runtimeIdentifiers, out var appPath);
			var properties = new Dictionary<string, string> (verbosity);
			SetRuntimeIdentifiers (properties, runtimeIdentifiers);

			var mainContents = @"
class MainClass {
	static int Main ()
	{
		return 123;
	}
}
";
			var mainFile = Path.Combine (Path.GetDirectoryName (project_path)!, "Main.cs");

			File.WriteAllText (mainFile, mainContents);

			// Build the first time
			var rv = DotNet.AssertBuild (project_path, properties);
			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_AOTCompile", "A");
			AssertTargetExecuted (allTargets, "_CompileNativeExecutable", "A");
			AssertTargetExecuted (allTargets, "_LinkNativeExecutable", "A");

			// Capture the current time
			var timestamp = DateTime.UtcNow;
			File.WriteAllText (mainFile, mainContents);

			// Build again
			rv = DotNet.AssertBuild (project_path, properties);

			// Check that some targets executed
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			AssertTargetExecuted (allTargets, "_AOTCompile", "B");
			AssertTargetNotExecuted (allTargets, "_CompileNativeExecutable", "B");
			AssertTargetExecuted (allTargets, "_LinkNativeExecutable", "B");

			// Verify that the timestamp of the executable has been updated
			var executable = GetNativeExecutable (platform, appPath!);
			Assert.That (File.GetLastWriteTimeUtc (executable), Is.GreaterThan (timestamp), "B: Executable modified");
		}

		[Test]
		// this test is fairly slow, so execute on one arch only
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void Interpreter (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			// Build with the interpreter disabled
			properties ["UseInterpreter"] = "false";
			DotNet.AssertBuild (project_path, properties);

			// Make sure it runs successfully (if on desktop)
			var appExecutable = GetNativeExecutable (platform, appPath);
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Capture when executable was created
			var appExecutableTimestamp = File.GetLastWriteTimeUtc (appExecutable);

			// Build again, now enabling the interpreter
			Configuration.Touch (project_path);
			properties ["UseInterpreter"] = "true";
			DotNet.AssertBuild (project_path, properties);

			// Executing should work just fine
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// The main executable must be modified
			Assert.That (File.GetLastWriteTimeUtc (appExecutable), Is.GreaterThan (appExecutableTimestamp), "Modified A");

			// Capture when executable was rebuilt
			appExecutableTimestamp = File.GetLastWriteTimeUtc (appExecutable);

			// Build again, not doing anything
			DotNet.AssertBuild (project_path, properties);

			// Executing should work just fine
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// The main executable must not be modified
			Assert.That (File.GetLastWriteTimeUtc (appExecutable), Is.EqualTo (appExecutableTimestamp), "Modified B");
		}

	}
}
