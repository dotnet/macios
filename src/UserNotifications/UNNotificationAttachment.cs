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
using System;
using Foundation;

namespace UserNotifications {
	public partial class UNNotificationAttachment {

		/// <param name="identifier">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="attachmentOptions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static UNNotificationAttachment? FromIdentifier (string identifier, NSUrl url, UNNotificationAttachmentOptions attachmentOptions, out NSError? error)
		{
			return FromIdentifier (identifier, url, attachmentOptions?.Dictionary, out error);
		}
	}
}
#endif // !TVOS
