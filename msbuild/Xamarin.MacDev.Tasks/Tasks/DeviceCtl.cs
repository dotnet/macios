// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	// Copies a file to a device's app data container using 'xcrun devicectl device copy to',
	// and extracts the resulting on-device path from devicectl's json output.
	public class DeviceCtl : XamarinTask {
		#region Inputs

		[Required]
		public string Device { get; set; } = "";

		[Required]
		public string CopySource { get; set; } = "";

		[Required]
		public string CopyDestination { get; set; } = "";

		[Required]
		public string DomainType { get; set; } = "";

		[Required]
		public string DomainIdentifier { get; set; } = "";

		#endregion

		#region Outputs

		[Output]
		public string? CopiedDevicePath { get; set; }

		#endregion

		public override bool Execute ()
		{
			var jsonOutput = Path.GetTempFileName ();

			try {
				var arguments = new List<string> {
					"devicectl",
					"device",
					"copy",
					"to",
					"--device", Device,
					"--source", CopySource,
					"--domain-type", DomainType,
					"--domain-identifier", DomainIdentifier,
					"--destination", CopyDestination,
					"--json-output", jsonOutput,
				};

				var rv = ExecuteAsync ("xcrun", arguments).Result;
				if (rv.ExitCode != 0 || Log.HasLoggedErrors)
					return false;

				CopiedDevicePath = GetDestinationPath (jsonOutput);

				return !Log.HasLoggedErrors;
			} finally {
				if (File.Exists (jsonOutput))
					File.Delete (jsonOutput);
			}
		}

		// devicectl reports the on-device destination in its JSON output as
		// { "result": { "destination": "file:///private/var/..." } }; extract that path.
		string GetDestinationPath (string jsonOutput)
		{
			var json = File.Exists (jsonOutput) ? File.ReadAllText (jsonOutput) : "";

			try {
				using var document = JsonDocument.Parse (json);
				if (document.RootElement.TryGetProperty ("result", out var result)
					&& result.TryGetProperty ("destination", out var destination)
					&& destination.GetString () is string path) {
					if (path.StartsWith ("file://", StringComparison.Ordinal))
						path = path.Substring ("file://".Length);
					return path;
				}
			} catch (JsonException) {
				// Fall through to the error below.
			}

			Log.LogError (MSBStrings.E7184 /* Could not determine the on-device destination path after copying '{0}' to the device. The 'xcrun devicectl' JSON output was: {1} */, CopySource, json);
			return "";
		}
	}
}
