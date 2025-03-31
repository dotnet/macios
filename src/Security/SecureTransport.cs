// Copyright 2014 Xamarin Inc. All rights reserved.

using System;
using ObjCRuntime;

namespace Security {

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates SSL protocols.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'TlsProtocolVersion' instead.")]
	public enum SslProtocol {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Ssl_3_0 = 2,
		/// <summary>To be added.</summary>
		Tls_1_0 = 4,
		/// <summary>To be added.</summary>
		Tls_1_1 = 7,
		/// <summary>To be added.</summary>
		Tls_1_2 = 8,
		/// <summary>To be added.</summary>
		Dtls_1_0 = 9,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Tls_1_3 = 10,
		Dtls_1_2 = 11,

		/* Obsolete on iOS */
		/// <summary>To be added.</summary>
		Ssl_2_0 = 1,
		/// <summary>To be added.</summary>
		Ssl_3_0_only = 3,
		/// <summary>To be added.</summary>
		Tls_1_0_only = 5,
		/// <summary>To be added.</summary>
		All = 6,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_protocol_version_t)
	[NativeName ("tls_protocol_version_t")]
	public enum TlsProtocolVersion : ushort {
		Tls10 = 769,
		Tls11 = 770,
		Tls12 = 771,
		Tls13 = 772,
		Dtls10 = 65279,
		Dtls12 = 65277,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_ciphersuite_t)
	[NativeName ("tls_ciphersuite_t")]
	public enum TlsCipherSuite : ushort {
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		RsaWith3desEdeCbcSha = 10,
		RsaWithAes128CbcSha = 47,
		RsaWithAes256CbcSha = 53,
		RsaWithAes128GcmSha256 = 156,
		RsaWithAes256GcmSha384 = 157,
		RsaWithAes128CbcSha256 = 60,
		RsaWithAes256CbcSha256 = 61,
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		EcdheEcdsaWith3desEdeCbcSha = 49160,
		EcdheEcdsaWithAes128CbcSha = 49161,
		EcdheEcdsaWithAes256CbcSha = 49162,
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		EcdheRsaWith3desEdeCbcSha = 49170,
		EcdheRsaWithAes128CbcSha = 49171,
		EcdheRsaWithAes256CbcSha = 49172,
		EcdheEcdsaWithAes128CbcSha256 = 49187,
		EcdheEcdsaWithAes256CbcSha384 = 49188,
		EcdheRsaWithAes128CbcSha256 = 49191,
		EcdheRsaWithAes256CbcSha384 = 49192,
		EcdheEcdsaWithAes128GcmSha256 = 49195,
		EcdheEcdsaWithAes256GcmSha384 = 49196,
		EcdheRsaWithAes128GcmSha256 = 49199,
		EcdheRsaWithAes256GcmSha384 = 49200,
		EcdheRsaWithChacha20Poly1305Sha256 = 52392,
		EcdheEcdsaWithChacha20Poly1305Sha256 = 52393,
		Aes128GcmSha256 = 4865,
		Aes256GcmSha384 = 4866,
		Chacha20Poly1305Sha256 = 4867,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_ciphersuite_group_t)
	[NativeName ("tls_ciphersuite_group_t")]
	public enum TlsCipherSuiteGroup : ushort {
		Default,
		Compatibility,
		Legacy,
		Ats,
		AtsCompatibility,
	}

