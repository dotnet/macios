using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;
using CoreLocation;
#if !TVOS
using Contacts;
#endif

namespace CloudKit {

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: You must call -[CKAsset initWithFileURL:] or -[CKAsset initWithData:]
	[BaseType (typeof (NSObject))]
	interface CKAsset : NSCoding, NSSecureCoding, CKRecordValue {

		[Export ("initWithFileURL:")]
		NativeHandle Constructor (NSUrl fileUrl);

		[Export ("fileURL", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSUrl FileUrl { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKUserIdentityLookupInfo : NSSecureCoding, NSCopying {
		[Internal, Export ("initWithEmailAddress:")]
		IntPtr _FromEmail (string emailAddress);

		[Internal, Export ("initWithPhoneNumber:")]
		IntPtr _FromPhoneNumber (string phoneNumber);

		[Export ("initWithUserRecordID:")]
		CKUserIdentityLookupInfo Constructor (CKRecordID userRecordID);

		[Static]
		[Export ("lookupInfosWithEmails:")]
		CKUserIdentityLookupInfo [] GetLookupInfosWithEmails (string [] emails);

		[Static]
		[Export ("lookupInfosWithPhoneNumbers:")]
		CKUserIdentityLookupInfo [] GetLookupInfosWithPhoneNumbers (string [] phoneNumbers);

		[Static]
		[Export ("lookupInfosWithRecordIDs:")]
		CKUserIdentityLookupInfo [] GetLookupInfos (CKRecordID [] recordIDs);

		[NullAllowed, Export ("emailAddress")]
		string EmailAddress { get; }

		[NullAllowed, Export ("phoneNumber")]
		string PhoneNumber { get; }

		[NullAllowed, Export ("userRecordID", ArgumentSemantic.Copy)]
		CKRecordID UserRecordID { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKUserIdentity : NSSecureCoding, NSCopying {
		[NullAllowed, Export ("lookupInfo", ArgumentSemantic.Copy)]
		CKUserIdentityLookupInfo LookupInfo { get; }

		[NullAllowed, Export ("nameComponents", ArgumentSemantic.Copy)]
		NSPersonNameComponents NameComponents { get; }

		[NullAllowed, Export ("userRecordID", ArgumentSemantic.Copy)]
		CKRecordID UserRecordID { get; }

		[Export ("hasiCloudAccount")]
		bool HasICloudAccount { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Please read Apple's document about 'Sharing CloudKit Data with Other iCloud Users'.")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Please read Apple's document about 'Sharing CloudKit Data with Other iCloud Users'.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Please read Apple's document about 'Sharing CloudKit Data with Other iCloud Users'.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Please read Apple's document about 'Sharing CloudKit Data with Other iCloud Users'.")]
		[Export ("contactIdentifiers", ArgumentSemantic.Copy)]
		string [] ContactIdentifiers { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CKShareMetadata : NSCopying, NSSecureCoding {

#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Get them from 'CKFetchShareMetadataOperation' or platform-specific scene / app delegate callbacks.")]
		[Export ("init")]
		NativeHandle Constructor ();
#endif

		[Export ("containerIdentifier")]
		string ContainerIdentifier { get; }

		[Export ("share", ArgumentSemantic.Strong)]
		CKShare Share { get; }

		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'HierarchicalRootRecordId' instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'HierarchicalRootRecordId' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'HierarchicalRootRecordId' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'HierarchicalRootRecordId' instead.")]
		[Export ("rootRecordID", ArgumentSemantic.Copy)]
		CKRecordID RootRecordID { get; }

		[MacCatalyst (13, 1)]
		[Export ("participantRole", ArgumentSemantic.Assign)]
		CKShareParticipantRole ParticipantRole { get; }

		[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'ParticipantRole' instead.")]
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ParticipantRole' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'ParticipantRole' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ParticipantRole' instead.")]
		[Export ("participantType", ArgumentSemantic.Assign)]
		CKShareParticipantType Type { get; }

		[Export ("participantStatus", ArgumentSemantic.Assign)]
		CKShareParticipantAcceptanceStatus Status { get; }

		[Export ("participantPermission", ArgumentSemantic.Assign)]
		CKShareParticipantPermission Permission { get; }

		[Export ("ownerIdentity", ArgumentSemantic.Strong)]
		CKUserIdentity OwnerIdentity { get; }

		[NullAllowed, Export ("rootRecord", ArgumentSemantic.Strong)]
		CKRecord RootRecord { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("hierarchicalRootRecordID", ArgumentSemantic.Copy)]
		CKRecordID HierarchicalRootRecordId { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKRecord))]
	[DisableDefaultCtor]
	interface CKShare {
		[Export ("initWithRootRecord:")]
		NativeHandle Constructor (CKRecord rootRecord);

		[Export ("initWithRootRecord:shareID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CKRecord rootRecord, CKRecordID shareID);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithRecordZoneID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CKRecordZoneID recordZoneId);

		[Export ("publicPermission", ArgumentSemantic.Assign)]
		CKShareParticipantPermission PublicPermission { get; set; }

		[NullAllowed]
		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		[Export ("participants", ArgumentSemantic.Strong)]
		CKShareParticipant [] Participants { get; }

		[Export ("owner", ArgumentSemantic.Strong)]
		CKShareParticipant Owner { get; }

		[NullAllowed, Export ("currentUserParticipant", ArgumentSemantic.Strong)]
		CKShareParticipant CurrentUser { get; }

		[Export ("addParticipant:")]
		void Add (CKShareParticipant participant);

		[Export ("removeParticipant:")]
		void Remove (CKShareParticipant participant);
	}

	/// <summary>Constants used by various CloudKit classes.</summary>
	/// <remarks>To be added.</remarks>
	[Static]
	[MacCatalyst (13, 1)]
	partial interface CKShareKeys {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CKShareTitleKey")]
		NSString Title { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CKShareThumbnailImageDataKey")]
		NSString ThumbnailImageData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CKShareTypeKey")]
		NSString Type { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKShareParticipant : NSSecureCoding, NSCopying {
		[Export ("userIdentity", ArgumentSemantic.Strong)]
		CKUserIdentity UserIdentity { get; }

		[MacCatalyst (13, 1)]
		[Export ("role", ArgumentSemantic.Assign)]
		CKShareParticipantRole Role { get; set; }

		[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Role' instead.")]
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Role' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Role' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Role' instead.")]
		[Export ("type", ArgumentSemantic.Assign)]
		CKShareParticipantType Type { get; set; }

		[Export ("acceptanceStatus", ArgumentSemantic.Assign)]
		CKShareParticipantAcceptanceStatus AcceptanceStatus { get; }

		[Export ("permission", ArgumentSemantic.Assign)]
		CKShareParticipantPermission Permission { get; set; }

		// This showed up in Xcode 16's b1 headers, but according to the availability attributes it's always been available.
		[Export ("participantID", ArgumentSemantic.Copy)]
		string ParticipantId { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInternalInconsistencyException Reason: Use +[CKContainer privateCloudDatabase] or +[CKContainer publicCloudDatabase] instead of creating your own
	[BaseType (typeof (NSObject))]
	interface CKContainer {

		/// <summary>Developers should not use this deprecated property. Developers should use 'CurrentUserDefaultName' instead.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CurrentUserDefaultName' instead.")]
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'CurrentUserDefaultName' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CurrentUserDefaultName' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CurrentUserDefaultName' instead.")]
		[Field ("CKOwnerDefaultName")]
		NSString OwnerDefaultName { get; }

		/// <summary>The current user of the database.</summary>
		///         <value>The default value is "defaultOwner".</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CKCurrentUserDefaultName")]
		NSString CurrentUserDefaultName { get; }

		[Static]
		[Export ("defaultContainer")]
		CKContainer DefaultContainer { get; }

		[Static]
		[Export ("containerWithIdentifier:")]
		CKContainer FromIdentifier (string containerIdentifier);

		[NullAllowed, Export ("containerIdentifier")]
		string ContainerIdentifier { get; }

		[Export ("addOperation:")]
		void AddOperation (CKOperation operation);

		[Export ("privateCloudDatabase")]
		CKDatabase PrivateCloudDatabase { get; }

		[Export ("publicCloudDatabase")]
		CKDatabase PublicCloudDatabase { get; }

		[MacCatalyst (13, 1)]
		[Export ("sharedCloudDatabase")]
		CKDatabase SharedCloudDatabase { get; }

		[MacCatalyst (13, 1)]
		[Export ("databaseWithDatabaseScope:")]
		CKDatabase GetDatabase (CKDatabaseScope databaseScope);

		[Export ("accountStatusWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Retrieves the current user's <see cref="CloudKit.CKAccountStatus" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous GetAccountStatus operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKAccountStatus,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void GetAccountStatus (Action<CKAccountStatus, NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[Export ("statusForApplicationPermission:completionHandler:")]
		[Async (XmlDocs = """
			<param name="applicationPermission">To be added.</param>
			<summary>Requests the current <see cref="CloudKit.CKApplicationPermissionStatus" /> for the specified <see cref="CloudKit.CKApplicationPermissions" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous StatusForApplicationPermission operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKApplicationPermissionStatus,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void StatusForApplicationPermission (CKApplicationPermissions applicationPermission, Action<CKApplicationPermissionStatus, NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[Export ("requestApplicationPermission:completionHandler:")]
		[Async (XmlDocs = """
			<param name="applicationPermission">To be added.</param>
			<summary>Triggers the system UX for asking the user's permission for the requested <paramref name="applicationPermission" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous RequestApplicationPermission operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKApplicationPermissionStatus,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void RequestApplicationPermission (CKApplicationPermissions applicationPermission, Action<CKApplicationPermissionStatus, NSError> completionHandler);

		[Export ("fetchUserRecordIDWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Retrieves the <see cref="CloudKit.CKRecordID" /> of the current user.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchUserRecordId operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordID,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchUserRecordId (Action<CKRecordID, NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("discoverAllIdentitiesWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Fetches all user records in the <see cref="CloudKit.CKContainer" /> that correspond to an entry in the user's address book.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DiscoverAllIdentities operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKUserIdentity[],Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para>The "identity discovery" methods in <see cref="CloudKit.CKContainer" /> allow the developer to implement "friends who also use" functionality in their apps. These methods can be used to find user records in the CloudKit container that correspond to entries in the user's address book. No information about the user, beyond the fact that they use the app and agreed to share that status, is available from the <see cref="CloudKit.CKContainer" />.</para>
			        </remarks>
			""")]
		void DiscoverAllIdentities (Action<CKUserIdentity [], NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[MacCatalyst (13, 1)]
		[Export ("discoverUserIdentityWithEmailAddress:completionHandler:")]
		[Async (XmlDocs = """
			<param name="email">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKUserIdentity" /> data for the record with specified <paramref name="email" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DiscoverUserIdentityWithEmailAddress operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKUserIdentity,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para>The "identity discovery" methods in <see cref="CloudKit.CKContainer" /> allow the developer to implement "friends who also use" functionality in their apps. These methods can be used to find user records in the CloudKit container that correspond to entries in the user's address book. No information about the user, beyond the fact that they use the app and agreed to share that status, is available from the <see cref="CloudKit.CKContainer" />.</para>
			        </remarks>
			""")]
		void DiscoverUserIdentityWithEmailAddress (string email, Action<CKUserIdentity, NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[MacCatalyst (13, 1)]
		[Export ("discoverUserIdentityWithPhoneNumber:completionHandler:")]
		[Async (XmlDocs = """
			<param name="phoneNumber">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKUserIdentity" /> data for the record with specified <paramref name="phoneNumber" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DiscoverUserIdentityWithPhoneNumber operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKUserIdentity,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para>The "identity discovery" methods in <see cref="CloudKit.CKContainer" /> allow the developer to implement "friends who also use" functionality in their apps. These methods can be used to find user records in the CloudKit container that correspond to entries in the user's address book. No information about the user, beyond the fact that they use the app and agreed to share that status, is available from the <see cref="CloudKit.CKContainer" />.</para>
			        </remarks>
			""")]
		void DiscoverUserIdentityWithPhoneNumber (string phoneNumber, Action<CKUserIdentity, NSError> completionHandler);

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[MacCatalyst (13, 1)]
		[Export ("discoverUserIdentityWithUserRecordID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="userRecordID">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKUserIdentity" /> data for the record with specified <paramref name="userRecordID" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DiscoverUserIdentity operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKUserIdentity,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para>The "identity discovery" methods in <see cref="CloudKit.CKContainer" /> allow the developer to implement "friends who also use" functionality in their apps. These methods can be used to find user records in the CloudKit container that correspond to entries in the user's address book. No information about the user, beyond the fact that they use the app and agreed to share that status, is available from the <see cref="CloudKit.CKContainer" />.</para>
			        </remarks>
			""")]
		void DiscoverUserIdentity (CKRecordID userRecordID, Action<CKUserIdentity, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Field ("CKAccountChangedNotification")]
		[Notification]
		NSString AccountChangedNotification { get; }

		[NoTV] // does not answer on devices
		[MacCatalyst (13, 1)]
		[Export ("fetchAllLongLivedOperationIDsWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Returns an array that contains the identifiers for all the currently active long-lived operations.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchAllLongLivedOperationIDs operation.  The value of the TResult parameter is of type System.Action&lt;Foundation.NSDictionary&lt;Foundation.NSString,Foundation.NSOperation&gt;,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchAllLongLivedOperationIDs (Action<NSDictionary<NSString, NSOperation>, NSError> completionHandler);

		[NoTV] // does not answer on devices
		[MacCatalyst (13, 1)]
		[Export ("fetchLongLivedOperationWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="operationID">The ID of the long-lived operation to fetch.</param>
			<summary>Fetches the long-lived operation that is identified by <paramref name="operationID" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchLongLivedOperation operation.  The value of the TResult parameter is of type System.Action&lt;Foundation.NSDictionary&lt;Foundation.NSString,Foundation.NSOperation&gt;,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchLongLivedOperation (string [] operationID, Action<NSDictionary<NSString, NSOperation>, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("fetchShareParticipantWithEmailAddress:completionHandler:")]
		[Async (XmlDocs = """
			<param name="emailAddress">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKShareParticipant" /> information for the user who accepted a shared record..</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchShareParticipantWithEmailAddress operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKShareParticipant,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchShareParticipantWithEmailAddress (string emailAddress, Action<CKShareParticipant, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("fetchShareParticipantWithPhoneNumber:completionHandler:")]
		[Async (XmlDocs = """
			<param name="phoneNumber">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKShareParticipant" /> information for the user who accepted a shared record.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchShareParticipantWithPhoneNumber operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKShareParticipant,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchShareParticipantWithPhoneNumber (string phoneNumber, Action<CKShareParticipant, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("fetchShareParticipantWithUserRecordID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="userRecordID">To be added.</param>
			<summary>Retrieves the <see cref="CloudKit.CKShareParticipant" /> information for the user who accepted a shared record.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchShareParticipant operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKShareParticipant,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchShareParticipant (CKRecordID userRecordID, Action<CKShareParticipant, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("fetchShareMetadataWithURL:completionHandler:")]
		[Async (XmlDocs = """
			<param name="url">To be added.</param>
			<summary>To be added.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchShareMetadata operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKShareMetadata,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchShareMetadata (NSUrl url, Action<CKShareMetadata, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("acceptShareMetadata:completionHandler:")]
		[Async (XmlDocs = """
			<param name="metadata">To be added.</param>
			<summary>To be added.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous AcceptShareMetadata operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKShare,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The AcceptShareMetadataAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void AcceptShareMetadata (CKShareMetadata metadata, Action<CKShare, NSError> completionHandler);
	}

	/// <param name="subscriptionId">To be added.</param>
	/// <param name="error">To be added.</param>
	/// <summary>Completion handler for the <see cref="CloudKit.CKDatabase.DeleteSubscription(System.String,CloudKit.CKDatabaseDeleteSubscriptionHandler)" /> method.</summary>
	/// <remarks>To be added.</remarks>
	delegate void CKDatabaseDeleteSubscriptionHandler ([NullAllowed] string subscriptionId, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // *** Assertion failure in -[CKDatabase init]
	[BaseType (typeof (NSObject))]
	interface CKDatabase {
		[Export ("addOperation:")]
		void AddOperation (CKDatabaseOperation operation);

		[MacCatalyst (13, 1)]
		[Export ("databaseScope", ArgumentSemantic.Assign)]
		CKDatabaseScope DatabaseScope { get; }

		[Export ("fetchRecordWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="recordId">To be added.</param>
			<summary>Fetches the <see cref="CloudKit.CKRecord" /> with the specified <paramref name="recordId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchRecord operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecord,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchRecord (CKRecordID recordId, Action<CKRecord, NSError> completionHandler);

		[Export ("saveRecord:completionHandler:")]
		[Async (XmlDocs = """
			<param name="record">To be added.</param>
			<summary>Saves the specified <see cref="CloudKit.CKRecord" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous SaveRecord operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecord,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void SaveRecord (CKRecord record, Action<CKRecord, NSError> completionHandler);

		[Export ("deleteRecordWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="recordId">To be added.</param>
			<summary>Deletes the record with the <paramref name="recordId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DeleteRecord operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordID,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void DeleteRecord (CKRecordID recordId, Action<CKRecordID, NSError> completionHandler);

		[Export ("performQuery:inZoneWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="query">To be added.</param>
			<param name="zoneId">To be added.</param>
			<summary>Executes the <paramref name="query" /> on the zone identified by <paramref name="zoneId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous PerformQuery operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecord[],Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void PerformQuery (CKQuery query, [NullAllowed] CKRecordZoneID zoneId, Action<CKRecord [], NSError> completionHandler);

		[Export ("fetchAllRecordZonesWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Retrieves all record zones, with low priority.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchAllRecordZones operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordZone[],Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchAllRecordZones (Action<CKRecordZone [], NSError> completionHandler);

		[Export ("fetchRecordZoneWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="zoneId">To be added.</param>
			<summary>Fetches the <see cref="CloudKit.CKRecordZone" /> with the specified <paramref name="zoneId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchRecordZone operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordZone,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchRecordZone (CKRecordZoneID zoneId, Action<CKRecordZone, NSError> completionHandler);

		[Export ("saveRecordZone:completionHandler:")]
		[Async (XmlDocs = """
			<param name="zone">To be added.</param>
			<summary>Saves the specified <paramref name="zone" /> to the current database.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous SaveRecordZone operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordZone,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void SaveRecordZone (CKRecordZone zone, Action<CKRecordZone, NSError> completionHandler);

		[Export ("deleteRecordZoneWithID:completionHandler:")]
		[Async (XmlDocs = """
			<param name="zoneId">To be added.</param>
			<summary>Deletes the zone at the specified <paramref name="zoneId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DeleteRecordZone operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKRecordZoneID,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void DeleteRecordZone (CKRecordZoneID zoneId, Action<CKRecordZoneID, NSError> completionHandler);

		[Export ("fetchSubscriptionWithID:completionHandler:")]
		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<param name="subscriptionId">To be added.</param>
			<summary>Fetches the <see cref="CloudKit.CKSubscription" /> with the specified <paramref name="subscriptionId" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchSubscription operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKSubscription,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchSubscription (string subscriptionId, Action<CKSubscription, NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("fetchAllSubscriptionsWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Retrieves all the <see cref="CloudKit.CKSubscription" /> objects from the database.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FetchAllSubscriptions operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKSubscription[],Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FetchAllSubscriptions (Action<CKSubscription [], NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Export ("saveSubscription:completionHandler:")]
		[Async (XmlDocs = """
			<param name="subscription">To be added.</param>
			<summary>Saves the specified <paramref name="subscription" /> to the current database.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous SaveSubscription operation.  The value of the TResult parameter is of type System.Action&lt;CloudKit.CKSubscription,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void SaveSubscription (CKSubscription subscription, Action<CKSubscription, NSError> completionHandler);

		[Export ("deleteSubscriptionWithID:completionHandler:")]
		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<param name="subscriptionID">To be added.</param>
			<summary>Deletes the <see cref="CloudKit.CKSubscription" /> with the specified <paramref name="subscriptionID" />.</summary>
			<returns>
			          <para>A task that represents the asynchronous DeleteSubscription operation.   The value of the TResult parameter is a CloudKit.CKDatabaseDeleteSubscriptionHandler.</para>
			        </returns>
			<remarks>
			          <para copied="true">The DeleteSubscriptionAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void DeleteSubscription (string subscriptionID, CKDatabaseDeleteSubscriptionHandler completionHandler);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor]
	[Abstract]
	interface CKDatabaseOperation {

		[Export ("database", ArgumentSemantic.Retain)]
		[NullAllowed]
		CKDatabase Database { get; set; }
	}

	// CKError.h Fields
	/// <summary>Holds error constants used by CloudKit.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Static]
	interface CKErrorFields {
		/// <summary>Represents the value associated with the constant CKPartialErrorsByItemIDKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKPartialErrorsByItemIDKey")]
		NSString PartialErrorsByItemIdKey { get; }

		/// <summary>Represents the value associated with the constant CKRecordChangedErrorAncestorRecordKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKRecordChangedErrorAncestorRecordKey")]
		NSString RecordChangedErrorAncestorRecordKey { get; }

		/// <summary>Represents the value associated with the constant CKRecordChangedErrorServerRecordKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKRecordChangedErrorServerRecordKey")]
		NSString RecordChangedErrorServerRecordKey { get; }

		/// <summary>Represents the value associated with the constant CKRecordChangedErrorClientRecordKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKRecordChangedErrorClientRecordKey")]
		NSString RecordChangedErrorClientRecordKey { get; }

		/// <summary>Represents the value associated with the constant CKErrorRetryAfterKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKErrorRetryAfterKey")]
		NSString ErrorRetryAfterKey { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CKErrorUserDidResetEncryptedDataKey")]
		NSString UserDidResetEncryptedDataKey { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // Objective-C exception thrown.  Name: CKException Reason: You can't call init on CKServerChangeToken
	[BaseType (typeof (NSObject))]
	interface CKServerChangeToken : NSCopying, NSSecureCoding {

	}

	/// <param name="serverChangeToken">To be added.</param>
	/// <param name="clientChangeTokenData">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKFetchRecordChangesOperation.AllChangesReported" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKFetchRecordChangesHandler ([NullAllowed] CKServerChangeToken serverChangeToken, [NullAllowed] NSData clientChangeTokenData, [NullAllowed] NSError operationError);

	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKFetchRecordZoneChangesOperation' instead.")]
	[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'CKFetchRecordZoneChangesOperation' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKFetchRecordZoneChangesOperation' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKFetchRecordZoneChangesOperation' instead.")]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchRecordChangesOperation {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithRecordZoneID:previousServerChangeToken:")]
		NativeHandle Constructor (CKRecordZoneID recordZoneID, [NullAllowed] CKServerChangeToken previousServerChangeToken);

		[NullAllowed] // by default this property is null
		[Export ("recordZoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID RecordZoneId { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("previousServerChangeToken", ArgumentSemantic.Copy)]
		CKServerChangeToken PreviousServerChangeToken { get; set; }

		[Export ("resultsLimit", ArgumentSemantic.UnsafeUnretained)]
		nuint ResultsLimit { get; set; }

		[Export ("desiredKeys", ArgumentSemantic.Copy)]
		[NullAllowed]
		string [] DesiredKeys { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("recordChangedBlock", ArgumentSemantic.Copy)]
		Action<CKRecord> RecordChanged {
			get;
			set;
		}

		[NullAllowed] // by default this property is null
		[Export ("recordWithIDWasDeletedBlock", ArgumentSemantic.Copy)]
		Action<CKRecordID> RecordDeleted {
			get;
			set;
		}

		[Export ("moreComing")]
		bool MoreComing { get; }

		[NullAllowed] // by default this property is null
		[Export ("fetchRecordChangesCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchRecordChangesHandler AllChangesReported {
			get;
			set;
		}
	}

	[MacCatalyst (13, 1)]
	delegate void CKFetchRecordZoneChangesWithIDWasDeletedHandler (CKRecordID recordID, NSString recordType);

	[MacCatalyst (13, 1)]
	delegate void CKFetchRecordZoneChangesTokensUpdatedHandler ([NullAllowed] CKRecordZoneID recordZoneID, [NullAllowed] CKServerChangeToken serverChangeToken, [NullAllowed] NSData clientChangeTokenData);

	[MacCatalyst (13, 1)]
	delegate void CKFetchRecordZoneChangesFetchCompletedHandler ([NullAllowed] CKRecordZoneID recordZoneID, [NullAllowed] CKServerChangeToken serverChangeToken, [NullAllowed] NSData clientChangeTokenData, bool moreComing, [NullAllowed] NSError recordZoneError);

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	delegate void CKFetchRecordZoneChangesRecordWasChangedHandler ([NullAllowed] CKRecordID recordId, [NullAllowed] CKRecord record, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchRecordZoneChangesOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Deprecated (PlatformName.TvOS, 12, 0, message: "Use the overload with the 'NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration>' parameter instead.")]
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use the overload with the 'NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration>' parameter instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use the overload with the 'NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration>' parameter instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the overload with the 'NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration>' parameter instead.")]
		[Export ("initWithRecordZoneIDs:optionsByRecordZoneID:")]
		NativeHandle Constructor (CKRecordZoneID [] recordZoneIDs, [NullAllowed] NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesOptions> optionsByRecordZoneID);

		[MacCatalyst (13, 1)]
		[Export ("initWithRecordZoneIDs:configurationsByRecordZoneID:")]
		NativeHandle Constructor (CKRecordZoneID [] recordZoneIDs, [NullAllowed] NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration> configurationsByRecordZoneID);

		[NullAllowed]
		[Export ("recordZoneIDs", ArgumentSemantic.Copy)]
		CKRecordZoneID [] RecordZoneIDs { get; set; }

		[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'ConfigurationsByRecordZoneID' instead.")]
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ConfigurationsByRecordZoneID' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'ConfigurationsByRecordZoneID' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ConfigurationsByRecordZoneID' instead.")]
		[NullAllowed, Export ("optionsByRecordZoneID", ArgumentSemantic.Copy)]
		NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesOptions> OptionsByRecordZoneID { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("configurationsByRecordZoneID", ArgumentSemantic.Copy)]
		NSDictionary<CKRecordZoneID, CKFetchRecordZoneChangesConfiguration> ConfigurationsByRecordZoneID { get; set; }

		[Export ("fetchAllChanges")]
		bool FetchAllChanges { get; set; }

		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'RecordWasChangedHandler' instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'RecordWasChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'RecordWasChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'RecordWasChangedHandler' instead.")]
		[NullAllowed, Export ("recordChangedBlock", ArgumentSemantic.Copy)]
		Action<CKRecord> RecordChanged { get; set; }

		[NullAllowed, Export ("recordWithIDWasDeletedBlock", ArgumentSemantic.Copy)]
		CKFetchRecordZoneChangesWithIDWasDeletedHandler RecordWithIDWasDeleted { get; set; }

		[NullAllowed, Export ("recordZoneChangeTokensUpdatedBlock", ArgumentSemantic.Copy)]
		CKFetchRecordZoneChangesTokensUpdatedHandler RecordZoneChangeTokensUpdated { get; set; }

		[NullAllowed, Export ("recordZoneFetchCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchRecordZoneChangesFetchCompletedHandler FetchCompleted { get; set; }

		[NullAllowed, Export ("fetchRecordZoneChangesCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> ChangesCompleted { get; set; }

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("recordWasChangedBlock", ArgumentSemantic.Copy)]
		CKFetchRecordZoneChangesRecordWasChangedHandler RecordWasChangedHandler { get; set; }
	}

	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'CKFetchRecordZoneChangesConfiguration' instead.")]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'CKFetchRecordZoneChangesConfiguration' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'CKFetchRecordZoneChangesConfiguration' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKFetchRecordZoneChangesConfiguration' instead.")]
	[BaseType (typeof (NSObject))]
	interface CKFetchRecordZoneChangesOptions : NSSecureCoding, NSCopying {
		[NullAllowed, Export ("previousServerChangeToken", ArgumentSemantic.Copy)]
		CKServerChangeToken PreviousServerChangeToken { get; set; }

		[Export ("resultsLimit")]
		nuint ResultsLimit { get; set; }

		[NullAllowed, Export ("desiredKeys", ArgumentSemantic.Copy)]
		string [] DesiredKeys { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CKFetchRecordZoneChangesConfiguration : NSSecureCoding, NSCopying {

		[NullAllowed, Export ("previousServerChangeToken", ArgumentSemantic.Copy)]
		CKServerChangeToken PreviousServerChangeToken { get; set; }

		[Export ("resultsLimit")]
		nuint ResultsLimit { get; set; }

		[NullAllowed, Export ("desiredKeys", ArgumentSemantic.Copy)]
		string [] DesiredKeys { get; set; }
	}

	/// <param name="recordsByRecordId">To be added.</param>
	/// <param name="error">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKFetchRecordsOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKFetchRecordsCompletedHandler ([NullAllowed] NSDictionary recordsByRecordId, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (CKDatabaseOperation))]
	interface CKFetchRecordsOperation {

		[Export ("initWithRecordIDs:")]
		NativeHandle Constructor (CKRecordID [] recordIds);

		[NullAllowed] // by default this property is null
		[Export ("recordIDs", ArgumentSemantic.Copy)]
		CKRecordID [] RecordIds { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("desiredKeys", ArgumentSemantic.Copy)]
		string [] DesiredKeys { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("perRecordProgressBlock", ArgumentSemantic.Copy)]
		Action<CKRecordID, double> PerRecordProgress {
			get;
			set;
		}

		[NullAllowed] // by default this property is null
		[Export ("perRecordCompletionBlock", ArgumentSemantic.Copy)]
		Action<CKRecord, CKRecordID, NSError> PerRecordCompletion {
			get;
			set;
		}

		[NullAllowed] // by default this property is null
		[Export ("fetchRecordsCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchRecordsCompletedHandler Completed {
			get;
			set;
		}

		[Static]
		[Export ("fetchCurrentUserRecordOperation")]
		CKFetchRecordsOperation FetchCurrentUserRecordOperation ();
	}

	/// <param name="recordZonesByZoneId">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKFetchRecordZonesOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKRecordZoneCompleteHandler ([NullAllowed] NSDictionary recordZonesByZoneId, [NullAllowed] NSError operationError);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKRecordZonePerRecordZoneCompletionHandler ([NullAllowed] CKRecordZoneID recordZoneId, [NullAllowed] CKRecordZone recordZone, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DesignatedDefaultCtor]
	interface CKFetchRecordZonesOperation {

		[Export ("initWithRecordZoneIDs:")]
		NativeHandle Constructor (CKRecordZoneID [] zoneIds);

		[NullAllowed] // by default this property is null
		[Export ("recordZoneIDs", ArgumentSemantic.Copy)]
		CKRecordZoneID [] RecordZoneIds { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("fetchRecordZonesCompletionBlock", ArgumentSemantic.Copy)]
		CKRecordZoneCompleteHandler Completed {
			get;
			set;
		}

		[Static]
		[Export ("fetchAllRecordZonesOperation")]
		CKFetchRecordZonesOperation FetchAllRecordZonesOperation ();

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perRecordZoneCompletionBlock", ArgumentSemantic.Copy)]
		CKRecordZonePerRecordZoneCompletionHandler PerRecordZoneCompletionHandler { get; set; }
	}

	/// <param name="subscriptionsBySubscriptionId">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKFetchSubscriptionsOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKFetchSubscriptionsCompleteHandler ([NullAllowed] NSDictionary subscriptionsBySubscriptionId, [NullAllowed] NSError operationError);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKFetchSubscriptionsPerSubscriptionCompletionHandler ([NullAllowed] NSString subscriptionId, [NullAllowed] CKSubscription subscription, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchSubscriptionsOperation {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithSubscriptionIDs:")]
		NativeHandle Constructor (string [] subscriptionIds);

		[NullAllowed] // by default this property is null
		[Export ("subscriptionIDs", ArgumentSemantic.Copy)]
		string [] SubscriptionIds { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("fetchSubscriptionCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchSubscriptionsCompleteHandler Completed {
			get;
			set;
		}

		[Static]
		[Export ("fetchAllSubscriptionsOperation")]
		CKFetchSubscriptionsOperation FetchAllSubscriptionsOperation ();

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perSubscriptionCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchSubscriptionsPerSubscriptionCompletionHandler PerSubscriptionCompletionHandler { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSSortDescriptor))]
	interface CKLocationSortDescriptor : NSSecureCoding {
		[DesignatedInitializer]
		[Export ("initWithKey:relativeLocation:")]
		NativeHandle Constructor (string key, CLLocation relativeLocation);

		[Export ("relativeLocation", ArgumentSemantic.Copy)]
		CLLocation RelativeLocation { get; }
	}

	/// <param name="savedRecords">To be added.</param>
	/// <param name="deletedRecordIds">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKModifyRecordsOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKModifyRecordsOperationHandler ([NullAllowed] CKRecord [] savedRecords, [NullAllowed] CKRecordID [] deletedRecordIds, [NullAllowed] NSError operationError);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifyRecordsOperationPerRecordSaveHandler ([NullAllowed] CKRecordID recordId, [NullAllowed] CKRecord record, [NullAllowed] NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifyRecordsOperationPerRecordDeleteHandler ([NullAllowed] CKRecordID recordId, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (CKDatabaseOperation))]
	interface CKModifyRecordsOperation {

		[Export ("initWithRecordsToSave:recordIDsToDelete:")]
		NativeHandle Constructor ([NullAllowed] CKRecord [] recordsToSave, [NullAllowed] CKRecordID [] recordsToDelete);

		[NullAllowed] // by default this property is null
		[Export ("recordsToSave", ArgumentSemantic.Copy)]
		CKRecord [] RecordsToSave { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("recordIDsToDelete", ArgumentSemantic.Copy)]
		CKRecordID [] RecordIdsToDelete { get; set; }

		[Export ("savePolicy", ArgumentSemantic.Assign)]
		CKRecordSavePolicy SavePolicy { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("clientChangeTokenData", ArgumentSemantic.Copy)]
		NSData ClientChangeTokenData { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("perRecordProgressBlock", ArgumentSemantic.Copy)]
		Action<CKRecord, double> PerRecordProgress {
			get;
			set;
		}

		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'PerRecordResultHandler' instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'PerRecordResultHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'PerRecordResultHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'PerRecordResultHandler' instead.")]
		[NullAllowed] // by default this property is null
		[Export ("perRecordCompletionBlock", ArgumentSemantic.Copy)]
		Action<CKRecord, NSError> PerRecordCompletion {
			get;
			set;
		}

		[NullAllowed] // by default this property is null
		[Export ("modifyRecordsCompletionBlock", ArgumentSemantic.Copy)]
		CKModifyRecordsOperationHandler Completed {
			get;
			set;
		}

		[Export ("atomic")]
		bool Atomic { get; set; }

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perRecordSaveBlock", ArgumentSemantic.Copy)]
		CKModifyRecordsOperationPerRecordSaveHandler PerRecordSaveHandler { get; set; }

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perRecordDeleteBlock", ArgumentSemantic.Copy)]
		CKModifyRecordsOperationPerRecordDeleteHandler PerRecordDeleteHandler { get; set; }

	}

	/// <param name="savedRecordZones">To be added.</param>
	/// <param name="deletedRecordZoneIds">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKModifyRecordZonesOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKModifyRecordZonesHandler ([NullAllowed] CKRecordZone [] savedRecordZones, [NullAllowed] CKRecordZoneID [] deletedRecordZoneIds, [NullAllowed] NSError operationError);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifyRecordZonesPerRecordZoneSaveHandler ([NullAllowed] CKRecordZoneID zoneId, [NullAllowed] CKRecordZone zone, [NullAllowed] NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifyRecordZonesPerRecordZoneDeleteHandler ([NullAllowed] CKRecordZoneID zoneId, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (CKDatabaseOperation))]
	interface CKModifyRecordZonesOperation {

		[Export ("initWithRecordZonesToSave:recordZoneIDsToDelete:")]
		NativeHandle Constructor ([NullAllowed] CKRecordZone [] recordZonesToSave, [NullAllowed] CKRecordZoneID [] recordZoneIdsToDelete);

		[NullAllowed] // by default this property is null
		[Export ("recordZonesToSave", ArgumentSemantic.Copy)]
		CKRecordZone [] RecordZonesToSave { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("recordZoneIDsToDelete", ArgumentSemantic.Copy)]
		CKRecordZoneID [] RecordZoneIdsToDelete { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("modifyRecordZonesCompletionBlock", ArgumentSemantic.Copy)]
		CKModifyRecordZonesHandler Completed {
			get;
			set;
		}

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perRecordZoneSaveBlock", ArgumentSemantic.Copy)]
		CKModifyRecordZonesPerRecordZoneSaveHandler PerRecordZoneSaveHandler { get; set; }

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perRecordZoneDeleteBlock", ArgumentSemantic.Copy)]
		CKModifyRecordZonesPerRecordZoneDeleteHandler PerRecordZoneDeleteHandler { get; set; }
	}

	/// <param name="savedSubscriptions">To be added.</param>
	/// <param name="deletedSubscriptionIds">To be added.</param>
	/// <param name="operationError">To be added.</param>
	/// <summary>Delegate for the <see cref="CloudKit.CKModifySubscriptionsOperation.Completed" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	delegate void CKModifySubscriptionsHandler ([NullAllowed] CKSubscription [] savedSubscriptions, [NullAllowed] string [] deletedSubscriptionIds, [NullAllowed] NSError operationError);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifySubscriptionsPerSubscriptionSaveHandler ([NullAllowed] NSString subscriptionId, [NullAllowed] CKSubscription subscription, [NullAllowed] NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKModifySubscriptionsPerSubscriptionDeleteHandler ([NullAllowed] NSString subscriptionId, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKModifySubscriptionsOperation {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithSubscriptionsToSave:subscriptionIDsToDelete:")]
		NativeHandle Constructor ([NullAllowed] CKSubscription [] subscriptionsToSave, [NullAllowed] string [] subscriptionIdsToDelete);

		[NullAllowed] // by default this property is null
		[Export ("subscriptionsToSave", ArgumentSemantic.Copy)]
		CKSubscription [] SubscriptionsToSave { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("subscriptionIDsToDelete", ArgumentSemantic.Copy)]
		string [] SubscriptionIdsToDelete { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("modifySubscriptionsCompletionBlock", ArgumentSemantic.Copy)]
		CKModifySubscriptionsHandler Completed {
			get;
			set;
		}

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perSubscriptionSaveBlock", ArgumentSemantic.Copy)]
		CKModifySubscriptionsPerSubscriptionSaveHandler PerSubscriptionSaveHandler { get; set; }

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("perSubscriptionDeleteBlock", ArgumentSemantic.Copy)]
		CKModifySubscriptionsPerSubscriptionDeleteHandler PerSubscriptionDeleteHandler { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // doc: <quote>You do not create notification IDs directly.</quote>
	[BaseType (typeof (NSObject))]
	interface CKNotificationID : NSCopying, NSSecureCoding, NSCoding {

	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: CKNotification is not meant for direct instantiation
	[BaseType (typeof (NSObject))]
	[Abstract]
	interface CKNotification : NSSecureCoding {

		[Export ("notificationType", ArgumentSemantic.Assign)]
		CKNotificationType NotificationType { get; }

		[NullAllowed]
		[Export ("notificationID", ArgumentSemantic.Copy)]
		CKNotificationID NotificationId { get; }

		[NullAllowed, Export ("containerIdentifier")]
		string ContainerIdentifier { get; }

		[Export ("isPruned", ArgumentSemantic.UnsafeUnretained)]
		bool IsPruned { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("alertBody")]
		string AlertBody { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("alertLocalizationKey")]
		string AlertLocalizationKey { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("alertLocalizationArgs", ArgumentSemantic.Copy)]
		string [] AlertLocalizationArgs { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("alertActionLocalizationKey")]
		string AlertActionLocalizationKey { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("alertLaunchImage")]
		string AlertLaunchImage { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.TvOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("badge", ArgumentSemantic.Copy)]
		NSNumber Badge { get; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use the UserNotifications framework instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use the UserNotifications framework instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("soundName")]
		string SoundName { get; }

		[Static]
		[Export ("notificationFromRemoteNotificationDictionary:")]
		[return: NullAllowed]
		CKNotification FromRemoteNotificationDictionary (NSDictionary notificationDictionary);

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subscriptionID")]
		string SubscriptionID { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("category")]
		string Category { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("title")]
		string Title { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("titleLocalizationKey")]
		string TitleLocalizationKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("titleLocalizationArgs", ArgumentSemantic.Copy)]
		string [] TitleLocalizationArgs { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitle")]
		string Subtitle { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitleLocalizationKey")]
		string SubtitleLocalizationKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitleLocalizationArgs", ArgumentSemantic.Copy)]
		string [] SubtitleLocalizationArgs { get; }

		[TV (14, 0), Mac (10, 16), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("subscriptionOwnerUserRecordID", ArgumentSemantic.Copy)]
		CKRecordID SubscriptionOwnerUserRecordId { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: CKQueryNotification is not meant for direct instantiation
	[BaseType (typeof (CKNotification))]
	interface CKQueryNotification : NSCoding, NSSecureCoding {

		[Export ("queryNotificationReason", ArgumentSemantic.Assign)]
		CKQueryNotificationReason QueryNotificationReason { get; }

		[NullAllowed, Export ("recordFields", ArgumentSemantic.Copy)]
#if XAMCORE_5_0 // delayed until next time due to #13704.
		NSDictionary<NSString, NSObject> RecordFields { get; }
#else
		NSDictionary RecordFields { get; }
#endif

		[NullAllowed, Export ("recordID", ArgumentSemantic.Copy)]
		CKRecordID RecordId { get; }

		[MacCatalyst (13, 1)]
		[Export ("databaseScope", ArgumentSemantic.Assign)]
		CKDatabaseScope DatabaseScope { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // objc_exception_throw on CKNotification init
	[BaseType (typeof (CKNotification))]
	interface CKRecordZoneNotification : NSCoding, NSSecureCoding {

		[NullAllowed]
		[Export ("recordZoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID RecordZoneId { get; }

		[MacCatalyst (13, 1)]
		[Export ("databaseScope", ArgumentSemantic.Assign)]
		CKDatabaseScope DatabaseScope { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // objc_exception_throw on CKNotification init
	[BaseType (typeof (CKNotification))]
	interface CKDatabaseNotification {
		[Export ("databaseScope", ArgumentSemantic.Assign)]
		CKDatabaseScope DatabaseScope { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CKOperationConfiguration : NSSecureCoding, NSCopying {
		[NullAllowed, Export ("container", ArgumentSemantic.Strong)]
		CKContainer Container { get; set; }

		[Export ("qualityOfService", ArgumentSemantic.Assign)]
		NSQualityOfService QualityOfService { get; set; }

		[Export ("allowsCellularAccess")]
		bool AllowsCellularAccess { get; set; }

		/// <summary>Gets or sets a Boolean value that tells whether the operation is long-lived.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("longLived")]
		bool LongLived { [Bind ("isLongLived")] get; set; }

		[Export ("timeoutIntervalForRequest")]
		double TimeoutIntervalForRequest { get; set; }

		[Export ("timeoutIntervalForResource")]
		double TimeoutIntervalForResource { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSOperation))]
	[DisableDefaultCtor]
	[Abstract]
	interface CKOperation {

		[Protected] // since it should (and will) be `abstract`
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		// Apple removed, without deprecation, this property in iOS 9.3 SDK
		// 
		// [Export ("activityStart")]
		// ulong ActivityStart ();

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKOperationConfiguration' instead.")]
		[NullAllowed, Export ("container", ArgumentSemantic.Retain)]
		CKContainer Container { get; set; }

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKOperationConfiguration' instead.")]
		[Export ("allowsCellularAccess", ArgumentSemantic.UnsafeUnretained)]
		bool AllowsCellularAccess { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("operationID")]
		string OperationID { get; }

		/// <summary>Gets or sets a value that tells whether this operation is long-lived.</summary>
		///         <value>A value that tells whether this operation is long-lived.</value>
		///         <remarks>To be added.</remarks>
		[Export ("longLived")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKOperationConfiguration' instead.")]
		bool LongLived { [Bind ("isLongLived")] get; set; }

		[Export ("timeoutIntervalForRequest")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKOperationConfiguration' instead.")]
		double TimeoutIntervalForRequest { get; set; }

		[Export ("timeoutIntervalForResource")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CKOperationConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CKOperationConfiguration' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKOperationConfiguration' instead.")]
		double TimeoutIntervalForResource { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("longLivedOperationWasPersistedBlock", ArgumentSemantic.Strong)]
		Action LongLivedOperationWasPersistedCallback { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("configuration", ArgumentSemantic.Copy)]
		CKOperationConfiguration Configuration { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("group", ArgumentSemantic.Strong)]
		CKOperationGroup Group { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface CKOperationGroup : NSSecureCoding {

		[Export ("operationGroupID")]
		string OperationGroupId { get; }

		[NullAllowed] // null_resettable
		[Export ("defaultConfiguration", ArgumentSemantic.Copy)]
		CKOperationConfiguration DefaultConfiguration { get; set; }

		[NullAllowed, Export ("name")]
		string Name { get; set; }

		[Export ("quantity")]
		nuint Quantity { get; set; }

		[Export ("expectedSendSize", ArgumentSemantic.Assign)]
		CKOperationGroupTransferSize ExpectedSendSize { get; set; }

		[Export ("expectedReceiveSize", ArgumentSemantic.Assign)]
		CKOperationGroupTransferSize ExpectedReceiveSize { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: You must call -[CKQuery initWithRecordType:predicate:sortDescriptors:]
	[BaseType (typeof (NSObject))]
	interface CKQuery : NSSecureCoding, NSCopying {

		[DesignatedInitializer]
		[Export ("initWithRecordType:predicate:")]
		NativeHandle Constructor (string recordType, NSPredicate predicate);

		[Export ("recordType")]
		string RecordType { get; }

		[Export ("predicate", ArgumentSemantic.Copy)]
		NSPredicate Predicate { get; }

		[NullAllowed, Export ("sortDescriptors", ArgumentSemantic.Copy)]
		NSSortDescriptor [] SortDescriptors { get; set; }
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void CKQueryOperationRecordMatchedHandler ([NullAllowed] CKRecordID recordId, [NullAllowed] CKRecord record, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DesignatedDefaultCtor]
	interface CKQueryOperation {

		[Field ("CKQueryOperationMaximumResults")]
		nint MaximumResults { get; }

		[Export ("initWithQuery:")]
		NativeHandle Constructor (CKQuery query);

		[Export ("initWithCursor:")]
		NativeHandle Constructor (CKQueryCursor cursor);

		[NullAllowed] // by default this property is null
		[Export ("query", ArgumentSemantic.Copy)]
		CKQuery Query { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("cursor", ArgumentSemantic.Copy)]
		CKQueryCursor Cursor { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneId { get; set; }

		[Export ("resultsLimit", ArgumentSemantic.UnsafeUnretained)]
		nuint ResultsLimit { get; set; }

		[Export ("desiredKeys", ArgumentSemantic.Copy)]
		[NullAllowed]
		string [] DesiredKeys { get; set; }

		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'RecordMatchedHandler' instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'RecordMatchedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'RecordMatchedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'RecordMatchedHandler' instead.")]
		[NullAllowed] // by default this property is null
		[Export ("recordFetchedBlock", ArgumentSemantic.Copy)]
		Action<CKRecord> RecordFetched {
			get;
			set;
		}

		[NullAllowed] // by default this property is null
		[Export ("queryCompletionBlock", ArgumentSemantic.Copy)]
		Action<CKQueryCursor, NSError> Completed {
			get;
			set;
		}

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("recordMatchedBlock", ArgumentSemantic.Copy)]
		CKQueryOperationRecordMatchedHandler RecordMatchedHandler { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="CloudKit.CKRecordValue" />.</summary>
	/// <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="CloudKit.CKRecordValue" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="CloudKit.CKRecordValue" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="CloudKit.CKRecordValue_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CKRecordValue {

	}

	interface ICKRecordValue { }

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // Crashes [CKRecord init] objc_exception_throw
	[BaseType (typeof (NSObject))]
	interface CKRecord : NSSecureCoding, NSCopying {

		/// <summary>Represents the value associated with the constant CKRecordTypeUserRecord</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CKRecordTypeUserRecord")]
		NSString TypeUserRecord { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CKRecordParentKey")]
		NSString ParentKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CKRecordShareKey")]
		NSString ShareKey { get; }

		[TV (17, 0)]
		[Field ("CKRecordRecordIDKey")]
		NSString RecordIdKey { get; }

		[TV (17, 0)]
		[Field ("CKRecordModificationDateKey")]
		NSString ModificationDateKey { get; }

		[TV (17, 0)]
		[Field ("CKRecordLastModifiedUserRecordIDKey")]
		NSString LastModifiedUserRecordIdKey { get; }

		[TV (17, 0)]
		[Field ("CKRecordCreatorUserRecordIDKey")]
		NSString CreatorUserRecordIdKey { get; }

		[TV (17, 0)]
		[Field ("CKRecordCreationDateKey")]
		NSString CreationDateKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CKRecordTypeShare")]
		NSString TypeShare { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CKRecordNameZoneWideShare")]
		NSString NameZoneWideShare { get; }

		[Export ("initWithRecordType:")]
		NativeHandle Constructor (string recordType);

		[Export ("initWithRecordType:recordID:")]
		NativeHandle Constructor (string recordType, CKRecordID recordId);

		[Export ("initWithRecordType:zoneID:")]
		NativeHandle Constructor (string recordType, CKRecordZoneID zoneId);

		[Export ("recordType")]
		string RecordType { get; }

		[Export ("recordID", ArgumentSemantic.Copy)]
		CKRecordID Id { get; }

		[NullAllowed, Export ("recordChangeTag")]
		string RecordChangeTag { get; }

		[NullAllowed, Export ("creatorUserRecordID", ArgumentSemantic.Copy)]
		CKRecordID CreatorUserRecordId { get; }

		[NullAllowed, Export ("creationDate", ArgumentSemantic.Copy)]
		NSDate CreationDate { get; }

		[NullAllowed, Export ("lastModifiedUserRecordID", ArgumentSemantic.Copy)]
		CKRecordID LastModifiedUserRecordId { get; }

		[NullAllowed, Export ("modificationDate", ArgumentSemantic.Copy)]
		NSDate ModificationDate { get; }

		[return: NullAllowed]
		[Export ("objectForKey:")]
		[Internal]
		NSObject _ObjectForKey (string key);

		[Export ("setObject:forKey:")]
		[Internal]
		void _SetObject (IntPtr obj, string key);

		[Export ("allKeys")]
		string [] AllKeys ();

		[Export ("allTokens")]
		string [] AllTokens ();

		// No need for this ones
		//		[Export ("objectForKeyedSubscript:")]
		//		NSObject ObjectForKeyedSubscript (string key);
		//
		//		[Export ("setObject:forKeyedSubscript:")]
		//		void SetObject (CKRecordValue obj, string key);

		[Export ("changedKeys")]
		string [] ChangedKeys ();

		[Export ("encodeSystemFieldsWithCoder:")]
		void EncodeSystemFields (NSCoder coder);

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("share", ArgumentSemantic.Copy)]
		CKReference Share { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("parent", ArgumentSemantic.Copy)]
		CKReference Parent { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("setParentReferenceFromRecord:")]
		void SetParent ([NullAllowed] CKRecord parentRecord);

		[MacCatalyst (13, 1)]
		[Export ("setParentReferenceFromRecordID:")]
		void SetParent ([NullAllowed] CKRecordID parentRecordID);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSInvalidArgumentException You must call -[CKRecordID initWithRecordName:] or -[CKRecordID initWithRecordName:zoneID:]
	interface CKRecordID : NSSecureCoding, NSCopying {

		[Export ("initWithRecordName:")]
		NativeHandle Constructor (string recordName);

		[DesignatedInitializer]
		[Export ("initWithRecordName:zoneID:")]
		NativeHandle Constructor (string recordName, CKRecordZoneID zoneId);

		[Export ("recordName", ArgumentSemantic.Retain)]
		string RecordName { get; }

		[Export ("zoneID", ArgumentSemantic.Retain)]
		CKRecordZoneID ZoneId { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CKRecordZone : NSSecureCoding, NSCopying {

		/// <summary>Represents the value associated with the constant CKRecordZoneDefaultName</summary>
		///         <value>
		///         </value>
		///         <remarks>The name of the default zone.</remarks>
		[Field ("CKRecordZoneDefaultName")]
		NSString DefaultName { get; }

		[Export ("initWithZoneName:")]
		NativeHandle Constructor (string zoneName);

		[Export ("initWithZoneID:")]
		NativeHandle Constructor (CKRecordZoneID zoneId);

		[Export ("zoneID", ArgumentSemantic.Retain)]
		CKRecordZoneID ZoneId { get; }

		[Export ("capabilities", ArgumentSemantic.UnsafeUnretained)]
		CKRecordZoneCapabilities Capabilities { get; }

		[Static]
		[Export ("defaultRecordZone")]
		CKRecordZone DefaultRecordZone ();

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("share", ArgumentSemantic.Copy)]
		CKReference Share { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSInvalidArgumentException You must call -[CKRecordZoneID initWithZoneName:ownerName:]
	interface CKRecordZoneID : NSSecureCoding, NSCopying {

		[DesignatedInitializer]
		[Export ("initWithZoneName:ownerName:")]
		NativeHandle Constructor (string zoneName, string ownerName);

		[Export ("zoneName", ArgumentSemantic.Retain)]
		string ZoneName { get; }

		[Export ("ownerName", ArgumentSemantic.Retain)]
		string OwnerName { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: You must call -[CKReference initWithRecordID:] or -[CKReference initWithRecord:] or -[CKReference initWithAsset:]
	[BaseType (typeof (NSObject))]
	interface CKReference : NSSecureCoding, NSCopying, CKRecordValue {

		[DesignatedInitializer]
		[Export ("initWithRecordID:action:")]
		NativeHandle Constructor (CKRecordID recordId, CKReferenceAction action);

		[Export ("initWithRecord:action:")]
		NativeHandle Constructor (CKRecord record, CKReferenceAction action);

		[Export ("referenceAction", ArgumentSemantic.Assign)]
		CKReferenceAction ReferenceAction { get; }

		[Export ("recordID", ArgumentSemantic.Copy)]
		CKRecordID RecordId { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CKSubscription))]
	interface CKQuerySubscription : NSSecureCoding, NSCopying {
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[Deprecated (PlatformName.MacCatalyst, 10, 0)]
		[Export ("initWithRecordType:predicate:options:")]
		NativeHandle Constructor (string recordType, NSPredicate predicate, CKQuerySubscriptionOptions querySubscriptionOptions);

		[Export ("initWithRecordType:predicate:subscriptionID:options:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string recordType, NSPredicate predicate, string subscriptionID, CKQuerySubscriptionOptions querySubscriptionOptions);

		[Export ("recordType")]
		string RecordType { get; }

		[Export ("predicate", ArgumentSemantic.Copy)]
		NSPredicate Predicate { get; }

		[NullAllowed, Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneID { get; set; }

		[Export ("querySubscriptionOptions", ArgumentSemantic.Assign)]
		CKQuerySubscriptionOptions SubscriptionOptions { get; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CKSubscription))]
	interface CKRecordZoneSubscription : NSSecureCoding, NSCopying {

		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[Deprecated (PlatformName.MacCatalyst, 10, 0)]
		[Export ("initWithZoneID:")]
		NativeHandle Constructor (CKRecordZoneID zoneID);

		[Export ("initWithZoneID:subscriptionID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CKRecordZoneID zoneID, string subscriptionID);

		[Export ("zoneID", ArgumentSemantic.Copy)]
		// we need the setter since it was bound in the base type
		CKRecordZoneID ZoneID { get; [NotImplemented] set; }

		[NullAllowed, Export ("recordType")]
		string RecordType { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CKSubscription))]
	interface CKDatabaseSubscription : NSSecureCoding, NSCopying {

		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithSubscriptionID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string subscriptionID);

		[NullAllowed, Export ("recordType")]
		string RecordType { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // objc_exception_throw on [CKSubscription init]
	[BaseType (typeof (NSObject))]
	interface CKSubscription : NSSecureCoding, NSCopying {
		[Export ("subscriptionID")]
		string SubscriptionId { get; }

		[Export ("subscriptionType", ArgumentSemantic.UnsafeUnretained)]
		CKSubscriptionType SubscriptionType { get; }

		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKQuerySubscription'.")]
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'CKQuerySubscription'.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKQuerySubscription'.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKQuerySubscription'.")]
		[NullAllowed]
		[Export ("recordType")]
		string RecordType { get; }

		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKQuerySubscription'.")]
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'CKQuerySubscription'.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKQuerySubscription'.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKQuerySubscription'.")]
		[NullAllowed]
		[Export ("predicate", ArgumentSemantic.Copy)]
		NSPredicate Predicate { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("notificationInfo", ArgumentSemantic.Copy)]
		CKNotificationInfo NotificationInfo { get; set; }

		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKRecordZoneSubscription'.")]
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'CKRecordZoneSubscription'.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKRecordZoneSubscription'.")]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKRecordZoneSubscription'.")]
		[NullAllowed]
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneID { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CKNotificationInfo : NSSecureCoding, NSCopying, NSCoding {

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("alertBody")]
		string AlertBody { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("alertLocalizationKey")]
		string AlertLocalizationKey { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("alertLocalizationArgs", ArgumentSemantic.Copy)]
		string [] AlertLocalizationArgs { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("alertActionLocalizationKey")]
		string AlertActionLocalizationKey { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("alertLaunchImage")]
		string AlertLaunchImage { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("soundName")]
		string SoundName { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("desiredKeys", ArgumentSemantic.Copy)]
		string [] DesiredKeys { get; set; }

		[Export ("shouldBadge", ArgumentSemantic.UnsafeUnretained)]
		bool ShouldBadge { get; set; }

		[Export ("shouldSendContentAvailable")]
		bool ShouldSendContentAvailable { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("category")]
		string Category { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("title")]
		string Title { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("titleLocalizationKey")]
		string TitleLocalizationKey { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("titleLocalizationArgs", ArgumentSemantic.Copy)]
		string [] TitleLocalizationArgs { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitle")]
		string Subtitle { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitleLocalizationKey")]
		string SubtitleLocalizationKey { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subtitleLocalizationArgs", ArgumentSemantic.Copy)]
		string [] SubtitleLocalizationArgs { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("shouldSendMutableContent")]
		bool ShouldSendMutableContent { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("collapseIDKey")]
		string CollapseIdKey { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // Name: CKException Reason: You can't call init on CKQueryCursor
	[BaseType (typeof (NSObject))]
	interface CKQueryCursor : NSCopying, NSSecureCoding {

	}

	delegate void CKFetchWebAuthTokenOperationHandler ([NullAllowed] string webAuthToken, [NullAllowed] NSError operationError);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchWebAuthTokenOperation {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithAPIToken:")]
		NativeHandle Constructor (string token);

		[NullAllowed]
		[Export ("APIToken")]
		string ApiToken { get; set; }

		[NullAllowed]
		[Export ("fetchWebAuthTokenCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchWebAuthTokenOperationHandler Completed { get; set; }
	}

	[Obsoleted (PlatformName.MacOSX, 14, 0)]
	[Obsoleted (PlatformName.iOS, 17, 0)]
	[Obsoleted (PlatformName.TvOS, 17, 0)]
	[Obsoleted (PlatformName.MacCatalyst, 17, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor] // designated
	interface CKDiscoverUserIdentitiesOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithUserIdentityLookupInfos:")]
		NativeHandle Constructor (CKUserIdentityLookupInfo [] userIdentityLookupInfos);

		[Export ("userIdentityLookupInfos", ArgumentSemantic.Copy)]
		CKUserIdentityLookupInfo [] UserIdentityLookupInfos { get; set; }

		[NullAllowed, Export ("userIdentityDiscoveredBlock", ArgumentSemantic.Copy)]
		Action<CKUserIdentity, CKUserIdentityLookupInfo> Discovered { get; set; }

		[NullAllowed, Export ("discoverUserIdentitiesCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> Completed { get; set; }
	}

	/// <summary>An operation that gets all of the discoverable users in the system's address book.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CloudKit/CKDiscoverAllUserIdentitiesOperation">Apple documentation for <c>CKDiscoverAllUserIdentitiesOperation</c></related>
	[Deprecated (PlatformName.MacOSX, 14, 0)]
	[Deprecated (PlatformName.iOS, 17, 0)]
	[Deprecated (PlatformName.MacCatalyst, 17, 0)]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor] // designated
	interface CKDiscoverAllUserIdentitiesOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[NullAllowed, Export ("userIdentityDiscoveredBlock", ArgumentSemantic.Copy)]
		Action<CKUserIdentity> Discovered { get; set; }

		[NullAllowed, Export ("discoverAllUserIdentitiesCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> Completed { get; set; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	delegate void CKFetchShareParticipantsOperationPerShareParticipantCompletionHandler ([NullAllowed] CKUserIdentityLookupInfo identityLookupInfo, [NullAllowed] CKShareParticipant participant, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchShareParticipantsOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithUserIdentityLookupInfos:")]
		NativeHandle Constructor (CKUserIdentityLookupInfo [] userIdentityLookupInfos);

		[NullAllowed]
		[Export ("userIdentityLookupInfos", ArgumentSemantic.Copy)]
		CKUserIdentityLookupInfo [] UserIdentityLookupInfos { get; set; }

		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'PerShareParticipantCompletionHandler' instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'PerShareParticipantCompletionHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'PerShareParticipantCompletionHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'PerShareParticipantCompletionHandler' instead.")]
		[NullAllowed, Export ("shareParticipantFetchedBlock", ArgumentSemantic.Copy)]
		Action<CKShareParticipant> Fetched { get; set; }

		[NullAllowed, Export ("fetchShareParticipantsCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> Completed { get; set; }

		[NullAllowed]
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("perShareParticipantCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchShareParticipantsOperationPerShareParticipantCompletionHandler PerShareParticipantCompletionBlock { get; set; }
	}

	[MacCatalyst (13, 1)]
	delegate void CKAcceptPerShareCompletionHandler (CKShareMetadata shareMetadata, [NullAllowed] CKShare acceptedShare, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor] // designated
	interface CKAcceptSharesOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithShareMetadatas:")]
		NativeHandle Constructor (CKShareMetadata [] shareMetadatas);

		[Export ("shareMetadatas", ArgumentSemantic.Copy)]
		[NullAllowed]
		CKShareMetadata [] ShareMetadatas { get; set; }

		[NullAllowed, Export ("perShareCompletionBlock", ArgumentSemantic.Copy)]
		CKAcceptPerShareCompletionHandler PerShareCompleted { get; set; }

		[NullAllowed, Export ("acceptSharesCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> AcceptSharesCompleted { get; set; }
	}

	[MacCatalyst (13, 1)]
	delegate void CKFetchPerShareMetadataHandler (NSUrl shareURL, [NullAllowed] CKShareMetadata shareMetadata, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchShareMetadataOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithShareURLs:")]
		NativeHandle Constructor (NSUrl [] shareUrls);

		[NullAllowed]
		[Export ("shareURLs", ArgumentSemantic.Copy)]
		NSUrl [] ShareUrls { get; set; }

		[Export ("shouldFetchRootRecord")]
		bool ShouldFetchRootRecord { get; set; }

		[NullAllowed, Export ("rootRecordDesiredKeys", ArgumentSemantic.Copy)]
		string [] RootRecordDesiredKeys { get; set; }

		[NullAllowed, Export ("perShareMetadataBlock", ArgumentSemantic.Copy)]
		CKFetchPerShareMetadataHandler PerShareMetadata { get; set; }

		[NullAllowed, Export ("fetchShareMetadataCompletionBlock", ArgumentSemantic.Copy)]
		Action<NSError> FetchShareMetadataCompleted { get; set; }
	}

	[MacCatalyst (13, 1)]
	delegate void CKFetchDatabaseChangesCompletionHandler ([NullAllowed] CKServerChangeToken serverChangeToken, bool moreComing, [NullAllowed] NSError operationError);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (CKDatabaseOperation))]
	[DisableDefaultCtor] // designated
	interface CKFetchDatabaseChangesOperation {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("initWithPreviousServerChangeToken:")]
		NativeHandle Constructor ([NullAllowed] CKServerChangeToken previousServerChangeToken);

		[NullAllowed, Export ("previousServerChangeToken", ArgumentSemantic.Copy)]
		CKServerChangeToken PreviousServerChangeToken { get; set; }

		// @property (assign, nonatomic) NSUInteger resultsLimit;
		[Export ("resultsLimit")]
		nuint ResultsLimit { get; set; }

		[Export ("fetchAllChanges")]
		bool FetchAllChanges { get; set; }

		[NullAllowed, Export ("recordZoneWithIDChangedBlock", ArgumentSemantic.Copy)]
		Action<CKRecordZoneID> Changed { get; set; }

		[NullAllowed, Export ("recordZoneWithIDWasDeletedBlock", ArgumentSemantic.Copy)]
		Action<CKRecordZoneID> WasDeleted { get; set; }

		[NullAllowed, Export ("changeTokenUpdatedBlock", ArgumentSemantic.Copy)]
		Action<CKServerChangeToken> ChangeTokenUpdated { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("recordZoneWithIDWasPurgedBlock", ArgumentSemantic.Copy)]
		Action<CKRecordZoneID> WasPurged { get; set; }

		[NullAllowed, Export ("fetchDatabaseChangesCompletionBlock", ArgumentSemantic.Copy)]
		CKFetchDatabaseChangesCompletionHandler ChangesCompleted { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("recordZoneWithIDWasDeletedDueToUserEncryptedDataResetBlock", ArgumentSemantic.Copy)]
		Action<CKRecordZoneID> RecordZoneWithIdWasDeletedDueToUserEncryptedDataReset { get; set; }
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKAllowedSharingOptions : NSSecureCoding, NSCopying {
		[Export ("initWithAllowedParticipantPermissionOptions:allowedParticipantAccessOptions:")]
		NativeHandle Constructor (CKSharingParticipantPermissionOption allowedParticipantPermissionOptions, CKSharingParticipantAccessOption allowedParticipantAccessOptions);

		[Export ("allowedParticipantPermissionOptions", ArgumentSemantic.Assign)]
		CKSharingParticipantPermissionOption AllowedParticipantPermissionOptions { get; set; }

		[Export ("allowedParticipantAccessOptions", ArgumentSemantic.Assign)]
		CKSharingParticipantAccessOption AllowedParticipantAccessOptions { get; set; }

		[Static]
		[Export ("standardOptions", ArgumentSemantic.Strong)]
		CKAllowedSharingOptions StandardOptions { get; }
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSystemSharingUIObserver {
		[Export ("initWithContainer:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CKContainer container);

		[NullAllowed, Export ("systemSharingUIDidSaveShareBlock", ArgumentSemantic.Copy)]
		Action<CKRecordID, CKShare, NSError> SystemSharingUIDidSaveShareHandler { get; set; }

		[NullAllowed, Export ("systemSharingUIDidStopSharingBlock", ArgumentSemantic.Copy)]
		Action<CKRecordID, NSError> SystemSharingUIDidStopSharingHandler { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface CKSyncEngineSendChangesScope : NSCopying {
		[NullAllowed, Export ("zoneIDs", ArgumentSemantic.Copy)]
		NSSet<CKRecordZoneID> ZoneIds { get; }

		[Export ("excludedZoneIDs", ArgumentSemantic.Copy)]
		NSSet<CKRecordZoneID> ExcludedZoneIds { get; }

		[NullAllowed, Export ("recordIDs", ArgumentSemantic.Copy)]
		NSSet<CKRecordID> RecordIds { get; }

		[Internal]
		[Export ("initWithZoneIDs:")]
		NativeHandle _InitWithZoneIds ([NullAllowed] NSSet<CKRecordZoneID> zoneIds);

		[Internal]
		[Export ("initWithExcludedZoneIDs:")]
		NativeHandle _InitWithExcludedZoneIds (NSSet<CKRecordZoneID> excludedZoneIds);

		[Export ("initWithRecordIDs:")]
		NativeHandle Constructor ([NullAllowed] NSSet<CKRecordID> recordIds);

		[Export ("containsRecordID:")]
		bool ContainsRecordId (CKRecordID recordId);

		[Export ("containsPendingRecordZoneChange:")]
		bool ContainsPendingRecordZoneChange (CKSyncEnginePendingRecordZoneChange pendingRecordZoneChange);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface CKSyncEngineFetchChangesScope : NSCopying {
		[NullAllowed, Export ("zoneIDs", ArgumentSemantic.Copy)]
		NSSet<CKRecordZoneID> ZoneIds { get; }

		[Export ("excludedZoneIDs", ArgumentSemantic.Copy)]
		NSSet<CKRecordZoneID> ExcludedZoneIds { get; }

		[Internal]
		[Export ("initWithZoneIDs:")]
		NativeHandle _InitWithZoneIds ([NullAllowed] NSSet<CKRecordZoneID> zoneIds);

		[Internal]
		[Export ("initWithExcludedZoneIDs:")]
		NativeHandle _InitWithExcludedZoneIds (NSSet<CKRecordZoneID> zoneIds);

		[TV (17, 2), MacCatalyst (17, 2), Mac (14, 2), iOS (17, 2)]
		[Export ("containsZoneID:")]
		bool Contains (CKRecordZoneID zoneId);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineFetchChangesContext {
		[Export ("reason", ArgumentSemantic.Assign)]
		CKSyncEngineSyncReason Reason { get; }

		[Export ("options", ArgumentSemantic.Copy)]
		CKSyncEngineFetchChangesOptions Options { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineEvent {
		[Export ("type", ArgumentSemantic.Assign)]
		CKSyncEngineEventType Type { get; }

		[Export ("stateUpdateEvent", ArgumentSemantic.Strong)]
		CKSyncEngineStateUpdateEvent StateUpdateEvent { get; }

		[Export ("accountChangeEvent", ArgumentSemantic.Strong)]
		CKSyncEngineAccountChangeEvent AccountChangeEvent { get; }

		[Export ("fetchedDatabaseChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineFetchedDatabaseChangesEvent FetchedDatabaseChangesEvent { get; }

		[Export ("fetchedRecordZoneChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineFetchedRecordZoneChangesEvent FetchedRecordZoneChangesEvent { get; }

		[Export ("sentDatabaseChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineSentDatabaseChangesEvent SentDatabaseChangesEvent { get; }

		[Export ("sentRecordZoneChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineSentRecordZoneChangesEvent SentRecordZoneChangesEvent { get; }

		[Export ("willFetchChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineWillFetchChangesEvent WillFetchChangesEvent { get; }

		[Export ("willFetchRecordZoneChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineWillFetchRecordZoneChangesEvent WillFetchRecordZoneChangesEvent { get; }

		[Export ("didFetchRecordZoneChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineDidFetchRecordZoneChangesEvent DidFetchRecordZoneChangesEvent { get; }

		[Export ("didFetchChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineDidFetchChangesEvent DidFetchChangesEvent { get; }

		[Export ("willSendChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineWillSendChangesEvent WillSendChangesEvent { get; }

		[Export ("didSendChangesEvent", ArgumentSemantic.Strong)]
		CKSyncEngineDidSendChangesEvent DidSendChangesEvent { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineAccountChangeEvent {
		[Export ("changeType", ArgumentSemantic.Assign)]
		CKSyncEngineAccountChangeType ChangeType { get; }

		[NullAllowed, Export ("previousUser", ArgumentSemantic.Copy)]
		CKRecordID PreviousUser { get; }

		[NullAllowed, Export ("currentUser", ArgumentSemantic.Copy)]
		CKRecordID CurrentUser { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineStateSerialization : NSSecureCoding { }

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineConfiguration {
		[Export ("initWithDatabase:stateSerialization:delegate:")]
		NativeHandle Constructor (CKDatabase database, [NullAllowed] CKSyncEngineStateSerialization stateSerialization, ICKSyncEngineDelegate @delegate);

		[Export ("database", ArgumentSemantic.Strong)]
		CKDatabase Database { get; set; }

		[NullAllowed, Export ("stateSerialization", ArgumentSemantic.Copy)]
		CKSyncEngineStateSerialization StateSerialization { get; set; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICKSyncEngineDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("automaticallySync")]
		bool AutomaticallySync { get; set; }

		[NullAllowed, Export ("subscriptionID")]
		string SubscriptionId { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineWillSendChangesEvent {
		[Export ("context", ArgumentSemantic.Strong)]
		CKSyncEngineSendChangesContext Context { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineWillFetchRecordZoneChangesEvent {
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneId { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineSendChangesContext {
		[Export ("reason", ArgumentSemantic.Assign)]
		CKSyncEngineSyncReason Reason { get; }

		[Export ("options", ArgumentSemantic.Copy)]
		CKSyncEngineSendChangesOptions Options { get; }
	}

	interface ICKSyncEngineDelegate { }

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CKSyncEngineDelegate {
		[Abstract]
		[Export ("syncEngine:handleEvent:")]
		void SyncEngine (CKSyncEngine syncEngine, CKSyncEngineEvent @event);

		[Abstract]
		[Export ("syncEngine:nextRecordZoneChangeBatchForContext:")]
		[return: NullAllowed]
		CKSyncEngineRecordZoneChangeBatch SyncEngine (CKSyncEngine syncEngine, CKSyncEngineSendChangesContext context);

		[Export ("syncEngine:nextFetchChangesOptionsForContext:")]
		CKSyncEngineFetchChangesOptions SyncEngine (CKSyncEngine syncEngine, CKSyncEngineFetchChangesContext context);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineDidSendChangesEvent {
		[Export ("context", ArgumentSemantic.Strong)]
		CKSyncEngineSendChangesContext Context { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEnginePendingRecordZoneChange {
		[Export ("initWithRecordID:type:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CKRecordID recordId, CKSyncEnginePendingRecordZoneChangeType type);

		[Export ("recordID", ArgumentSemantic.Copy)]
		CKRecordID RecordId { get; }

		[Export ("type", ArgumentSemantic.Assign)]
		CKSyncEnginePendingRecordZoneChangeType Type { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface CKSyncEngineFetchChangesOptions : NSCopying {
		[Export ("scope", ArgumentSemantic.Copy)]
		CKSyncEngineFetchChangesScope Scope { get; set; }

		[Export ("operationGroup", ArgumentSemantic.Strong)]
		CKOperationGroup OperationGroup { get; set; }

		[Export ("prioritizedZoneIDs", ArgumentSemantic.Copy)]
		CKRecordZoneID [] PrioritizedZoneIds { get; set; }

		[Export ("initWithScope:")]
		NativeHandle Constructor ([NullAllowed] CKSyncEngineFetchChangesScope scope);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineStateUpdateEvent {
		[Export ("stateSerialization", ArgumentSemantic.Copy)]
		CKSyncEngineStateSerialization StateSerialization { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineFetchedDatabaseChangesEvent {
		[Export ("modifications", ArgumentSemantic.Copy)]
		CKRecordZone [] Modifications { get; }

		[Export ("deletions", ArgumentSemantic.Copy)]
		CKSyncEngineFetchedZoneDeletion [] Deletions { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineFetchedRecordZoneChangesEvent {
		[Export ("modifications", ArgumentSemantic.Copy)]
		CKRecord [] Modifications { get; }

		[Export ("deletions", ArgumentSemantic.Copy)]
		CKSyncEngineFetchedRecordDeletion [] Deletions { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineSentDatabaseChangesEvent {
		[Export ("savedZones", ArgumentSemantic.Copy)]
		CKRecordZone [] SavedZones { get; }

		[Export ("failedZoneSaves", ArgumentSemantic.Copy)]
		CKSyncEngineFailedZoneSave [] FailedZoneSaves { get; }

		[Export ("deletedZoneIDs", ArgumentSemantic.Copy)]
		CKRecordZoneID [] DeletedZoneIds { get; }

		[Export ("failedZoneDeletes", ArgumentSemantic.Copy)]
		NSDictionary<CKRecordZoneID, NSError> FailedZoneDeletes { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineWillFetchChangesEvent {

		[TV (17, 2), MacCatalyst (17, 2), Mac (14, 2), iOS (17, 2)]
		[Export ("context", ArgumentSemantic.Strong)]
		CKSyncEngineFetchChangesContext Context { get; }

	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineSentRecordZoneChangesEvent {
		[Export ("savedRecords", ArgumentSemantic.Copy)]
		CKRecord [] SavedRecords { get; }

		[Export ("failedRecordSaves", ArgumentSemantic.Copy)]
		CKSyncEngineFailedRecordSave [] FailedRecordSaves { get; }

		[Export ("deletedRecordIDs", ArgumentSemantic.Copy)]
		CKRecordID [] DeletedRecordIds { get; }

		[Export ("failedRecordDeletes", ArgumentSemantic.Copy)]
		NSDictionary<CKRecordID, NSError> FailedRecordDeletes { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface CKSyncEngineSendChangesOptions : NSCopying {
		[Export ("scope", ArgumentSemantic.Copy)]
		CKSyncEngineSendChangesScope Scope { get; set; }

		[Export ("operationGroup", ArgumentSemantic.Strong)]
		CKOperationGroup OperationGroup { get; set; }

		[Export ("initWithScope:")]
		NativeHandle Constructor ([NullAllowed] CKSyncEngineSendChangesScope scope);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineFailedRecordSave {
		[Export ("record", ArgumentSemantic.Strong)]
		CKRecord Record { get; }

		[Export ("error", ArgumentSemantic.Strong)]
		NSError Error { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngine {
		[Export ("initWithConfiguration:")]
		NativeHandle Constructor (CKSyncEngineConfiguration configuration);

		[Export ("database", ArgumentSemantic.Strong)]
		CKDatabase Database { get; }

		[Export ("state", ArgumentSemantic.Strong)]
		CKSyncEngineState State { get; }

		[Async]
		[Export ("fetchChangesWithCompletionHandler:")]
		void FetchChanges ([NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("fetchChangesWithOptions:completionHandler:")]
		void FetchChanges (CKSyncEngineFetchChangesOptions options, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("sendChangesWithCompletionHandler:")]
		void SendChanges ([NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("sendChangesWithOptions:completionHandler:")]
		void SendChanges (CKSyncEngineSendChangesOptions options, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("cancelOperationsWithCompletionHandler:")]
		void CancelOperations ([NullAllowed] Action completionHandler);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineDidFetchChangesEvent {

		[TV (17, 2), MacCatalyst (17, 2), Mac (14, 2), iOS (17, 2)]
		[Export ("context", ArgumentSemantic.Strong)]
		CKSyncEngineFetchChangesContext Context { get; }

	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEngineEvent))]
	interface CKSyncEngineDidFetchRecordZoneChangesEvent {
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneId { get; }

		[NullAllowed, Export ("error", ArgumentSemantic.Copy)]
		NSError Error { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineRecordZoneChangeBatch {
		[Export ("initWithPendingChanges:recordProvider:")]
		NativeHandle Constructor (CKSyncEnginePendingRecordZoneChange [] pendingChanges, Func<CKRecordID, CKRecord> recordProvider);

		[Export ("initWithRecordsToSave:recordIDsToDelete:atomicByZone:")]
		NativeHandle Constructor ([NullAllowed] CKRecord [] recordsToSave, [NullAllowed] CKRecordID [] recordIdsToDelete, bool atomicByZone);

		[Export ("recordsToSave", ArgumentSemantic.Copy)]
		CKRecord [] RecordsToSave { get; }

		[Export ("recordIDsToDelete", ArgumentSemantic.Copy)]
		CKRecordID [] RecordIdsToDelete { get; }

		[Export ("atomicByZone")]
		bool AtomicByZone { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineFetchedZoneDeletion {
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneId { get; }

		[Export ("reason", ArgumentSemantic.Assign)]
		CKSyncEngineZoneDeletionReason Reason { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineFetchedRecordDeletion {
		[Export ("recordID", ArgumentSemantic.Copy)]
		CKRecordID RecordId { get; }

		[Export ("recordType")]
		string RecordType { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineFailedZoneSave {
		[Export ("recordZone", ArgumentSemantic.Strong)]
		CKRecordZone RecordZone { get; }

		[Export ("error", ArgumentSemantic.Strong)]
		NSError Error { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEngineState {
		[Export ("pendingRecordZoneChanges", ArgumentSemantic.Copy)]
		CKSyncEnginePendingRecordZoneChange [] PendingRecordZoneChanges { get; }

		[Export ("pendingDatabaseChanges", ArgumentSemantic.Copy)]
		CKSyncEnginePendingDatabaseChange [] PendingDatabaseChanges { get; }

		[Export ("hasPendingUntrackedChanges")]
		bool HasPendingUntrackedChanges { get; set; }

		[Export ("zoneIDsWithUnfetchedServerChanges", ArgumentSemantic.Copy)]
		CKRecordZoneID [] ZoneIdsWithUnfetchedServerChanges { get; }

		[Export ("addPendingRecordZoneChanges:")]
		void AddPendingRecordZoneChanges (CKSyncEnginePendingRecordZoneChange [] changes);

		[Export ("removePendingRecordZoneChanges:")]
		void RemovePendingRecordZoneChanges (CKSyncEnginePendingRecordZoneChange [] changes);

		[Export ("addPendingDatabaseChanges:")]
		void AddPendingDatabaseChanges (CKSyncEnginePendingDatabaseChange [] changes);

		[Export ("removePendingDatabaseChanges:")]
		void RemovePendingDatabaseChanges (CKSyncEnginePendingDatabaseChange [] changes);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CKSyncEnginePendingDatabaseChange {
		[Export ("zoneID", ArgumentSemantic.Copy)]
		CKRecordZoneID ZoneId { get; }

		[Export ("type", ArgumentSemantic.Assign)]
		CKSyncEnginePendingDatabaseChangeType Type { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEnginePendingDatabaseChange))]
	interface CKSyncEnginePendingZoneSave {
		[Export ("initWithZone:")]
		NativeHandle Constructor (CKRecordZone zone);

		[Export ("zone", ArgumentSemantic.Copy)]
		CKRecordZone Zone { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (CKSyncEnginePendingDatabaseChange))]
	interface CKSyncEnginePendingZoneDelete {
		[Export ("initWithZoneID:")]
		NativeHandle Constructor (CKRecordZoneID zoneId);
	}
}
