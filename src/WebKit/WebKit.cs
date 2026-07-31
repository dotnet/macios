#if __MACOS__

#nullable enable

namespace WebKit {

	public partial class WebFrame {
		/// <param name="htmlString">The html string.</param>
		///         <param name="baseUrl">The base url.</param>
		///         <summary>To be added.</summary>
		public void LoadHtmlString (string htmlString, NSUrl baseUrl)
		{
			LoadHtmlString ((NSString) htmlString, baseUrl);
		}
	}
}

#endif // __MACOS__
