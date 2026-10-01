//
// UNNotificationAttachment extensions & syntax sugar
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

#if !TVOS

namespace UserNotifications {
	public partial class UNNotificationAttachment {

		/// <summary>Creates a new attachment for a notification from the file at the specified <paramref name="url" />.</summary>
		/// <param name="identifier">The unique identifier for the attachment, or an empty string to have one generated automatically.</param>
		/// <param name="url">The URL of the file to attach.</param>
		/// <param name="attachmentOptions">Options that describe how the attachment should be handled, or <see langword="null" /> to use the default options.</param>
		/// <param name="error">On failure, contains the error that describes why the attachment could not be created.</param>
		/// <returns>A new <see cref="UserNotifications.UNNotificationAttachment" />, or <see langword="null" /> if the attachment could not be created.</returns>
		public static UNNotificationAttachment? FromIdentifier (string identifier, NSUrl url, UNNotificationAttachmentOptions? attachmentOptions, out NSError? error)
		{
			return FromIdentifier (identifier, url, attachmentOptions?.Dictionary, out error);
		}
	}
}
#endif // !TVOS
