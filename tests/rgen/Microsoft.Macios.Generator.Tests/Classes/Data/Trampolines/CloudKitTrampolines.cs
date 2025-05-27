// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CloudKit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CloudKitTrampolines {

	[Export<Property> ("ckAcceptPerShareCompletionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKAcceptPerShareCompletionHandler CKAcceptPerShareCompletionHandler { get; set; }

	[Export<Property> ("ckDatabaseDeleteSubscriptionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKDatabaseDeleteSubscriptionHandler CKDatabaseDeleteSubscriptionHandler { get; set; }

	[Export<Property> ("ckFetchDatabaseChangesCompletionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchDatabaseChangesCompletionHandler CKFetchDatabaseChangesCompletionHandler { get; set; }

	[Export<Property> ("ckFetchPerShareMetadataHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchPerShareMetadataHandler CKFetchPerShareMetadataHandler { get; set; }

	[Export<Property> ("ckFetchRecordChangesHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordChangesHandler CKFetchRecordChangesHandler { get; set; }

	[Export<Property> ("ckFetchRecordZoneChangesFetchCompletedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordZoneChangesFetchCompletedHandler CKFetchRecordZoneChangesFetchCompletedHandler { get; set; }

	[Export<Property> ("ckFetchRecordZoneChangesRecordWasChangedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordZoneChangesRecordWasChangedHandler CKFetchRecordZoneChangesRecordWasChangedHandler { get; set; }

	[Export<Property> ("ckFetchRecordZoneChangesTokensUpdatedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordZoneChangesTokensUpdatedHandler CKFetchRecordZoneChangesTokensUpdatedHandler { get; set; }

	[Export<Property> ("ckFetchRecordZoneChangesWithIDWasDeletedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordZoneChangesWithIDWasDeletedHandler CKFetchRecordZoneChangesWithIDWasDeletedHandler { get; set; }

	[Export<Property> ("ckFetchRecordsCompletedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchRecordsCompletedHandler CKFetchRecordsCompletedHandler { get; set; }

	[Export<Property> ("ckFetchShareParticipantsOperationPerShareParticipantCompletionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchShareParticipantsOperationPerShareParticipantCompletionHandler CKFetchShareParticipantsOperationPerShareParticipantCompletionHandler { get; set; }

	[Export<Property> ("ckFetchSubscriptionsCompleteHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchSubscriptionsCompleteHandler CKFetchSubscriptionsCompleteHandler { get; set; }

	[Export<Property> ("ckFetchSubscriptionsPerSubscriptionCompletionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchSubscriptionsPerSubscriptionCompletionHandler CKFetchSubscriptionsPerSubscriptionCompletionHandler { get; set; }

	[Export<Property> ("ckFetchWebAuthTokenOperationHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKFetchWebAuthTokenOperationHandler CKFetchWebAuthTokenOperationHandler { get; set; }

	[Export<Property> ("ckModifyRecordZonesHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordZonesHandler CKModifyRecordZonesHandler { get; set; }

	[Export<Property> ("ckModifyRecordZonesPerRecordZoneDeleteHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordZonesPerRecordZoneDeleteHandler CKModifyRecordZonesPerRecordZoneDeleteHandler { get; set; }

	[Export<Property> ("ckModifyRecordZonesPerRecordZoneSaveHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordZonesPerRecordZoneSaveHandler CKModifyRecordZonesPerRecordZoneSaveHandler { get; set; }

	[Export<Property> ("ckModifyRecordsOperationHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordsOperationHandler CKModifyRecordsOperationHandler { get; set; }

	[Export<Property> ("ckModifyRecordsOperationPerRecordDeleteHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordsOperationPerRecordDeleteHandler CKModifyRecordsOperationPerRecordDeleteHandler { get; set; }

	[Export<Property> ("ckModifyRecordsOperationPerRecordSaveHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifyRecordsOperationPerRecordSaveHandler CKModifyRecordsOperationPerRecordSaveHandler { get; set; }

	[Export<Property> ("ckModifySubscriptionsHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifySubscriptionsHandler CKModifySubscriptionsHandler { get; set; }

	[Export<Property> ("ckModifySubscriptionsPerSubscriptionDeleteHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifySubscriptionsPerSubscriptionDeleteHandler CKModifySubscriptionsPerSubscriptionDeleteHandler { get; set; }

	[Export<Property> ("ckModifySubscriptionsPerSubscriptionSaveHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKModifySubscriptionsPerSubscriptionSaveHandler CKModifySubscriptionsPerSubscriptionSaveHandler { get; set; }

	[Export<Property> ("ckQueryOperationRecordMatchedHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKQueryOperationRecordMatchedHandler CKQueryOperationRecordMatchedHandler { get; set; }

	[Export<Property> ("ckRecordZoneCompleteHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKRecordZoneCompleteHandler CKRecordZoneCompleteHandler { get; set; }

	[Export<Property> ("ckRecordZonePerRecordZoneCompletionHandler", ArgumentSemantic.Copy)]
	public partial CloudKit.CKRecordZonePerRecordZoneCompletionHandler CKRecordZonePerRecordZoneCompletionHandler { get; set; }
}
