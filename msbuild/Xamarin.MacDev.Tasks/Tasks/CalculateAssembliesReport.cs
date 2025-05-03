using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using Microsoft.Build.Framework;
using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;

namespace Xamarin.MacDev.Tasks {
	public class CalculateAssembliesReport : XamarinTask {
		[Required]
		public string WorkingDirectory { get; set; } = string.Empty;

		[Required]
		public string TargetReportFile { get; set; } = string.Empty;

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ()) {
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;
			}

			if (!Directory.Exists (WorkingDirectory)) {
				Log.LogError (MSBStrings.E7149 /* Unable to calculate the assemblies report from '{0}'. Directory not found. */, WorkingDirectory);

				return false;
			}

			try {
				var reportEntriesBuilder = new StringBuilder ();
				IEnumerable<string> files = Directory.GetFiles (WorkingDirectory, "*.dll", SearchOption.TopDirectoryOnly);

				foreach (var file in files) {
					try {
						var fileInfo = new FileInfo (file);
						using Stream stream = fileInfo.OpenRead ();
						using var peReader = new PEReader (stream);
						MetadataReader metadataReader = peReader.GetMetadataReader ();
						Guid mvid = metadataReader.GetGuid (metadataReader.GetModuleDefinition ().Mvid);

						//Appending the file name, length and mvid like: Foo.dll/23189/768C814C-05C3-4563-9B53-35FEF571968E
						reportEntriesBuilder.AppendLine ($"{Path.GetFileName (file)}/{fileInfo.Length}/{mvid}");
					} catch (Exception) {
						Log.LogWarning (MSBStrings.W7145 /* Unable to retrieve information from '{0}'. The file may not be a valid PE file."\ */, Path.GetFileName (file));
						continue;
					}
				}

				//Creates or overwrites the report file
				File.WriteAllText (TargetReportFile, reportEntriesBuilder.ToString ());

				return true;
			} catch (Exception ex) {
				Log.LogError (MSBStrings.E7150 /* Unable to calculate assemblies report from '{0}'. An unexpected error occurred. */, WorkingDirectory);
				Log.LogErrorFromException (ex);

				return false;
			}
		}
	}
}
