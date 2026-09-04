#nullable enable

using Foundation;
using ObjCRuntime;

namespace LinkSecurity {

	/// <summary>Provides the result of an asynchronous flagged URL check.</summary>
	/// <param name="isFlagged"><see langword="true" /> if the URL is flagged for additional security considerations; otherwise, <see langword="false" />.</param>
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	delegate void LSLinkSecurityManagerCheckIsFlaggedUrlCompletionHandler (bool isFlagged);

	/// <summary>Stores URLs that require additional security considerations and checks whether URLs are flagged.</summary>
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	interface LSLinkSecurityManager {
		/// <summary>Gets the shared Link Security manager.</summary>
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		LSLinkSecurityManager SharedManager { get; }

		/// <summary>Gets a value indicating whether any URLs are currently flagged for additional security considerations.</summary>
		[Export ("hasFlaggedURLs")]
		bool HasFlaggedUrls { get; }

		/// <summary>Flags a URL for additional security considerations.</summary>
		/// <param name="url">The URL to flag.</param>
		[Export ("addFlaggedURL:")]
		void AddFlaggedUrl (NSUrl url);

		/// <summary>Flags a collection of URLs for additional security considerations.</summary>
		/// <param name="urls">The URLs to flag.</param>
		[Export ("addFlaggedURLs:")]
		void AddFlaggedUrls (NSUrl [] urls);

		/// <summary>Checks whether a URL is flagged for additional security considerations.</summary>
		/// <param name="url">The URL to check.</param>
		/// <param name="completionHandler">The handler to invoke with the result.</param>
		[Async (XmlDocs = """
			<summary>Checks whether a URL is flagged for additional security considerations.</summary>
			<param name="url">The URL to check.</param>
			<returns>A task whose result is <see langword="true" /> if the URL is flagged; otherwise, <see langword="false" />.</returns>
			""")]
		[Export ("checkIsFlaggedURL:completion:")]
		void CheckIsFlaggedUrl (NSUrl url, LSLinkSecurityManagerCheckIsFlaggedUrlCompletionHandler completionHandler);
	}
}
