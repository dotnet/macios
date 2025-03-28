//
// Enums.cs: enumeration definitions for Foundation
//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using ObjCRuntime;

namespace Foundation {

	[Native]
	public enum NSStringEncoding : ulong {
		/// <summary>ASCII encoding contains, 7-bit of information stored in 8 bits.</summary>
		ASCIIStringEncoding = 1,
		/// <summary>To be added.</summary>
		NEXTSTEP = 2,
		/// <summary>To be added.</summary>
		JapaneseEUC = 3,
		/// <summary>8-bit based variable-length character encoding for Unicode. </summary>
		UTF8 = 4,
		/// <summary>8-bit ISO/IEC 8859-1, also knows as Latin1 encoding.</summary>
		ISOLatin1 = 5,
		/// <summary>To be added.</summary>
		Symbol = 6,
		/// <summary>To be added.</summary>
		NonLossyASCII = 7,
		/// <summary>To be added.</summary>
		ShiftJIS = 8,
		/// <summary>8-bit ISO/IEC 8859-2, also knows as Latin2 encoding.</summary>
		ISOLatin2 = 9,
		/// <summary>To be added.</summary>
		Unicode = 10,
		/// <summary>To be added.</summary>
		WindowsCP1251 = 11,
		/// <summary>To be added.</summary>
		WindowsCP1252 = 12,
		/// <summary>To be added.</summary>
		WindowsCP1253 = 13,
		/// <summary>To be added.</summary>
		WindowsCP1254 = 14,
		/// <summary>To be added.</summary>
		WindowsCP1250 = 15,
		/// <summary>To be added.</summary>
		ISO2022JP = 21,
		/// <summary>To be added.</summary>
		MacOSRoman = 30,
		/// <summary>16 bit-based variable length encoding, blocks are interpreted as big endian.</summary>
		UTF16BigEndian = 0x90000100,
		/// <summary>16 bit-based variable length encoding, blocks are interpreted as litle endian. </summary>
		UTF16LittleEndian = 0x94000100,
		/// <summary>32-bit unicode encoding.</summary>
		UTF32 = 0x8c000100,
		/// <summary>32 bit encoding, blocks are interpreted as big endian.</summary>
		UTF32BigEndian = 0x98000100,
		/// <summary>32 bit encoding, blocks are interpreted as little endian.</summary>
		UTF32LittleEndian = 0x9c000100,
	};

	[Native]
	public enum NSStringCompareOptions : ulong {
		/// <summary>To be added.</summary>
		CaseInsensitiveSearch = 1,
		/// <summary>To be added.</summary>
		LiteralSearch = 2,
		/// <summary>To be added.</summary>
		BackwardsSearch = 4,
		/// <summary>To be added.</summary>
		AnchoredSearch = 8,
		/// <summary>To be added.</summary>
		NumericSearch = 64,
		/// <summary>To be added.</summary>
		DiacriticInsensitiveSearch = 128,
		/// <summary>To be added.</summary>
		WidthInsensitiveSearch = 256,
		/// <summary>To be added.</summary>
		ForcedOrderingSearch = 512,
		/// <summary>To be added.</summary>
		RegularExpressionSearch = 1024,
	}

	[Native]
	public enum NSUrlCredentialPersistence : ulong {
		None,
		ForSession,
		Permanent,
		Synchronizable,
	}

#if MONOMAC

	/// <summary>The bundle's architecture.</summary>
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	public enum NSBundleExecutableArchitecture {
		/// <summary>x86</summary>
		I386 = 0x00000007,
		/// <summary>PPC</summary>
		PPC = 0x00000012,
		/// <summary>x86-64</summary>
		X86_64 = 0x01000007,
		/// <summary>PPC64</summary>
		PPC64 = 0x01000012,
		ARM64 = 0x0100000c,
	}
#endif

	[Native]
	public enum NSComparisonResult : long {
		/// <summary>To be added.</summary>
		Ascending = -1,
		/// <summary>To be added.</summary>
		Same,
		/// <summary>To be added.</summary>
		Descending,
	}

	[Native]
	public enum NSUrlRequestCachePolicy : ulong {
		UseProtocolCachePolicy = 0,
		ReloadIgnoringLocalCacheData = 1,
		ReloadIgnoringLocalAndRemoteCacheData = 4, // Unimplemented
		ReloadIgnoringCacheData = ReloadIgnoringLocalCacheData,

		ReturnCacheDataElseLoad = 2,
		ReturnCacheDataDoNotLoad = 3,

		ReloadRevalidatingCacheData = 5, // Unimplemented
	}

	[Native]
	public enum NSUrlCacheStoragePolicy : ulong {
		Allowed,
		AllowedInMemoryOnly,
		NotAllowed,
	}

	[Native]
	public enum NSStreamStatus : ulong {
		/// <summary>The stream is not yet open.</summary>
		NotOpen = 0,
		/// <summary>The stream is in the opening state.</summary>
		Opening = 1,
		/// <summary>The stream has been opened.</summary>
		Open = 2,
		/// <summary>The stream is reading.</summary>
		Reading = 3,
		/// <summary>The stream is writing.</summary>
		Writing = 4,
		/// <summary>At the end.</summary>
		AtEnd = 5,
		/// <summary>The stream is closed.</summary>
		Closed = 6,
		/// <summary>Error</summary>
		Error = 7,
	}

	[Native]
	public enum NSPropertyListFormat : ulong {
		OpenStep = 1,
		Xml = 100,
		Binary = 200,
	}

	[Native]
	public enum NSPropertyListMutabilityOptions : ulong {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2,
	}

	// Should mirror NSPropertyListMutabilityOptions
	[Native]
	public enum NSPropertyListWriteOptions : ulong {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2,
	}

	// Should mirror NSPropertyListMutabilityOptions, but currently
	// not implemented (always use Immutable/0)
	[Native]
	public enum NSPropertyListReadOptions : ulong {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2,
	}

