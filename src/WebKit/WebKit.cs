#if __MACOS__

#nullable enable

namespace WebKit {

	public partial class WebFrame {
		/// <summary>Loads the given HTML string into the web frame.</summary>
		/// <param name="htmlString">The HTML content to load.</param>
		/// <param name="baseUrl">The base URL used to resolve relative URLs in the HTML content.</param>
		public void LoadHtmlString (string htmlString, NSUrl baseUrl)
		{
			LoadHtmlString ((NSString) htmlString, baseUrl);
		}
	}
}

#endif // __MACOS__
