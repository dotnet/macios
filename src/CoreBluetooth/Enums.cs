//
// Enums.cs: Enums definitions for CoreBluetooth
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2011-2014 Xamarin Inc
//

using System;
using ObjCRuntime;

#nullable enable

namespace CoreBluetooth {

	[MacCatalyst (13, 1)]
	[Native]
	public enum CBManagerState : long {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Resetting,
		/// <summary>To be added.</summary>
		Unsupported,
		/// <summary>To be added.</summary>
		Unauthorized,
		/// <summary>To be added.</summary>
		PoweredOff,
		/// <summary>To be added.</summary>
		PoweredOn,
	}

	// NSInteger -> CBCentralManager.h
	/// <summary>Enumerates possible states of a <see cref="CoreBluetooth.CBCentralManager" />.</summary>
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CBManagerState' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CBManagerState' instead.")]
	[Native]
	public enum CBCentralManagerState : long {
		/// <summary>To be added.</summary>
		Unknown = CBManagerState.Unknown,
		/// <summary>To be added.</summary>
		Resetting = CBManagerState.Resetting,
		/// <summary>To be added.</summary>
		Unsupported = CBManagerState.Unsupported,
		/// <summary>To be added.</summary>
		Unauthorized = CBManagerState.Unauthorized,
		/// <summary>To be added.</summary>
		PoweredOff = CBManagerState.PoweredOff,
		/// <summary>To be added.</summary>
		PoweredOn = CBManagerState.PoweredOn,
	}

	// NSInteger -> CBPeripheralManager.h
	/// <summary>Enumerates the possible states of the <see cref="CoreBluetooth.CBPeripheralManager" />.</summary>
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CBManagerState' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CBManagerState' instead.")]
	[Native]
	public enum CBPeripheralManagerState : long {
		/// <summary>To be added.</summary>
		Unknown = CBManagerState.Unknown,
		/// <summary>To be added.</summary>
		Resetting = CBManagerState.Resetting,
		/// <summary>To be added.</summary>
		Unsupported = CBManagerState.Unsupported,
		/// <summary>To be added.</summary>
		Unauthorized = CBManagerState.Unauthorized,
		/// <summary>To be added.</summary>
		PoweredOff = CBManagerState.PoweredOff,
		/// <summary>To be added.</summary>
		PoweredOn = CBManagerState.PoweredOn,
	}

