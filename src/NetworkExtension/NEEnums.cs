using ObjCRuntime;

namespace NetworkExtension {

	/// <summary>Enumeration of error conditions relating to the VPN configuration.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("NEVPNErrorDomain")]
	[Native]
	public enum NEVpnError : long {
		/// <summary>The VPN configuration was not valid.</summary>
		ConfigurationInvalid = 1,
		/// <summary>The VPN configuration was not enabled.</summary>
		ConfigurationDisabled = 2,
		/// <summary>The VPN connection failed.</summary>
		ConnectionFailed = 3,
		/// <summary>The VPN configuration was updated by another process after it was loaded.</summary>
		ConfigurationStale = 4,
		/// <summary>The VPN configuration could either not be read from or written to.</summary>
		ConfigurationReadWriteFailed = 5,
		/// <summary>The VPN configuration was not recognized.</summary>
		ConfigurationUnknown = 6,
	}

	/// <summary>Enumerates the state of a VPN connection.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEVpnStatus : long {
		/// <summary>The VPN configuration is invalid or disabled.</summary>
		Invalid = 0,
		/// <summary>The VPN is disconnected.</summary>
		Disconnected = 1,
		/// <summary>The VPN is in the process of attempting to connect.</summary>
		Connecting = 2,
		/// <summary>The VPN is connected.</summary>
		Connected = 3,
		/// <summary>The VPN is in the process of reconnecting.</summary>
		Reasserting = 4,
		/// <summary>The VPN is in the process of attempting to disconnect.</summary>
		Disconnecting = 5,
	}

	/// <summary>Enumerates supported techniques for authenticating Internet Key Exchange.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEVpnIkeAuthenticationMethod : long {
		/// <summary>In IKEv2, supports extended authentication if <see cref="NetworkExtension.NEVpnProtocolIpSec.ExtendedAuthentication" /> is <see langword="true" />.</summary>
		None = 0,
		/// <summary>A certificate and private key.</summary>
		Certificate = 1,
		/// <summary>A shared secret is used for authentication.</summary>
		SharedSecret = 2,
	}

	/// <summary>Enumerates the algorithms that can be used for <see cref="NetworkExtension.NEVpnIke2SecurityAssociationParameters.EncryptionAlgorithm" />.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native ("NEVPNIKEv2EncryptionAlgorithm")]
	public enum NEVpnIke2EncryptionAlgorithm : long {
		/// <summary>To be added.</summary>
		[NoTV]
		DES = 1,
		/// <summary>To be added.</summary>
		[NoTV]
		TripleDES = 2,
		/// <summary>To be added.</summary>
		AES128 = 3,
		/// <summary>To be added.</summary>
		AES256 = 4,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		AES128GCM = 5,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		AES256GCM = 6,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		ChaCha20Poly1305 = 7,
	}

	/// <summary>Enumerates the valid integrity algorithms for <see cref="NetworkExtension.NEVpnIke2SecurityAssociationParameters.IntegrityAlgorithm" />.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native ("NEVPNIKEv2IntegrityAlgorithm")]
	public enum NEVpnIke2IntegrityAlgorithm : long {
		/// <summary>Indicates the SHA-1 96-bit algorithm.</summary>
		[NoTV]
		SHA96 = 1,
		/// <summary>Indicates the SHA-1 160-bit algorithm.</summary>
		[NoTV]
		SHA160 = 2,
		/// <summary>Indicates the SHA-2 256-bit algorithm.</summary>
		SHA256 = 3,
		/// <summary>Indicates the SHA-2 384-bit algorithm.</summary>
		SHA384 = 4,
		/// <summary>Indicates the SHA-2 512-bit algorithm.</summary>
		SHA512 = 5,
	}

	/// <summary>Enumerates the frequencies with which the connection attempts to detect dead peers.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native ("NEVPNIKEv2DeadPeerDetectionRate")]
	public enum NEVpnIke2DeadPeerDetectionRate : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Low = 1,
		/// <summary>To be added.</summary>
		Medium = 2,
		/// <summary>To be added.</summary>
		High = 3,
	}

	/// <summary>Enumeration of Diffie Hellman groups, which determine encryption strength.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native ("NEVPNIKEv2DiffieHellmanGroup")]
	public enum NEVpnIke2DiffieHellman : long {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Group0 = Invalid,
		/// <summary>To be added.</summary>
		[NoTV]
		Group1 = 1,
		/// <summary>To be added.</summary>
		[NoTV]
		Group2 = 2,
		/// <summary>To be added.</summary>
		[NoTV]
		Group5 = 5,
		/// <summary>To be added.</summary>
		Group14 = 14,
		/// <summary>To be added.</summary>
		Group15 = 15,
		/// <summary>To be added.</summary>
		Group16 = 16,
		/// <summary>To be added.</summary>
		Group17 = 17,
		/// <summary>To be added.</summary>
		Group18 = 18,
		/// <summary>To be added.</summary>
		Group19 = 19,
		/// <summary>To be added.</summary>
		Group20 = 20,
		/// <summary>To be added.</summary>
		Group21 = 21,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Group31 = 31,
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		Group32 = 32,
	}

	/// <summary>Enumerates the <see cref="NetworkExtension.NEOnDemandRule.Action" /> values of a <see cref="NetworkExtension.NEOnDemandRule" />.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEOnDemandRuleAction : long {
		/// <summary>To be added.</summary>
		Connect = 1,
		/// <summary>To be added.</summary>
		Disconnect = 2,
		/// <summary>To be added.</summary>
		EvaluateConnection = 3,
		/// <summary>To be added.</summary>
		Ignore = 4,
	}

	/// <summary>Enumerates the valid network interface types.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[TV (17, 0)]
	[Native]
	public enum NEOnDemandRuleInterfaceType : long {
		/// <summary>Indicates that any interface type should be matched.</summary>
		Any = 0,
		/// <summary>Indicates that ethernet interfaces should be matched.</summary>
		[NoiOS, NoMacCatalyst]
		Ethernet = 1,
		/// <summary>Indicates that Wi-Fi interfaces should be matched.</summary>
		WiFi = 2,
		/// <summary>Indicates that cellular interfaces should be matched.</summary>
		[NoTV, NoMac]
		Cellular = 3,
	}

	/// <summary>Enumerates behavior if the matching host name cannot be resolved.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEEvaluateConnectionRuleAction : long {
		/// <summary>Attempts to start the VPN connection if the host name cannot be resolved.</summary>
		ConnectIfNeeded = 1,
		/// <summary>Does not attempt to start the VPN connection.</summary>
		NeverConnect = 2,
	}

	/// <summary>Enumerates the cryptographic algorithm associated with the certificate.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native ("NEVPNIKEv2CertificateType")] // NSInteger
	public enum NEVpnIke2CertificateType : long {
		/// <summary>To be added.</summary>
		RSA = 1,
		/// <summary>To be added.</summary>
		ECDSA256 = 2,
		/// <summary>To be added.</summary>
		ECDSA384 = 3,
		/// <summary>To be added.</summary>
		ECDSA521 = 4,
		[NoTV]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Ed25519 = 5,
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		Rsapss = 6,
	}

	// in Xcode7 SDK but marked as 8.0
	/// <summary>Enumerates errors associated with <see cref="NetworkExtension.NEFilterManager" />.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("NEFilterErrorDomain")]
	[Native]
	public enum NEFilterManagerError : long {
		/// <summary>No error occurred.</summary>
		None = 0,
		/// <summary>The configuration is invalid.</summary>
		Invalid = 1,
		/// <summary>The configuration is disabled.</summary>
		Disabled = 2,
		/// <summary>Another app or process modified the configuration since it was last loaded.</summary>
		Stale = 3,
		/// <summary>The configuration could not be removed.</summary>
		CannotBeRemoved = 4,
		ConfigurationPermissionDenied = 5,
		ConfigurationInternalError = 6,
	}

	/// <summary>Enumerates network tunnel errors.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("NETunnelProviderErrorDomain")]
	[Native]
	public enum NETunnelProviderError : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Invalid = 1,
		/// <summary>To be added.</summary>
		Canceled = 2,
		/// <summary>To be added.</summary>
		Failed = 3,
	}

	/// <summary>Enumerates <see cref="NetworkExtension.NEAppProxyFlow" /> error codes.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("NEAppProxyErrorDomain")]
	[Native]
	public enum NEAppProxyFlowError : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		NotConnected = 1,
		/// <summary>To be added.</summary>
		PeerReset = 2,
		/// <summary>To be added.</summary>
		HostUnreachable = 3,
		/// <summary>To be added.</summary>
		InvalidArgument = 4,
		/// <summary>To be added.</summary>
		Aborted = 5,
		/// <summary>To be added.</summary>
		Refused = 6,
		/// <summary>To be added.</summary>
		TimedOut = 7,
		/// <summary>To be added.</summary>
		Internal = 8,
		// iOS 9.3
		/// <summary>The datagram was too large.</summary>
		DatagramTooLarge = 9,
		/// <summary>A read operation was already pending.</summary>
		ReadAlreadyPending = 10,
	}

	/// <summary>Enumerates reasons that a provider extension has stopped.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEProviderStopReason : long {
		/// <summary>An unspecified failure occurred, or no failure occurred.</summary>
		None = 0,
		/// <summary>The user stopped the provider.</summary>
		UserInitiated = 1,
		/// <summary>The provider failed.</summary>
		ProviderFailed = 2,
		/// <summary>The network was unavailable.</summary>
		NoNetworkAvailable = 3,
		/// <summary>The network connectivity changed and the provider could not recover.</summary>
		UnrecoverableNetworkChange = 4,
		/// <summary>The provider was not enabled.</summary>
		ProviderDisabled = 5,
		/// <summary>An authentication operation was canceled.</summary>
		AuthenticationCanceled = 6,
		/// <summary>The network configuration failed.</summary>
		ConfigurationFailed = 7,
		/// <summary>The session timed out.</summary>
		IdleTimeout = 8,
		/// <summary>The network configuration was disabled.</summary>
		ConfigurationDisabled = 9,
		/// <summary>The netowrk configuration was removed.</summary>
		ConfigurationRemoved = 10,
		/// <summary>The configuration was superseded by another.</summary>
		Superseded = 11,
		/// <summary>The user logged off.</summary>
		UserLogout = 12,
		/// <summary>The user changed.</summary>
		UserSwitch = 13,
		/// <summary>The network connection failed.</summary>
		ConnectionFailed = 14,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Sleep = 15,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		AppUpdate = 16,
		[iOS (18, 1), MacCatalyst (18, 1), TV (18, 1), Mac (15, 1)]
		InternalError = 17,
	}

	/// <summary>Enumerates status information about network connection paths.</summary>
	/// <remarks>To be added.</remarks>
	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPathStatus' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPathStatus' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPathStatus' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPathStatus' instead.")]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NWPathStatus : long {
		/// <summary>Indicates that the path is not valid.</summary>
		Invalid = 0,
		/// <summary>Indicates that the path is ready for use.</summary>
		Satisfied = 1,
		/// <summary>The path cannot be used.</summary>
		Unsatisfied = 2,
		/// <summary>Indicates that the path is able to be used if, for example, a VPN becomes available or cellular data is enabled.</summary>
		Satisfiable = 3,
	}

	/// <summary>Enumerates states that can be encountered while establishing a TCP connection.</summary>
	/// <remarks>To be added.</remarks>
	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NWTcpConnectionState : long {
		/// <summary>Indicates that the connection is invalid.</summary>
		Invalid = 0,
		/// <summary>Indicates that a connection is being established.</summary>
		Connecting = 1,
		/// <summary>Indicates that better connection conditions are being waited for.</summary>
		Waiting = 2,
		/// <summary>Indicates that the connection is working.</summary>
		Connected = 3,
		/// <summary>Indicates that the connection was active, but was disconnected and that the developer should cancel the connection.</summary>
		Disconnected = 4,
		/// <summary>Indicates that the connection was canceled by the client.</summary>
		Cancelled = 5,
	}

	/// <summary>Enumerates states that can be encountered while establishing a UDP connection.</summary>
	/// <remarks>To be added.</remarks>
	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnectionState' instead.")]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NWUdpSessionState : long {
		/// <summary>Indicates that the session is invalid or hasn't been initialized.</summary>
		Invalid = 0,
		/// <summary>Indicates that the session is waiting for better connection conditions.</summary>
		Waiting = 1,
		/// <summary>Indicates that the session is attempting to resolve a remote endpoint.</summary>
		Preparing = 2,
		/// <summary>Indicate that data may be written and read.</summary>
		Ready = 3,
		/// <summary>Indicates that none of the endpoints can be resolved.</summary>
		Failed = 4,
		/// <summary>Indicates that the session was cancelled.</summary>
		Cancelled = 5,
	}

	/// <summary>Enumerates tunnel routing methods.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NETunnelProviderRoutingMethod : long {
		/// <summary>Indicates that network traffic should be routed according to the destination IP version.</summary>
		DestinationIP = 1,
		/// <summary>Indicates that network traffic should be routed according to the source application IP version.</summary>
		SourceApplication = 2,
		[NoiOS]
		[NoMacCatalyst]
		NetworkRule = 3,
	}

