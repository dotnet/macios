//
// UserNotifications bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreLocation;
using CoreMedia;

using Intents;

#if MONOMAC
using UIScene = Foundation.NSObject;
#else
using UIKit;
#endif

namespace UserNotifications {

	[MacCatalyst (13, 1)]
	[ErrorDomain ("UNErrorDomain")]
	[Native]
	public enum UNErrorCode : long {
		/// <summary>The app is not allowed to submit notification requests.</summary>
		NotificationsNotAllowed = 1,
		/// <summary>The URL for the file is not valid.</summary>
		AttachmentInvalidUrl = 100,
		/// <summary>The type of the attached file was not recognized.</summary>
		AttachmentUnrecognizedType,
		/// <summary>The attached file is too big.</summary>
		AttachmentInvalidFileSize,
		/// <summary>The local attached file was not present.</summary>
		AttachmentNotInDataStore,
		/// <summary>The attachment could not be moved onto the system data store.</summary>
		AttachmentMoveIntoDataStoreFailed,
		/// <summary>The attached file is corrupt.</summary>
		AttachmentCorrupt,
		/// <summary>To be added.</summary>
		NotificationInvalidNoDate = 1400,
		/// <summary>To be added.</summary>
		NotificationInvalidNoContent,
		ContentProvidingObjectNotAllowed = 1500,
		ContentProvidingInvalid = 1501,
		BadgeInputInvalid = 1600,
	}

