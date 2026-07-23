using System;
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
			var plist = PObject.FromFile (Input!.ItemSpec);
			if (plist is null)
				throw new FormatException ($"Could not parse the property list '{Input.ItemSpec}'.");

			Directory.CreateDirectory (Path.GetDirectoryName (Output!.ItemSpec)!);
			plist.Save (Output.ItemSpec, binary: true);
			return true;
		}
	}
}
