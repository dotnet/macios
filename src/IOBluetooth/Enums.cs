// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using ObjCRuntime;

namespace IOBluetooth {

	/// <summary>The power state of a Bluetooth host controller.</summary>
	[NoMacCatalyst]
	public enum BluetoothHCIPowerState : uint {
		/// <summary>The controller is powered off.</summary>
		Off = 0x00,
		/// <summary>The controller is powered on.</summary>
		On = 0x01,
		/// <summary>The power state is not known.</summary>
		Uninitialized = 0xFF,
	}

	/// <summary>The parity used for an RFCOMM serial connection.</summary>
	[NoMacCatalyst]
	public enum BluetoothRFCOMMParityType : uint {
		/// <summary>No parity.</summary>
		NoParity = 0,
		/// <summary>Odd parity.</summary>
		OddParity = 1,
		/// <summary>Even parity.</summary>
		EvenParity = 2,
		/// <summary>The maximum value for the parity type.</summary>
		MaxParity = 3,
	}

	/// <summary>The line status values that can be sent over an RFCOMM channel.</summary>
	[NoMacCatalyst]
	public enum BluetoothRFCOMMLineStatus : uint {
		/// <summary>There is no error.</summary>
		NoError = 0,
		/// <summary>An overrun error occurred.</summary>
		OverrunError = 1,
		/// <summary>A parity error occurred.</summary>
		ParityError = 2,
		/// <summary>A framing error occurred.</summary>
		FramingError = 3,
	}

	/// <summary>The direction of a channel, used when registering for channel notifications.</summary>
	[NoMacCatalyst]
	public enum IOBluetoothUserNotificationChannelDirection : uint {
		/// <summary>Any direction.</summary>
		Any = 0,
		/// <summary>Incoming channels.</summary>
		Incoming = 1,
		/// <summary>Outgoing channels.</summary>
		Outgoing = 2,
	}

	/// <summary>The kinds of devices to search for.</summary>
	[NoMacCatalyst]
	[Flags]
	public enum IOBluetoothDeviceSearchTypes : uint {
		/// <summary>Search for classic Bluetooth devices.</summary>
		Classic = 1,
		/// <summary>Search for Bluetooth Low Energy devices.</summary>
		LowEnergy = 2,
	}

	/// <summary>The data type of an SDP data element.</summary>
	[NoMacCatalyst]
	public enum BluetoothSDPDataElementType : byte {
		/// <summary>The nil type.</summary>
		Nil = 0,
		/// <summary>An unsigned integer.</summary>
		UnsignedInt = 1,
		/// <summary>A signed integer.</summary>
		SignedInt = 2,
		/// <summary>A UUID.</summary>
		UUID = 3,
		/// <summary>A string.</summary>
		String = 4,
		/// <summary>A boolean.</summary>
		Boolean = 5,
		/// <summary>A sequence of data elements.</summary>
		DataElementSequence = 6,
		/// <summary>An alternative of data elements.</summary>
		DataElementAlternative = 7,
		/// <summary>A URL.</summary>
		URL = 8,
	}

	/// <summary>Features supported by a hands-free device.</summary>
	[NoMacCatalyst]
	[Flags]
	public enum IOBluetoothHandsFreeDeviceFeatures : uint {
		/// <summary>No features.</summary>
		None = 0,
		/// <summary>Echo cancelling and/or noise reduction.</summary>
		EchoCancellingAndOrNoiseReduction = 1 << 0,
		/// <summary>Three-way calling.</summary>
		ThreeWayCalling = 1 << 1,
		/// <summary>Calling line identification presentation.</summary>
		CallingLineIdentificationPresentation = 1 << 2,
		/// <summary>Voice recognition.</summary>
		VoiceRecognition = 1 << 3,
		/// <summary>Remote volume control.</summary>
		RemoteVolumeControl = 1 << 4,
		/// <summary>Enhanced call status.</summary>
		EnhancedCallStatus = 1 << 5,
		/// <summary>Enhanced call control.</summary>
		EnhancedCallControl = 1 << 6,
		/// <summary>Codec negotiation.</summary>
		CodecNegotiation = 1 << 7,
	}

