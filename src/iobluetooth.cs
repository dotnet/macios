// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// iobluetooth.cs: Bindings for the IOBluetooth framework (macOS only).
//
// APIs that were deprecated in macOS 10.9 or earlier (including all the
// *Ref-based accessors) are not bound.

using Foundation;
using ObjCRuntime;

namespace IOBluetooth {

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothObject : NSCopying {
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothUserNotification {
		[Export ("unregister")]
		void Unregister ();
	}

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothDeviceAsyncCallbacks {
		[Export ("remoteNameRequestComplete:status:")]
		void RemoteNameRequestComplete (IOBluetoothDevice device, int status);

		[Export ("connectionComplete:status:")]
		void ConnectionComplete (IOBluetoothDevice device, int status);

		[Export ("sdpQueryComplete:status:")]
		void SdpQueryComplete (IOBluetoothDevice device, int status);
	}

	[NoMacCatalyst]
	[BaseType (typeof (IOBluetoothObject))]
	[DisableDefaultCtor]
	interface IOBluetoothDevice : NSCoding, NSSecureCoding {
		[Static]
		[Export ("registerForConnectNotifications:selector:")]
		IOBluetoothUserNotification RegisterForConnectNotifications (NSObject observer, Selector selector);

		[Export ("registerForDisconnectNotification:selector:")]
		IOBluetoothUserNotification RegisterForDisconnectNotification (NSObject observer, Selector selector);

		[Static]
		[Export ("deviceWithAddress:")]
		[return: NullAllowed]
		IOBluetoothDevice FromAddress (ref BluetoothDeviceAddress address);

		[Static]
		[Export ("deviceWithAddressString:")]
		[return: NullAllowed]
		IOBluetoothDevice FromAddressString (string address);

		[Export ("openL2CAPChannelSync:withPSM:delegate:")]
		int OpenL2CapChannelSync ([NullAllowed] out IOBluetoothL2CAPChannel newChannel, ushort psm, [NullAllowed] NSObject channelDelegate);

		[Export ("openL2CAPChannelAsync:withPSM:delegate:")]
		int OpenL2CapChannelAsync ([NullAllowed] out IOBluetoothL2CAPChannel newChannel, ushort psm, [NullAllowed] NSObject channelDelegate);

		[Export ("openL2CAPChannelSync:withPSM:withConfiguration:delegate:")]
		int OpenL2CapChannelSync ([NullAllowed] out IOBluetoothL2CAPChannel newChannel, ushort psm, [NullAllowed] NSDictionary channelConfiguration, [NullAllowed] NSObject channelDelegate);

		[Export ("openL2CAPChannelAsync:withPSM:withConfiguration:delegate:")]
		int OpenL2CapChannelAsync ([NullAllowed] out IOBluetoothL2CAPChannel newChannel, ushort psm, [NullAllowed] NSDictionary channelConfiguration, [NullAllowed] NSObject channelDelegate);

		[Export ("sendL2CAPEchoRequest:length:")]
		int SendL2CapEchoRequest (IntPtr data, ushort length);

		[Export ("openRFCOMMChannelSync:withChannelID:delegate:")]
		int OpenRfcommChannelSync ([NullAllowed] out IOBluetoothRFCOMMChannel rfcommChannel, byte channelId, [NullAllowed] NSObject channelDelegate);

		[Export ("openRFCOMMChannelAsync:withChannelID:delegate:")]
		int OpenRfcommChannelAsync ([NullAllowed] out IOBluetoothRFCOMMChannel rfcommChannel, byte channelId, [NullAllowed] NSObject channelDelegate);

		[Export ("classOfDevice")]
		uint ClassOfDevice { get; }

		[Export ("serviceClassMajor")]
		uint ServiceClassMajor { get; }

		[Export ("deviceClassMajor")]
		uint DeviceClassMajor { get; }

		[Export ("deviceClassMinor")]
		uint DeviceClassMinor { get; }

		[NullAllowed, Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("nameOrAddress")]
		string NameOrAddress { get; }

		[NullAllowed, Export ("lastNameUpdate", ArgumentSemantic.Copy)]
		NSDate LastNameUpdate { get; }

		// const BluetoothDeviceAddress *
		[Export ("getAddress")]
		IntPtr GetAddress ();

		[NullAllowed, Export ("addressString")]
		string AddressString { get; }

		[Export ("getPageScanRepetitionMode")]
		byte GetPageScanRepetitionMode ();

		[Export ("getPageScanPeriodMode")]
		byte GetPageScanPeriodMode ();

		[Export ("getPageScanMode")]
		byte GetPageScanMode ();

		[Export ("getClockOffset")]
		ushort GetClockOffset ();

		[return: NullAllowed]
		[Export ("getLastInquiryUpdate")]
		NSDate GetLastInquiryUpdate ();

		[Export ("RSSI")]
		sbyte Rssi ();

		[Export ("rawRSSI")]
		sbyte RawRssi ();

		[Export ("isConnected")]
		bool IsConnected { get; }

		[Export ("openConnection")]
		int OpenConnection ();

		[Export ("openConnection:")]
		int OpenConnection ([NullAllowed] NSObject target);

		[Export ("openConnection:withPageTimeout:authenticationRequired:")]
		int OpenConnection ([NullAllowed] NSObject target, ushort pageTimeoutValue, bool authenticationRequired);

		[Export ("closeConnection")]
		int CloseConnection ();

		[Export ("remoteNameRequest:")]
		int RemoteNameRequest ([NullAllowed] NSObject target);

		[Export ("remoteNameRequest:withPageTimeout:")]
		int RemoteNameRequest ([NullAllowed] NSObject target, ushort pageTimeoutValue);

		[Export ("requestAuthentication")]
		int RequestAuthentication ();

		[Export ("connectionHandle")]
		ushort ConnectionHandle { get; }

		[Export ("isIncoming")]
		bool IsIncoming { get; }

		[Export ("getLinkType")]
		byte GetLinkType ();

		[Export ("getEncryptionMode")]
		byte GetEncryptionMode ();

		[Export ("performSDPQuery:")]
		int PerformSdpQuery ([NullAllowed] NSObject target);

		[Export ("performSDPQuery:uuids:")]
		int PerformSdpQuery ([NullAllowed] NSObject target, [NullAllowed] NSArray uuidArray);

		[NullAllowed, Export ("services")]
		IOBluetoothSDPServiceRecord [] Services { get; }

		[return: NullAllowed]
		[Export ("getLastServicesUpdate")]
		NSDate GetLastServicesUpdate ();

		[return: NullAllowed]
		[Export ("getServiceRecordForUUID:")]
		IOBluetoothSDPServiceRecord GetServiceRecord (IOBluetoothSDPUUID sdpUuid);

		[Static]
		[NullAllowed, Export ("favoriteDevices")]
		IOBluetoothDevice [] FavoriteDevices { get; }

		[Export ("isFavorite")]
		bool IsFavorite { get; }

		[Export ("addToFavorites")]
		int AddToFavorites ();

		[Export ("removeFromFavorites")]
		int RemoveFromFavorites ();

		[Static]
		[Export ("recentDevices:")]
		[return: NullAllowed]
		IOBluetoothDevice [] GetRecentDevices (nuint numDevices);

		[return: NullAllowed]
		[Export ("recentAccessDate")]
		NSDate GetRecentAccessDate ();

		[Static]
		[NullAllowed, Export ("pairedDevices")]
		IOBluetoothDevice [] PairedDevices { get; }

		[Export ("isPaired")]
		bool IsPaired { get; }

		[Export ("setSupervisionTimeout:")]
		int SetSupervisionTimeout (ushort timeout);
	}

	[NoMacCatalyst]
	[Category]
	[BaseType (typeof (IOBluetoothDevice))]
	interface IOBluetoothDevice_HandsFreeDeviceAdditions {
		[return: NullAllowed]
		[Export ("handsFreeAudioGatewayServiceRecord")]
		IOBluetoothSDPServiceRecord GetHandsFreeAudioGatewayServiceRecord ();

		[Export ("isHandsFreeAudioGateway")]
		bool GetIsHandsFreeAudioGateway ();

		[return: NullAllowed]
		[Export ("handsFreeDeviceServiceRecord")]
		IOBluetoothSDPServiceRecord GetHandsFreeDeviceServiceRecord ();

		[Export ("isHandsFreeDevice")]
		bool GetIsHandsFreeDevice ();
	}

	[NoMacCatalyst]
	[Category]
	[BaseType (typeof (IOBluetoothSDPServiceRecord))]
	interface IOBluetoothSDPServiceRecord_HandsFreeSDPServiceRecordAdditions {
		[Export ("handsFreeSupportedFeatures")]
		ushort GetHandsFreeSupportedFeatures ();
	}

	interface IIOBluetoothDeviceInquiryDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothDeviceInquiryDelegate {
		[Export ("deviceInquiryStarted:")]
		void DeviceInquiryStarted (IOBluetoothDeviceInquiry sender);

		[Export ("deviceInquiryDeviceFound:device:")]
		void DeviceInquiryDeviceFound (IOBluetoothDeviceInquiry sender, IOBluetoothDevice device);

		[Export ("deviceInquiryUpdatingDeviceNamesStarted:devicesRemaining:")]
		void DeviceInquiryUpdatingDeviceNamesStarted (IOBluetoothDeviceInquiry sender, uint devicesRemaining);

		[Export ("deviceInquiryDeviceNameUpdated:device:devicesRemaining:")]
		void DeviceInquiryDeviceNameUpdated (IOBluetoothDeviceInquiry sender, IOBluetoothDevice device, uint devicesRemaining);

		[Export ("deviceInquiryComplete:error:aborted:")]
		void DeviceInquiryComplete (IOBluetoothDeviceInquiry sender, int error, bool aborted);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothDeviceInquiry {
		[NullAllowed, Wrap ("WeakDelegate")]
		IIOBluetoothDeviceInquiryDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Static]
		[Export ("inquiryWithDelegate:")]
		IOBluetoothDeviceInquiry Create ([NullAllowed] NSObject delegateObject);

		[Export ("initWithDelegate:")]
		NativeHandle Constructor ([NullAllowed] NSObject delegateObject);

		[Export ("start")]
		int Start ();

		[Export ("stop")]
		int Stop ();

		[Export ("inquiryLength")]
		byte InquiryLength { get; set; }

		[Export ("searchType", ArgumentSemantic.Assign)]
		IOBluetoothDeviceSearchTypes SearchType { get; set; }

		[Export ("updateNewDeviceNames")]
		bool UpdateNewDeviceNames { get; set; }

		[NullAllowed, Export ("foundDevices")]
		IOBluetoothDevice [] FoundDevices { get; }

		[Export ("clearFoundDevices")]
		void ClearFoundDevices ();

		[Export ("setSearchCriteria:majorDeviceClass:minorDeviceClass:")]
		void SetSearchCriteria (uint serviceClassMajor, uint majorDeviceClass, uint minorDeviceClass);
	}

	interface IIOBluetoothDevicePairDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothDevicePairDelegate {
		[Export ("devicePairingStarted:")]
		void DevicePairingStarted (NSObject sender);

		[Export ("devicePairingConnecting:")]
		void DevicePairingConnecting (NSObject sender);

		[Export ("devicePairingConnected:")]
		void DevicePairingConnected (NSObject sender);

		[Export ("devicePairingPINCodeRequest:")]
		void DevicePairingPinCodeRequest (NSObject sender);

		[Export ("devicePairingUserConfirmationRequest:numericValue:")]
		void DevicePairingUserConfirmationRequest (NSObject sender, uint numericValue);

		[Export ("devicePairingUserPasskeyNotification:passkey:")]
		void DevicePairingUserPasskeyNotification (NSObject sender, uint passkey);

		[Export ("devicePairingFinished:error:")]
		void DevicePairingFinished (NSObject sender, int error);

		[Export ("deviceSimplePairingComplete:status:")]
		void DeviceSimplePairingComplete (NSObject sender, byte status);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothDevicePair {
		[NullAllowed, Wrap ("WeakDelegate")]
		IIOBluetoothDevicePairDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Static]
		[Export ("pairWithDevice:")]
		IOBluetoothDevicePair Create (IOBluetoothDevice device);

		[Export ("start")]
		int Start ();

		[Export ("stop")]
		void Stop ();

		[NullAllowed, Export ("device")]
		IOBluetoothDevice Device { get; set; }

		[Export ("replyPINCode:PINCode:")]
		void ReplyPinCode (nuint pinCodeSize, ref BluetoothPINCode pinCode);

		[Export ("replyUserConfirmation:")]
		void ReplyUserConfirmation (bool reply);
	}

	[NoMacCatalyst]
	[Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothHostControllerDelegate {
		// BluetoothHCIRSSIInfo *
		[Export ("readRSSIForDeviceComplete:device:info:error:")]
		void ReadRssiForDeviceComplete (NSObject controller, IOBluetoothDevice device, IntPtr info, int error);

		// BluetoothHCILinkQualityInfo *
		[Export ("readLinkQualityForDeviceComplete:device:info:error:")]
		void ReadLinkQualityForDeviceComplete (NSObject controller, IOBluetoothDevice device, IntPtr info, int error);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothHostController {
		[NullAllowed, Wrap ("WeakDelegate")]
		IOBluetoothHostControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Static]
		[NullAllowed, Export ("defaultController")]
		IOBluetoothHostController DefaultController { get; }

		[Export ("powerState")]
		BluetoothHCIPowerState PowerState { get; }

		[Export ("classOfDevice")]
		uint ClassOfDevice { get; }

		[Export ("setClassOfDevice:forTimeInterval:")]
		int SetClassOfDevice (uint classOfDevice, double seconds);

		[NullAllowed, Export ("addressAsString")]
		string AddressAsString { get; }

		[NullAllowed, Export ("nameAsString")]
		string NameAsString { get; }

		[Notification]
		[Field ("IOBluetoothHostControllerPoweredOnNotification", "IOBluetooth")]
		NSString PoweredOnNotification { get; }

		[Notification]
		[Field ("IOBluetoothHostControllerPoweredOffNotification", "IOBluetooth")]
		NSString PoweredOffNotification { get; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSData))]
	[DisableDefaultCtor]
	interface IOBluetoothSDPUUID {
		[Static]
		[Export ("uuidWithBytes:length:")]
		[return: NullAllowed]
		IOBluetoothSDPUUID FromBytes (IntPtr bytes, uint length);

		[Static]
		[Export ("uuidWithData:")]
		[return: NullAllowed]
		IOBluetoothSDPUUID FromData (NSData data);

		[Static]
		[Export ("uuid16:")]
		[return: NullAllowed]
		IOBluetoothSDPUUID FromUuid16 (ushort uuid16);

		[Static]
		[Export ("uuid32:")]
		[return: NullAllowed]
		IOBluetoothSDPUUID FromUuid32 (uint uuid32);

		[Export ("initWithUUID16:")]
		NativeHandle Constructor (ushort uuid16);

		[Export ("initWithUUID32:")]
		NativeHandle Constructor (uint uuid32);

		[Export ("getUUIDWithLength:")]
		[return: NullAllowed]
		IOBluetoothSDPUUID GetUuid (uint newLength);

		[Export ("isEqualToUUID:")]
		bool IsEqualTo (IOBluetoothSDPUUID otherUuid);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothSDPServiceAttribute : NSCoding, NSSecureCoding {
		[Static]
		[Export ("withID:attributeElementValue:")]
		[return: NullAllowed]
		IOBluetoothSDPServiceAttribute Create (ushort attributeId, NSObject attributeElementValue);

		[Static]
		[Export ("withID:attributeElement:")]
		[return: NullAllowed]
		IOBluetoothSDPServiceAttribute Create (ushort attributeId, IOBluetoothSDPDataElement attributeElement);

		[Export ("initWithID:attributeElementValue:")]
		NativeHandle Constructor (ushort attributeId, NSObject attributeElementValue);

		[Export ("initWithID:attributeElement:")]
		NativeHandle Constructor (ushort attributeId, IOBluetoothSDPDataElement attributeElement);

		[Export ("getAttributeID")]
		ushort GetAttributeId ();

		[Export ("getDataElement")]
		IOBluetoothSDPDataElement GetDataElement ();

		[Export ("getIDDataElement")]
		IOBluetoothSDPDataElement GetIdDataElement ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothSDPDataElement : NSCoding, NSSecureCoding {
		[Static]
		[Export ("withElementValue:")]
		[return: NullAllowed]
		IOBluetoothSDPDataElement Create (NSObject element);

		[Static]
		[Export ("withType:sizeDescriptor:size:value:")]
		[return: NullAllowed]
		IOBluetoothSDPDataElement Create (BluetoothSDPDataElementType type, byte sizeDescriptor, uint size, NSObject value);

		[Export ("initWithElementValue:")]
		NativeHandle Constructor (NSObject element);

		[Export ("initWithType:sizeDescriptor:size:value:")]
		NativeHandle Constructor (BluetoothSDPDataElementType type, byte sizeDescriptor, uint size, NSObject value);

		[Export ("getTypeDescriptor")]
		BluetoothSDPDataElementType GetTypeDescriptor ();

		[Export ("getSizeDescriptor")]
		byte GetSizeDescriptor ();

		[Export ("getSize")]
		uint GetSize ();

		[return: NullAllowed]
		[Export ("getNumberValue")]
		NSNumber GetNumberValue ();

		[return: NullAllowed]
		[Export ("getDataValue")]
		NSData GetDataValue ();

		[return: NullAllowed]
		[Export ("getStringValue")]
		string GetStringValue ();

		[return: NullAllowed]
		[Export ("getArrayValue")]
		NSArray GetArrayValue ();

		[return: NullAllowed]
		[Export ("getUUIDValue")]
		IOBluetoothSDPUUID GetUuidValue ();

		[return: NullAllowed]
		[Export ("getValue")]
		NSObject GetValue ();

		[Export ("containsDataElement:")]
		bool ContainsDataElement (IOBluetoothSDPDataElement dataElement);

		[Export ("containsValue:")]
		bool ContainsValue (NSObject value);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothSDPServiceRecord : NSCoding, NSSecureCoding {
		[Static]
		[Export ("publishedServiceRecordWithDictionary:")]
		[return: NullAllowed]
		IOBluetoothSDPServiceRecord PublishedServiceRecord (NSDictionary serviceDict);

		[Export ("removeServiceRecord")]
		int RemoveServiceRecord ();

		[Static]
		[Export ("withServiceDictionary:device:")]
		[return: NullAllowed]
		IOBluetoothSDPServiceRecord Create (NSDictionary serviceDict, [NullAllowed] IOBluetoothDevice device);

		[Export ("initWithServiceDictionary:device:")]
		NativeHandle Constructor (NSDictionary serviceDict, [NullAllowed] IOBluetoothDevice device);

		[NullAllowed, Export ("device", ArgumentSemantic.Retain)]
		IOBluetoothDevice Device { get; }

		[NullAllowed, Export ("attributes", ArgumentSemantic.Copy)]
		NSDictionary Attributes { get; }

		[return: NullAllowed]
		[Export ("getAttributeDataElement:")]
		IOBluetoothSDPDataElement GetAttributeDataElement (ushort attributeId);

		[return: NullAllowed]
		[Export ("getServiceName")]
		string GetServiceName ();

		[Export ("getRFCOMMChannelID:")]
		int GetRfcommChannelId (out byte rfcommChannelId);

		[Export ("getL2CAPPSM:")]
		int GetL2CapPsm (out ushort psm);

		[Export ("getServiceRecordHandle:")]
		int GetServiceRecordHandle (out uint serviceRecordHandle);

		[Export ("matchesUUID16:")]
		bool MatchesUuid16 (ushort uuid16);

		[Export ("matchesUUIDArray:")]
		bool MatchesUuidArray (NSArray uuidArray);

		[Export ("matchesSearchArray:")]
		bool MatchesSearchArray (NSArray searchArray);

		[Export ("hasServiceFromArray:")]
		bool HasServiceFromArray (NSArray array);

		[NullAllowed, Export ("sortedAttributes", ArgumentSemantic.Copy)]
		NSArray SortedAttributes { get; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (IOBluetoothObject))]
	[DisableDefaultCtor]
	interface IOBluetoothL2CAPChannel {
		[Static]
		[Export ("registerForChannelOpenNotifications:selector:")]
		IOBluetoothUserNotification RegisterForChannelOpenNotifications (NSObject target, Selector selector);

		[Static]
		[Export ("registerForChannelOpenNotifications:selector:withPSM:direction:")]
		IOBluetoothUserNotification RegisterForChannelOpenNotifications (NSObject target, Selector selector, ushort psm, IOBluetoothUserNotificationChannelDirection direction);

		[Static]
		[Export ("withObjectID:")]
		[return: NullAllowed]
		IOBluetoothL2CAPChannel FromObjectId (nuint objectId);

		[Export ("closeChannel")]
		int CloseChannel ();

		[Export ("outgoingMTU")]
		ushort OutgoingMtu { get; }

		[Export ("incomingMTU")]
		ushort IncomingMtu { get; }

		[Export ("requestRemoteMTU:")]
		int RequestRemoteMtu (ushort remoteMtu);

		[Export ("writeAsyncTrap:length:refcon:")]
		int WriteAsyncTrap (IntPtr data, ushort length, IntPtr refcon);

		[Export ("writeAsync:length:refcon:")]
		int WriteAsync (IntPtr data, ushort length, IntPtr refcon);

		[Export ("writeSync:length:")]
		int WriteSync (IntPtr data, ushort length);

		[Export ("setDelegate:")]
		int SetDelegate ([NullAllowed] NSObject channelDelegate);

		[Export ("setDelegate:withConfiguration:")]
		int SetDelegate ([NullAllowed] NSObject channelDelegate, [NullAllowed] NSDictionary channelConfiguration);

		[NullAllowed, Export ("delegate")]
		NSObject WeakDelegate { get; }

		[NullAllowed, Export ("device", ArgumentSemantic.Retain)]
		IOBluetoothDevice Device { get; }

		[Export ("objectID", ArgumentSemantic.Assign)]
		nuint ObjectId { get; }

		[Export ("PSM", ArgumentSemantic.Assign)]
		ushort Psm { get; }

		[Export ("localChannelID", ArgumentSemantic.Assign)]
		ushort LocalChannelId { get; }

		[Export ("remoteChannelID", ArgumentSemantic.Assign)]
		ushort RemoteChannelId { get; }

		[Export ("isIncoming")]
		bool IsIncoming { get; }

		[Export ("registerForChannelCloseNotification:selector:")]
		IOBluetoothUserNotification RegisterForChannelCloseNotification (NSObject observer, Selector selector);

		[Notification]
		[Field ("IOBluetoothL2CAPChannelPublishedNotification", "IOBluetooth")]
		NSString PublishedNotification { get; }

		[Notification]
		[Field ("IOBluetoothL2CAPChannelTerminatedNotification", "IOBluetooth")]
		NSString TerminatedNotification { get; }
	}

	interface IIOBluetoothL2CAPChannelDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothL2CAPChannelDelegate {
		[Export ("l2capChannelData:data:length:")]
		void ChannelData (IOBluetoothL2CAPChannel l2capChannel, IntPtr dataPointer, nuint dataLength);

		[Export ("l2capChannelOpenComplete:status:")]
		void ChannelOpenComplete (IOBluetoothL2CAPChannel l2capChannel, int error);

		[Export ("l2capChannelClosed:")]
		void ChannelClosed (IOBluetoothL2CAPChannel l2capChannel);

		[Export ("l2capChannelReconfigured:")]
		void ChannelReconfigured (IOBluetoothL2CAPChannel l2capChannel);

		[Export ("l2capChannelWriteComplete:refcon:status:")]
		void ChannelWriteComplete (IOBluetoothL2CAPChannel l2capChannel, IntPtr refcon, int error);

		[Export ("l2capChannelQueueSpaceAvailable:")]
		void ChannelQueueSpaceAvailable (IOBluetoothL2CAPChannel l2capChannel);
	}

	[NoMacCatalyst]
	[BaseType (typeof (IOBluetoothObject))]
	[DisableDefaultCtor]
	interface IOBluetoothRFCOMMChannel {
		[Static]
		[Export ("registerForChannelOpenNotifications:selector:")]
		IOBluetoothUserNotification RegisterForChannelOpenNotifications (NSObject target, Selector selector);

		[Static]
		[Export ("registerForChannelOpenNotifications:selector:withChannelID:direction:")]
		IOBluetoothUserNotification RegisterForChannelOpenNotifications (NSObject target, Selector selector, byte channelId, IOBluetoothUserNotificationChannelDirection direction);

		[Static]
		[Export ("withObjectID:")]
		[return: NullAllowed]
		IOBluetoothRFCOMMChannel FromObjectId (nuint objectId);

		[Export ("closeChannel")]
		int CloseChannel ();

		[Export ("isOpen")]
		bool IsOpen { get; }

		[Export ("getMTU")]
		ushort GetMtu ();

		[Export ("isTransmissionPaused")]
		bool IsTransmissionPaused { get; }

		[Export ("writeAsync:length:refcon:")]
		int WriteAsync (IntPtr data, ushort length, IntPtr refcon);

		[Export ("writeSync:length:")]
		int WriteSync (IntPtr data, ushort length);

		[Export ("setSerialParameters:dataBits:parity:stopBits:")]
		int SetSerialParameters (uint speed, byte dataBits, BluetoothRFCOMMParityType parity, byte stopBits);

		[Export ("sendRemoteLineStatus:")]
		int SendRemoteLineStatus (BluetoothRFCOMMLineStatus lineStatus);

		[Export ("setDelegate:")]
		int SetDelegate ([NullAllowed] NSObject channelDelegate);

		[NullAllowed, Export ("delegate")]
		NSObject WeakDelegate { get; }

		[Export ("getChannelID")]
		byte GetChannelId ();

		[Export ("isIncoming")]
		bool IsIncoming { get; }

		[return: NullAllowed]
		[Export ("getDevice")]
		IOBluetoothDevice GetDevice ();

		[Export ("getObjectID")]
		nuint GetObjectId ();

		[Export ("registerForChannelCloseNotification:selector:")]
		IOBluetoothUserNotification RegisterForChannelCloseNotification (NSObject observer, Selector selector);
	}

	interface IIOBluetoothRFCOMMChannelDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothRFCOMMChannelDelegate {
		[Export ("rfcommChannelData:data:length:")]
		void ChannelData (IOBluetoothRFCOMMChannel rfcommChannel, IntPtr dataPointer, nuint dataLength);

		[Export ("rfcommChannelOpenComplete:status:")]
		void ChannelOpenComplete (IOBluetoothRFCOMMChannel rfcommChannel, int error);

		[Export ("rfcommChannelClosed:")]
		void ChannelClosed (IOBluetoothRFCOMMChannel rfcommChannel);

		[Export ("rfcommChannelControlSignalsChanged:")]
		void ChannelControlSignalsChanged (IOBluetoothRFCOMMChannel rfcommChannel);

		[Export ("rfcommChannelFlowControlChanged:")]
		void ChannelFlowControlChanged (IOBluetoothRFCOMMChannel rfcommChannel);

		[Export ("rfcommChannelWriteComplete:refcon:status:")]
		void ChannelWriteComplete (IOBluetoothRFCOMMChannel rfcommChannel, IntPtr refcon, int error);

		[Export ("rfcommChannelWriteComplete:refcon:status:bytesWritten:")]
		void ChannelWriteComplete (IOBluetoothRFCOMMChannel rfcommChannel, IntPtr refcon, int error, nuint length);

		[Export ("rfcommChannelQueueSpaceAvailable:")]
		void ChannelQueueSpaceAvailable (IOBluetoothRFCOMMChannel rfcommChannel);
	}

	[NoMacCatalyst]
	[Static]
	interface IOBluetoothHandsFreeKeys {
		[Field ("IOBluetoothHandsFreeIndicatorService", "IOBluetooth")]
		NSString IndicatorService { get; }

		[Field ("IOBluetoothHandsFreeIndicatorCall", "IOBluetooth")]
		NSString IndicatorCall { get; }

		[Field ("IOBluetoothHandsFreeIndicatorCallSetup", "IOBluetooth")]
		NSString IndicatorCallSetup { get; }

		[Field ("IOBluetoothHandsFreeIndicatorCallHeld", "IOBluetooth")]
		NSString IndicatorCallHeld { get; }

		[Field ("IOBluetoothHandsFreeIndicatorSignal", "IOBluetooth")]
		NSString IndicatorSignal { get; }

		[Field ("IOBluetoothHandsFreeIndicatorRoam", "IOBluetooth")]
		NSString IndicatorRoam { get; }

		[Field ("IOBluetoothHandsFreeIndicatorBattChg", "IOBluetooth")]
		NSString IndicatorBattChg { get; }

		[Field ("IOBluetoothHandsFreeCallIndex", "IOBluetooth")]
		NSString CallIndex { get; }

		[Field ("IOBluetoothHandsFreeCallDirection", "IOBluetooth")]
		NSString CallDirection { get; }

		[Field ("IOBluetoothHandsFreeCallStatus", "IOBluetooth")]
		NSString CallStatus { get; }

		[Field ("IOBluetoothHandsFreeCallMode", "IOBluetooth")]
		NSString CallMode { get; }

		[Field ("IOBluetoothHandsFreeCallMultiparty", "IOBluetooth")]
		NSString CallMultiparty { get; }

		[Field ("IOBluetoothHandsFreeCallNumber", "IOBluetooth")]
		NSString CallNumber { get; }

		[Field ("IOBluetoothHandsFreeCallType", "IOBluetooth")]
		NSString CallType { get; }

		[Field ("IOBluetoothHandsFreeCallName", "IOBluetooth")]
		NSString CallName { get; }

		[Field ("IOBluetoothPDUServicCenterAddress", "IOBluetooth")]
		NSString PduServiceCenterAddress { get; }

		[Field ("IOBluetoothPDUServiceCenterAddressType", "IOBluetooth")]
		NSString PduServiceCenterAddressType { get; }

		[Field ("IOBluetoothPDUType", "IOBluetooth")]
		NSString PduType { get; }

		[Field ("IOBluetoothPDUOriginatingAddress", "IOBluetooth")]
		NSString PduOriginatingAddress { get; }

		[Field ("IOBluetoothPDUOriginatingAddressType", "IOBluetooth")]
		NSString PduOriginatingAddressType { get; }

		[Field ("IOBluetoothPDUProtocolID", "IOBluetooth")]
		NSString PduProtocolId { get; }

		[Field ("IOBluetoothPDUTimestamp", "IOBluetooth")]
		NSString PduTimestamp { get; }

		[Field ("IOBluetoothPDUEncoding", "IOBluetooth")]
		NSString PduEncoding { get; }

		[Field ("IOBluetoothPDUUserData", "IOBluetooth")]
		NSString PduUserData { get; }
	}

	interface IIOBluetoothHandsFreeDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothHandsFreeDelegate {
		[Export ("handsFree:connected:")]
		void Connected (IOBluetoothHandsFree device, NSNumber status);

		[Export ("handsFree:disconnected:")]
		void Disconnected (IOBluetoothHandsFree device, NSNumber status);

		[Export ("handsFree:scoConnectionOpened:")]
		void ScoConnectionOpened (IOBluetoothHandsFree device, NSNumber status);

		[Export ("handsFree:scoConnectionClosed:")]
		void ScoConnectionClosed (IOBluetoothHandsFree device, NSNumber status);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface IOBluetoothHandsFree {
		[Export ("supportedFeatures")]
		uint SupportedFeatures { get; set; }

		[Export ("inputVolume")]
		float InputVolume { get; set; }

		[Export ("inputMuted")]
		bool InputMuted { [Bind ("isInputMuted")] get; set; }

		[Export ("outputVolume")]
		float OutputVolume { get; set; }

		[Export ("outputMuted")]
		bool OutputMuted { [Bind ("isOutputMuted")] get; set; }

		[Export ("device", ArgumentSemantic.Retain)]
		IOBluetoothDevice Device { get; }

		[Export ("deviceSupportedFeatures")]
		uint DeviceSupportedFeatures { get; }

		[Export ("deviceSupportedSMSServices")]
		uint DeviceSupportedSmsServices { get; }

		[Export ("deviceCallHoldModes")]
		uint DeviceCallHoldModes { get; }

		[Export ("SMSMode")]
		IOBluetoothSMSMode SmsMode { get; }

		[Export ("SMSEnabled")]
		bool SmsEnabled { [Bind ("isSMSEnabled")] get; }

		[NullAllowed, Wrap ("WeakDelegate")]
		IIOBluetoothHandsFreeDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Export ("indicator:")]
		int GetIndicator (string indicatorName);

		[Export ("setIndicator:value:")]
		void SetIndicator (string indicatorName, int indicatorValue);

		[Export ("initWithDevice:delegate:")]
		NativeHandle Constructor (IOBluetoothDevice device, [NullAllowed] NSObject delegateObject);

		[Export ("connect")]
		void Connect ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get; }

		[Export ("connectSCO")]
		void ConnectSco ();

		[Export ("disconnectSCO")]
		void DisconnectSco ();

		[Export ("isSCOConnected")]
		bool IsScoConnected { get; }
	}

	interface IIOBluetoothHandsFreeAudioGatewayDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothHandsFreeAudioGatewayDelegate {
		[Export ("handsFree:hangup:")]
		void Hangup (IOBluetoothHandsFreeAudioGateway device, NSNumber hangup);

		[Export ("handsFree:redial:")]
		void Redial (IOBluetoothHandsFreeAudioGateway device, NSNumber redial);
	}

	[NoMacCatalyst]
	[BaseType (typeof (IOBluetoothHandsFree))]
	[DisableDefaultCtor]
	interface IOBluetoothHandsFreeAudioGateway {
		[Export ("initWithDevice:delegate:")]
		NativeHandle Constructor (IOBluetoothDevice device, [NullAllowed] NSObject delegateObject);

		[Export ("createIndicator:min:max:currentValue:")]
		void CreateIndicator (string indicatorName, int minValue, int maxValue, int currentValue);

		[Export ("processATCommand:")]
		void ProcessAtCommand (string atCommand);

		[Export ("sendOKResponse")]
		void SendOkResponse ();

		[Export ("sendResponse:")]
		void SendResponse (string response);

		[Export ("sendResponse:withOK:")]
		void SendResponse (string response, bool withOk);
	}

	interface IIOBluetoothHandsFreeDeviceDelegate { }

	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface IOBluetoothHandsFreeDeviceDelegate : IOBluetoothHandsFreeDelegate {
		[Export ("handsFree:isServiceAvailable:")]
		void IsServiceAvailable (IOBluetoothHandsFreeDevice device, NSNumber isServiceAvailable);

		[Export ("handsFree:isCallActive:")]
		void IsCallActive (IOBluetoothHandsFreeDevice device, NSNumber isCallActive);

		[Export ("handsFree:callSetupMode:")]
		void CallSetupMode (IOBluetoothHandsFreeDevice device, NSNumber callSetupMode);

		[Export ("handsFree:callHoldState:")]
		void CallHoldState (IOBluetoothHandsFreeDevice device, NSNumber callHoldState);

		[Export ("handsFree:signalStrength:")]
		void SignalStrength (IOBluetoothHandsFreeDevice device, NSNumber signalStrength);

		[Export ("handsFree:isRoaming:")]
		void IsRoaming (IOBluetoothHandsFreeDevice device, NSNumber isRoaming);

		[Export ("handsFree:batteryCharge:")]
		void BatteryCharge (IOBluetoothHandsFreeDevice device, NSNumber batteryCharge);

		[Export ("handsFree:incomingCallFrom:")]
		void IncomingCallFrom (IOBluetoothHandsFreeDevice device, string number);

		[Export ("handsFree:ringAttempt:")]
		void RingAttempt (IOBluetoothHandsFreeDevice device, NSNumber ringAttempt);

		[Export ("handsFree:currentCall:")]
		void CurrentCall (IOBluetoothHandsFreeDevice device, NSDictionary currentCall);

		[Export ("handsFree:subscriberNumber:")]
		void SubscriberNumber (IOBluetoothHandsFreeDevice device, string subscriberNumber);

		[Export ("handsFree:incomingSMS:")]
		void IncomingSms (IOBluetoothHandsFreeDevice device, NSDictionary sms);

		[Export ("handsFree:unhandledResultCode:")]
		void UnhandledResultCode (IOBluetoothHandsFreeDevice device, string resultCode);
	}

	[NoMacCatalyst]
	[BaseType (typeof (IOBluetoothHandsFree))]
	[DisableDefaultCtor]
	interface IOBluetoothHandsFreeDevice {
		[Export ("initWithDevice:delegate:")]
		NativeHandle Constructor (IOBluetoothDevice device, [NullAllowed] NSObject delegateObject);

		[Export ("dialNumber:")]
		void DialNumber (string number);

		[Export ("memoryDial:")]
		void MemoryDial (int memoryLocation);

		[Export ("redial")]
		void Redial ();

		[Export ("endCall")]
		void EndCall ();

		[Export ("acceptCall")]
		void AcceptCall ();

		[Export ("acceptCallOnPhone")]
		void AcceptCallOnPhone ();

		[Export ("sendDTMF:")]
		void SendDtmf (string character);

		[Export ("subscriberNumber")]
		void RequestSubscriberNumber ();

		[Export ("currentCallList")]
		void RequestCurrentCallList ();

		[Export ("releaseHeldCalls")]
		void ReleaseHeldCalls ();

		[Export ("releaseActiveCalls")]
		void ReleaseActiveCalls ();

		[Export ("releaseCall:")]
		void ReleaseCall (int index);

		[Export ("holdCall")]
		void HoldCall ();

		[Export ("placeAllOthersOnHold:")]
		void PlaceAllOthersOnHold (int index);

		[Export ("addHeldCall")]
		void AddHeldCall ();

		[Export ("callTransfer")]
		void CallTransfer ();

		[Export ("transferAudioToComputer")]
		void TransferAudioToComputer ();

		[Export ("transferAudioToPhone")]
		void TransferAudioToPhone ();

		[Export ("sendSMS:message:")]
		void SendSms (string number, string message);

		[Export ("sendATCommand:")]
		void SendAtCommand (string atCommand);

		[Export ("sendATCommand:timeout:selector:target:")]
		void SendAtCommand (string atCommand, float timeout, Selector selector, NSObject target);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface OBEXSession {
		[Export ("OBEXConnect:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexConnect (byte flags, ushort maxPacketLength, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXDisconnect:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexDisconnect (IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXPut:headersData:headersDataLength:bodyData:bodyDataLength:eventSelector:selectorTarget:refCon:")]
		int ObexPut (bool isFinalChunk, IntPtr headersData, nuint headersDataLength, IntPtr bodyData, nuint bodyDataLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXGet:headers:headersLength:eventSelector:selectorTarget:refCon:")]
		int ObexGet (bool isFinalChunk, IntPtr headers, nuint headersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXAbort:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexAbort (IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXSetPath:constants:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexSetPath (byte flags, byte constants, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXConnectResponse:flags:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexConnectResponse (byte responseOpCode, byte flags, ushort maxPacketLength, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXDisconnectResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexDisconnectResponse (byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXPutResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexPutResponse (byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXGetResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexGetResponse (byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXAbortResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexAbortResponse (byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("OBEXSetPathResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
		int ObexSetPathResponse (byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("getAvailableCommandPayloadLength:")]
		ushort GetAvailableCommandPayloadLength (byte opCode);

		[Export ("getAvailableCommandResponsePayloadLength:")]
		ushort GetAvailableCommandResponsePayloadLength (byte opCode);

		[Export ("getMaxPacketLength")]
		ushort GetMaxPacketLength ();

		[Export ("hasOpenOBEXConnection")]
		bool HasOpenObexConnection { get; }

		[Export ("setEventSelector:target:refCon:")]
		void SetEventSelector (Selector eventSelector, NSObject eventSelectorTarget, IntPtr userRefCon);

		// OBEXTransportEvent *
		[Export ("serverHandleIncomingData:")]
		void ServerHandleIncomingData (IntPtr transportEvent);

		// OBEXTransportEvent *
		[Export ("clientHandleIncomingData:")]
		void ClientHandleIncomingData (IntPtr transportEvent);

		[Export ("sendDataToTransport:dataLength:")]
		int SendDataToTransport (IntPtr dataToSend, nuint dataLength);

		[Export ("openTransportConnection:selectorTarget:refCon:")]
		int OpenTransportConnection (Selector selector, NSObject target, IntPtr userRefCon);

		[Export ("hasOpenTransportConnection")]
		bool HasOpenTransportConnection { get; }

		[Export ("closeTransportConnection")]
		int CloseTransportConnection ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (OBEXSession))]
	[DisableDefaultCtor]
	interface IOBluetoothOBEXSession : IOBluetoothRFCOMMChannelDelegate {
		[Static]
		[Export ("withSDPServiceRecord:")]
		[return: NullAllowed]
		IOBluetoothOBEXSession Create (IOBluetoothSDPServiceRecord serviceRecord);

		[Static]
		[Export ("withDevice:channelID:")]
		[return: NullAllowed]
		IOBluetoothOBEXSession Create (IOBluetoothDevice device, byte channelId);

		[Static]
		[Export ("withIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
		[return: NullAllowed]
		IOBluetoothOBEXSession Create (IOBluetoothRFCOMMChannel channel, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[Export ("initWithSDPServiceRecord:")]
		NativeHandle Constructor (IOBluetoothSDPServiceRecord serviceRecord);

		[Export ("initWithDevice:channelID:")]
		NativeHandle Constructor (IOBluetoothDevice device, byte channelId);

		[Export ("initWithIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
		NativeHandle Constructor (IOBluetoothRFCOMMChannel channel, Selector eventSelector, NSObject selectorTarget, IntPtr userRefCon);

		[return: NullAllowed]
		[Export ("getRFCOMMChannel")]
		IOBluetoothRFCOMMChannel GetRfcommChannel ();

		[return: NullAllowed]
		[Export ("getDevice")]
		IOBluetoothDevice GetDevice ();

		[Export ("sendBufferTroughChannel")]
		int SendBufferThroughChannel ();

		[Export ("restartTransmission")]
		void RestartTransmission ();

		[Export ("isSessionTargetAMac")]
		bool IsSessionTargetAMac { get; }
	}

	[NoMacCatalyst]
	[Static]
	interface OBEXFileTransferServicesKeys {
		[Field ("kFTSProgressBytesTransferredKey", "IOBluetooth")]
		NSString ProgressBytesTransferred { get; }

		[Field ("kFTSProgressBytesTotalKey", "IOBluetooth")]
		NSString ProgressBytesTotal { get; }

		[Field ("kFTSProgressPercentageKey", "IOBluetooth")]
		NSString ProgressPercentage { get; }

		[Field ("kFTSProgressEstimatedTimeKey", "IOBluetooth")]
		NSString ProgressEstimatedTime { get; }

		[Field ("kFTSProgressTimeElapsedKey", "IOBluetooth")]
		NSString ProgressTimeElapsed { get; }

		[Field ("kFTSProgressTransferRateKey", "IOBluetooth")]
		NSString ProgressTransferRate { get; }

		[Field ("kFTSListingNameKey", "IOBluetooth")]
		NSString ListingName { get; }

		[Field ("kFTSListingTypeKey", "IOBluetooth")]
		NSString ListingType { get; }

		[Field ("kFTSListingSizeKey", "IOBluetooth")]
		NSString ListingSize { get; }
	}

	[NoMacCatalyst]
	[Model]
	[BaseType (typeof (NSObject))]
	interface OBEXFileTransferServicesDelegate {
		[Export ("fileTransferServicesConnectionComplete:error:")]
		void ConnectionComplete (OBEXFileTransferServices services, int error);

		[Export ("fileTransferServicesDisconnectionComplete:error:")]
		void DisconnectionComplete (OBEXFileTransferServices services, int error);

		[Export ("fileTransferServicesAbortComplete:error:")]
		void AbortComplete (OBEXFileTransferServices services, int error);

		[Export ("fileTransferServicesRemoveItemComplete:error:removedItem:")]
		void RemoveItemComplete (OBEXFileTransferServices services, int error, string itemName);

		[Export ("fileTransferServicesCreateFolderComplete:error:folder:")]
		void CreateFolderComplete (OBEXFileTransferServices services, int error, string folderName);

		[Export ("fileTransferServicesPathChangeComplete:error:finalPath:")]
		void PathChangeComplete (OBEXFileTransferServices services, int error, string path);

		[Export ("fileTransferServicesRetrieveFolderListingComplete:error:listing:")]
		void RetrieveFolderListingComplete (OBEXFileTransferServices services, int error, NSArray listing);

		[Export ("fileTransferServicesFilePreparationComplete:error:")]
		void FilePreparationComplete (OBEXFileTransferServices services, int error);

		[Export ("fileTransferServicesSendFileProgress:transferProgress:")]
		void SendFileProgress (OBEXFileTransferServices services, NSDictionary progressDescription);

		[Export ("fileTransferServicesSendFileComplete:error:")]
		void SendFileComplete (OBEXFileTransferServices services, int error);

		[Export ("fileTransferServicesCopyRemoteFileProgress:transferProgress:")]
		void CopyRemoteFileProgress (OBEXFileTransferServices services, NSDictionary progressDescription);

		[Export ("fileTransferServicesCopyRemoteFileComplete:error:")]
		void CopyRemoteFileComplete (OBEXFileTransferServices services, int error);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface OBEXFileTransferServices {
		[NullAllowed, Wrap ("WeakDelegate")]
		OBEXFileTransferServicesDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Static]
		[Export ("withOBEXSession:")]
		[return: NullAllowed]
		OBEXFileTransferServices Create (IOBluetoothOBEXSession session);

		[Export ("initWithOBEXSession:")]
		NativeHandle Constructor (IOBluetoothOBEXSession session);

		[Export ("currentPath")]
		string CurrentPath { get; }

		[Export ("isBusy")]
		bool IsBusy { get; }

		[Export ("isConnected")]
		bool IsConnected { get; }

		[Export ("connectToFTPService")]
		int ConnectToFtpService ();

		[Export ("connectToObjectPushService")]
		int ConnectToObjectPushService ();

		[Export ("disconnect")]
		int Disconnect ();

		[Export ("changeCurrentFolderToRoot")]
		int ChangeCurrentFolderToRoot ();

		[Export ("changeCurrentFolderBackward")]
		int ChangeCurrentFolderBackward ();

		[Export ("changeCurrentFolderForwardToPath:")]
		int ChangeCurrentFolderForward (string dirName);

		[Export ("createFolder:")]
		int CreateFolder (string dirName);

		[Export ("removeItem:")]
		int RemoveItem (string itemName);

		[Export ("retrieveFolderListing")]
		int RetrieveFolderListing ();

		[Export ("sendFile:")]
		int SendFile (string localPathAndName);

		[Export ("copyRemoteFile:toLocalPath:")]
		int CopyRemoteFile (string remoteFileName, string localPathAndName);

		[Export ("sendData:type:name:")]
		int SendData (NSData data, string type, string name);

		[Export ("getDefaultVCard:")]
		int GetDefaultVCard (string localPathAndName);

		[Export ("abort")]
		int Abort ();
	}

	[NoMacCatalyst]
	[Category]
	[BaseType (typeof (NSMutableDictionary))]
	interface NSMutableDictionary_NSDictionaryOBEXExtensions {
		[Export ("getHeaderBytes")]
		NSMutableData GetObexHeaderBytes ();

		[Export ("addTargetHeader:length:")]
		int AddObexTargetHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addHTTPHeader:length:")]
		int AddObexHttpHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addBodyHeader:length:endOfBody:")]
		int AddObexBodyHeader (IntPtr headerData, uint headerDataLength, bool isEndOfBody);

		[Export ("addWhoHeader:length:")]
		int AddObexWhoHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addConnectionIDHeader:length:")]
		int AddObexConnectionIdHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addApplicationParameterHeader:length:")]
		int AddObexApplicationParameterHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addByteSequenceHeader:length:")]
		int AddObexByteSequenceHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addObjectClassHeader:length:")]
		int AddObexObjectClassHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addAuthorizationChallengeHeader:length:")]
		int AddObexAuthorizationChallengeHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addAuthorizationResponseHeader:length:")]
		int AddObexAuthorizationResponseHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addTimeISOHeader:length:")]
		int AddObexTimeIsoHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addTypeHeader:")]
		int AddObexTypeHeader (string type);

		[Export ("addLengthHeader:")]
		int AddObexLengthHeader (uint length);

		[Export ("addTime4ByteHeader:")]
		int AddObexTime4ByteHeader (uint time4Byte);

		[Export ("addCountHeader:")]
		int AddObexCountHeader (uint count);

		[Export ("addDescriptionHeader:")]
		int AddObexDescriptionHeader (string descriptionString);

		[Export ("addNameHeader:")]
		int AddObexNameHeader (string nameString);

		[Export ("addUserDefinedHeader:length:")]
		int AddObexUserDefinedHeader (IntPtr headerData, uint headerDataLength);

		[Export ("addImageHandleHeader:")]
		int AddObexImageHandleHeader (string type);

		[Export ("addImageDescriptorHeader:length:")]
		int AddObexImageDescriptorHeader (IntPtr headerData, uint headerDataLength);
	}
}