	// subset of OSStatus (int)
	/// <summary>Enumerates SSL connection status.</summary>
	public enum SslStatus {
		/// <summary>The connection succeeded.</summary>
		Success = 0,        // errSecSuccess in SecBase.h
		/// <summary>A general SSL protocol error.</summary>
		Protocol = -9800,
		/// <summary>Cipher suite negotation failed.</summary>
		Negotiation = -9801,
		/// <summary>Fatal alert.</summary>
		FatalAlert = -9802,
		/// <summary>The SSLHandshake method must be called again.</summary>
		WouldBlock = -9803,
		/// <summary>The session could not be found.</summary>
		SessionNotFound = -9804,
		/// <summary>The connection was closed gracefully.</summary>
		ClosedGraceful = -9805,
		/// <summary>The connection was closed due to an error.</summary>
		ClosedAbort = -9806,
		/// <summary>The verification of the common name field in the peer's certificate failed.</summary>
		XCertChainInvalid = -9807,
		/// <summary>Bad certificate format.</summary>
		BadCert = -9808,
		/// <summary>Undefined SSL cryptographic error.</summary>
		Crypto = -9809,
		/// <summary>Internal error.</summary>
		Internal = -9810,
		/// <summary>Module attach error.</summary>
		ModuleAttach = -9811,
		/// <summary>The root certificate is unknown.</summary>
		UnknownRootCert = -9812,
		/// <summary>The certificate chain does not have a root certificate.</summary>
		NoRootCert = -9813,
		/// <summary>The SSL certificate chain has expired.</summary>
		CertExpired = -9814,
		/// <summary>The SSL certificate chain has a certificate that is not yet valid.</summary>
		CertNotYetValid = -9815,
		/// <summary>The server closed the session without notification.</summary>
		ClosedNotNotified = -9816,
		/// <summary>The supplied buffer was too small.</summary>
		BufferOverflow = -9817,
		/// <summary>Bad SSL cipher suite.</summary>
		BadCipherSuite = -9818,
		/// <summary>An unexpected message was received.</summary>
		PeerUnexpectedMsg = -9819,
		/// <summary>Bad Message Authentication Code encountered.</summary>
		PeerBadRecordMac = -9820,
		/// <summary>Decryption failed.</summary>
		PeerDecryptionFail = -9821,
		/// <summary>A record overflow was encountered.</summary>
		PeerRecordOverflow = -9822,
		/// <summary>Decompression failure.</summary>
		PeerDecompressFail = -9823,
		/// <summary>The handshake with the peer failed.</summary>
		PeerHandshakeFail = -9824,
		/// <summary>A bad certificate was encountered.</summary>
		PeerBadCert = -9825,
		/// <summary>An unsupported certificate was encountered.</summary>
		PeerUnsupportedCert = -9826,
		/// <summary>A certificate was revoked.</summary>
		PeerCertRevoked = -9827,
		/// <summary>A certificate has expired.</summary>
		PeerCertExpired = -9828,
		/// <summary>A certificate is unknown.</summary>
		PeerCertUnknown = -9829,
		/// <summary>A bad parameter was detected.</summary>
		IllegalParam = -9830,
		/// <summary>An unknown certificate authority was encountered.</summary>
		PeerUnknownCA = -9831,
		/// <summary>Access was denied.</summary>
		PeerAccessDenied = -9832,
		/// <summary>Decoding error.</summary>
		PeerDecodeError = -9833,
		/// <summary>Decryption error.</summary>
		PeerDecryptError = -9834,
		/// <summary>An export restriction occurred.</summary>
		PeerExportRestriction = -9835,
		/// <summary>Bad protocol version.</summary>
		PeerProtocolVersion = -9836,
		/// <summary>There is insufficient security for the requested operation.</summary>
		PeerInsufficientSecurity = -9837,
		/// <summary>There was an internal error at the peer.</summary>
		PeerInternalError = -9838,
		/// <summary>The used cancelled the operation.</summary>
		PeerUserCancelled = -9839,
		/// <summary>Renegotiation is not allowed.</summary>
		PeerNoRenegotiation = -9840,
		/// <summary>Server certificate was valid or, if verification disabled, was ignored.</summary>
		PeerAuthCompleted = -9841, // non fatal
		/// <summary>The server has requested a client certificate.</summary>
		PeerClientCertRequested = -9842, // non fatal
		/// <summary>The host name connected to is not in the certificate.</summary>
		HostNameMismatch = -9843,
		/// <summary>The peer dropped the connection prior to responding.</summary>
		ConnectionRefused = -9844,
		/// <summary>Decrytion failed.</summary>
		DecryptionFail = -9845,
		/// <summary>Bad Method Authentication Code.</summary>
		BadRecordMac = -9846,
		/// <summary>A record overflow was encountered.</summary>
		RecordOverflow = -9847,
		/// <summary>Configuration error.</summary>
		BadConfiguration = -9848,
		/// <summary>An unexpected record was encountered.</summary>
		UnexpectedRecord = -9849,
		/// <summary>The Diffie-Hellman ephemeral key was weak.</summary>
		SSLWeakPeerEphemeralDHKey = -9850,
		/// <summary>A client hello was received.</summary>
		SSLClientHelloReceived = -9851, // non falta
		/// <summary>The socket reset.</summary>
		SSLTransportReset = -9852,
		/// <summary>The SSL network timed out.</summary>
		SSLNetworkTimeout = -9853,
		/// <summary>SSL configuration failed.</summary>
		SSLConfigurationFailed = -9854,
		/// <summary>The SSL extension is not supported.</summary>
		SSLUnsupportedExtension = -9855,
		/// <summary>An unexpected message was received.</summary>
		SSLUnexpectedMessage = -9856,
		/// <summary>Decompression failed.</summary>
		SSLDecompressFail = -9857,
		/// <summary>The SSL handshake could not negotiate a secure connection.</summary>
		SSLHandshakeFail = -9858,
		/// <summary>Certificate decoding failed.</summary>
		SSLDecodeError = -9859,
		/// <summary>The current SSL request is at a lower version than that of a prior attempt, of which the client is capable.</summary>
		SSLInappropriateFallback = -9860,
		/// <summary>A required extension was missing.</summary>
		SSLMissingExtension = -9861,
		/// <summary>The OCSP response was bad.</summary>
		SSLBadCertificateStatusResponse = -9862,
		/// <summary>An SSL certificate is required.</summary>
		SSLCertificateRequired = -9863,
		/// <summary>An unknown shared key (PSK) or remote password (SRP) identity was encountered.</summary>
		SSLUnknownPskIdentity = -9864,
		/// <summary>An unrecognized name was encountered.</summary>
		SSLUnrecognizedName = -9865,

