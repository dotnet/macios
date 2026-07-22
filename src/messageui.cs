//
// MessageUI.cs: This file describes the API that the generator will produce for MessageUI
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011, 2013 Xamarin, Inc.
//

using CoreFoundation;
using Messages;
using UIKit;

namespace MessageUI {

	[iOS (18, 0), MacCatalyst (18, 0)]
	[Native]
	enum MFMailComposeControllerDeferredAction : long {
		None,
		AdjustInsertionPoint,
		AddMissingRecipients,
	}

	/// <summary>Represents the contents of an email draft.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // init leaves the non-null properties uninitialized
	interface MFMailDraft {
		/// <summary>Gets the sender address, or <see langword="null" /> if the draft does not specify one.</summary>
		[NullAllowed, Export ("from")]
		string From { get; }

		/// <summary>Gets the subject.</summary>
		[Export ("subject")]
		string Subject { get; }

		/// <summary>Gets the attributed message body.</summary>
		[Export ("body")]
		NSAttributedString Body { get; }

		/// <summary>Gets the primary recipient addresses.</summary>
		[Export ("to")]
		string [] To { get; }

		/// <summary>Gets the carbon-copy recipient addresses.</summary>
		[Export ("cc")]
		string [] Cc { get; }

		/// <param name="subject">The subject.</param>
		/// <param name="body">The attributed message body.</param>
		/// <param name="from">The sender address.</param>
		/// <param name="to">The primary recipient addresses.</param>
		/// <param name="cc">The carbon-copy recipient addresses.</param>
		/// <summary>Creates an email draft with the specified contents and recipients.</summary>
		[Export ("initWithSubject:body:from:to:cc:")]
		NativeHandle Constructor (string subject, NSAttributedString body, string from, string [] to, string [] cc);
	}

	interface IMFComposeAssistantViewControllerDelegate { }

