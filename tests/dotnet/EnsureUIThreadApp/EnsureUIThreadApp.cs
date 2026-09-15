// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace EnsureUIThreadApp {
	public class Program {
		static int Main (string [] args)
		{
			// Reference [NS|UI]Application.EnsureUIThread so the linker keeps the method (this makes it
			// possible to inspect the method body in the linked platform assembly). The call is guarded
			// so that it never actually executes (which could otherwise throw when not on the UI thread) -
			// the linker only needs to see the call instruction to keep the method reachable.
			if (args.Length > 1_000_000) {
#if __MACOS__
				AppKit.NSApplication.EnsureUIThread ();
#else
				UIKit.UIApplication.EnsureUIThread ();
#endif
			}

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return args.Length;
		}
	}
}
