#if !__MACCATALYST__
using System;
using PrintCore;

#nullable enable

namespace AppKit {
	public partial class NSPrintInfo {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMPrintSession GetPrintSession ()
		{
			var ptr = GetPMPrintSession ();
			return new PMPrintSession (ptr, false);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMPageFormat GetPageFormat ()
		{
			var ptr = GetPMPageFormat ();
			return new PMPageFormat (ptr, false);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMPrintSettings GetPrintSettings ()
		{
			var ptr = GetPMPrintSettings ();
			return new PMPrintSettings (ptr, false);
		}
	}
}
#endif // !__MACCATALYST__
