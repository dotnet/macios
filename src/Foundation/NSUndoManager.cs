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

using System;
using Foundation;
using ObjCRuntime;

namespace Foundation {
	public partial class NSUndoManager {
#if !NET
		public virtual void SetActionName (string actionName)
		{
			SetActionname (actionName);
		}
#endif

#if NET
		/// <summary>Returns the modes governing the types of input handled during a cycle of the run loop.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
#endif
	}
}
