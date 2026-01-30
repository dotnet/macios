using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;

namespace Xamarin.MacDev.Tasks;

// This task will select which runtime (monovm/coreclr/nativeaot) libraries to add
// to the app bundle.
public class ProcessRuntimeLibraries : XamarinTask, ICancelableTask {
	[Required]
	public string DotNetRuntime { get; set; } = string.Empty;

	[Required]
	public ITaskItem [] ResolvedFileToPublish { get; set; } = [];

	[Required]
	public string RuntimeIdentifier { get; set; } = string.Empty;

	[Required]
	public string RuntimeLibLinkMode { get; set; } = string.Empty;

	[Required]
	public string RuntimeNuGetPackageId { get; set; } = string.Empty;

	[Output]
	public ITaskItem [] OutputResolvedFileToPublish { get; set; } = [];

	[Output]
	public ITaskItem [] DylibsToConvertToFrameworks { get; set; } = [];

	public override bool Execute ()
	{
		var output = new List<ITaskItem> ();
		var dylibsToFrameworks = new List<ITaskItem> ();

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
		output.AddRange (splitRuntimeLibraries [false]);

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

			if (string.Equals (DotNetRuntime, "monovm", StringComparison.OrdinalIgnoreCase)) {
				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// don't keep any:
					// * static libraries are linked into the binary, so they're not copied as-is to the app bundle
					// * we're linking statically, so we don't want the .dylib files either.
					continue;
				case "dylib":
					// keep .dylib, remove anything else
					output.AddRange (group.Where (v => string.Equals (v.GetMetadata ("Extension"), ".dylib", StringComparison.OrdinalIgnoreCase)));
					continue;
				default:
					Log.LogError (MSBStrings.E7164 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			if (string.Equals (DotNetRuntime, "nativeaot", StringComparison.OrdinalIgnoreCase)) {
				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// don't keep any:
					// * static libraries are linked into the binary, so they're not copied as-is to the app bundle
					// * we're linking statically, so we don't want the .dylib files either.
					continue;
				case "dylib":
					// NativeAOT is static only
					Log.LogError (MSBStrings.E7166 /* Unexpected RuntimeLibLinkMode value for 'NativeAOT': '{0}' */, RuntimeLibLinkMode);
					continue;
				default:
					Log.LogError (MSBStrings.E7164 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			if (string.Equals (DotNetRuntime, "coreclr", StringComparison.OrdinalIgnoreCase)) {
				if (Platform != Utils.ApplePlatform.MacOSX && string.Equals (kvp.Key, "libclrjit", StringComparison.OrdinalIgnoreCase)) {
					// don't include libclrjit unless we're targeting macOS
					continue;
				}

				switch (RuntimeLibLinkMode.ToLowerInvariant ()) {
				case "static":
					// if we only have a single .a, we don't need to keep it (it's linked into the binary, not copied as-is to the app bundle)
					if (group.All (v => v.GetMetadata ("Extension").Equals (".a", StringComparison.OrdinalIgnoreCase)))
						continue;

					// if we have a single .dylib, but we're linking statically, we need to convert it to a framework
					// if we have both a .dylib and a .a, and we're linking statically, we still need to convert the .dylib to a .framework, because the .a is ignored/irrelevant
					dylibsToFrameworks.AddRange (group.Where (v => v.GetMetadata ("Extension").Equals (".dylib", StringComparison.OrdinalIgnoreCase)));
					continue;
				case "dylib":
					// we don't want any .a files, but we want all .dylib files.
					output.AddRange (group.Where (v => v.GetMetadata ("Extension").Equals (".dylib", StringComparison.OrdinalIgnoreCase)));
					continue;
				default:
					Log.LogError (MSBStrings.E7164 /* Invalid RuntimeLibLinkMode value: '{0}' */, RuntimeLibLinkMode);
					return false;
				}
			}

			Log.LogError (MSBStrings.E7165 /* Invalid DotNetRuntime value: '{0}' */, DotNetRuntime);
			return false;
		}

		OutputResolvedFileToPublish = output.ToArray ();
		DylibsToConvertToFrameworks = dylibsToFrameworks.ToArray ();

		return !Log.HasLoggedErrors;
	}

	public void Cancel ()
	{
		if (ShouldExecuteRemotely ())
			BuildConnection.CancelAsync (BuildEngine4).Wait ();
	}
}
