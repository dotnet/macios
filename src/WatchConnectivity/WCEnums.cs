//
// WatchConnectivity enums
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;

namespace WatchConnectivity {

	// NSInteger -> WCError.h
	/// <summary>Enumerates error codes relating to watch connectivity.</summary>
	[Native]
	public enum WCErrorCode : long {
		/// <summary>The exact error is unknown.</summary>
		GenericError = 7001,
		/// <summary>The device does not support <see cref="T:WatchConnectivity.WCSession" /> objects.</summary>
		SessionNotSupported = 7002,
		/// <summary>There is no active extension delegate for the <see cref="T:WatchConnectivity.WCSession" />.</summary>
		SessionMissingDelegate = 7003,
		/// <summary>The companion app has not activated a <see cref="T:WatchConnectivity.WCSession" />.</summary>
		SessionNotActivated = 7004,
		/// <summary>Indicates that no device was paired.</summary>
		DeviceNotPaired = 7005,
		/// <summary>The app is not installed on the user's paired Apple Watch.</summary>
		WatchAppNotInstalled = 7006,
		/// <summary>The companion app is not available.</summary>
		NotReachable = 7007,
		/// <summary>A bad argument was passed.</summary>
		InvalidParameter = 7008,
		/// <summary>The data was too large to transfer.</summary>
		PayloadTooLarge = 7009,
		/// <summary>Transferred dictionaries may only contain property-list types.</summary>
		PayloadUnsupportedTypes = 7010,
		/// <summary>The reply could not be returned.</summary>
		MessageReplyFailed = 7011,
		/// <summary>The companion app did not return a reply within the allowed time.</summary>
		MessageReplyTimedOut = 7012,
		/// <summary>File transfer failed due to permissions.</summary>
		FileAccessDenied = 7013,
		/// <summary>Indicates that the payload was not delivered.</summary>
		DeliveryFailed = 7014,
		/// <summary>Indicates that the receiver did not have enough storage to receive the payload.</summary>
		InsufficientSpace = 7015,
		// iOS 9.3 / watchOS 2.2
		/// <summary>Indicates taht the session was not active.</summary>
		SessionInactive = 7016,
		/// <summary>Indicates that the transfer timed out.</summary>
		TransferTimedOut = 7017,
		// iOS 13, watchOS 6
		CompanionAppNotInstalled = 7018,
		WatchOnlyApp = 7019,
	}

	/// <summary>Enumerates session states.</summary>
	[Native]
	public enum WCSessionActivationState : long {
		/// <summary>Indicates that the session is inactive and the watch and the phone are not connected.</summary>
		NotActivated = 0,
		/// <summary>Indicates that the session is deactivating. Data can be received by the app, but not sent to the phone.</summary>
		Inactive = 1,
		/// <summary>Indicates that the seesion is activated and the watch app and iOS can communicate.</summary>
		Activated = 2,
	}
}
