using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks;

// This task will select which runtime (monovm/coreclr/nativeaot) libraries to:
// * Add to the app bundle
// * Link with
public class ProcessRuntimeLibraries : XamarinTask, ICancelableTask {
	[Required]
	public string DotNetRuntime { get; set; } = string.Empty;

	public ITaskItem [] MonoRuntimeComponentLink { get; set; } = [];

	public ITaskItem [] MonoRuntimeComponentDontLink { get; set; } = [];

	[Required]
	public ITaskItem [] ResolvedFileToPublish { get; set; } = [];

	[Required]
	public string RuntimeIdentifier { get; set; } = string.Empty;

	[Required]
	public string RuntimeLibLinkMode { get; set; } = string.Empty;

	[Required]
	public string RuntimeNuGetPackageId { get; set; } = string.Empty;

	public bool DebuggerSupport { get; set; }

	[Output]
	public ITaskItem [] OutputResolvedFileToPublish { get; set; } = [];

	[Output]
	public ITaskItem [] DylibsToConvertToFrameworks { get; set; } = [];

	[Output]
	public ITaskItem [] LinkWithRuntimeLibraries { get; set; } = [];

	public override bool Execute ()
	{
		var copyToAppBundle = new List<ITaskItem> ();
		var dylibsToFrameworks = new List<ITaskItem> ();
		var linkWithRuntimeLibraries = new List<ITaskItem> ();

		// Split the ResolvedFileToPublish into runtime libraries and others
		var splitRuntimeLibraries = ResolvedFileToPublish
			.GroupBy (item => {
				if (!string.Equals (item.GetMetadata ("AssetType"), "native", StringComparison.OrdinalIgnoreCase))
					return false;
				if (!string.Equals (item.GetMetadata ("RuntimeIdentifier"), RuntimeIdentifier, StringComparison.OrdinalIgnoreCase))
					return false;
				if (!string.Equals (item.GetMetadata ("NuGetPackageId"), RuntimeNuGetPackageId, StringComparison.OrdinalIgnoreCase))
					return false;
				var extension = item.GetMetadata ("Extension");
				if (string.Equals (extension, ".a", StringComparison.OrdinalIgnoreCase))
					return true;
				if (string.Equals (extension, ".dylib", StringComparison.OrdinalIgnoreCase))
					return true;
				if (string.Equals (extension, ".o", StringComparison.OrdinalIgnoreCase))
					return true;
				return false;
			})
			.ToDictionary (g => g.Key, g => g.ToList ());

		// Add non-runtime libraries to output as-is
		copyToAppBundle.AddRange (splitRuntimeLibraries [false]);

		var runtimeLibraries = splitRuntimeLibraries [true];

		var groupedRuntimeLibraries = runtimeLibraries
			.GroupBy (item => item.GetMetadata ("Filename"))
			.ToDictionary (g => g.Key, g => g.ToList ());

		// group by library name
		// multiple versions of each library might be present (e.g. .dylib, .a, etc)
		// select which to keep based on DotNetRuntime and RuntimeLibLinkMode
		foreach (var kvp in groupedRuntimeLibraries) {
			var group = kvp.Value;
			if (group.Count == 0)
				continue;

			var dylibs = group.Where (v => string.Equals (v.GetMetadata ("Extension"), ".dylib", StringComparison.OrdinalIgnoreCase));
			var staticlibs = group.Where (v => string.Equals (v.GetMetadata ("Extension"), ".a", StringComparison.OrdinalIgnoreCase));

			if (string.Equals (DotNetRuntime, "monovm", StringComparison.OrdinalIgnoreCase)) {
				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// only link
					linkWithRuntimeLibraries.AddRange (staticlibs);
					continue;
				case "dylib":
					// copy + link
					copyToAppBundle.AddRange (dylibs);
					linkWithRuntimeLibraries.AddRange (staticlibs);
					continue;
				default:
					Log.LogError (MSBStrings.E7170 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			if (string.Equals (DotNetRuntime, "nativeaot", StringComparison.OrdinalIgnoreCase)) {
				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// only link, but NativeAOT's build logic computes these libraries, so there's nothing for us to do here.
					continue;
				case "dylib":
					// NativeAOT is static only
					Log.LogError (MSBStrings.E7166 /* Unexpected RuntimeLibLinkMode value for 'NativeAOT': '{0}' */, RuntimeLibLinkMode);
					continue;
				default:
					Log.LogError (MSBStrings.E7170 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			if (string.Equals (DotNetRuntime, "coreclr", StringComparison.OrdinalIgnoreCase)) {
				if (Platform != Utils.ApplePlatform.MacOSX && string.Equals (kvp.Key, "libclrjit", StringComparison.OrdinalIgnoreCase)) {
					// don't include libclrjit unless we're targeting macOS
					continue;
				}

				if (Platform != Utils.ApplePlatform.MacOSX && !DebuggerSupport) {
					// libmscordaccore and libmscordbi are debug-only libraries, don't include them when debugger support is disabled on mobile platforms
					if (string.Equals (kvp.Key, "libmscordaccore", StringComparison.OrdinalIgnoreCase) ||
						string.Equals (kvp.Key, "libmscordbi", StringComparison.OrdinalIgnoreCase)) {
						continue;
					}
				}

				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// if we only have a single .a, we need to link with it, but not copy to the app bundle
					if (staticlibs.Count () == group.Count ()) {
						linkWithRuntimeLibraries.AddRange (staticlibs);
						continue;
					}

					// if we have a single .dylib, but we're linking statically, we need to convert it to a framework (if we're targeting a mobile platform)
					// if we have both a .dylib and a .a, and we're linking statically, we still need to convert the .dylib to a .framework, because the .a is ignored/irrelevant
					if (Platform == ApplePlatform.iOS || Platform == ApplePlatform.TVOS) {
						dylibsToFrameworks.AddRange (dylibs);
					} else {
						// on desktop just link with the dylib
						copyToAppBundle.AddRange (dylibs);
					}
					continue;
				case "dylib":
					// we don't want any .a files, but we want all .dylib files.
					copyToAppBundle.AddRange (dylibs);
					continue;
				default:
					Log.LogError (MSBStrings.E7170 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			Log.LogError (MSBStrings.E7165 /* Invalid DotNetRuntime value: '{0}' */, DotNetRuntime);
			return false;
		}

		if (string.Equals (DotNetRuntime, "monovm", StringComparison.OrdinalIgnoreCase) && RuntimeLibLinkMode.ToLowerInvariant () == "static") {
			var dontLinkWithFilenames = new HashSet<string> (MonoRuntimeComponentDontLink.Select (v => v.ItemSpec));
			var dontLinkWith = linkWithRuntimeLibraries.Where (v => dontLinkWithFilenames.Contains (v.GetMetadata ("Filename") + v.GetMetadata ("Extension")));
			linkWithRuntimeLibraries = linkWithRuntimeLibraries.Except (dontLinkWith).ToList ();
			Log.LogMessage (MessageImportance.Low, "Not linking with the following libraries, because the corresponding component was enabled/disabled:");
			foreach (var item in dontLinkWith)
				Log.LogMessage (MessageImportance.Low, $"    {item.ItemSpec}");
		}

		OutputResolvedFileToPublish = copyToAppBundle.ToArray ();
		DylibsToConvertToFrameworks = dylibsToFrameworks.ToArray ();
		LinkWithRuntimeLibraries = linkWithRuntimeLibraries.ToArray ();

		return !Log.HasLoggedErrors;
	}

	public void Cancel ()
	{
		if (ShouldExecuteRemotely ())
			BuildConnection.CancelAsync (BuildEngine4).Wait ();
	}
}