	[Native]
	[Flags]
	public enum NSMachPortRights : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		SendRight = (1 << 0),
		/// <summary>To be added.</summary>
		ReceiveRight = (1 << 1),
	}

	[Native]
	public enum NSNetServicesStatus : long {
		UnknownError = -72000,
		CollisionError = -72001,
		NotFoundError = -72002,
		ActivityInProgress = -72003,
		BadArgumentError = -72004,
		CancelledError = -72005,
		InvalidError = -72006,
		TimeoutError = -72007,
		MissingRequiredConfigurationError = -72008,
	}

	[Flags]
	[Native]
	public enum NSNetServiceOptions : ulong {
		NoAutoRename = 1 << 0,
		ListenForConnections = 1 << 1,
	}

	[Native]
	public enum NSDateFormatterStyle : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Short,
		/// <summary>To be added.</summary>
		Medium,
		/// <summary>To be added.</summary>
		Long,
		/// <summary>To be added.</summary>
		Full,
	}

	[Native]
	public enum NSDateFormatterBehavior : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Mode_10_0 = 1000,
		/// <summary>To be added.</summary>
		Mode_10_4 = 1040,
	}

	[Native]
	public enum NSHttpCookieAcceptPolicy : ulong {
		/// <summary>To be added.</summary>
		Always,
		/// <summary>To be added.</summary>
		Never,
		/// <summary>To be added.</summary>
		OnlyFromMainDocumentDomain,
	}

	[Flags]
	[Native]
	public enum NSCalendarUnit : ulong {
		/// <summary>To be added.</summary>
		Era = 2,
		/// <summary>To be added.</summary>
		Year = 4,
		/// <summary>To be added.</summary>
		Month = 8,
		/// <summary>To be added.</summary>
		Day = 16,
		/// <summary>To be added.</summary>
		Hour = 32,
		/// <summary>To be added.</summary>
		Minute = 64,
		/// <summary>To be added.</summary>
		Second = 128,
		/// <summary>Developers should not use this deprecated field. </summary>
		[Deprecated (PlatformName.MacOSX, 10, 10)]
		[Deprecated (PlatformName.iOS, 8, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		Week = 256,
		/// <summary>To be added.</summary>
		Weekday = 512,
		/// <summary>To be added.</summary>
		WeekdayOrdinal = 1024,
		/// <summary>To be added.</summary>
		Quarter = 2048,

		/// <summary>To be added.</summary>
		WeekOfMonth = (1 << 12),
		/// <summary>To be added.</summary>
		WeekOfYear = (1 << 13),
		/// <summary>To be added.</summary>
		YearForWeakOfYear = (1 << 14),

		/// <summary>To be added.</summary>
		Nanosecond = (1 << 15),
		DayOfYear = (1 << 16),

		/// <summary>To be added.</summary>
		Calendar = (1 << 20),
		/// <summary>To be added.</summary>
		TimeZone = (1 << 21),
	}

	[Flags]
	[Native]
	public enum NSDataReadingOptions : ulong {
		/// <summary>Use the kernel's virtual memory map to load the file, if possible. If sucessful, this replaces read/write memory that can be very expensive with discardable memory that is backed by a file.</summary>
		Mapped = 1 << 0,
		/// <summary>Notify the kernel that it should not try to cache the contents of this file in its buffer cache.</summary>
		Uncached = 1 << 1,
		/// <summary>Force NSData to try to use the kernel's mapping support to load the file.   If sucessful, this replaces read/write memory that can be very expensive with discardable memory that is backed by a file.</summary>
		MappedAlways = 1 << 3,
	}

	[Flags]
	[Native]
	public enum NSDataWritingOptions : ulong {
		/// <summary>To be added.</summary>
		Atomic = 1,

		/// <summary>To be added.</summary>
		WithoutOverwriting = 2,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FileProtectionNone = 0x10000000,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FileProtectionComplete = 0x20000000,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FileProtectionMask = 0xf0000000,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FileProtectionCompleteUnlessOpen = 0x30000000,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FileProtectionCompleteUntilFirstUserAuthentication = 0x40000000,
		[iOS (17, 0), NoMac, MacCatalyst (17, 0), TV (17, 0)]
		FileProtectionCompleteWhenUserInactive = 0x50000000,
	}

	public delegate void NSSetEnumerator (NSObject obj, ref bool stop);

	[Native]
	public enum NSOperationQueuePriority : long {
		/// <summary>To be added.</summary>
		VeryLow = -8,
		/// <summary>To be added.</summary>
		Low = -4,
		/// <summary>To be added.</summary>
		Normal = 0,
		/// <summary>To be added.</summary>
		High = 4,
		/// <summary>To be added.</summary>
		VeryHigh = 8,
	}

	[Flags]
	[Native]
	public enum NSNotificationCoalescing : ulong {
		NoCoalescing = 0,
		CoalescingOnName = 1,
		CoalescingOnSender = 2,
	}

	[Native]
	public enum NSPostingStyle : ulong {
		/// <summary>To be added.</summary>
		PostWhenIdle = 1,
		/// <summary>To be added.</summary>
		PostASAP = 2,
		/// <summary>To be added.</summary>
		Now = 3,
	}

	[Flags]
	[Native]
	public enum NSDataSearchOptions : ulong {
		/// <summary>Starts search from the end, instead of the start.</summary>
		SearchBackwards = 1,
		/// <summary>Limits the search to the start (or end if SearchBackwards is specified)</summary>
		SearchAnchored = 2,
	}

	[Native]
	public enum NSExpressionType : ulong {
		/// <summary>To be added.</summary>
		ConstantValue = 0,
		/// <summary>To be added.</summary>
		EvaluatedObject,
		/// <summary>To be added.</summary>
		Variable,
		/// <summary>To be added.</summary>
		KeyPath,
		/// <summary>To be added.</summary>
		Function,
		/// <summary>To be added.</summary>
		UnionSet,
		/// <summary>To be added.</summary>
		IntersectSet,
		/// <summary>To be added.</summary>
		MinusSet,
		/// <summary>To be added.</summary>
		Subquery = 13,
		/// <summary>To be added.</summary>
		NSAggregate,
		/// <summary>To be added.</summary>
		AnyKey = 15,
		/// <summary>To be added.</summary>
		Block = 19,
		/// <summary>To be added.</summary>
		Conditional = 20,
	}

	public enum NSCocoaError : int {
		/// <summary>To be added.</summary>
		None,

		/// <summary>To be added.</summary>
		FileNoSuchFile = 4,
		/// <summary>To be added.</summary>
		FileLocking = 255,
		/// <summary>To be added.</summary>
		FileReadUnknown = 256,
		/// <summary>To be added.</summary>
		FileReadNoPermission = 257,
		/// <summary>To be added.</summary>
		FileReadInvalidFileName = 258,
		/// <summary>To be added.</summary>
		FileReadCorruptFile = 259,
		/// <summary>To be added.</summary>
		FileReadNoSuchFile = 260,
		/// <summary>To be added.</summary>
		FileReadInapplicableStringEncoding = 261,
		/// <summary>To be added.</summary>
		FileReadUnsupportedScheme = 262,
		/// <summary>To be added.</summary>
		FileReadTooLarge = 263,
		/// <summary>To be added.</summary>
		FileReadUnknownStringEncoding = 264,
		/// <summary>To be added.</summary>
		FileWriteUnknown = 512,
		/// <summary>To be added.</summary>
		FileWriteNoPermission = 513,
		/// <summary>To be added.</summary>
		FileWriteInvalidFileName = 514,
		/// <summary>To be added.</summary>
		FileWriteFileExists = 516,
		/// <summary>To be added.</summary>
		FileWriteInapplicableStringEncoding = 517,
		/// <summary>To be added.</summary>
		FileWriteUnsupportedScheme = 518,
		/// <summary>To be added.</summary>
		FileWriteOutOfSpace = 640,
		/// <summary>To be added.</summary>
		FileWriteVolumeReadOnly = 642,

#if MONOMAC
		/// <summary>To be added.</summary>
		FileManagerUnmountUnknownError = 768,
		/// <summary>To be added.</summary>
		FileManagerUnmountBusyError = 769,
#endif

		/// <summary>To be added.</summary>
		KeyValueValidation = 1024,
		/// <summary>To be added.</summary>
		Formatting = 2048,
		/// <summary>To be added.</summary>
		UserCancelled = 3072,
		/// <summary>To be added.</summary>
		FeatureUnsupported = 3328,
		/// <summary>To be added.</summary>
		ExecutableNotLoadable = 3584,
		/// <summary>To be added.</summary>
		ExecutableArchitectureMismatch = 3585,
		/// <summary>To be added.</summary>
		ExecutableRuntimeMismatch = 3586,
		/// <summary>To be added.</summary>
		ExecutableLoad = 3587,
		/// <summary>To be added.</summary>
		ExecutableLink = 3588,
		/// <summary>To be added.</summary>
		FileErrorMinimum = 0,
		/// <summary>To be added.</summary>
		FileErrorMaximum = 1023,
		/// <summary>To be added.</summary>
		ValidationErrorMinimum = 1024,
		/// <summary>To be added.</summary>
		ValidationErrorMaximum = 2047,
		/// <summary>To be added.</summary>
		ExecutableErrorMinimum = 3584,
		/// <summary>To be added.</summary>
		ExecutableErrorMaximum = 3839,
		/// <summary>To be added.</summary>
		FormattingErrorMinimum = 2048,
		/// <summary>To be added.</summary>
		FormattingErrorMaximum = 2559,

		/// <summary>To be added.</summary>
		PropertyListReadCorrupt = 3840,
		/// <summary>To be added.</summary>
		PropertyListReadUnknownVersion = 3841,
		/// <summary>To be added.</summary>
		PropertyListReadStream = 3842,
		/// <summary>To be added.</summary>
		PropertyListWriteStream = 3851,
		/// <summary>To be added.</summary>
		PropertyListWriteInvalid = 3852,
		/// <summary>To be added.</summary>
		PropertyListErrorMinimum = 3840,
		/// <summary>To be added.</summary>
		PropertyListErrorMaximum = 4095,

		/// <summary>To be added.</summary>
		XpcConnectionInterrupted = 4097,
		/// <summary>To be added.</summary>
		XpcConnectionInvalid = 4099,
		/// <summary>To be added.</summary>
		XpcConnectionReplyInvalid = 4101,
		XpcConnectionCodeSigningRequirementFailure = 4102,
		/// <summary>To be added.</summary>
		XpcConnectionErrorMinimum = 4096,
		/// <summary>To be added.</summary>
		XpcConnectionErrorMaximum = 4224,

		/// <summary>To be added.</summary>
		UbiquitousFileUnavailable = 4353,
		/// <summary>To be added.</summary>
		UbiquitousFileNotUploadedDueToQuota = 4354,
		/// <summary>To be added.</summary>
		UbiquitousFileUbiquityServerNotAvailable = 4355,
		/// <summary>To be added.</summary>
		UbiquitousFileErrorMinimum = 4352,
		/// <summary>To be added.</summary>
		UbiquitousFileErrorMaximum = 4607,

		/// <summary>To be added.</summary>
		UserActivityHandoffFailedError = 4608,
		/// <summary>To be added.</summary>
		UserActivityConnectionUnavailableError = 4609,
		/// <summary>To be added.</summary>
		UserActivityRemoteApplicationTimedOutError = 4610,
		/// <summary>To be added.</summary>
		UserActivityHandoffUserInfoTooLargeError = 4611,

		/// <summary>To be added.</summary>
		UserActivityErrorMinimum = 4608,
		/// <summary>To be added.</summary>
		UserActivityErrorMaximum = 4863,

		/// <summary>To be added.</summary>
		CoderReadCorruptError = 4864,
		/// <summary>To be added.</summary>
		CoderValueNotFoundError = 4865,
		/// <summary>To be added.</summary>
		CoderInvalidValueError = 4866,
		/// <summary>To be added.</summary>
		CoderErrorMinimum = 4864,
		/// <summary>To be added.</summary>
		CoderErrorMaximum = 4991,

		/// <summary>To be added.</summary>
		BundleErrorMinimum = 4992,
		/// <summary>To be added.</summary>
		BundleErrorMaximum = 5119,

		/// <summary>To be added.</summary>
		BundleOnDemandResourceOutOfSpaceError = 4992,
		/// <summary>To be added.</summary>
		BundleOnDemandResourceExceededMaximumSizeError = 4993,
		/// <summary>To be added.</summary>
		BundleOnDemandResourceInvalidTagError = 4994,

		/// <summary>To be added.</summary>
		CloudSharingNetworkFailureError = 5120,
		/// <summary>To be added.</summary>
		CloudSharingQuotaExceededError = 5121,
		/// <summary>To be added.</summary>
		CloudSharingTooManyParticipantsError = 5122,
		/// <summary>To be added.</summary>
		CloudSharingConflictError = 5123,
		/// <summary>To be added.</summary>
		CloudSharingNoPermissionError = 5124,
		/// <summary>To be added.</summary>
		CloudSharingOtherError = 5375,
		/// <summary>To be added.</summary>
		CloudSharingErrorMinimum = 5120,
		/// <summary>To be added.</summary>
		CloudSharingErrorMaximum = 5375,

		CompressionFailedError = 5376,
		DecompressionFailedError = 5377,
		CompressionErrorMinimum = 5376,
		CompressionErrorMaximum = 5503,
	}

	// note: Make sure names are identical/consistent with CFNetworkErrors.*
	// they share the same values but there's more entries in CFNetworkErrors
	// so anything new probably already exists over there
	public enum NSUrlError : int {
		Unknown = -1,

		BackgroundSessionRequiresSharedContainer = -995,
		BackgroundSessionInUseByAnotherProcess = -996,
		BackgroundSessionWasDisconnected = -997,

		Cancelled = -999,
		BadURL = -1000,
		TimedOut = -1001,
		UnsupportedURL = -1002,
		CannotFindHost = -1003,
		CannotConnectToHost = -1004,
		NetworkConnectionLost = -1005,
		DNSLookupFailed = -1006,
		HTTPTooManyRedirects = -1007,
		ResourceUnavailable = -1008,
		NotConnectedToInternet = -1009,
		RedirectToNonExistentLocation = -1010,
		BadServerResponse = -1011,
		UserCancelledAuthentication = -1012,
		UserAuthenticationRequired = -1013,
		ZeroByteResource = -1014,
		CannotDecodeRawData = -1015,
		CannotDecodeContentData = -1016,
		CannotParseResponse = -1017,
		InternationalRoamingOff = -1018,
		CallIsActive = -1019,
		DataNotAllowed = -1020,
		RequestBodyStreamExhausted = -1021,
		AppTransportSecurityRequiresSecureConnection = -1022,

		FileDoesNotExist = -1100,
		FileIsDirectory = -1101,
		NoPermissionsToReadFile = -1102,
		DataLengthExceedsMaximum = -1103,
		FileOutsideSafeArea = -1104,

		SecureConnectionFailed = -1200,
		ServerCertificateHasBadDate = -1201,
		ServerCertificateUntrusted = -1202,
		ServerCertificateHasUnknownRoot = -1203,
		ServerCertificateNotYetValid = -1204,
		ClientCertificateRejected = -1205,
		ClientCertificateRequired = -1206,

		CannotLoadFromNetwork = -2000,

		// Download and file I/O errors
		CannotCreateFile = -3000,
		CannotOpenFile = -3001,
		CannotCloseFile = -3002,
		CannotWriteToFile = -3003,
		CannotRemoveFile = -3004,
		CannotMoveFile = -3005,
		DownloadDecodingFailedMidStream = -3006,
		DownloadDecodingFailedToComplete = -3007,
	}

	[Flags]
	[Native]
	public enum NSKeyValueObservingOptions : ulong {
		None = 0,
		/// <summary>Request that new values be delivered to the observer.</summary>
		New = 1,
		/// <summary>Requests that old values be delivered to the observer.</summary>
		Old = 2,
		/// <summary>Requests that both old and new values be delivered to the observer.</summary>
		OldNew = 3,
		/// <summary>Use this to send a notification on first use, without waiting for an actual change to take place.</summary>
		Initial = 4,
		/// <summary>If set, notifications are sent before and after the change to the property is made.   Otherwise the notifications are only sent after the property has been set.</summary>
		Prior = 8,
	}

	[Native]
	public enum NSKeyValueChange : ulong {
		/// <summary>The change is reported for setting a value in a property.</summary>
		Setting = 1,
		/// <summary>The change being reported is an insertion into a collection.</summary>
		Insertion,
		/// <summary>The change being reported is a removal from a collection.</summary>
		Removal,
		/// <summary>The change being reported is a set of changes in a collection.</summary>
		Replacement,
	}

	[Native]
	public enum NSKeyValueSetMutationKind : ulong {
		/// <summary>To be added.</summary>
		UnionSet = 1,
		/// <summary>To be added.</summary>
		MinusSet,
		/// <summary>To be added.</summary>
		IntersectSet,
		/// <summary>To be added.</summary>
		SetSet,
	}

	[Flags]
	[Native]
	public enum NSEnumerationOptions : ulong {
		/// <summary>To be added.</summary>
		SortConcurrent = 1,
		/// <summary>To be added.</summary>
		Reverse = 2,
	}

	[Flags]
	[Native]
	public enum NSStreamEvent : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		OpenCompleted = 1 << 0,
		/// <summary>To be added.</summary>
		HasBytesAvailable = 1 << 1,
		/// <summary>To be added.</summary>
		HasSpaceAvailable = 1 << 2,
		/// <summary>To be added.</summary>
		ErrorOccurred = 1 << 3,
		/// <summary>To be added.</summary>
		EndEncountered = 1 << 4,
	}

	[Native]
	public enum NSComparisonPredicateModifier : ulong {
		/// <summary>To be added.</summary>
		Direct,
		/// <summary>To be added.</summary>
		All,
		/// <summary>To be added.</summary>
		Any,
	}

	[Native]
	public enum NSPredicateOperatorType : ulong {
		/// <summary>To be added.</summary>
		LessThan,
		/// <summary>To be added.</summary>
		LessThanOrEqualTo,
		/// <summary>To be added.</summary>
		GreaterThan,
		/// <summary>To be added.</summary>
		GreaterThanOrEqualTo,
		/// <summary>To be added.</summary>
		EqualTo,
		/// <summary>To be added.</summary>
		NotEqualTo,
		/// <summary>To be added.</summary>
		Matches,
		/// <summary>To be added.</summary>
		Like,
		/// <summary>To be added.</summary>
		BeginsWith,
		/// <summary>To be added.</summary>
		EndsWith,
		/// <summary>To be added.</summary>
		In,
		/// <summary>To be added.</summary>
		CustomSelector,
		/// <summary>To be added.</summary>
		Contains = 99,
		/// <summary>To be added.</summary>
		Between,
	}

	[Flags]
	[Native]
	public enum NSComparisonPredicateOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0x00,
		/// <summary>To be added.</summary>
		CaseInsensitive = 1 << 0,
		/// <summary>To be added.</summary>
		DiacriticInsensitive = 1 << 1,
		/// <summary>To be added.</summary>
		Normalized = 1 << 2,
	}

	[Native]
	public enum NSCompoundPredicateType : ulong {
		/// <summary>To be added.</summary>
		Not,
		/// <summary>To be added.</summary>
		And,
		/// <summary>To be added.</summary>
		Or,
	}

	[Flags]
	[Native]
	public enum NSVolumeEnumerationOptions : ulong {
		None = 0,
		// skip                  = 1 << 0,
		SkipHiddenVolumes = 1 << 1,
		ProduceFileReferenceUrls = 1 << 2,
	}

	[Flags]
	[Native]
	public enum NSDirectoryEnumerationOptions : ulong {
		None = 0,
		/// <summary>To be added.</summary>
		SkipsSubdirectoryDescendants = 1 << 0,
		/// <summary>To be added.</summary>
		SkipsPackageDescendants = 1 << 1,
		/// <summary>To be added.</summary>
		SkipsHiddenFiles = 1 << 2,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		IncludesDirectoriesPostOrder = 1 << 3,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		ProducesRelativePathUrls = 1 << 4,
	}

	[Flags]
	[Native]
	public enum NSFileManagerItemReplacementOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		UsingNewMetadataOnly = 1 << 0,
		/// <summary>To be added.</summary>
		WithoutDeletingBackupItem = 1 << 1,
	}

	[Native]
	public enum NSSearchPathDirectory : ulong {
		ApplicationDirectory = 1,
		DemoApplicationDirectory,
		DeveloperApplicationDirectory,
		AdminApplicationDirectory,
		LibraryDirectory,
		DeveloperDirectory,
		UserDirectory,
		DocumentationDirectory,
		DocumentDirectory,
		CoreServiceDirectory,
		AutosavedInformationDirectory = 11,
		DesktopDirectory = 12,
		CachesDirectory = 13,
		ApplicationSupportDirectory = 14,
		DownloadsDirectory = 15,
		InputMethodsDirectory = 16,
		MoviesDirectory = 17,
		MusicDirectory = 18,
		PicturesDirectory = 19,
		PrinterDescriptionDirectory = 20,
		SharedPublicDirectory = 21,
		PreferencePanesDirectory = 22,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		ApplicationScriptsDirectory = 23,
		ItemReplacementDirectory = 99,
		AllApplicationsDirectory = 100,
		AllLibrariesDirectory = 101,
		[NoTV]
		[MacCatalyst (13, 1)]
		TrashDirectory = 102,
	}

	[Flags]
	[Native]
	public enum NSSearchPathDomain : ulong {
		/// <summary>Do not return anything.</summary>
		None = 0,
		/// <summary>Return user directories.</summary>
		User = 1 << 0,
		/// <summary>Return local directories.</summary>
		Local = 1 << 1,
		/// <summary>Return network directories.</summary>
		Network = 1 << 2,
		/// <summary>Return the system directories.</summary>
		System = 1 << 3,
		/// <summary>Return items for all domains.</summary>
		All = 0x0ffff,
	}

	[Native]
	public enum NSRoundingMode : ulong {
		Plain,
		Down,
		Up,
		Bankers,
	}

	[Native]
	public enum NSCalculationError : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		PrecisionLoss,
		/// <summary>To be added.</summary>
		Underflow,
		/// <summary>To be added.</summary>
		Overflow,
		/// <summary>To be added.</summary>
		DivideByZero,
	}

	[Flags]
	[Native]
	public enum NSStringDrawingOptions : ulong {
		/// <summary>To be added.</summary>
		UsesLineFragmentOrigin = (1 << 0),
		/// <summary>To be added.</summary>
		UsesFontLeading = (1 << 1),
		/// <summary>This value is OSX-specific (not available in iOS)</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacOSX, 10, 11)]
		DisableScreenFontSubstitution = (1 << 2),
		/// <summary>To be added.</summary>
		UsesDeviceMetrics = (1 << 3),
		/// <summary>This value is OSX-specific (not available in iOS)</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacOSX, 10, 11)]
		OneShot = (1 << 4),
		/// <summary>To be added.</summary>
		TruncatesLastVisibleLine = (1 << 5),
	}

	[Native]
	public enum NSNumberFormatterStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Decimal = 1,
		/// <summary>To be added.</summary>
		Currency = 2,
		/// <summary>To be added.</summary>
		Percent = 3,
		/// <summary>To be added.</summary>
		Scientific = 4,
		/// <summary>To be added.</summary>
		SpellOut = 5,
		[MacCatalyst (13, 1)]
		OrdinalStyle = 6,
		[MacCatalyst (13, 1)]
		CurrencyIsoCodeStyle = 8,
		[MacCatalyst (13, 1)]
		CurrencyPluralStyle = 9,
		[MacCatalyst (13, 1)]
		CurrencyAccountingStyle = 10,
	}

	[Native]
	public enum NSNumberFormatterBehavior : ulong {
		Default = 0,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Version_10_0 = 1000,
		Version_10_4 = 1040,
	}

	[Native]
	public enum NSNumberFormatterPadPosition : ulong {
		BeforePrefix,
		AfterPrefix,
		BeforeSuffix,
		AfterSuffix,
	}

	[Native]
	public enum NSNumberFormatterRoundingMode : ulong {
		/// <summary>To be added.</summary>
		Ceiling,
		/// <summary>To be added.</summary>
		Floor,
		/// <summary>To be added.</summary>
		Down,
		/// <summary>To be added.</summary>
		Up,
		/// <summary>To be added.</summary>
		HalfEven,
		/// <summary>To be added.</summary>
		HalfDown,
		/// <summary>To be added.</summary>
		HalfUp,
	}

	[Flags]
	[Native]
	public enum NSFileVersionReplacingOptions : ulong {
		/// <summary>To be added.</summary>
		ByMoving = 1 << 0,
	}

	[Flags]
	[Native]
	public enum NSFileVersionAddingOptions : ulong {
		/// <summary>To be added.</summary>
		ByMoving = 1 << 0,
	}

	[Flags]
	[Native]
	public enum NSFileCoordinatorReadingOptions : ulong {
		/// <summary>To be added.</summary>
		WithoutChanges = 1,
		/// <summary>To be added.</summary>
		ResolvesSymbolicLink = 1 << 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ImmediatelyAvailableMetadataOnly = 1 << 2,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ForUploading = 1 << 3,
	}

	[Flags]
	[Native]
	public enum NSFileCoordinatorWritingOptions : ulong {
		/// <summary>To be added.</summary>
		ForDeleting = 1,
		/// <summary>To be added.</summary>
		ForMoving = 2,
		/// <summary>To be added.</summary>
		ForMerging = 4,
		/// <summary>To be added.</summary>
		ForReplacing = 8,
		[MacCatalyst (13, 1)]
		ContentIndependentMetadataOnly = 16,
	}

	[Flags]
	[Native]
	public enum NSLinguisticTaggerOptions : ulong {
		/// <summary>To be added.</summary>
		OmitWords = 1,
		/// <summary>To be added.</summary>
		OmitPunctuation = 2,
		/// <summary>To be added.</summary>
		OmitWhitespace = 4,
		/// <summary>To be added.</summary>
		OmitOther = 8,
		/// <summary>To be added.</summary>
		JoinNames = 16,
	}

	[Native]
	public enum NSUbiquitousKeyValueStoreChangeReason : long {
		ServerChange,
		InitialSyncChange,
		QuotaViolationChange,
		AccountChange,
	}

	[Flags]
	[Native]
	public enum NSJsonReadingOptions : ulong {
		/// <summary>To be added.</summary>
		MutableContainers = 1,
		/// <summary>To be added.</summary>
		MutableLeaves = 2,
		FragmentsAllowed = 4,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		Json5Allowed = 8,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		TopLevelDictionaryAssumed = 16,
	}

	[Flags]
	[Native]
	public enum NSJsonWritingOptions : ulong {
		/// <summary>To be added.</summary>
		PrettyPrinted = 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SortedKeys = (1 << 1),
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		FragmentsAllowed = (1 << 2),
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		WithoutEscapingSlashes = (1 << 3),
	}

	[Native]
	public enum NSLocaleLanguageDirection : ulong {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		LeftToRight,
		/// <summary>To be added.</summary>
		RightToLeft,
		/// <summary>To be added.</summary>
		TopToBottom,
		/// <summary>To be added.</summary>
		BottomToTop,
	}

	[Flags]
	public enum NSAlignmentOptions : long {
		/// <summary>To be added.</summary>
		MinXInward = 1 << 0,
		/// <summary>To be added.</summary>
		MinYInward = 1 << 1,
		/// <summary>To be added.</summary>
		MaxXInward = 1 << 2,
		/// <summary>To be added.</summary>
		MaxYInward = 1 << 3,
		/// <summary>To be added.</summary>
		WidthInward = 1 << 4,
		/// <summary>To be added.</summary>
		HeightInward = 1 << 5,

		/// <summary>To be added.</summary>
		MinXOutward = 1 << 8,
		/// <summary>To be added.</summary>
		MinYOutward = 1 << 9,
		/// <summary>To be added.</summary>
		MaxXOutward = 1 << 10,
		/// <summary>To be added.</summary>
		MaxYOutward = 1 << 11,
		/// <summary>To be added.</summary>
		WidthOutward = 1 << 12,
		/// <summary>To be added.</summary>
		HeightOutward = 1 << 13,

		/// <summary>To be added.</summary>
		MinXNearest = 1 << 16,
		/// <summary>To be added.</summary>
		MinYNearest = 1 << 17,
		/// <summary>To be added.</summary>
		MaxXNearest = 1 << 18,
		/// <summary>To be added.</summary>
		MaxYNearest = 1 << 19,
		/// <summary>To be added.</summary>
		WidthNearest = 1 << 20,
		/// <summary>To be added.</summary>
		HeightNearest = 1 << 21,

		/// <summary>To be added.</summary>
		RectFlipped = unchecked((long) (1UL << 63)),

		/// <summary>To be added.</summary>
		AllEdgesInward = MinXInward | MaxXInward | MinYInward | MaxYInward,
		/// <summary>To be added.</summary>
		AllEdgesOutward = MinXOutward | MaxXOutward | MinYOutward | MaxYOutward,
		/// <summary>To be added.</summary>
		AllEdgesNearest = MinXNearest | MaxXNearest | MinYNearest | MaxYNearest,
	}

	[Flags]
	[Native]
	public enum NSFileWrapperReadingOptions : ulong {
		/// <summary>To be added.</summary>
		Immediate = 1 << 0,
		/// <summary>To be added.</summary>
		WithoutMapping = 1 << 1,
	}

	[Flags]
	[Native]
	public enum NSFileWrapperWritingOptions : ulong {
		/// <summary>To be added.</summary>
		Atomic = 1 << 0,
		/// <summary>To be added.</summary>
		WithNameUpdating = 1 << 1,
	}

	[Flags]
	[Native ("NSAttributedStringEnumerationOptions")]
	public enum NSAttributedStringEnumeration : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Reverse = 1 << 1,
		/// <summary>To be added.</summary>
		LongestEffectiveRangeNotRequired = 1 << 20,
	}

	// macOS has defined this in AppKit as well, but starting with .NET we're going
	// to use this one only.
	[Native]
	public enum NSUnderlineStyle : long {
		/// <summary>To be added.</summary>
		None = 0x00,
		Single = 0x01,
		Thick = 0x02,
		Double = 0x09,
		PatternSolid = 0x0000,
		PatternDot = 0x0100,
		PatternDash = 0x0200,
		PatternDashDot = 0x0300,
		PatternDashDotDot = 0x0400,
		ByWord = 0x8000,
	}

	// There's an AppKit.NSWritingDirection, which is deprecated.
	// There's also an UIKit.UITextWritingDirection, which is deprecated too.
	// This is the enum we should be using.
	// See https://github.com/xamarin/xamarin-macios/issues/6573
	[Native]
	public enum NSWritingDirection : long {
		Natural = -1,
		LeftToRight = 0,
		RightToLeft = 1,
	}

	[Flags]
	[Native]
	public enum NSByteCountFormatterUnits : ulong {
		/// <summary>To be added.</summary>
		UseDefault = 0,
		/// <summary>To be added.</summary>
		UseBytes = 1 << 0,
		/// <summary>To be added.</summary>
		UseKB = 1 << 1,
		/// <summary>To be added.</summary>
		UseMB = 1 << 2,
		/// <summary>To be added.</summary>
		UseGB = 1 << 3,
		/// <summary>To be added.</summary>
		UseTB = 1 << 4,
		/// <summary>To be added.</summary>
		UsePB = 1 << 5,
		/// <summary>To be added.</summary>
		UseEB = 1 << 6,
		/// <summary>To be added.</summary>
		UseZB = 1 << 7,
		/// <summary>To be added.</summary>
		UseYBOrHigher = 0x0FF << 8,
		/// <summary>To be added.</summary>
		UseAll = 0x0FFFF,
	}

	[Native]
	public enum NSByteCountFormatterCountStyle : long {
		/// <summary>To be added.</summary>
		File,
		/// <summary>To be added.</summary>
		Memory,
		/// <summary>To be added.</summary>
		Decimal,
		/// <summary>To be added.</summary>
		Binary,
	}

	[Flags]
	[Native]
	public enum NSUrlBookmarkCreationOptions : ulong {
		PreferFileIDResolution = 1 << 8,
		MinimalBookmark = 1 << 9,
		SuitableForBookmarkFile = 1 << 10,
		[NoiOS, NoTV]
		[NoMacCatalyst]
		WithSecurityScope = 1 << 11,
		[NoiOS, NoTV]
		[NoMacCatalyst]
		SecurityScopeAllowOnlyReadAccess = 1 << 12,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		CreationWithoutImplicitSecurityScope = 1 << 29,
	}

	[Flags]
	[Native]
	public enum NSUrlBookmarkResolutionOptions : ulong {
		WithoutUI = 1 << 8,
		WithoutMounting = 1 << 9,
		[NoiOS, NoTV]
		[NoMacCatalyst]
		WithSecurityScope = 1 << 10,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		WithoutImplicitStartAccessing = 1 << 15,
	}

	[Native]
	public enum NSLigatureType : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		All,
	}

	[Flags]
	[Native]
	public enum NSCalendarOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		WrapCalendarComponents = 1 << 0,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchStrictly = 1 << 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SearchBackwards = 1 << 2,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchPreviousTimePreservingSmallerUnits = 1 << 8,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchNextTimePreservingSmallerUnits = 1 << 9,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchNextTime = 1 << 10,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchFirst = 1 << 12,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MatchLast = 1 << 13,
	}

	[Native]
	public enum NSUrlRequestNetworkServiceType : ulong {
		Default,
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'PushKit' framework instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'PushKit' framework instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'PushKit' framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PushKit' framework instead.")]
		VoIP,
		Video,
		Background,
		Voice,
		[MacCatalyst (13, 1)]
		ResponsiveData = 6,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		AVStreaming = 8,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		ResponsiveAV = 9,
		[MacCatalyst (13, 1)]
		CallSignaling = 11,
	}

	[Flags]
	[Native]
	public enum NSSortOptions : ulong {
		/// <summary>To be added.</summary>
		Concurrent = 1 << 0,
		/// <summary>To be added.</summary>
		Stable = 1 << 4,
	}

	[Flags]
	[Native]
	public enum NSDataBase64DecodingOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		IgnoreUnknownCharacters = 1,
	}

	[Flags]
	[Native]
	public enum NSDataBase64EncodingOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		SixtyFourCharacterLineLength = 1,
		/// <summary>To be added.</summary>
		SeventySixCharacterLineLength = 1 << 1,
		/// <summary>To be added.</summary>
		EndLineWithCarriageReturn = 1 << 4,
		/// <summary>To be added.</summary>
		EndLineWithLineFeed = 1 << 5,
	}

	[Native]
	public enum NSUrlSessionAuthChallengeDisposition : long {
		UseCredential = 0,
		PerformDefaultHandling = 1,
		CancelAuthenticationChallenge = 2,
		RejectProtectionSpace = 3,
	}

	[Native]
	public enum NSUrlSessionTaskState : long {
		Running = 0,
		Suspended = 1,
		Canceling = 2,
		Completed = 3,
	}

	[Native]
	public enum NSUrlSessionResponseDisposition : long {
		Cancel = 0,
		Allow = 1,
		BecomeDownload = 2,
		BecomeStream = 3,
	}

	[Native]
	public enum NSUrlErrorCancelledReason : long {
		UserForceQuitApplication,
		BackgroundUpdatesDisabled,
		InsufficientSystemResources,
	}

	[Flags]
	public enum NSActivityOptions : ulong {
		/// <summary>To be added.</summary>
		IdleDisplaySleepDisabled = 1UL << 40,
		/// <summary>To be added.</summary>
		IdleSystemSleepDisabled = 1UL << 20,
		/// <summary>To be added.</summary>
		SuddenTerminationDisabled = 1UL << 14,
		/// <summary>To be added.</summary>
		AutomaticTerminationDisabled = 1UL << 15,
		AnimationTrackingEnabled = 1uL << 45,
		TrackingEnabled = 1uL << 46,
		UserInteractive = (UserInitiated | LatencyCritical),
		/// <summary>To be added.</summary>
		UserInitiated = 0x00FFFFFFUL | IdleSystemSleepDisabled,
		/// <summary>To be added.</summary>
		Background = 0x000000ffUL,
		/// <summary>To be added.</summary>
		LatencyCritical = 0xFF00000000UL,
		InitiatedAllowingIdleSystemSleep = UserInitiated & ~IdleSystemSleepDisabled,
	}

	[Native]
	public enum NSTimeZoneNameStyle : long {
		Standard,
		ShortStandard,
		DaylightSaving,
		ShortDaylightSaving,
		Generic,
		ShortGeneric,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSItemProviderErrorCode : long {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		ItemUnavailable = -1000,
		/// <summary>To be added.</summary>
		UnexpectedValueClass = -1100,
		/// <summary>To be added.</summary>
		UnavailableCoercion = -1200,
	}

	[Native]
	[MacCatalyst (13, 1)]
	public enum NSDateComponentsFormatterUnitsStyle : long {
		/// <summary>To be added.</summary>
		Positional = 0,
		/// <summary>To be added.</summary>
		Abbreviated,
		/// <summary>To be added.</summary>
		Short,
		/// <summary>To be added.</summary>
		Full,
		/// <summary>To be added.</summary>
		SpellOut,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Brief,
	}

	[Flags]
	[Native]
	[MacCatalyst (13, 1)]
	public enum NSDateComponentsFormatterZeroFormattingBehavior : ulong {
		/// <summary>To be added.</summary>
		None = (0),
		/// <summary>To be added.</summary>
		Default = (1 << 0),
		/// <summary>To be added.</summary>
		DropLeading = (1 << 1),
		/// <summary>To be added.</summary>
		DropMiddle = (1 << 2),
		/// <summary>To be added.</summary>
		DropTrailing = (1 << 3),
		/// <summary>To be added.</summary>
		DropAll = (DropLeading | DropMiddle | DropTrailing),
		/// <summary>To be added.</summary>
		Pad = (1 << 16),
	}

	[Native]
	[MacCatalyst (13, 1)]
	public enum NSFormattingContext : long {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Dynamic = 1,
		/// <summary>To be added.</summary>
		Standalone = 2,
		/// <summary>To be added.</summary>
		ListItem = 3,
		/// <summary>To be added.</summary>
		BeginningOfSentence = 4,
		/// <summary>To be added.</summary>
		MiddleOfSentence = 5,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSDateIntervalFormatterStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Short = 1,
		/// <summary>To be added.</summary>
		Medium = 2,
		/// <summary>To be added.</summary>
		Long = 3,
		/// <summary>To be added.</summary>
		Full = 4,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSEnergyFormatterUnit : long {
		/// <summary>To be added.</summary>
		Joule = 11,
		/// <summary>To be added.</summary>
		Kilojoule = 14,
		/// <summary>To be added.</summary>
		Calorie = (7 << 8) + 1,
		/// <summary>To be added.</summary>
		Kilocalorie = (7 << 8) + 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSFormattingUnitStyle : long {
		/// <summary>To be added.</summary>
		Short = 1,
		/// <summary>To be added.</summary>
		Medium,
		/// <summary>To be added.</summary>
		Long,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSMassFormatterUnit : long {
		/// <summary>To be added.</summary>
		Gram = 11,
		/// <summary>To be added.</summary>
		Kilogram = 14,
		/// <summary>To be added.</summary>
		Ounce = (6 << 8) + 1,
		/// <summary>To be added.</summary>
		Pound = (6 << 8) + 2,
		/// <summary>To be added.</summary>
		Stone = (6 << 8) + 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSLengthFormatterUnit : long {
		/// <summary>To be added.</summary>
		Millimeter = 8,
		/// <summary>To be added.</summary>
		Centimeter = 9,
		/// <summary>To be added.</summary>
		Meter = 11,
		/// <summary>To be added.</summary>
		Kilometer = 14,
		/// <summary>To be added.</summary>
		Inch = (5 << 8) + 1,
		/// <summary>To be added.</summary>
		Foot = (5 << 8) + 2,
		/// <summary>To be added.</summary>
		Yard = (5 << 8) + 3,
		/// <summary>To be added.</summary>
		Mile = (5 << 8) + 4,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSQualityOfService : long {
		UserInteractive = 33,
		UserInitiated = 25,
		Utility = 17,
		Background = 9,
		Default = -1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSProcessInfoThermalState : long {
		Nominal,
		Fair,
		Serious,
		Critical,
	}

	[Native]
	public enum NSUrlRelationship : long {
		Contains,
		Same,
		Other,
	}

	// NSTextCheckingResult.h:typedef NS_OPTIONS(uint64_t, NSTextCheckingType)
	[Flags]
	public enum NSTextCheckingType : ulong {
		Orthography = 1 << 0,
		Spelling = 1 << 1,
		Grammar = 1 << 2,
		Date = 1 << 3,
		Address = 1 << 4,
		Link = 1 << 5,
		Quote = 1 << 6,
		Dash = 1 << 7,
		Replacement = 1 << 8,
		Correction = 1 << 9,
		RegularExpression = 1 << 10,
		PhoneNumber = 1 << 11,
		TransitInformation = 1 << 12,
	}

	// NSTextCheckingResult.h:typedef uint64_t NSTextCheckingTypes;
	public enum NSTextCheckingTypes : ulong {
		AllSystemTypes = 0xffffffff,
		AllCustomTypes = 0xffffffff00000000,
		AllTypes = 0xffffffffffffffff,
	}

	[Native]
	[Flags]
	public enum NSRegularExpressionOptions : ulong {
		CaseInsensitive = 1 << 0,
		AllowCommentsAndWhitespace = 1 << 1,
		IgnoreMetacharacters = 1 << 2,
		DotMatchesLineSeparators = 1 << 3,
		AnchorsMatchLines = 1 << 4,
		UseUnixLineSeparators = 1 << 5,
		UseUnicodeWordBoundaries = 1 << 6,
	}

	[Native]
	[Flags]
	public enum NSMatchingOptions : ulong {
		/// <summary>To be added.</summary>
		ReportProgress = 1 << 0,
		/// <summary>To be added.</summary>
		ReportCompletion = 1 << 1,
		/// <summary>To be added.</summary>
		Anchored = 1 << 2,
		/// <summary>To be added.</summary>
		WithTransparentBounds = 1 << 3,
		/// <summary>To be added.</summary>
		WithoutAnchoringBounds = 1 << 4,
	}

	[Native]
	[Flags]
	public enum NSMatchingFlags : ulong {
		/// <summary>To be added.</summary>
		Progress = 1 << 0,
		/// <summary>To be added.</summary>
		Completed = 1 << 1,
		/// <summary>To be added.</summary>
		HitEnd = 1 << 2,
		/// <summary>To be added.</summary>
		RequiredEnd = 1 << 3,
		/// <summary>To be added.</summary>
		InternalError = 1 << 4,
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum NSPersonNameComponentsFormatterOptions : ulong {
		/// <summary>Value that indicates that the phonetic representation of a name should be formatted, rather than the name object's own components.</summary>
		Phonetic = (1 << 1),
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSPersonNameComponentsFormatterStyle : long {
		/// <summary>Display a medium-length representation of the name. Equivalent to <c>Medium</c>.</summary>
		Default = 0,
		/// <summary>Display a shortened form of the name by abbreviating or omitting various, user-specified, components.</summary>
		Short,
		/// <summary>Display a medium-length representation of the name. Equivalent to <c>Default</c>.</summary>
		Medium,
		/// <summary>Use all available name components, except for the nickname, to display the name.</summary>
		Long,
		/// <summary>Display the most abbreviated form of the name.</summary>
		Abbreviated,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSDecodingFailurePolicy : long {
		/// <summary>To be added.</summary>
		RaiseException,
		/// <summary>To be added.</summary>
		SetErrorAndReturn,
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum NSIso8601DateFormatOptions : ulong {
		/// <summary>Includes the year in the string.</summary>
		Year = 1 << 0,
		/// <summary>Includes the month in the string.</summary>
		Month = 1 << 1,
		/// <summary>Includes the week of the year in the string (e.g., "W33").</summary>
		WeekOfYear = 1 << 2,
		/// <summary>Includes the day in the string.</summary>
		Day = 1 << 4,
		/// <summary>Includes the time in the string.</summary>
		Time = 1 << 5,
		/// <summary>Includes the timezone in the string.</summary>
		TimeZone = 1 << 6,
		/// <summary>Puts a space between the date and the time.</summary>
		SpaceBetweenDateAndTime = 1 << 7,
		/// <summary>Separates date components with a dash.</summary>
		DashSeparatorInDate = 1 << 8,
		/// <summary>Puts a colon between time components</summary>
		ColonSeparatorInTime = 1 << 9,
		/// <summary>Puts a colon in the time.</summary>
		ColonSeparatorInTimeZone = 1 << 10,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		FractionalSeconds = 1 << 11,
		/// <summary>Includes the date in the string.</summary>
		FullDate = Year | Month | Day | DashSeparatorInDate,
		/// <summary>Includes the time in the string.</summary>
		FullTime = Time | ColonSeparatorInTime | TimeZone | ColonSeparatorInTimeZone,
		/// <summary>The standard ISO-8601 format (<c>yyyy-MM-ddTHH:mm:ssZ</c>).</summary>
		InternetDateTime = FullDate | FullTime,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSUrlSessionTaskMetricsResourceFetchType : long {
		Unknown,
		NetworkLoad,
		ServerPush,
		LocalCache,
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum NSMeasurementFormatterUnitOptions : ulong {
		/// <summary>To be added.</summary>
		ProvidedUnit = (1 << 0),
		/// <summary>To be added.</summary>
		NaturalScale = (1 << 1),
		/// <summary>To be added.</summary>
		TemperatureWithoutUnit = (1 << 2),
	}


	[MacCatalyst (13, 1)]
	[Native]
	public enum NSItemProviderRepresentationVisibility : long {
		/// <summary>To be added.</summary>
		All = 0,
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		Team = 1,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		Group = 2,
		/// <summary>To be added.</summary>
		OwnProcess = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSItemProviderFileOptions : long {
		/// <summary>To be added.</summary>
		OpenInPlace = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSLinguisticTaggerUnit : long {
		/// <summary>To be added.</summary>
		Word,
		/// <summary>To be added.</summary>
		Sentence,
		/// <summary>To be added.</summary>
		Paragraph,
		/// <summary>To be added.</summary>
		Document,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSUrlSessionDelayedRequestDisposition : long {
		ContinueLoading = 0,
		UseNewRequest = 1,
		Cancel = 2,
	}

	[Native]
	public enum NSXpcConnectionOptions : ulong {
		Privileged = (1 << 12),
	}

	[MacCatalyst (13, 1)]
	public enum NSFileProtectionType {
		[Field ("NSFileProtectionComplete")]
		Complete,
		[Field ("NSFileProtectionCompleteUnlessOpen")]
		CompleteUnlessOpen,
		[Field ("NSFileProtectionCompleteUntilFirstUserAuthentication")]
		CompleteUntilFirstUserAuthentication,
		[Field ("NSFileProtectionNone")]
		None,
		[TV (17, 0), NoMac, iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("NSFileProtectionCompleteWhenUserInactive")]
		CompleteWhenUserInactive,
	}
}