	/// <summary>Features supported by a hands-free audio gateway.</summary>
	[NoMacCatalyst]
	[Flags]
	public enum IOBluetoothHandsFreeAudioGatewayFeatures : uint {
		/// <summary>No features.</summary>
		None = 0,
		/// <summary>Three-way calling.</summary>
		ThreeWayCalling = 1 << 0,
		/// <summary>Echo cancelling and/or noise reduction.</summary>
		EchoCancellingAndOrNoiseReduction = 1 << 1,
		/// <summary>Voice recognition.</summary>
		VoiceRecognition = 1 << 2,
		/// <summary>In-band ring tone.</summary>
		InBandRingTone = 1 << 3,
		/// <summary>Attach a number to a voice tag.</summary>
		AttachedNumberToVoiceTag = 1 << 4,
		/// <summary>Ability to reject a call.</summary>
		RejectCallCapability = 1 << 5,
		/// <summary>Enhanced call status.</summary>
		EnhancedCallStatus = 1 << 6,
		/// <summary>Enhanced call control.</summary>
		EnhancedCallControl = 1 << 7,
		/// <summary>Extended error result codes.</summary>
		ExtendedErrorResultCodes = 1 << 8,
		/// <summary>Codec negotiation.</summary>
		CodecNegotiation = 1 << 9,
	}

	/// <summary>The call hold modes supported by a hands-free device.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum IOBluetoothHandsFreeCallHoldModes : ulong {
		/// <summary>Release all held calls.</summary>
		Mode0 = 1 << 0,
		/// <summary>Release all active calls and accept the other call.</summary>
		Mode1 = 1 << 1,
		/// <summary>Release a specific active call.</summary>
		Mode1Index = 1 << 2,
		/// <summary>Place all active calls on hold and accept the other call.</summary>
		Mode2 = 1 << 3,
		/// <summary>Place all active calls except a specific call on hold.</summary>
		Mode2Index = 1 << 4,
		/// <summary>Add a held call to the conversation.</summary>
		Mode3 = 1 << 5,
		/// <summary>Connect the two calls and disconnect the subscriber.</summary>
		Mode4 = 1 << 6,
	}

	/// <summary>The audio codecs a hands-free connection can use.</summary>
	[NoMacCatalyst]
	public enum IOBluetoothHandsFreeCodecID : byte {
		/// <summary>The CVSD codec.</summary>
		CVSD = 0x01,
		/// <summary>The mSBC codec.</summary>
		MSBC = 0x02,
		/// <summary>The AAC-ELD codec.</summary>
		AacEld = 0x80,
	}

	/// <summary>The SMS modes of a hands-free connection.</summary>
	[NoMacCatalyst]
	[Native]
	public enum IOBluetoothSMSMode : ulong {
		/// <summary>PDU mode.</summary>
		PDU = 0,
		/// <summary>Text mode.</summary>
		Text = 1,
	}

	/// <summary>The SMS services supported by a hands-free device.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum IOBluetoothHandsFreeSMSSupport : ulong {
		/// <summary>GSM 07.05 phase 2 SMS support.</summary>
		Phase2 = 1 << 0,
		/// <summary>GSM 07.05 phase 2+ SMS support.</summary>
		Phase2Plus = 1 << 1,
		/// <summary>Manufacturer-specific SMS support.</summary>
		ManufactureSpecific = 1 << 2,
	}

	/// <summary>The status of an SMS message in PDU mode.</summary>
	[NoMacCatalyst]
	[Native]
	public enum IOBluetoothHandsFreePDUMessageStatus : ulong {
		/// <summary>Received, unread.</summary>
		ReceivedUnread = 0,
		/// <summary>Received, read.</summary>
		ReceivedRead = 1,
		/// <summary>Stored, unsent.</summary>
		StoredUnsent = 2,
		/// <summary>Stored, sent.</summary>
		StoredSent = 3,
		/// <summary>All messages.</summary>
		All = 4,
	}

	/// <summary>The type of an item in a file transfer listing.</summary>
	[NoMacCatalyst]
	public enum FTSFileType {
		/// <summary>A folder.</summary>
		Folder = 1,
		/// <summary>A file.</summary>
		File = 2,
	}
}
