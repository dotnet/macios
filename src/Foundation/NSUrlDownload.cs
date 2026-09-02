#if MONOMAC
#if !XAMCORE_5_0

#nullable enable

namespace Foundation {

	public partial class NSUrlDownload {

		/// <summary>Returns a string representation of the current <see cref="NSUrlDownload" /> instance.</summary>
		/// <returns>A string containing the fully qualified type name.</returns>
		public override string ToString ()
		{
			return GetType ().ToString ();
		}
	}
}
#endif // !XAMCORE_5_0
#endif
