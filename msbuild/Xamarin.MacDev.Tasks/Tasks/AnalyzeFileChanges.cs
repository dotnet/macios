using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;

namespace Xamarin.MacDev.Tasks {
	public class AnalyzeFileChanges : XamarinTask, ITaskCallback {
		[Required]
		public string WorkingDirectory { get; set; } = string.Empty;

		[Required]
		public ITaskItem? ReportFile { get; set; }

		[Output]
		public ITaskItem [] ChangedFiles { get; set; } = [];

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => [];

		//We need the ReportFile input to be copied to the Mac. Since it's the only ITaskItem input, it's safe to return true
		public bool ShouldCopyToBuildServer (ITaskItem item) => true;

		//In case it's a remote execution, we don't want empty output files to be copied since we need the real files to be copied
		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;
			}

			if (!Directory.Exists (WorkingDirectory)) {
				Log.LogError (MSBStrings.E7146 /* Unable to analyze file changes in '{0}'. Directory not found. */, WorkingDirectory);

				return false;
			}

			if (!File.Exists (ReportFile!.ItemSpec)) {
				Log.LogError (MSBStrings.E7147 /* Unable to analyze file changes in '{0}'. The report file '{1}' does not exist. */, WorkingDirectory, ReportFile.ItemSpec);

				return false;
			}

			try {
				var changedFiles = new List<ITaskItem> ();
				//Gets a dictionary of file names, lengths and MVIDs from the ReportFile
				IDictionary<string, (long length, Guid mvid)> reportFileList = GetReportFileList ();
				IEnumerable<string> files = Directory.GetFiles (WorkingDirectory, "*.dll", SearchOption.TopDirectoryOnly);

				foreach (string file in files) {
					//If there is a new assembly in the remote side not present in the report file, we register it for copying back
					if (!reportFileList.TryGetValue (Path.GetFileName (file), out (long length, Guid mvid) localInfo)) {
						changedFiles.Add (new TaskItem (file));
						TryAddPdbFile (file, changedFiles);

						continue;
					}

					var fileInfo = new FileInfo (file);

					//If the file lengths differ, it means local and remote versions are different
					if (fileInfo.Length != localInfo.length) {
						changedFiles.Add (new TaskItem (file));
						TryAddPdbFile (file, changedFiles);

						continue;
					}

					using Stream stream = fileInfo.OpenRead ();
					using var peReader = new PEReader (stream);
					MetadataReader metadataReader = peReader.GetMetadataReader ();
					Guid mvid = metadataReader.GetGuid (metadataReader.GetModuleDefinition ().Mvid);

					//If the MVID from the report file (local MVID) is different than the calculated MVID of the file, it means local and remote versions are different
					if (mvid != localInfo.mvid) {
						changedFiles.Add (new TaskItem (file));
						TryAddPdbFile (file, changedFiles);
					}
				}

				ChangedFiles = [.. changedFiles];

				return true;
			} catch (Exception ex) {
				Log.LogError (MSBStrings.E7148 /* Unable to analyze file changes in '{0}'. An unexpected error occurred. */, WorkingDirectory);
				Log.LogErrorFromException (ex);

				return false;
			}
		}

		IDictionary<string, (long length, Guid mvid)> GetReportFileList ()
		{
			var reportFileList = new Dictionary<string, (long length, Guid mvid)> ();

			//Expected format of the report file lines (defined in the CalculateAssembliesReport task): Foo.dll/23189/768C814C-05C3-4563-9B53-35FEF571968E
			foreach (var line in File.ReadLines (ReportFile!.ItemSpec)) {
				string [] lineParts = line.Split (['/'], StringSplitOptions.RemoveEmptyEntries);

				// Skip lines that don't match the expected format
				if (lineParts.Length == 3 && long.TryParse (lineParts [1], out long fileLength) && Guid.TryParse (lineParts [2], out Guid mvid)) {
					// Adds file name, length and MVID to the dictionary
					reportFileList.Add (lineParts [0], (fileLength, mvid));
				}
			}

			return reportFileList;
		}

		bool TryAddPdbFile (string file, List<ITaskItem> changedFiles)
		{
			var pdbFile = Path.ChangeExtension (file, ".pdb");

			if (!File.Exists (pdbFile)) {
				return false;
			}

			changedFiles.Add (new TaskItem (pdbFile));

			return true;
		}
	}
}
