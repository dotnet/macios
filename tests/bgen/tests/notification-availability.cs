// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using ObjCRuntime;

namespace NotificationAvailability {
	[BaseType (typeof (NSObject))]
	interface NotificationSource {
		[Notification]
		[Field ("ChangedNotification", "__Internal")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'Replacement' instead.")]
		NSString ChangedNotification { get; }

		[Notification (typeof (ChangedEventArgs))]
		[Field ("TypedChangedNotification", "__Internal")]
		[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Deprecated (PlatformName.iOS, 27, 0, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.MacOSX, 27, 0, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 27, 0, message: "Use 'Replacement' instead.")]
		[Deprecated (PlatformName.TvOS, 27, 0, message: "Use 'Replacement' instead.")]
		NSString TypedChangedNotification { get; }

		[Notification]
		[Field ("CurrentNotification", "__Internal")]
		NSString CurrentNotification { get; }
	}

	interface ChangedEventArgs {
	}
}
