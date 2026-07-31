#if __MACOS__

#nullable enable

namespace WebKit {

	public partial class WebFrame {
		/// <param name="htmlString">The HTML content to load.</param>
		///         <param name="baseUrl">The base URL used to resolve relative URLs in the HTML content.</param>
		///         <summary>To be added.</summary>
		public void LoadHtmlString (string htmlString, NSUrl baseUrl)
		{
			LoadHtmlString ((NSString) htmlString, baseUrl);
		}
	}
}

#endif // __MACOS__
