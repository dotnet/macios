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

#if XAMCORE_5_0
		[NoiOS]
#else
		[iOS (18, 0)]
#if __IOS__ && !__MACCATALYST__
		[Obsolete ("This value is not available on this platform.")]
#endif
#endif
		[NoTV, MacCatalyst (18, 0), Mac (15, 0)]
		DeviceOwnerAuthenticationWithCompanion = 3,

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'DeviceOwnerAuthenticationWithBiometricsOrCompanion' instead.")]
		[NoiOS]
		[NoMacCatalyst]
		DeviceOwnerAuthenticationWithBiometricsOrWatch = 4,

#if XAMCORE_5_0
		[NoiOS]
#else
		[iOS (18, 0)]
#if __IOS__ && !__MACCATALYST__
		[Obsolete ("This value is not available on this platform.")]
#endif
#endif
		[NoTV, MacCatalyst (18, 0), Mac (15, 0)]
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
		/// <summary>Authentication was canceled by the application.</summary>
		AppCancel = -9,
		/// <summary>The authentication context was invalidated.</summary>
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
		/// <summary>Authentication failed because the user interface was not allowed to be presented.</summary>
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

	/// <summary>Enumerates the states of a right.</summary>
	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum LARightState : long {
		/// <summary>The right hasn't been evaluated yet. This is the initial state of a right.</summary>
		Unknown = 0,
		/// <summary>The system is evaluating the requirements. This happens after the app requests authorization, but before the user grants the right.</summary>
		Authorizing = 1,
		/// <summary>The right was authorized.</summary>
		Authorized = 2,
		/// <summary>The right wasn't authorized. For example, the requirements weren't met or the user rejected the request.</summary>
		NotAuthorized = 3,
	}
}
