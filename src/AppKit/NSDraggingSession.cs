#if !__MACCATALYST__
using System;
using System.Runtime.InteropServices;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	public partial class NSDraggingSession {
		public void EnumerateDraggingItems (NSDraggingItemEnumerationOptions enumOpts, NSView view, INSPasteboardReading [] classArray, NSDictionary searchOptions, NSDraggingEnumerator enumerator)
		{
			var nsa_classArray = NSArray.FromNSObjects (classArray);
			EnumerateDraggingItems (enumOpts, view, nsa_classArray.Handle, searchOptions, enumerator);
			nsa_classArray.Dispose ();
		}

		/// <param name="enumOpts">To be added.</param>
		///         <param name="view">To be added.</param>
		///         <param name="classArray">To be added.</param>
		///         <param name="searchOptions">To be added.</param>
		///         <param name="enumerator">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void EnumerateDraggingItems (NSDraggingItemEnumerationOptions enumOpts, NSView view, NSArray classArray, NSDictionary searchOptions, NSDraggingEnumerator enumerator)
		{
			EnumerateDraggingItems (enumOpts, view, classArray.Handle, searchOptions, enumerator);
			GC.KeepAlive (classArray);
		}
	}
}
#endif // !__MACCATALYST__
