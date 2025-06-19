using System;
using System.IO;

#nullable enable

namespace Xamarin.Tests {
	public static class Extensions {
		public static void AssertNoWarnings (this ExecutionResult result, Func<BuildLogEvent, bool>? filter = null)
		{
			var warnings = BinLog.GetBuildLogWarnings (result.BinLogPath);
			if (filter is not null)
				warnings = warnings.Where (filter);
			if (!warnings.Any ())
				return;

			Assert.Fail ($"No warnings expected, but got:\n\t{string.Join ("\n\t", warnings.Select (v => v.ToString ()))}");
		}

		public static void AssertWarnings (this IEnumerable<BuildLogEvent> actualWarnings, IEnumerable<ExpectedBuildMessage> expectedWarnings)
		{
			// Source paths may be full (and local) paths. So make full paths relative to the root folder of xamarin-macios.
			// We have noticed that in certain bots the SourceRoot property is different to the one we expect, so instead we 
			// use the fact that we know we are in the macios folder and we can use that to calculate the relative path.
			actualWarnings = actualWarnings.Select (w => {
				var path = w.File ?? string.Empty;
				var file = w.File;
				var message = w.Message?.Replace (Configuration.SourceRoot, "<root>");
				var clone = message != w.Message;
				if (!string.IsNullOrEmpty (path) && Path.IsPathRooted (path)) {
					// use the last index of the /macios/ since some paths might contain it more than once
					file = path.Substring (path.LastIndexOf ("/macios/") + "/macios/".Length);
					clone = true;
				}
				if (clone) {
					var rv = w.Clone ();
					rv.File = file;
					rv.Message = message;
					return rv;
				}
				return w;
			});

			var newWarnings = actualWarnings.Where (v => !expectedWarnings.Any (x => x.IsMatch (v))).ToArray ();
			var missingWarnings = expectedWarnings.Where (v => !actualWarnings.Any (x => v.IsMatch (x))).ToArray ();

			if (newWarnings.Length == 0 && missingWarnings.Length == 0)
				return;

			const string indent = "\t\t\t\t";
			var sb = new StringBuilder ();
			sb.AppendLine ($"{indent}expectedWarnings = new ExpectedBuildMessage [] {{");
			foreach (var w in actualWarnings.OrderBy (v => v.File).ThenBy (v => v.Message)) {
				var simplify = new Func<string, string> (v => {
					foreach (var rid in Configuration.GetAllRuntimeIdentifiers ()) {
						v = v.Replace ($"/{rid}/", "{runtimeIdentifierInfix}");
						v = v.Replace (rid, "{runtimeIdentifier}");
					}
					foreach (var pl in Configuration.GetAllPlatforms ()) {
						v = v.Replace (pl.AsString ().ToLower (), "{platform.AsString ().ToLower ()}");
						v = v.Replace (pl.AsString (), "{platform.AsString ()}");
					}
					v = v.Replace (Configuration.DotNetTfm, "{Configuration.DotNetTfm}");
					v = v.Replace ("/Debug/", "/{config}/");
					v = v.Replace ("/Release/", "/{config}/");
					return v;
				});
				var file = simplify (w.File ?? string.Empty);
				var msg = simplify (w.Message ?? string.Empty).Replace ("\"", "\\\"");

				if (w.LineNumber != 0) {
					sb.AppendLine ($"{indent}\tnew ExpectedBuildMessage ($\"{file}\" /* line {w.LineNumber} */, $\"{msg}\"),");
				} else {
					sb.AppendLine ($"{indent}\tnew ExpectedBuildMessage ($\"{file}\", $\"{msg}\"),");
				}
			}
			sb.AppendLine ($"{indent}}};");

			if (newWarnings.Length > 0) {
				Console.WriteLine ($"Got {newWarnings.Length} new warnings:");
				Console.WriteLine ();
				foreach (var evt in newWarnings) {
					if (evt.LineNumber != 0) {
						Console.WriteLine ($"    {evt.File} {evt.Message}");
					} else {
						Console.WriteLine ($"    {evt.File}:{evt.LineNumber} {evt.Message}");
					}
				}
			}
			if (missingWarnings.Length > 0) {
				Console.WriteLine ($"Did not get {missingWarnings.Length} missing warnings:");
				Console.WriteLine ();
				foreach (var evt in missingWarnings)
					Console.WriteLine ($"    {evt.File}: {evt.Message}");
			}
			var fn = Path.Combine (Cache.CreateTemporaryDirectory ("warnings"), "updated-warnings.txt");
			Console.WriteLine ($"If this is expected, an updated list of expected warnings in stored in {fn}");
			File.WriteAllText (fn, sb.ToString ());

			// Rather than doing an Assert.IsEmpty, which produces a horrendous error message, we'll do an Assert.Multiple which generates a 
			// nice enumerated output of all the failures.
			Assert.Multiple (() => {
				// fail for each of the new warnings
				foreach (var evt in newWarnings) {
					if (evt.LineNumber != 0) {
						Assert.Fail ($"Unexpected warning: {evt.File}:{evt.LineNumber} {evt.Message}");
					} else {
						Assert.Fail ($"Unexpected warning: {evt.File}: {evt.Message}");
					}
				}

				// fail for each of the missing warnings
				foreach (var evt in missingWarnings)
					Assert.Fail ($"Missing warning: {evt.File}: {evt.Message}");
			});
		}
	}

	public class ExpectedBuildMessage {
		public string File;
		public string Message;

		public ExpectedBuildMessage (string file, string message)
		{
			File = file;
			Message = message;
		}

		public bool IsMatch (BuildLogEvent evt)
		{
			if (evt.Message != Message)
				return false;
			if (evt.File != File)
				return false;
			return true;
		}

		public override string? ToString ()
		{
			return $"{File}: {Message}";
		}
	}
}
