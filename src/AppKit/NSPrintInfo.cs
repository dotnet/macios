#if !__MACCATALYST__
using PrintCore;

#nullable enable

namespace AppKit {
	public partial class NSPrintInfo {
		/// <summary>GetPrintSession.</summary>
		public PMPrintSession GetPrintSession ()
		{
			var ptr = GetPMPrintSession ();
			return new PMPrintSession (ptr, false);
		}

		/// <summary>GetPageFormat.</summary>
		public PMPageFormat GetPageFormat ()
		{
			var ptr = GetPMPageFormat ();
			return new PMPageFormat (ptr, false);
		}

		/// <summary>GetPrintSettings.</summary>
		public PMPrintSettings GetPrintSettings ()
		{
			var ptr = GetPMPrintSettings ();
			return new PMPrintSettings (ptr, false);
		}
	}
}
#endif // !__MACCATALYST__