	/// <summary>Methods for receiving email drafts from a compose assistant.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface MFComposeAssistantViewControllerDelegate {
		/// <param name="controller">The compose assistant that created the draft.</param>
		/// <param name="draft">The email draft.</param>
		/// <summary>Notifies the delegate that the compose assistant created an email draft.</summary>
		[Abstract]
		[Export ("composeAssistantViewController:didComposeDraft:")]
		void DidComposeDraft (MFComposeAssistantViewController controller, MFMailDraft draft);
	}

	/// <summary>Provides an interface for composing an email draft.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (UIViewController))]
	[PrivateDefaultCtor] // init and initWithCoder: abort at runtime
	interface MFComposeAssistantViewController {
		/// <param name="delegate">The object that receives the composed draft.</param>
		/// <summary>Creates a compose assistant with the specified delegate.</summary>
		[Export ("initWithDelegate:")]
		NativeHandle Constructor (IMFComposeAssistantViewControllerDelegate @delegate);
	}

	/// <summary>Provides a user interface for composing and sending email messages.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MessageUI/Reference/MFMailComposeViewController_class/index.html">Apple documentation for <c>MFMailComposeViewController</c></related>
	[BaseType (typeof (UINavigationController))]
	interface MFMailComposeViewController : UIAppearance {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("canSendMail")]
		bool CanSendMail { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("mailComposeDelegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakMailComposeDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakMailComposeDelegate")]
		IMFMailComposeViewControllerDelegate MailComposeDelegate { get; set; }

		/// <param name="subject">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setSubject:")]
		void SetSubject (string subject);

		/// <param name="recipients">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setToRecipients:")]
		void SetToRecipients ([NullAllowed] string [] recipients);

		/// <param name="ccRecipients">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setCcRecipients:")]
		void SetCcRecipients ([NullAllowed] string [] ccRecipients);

		/// <param name="bccRecipients">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setBccRecipients:")]
		void SetBccRecipients ([NullAllowed] string [] bccRecipients);

		/// <param name="body">To be added.</param>
		///         <param name="isHtml">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setMessageBody:isHTML:")]
		void SetMessageBody (string body, bool isHtml);

		/// <param name="attachment">To be added.</param>
		///         <param name="mimeType">To be added.</param>
		///         <param name="fileName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addAttachmentData:mimeType:fileName:")]
		void AddAttachmentData (NSData attachment, string mimeType, string fileName);

		/// <param name="emailAddress">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setPreferredSendingEmailAddress:")]
		void SetPreferredSendingEmailAddress (string emailAddress);

		[MacCatalyst (18, 4), iOS (18, 4)]
		[Export ("insertCollaborationItemProvider:completionHandler:")]
		[Async]
		void InsertCollaborationItemProvider (NSItemProvider itemProvider, Action<bool> completionHandler);
	}

	interface IMFMailComposeViewControllerDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MFMailComposeViewControllerDelegate {
		/// <param name="controller">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("mailComposeController:didFinishWithResult:error:")]
		void Finished (MFMailComposeViewController controller, MFMailComposeResult result, [NullAllowed] NSError error);
	}

	/// <summary>Provides data for the  event.</summary>
	interface MFMessageAvailabilityChangedEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MFMessageComposeViewControllerTextMessageAvailabilityKey")]
		bool TextMessageAvailability { get; }
	}

	/// <summary>Provides a user interface for composing and sending messages.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MessageUI/Reference/MFMessageComposeViewController_class/index.html">Apple documentation for <c>MFMessageComposeViewController</c></related>
	[BaseType (typeof (UINavigationController))]
	interface MFMessageComposeViewController : UIAppearance {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("messageComposeDelegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakMessageComposeDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakMessageComposeDelegate")]
		IMFMessageComposeViewControllerDelegate MessageComposeDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("recipients", ArgumentSemantic.Copy)]
		string [] Recipients { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("body", ArgumentSemantic.Copy)]
		string Body { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canSendText")]
		bool CanSendText { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canSendAttachments")]
		bool CanSendAttachments { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canSendSubject")]
		bool CanSendSubject { get; }

		/// <param name="uti">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("isSupportedAttachmentUTI:")]
		bool IsSupportedAttachment (string uti);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("subject", ArgumentSemantic.Copy)]
		string Subject { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("attachments")]
		NSDictionary [] GetAttachments ();

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("message", ArgumentSemantic.Copy)]
		MSMessage Message { get; set; }

		/// <param name="attachmentURL">To be added.</param>
		///         <param name="alternateFilename">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addAttachmentURL:withAlternateFilename:")]
		bool AddAttachment (NSUrl attachmentURL, [NullAllowed] string alternateFilename);

		/// <param name="attachmentData">To be added.</param>
		///         <param name="uti">To be added.</param>
		///         <param name="filename">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addAttachmentData:typeIdentifier:filename:")]
		bool AddAttachment (NSData attachmentData, string uti, string filename);

		[iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("insertCollaborationItemProvider:")]
		bool InsertCollaboration (NSItemProvider itemProvider);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("disableUserAttachments")]
		void DisableUserAttachments ();

		[Field ("MFMessageComposeViewControllerTextMessageAvailabilityDidChangeNotification")]
		[Notification (typeof (MFMessageAvailabilityChangedEventArgs))]
		NSString TextMessageAvailabilityDidChangeNotification { get; }

		/// <summary>Represents the value associated with the constant MFMessageComposeViewControllerTextMessageAvailabilityKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MFMessageComposeViewControllerTextMessageAvailabilityKey")]
		NSString TextMessageAvailabilityKey { get; }

		/// <summary>Represents the value associated with the constant MFMessageComposeViewControllerAttachmentAlternateFilename</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MFMessageComposeViewControllerAttachmentAlternateFilename")]
		NSString AttachmentAlternateFilename { get; }

		/// <summary>Represents the value associated with the constant MFMessageComposeViewControllerAttachmentURL</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MFMessageComposeViewControllerAttachmentURL")]
		NSString AttachmentURL { get; }

		[Async]
		[iOS (17, 0), NoMacCatalyst, NoTV]
		[Export ("setUPIVerificationCodeSendCompletion:")]
		void SetUpiVerificationCodeSendCompletion (Action<bool> completion);
	}

	interface IMFMessageComposeViewControllerDelegate { }

	/// <summary>Used to receive notifications from the MFMessageComposeViewController class.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MessageUI/Reference/MFMessageComposeViewControllerDelegate_protocol/index.html">Apple documentation for <c>MFMessageComposeViewControllerDelegate</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MFMessageComposeViewControllerDelegate {
		/// <param name="controller">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("messageComposeViewController:didFinishWithResult:")]
		void Finished (MFMessageComposeViewController controller, MessageComposeResult result);
	}
}