	// NSInteger -> CBPeripheralManager.h
	/// <summary>Enumerates the possible connection states of a <see cref="CoreBluetooth.CBPeripheral" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBPeripheralState : long {
		/// <summary>Indicates that the peripheral is not connected.</summary>
		Disconnected,
		/// <summary>Indicates that the peripheral is connecting.</summary>
		Connecting,
		/// <summary>Indicates that the peripheral is connected.</summary>
		Connected,
		/// <summary>Indicates that the peripheral is disconnecting.</summary>
		[MacCatalyst (13, 1)]
		Disconnecting,
	}

	// NSUInteger -> CBCharacteristic.h
	/// <summary>The possible properties of a characteristic. A characteristic may have multiple properties.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum CBCharacteristicProperties : ulong {
		/// <summary>To be added.</summary>
		Broadcast = 1,
		/// <summary>To be added.</summary>
		Read = 2,
		/// <summary>To be added.</summary>
		WriteWithoutResponse = 4,
		/// <summary>To be added.</summary>
		Write = 8,
		/// <summary>To be added.</summary>
		Notify = 16,
		/// <summary>To be added.</summary>
		Indicate = 32,
		/// <summary>To be added.</summary>
		AuthenticatedSignedWrites = 64,
		/// <summary>To be added.</summary>
		ExtendedProperties = 128,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		NotifyEncryptionRequired = 0x100,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		IndicateEncryptionRequired = 0x200,
	}

	/// <summary>Errors possible during Bluetooth LE transactions.</summary>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("CBErrorDomain")]
	[Native] // NSInteger -> CBError.h
	public enum CBError : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		InvalidParameters,
		/// <summary>To be added.</summary>
		InvalidHandle,
		/// <summary>To be added.</summary>
		NotConnected,
		/// <summary>To be added.</summary>
		OutOfSpace,
		/// <summary>To be added.</summary>
		OperationCancelled,
		/// <summary>To be added.</summary>
		ConnectionTimeout,
		/// <summary>To be added.</summary>
		PeripheralDisconnected,
		/// <summary>To be added.</summary>
		UUIDNotAllowed,
		/// <summary>To be added.</summary>
		AlreadyAdvertising,
		/// <summary>To be added.</summary>
		ConnectionFailed,
		/// <summary>To be added.</summary>
		ConnectionLimitReached,
		/// <summary>To be added.</summary>
		UnknownDevice,
		/// <summary>To be added.</summary>
		OperationNotSupported,
		PeerRemovedPairingInformation,
		EncryptionTimedOut,
		TooManyLEPairedDevices = 16,
		LEGattExceededBackgroundNotificationLimit = 17,
		LEGattNearBackgroundNotificationLimit = 18,
	}

	/// <summary>Errors returned by a GATT server.</summary>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("CBATTErrorDomain")]
	[Native] // NSInteger -> CBError.h
	public enum CBATTError : long {
		/// <summary>To be added.</summary>
		Success = 0,
		/// <summary>To be added.</summary>
		InvalidHandle,
		/// <summary>To be added.</summary>
		ReadNotPermitted,
		/// <summary>To be added.</summary>
		WriteNotPermitted,
		/// <summary>To be added.</summary>
		InvalidPdu,
		/// <summary>To be added.</summary>
		InsufficientAuthentication,
		/// <summary>To be added.</summary>
		RequestNotSupported,
		/// <summary>To be added.</summary>
		InvalidOffset,
		/// <summary>To be added.</summary>
		InsufficientAuthorization,
		/// <summary>To be added.</summary>
		PrepareQueueFull,
		/// <summary>To be added.</summary>
		AttributeNotFound,
		/// <summary>To be added.</summary>
		AttributeNotLong,
		/// <summary>To be added.</summary>
		InsufficientEncryptionKeySize,
		/// <summary>To be added.</summary>
		InvalidAttributeValueLength,
		/// <summary>To be added.</summary>
		UnlikelyError,
		/// <summary>To be added.</summary>
		InsufficientEncryption,
		/// <summary>To be added.</summary>
		UnsupportedGroupType,
		/// <summary>To be added.</summary>
		InsufficientResources,
	}

	// NSInteger -> CBPeripheral.h
	/// <summary>Enumerates the possible types of writes to a characteristic's value.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBCharacteristicWriteType : long {
		/// <summary>To be added.</summary>
		WithResponse,
		/// <summary>To be added.</summary>
		WithoutResponse,
	}

	// NSUInteger -> CBCharacteristic.h
	/// <summary>Enumerates the read, write, and encryption permissions for a characteristic's values.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum CBAttributePermissions : ulong {
		/// <summary>To be added.</summary>
		Readable = 1,
		/// <summary>To be added.</summary>
		Writeable = 1 << 1,
		/// <summary>To be added.</summary>
		ReadEncryptionRequired = 1 << 2,
		/// <summary>To be added.</summary>
		WriteEncryptionRequired = 1 << 3,
	}

	// NSInteger -> CBPeripheralManager.h
	/// <summary>The connection latency of the <see cref="CoreBluetooth.CBPeripheralManager" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBPeripheralManagerConnectionLatency : long {
		/// <summary>To be added.</summary>
		Low = 0,
		/// <summary>To be added.</summary>
		Medium,
		/// <summary>To be added.</summary>
		High,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBConnectionEvent : long {
		Disconnected = 0,
		Connected = 1,
	}

	[Flags, iOS (13, 0), TV (13, 0), NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBCentralManagerFeature : ulong {
		ExtendedScanAndConnect = 1uL << 0,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum CBManagerAuthorization : long {
		NotDetermined = 0,
		Restricted,
		Denied,
		AllowedAlways,
	}
}
