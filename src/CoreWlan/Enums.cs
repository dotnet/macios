// Copyright 2014 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using System;

namespace CoreWlan {

	[NoMacCatalyst]
	[Native]
	[ErrorDomain ("CWErrorDomain")] // enum named `CWErr` in headers
	public enum CWStatus : long {
		/// <summary>To be added.</summary>
		Ok = 0,
		/// <summary>To be added.</summary>
		EAPOL = 1,
		/// <summary>To be added.</summary>
		InvalidParameter = -3900,
		/// <summary>To be added.</summary>
		NoMemory = -3901,
		/// <summary>To be added.</summary>
		Unknown = -3902,
		/// <summary>To be added.</summary>
		NotSupported = -3903,
		/// <summary>To be added.</summary>
		InvalidFormat = -3904,
		/// <summary>To be added.</summary>
		Timeout = -3905,
		/// <summary>To be added.</summary>
		UnspecifiedFailure = -3906,
		/// <summary>To be added.</summary>
		UnsupportedCapabilities = -3907,
		/// <summary>To be added.</summary>
		ReassociationDenied = -3908,
		/// <summary>To be added.</summary>
		AssociationDenied = -3909,
		/// <summary>To be added.</summary>
		AuthenticationAlgorithmUnsupported = -3910,
		/// <summary>To be added.</summary>
		InvalidAuthenticationSequenceNumber = -3911,
		/// <summary>To be added.</summary>
		ChallengeFailure = -3912,
		/// <summary>To be added.</summary>
		APFull = -3913,
		/// <summary>To be added.</summary>
		UnsupportedRateSet = -3914,
		/// <summary>To be added.</summary>
		ShortSlotUnsupported = -3915,
		/// <summary>To be added.</summary>
		DSSSOFDMUnsupported = -3916,
		/// <summary>To be added.</summary>
		InvalidInformationElement = -3917,
		/// <summary>To be added.</summary>
		InvalidGroupCipher = -3918,
		/// <summary>To be added.</summary>
		InvalidPairwiseCipher = -3919,
		/// <summary>To be added.</summary>
		InvalidAKMP = -3920,
		/// <summary>To be added.</summary>
		UnsupportedRSNVersion = -3921,
		/// <summary>To be added.</summary>
		InvalidRSNCapabilities = -3922,
		/// <summary>To be added.</summary>
		CipherSuiteRejected = -3923,
		/// <summary>To be added.</summary>
		InvalidPMK = -3924,
		/// <summary>To be added.</summary>
		SupplicantTimeout = -3925,
		/// <summary>To be added.</summary>
		HTFeaturesNotSupported = -3926,
		/// <summary>To be added.</summary>
		PCOTransitionTimeNotSupported = -3927,
		/// <summary>To be added.</summary>
		ReferenceNotBound = -3928,
		/// <summary>To be added.</summary>
		IPCFailure = -3929,
		/// <summary>To be added.</summary>
		OperationNotPermitted = -3930,
		/// <summary>To be added.</summary>
		Status = -3931,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWPhyMode : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		A = 1,
		/// <summary>To be added.</summary>
		B = 2,
		/// <summary>To be added.</summary>
		G = 3,
		/// <summary>To be added.</summary>
		N = 4,
		/// <summary>To be added.</summary>
		AC = 5,
		AX = 6,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWInterfaceMode : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Station = 1,
		/// <summary>To be added.</summary>
		Ibss = 2,
		/// <summary>To be added.</summary>
		HostAP = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWSecurity : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		WEP = 1,
		/// <summary>To be added.</summary>
		WPAPersonal = 2,
		/// <summary>To be added.</summary>
		WPAPersonalMixed = 3,
		/// <summary>To be added.</summary>
		WPA2Personal = 4,
		/// <summary>To be added.</summary>
		Personal = 5,
		/// <summary>To be added.</summary>
		DynamicWEP = 6,
		/// <summary>To be added.</summary>
		WPAEnterprise = 7,
		/// <summary>To be added.</summary>
		WPAEnterpriseMixed = 8,
		/// <summary>To be added.</summary>
		WPA2Enterprise = 9,
		/// <summary>To be added.</summary>
		Enterprise = 10,
		Wpa3Personal = 11,
		Wpa3Enterprise = 12,
		Wpa3Transition = 13,
		[Mac (13, 0)]
		Owe = 14,
		[Mac (13, 0)]
		OweTransition = 15,
		/// <summary>To be added.</summary>
		Unknown = long.MaxValue,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[Native]
	public enum CWIbssModeSecurity : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		WEP40 = 1,
		/// <summary>To be added.</summary>
		WEP104 = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWChannelWidth : ulong {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		TwentyMHz = 1,
		/// <summary>To be added.</summary>
		FourtyMHz = 2,
		/// <summary>To be added.</summary>
		EightyMHz = 3,
		/// <summary>To be added.</summary>
		OneHundredSixtyMHz = 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWChannelBand : ulong {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		TwoGHz = 1,
		/// <summary>To be added.</summary>
		FiveGHz = 2,
		SixGHz = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWCipherKeyFlags : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Unicast = 1 << 1,
		/// <summary>To be added.</summary>
		Multicast = 1 << 2,
		/// <summary>To be added.</summary>
		Tx = 1 << 3,
		/// <summary>To be added.</summary>
		Rx = 1 << 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWKeychainDomain : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		User = 1,
		/// <summary>To be added.</summary>
		System = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum CWEventType : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		PowerDidChange = 1,
		/// <summary>To be added.</summary>
		SsidDidChange = 2,
		/// <summary>To be added.</summary>
		BssidDidChange = 3,
		/// <summary>To be added.</summary>
		CountryCodeDidChange = 4,
		/// <summary>To be added.</summary>
		LinkDidChange = 5,
		/// <summary>To be added.</summary>
		LinkQualityDidChange = 6,
		/// <summary>To be added.</summary>
		ModeDidChange = 7,
		/// <summary>To be added.</summary>
		ScanCacheUpdated = 8,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0)]
		VirtualInterfaceStateChanged = 9,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0)]
		RangingReportEvent = 10,
		/// <summary>To be added.</summary>
		Unknown = long.MaxValue,
	}
}
