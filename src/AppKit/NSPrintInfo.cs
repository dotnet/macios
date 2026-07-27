#if !__MACCATALYST__
using PrintCore;

#nullable enable

namespace AppKit {
	public partial class NSPrintInfo {
		/// <summary>Returns the print session associated with this print info.</summary>
		public PMPrintSession GetPrintSession ()
		{
			var ptr = GetPMPrintSession ();
			return new PMPrintSession (ptr, false);
		}

		/// <summary>Returns the page format associated with this print info.</summary>
		public PMPageFormat GetPageFormat ()
		{
			var ptr = GetPMPageFormat ();
			return new PMPageFormat (ptr, false);
		}

		/// <summary>Returns the print settings associated with this print info.</summary>
		public PMPrintSettings GetPrintSettings ()
		{
			var ptr = GetPMPrintSettings ();
			return new PMPrintSettings (ptr, false);
		}
	}
}
#endif // !__MACCATALYST__
