// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Sharpie.Bind;

class Program {
	static void ShowHelp ()
	{
		var help =
"""
usage: sharpie [OPTIONS] TOOL [TOOL_OPTIONS]

Copyright 2026 Microsoft. All rights reserved.

Documentation:
  https://aka.ms/sharpie-docs

Options:
  -h, -help        Show detailed help
  -v, -version     Show version information

Available Tools:
  bind               Create a C# binding to Objective-C APIs
  sdk-db             Generate bindings for an entire platform's SDK
""";
		Console.WriteLine (help);
	}

	static int Main (string [] args)
	{
		try {
			if (args.Length == 0) {
				ShowHelp ();
				return 0;
			}

			switch (args [0]) {
			case "-h":
			case "--help":
			case "-help":
			case "help":
			case "?":
				ShowHelp ();
				return 0;
			case "-v":
			case "--version":
			case "-version":
				Console.WriteLine ($"dotnet-sharpie {Xamarin.ProductConstants.SharpieVersion}");
				return 0;
			case "bind":
				return Tools.Bind (args.Skip (1).ToArray ());
			case "sdk-db":
				return Tools.SdkDb (args.Skip (1).ToArray ());
			default:
				Console.Error.WriteLine ($"Unknown option: {args [0]}");
				return 1;
			}
		} catch (Exception e) {
			Console.Error.WriteLine ("An unexpected exception occurred: " + e);
			return 2;
		}
	}
}
