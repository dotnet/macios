// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

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
			GC.KeepAlive (typeof (MyRequiresDynamicCodeClass));
			GC.KeepAlive (typeof (MyRequiresBothClass));
			GC.KeepAlive (typeof (MyMethodLevelAnnotatedClass));

			return args.Length;
		}
	}

	// Scenario 1: [RequiresUnreferencedCode] on the class (IL2026)
	// Simulates HybridWebViewHandler.SchemeHandler pattern.
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

	// Scenario 2: [RequiresDynamicCode] on the class (IL3050)
	// Tests whether the registrar also triggers IL3050 for types that
	// require dynamic code generation.
	[RequiresDynamicCode ("This type requires dynamic code generation for testing purposes.")]
	public class MyRequiresDynamicCodeClass : NSObject {
		[Export ("value")]
		public NSObject? Value { get; }

		[Export ("compute")]
		public void Compute ()
		{
			// Intentionally empty
		}
	}

	// Scenario 3: Both [RequiresUnreferencedCode] and [RequiresDynamicCode] (IL2026 + IL3050)
	// Tests the combination of both attributes on the same class.
	[RequiresUnreferencedCode ("This type uses dynamic features for testing purposes.")]
	[RequiresDynamicCode ("This type requires dynamic code generation for testing purposes.")]
	public class MyRequiresBothClass : NSObject {
		[Export ("result")]
		public NSObject? Result { get; }

		[Export ("process")]
		public void Process ()
		{
			// Intentionally empty
		}
	}

	// Scenario 4: Trim attributes on individual methods rather than the class (IL2026 + IL3050)
	// Tests whether the registrar also warns when the annotation is on the method
	// rather than the declaring type.
	public class MyMethodLevelAnnotatedClass : NSObject {
		[Export ("unsafeCompute")]
		[RequiresUnreferencedCode ("This method uses dynamic features.")]
		public void UnsafeCompute ()
		{
			// Intentionally empty
		}

		[Export ("unsafeDynamic")]
		[RequiresDynamicCode ("This method requires dynamic code.")]
		public void UnsafeDynamic ()
		{
			// Intentionally empty
		}

		[Export ("safeMethod")]
		public void SafeMethod ()
		{
			// This method has no trim annotations - should never warn
		}
	}
}

