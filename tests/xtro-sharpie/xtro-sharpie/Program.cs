using System;
using System.Collections.Generic;

using Mono.Options;

namespace Extrospection {
	class MainClass {

		static int Main (string [] arguments)
		{
			var outputDirectory = string.Empty;
			var searchDirectories = new List<string> ();
			var rspFile = string.Empty;
			var options = new OptionSet {
				{ "output-directory=", (v) => outputDirectory = v },
				{ "lib=", (v) => searchDirectories.Add (v) },
				{ "rsp=", (v) => rspFile = v },
				new ResponseFileSource (),
			};
			var args = options.Parse (arguments);

			if (args.Count < 1) {
				Console.Error.WriteLine ("Usage: xtro-sharpie.exe [--output-directory=<output directory>] [--lib=<assembly search directory>] --rsp=<response file> dll-file");
				return 1;
			}

			try {
				var assemblies = new List<string> (args);
				new Runner ().Execute (assemblies, outputDirectory, searchDirectories, rspFile);
				return 0;
			} catch (Exception e) {
				Console.WriteLine (e);
				return 1;
			}
		}
	}
}
