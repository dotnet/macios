// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreData;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreDataTrampolines {

	[Export<Property> ("nsBatchInsertRequestDictionaryHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSBatchInsertRequestDictionaryHandler NSBatchInsertRequestDictionaryHandler { get; set; }

	[Export<Property> ("nsBatchInsertRequestManagedObjectHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSBatchInsertRequestManagedObjectHandler NSBatchInsertRequestManagedObjectHandler { get; set; }

	[Export<Property> ("nsPersistentCloudKitContainerAcceptShareInvitationsHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSPersistentCloudKitContainerAcceptShareInvitationsHandler NSPersistentCloudKitContainerAcceptShareInvitationsHandler { get; set; }

	[Export<Property> ("nsPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosHandler NSPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosHandler { get; set; }

	[Export<Property> ("nsPersistentCloudKitContainerPersistUpdatedShareHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSPersistentCloudKitContainerPersistUpdatedShareHandler NSPersistentCloudKitContainerPersistUpdatedShareHandler { get; set; }

	[Export<Property> ("nsPersistentCloudKitContainerPurgeObjectsAndRecordsInZoneHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSPersistentCloudKitContainerPurgeObjectsAndRecordsInZoneHandler NSPersistentCloudKitContainerPurgeObjectsAndRecordsInZoneHandler { get; set; }

	[Export<Property> ("nsPersistentCloudKitContainerShareManagedObjectsHandler", ArgumentSemantic.Copy)]
	public partial CoreData.NSPersistentCloudKitContainerShareManagedObjectsHandler NSPersistentCloudKitContainerShareManagedObjectsHandler { get; set; }
}
