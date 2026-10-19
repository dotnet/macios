// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using ObjCRuntime;

namespace EnsureUIThreadApp {
	class GenericObject<T> : NSObject where T : NSObject {
		protected GenericObject (NativeHandle handle)
			: base (handle)
		{
		}
	}

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
				using var field = new AppKit.NSComboBox ();
				field.SelectionChanged += (sender, e) => { };
				field.Delegate = new AppKit.NSComboBoxDelegate ();
#else
				UIKit.UIApplication.EnsureUIThread ();
				using var field = new UIKit.UITextField ();
				field.Started += (sender, e) => { };
				field.Delegate = new UIKit.UITextFieldDelegate ();
#endif
				Runtime.GetNSObjectChecked<GenericObject<NSString>> (IntPtr.Zero);
				Runtime.GetINativeObject<GenericObject<NSString>> (IntPtr.Zero, false);
			}

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return args.Length;
		}
	}
}
