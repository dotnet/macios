// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.Common.Logging;

namespace Xharness {
	public enum SpringBoardReportKind {
		// The file is incomplete (or still being written); check it again later.
		NotReady,
		NoMatch,
		// The known launch-time SpringBoard crash, in the right simulator, during the launch window.
		Recoverable,
		// The known SpringBoard crash, in the right simulator, but too late after launch to relaunch.
		OutsideLaunchWindow,
	}

	// Detects the SpringBoard crash that sometimes happens in the simulator right after xharness launches
	// a test app: SpringBoard aborts in -[RBSAssertion dealloc] while taking a wallpaper snapshot, and the
	// test app then stops making progress.
	public class SpringBoardCrashDetector {
		public static readonly TimeSpan ClockTolerance = TimeSpan.FromSeconds (2);
		// In the 19 crashes we've seen on CI, SpringBoard crashed 17-24 seconds after mlaunch started (6-9 seconds
		// after the app launched), and mlaunch took at most 31 seconds to launch the app (in 296 launches).
		public static readonly TimeSpan LaunchWindow = TimeSpan.FromSeconds (60);
		public static readonly TimeSpan PollInterval = TimeSpan.FromSeconds (5);

		static readonly string [] captureTimeFormats = {
			"yyyy-MM-dd HH:mm:ss.FFFFFFF zzz",
			"yyyy-MM-dd HH:mm:ss zzz",
		};

		readonly string directory;
		readonly string udid;
		readonly DateTimeOffset attemptStart;
		readonly ILog log;
		// Reports that existed before the launch, and reports we've already made a decision about.
		readonly HashSet<string> seen;

