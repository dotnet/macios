// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

class Program {
	static int Main (string [] args)
	{
		Console.Error.WriteLine ("error: sharpie is not supported on x64. Please use an Apple Silicon (arm64) Mac.");
		return 1;
	}
}
