//
// ios5-twitter.cs: Twitter bindings
//
// Authors:
//   Miguel de Icaza
//

using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using Twitter;
using Accounts;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Twitter {

	/// <summary>A delegate that is used as the <c>handler</c> in calls to <see cref="Twitter.TWRequest.PerformRequest(Twitter.TWRequestHandler)" />.</summary>
	delegate void TWRequestHandler (NSData responseData, NSHttpUrlResponse urlResponse, NSError error);

	/// <summary>A Twitter request.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Twitter/Reference/TWRequestClassRef/index.html">Apple documentation for <c>TWRequest</c></related>
	[Deprecated (PlatformName.iOS, 6, 0, message: "Use the 'Social' framework.")]
	[BaseType (typeof (NSObject))]
	interface TWRequest {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("account")]
		ACAccount Account { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requestMethod")]
		TWRequestMethod RequestMethod { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URL")]
		NSUrl Url { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("parameters")]
		NSDictionary Parameters { get; }

		/// <param name="url">To be added.</param>
		/// <param name="parameters">
		///           <para>HTTP parameters for this request.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="requestMethod">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithURL:parameters:requestMethod:")]
		NativeHandle Constructor (NSUrl url, [NullAllowed] NSDictionary parameters, TWRequestMethod requestMethod);

		/// <param name="data">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addMultiPartData:withName:type:")]
		void AddMultiPartData (NSData data, string name, string type);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("signedURLRequest")]
		NSUrlRequest SignedUrlRequest { get; }

		/// <param name="handler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("performRequestWithHandler:")]
		[Async (ResultTypeName = "TWRequestResult", XmlDocs = """
			<summary>To be added.</summary>
			<returns>
			          <para>A task that represents the asynchronous PerformRequest operation.   The value of the TResult parameter is of type <c>Action&lt;Twitter.TWRequestResult&gt;</c>.</para>
			        </returns>
			<remarks>
			          <para copied="true">The PerformRequestAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void PerformRequest (TWRequestHandler handler);
	}

	/// <summary>A <see cref="UIKit.UIViewController" /> that manages the user experience of composing a tweet.</summary>
	///     
	///     <related type="recipe" href="https://developer.xamarin.com/ios/Recipes/Shared_Resources/Twitter/Send_a_Tweet">Send a Tweet</related>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Twitter/Reference/TWTweetSheetViewControllerClassRef/index.html">Apple documentation for <c>TWTweetComposeViewController</c></related>
	[Deprecated (PlatformName.iOS, 6, 0, message: "Use the 'Social' framework.")]
	[BaseType (typeof (UIViewController))]
	interface TWTweetComposeViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("completionHandler")]
		Action<TWTweetComposeViewControllerResult> CompletionHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canSendTweet")]
		bool CanSendTweet { get; }

		/// <param name="text">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setInitialText:")]
		bool SetInitialText (string text);

		/// <param name="image">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addImage:")]
		bool AddImage (UIImage image);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllImages")]
		bool RemoveAllImages ();

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addURL:")]
		bool AddUrl (NSUrl url);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllURLs")]
		bool RemoveAllUrls ();
	}

}
