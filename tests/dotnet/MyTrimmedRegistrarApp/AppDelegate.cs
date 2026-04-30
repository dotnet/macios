// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace MyTrimmedRegistrarApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly
			GC.KeepAlive (typeof (MyRequiresUnreferencedCodeClass));

			return args.Length;
		}
	}

	// This class simulates the pattern from HybridWebViewHandler.SchemeHandler:
	// An NSObject subclass that has [RequiresUnreferencedCode] and exports ObjC members
	// including a property getter. The managed registrar generates trampolines that
	// reference these members, which triggers IL2026 when trimming is enabled.
	[RequiresUnreferencedCode ("This type uses dynamic features for testing purposes.")]
	public class MyRequiresUnreferencedCodeClass : NSObject {
		[Export ("handler")]
		public NSObject? Handler { get; }

		[Export ("doSomething")]
		public void DoSomething ()
		{
			// Intentionally empty
		}
	}
}
