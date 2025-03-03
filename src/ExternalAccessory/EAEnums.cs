//
// EAEnums.cs: API definition for ExternalAccessory binding
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//
using System;
using Foundation;
using ObjCRuntime;

namespace ExternalAccessory {

	/// <summary>Enumerates unconfigured but supported features.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum EAWiFiUnconfiguredAccessoryProperties : ulong {
		/// <summary>To be added.</summary>
		SupportsAirPlay = (1 << 0),
		/// <summary>To be added.</summary>
		SupportsAirPrint = (1 << 1),
		/// <summary>To be added.</summary>
		SupportsHomeKit = (1 << 2), // iOS 8 beta 5
	}

	/// <summary>Enumerates the possible states of unconfigured accessory browsers.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum EAWiFiUnconfiguredAccessoryBrowserState : long {
		/// <summary>To be added.</summary>
		WiFiUnavailable = 0,
		/// <summary>To be added.</summary>
		Stopped,
		/// <summary>To be added.</summary>
		Searching,
		/// <summary>To be added.</summary>
		Configuring,
	}

	// NSInteger -> EAWiFiUnconfiguredAccessoryBrowser.h
	/// <summary>Enumerates the possible states encountered while configuring accessory browsers.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum EAWiFiUnconfiguredAccessoryConfigurationStatus : long {
		/// <summary>To be added.</summary>
		Success,
		/// <summary>To be added.</summary>
		UserCancelledConfiguration,
		/// <summary>To be added.</summary>
		Failed,
	}

	// NSInteger -> EAAccessoryManager.h
	[MacCatalyst (13, 1)]
	[Native ("EABluetoothAccessoryPickerErrorCode")]
	[ErrorDomain ("EABluetoothAccessoryPickerErrorDomain")]
	public enum EABluetoothAccessoryPickerError : long {
		/// <summary>To be added.</summary>
		AlreadyConnected,
		/// <summary>To be added.</summary>
		NotFound,
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		Failed,
	}
}
