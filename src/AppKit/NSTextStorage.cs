#if !__MACCATALYST__
using CoreText;
using Foundation;

#nullable enable

namespace AppKit {
	public partial class NSTextStorage {
		/// <param name="str">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSTextStorage (string str, NSDictionary attributes) : base (str, attributes)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSTextStorage (NSAttributedString other) : base (other)
		{
		}

		/// <param name="str">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSTextStorage (string str, CTStringAttributes attributes) : base (str, attributes)
		{
		}
	}
}
#endif // !__MACCATALYST__
