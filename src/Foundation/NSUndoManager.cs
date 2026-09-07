//
// This file implements the NSUndoManager interfase
//
// Authors:
//   Paola Villarreal
//
// Copyright 2015 Xamarin Inc.
//
//

#nullable enable

namespace Foundation {
	public partial class NSUndoManager {
		/// <summary>Returns the modes governing the types of input handled during a cycle of the run loop.</summary>
		/// <value>An array of run loop modes in which the undo manager accepts undo and redo operations.</value>
		public NSRunLoopMode [] RunLoopModes {
			get {
				var modes = WeakRunLoopModes;
				if (modes is null)
					return Array.Empty<NSRunLoopMode> ();

				var array = new NSRunLoopMode [modes.Length];
				for (int n = 0; n < modes.Length; n++)
					array [n] = NSRunLoopModeExtensions.GetValue (modes [n]);
				return array;
			}
			set {
				WeakRunLoopModes = value?.GetConstants ()!;
			}
		}
	}
}
