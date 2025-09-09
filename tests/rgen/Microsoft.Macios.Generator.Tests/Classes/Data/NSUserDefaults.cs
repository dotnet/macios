// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;

namespace Foundation

public partial class MyNotificationArgs {

}

[BindingType<Class>]
public partial class NSUserDefaults : NSObject {

	// default values

	[Field<Property> ("NSUserDefaultsSizeLimitExceededNotification", Flags = Property.Notification)]
	public static partial NSString SizeLimitExceededNotification { get; }

	// special notification center
	[Field<Property> ("NSUbiquitousUserDefaultsNoCloudAccountNotification", Flags = Property.Notification, NotificationCenter = "SharedWorkspace.NotificationCenter")]
	public static partial NSString NoCloudAccountNotification { get; }

	// special args
	[Field<Property> ("NSUbiquitousUserDefaultsDidChangeAccountsNotification", Flags = Property.Notification, Type = typeof (MyNotificationArgs))]
	public static partial NSString DidChangeAccountsNotification { get; }

	// full customization
	[Field<Property> ("NSUbiquitousUserDefaultsCompletedInitialSyncNotification", Flags = Property.Notification, Type = typeof (MyNotificationArgs), NotificationCenter = "SharedWorkspace.NotificationCenter")]
	public static partial NSString CompletedInitialSyncNotification { get; }
}
