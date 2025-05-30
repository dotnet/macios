#if !__MACCATALYST__
using System;
using System.Runtime.InteropServices;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	public partial class NSPasteboard {
		/// <param name="objects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool WriteObjects (INSPasteboardWriting [] objects)
		{
			var nsa_pasteboardReading = NSArray.FromNSObjects (objects);
			bool result = WriteObjects (nsa_pasteboardReading.Handle);
			nsa_pasteboardReading.Dispose ();
			return result;
		}
	}
}
#endif // !__MACCATALYST__
