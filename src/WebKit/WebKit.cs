#if __MACOS__

#nullable enable

using Foundation;

namespace WebKit {

	public partial class WebFrame {
		/// <param name="htmlString">To be added.</param>
		///         <param name="baseUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void LoadHtmlString (string htmlString, NSUrl baseUrl)
		{
			LoadHtmlString ((NSString) htmlString, baseUrl);
		}
	}
}

#endif // __MACOS__