		public static string DefaultDirectory => Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), "Library", "Logs", "DiagnosticReports");

		public SpringBoardCrashDetector (string directory, string udid, DateTimeOffset attemptStart, ILog log)
		{
			this.directory = directory;
			this.udid = udid;
			this.attemptStart = attemptStart;
			this.log = log;
			seen = new HashSet<string> (ListReports (), StringComparer.Ordinal);
		}

		IEnumerable<string> ListReports ()
		{
			if (!Directory.Exists (directory))
				return [];
			return Directory.EnumerateFiles (directory, "SpringBoard*.ips");
		}

		// Returns the path of a new report with the recoverable crash, if there is one.
		public string? Poll ()
		{
			foreach (var path in ListReports ().OrderBy (v => v, StringComparer.Ordinal)) {
				if (seen.Contains (path))
					continue;

				var (kind, reason) = Evaluate (path, udid, attemptStart);
				if (kind == SpringBoardReportKind.NotReady)
					continue;

				seen.Add (path);
				if (kind == SpringBoardReportKind.Recoverable)
					return path;
				log.WriteLine ($"Ignoring SpringBoard crash report {path}: {reason}");
			}
			return null;
		}

		// Polls until a report with the recoverable crash shows up, or until cancelled (in which case null is returned).
		public async Task<string?> WaitForCrashAsync (CancellationToken cancellationToken)
		{
			try {
				while (true) {
					var path = Poll ();
					if (path is not null)
						return path;
					await Task.Delay (PollInterval, cancellationToken);
				}
			} catch (OperationCanceledException) {
				return null;
			} catch (Exception e) {
				// Never let crash detection break the test run.
				log.WriteLine ($"Stopped looking for SpringBoard crash reports: {e.Message}");
				return null;
			}
		}

		public static (SpringBoardReportKind Kind, string Reason) Evaluate (string path, string udid, DateTimeOffset attemptStart)
		{
			string text;
			try {
				text = File.ReadAllText (path);
			} catch (IOException) {
				return (SpringBoardReportKind.NotReady, "the file couldn't be read");
			} catch (UnauthorizedAccessException) {
				return (SpringBoardReportKind.NoMatch, "the file couldn't be read");
			}

			// An .ips file is a one-line JSON header followed by a JSON body.
			var newline = text.IndexOf ('\n');
			if (newline < 0)
				return (SpringBoardReportKind.NotReady, "the file is incomplete");

			JsonDocument header;
			JsonDocument body;
			try {
				header = JsonDocument.Parse (text.Substring (0, newline));
			} catch (JsonException) {
				return (SpringBoardReportKind.NotReady, "the header is incomplete");
			}
			using (header) {
				try {
					body = JsonDocument.Parse (text.Substring (newline + 1));
				} catch (JsonException) {
					return (SpringBoardReportKind.NotReady, "the report is incomplete");
				}
				using (body)
					return Evaluate (header.RootElement, body.RootElement, udid, attemptStart);
			}
		}

		static (SpringBoardReportKind Kind, string Reason) Evaluate (JsonElement header, JsonElement body, string udid, DateTimeOffset attemptStart)
		{
			if (GetString (header, "app_name") != "SpringBoard" || GetString (header, "bug_type") != "309")
				return (SpringBoardReportKind.NoMatch, "not a SpringBoard crash report");

			var coalition = GetString (body, "coalitionName");
			if (coalition != "com.apple.CoreSimulator.SimDevice." + udid)
				return (SpringBoardReportKind.NoMatch, $"it's for a different simulator ({coalition})");

			// Don't use the header's 'timestamp': it's when the report was written, which can be much later than the crash.
			var captureTimeValue = GetString (body, "captureTime");
			if (!TryParseCaptureTime (captureTimeValue, out var captureTime))
				return (SpringBoardReportKind.NoMatch, $"the capture time '{captureTimeValue}' couldn't be parsed");
			if (captureTime < attemptStart - ClockTolerance)
				return (SpringBoardReportKind.NoMatch, $"it's from before this launch ({captureTimeValue})");

			if (!body.TryGetProperty ("exception", out var exception) || GetString (exception, "signal") != "SIGABRT")
				return (SpringBoardReportKind.NoMatch, "it's not a SIGABRT");

			if (!HasWallpaperSnapshotAssertion (body))
				return (SpringBoardReportKind.NoMatch, "it doesn't have the -[RBSAssertion dealloc] wallpaper snapshot crash");

			if (captureTime > attemptStart + LaunchWindow)
				return (SpringBoardReportKind.OutsideLaunchWindow, $"the crash happened too long after the launch to relaunch ({captureTimeValue})");

			return (SpringBoardReportKind.Recoverable, $"SpringBoard crashed at {captureTimeValue}");
		}

		// Checks that the crashed thread has an NSAssertionHandler failure in -[RBSAssertion dealloc],
		// called from a wallpaper snapshot in PosterUIFoundation or PaperBoardUI.
		static bool HasWallpaperSnapshotAssertion (JsonElement body)
		{
			if (!body.TryGetProperty ("threads", out var threads) || threads.ValueKind != JsonValueKind.Array)
				return false;
			if (!body.TryGetProperty ("usedImages", out var images) || images.ValueKind != JsonValueKind.Array)
				return false;

			foreach (var thread in threads.EnumerateArray ()) {
				if (!thread.TryGetProperty ("triggered", out var triggered) || triggered.ValueKind != JsonValueKind.True)
					continue;
				if (!thread.TryGetProperty ("frames", out var frames) || frames.ValueKind != JsonValueKind.Array)
					return false;

				var step = 0;
				foreach (var frame in frames.EnumerateArray ()) {
					var symbol = GetString (frame, "symbol") ?? "";
					var image = GetImageName (images, frame);
					switch (step) {
					case 0:
						if (image == "Foundation" && symbol.StartsWith ("-[NSAssertionHandler handleFailureInMethod:", StringComparison.Ordinal))
							step++;
						break;
					case 1:
						if (image == "RunningBoardServices" && symbol == "-[RBSAssertion dealloc]")
							step++;
						break;
					case 2:
						if ((image == "PosterUIFoundation" || image == "PaperBoardUI") && symbol.Contains ("napshot", StringComparison.Ordinal))
							return true;
						break;
					}
				}
				return false;
			}
			return false;
		}

		static string? GetImageName (JsonElement images, JsonElement frame)
		{
			if (!frame.TryGetProperty ("imageIndex", out var index) || !index.TryGetInt32 (out var i))
				return null;
			if (i < 0 || i >= images.GetArrayLength ())
				return null;
			return GetString (images [i], "name");
		}

		static string? GetString (JsonElement element, string property)
		{
			if (element.ValueKind != JsonValueKind.Object || !element.TryGetProperty (property, out var value) || value.ValueKind != JsonValueKind.String)
				return null;
			return value.GetString ();
		}

		// The capture time looks like this: "2026-11-27 04:31:20.2029 -0800"
		public static bool TryParseCaptureTime (string? value, out DateTimeOffset captureTime)
		{
			captureTime = default;
			if (string.IsNullOrEmpty (value))
				return false;

			// Convert the "-0800" offset into "-08:00", which is what 'zzz' expects.
			var space = value.LastIndexOf (' ');
			if (space < 0)
				return false;
			var offset = value.Substring (space + 1);
			if (offset.Length != 5 || (offset [0] != '+' && offset [0] != '-'))
				return false;
			value = value.Substring (0, space + 1) + offset.Substring (0, 3) + ":" + offset.Substring (3);

			return DateTimeOffset.TryParseExact (value, captureTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out captureTime);
		}
	}
}
