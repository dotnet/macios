// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

class Program {
	static int Main (string [] args)
	{
		Console.Error.WriteLine ("error: sharpie is not supported on x64. Please use an Apple Silicon (arm64) Mac.");
		Console.Error.WriteLine ();
		Console.Error.WriteLine ("sharpie requires Apple's libclang, which is only available for arm64.");
		Console.Error.WriteLine ("If you're running on an Intel Mac, consider using a Mac with Apple Silicon (M1 or later).");
		return 1;
	}
}
