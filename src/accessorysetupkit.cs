using System;

using CoreBluetooth;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using UIKit;

using ASAccessoryWiFiAwarePairedDeviceId = System.UInt64;

namespace AccessorySetupKit {
	[Native]
	[iOS (18, 0)]
	public enum ASAccessoryState : long {
		Unauthorized = 0,
		AwaitingAuthorization = 10,
		Authorized = 20,
	}

	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASAccessoryRenameOptions : ulong {
		Ssid = 1U << 0,
	}

	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASAccessorySupportOptions : ulong {
		BluetoothPairingLE = 1U << 1,
		BluetoothTransportBridging = 1U << 2,
		[iOS (18, 4)]
		BluetoothHid = 1U << 3,
	}

	[Native]
	[iOS (18, 0)]
	public enum ASDiscoveryDescriptorRange : long {
		Default = 0,
		Immediate = 10,
	}

	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASPickerDisplayItemSetupOptions : long {
		Rename = 1 << 0,
		ConfirmAuthorization = 1 << 1,
		FinishInApp = 1 << 2,
	}

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	[DisableDefaultCtor]
	interface ASAccessory {
		[Export ("state", ArgumentSemantic.Assign)]
		ASAccessoryState State { get; }

		[Export ("bluetoothIdentifier", ArgumentSemantic.Copy), NullAllowed]
		NSUuid BluetoothIdentifier { get; }

		[Export ("displayName", ArgumentSemantic.Copy)]
		string DisplayName { get; }

		[Export ("SSID", ArgumentSemantic.Copy), NullAllowed]
		string Ssid { get; }

		[Export ("descriptor", ArgumentSemantic.Copy)]
		ASDiscoveryDescriptor Descriptor { get; }

