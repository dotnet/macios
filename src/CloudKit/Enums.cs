using CoreFoundation;
using ObjCRuntime;
using Foundation;
using System;

#nullable enable

namespace CloudKit {
	// NSInteger -> CKContainer.h
	/// <summary>Enumerates values that indicate whether a user's iCloud account is available.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKAccountStatus : long {
		/// <summary>An error occured when the application tried to determine if the user's account is available.</summary>
		CouldNotDetermine = 0,
		/// <summary>The user's account is available.</summary>
		Available = 1,
		/// <summary>The user has an account, but a parental control or mobile restriction prevents its use.</summary>
		Restricted = 2,
		/// <summary>The user has no iCloud account.</summary>
		NoAccount = 3,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		TemporarilyUnavailable = 4,
	}

	// NSUInteger -> CKContainer.h
	/// <summary>Enumerates a value that indicates that other app users can discover the current user by email address.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum CKApplicationPermissions : ulong {
		/// <summary>Other app users can discover the current user by email address.</summary>
		UserDiscoverability = 1 << 0,
	}

	// NSInteger -> CKContainer.h
	/// <summary>Enumerates the states that an application can have when attempting to obtain a permission.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKApplicationPermissionStatus : long {
		/// <summary>The application has not yet requested the permission.</summary>
		InitialState = 0,
		/// <summary>An error occured while attempting to obtain the permission.</summary>
		CouldNotComplete = 1,
		/// <summary>The user denied the permission request.</summary>
		Denied = 2,
		/// <summary>The user granted the permission request.</summary>
		Granted = 3,
	}

	// NSInteger -> CKError.h
	/// <summary>Enumerates CloudKit error conditions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("CKErrorDomain")]
	public enum CKErrorCode : long {
		/// <summary>The operation succeeded.</summary>
		None,
		/// <summary>A CloudKit internal error. Non-recoverable.</summary>
		InternalError = 1,
		/// <summary>Indicates that some items failed but the overall operation succeeded.</summary>
		PartialFailure = 2,
		/// <summary>Indicates that the user is not online.</summary>
		NetworkUnavailable = 3,
		/// <summary>The network returned an error during processing.</summary>
		NetworkFailure = 4,
		/// <summary>The specified container is unknown or unauthorized.</summary>
		BadContainer = 5,
		/// <summary>CloudKit is not available.</summary>
		ServiceUnavailable = 6,
		/// <summary>Indicates that the client is currently rate-limited.</summary>
		RequestRateLimited = 7,
		/// <summary>The app is missing the required entitlement.</summary>
		MissingEntitlement = 8,
		/// <summary>The user is not currently authenticated. Common cause: User is not logged in to iCloud.</summary>
		NotAuthenticated = 9,
		/// <summary>Indicates the user has not allowed access for the data in the fetch or save.</summary>
		PermissionFailure = 10,
		/// <summary>Indicates the requested record does not exist.</summary>
		UnknownItem = 11,
		/// <summary>Indicates an error in the form or content of the request.</summary>
		InvalidArguments = 12,
		/// <summary>This error message is deprecated. Indicates that results were truncated by the server.</summary>
		ResultsTruncated = 13,
		/// <summary>The record was rejected because the server's values were different.</summary>
		ServerRecordChanged = 14,
		/// <summary>Server rejected request. Non-recoverable.</summary>
		ServerRejectedRequest = 15,
		/// <summary>The specified asset file could not be found.</summary>
		AssetFileNotFound = 16,
		/// <summary>The specified asset file was modified during the save operation.</summary>
		AssetFileModified = 17,
		/// <summary>The app version is less than the CloudKit-specified minimum acceptable version.</summary>
		IncompatibleVersion = 18,
		/// <summary>The request was rejected because of a conflict in a unique field.</summary>
		ConstraintViolation = 19,
		/// <summary>Indicates the operation was cancelled programmatically by the app.</summary>
		OperationCancelled = 20,
		/// <summary>Indicates the <see cref="P:CloudKit.CKFetchRecordChangesOperation.PreviousServerChangeToken" /> expired and the client must resynchronize.</summary>
		ChangeTokenExpired = 21,
		/// <summary>An item failed in a zone with atomic updates, so the batch was rejected.</summary>
		BatchRequestFailed = 22,
		/// <summary>Indicates that the zone is currently too busy. Suggested action is a delayed retry.</summary>
		ZoneBusy = 23,
		/// <summary>The operation could not be completed. Common cause: attempt to modify zones on public database.</summary>
		BadDatabase = 24,
		/// <summary>Indicates that the requested operation would have exceeded the user's storage quota.</summary>
		QuotaExceeded = 25,
		/// <summary>The specified zone is not recognized by the server.</summary>
		ZoneNotFound = 26,
		/// <summary>Indicates the request was too large. Suggested change: Refactor request into smaller batches.</summary>
		LimitExceeded = 27,
		/// <summary>Indicates the user deleted the zone. Suggested action: Remove local copy of zone data or ask user if app may upload data again.</summary>
		UserDeletedZone = 28,
		/// <summary>Indicates that there are too many participants attached to the share.</summary>
		TooManyParticipants = 29,
		/// <summary>Indicates that the <see cref="T:CloudKit.CKRecord" /> or <see cref="T:CloudKit.CKShare" /> hierarchy already exists in another share.</summary>
		AlreadyShared = 30,
		/// <summary>Indicates the <see cref="T:CloudKit.CKRecord" /> object's parent reference or it's share reference could not be found.</summary>
		ReferenceViolation = 31,
		/// <summary>Indicates an error due to a managed account restriction.</summary>
		ManagedAccountRestricted = 32,
		/// <summary>The user is not yet partificipating in the share..</summary>
		ParticipantMayNeedVerification = 33,
		/// <summary>Indicates that the response was lost.</summary>
		ResponseLost = 34,
		/// <summary>To be added.</summary>
		AssetNotAvailable = 35,
		TemporarilyUnavailable = 36,
	}

	// NSInteger -> CKModifyRecordsOperation.h
	/// <summary>Enumerates policies that control when or if a record should be saved.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKRecordSavePolicy : long {
		SaveIfServerRecordUnchanged = 0,
		SaveChangedKeys = 1,
		SaveAllKeys = 2,
	}

	// NSInteger -> CKNotification.h
	/// <summary>Enumerates the events that can generate a push notification.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKNotificationType : long {
		Query = 1,
		RecordZone = 2,
		ReadNotification = 3,
		[MacCatalyst (13, 1)]
		Database = 4,
	}

	// NSInteger -> CKNotification.h
	/// <summary>Enumerates the persistent storage events that can trigger data lifecycle notifications.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKQueryNotificationReason : long {
		RecordCreated = 1,
		RecordUpdated,
		RecordDeleted,
	}

	// NSUInteger -> CKRecordZone.h
	/// <summary>Enumerates the special operations that a zone is capable of.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum CKRecordZoneCapabilities : ulong {
		FetchChanges = 1 << 0,
		Atomic = 1 << 1,
		[MacCatalyst (13, 1)]
		Sharing = 1 << 2,
		[iOS (15, 0), TV (15, 0)]
		[MacCatalyst (15, 0)]
		ZoneWideSharing = 1 << 3,

	}

	// NSUInteger -> CKReference.h
	/// <summary>Enumerates values that control whether a reference should delete itself when its target record is deleted.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKReferenceAction : ulong {
		None = 0,
		DeleteSelf = 1,
	}

	// NSInteger -> CKSubscription.h
	/// <summary>Enumerates subscription types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKSubscriptionType : long {
		Query = 1,
		RecordZone = 2,
		[MacCatalyst (13, 1)]
		Database = 3,
	}

	// NSInteger -> CKSubscription.h

