using System;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.MacDev.Tasks;
using Xamarin.MacDev;
using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class CompileITunesMetadata : XamarinTask, ICancelableTask {
		#region Inputs

		[Required]
		public string BundleIdentifier { get; set; } = string.Empty;

		public string BundleDisplayName { get; set; } = string.Empty;

		public string BundleVersion { get; set; } = string.Empty;

		public ITaskItem [] ITunesMetadata { get; set; } = Array.Empty<ITaskItem> ();

		[Output]
		[Required]
		public ITaskItem? OutputPath { get; set; }

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return ExecuteRemotely ();

			PDictionary metadata;

			if (ITunesMetadata is not null) {
				if (ITunesMetadata.Length > 1) {
					Log.LogError (23, null, MSBStrings.E0023); // Cannot have more than 1 iTunesMetadata.plist.
					return false;
				}

				var path = ITunesMetadata [0].GetMetadata ("FullPath");

				try {
					metadata = PDictionary.OpenFile (path);
				} catch (Exception ex) {
					Log.LogError (10, path, MSBStrings.E0010, path, ex.Message); // Error loading '{0}': {1}
					return false;
				}
			} else {
				var displayName = BundleDisplayName;
				var bundleVersion = BundleVersion;

				metadata = new PDictionary ();

				metadata.Add ("genre", new PString ("Application"));
				if (!string.IsNullOrEmpty (bundleVersion))
					metadata.Add ("bundleVersion", (PString) bundleVersion);
				if (!string.IsNullOrEmpty (displayName))
					metadata.Add ("itemName", (PString) displayName);
				metadata.Add ("kind", (PString) "software");
				if (displayName is not null)
					metadata.Add ("playlistName", (PString) displayName);
				metadata.Add ("softwareIconNeedsShine", (PBoolean) true);
				metadata.Add ("softwareVersionBundleId", (PString) BundleIdentifier);
			}

			Directory.CreateDirectory (Path.GetDirectoryName (OutputPath!.ItemSpec)!);
			metadata.Save (OutputPath.ItemSpec, true);

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}
