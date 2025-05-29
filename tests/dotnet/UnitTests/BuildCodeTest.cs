#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class BuildCodeTest : TestBaseClass {

		static IEnumerable<TestCaseData> MonoComponentsSource {
			get {
				yield return new TestCaseData (ApplePlatform.iOS, "ios-arm64", new string [] {
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-static.a",
					"libmono-component-diagnostics_tracing-static.a",
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
				yield return new TestCaseData (ApplePlatform.iOS, "ios-arm64", new string [] {
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-static.a",
					"libmono-component-diagnostics_tracing-static.a",
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
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-static.a",
					"libmono-component-diagnostics_tracing-static.a",
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
					"libmono-component-debugger-static.a",
					"libmono-component-debugger-static.a",
					"libmono-component-diagnostics_tracing-static.a",
					"libmono-component-diagnostics_tracing-static.a",
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
					"libSystem.Security.Cryptography.Native.OpenSsl.dylib",
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
			var properties = GetDefaultProperties (runtimeIdentifiers);
			var result = DotNet.GetItems (project_path, "_MonoLibrary", target: "ResolveFrameworkReferences;_ComputeVariables;_ComputeMonoLibraries", properties: properties);
			var libs = result.
							Split (['\n', '\r']).
							Where (v => v.Contains ("\"Identity\": ")).
							Select (v => v.Replace ("\"Identity\":", "")).
							Select (v => v.Trim (new char [] { ' ', '\t', '"', ',' })).
							Select (Path.GetFileName).
							OrderBy (v => v).
							ToArray ();
			Assert.That (libs, Is.EqualTo (expectedLibraries), "libs");
		}
	}
}