		[Export ("bluetoothTransportBridgingIdentifier", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothTransportBridgingIdentifier { get; }

		[iOS (26, 0)]
		[Export ("wifiAwarePairedDeviceID")]
		ASAccessoryWiFiAwarePairedDeviceId WifiAwarePairedDeviceId { get; }
	}

	[Native]
	[iOS (18, 0)]
	public enum ASAccessoryEventType : long {
		Unknown = 0,
		Activated = 10,
		Invalidated = 11,
		MigrationComplete = 20,
		AccessoryAdded = 30,
		AccessoryRemoved = 31,
		AccessoryChanged = 32,
		Discovered = 33,
		PickerDidPresent = 40,
		PickerDidDismiss = 50,
		PickerSetupBridging = 60,
		PickerSetupFailed = 70,
		PickerSetupPairing = 80,
		PickerSetupRename = 90,
	}

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	[DisableDefaultCtor]
	interface ASAccessoryEvent {
		[Export ("eventType", ArgumentSemantic.Assign)]
		ASAccessoryEventType EventType { get; }

		[Export ("accessory", ArgumentSemantic.Copy), NullAllowed]
		ASAccessory Accessory { get; }

		[Export ("error", ArgumentSemantic.Copy), NullAllowed]
		NSError Error { get; }
	}

	delegate void ASAccessorySessionCompletionHandler ([NullAllowed] NSError error);

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	interface ASAccessorySession {
		[Export ("accessories", ArgumentSemantic.Copy)]
		ASAccessory [] Accessories { get; }

		[Export ("activateWithQueue:eventHandler:")]
		void Activate (DispatchQueue queue, Action<ASAccessoryEvent> eventHandler);

		[Export ("invalidate")]
		void Invalidate ();

		[Async]
		[Export ("showPickerWithCompletionHandler:")]
		void ShowPicker (ASAccessorySessionCompletionHandler completionHandler);

		[Async]
		[Export ("showPickerForDisplayItems:completionHandler:")]
		void ShowPicker (ASPickerDisplayItem [] displayItems, ASAccessorySessionCompletionHandler completionHandler);

		[Async]
		[Export ("finishAuthorization:settings:completionHandler:")]
		void FinishAuthorization (ASAccessory accessory, ASAccessorySettings settings, ASAccessorySessionCompletionHandler completionHandler);

		[Async]
		[Export ("removeAccessory:completionHandler:")]
		void RemoveAccessory (ASAccessory accessory, ASAccessorySessionCompletionHandler completionHandler);

		[Async]
		[Export ("renameAccessory:options:completionHandler:")]
		void RenameAccessory (ASAccessory accessory, ASAccessoryRenameOptions renameOptions, ASAccessorySessionCompletionHandler completionHandler);

		[Async]
		[Export ("failAuthorization:completionHandler:")]
		void FailAuthorization (ASAccessory accessory, ASAccessorySessionCompletionHandler completionHandler);

		[iOS (26, 0)]
		[Export ("pickerDisplaySettings", ArgumentSemantic.Copy)]
		[NullAllowed]
		ASPickerDisplaySettings PickerDisplaySettings { get; set; }

		[Async]
		[iOS (26, 0)]
		[Export ("updateAuthorization:descriptor:completionHandler:")]
		void UpdateAuthorization (ASAccessory accessory, ASDiscoveryDescriptor descriptor, ASAccessorySessionUpdateAuthorizationHandler completionHandler);

		[Async]
		[iOS (26, 1)]
		[Export ("updatePickerShowingDiscoveredDisplayItems:completionHandler:")]
		void UpdatePicker (ASDiscoveredDisplayItem [] showingDisplayItems, ASAccessorySessionUpdatePickerHandler completionHandler);

		[Async]
		[iOS (26, 1)]
		[Export ("finishPickerDiscovery:")]
		void FinishPickerDiscovery (ASAccessorySessionFinishPickerDiscoveryHandler completionHandler);
	}

	delegate void ASAccessorySessionUpdateAuthorizationHandler ([NullAllowed] NSError error);
	delegate void ASAccessorySessionUpdatePickerHandler ([NullAllowed] NSError error);
	delegate void ASAccessorySessionFinishPickerDiscoveryHandler ([NullAllowed] NSError error);

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	interface ASAccessorySettings {
		[Export ("defaultSettings")]
		[Static]
		ASAccessorySettings DefaultSettings { get; }

		[Export ("SSID", ArgumentSemantic.Copy), NullAllowed]
		string Ssid { get; set; }

		[Export ("bluetoothTransportBridgingIdentifier", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothTransportBridgingIdentifier { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	interface ASDiscoveryDescriptor {
		[Export ("supportedOptions", ArgumentSemantic.Assign)]
		ASAccessorySupportOptions SupportedOptions { get; set; }

		[Export ("bluetoothCompanyIdentifier", ArgumentSemantic.Assign)]
		ushort /* ASBluetoothCompanyIdentifier */ BluetoothCompanyIdentifier { get; set; }

		[Export ("bluetoothManufacturerDataBlob", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothManufacturerDataBlob { get; set; }

		[Export ("bluetoothManufacturerDataMask", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothManufacturerDataMask { get; set; }

		[iOS (18, 2)]
		[Export ("bluetoothNameSubstringCompareOptions", ArgumentSemantic.Assign), NullAllowed]
		NSStringCompareOptions BluetoothNameSubstringCompareOptions { get; set; }

		[Export ("bluetoothNameSubstring", ArgumentSemantic.Copy), NullAllowed]
		string BluetoothNameSubstring { get; set; }

		[Export ("bluetoothRange", ArgumentSemantic.Assign)]
		ASDiscoveryDescriptorRange BluetoothRange { get; set; }

		[Export ("bluetoothServiceDataBlob", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothServiceDataBlob { get; set; }

		[Export ("bluetoothServiceDataMask", ArgumentSemantic.Copy), NullAllowed]
		NSData BluetoothServiceDataMask { get; set; }

		[Export ("bluetoothServiceUUID", ArgumentSemantic.Copy), NullAllowed]
		CBUUID BluetoothServiceUuid { get; set; }
		[Export ("SSID", ArgumentSemantic.Copy), NullAllowed]
		string Ssid { get; set; }

		[Export ("SSIDPrefix", ArgumentSemantic.Copy), NullAllowed]
		string SsidPrefix { get; set; }

		[iOS (26, 0)]
		[Export ("wifiAwareServiceName")]
		[NullAllowed]
		string WifiAwareServiceName { get; set; }

		[iOS (26, 0)]
		[Export ("wifiAwareServiceRole", ArgumentSemantic.Assign)]
		ASDiscoveryDescriptorWiFiAwareServiceRole WifiAwareServiceRole { get; set; }

		[iOS (26, 0)]
		[Export ("wifiAwareModelNameMatch", ArgumentSemantic.Copy)]
		[NullAllowed]
		ASPropertyCompareString WifiAwareModelNameMatch { get; set; }

		[iOS (26, 0)]
		[Export ("wifiAwareVendorNameMatch", ArgumentSemantic.Copy)]
		[NullAllowed]
		ASPropertyCompareString WifiAwareVendorNameMatch { get; set; }
	}

	[Native]
	[iOS (18, 0)]
	[ErrorDomain ("ASErrorDomain")]
	enum ASErrorCode : long {
		Success = 0,
		Unknown = 1,
		ActivationFailed = 100,
		ConnectionFailed = 150,
		DiscoveryTimeout = 200,
		ExtensionNotFound = 300,
		Invalidated = 400,
		InvalidRequest = 450,
		PickerAlreadyActive = 500,
		PickerRestricted = 550,
		UserCancelled = 700,
		UserRestricted = 750,
	}

	[BaseType (typeof (NSObject))]
	[iOS (18, 0)]
	[DisableDefaultCtor]
	interface ASPickerDisplayItem {
		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; }

		[Export ("productImage", ArgumentSemantic.Copy)]
		UIImage ProductImage { get; }

		[Export ("descriptor", ArgumentSemantic.Copy)]
		ASDiscoveryDescriptor Descriptor {
			get;
#if !XAMCORE_5_0
			[Obsoleted (PlatformName.iOS, 26, 0, "This property setter is not available.")]
			set;
#endif // !XAMCORE_5_0
		}

		[Export ("renameOptions", ArgumentSemantic.Assign)]
		ASAccessoryRenameOptions RenameOptions { get; set; }

		[Export ("setupOptions", ArgumentSemantic.Assign)]
		ASPickerDisplayItemSetupOptions SetupOptions { get; set; }

		[Export ("initWithName:productImage:descriptor:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, UIImage productImage, ASDiscoveryDescriptor descriptor);
	}

	[BaseType (typeof (ASPickerDisplayItem))]
	[iOS (18, 0)]
	[DisableDefaultCtor]
	interface ASMigrationDisplayItem {
		[Export ("peripheralIdentifier", ArgumentSemantic.Copy), NullAllowed]
		NSUuid PeripheralIdentifier { get; set; }

		[Export ("hotspotSSID", ArgumentSemantic.Copy), NullAllowed]
		string HotspotSsid { get; set; }

		// re-exposed from base
		[Export ("initWithName:productImage:descriptor:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, UIImage productImage, ASDiscoveryDescriptor descriptor);

		[iOS (26, 1)]
		[Export ("wifiAwarePairedDeviceID")]
		ulong WifiAwarePairedDeviceId { get; set; }
	}

	[iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ASPropertyCompareString {
		[Export ("string")]
		string String { get; }

		[Export ("compareOptions", ArgumentSemantic.Assign)]
		NSStringCompareOptions CompareOptions { get; }

		[Export ("initWithString:compareOptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string @string, NSStringCompareOptions compareOptions);
	}

	[iOS (26, 0)]
	[Native]
	public enum ASDiscoveryDescriptorWiFiAwareServiceRole : long {
		Subscriber = 10,
		Publisher = 20,
	}

	[iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface ASPickerDisplaySettings {
		[Static]
		[Export ("defaultSettings")]
		ASPickerDisplaySettings DefaultSettings { get; }

		[Export ("discoveryTimeout")]
		double DiscoveryTimeout { get; set; }

		[iOS (26, 1)]
		[Export ("options", ArgumentSemantic.Assign)]
		ASPickerDisplaySettingsOptions Options { get; set; }
	}

	[Static]
	partial interface ASPickerDisplaySettingsDiscoveryTimeout {
		[iOS (26, 0)]
		[Field ("ASPickerDisplaySettingsDiscoveryTimeoutShort")]
		double Short { get; }

		[iOS (26, 0)]
		[Field ("ASPickerDisplaySettingsDiscoveryTimeoutMedium")]
		double Medium { get; }

		[iOS (26, 0)]
		[Field ("ASPickerDisplaySettingsDiscoveryTimeoutLong")]
		double Long { get; }

		[iOS (26, 1)]
		[Field ("ASPickerDisplaySettingsDiscoveryTimeoutUnbounded")]
		double Unbounded { get; }
	}

	[iOS (26, 1)]
	[BaseType (typeof (ASAccessory))]
	interface ASDiscoveredAccessory {
		[NullAllowed]
		[Wrap ("WeakBluetoothAdvertisementData")]
		CoreBluetooth.AdvertisementData BluetoothAdvertisementData { get; }

		[NullAllowed, Export ("bluetoothAdvertisementData", ArgumentSemantic.Copy)]
		NSDictionary WeakBluetoothAdvertisementData { get; }

		[Export ("bluetoothRSSI", ArgumentSemantic.Copy)]
		[BindAs (typeof (nint?))]
		NSNumber BluetoothRSSI { get; }
	}

	[iOS (26, 1)]
	[BaseType (typeof (ASPickerDisplayItem))]
	[DisableDefaultCtor]
	interface ASDiscoveredDisplayItem {
		[Export ("initWithName:productImage:accessory:")]
		NativeHandle Constructor (string name, UIImage productImage, ASDiscoveredAccessory accessory);
	}

	[iOS (26, 1)]
	[Flags]
	[Native]
	public enum ASPickerDisplaySettingsOptions : ulong {
		None = 0,
		FilterDiscoveryResults = (1uL << 0),
	}
}
