//
// EAEnums.cs: API definition for ExternalAccessory binding
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

namespace ExternalAccessory {

	/// <summary>Enumerates unconfigured but supported features.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum EAWiFiUnconfiguredAccessoryProperties : ulong {
		/// <summary>Indicates that the accessory supports AirPlay.</summary>
		SupportsAirPlay = (1 << 0),
		/// <summary>Indicates that the accessory supports AirPrint.</summary>
		SupportsAirPrint = (1 << 1),
		/// <summary>Indicates that the accessory supports HomeKit.</summary>
		SupportsHomeKit = (1 << 2), // iOS 8 beta 5
	}

	/// <summary>Enumerates the possible states of unconfigured accessory browsers.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum EAWiFiUnconfiguredAccessoryBrowserState : long {
		/// <summary>Indicates that Wi-Fi is unavailable on the device.</summary>
		WiFiUnavailable = 0,
		/// <summary>Indicates that the browser has stopped searching for accessories.</summary>
		Stopped,
		/// <summary>Indicates that the browser is searching for accessories.</summary>
		Searching,
		/// <summary>Indicates that an accessory is being configured.</summary>
		Configuring,
	}

	// NSInteger -> EAWiFiUnconfiguredAccessoryBrowser.h
	/// <summary>Enumerates the possible states encountered while configuring accessory browsers.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum EAWiFiUnconfiguredAccessoryConfigurationStatus : long {
		/// <summary>Indicates that accessory configuration succeeded.</summary>
		Success,
		/// <summary>Indicates that the user canceled accessory configuration.</summary>
		UserCancelledConfiguration,
		/// <summary>Indicates that accessory configuration failed.</summary>
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
