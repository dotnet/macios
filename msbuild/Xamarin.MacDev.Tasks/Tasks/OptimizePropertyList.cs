using System.IO;

using Microsoft.Build.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class OptimizePropertyList : XamarinTask {
		#region Inputs

		[Required]
		public ITaskItem? Input { get; set; }

		[Required]
		[Output]
		public ITaskItem? Output { get; set; }

		#endregion

		public override bool Execute ()
		{
			var input = Input!.ItemSpec;
			var output = Output!.ItemSpec;

			var plist = PObject.FromFile (input);
			if (plist is null) {
				Log.LogError (null, null, null, input, 0, 0, 0, 0, "Could not parse the property list '{0}'.", input);
				return false;
			}

			var outputDirectory = Path.GetDirectoryName (output);
			if (!string.IsNullOrEmpty (outputDirectory))
				Directory.CreateDirectory (outputDirectory);
			plist.Save (output, binary: true);
			return true;
		}
	}
}
