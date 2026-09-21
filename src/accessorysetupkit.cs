
using CoreBluetooth;
using CoreFoundation;
using UIKit;

using ASAccessoryWiFiAwarePairedDeviceId = System.UInt64;

namespace AccessorySetupKit {
	/// <summary>Specifies the authorization state of an accessory.</summary>
	[Native]
	[iOS (18, 0)]
	public enum ASAccessoryState : long {
		/// <summary>The accessory is invalid or unauthorized.</summary>
		Unauthorized = 0,
		/// <summary>The accessory is selected, but full authorization is pending.</summary>
		AwaitingAuthorization = 10,
		/// <summary>The accessory is authorized and available.</summary>
		Authorized = 20,
	}

	/// <summary>Specifies options for renaming an accessory.</summary>
	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASAccessoryRenameOptions : ulong {
		/// <summary>Changes the accessory's SSID along with its display name.</summary>
		Ssid = 1U << 0,
	}

	/// <summary>Specifies the technologies that an accessory supports.</summary>
	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASAccessorySupportOptions : ulong {
		/// <summary>The accessory supports Bluetooth Low Energy pairing.</summary>
		BluetoothPairingLE = 1U << 1,
		/// <summary>The accessory supports activating Bluetooth Classic transport profiles over a Bluetooth Low Energy connection.</summary>
		BluetoothTransportBridging = 1U << 2,
		/// <summary>The accessory supports the Bluetooth Low Energy Human Interface Device service.</summary>
		[iOS (18, 4)]
		BluetoothHid = 1U << 3,
	}

	/// <summary>Specifies the range within which to discover Bluetooth accessories.</summary>
	[Native]
	[iOS (18, 0)]
	public enum ASDiscoveryDescriptorRange : long {
		/// <summary>Uses the default accessory discovery range.</summary>
		Default = 0,
		/// <summary>Discovers accessories in the immediate vicinity of the device.</summary>
		Immediate = 10,
	}

	/// <summary>Specifies options for setting up an accessory in the picker.</summary>
	[Flags]
	[Native]
	[iOS (18, 0)]
	public enum ASPickerDisplayItemSetupOptions : long {
		/// <summary>Asks the user to rename the accessory.</summary>
		Rename = 1 << 0,
		/// <summary>Requires the app to finish accessory authorization before showing the setup view.</summary>
		ConfirmAuthorization = 1 << 1,
		/// <summary>Asks the user to complete additional setup in the app after authorization.</summary>
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

	/// <summary>A completion handler for accessory session operations.</summary>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the operation succeeded.</param>
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

	/// <summary>A completion handler for updating accessory authorization.</summary>
	/// <param name="error">The error that occurred, or <see langword="null" /> if authorization was updated successfully.</param>
	delegate void ASAccessorySessionUpdateAuthorizationHandler ([NullAllowed] NSError error);
	/// <summary>A completion handler for updating the accessories shown in the picker.</summary>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the picker was updated successfully.</param>
	delegate void ASAccessorySessionUpdatePickerHandler ([NullAllowed] NSError error);
	/// <summary>A completion handler for finishing picker discovery.</summary>
	/// <param name="error">The error that occurred, or <see langword="null" /> if discovery finished successfully.</param>
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

	/// <summary>Specifies errors reported by AccessorySetupKit.</summary>
	[Native]
	[iOS (18, 0)]
	[ErrorDomain ("ASErrorDomain")]
	enum ASErrorCode : long {
		/// <summary>The operation completed successfully.</summary>
		Success = 0,
		/// <summary>An underlying failure occurred for an unknown reason.</summary>
		Unknown = 1,
		/// <summary>The session could not be activated.</summary>
		ActivationFailed = 100,
		/// <summary>The session could not establish a connection with the accessory.</summary>
		ConnectionFailed = 150,
		/// <summary>Accessory discovery timed out.</summary>
		DiscoveryTimeout = 200,
		/// <summary>The app extension could not be found.</summary>
		ExtensionNotFound = 300,
		/// <summary>The session was invalidated before the operation completed.</summary>
		Invalidated = 400,
		/// <summary>The session received an invalid request.</summary>
		InvalidRequest = 450,
		/// <summary>The picker received a request to show while it was already active.</summary>
		PickerAlreadyActive = 500,
		/// <summary>The picker cannot be used because the app is in the background.</summary>
		PickerRestricted = 550,
		/// <summary>The user canceled the operation.</summary>
		UserCancelled = 700,
		/// <summary>The user restricted access.</summary>
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

	/// <summary>Specifies the service role of a Wi-Fi Aware accessory.</summary>
	[iOS (26, 0)]
	[Native]
	public enum ASDiscoveryDescriptorWiFiAwareServiceRole : long {
		/// <summary>The accessory uses the subscriber service role.</summary>
		Subscriber = 10,
		/// <summary>The accessory uses the publisher service role.</summary>
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

	/// <summary>Provides information about an accessory discovered for custom picker filtering.</summary>
	[iOS (26, 1)]
	[BaseType (typeof (ASAccessory))]
	interface ASDiscoveredAccessory {
		/// <summary>Gets the parsed Bluetooth advertisement data from the discovered accessory.</summary>
		[NullAllowed]
		[Wrap ("WeakBluetoothAdvertisementData")]
		CoreBluetooth.AdvertisementData BluetoothAdvertisementData { get; }

		/// <summary>Gets the raw Bluetooth advertisement data from the discovered accessory.</summary>
		[NullAllowed, Export ("bluetoothAdvertisementData", ArgumentSemantic.Copy)]
		NSDictionary WeakBluetoothAdvertisementData { get; }

		/// <summary>Gets the Bluetooth received signal strength, in dBm, when the accessory was discovered.</summary>
		[Export ("bluetoothRSSI", ArgumentSemantic.Copy)]
		[BindAs (typeof (nint?))]
		NSNumber BluetoothRSSI { get; }
	}

	/// <summary>A picker display item created by customizing a discovered accessory.</summary>
	[iOS (26, 1)]
	[BaseType (typeof (ASPickerDisplayItem))]
	[DisableDefaultCtor]
	interface ASDiscoveredDisplayItem {
		/// <summary>Creates a picker display item for a discovered accessory.</summary>
		/// <param name="name">The accessory name to display in the picker.</param>
		/// <param name="productImage">The accessory image to display in the picker.</param>
		/// <param name="accessory">The discovered accessory to display in the picker.</param>
		[Export ("initWithName:productImage:accessory:")]
		NativeHandle Constructor (string name, UIImage productImage, ASDiscoveredAccessory accessory);
	}

	/// <summary>Specifies options that customize accessory picker discovery.</summary>
	[iOS (26, 1)]
	[Flags]
	[Native]
	public enum ASPickerDisplaySettingsOptions : ulong {
		/// <summary>No custom picker discovery options are enabled.</summary>
		None = 0,
		/// <summary>Passes discovered accessories to the app for filtering before displaying them in the picker.</summary>
		FilterDiscoveryResults = (1uL << 0),
	}
}