	/// <summary>Enumerates behavior options for notification actions.</summary>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum UNNotificationActionOptions : ulong {
		/// <summary>No options are specified.</summary>
		None = 0,
		/// <summary>Prompt the user to unlock the device to respond to the action.</summary>
		AuthenticationRequired = (1 << 0),
		/// <summary>Indicates that the notification action will perform an irrevocable change. (Special highlighting is applied to the action.)</summary>
		Destructive = (1 << 1),
		/// <summary>The app will be launched in the foreground when responding to the action, prompting the user to unlock the device if it is locked.</summary>
		Foreground = (1 << 2),
	}

	/// <summary>Contains flags that control whether a custom dismiss action is used, and whether the category is allowed in CarPlay application.</summary>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum UNNotificationCategoryOptions : ulong {
		/// <summary>No options are specified.</summary>
		None = 0,
		/// <summary>Send the dismiss action to the user notification center for handling.</summary>
		CustomDismissAction = (1 << 0),
		/// <summary>Whether to allow the notification in CarPlay. (If this value is not specified, the notification is disallowed in CarPlay.)</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		AllowInCarPlay = (2 << 0),
		/// <summary>Display titles for notification previews, even when hidden.</summary>
		HiddenPreviewsShowTitle = (1 << 2),
		/// <summary>Display subtitles for notification previews, even when hidden.</summary>
		HiddenPreviewsShowSubtitle = (1 << 3),
		[iOS (13, 0)]
		[NoMac]
		[MacCatalyst (13, 1)]
		AllowAnnouncement = (1 << 4),
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum UNAuthorizationStatus : long {
		/// <summary>The user has not yet allowed or denied the app permission to post notifications.</summary>
		NotDetermined = 0,
		/// <summary>The user has denied the app permission to post notifications.</summary>
		Denied,
		/// <summary>The user has allowed the app to post notifications.</summary>
		Authorized,
		/// <summary>The user has allowed provisional noncritical notifications.</summary>
		[MacCatalyst (13, 1)]
		Provisional,
		[iOS (14, 0)]
		[NoMac, NoTV]
		[MacCatalyst (14, 0)]
		Ephemeral,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum UNNotificationSetting : long {
		/// <summary>The notification type is not supported by the target application.</summary>
		NotSupported = 0,
		/// <summary>The notification is disabled.</summary>
		Disabled,
		/// <summary>The notification is enabled.</summary>
		Enabled,
	}

	/// <summary>Enumerates the kinds of alerts that notifications display.</summary>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum UNAlertStyle : long {
		/// <summary>The app may not display alerts.</summary>
		None = 0,
		/// <summary>The notification may display a temporary slide-down banner alert.</summary>
		Banner,
		/// <summary>The notification may display a modal alert.</summary>
		Alert,
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum UNAuthorizationOptions : ulong {
		/// <summary>Indicates that no request type is specified.</summary>
		None = 0,
		/// <summary>Indicates authorization to update the app's badge.</summary>
		Badge = (1 << 0),
		/// <summary>Indicates authorization to play sound.</summary>
		Sound = (1 << 1),
		/// <summary>Indicates authorization to display alerts.</summary>
		Alert = (1 << 2),
		/// <summary>Indicates authorization to display notifications in CarPlay.</summary>
		CarPlay = (1 << 3),
		/// <summary>Indicates authorization to display critical alerts.</summary>
		[MacCatalyst (13, 1)]
		CriticalAlert = (1 << 4),
		/// <summary>Indicates that the app should provide a button for notification settings.</summary>
		[MacCatalyst (13, 1)]
		ProvidesAppNotificationSettings = (1 << 5),
		/// <summary>Indicates authorization to make provisional noncritical notifications.</summary>
		[MacCatalyst (13, 1)]
		Provisional = (1 << 6),
		[iOS (13, 0)]
		[TV (13, 0)]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Announcement is always included.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Announcement is always included.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Announcement is always included.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Announcement is always included.")]
		Announcement = (1 << 7),
		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		TimeSensitive = (1 << 8),
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum UNNotificationPresentationOptions : ulong {
		/// <summary>No options are set.</summary>
		None = 0,
		/// <summary>Display the notification badge value in the application's badge.</summary>
		Badge = (1 << 0),
		/// <summary>Play the notification sound.</summary>
		Sound = (1 << 1),
		/// <summary>Display the notification as an alert, using the notification text.</summary>
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'List | Banner' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'List | Banner' instead.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'List | Banner' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'List | Banner' instead.")]
		Alert = (1 << 2),
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		List = (1 << 3),
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		Banner = (1 << 4),
	}

	/// <summary>Enumerates times when notification previews are shown.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum UNShowPreviewsSetting : long {
		/// <summary>Previews are always shown.</summary>
		Always,
		/// <summary>Previews are shown when the device is unlocked.</summary>
		WhenAuthenticated,
		/// <summary>Previews are never shown.</summary>
		Never,
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
	[Native]
	public enum UNNotificationInterruptionLevel : long {
#if XAMCORE_5_0
		Passive,
		Active,
		TimeSensitive,
		Critical,
#else
		[Obsolete ("Use 'Active2'.")]
		Active,
		[Obsolete ("Use 'Critical2'.")]
		Critical,
		[Obsolete ("Use 'Passive2'.")]
		Passive,
		[Obsolete ("Use 'TimeSensitive2'.")]
		TimeSensitive,
#endif // XAMCORE_5_0

		// Additional enum values to fix reordering - to be at the end of the enum
#if !XAMCORE_5_0
#pragma warning disable 0618 // warning CS0618: 'UNNotificationInterruptionLevel.[field]' is obsolete: 'Use '[replacement]'.'
		Active2 = Critical,
		Critical2 = TimeSensitive,
		Passive2 = Active,
		TimeSensitive2 = Passive,
#pragma warning restore
#endif // !XAMCORE_5_0
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs (not user created)
	interface UNNotification : NSCopying, NSSecureCoding {

		/// <summary>Gets the date on which the notification was delivered.</summary>
		///         <value>The date on which the notification was delivered.</value>
		///         <remarks>To be added.</remarks>
		[Export ("date", ArgumentSemantic.Copy)]
		NSDate Date { get; }

		/// <summary>Gets the original notification request for the notification.</summary>
		///         <value>The original notification request for the notification.</value>
		///         <remarks>To be added.</remarks>
		[Export ("request", ArgumentSemantic.Copy)]
		UNNotificationRequest Request { get; }
	}

	/// <summary>An action that can be performed in response to a notification.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationAction">Apple documentation for <c>UNNotificationAction</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs (use FromIdentifier)
	interface UNNotificationAction : NSCopying, NSSecureCoding {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>Gets the localized action title.</summary>
		///         <value>The localized action title.</value>
		///         <remarks>To be added.</remarks>
		[Export ("title")]
		string Title { get; }

		/// <summary>Gets the unique identifier that the application will use to find the action.</summary>
		///         <value>The unique identifier that the application will use to find the action.</value>
		///         <remarks>To be added.</remarks>
		[Export ("options")]
		UNNotificationActionOptions Options { get; }

		/// <param name="identifier">The unique identifier that the application will use to find the action.</param>
		///         <param name="title">A localized action title.</param>
		///         <param name="options">A mask that indicates whether authentication is required, whether the action is destructive, and/or whether to run the application in the foreground.</param>
		///         <summary>Creates and returns a new notification action with the specified <paramref name="identifier" />, <paramref name="title" />, and <paramref name="options" />.</summary>
		///         <returns>A new notification action with the specified <paramref name="identifier" />, <paramref name="title" />, and <paramref name="options" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("actionWithIdentifier:title:options:")]
		UNNotificationAction FromIdentifier (string identifier, string title, UNNotificationActionOptions options);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Static]
		[Export ("actionWithIdentifier:title:options:icon:")]
		UNNotificationAction FromIdentifier (string identifier, string title, UNNotificationActionOptions options, [NullAllowed] UNNotificationActionIcon icon);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("icon", ArgumentSemantic.Copy)]
		UNNotificationActionIcon Icon { get; }
	}

	/// <summary>An action that accepts text input from the user before the response is given to the app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNTextInputNotificationAction">Apple documentation for <c>UNTextInputNotificationAction</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UNNotificationAction))]
	[DisableDefaultCtor] // as per docs (use FromIdentifier)
	interface UNTextInputNotificationAction {

		/// <param name="identifier">The unique identifier for the action within the scope of the app.</param>
		///         <param name="title">The title of the action.</param>
		///         <param name="options">The notification action options.</param>
		///         <param name="textInputButtonTitle">The title of the text input button.</param>
		///         <param name="textInputPlaceholder">The placeholder text.</param>
		///         <summary>Creates and returns a new text input notification action with the specified values.</summary>
		///         <returns>A new text input notification action with the specified values</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("actionWithIdentifier:title:options:textInputButtonTitle:textInputPlaceholder:")]
		UNTextInputNotificationAction FromIdentifier (string identifier, string title, UNNotificationActionOptions options, string textInputButtonTitle, string textInputPlaceholder);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Static]
		[Export ("actionWithIdentifier:title:options:icon:textInputButtonTitle:textInputPlaceholder:")]
		UNTextInputNotificationAction FromIdentifier (string identifier, string title, UNNotificationActionOptions options, [NullAllowed] UNNotificationActionIcon icon, string textInputButtonTitle, string textInputPlaceholder);

		/// <summary>Gets the title of the text input button.</summary>
		///         <value>The title of the text input button.</value>
		///         <remarks>To be added.</remarks>
		[Export ("textInputButtonTitle")]
		string TextInputButtonTitle { get; }

		/// <summary>Gets the placeholder text.</summary>
		///         <value>The placeholder text.</value>
		///         <remarks>To be added.</remarks>
		[Export ("textInputPlaceholder")]
		string TextInputPlaceholder { get; }
	}

	/// <summary>Audio, video, or images that are displayed with notifications.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationAttachment">Apple documentation for <c>UNNotificationAttachment</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs (use FromIdentifier)
	interface UNNotificationAttachment : NSCopying, NSSecureCoding {

		/// <summary>Gets the identifier for the attachment.</summary>
		///         <value>The identifier for the attachment.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>Gets the URL for the attachment.</summary>
		///         <value>The URL for the attachment.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		/// <summary>Gets the type of the attachment.</summary>
		///         <value>The type of the attachment.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		string Type { get; }

		/// <param name="identifier">The unique attachment identifier.</param>
		///         <param name="url">The location of the attachment.</param>
		///         <param name="options">
		///           <para>A dictionary of attachment options, such as clipping rectangles, animation frame numbers, and so on.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>A location to which errors will be written.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates and returns a new notification attachment with the supplied <paramref name="identifier" />, <paramref name="url" />, <paramref name="options" />, and <paramref name="options" />.</summary>
		///         <returns>A new notification attachmen.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("attachmentWithIdentifier:URL:options:error:")]
		[return: NullAllowed]
		UNNotificationAttachment FromIdentifier (string identifier, NSUrl url, [NullAllowed] NSDictionary options, [NullAllowed] out NSError error);
	}

	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	interface UNNotificationAttachmentOptionsKeys {

		[Field ("UNNotificationAttachmentOptionsTypeHintKey")]
		NSString TypeHint { get; }

		[Field ("UNNotificationAttachmentOptionsThumbnailHiddenKey")]
		NSString ThumbnailHidden { get; }

		[Field ("UNNotificationAttachmentOptionsThumbnailClippingRectKey")]
		NSString ThumbnailClippingRect { get; }

		[Field ("UNNotificationAttachmentOptionsThumbnailTimeKey")]
		NSString ThumbnailTime { get; }
	}

	/// <summary>A dictionary of values that control aspects of notification behavior.</summary>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[StrongDictionary ("UNNotificationAttachmentOptionsKeys")]
	interface UNNotificationAttachmentOptions {

		/// <summary>Gets or sets a type hint for the attachment.</summary>
		///         <value>The type hint for the attachment.</value>
		///         <remarks>To be added.</remarks>
		[Export ("TypeHint")]
		string TypeHint { get; set; }

		/// <summary>Gets or sets a value that controls whether the thumbnail image is hidden.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ThumbnailHidden")]
		bool ThumbnailHidden { get; set; }

		/// <summary>Gets or sets the clipping rectangle for displaying a thumbail image from a larger source image.</summary>
		///         <value>The clipping rectangle for displaying a thumbail image from a larger source image.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ThumbnailClippingRect")]
		CGRect ThumbnailClippingRect { get; set; }
		// According to apple docs UNNotificationAttachmentOptionsThumbnailTimeKey
		// can be either a CMTime or a NSNumber (in seconds). Exposing both options
		// in the strong dictionary because watchOS does not have CMTime or
		// CoreMedia framework at all.
		/// <summary>Gets or sets the number of a starting frame for an animated attachment image.</summary>
		///         <value>The number of a starting frame for an animated image.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ThumbnailTime")]
		CMTime ThumbnailTime { get; set; }

		/// <summary>Gets or sets the time offset at which to begin playing an animated attachment image.</summary>
		///         <value>The time offset at which to begin playing an animated attachment image.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ThumbnailTime")]
		double ThumbnailTimeInSeconds { get; set; }
	}

	/// <summary>Implements a group of actions and options that comprise a category of notifications.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationCategory">Apple documentation for <c>UNNotificationCategory</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs (use FromIdentifier)
	interface UNNotificationCategory : NSCopying, NSSecureCoding {

		/// <summary>Gets the unique category identifier.</summary>
		///         <value>The unique category identifier.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>Gets the complete list of actions for this notification category.</summary>
		///         <value>The complete list of actions for this notification category.</value>
		///         <remarks>To be added.</remarks>
		[Export ("actions", ArgumentSemantic.Copy)]
		UNNotificationAction [] Actions { get; }

		/// <summary>Gets the list of identifiers for the supported intents for the category.</summary>
		///         <value>The list of identifiers for the supported intents for the category.</value>
		///         <remarks>To be added.</remarks>
		[Export ("intentIdentifiers", ArgumentSemantic.Copy)]
		string [] IntentIdentifiers { get; }

		/// <summary>Gets the notification handling options.</summary>
		///         <value>The notification handling options..</value>
		///         <remarks>To be added.</remarks>
		[Export ("options")]
		UNNotificationCategoryOptions Options { get; }

		/// <summary>Gets the placeholder text that is used when notification previews are disabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("hiddenPreviewsBodyPlaceholder")]
		string HiddenPreviewsBodyPlaceholder { get; }

		/// <param name="identifier">The app-unique identifier for the category.</param>
		///         <param name="actions">Four or fewer actions to display.</param>
		///         <param name="intentIdentifiers">The intent identifiers for the category.</param>
		///         <param name="options">Category options.</param>
		///         <summary>Creates and returns a new notification category from the specified arguments.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("categoryWithIdentifier:actions:intentIdentifiers:options:")]
		UNNotificationCategory FromIdentifier (string identifier, UNNotificationAction [] actions, string [] intentIdentifiers, UNNotificationCategoryOptions options);

		/// <param name="identifier">The app-unique identifier for the category.</param>
		///         <param name="actions">Four or fewer actions to display.</param>
		///         <param name="intentIdentifiers">The intent identifiers for the category.</param>
		///         <param name="hiddenPreviewsBodyPlaceholder">A string to display when notification previews are disabled.</param>
		///         <param name="options">Category options.</param>
		///         <summary>Creates and returns a new notification category from the specified arguments.</summary>
		///         <returns>A new notification category from the specified arguments.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("categoryWithIdentifier:actions:intentIdentifiers:hiddenPreviewsBodyPlaceholder:options:")]
		UNNotificationCategory FromIdentifier (string identifier, UNNotificationAction [] actions, string [] intentIdentifiers, string hiddenPreviewsBodyPlaceholder, UNNotificationCategoryOptions options);

		/// <param name="identifier">To be added.</param>
		///         <param name="actions">To be added.</param>
		///         <param name="intentIdentifiers">To be added.</param>
		///         <param name="hiddenPreviewsBodyPlaceholder">To be added.</param>
		///         <param name="categorySummaryFormat">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("categoryWithIdentifier:actions:intentIdentifiers:hiddenPreviewsBodyPlaceholder:categorySummaryFormat:options:")]
		UNNotificationCategory FromIdentifier (string identifier, UNNotificationAction [] actions, string [] intentIdentifiers, [NullAllowed] string hiddenPreviewsBodyPlaceholder, [NullAllowed] NSString categorySummaryFormat, UNNotificationCategoryOptions options);

		/// <summary>Gets the category summary format string.</summary>
		///         <value>The category summary format string.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("categorySummaryFormat")]
		string CategorySummaryFormat { get; }

	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs
	interface UNNotificationContent : NSCopying, NSMutableCopying, NSSecureCoding {

		/// <summary>Gets an array of <see cref="T:UserNotifications.UNNotificationAttachment" /> objects that contains the attachments for the notification.</summary>
		///         <value>An array of <see cref="T:UserNotifications.UNNotificationAttachment" /> objects that contains the attachments for the notification.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("attachments", ArgumentSemantic.Copy)]
		UNNotificationAttachment [] Attachments { get; }

		/// <summary>Gets the number to display in the app's icon badge.</summary>
		///         <value>
		///           <para>The number to display in the app's icon badge.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("badge", ArgumentSemantic.Copy)]
		NSNumber Badge { get; }

		/// <summary>Gets the message that is displayed in the notification alert.</summary>
		///         <value>The message that is displayed in the notification alert.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("body")]
		string Body { get; }

		/// <summary>Gets an application-defined category object identifier.</summary>
		///         <value>An application-defined category object identifier.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("categoryIdentifier")]
		string CategoryIdentifier { get; }

		/// <summary>Gets the name of an image that is stored in the application's bundle to display when the user launches the application from the notification.</summary>
		///         <value>The name of an image that is stored in the application's bundle to display when the user launches the application from the notification.</value>
		///         <remarks>To be added.</remarks>
		[NoTV, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("launchImageName")]
		string LaunchImageName { get; }

		/// <summary>Gets the sound that is played when the notification is triggered.</summary>
		///         <value>
		///           <para>The sound that is played when the notification is triggered.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[NullAllowed, Export ("sound", ArgumentSemantic.Copy)]
		UNNotificationSound Sound { get; }

		/// <summary>Gets the notification subtitle.</summary>
		///         <value>The notification subtitle.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("subtitle")]
		string Subtitle { get; }

		/// <summary>Gets an application-specific identifier that is used to group notifications.</summary>
		///         <value>An application-specific identifier that is used to group notifications.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("threadIdentifier")]
		string ThreadIdentifier { get; }

		/// <summary>Gets the notification descritpion.</summary>
		///         <value>The notification descritpion.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("title")]
		string Title { get; }

		/// <summary>Gets the dictionary of notification custom data.</summary>
		///         <value>The dictionary of notification custom data.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary UserInfo { get; }

		/// <summary>Gets the notification-specific addition to the category summary string.</summary>
		///         <value>The notification-specific addition to the category summary string.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 15, 0, message: "This property is ignored.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "This property is ignored.")]
		[Export ("summaryArgument")]
		string SummaryArgument { get; }

		/// <summary>Gets the number of arguments that the notification adds to the category summary string.</summary>
		///         <value>The number of arguments added to the category notification string.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 15, 0, message: "This property is ignored.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "This property is ignored.")]
		[Export ("summaryArgumentCount")]
		nuint SummaryArgumentCount { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("targetContentIdentifier")]
		string TargetContentIdentifier { get; [NotImplemented] set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("contentByUpdatingWithProvider:error:")]
		[return: NullAllowed]
		UNNotificationContent Update (IUNNotificationContentProviding fromProvider, [NullAllowed] out NSError outError);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("interruptionLevel", ArgumentSemantic.Assign)]
		UNNotificationInterruptionLevel InterruptionLevel { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("relevanceScore")]
		double RelevanceScore { get; }

		[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("filterCriteria")]
		[NullAllowed]
		string FilterCriteria { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (UNNotificationContent))]
	interface UNMutableNotificationContent {

		/// <summary>Gets or sets an array of <see cref="T:UserNotifications.UNNotificationAttachment" /> objects that contains the attachments for the notification.</summary>
		///         <value>An array of <see cref="T:UserNotifications.UNNotificationAttachment" /> objects that contains the attachments for the notification.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("attachments", ArgumentSemantic.Copy)]
		UNNotificationAttachment [] Attachments { get; set; }

		/// <summary>Gets or sets the number to display in the app's icon badge.</summary>
		///         <value>
		///           <para>The number to display in the app's icon badge.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("badge", ArgumentSemantic.Copy)]
		NSNumber Badge { get; set; }

		/// <summary>Gets or sets the message that is displayed in the notification alert.</summary>
		///         <value>The message that is displayed in the notification alert.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("body")]
		string Body { get; set; }

		/// <summary>Gets or sets an application-defined category object identifier.</summary>
		///         <value>An application-defined category object identifier.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("categoryIdentifier")]
		string CategoryIdentifier { get; set; }

		/// <summary>Gets or sets the name of an image that is stored in the application's bundle to display when the user launches the application from the notification.</summary>
		///         <value>The name of an image that is stored in the application's bundle to display when the user launches the application from the notification.</value>
		///         <remarks>To be added.</remarks>
		[NoTV, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("launchImageName")]
		string LaunchImageName { get; set; }

		/// <summary>Gets or sets the sound that is played when the notification is triggered.</summary>
		///         <value>
		///           <para>The sound that is played when the notification is triggered.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[NullAllowed, Export ("sound", ArgumentSemantic.Copy)]
		UNNotificationSound Sound { get; set; }

		/// <summary>Gets or sets the notification subtitle.</summary>
		///         <value>The notification subtitle.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("subtitle")]
		string Subtitle { get; set; }

		/// <summary>Gets or sets an application-specific identifier that is used to group notifications.</summary>
		///         <value>An application-specific identifier that is used to group notifications.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("threadIdentifier")]
		string ThreadIdentifier { get; set; }

		/// <summary>Gets or sets the notification descritpion.</summary>
		///         <value>The notification descritpion.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("title")]
		string Title { get; set; }

		/// <summary>Gets or sets the dictionary of notification custom data.</summary>
		///         <value>The dictionary of notification custom data.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary UserInfo { get; set; }

		/// <summary>Gets or sets the notification-specific addition to the category summary string.</summary>
		///         <value>The notification-specific addition to the category summary string.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 15, 0, message: "This property is ignored.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "This property is ignored.")]
		[Export ("summaryArgument")]
		string SummaryArgument { get; set; }

		/// <summary>Gets or sets the number of arguments that the notification adds to the category summary string.</summary>
		///         <value>The number of arguments added to the category notification string.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 15, 0, message: "This property is ignored.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "This property is ignored.")]
		[Export ("summaryArgumentCount")]
		nuint SummaryArgumentCount { get; set; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("targetContentIdentifier")]
		string TargetContentIdentifier { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("interruptionLevel", ArgumentSemantic.Assign)]
		UNNotificationInterruptionLevel InterruptionLevel { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("relevanceScore")]
		double RelevanceScore { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("filterCriteria")]
		string FilterCriteria { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface UNNotificationRequest : NSCopying, NSSecureCoding {

		/// <summary>Gets the identifier that the application uses to identify the notification.</summary>
		///         <value>The identifier that the application uses to identify the notification.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>Gets the content of the notification.</summary>
		///         <value>The content of the notification.</value>
		///         <remarks>To be added.</remarks>
		[Export ("content", ArgumentSemantic.Copy)]
		UNNotificationContent Content { get; }

		/// <summary>Gets the trigger that activates the notification when the trigger's conditions are met.</summary>
		///         <value>
		///           <para>The trigger that activates the notification when the trigger's conditions are met.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("trigger", ArgumentSemantic.Copy)]
		UNNotificationTrigger Trigger { get; }

		/// <param name="identifier">An identifer, unique to the application scope.</param>
		///         <param name="content">The content of the notification.</param>
		///         <param name="trigger">
		///           <para>The trigger that activates the notification when the trigger's conditions are met.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a new notification request with the specified <paramref name="identifier" />, <paramref name="content" />, and <paramref name="trigger" />.</summary>
		///         <returns>A new notification request.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("requestWithIdentifier:content:trigger:")]
		UNNotificationRequest FromIdentifier (string identifier, UNNotificationContent content, [NullAllowed] UNNotificationTrigger trigger);
	}

	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	interface UNNotificationActionIdentifier {

		[Field ("UNNotificationDefaultActionIdentifier")]
		NSString Default { get; }

		[Field ("UNNotificationDismissActionIdentifier")]
		NSString Dismiss { get; }
	}

	/// <summary>A system-created object that contains information about how a user responded to a notification.</summary>
	///     <remarks>This class is not related to <see cref="T:UserNotifications.UNNotificationRequest" />, which represents the data needed to request a notification from the system.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationResponse">Apple documentation for <c>UNNotificationResponse</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs
	interface UNNotificationResponse : NSCopying, NSSecureCoding {

		/// <summary>Gets the notification to which the user responded.</summary>
		///         <value>The notification to which the user responded.</value>
		///         <remarks>To be added.</remarks>
		[Export ("notification", ArgumentSemantic.Copy)]
		UNNotification Notification { get; }

		/// <summary>Gets the action identifer that tells what the user did in response to the notification.</summary>
		///         <value>The action identifer that tells what the user did in response to the notification.</value>
		///         <remarks>To be added.</remarks>
		[Export ("actionIdentifier")]
		NSString ActionIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("ActionIdentifier == UNNotificationActionIdentifier.Default")]
		bool IsDefaultAction { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("ActionIdentifier == UNNotificationActionIdentifier.Dismiss")]
		bool IsDismissAction { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("!IsDefaultAction && !IsDismissAction")]
		bool IsCustomAction { get; }

		[iOS (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("targetScene")]
		UIScene TargetScene { get; }
	}

	/// <summary>System-created object that contains a user response to a <see cref="T:UserNotifications.UNTextInputNotificationAction" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNTextInputNotificationResponse">Apple documentation for <c>UNTextInputNotificationResponse</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UNNotificationResponse))]
	[DisableDefaultCtor] // as per docs
	interface UNTextInputNotificationResponse {

		/// <summary>Gets the text that the user entered.</summary>
		///         <value>The text that the user entered.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userText")]
		string UserText { get; }
	}

	/// <summary>Class for processing push notification payloads before delivery.</summary>
	///     <remarks>Developers can use this class to, for example, replace a payload, download a large payload in parts, or etc.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationServiceExtension">Apple documentation for <c>UNNotificationServiceExtension</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs
	interface UNNotificationServiceExtension {

		// Not async because app developers are supposed to implement/override this method, not call it themselves.
		/// <param name="request">The request that was received.</param>
		///         <param name="contentHandler">An action to perform on the modified payload.</param>
		///         <summary>Method that is called to modify a notification.</summary>
		///         <remarks>Developers overload this method to modify a notification.</remarks>
		[Export ("didReceiveNotificationRequest:withContentHandler:")]
		void DidReceiveNotificationRequest (UNNotificationRequest request, Action<UNNotificationContent> contentHandler);

		/// <summary>Method that is called when the extension is about to expire.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("serviceExtensionTimeWillExpire")]
		void TimeWillExpire ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs
	interface UNNotificationSettings : NSCopying, NSSecureCoding {

		/// <summary>Gets a value that tells whether the app may post alerts to the user with notifications.</summary>
		///         <value>A value that tells whether the app may post alerts to the user with notifications.</value>
		///         <remarks>To be added.</remarks>
		[Export ("authorizationStatus")]
		UNAuthorizationStatus AuthorizationStatus { get; }

		/// <summary>Gets a value that tells whether the app may bplay notification sounds.</summary>
		///         <value>A value that tells whether the app may bplay notification sounds.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("soundSetting")]
		UNNotificationSetting SoundSetting { get; }

		/// <summary>Gets a value that tells whether the notification may update the app's badge.</summary>
		///         <value>A value that tells whether the notification may update the app's badge..</value>
		///         <remarks>To be added.</remarks>
		[Export ("badgeSetting")]
		UNNotificationSetting BadgeSetting { get; }

		/// <summary>Gets a value that tells whether the app may display alerts.</summary>
		///         <value>A value that tells whether the app may display alerts.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("alertSetting")]
		UNNotificationSetting AlertSetting { get; }

		/// <summary>Gets a value that tells whether the app's notifications may be displayed in the Notification Center.</summary>
		///         <value>A value that tells whether the app's notifications may be displayed in the Notification Center.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("notificationCenterSetting")]
		UNNotificationSetting NotificationCenterSetting { get; }

		/// <summary>Gets a value that tells whether the app may display notifications on the lock screen.</summary>
		///         <value>A value that tells whether the app may display notifications on the lock screen.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("lockScreenSetting")]
		UNNotificationSetting LockScreenSetting { get; }

		/// <summary>Gets a value that tells whether the app may display notifications in CarPlay.</summary>
		///         <value>A value that tells whether the app may display notifications in CarPlay.</value>
		///         <remarks>To be added.</remarks>
		[NoTV, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("carPlaySetting")]
		UNNotificationSetting CarPlaySetting { get; }

		/// <summary>Gets a value that indicates the alert styles that the app may display.</summary>
		///         <value>A value that indicates the alert styles that the app may display.</value>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("alertStyle")]
		UNAlertStyle AlertStyle { get; }

		/// <summary>Gets a value that tells when previews are shown.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("showPreviewsSetting")]
		UNShowPreviewsSetting ShowPreviewsSetting { get; }

		/// <summary>Gets a value that determines whether are supported, enabled, or disabled.</summary>
		///         <value>A value that determines whether are supported, enabled, or disabled.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("criticalAlertSetting")]
		UNNotificationSetting CriticalAlertSetting { get; }

		/// <summary>Gets a Boolean value that tells whether the app displays a notification settings button.</summary>
		///         <value>A Boolean value that tells whether the app displays a notification settings button.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("providesAppNotificationSettings")]
		bool ProvidesAppNotificationSettings { get; }

		[NoTV, NoMac, iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("announcementSetting")]
		UNNotificationSetting AnnouncementSetting { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("timeSensitiveSetting")]
		UNNotificationSetting TimeSensitiveSetting { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("scheduledDeliverySetting")]
		UNNotificationSetting ScheduledDeliverySetting { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("directMessagesSetting")]
		UNNotificationSetting DirectMessagesSetting { get; }
	}

	/// <summary>Contains a Linear PCM, MA4, µLaw, or aLaw sound to play when a notification is delivered.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNNotificationSound">Apple documentation for <c>UNNotificationSound</c></related>
	[Unavailable (PlatformName.TvOS)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // as per docs (use provided methods)
	interface UNNotificationSound : NSCopying, NSSecureCoding {

		/// <summary>Gets the default notification sound.</summary>
		///         <value>The default notification sound.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultSound")]
		UNNotificationSound Default { get; }

		[NoTV, NoMacCatalyst, NoMac, iOS (15, 2)]
		[Static]
		[Export ("defaultRingtoneSound", ArgumentSemantic.Copy)]
		UNNotificationSound DefaultRingtoneSound { get; }

		/// <param name="name">The name of the sound to get.</param>
		///         <summary>Gets the sound that is specified by <paramref name="name" />.</summary>
		///         <returns>The sound that is specified by <paramref name="name" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("soundNamed:")]
		UNNotificationSound GetSound (string name);

		/// <summary>Gets the default critical notification sound.</summary>
		///         <value>The default critical notification sound.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("defaultCriticalSound", ArgumentSemantic.Copy)]
		UNNotificationSound DefaultCriticalSound { get; }

		/// <param name="volume">The volume at which to play the alert sound.</param>
		///         <summary>Creates and returns a default critical notification sound.</summary>
		///         <returns>The default critical notification sound.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("defaultCriticalSoundWithAudioVolume:")]
		UNNotificationSound GetDefaultCriticalSound (float volume);

		[NoTV, NoMacCatalyst, NoMac, iOS (15, 2)]
		[Static]
		[Export ("ringtoneSoundNamed:")]
		UNNotificationSound GetRingtoneSound (string name);

		/// <param name="name">The name of the file that contains the sound to play.</param>
		///         <summary>Creates and returns a default critical notification sound.</summary>
		///         <returns>The default critical notification sound.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("criticalSoundNamed:")]
		UNNotificationSound GetCriticalSound (string name);

		/// <param name="name">The name of the file that contains the sound to play.</param>
		///         <param name="volume">The volume at which to play the alert sound.</param>
		///         <summary>Creates and returns a critical notification sound.</summary>
		///         <returns>The critical notification sound.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("criticalSoundNamed:withAudioVolume:")]
		UNNotificationSound GetCriticalSound (string name, float volume);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Abstract] // as per docs
	[DisableDefaultCtor]
	interface UNNotificationTrigger : NSCopying, NSSecureCoding {

		/// <summary>Gets a value that tells whether the trigger will be activated more than once.</summary>
		///         <value>A value that tells whether the trigger will be activated more than once.</value>
		///         <remarks>To be added.</remarks>
		[Export ("repeats")]
		bool Repeats { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (UNNotificationTrigger))]
	[DisableDefaultCtor] // as per docs (system created)
	interface UNPushNotificationTrigger {

	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (UNNotificationTrigger))]
	[DisableDefaultCtor] // as per doc, use supplied method (CreateTrigger)
	interface UNTimeIntervalNotificationTrigger {

		/// <summary>Gets the time interval after which the trigger is activated.</summary>
		///         <value>Te time interval after which the trigger is activated.</value>
		///         <remarks>Application developers can set <see cref="P:CoreLocation.CLRegion.NotifyOnEntry" /> and <see cref="P:CoreLocation.CLRegion.NotifyOnExit" /> to control when the trigger is activated.</remarks>
		[Export ("timeInterval")]
		double TimeInterval { get; }

		/// <param name="timeInterval">To be added.</param>
		///         <param name="repeats">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("triggerWithTimeInterval:repeats:")]
		UNTimeIntervalNotificationTrigger CreateTrigger (double timeInterval, bool repeats);

		/// <summary>Gets the date and time that the trigger will activate next.</summary>
		///         <value>
		///           <para>The date and time that the trigger will activate next.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Application developers can set <see cref="P:CoreLocation.CLRegion.NotifyOnEntry" /> and <see cref="P:CoreLocation.CLRegion.NotifyOnExit" /> to control when the trigger is activated.</remarks>
		[NullAllowed, Export ("nextTriggerDate")]
		NSDate NextTriggerDate { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // as per doc, use supplied method (CreateTrigger)
	[BaseType (typeof (UNNotificationTrigger))]
	interface UNCalendarNotificationTrigger {

		/// <summary>Gets the date components that describe when the notification will be triggered.</summary>
		///         <value>The date components that describe when the notification will be triggered.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dateComponents", ArgumentSemantic.Copy)]
		NSDateComponents DateComponents { get; }

		/// <param name="dateComponents">To be added.</param>
		///         <param name="repeats">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("triggerWithDateMatchingComponents:repeats:")]
		UNCalendarNotificationTrigger CreateTrigger (NSDateComponents dateComponents, bool repeats);

		/// <summary>Gets the date at which the notification will next be triggered.</summary>
		///         <value>
		///           <para>The date at which the notification will next be triggered.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("nextTriggerDate")]
		NSDate NextTriggerDate { get; }
	}

	/// <summary>Triggers the delivery of a notification when the device enters or leaves a geographic area.</summary>
	///     <remarks>Application developers can set <see cref="P:CoreLocation.CLRegion.NotifyOnEntry" /> and <see cref="P:CoreLocation.CLRegion.NotifyOnExit" /> to control when the trigger is activated.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UserNotifications/UNLocationNotificationTrigger">Apple documentation for <c>UNLocationNotificationTrigger</c></related>
	[NoMac]
	[NoMacCatalyst]
	[Unavailable (PlatformName.TvOS)]
	[BaseType (typeof (UNNotificationTrigger))]
	[DisableDefaultCtor] // as per doc, use supplied method (CreateTrigger)
	interface UNLocationNotificationTrigger {

		/// <summary>Gets the region whose boundary activates the trigger.</summary>
		///         <value>Application developers can set <see cref="P:CoreLocation.CLRegion.NotifyOnEntry" /> and <see cref="P:CoreLocation.CLRegion.NotifyOnExit" /> to control when the trigger is activated.</value>
		///         <remarks>To be added.</remarks>
		[Export ("region", ArgumentSemantic.Copy)]
		CLRegion Region { get; }

		/// <param name="region">To be added.</param>
		///         <param name="repeats">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("triggerWithRegion:repeats:")]
		UNLocationNotificationTrigger CreateTrigger (CLRegion region, bool repeats);
	}

	interface IUNUserNotificationCenterDelegate { }

	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface UNUserNotificationCenterDelegate {

		/// <param name="center">The notification center that received the response.</param>
		///         <param name="notification">To be added.</param>
		///         <param name="completionHandler">An action that takes no arguments and returns no value.</param>
		///         <summary>Called to deliver a notification to an application that is running in the foreground.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
		void WillPresentNotification (UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler);

		/// <param name="center">The notification center that received the response.</param>
		///         <param name="response">The user's response.</param>
		///         <param name="completionHandler">An action that takes no arguments and returns no value.</param>
		///         <summary>Called after the user selects an action from a notification from the app.</summary>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
		void DidReceiveNotificationResponse (UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler);

		/// <param name="center">The notification center that received the response.</param>
		///         <param name="notification">The notification.</param>
		///         <summary>Called to open the in-app notification settings.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("userNotificationCenter:openSettingsForNotification:")]
		void OpenSettings (UNUserNotificationCenter center, [NullAllowed] UNNotification notification);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface UNUserNotificationCenter {

		/// <summary>An instance of the UserNotifications.IUNUserNotificationCenterDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the UserNotifications.IUNUserNotificationCenterDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IUNUserNotificationCenterDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("supportsContentExtensions")]
		bool SupportsContentExtensions { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("currentNotificationCenter")]
		UNUserNotificationCenter Current { get; }

		/// <param name="options">The options for the authorization request.</param>
		///         <param name="completionHandler">A task that takes a <see langword="bool" /> success value and an error to process.</param>
		///         <summary>Requests notification authorization with the specified options, and processes the result of the request.</summary>
		///         <remarks>To be added.</remarks>
		[Async (XmlDocs = """
			<param name="options">The options for the authorization request.</param>
			<summary>Requests notification authorization with the specified options, and processes the result of the request.</summary>
			<returns>A task that takes an authorization options object and returns a tuple that contains a boolean that indicates the result of the request and an error.</returns>
			<remarks>The error in the returned tuple may be <see langword="null" />.</remarks>
			""")]
		[Export ("requestAuthorizationWithOptions:completionHandler:")]
		void RequestAuthorization (UNAuthorizationOptions options, Action<bool, NSError> completionHandler);

		/// <param name="categories">The set of categories to support.</param>
		///         <summary>Sets the specified supported notification categories</summary>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("setNotificationCategories:")]
		void SetNotificationCategories (NSSet<UNNotificationCategory> categories);

		/// <param name="completionHandler">An action that takes an array of the currently registered notification categories and returns <see langword="void" />.</param>
		///         <summary>Returns the currently registered notification categories for the app, processing them before they are returned.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Unavailable (PlatformName.TvOS)]
		[Export ("getNotificationCategoriesWithCompletionHandler:")]
		void GetNotificationCategories (Action<NSSet<UNNotificationCategory>> completionHandler);

		/// <param name="completionHandler">An action that takes a notification settings object and returns <see langword="void" />.</param>
		///         <summary>Returns the notification settings object for the app, processing it before it is returned.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("getNotificationSettingsWithCompletionHandler:")]
		void GetNotificationSettings (Action<UNNotificationSettings> completionHandler);

		/// <param name="request">The data and settings for the notification.</param>
		///         <param name="completionHandler">An action that returns <see langword="void" /> and takes an out parameter for storing any errors that occur while trying to add the request.<para tool="nullallowed">This parameter can be <see langword="null" />.</para></param>
		///         <summary>Adds the local notification that is specified by <paramref name="request" />, with the specified <paramref name="completionHandler" />.</summary>
		///         <remarks>To be added.</remarks>
		[Async (XmlDocs = """
			<param name="request">The data and settings for the notification.</param>
			<summary>Asynchronously adds the local notification that is specified by <paramref name="request" />.</summary>
			<returns>A task that represents the asynchronous AddNotificationRequest operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("addNotificationRequest:withCompletionHandler:")]
		void AddNotificationRequest (UNNotificationRequest request, [NullAllowed] Action<NSError> completionHandler);

		/// <param name="completionHandler">An action that takes an array of the pending notification requests and returns <see langword="void" />.</param>
		///         <summary>Returns an array that contains the pending notification requests, processing them before returning them.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("getPendingNotificationRequestsWithCompletionHandler:")]
		void GetPendingNotificationRequests (Action<UNNotificationRequest []> completionHandler);

		/// <param name="identifiers">The identifiers for which to remove the corresponding notification requests.</param>
		///         <summary>Removes all pending notification requests for the app that have any of the the specified <paramref name="identifiers" /> from the notification center.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removePendingNotificationRequestsWithIdentifiers:")]
		void RemovePendingNotificationRequests (string [] identifiers);

		/// <summary>Removes all pending notification requests for the app from the notification center.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllPendingNotificationRequests")]
		void RemoveAllPendingNotificationRequests ();

		/// <param name="completionHandler">An action that takes an array of the delivered notifications and returns <see langword="void" />.</param>
		///         <summary>Returns the delivered notifications that are still in the notification center, processing them before they are returned.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Unavailable (PlatformName.TvOS)]
		[Export ("getDeliveredNotificationsWithCompletionHandler:")]
		void GetDeliveredNotifications (Action<UNNotification []> completionHandler);

		/// <param name="identifiers">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("removeDeliveredNotificationsWithIdentifiers:")]
		void RemoveDeliveredNotifications (string [] identifiers);

		/// <summary>Removes all delivered notifications for the app from the notification center.</summary>
		///         <remarks>To be added.</remarks>
		[Unavailable (PlatformName.TvOS)]
		[Export ("removeAllDeliveredNotifications")]
		void RemoveAllDeliveredNotifications ();

		[Async]
		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("setBadgeCount:withCompletionHandler:")]
		void SetBadgeCount (nint newBadgeCount, [NullAllowed] Action<NSError> completionHandler);
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface UNNotificationActionIcon : NSCopying, NSSecureCoding {
		[Static]
		[Export ("iconWithTemplateImageName:")]
		UNNotificationActionIcon CreateFromTemplate (string imageName);

		[Static]
		[Export ("iconWithSystemImageName:")]
		UNNotificationActionIcon CreateFromSystem (string imageName);
	}

	interface IUNNotificationContentProviding { }

	[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
	[Protocol]
	interface UNNotificationContentProviding {
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface UNNotificationAttributedMessageContext : UNNotificationContentProviding {
		[Static]
		[Export ("contextWithSendMessageIntent:attributedContent:")]
		UNNotificationAttributedMessageContext Create (INSendMessageIntent sendMessageIntent, NSAttributedString attributedContent);
	}
}

