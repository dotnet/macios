// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Build.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// This task merges the 'runtimeOptions.configProperties' from a '*.runtimeconfig.dev.json' file on top of
	// the ones from the main '*.runtimeconfig.json' file, writing the merged result to an output file.
	//
	// We do this because we hand the runtime configuration directly to the runtime (bypassing hostfxr), and
	// hostfxr is what would normally merge the dev file (which contains Debug-only switches such as Hot Reload)
	// into the main runtime configuration. The dev values win, matching hostfxr's behavior.
	public class MergeRuntimeConfigFiles : XamarinTask {
		[Required]
		public string RuntimeConfigFile { get; set; } = "";

		public string? RuntimeConfigDevFile { get; set; }

		[Required]
		public string OutputFile { get; set; } = "";

		static readonly JsonDocumentOptions documentOptions = new JsonDocumentOptions {
			AllowTrailingCommas = true,
			CommentHandling = JsonCommentHandling.Skip,
		};

		public override bool Execute ()
		{
			var mainNode = JsonNode.Parse (File.ReadAllText (RuntimeConfigFile), documentOptions: documentOptions);
			if (mainNode is not JsonObject mainObject) {
				Log.LogError (MSBStrings.E7185 /* The runtime configuration file '{0}' is not a valid JSON object. */, RuntimeConfigFile);
				return false;
			}

			if (!string.IsNullOrEmpty (RuntimeConfigDevFile) && File.Exists (RuntimeConfigDevFile)) {
				var devNode = JsonNode.Parse (File.ReadAllText (RuntimeConfigDevFile!), documentOptions: documentOptions);
				var devConfigProperties = (devNode as JsonObject)? ["runtimeOptions"]? ["configProperties"] as JsonObject;
				if (devConfigProperties is not null && devConfigProperties.Count > 0) {
					var runtimeOptions = mainObject ["runtimeOptions"] as JsonObject;
					if (runtimeOptions is null) {
						runtimeOptions = new JsonObject ();
						mainObject ["runtimeOptions"] = runtimeOptions;
					}

					var configProperties = runtimeOptions ["configProperties"] as JsonObject;
					if (configProperties is null) {
						configProperties = new JsonObject ();
						runtimeOptions ["configProperties"] = configProperties;
					}

					foreach (var property in devConfigProperties) {
						configProperties [property.Key] = property.Value is null ? null : JsonNode.Parse (property.Value.ToJsonString ());
					}
				}
			}

			var outputDirectory = Path.GetDirectoryName (OutputFile);
			if (!string.IsNullOrEmpty (outputDirectory))
				Directory.CreateDirectory (outputDirectory!);

			File.WriteAllText (OutputFile, mainObject.ToJsonString (new JsonSerializerOptions { WriteIndented = true }));

			return !Log.HasLoggedErrors;
		}
	}
}
