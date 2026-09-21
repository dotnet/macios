#if !__MACCATALYST__

using CoreGraphics;

#nullable enable

namespace AppKit {
	public partial class NSPasteboard {
		/// <summary>Writes the specified objects to the pasteboard.</summary>
		/// <param name="objects">The objects to write to the pasteboard.</param>
		/// <returns><see langword="true" /> if the objects were written successfully; otherwise, <see langword="false" />.</returns>
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
