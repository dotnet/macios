using System;
using System.Linq;
using System.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;

namespace Xamarin.MacDev.Tasks {

	/// <summary>
	/// Provides a localizable way to log a message from an MSBuild target.
	/// </summary>
	public class MacDevMessage : Task {

		/// <summary>
		/// The name of the resource from Properties\Resources.resx that contains the message
		/// </summary>
		[Required]
		public string ResourceName { get; set; } = string.Empty;

		/// <summary>
		/// The string format arguments to use for any numbered format items in the resource provided by ResourceName
		/// </summary>
		public ITaskItem [] FormatArguments { get; set; } = Array.Empty<ITaskItem> ();

		// Metadata preserves empty arguments and values that would otherwise be treated as paths.
		public string FormatArgumentsMetadata { get; set; } = "";

		public bool Error { get; set; }
		public bool Warning { get; set; }

		public override bool Execute ()
		{
			var msg = MSBStrings.ResourceManager.GetString (ResourceName, MSBStrings.Culture) ?? $"Could not find the resource '{ResourceName}'.";
			var args = FormatArguments.Select (v => string.IsNullOrEmpty (FormatArgumentsMetadata) ? v.ItemSpec : v.GetMetadata (FormatArgumentsMetadata)).ToArray ();
			var message = string.Format (msg, args);
			if (Error) {
				Log.LogError (message);
			} else if (Warning) {
				Log.LogWarning (message);
			} else {
				Log.LogMessage (message);
			}
			return !Log.HasLoggedErrors;
		}
	}
}
