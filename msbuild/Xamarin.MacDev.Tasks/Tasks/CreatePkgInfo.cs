using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Messaging.Build.Client;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class CreatePkgInfo : XamarinTask, ICancelableTask, ITaskCallback {
		static readonly byte [] PkgInfoData = { 0X41, 0X50, 0X50, 0X4C, 0x3f, 0x3f, 0x3f, 0x3f };
		#region Inputs

		[Required]
		[Output]
		public ITaskItem OutputPath { get; set; } = null!;

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			if (!File.Exists (OutputPath.ItemSpec)) {
				Directory.CreateDirectory (Path.GetDirectoryName (OutputPath.ItemSpec));

				using (var stream = File.OpenWrite (OutputPath.ItemSpec)) {
					stream.Write (PkgInfoData, 0, PkgInfoData.Length);
				}
			}

			return true;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => true;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
