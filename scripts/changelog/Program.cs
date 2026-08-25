// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace changelog {

	class Program {
		static readonly List<string> list = new ();
		static readonly List<string> filters = new ();

		// current repo (1) points to dotnet/installer (2)
		// getting into other repos (pointed by dotnet/installer) can show different results
		static int level = 2;

		static async Task Main (string [] args)
		{
			if (args.Length < 2) {
				Console.Error.WriteLine ("Usage: changelog <pull-request-url-or-diff-file> <output-file> [repo-filter ...]");
				Environment.ExitCode = 1;
				return;
			}

			var pr = args [0];
			var outputFile = args [1];
			for (int i = 2; i < args.Length; i++)
				filters.Add (args [i]);

			using (var writer = new StreamWriter (outputFile)) {
				writer.WriteLine ($"# .NET Changelog for {pr}");
				if (TryGetGitHubPullRequestDiffUrl (pr, out var diffUrl))
					pr = diffUrl;
				list.Add (pr);
				await Process (writer);
				writer.WriteLine ("Generated using scripts/changelog");
			}
		}

		static bool TryGetGitHubPullRequestDiffUrl (string url, out string diffUrl)
		{
			diffUrl = url;
			if (!Uri.TryCreate (url, UriKind.Absolute, out var uri))
				return false;
			if (uri.Host != "github.com")
				return false;
			if (url.EndsWith (".diff", StringComparison.Ordinal))
				return false;

			var parts = uri.AbsolutePath.Split ('/', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 4 || parts [2] != "pull")
				return false;

			diffUrl = $"https://patch-diff.githubusercontent.com/raw/{string.Join ('/', parts)}.diff";
			return true;
		}

		static async Task Process (TextWriter writer)
		{
			using var client = new HttpClient ();
			for (int i = 0; i < Math.Min (list.Count, level); i++) {
				writer.WriteLine ($"## Level {i + 1}");
				var url = list [i];
				if (url.StartsWith ("https://", StringComparison.Ordinal)) {
					using var result = await client.GetAsync (list [i]);
					result.EnsureSuccessStatusCode ();
					using var stream = await result.Content.ReadAsStreamAsync ();
					ProcessDiff (stream, writer);
				} else {
					using var stream = new FileStream (url, FileMode.Open, FileAccess.Read);
					ProcessDiff (stream, writer);
				}
				writer.WriteLine ();
			}
		}

		static bool Include (string uri)
		{
			if (filters.Count == 0)
				return true;

			foreach (var filter in filters) {
				if (uri.EndsWith (filter, StringComparison.Ordinal))
					return true;
			}
			return false;
		}

		static void ProcessDiff (Stream s, TextWriter writer)
		{
			bool processing = false;
			var uri = "";
			var old_sha = "";
			var new_sha = "";
			using (var sr = new StreamReader (s)) {
				while (!sr.EndOfStream) {
					var line = sr.ReadLine ();
					if (line is null)
						break;
					if (line == "diff --git a/eng/Version.Details.xml b/eng/Version.Details.xml") {
						processing = true;
						continue;
					}
					if (processing) {
						if (line.StartsWith ("diff --git ", StringComparison.Ordinal))
							return;
						if (line.Length < 1)
							continue;
						bool removal = (line [0] == '-');
						bool addition = (line [0] == '+');
						var tl = (removal || addition) ? line [1..] : line;
						tl = tl.Trim ();
						if (tl.StartsWith ("<Uri>", StringComparison.Ordinal)) {
							uri = tl [5..^6];
							old_sha = "";
							new_sha = "";
						} else if (removal && tl.StartsWith ("<Sha>", StringComparison.Ordinal)) {
							old_sha = tl [5..^6];
						} else if (addition && tl.StartsWith ("<Sha>", StringComparison.Ordinal)) {
							new_sha = tl [5..^6];
							if (!Include (uri))
								continue;
							if (string.IsNullOrEmpty (old_sha)) {
								writer.WriteLine ($"* {uri} [{new_sha [0..7]}]({uri}/commits/{new_sha}) (new dependency)");
							} else {
								var diff_url = $"{uri}/compare/{old_sha}..{new_sha}.diff";
								writer.WriteLine ($"* {uri} [{old_sha [0..7]}...{new_sha [0..7]}]({uri}/compare/{old_sha}...{new_sha})");
								// skip duplicates (if same revisions are used)
								if (!list.Contains (diff_url))
									list.Add (diff_url);
							}
						}
					}
				}
			}
		}
	}
}
