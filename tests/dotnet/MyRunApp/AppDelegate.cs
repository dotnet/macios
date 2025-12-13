using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Foundation;

namespace MyRunApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			var testCaseString = Environment.GetEnvironmentVariable ("TEST_CASE");
			if (string.IsNullOrEmpty (testCaseString)) {
				Console.WriteLine ($"The environment variable TEST_CASE wasn't set.");
				return 2;
			}

			var testCase = int.Parse (testCaseString);
			switch (testCase) {
			case 1:
				var filename = Environment.GetEnvironmentVariable ("TEST_FILENAME");
				if (string.IsNullOrEmpty (filename)) {
					Console.WriteLine ($"The environment variable TEST_FILENAME wasn't set.");
					return 3;
				}

				var sb = new StringBuilder ();
				foreach (var kvp in Environment.GetEnvironmentVariables ().Cast<DictionaryEntry> ().OrderBy (v => v.Key)) {
					sb.AppendLine ($"{kvp.Key}={kvp.Value}");
				}
				File.WriteAllText (filename, sb.ToString ());

				return 0;
			}

			return 1;
		}
	}
}
