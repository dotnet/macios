using System;
using System.Globalization;
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

		public bool Error { get; set; }
		public bool Warning { get; set; }

		public override bool Execute ()
		{
			var code = 0;
			if ((Error || Warning) && (ResourceName.Length < 5 ||
				(ResourceName [0] != 'E' && ResourceName [0] != 'W') ||
				!int.TryParse (ResourceName.Substring (1, 4), NumberStyles.None, CultureInfo.InvariantCulture, out code))) {
				Log.LogError (7223, null, MSBStrings.E7223, ResourceName); // The resource '{0}' is not a numbered SDK diagnostic.
				return false;
			}
			var msg = MSBStrings.ResourceManager.GetString (ResourceName, MSBStrings.Culture) ?? $"Could not find the resource '{ResourceName}'.";
			var args = FormatArguments.Select (v => v.ItemSpec).ToArray ();
			var message = string.Format (msg, args);
			if (Error) {
				Log.LogError (code, null, "{0}", message);
			} else if (Warning) {
				Log.LogWarning (code, null, "{0}", message);
			} else {
				Log.LogMessage (message);
			}
			return !Log.HasLoggedErrors;
		}
	}
}