		// xcode 11
		SslAtsViolation = -9880,
		SslAtsMinimumVersionViolation = -9881,
		SslAtsCiphersuiteViolation = -9882,
		SslAtsMinimumKeySizeViolation = -9883,
		SslAtsLeafCertificateHashAlgorithmViolation = -9884,
		SslAtsCertificateHashAlgorithmViolation = -9885,
		SslAtsCertificateTrustViolation = -9886,
		// xcode 12
		SslEarlyDataRejected = -9890,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates SSL session behavior options.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionOption {
		/// <summary>To be added.</summary>
		BreakOnServerAuth,
		/// <summary>To be added.</summary>
		BreakOnCertRequested,
		/// <summary>To be added.</summary>
		BreakOnClientAuth,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FalseStart,

		/// <summary>To be added.</summary>
		SendOneByteRecord,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		AllowServerIdentityChange = 5,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Fallback = 6,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		BreakOnClientHello = 7,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		AllowRenegotiation = 8,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		EnableSessionTickets = 9,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates values that control when to use SSL.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslAuthenticate {
		/// <summary>To be added.</summary>
		Never,
		/// <summary>To be added.</summary>
		Always,
		/// <summary>To be added.</summary>
		Try,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates values that indicate whether a server side or client side <see cref="T:Security.SslContext" /> should be created.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslProtocolSide {
		/// <summary>To be added.</summary>
		Server,
		/// <summary>To be added.</summary>
		Client,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates types of SSL connections.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslConnectionType {
		/// <summary>To be added.</summary>
		Stream,
		/// <summary>To be added.</summary>
		Datagram,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates stages in the SSL session life cycle.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionState {
		/// <summary>To be added.</summary>
		Invalid = -1,
		/// <summary>To be added.</summary>
		Idle,
		/// <summary>To be added.</summary>
		Handshake,
		/// <summary>To be added.</summary>
		Connected,
		/// <summary>To be added.</summary>
		Closed,
		/// <summary>To be added.</summary>
		Aborted,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionStrengthPolicy {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		ATSv1,
		/// <summary>To be added.</summary>
		ATSv1NoPFS,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates stages in an SSL client certificate exchange.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslClientCertificateState {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Requested,
		/// <summary>To be added.</summary>
		Sent,
		/// <summary>To be added.</summary>
		Rejected,
	}

#if !NET
	// Security.framework/Headers/CipherSuite.h
	// 32 bits (uint32_t) on OSX, 16 bits (uint16_t) on iOS
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsCipherSuite' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsCipherSuite' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsCipherSuite' instead.")]
#if MONOMAC || __MACCATALYST__
	public enum SslCipherSuite : uint {
#else
	public enum SslCipherSuite : ushort {
#endif
		// DO NOT RENAME VALUES - they don't look good but we need them to keep compatibility with our System.dll code
		// it's how it's defined across most SSL/TLS implementation (from RFC)

		SSL_NULL_WITH_NULL_NULL = 0x0000,   // value used before (not after) negotiation
		TLS_NULL_WITH_NULL_NULL = 0x0000,

		// Not the whole list (too much unneeed metadata) but only what's supported
		// FIXME needs to be expended with OSX 10.9

		SSL_RSA_WITH_NULL_MD5 = 0x0001,
		SSL_RSA_WITH_NULL_SHA = 0x0002,
		SSL_RSA_EXPORT_WITH_RC4_40_MD5 = 0x0003,    // iOS 5.1 only
		SSL_RSA_WITH_RC4_128_MD5 = 0x0004,
		SSL_RSA_WITH_RC4_128_SHA = 0x0005,
		SSL_RSA_WITH_3DES_EDE_CBC_SHA = 0x000A,
		SSL_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 0x0016,
		SSL_DH_anon_EXPORT_WITH_RC4_40_MD5 = 0x0017,    // iOS 5.1 only
		SSL_DH_anon_WITH_RC4_128_MD5 = 0x0018,
		SSL_DH_anon_WITH_3DES_EDE_CBC_SHA = 0x001B,

		// TLS - identical values to SSL (above)

		TLS_RSA_WITH_NULL_MD5 = 0x0001,
		TLS_RSA_WITH_NULL_SHA = 0x0002,
		TLS_RSA_WITH_RC4_128_MD5 = 0x0004,
		TLS_RSA_WITH_RC4_128_SHA = 0x0005,
		TLS_RSA_WITH_3DES_EDE_CBC_SHA = 0x000A,
		TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 0x0016,
		TLS_DH_anon_WITH_RC4_128_MD5 = 0x0018,
		TLS_DH_anon_WITH_3DES_EDE_CBC_SHA = 0x001B,

		// TLS specific

		TLS_PSK_WITH_NULL_SHA = 0x002C,
		TLS_RSA_WITH_AES_128_CBC_SHA = 0x002F,
		TLS_DHE_RSA_WITH_AES_128_CBC_SHA = 0x0033,
		TLS_DH_anon_WITH_AES_128_CBC_SHA = 0x0034,
		TLS_RSA_WITH_AES_256_CBC_SHA = 0x0035,
		TLS_DHE_RSA_WITH_AES_256_CBC_SHA = 0x0039,
		TLS_DH_anon_WITH_AES_256_CBC_SHA = 0x003A,
		TLS_RSA_WITH_NULL_SHA256 = 0x003B,
		TLS_RSA_WITH_AES_128_CBC_SHA256 = 0x003C,
		TLS_RSA_WITH_AES_256_CBC_SHA256 = 0x003D,
		TLS_DHE_RSA_WITH_AES_128_CBC_SHA256 = 0x0067,
		TLS_DHE_RSA_WITH_AES_256_CBC_SHA256 = 0x006B,
		TLS_DH_anon_WITH_AES_128_CBC_SHA256 = 0x006C,
		TLS_DH_anon_WITH_AES_256_CBC_SHA256 = 0x006D,
		TLS_PSK_WITH_RC4_128_SHA = 0x008A,
		TLS_PSK_WITH_3DES_EDE_CBC_SHA = 0x008B,
		TLS_PSK_WITH_AES_128_CBC_SHA = 0x008C,
		TLS_PSK_WITH_AES_256_CBC_SHA = 0x008D,

		TLS_RSA_WITH_AES_128_GCM_SHA256 = 0x009C,   // iOS 9+
		TLS_RSA_WITH_AES_256_GCM_SHA384 = 0x009D,   // iOS 9+
		TLS_DHE_RSA_WITH_AES_128_GCM_SHA256 = 0x009E,   // iOS 9+
		TLS_DHE_RSA_WITH_AES_256_GCM_SHA384 = 0x009F,   // iOS 9+

		TLS_DH_DSS_WITH_AES_256_GCM_SHA384 = 0x00A5,
		TLS_DH_anon_WITH_AES_128_GCM_SHA256 = 0x00A6,   // iOS 5.1 only
		TLS_DH_anon_WITH_AES_256_GCM_SHA384 = 0x00A7,   // iOS 5.1 only

		/* RFC 5487 - PSK with SHA-256/384 and AES GCM */
		TLS_PSK_WITH_AES_128_GCM_SHA256 = 0x00A8,
		TLS_PSK_WITH_AES_256_GCM_SHA384 = 0x00A9,
		TLS_DHE_PSK_WITH_AES_128_GCM_SHA256 = 0x00AA,
		TLS_DHE_PSK_WITH_AES_256_GCM_SHA384 = 0x00AB,
		TLS_RSA_PSK_WITH_AES_128_GCM_SHA256 = 0x00AC,
		TLS_RSA_PSK_WITH_AES_256_GCM_SHA384 = 0x00AD,

		TLS_PSK_WITH_AES_128_CBC_SHA256 = 0x00AE,
		TLS_PSK_WITH_AES_256_CBC_SHA384 = 0x00AF,
		TLS_PSK_WITH_NULL_SHA256 = 0x00B0,
		TLS_PSK_WITH_NULL_SHA384 = 0x00B1,

		TLS_DHE_PSK_WITH_AES_128_CBC_SHA256 = 0x00B2,
		TLS_DHE_PSK_WITH_AES_256_CBC_SHA384 = 0x00B3,
		TLS_DHE_PSK_WITH_NULL_SHA256 = 0x00B4,
		TLS_DHE_PSK_WITH_NULL_SHA384 = 0x00B5,

		TLS_RSA_PSK_WITH_AES_128_CBC_SHA256 = 0x00B6,
		TLS_RSA_PSK_WITH_AES_256_CBC_SHA384 = 0x00B7,
		TLS_RSA_PSK_WITH_NULL_SHA256 = 0x00B8,
		TLS_RSA_PSK_WITH_NULL_SHA384 = 0x00B9,

		TLS_ECDH_ECDSA_WITH_NULL_SHA = 0xC001,
		TLS_ECDH_ECDSA_WITH_RC4_128_SHA = 0xC002,
		TLS_ECDH_ECDSA_WITH_3DES_EDE_CBC_SHA = 0xC003,
		TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA = 0xC004,
		TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA = 0xC005,
		TLS_ECDHE_ECDSA_WITH_NULL_SHA = 0xC006,
		TLS_ECDHE_ECDSA_WITH_RC4_128_SHA = 0xC007,
		TLS_ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA = 0xC008,
		TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA = 0xC009,
		TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA = 0xC00A,
		TLS_ECDH_RSA_WITH_NULL_SHA = 0xC00B,
		TLS_ECDH_RSA_WITH_RC4_128_SHA = 0xC00C,
		TLS_ECDH_RSA_WITH_3DES_EDE_CBC_SHA = 0xC00D,
		TLS_ECDH_RSA_WITH_AES_128_CBC_SHA = 0xC00E,
		TLS_ECDH_RSA_WITH_AES_256_CBC_SHA = 0xC00F,
		TLS_ECDHE_RSA_WITH_NULL_SHA = 0xC010,
		TLS_ECDHE_RSA_WITH_RC4_128_SHA = 0xC011,
		TLS_ECDHE_RSA_WITH_3DES_EDE_CBC_SHA = 0xC012,
		TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA = 0xC013,
		TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA = 0xC014,

		TLS_ECDH_anon_WITH_NULL_SHA = 0xC015,
		TLS_ECDH_anon_WITH_RC4_128_SHA = 0xC016,
		TLS_ECDH_anon_WITH_3DES_EDE_CBC_SHA = 0xC017,
		TLS_ECDH_anon_WITH_AES_128_CBC_SHA = 0xC018,
		TLS_ECDH_anon_WITH_AES_256_CBC_SHA = 0xC019,

		TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 = 0xC023,
		TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 = 0xC024,
		TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256 = 0xC025,
		TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384 = 0xC026,
		TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256 = 0xC027,
		TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384 = 0xC028,
		TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256 = 0xC029,
		TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384 = 0xC02A,

		TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 = 0xC02B,   // iOS 9+
		TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 = 0xC02C,   // iOS 9+
		TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256 = 0xC02D,    // iOS 9+
		TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384 = 0xC02E,    // iOS 9+
		TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256 = 0xC02F, // iOS 9+
		TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384 = 0xC030, // iOS 9+
		TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256 = 0xC031,  // iOS 9+
		TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384 = 0xC032,  // iOS 9+

		// rfc 5489
		TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA = 0xC035,
		TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA = 0xC036,

		// https://tools.ietf.org/html/rfc7905
		TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 0xCCA8,   // Xcode 9+
		TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 = 0xCCA9, // Xcode 9+

		// rfc 7905
		TLS_PSK_WITH_CHACHA20_POLY1305_SHA256 = 0xCCAB,

		// https://tools.ietf.org/html/rfc5746 secure renegotiation
		TLS_EMPTY_RENEGOTIATION_INFO_SCSV = 0x00FF,

		/* TLS 1.3 */
		TLS_AES_128_GCM_SHA256 = 0x1301,    // iOS 11+
		TLS_AES_256_GCM_SHA384 = 0x1302,    // iOS 11+
		TLS_CHACHA20_POLY1305_SHA256 = 0x1303,  // iOS 11+
		TLS_AES_128_CCM_SHA256 = 0x1304,    // iOS 11+
		TLS_AES_128_CCM_8_SHA256 = 0x1305,  // iOS 11+

		SSL_RSA_WITH_RC2_CBC_MD5 = 0xFF80,
		SSL_RSA_WITH_IDEA_CBC_MD5 = 0xFF81,
		SSL_RSA_WITH_DES_CBC_MD5 = 0xFF82,
		SSL_RSA_WITH_3DES_EDE_CBC_MD5 = 0xFF83,
		SSL_NO_SUCH_CIPHERSUITE = 0xFFFF,

	}
#endif // !NET

	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'TlsCipherSuiteGroup' instead.")]
	// typedef CF_ENUM(int, SSLCiphersuiteGroup)
	public enum SslCipherSuiteGroup {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Compatibility,
		/// <summary>To be added.</summary>
		Legacy,
		/// <summary>To be added.</summary>
		Ats,
		/// <summary>To be added.</summary>
		AtsCompatibility,
	}
}
