// Copyright 2011-2015 Xamarin Inc. All rights reserved.
using System;

using Foundation;
using ObjCRuntime;

namespace Security {

	// this is a subset of OSStatus -> SInt32 -> signed int - see CoreFoundation.framework/Headers/CFBase.h
	// values are defined in Security.framework/Headers/SecBase.h 
	/// <summary>Status return from the SecKeyChain operations.</summary>
	public enum SecStatusCode {
		/// <summary>Success, there was no error.</summary>
		Success = 0,
		/// <summary>The specified feature is not implemented.</summary>
		Unimplemented = -4,
		/// <summary>Indicates that the disk was full.</summary>
		DiskFull = -34,
		/// <summary>Indicates an IO error.</summary>
		IO = -36,
		/// <summary>Indicates that a file is already open with read and write privileges.</summary>
		OpWr = -49,
		/// <summary>Invalid or incomplete parameters passed.</summary>
		Param = -50,
		/// <summary>Indicates that write permissions are already granted.</summary>
		WritePermissions = -61,
		/// <summary>Failure to allocate memory.</summary>
		Allocate = -108,
		/// <summary>Indicates that the user cancelled the operation.</summary>
		UserCanceled = -128,
		/// <summary>Indicates that authentication failed.</summary>
		BadReq = -909,
		/// <summary>Indicates an unspecified error in an internal component.</summary>
		InternalComponent = -2070,
		/// <summary>Indicates that an unknown Core Foundation error occurred.</summary>
		CoreFoundationUnknown = -4960,
		/// <summary>Indicates that a trust results were not available.</summary>
		NotAvailable = -25291,
		/// <summary>The keychain is read only.</summary>
		ReadOnly = -25292,
		/// <summary>Authentication failed.</summary>
		AuthFailed = -25293,
		/// <summary>The keychain specified does not exist.</summary>
		NoSuchKeyChain = -25294,
		/// <summary>The keychain provided is invalid.</summary>
		InvalidKeyChain = -25295,
		/// <summary>Duplicated key chain.</summary>
		DuplicateKeyChain = -25296,
		/// <summary>The item is duplicated.</summary>
		DuplicateItem = -25299,
		/// <summary>The item was not found.</summary>
		ItemNotFound = -25300,
		/// <summary>Indicates that the interaction with the Security Server was not allowed.</summary>
		InteractionNotAllowed = -25308,
		/// <summary>Indicates that data could not be decoded.</summary>
		Decode = -26275,
		/// <summary>Indicates that a callback was duplicated</summary>
		DuplicateCallback = -25297,
		/// <summary>Indicates that a callback was not valid.</summary>
		InvalidCallback = -25298,
		/// <summary>Indicates that a buffer was too small.</summary>
		BufferTooSmall = -25301,
		/// <summary>Indicates that data were too large.</summary>
		DataTooLarge = -25302,
		/// <summary>Indicates that no such attribute was found.</summary>
		NoSuchAttribute = -25303,
		/// <summary>Indicates an invalid item reference.</summary>
		InvalidItemRef = -25304,
		/// <summary>Indicates an invalid search reference.</summary>
		InvalidSearchRef = -25305,
		/// <summary>Indicates that no such class was found.</summary>
		NoSuchClass = -25306,
		/// <summary>Indicates that no default key chain was found.</summary>
		NoDefaultKeychain = -25307,
		/// <summary>Indicates that an attribute is read-only.</summary>
		ReadOnlyAttribute = -25309,
		/// <summary>Indicates that the wrong security version was encountered.</summary>
		WrongSecVersion = -25310,
		/// <summary>Indicates that a key's size was not supported.</summary>
		KeySizeNotAllowed = -25311,
		/// <summary>Indicates that no storage module was found.</summary>
		NoStorageModule = -25312,
		/// <summary>Indicates that no certificate module was found.</summary>
		NoCertificateModule = -25313,
		/// <summary>Indicates that no policy module was found.</summary>
		NoPolicyModule = -25314,
		/// <summary>Indicates that user interaction was required.</summary>
		InteractionRequired = -25315,
		/// <summary>Indicates that certain data were not available.</summary>
		DataNotAvailable = -25316,
		/// <summary>Indicates that data could not be modified.</summary>
		DataNotModifiable = -25317,
		/// <summary>Indicates that the certificate chain failed.</summary>
		CreateChainFailed = -25318,
		/// <summary>Indicates an invalid preferences domain.</summary>
		InvalidPrefsDomain = -25319,
		/// <summary>Indicates that a dark wake state prevented a UI from being displayed.</summary>
		InDarkWake = -25320,
		/// <summary>Indicates that the ACL was not in its canonical form.</summary>
		ACLNotSimple = -25240,
		/// <summary>Indicates that a trust policy could not be found.</summary>
		PolicyNotFound = -25241,
		/// <summary>Indicates an invalid trust setting.</summary>
		InvalidTrustSetting = -25242,
		/// <summary>Indicates that an item does not have access control.</summary>
		NoAccessForItem = -25243,
		/// <summary>Indicates an invalid owner change.</summary>
		InvalidOwnerEdit = -25244,
		/// <summary>Indicates that trust results are not available.</summary>
		TrustNotAvailable = -25245,
		/// <summary>Indicates that an unsupported format was encountered.</summary>
		UnsupportedFormat = -25256,
		/// <summary>Indicates that an unknown format was encountered.</summary>
		UnknownFormat = -25257,
		/// <summary>Indicates that an attempt was made to send a sensitive key unwrapped.</summary>
		KeyIsSensitive = -25258,
		/// <summary>Indicates that a module failed to load.</summary>
		MultiplePrivateKeys = -25259,
		/// <summary>Indicates that a pass phrase is required.</summary>
		PassphraseRequired = -25260,
		/// <summary>Indicates an invalid password reference.</summary>
		InvalidPasswordRef = -25261,
		/// <summary>Indicates invalid trust settings.</summary>
		InvalidTrustSettings = -25262,
		/// <summary>Indicates that trust settings were not found.</summary>
		NoTrustSettings = -25263,
		/// <summary>Indicates that a PKCS12 import failure caused a MAC failure.</summary>
		Pkcs12VerifyFailure = -25264,
		/// <summary>Indicates that the parent of the certificate is not actually the signer.</summary>
		NotSigner = -26267,
		/// <summary>To be added.</summary>
		MissingEntitlement = -34018,
		RestrictedApi = -34020,
		/// <summary>Indicates that a service was not available.</summary>
		ServiceNotAvailable = -67585,
		/// <summary>Indicates that a client ID was wrong.</summary>
		InsufficientClientID = -67586,
		/// <summary>Indicates that a device reset.</summary>
		DeviceReset = -67587,
		/// <summary>Indicates that a device failed.</summary>
		DeviceFailed = -67588,
		/// <summary>Indicates that an application ACL subject could not be added..</summary>
		AppleAddAppACLSubject = -67589,
		/// <summary>Indicates that a public key was incomplete.</summary>
		ApplePublicKeyIncomplete = -67590,
		/// <summary>Indicates a signature mismatch.</summary>
		AppleSignatureMismatch = -67591,
		/// <summary>Indicates that the start date for a key was invalid.</summary>
		AppleInvalidKeyStartDate = -67592,
		/// <summary>Indicates that the end date for a key was invalid.</summary>
		AppleInvalidKeyEndDate = -67593,
		/// <summary>Indicates that a conversion error occurred.</summary>
		ConversionError = -67594,
		/// <summary>Indicates that rollback to SSLv2 encountered an error.</summary>
		AppleSSLv2Rollback = -67595,
		/// <summary>Indicates that a quota was exceeded.</summary>
		QuotaExceeded = -67596,
		/// <summary>Indicates that a file was too large.</summary>
		FileTooBig = -67597,
		/// <summary>Indicates that a database blob ws not valid.</summary>
		InvalidDatabaseBlob = -67598,
		/// <summary>Indicates an invalid key blob.</summary>
		InvalidKeyBlob = -67599,
		/// <summary>Indicates that a database blob was not compatible.</summary>
		IncompatibleDatabaseBlob = -67600,
		/// <summary>Indicates that a database key blob was incompatible.</summary>
		IncompatibleKeyBlob = -67601,
		/// <summary>Indicates that a hostname mismatch occurred.</summary>
		HostNameMismatch = -67602,
		/// <summary>Indicates that an unknown critical extension flag was encountered</summary>
		UnknownCriticalExtensionFlag = -67603,
		/// <summary>Indicates that basic constraints were not found.</summary>
		NoBasicConstraints = -67604,
		/// <summary>Indicates that no basic CA constraints were found.</summary>
		NoBasicConstraintsCA = -67605,
		/// <summary>Indicates that the authority key ID was not valid.</summary>
		InvalidAuthorityKeyID = -67606,
		/// <summary>Indicates an invalid subject key ID.</summary>
		InvalidSubjectKeyID = -67607,   /* The subject key ID is not valid. */
		/// <summary>Indicates an invalid key usage for a particular policy.</summary>
		InvalidKeyUsageForPolicy = -67608,  /* The key usage is not valid for the specified policy. */
		/// <summary>Indicate an invalid extended key usage.</summary>
		InvalidExtendedKeyUsage = -67609,   /* The extended key usage is not valid. */
		/// <summary>Indicates an invalid linkage to an ID.</summary>
		InvalidIDLinkage = -67610,  /* The ID linkage is not valid. */
		/// <summary>Indicates that a path was too long.</summary>
		PathLengthConstraintExceeded = -67611,  /* The path length constraint was exceeded. */
		/// <summary>Indicates an invalid root.</summary>
		InvalidRoot = -67612,   /* The root or anchor certificate is not valid. */
		/// <summary>Indicates that a CRL was expired.</summary>
		CRLExpired = -67613,    /* The CRL has expired. */
		/// <summary>Indicates that a CRL was not yet valid.</summary>
		CRLNotValidYet = -67614,    /* The CRL is not yet valid. */
		/// <summary>Indicates that a CRL could not be found.</summary>
		CRLNotFound = -67615,   /* The CRL was not found. */
		/// <summary>Indicates that a CRL server was down.</summary>
		CRLServerDown = -67616, /* The CRL server is down. */
		/// <summary>Indicates that a URI was bad.</summary>
		CRLBadURI = -67617, /* The CRL has a bad Uniform Resource Identifier. */
		/// <summary>Indicates that an unknown certificate extension was encountered.</summary>
		UnknownCertExtension = -67618,  /* An unknown certificate extension was encountered. */
		/// <summary>Indicates that an unknown CRL extension was encountered.</summary>
		UnknownCRLExtension = -67619,   /* An unknown CRL extension was encountered. */
		/// <summary>Indicates that a CRL was untrusted.</summary>
		CRLNotTrusted = -67620, /* The CRL is not trusted. */
		/// <summary>Indicates that a CRL revocation policy failed.</summary>
		CRLPolicyFailed = -67621,   /* The CRL policy failed. */
		/// <summary>Indicates that the issuing distribution point was not valid.</summary>
		IDPFailure = -67622,    /* The issuing distribution point was not valid. */
		/// <summary>Indicates that a email addresses could not be found.</summary>
		SMIMEEmailAddressesNotFound = -67623,   /* An email address mismatch was encountered. */
		/// <summary>Indicates that a bad SMIME extended key usage was encountered.</summary>
		SMIMEBadExtendedKeyUsage = -67624,  /* The appropriate extended key usage for SMIME was not found. */
		/// <summary>Indicates that a bad SMIME key usage was encountered.</summary>
		SMIMEBadKeyUsage = -67625,  /* The key usage is not compatible with SMIME. */
		/// <summary>Indicates that a critical SMIME extended key usage was encountered where prohibited.</summary>
		SMIMEKeyUsageNotCritical = -67626,  /* The key usage extension is not marked as critical. */
		/// <summary>Indicates that the certificate did not contain the email address.</summary>
		SMIMENoEmailAddress = -67627,   /* No email address was found in the certificate. */
		/// <summary>Indicates that the alternative name for the subject field was not critical.</summary>
		SMIMESubjAltNameNotCritical = -67628,   /* The subject alternative name extension is not marked as critical. */
		/// <summary>Indicates that a bad extended key usage was encountered.</summary>
		SSLBadExtendedKeyUsage = -67629,    /* The appropriate extended key usage for SSL was not found. */
		/// <summary>Indicates that an OCSP response is bad.</summary>
		OCSPBadResponse = -67630,   /* The OCSP response was incorrect or could not be parsed. */
		/// <summary>Indicates that an OCSP request is bad.</summary>
		OCSPBadRequest = -67631,    /* The OCSP request was incorrect or could not be parsed. */
		/// <summary>Indicates that an OCSP server is not available.</summary>
		OCSPUnavailable = -67632,   /* OCSP service is unavailable. */
		/// <summary>Indicates that an OCSP responder did not recognize a certificate.</summary>
		OCSPStatusUnrecognized = -67633,    /* The OCSP server did not recognize this certificate. */
		/// <summary>Indicates that the end of data was unexpectedly reached.</summary>
		EndOfData = -67634, /* An end-of-data was detected. */
		/// <summary>Indicates a certificate revocation check was incomplete.</summary>
		IncompleteCertRevocationCheck = -67635, /* An incomplete certificate revocation check occurred. */
		/// <summary>Indicates a network failure.</summary>
		NetworkFailure = -67636,    /* A network failure occurred. */
		/// <summary>Indicates that an OCSP response is not trusted back to an anchor.</summary>
		OCSPNotTrustedToAnchor = -67637,    /* The OCSP response was not trusted to a root or anchor certificate. */
		/// <summary>Indicates that a record was modified.</summary>
		RecordModified = -67638,    /* The record was modified. */
		/// <summary>Indicates an invalid OCSP response signature.</summary>
		OCSPSignatureError = -67639,    /* The OCSP response had an invalid signature. */
		/// <summary>Indicates that an OCSP response has no signer.</summary>
		OCSPNoSigner = -67640,  /* The OCSP response had no signer. */
		/// <summary>Indicates that an OCSP responder received a malformed request.</summary>
		OCSPResponderMalformedReq = -67641, /* The OCSP responder was given a malformed request. */
		/// <summary>Indicates an OCSP responder internal error.</summary>
		OCSPResponderInternalError = -67642,    /* The OCSP responder encountered an internal error. */
		/// <summary>Indicates that an OCSP responder is busy.</summary>
		OCSPResponderTryLater = -67643, /* The OCSP responder is busy, try again later. */
		/// <summary>Indicates than an OCSP responder requires a signature.</summary>
		OCSPResponderSignatureRequired = -67644,    /* The OCSP responder requires a signature. */
		/// <summary>Indicates that an OCSP responder determined that a request was unauthorized.</summary>
		OCSPResponderUnauthorized = -67645, /* The OCSP responder rejected this request as unauthorized. */
		/// <summary>Indicates that an OCSP response nonce is different thatn the request's.</summary>
		OCSPResponseNonceMismatch = -67646, /* The OCSP response nonce did not match the request. */
		/// <summary>Indicates that the length of a code signing chain was bad.</summary>
		CodeSigningBadCertChainLength = -67647, /* Code signing encountered an incorrect certificate chain length. */
		/// <summary>To be added.</summary>
		CodeSigningNoBasicConstraints = -67648, /* Code signing found no basic constraints. */
		/// <summary>Indicates that the length of a code signing path was too long.</summary>
		CodeSigningBadPathLengthConstraint = -67649,    /* Code signing encountered an incorrect path length constraint. */
		/// <summary>Indicates that no basic constraints were found.</summary>
		CodeSigningNoExtendedKeyUsage = -67650, /* Code signing found no extended key usage. */
		/// <summary>Indicates that a development-only certificate was used.</summary>
		CodeSigningDevelopment = -67651,    /* Code signing indicated use of a development-only certificate. */
		/// <summary>Indicates that an invalid certificate chain length was encountered.</summary>
		ResourceSignBadCertChainLength = -67652,    /* Resource signing has encountered an incorrect certificate chain length. */
		/// <summary>Indicates that an invalid certificate extension key usage was encountered.</summary>
		ResourceSignBadExtKeyUsage = -67653,    /* Resource signing has encountered an error in the extended key usage. */
		/// <summary>Indicates that the trust setting is DENY.</summary>
		TrustSettingDeny = -67654,  /* The trust setting for this policy was set to Deny. */
		/// <summary>Indicates an invalid subject name.</summary>
		InvalidSubjectName = -67655,    /* An invalid certificate subject name was encountered. */
		/// <summary>Indicates that an unknown qualified certificate statement was encountered.</summary>
		UnknownQualifiedCertStatement = -67656, /* An unknown qualified certificate statement was encountered. */
		/// <summary>Indicates that a MobileMe request was queued.</summary>
		MobileMeRequestQueued = -67657, /* The MobileMe request will be sent during the next connection. */
		/// <summary>Indicates that a MobileMe request was redirected.</summary>
		MobileMeRequestRedirected = -67658, /* The MobileMe request was redirected. */
		/// <summary>Indicates a MobileMe server error.</summary>
		MobileMeServerError = -67659,   /* A MobileMe server error occurred. */
		/// <summary>Indicates that a MobileMe server was not available.</summary>
		MobileMeServerNotAvailable = -67660,    /* The MobileMe server is not available. */
		/// <summary>Indicates that no MobileMe server already exists.</summary>
		MobileMeServerAlreadyExists = -67661,   /* The MobileMe server reported that the item already exists. */
		/// <summary>Indicates a MobileMe server service error.</summary>
		MobileMeServerServiceErr = -67662,  /* A MobileMe service error has occurred. */
		/// <summary>Indicates that a MobileMe request was pending.</summary>
		MobileMeRequestAlreadyPending = -67663, /* A MobileMe request is already pending. */
		/// <summary>Indicates that no MobileMe request was pending.</summary>
		MobileMeNoRequestPending = -67664,  /* MobileMe has no request pending. */
		/// <summary>Indicates a mobile CSVR failure.</summary>
		MobileMeCSRVerifyFailure = -67665,  /* A MobileMe CSR verification failure has occurred. */
		/// <summary>Indicates a MobileMe consistency check failure.</summary>
		MobileMeFailedConsistencyCheck = -67666,    /* MobileMe has found a failed consistency check. */
		/// <summary>Indicates that the common security services manager ws not initialized.</summary>
		NotInitialized = -67667,    /* A function was called without initializing CSSM. */
		/// <summary>Indicates an invalid handle usage.</summary>
		InvalidHandleUsage = -67668,    /* The CSSM handle does not match with the service type. */
		/// <summary>Indicates that a PVC reference could not be found.</summary>
		PVCReferentNotFound = -67669,   /* A reference to the calling module was not found in the list of authorized callers. */
		/// <summary>Indicates that the address of a function is outside the verified module.</summary>
		FunctionIntegrityFail = -67670, /* A function address was not within the verified module. */
		/// <summary>Indicates an unspecified internal error.</summary>
		InternalError = -67671, /* An internal error has occurred. */
		/// <summary>Indicates a memory error.</summary>
		MemoryError = -67672,   /* A memory error has occurred. */
		/// <summary>Indicates that data were invalid.</summary>
		InvalidData = -67673,   /* Invalid data was encountered. */
		/// <summary>Indicates a module directory service error.</summary>
		MDSError = -67674,  /* A Module Directory Service error has occurred. */
		/// <summary>Indicates an invalid pointer.</summary>
		InvalidPointer = -67675,    /* An invalid pointer was encountered. */
		/// <summary>Indicates that a self-check failed.</summary>
		SelfCheckFailed = -67676,   /* Self-check has failed. */
		/// <summary>Indicates that a function failed.</summary>
		FunctionFailed = -67677,    /* A function has failed. */
		/// <summary>Indicates that a module manifest failed to be verified.</summary>
		ModuleManifestVerifyFailed = -67678,    /* A module manifest verification failure has occurred. */
		/// <summary>Indicates an invalid GUID.</summary>
		InvalidGUID = -67679,   /* An invalid GUID was encountered. */
		/// <summary>Indicates an invalid handle.</summary>
		InvalidHandle = -67680, /* An invalid handle was encountered. */
		/// <summary>Indicates that a database list was not valid.</summary>
		InvalidDBList = -67681, /* An invalid DB list was encountered. */
		/// <summary>Indicates an invalid passthrough ID.</summary>
		InvalidPassthroughID = -67682,  /* An invalid passthrough ID was encountered. */
		/// <summary>Indicates an invalid network address.</summary>
		InvalidNetworkAddress = -67683, /* An invalid network address was encountered. */
		/// <summary>Indicatest that the CRL was already signed.</summary>
		CRLAlreadySigned = -67684,  /* The certificate revocation list is already signed. */
		/// <summary>Indicates an invalid number of fields.</summary>
		InvalidNumberOfFields = -67685, /* An invalid number of fields were encountered. */
		/// <summary>To be added.</summary>
		VerificationFailure = -67686,   /* A verification failure occurred. */
		/// <summary>Indicates that an unknown tag was encountered.</summary>
		UnknownTag = -67687,    /* An unknown tag was encountered. */
		/// <summary>Indicates an invalid signature.</summary>
		InvalidSignature = -67688,  /* An invalid signature was encountered. */
		/// <summary>Indicates an invalid name.</summary>
		InvalidName = -67689,   /* An invalid name was encountered. */
		/// <summary>Indicates that the certificate reference was not valid.</summary>
		InvalidCertificateRef = -67690, /* An invalid certificate reference was encountered. */
		/// <summary>Indicates that the cert group was not valid.</summary>
		InvalidCertificateGroup = -67691,   /* An invalid certificate group was encountered. */
		/// <summary>Indicates that a tag could not be found.</summary>
		TagNotFound = -67692,   /* The specified tag was not found. */
		/// <summary>Indicates an invalid query.</summary>
		InvalidQuery = -67693,  /* The specified query was not valid. */
		/// <summary>Indicates an invalid value.</summary>
		InvalidValue = -67694,  /* An invalid value was detected. */
		/// <summary>Indicates that a callback failed.</summary>
		CallbackFailed = -67695,    /* A callback has failed. */
		/// <summary>Indicates that an ACL delete operation failed.</summary>
		ACLDeleteFailed = -67696,   /* An ACL delete operation has failed. */
		/// <summary>Indicates that an ACL replace operation failed.</summary>
		ACLReplaceFailed = -67697,  /* An ACL replace operation has failed. */
		/// <summary>Indicates that an ACL add operation failed.</summary>
		ACLAddFailed = -67698,  /* An ACL add operation has failed. */
		/// <summary>Indicates that an ACL change operation failed.</summary>
		ACLChangeFailed = -67699,   /* An ACL change operation has failed. */
		/// <summary>Indicates that the access credentials were not valid.</summary>
		InvalidAccessCredentials = -67700,  /* Invalid access credentials were encountered. */
		/// <summary>Indicates an invalid record.</summary>
		InvalidRecord = -67701, /* An invalid record was encountered. */
		/// <summary>Indicates that the ACL was not valid.</summary>
		InvalidACL = -67702,    /* An invalid ACL was encountered. */
		/// <summary>Indicates an invalid sample value.</summary>
		InvalidSampleValue = -67703,    /* An invalid sample value was encountered. */
		/// <summary>Indicates an incompatible version.</summary>
		IncompatibleVersion = -67704,   /* An incompatible version was encountered. */
		/// <summary>Indicates that an access privilege was not granted.</summary>
		PrivilegeNotGranted = -67705,   /* The privilege was not granted. */
		/// <summary>Indicates an invalid scope.</summary>
		InvalidScope = -67706,  /* An invalid scope was encountered. */
		/// <summary>Indicates that PVC is already configured.</summary>
		PVCAlreadyConfigured = -67707,  /* The PVC is already configured. */
		/// <summary>Indicates an invalid PVC</summary>
		InvalidPVC = -67708,    /* An invalid PVC was encountered. */
		/// <summary>Indicates that an EMM unload operation failed.</summary>
		EMMLoadFailed = -67709, /* The EMM load has failed. */
		/// <summary>Indicates that an EMM load operation failed.</summary>
		EMMUnloadFailed = -67710,   /* The EMM unload has failed. */
		/// <summary>To be added.</summary>
		AddinLoadFailed = -67711,   /* The add-in load operation has failed. */
		/// <summary>Indicates an invalid key reference.</summary>
		InvalidKeyRef = -67712, /* An invalid key was encountered. */
		/// <summary>Indicates an invalid key hierarchy.</summary>
		InvalidKeyHierarchy = -67713,   /* An invalid key hierarchy was encountered. */
		/// <summary>Indicates that an add-in failed to load.</summary>
		AddinUnloadFailed = -67714, /* The add-in unload operation has failed. */
		/// <summary>Indicates that a library reference could not be found.</summary>
		LibraryReferenceNotFound = -67715,  /* A library reference was not found. */
		/// <summary>Indicates that the add-in function table was not valid.</summary>
		InvalidAddinFunctionTable = -67716, /* An invalid add-in function table was encountered. */
		/// <summary>Indicates an invalid service mask.</summary>
		InvalidServiceMask = -67717,    /* An invalid service mask was encountered. */
		/// <summary>To be added.</summary>
		ModuleNotLoaded = -67718,   /* A module was not loaded. */
		/// <summary>Indicates an invalid subservice ID.</summary>
		InvalidSubServiceID = -67719,   /* An invalid subservice ID was encountered. */
		/// <summary>Indicates that a requested attribute was not within a context.</summary>
		AttributeNotInContext = -67720, /* An attribute was not in the context. */
		/// <summary>Indicates that a module failed while initializing.</summary>
		ModuleManagerInitializeFailed = -67721, /* A module failed to initialize. */
		/// <summary>Indicates that a module was not found.</summary>
		ModuleManagerNotFound = -67722, /* A module was not found. */
		/// <summary>Indicates that a callback could not be found for a notification event.</summary>
		EventNotificationCallbackNotFound = -67723, /* An event notification callback was not found. */
		/// <summary>Indicates that an input was too short or too long.</summary>
		InputLengthError = -67724,  /* An input length error was encountered. */
		/// <summary>Indicates an output length error.</summary>
		OutputLengthError = -67725, /* An output length error was encountered. */
		/// <summary>ndicates that an access privilege was not supported.</summary>
		PrivilegeNotSupported = -67726, /* The privilege is not supported. */
		/// <summary>Indicates that an error occurred on a device.</summary>
		DeviceError = -67727,   /* A device error was encountered. */
		/// <summary>Indicates that the CSP handle was busy.</summary>
		AttachHandleBusy = -67728,  /* The CSP handle was busy. */
		/// <summary>Indicates that the developer is not logged in.</summary>
		NotLoggedIn = -67729,   /* You are not logged in. */
		/// <summary>Indicates that there was a mismatch between security algorithms.</summary>
		AlgorithmMismatch = -67730, /* An algorithm mismatch was encountered. */
		/// <summary>Indicates that a key was used incorrectly.</summary>
		KeyUsageIncorrect = -67731, /* The key usage is incorrect. */
		/// <summary>Indicates that a key blob type was incorrect.</summary>
		KeyBlobTypeIncorrect = -67732,  /* The key blob type is incorrect. */
		/// <summary>Indicates that a key header was inconsistent.</summary>
		KeyHeaderInconsistent = -67733, /* The key header is inconsistent. */
		/// <summary>Indicates that unsupported key format was encountered.</summary>
		UnsupportedKeyFormat = -67734,  /* The key header format is not supported. */
		/// <summary>Indicates that unsupported key size was encountered.</summary>
		UnsupportedKeySize = -67735,    /* The key size is not supported. */
		/// <summary>Indicates an invalid key usage mask.</summary>
		InvalidKeyUsageMask = -67736,   /* The key usage mask is not valid. */
		/// <summary>Indicates that unsupported key usage mask was encountered.</summary>
		UnsupportedKeyUsageMask = -67737,   /* The key usage mask is not supported. */
		/// <summary>Indicates an invalid key attribute mask.</summary>
		InvalidKeyAttributeMask = -67738,   /* The key attribute mask is not valid. */
		/// <summary>Indicates that unsupported key attribute mask was encountered.</summary>
		UnsupportedKeyAttributeMask = -67739,   /* The key attribute mask is not supported. */
		/// <summary>Indicates an invalid key label.</summary>
		InvalidKeyLabel = -67740,   /* The key label is not valid. */
		/// <summary>Indicates that unsupported key label was encountered.</summary>
		UnsupportedKeyLabel = -67741,   /* The key label is not supported. */
		/// <summary>Indicates an invalid key format.</summary>
		InvalidKeyFormat = -67742,  /* The key format is not valid. */
		/// <summary>Indicates that unsupported vector buffers were encountered.</summary>
		UnsupportedVectorOfBuffers = -67743,    /* The vector of buffers is not supported. */
		/// <summary>Indicates an invalid input vector.</summary>
		InvalidInputVector = -67744,    /* The input vector is not valid. */
		/// <summary>Indicates an invalid output vector.</summary>
		InvalidOutputVector = -67745,   /* The output vector is not valid. */
		/// <summary>Indicates that the context was not valid.</summary>
		InvalidContext = -67746,    /* An invalid context was encountered. */
		/// <summary>Indicates that the security algorithm was not valid.</summary>
		InvalidAlgorithm = -67747,  /* An invalid algorithm was encountered. */
		/// <summary>Indicates that the key attribute was not valid.</summary>
		InvalidAttributeKey = -67748,   /* A key attribute was not valid. */
		/// <summary>Indicates that a key attribute was missing.</summary>
		MissingAttributeKey = -67749,   /* A key attribute was missing. */
		/// <summary>Indicates that the init vector attribute was not valid.</summary>
		InvalidAttributeInitVector = -67750,    /* An init vector attribute was not valid. */
		/// <summary>Indicates that an init vector attribute was missing.</summary>
		MissingAttributeInitVector = -67751,    /* An init vector attribute was missing. */
		/// <summary>Indicates that the salt attribute was not valid.</summary>
		InvalidAttributeSalt = -67752,  /* A salt attribute was not valid. */
		/// <summary>Indicates that a salt attribute was missing.</summary>
		MissingAttributeSalt = -67753,  /* A salt attribute was missing. */
		/// <summary>Indicates that the padding attribute was not valid.</summary>
		InvalidAttributePadding = -67754,   /* A padding attribute was not valid. */
		/// <summary>Indicates that a padding attribute was missing.</summary>
		MissingAttributePadding = -67755,   /* A padding attribute was missing. */
		/// <summary>Indicates that the random attribute was not valid.</summary>
		InvalidAttributeRandom = -67756,    /* A random number attribute was not valid. */
		/// <summary>Indicates that a random attribute was missing.</summary>
		MissingAttributeRandom = -67757,    /* A random number attribute was missing. */
		/// <summary>Indicates that the seed attribute was not valid.</summary>
		InvalidAttributeSeed = -67758,  /* A seed attribute was not valid. */
		/// <summary>Indicates that a seed attribute was missing.</summary>
		MissingAttributeSeed = -67759,  /* A seed attribute was missing. */
		/// <summary>Indicates that the passphrase attribute was not valid.</summary>
		InvalidAttributePassphrase = -67760,    /* A passphrase attribute was not valid. */
		/// <summary>Indicates that a pass phrase attribute was missing.</summary>
		MissingAttributePassphrase = -67761,    /* A passphrase attribute was missing. */
		/// <summary>Indicates that the key length attribute was not valid.</summary>
		InvalidAttributeKeyLength = -67762, /* A key length attribute was not valid. */
		/// <summary>Indicates that a key length attribute was missing.</summary>
		MissingAttributeKeyLength = -67763, /* A key length attribute was missing. */
		/// <summary>Indicates that the block size attribute was not valid.</summary>
		InvalidAttributeBlockSize = -67764, /* A block size attribute was not valid. */
		/// <summary>Indicates that a block size attribute was missing.</summary>
		MissingAttributeBlockSize = -67765, /* A block size attribute was missing. */
		/// <summary>Indicates that the output size attribute was not valid.</summary>
		InvalidAttributeOutputSize = -67766,    /* An output size attribute was not valid. */
		/// <summary>Indicates that an output size attribute was missing.</summary>
		MissingAttributeOutputSize = -67767,    /* An output size attribute was missing. */
		/// <summary>Indicates that the rounds attribute was not valid.</summary>
		InvalidAttributeRounds = -67768,    /* The number of rounds attribute was not valid. */
		/// <summary>Indicates that a rounds attribute was missing.</summary>
		MissingAttributeRounds = -67769,    /* The number of rounds attribute was missing. */
		/// <summary>Indicates that the security algorithm was called with invalid parameters.</summary>
		InvalidAlgorithmParms = -67770, /* An algorithm parameters attribute was not valid. */
		/// <summary>Indicates that required parameters for a security algorithm were missing.</summary>
		MissingAlgorithmParms = -67771, /* An algorithm parameters attribute was missing. */
		/// <summary>Indicates that the label attribute was not valid.</summary>
		InvalidAttributeLabel = -67772, /* A label attribute was not valid. */
		/// <summary>Indicates that a label attribute was missing.</summary>
		MissingAttributeLabel = -67773, /* A label attribute was missing. */
		/// <summary>Indicates that the key type attribute was not valid.</summary>
		InvalidAttributeKeyType = -67774,   /* A key type attribute was not valid. */
		/// <summary>Indicates that a key type attribute was missing.</summary>
		MissingAttributeKeyType = -67775,   /* A key type attribute was missing. */
		/// <summary>Indicates that the mode attribute was not valid.</summary>
		InvalidAttributeMode = -67776,  /* A mode attribute was not valid. */
		/// <summary>Indicates that a mode attribute was missing.</summary>
		MissingAttributeMode = -67777,  /* A mode attribute was missing. */
		/// <summary>Indicates that the effective bits attribute was not valid.</summary>
		InvalidAttributeEffectiveBits = -67778, /* An effective bits attribute was not valid. */
		/// <summary>Indicates that an effective bits attribute was missing.</summary>
		MissingAttributeEffectiveBits = -67779, /* An effective bits attribute was missing. */
		/// <summary>Indicates that the start date attribute was not valid.</summary>
		InvalidAttributeStartDate = -67780, /* A start date attribute was not valid. */
		/// <summary>Indicates that a start date attribute was missing.</summary>
		MissingAttributeStartDate = -67781, /* A start date attribute was missing. */
		/// <summary>Indicates that the end date attribute was not valid.</summary>
		InvalidAttributeEndDate = -67782,   /* An end date attribute was not valid. */
		/// <summary>Indicates that an end date attribute was missing.</summary>
		MissingAttributeEndDate = -67783,   /* An end date attribute was missing. */
		/// <summary>Indicates that the version attribute was not valid.</summary>
		InvalidAttributeVersion = -67784,   /* A version attribute was not valid. */
		/// <summary>Indicates that a versions attribute was missing.</summary>
		MissingAttributeVersion = -67785,   /* A version attribute was missing. */
		/// <summary>Indicates that a prime attribute was not valid.</summary>
		InvalidAttributePrime = -67786, /* A prime attribute was not valid. */
		/// <summary>Indicates that a prime attribute was missing.</summary>
		MissingAttributePrime = -67787, /* A prime attribute was missing. */
		/// <summary>Indicates that the base attribute was not valid.</summary>
		InvalidAttributeBase = -67788,  /* A base attribute was not valid. */
		/// <summary>Indicates that a base attribute was missing.</summary>
		MissingAttributeBase = -67789,  /* A base attribute was missing. */
		/// <summary>Indicates that the subprime attribute was not valid.</summary>
		InvalidAttributeSubprime = -67790,  /* A subprime attribute was not valid. */
		/// <summary>Indicates that a subprime attribute was missing.</summary>
		MissingAttributeSubprime = -67791,  /* A subprime attribute was missing. */
		/// <summary>Indicates that the iteration count attribute was not valid.</summary>
		InvalidAttributeIterationCount = -67792,    /* An iteration count attribute was not valid. */
		/// <summary>Indicates that an iteration count attribute was missing.</summary>
		MissingAttributeIterationCount = -67793,    /* An iteration count attribute was missing. */
		/// <summary>Indicates that the database handle attribute was missing or not valid.</summary>
		InvalidAttributeDLDBHandle = -67794,    /* A database handle attribute was not valid. */
		/// <summary>Indicates that a database handle attribute was missing.</summary>
		MissingAttributeDLDBHandle = -67795,    /* A database handle attribute was missing. */
		/// <summary>Indicates that the access credentials attribute was not valid.</summary>
		InvalidAttributeAccessCredentials = -67796, /* An access credentials attribute was not valid. */
		/// <summary>Indicates that the access credentials were missing.</summary>
		MissingAttributeAccessCredentials = -67797, /* An access credentials attribute was missing. */
		/// <summary>Indicates that the public key format attribute was not valid.</summary>
		InvalidAttributePublicKeyFormat = -67798,   /* A public key format attribute was not valid. */
		/// <summary>Indicates that a public key format attribute was missing.</summary>
		MissingAttributePublicKeyFormat = -67799,   /* A public key format attribute was missing. */
		/// <summary>Indicates that the private key attribute was not valid.</summary>
		InvalidAttributePrivateKeyFormat = -67800,  /* A private key format attribute was not valid. */
		/// <summary>Indicates that a private key format attribute was missing.</summary>
		MissingAttributePrivateKeyFormat = -67801,  /* A private key format attribute was missing. */
		/// <summary>Indicates that the symmetric key format attribute was not valid.</summary>
		InvalidAttributeSymmetricKeyFormat = -67802,    /* A symmetric key format attribute was not valid. */
		/// <summary>Indicates that a symmetric key format attribute was missing.</summary>
		MissingAttributeSymmetricKeyFormat = -67803,    /* A symmetric key format attribute was missing. */
		/// <summary>Indicates that a wrapped key format attribute was not valid.</summary>
		InvalidAttributeWrappedKeyFormat = -67804,  /* A wrapped key format attribute was not valid. */
		/// <summary>Indicates that a wrapped key format attribute was missing.</summary>
		MissingAttributeWrappedKeyFormat = -67805,  /* A wrapped key format attribute was missing. */
		/// <summary>Indicates that a staged operation is in progress, so a time stamp is not (yet) appropriate.</summary>
		StagedOperationInProgress = -67806, /* A staged operation is in progress. */
		/// <summary>Indicates that a staged operation has not yet started, so a time stamp is not (yet) appropriate.</summary>
		StagedOperationNotStarted = -67807, /* A staged operation was not started. */
		/// <summary>Indicates that a verification failed.</summary>
		VerifyFailed = -67808,  /* A cryptographic verification failure has occurred. */
		/// <summary>Indicates that the query size is unknown.</summary>
		QuerySizeUnknown = -67809,  /* The query size is unknown. */
		/// <summary>Indicates mismatched block sizes.</summary>
		BlockSizeMismatch = -67810, /* A block size mismatch occurred. */
		/// <summary>Indicates that a public key was inconsistent.</summary>
		PublicKeyInconsistent = -67811, /* The public key was inconsistent. */
		/// <summary>Indicates that a device could not be verified.</summary>
		DeviceVerifyFailed = -67812,    /* A device verification failure has occurred. */
		/// <summary>Indicates an invalid login name </summary>
		InvalidLoginName = -67813,  /* An invalid login name was detected. */
		/// <summary>Indicates that the user was already logged in.</summary>
		AlreadyLoggedIn = -67814,   /* The user is already logged in. */
		/// <summary>Indicates that an invalid digest algorithm was specified.</summary>
		InvalidDigestAlgorithm = -67815,    /* An invalid digest algorithm was detected. */
		/// <summary>Indicates that the CRL group was not valid.</summary>
		InvalidCRLGroup = -67816,   /* An invalid CRL group was detected. */
		/// <summary>Indicates that a certificate could not operate.</summary>
		CertificateCannotOperate = -67817,  /* The certificate cannot operate. */
		/// <summary>Indicates that a certificate was expired.</summary>
		CertificateExpired = -67818,    /* An expired certificate was detected. */
		/// <summary>Indicates that a certificate was not yet valid.</summary>
		CertificateNotValidYet = -67819,    /* The certificate is not yet valid. */
		/// <summary>Indicates that a certificate was revoked.</summary>
		CertificateRevoked = -67820,    /* The certificate was revoked. */
		/// <summary>Indicates that a certificate was suspended.</summary>
		CertificateSuspended = -67821,  /* The certificate was suspended. */
		/// <summary>Indicates that credentials were not sufficient for access.</summary>
		InsufficientCredentials = -67822,   /* Insufficient credentials were detected. */
		/// <summary>Indicates that an invalid action was not attempted.</summary>
		InvalidAction = -67823, /* The action was not valid. */
		/// <summary>Indicates that the authority was not valid.</summary>
		InvalidAuthority = -67824,  /* The authority was not valid. */
		/// <summary>Indicates a verification action failure.</summary>
		VerifyActionFailed = -67825,    /* A verify action has failed. */
		/// <summary>Indicates that the cert authority was not valid.</summary>
		InvalidCertAuthority = -67826,  /* The certificate authority was not valid. */
		/// <summary>Indicates that the CRL authority was not valid.</summary>
		InvalidCRLAuthority = -67827,   /* The CRL authority was not valid. */
#if MONOMAC
		/// <summary>To be added.</summary>
		[Obsolete ("Use InvalidCRLAuthority.")]
		InvaldCRLAuthority = InvalidCRLAuthority,
#endif
		/// <summary>Indicates that the CRL encoding was not valid.</summary>
		InvalidCRLEncoding = -67828,    /* The CRL encoding was not valid. */
		/// <summary>Indicates that the CRL type was not valid..</summary>
		InvalidCRLType = -67829,    /* The CRL type was not valid. */
		/// <summary>Indicates that the CRL was not valid.</summary>
		InvalidCRL = -67830,    /* The CRL was not valid. */
		/// <summary>Indicates an invalid form type.</summary>
		InvalidFormType = -67831,   /* The form type was not valid. */
		/// <summary>Indicates an invalid identifier.</summary>
		InvalidID = -67832, /* The ID was not valid. */
		/// <summary>Indicates an invalid identifier.</summary>
		InvalidIdentifier = -67833, /* The identifier was not valid. */
		/// <summary>Indicates an invalid index.</summary>
		InvalidIndex = -67834,  /* The index was not valid. */
		/// <summary>Indicates invalid policy identifiers.</summary>
		InvalidPolicyIdentifiers = -67835,  /* The policy identifiers are not valid. */
		/// <summary>Indicates an invalid time string.</summary>
		InvalidTimeString = -67836, /* The time specified was not valid. */
		/// <summary>Indicates an invalid reason.</summary>
		InvalidReason = -67837, /* The trust policy reason was not valid. */
		/// <summary>Indicates invalid inputs to a request.</summary>
		InvalidRequestInputs = -67838,  /* The request inputs are not valid. */
		/// <summary>Indicates an invalid response vector.</summary>
		InvalidResponseVector = -67839, /* The response vector was not valid. */
		/// <summary>Indicates an invalid stop-on policy.</summary>
		InvalidStopOnPolicy = -67840,   /* The stop-on policy was not valid. */
		/// <summary>Indicates an invalid tuple.</summary>
		InvalidTuple = -67841,  /* The tuple was not valid. */
		/// <summary>Indicates that multiple values were used where not supported.</summary>
		MultipleValuesUnsupported = -67842, /* Multiple values are not supported. */
		/// <summary>Indicates that the policy is not trusted.</summary>
		NotTrusted = -67843,    /* The trust policy was not trusted. */
		/// <summary>Indicates that no default authority was found.</summary>
		NoDefaultAuthority = -67844,    /* No default authority was detected. */
		/// <summary>Indicates that a trust policy rejected a form.</summary>
		RejectedForm = -67845,  /* The trust policy had a rejected form. */
		/// <summary>Indicates that a request was lost.</summary>
		RequestLost = -67846,   /* The request was lost. */
		/// <summary>Indicates that a request was rejected.</summary>
		RequestRejected = -67847,   /* The request was rejected. */
		/// <summary>Indicates that an unsupported address type was encountered.</summary>
		UnsupportedAddressType = -67848,    /* The address type is not supported. */
		/// <summary>Indicates that an unsupported service was requested.</summary>
		UnsupportedService = -67849,    /* The service is not supported. */
		/// <summary>Indicates an invalid tuple group.</summary>
		InvalidTupleGroup = -67850, /* The tuple group was not valid. */
		/// <summary>Indicates that the base ACLs were not valid.</summary>
		InvalidBaseACLs = -67851,   /* The base ACLs are not valid. */
		/// <summary>Indicates an invalid credentials tuple.</summary>
		InvalidTupleCredentials = -67852,   /* The tuple credentials are not valid. */
#if MONOMAC
		/// <summary>To be added.</summary>
		[Obsolete ("Use InvalidTupleCredentials.")]
		InvalidTupleCredendtials = InvalidTupleCredentials,
#endif
		/// <summary>Indicates an invalid encoding.</summary>
		InvalidEncoding = -67853,   /* The encoding was not valid. */
		/// <summary>Indicates an invalid identity period.</summary>
		InvalidValidityPeriod = -67854, /* The validity period was not valid. */
		/// <summary>Indicates an invalid requestor.</summary>
		InvalidRequestor = -67855,  /* The requestor was not valid. */
		/// <summary>Indicates that a request descriptor was invalid.</summary>
		RequestDescriptor = -67856, /* The request descriptor was not valid. */
		/// <summary>Indicates that the bundle info was not valid.</summary>
		InvalidBundleInfo = -67857, /* The bundle information was not valid. */
		/// <summary>Indicates that a CRL index was not valid.</summary>
		InvalidCRLIndex = -67858,   /* The CRL index was not valid. */
		/// <summary>Indicates that no field values were found.</summary>
		NoFieldValues = -67859, /* No field values were detected. */
		/// <summary>Indicates that an unsupported field format was encountered.</summary>
		UnsupportedFieldFormat = -67860,    /* The field format is not supported. */
		/// <summary>Indicates that unsupported index information was encountered.</summary>
		UnsupportedIndexInfo = -67861,  /* The index information is not supported. */
		/// <summary>Indicates that unsupported locality was encountered.</summary>
		UnsupportedLocality = -67862,   /* The locality is not supported. */
		/// <summary>Indicates that an unsupported number of attributes was encountered.</summary>
		UnsupportedNumAttributes = -67863,  /* The number of attributes is not supported. */
		/// <summary>Indicates that an unsupported number of indices was encountered.</summary>
		UnsupportedNumIndexes = -67864, /* The number of indexes is not supported. */
		/// <summary>Indicates that an unsupported number of record types was encountered.</summary>
		UnsupportedNumRecordTypes = -67865, /* The number of record types is not supported. */
		/// <summary>Indicates that a field was specified multiple times.</summary>
		FieldSpecifiedMultiple = -67866,    /* Too many fields were specified. */
		/// <summary>Indicates that a field format was not compatible.</summary>
		IncompatibleFieldFormat = -67867,   /* The field format was incompatible. */
		/// <summary>Indicates an invalid parsing module.</summary>
		InvalidParsingModule = -67868,  /* The parsing module was not valid. */
		/// <summary>Indicates that a database was locked.</summary>
		DatabaseLocked = -67869,    /* The database is locked. */
		/// <summary>Indicates that a data store was open.</summary>
		DatastoreIsOpen = -67870,   /* The data store is open. */
		/// <summary>Indicates that a value was missing.</summary>
		MissingValue = -67871,  /* A missing value was detected. */
		/// <summary>Indicates that unsupported query limits were encountered.</summary>
		UnsupportedQueryLimits = -67872,    /* The query limits are not supported. */
		/// <summary>Indicates that an unsupported number of selection predicates was encountered.</summary>
		UnsupportedNumSelectionPreds = -67873,  /* The number of selection predicates is not supported. */
		/// <summary>Indicates that an unsupported operator was encountered.</summary>
		UnsupportedOperator = -67874,   /* The operator is not supported. */
		/// <summary>Indicates that a database location was not valid.</summary>
		InvalidDBLocation = -67875, /* The database location is not valid. */
		/// <summary>Indicates that an invalid access request was made.</summary>
		InvalidAccessRequest = -67876,  /* The access request is not valid. */
		/// <summary>Indicates invalid index info.</summary>
		InvalidIndexInfo = -67877,  /* The index information is not valid. */
		/// <summary>Indicates an invalid owner.</summary>
		InvalidNewOwner = -67878,   /* The new owner is not valid. */
		/// <summary>Indicates an invalid modification mode.</summary>
		InvalidModifyMode = -67879, /* The modify mode is not valid. */
		/// <summary>Indicates that a required extension was missing.</summary>
		MissingRequiredExtension = -67880,  /* A required certificate extension is missing. */
		/// <summary>Indicates that a noncritical extended key was encountered where they are disallowed.</summary>
		ExtendedKeyUsageNotCritical = -67881,   /* The extended key usage extension was not marked critical. */
		/// <summary>Indicates that a timestamp was missing.</summary>
		TimestampMissing = -67882,  /* A timestamp was expected but was not found. */
		/// <summary>Indicates that a timestamp was invalid.</summary>
		TimestampInvalid = -67883,  /* The timestamp was not valid. */
		/// <summary>Indicates that a timestamp was not trusted.</summary>
		TimestampNotTrusted = -67884,   /* The timestamp was not trusted. */
		/// <summary>Indicates that a timestamp service was unavailable.</summary>
		TimestampServiceNotAvailable = -67885,  /* The timestamp service is not available. */
		/// <summary>Indicates that a bad algorithm ID was found in the timestamp.</summary>
		TimestampBadAlg = -67886,   /* An unrecognized or unsupported Algorithm Identifier in timestamp. */
		/// <summary>Indicates that the timestamp used an incorrect format.</summary>
		TimestampBadRequest = -67887,   /* The timestamp transaction is not permitted or supported. */
		/// <summary>Indicates that the timestamp used an incorrect format.</summary>
		TimestampBadDataFormat = -67888,    /* The timestamp data submitted has the wrong format. */
		/// <summary>Indicates that a timestamp time was not available.</summary>
		TimestampTimeNotAvailable = -67889, /* The time source for the Timestamp Authority is not available. */
		/// <summary>Indicates that the policy is not accepted by the timestamp authority.</summary>
		TimestampUnacceptedPolicy = -67890, /* The requested policy is not supported by the Timestamp Authority. */
		/// <summary>Indicates that an unaccepted extension was requested of a timestamp server.</summary>
		TimestampUnacceptedExtension = -67891,  /* The requested extension is not supported by the Timestamp Authority. */
		/// <summary>Indicates that additional information was not available for the timestamp.</summary>
		TimestampAddInfoNotAvailable = -67892,  /* The additional information requested is not available. */
		/// <summary>Indicates a timestamp system failure.</summary>
		TimestampSystemFailure = -67893,    /* The timestamp request cannot be handled due to system failure. */
		/// <summary>Indicates that a certificate signing time was missing.</summary>
		SigningTimeMissing = -67894,    /* A signing time was expected but was not found. */
		/// <summary>Indicates that a timestamp was rejected.</summary>
		TimestampRejection = -67895,    /* A timestamp transaction was rejected. */
		/// <summary>Indicates that a timestamp transaction was waiting.</summary>
		TimestampWaiting = -67896,  /* A timestamp transaction is waiting. */
		/// <summary>Indicates that a timestamp warning has been issued.</summary>
		TimestampRevocationWarning = -67897,    /* A timestamp authority revocation warning was issued. */
		/// <summary>Indicates that a timestamp revocation notification has been issued.</summary>
		TimestampRevocationNotification = -67898,   /* A timestamp authority revocation notification was issued. */
		CertificatePolicyNotAllowed = -67899,
		CertificateNameNotAllowed = -67900,
		CertificateValidityPeriodTooLong = -67901,
		CertificateIsCA = -67902,
		CertificateDuplicateExtension = -67903,
		MissingQualifiedCertStatement = -67904,
	}

