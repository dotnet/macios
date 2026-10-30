// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Sharpie.Bind.Tests;

public class SdkDbTest {

	[TestCase (null)]
	[TestCase (true)]
	[TestCase (false)]
	public void CommandLineModules (bool? modules)
	{
		Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.MacOSX);
		var args = new List<string> {
			"--sdk=macosx",
			"--output=" + Cache.CreateTemporaryDirectory (),
			"--clang-resource-dir=" + Extensions.GetClangResourceDirectory (),
		};
		var sdk = new Sdk (new BindingResult (), "macosx");
		args.AddRange (sdk.Frameworks.Where (fx => fx.Name != "Foundation").Select (fx => "--exclude=" + fx.Name));
		if (modules.HasValue)
			args.Add ("--modules=" + modules.Value);
		using var binder = Tools.CreateSdkDbTool (args.ToArray ()) ?? throw new InvalidOperationException ("Expected an SDK tool.");
		Assert.That (binder.EnableModules, Is.EqualTo (modules ?? true), "Module setting");
		var translationUnit = binder.CreateTranslationUnit ();
		binder.BindingResult.AssertSuccess (null);
		binder.BindingResult.AssertNoWarnings ();
		Assert.That (translationUnit, Is.Not.Null, "Translation unit");
	}

	static IEnumerable<TestCaseData> TestCases {
		get {
			var desktopCompilerArguments = new string [] {
				"-DGL_DO_NOT_WARN_IF_MULTI_GL_VERSION_HEADERS_INCLUDED",
			};

			var desktopExcludeNoModules = new string [] {
				"Kerberos",
			};

			var macOSExclude = new string [] {
				"DriverKit", // must be compiled as C++?
				"Tk", // depends on X11 headers, which don't exist anymore
			};

			var macCatalystExclude = new string [] {
				// most of these frameworks are not available on Mac Catalyst
				"AccessoryAccess",
				"AccessorySetupKit",
				"AudioVideoBridging",
				"Carbon",
				"CompositorServices",
				"DiscRecordingUI",
				"DriverKit", // must be compiled as C++?
				"FSKit",
				"GLKit",
				"Hypervisor",
				"InputMethodKit",
				"InstallerPlugins",
				"LDAP",
				"Tk", // depends on X11 headers, which don't exist anymore
				"Quartz",
				"QuickLookUI",
				"Virtualization",
			};

			var sdkNames = new [] {
				( Platform: "iOS", Sdk: "iphoneos" + SdkVersions.iOS, ExcludedFrameworks: [], NoModulesExcludedFrameworks: [],  ClangArguments: [] ),
				( Platform: "tvOS", Sdk: "appletvos" + SdkVersions.TVOS, ExcludedFrameworks: [], NoModulesExcludedFrameworks: [], ClangArguments: [] ),
				( Platform: "macOS", Sdk: "macosx" + SdkVersions.OSX, ExcludedFrameworks: macOSExclude, NoModulesExcludedFrameworks: desktopExcludeNoModules, ClangArguments: desktopCompilerArguments ),
				( Platform: "MacCatalyst", Sdk: "ios" + SdkVersions.MacCatalyst + "-macabi", ExcludedFrameworks: macCatalystExclude, NoModulesExcludedFrameworks: desktopExcludeNoModules, ClangArguments: desktopCompilerArguments ),
			};

			foreach (var sdk in sdkNames) {
				yield return new TestCaseData (false, sdk.Sdk, sdk.ExcludedFrameworks.Union (sdk.NoModulesExcludedFrameworks).ToArray (), sdk.ClangArguments).SetName ($"{sdk.Platform} (no modules)");
				yield return new TestCaseData (true, sdk.Sdk, sdk.ExcludedFrameworks, sdk.ClangArguments).SetName ($"{sdk.Platform} (with modules)");
			}
		}
	}

	[Test]
	[TestCaseSource (nameof (TestCases))]
	public void TestSdkPath (bool usingModules, string sdk, string []? excludedFrameworks = null, string []? clangArguments = null)
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var binder = new SdkDbTool ();
		if (clangArguments is not null)
			binder.ClangArguments.AddRange (clangArguments);
		binder.EnableModules = usingModules;
		binder.ExcludedFrameworks.AddRange (excludedFrameworks ?? Array.Empty<string> ());
		binder.Sdk = sdk;
		binder.OutputDirectory = tmpdir;
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var rv = binder.BindInOrOut ();
		rv.AssertSuccess (null);
		rv.AssertNoWarnings ();
	}
}