#if !NET
	[Obsoleted (PlatformName.iOS, 14, 0, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Obsoleted (PlatformName.MacOSX, 10, 16, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Flags]
	[Native]
	public enum CKSubscriptionOptions : ulong {
		FiresOnRecordCreation = 1 << 0,
		FiresOnRecordUpdate = 1 << 1,
		FiresOnRecordDeletion = 1 << 2,
		FiresOnce = 1 << 3,
	}
#endif

	/// <summary>Enumerates values that tell whether a database is private, shared, or public.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKDatabaseScope : long {
		/// <summary>Indicates a public database.</summary>
		Public = 1,
		/// <summary>Indicates a private database.</summary>
		Private,
		/// <summary>Indicates a shared database.</summary>
		Shared,
	}

	/// <summary>Enumerates responses to share participation requests.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantAcceptanceStatus : long {
		Unknown,
		Pending,
		Accepted,
		Removed,
	}

	/// <summary>Enumerates user share permissions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantPermission : long {
		Unknown,
		None,
		ReadOnly,
		ReadWrite,
	}

	/// <summary>Enumerates share participant types.</summary>
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'CKShareParticipantRole' instead.")]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'CKShareParticipantRole' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'CKShareParticipantRole' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKShareParticipantRole' instead.")]
	[Native]
	public enum CKShareParticipantType : long {
		Unknown = 0,
		Owner = 1,
		PrivateUser = 3,
		PublicUser = 4,
	}

	/// <summary>Enumerates the time or times when a <see cref="T:CloudKit.CKSubscription" /> fires a notification.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKQuerySubscriptionOptions : ulong {
		RecordCreation = 1 << 0,
		RecordUpdate = 1 << 1,
		RecordDeletion = 1 << 2,
		FiresOnce = 1 << 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CKOperationGroupTransferSize : long {
		Unknown,
		Kilobytes,
		Megabytes,
		TensOfMegabytes,
		HundredsOfMegabytes,
		Gigabytes,
		TensOfGigabytes,
		HundredsOfGigabytes,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantRole : long {
		Unknown = 0,
		Owner = 1,
		PrivateUser = 3,
		PublicUser = 4,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native, Flags]
	public enum CKSharingParticipantAccessOption : ulong {
		AnyoneWithLink = 1uL << 0,
		SpecifiedRecipientsOnly = 1uL << 1,
		Any = AnyoneWithLink | SpecifiedRecipientsOnly,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native, Flags]
	public enum CKSharingParticipantPermissionOption : ulong {
		ReadOnly = 1uL << 0,
		ReadWrite = 1uL << 1,
		Any = ReadOnly | ReadWrite,
	}


	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineAccountChangeType : long {
		SignIn,
		SignOut,
		SwitchAccounts,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineSyncReason : long {
		Scheduled,
		Manual,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineEventType : long {
		StateUpdate,
		AccountChange,
		FetchedDatabaseChanges,
		FetchedRecordZoneChanges,
		SentDatabaseChanges,
		SentRecordZoneChanges,
		WillFetchChanges,
		WillFetchRecordZoneChanges,
		DidFetchRecordZoneChanges,
		DidFetchChanges,
		WillSendChanges,
		DidSendChanges,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEnginePendingRecordZoneChangeType : long {
		SaveRecord,
		DeleteRecord,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineZoneDeletionReason : long {
		Deleted,
		Purged,
		EncryptedDataReset,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEnginePendingDatabaseChangeType : long {
		SaveZone,
		DeleteZone,
	}

}
