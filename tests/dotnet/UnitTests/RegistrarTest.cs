using Mono.Cecil;

namespace Xamarin.Tests {
	[TestFixture]
	public class RegistrarTest : TestBaseClass {
		// This test does evil things that the AOT runtime complains about, so it only works when not running the AOT compiler (aka x64 when using Mono).
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64", true)]
		[TestCase (ApplePlatform.MacOSX, null, true)]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64", false)]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-x64", false)]
		public void InvalidStaticRegistrarValidation (ApplePlatform platform, string? runtimeIdentifiers, bool validated)
		{
			var project = "MyRegistrarApp";
			var configuration = "Debug";

			runtimeIdentifiers ??= GetDefaultRuntimeIdentifier (platform);

			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Registrar"] = "static";
			// enable the linker (so that the main assembly is modified)
			properties ["LinkMode"] = "full";
			properties ["MtouchLink"] = "full";

			DotNet.AssertBuild (projectPath, properties);

			var appDir = GetAppPath (projectPath, platform, runtimeIdentifiers, configuration);
			var asmDir = Path.Combine (appDir, GetRelativeAssemblyDirectory (platform));
			var appExecutable = Path.Combine (asmDir, project + ".dll");

			// Save the first version of the main assembly in memory
			var firstAssembly = File.ReadAllBytes (appExecutable);

			// Build again, including additional code
			properties ["AdditionalDefineConstants"] = "INCLUDED_ADDITIONAL_CODE";
			DotNet.AssertBuild (projectPath, properties);

			// Revert to the original version of the main assembly
			File.WriteAllBytes (appExecutable, firstAssembly);

			Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", "1");
			try {
				if (validated) {
					ExecuteProjectWithMagicWordAndAssert (projectPath, platform, runtimeIdentifiers);
				} else if (CanExecute (platform, runtimeIdentifiers)) {
					var rv = base.Execute (GetNativeExecutable (platform, appDir), out var output, out _);
					Assert.That (rv.ExitCode, Is.EqualTo (1), "Expected no validation");
				}
			} finally {
				Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", null);
			}
		}

		[TestCase (ApplePlatform.MacCatalyst, false)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.MacCatalyst, true)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.TVOS, true)]
		public void ClassRewriterTest (ApplePlatform platform, bool rewriteHandles)
		{
			var project = "MyClassRedirectApp";
			var configuration = "Debug";
			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath (project, platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties ();
			properties ["Registrar"] = "static";
			// enable the linker (so that the main assembly is modified)
			properties ["LinkMode"] = "full";
			properties ["MtouchLink"] = "full";
			properties ["InlineClassGetHandle"] = "disabled";
			if (rewriteHandles)
				properties ["MtouchExtraArgs"] = "--optimize=redirect-class-handles";

			DotNet.AssertBuild (projectPath, properties);

			var appDir = GetAppPath (projectPath, platform, runtimeIdentifiers, configuration);
			var asmDir = Path.Combine (appDir, GetRelativeAssemblyDirectory (platform));

			var appExecutable = Path.Combine (asmDir, project + ".dll");
			var platformDll = Path.Combine (asmDir, Configuration.GetBaseLibraryName (platform));
			Assert.That (File.Exists (platformDll), "No platform dll.");
			var module = ModuleDefinition.ReadModule (platformDll);
			var classHandlesMaybe = AllTypes (module).FirstOrDefault (t => t.FullName == "ObjCRuntime.Runtime/ClassHandles");
			Assert.That (classHandlesMaybe, Is.Not.Null, "Couldn't find ClassHandles type.");
			var classHandles = classHandlesMaybe!;
			if (!rewriteHandles) {
				// NB: there is always at least one field named "unused"
				var fields = classHandles.Fields.Where (f => f.Name != "unused").Select (f => f.Name).ToList ();
				var sb = new StringBuilder ();
				foreach (var f in fields) {
					sb.Append (" ").Append (f);
				}
				Assert.That (fields.Count == 0, "There are fields in classHandles - rewriter was called when it should have done nothing." + sb);
			} else {
				// NB: there is always at least one field named "unused"
				Assert.That (classHandles.HasFields && classHandles.Fields.Count () > 1, "There are no fields in ClassHandles - rewriter did nothing.");
				var field = classHandles.Fields.FirstOrDefault (f => f.Name.Contains ("SomeObj"));
				Assert.That (field, Is.Not.Null, "Didn't find a field for 'SomeObj'");
			}
		}

		// Ref: https://github.com/dotnet/macios/issues/24869
		// Extensions crash at runtime when using the managed-static registrar because
		// the registrar's function table has a zero entry for the extension class's constructor callback.
		// The default registrar for iOS/tvOS device builds is managed-static, so extensions crash by default on device.
		// [TestCase (ApplePlatform.iOS)]
		// [TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		public void ExtensionWithManagedStaticRegistrar (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var dotnetTestDir = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "AudioUnitExtension");
			var platformName = platform.AsString ();
			var containerProjectPath = Path.Combine (dotnetTestDir, "ContainerApp", platformName, "ContainerApp.csproj");
			var extensionProjectPath = Path.Combine (dotnetTestDir, "AppExtension", platformName, "AppExtension.csproj");
			var appPath = Path.Combine (Path.GetDirectoryName (containerProjectPath)!, "bin", "Debug", platform.ToFramework (), runtimeIdentifiers, "ContainerApp.app");

			Clean (extensionProjectPath);
			Clean (containerProjectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Registrar"] = "managed-static";

			DotNet.AssertBuild (containerProjectPath, properties);

			var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), "AppExtension.appex");
			Assert.That (Directory.Exists (extensionPath), Is.True, $"App extension directory does not exist: {extensionPath}");

			if (CanExecute (platform, runtimeIdentifiers)) {
				// Verify the host app can be executed.
				ExecuteProjectWithMagicWordAndAssert (containerProjectPath, platform, runtimeIdentifiers);

				if (platform == ApplePlatform.MacOSX)
					TriggerAudioUnitExtension (appPath, extensionPath);
			}
		}

		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void MonotouchTestInAudioUnitExtension (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var extensionRoot = Path.Combine (Configuration.SourceRoot, "tests", "monotouch-test", "dotnet", "extensions", "audio-unit", platform.AsString ());
			var containerProjectPath = Path.Combine (extensionRoot, "ContainerApp", "ContainerApp.csproj");
			var extensionProjectPath = Path.Combine (extensionRoot, "AppExtension", "AppExtension.csproj");
			var appPath = Path.Combine (Path.GetDirectoryName (containerProjectPath)!, "bin", "Debug", platform.ToFramework (), runtimeIdentifiers, "ContainerApp.app");

			Clean (extensionProjectPath);
			Clean (containerProjectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Registrar"] = "managed-static";
			properties ["MonotouchExtensionTestName"] = "MonoTouchFixtures.AudioUnit.AppExtensionSmokeTest";

			DotNet.AssertBuild (containerProjectPath, properties);

			var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), "AppExtension.appex");
			Assert.That (Directory.Exists (extensionPath), Is.True, $"App extension directory does not exist: {extensionPath}");

			if (CanExecute (platform, runtimeIdentifiers)) {
				ExecuteProjectWithMagicWordAndAssert (containerProjectPath, platform, runtimeIdentifiers);

				var logText = TriggerAudioUnitExtension (appPath, extensionPath, "MonoTouchFixtures.AudioUnit.AppExtensionSmokeTest");
				Assert.That (logText, Does.Contain ("[monotouch-test-audio-unit-extension] Starting monotouch-test audio unit extension test run"), "Start marker");
				Assert.That (logText, Does.Contain ("MonoTouchFixtures.AudioUnit.AppExtensionSmokeTest"), "Smoke test selection");
				Assert.That (logText, Does.Contain ("[monotouch-test-audio-unit-extension] Finished monotouch-test audio unit extension test run. Passed: 1 Failed: 0"), "Summary");
			}
		}

		// Trigger the Audio Unit extension via auvaltool to verify the extension
		// actually loads and runs with the managed-static registrar.
		// This requires:
		// 1. A non-ad-hoc signing certificate (extension discovery requires a team ID)
		// 2. The host app to be registered with Launch Services (for extension discovery)
		// 3. auvaltool -v to trigger Audio Unit validation which loads the extension
		string TriggerAudioUnitExtension (string appPath, string extensionPath, string? testName = null)
		{
			int exitCode;
			StringBuilder output;
			var testFilterFile = Path.Combine (extensionPath, "Contents", "Resources", "monotouch-extension-test-filter.txt");
			var hostTestFilterFile = Path.Combine (appPath, "Contents", "Resources", "monotouch-extension-test-filter.txt");

			// Register the app with Launch Services so the system discovers the
			// extension and its AudioComponents.
			var lsregister = "/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister";
			Console.WriteLine ($"Executing: {lsregister} -f {appPath}");
			exitCode = ExecutionHelper.Execute (lsregister, new [] { "-f", appPath }, out output, (string) null!);
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);

			// Register the extension explicitly with pluginkit.
			Console.WriteLine ($"Executing: pluginkit -a {extensionPath}");
			exitCode = ExecutionHelper.Execute ("pluginkit", new [] { "-a", extensionPath }, out output, (string) null!);
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);

			try {
				if (string.IsNullOrEmpty (testName)) {
					ExecutionHelper.Execute ("defaults", new [] { "delete", "com.xamarin.monotouch-test.AudioUnitExtension", "test.name" }, out output, (string) null!);
					if (File.Exists (testFilterFile))
						File.Delete (testFilterFile);
					if (File.Exists (hostTestFilterFile))
						File.Delete (hostTestFilterFile);
				} else {
					exitCode = ExecutionHelper.Execute ("defaults", new [] { "write", "com.xamarin.monotouch-test.AudioUnitExtension", "test.name", "-string", testName }, out output, (string) null!);
					Directory.CreateDirectory (Path.GetDirectoryName (testFilterFile)!);
					Directory.CreateDirectory (Path.GetDirectoryName (hostTestFilterFile)!);
					File.WriteAllText (testFilterFile, testName);
					File.WriteAllText (hostTestFilterFile, testName);
				}

				// Record the current time so we can query system logs after triggering.
				var logStartTime = DateTime.Now;

				// Run auvaltool to validate the Audio Unit, which triggers the system
				// to discover and launch the extension process.
				// aufx = effect type, test = subtype, Xmrn = manufacturer (matching Info.plist).
				// auvaltool may fail validation (the AU is minimal), but the system will
				// still attempt to load the extension process.
				Console.WriteLine ("Executing: auvaltool -v aufx test Xmrn");
				exitCode = ExecutionHelper.Execute ("auvaltool", new [] { "-v", "aufx", "test", "Xmrn" }, out output, (string) null!, timeout: TimeSpan.FromMinutes (2));
				Console.WriteLine ($"Exit code: {exitCode}");
				Console.WriteLine (output);
				var auvalOutput = output.ToString ();
				Assert.That (auvalOutput, Does.Contain ("Loaded AudioUnit out-of-process: true"),
					"auvaltool did not report loading the audio unit extension out-of-process.");

				// Check system logs for evidence the extension process was launched.
				var logEnd = DateTime.Now;
				var logStartStr = logStartTime.ToString ("yyyy-MM-dd HH:mm:ss");
				var logEndStr = logEnd.ToString ("yyyy-MM-dd HH:mm:ss");
				var logArgs = new [] {
					"show",
					"--style", "compact",
					"--predicate", "process == \"AppExtension\" OR eventMessage CONTAINS[c] \"monotouch-test-audio-unit-extension\" OR eventMessage CONTAINS[c] \"AppExtensionSmokeTest\"",
					"--start", logStartStr,
					"--end", logEndStr,
				};
				Console.WriteLine ($"Executing: log {string.Join (" ", logArgs)}");
				exitCode = ExecutionHelper.Execute ("log", logArgs, out output, (string) null!);
				Console.WriteLine ($"Exit code: {exitCode}");
				var logText = output.ToString ();
				Console.WriteLine (logText);
				return auvalOutput + Environment.NewLine + logText;
			} finally {
				ExecutionHelper.Execute ("defaults", new [] { "delete", "com.xamarin.monotouch-test.AudioUnitExtension", "test.name" }, out output, (string) null!);
				if (File.Exists (testFilterFile))
					File.Delete (testFilterFile);
				if (File.Exists (hostTestFilterFile))
					File.Delete (hostTestFilterFile);
			}
		}

		[TestCase (ApplePlatform.MacOSX)]
		public void SpotlightImportExtension (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var dotnetTestDir = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "SpotlightImportExtension");
			var platformName = platform.AsString ();
			var containerProjectPath = Path.Combine (dotnetTestDir, "ContainerApp", platformName, "ContainerApp.csproj");
			var extensionProjectPath = Path.Combine (dotnetTestDir, "AppExtension", platformName, "AppExtension.csproj");
			var appPath = Path.Combine (Path.GetDirectoryName (containerProjectPath)!, "bin", "Debug", platform.ToFramework (), runtimeIdentifiers, "ContainerApp.app");

			Clean (extensionProjectPath);
			Clean (containerProjectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);

			DotNet.AssertBuild (containerProjectPath, properties);

			var extensionPath = Path.Combine (appPath, GetPlugInsRelativePath (platform), "AppExtension.appex");
			Assert.That (Directory.Exists (extensionPath), Is.True, $"App extension directory does not exist: {extensionPath}");

			if (CanExecute (platform, runtimeIdentifiers)) {
				ExecuteProjectWithMagicWordAndAssert (containerProjectPath, platform, runtimeIdentifiers);

				if (platform == ApplePlatform.MacOSX)
					TriggerSpotlightImportExtension (appPath, extensionPath);
			}
		}

		void TriggerSpotlightImportExtension (string appPath, string extensionPath)
		{
			int exitCode;
			StringBuilder output;

			var lsregister = "/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister";
			Console.WriteLine ($"Executing: {lsregister} -f {appPath}");
			exitCode = ExecutionHelper.Execute (lsregister, new [] { "-f", appPath }, out output, (string) null!);
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);

			Console.WriteLine ($"Executing: pluginkit -a {extensionPath}");
			exitCode = ExecutionHelper.Execute ("pluginkit", new [] { "-a", extensionPath }, out output, (string) null!);
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);

			// Create a test file with the .xspotlight extension.
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var testFile = Path.Combine (tmpDir, "test.xspotlight");
			File.WriteAllText (testFile, "spotlight test content");

			// Use mdimport -t -o to test-import the file and capture the
			// imported attributes to a plist file. This verifies the UTI
			// (com.xamarin.test-spotlight) is correctly registered.
			var outFile = Path.Combine (tmpDir, "attributes.plist");
			Console.WriteLine ($"Executing: mdimport -t -d3 -o {outFile} {testFile}");
			exitCode = ExecutionHelper.Execute ("mdimport", new [] { "-t", "-d3", "-o", outFile, testFile }, out output, (string) null!, timeout: TimeSpan.FromSeconds (15));
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);
			Assert.That (File.Exists (outFile), Is.True, "mdimport did not produce an output file");
			var attributes = File.ReadAllText (outFile);
			Console.WriteLine (attributes);
			Assert.That (attributes, Does.Contain ("com.xamarin.test-spotlight"),
				"The imported attributes should contain the custom UTI.");

			var logStartTime = DateTime.Now;

			// Use mdimport -m to trigger the modern (app extension based) importer,
			// which actually launches our extension process.
			Console.WriteLine ($"Executing: mdimport -m -y com.xamarin.test-spotlight -u file://{testFile}");
			exitCode = ExecutionHelper.Execute ("mdimport", new [] { "-m", "-y", "com.xamarin.test-spotlight", "-u", $"file://{testFile}" }, out output, (string) null!, timeout: TimeSpan.FromSeconds (15));
			Console.WriteLine ($"Exit code: {exitCode}");
			Console.WriteLine (output);

			// Check system logs for evidence the extension process was launched.
			var logEnd = DateTime.Now;
			var logStartStr = logStartTime.ToString ("yyyy-MM-dd HH:mm:ss");
			var logEndStr = logEnd.ToString ("yyyy-MM-dd HH:mm:ss");
			var logArgs = new [] {
				"show",
				"--predicate", "eventMessage CONTAINS \"SpotlightImportExtensionTest\"",
				"--start", logStartStr,
				"--end", logEndStr,
			};
			Console.WriteLine ($"Executing: log {string.Join (" ", logArgs)}");
			exitCode = ExecutionHelper.Execute ("log", logArgs, out output, (string) null!);
			Console.WriteLine ($"Exit code: {exitCode}");
			var logText = output.ToString ();
			Console.WriteLine (logText);
			Assert.That (logText, Does.Contain ("SpotlightImportExtensionTest"),
				"The Spotlight import extension process was not launched by the system.");
		}

		static string? FindCodesignCertificate ()
		{
			var rv = ExecutionHelper.Execute ("security", new [] { "find-identity", "-v", "-p", "codesigning" }, out var output);
			if (rv != 0)
				return null;

			// Parse output lines like:
			//   1) C884... "Apple Development: Name (ID)"
			// Skip ad-hoc ("-") and pick the first valid certificate.
			foreach (var line in output.ToString ().Split ('\n')) {
				var trimmed = line.Trim ();
				if (!trimmed.Contains (')'))
					continue;
				var parts = trimmed.Split (')');
				if (parts.Length < 2)
					continue;
				var afterParen = parts [0];
				var sha = afterParen.Split (' ').LastOrDefault ()?.Trim ();
				if (string.IsNullOrEmpty (sha) || sha.Length != 40)
					continue;
				if (trimmed.Contains ("\"Apple Development:") || trimmed.Contains ("\"Developer ID Application:"))
					return sha;
			}

			return null;
		}

		IEnumerable<TypeDefinition> AllTypes (ModuleDefinition module)
		{
			foreach (var type in module.Types) {
				yield return type;
				foreach (var t in InnerTypes (type))
					yield return t;
			}
		}

		IEnumerable<TypeDefinition> InnerTypes (TypeDefinition type)
		{
			if (type.HasNestedTypes) {
				foreach (var t in type.NestedTypes) {
					yield return t;
					foreach (var nt in InnerTypes (t))
						yield return nt;
				}
			}
		}
	}
}
