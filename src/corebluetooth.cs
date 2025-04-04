//
// corebluetooth.cs: API definitions for CoreBluetooth
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2011-2013 Xamarin Inc
//
using System.ComponentModel;

using ObjCRuntime;
using Foundation;
using System;
using CoreFoundation;

namespace CoreBluetooth {

	/// <summary>Class that uniquely identifies a Bluetooth device.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreBluetooth/CBAttribute">Apple documentation for <c>CBAttribute</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CBAttribute {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("UUID")]
		CBUUID UUID { get; [NotImplemented] set; }
	}

	/// <summary>Contains options for initializing a Bluetooth central manager.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("CBCentralManager")]
	interface CBCentralInitOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("OptionShowPowerAlertKey")]
		bool ShowPowerAlert { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("OptionRestoreIdentifierKey")]
		string RestoreIdentifier { get; set; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreBluetooth/CBManager">Apple documentation for <c>CBManager</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CBManager {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("state", ArgumentSemantic.Assign)]
		CBManagerState State { get; }

		[Internal]
		[iOS (13, 0)]
		[NoTV]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("authorization", ArgumentSemantic.Assign)]
		CBManagerAuthorization _IAuthorization { get; }

		[Internal]
		[iOS (13, 1)]
		[NoTV]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("authorization", ArgumentSemantic.Assign)]
		CBManagerAuthorization _SAuthorization { get; }

		[TV (13, 0)]
		[NoiOS]
		[NoMacCatalyst]
		[Static]
		[Export ("authorization", ArgumentSemantic.Assign)]
		CBManagerAuthorization Authorization { get; }
	}

	[iOS (13, 0), TV (13, 0), NoMac]
	[MacCatalyst (13, 1)]
	[StrongDictionary ("CBConnectionEventMatchingOptionsKeys")]
	interface CBConnectionEventMatchingOptions {
		NSUuid [] PeripheralUuids { get; set; }
		CBUUID [] ServiceUuids { get; set; }
	}

	[iOS (13, 0), TV (13, 0), NoMac]
	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	interface CBConnectionEventMatchingOptionsKeys {
		[Field ("CBConnectionEventMatchingOptionPeripheralUUIDs")]
		NSString PeripheralUuidsKey { get; }
		[Field ("CBConnectionEventMatchingOptionServiceUUIDs")]
		NSString ServiceUuidsKey { get; }
	}

	[StrongDictionary ("CBConnectPeripheralOptionsKeys")]
	interface CBConnectPeripheralOptions {
		[MacCatalyst (13, 1)]
		bool NotifyOnConnection { get; set; }
		bool NotifyOnDisconnection { get; set; }
		[MacCatalyst (13, 1)]
		bool NotifyOnNotification { get; set; }
		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		bool EnableTransportBridging { get; set; }
		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		bool RequiresAncs { get; set; }
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		bool EnableAutoReconnect { get; }
	}

	[Static]
	[Internal]
	interface CBConnectPeripheralOptionsKeys {
		[MacCatalyst (13, 1)]
		[Field ("CBConnectPeripheralOptionNotifyOnConnectionKey")]
		NSString NotifyOnConnectionKey { get; }
		[Field ("CBConnectPeripheralOptionNotifyOnDisconnectionKey")]
		NSString NotifyOnDisconnectionKey { get; }
		[MacCatalyst (13, 1)]
		[Field ("CBConnectPeripheralOptionNotifyOnNotificationKey")]
		NSString NotifyOnNotificationKey { get; }
		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Field ("CBConnectPeripheralOptionEnableTransportBridgingKey")]
		NSString EnableTransportBridgingKey { get; }
		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Field ("CBConnectPeripheralOptionRequiresANCS")]
		NSString RequiresAncsKey { get; }
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("CBConnectPeripheralOptionEnableAutoReconnect")]
		NSString EnableAutoReconnectKey { get; }
	}

	/// <summary>Represents the local central device in Bluetooth LE.  Use the CBCentralManager to scan, discover and connect to remote peripherals.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBCentralManager_Class/index.html">Apple documentation for <c>CBCentralManager</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBManager), Delegates = new [] { "WeakDelegate" }, Events = new [] { typeof (CBCentralManagerDelegate) })]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBCentralManager {

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the CoreBluetooth.ICBCentralManagerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreBluetooth.ICBCentralManagerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		ICBCentralManagerDelegate Delegate { get; set; }

		[Export ("initWithDelegate:queue:")]
		[PostGet ("WeakDelegate")]
		NativeHandle Constructor ([NullAllowed] ICBCentralManagerDelegate centralDelegate, [NullAllowed] DispatchQueue queue);

		[DesignatedInitializer]
		[MacCatalyst (13, 1)]
		[Export ("initWithDelegate:queue:options:")]
		[PostGet ("WeakDelegate")]
		NativeHandle Constructor ([NullAllowed] ICBCentralManagerDelegate centralDelegate, [NullAllowed] DispatchQueue queue, [NullAllowed] NSDictionary options);

		[MacCatalyst (13, 1)]
		[Wrap ("this (centralDelegate, queue, options.GetDictionary ())")]
		NativeHandle Constructor ([NullAllowed] ICBCentralManagerDelegate centralDelegate, [NullAllowed] DispatchQueue queue, CBCentralInitOptions options);

		[Export ("scanForPeripheralsWithServices:options:"), Internal]
		void ScanForPeripherals ([NullAllowed] NSArray serviceUUIDs, [NullAllowed] NSDictionary options);

		/// <summary>Tells the manager to stop scanning for peripherals.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopScan")]
		void StopScan ();

		/// <param name="peripheral">Peripheral to connect to.</param>
		///         <param name="options">
		///           <para>Options to configure the peripheral connection, the keys include OptionAllowDuplicatesKey and OptionNotifyOnDisconnectionKey which should contain NSNumbers.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Connects to the specified peripheral (weakly typed parameter version).</summary>
		///         <remarks>
		///         </remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("connectPeripheral:options:")]
		void ConnectPeripheral (CBPeripheral peripheral, [NullAllowed] NSDictionary options);

		[Wrap ("ConnectPeripheral (peripheral, options.GetDictionary ())")]
		void ConnectPeripheral (CBPeripheral peripheral, [NullAllowed] CBConnectPeripheralOptions options);

		/// <param name="peripheral">To be added.</param>
		///         <summary>Cancels an active or pending connection to the specified <paramref name="peripheral" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelPeripheralConnection:")]
		void CancelPeripheralConnection (CBPeripheral peripheral);

		/// <summary>Represents the value associated with the constant CBCentralManagerScanOptionAllowDuplicatesKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerScanOptionAllowDuplicatesKey")]
		NSString ScanOptionAllowDuplicatesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBConnectPeripheralOptionStartDelayKey")]
		NSString OptionStartDelayKey { get; }

		/// <summary>Represents the value associated with the constant CBCentralManagerOptionRestoreIdentifierKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerOptionRestoreIdentifierKey")]
		[MacCatalyst (13, 1)]
		NSString OptionRestoreIdentifierKey { get; }

		/// <summary>Represents the value associated with the constant CBCentralManagerRestoredStatePeripheralsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerRestoredStatePeripheralsKey")]
		[MacCatalyst (13, 1)]
		NSString RestoredStatePeripheralsKey { get; }

		/// <summary>Represents the value associated with the constant CBCentralManagerRestoredStateScanServicesKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerRestoredStateScanServicesKey")]
		[MacCatalyst (13, 1)]
		NSString RestoredStateScanServicesKey { get; }

		/// <summary>Represents the value associated with the constant CBCentralManagerRestoredStateScanOptionsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerRestoredStateScanOptionsKey")]
		[MacCatalyst (13, 1)]
		NSString RestoredStateScanOptionsKey { get; }

		/// <param name="identifiers">To be added.</param>
		///         <summary>Returns all peripherals that are identified by the specified <paramref name="identifiers" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("retrievePeripheralsWithIdentifiers:")]
		CBPeripheral [] RetrievePeripheralsWithIdentifiers ([Params] NSUuid [] identifiers);

		/// <param name="serviceUUIDs">To be added.</param>
		///         <summary>Returns all connected peripherals that have services that are identified by the specified <paramref name="serviceUUIDs" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("retrieveConnectedPeripheralsWithServices:")]
		CBPeripheral [] RetrieveConnectedPeripherals ([Params] CBUUID [] serviceUUIDs);

		/// <summary>Represents the value associated with the constant CBCentralManagerOptionShowPowerAlertKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerOptionShowPowerAlertKey")]
		[MacCatalyst (13, 1)]
		NSString OptionShowPowerAlertKey { get; }

		[iOS (16, 0), NoMac, TV (16, 0), MacCatalyst (16, 0)]
		[Field ("CBCentralManagerOptionDeviceAccessForMedia")]
		NSString OptionDeviceAccessForMedia { get; }

		/// <summary>Represents the value associated with the constant CBCentralManagerScanOptionSolicitedServiceUUIDsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBCentralManagerScanOptionSolicitedServiceUUIDsKey")]
		[MacCatalyst (13, 1)]
		NSString ScanOptionSolicitedServiceUUIDsKey { get; }

		/// <summary>Gets a Boolean value that tells whether the manager is currently scanning for peripherals.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isScanning")]
		bool IsScanning { get; }

		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("supportsFeatures:")]
		bool SupportsFeatures (CBCentralManagerFeature features);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Export ("registerForConnectionEventsWithOptions:")]
		void RegisterForConnectionEvents ([NullAllowed] NSDictionary options);

		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Wrap ("RegisterForConnectionEvents (options.GetDictionary ())")]
		void RegisterForConnectionEvents ([NullAllowed] CBConnectionEventMatchingOptions options);
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("AdvertisementDataKeys")]
	interface AdvertisementData {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string LocalName { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSData ManufacturerData { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary<CBUUID, NSData> ServiceData { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CBUUID [] ServiceUuids { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CBUUID [] OverflowServiceUuids { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSNumber TxPowerLevel { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool IsConnectable { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CBUUID [] SolicitedServiceUuids { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Static, Internal]
	interface AdvertisementDataKeys {
		[Field ("CBAdvertisementDataLocalNameKey")]
		NSString LocalNameKey { get; }

		[Field ("CBAdvertisementDataManufacturerDataKey")]
		NSString ManufacturerDataKey { get; }

		[Field ("CBAdvertisementDataServiceDataKey")]
		NSString ServiceDataKey { get; }

		[Field ("CBAdvertisementDataServiceUUIDsKey")]
		NSString ServiceUuidsKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataOverflowServiceUUIDsKey")]
		NSString OverflowServiceUuidsKey { get; }

		[Field ("CBAdvertisementDataTxPowerLevelKey")]
		NSString TxPowerLevelKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataIsConnectable")]
		NSString IsConnectableKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataSolicitedServiceUUIDsKey")]
		NSString SolicitedServiceUuidsKey { get; }
	}

	/// <summary>Possible values for the options parameter in calls to <see cref="M:CoreBluetooth.CBCentralManager.ScanForPeripherals(CoreBluetooth.CBUUID[],Foundation.NSDictionary)" />.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("PeripheralScanningOptionsKeys")]
	interface PeripheralScanningOptions { }

	[MacCatalyst (13, 1)]
	[StrongDictionary ("RestoredStateKeys")]
	interface RestoredState {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CBPeripheral [] Peripherals { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CBPeripheral [] ScanServices { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		PeripheralScanningOptions ScanOptions { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Static, Internal]
	interface RestoredStateKeys {
		[MacCatalyst (13, 1)]
		[Field ("CBCentralManagerRestoredStatePeripheralsKey")]
		NSString PeripheralsKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("CBCentralManagerRestoredStateScanServicesKey")]
		NSString ScanServicesKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("CBCentralManagerRestoredStateScanOptionsKey")]
		NSString ScanOptionsKey { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:CoreBluetooth.CBCentralManagerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:CoreBluetooth.CBCentralManagerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:CoreBluetooth.CBCentralManagerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:CoreBluetooth.CBCentralManagerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICBCentralManagerDelegate { }

	/// <summary>Delegate objects for <see cref="T:CoreBluetooth.CBCentralManager" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBCentralManagerDelegate_Protocol/index.html">Apple documentation for <c>CBCentralManagerDelegate</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CBCentralManagerDelegate {
		/// <param name="central">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("centralManagerDidUpdateState:")]
		void UpdatedState (CBCentralManager central);

		/// <param name="central">To be added.</param>
		///         <param name="peripheral">To be added.</param>
		///         <param name="advertisementData">To be added.</param>
		///         <param name="RSSI">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("centralManager:didDiscoverPeripheral:advertisementData:RSSI:"), EventArgs ("CBDiscoveredPeripheral")]
#if XAMCORE_5_0
		void DiscoveredPeripheral (CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber rssi);
#else
		void DiscoveredPeripheral (CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI);
#endif

		/// <param name="central">To be added.</param>
		///         <param name="peripheral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("centralManager:didConnectPeripheral:"), EventArgs ("CBPeripheral")]
		void ConnectedPeripheral (CBCentralManager central, CBPeripheral peripheral);

		/// <param name="central">To be added.</param>
		///         <param name="peripheral">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("centralManager:didFailToConnectPeripheral:error:"), EventArgs ("CBPeripheralError")]
		void FailedToConnectPeripheral (CBCentralManager central, CBPeripheral peripheral, [NullAllowed] NSError error);

		/// <param name="central">To be added.</param>
		///         <param name="peripheral">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("centralManager:didDisconnectPeripheral:error:"), EventArgs ("CBPeripheralError")]
		void DisconnectedPeripheral (CBCentralManager central, CBPeripheral peripheral, [NullAllowed] NSError error);

		/// <param name="central">To be added.</param>
		///         <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("centralManager:willRestoreState:"), EventArgs ("CBWillRestore")]
		void WillRestoreState (CBCentralManager central, NSDictionary dict);

		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Export ("centralManager:connectionEventDidOccur:forPeripheral:"), EventArgs ("CBPeripheralConnectionEvent")]
		void ConnectionEventDidOccur (CBCentralManager central, CBConnectionEvent connectionEvent, CBPeripheral peripheral);

		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Export ("centralManager:didUpdateANCSAuthorizationForPeripheral:"), EventArgs ("CBAncsAuthorizationUpdate")]
		void DidUpdateAncsAuthorization (CBCentralManager central, CBPeripheral peripheral);

		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0), EventArgs ("CBPeripheralDiconnectionEvent")]
		[Export ("centralManager:didDisconnectPeripheral:timestamp:isReconnecting:error:")]
		void DidDisconnectPeripheral (CBCentralManager central, CBPeripheral peripheral, double timestamp, bool isReconnecting, [NullAllowed] NSError error);
	}

	/// <summary>Keys used to lookup dictionary values from the NSDictionary received as a parameter in <see cref="M:CoreBluetooth.CBCentralManagerDelegate.DiscoveredPeripheral(CoreBluetooth.CBCentralManager,CoreBluetooth.CBPeripheral,Foundation.NSDictionary,Foundation.NSNumber)" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface CBAdvertisement {
		/// <summary>Represents the value associated with the constant CBAdvertisementDataServiceUUIDsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBAdvertisementDataServiceUUIDsKey")]
		NSString DataServiceUUIDsKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataLocalNameKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBAdvertisementDataLocalNameKey")]
		NSString DataLocalNameKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataTxPowerLevelKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBAdvertisementDataTxPowerLevelKey")]
		NSString DataTxPowerLevelKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataManufacturerDataKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBAdvertisementDataManufacturerDataKey")]
		NSString DataManufacturerDataKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataServiceDataKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBAdvertisementDataServiceDataKey")]
		NSString DataServiceDataKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataOverflowServiceUUIDsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataOverflowServiceUUIDsKey")]
		NSString DataOverflowServiceUUIDsKey { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataIsConnectable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataIsConnectable")]
		NSString IsConnectable { get; }

		/// <summary>Represents the value associated with the constant CBAdvertisementDataSolicitedServiceUUIDsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBAdvertisementDataSolicitedServiceUUIDsKey")]
		NSString DataSolicitedServiceUUIDsKey { get; }

	}

	/// <summary>Characteristics of a remote peripheral.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBCharacteristic_Class/index.html">Apple documentation for <c>CBCharacteristic</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBAttribute))]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBCharacteristic {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("properties")]
		CBCharacteristicProperties Properties { get; [NotImplemented ("Not available on CBCharacteristic, only available on CBMutableCharacteristic")] set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("value", ArgumentSemantic.Retain)]
		NSData Value { get; [NotImplemented ("Not available on CBCharacteristic, only available on CBMutableCharacteristic")] set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("descriptors", ArgumentSemantic.Retain)]
		CBDescriptor [] Descriptors { get; [NotImplemented ("Not available on CBCharacteristic, only available on CBMutableCharacteristic")] set; }

		/// <summary>Developers should not use this deprecated property. </summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 8, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("isBroadcasted")]
		bool IsBroadcasted { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isNotifying")]
		bool IsNotifying { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("service", ArgumentSemantic.Weak)]
		CBService Service { get; }
	}

	/// <summary>A mutable <see cref="T:CoreBluetooth.CBCharacteristic" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBMutableCharacteristic_Class/index.html">Apple documentation for <c>CBMutableCharacteristic</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBCharacteristic))]
	[DisableDefaultCtor]
	interface CBMutableCharacteristic {

		[NoTV]
		[MacCatalyst (13, 1)]
		[DesignatedInitializer]
		[Export ("initWithType:properties:value:permissions:")]
		[PostGet ("UUID")]
		[PostGet ("Value")]
		NativeHandle Constructor (CBUUID uuid, CBCharacteristicProperties properties, [NullAllowed] NSData value, CBAttributePermissions permissions);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("permissions", ArgumentSemantic.Assign)]
		CBAttributePermissions Permissions { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("properties", ArgumentSemantic.Assign)]
		[Override]
		CBCharacteristicProperties Properties { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("value", ArgumentSemantic.Retain)]
		[Override]
		NSData Value { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("descriptors", ArgumentSemantic.Retain)]
		[Override]
		CBDescriptor [] Descriptors { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subscribedCentrals")]
		[NullAllowed]
		CBCentral [] SubscribedCentrals { get; }
	}

	/// <summary>An immutable description of a peripheral's characteristic. See also <see cref="T:CoreBluetooth.CBMutableDescriptor" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBDescriptor_Class/index.html">Apple documentation for <c>CBDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBAttribute))]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBDescriptor {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSObject Value { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("characteristic", ArgumentSemantic.Weak)]
		CBCharacteristic Characteristic { get; }
	}

	/// <summary>A mutable <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:Corebluetooth.CBDescriptor&amp;scope=Xamarin" title="T:Corebluetooth.CBDescriptor">T:Corebluetooth.CBDescriptor</a></format>.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBMutableDescriptor_Class/index.html">Apple documentation for <c>CBMutableDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBDescriptor))]
	[DisableDefaultCtor]
	interface CBMutableDescriptor {
		[NoTV]
		[MacCatalyst (13, 1)]
		[DesignatedInitializer]
		[Export ("initWithType:value:")]
		[PostGet ("UUID")]
		[PostGet ("Value")]
		NativeHandle Constructor (CBUUID uuid, [NullAllowed] NSObject descriptorValue);
	}

	/// <summary>Represents a CoreBluetooth peripheral.</summary>
	///     <remarks>
	///       <para>
	/// 	CBPeripherals represent a remote peripheral that the device
	/// 	has discovered or has discovered and connected to.
	///       </para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBPeripheral_Class/index.html">Apple documentation for <c>CBPeripheral</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBPeer), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (CBPeripheralDelegate) })]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBPeripheral : NSCopying {
		/// <summary>Gets the peripheral name.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name", ArgumentSemantic.Retain)]
		[DisableZeroCopy]
		[NullAllowed]
		string Name { get; }

		/// <summary>Developers should not use this deprecated property. </summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 8, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("RSSI", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSNumber RSSI { get; }

		/// <summary>The discovered <see cref="T:CoreBluetooth.CBService" />s of this peripheral.</summary>
		///         <value>Will be <see langword="null" /> until some time after <see cref="M:CoreBluetooth.CBPeripheral.DiscoverServices(CoreBluetooth.CBUUID[])" /> is called.</value>
		///         <remarks>
		///           <para>This property is mutated asynchronously subsequent to calls to <see cref="M:CoreBluetooth.CBPeripheral.DiscoverServices(CoreBluetooth.CBUUID[])" />. Application developers generally override <see cref="M:CoreBluetooth.CBPeripheralDelegate.DiscoveredService(CoreBluetooth.CBPeripheral,Foundation.NSError)" /> to enumerate services. </para>
		///         </remarks>
		[Export ("services", ArgumentSemantic.Retain)]
		[NullAllowed]
		CBService [] Services { get; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the CoreBluetooth.ICBPeripheralDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreBluetooth.ICBPeripheralDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		ICBPeripheralDelegate Delegate { get; set; }

		/// <summary>Reads the signal strength of the peripheral.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readRSSI")]
		void ReadRSSI ();

		[Export ("discoverServices:"), Internal]
		void DiscoverServices ([NullAllowed] NSArray serviceUUIDs);

		[Export ("discoverIncludedServices:forService:"), Internal]
		void DiscoverIncludedServices ([NullAllowed] NSArray includedServiceUUIDs, CBService forService);

		[Export ("discoverCharacteristics:forService:"), Internal]
		void DiscoverCharacteristics ([NullAllowed] NSArray characteristicUUIDs, CBService forService);

		/// <param name="characteristic">To be added.</param>
		///         <summary>Reads the value of the specified <paramref name="characteristic" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readValueForCharacteristic:")]
		void ReadValue (CBCharacteristic characteristic);

		/// <param name="data">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>Writes <paramref name="data" /> to the specified <paramref name="characteristic" /> with the specified <paramref name="type" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("writeValue:forCharacteristic:type:")]
		void WriteValue (NSData data, CBCharacteristic characteristic, CBCharacteristicWriteType type);

		/// <param name="enabled">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <summary>Sets the notification status for the specified <paramref name="characteristic" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNotifyValue:forCharacteristic:")]
		void SetNotifyValue (bool enabled, CBCharacteristic characteristic);

		/// <param name="characteristic">To be added.</param>
		///         <summary>Finds descriptors for the specified <paramref name="characteristic" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("discoverDescriptorsForCharacteristic:")]
		void DiscoverDescriptors (CBCharacteristic characteristic);

		/// <param name="descriptor">To be added.</param>
		///         <summary>Reads the value of the characteristic that is identified by the specified <paramref name="descriptor" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readValueForDescriptor:")]
		void ReadValue (CBDescriptor descriptor);

		/// <param name="data">The data to write.</param>
		///         <param name="descriptor">The descriptor to use for the data.</param>
		///         <summary>Writes <paramref name="data" /> to the characteristic that is identified by the specified <paramref name="descriptor" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("writeValue:forDescriptor:")]
		void WriteValue (NSData data, CBDescriptor descriptor);

		/// <param name="type">To be added.</param>
		///         <summary>Gets the maximum write length for data that is written to the values of the specified <paramref name="type" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("maximumWriteValueLengthForType:")]
		nuint GetMaximumWriteValueLength (CBCharacteristicWriteType type);

		/// <summary>Gets the connection state for the peripheral.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("state")]
		CBPeripheralState State { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("canSendWriteWithoutResponse")]
		bool CanSendWriteWithoutResponse { get; }

		/// <param name="psm">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("openL2CAPChannel:")]
		void OpenL2CapChannel (ushort psm);

		[iOS (13, 0), TV (13, 0), NoMac]
		[MacCatalyst (13, 1)]
		[Export ("ancsAuthorized")]
		bool AncsAuthorized { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:CoreBluetooth.CBPeripheralDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:CoreBluetooth.CBPeripheralDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:CoreBluetooth.CBPeripheralDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:CoreBluetooth.CBPeripheralDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICBPeripheralDelegate { }

	/// <summary>Delegate object for <see cref="T:CoreBluetooth.CBPeripheral" />. Provides methods called on events relating to discovery, exploration, and interaction with a remote peripheral.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBPeripheralDelegate_Protocol/index.html">Apple documentation for <c>CBPeripheralDelegate</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CBPeripheralDelegate {
		/// <param name="peripheral">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'RssiRead' instead.")]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'RssiRead' instead.")]
		[Deprecated (PlatformName.TvOS, 9, 0, message: "Use 'RssiRead' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RssiRead' instead.")]
		[Export ("peripheralDidUpdateRSSI:error:"), EventArgs ("NSError", true)]
		void RssiUpdated (CBPeripheral peripheral, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="rssi">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("peripheral:didReadRSSI:error:"), EventArgs ("CBRssi")]
		void RssiRead (CBPeripheral peripheral, NSNumber rssi, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didDiscoverServices:"), EventArgs ("NSError", true)]
#if XAMCORE_5_0
		void DiscoveredServices (CBPeripheral peripheral, [NullAllowed] NSError error);
#else
		void DiscoveredService (CBPeripheral peripheral, [NullAllowed] NSError error);
#endif

		/// <param name="peripheral">To be added.</param>
		///         <param name="service">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didDiscoverIncludedServicesForService:error:"), EventArgs ("CBService")]
		void DiscoveredIncludedService (CBPeripheral peripheral, CBService service, [NullAllowed] NSError error);

		[Export ("peripheral:didDiscoverCharacteristicsForService:error:"), EventArgs ("CBService")]
#if NET
		void DiscoveredCharacteristics (CBPeripheral peripheral, CBService service, [NullAllowed] NSError error);
#else
		void DiscoveredCharacteristic (CBPeripheral peripheral, CBService service, [NullAllowed] NSError error);
#endif

		/// <param name="peripheral">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didUpdateValueForCharacteristic:error:"), EventArgs ("CBCharacteristic")]
		void UpdatedCharacterteristicValue (CBPeripheral peripheral, CBCharacteristic characteristic, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didWriteValueForCharacteristic:error:"), EventArgs ("CBCharacteristic")]
		void WroteCharacteristicValue (CBPeripheral peripheral, CBCharacteristic characteristic, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didUpdateNotificationStateForCharacteristic:error:"), EventArgs ("CBCharacteristic")]
		void UpdatedNotificationState (CBPeripheral peripheral, CBCharacteristic characteristic, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didDiscoverDescriptorsForCharacteristic:error:"), EventArgs ("CBCharacteristic")]
		void DiscoveredDescriptor (CBPeripheral peripheral, CBCharacteristic characteristic, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="descriptor">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didUpdateValueForDescriptor:error:"), EventArgs ("CBDescriptor")]
		void UpdatedValue (CBPeripheral peripheral, CBDescriptor descriptor, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="descriptor">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didWriteValueForDescriptor:error:"), EventArgs ("CBDescriptor")]
		void WroteDescriptorValue (CBPeripheral peripheral, CBDescriptor descriptor, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralDidUpdateName:")]
		void UpdatedName (CBPeripheral peripheral);

		/// <param name="peripheral">To be added.</param>
		///         <param name="services">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheral:didModifyServices:"), EventArgs ("CBPeripheralServices")]
		void ModifiedServices (CBPeripheral peripheral, CBService [] services);

		/// <param name="peripheral">To be added.</param>
		///         <param name="channel">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EventArgs ("CBPeripheralOpenL2CapChannel")]
		[Export ("peripheral:didOpenL2CAPChannel:error:")]
		void DidOpenL2CapChannel (CBPeripheral peripheral, [NullAllowed] CBL2CapChannel channel, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("peripheralIsReadyToSendWriteWithoutResponse:")]
		void IsReadyToSendWriteWithoutResponse (CBPeripheral peripheral);
	}

	/// <summary>Represents the services of a remote peripheral.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBService_Class/index.html">Apple documentation for <c>CBService</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBAttribute))]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBService {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isPrimary")]
		bool Primary { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includedServices", ArgumentSemantic.Retain)]
		[NullAllowed]
		CBService [] IncludedServices { get; [NotImplemented ("Not available on 'CBService', only available on CBMutableService.")] set; }

		/// <summary>The list of characteristics discovered by this service.</summary>
		///         <value>Array of CBCharacteristic objects.</value>
		///         <remarks>
		/// 	  The contents of this property are only updated after you
		/// 	  have initiated a characteristic discovery using the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Bluetooth%20CBPeripheral%20Discover%20Characteristic&amp;scope=Xamarin" title="E:CoreBluetooth.CBPeripheral.DiscoverCharacteristic">E:CoreBluetooth.CBPeripheral.DiscoverCharacteristic</a></format>
		/// 	  method.
		/// 	</remarks>
		[Export ("characteristics", ArgumentSemantic.Retain)]
		[NullAllowed]
		CBCharacteristic [] Characteristics { get; [NotImplemented ("Not available on 'CBService', only available on CBMutableService.")] set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("peripheral", ArgumentSemantic.Weak)]
		CBPeripheral Peripheral { get; }

	}

	/// <summary>A mutable <see cref="T:CoreBluetooth.CBService" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBMutableService_Class/index.html">Apple documentation for <c>CBMutableService</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBService))]
	[DisableDefaultCtor]
	interface CBMutableService {
		[NoTV]
		[MacCatalyst (13, 1)]
		[DesignatedInitializer]
		[Export ("initWithType:primary:")]
		[PostGet ("UUID")]
		NativeHandle Constructor (CBUUID uuid, bool primary);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includedServices", ArgumentSemantic.Retain)]
		[Override]
		[NullAllowed]
		CBService [] IncludedServices { get; set; }  // TODO: check array type

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("characteristics", ArgumentSemantic.Retain)]
		[Override]
		[NullAllowed]
		CBCharacteristic [] Characteristics { get; set; }   // TODO: check array type
	}

	/// <summary>Universal Unique Identifiers for the Bluetooth stack.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBUUID_Class/index.html">Apple documentation for <c>CBUUID</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // crash (at dispose time) on OSX
	interface CBUUID : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("data")]
		NSData Data { get; }

		/// <param name="theString">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[MarshalNativeExceptions]
		[Export ("UUIDWithString:")]
		CBUUID FromString (string theString);

		/// <param name="theData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("UUIDWithData:")]
		CBUUID FromData (NSData theData);

		/// <param name="theUUID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Deprecated (PlatformName.iOS, 9, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Static]
		[Export ("UUIDWithCFUUID:")]
		CBUUID FromCFUUID (IntPtr theUUID);

		/// <param name="theUUID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[MacCatalyst (13, 1)]
		[Export ("UUIDWithNSUUID:")]
		CBUUID FromNSUuid (NSUuid theUUID);

		/// <summary>Represents the value associated with the constant CBUUIDCharacteristicExtendedPropertiesString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDCharacteristicExtendedPropertiesString")]
		NSString CharacteristicExtendedPropertiesString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDCharacteristicUserDescriptionString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDCharacteristicUserDescriptionString")]
		NSString CharacteristicUserDescriptionString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDClientCharacteristicConfigurationString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDClientCharacteristicConfigurationString")]
		NSString ClientCharacteristicConfigurationString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDServerCharacteristicConfigurationString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDServerCharacteristicConfigurationString")]
		NSString ServerCharacteristicConfigurationString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDCharacteristicFormatString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDCharacteristicFormatString")]
		NSString CharacteristicFormatString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDCharacteristicAggregateFormatString</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBUUIDCharacteristicAggregateFormatString")]
		NSString CharacteristicAggregateFormatString { get; }

		[Internal]
		[Field ("CBUUIDValidRangeString")]
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Obsoleted (PlatformName.MacOSX, 10, 13)]
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		NSString CBUUIDValidRangeString { get; }

		/// <summary>Represents the value associated with the constant CBUUIDCharacteristicValidRangeString</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBUUIDCharacteristicValidRangeString")]
		NSString CharacteristicValidRangeString { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("CBUUIDCharacteristicObservationScheduleString")]
		NSString CharacteristicObservationScheduleString { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CBUUIDL2CAPPSMCharacteristicString")]
		NSString L2CapPsmCharacteristicString { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("UUIDString")]
		string Uuid { get; }
	}

	/// <summary>An Attribute Protocol request for reading or writing.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBATTRequest_class/index.html">Apple documentation for <c>CBATTRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CBATTRequest {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("central", ArgumentSemantic.Retain)]
		CBCentral Central { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("characteristic", ArgumentSemantic.Retain)]
		CBCharacteristic Characteristic { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("offset")]
		nint Offset { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSData Value { get; set; }
	}

	/// <summary>Used to identify centrals (that are not the current device).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBCentral_Class/index.html">Apple documentation for <c>CBCentral</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CBPeer))]
	// `delloc` a default instance crash applications and a default instance, without the ability to change the UUID, does not make sense
	[DisableDefaultCtor]
	interface CBCentral : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("identifier")]
		NSUuid Identifier { get; }

		// Introduced with iOS7, but does not have NS_AVAILABLE
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumUpdateValueLength")]
		nuint MaximumUpdateValueLength { get; }
	}

	/// <summary>Manages published services per the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:Coreblutooth.CBPeripheral&amp;scope=Xamarin" title="T:Coreblutooth.CBPeripheral">T:Coreblutooth.CBPeripheral</a></format> device's GATT database.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBPeripheralManager_Class/index.html">Apple documentation for <c>CBPeripheralManager</c></related>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CBManager), Delegates = new [] { "WeakDelegate" }, Events = new [] { typeof (CBPeripheralManagerDelegate) })]
	interface CBPeripheralManager {

		[Export ("init")]
		NativeHandle Constructor ();

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("initWithDelegate:queue:")]
		[PostGet ("WeakDelegate")]
		NativeHandle Constructor ([NullAllowed] ICBPeripheralManagerDelegate peripheralDelegate, [NullAllowed] DispatchQueue queue);

		[NoTV]
		[MacCatalyst (13, 1)]
		[DesignatedInitializer]
		[Export ("initWithDelegate:queue:options:")]
		[PostGet ("WeakDelegate")]
		NativeHandle Constructor ([NullAllowed] ICBPeripheralManagerDelegate peripheralDelegate, [NullAllowed] DispatchQueue queue, [NullAllowed] NSDictionary options);

		/// <summary>An instance of the CoreBluetooth.ICBPeripheralManagerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreBluetooth.ICBPeripheralManagerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[NullAllowed]
		[Wrap ("WeakDelegate")]
		ICBPeripheralManagerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NullAllowed]
		[Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isAdvertising")]
		bool Advertising { get; }

		/// <param name="service">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addService:")]
		void AddService (CBMutableService service);

		/// <param name="service">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeService:")]
		void RemoveService (CBMutableService service);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllServices")]
		void RemoveAllServices ();

		/// <param name="request">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("respondToRequest:withResult:")]
		void RespondToRequest (CBATTRequest request, CBATTError result); // TODO: Could it return CBATTError?. This won't work because it's a value

		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("startAdvertising:")]
		void StartAdvertising ([NullAllowed] NSDictionary options);

		/// <param name="options">
		///           <para>Weakly typed set of options to advertise.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("StartAdvertising (options.GetDictionary ())")]
		void StartAdvertising ([NullAllowed] StartAdvertisingOptions options);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopAdvertising")]
		void StopAdvertising ();

		/// <param name="latency">To be added.</param>
		///         <param name="connectedCentral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setDesiredConnectionLatency:forCentral:")]
		void SetDesiredConnectionLatency (CBPeripheralManagerConnectionLatency latency, CBCentral connectedCentral);

		/// <param name="value">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <param name="subscribedCentrals">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("updateValue:forCharacteristic:onSubscribedCentrals:")]
		bool UpdateValue (NSData value, CBMutableCharacteristic characteristic, [NullAllowed] CBCentral [] subscribedCentrals);

		/// <param name="encryptionRequired">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("publishL2CAPChannelWithEncryption:")]
		void PublishL2CapChannel (bool encryptionRequired);

		/// <param name="psm">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("unpublishL2CAPChannel:")]
		void UnpublishL2CapChannel (ushort psm);

		/// <summary>Represents the value associated with the constant CBPeripheralManagerOptionShowPowerAlertKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBPeripheralManagerOptionShowPowerAlertKey")]
		NSString OptionShowPowerAlertKey { get; }

		/// <summary>Represents the value associated with the constant CBPeripheralManagerOptionRestoreIdentifierKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBPeripheralManagerOptionRestoreIdentifierKey")]
		NSString OptionRestoreIdentifierKey { get; }

		/// <summary>Represents the value associated with the constant CBPeripheralManagerRestoredStateServicesKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBPeripheralManagerRestoredStateServicesKey")]
		NSString RestoredStateServicesKey { get; }

		/// <summary>Represents the value associated with the constant CBPeripheralManagerRestoredStateAdvertisementDataKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CBPeripheralManagerRestoredStateAdvertisementDataKey")]
		NSString RestoredStateAdvertisementDataKey { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:CoreBluetooth.CBPeripheralManagerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:CoreBluetooth.CBPeripheralManagerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:CoreBluetooth.CBPeripheralManagerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:CoreBluetooth.CBPeripheralManagerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICBPeripheralManagerDelegate { }

	/// <summary>Delegate object for <see cref="T:CoreBluetooth.CBPeripheralManager" />. Adds methods for events relating to availability, publishing, advertising, and subscription.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreBluetooth/Reference/CBPeripheralManagerDelegate_Protocol/index.html">Apple documentation for <c>CBPeripheralManagerDelegate</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CBPeripheralManagerDelegate {
		/// <param name="peripheral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("peripheralManagerDidUpdateState:")]
		void StateUpdated (CBPeripheralManager peripheral);

		/// <param name="peripheral">To be added.</param>
		///         <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:willRestoreState:"), EventArgs ("CBWillRestore")]
		void WillRestoreState (CBPeripheralManager peripheral, NSDictionary dict);

		/// <param name="peripheral">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManagerDidStartAdvertising:error:"), EventArgs ("NSError", true)]
		void AdvertisingStarted (CBPeripheralManager peripheral, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="service">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:didAddService:error:"), EventArgs ("CBPeripheralManagerService")]
		void ServiceAdded (CBPeripheralManager peripheral, CBService service, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="central">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:central:didSubscribeToCharacteristic:"), EventArgs ("CBPeripheralManagerSubscription")]
		void CharacteristicSubscribed (CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic);

		/// <param name="peripheral">To be added.</param>
		///         <param name="central">To be added.</param>
		///         <param name="characteristic">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:central:didUnsubscribeFromCharacteristic:"), EventArgs ("CBPeripheralManagerSubscription")]
		void CharacteristicUnsubscribed (CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic);

		/// <param name="peripheral">To be added.</param>
		///         <param name="request">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:didReceiveReadRequest:"), EventArgs ("CBATTRequest")]
		void ReadRequestReceived (CBPeripheralManager peripheral, CBATTRequest request);

		/// <param name="peripheral">To be added.</param>
		///         <param name="requests">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManager:didReceiveWriteRequests:"), EventArgs ("CBATTRequests")]
		void WriteRequestsReceived (CBPeripheralManager peripheral, CBATTRequest [] requests);

		/// <param name="peripheral">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peripheralManagerIsReadyToUpdateSubscribers:")]
		void ReadyToUpdateSubscribers (CBPeripheralManager peripheral);

		/// <param name="peripheral">To be added.</param>
		///         <param name="channel">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EventArgs ("CBPeripheralManagerOpenL2CapChannel")]
		[Export ("peripheralManager:didOpenL2CAPChannel:error:")]
		void DidOpenL2CapChannel (CBPeripheralManager peripheral, [NullAllowed] CBL2CapChannel channel, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="psm">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EventArgs ("CBPeripheralManagerL2CapChannelOperation")]
		[Export ("peripheralManager:didUnpublishL2CAPChannel:error:")]
		void DidUnpublishL2CapChannel (CBPeripheralManager peripheral, ushort psm, [NullAllowed] NSError error);

		/// <param name="peripheral">To be added.</param>
		///         <param name="psm">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EventArgs ("CBPeripheralManagerL2CapChannelOperation")]
		[Export ("peripheralManager:didPublishL2CAPChannel:error:")]
		void DidPublishL2CapChannel (CBPeripheralManager peripheral, ushort psm, [NullAllowed] NSError error);
	}

	/// <summary>Class that represents a central or perhipheral Bluetooth device.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreBluetooth/CBPeer">Apple documentation for <c>CBPeer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // CBPeer.h: - (instancetype)init NS_UNAVAILABLE;
	interface CBPeer : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		NSUuid Identifier { get; }
	}

	// The type is available in 32bits macOS 10.13 even if most properties are 64 bits only
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "CBL2CAPChannel")]
	interface CBL2CapChannel {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("peer")]
		CBPeer Peer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputStream")]
		NSInputStream InputStream { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outputStream")]
		NSOutputStream OutputStream { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PSM")]
		/* uint16_t */
		ushort Psm { get; }
	}
}