#if !MONOMAC
	/// <summary>Enumerates commands that can be made to a Hotspot Helper to facilitate registering for Wi-Fi access.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEHotspotHelperCommandType : long {
		/// <summary>No command.</summary>
		None = 0,
		/// <summary>Command to filter scan list.</summary>
		FilterScanList = 1,
		/// <summary>Command to evaluate the network.</summary>
		Evaluate = 2,
		/// <summary>Command to authenticate on the network.</summary>
		Authenticate = 3,
		/// <summary>Command to present a user interface.</summary>
		PresentUI = 4,
		/// <summary>Command to maintain the network connection.</summary>
		Maintain = 5,
		/// <summary>Command to log off.</summary>
		Logoff = 6,
	}

	/// <summary>Enumerates values that describe how likely a Hotspot Helper will be able to handle a network.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEHotspotHelperConfidence : long {
		/// <summary>Indicates the inability to cope.</summary>
		None = 0,
		/// <summary>Indicates low confidence.</summary>
		Low = 1,
		/// <summary>Indicates high confidence.</summary>
		High = 2,
	}

	/// <summary>Enumerates Hotspot Helper command response results.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NEHotspotHelperResult : long {
		/// <summary>The command succeeded.</summary>
		Success = 0,
		/// <summary>The command failed.</summary>
		Failure = 1,
		/// <summary>In response to an "authenticate" command, user interaction is needed.</summary>
		UIRequired = 2,
		/// <summary>The command was not recognized.</summary>
		CommandNotRecognized = 3,
		/// <summary>In response to a "maintain" command, the network requires reauthentication.</summary>
		AuthenticationRequired = 4,
		/// <summary>The network is not supported.</summary>
		UnsupportedNetwork = 5,
		/// <summary>In response to an "authenticate" or "presentUI" command, a temporary failure resulted.</summary>
		TemporaryFailure = 6,
	}
#endif

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	[ErrorDomain ("NEAppPushErrorDomain")]
	public enum NEAppPushManagerError : long {
		ConfigurationInvalid = 1,
		ConfigurationNotLoaded = 2,
		InternalError = 3,
		InactiveSession = 4,
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native ("NEDNSProtocol")]
	public enum NEDnsProtocol : long {
		Cleartext = 1,
		Tls = 2,
		Https = 3,
	}

	[NoTV, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native ("NEDNSSettingsManagerError")]
	[ErrorDomain ("NEDNSSettingsErrorDomain")]
	public enum NEDnsSettingsManagerError : long {
		Invalid = 1,
		Disabled = 2,
		Stale = 3,
		CannotBeRemoved = 4,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum NERelayManagerError : long {
		Invalid = 1,
		Disabled = 2,
		Stale = 3,
		CannotBeRemoved = 4,
	}

}