	// typedef uint32_t SecPadding;
	// values are defined in Security.framework/Headers/SecKey.h 
	/// <summary>An enumeration whose values specify the type of padding to be used in encryption and decryption.</summary>
	public enum SecPadding {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		PKCS1 = 1,

		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		OAEP = 2,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Raw = 0x4000,

		/// <summary>To be added.</summary>
		[Obsolete ("Don't use hash algorithm.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		PKCS1MD2 = 0x8000,

		/// <summary>To be added.</summary>
		[Obsolete ("Don't use hash algorithm.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		PKCS1MD5 = 0x8001,

		/// <summary>To be added.</summary>
		PKCS1SHA1 = 0x8002,

		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA224 = 0x8003,
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA256 = 0x8004,
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA384 = 0x8005,
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA512 = 0x8006,
	}

	// typedef uint32_t SecTrustResultType;
	// values are defined in Security.framework/Headers/SecTrust.h 
	/// <summary>Result code for SecTrust.Evaluate</summary>
	///     <remarks>In general both Proceed and Unspecified means you can trust the certificate, other values means it should not be trusted.</remarks>
	[NativeName ("SecTrustResultType")]
	public enum SecTrustResult {
		/// <summary>The supplied data cannot be used to determine if the certificate can be trusted.</summary>
		Invalid,
		/// <summary>The certificate is trusted and the system is telling you to proceed with its intended usage.</summary>
		Proceed,

		/// <summary>Developers should not use this deprecated field. </summary>
		[Deprecated (PlatformName.iOS, 7, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		Confirm,
		/// <summary>Trust for this certificate is being denied.</summary>
		Deny,
		/// <summary>The certificate is trusted and the default system action should be executed. In general it means it's trusted and you can go on.</summary>
		Unspecified,
		/// <summary>Not enough information is available to trust this certificate. If extra information is supplied then it could be trusted (or not).</summary>
		RecoverableTrustFailure,
		/// <summary>The certificate could not be trace back to a trusted root.</summary>
		FatalTrustFailure,
		/// <summary>An error occured while trying to determine the certificate trust.</summary>
		ResultOtherError,
	}

	// convenience enum mapping to kSecUseAuthenticationUI* fields
	/// <summary>Enumeration whose values represent valid options for <see cref="Security.SecRecord.AuthenticationUI" />.</summary>
	[MacCatalyst (13, 1)]
	public enum SecAuthenticationUI {
		/// <summary>To be added.</summary>
		NotSet = -1,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Field ("kSecUseAuthenticationUIAllow")]
		Allow,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Field ("kSecUseAuthenticationUIFail")]
		Fail,

		/// <summary>To be added.</summary>
		[Field ("kSecUseAuthenticationUISkip")]
		Skip,
	}

	/// <summary>Enumeration defining valid options for <see cref="Security.SecRecord.TokenID" />.</summary>
	[MacCatalyst (13, 1)]
	public enum SecTokenID {
		/// <summary>To be added.</summary>
		None = 0,

		/// <summary>To be added.</summary>
		[Field ("kSecAttrTokenIDSecureEnclave")]
		SecureEnclave,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SecKeyOperationType : long {
		/// <summary>To be added.</summary>
		Sign = 0,
		/// <summary>To be added.</summary>
		Verify = 1,
		/// <summary>To be added.</summary>
		Encrypt = 2,
		/// <summary>To be added.</summary>
		Decrypt = 3,
		/// <summary>To be added.</summary>
		KeyExchange = 4,
	}

	// untyped enum in Security.framework/Headers/SecPolicy.h but the API use CFOptionFlags
	// which is defined as in CFBase.h (do not trust Apple web documentation)
	/// <summary>Enumerates certificate revocation methods.</summary>
	[Flags]
	[Native]
	public enum SecRevocation : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		OCSPMethod = 1,
		/// <summary>To be added.</summary>
		CRLMethod = 2,
		/// <summary>To be added.</summary>
		PreferCRL = 4,
		/// <summary>To be added.</summary>
		RequirePositiveResponse = 8,
		/// <summary>To be added.</summary>
		NetworkAccessDisabled = 16,
		/// <summary>To be added.</summary>
		UseAnyAvailableMethod = OCSPMethod | CRLMethod,
	}
}
