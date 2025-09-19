//
// Messages bindings
//
// Authors:
//	Vincent Dondain <vincent@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

#if !MONOMAC
namespace Messages {

	[MacCatalyst (14, 0)]
	[Native]
	public enum MSMessagesAppPresentationStyle : ulong {
		/// <summary>To be added.</summary>
		Compact,
		/// <summary>To be added.</summary>
		Expanded,
		/// <summary>To be added.</summary>
		Transcript,
	}

	/// <summary>Enumerates the sizes of stickers.</summary>
	[MacCatalyst (14, 0)]
	[Native]
	public enum MSStickerSize : long {
		/// <summary>100 x 100 points.</summary>
		Small,
		/// <summary>136 x 136 points.</summary>
		Regular,
		/// <summary>206 x 206 points.</summary>
		Large,
	}

	/// <summary>Enumerates errors associated with Messages.</summary>
	[MacCatalyst (14, 0)]
	[Native]
	[ErrorDomain ("MSMessagesErrorDomain")]
	public enum MSMessageErrorCode : long {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>A resource file was not found.</summary>
		FileNotFound = 1,
		/// <summary>A resource file could not be interpreted.</summary>
		FileUnreadable,
		/// <summary>A resource file was for a different type of resource.</summary>
		ImproperFileType,
		/// <summary>An error in the format of the File URL for a resource file.</summary>
		ImproperFileUrl,
		/// <summary>To be added.</summary>
		StickerFileImproperFileAttributes,
		/// <summary>A sticker file was not properly sized.</summary>
		StickerFileImproperFileSize,
		/// <summary>A sticker file had an improper format.</summary>
		StickerFileImproperFileFormat,
		/// <summary>To be added.</summary>
		UrlExceedsMaxSize,
		/// <summary>To be added.</summary>
		SendWithoutRecentInteraction,
		/// <summary>To be added.</summary>
		SendWhileNotVisible,
		/// <summary>Indicates that an attempt was made to use an API that cannot be used in the current presentation context.</summary>
		APIUnavailableInPresentationContext,
	}

	/// <summary>Enumerates iMessage presentation contexts.</summary>
	[MacCatalyst (14, 0)]
	[Native]
	public enum MSMessagesAppPresentationContext : long {
		/// <summary>Indicates that iMessage appears in Messages only.</summary>
		Messages,
		/// <summary>Indicates that iMessage appears in Messages and FaceTime.</summary>
		Media,
	}

	[MacCatalyst (14, 0)]
	[Protocol]
	interface MSMessagesAppTranscriptPresentation {
		/// <param name="size">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("contentSizeThatFits:")]
		CGSize GetContentSizeThatFits (CGSize size);

