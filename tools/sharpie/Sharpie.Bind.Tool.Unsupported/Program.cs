// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

class Program {
	static int Main (string [] args)
	{
		Console.Error.WriteLine ("error: sharpie is not supported with an x64 runtime.");
		Console.Error.WriteLine ();
		Console.Error.WriteLine ("sharpie requires Apple's libclang, which is only available for arm64.");
		Console.Error.WriteLine ("Please install the arm64 version of .NET and run sharpie with the arm64 .NET runtime.");
		return 1;
	}
}
