// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class PrepareWatchOSApp : XamarinTask, ITaskCallback, ICancelableTask {
		readonly CancellationTokenSource cancellation = new CancellationTokenSource ();

		public ITaskItem [] Apps { get; set; } = [];
		public ITaskItem [] Signing { get; set; } = [];
		public bool InputIsOnBuildServer { get; set; }

		[Required]
		public string AppBundleDir { get; set; } = "";

		[Required]
		public string IntermediateOutputPath { get; set; } = "";

		[Required]
		public string RuntimeIdentifier { get; set; } = "";

		public string ArtifactDirectory { get; set; } = "";
		public string BundleIdentifier { get; set; } = "";
		public string SigningKey { get; set; } = "";
		public string ProvisioningProfile { get; set; } = "";
		public string CodesignAllocate { get; set; } = "";
		public string CodesignKeychain { get; set; } = "";
		public string ValidateEntitlements { get; set; } = "";
		public string SdkVersion { get; set; } = "";
		public bool EnableCodeSigning { get; set; } = true;
		public bool Debug { get; set; }

		[Output]
		public ITaskItem [] CodesignBundles { get; set; } = [];

		[Output]
		public ITaskItem [] FileWrites { get; set; } = [];

		[Output]
		public string WatchKitSupportPath { get; set; } = "";

		bool Simulator => RuntimeIdentifier.StartsWith ("iossimulator-", StringComparison.Ordinal);

		static string RemotePath (string path)
		{
			return string.IsNullOrEmpty (path) ? path : PathUtils.AbsoluteToRelative (Environment.CurrentDirectory, path).Replace ('\\', '/');
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				AppBundleDir = RemotePath (AppBundleDir);
				IntermediateOutputPath = RemotePath (IntermediateOutputPath);
				ArtifactDirectory = RemotePath (ArtifactDirectory);
				foreach (var item in Apps.Concat (Signing)) {
					foreach (var name in new [] { "CodesignEntitlements", "DSymPath" }) {
						var value = item.GetMetadata (name);
						if (value.Length > 0)
							item.SetMetadata (name, RemotePath (value));
					}
				}
				return ExecuteRemotely ();
			}
			try {
				return ExecuteLocally ();
			} catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException || ex is System.Xml.XmlException || ex is ArgumentException) {
				Log.LogError (MSBStrings.WatchOS_ArtifactError, Apps.FirstOrDefault ()?.ItemSpec ?? "", ex.Message);
				return false;
			}
		}

		bool ExecuteLocally ()
		{
			if (Apps.Length > 1) {
				Log.LogError (MSBStrings.WatchOS_RequireApp, string.Join (";", Apps.Select (v => v.ItemSpec)));
				return false;
			}
			foreach (var duplicate in Signing.GroupBy (v => v.ItemSpec).Where (v => v.Count () > 1))
				Log.LogError (MSBStrings.WatchOS_DuplicateSigningBundle, duplicate.Key);
			if (Log.HasLoggedErrors)
				return false;
			var destinationRoot = Path.Combine (AppBundleDir, "Watch");
			var receipt = Path.Combine (IntermediateOutputPath, "embedded-app.txt");
			if (Apps.Length == 0) {
				if (File.Exists (receipt)) {
					var name = File.ReadAllText (receipt);
					if (Path.GetFileName (name) == name && name.EndsWith (".app", StringComparison.Ordinal)) {
						var previous = Path.Combine (destinationRoot, name);
						if (Directory.Exists (previous)) {
							Directory.Delete (previous, true);
							Directory.SetLastWriteTimeUtc (AppBundleDir, DateTime.UtcNow);
						}
					}
					File.Delete (receipt);
				}
				var oldSymbols = Path.Combine (IntermediateOutputPath, "dSYMs");
				if (Directory.Exists (oldSymbols))
					Directory.Delete (oldSymbols, true);
				foreach (var settings in Signing)
					Log.LogError (MSBStrings.WatchOS_UnknownSigningBundle, settings.ItemSpec);
				return !Log.HasLoggedErrors;
			}
			if (!BuildWatchOSApp.IsSupportedRuntimeIdentifier (RuntimeIdentifier)) {
				Log.LogError (MSBStrings.WatchOS_UnsupportedRuntimeIdentifier, RuntimeIdentifier);
				return false;
			}
			Directory.CreateDirectory (IntermediateOutputPath);
			var item = Apps [0];
			var source = item.ItemSpec;
			var artifact = ArtifactDirectory;
			if (source.EndsWith (".zip", StringComparison.OrdinalIgnoreCase)) {
				artifact = Path.Combine (IntermediateOutputPath, "extracted");
				// Replace rather than merge: a new ZIP may have removed files.
				if (Directory.Exists (artifact))
					Directory.Delete (artifact, true);
				using var zip = ZipFile.OpenRead (source);
				if (!TrySelectZipResource (zip, RuntimeIdentifier, out var resource)) {
					Log.LogError (MSBStrings.WatchOS_InvalidZip, source, RuntimeIdentifier);
					return false;
				}
				var files = new List<string> ();
				if (!CompressionHelper.TryDecompress (this, source, resource, artifact, files, cancellation.Token, out var extracted))
					return false;
				artifact = extracted;
				var apps = Directory.GetDirectories (artifact, "*.app");
				if (apps.Length != 1) {
					Log.LogError (MSBStrings.WatchOS_RequireApp, source);
					return false;
				}
				source = apps [0];
			} else if (!source.EndsWith (".app", StringComparison.OrdinalIgnoreCase) || !Directory.Exists (source)) {
				Log.LogError (MSBStrings.WatchOS_RequireApp, source);
				return false;
			}
			var manifest = PDictionary.OpenFile (Path.Combine (source, "Info.plist"));
			if (manifest.GetString ("WKCompanionAppBundleIdentifier")?.Value != BundleIdentifier) {
				Log.LogError (MSBStrings.WatchOS_CompanionMismatch, source, BundleIdentifier);
				return false;
			}
			var nameOfApp = Path.GetFileName (source.TrimEnd ('/', '\\'));
			var destination = Path.Combine (destinationRoot, nameOfApp);
			if (PathUtils.ResolveSymbolicLinks (Path.GetFullPath (source)) == PathUtils.ResolveSymbolicLinks (Path.GetFullPath (destination)) ||
				PathUtils.IsPathContained (source, destination) || PathUtils.IsPathContained (destinationRoot, source) || !ValidateBundlePaths (source)) {
				Log.LogError (MSBStrings.WatchOS_ArtifactError, source, MSBStrings.WatchOS_InvalidBundlePaths);
				return false;
			}
			if (File.Exists (receipt)) {
				var oldName = File.ReadAllText (receipt);
				if (oldName != nameOfApp && Path.GetFileName (oldName) == oldName && oldName.EndsWith (".app", StringComparison.Ordinal)) {
					var oldPath = Path.Combine (destinationRoot, oldName);
					if (Directory.Exists (oldPath))
						Directory.Delete (oldPath, true);
				}
			}
			if (Directory.Exists (destination))
				Directory.Delete (destination, true);
			if (ExecuteAsync ("/usr/bin/ditto", new [] { source, destination }, cancellationToken: cancellation.Token).Result.ExitCode != 0)
				return false;
			if (!ValidateBundlePaths (destination)) {
				Log.LogError (MSBStrings.WatchOS_ArtifactError, destination, MSBStrings.WatchOS_InvalidBundlePaths);
				return false;
			}
			var signingStamps = Path.Combine (IntermediateOutputPath, "signing");
			if (Directory.Exists (signingStamps))
				Directory.Delete (signingStamps, true);
			File.WriteAllText (receipt, nameOfApp);

			var bundles = new List<ITaskItem> ();
			var usedSigning = new HashSet<string> (StringComparer.Ordinal);
			var identifiers = new HashSet<string> (StringComparer.Ordinal);
			foreach (var bundle in EnumerateBundles (destination)) {
				cancellation.Token.ThrowIfCancellationRequested ();
				var plist = PDictionary.OpenFile (Path.Combine (bundle, "Info.plist"));
				var identifier = plist.GetCFBundleIdentifier ();
				var executable = plist.GetCFBundleExecutable ();
				if (string.IsNullOrEmpty (identifier) || identifier.IndexOfAny (new [] { '/', '\\', ':' }) >= 0 || identifier == "." || identifier == ".." || !identifiers.Add (identifier) || string.IsNullOrEmpty (executable) ||
					Path.GetFileName (executable) != executable) {
					Log.LogError (MSBStrings.WatchOS_ArtifactError, bundle, MSBStrings.WatchOS_InvalidBundleManifest);
					return false;
				}
				if (!ValidateExecutable (Path.Combine (bundle, executable)))
					return false;
				var settings = Signing.FirstOrDefault (v => v.ItemSpec == identifier);
				if (settings is not null)
					usedSigning.Add (settings.ItemSpec);
				else if (bundle == destination)
					settings = item;

				var entitlements = settings?.GetMetadata ("CodesignEntitlements") ?? "";
				if (entitlements.Length == 0 && artifact.Length > 0) {
					var template = Path.Combine (artifact, "Entitlements", identifier + ".plist");
					if (File.Exists (template))
						entitlements = template;
				}
				if (entitlements.Length == 0)
					entitlements = RecoverEntitlements (bundle, identifier);
				if (Log.HasLoggedErrors)
					return false;

				var key = SigningKey;
				var profile = "";
				if (EnableCodeSigning && !Simulator) {
					var parentProfile = MobileProvisionIndex.GetMobileProvision (MobileProvisionPlatform.iOS, ProvisioningProfile);
					if (parentProfile is null) {
						Log.LogError (MSBStrings.E0049, ProvisioningProfile);
						return false;
					}
					var provision = settings?.GetMetadata ("CodesignProvision") ?? "";
					if (provision.Length == 0)
						provision = parentProfile.DistributionType == MobileProvisionDistributionType.Development ? "Automatic" : "Automatic:" + parentProfile.DistributionType;
					var detect = new DetectSigningIdentity {
						BuildEngine = BuildEngine,
						AppBundleName = Path.GetFileName (bundle),
						BundleIdentifier = identifier,
						CodesignEntitlements = new TaskItem (entitlements),
						SigningKey = SigningKey,
						ProvisioningProfile = provision,
						SdkPlatform = "iPhoneOS",
						SdkDevPath = SdkDevPath,
						TargetFrameworkMoniker = TargetFrameworkMoniker,
						Keychain = CodesignKeychain,
					};
					if (!detect.Execute ())
						return false;
					key = detect.DetectedCodeSigningKey;
					profile = detect.DetectedProvisioningProfile;
					var selectedProfile = MobileProvisionIndex.GetMobileProvision (MobileProvisionPlatform.iOS, profile);
					if (selectedProfile is null || selectedProfile.DistributionType != parentProfile.DistributionType) {
						Log.LogError (MSBStrings.WatchOS_IncompatibleProfile, identifier, profile, parentProfile.Name);
						return false;
					}
					var embed = new EmbedProvisionProfile {
						BuildEngine = BuildEngine,
						AppBundleDir = bundle,
						EmbeddedProvisionProfilePath = Path.Combine (bundle, "embedded.mobileprovision"),
						ProvisioningProfile = profile,
						TargetFrameworkMoniker = TargetFrameworkMoniker,
					};
					if (!embed.Execute ())
						return false;
				} else if (Simulator) {
					var embedded = Path.Combine (bundle, "embedded.mobileprovision");
					if (File.Exists (embedded))
						File.Delete (embedded);
				}
				var signingEntitlements = "";
				if (EnableCodeSigning && !Simulator) {
					var compile = new CompileEntitlements {
						BuildEngine = BuildEngine,
						AppBundleDir = bundle,
						BundleIdentifier = identifier,
						CompiledEntitlements = new TaskItem (Path.Combine (IntermediateOutputPath, identifier + ".xcent")),
						Entitlements = entitlements,
						ProvisioningProfile = profile,
						InjectDefaultPlatformEntitlements = "false",
						GenerateArchivedExpandedEntitlements = false,
						SdkPlatform = "iPhoneOS",
						SdkVersion = SdkVersion,
						SdkDevPath = SdkDevPath,
						TargetFrameworkMoniker = TargetFrameworkMoniker,
						Debug = Debug,
						ValidateEntitlements = ValidateEntitlements,
					};
					if (!compile.Execute ())
						return false;
					signingEntitlements = compile.EntitlementsInSignature?.ItemSpec ?? "";
				}
				var sign = new TaskItem (PathUtils.AbsoluteToRelative (Path.GetFullPath (Path.Combine (AppBundleDir, "..")), Path.GetFullPath (bundle)));
				sign.SetMetadata ("CodesignSigningKey", EnableCodeSigning ? key : "");
				sign.SetMetadata ("EnableCodeSigning", EnableCodeSigning.ToString ());
				sign.SetMetadata ("CodesignEntitlements", signingEntitlements);
				sign.SetMetadata ("CodesignAllocate", CodesignAllocate);
				sign.SetMetadata ("CodesignKeychain", CodesignKeychain);
				sign.SetMetadata ("CodesignDisableTimestamp", (Simulator || Debug).ToString ());
				sign.SetMetadata ("CodesignStampFile", Path.Combine (IntermediateOutputPath, "signing", identifier + ".stamp"));
				bundles.Add (sign);
			}
			foreach (var settings in Signing.Where (v => !usedSigning.Contains (v.ItemSpec)))
				Log.LogError (MSBStrings.WatchOS_UnknownSigningBundle, settings.ItemSpec);
			foreach (var library in Directory.EnumerateFiles (destination, "*.dylib", SearchOption.AllDirectories)
				.Concat (Directory.EnumerateDirectories (destination, "*.framework", SearchOption.AllDirectories).Select (v => Path.Combine (v, Path.GetFileNameWithoutExtension (v))))) {
				if (!ValidateExecutable (library))
					return false;
			}
			var stub = Path.Combine (destination, "_WatchKitStub", "WK");
			WatchKitSupportPath = File.Exists (stub) ? stub : "";
			var symbolOutput = Path.Combine (IntermediateOutputPath, "dSYMs");
			if (Directory.Exists (symbolOutput))
				Directory.Delete (symbolOutput, true);
			if (artifact.Length > 0) {
				var symbols = Path.Combine (artifact, "dSYMs");
				if (Directory.Exists (symbols)) {
					if (ExecuteAsync ("/usr/bin/ditto", new [] { symbols, symbolOutput }, cancellationToken: cancellation.Token).Result.ExitCode != 0)
						return false;
				}
			}
			var symbolInput = item.GetMetadata ("DSymPath");
			if (symbolInput.Length > 0 && ExecuteAsync ("/usr/bin/ditto", new [] { symbolInput, Path.Combine (IntermediateOutputPath, "dSYMs") }, cancellationToken: cancellation.Token).Result.ExitCode != 0)
				return false;
			CodesignBundles = bundles.ToArray ();
			FileWrites = Directory.GetFiles (IntermediateOutputPath, "*", SearchOption.AllDirectories)
				.Concat (Directory.GetFiles (destination, "*", SearchOption.AllDirectories)).Select (v => new TaskItem (v)).ToArray ();
			return !Log.HasLoggedErrors;
		}

		static bool ValidateBundlePaths (string root)
		{
			if ((File.GetAttributes (root) & FileAttributes.ReparsePoint) != 0)
				return false;
			foreach (var path in Directory.EnumerateFileSystemEntries (root)) {
				if (!PathUtils.IsPathContained (root, path))
					return false;
				if ((File.GetAttributes (path) & FileAttributes.ReparsePoint) != 0) {
					if (Path.IsPathRooted (PathUtils.GetSymlinkTarget (path)))
						return false;
					continue;
				}
				if (Directory.Exists (path) && !ValidateBundlePaths (path))
					return false;
			}
			return true;
		}

		string RecoverEntitlements (string bundle, string identifier)
		{
			var template = new PDictionary ();
			if (Directory.Exists (Path.Combine (bundle, "_CodeSignature"))) {
				var result = ExecuteAsync ("/usr/bin/codesign", new [] { "-d", "--entitlements", ":-", bundle }, showErrorIfFailure: true, cancellationToken: cancellation.Token).Result;
				if (result.ExitCode != 0)
					return "";
				if (!string.IsNullOrWhiteSpace (result.Output.StandardOutput)) {
					if (PObject.FromString (result.Output.StandardOutput) is not PDictionary recovered) {
						Log.LogError (MSBStrings.WatchOS_ArtifactError, bundle, MSBStrings.WatchOS_InvalidEntitlements);
						return "";
					}
					template = recovered;
				}
				// These values belong to the selected profile, not to the prebuilt configuration.
				template.Remove ("application-identifier");
				template.Remove ("com.apple.developer.team-identifier");
				template.Remove ("get-task-allow");
				template.Remove ("aps-environment");
				template.Remove ("beta-reports-active");
			}
			var path = Path.Combine (IntermediateOutputPath, identifier + ".plist");
			template.Save (path);
			return path;
		}

		bool ValidateExecutable (string path)
		{
			if (!File.Exists (path)) {
				Log.LogError (MSBStrings.WatchOS_InvalidExecutable, path, RuntimeIdentifier);
				return false;
			}
			var slices = MachO.Read (path).ToArray ();
			var expected = Simulator ? MachO.Platform.WatchOSSimulator : MachO.Platform.WatchOS;
			var matchingArchitecture = false;
			foreach (var slice in slices) {
				var platforms = slice.load_commands.OfType<BuildVersionCommand> ().Select (v => v.Platform).ToArray ();
				var legacyDevice = !Simulator && slice.load_commands.Any (v => v.Command == MachO.LoadCommands.MinwatchOS);
				if ((platforms.Length == 0 && !legacyDevice) || platforms.Any (v => v != expected)) {
					Log.LogError (MSBStrings.WatchOS_InvalidExecutable, path, RuntimeIdentifier);
					return false;
				}
				if (Simulator)
					matchingArchitecture |= slice.Architecture == (RuntimeIdentifier == "iossimulator-x64" ? MachO.Architectures.x86_64 : MachO.Architectures.ARM64);
				else
					matchingArchitecture |= slice.Architecture == MachO.Architectures.ARMv7k || slice.Architecture == MachO.Architectures.ARM64_32 || slice.Architecture == MachO.Architectures.ARM64;
			}
			if (slices.Length == 0 || !matchingArchitecture)
				Log.LogError (MSBStrings.WatchOS_InvalidExecutable, path, RuntimeIdentifier);
			return !Log.HasLoggedErrors;
		}

		static IEnumerable<string> EnumerateBundles (string bundle)
		{
			yield return bundle;
			var plugins = Path.Combine (bundle, "PlugIns");
			if (Directory.Exists (plugins)) {
				foreach (var extension in Directory.GetDirectories (plugins, "*.appex"))
					foreach (var nested in EnumerateBundles (extension))
						yield return nested;
			}
		}

		public static bool TrySelectZipResource (ZipArchive archive, string rid, out string resource)
		{
			resource = "";
			var names = archive.Entries.Select (v => v.FullName).ToArray ();
			var roots = names.Select (v => v.Split ('/') [0]).Distinct ().ToArray ();
			if (roots.Length == 0)
				return false;
			if (roots.All (BuildWatchOSApp.IsSupportedRuntimeIdentifier)) {
				if (!roots.Contains (rid))
					return false;
				resource = rid;
			} else if (roots.Count (v => v.EndsWith (".app", StringComparison.Ordinal)) != 1 || roots.Any (v => !v.EndsWith (".app", StringComparison.Ordinal) && v != "Entitlements" && v != "dSYMs")) {
				return false;
			}
			var prefix = resource.Length == 0 ? "" : resource + "/";
			var selected = archive.Entries.Where (v => v.FullName.StartsWith (prefix, StringComparison.Ordinal)).ToArray ();
			var selectedRoots = selected.Select (v => v.FullName.Substring (prefix.Length).Split ('/') [0]).Where (v => v.Length > 0).Distinct ().ToArray ();
			if (selectedRoots.Count (v => v.EndsWith (".app", StringComparison.Ordinal)) != 1 ||
				selectedRoots.Any (v => !v.EndsWith (".app", StringComparison.Ordinal) && v != "Entitlements" && v != "dSYMs"))
				return false;
			var paths = new HashSet<string> (StringComparer.OrdinalIgnoreCase);
			var links = new HashSet<string> (StringComparer.OrdinalIgnoreCase);
			foreach (var entry in selected) {
				var path = entry.FullName.TrimEnd ('/');
				if (path.Length == 0 || path.StartsWith ("/", StringComparison.Ordinal) || path.Contains ('\\') || path.Contains (':') ||
					path.Split ('/').Any (v => v.Length == 0 || v == "." || v == "..") || !paths.Add (path))
					return false;
				if (((CompressionHelper.GetExternalAttributes (entry) >> 16) & 0xf000) != 0xa000)
					continue;
				if (entry.Length > 4096)
					return false;
				links.Add (path);
				using var reader = new StreamReader (entry.Open ());
				var target = reader.ReadToEnd ();
				if (target.Length == 0 || Path.IsPathRooted (target) || target.Contains ('\\') || target.Split ('/').Contains (".."))
					return false;
				var root = Path.GetFullPath (Path.Combine (Path.GetTempPath (), "watch-zip-validation"));
				var resolved = Path.GetFullPath (Path.Combine (root, Path.GetDirectoryName (path) ?? "", target));
				if (!resolved.StartsWith (root + Path.DirectorySeparatorChar + prefix, StringComparison.Ordinal))
					return false;
			}
			return !selected.Any (v => links.Any (link => v.FullName.StartsWith (link + "/", StringComparison.OrdinalIgnoreCase)));
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
			else
				cancellation.Cancel ();
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => !Signing.Contains (item) && (!InputIsOnBuildServer || !Apps.Contains (item));
		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied ()
		{
			foreach (var item in Apps.Concat (Signing)) {
				foreach (var name in new [] { "CodesignEntitlements", "DSymPath" }) {
					var path = item.GetMetadata (name);
					if (File.Exists (path))
						yield return new TaskItem (path);
					else if (Directory.Exists (path))
						foreach (var file in CreateItemsForAllFilesRecursively (path))
							yield return file;
				}
			}
		}
	}
}