		[iOS (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("messageTintColor")]
		UIColor MessageTintColor { get; }

		[iOS (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("messageCornerRadius")]
		nfloat MessageCornerRadius { get; }

		[iOS (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("invalidateMessageTintColor")]
		void InvalidateMessageTintColor ();
	}

	/// <summary>A <see cref="UIViewController" /> that, when extended by the developer, is used to present a custom user-experience within the Messages app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSMessagesAppViewController">Apple documentation for <c>MSMessagesAppViewController</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIViewController))]
	interface MSMessagesAppViewController : MSMessagesAppTranscriptPresentation {
		// inlined ctor
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
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[NullAllowed, Export ("activeConversation", ArgumentSemantic.Strong)]
		MSConversation ActiveConversation { get; }

		[Export ("presentationStyle", ArgumentSemantic.Assign)]
		MSMessagesAppPresentationStyle PresentationStyle { get; }

		[Export ("requestPresentationStyle:")]
		void Request (MSMessagesAppPresentationStyle presentationStyle);

		[Export ("dismiss")]
		void Dismiss ();

		[Export ("willBecomeActiveWithConversation:")]
		void WillBecomeActive (MSConversation conversation);

		[Export ("didBecomeActiveWithConversation:")]
		void DidBecomeActive (MSConversation conversation);

		[Export ("willResignActiveWithConversation:")]
		void WillResignActive (MSConversation conversation);

		[Export ("didResignActiveWithConversation:")]
		void DidResignActive (MSConversation conversation);

		[Export ("willSelectMessage:conversation:")]
		void WillSelectMessage (MSMessage message, MSConversation conversation);

		[Export ("didSelectMessage:conversation:")]
		void DidSelectMessage (MSMessage message, MSConversation conversation);

		[Export ("didReceiveMessage:conversation:")]
		void DidReceiveMessage (MSMessage message, MSConversation conversation);

		[Export ("didStartSendingMessage:conversation:")]
		void DidStartSendingMessage (MSMessage message, MSConversation conversation);

		[Export ("didCancelSendingMessage:conversation:")]
		void DidCancelSendingMessage (MSMessage message, MSConversation conversation);

		[Export ("willTransitionToPresentationStyle:")]
		void WillTransition (MSMessagesAppPresentationStyle presentationStyle);

		[Export ("didTransitionToPresentationStyle:")]
		void DidTransition (MSMessagesAppPresentationStyle presentationStyle);

		[MacCatalyst (14, 0)]
		[Export ("presentationContext")]
		MSMessagesAppPresentationContext PresentationContext { get; }
	}

	/// <summary>A sequences of messages between the user and one or more others.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSConversation">Apple documentation for <c>MSConversation</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MSConversation {
		[Export ("localParticipantIdentifier")]
		NSUuid LocalParticipantIdentifier { get; }

		[Export ("remoteParticipantIdentifiers")]
		NSUuid [] RemoteParticipantIdentifiers { get; }

		[NullAllowed, Export ("selectedMessage")]
		MSMessage SelectedMessage { get; }

		[Export ("insertMessage:completionHandler:")]
		[Async (XmlDocs = """
			<param name="message">To be added.</param>
			<summary>Asynchronously adds the <paramref name="message" /> to the conversation.</summary>
			<returns>A task that represents the asynchronous InsertMessage operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void InsertMessage (MSMessage message, [NullAllowed] Action<NSError> completionHandler);

		[Export ("insertSticker:completionHandler:")]
		[Async (XmlDocs = """
			<param name="sticker">To be added.</param>
			<summary>Asynchronously adds the <paramref name="sticker" /> to the conversation.</summary>
			<returns>A task that represents the asynchronous InsertSticker operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void InsertSticker (MSSticker sticker, [NullAllowed] Action<NSError> completionHandler);

		[Export ("insertText:completionHandler:")]
		[Async (XmlDocs = """
			<param name="text">To be added.</param>
			<summary>Asynchronously adds the <paramref name="text" /> to the conversation.</summary>
			<returns>A task that represents the asynchronous InsertText operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void InsertText (string text, [NullAllowed] Action<NSError> completionHandler);

		[Export ("insertAttachment:withAlternateFilename:completionHandler:")]
		[Async (XmlDocs = """
			<param name="url">Must be a file URL.</param>
			<param name="filename">To be added.</param>
			<summary>Asynchronously inserts the media in the file <paramref name="url" />, describing it in the message as <paramref name="filename" />.</summary>
			<returns>A task that represents the asynchronous InsertAttachment operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void InsertAttachment (NSUrl url, [NullAllowed] string filename, [NullAllowed] Action<NSError> completionHandler);

		[MacCatalyst (14, 0)]
		[Export ("sendMessage:completionHandler:")]
		[Async (XmlDocs = """
			<param name="message">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous SendMessage operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void SendMessage (MSMessage message, [NullAllowed] Action<NSError> completionHandler);

		[MacCatalyst (14, 0)]
		[Export ("sendSticker:completionHandler:")]
		[Async (XmlDocs = """
			<param name="sticker">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous SendSticker operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void SendSticker (MSSticker sticker, [NullAllowed] Action<NSError> completionHandler);

		[MacCatalyst (14, 0)]
		[Export ("sendText:completionHandler:")]
		[Async (XmlDocs = """
			<param name="text">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous SendText operation</returns>
			<remarks>To be added.</remarks>
			""")]
		void SendText (string text, [NullAllowed] Action<NSError> completionHandler);

		[MacCatalyst (14, 0)]
		[Export ("sendAttachment:withAlternateFilename:completionHandler:")]
		[Async (XmlDocs = """
			<param name="url">To be added.</param>
			<param name="filename">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous SendAttachment operation</returns>
			<remarks>
			          <para copied="true">The SendAttachmentAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void SendAttachment (NSUrl url, [NullAllowed] string filename, [NullAllowed] Action<NSError> completionHandler);
	}

	/// <summary>An interactive message object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSMessage">Apple documentation for <c>MSMessage</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface MSMessage : NSCopying, NSSecureCoding {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithSession:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MSSession session);

		[NullAllowed, Export ("session")]
		MSSession Session { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)]
		[Export ("pending")]
		bool Pending { [Bind ("isPending")] get; }

		[Export ("senderParticipantIdentifier")]
		NSUuid SenderParticipantIdentifier { get; }

		[NullAllowed, Export ("layout", ArgumentSemantic.Copy)]
		MSMessageLayout Layout { get; set; }

		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }

		[Export ("shouldExpire")]
		bool ShouldExpire { get; set; }

		[NullAllowed, Export ("accessibilityLabel")]
		string AccessibilityLabel { get; set; }

		[NullAllowed, Export ("summaryText")]
		string SummaryText { get; set; }

		[NullAllowed, Export ("error", ArgumentSemantic.Copy)]
		NSError Error { get; set; }
	}

	/// <summary>Abstract base class for defining the appearance of <see cref="Messages.MSMessage" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSMessageLayout">Apple documentation for <c>MSMessageLayout</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[Abstract] // as per docs
	[DisableDefaultCtor]
	interface MSMessageLayout : NSCopying { }

	/// <summary>Lays out the display of <see cref="Messages.MSMessage" /> objects.</summary>
	///     <remarks>
	///       <para>The layout of a message template is:</para>
	///       <para>
	///         <img href="~/xml/Messages/_images/Messages.MSMessageTemplateLayout.png" alt="Image showing the layout of the message template" />
	///       </para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSMessageTemplateLayout">Apple documentation for <c>MSMessageTemplateLayout</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (MSMessageLayout))]
	interface MSMessageTemplateLayout {
		[NullAllowed, Export ("caption")]
		string Caption { get; set; }

		[NullAllowed, Export ("subcaption")]
		string Subcaption { get; set; }

		[NullAllowed, Export ("trailingCaption")]
		string TrailingCaption { get; set; }

		[NullAllowed, Export ("trailingSubcaption")]
		string TrailingSubcaption { get; set; }

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; set; }

		[NullAllowed, Export ("mediaFileURL", ArgumentSemantic.Copy)]
		NSUrl MediaFileUrl { get; set; }

		[NullAllowed, Export ("imageTitle")]
		string ImageTitle { get; set; }

		[NullAllowed, Export ("imageSubtitle")]
		string ImageSubtitle { get; set; }
	}

	/// <summary>Represents a messaging session in which <see cref="Messages.MSMessage" /> objects can be created and updated.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSSession">Apple documentation for <c>MSSession</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MSSession : NSSecureCoding { }

	/// <summary>A custom Messages sticker.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSSticker">Apple documentation for <c>MSSticker</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MSSticker {
		[Export ("initWithContentsOfFileURL:localizedDescription:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl fileUrl, string localizedDescription, [NullAllowed] out NSError error);

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("initWithFileURL:identifier:localizedDescription:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url, NSUuid identifier, string localizedDescription);

		[Export ("imageFileURL", ArgumentSemantic.Strong)]
		NSUrl ImageFileUrl { get; }

		[Export ("localizedDescription")]
		string LocalizedDescription { get; }
	}

	/// <summary>A <see cref="UIKit.UIView" /> that displays a developer-defined Messages sticker.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSStickerView">Apple documentation for <c>MSStickerView</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIView))]
	interface MSStickerView {
		// inlined ctor
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("initWithFrame:sticker:")]
		NativeHandle Constructor (CGRect frame, [NullAllowed] MSSticker sticker);

		[NullAllowed, Export ("sticker", ArgumentSemantic.Strong)]
		MSSticker Sticker { get; set; }

		[Export ("animationDuration")]
		double AnimationDuration { get; }

		[Export ("startAnimating")]
		void StartAnimating ();

		[Export ("stopAnimating")]
		void StopAnimating ();

		[Export ("isAnimating")]
		bool IsAnimating { get; }
	}

	interface IMSStickerBrowserViewDataSource { }

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSStickerBrowserViewDataSource">Apple documentation for <c>MSStickerBrowserViewDataSource</c></related>
	[MacCatalyst (14, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface MSStickerBrowserViewDataSource {
		/// <param name="stickerBrowserView">To be added.</param>
		/// <summary>The number of <see cref="Messages.MSSticker" /> objects held by this data source.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("numberOfStickersInStickerBrowserView:")]
		nint GetNumberOfStickers (MSStickerBrowserView stickerBrowserView);

		/// <param name="stickerBrowserView">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("stickerBrowserView:stickerAtIndex:")]
		MSSticker GetSticker (MSStickerBrowserView stickerBrowserView, nint index);
	}

	/// <summary>A <see cref="UIKit.UIView" /> that can be extended to provide a custom sticker-selection experience.</summary>
	///     <remarks>
	///       <para>Developers who do not need to customize the sticker browsing experience should probably create a "sticker pack" that can be presented by the standard browser. Sticker packs do not require any coding.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSStickerBrowserView">Apple documentation for <c>MSStickerBrowserView</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIView))]
	interface MSStickerBrowserView {
		[Export ("initWithFrame:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGRect frame);

		[Export ("initWithFrame:stickerSize:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGRect frame, MSStickerSize stickerSize);

		[Export ("stickerSize", ArgumentSemantic.Assign)]
		MSStickerSize StickerSize { get; }

		[NullAllowed, Export ("dataSource", ArgumentSemantic.Weak)]
		IMSStickerBrowserViewDataSource DataSource { get; set; }

		[Export ("contentOffset", ArgumentSemantic.Assign)]
		CGPoint ContentOffset { get; set; }

		[Export ("contentInset", ArgumentSemantic.Assign)]
		UIEdgeInsets ContentInset { get; set; }

		[Export ("setContentOffset:animated:")]
		void SetContentOffset (CGPoint contentOffset, bool animated);

		[Export ("reloadData")]
		void ReloadData ();
	}

	/// <summary>A standard <see cref="UIKit.UIViewController" /> that presents a virtual roll of user-provided stickers.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Messages/MSStickerBrowserViewController">Apple documentation for <c>MSStickerBrowserViewController</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIViewController))]
	interface MSStickerBrowserViewController : MSStickerBrowserViewDataSource {
		[Export ("initWithStickerSize:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MSStickerSize stickerSize);

		[Export ("stickerBrowserView", ArgumentSemantic.Strong)]
		MSStickerBrowserView StickerBrowserView { get; }

		[Export ("stickerSize")]
		MSStickerSize StickerSize { get; }
	}

	[MacCatalyst (14, 0)]
	[BaseType (typeof (MSMessageLayout))]
	[DisableDefaultCtor]
	interface MSMessageLiveLayout {
		[Export ("initWithAlternateLayout:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MSMessageTemplateLayout alternateLayout);

		[Export ("alternateLayout")]
		MSMessageTemplateLayout AlternateLayout { get; }
	}
}
#endif // !MONOMAC
