#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class BuildCodeTest : TestBaseClass {

		static IEnumerable<TestCaseData> MonoComponentsSource {
			get {
				yield return new TestCaseData (ApplePlatform.iOS, "ios-arm64", new string [] {
					"libbrotlicommon.a",
					"libbrotlidec.a",
					"libbrotlienc.a",
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmonosgen-2.0.a",
					"libSystem.Globalization.Native.a",
					"libSystem.IO.Compression.Native.a",
					"libSystem.Native.a",
					"libSystem.Net.Security.Native.a",
					"libSystem.Security.Cryptography.Native.Apple.a",
				});
				yield return new TestCaseData (ApplePlatform.iOS, "iossimulator-arm64", new string [] {
					"libmono-component-debugger.dylib",
					"libmono-component-diagnostics_tracing.dylib",
					"libmono-component-hot_reload.dylib",
					"libmono-component-marshal-ilgen.dylib",
					"libmonosgen-2.0.dylib",
					"libSystem.Globalization.Native.dylib",
					"libSystem.IO.Compression.Native.dylib",
					"libSystem.Native.dylib",
					"libSystem.Net.Security.Native.dylib",
					"libSystem.Security.Cryptography.Native.Apple.dylib",
				});
				yield return new TestCaseData (ApplePlatform.MacCatalyst, "maccatalyst-arm64", new string [] {
					"libbrotlicommon.a",
					"libbrotlidec.a",
					"libbrotlienc.a",
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmonosgen-2.0.a",
					"libSystem.Globalization.Native.a",
					"libSystem.IO.Compression.Native.a",
					"libSystem.Native.a",
					"libSystem.Net.Security.Native.a",
					"libSystem.Security.Cryptography.Native.Apple.a",
				});
				yield return new TestCaseData (ApplePlatform.TVOS, "tvossimulator-arm64", new string [] {
					"libmono-component-debugger.dylib",
					"libmono-component-diagnostics_tracing.dylib",
					"libmono-component-hot_reload.dylib",
					"libmono-component-marshal-ilgen.dylib",
					"libmonosgen-2.0.dylib",
					"libSystem.Globalization.Native.dylib",
					"libSystem.IO.Compression.Native.dylib",
					"libSystem.Native.dylib",
					"libSystem.Security.Cryptography.Native.Apple.dylib",
				});
				yield return new TestCaseData (ApplePlatform.TVOS, "tvos-arm64", new string [] {
					"libbrotlicommon.a",
					"libbrotlidec.a",
					"libbrotlienc.a",
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-diagnostics_tracing-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-hot_reload-stub-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmono-component-marshal-ilgen-static.a",
					"libmonosgen-2.0.a",
					"libSystem.Globalization.Native.a",
					"libSystem.IO.Compression.Native.a",
					"libSystem.Native.a",
					"libSystem.Security.Cryptography.Native.Apple.a",
				});
				yield return new TestCaseData (ApplePlatform.MacOSX, "osx-arm64", new string [] {
					"libclrgc.dylib",
					"libclrgcexp.dylib",
					"libclrjit.dylib",
					"libcoreclr.dylib",
					"libhostfxr.dylib",
					"libhostpolicy.dylib",
					"libmscordaccore.dylib",
					"libmscordbi.dylib",
					"libSystem.Globalization.Native.dylib",
					"libSystem.IO.Compression.Native.dylib",
					"libSystem.Native.dylib",
					"libSystem.Net.Security.Native.dylib",
					"libSystem.Security.Cryptography.Native.Apple.dylib",
				});
			}
		}

		[Test]
		[Category ("RemoteWindowsInclusive")]
		[TestCaseSource (nameof (MonoComponentsSource))]
		public void MonoComponents (ApplePlatform platform, string runtimeIdentifiers, string [] expectedLibraries)
		{
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			// we don't need to connect to the remote mac for this test
			var properties = GetDefaultProperties (runtimeIdentifiers, includeRemoteProperties: false);
			var environment = new Dictionary<string, string?> ();
			environment ["ServerAddress"] = null;
			environment ["ServerUser"] = null;
			environment ["ServerPassword"] = null;

			var result = DotNet.GetItems (project_path, "_MonoLibrary", target: "ResolveFrameworkReferences;_ComputeVariables;_ComputeMonoLibraries", properties: properties, environment: environment);
			var libs = result.
							Split (['\n', '\r']).
							Where (v => v.Contains ("\"Identity\": ")).
							Select (v => v.Replace ("\"Identity\":", "")).
							Select (v => v.Trim (new char [] { ' ', '\t', '"', ',' })).
							Select (Path.GetFileName).
							OrderBy (v => v).
							ToArray ();

			// NUnit's rendering of assertions failures if the arrays are different is rather lacking.
			if (expectedLibraries.Length != libs.Length) {
				Assert.Fail ($"Expected {expectedLibraries.Length} components, got {libs.Length} components.\nExpected:\n\t{string.Join ("\n\t", expectedLibraries)}\nActual:\n\t{string.Join ("\n\t", libs)}");
			} else {
				for (var i = 0; i < expectedLibraries.Length; i++) {
					if (expectedLibraries [i] == libs [i])
						continue;
					Assert.Fail ($"Expected {expectedLibraries [i]} at index #{i}, got {libs [i]}.\nExpected:\n\t{string.Join ("\n\t", expectedLibraries)}\nActual:\n\t{string.Join ("\n\t", libs)}");
				}
			}
		}
	}
}
