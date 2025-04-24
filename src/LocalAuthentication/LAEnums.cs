using System;
using ObjCRuntime;
using Foundation;

namespace LocalAuthentication {

	/// <summary>Authentication policies.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LAPolicy : long {
		/// <summary>Use the TouchID sensor to authenticate the user.</summary>
		[MacCatalyst (13, 1)]
		DeviceOwnerAuthenticationWithBiometrics = 1,
		/// <summary>Use the Touch ID sensor or the device password to authenticate the user.</summary>
		DeviceOwnerAuthentication = 2,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'DeviceOwnerAuthenticationWithCompanion' instead.")]
		[NoiOS]
		[NoMacCatalyst]
		DeviceOwnerAuthenticationWithWatch = 3,
		[NoTV, MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		DeviceOwnerAuthenticationWithCompanion = 3,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'DeviceOwnerAuthenticationWithBiometricsOrCompanion' instead.")]
		[NoiOS]
		[NoMacCatalyst]
		DeviceOwnerAuthenticationWithBiometricsOrWatch = 4,
		[NoTV, MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		DeviceOwnerAuthenticationWithBiometricsOrCompanion = 4,
		[Obsolete ("Use DeviceOwnerAuthenticationWithBiometricsOrWatch enum value instead.")]
		[NoiOS]
		[NoMacCatalyst]
		OwnerAuthenticationWithBiometricsOrWatch = DeviceOwnerAuthenticationWithBiometricsOrWatch,
		[NoMac, NoiOS, NoMacCatalyst]
		DeviceOwnerAuthenticationWithWristDetection = 5,
	}

	/// <summary>Status and error codes returned by methods in LocalAuthentication.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native ("LAError")]
	[ErrorDomain ("LAErrorDomain")]
	public enum LAStatus : long {
		/// <summary>Authentication succeeded.</summary>
		Success = 0,
		/// <summary>Authentication was not successful, because user failed to provide valid credentials.</summary>
		AuthenticationFailed = -1,
		/// <summary>Authentication was canceled by user (e.g. tapped Cancel button).</summary>
		UserCancel = -2,
		/// <summary>Authentication was canceled, because the user tapped the fallback button (Enter Password).</summary>
		UserFallback = -3,
		/// <summary>Authentication was canceled by system (e.g. another application went to foreground).</summary>
		SystemCancel = -4,
		/// <summary>Authentication could not start, because passcode is not set on the device.</summary>
		PasscodeNotSet = -5,

#if !NET
		/// Authentication could not start, because Touch ID is not available on the device.
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryNotAvailable' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryNotAvailable' instead.")]
		TouchIDNotAvailable = BiometryNotAvailable,

		/// Authentication could not start, because Touch ID has no enrolled fingers.
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryNotEnrolled' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryNotEnrolled' instead.")]
		TouchIDNotEnrolled = BiometryNotEnrolled,

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryLockout' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryLockout' instead.")]
		TouchIDLockout = BiometryLockout,
#endif
		/// <summary>To be added.</summary>
		AppCancel = -9,
		/// <summary>To be added.</summary>
		InvalidContext = -10,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'CompanionNotAvailable' instead.")]
		[NoiOS, NoMacCatalyst]
		WatchNotAvailable = -11,
		[NoiOS, NoMacCatalyst]
		BiometryNotPaired = -12,
		[NoiOS, NoMacCatalyst]
		BiometryDisconnected = -13,
		[NoiOS, NoMacCatalyst]
		InvalidDimension = -14,
		/// <summary>Indicates that biometric authentication is not supported on the device.</summary>
		[MacCatalyst (13, 1)]
		BiometryNotAvailable = -6,
		/// <summary>Indicates that the user has not enrolled for biometric authentication.</summary>
		[MacCatalyst (13, 1)]
		BiometryNotEnrolled = -7,
		/// <summary>Indicates that biometric authentication has failed too many times, and the user is now locked out.</summary>
		[MacCatalyst (13, 1)]
		BiometryLockout = -8,
		/// <summary>To be added.</summary>
		NotInteractive = -1004,
		CompanionNotAvailable = -11,
	}

	/// <summary>Enumerates local authentication credential types.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LACredentialType : long {
		/// <summary>Indicates that an application provided a password.</summary>
		ApplicationPassword = 0,
		[iOS (13, 4), NoTV]
		[MacCatalyst (13, 1)]
		SmartCardPin = -3,
	}

	/// <summary>Enumerates access control operations for the <see cref="LocalAuthentication.LAContext.EvaluateAccessControl(Security.SecAccessControl,LocalAuthentication.LAAccessControlOperation,System.String,System.Action{System.Boolean,Foundation.NSError})" /> method.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LAAccessControlOperation : long {
		/// <summary>Indicates an item creation operation.</summary>
		CreateItem,
		/// <summary>Indicates an item use operation.</summary>
		UseItem,
		/// <summary>Indicates a key creation operation.</summary>
		CreateKey,
		/// <summary>Indicates an key signing operation.</summary>
		UseKeySign,
		/// <summary>Indicates a decryption operation with a key.</summary>
		[MacCatalyst (13, 1)]
		UseKeyDecrypt,
		/// <summary>Indicates an key exchange operation.</summary>
		[MacCatalyst (13, 1)]
		UseKeyKeyExchange,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum LARightState : long {
		Unknown = 0,
		Authorizing = 1,
		Authorized = 2,
		NotAuthorized = 3,
	}
}
