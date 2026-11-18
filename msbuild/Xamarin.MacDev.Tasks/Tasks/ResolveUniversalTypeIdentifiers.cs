using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Xamarin.Localization.MSBuild;
using Xamarin.MacDev.Tasks;
using Xamarin.Messaging.Build.Client;
using Xamarin.Messaging.Build.Contracts;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class ResolveUniversalTypeIdentifiers : XamarinTask {
		[Required]
		public ITaskItem [] ImageAssets { get; set; } = [];

		[Required]
		public string ProjectDir { get; set; } = "";

		public override bool Execute ()
		{
			try {
				LogTaskProperty ("ImageAssets", ImageAssets);
				LogTaskProperty ("ProjectDir", ProjectDir);
				LogTaskProperty ("SessionId", SessionId);

				var connection = BuildConnection.GetAsync (BuildEngine4).Result;
				var buildClient = connection.GetClient (SessionId) as BuildClient;

				if (!connection.IsConnected || buildClient is null) {
					Log.LogWarning (179, null, MSBStrings.E0179, nameof (ResolveUniversalTypeIdentifiers)); // There is no available connection to the Mac. Task '{0}' will not be executed

					return true;
				}

				var contentFiles = ImageAssets
					.Where (asset => Path.GetFileName (asset.ItemSpec) == "Contents.json" &&
						Path.GetDirectoryName (asset.ItemSpec)!.EndsWith (".dataset"))
					.Select (x => x.ItemSpec);

				foreach (var filePath in contentFiles) {
					string content;
					DataSet? dataset = null;

					if (File.Exists (filePath)) {
						content = File.ReadAllText (filePath);
						dataset = JsonConvert.DeserializeObject<DataSet> (content);
					}

					if (dataset is null) {
						Log.LogError (180, null, MSBStrings.E0180, Path.GetDirectoryName (filePath)); // Could not deserialize DataSet {0}
						continue;
					}

					var dataItemsToComplete = dataset
						?.DataItems
						?.Where (item => string.IsNullOrEmpty (item.UniversalTypeIdentifier) &&
							!string.IsNullOrEmpty (item.Filename)).ToList ()
						?? [];

					foreach (var data in dataItemsToComplete) {
						var file = ImageAssets.FirstOrDefault (x => Path.GetFileName (x.ItemSpec) == data.Filename);

						if (file is null) {
							Log.LogWarning (181, null, MSBStrings.E0181, data.Filename); // The file {0} does not have ImageAsset as build action.

							continue;
						}

						var message = new GetUniversalTypeIdentifierMessage {
							Payload = File.ReadAllBytes (file.ItemSpec),
							FileName = Path.GetFileName (data.Filename)
						};

						var response = buildClient
							.RunMessagingAsync<GetUniversalTypeIdentifierMessage, GetUniversalTypeIdentifierResult> (message, timeoutSecs: 10)
							.Result;

						if (string.IsNullOrEmpty (response.UniversalTypeIdentifier))
							Log.LogError (182, null, MSBStrings.E0182, data.Filename, Path.GetDirectoryName (filePath)); // Could not resolve UTI for {0} in catalog {1}

						data.UniversalTypeIdentifier = response.UniversalTypeIdentifier;
					}

					if (dataItemsToComplete.Any ()) {
						content = JsonConvert.SerializeObject (dataset, Formatting.Indented);
						File.WriteAllText (filePath, content);
					}
				}

				return !Log.HasLoggedErrors;
			} catch (Exception ex) {
				Log.LogErrorFromException (ex);

				return false;
			}
		}

		//TODO: Ideally we should get this from the LoggingExtensions in Xamarin.MacDev.Tasks. We would need the reference for that

		void LogTaskProperty (string propertyName, ITaskItem [] items)
		{
			if (items is null) {
				Log.LogMessage (MessageImportance.Normal, "  {0}: <null>", propertyName);

				return;
			}

			Log.LogMessage (MessageImportance.Normal, "  {0}:", propertyName);

			for (int i = 0; i < items.Length; i++)
				Log.LogMessage (MessageImportance.Normal, "    {0}", items [i].ItemSpec);
		}

		void LogTaskProperty (string propertyName, string value)
		{
			Log.LogMessage (MessageImportance.Normal, "  {0}: {1}", propertyName, value ?? "<null>");
		}
	}
}
