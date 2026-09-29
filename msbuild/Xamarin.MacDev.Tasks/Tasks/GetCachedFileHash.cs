// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

extern alias Microsoft_Build_Tasks_Core;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public class GetCachedFileHash : XamarinTask {
		public sealed class CacheEntry {
			public long Length { get; set; }
			public long LastWriteTimeUtcTicks { get; set; }
			public string Hash { get; set; } = "";
		}

		[Required]
		public ITaskItem [] Files { get; set; } = [];

		[Required]
		public string CacheFile { get; set; } = "";

		[Output]
		public ITaskItem [] Items { get; set; } = [];

		[Output]
		public int FilesHashed { get; set; }

		public override bool Execute ()
		{
			var cacheNeedsWriting = !File.Exists (CacheFile);
			var cached = new Dictionary<string, CacheEntry> (StringComparer.Ordinal);
			if (!cacheNeedsWriting) {
				try {
					cached = JsonSerializer.Deserialize<Dictionary<string, CacheEntry>> (File.ReadAllText (CacheFile))
						?? throw new JsonException ("The cache does not contain any entries.");
				} catch (JsonException e) {
					Log.LogWarning ("Ignoring invalid R2R input hash cache '{0}': {1}", CacheFile, e.Message);
					cacheNeedsWriting = true;
				}
			}

			var current = new Dictionary<string, CacheEntry> (StringComparer.Ordinal);
			var filesToHash = new List<ITaskItem> ();
			foreach (var file in Files) {
				var info = new FileInfo (file.ItemSpec);
				if (!info.Exists) {
					Log.LogError ("R2R input file '{0}' does not exist.", file.ItemSpec);
					return false;
				}

				if (cached.TryGetValue (info.FullName, out var entry)) {
					if (entry is null) {
						Log.LogWarning ("Ignoring invalid R2R input hash for '{0}' in cache '{1}'.", info.FullName, CacheFile);
					} else if (entry.Length == info.Length && entry.LastWriteTimeUtcTicks == info.LastWriteTimeUtc.Ticks) {
						if (IsValidHash (entry.Hash)) {
							file.SetMetadata ("FileHash", entry.Hash);
							file.SetMetadata ("FileHashAlgorithm", "SHA256");
							current [info.FullName] = entry;
							continue;
						}
						Log.LogWarning ("Ignoring invalid R2R input hash for '{0}' in cache '{1}'.", info.FullName, CacheFile);
					}
				}
				filesToHash.Add (file);
				current [info.FullName] = new CacheEntry {
					Length = info.Length,
					LastWriteTimeUtcTicks = info.LastWriteTimeUtc.Ticks,
				};
			}

			if (filesToHash.Count > 0) {
				var getFileHash = new Microsoft_Build_Tasks_Core::Microsoft.Build.Tasks.GetFileHash {
					BuildEngine = BuildEngine,
					Files = filesToHash.ToArray (),
					Algorithm = "SHA256",
				};
				if (!getFileHash.Execute ())
					return false;

				foreach (var file in filesToHash)
					current [Path.GetFullPath (file.ItemSpec)].Hash = file.GetMetadata ("FileHash");
			}

			if (cacheNeedsWriting || filesToHash.Count > 0 || current.Count != cached.Count) {
				PathUtils.CreateDirectoryForFile (CacheFile);
				File.WriteAllText (CacheFile, JsonSerializer.Serialize (current));
			}

			FilesHashed = filesToHash.Count;
			Log.LogMessage (MessageImportance.Low, "Hashed {0} of {1} R2R input files.", FilesHashed, Files.Length);
			Items = Files;
			return !Log.HasLoggedErrors;
		}

		static bool IsValidHash (string hash)
		{
			if (hash is null || hash.Length != 64)
				return false;

			foreach (var c in hash) {
				if (c < '0' || (c > '9' && c < 'A') || c > 'F')
					return false;
			}
			return true;
		}
	}
}
