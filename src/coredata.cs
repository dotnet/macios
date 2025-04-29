// This file describes the API that the generator will produce
//
// Authors:
//   MonoMac community
//   Miguel de Icaza
//
// Copyright 2009, 2011, MonoMac community
// Copyright 2011, 2015 Xamarin Inc.
//
using System;
using Foundation;
using ObjCRuntime;
using CloudKit;
#if MONOMAC
using AppKit;
#endif
using CoreSpotlight;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CoreData {
	/// <summary>Contains Core Data error information.</summary>
	/// <remarks>To be added.</remarks>
	[StrongDictionary ("UserInfoKeys")]
	interface UserInfo {
		/// <summary>Gets or sets an array that contains the multiple erros that occurred, if multiple errors occurred.</summary>
		///         <value>An array of <see cref="T:Foundation.NSError" /> objects that contains the errors that occurred.</value>
		///         <remarks>To be added.</remarks>
		NSError [] DetailedErrors { get; set; }
		/// <summary>Gets or sets the object that failed validation.</summary>
		///         <value>The object that failed validation.</value>
		///         <remarks>To be added.</remarks>
		NSManagedObject ObjectForValidationError { get; set; }
		/// <summary>Gets or sets the key that identifies the validation error that occurred.</summary>
		///         <value>The key that identifies the validation error that occurred.</value>
		///         <remarks>To be added.</remarks>
		NSString KeyForValidationError { get; set; }
		/// <summary>Gets or sets the predicate that failed validation.</summary>
		///         <value>The <see cref="T:Foundation.NSPredicate" /> object for the predicate that failed.</value>
		///         <remarks>To be added.</remarks>
		NSPredicate PredicateForValidationError { get; set; }
		/// <summary>Gets or sets the value that failed validation.</summary>
		///         <value>The value that failed validation.</value>
		///         <remarks>To be added.</remarks>
		NSValue ValueForValidationError { get; set; }
		/// <summary>Gets or sets the array of merge conflicts.</summary>
		///         <value>An array of <see cref="T:CoreData.NSMergeConflict" /> objects.</value>
		///         <remarks>To be added.</remarks>
		NSMergeConflict [] PersistentStoreSaveConflicts { get; set; }
		/// <summary>Gets or sets an array that contains the stores that were affected by the error.</summary>
		///         <value>An array that contains the stores that were affected by the error.</value>
		///         <remarks>To be added.</remarks>
		NSPersistentStore [] AffectedStoresForError { get; set; }
	}

	/// <summary>Contains keys for error information that Core Data stores in a dictionary.</summary>
	/// <remarks>To be added.</remarks>
	[Static]
	interface UserInfoKeys {
		/// <summary>To be added.</summary>
		///         <value>A key that identifies an array of <see cref="T:Foundation.NSError" /> objects that contains the errors that occured.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.DetailedErrors" /> property.</para>
		///         </remarks>
		[Field ("NSDetailedErrorsKey")]
		NSString DetailedErrorsKey { get; }

		/// <summary>Gets the key that identifies the object that failed validation.</summary>
		///         <value>The key that identifies the object that failed validation.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.ObjectForValidationError" /> property.</para>
		///         </remarks>
		[Field ("NSValidationObjectErrorKey")]
		NSString ObjectForValidationErrorKey { get; }

		/// <summary>Gets the key that identifies the key that, in turn, identifies the validation error that occurred.</summary>
		///         <value>The key that identifies the key that, in turn, identifies the validation error that occurred.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.KeyForValidationError" /> property.</para>
		///         </remarks>
		[Field ("NSValidationKeyErrorKey")]
		NSString KeyForValidationErrorKey { get; }

		/// <summary>Gets the key that identifies the predicate that failed validation..</summary>
		///         <value>The key that identifies the <see cref="T:Foundation.NSPredicate" /> object for the predicate that failed..</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.PredicateForValidationError" /> property.</para>
		///         </remarks>
		[Field ("NSValidationPredicateErrorKey")]
		NSString PredicateForValidationErrorKey { get; }

		/// <summary>Gets the key that identifies the value that failed validation.</summary>
		///         <value>The key that identifies the value that failed validation.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.ValueForValidationError" /> property.</para>
		///         </remarks>
		[Field ("NSValidationValueErrorKey")]
		NSString ValueForValidationErrorKey { get; }

		/// <summary>Gets the key that identifies the array of merge conflicts..</summary>
		///         <value>A key that identifies an array of <see cref="T:CoreData.NSMergeConflict" /> objects.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfoKeys.PersistentStoreSaveConflictsKey" /> property.</para>
		///         </remarks>
		[Field ("NSPersistentStoreSaveConflictsErrorKey")]
		NSString PersistentStoreSaveConflictsKey { get; }

		/// <summary>Gets the key that identifies an array that contains the stores that were affected by the error.</summary>
		///         <value>A key that identifies an array of <see cref="T:CoreData.NSPersistentStore" /> objects that were affected by the error.</value>
		///         <remarks>
		///           <para>This key corresponds to the <see cref="P:CoreData.UserInfo.AffectedStoresForError" /> property.</para>
		///         </remarks>
		[Field ("NSAffectedStoresErrorKey")]
		NSString AffectedStoresForErrorKey { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("NSPersistentStoreStagedMigrationManagerOptionKey")]
		NSString StagedMigrationManagerOptionKey { get; }

		[TV (14, 0), iOS (14, 0), MacCatalyst (17, 0)]
		[Field ("NSPersistentStoreDeferredLightweightMigrationOptionKey")]
		NSString DeferredLightweightMigrationOptionKey { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("NSPersistentStoreModelVersionChecksumKey")]
		NSString ModelVersionChecksumKey { get; }
	}

	/// <summary>Enumerates valid keys for the user information dictionary used in <see cref="P:CoreData.NSPersistentStoreCoordinator.StoresWillChangeNotification" /> and <see cref="P:CoreData.NSPersistentStoreCoordinator.StoresDidChangeNotification" />.</summary>
	[NoTV]
	[Native] // NUInteger -> NSPersistentStoreCoordinator.h
	[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "Please see the release notes and Core Data documentation.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
	public enum NSPersistentStoreUbiquitousTransitionType : ulong {
		/// <summary>To be added.</summary>
		AccountAdded = 1,
		/// <summary>To be added.</summary>
		AccountRemoved,
		/// <summary>To be added.</summary>
		ContentRemoved,
		/// <summary>To be added.</summary>
		InitialImportCompleted,
	}

	/// <summary>Enumerates reasons that a managed object may need to reinitialize certain values when it awakes.</summary>
	/// <remarks>
	///       <para>The values in this enumeration are returned by the <see cref="M:CoreData.NSManagedObject.AwakeFromSnapshotEvents(CoreData.NSSnapshotEventType)" /> method.</para>
	///     </remarks>
	[Native]
	public enum NSSnapshotEventType : ulong {
		/// <summary>Indicates that an insertion was undone.</summary>
		UndoInsertion = 1 << 1,
		/// <summary>Indicates that a deletion was undone.</summary>
		UndoDeletion = 1 << 2,
		/// <summary>Indicates that an update was undone.</summary>
		UndoUpdate = 1 << 3,
		/// <summary>Indicates that a rollback occurred.</summary>
		Rollback = 1 << 4,
		/// <summary>Indicates that a managed object was refreshed.</summary>
		Refresh = 1 << 5,
		/// <summary>Indicates that a conflict was resolved while saving.</summary>
		MergePolicy = 1 << 6,
	}

	/// <summary>A base class for 'atomic stores,' which can be used to store custom file formats in Core Data.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSAtomicStore_class/index.html">Apple documentation for <c>NSAtomicStore</c></related>
	[BaseType (typeof (NSPersistentStore))]
	// Objective-C exception thrown.  Name: NSInternalInconsistencyException Reason: NSMappedObjectStore must be initialized with initWithPersistentStoreCoordinator:configurationName:URL:options
	[DisableDefaultCtor]
	interface NSAtomicStore {

		/// <param name="coordinator">To be added.</param>
		/// <param name="configurationName">To be added.</param>
		/// <param name="url">To be added.</param>
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		NativeHandle Constructor ([NullAllowed] NSPersistentStoreCoordinator coordinator, [NullAllowed] string configurationName, NSUrl url, [NullAllowed] NSDictionary options);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("load:")]
		bool Load (out NSError error);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("save:")]
		bool Save (out NSError error);

		/// <param name="managedObject">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newCacheNodeForManagedObject:")]
		NSAtomicStoreCacheNode NewCacheNodeForManagedObject (NSManagedObject managedObject);

		/// <param name="node">To be added.</param>
		///         <param name="managedObject">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("updateCacheNode:fromManagedObject:")]
		void UpdateCacheNode (NSAtomicStoreCacheNode node, NSManagedObject managedObject);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cacheNodes")]
#if XAMCORE_5_0
		NSSet<NSAtomicStoreCacheNode> CacheNodes { get; }
#else
		NSSet CacheNodes { get; }
#endif

		/// <param name="cacheNodes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addCacheNodes:")]

#if XAMCORE_5_0
		void AddCacheNodes (NSSet<NSAtomicStoreCacheNode> cacheNodes);
#else
		void AddCacheNodes (NSSet cacheNodes);
#endif

		/// <param name="cacheNodes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willRemoveCacheNodes:")]
#if XAMCORE_5_0
		void WillRemoveCacheNodes (NSSet<NSAtomicStoreCacheNode> cacheNodes);
#else
		void WillRemoveCacheNodes (NSSet cacheNodes);
#endif

		/// <param name="objectID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("cacheNodeForObjectID:")]
		[return: NullAllowed]
		NSAtomicStoreCacheNode CacheNodeForObjectID (NSManagedObjectID objectID);

		/// <param name="entity">To be added.</param>
		///         <param name="data">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("objectIDForEntity:referenceObject:")]
		NSManagedObjectID ObjectIDForEntity (NSEntityDescription entity, NSObject data);

		/// <param name="managedObject">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newReferenceObjectForManagedObject:")]
		NSAtomicStore NewReferenceObjectForManagedObject (NSManagedObject managedObject);

		/// <param name="objectID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("referenceObjectForObjectID:")]
		NSAtomicStore ReferenceObjectForObjectID (NSManagedObjectID objectID);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSFetchIndexElementDescription : NSCoding, NSCopying {
		/// <param name="property">To be added.</param>
		/// <param name="collationType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithProperty:collationType:")]
		NativeHandle Constructor (NSPropertyDescription property, NSFetchIndexElementType collationType);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("property", ArgumentSemantic.Retain)]
		NSPropertyDescription Property { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("propertyName", ArgumentSemantic.Retain)]
		string PropertyName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("collationType", ArgumentSemantic.Assign)]
		NSFetchIndexElementType CollationType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ascending")]
		bool IsAscending { [Bind ("isAscending")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("indexDescription", ArgumentSemantic.Assign)]
		NSFetchIndexDescription IndexDescription { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSFetchIndexDescription : NSCoding, NSCopying {
		/// <param name="name">To be added.</param>
		/// <param name="elements">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithName:elements:")]
		NativeHandle Constructor (string name, [NullAllowed] NSFetchIndexElementDescription [] elements);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elements", ArgumentSemantic.Copy)]
		NSFetchIndexElementDescription [] Elements { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("entity", ArgumentSemantic.Assign)]
		NSEntityDescription Entity { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("partialIndexPredicate", ArgumentSemantic.Copy)]
		NSPredicate PartialIndexPredicate { get; set; }
	}

	/// <summary>Represents a single record in a Core Data atomic store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSAtomicStoreCacheNode_class/index.html">Apple documentation for <c>NSAtomicStoreCacheNode</c></related>
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: NSAtomicStoreCacheNodes must be initialized using initWithObjectID:(NSManagedObjectID *)
	[DisableDefaultCtor]
	interface NSAtomicStoreCacheNode {

		/// <param name="moid">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithObjectID:")]
		NativeHandle Constructor (NSManagedObjectID moid);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("objectID", ArgumentSemantic.Strong)]
		NSManagedObjectID ObjectID { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("propertyCache", ArgumentSemantic.Retain)]
#if XAMCORE_5_0
		NSMutableDictionary<NSString, NSObject> PropertyCache { get; set; }
#else
		NSDictionary PropertyCache { get; set; }
#endif

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("valueForKey:")]
		[return: NullAllowed]
		NSAtomicStoreCacheNode ValueForKey (string key);

		/// <param name="value">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setValue:forKey:")]
		void SetValue ([NullAllowed] NSObject value, string key);

	}
	/// <summary>Describes an attribute of an <see cref="T:CoreData.NSEntityDescription" /> object.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSAttributeDescription_Class/index.html">Apple documentation for <c>NSAttributeDescription</c></related>
	[BaseType (typeof (NSPropertyDescription))]
	interface NSAttributeDescription {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributeType")]
		NSAttributeType AttributeType { get; set; }

		// Default property value is null but it cannot be set to that value
		// NSInternalInconsistencyException Reason: Can't set attributeValueClassName to nil for a non-transient attribute.
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributeValueClassName")]
		[NullAllowed]
		string AttributeValueClassName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("defaultValue", ArgumentSemantic.Retain)]
		NSObject DefaultValue { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("versionHash")]
		NSData VersionHash { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("valueTransformerName")]
		string ValueTransformerName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsExternalBinaryDataStorage")]
		bool AllowsExternalBinaryDataStorage { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("preservesValueInHistoryOnDeletion")]
		bool PreservesValueInHistoryOnDeletion { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("allowsCloudEncryption")]
		bool AllowsCloudEncryption { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSEntityDescription : NSCoding, NSCopying {

		/// <param name="entityName">To be added.</param>
		///         <param name="context">To be added.</param>
		///         <summary>Retrieves the entity with <paramref name="entityName" /> that resides in the specified managed object <paramref name="context" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("entityForName:inManagedObjectContext:")]
		[return: NullAllowed]
		NSEntityDescription EntityForName (string entityName, NSManagedObjectContext context);

		[Static, Export ("insertNewObjectForEntityForName:inManagedObjectContext:")]
#if !NET
		NSObject InsertNewObjectForEntityForName (string entityName, NSManagedObjectContext context);
#else
		NSManagedObject InsertNewObject (string entityName, NSManagedObjectContext context);
#endif

		/// <summary>Gets the managed object model for the receiver.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectModel")]
		NSManagedObjectModel ManagedObjectModel { get; }

		/// <summary>Gets or sets the name of the receiver's class.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectClassName")]
		[NullAllowed]
		string ManagedObjectClassName { get; set; }

		/// <summary>Gets or sets the a value that is used to resolve name conflicts from one model to another.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("renamingIdentifier")]
		string RenamingIdentifier { get; set; }

		/// <summary>Gets the receiver's entity name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("name")]
		string Name { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the receiver is an abstract entity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("abstract")]
		bool Abstract { [Bind ("isAbstract")] get; set; }

		/// <summary>Gets a dictionary of the subentities for this entity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subentitiesByName")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSEntityDescription> SubentitiesByName { get; }
#else
		NSDictionary SubentitiesByName { get; }
#endif

		/// <summary>Gets an array of the subentities for this entity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subentities", ArgumentSemantic.Retain)]
		NSEntityDescription [] Subentities { get; set; }

		/// <summary>Gets the superentity of this entity.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("superentity")]
		NSEntityDescription Superentity { get; }

		/// <summary>Gets a dictionary of the receiver's properties.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("propertiesByName")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSPropertyDescription> PropertiesByName { get; }
#else
		NSDictionary PropertiesByName { get; }
#endif

		/// <summary>Gets an array that contains the receiver's properties.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("properties", ArgumentSemantic.Retain)]
		NSPropertyDescription [] Properties { get; set; }

		/// <summary>Gets or sets the receiver's user information dictionary.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Retain)]
		NSDictionary UserInfo { get; set; }

		/// <summary>Gets the a dictionary whose keys are the receiver attribute names and whose values are the corresponding attribute values.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributesByName")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSAttributeDescription> AttributesByName { get; }
#else
		NSDictionary AttributesByName { get; }
#endif

		/// <summary>Gets a dictionary of the receiver's relationships.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("relationshipsByName")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSRelationshipDescription> RelationshipsByName { get; }
#else
		NSDictionary RelationshipsByName { get; }
#endif

		/// <param name="entity">To be added.</param>
		///         <summary>Gets a dictionary of the relationships that the receiver has with <paramref name="entity" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("relationshipsWithDestinationEntity:")]
		NSRelationshipDescription [] RelationshipsWithDestinationEntity (NSEntityDescription entity);

		/// <param name="entity">To be added.</param>
		///         <summary>Returns a Boolean value that tells whether the receiver is a subtype of <paramref name="entity" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("isKindOfEntity:")]
		bool IsKindOfEntity (NSEntityDescription entity);

		/// <summary>Gets the receiver's version hash.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("versionHash")]
		NSData VersionHash { get; }

		/// <summary>Gets the modifier for the receiver's version hash.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'NSEntityDescription.Indexes' instead.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("compoundIndexes", ArgumentSemantic.Retain)]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSEntityDescription.Indexes' instead.")]
		NSPropertyDescription [] CompoundIndexes { get; set; }

		/// <summary>Gets or sets the indices for the entity.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("indexes", ArgumentSemantic.Copy)]
		NSFetchIndexDescription [] Indexes { get; set; }

		// @property (strong) NSArray<NSArray<id __nonnull> * __nonnull> * __nonnull uniquenessConstraints __attribute__((availability(ios, introduced=9.0)));
		[MacCatalyst (13, 1)]
		[Internal, Export ("uniquenessConstraints", ArgumentSemantic.Strong)]
		NSArray _UniquenessConstraints { get; set; }

		/// <summary>Gets or sets the expression that will be used to calculate the core spotlight display name for the entity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("coreSpotlightDisplayNameExpression", ArgumentSemantic.Retain)]
		NSExpression CoreSpotlightDisplayNameExpression { get; set; }
	}

	/// <summary>Specifies the mapping between an in-memory object and its persistent representation.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSEntityMapping_class/index.html">Apple documentation for <c>NSEntityMapping</c></related>
	[BaseType (typeof (NSObject))]
	interface NSEntityMapping {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		[NullAllowed]
		string Name { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mappingType")]
		NSEntityMappingType MappingType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("sourceEntityName")]
		string SourceEntityName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("sourceEntityVersionHash", ArgumentSemantic.Copy)]
		NSData SourceEntityVersionHash { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("destinationEntityName", ArgumentSemantic.Copy)]
		string DestinationEntityName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("destinationEntityVersionHash", ArgumentSemantic.Copy)]
		NSData DestinationEntityVersionHash { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributeMappings", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSPropertyMapping [] AttributeMappings { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("relationshipMappings", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSPropertyMapping [] RelationshipMappings { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("sourceExpression", ArgumentSemantic.Retain)]
		NSExpression SourceExpression { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("userInfo", ArgumentSemantic.Retain)]
		NSDictionary UserInfo { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("entityMigrationPolicyClassName")]
		string EntityMigrationPolicyClassName { get; set; }
	}

	/// <summary>Customizes the migration process during entity mapping.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSEntityMigrationPolicy_class/index.html">Apple documentation for <c>NSEntityMigrationPolicy</c></related>
	[BaseType (typeof (NSObject))]
	interface NSEntityMigrationPolicy {

		/// <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("beginEntityMapping:manager:error:")]
		bool BeginEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="sInstance">To be added.</param>
		///         <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("createDestinationInstancesForSourceInstance:entityMapping:manager:error:")]
		bool CreateDestinationInstancesForSourceInstance (NSManagedObject sInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("endInstanceCreationForEntityMapping:manager:error:")]
		bool EndInstanceCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="dInstance">To be added.</param>
		///         <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("createRelationshipsForDestinationInstance:entityMapping:manager:error:")]
		bool CreateRelationshipsForDestinationInstance (NSManagedObject dInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("endRelationshipCreationForEntityMapping:manager:error:")]
		bool EndRelationshipCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("performCustomValidationForEntityMapping:manager:error:")]
		bool PerformCustomValidationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		/// <param name="mapping">To be added.</param>
		///         <param name="manager">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("endEntityMapping:manager:error:")]
		bool EndEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);
	}

	/// <summary>Descriptor for a fetch result column that does not appear in the source, such as a sum or a minimum of a column in the source.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSExpressionDescription">Apple documentation for <c>NSExpressionDescription</c></related>
	[BaseType (typeof (NSPropertyDescription))]
	interface NSExpressionDescription {

		/// <summary>Gets or sets the expression that describes the column to return.</summary>
		///         <value>
		///           <para>The expression that describes the column to return.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("expression", ArgumentSemantic.Strong)]
		NSExpression Expression { get; set; }

		/// <summary>Gets or sets the data type for the column to return.</summary>
		///         <value>The data type for the column to return.</value>
		///         <remarks>To be added.</remarks>
		[Export ("expressionResultType")]
		NSAttributeType ResultType { get; set; }
	}

	/// <summary>Holds "fetched properties," that allow the developer to specify related objects via a weak unidirectional relationship in a fetch request.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSFetchedPropertyDescription_Class/index.html">Apple documentation for <c>NSFetchedPropertyDescription</c></related>
	[BaseType (typeof (NSPropertyDescription))]
	interface NSFetchedPropertyDescription {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("fetchRequest", ArgumentSemantic.Retain)]
		NSFetchRequest FetchRequest { get; set; }
	}

	/// <summary>Represents an expression that fetches results in a managed object context.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSFetchRequestExpression">Apple documentation for <c>NSFetchRequestExpression</c></related>
	[DisableDefaultCtor]
	[BaseType (typeof (NSExpression))]
	interface NSFetchRequestExpression {

		[Internal]
		[DesignatedInitializer]
		[Export ("initWithExpressionType:")]
		NativeHandle Constructor (NSExpressionType type);

		/// <param name="fetch">The expression from which to create a new fetch request expresssion.</param>
		///         <param name="context">The context in which to create the fetch request expression.</param>
		///         <param name="countOnly">Whether to create a fetch request expression that counts the matches, rather than returning them.</param>
		///         <summary>Creates a new <see cref="T:CoreData.NSFetchRequestExpression" /> from the specified expression and context.</summary>
		///         <returns>A new <see cref="T:CoreData.NSFetchRequestExpression" /> that was created from the specified expression and context.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("expressionForFetch:context:countOnly:")]
		NSFetchRequestExpression FromFetch (NSExpression fetch, NSExpression context, bool countOnly);

		/// <summary>Gets the expression that represents the request.</summary>
		///         <value>The expression that represents the request.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requestExpression")]
		NSExpression Request { get; }

		/// <summary>Gets the managed object context for the receiver.</summary>
		///         <value>The managed object context for the receiver.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contextExpression")]
		NSExpression Context { get; }

		/// <summary>Gets a <see langword="bool" /> that tells whether the fetch request expression counts the matching data, rather than returning them.</summary>
		///         <value>
		///           <see langword="true" /> if the fetch request expression counts the matching data, rather than returning them. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("countOnlyRequest")]
		bool IsCountOnly { [Bind ("isCountOnlyRequest")] get; }
	}

	interface INSFetchRequestResult { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface NSFetchRequestResult { }

	/// <summary>Holds search criteria used to retrieve data from a persistent store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSFetchRequest_Class/index.html">Apple documentation for <c>NSFetchRequest</c></related>
	[DisableDefaultCtor] // designated
	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSFetchRequest : NSCoding {

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		/// <remarks>
		///         </remarks>
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("entity", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSEntityDescription Entity { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("predicate", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSPredicate Predicate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("sortDescriptors", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSSortDescriptor [] SortDescriptors { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchLimit")]
		nuint FetchLimit { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchOffset")]
		nuint FetchOffset { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("affectedStores", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSPersistentStore [] AffectedStores { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType")]
		NSFetchRequestResultType ResultType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("returnsDistinctResults")]
		bool ReturnsDistinctResults { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includesSubentities")]
		bool IncludesSubentities { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includesPropertyValues")]
		bool IncludesPropertyValues { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includesPendingChanges")]
		bool IncludesPendingChanges { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("returnsObjectsAsFaults")]
		bool ReturnsObjectsAsFaults { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("relationshipKeyPathsForPrefetching")]
		[NullAllowed]
		string [] RelationshipKeyPathsForPrefetching { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("propertiesToFetch", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSPropertyDescription [] PropertiesToFetch { get; set; }

		/// <param name="entityName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("fetchRequestWithEntityName:")]
		// note: Xcode 6.3 changed the return value type from `NSFetchRequest*` to `instancetype`
		NSFetchRequest FromEntityName (string entityName);

		/// <param name="entityName">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithEntityName:")]
		NativeHandle Constructor (string entityName);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("entityName", ArgumentSemantic.Strong)]
		string EntityName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchBatchSize")]
		nint FetchBatchSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldRefreshRefetchedObjects")]
		bool ShouldRefreshRefetchedObjects { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("havingPredicate", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSPredicate HavingPredicate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("propertiesToGroupBy", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSPropertyDescription [] PropertiesToGroupBy { get; set; }

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("execute:")]
		[return: NullAllowed]
		INSFetchRequestResult [] Execute (out NSError error);
	}

	/// <summary>Controller object for Core Data fetch requests; generally used to provide data for a <see cref="T:UIKit.UITableView" />.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSFetchedResultsController_Class/index.html">Apple documentation for <c>NSFetchedResultsController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Delegates = new string [] { "WeakDelegate" })]
	interface NSFetchedResultsController {

		/// <param name="fetchRequest">To be added.</param>
		/// <param name="context">To be added.</param>
		/// <param name="sectionNameKeyPath">
		///           <para>the key path to the section name</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="name">
		///           <para>The cache name.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="T:CoreData.NSFetchedResultsController" /> from the specified values.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFetchRequest:managedObjectContext:sectionNameKeyPath:cacheName:")]
		NativeHandle Constructor (NSFetchRequest fetchRequest, NSManagedObjectContext context, [NullAllowed] string sectionNameKeyPath, [NullAllowed] string name);

		/// <summary>An instance of the CoreData.INSFetchedResultsControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreData.INSFetchedResultsControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		INSFetchedResultsControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>Gets the filename where section information is cached.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("cacheName")]
		string CacheName { get; }

		/// <summary>Gets an array that contains the fetched objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchedObjects")]
		[NullAllowed]
		NSObject [] FetchedObjects { get; }

		/// <summary>Gets the request for the fetch for which this object contains the results.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; }

		/// <summary>Returns the <see cref="T:CoreData.NSManagedObjectContext" /> that is used for fetching.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectContext")]
		NSManagedObjectContext ManagedObjectContext { get; }

		/// <summary>Returns the key path to the section name on fetched objects.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sectionNameKeyPath")]
		string SectionNameKeyPath { get; }

		/// <summary>Gets the sections of the fetch results.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sections")]
		INSFetchedResultsSectionInfo [] Sections { get; }

		/// <param name="error">To be added.</param>
		///         <summary>Performs the receiver's fetch request and stores any errors that occur in the specified <see langword="out" /> parameter.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("performFetch:")]
		bool PerformFetch (out NSError error);

		/// <param name="obj">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreData.NSFetchedResultsController" /> from the specified object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("indexPathForObject:")]
		[return: NullAllowed]
		NSIndexPath FromObject (NSObject obj);

		/// <param name="path">To be added.</param>
		///         <summary>Returns the obect that is located at the specified index <paramref name="path" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("objectAtIndexPath:")]
		NSObject ObjectAt (NSIndexPath path);

		/// <param name="title">To be added.</param>
		/// <param name="atIndex">To be added.</param>
		/// <summary>Returns the section number for the specified <paramref name="title" /> and index.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("sectionForSectionIndexTitle:atIndex:")]
		// name like UITableViewSource's similar (and linked) selector
		nint SectionFor (string title, nint atIndex);

#if !NET
		// badly named and conflict with the property
		[Export ("sectionIndexTitleForSectionName:")]
		[return: NullAllowed]
		string SectionIndexTitles (string sectionName);

		// expose a method as the property name is taken
		[Export ("sectionIndexTitles")]
		string [] GetSectionIndexTitles ();
#else
		/// <param name="sectionName">To be added.</param>
		/// <summary>Returns the section index titles for the specified section name.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("sectionIndexTitleForSectionName:")]
		[return: NullAllowed]
		string GetSectionIndexTitle (string sectionName);

		/// <summary>Returns an array that contains the section index titles.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("sectionIndexTitles")]
		string [] SectionIndexTitles { get; }
#endif

		/// <param name="name">
		///           <para>Name of the cache to delete.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Deletes the cache that resides that ahs the specified name.</summary>
		///         <remarks>Developers should note that passing <see langword="null" /> to this method deletes all caches.</remarks>
		[Static]
		[Export ("deleteCacheWithName:")]
		void DeleteCache ([NullAllowed] string name);
	}

	interface INSFetchedResultsControllerDelegate { }

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:CoreData.NSFetchedResultsControllerDelegate" />.</summary>
	/// <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:CoreData.NSFetchedResultsControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:CoreData.NSFetchedResultsControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:CoreData.NSFetchedResultsControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface NSFetchedResultsControllerDelegate {
		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("controllerWillChangeContent:")]
		void WillChangeContent (NSFetchedResultsController controller);

		/// <param name="controller">To be added.</param>
		///         <param name="anObject">To be added.</param>
		///         <param name="indexPath">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="type">To be added.</param>
		///         <param name="newIndexPath">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("controller:didChangeObject:atIndexPath:forChangeType:newIndexPath:")]
		void DidChangeObject (NSFetchedResultsController controller, NSObject anObject, [NullAllowed] NSIndexPath indexPath, NSFetchedResultsChangeType type, [NullAllowed] NSIndexPath newIndexPath);

		/// <param name="controller">To be added.</param>
		/// <param name="sectionInfo">To be added.</param>
		/// <param name="sectionIndex">To be added.</param>
		/// <param name="type">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("controller:didChangeSection:atIndex:forChangeType:")]
		void DidChangeSection (NSFetchedResultsController controller, INSFetchedResultsSectionInfo sectionInfo, nuint sectionIndex, NSFetchedResultsChangeType type);

		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("controllerDidChangeContent:")]
		void DidChangeContent (NSFetchedResultsController controller);

		/// <param name="controller">To be added.</param>
		///         <param name="sectionName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("controller:sectionIndexTitleForSectionName:")]
		[return: NullAllowed]
		string SectionFor (NSFetchedResultsController controller, string sectionName);
	}

	/// <include file="../docs/api/CoreData/INSFetchedResultsSectionInfo.xml" path="/Documentation/Docs[@DocId='T:CoreData.INSFetchedResultsSectionInfo']/*" />
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface NSFetchedResultsSectionInfo {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfObjects")]
		[Abstract]
		nint Count { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("objects")]
		[Abstract]
		NSObject [] Objects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		[Abstract]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("indexTitle")]
		[Abstract]
		string IndexTitle { get; }
	}

	interface INSFetchedResultsSectionInfo { }

	// 	NSInvalidArgumentException *** -loadMetadata: cannot be sent to an abstract object of class NSIncrementalStore: Create a concrete instance!
	//	Apple doc quote: "NSIncrementalStore is an abstract superclass..."
#if NET
	// Making a class abstract has problems: https://github.com/xamarin/xamarin-macios/issues/4969, so we're not doing this yet
	// [Abstract] // Abstract superclass.
#endif
	/// <summary>Supports the use of persistent stores that are loaded and saved incrementally, allowing for larger and shared datasets.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSIncrementalStore_Class/index.html">Apple documentation for <c>NSIncrementalStore</c></related>
	[BaseType (typeof (NSPersistentStore))]
	interface NSIncrementalStore {
#if NET
		/// <param name="root">To be added.</param>
		/// <param name="name">To be added.</param>
		/// <param name="url">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <summary>Creates a new <see cref="T:CoreData.NSIncrementalStore" /> with the specified values.</summary>
		/// <remarks>To be added.</remarks>
		[Protected]
#endif
		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		NativeHandle Constructor (NSPersistentStoreCoordinator root, string name, NSUrl url, NSDictionary options);

		/// <param name="error">To be added.</param>
		///         <summary>Loads the store metadata and reports any errors in <paramref name="error" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("loadMetadata:")]
		bool LoadMetadata (out NSError error);

		/// <param name="request">To be added.</param>
		///         <param name="context">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>Runs the specified <paramref name="request" /> in the specified <paramref name="context" />, and reports any errors in <paramref name="error" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("executeRequest:withContext:error:")]
		[return: NullAllowed]
		NSObject ExecuteRequest (NSPersistentStoreRequest request, [NullAllowed] NSManagedObjectContext context, out NSError error);

		/// <param name="forObjectId">To be added.</param>
		///         <param name="context">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Returns a <see cref="T:CoreData.NSIncrementalStoreNode" /> for the persisten external values of the identified object and reports any errors in <paramref name="error" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newValuesForObjectWithID:withContext:error:")]
		[return: NullAllowed]
		NSIncrementalStoreNode NewValues (NSManagedObjectID forObjectId, NSManagedObjectContext context, out NSError error);

		/// <param name="forRelationship">To be added.</param>
		///         <param name="forObjectI">To be added.</param>
		///         <param name="context">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>Returns a new relationship for the specified relationship data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newValueForRelationship:forObjectWithID:withContext:error:")]
		[return: NullAllowed]
		NSObject NewValue (NSRelationshipDescription forRelationship, NSManagedObjectID forObjectI, [NullAllowed] NSManagedObjectContext context, out NSError error);

		/// <param name="storeUrl">To be added.</param>
		/// <summary>Returns the identifier for the store at <paramref name="storeUrl" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static]
		[Export ("identifierForNewStoreAtURL:")]
#if NET
		NSObject GetIdentifierForNewStore (NSUrl storeUrl);
#else
		NSObject IdentifierForNewStoreAtURL (NSUrl storeUrl);
#endif

		/// <param name="array">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Returns an array of permanent identfiers for the provides newly-inserted objects and reports any errors in <paramref name="error" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("obtainPermanentIDsForObjects:error:")]
		[return: NullAllowed]
		NSObject [] ObtainPermanentIds (NSObject [] array, out NSError error);

		/// <param name="objectIds">To be added.</param>
		///         <summary>Method that is called when the <paramref name="objectIds" /> are registered for use.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectContextDidRegisterObjectsWithIDs:")]
		void ManagedObjectContextDidRegisterObjectsWithIds (NSObject [] objectIds);

		/// <param name="objectIds">To be added.</param>
		///         <summary>Method that is called when the <paramref name="objectIds" /> are unregistered for use.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectContextDidUnregisterObjectsWithIDs:")]
		void ManagedObjectContextDidUnregisterObjectsWithIds (NSObject [] objectIds);

		/// <param name="forEntity">To be added.</param>
		///         <param name="referenceObject">To be added.</param>
		///         <summary>Developers should not override this method. Returns a new ID for the entity and description.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newObjectIDForEntity:referenceObject:")]
		NSManagedObjectID NewObjectIdFor (NSEntityDescription forEntity, NSObject referenceObject);

		/// <param name="objectId">To be added.</param>
		///         <summary>Developers should not override this method. Returns a reference object for <paramref name="objectId" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("referenceObjectForObjectID:")]
		NSObject ReferenceObjectForObject (NSManagedObjectID objectId);

	}

	/// <summary>A concrete class that represents basic nodes in a <see cref="T:CoreData.NSIncrementalStore" />.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSIncrementalStoreNode_Class/index.html">Apple documentation for <c>NSIncrementalStoreNode</c></related>
	[BaseType (typeof (NSObject))]
	interface NSIncrementalStoreNode {
		/// <param name="objectId">To be added.</param>
		/// <param name="values">To be added.</param>
		/// <param name="version">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithObjectID:withValues:version:")]
#if XAMCORE_5_0
		NativeHandle Constructor (NSManagedObjectID objectId, NSDictionary<NSString, NSObject> values, ulong version);
#else
		NativeHandle Constructor (NSManagedObjectID objectId, NSDictionary values, ulong version);
#endif

		/// <param name="values">To be added.</param>
		///         <param name="version">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("updateWithValues:version:")]
#if XAMCORE_5_0
		void Update (NSDictionary<NSString, NSObject> values, ulong version);
#else
		void Update (NSDictionary values, ulong version);
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("objectID")]
		NSManagedObjectID ObjectId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("version")]
		long Version { get; }

		/// <param name="prop">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("valueForPropertyDescription:")]
		[return: NullAllowed]
		NSObject ValueForPropertyDescription (NSPropertyDescription prop);
	}

	/// <summary>A class that can be managed by a managed object context. Should have a correspondence to domain model classes, even if they are not direct subtypes.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSManagedObject_Class/index.html">Apple documentation for <c>NSManagedObject</c></related>
	[BaseType (typeof (NSObject))]
	// 'init' issues a warning: CoreData: error: Failed to call designated initializer on NSManagedObject class 'NSManagedObject' 
	// then crash while disposing the instance
	[DisableDefaultCtor]
	interface NSManagedObject : NSFetchRequestResult {
		/// <param name="entity">To be added.</param>
		/// <param name="context">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="T:CoreData.NSManagedObject" /> from an entity description and inserts the object into the specified managed object context.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithEntity:insertIntoManagedObjectContext:")]
		NativeHandle Constructor (NSEntityDescription entity, [NullAllowed] NSManagedObjectContext context);

		/// <param name="moc">To be added.</param>
		/// <summary>Creates a new <see cref="T:CoreData.NSManagedObject" /> in the specified managed object context.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("initWithContext:")]
		NativeHandle Constructor (NSManagedObjectContext moc);

		/// <summary>Gets the entity description for the receiver.</summary>
		///         <returns>The entity description for the receiver.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("entity")]
		NSEntityDescription GetEntityDescription ();

		/// <summary>Creates and returns a fetch request.</summary>
		///         <returns>A fetch request.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("fetchRequest")]
		NSFetchRequest CreateFetchRequest ();

		/// <summary>Gets the managed object context where this managed object is registered.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("managedObjectContext")]
		NSManagedObjectContext ManagedObjectContext { get; }

		/// <summary>Gets the receiver's entity description.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entity", ArgumentSemantic.Strong)]
		NSEntityDescription Entity { get; }

		/// <summary>Gets the object identifer.</summary>
		///         <value>To be added.</value>
		///         <remarks>This value changes when an unsaved object is saved.</remarks>
		[Export ("objectID", ArgumentSemantic.Strong)]
		NSManagedObjectID ObjectID { get; }

		/// <summary>Gets a Boolean value that tells whether the managed object should ignore changes to unmodeled properties.</summary>
		///         <value>A Boolean value that tells whether the managed object should ignore changes to unmodeled properties.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("contextShouldIgnoreUnmodeledPropertyChanges")]
		bool ContextShouldIgnoreUnModeledPropertyChanges { get; }

		/// <summary>Gets a Boolean value that tells whether the receiver was inserted into a managed object context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inserted")]
		bool IsInserted { [Bind ("isInserted")] get; }

		/// <summary>Gets a Boolean value that tells whether the receiver has changes that are not yet saved.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("updated")]
		bool IsUpdated { [Bind ("isUpdated")] get; }

		/// <summary>Gets a Boolean value that tells whether the receiver will be deleted in the next save.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("deleted")]
		bool IsDeleted { [Bind ("isDeleted")] get; }

		/// <summary>Gets a Boolean value that tells whether the receiver has been converted to a fault.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fault")]
		bool IsFault { [Bind ("isFault")] get; }

		/// <summary>Gets the receiver's faulting state.</summary>
		///         <value>The receiver's faulting state.</value>
		///         <remarks>To be added.</remarks>
		[Export ("faultingState")]
		nuint FaultingState { get; }

		/// <param name="key">To be added.</param>
		///         <summary>Gets a Boolean value that tells whether the receiver has a fault.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("hasFaultForRelationshipNamed:")]
		bool HasFaultForRelationshipNamed (string key);

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Method that is called before the value for the property that is identified by <paramref name="key" /> is accessed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willAccessValueForKey:")]
		void WillAccessValueForKey ([NullAllowed] string key);

		/// <param name="key">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Method that is called when the value for the property that is identified by <paramref name="key" /> is accessed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didAccessValueForKey:")]
		void DidAccessValueForKey ([NullAllowed] string key);

		/// <param name="key">To be added.</param>
		///         <summary>Method that is called before the value for the property that is identified by <paramref name="key" /> is changed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willChangeValueForKey:")]
		void WillChangeValueForKey (string key);

		/// <param name="key">To be added.</param>
		///         <summary>Method that is called when the value for the property that is identified by <paramref name="key" /> is changed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didChangeValueForKey:")]
		void DidChangeValueForKey (string key);

		/// <param name="inKey">To be added.</param>
		///         <param name="inMutationKind">To be added.</param>
		///         <param name="inObjects">To be added.</param>
		///         <summary>Method that is called before the value for the many-to-many property that is identified by <paramref name="inKey" /> is changed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willChangeValueForKey:withSetMutation:usingObjects:")]
		void WillChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		/// <param name="inKey">To be added.</param>
		///         <param name="inMutationKind">To be added.</param>
		///         <param name="inObjects">To be added.</param>
		///         <summary>Method that is called when the value for the many-to-many property that is identified by <paramref name="inKey" /> is changed.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didChangeValueForKey:withSetMutation:usingObjects:")]
		void DidChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		/// <summary>Gets the KVO information for the object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("observationInfo")]
		IntPtr ObservationInfo { get; set; }

		/// <summary>Method that is called after the receiver is fetched.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("awakeFromFetch")]
		void AwakeFromFetch ();

		/// <summary>Method that is called after the managed object is inserted into a managed object context.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("awakeFromInsert")]
		void AwakeFromInsert ();

		/// <param name="flags">To be added.</param>
		///         <summary>Method that is called to awaken the receiver when a property state change occurs.</summary>
		///         <remarks>
		///           <para>See the <see cref="T:CoreData.NSSnapshotEventType" /> enumeration for a list of the values that can be returned by this method.</para>
		///         </remarks>
		[Export ("awakeFromSnapshotEvents:")]
		void AwakeFromSnapshotEvents (NSSnapshotEventType flags);

		/// <summary>Method that is called before the receiver is saved.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willSave")]
		void WillSave ();

		/// <summary>Method that is called after the managed context operation saves values.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didSave")]
		void DidSave ();

		/// <summary>Method that is called before the receiver is turned into a fault.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willTurnIntoFault")]
		void WillTurnIntoFault ();

		/// <summary>Method that is called when the receiver becomes a fault.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didTurnIntoFault")]
		void DidTurnIntoFault ();

		/// <param name="key">To be added.</param>
		/// <summary>Returns the value for the property that is identified by the specified <paramref name="key" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("valueForKey:")]
		[return: NullAllowed]
#if NET
		NSObject GetValue (string key);
#else
		IntPtr ValueForKey (string key);
#endif

		/// <param name="value">To be added.</param>
		/// <param name="key">To be added.</param>
		/// <summary>Sets the receiver's value for the property that is specified by the provided <paramref name="key" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setValue:forKey:")]
#if NET
		void SetValue ([NullAllowed] NSObject value, string key);
#else
		void SetValue (IntPtr value, string key);
#endif

		/// <param name="key">To be added.</param>
		/// <summary>Returns the receiver's internal primitive value for the property that is specified by the provided <paramref name="key" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("primitiveValueForKey:")]
		[return: NullAllowed]
#if NET
		NSObject GetPrimitiveValue (string key);
#else
		IntPtr PrimitiveValueForKey (string key);
#endif

		/// <param name="value">To be added.</param>
		/// <param name="key">To be added.</param>
		/// <summary>Sets the receiver's internal primitive value for the property that is specified by the provided <paramref name="key" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setPrimitiveValue:forKey:")]
#if NET
		void SetPrimitiveValue ([NullAllowed] NSObject value, string key);
#else
		void SetPrimitiveValue (IntPtr value, string key);
#endif

		/// <param name="keys">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Returns a dictionary that contains property values for the specified <paramref name="keys" /> from before the last fetch or save.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("committedValuesForKeys:")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> GetCommittedValues ([NullAllowed] string[] keys);
#elif NET
		NSDictionary GetCommittedValues ([NullAllowed] string [] keys);
#else
		NSDictionary CommittedValuesForKeys ([NullAllowed] string [] keys);
#endif

		/// <summary>Gets a dictionary of the old values of persistent values that were recently changed since the receiver was last fetched.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changedValues")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> ChangedValues { get; }
#else
		NSDictionary ChangedValues { get; }
#endif

		/// <param name="value">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the specified <paramref name="value" /> is valid for the property that is identified by the specified <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("validateValue:forKey:error:")]
		bool ValidateValue (ref NSObject value, string key, out NSError error);

		/// <param name="error">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the receiver is valid for deletion.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("validateForDelete:")]
		bool ValidateForDelete (out NSError error);

		/// <param name="error">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the receiver is valid for insertion.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("validateForInsert:")]
		bool ValidateForInsert (out NSError error);

		/// <param name="error">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the receiver is valid for updating.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("validateForUpdate:")]
		bool ValidateForUpdate (out NSError error);

		/// <summary>Gets a Boolean value that tells whether the receiver has changes due to having been inserted or deleted, or due to having other unsaved changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasChanges")]
		bool HasChanges { get; }

		/// <summary>Returns a dictionary of the old values of persistent values that were recently changed since the context objects last changed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changedValuesForCurrentEvent")]
		NSDictionary ChangedValuesForCurrentEvent { get; }

		/// <summary>Method that is called prior to the object being deleted.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("prepareForDeletion")]
		void PrepareForDeletion ();

		// headers say this is introduced in 7.0,10.9 but Xcode 7 API diff
		// indicates it's new in 9.0,10.11... going by the header value...
		/// <summary>Gets a Boolean value that tells whether the receiver has unsaved changes to persistent values.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("hasPersistentChangedValues")]
		bool HasPersistentChangedValues { get; }

		/// <param name="relationshipName">To be added.</param>
		///         <summary>Gets the identifiers for all of the objects that are involved in the specified relationship.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("objectIDsForRelationshipNamed:")]
		NSManagedObjectID [] GetObjectIDs (string relationshipName);
	}

	/// <summary>Controls whether and how a managed object context pins itself to database transactions.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSQueryGenerationToken">Apple documentation for <c>NSQueryGenerationToken</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSQueryGenerationToken : NSSecureCoding, NSCopying {
		/// <summary>Gets the current query generation token.</summary>
		///         <value>The current query generation token.</value>
		///         <remarks>To be added</remarks>
		[Static, Export ("currentQueryGenerationToken", ArgumentSemantic.Strong)]
		NSQueryGenerationToken CurrentToken { get; }
	}

	/// <summary>A collection of related managed objects that create aninternally-consistent view of one or more persistent stores.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSManagedObjectContext_Class/index.html">Apple documentation for <c>NSManagedObjectContext</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSManagedObjectContext : NSCoding
#if !TVOS
	, NSLocking
#endif
#if MONOMAC
	, NSEditor, NSEditorRegistration
#endif
	{
		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		/// <remarks>
		///         </remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'NSManagedObjectContext (NSManagedObjectContextConcurrencyType)' instead.")]
		[Deprecated (PlatformName.TvOS, 9, 0, message: "Use 'NSManagedObjectContext (NSManagedObjectContextConcurrencyType)' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'NSManagedObjectContext (NSManagedObjectContextConcurrencyType)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSManagedObjectContext (NSManagedObjectContextConcurrencyType)' instead.")]
		[Export ("init")]
		NativeHandle Constructor ();

		/// <summary>Gets the receiver's store coordinator.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("persistentStoreCoordinator", ArgumentSemantic.Retain)]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; set; }

		/// <summary>Gets the receiver's undo manager.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("undoManager", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSUndoManager UndoManager { get; set; }

		/// <summary>Gets a Boolean value that tells whether the receiver has any unsaved changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasChanges")]
		bool HasChanges { get; }

		/// <param name="objectID">To be added.</param>
		///         <summary>Returns the object that is identified by <paramref name="objectID" />, if it represents a registered object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("objectRegisteredForID:")]
		[return: NullAllowed]
		NSManagedObject ObjectRegisteredForID (NSManagedObjectID objectID);

		/// <param name="objectID">To be added.</param>
		///         <summary>Returns the object that is identified by <paramref name="objectID" /></summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("objectWithID:")]
		NSManagedObject ObjectWithID (NSManagedObjectID objectID);

		/// <param name="request">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Runs the specified <paramref name="request" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("executeFetchRequest:error:")]
		[return: NullAllowed]
		NSObject [] ExecuteFetchRequest (NSFetchRequest request, out NSError error);

		/// <param name="request">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Returns the number of objects that <paramref name="request" /> would return if it were run.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("countForFetchRequest:error:")]
		nuint CountForFetchRequest (NSFetchRequest request, out NSError error);

		/// <param name="object1">To be added.</param>
		///         <summary>Inserts <paramref name="object1" /> into the context.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("insertObject:")]
		void InsertObject (NSManagedObject object1);

		/// <param name="object1">To be added.</param>
		///         <summary>Queues <paramref name="object1" /> for deletion.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deleteObject:")]
		void DeleteObject (NSManagedObject object1);

		/// <param name="object1">To be added.</param>
		///         <param name="flag">To be added.</param>
		///         <summary>Refreshes <paramref name="object1" /> with the most current values from its store.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("refreshObject:mergeChanges:")]
		void RefreshObject (NSManagedObject object1, bool flag);

		/// <param name="object1">To be added.</param>
		///         <summary>Marks <paramref name="object1" /> for conflict detection.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("detectConflictsForObject:")]
		void DetectConflictsForObject (NSManagedObject object1);

		/// <param name="keyPath">To be added.</param>
		/// <param name="object1">To be added.</param>
		/// <param name="change">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="context">To be added.</param>
		/// <summary>Observes the object that is identified by the provided object and keypath for changes.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("observeValueForKeyPath:ofObject:change:context:")]
#if XAMCORE_5_0
		void ObserveValue ([NullAllowed] string keyPath, [NullAllowed] NSObject object1, [NullAllowed] NSDictionary<NSString, NSObject> change, IntPtr context);
#elif NET
		void ObserveValue ([NullAllowed] string keyPath, [NullAllowed] NSObject object1, [NullAllowed] NSDictionary change, IntPtr context);
#else
		void ObserveValueForKeyPath ([NullAllowed] string keyPath, IntPtr object1, [NullAllowed] NSDictionary change, IntPtr context);
#endif

		/// <summary>Tells the receiver to process all changes on the object graph.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("processPendingChanges")]
		void ProcessPendingChanges ();

		/// <param name="object1">To be added.</param>
		/// <param name="store">To be added.</param>
		/// <summary>Assigns <paramref name="object1" /> to <paramref name="store" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("assignObject:toPersistentStore:")]
#if NET
		void AssignObject (NSObject object1, NSPersistentStore store);
#else
		void AssignObject (IntPtr object1, NSPersistentStore store);
#endif

		/// <summary>Gets the inserted, but unsaved, objects in the context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("insertedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> InsertedObjects { get; }
#else
		NSSet InsertedObjects { get; }
#endif

		/// <summary>Gets a set that contains all of the receiver's objects which have uncommitted changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("updatedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> UpdatedObjects { get; }
#else
		NSSet UpdatedObjects { get; }
#endif

		/// <summary>Gets a set that contains the objects that will be deleted by the next save operation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("deletedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> DeletedObjects { get; }
#else
		NSSet DeletedObjects { get; }
#endif

		/// <summary>Gets a set that contains all of the receiver's registered objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("registeredObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> RegisteredObjects { get; }
#else
		NSSet RegisteredObjects { get; }
#endif

		/// <summary>Instructs the receiver to undo its uncommitted changes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("undo")]
		void Undo ();

		/// <summary>Reverses the most recent unreversed undo.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("redo")]
		void Redo ();

		/// <summary>Resets the receiver.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reset")]
		void Reset ();

		/// <summary>Rolls the state of all objects in the object graph back to the most recent committed values.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("rollback")]
		void Rollback ();

		/// <param name="error">To be added.</param>
		///         <summary>Saves uncommitted changes and reports any error that it encounters.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("save:")]
		bool Save (out NSError error);

#pragma warning disable 0109 // warning CS0109: The member 'NSManagedObjectContext.Lock()' does not hide an accessible member. The new keyword is not required.
		/// <summary>Developers should not use this deprecated method. Developers should use a queue style context and 'PerformAndWait' instead.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[Export ("lock")]
		new void Lock ();
#pragma warning restore

#pragma warning disable 0109 // warning CS0109: The member 'NSManagedObjectContext.Unlock()' does not hide an accessible member. The new keyword is not required.
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use a queue style context and 'PerformAndWait' instead.")]
		[Export ("unlock")]
		new void Unlock ();
#pragma warning restore

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use a queue style context and 'Perform' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use a queue style context and 'Perform' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use a queue style context and 'Perform' instead.")]
		[Export ("tryLock")]
		bool TryLock { get; }

		/// <summary>Gets or sets a Boolean value that controls whether the receiver propagates deletes after change events.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("propagatesDeletesAtEndOfEvent")]
		bool PropagatesDeletesAtEndOfEvent { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the receiver retains hard references to its registered managed objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("retainsRegisteredObjects")]
		bool RetainsRegisteredObjects { get; set; }

		/// <summary>Gets or sets the maximum elapsed time after fetching which causes fault fulfillment to fetch new data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stalenessInterval")]
		double StalenessInterval { get; set; }

		/// <summary>Gets or sets the receiver's merge policy.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mergePolicy", ArgumentSemantic.Retain)]
		IntPtr MergePolicy { get; set; }

		/// <param name="objects">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Converts the objec identifiers to permanent identifiers for the provided <paramref name="objects" />. Returns <see langword="true" /> if all of the identifiers were converted.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("obtainPermanentIDsForObjects:error:")]
		bool ObtainPermanentIDsForObjects (NSManagedObject [] objects, out NSError error);

		/// <param name="notification">To be added.</param>
		///         <summary>Method that is called to merge the changes that are specified by <paramref name="notification" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("mergeChangesFromContextDidSaveNotification:")]
		void MergeChangesFromContextDidSaveNotification (NSNotification notification);

		/// <param name="ct">To be added.</param>
		/// <summary>Creates a new <see cref="T:CoreData.NSManagedObjectContext" /> of the specified type.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithConcurrencyType:")]
		NativeHandle Constructor (NSManagedObjectContextConcurrencyType ct);

		/// <param name="action">To be added.</param>
		///         <summary>Asynchronously performs the specified <paramref name="action" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("performBlock:")]
		void Perform (/* non null */ Action action);

		/// <param name="action">To be added.</param>
		///         <summary>Synchronously performs the specified <paramref name="action" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("performBlockAndWait:")]
		void PerformAndWait (/* non null */ Action action);

		/// <summary>Gets the receiver's user information.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userInfo", ArgumentSemantic.Strong)]
		NSMutableDictionary UserInfo { get; }

		/// <summary>Gets the receiver's concurrency type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("concurrencyType")]
		NSManagedObjectContextConcurrencyType ConcurrencyType { get; }

		//Detected properties
		// default is null, but setting it to null again would crash the app
		/// <summary>Gets the receiver's parent context.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("parentContext", ArgumentSemantic.Retain)]
		NSManagedObjectContext ParentContext { get; set; }

		/// <include file="../docs/api/CoreData/NSManagedObjectContext.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSManagedObjectContext.ObjectsDidChangeNotification']/*" />
		[Field ("NSManagedObjectContextObjectsDidChangeNotification")]
		[Notification (typeof (NSManagedObjectChangeEventArgs))]
		NSString ObjectsDidChangeNotification { get; }

		/// <include file="../docs/api/CoreData/NSManagedObjectContext.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSManagedObjectContext.DidSaveNotification']/*" />
		[Field ("NSManagedObjectContextDidSaveNotification")]
		[Notification (typeof (NSManagedObjectChangeEventArgs))]
		NSString DidSaveNotification { get; }

		/// <include file="../docs/api/CoreData/NSManagedObjectContext.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSManagedObjectContext.WillSaveNotification']/*" />
		[Field ("NSManagedObjectContextWillSaveNotification")]
		[Notification ()]
		NSString WillSaveNotification { get; }

		/// <summary>Gets or sets a name for the object.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("name")]
		string Name { get; set; }

		/// <param name="request">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Runs the specified <paramref name="request" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("executeRequest:error:")]
		[return: NullAllowed]
		NSPersistentStoreResult ExecuteRequest (NSPersistentStoreRequest request, out NSError error);

		/// <param name="objectID">The object ID of object to fetch.</param>
		///         <param name="error">On error, this will contain the error information.</param>
		///         <summary>Fetches an object with a specified id.</summary>
		///         <returns>The object with the associated ID, or null if the
		/// 	object does not exist, or can not be retrieved.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("existingObjectWithID:error:")]
		[return: NullAllowed]
		NSManagedObject GetExistingObject (NSManagedObjectID objectID, out NSError error);

		/// <summary>Gets or sets a Boolean value that controls whether inaccessible faults wil be deleted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shouldDeleteInaccessibleFaults")]
		bool ShouldDeleteInaccessibleFaults { get; set; }

		/// <param name="fault">To be added.</param>
		///         <param name="oid">To be added.</param>
		///         <param name="property">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Returns a Boolean value that controls whether inaccessible faults wil be handled.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shouldHandleInaccessibleFault:forObjectID:triggeredByProperty:")]
		bool ShouldHandleInaccessibleFault (NSManagedObject fault, NSManagedObjectID oid, [NullAllowed] NSPropertyDescription property);

		/// <param name="changeNotificationData">To be added.</param>
		///         <param name="contexts">To be added.</param>
		///         <summary>Merges remote changes.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("mergeChangesFromRemoteContextSave:intoContexts:")]
		void MergeChangesFromRemoteContextSave (NSDictionary changeNotificationData, NSManagedObjectContext [] contexts);

		/// <summary>Gets the token that identifes the current query generation behavior.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("queryGenerationToken", ArgumentSemantic.Strong)]
		NSQueryGenerationToken QueryGenerationToken { get; }

		/// <param name="generation">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>Sets the query generation from the specified query generation token.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setQueryGenerationFromToken:error:")]
		bool SetQueryGenerationFromToken ([NullAllowed] NSQueryGenerationToken generation, out NSError error);

		/// <summary>Gets or sets a Boolean that tells whether changes in the parent are automatically merged.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("automaticallyMergesChangesFromParent")]
		bool AutomaticallyMergesChangesFromParent { get; set; }

		/// <summary>Refreshes all objects in the store.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("refreshAllObjects")]
		void RefreshAllObjects ();

		/// <summary>Gets or sets the author of the persistent history context.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("transactionAuthor")]
		string TransactionAuthor { get; set; }

		[Notification (typeof (NSManagedObjectsIdsChangedEventArgs))]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("NSManagedObjectContextDidMergeChangesObjectIDsNotification")]
		NSString DidMergeChangesObjectIdsNotification { get; }

		[Notification (typeof (NSManagedObjectsIdsChangedEventArgs))]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("NSManagedObjectContextDidSaveObjectIDsNotification")]
		NSString DidSaveObjectIdsNotification { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	interface NSManagedObjectsIdsChangedEventArgs {
		[Export ("NSDeletedObjectIDsKey")]
		NSSet DeletedObjectIds { get; }

		[Export ("NSInsertedObjectIDsKey")]
		NSSet InsertedObjectIdsKey { get; }

		[Export ("NSInvalidatedObjectIDsKey")]
		NSSet InvalidatedObjectIdsKey { get; }

		[Export ("NSRefreshedObjectIDsKey")]
		NSSet RefreshedObjectIdsKey { get; }

		[Export ("NSUpdatedObjectIDsKey")]
		NSSet UpdatedObjectIdsKey { get; }
	}

	interface NSManagedObjectChangeEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NSInsertedObjectsKey")]
		NSSet InsertedObjects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NSUpdatedObjectsKey")]
		NSSet UpdatedObjects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NSDeletedObjectsKey")]
		NSSet DeletedObjects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NSRefreshedObjectsKey")]
		NSSet RefreshedObjects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NSInvalidatedObjectsKey")]
		NSSet InvalidatedObjects { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[ProbePresence]
		[Export ("NSInvalidatedAllObjectsKey")]
		bool InvalidatedAllObjects { get; }
	}

	/// <summary>A universal identifier of a Core Data managed object. Works across object contexts and applications.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSManagedObjectID_Class/index.html">Apple documentation for <c>NSManagedObjectID</c></related>
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -URIRepresentation cannot be sent to an abstract object of class NSManagedObjectID: Create a concrete instance!
	[DisableDefaultCtor]
	interface NSManagedObjectID : NSCopying, NSFetchRequestResult {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entity", ArgumentSemantic.Strong)]
		NSEntityDescription Entity { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("persistentStore", ArgumentSemantic.Weak)]
		NSPersistentStore PersistentStore { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("temporaryID")]
		bool IsTemporaryID { [Bind ("isTemporaryID")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URIRepresentation")]
		NSUrl URIRepresentation { get; }

	}

	/// <summary>A schema describing a graph of entities used by the application.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSManagedObjectModel_Class/index.html">Apple documentation for <c>NSManagedObjectModel</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface NSManagedObjectModel : NSCoding, NSCopying {

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		/// <remarks>
		///         </remarks>
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		/// <param name="bundles">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static, Export ("mergedModelFromBundles:")]
		[return: NullAllowed]
#if NET
		NSManagedObjectModel GetMergedModel ([NullAllowed] NSBundle [] bundles);
#else
		NSManagedObjectModel MergedModelFromBundles ([NullAllowed] NSBundle [] bundles);
#endif

		/// <param name="models">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("modelByMergingModels:")]
		[return: NullAllowed]
		NSManagedObjectModel ModelByMergingModels ([NullAllowed] NSManagedObjectModel [] models);

		/// <param name="url">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithContentsOfURL:")]
		NativeHandle Constructor (NSUrl url);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entitiesByName", ArgumentSemantic.Copy)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSEntityDescription> EntitiesByName { get; }
#else
		NSDictionary EntitiesByName { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entities", ArgumentSemantic.Retain)]
		NSEntityDescription [] Entities { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("configurations", ArgumentSemantic.Strong)]
		string [] Configurations { get; }

		/// <param name="configuration">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("entitiesForConfiguration:")]
		[return: NullAllowed]
		string [] EntitiesForConfiguration ([NullAllowed] string configuration);

		/// <param name="entities">To be added.</param>
		///         <param name="configuration">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setEntities:forConfiguration:")]
		void SetEntities (NSEntityDescription [] entities, string configuration);

		/// <param name="fetchRequestTemplate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setFetchRequestTemplate:forName:")]
		void SetFetchRequestTemplate ([NullAllowed] NSFetchRequest fetchRequestTemplate, string name);

		[Export ("fetchRequestTemplateForName:")]
		[return: NullAllowed]
#if NET
		NSFetchRequest GetFetchRequestTemplate (string name);
#else
		NSFetchRequest FetchRequestTemplateForName (string name);
#endif

		[Export ("fetchRequestFromTemplateWithName:substitutionVariables:")]
		[return: NullAllowed]
#if XAMCORE_5_0
		NSFetchRequest GetFetchRequestFromTemplate (string name, NSDictionary<NSString, NSObject> variables);
#elif NET
		NSFetchRequest GetFetchRequestFromTemplate (string name, NSDictionary variables);
#else
		NSFetchRequest FetchRequestFromTemplateWithName (string name, NSDictionary variables);
#endif

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("localizationDictionary", ArgumentSemantic.Retain)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSString> LocalizationDictionary { get; set; }
#else
		NSDictionary LocalizationDictionary { get; set; }
#endif

		/// <param name="bundles">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="metadata">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static, Export ("mergedModelFromBundles:forStoreMetadata:")]
		[return: NullAllowed]
#if XAMCORE_5_0
		NSManagedObjectModel GetMergedModel ([NullAllowed] NSBundle[] bundles, NSDictionary<NSString, NSObject> metadata);
#elif NET
		NSManagedObjectModel GetMergedModel ([NullAllowed] NSBundle [] bundles, NSDictionary metadata);
#else
		NSManagedObjectModel MergedModelFromBundles ([NullAllowed] NSBundle [] bundles, NSDictionary metadata);
#endif

		[Static, Export ("modelByMergingModels:forStoreMetadata:")]
		[return: NullAllowed]
#if XAMCORE_5_0
		NSManagedObjectModel GetModelByMerging (NSManagedObjectModel[] models, NSDictionary<NSString, NSObject> metadata);
#elif NET
		NSManagedObjectModel GetModelByMerging (NSManagedObjectModel [] models, NSDictionary metadata);
#else
		NSManagedObjectModel ModelByMergingModels (NSManagedObjectModel [] models, NSDictionary metadata);
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchRequestTemplatesByName", ArgumentSemantic.Copy)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSFetchRequest> FetchRequestTemplatesByName { get; }
#else
		NSDictionary FetchRequestTemplatesByName { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("versionIdentifiers", ArgumentSemantic.Copy)]
		NSSet VersionIdentifiers { get; set; }

		/// <param name="configuration">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="metadata">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isConfiguration:compatibleWithStoreMetadata:")]
#if XAMCORE_5_0
		bool IsConfigurationCompatibleWithStoreMetadata ([NullAllowed] string configuration, NSDictionary<NSString, NSObject> metadata);
#elif NET
		bool IsConfigurationCompatibleWithStoreMetadata ([NullAllowed] string configuration, NSDictionary metadata);
#else
		bool IsConfiguration ([NullAllowed] string configuration, NSDictionary metadata);
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entityVersionHashesByName", ArgumentSemantic.Copy)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSData> EntityVersionHashesByName { get; }
#else
		NSDictionary EntityVersionHashesByName { get; }
#endif

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("checksumsForVersionedModelAtURL:error:")]
		[return: NullAllowed]
		NSDictionary<NSString, NSString> ChecksumsForVersionedModel (NSUrl modelUrl, [NullAllowed] out NSError error);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("versionChecksum")]
		string VersionChecksum { get; }
	}

	/// <summary>Holds mappings between a source and destination managed object model.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSMappingModel_class/index.html">Apple documentation for <c>NSMappingModel</c></related>
	[BaseType (typeof (NSObject))]
	interface NSMappingModel {

		[Static, Export ("mappingModelFromBundles:forSourceModel:destinationModel:")]
		[return: NullAllowed]
#if NET
		NSMappingModel GetMappingModel ([NullAllowed] NSBundle [] bundles, [NullAllowed] NSManagedObjectModel sourceModel, [NullAllowed] NSManagedObjectModel destinationModel);
#else
		NSMappingModel MappingModelFromBundles ([NullAllowed] NSBundle [] bundles, [NullAllowed] NSManagedObjectModel sourceModel, [NullAllowed] NSManagedObjectModel destinationModel);
#endif

		/// <param name="source">To be added.</param>
		///         <param name="destination">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("inferredMappingModelForSourceModel:destinationModel:error:")]
		[return: NullAllowed]
		NSMappingModel GetInferredMappingModel (NSManagedObjectModel source, NSManagedObjectModel destination, out NSError error);

		/// <param name="url">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithContentsOfURL:")]
		NativeHandle Constructor ([NullAllowed] NSUrl url);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entityMappings", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSEntityMapping [] EntityMappings { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entityMappingsByName", ArgumentSemantic.Copy)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSEntityMapping> EntityMappingsByName { get; }
#else
		NSDictionary EntityMappingsByName { get; }
#endif

	}

	/// <summary>Models conflicts that can occur when saving changes.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSMergeConflict_Class/index.html">Apple documentation for <c>NSMergeConflict</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSMergeConflict {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceObject", ArgumentSemantic.Retain)]
		NSManagedObject SourceObject { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("objectSnapshot", ArgumentSemantic.Retain)]
		[NullAllowed]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> ObjectSnapshot { get; }
#else
		NSDictionary ObjectSnapshot { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cachedSnapshot", ArgumentSemantic.Retain)]
		[NullAllowed]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> CachedSnapshot { get; }
#else
		NSDictionary CachedSnapshot { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistedSnapshot", ArgumentSemantic.Retain)]
		[NullAllowed]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> PersistedSnapshot { get; }
#else
		NSDictionary PersistedSnapshot { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("newVersionNumber")]
		nuint NewVersionNumber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("oldVersionNumber")]
		nuint OldVersionNumber { get; }

		/// <param name="sourceObject">To be added.</param>
		/// <param name="newVersion">To be added.</param>
		/// <param name="oldVersion">To be added.</param>
		/// <param name="cachedSnapshot">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="persistedSnapshot">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithSource:newVersion:oldVersion:cachedSnapshot:persistedSnapshot:")]
#if XAMCORE_5_0
		NativeHandle Constructor (NSManagedObject sourceObject, nuint newVersion, nuint oldVersion, [NullAllowed] NSDictionary<NSString, NSObject> cachedSnapshot, [NullAllowed] NSDictionary<NSString, NSObject> persistedSnapshot);
#elif NET
		NativeHandle Constructor (NSManagedObject sourceObject, nuint newVersion, nuint oldVersion, [NullAllowed] NSDictionary cachedSnapshot, [NullAllowed] NSDictionary persistedSnapshot);
#else
		NativeHandle Constructor (NSManagedObject srcObject, nuint newvers, nuint oldvers, [NullAllowed] NSDictionary cachesnap, [NullAllowed] NSDictionary persnap);
#endif
	}

	/// <summary>Strategy for resolving conflicts between in-memory objects and those in persistent stores.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSMergePolicy_Class/index.html">Apple documentation for <c>NSMergePolicy</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSMergePolicy {
		/// <summary>Gets the policy's merge type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mergeType")]
		NSMergePolicyType MergeType { get; }

		/// <param name="ty">To be added.</param>
		/// <summary>Creates a new <see cref="T:CoreData.NSMergePolicy" /> from the specified merege policy type.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithMergeType:")]
		NativeHandle Constructor (NSMergePolicyType ty);

		/// <param name="list">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Attempts to resolve the specified conflicts, and reports any errors.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("resolveConflicts:error:")]
#if NET
		bool ResolveConflicts (NSMergeConflict [] list, out NSError error);
#else
		bool ResolveConflictserror (NSMergeConflict [] list, out NSError error);
#endif

		/// <param name="list">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Attempts to resolve the specified locking constraint conflicts, and reports any errors.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("resolveOptimisticLockingVersionConflicts:error:")]
		bool ResolveOptimisticLockingVersionConflicts (NSMergeConflict [] list, out NSError error);

		/// <param name="list">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Attempts to resolve the specified constraint conflicts, and reports any errors.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("resolveConstraintConflicts:error:")]
		bool ResolveConstraintConflicts (NSConstraintConflict [] list, out NSError error);

		/// <summary>Gets a value that indicates that saving will fail if an error is encountered.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("errorMergePolicy", ArgumentSemantic.Strong)]
		NSMergePolicy ErrorPolicy { get; }

		/// <summary>Gets a value that indicates tha in-memory state changes are discarded for conflicting objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("rollbackMergePolicy", ArgumentSemantic.Strong)]
		NSMergePolicy RollbackPolicy { get; }

		/// <summary>Gets a value that controls policy for handling overwrites.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("overwriteMergePolicy", ArgumentSemantic.Strong)]
		NSMergePolicy OverwritePolicy { get; }

		/// <summary>Gets a value that indicates that the property object is favored during merges.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("mergeByPropertyObjectTrumpMergePolicy", ArgumentSemantic.Strong)]
		NSMergePolicy MergeByPropertyObjectTrumpPolicy { get; }

		/// <summary>Gets a value that indicates that the property store is favored during merges.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("mergeByPropertyStoreTrumpMergePolicy", ArgumentSemantic.Strong)]
		NSMergePolicy MergeByPropertyStoreTrumpPolicy { get; }
	}

	/// <summary>Allows migration from one persistent store to another.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSMigrationManager_class/index.html">Apple documentation for <c>NSMigrationManager</c></related>
	[BaseType (typeof (NSObject))]
	interface NSMigrationManager {

		/// <param name="sourceModel">To be added.</param>
		/// <param name="destinationModel">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithSourceModel:destinationModel:")]
		NativeHandle Constructor (NSManagedObjectModel sourceModel, NSManagedObjectModel destinationModel);

		/// <param name="sourceUrl">To be added.</param>
		///         <param name="sStoreType">To be added.</param>
		///         <param name="sOptions">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="mappings">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="dUrl">To be added.</param>
		///         <param name="dStoreType">To be added.</param>
		///         <param name="dOptions">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("migrateStoreFromURL:type:options:withMappingModel:toDestinationURL:destinationType:destinationOptions:error:")]
		bool MigrateStoreFromUrl (NSUrl sourceUrl, string sStoreType, [NullAllowed] NSDictionary sOptions, [NullAllowed] NSMappingModel mappings, NSUrl dUrl, string dStoreType, [NullAllowed] NSDictionary dOptions, out NSError error);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reset")]
		void Reset ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mappingModel", ArgumentSemantic.Strong)]
		NSMappingModel MappingModel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceModel", ArgumentSemantic.Strong)]
		NSManagedObjectModel SourceModel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationModel", ArgumentSemantic.Strong)]
		NSManagedObjectModel DestinationModel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceContext", ArgumentSemantic.Strong)]
		NSManagedObjectContext SourceContext { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationContext", ArgumentSemantic.Strong)]
		NSManagedObjectContext DestinationContext { get; }

		/// <param name="mEntity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("sourceEntityForEntityMapping:")]
		[return: NullAllowed]
		NSEntityDescription SourceEntityForEntityMapping (NSEntityMapping mEntity);

		/// <param name="mEntity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("destinationEntityForEntityMapping:")]
		[return: NullAllowed]
		NSEntityDescription DestinationEntityForEntityMapping (NSEntityMapping mEntity);

		/// <param name="sourceInstance">To be added.</param>
		///         <param name="destinationInstance">To be added.</param>
		///         <param name="entityMapping">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("associateSourceInstance:withDestinationInstance:forEntityMapping:")]
		void AssociateSourceInstance (NSManagedObject sourceInstance, NSManagedObject destinationInstance, NSEntityMapping entityMapping);

		/// <param name="mappingName">To be added.</param>
		///         <param name="sourceInstances">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("destinationInstancesForEntityMappingNamed:sourceInstances:")]
		NSManagedObject [] DestinationInstancesForEntityMappingNamed (string mappingName, [NullAllowed] NSManagedObject [] sourceInstances);

		/// <param name="mappingName">To be added.</param>
		///         <param name="destinationInstances">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("sourceInstancesForEntityMappingNamed:destinationInstances:")]
		NSManagedObject [] SourceInstancesForEntityMappingNamed (string mappingName, [NullAllowed] NSManagedObject [] destinationInstances);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentEntityMapping", ArgumentSemantic.Strong)]
		NSEntityMapping CurrentEntityMapping { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("migrationProgress")]
		float MigrationProgress { get; }  /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("userInfo", ArgumentSemantic.Retain)]
		NSDictionary UserInfo { get; set; }

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelMigrationWithError:")]
		void CancelMigrationWithError (NSError error);

		// 5.0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("usesStoreSpecificMigrationManager")]
		bool UsesStoreSpecificMigrationManager { get; set; }
	}

	[Abstract]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSPersistentHistoryChange : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changeID")]
		long ChangeId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changedObjectID", ArgumentSemantic.Copy)]
		NSManagedObjectID ChangedObjectId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changeType")]
		NSPersistentHistoryChangeType ChangeType { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("tombstone", ArgumentSemantic.Copy)]
		NSDictionary Tombstone { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("transaction", ArgumentSemantic.Strong)]
		NSPersistentHistoryTransaction Transaction { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("updatedProperties", ArgumentSemantic.Copy)]
		NSSet<NSPropertyDescription> UpdatedProperties { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[NullAllowed, Export ("entityDescription")]
		NSEntityDescription EntityDescription { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[NullAllowed, Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("entityDescriptionWithContext:")]
		[return: NullAllowed]
		NSEntityDescription GetEntityDescription (NSManagedObjectContext context);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSPersistentHistoryToken : NSCopying //, NSSecureCoding TODO: The class does state that it supports the NSSecureCoding YET SupportsSecureCoding returns false, radar 32761925
	{
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	[DisableDefaultCtor]
	interface NSPersistentHistoryChangeRequest {
		/// <param name="date">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("fetchHistoryAfterDate:")]
		NSPersistentHistoryChangeRequest FetchHistoryAfter (NSDate date);

		/// <param name="token">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("fetchHistoryAfterToken:")]
		NSPersistentHistoryChangeRequest FetchHistoryAfter ([NullAllowed] NSPersistentHistoryToken token);

		/// <param name="transaction">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("fetchHistoryAfterTransaction:")]
		NSPersistentHistoryChangeRequest FetchHistoryAfter ([NullAllowed] NSPersistentHistoryTransaction transaction);

		/// <param name="date">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("deleteHistoryBeforeDate:")]
		NSPersistentHistoryChangeRequest DeleteHistoryBefore (NSDate date);

		/// <param name="token">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("deleteHistoryBeforeToken:")]
		NSPersistentHistoryChangeRequest DeleteHistoryBefore ([NullAllowed] NSPersistentHistoryToken token);

		/// <param name="transaction">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("deleteHistoryBeforeTransaction:")]
		NSPersistentHistoryChangeRequest DeleteHistoryBefore ([NullAllowed] NSPersistentHistoryTransaction transaction);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType", ArgumentSemantic.Assign)]
		NSPersistentHistoryResultType ResultType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("token", ArgumentSemantic.Strong)]
		NSPersistentHistoryToken Token { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("fetchHistoryWithFetchRequest:")]
		NSPersistentHistoryChangeRequest FetchHistory (NSFetchRequest fetchRequest);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("fetchRequest", ArgumentSemantic.Strong)]
		NSFetchRequest FetchRequest { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreResult))]
	interface NSPersistentHistoryResult {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("result", ArgumentSemantic.Strong)]
		NSObject Result { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType")]
		NSPersistentHistoryResultType ResultType { get; }
	}

	[Abstract]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSPersistentHistoryTransaction : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timestamp", ArgumentSemantic.Copy)]
		NSDate Timestamp { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("changes", ArgumentSemantic.Copy)]
		NSPersistentHistoryChange [] Changes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transactionNumber")]
		long TransactionNumber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("storeID")]
		string StoreId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bundleID")]
		string BundleId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("processID")]
		string ProcessId { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("contextName")]
		string ContextName { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("author")]
		string Author { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("token", ArgumentSemantic.Strong)]
		NSPersistentHistoryToken Token { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("objectIDNotification")]
		NSNotification ObjectIdNotification { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("entityDescriptionWithContext:")]
		[return: NullAllowed]
		NSEntityDescription GetEntityDescription (NSManagedObjectContext context);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[NullAllowed, Export ("entityDescription")]
		NSEntityDescription EntityDescription { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[NullAllowed, Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; }
	}

	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSInternalInconsistencyException Reason: NSCoreDataCoreSpotlightDelegate requires the use of the initializer initForStoreWithDescription:model: 
	interface NSCoreDataCoreSpotlightDelegate {

		[Notification]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("NSCoreDataCoreSpotlightDelegateIndexDidUpdateNotification")]
		NSString IndexDidUpdateNotification { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initForStoreWithDescription:coordinator:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSPersistentStoreDescription description, NSPersistentStoreCoordinator psc);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("domainIdentifier")]
		string DomainIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("indexName")]
		string IndexName { get; }

		/// <param name="description">To be added.</param>
		/// <param name="model">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the constructor that takes a NSPersistentStoreCoordinator instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the constructor that takes a NSPersistentStoreCoordinator instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the constructor that takes a NSPersistentStoreCoordinator instead.")]
		[Export ("initForStoreWithDescription:model:")]
		NativeHandle Constructor (NSPersistentStoreDescription description, NSManagedObjectModel model);

		/// <param name="object">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("attributeSetForObject:")]
		[return: NullAllowed]
		CSSearchableItemAttributeSet GetAttributeSet (NSManagedObject @object);

		/// <param name="searchableIndex">To be added.</param>
		///         <param name="acknowledgementHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("searchableIndex:reindexAllSearchableItemsWithAcknowledgementHandler:")]
		void ReindexAllSearchableItems (CSSearchableIndex searchableIndex, Action acknowledgementHandler);

		/// <param name="searchableIndex">To be added.</param>
		///         <param name="identifiers">To be added.</param>
		///         <param name="acknowledgementHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("searchableIndex:reindexSearchableItemsWithIdentifiers:acknowledgementHandler:")]
		void ReindexSearchableItems (CSSearchableIndex searchableIndex, string [] identifiers, Action acknowledgementHandler);

		[Async]
		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("deleteSpotlightIndexWithCompletionHandler:")]
		void DeleteSpotlightIndex (Action<NSError> completionHandler);

		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("indexingEnabled")]
		bool IndexingEnabled { [Bind ("isIndexingEnabled")] get; }

		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("startSpotlightIndexing")]
		void StartSpotlightIndexing ();

		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("stopSpotlightIndexing")]
		void StopSpotlightIndexing ();
	}

	// NSPersistentStore is an abstract type according to Apple's documentation, but Apple
	// also have internal subclasses of NSPersistentStore, and in those cases our closest
	// type is NSPersistentStore, which means we must be able to create managed wrappers
	// for such native classes using the managed NSPersistentStore. This means we can't
	// make our managed version [Abstract].
	/// <summary>Abstract base class for Core Data persistent stores.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSPersistentStore_Class/index.html">Apple documentation for <c>NSPersistentStore</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSPersistentStore {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("migrationManagerClass")]
		Class MigrationManagerClass { get; }

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Gets the metadata for the store at the provided URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		[return: NullAllowed]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> GetMetadataForPersistentStore (NSUrl url, out NSError error);
#else
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);
#endif

		/// <param name="metadata">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Sets the metadata for the store at the provided URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("setMetadata:forPersistentStoreWithURL:error:")]
#if XAMCORE_5_0
		bool SetMetadata ([NullAllowed] NSDictionary<NSString, NSObject> metadata, NSUrl url, out NSError error);
#else
		bool SetMetadata ([NullAllowed] NSDictionary metadata, NSUrl url, out NSError error);
#endif

#if NET
		/// <param name="root">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="name">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="url">To be added.</param>
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Protected]
#endif
		[DesignatedInitializer]
		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		NativeHandle Constructor ([NullAllowed] NSPersistentStoreCoordinator root, [NullAllowed] string name, NSUrl url, [NullAllowed] NSDictionary options);

		/// <param name="error">The error that was encountered, or <see langword="null" /> if no error was encountered.</param>
		///         <summary>Causes the store to load its metadata.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("loadMetadata:")]
		bool LoadMetadata (out NSError error);

		/// <summary>Gets the coordinator to which this store belongs, if present.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("persistentStoreCoordinator", ArgumentSemantic.Weak)]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; }

		/// <summary>Gets the managed object model configuration name for the configuration that was used to create the store.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("configurationName")]
		string ConfigurationName { get; }

		/// <summary>Gets the options with which the store was initialized.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("options", ArgumentSemantic.Strong)]
		NSDictionary Options { get; }

		/// <summary>Gets or sets the URL of the store.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("URL", ArgumentSemantic.Retain)]
		NSUrl Url { get; set; }

		/// <summary>Gets the unique identifier for this store.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		[NullAllowed]
		string Identifier { get; set; }

		/// <summary>Application developers override this method to provide a unique store type to use when adding the store to the store coordinator.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		string Type { get; }

		/// <summary>Gets or sets the read-only status of the store</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("readOnly")]
		bool ReadOnly { [Bind ("isReadOnly")] get; set; }

		/// <summary>Gets or sets the store metadata.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("metadata", ArgumentSemantic.Retain)]
		[NullAllowed]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> Metadata { get; set; }
#else
		NSDictionary Metadata { get; set; }
#endif

		/// <param name="coordinator">To be added.</param>
		///         <summary>Method that is called when this store is added to the store coordinator.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("didAddToPersistentStoreCoordinator:")]
		void DidAddToPersistentStoreCoordinator (NSPersistentStoreCoordinator coordinator);

		/// <param name="coordinator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Method that is called when the store is about to be removed from the coordinator.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("willRemoveFromPersistentStoreCoordinator:")]
		void WillRemoveFromPersistentStoreCoordinator ([NullAllowed] NSPersistentStoreCoordinator coordinator);

		/// <summary>Represents the value associated with the constant NSPersistentStoreSaveConflictsErrorKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSPersistentStoreSaveConflictsErrorKey")]
		NSString SaveConflictsErrorKey { get; }

		/// <summary>Gets the spotlight exporter for the store.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("coreSpotlightExporter")]
		NSCoreDataCoreSpotlightDelegate CoreSpotlightExporter { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreRemoteChangeNotificationPostOptionKey")]
		NSString RemoteChangeNotificationPostOptionKey { get; }

		[Notification (typeof (NSPersistentStoreRemoteChangeEventArgs))]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreRemoteChangeNotification")]
		NSString StoreRemoteChangeNotification { get; }

	}

	interface NSPersistentStoreRemoteChangeEventArgs {
		[Export ("NSStoreUUIDKey")]
		NSUuid Uuid { get; }

		[Export ("NSPersistentStoreURLKey")]
		string Url { get; }

		[Export ("NSPersistentHistoryTokenKey")]
		NSPersistentHistoryToken PersistentHistoryTracking { get; }
	}

	/// <summary>Descriptor for a persistent store inside a persistent container.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSPersistentStoreDescription">Apple documentation for <c>NSPersistentStoreDescription</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSPersistentStoreDescription : NSCopying {
		/// <param name="Url">The store URL for which to get a description.</param>
		///         <summary>Returns the description for the store at the specified URL.</summary>
		///         <returns>The description for the store at the specified URL.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("persistentStoreDescriptionWithURL:")]
		NSPersistentStoreDescription GetPersistentStoreDescription (NSUrl Url);

		/// <summary>Gets or sets the store type.</summary>
		///         <value>The store type.</value>
		///         <remarks>
		///           <para>
		///             <see cref="T:CoreData.NSPersistentStoreCoordinator" /> contains static properties that represent valid store types.</para>
		///         </remarks>
		[Export ("type")]
		string Type { get; set; }

		/// <summary>Gets or sets the configuration that is used by the store.</summary>
		///         <value>
		///           <para>The configuration that is used by the store.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("configuration")]
		string Configuration { get; set; }

		/// <summary>Gets or sets the URL for the store.</summary>
		///         <value>
		///           <para>The URL for the store.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }

		/// <summary>Gets the dictionary of options for the store.</summary>
		///         <value>The dictionary of options for the store.</value>
		///         <remarks>
		///           <para>
		///             <see cref="T:CoreData.NSPersistentStoreCoordinator" /> contains static properties that represent the option keys that are valid for this dictionary.</para>
		///         </remarks>
		[Export ("options", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Options { get; }

		/// <param name="option">
		///           <para>The value of the option to set.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="key">The key for the value to set.</param>
		///         <summary>Sets the option for the specified <paramref name="key" />.</summary>
		///         <remarks>
		///           <para>
		///             <see cref="T:CoreData.NSPersistentStoreCoordinator" /> contains static properties that represent the option keys that are valid for this dictionary.</para>
		///         </remarks>
		[Export ("setOption:forKey:")]
		void SetOption ([NullAllowed] NSObject option, string key);

		/// <summary>Gets or sets a value that tells whether the store is read-only.</summary>
		///         <value>
		///           <see langword="true" /> if the store is read-only. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("readOnly")]
		bool IsReadOnly { [Bind ("isReadOnly")] get; set; }

		/// <summary>Gets or sets the connection timeout interval.</summary>
		///         <value>The connection timeout interval.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timeout")]
		double Timeout { get; set; }

		/// <summary>Gets the SQLite pragmas for the store.</summary>
		///         <value>The SQLite pragmas for the store.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sqlitePragmas", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> SqlitePragmas { get; }

		/// <param name="value">
		///           <para>The value to set.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="name">To be added.</param>
		///         <summary>Sets the value for the specified option <paramref name="name" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setValue:forPragmaNamed:")]
		void SetValue ([NullAllowed] NSObject value, string name);

		/// <summary>Gets or sets a value that controls whether the store is added on the calling thread or on a background thread.</summary>
		///         <value>
		///           <see langword="false" />, the default value, to load the store on the main thread. <see langword="true" /> to load the store on a background thread.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldAddStoreAsynchronously")]
		bool ShouldAddStoreAsynchronously { get; set; }

		/// <summary>Gets or sets a value that controls whether the persistent store should be automatically migrated.</summary>
		///         <value>
		///           <see langword="true" /> to indicate that the store should be migrated. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldMigrateStoreAutomatically")]
		bool ShouldMigrateStoreAutomatically { get; set; }

		/// <summary>Gets or sets a value that controls whether the coordinator will attempt to create a mapping model by inspecting the schemas to be mapped.</summary>
		///         <value>
		///           <see langword="true" /> to indicate that the coordinator should attempt to create a mapping model. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldInferMappingModelAutomatically")]
		bool ShouldInferMappingModelAutomatically { get; set; }

		/// <param name="url">The url for the persistent store.</param>
		/// <summary>Creates a persistent store description with the specified store URL.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url);

		// NSPersistentStoreDescription_NSPersistentCloudKitContainerAdditions category
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("cloudKitContainerOptions", ArgumentSemantic.Strong)]
		NSPersistentCloudKitContainerOptions CloudKitContainerOptions { get; set; }
	}

	/// <summary>Creates and manages a Core Data stack.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSPersistentContainer">Apple documentation for <c>NSPersistentContainer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSPersistentContainer {
		/// <param name="name">The name of the persistent container to create.</param>
		///         <summary>Creates a persistent container with the specified name.</summary>
		///         <returns>A persistent container with the specified name.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("persistentContainerWithName:")]
		NSPersistentContainer GetPersistentContainer (string name);

		/// <param name="name">The name of the persistent container to create.</param>
		///         <param name="model">The object model for the persistent container to create.</param>
		///         <summary>Creates a persistent container with the specified name and object model.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("persistentContainerWithName:managedObjectModel:")]
		NSPersistentContainer GetPersistentContainer (string name, NSManagedObjectModel model);

		/// <summary>Gets the platform's default directory for persistent stores.</summary>
		///         <value>The platform's default directory for persistent stores..</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultDirectoryURL")]
		NSUrl DefaultDirectoryUrl { get; }

		/// <summary>Gets the name of the persistent container.</summary>
		///         <value>The name of the persistent container.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; }

		/// <summary>Gets the managed object context for the main queue.</summary>
		///         <value>The managed object context for the main queue.</value>
		///         <remarks>To be added.</remarks>
		[Export ("viewContext", ArgumentSemantic.Strong)]
		NSManagedObjectContext ViewContext { get; }

		/// <summary>Gets the managed object model for the persistent container.</summary>
		///         <value>The managed object model for the persistent container.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectModel", ArgumentSemantic.Strong)]
		NSManagedObjectModel ManagedObjectModel { get; }

		/// <summary>Gets the coordinator for this persistent container.</summary>
		///         <value>The coordinator for this persistent container.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentStoreCoordinator", ArgumentSemantic.Strong)]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; }

		/// <summary>Gets an array that contains the descriptions of the persistent stores in this persistent container.</summary>
		///         <value>An array that contains the descriptions of the persistent stores in this persistent container.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentStoreDescriptions", ArgumentSemantic.Copy)]
		NSPersistentStoreDescription [] PersistentStoreDescriptions { get; set; }

		/// <param name="name">The name for the Core Data stack manager.</param>
		/// <summary>Creates a new Core Data stack manager with the specified name.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithName:")]
		NativeHandle Constructor (string name);

		/// <param name="name">The name for the Core Data stack manager.</param>
		/// <param name="model">The managed object model to use.</param>
		/// <summary>Creates a new Core Data stack manager with the specified name and managed object model.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithName:managedObjectModel:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, NSManagedObjectModel model);

		/// <param name="block">A completion handler that takes a <see cref="T:CoreData.NSPersistentStoreDescription" /> and an error in which to store any errors that were encountered while attempting to load the stores.</param>
		///         <summary>Loads the persistent stores in the container and runs a completion handler when finished.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("loadPersistentStoresWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Loads the persistent stores in the container and runs a completion handler when finished.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous LoadPersistentStores operation.  The value of the TResult parameter is of type System.Action&lt;CoreData.NSPersistentStoreDescription,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The LoadPersistentStoresAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void LoadPersistentStores (Action<NSPersistentStoreDescription, NSError> block);

		/// <summary>Creates and returns a private managed object context.</summary>
		///         <value>A private managed object context.</value>
		///         <remarks>To be added.</remarks>
		[Export ("newBackgroundContext")]
		NSManagedObjectContext NewBackgroundContext { get; }

		/// <param name="block">The code to perform.</param>
		///         <summary>Performs background task that is represented by <paramref name="block" /> on the managed object context that was passed to it.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("performBackgroundTask:")]
		void Perform (Action<NSManagedObjectContext> block);
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // iOS8 -> Core Data: warning: client failed to call designated initializer on NSPersistentStoreCoordinator
	partial interface NSPersistentStoreCoordinator
#if !TVOS
		: NSLocking
#endif // !TVOS
	{

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("registeredStoreTypes", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSDictionary<NSString, NSValue> RegisteredStoreTypes { get; }
#else
		NSDictionary RegisteredStoreTypes { get; }
#endif

		/// <param name="storeClass">To be added.</param>
		///         <param name="storeType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static, Export ("registerStoreClass:forStoreType:")]
		void RegisterStoreClass ([NullAllowed] Class storeClass, NSString storeType);

		/// <param name="storeType">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use the method that takes an out NSError parameter.")]
		[Deprecated (PlatformName.TvOS, 9, 0, message: "Use the method that takes an out NSError parameter.")]
		[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use the method that takes an out NSError parameter.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the method that takes an out NSError parameter.")]
		[Static, Export ("metadataForPersistentStoreOfType:URL:error:")]
		[return: NullAllowed]
		NSDictionary MetadataForPersistentStoreOfType ([NullAllowed] NSString storeType, NSUrl url, out NSError error);

		/// <param name="storeType">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>Gets the metadata for the store at a URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("metadataForPersistentStoreOfType:URL:options:error:")]
		[return: NullAllowed]
		NSDictionary<NSString, NSObject> GetMetadata (string storeType, NSUrl url, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="metadata">To be added.</param>
		///         <param name="storeType">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Sets the metadata for a persistent store at a URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use the method that takes an 'out NSError' parameter.")]
		[Deprecated (PlatformName.TvOS, 9, 0, message: "Use the method that takes an 'out NSError' parameter.")]
		[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use the method that takes an 'out NSError' parameter.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the method that takes an 'out NSError' parameter.")]
		[Static, Export ("setMetadata:forPersistentStoreOfType:URL:error:")]
		bool SetMetadata ([NullAllowed] NSDictionary metadata, [NullAllowed] NSString storeType, NSUrl url, out NSError error);

		/// <param name="metadata">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="storeType">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">To be added.</param>
		///         <summary>Sets the metadata for a persistent store at a URL..</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("setMetadata:forPersistentStoreOfType:URL:options:error:")]
		bool SetMetadata ([NullAllowed] NSDictionary<NSString, NSObject> metadata, string storeType, NSUrl url, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="metadata">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="store">To be added.</param>
		///         <summary>Sets the metadata for a persistent store.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setMetadata:forPersistentStore:")]
#if XAMCORE_5_0
		void SetMetadata ([NullAllowed] NSDictionary<NSString, NSObject> metadata, NSPersistentStore store);
#else
		void SetMetadata ([NullAllowed] NSDictionary metadata, NSPersistentStore store);
#endif

		[Export ("metadataForPersistentStore:")]
#if XAMCORE_5_0
		NSDictionary<NSString, NSObject> GetMetadata (NSPersistentStore store);
#elif NET
		NSDictionary GetMetadata (NSPersistentStore store);
#else
		NSDictionary MetadataForPersistentStore (NSPersistentStore store);
#endif

		[DesignatedInitializer]
		[Export ("initWithManagedObjectModel:")]
		NativeHandle Constructor (NSManagedObjectModel model);

		/// <summary>Gets the managed objec model for the receiver.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectModel", ArgumentSemantic.Strong)]
		NSManagedObjectModel ManagedObjectModel { get; }

		/// <summary>Gets an array that contains the receiver's persistent stores.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentStores", ArgumentSemantic.Strong)]
		NSPersistentStore [] PersistentStores { get; }

		/// <param name="url">To be added.</param>
		///         <summary>Returns the persistent store at <paramref name="url" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("persistentStoreForURL:")]
		[return: NullAllowed]
		NSPersistentStore PersistentStoreForUrl (NSUrl url);

		/// <param name="store">To be added.</param>
		///         <summary>Returns the URL for the specified <paramref name="store" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("URLForPersistentStore:")]
		NSUrl UrlForPersistentStore (NSPersistentStore store);

		/// <param name="url">To be added.</param>
		///         <param name="store">To be added.</param>
		///         <summary>Sets the URL for the specfied <paramref name="store" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setURL:forPersistentStore:")]
		bool SetUrl (NSUrl url, NSPersistentStore store);

		[Export ("addPersistentStoreWithType:configuration:URL:options:error:")]
		[return: NullAllowed]
#if NET
		NSPersistentStore AddPersistentStore (NSString storeType, [NullAllowed] string configuration, [NullAllowed] NSUrl storeUrl, [NullAllowed] NSDictionary options, out NSError error);
#else
		NSPersistentStore AddPersistentStoreWithType (NSString storeType, [NullAllowed] string configuration, [NullAllowed] NSUrl storeUrl, [NullAllowed] NSDictionary options, out NSError error);
#endif

		/// <param name="storeDescription">To be added.</param>
		///         <param name="block">To be added.</param>
		///         <summary>Adds the described persistent store and runs a handler when it is complete.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("addPersistentStoreWithDescription:completionHandler:")]
		[Async]
		void AddPersistentStore (NSPersistentStoreDescription storeDescription, Action<NSPersistentStoreDescription, NSError> block);

		/// <param name="store">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Removes the specified <paramref name="store" /> and reports any errors that are encountered.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("removePersistentStore:error:")]
		bool RemovePersistentStore (NSPersistentStore store, out NSError error);

		/// <param name="store">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="storeType">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Migrates <paramref name="store" /> to <paramref name="url" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("migratePersistentStore:toURL:options:withType:error:")]
		[return: NullAllowed]
		NSPersistentStore MigratePersistentStore (NSPersistentStore store, NSUrl url, [NullAllowed] NSDictionary options, NSString storeType, out NSError error);

		/// <param name="url">To be added.</param>
		///         <summary>Returns a managed object id for the specified <paramref name="url" /> if a store that matches the URL can be found.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectIDForURIRepresentation:")]
		[return: NullAllowed]
		NSManagedObjectID ManagedObjectIDForURIRepresentation (NSUrl url);

#pragma warning disable 0109 // warning CS0109: The member 'NSManagedObjectContext.Lock()' does not hide an accessible member. The new keyword is not required.
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'PerformAndWait' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use 'PerformAndWait' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PerformAndWait' instead.")]
		[Export ("lock")]
		new void Lock ();
#pragma warning restore

#pragma warning disable 0109 // warning CS0109: The member 'NSManagedObjectContext.Unlock()' does not hide an accessible member. The new keyword is not required.
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'PerformAndWait' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use 'PerformAndWait' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PerformAndWait' instead.")]
		[Export ("unlock")]
		new void Unlock ();
#pragma warning restore

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'Perform' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use 'Perform' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Perform' instead.")]
		[Export ("tryLock")]
		bool TryLock { get; }

		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Deprecated (PlatformName.MacOSX, 10, 5)]
		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		[return: NullAllowed]
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);

		/// <summary>Represents the value associated with the constant NSSQLiteStoreType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSSQLiteStoreType")]
		NSString SQLiteStoreType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("NSXMLStoreType")]
		NSString XMLStoreType { get; }

		/// <summary>Represents the value associated with the constant NSBinaryStoreType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSBinaryStoreType")]
		NSString BinaryStoreType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("NSBinaryStoreSecureDecodingClasses")]
		NSString BinaryStoreSecureDecodingClasses { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("NSBinaryStoreInsecureDecodingCompatibilityOption")]
		NSString BinaryStoreInsecureDecodingCompatibilityOption { get; }

		/// <summary>Represents the value associated with the constant NSInMemoryStoreType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSInMemoryStoreType")]
		NSString InMemoryStoreType { get; }

		/// <summary>Represents the value associated with the constant NSStoreUUIDKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSStoreUUIDKey")]
		NSString StoreUUIDKey { get; }

		/// <summary>Represents the value associated with the constant NSAddedPersistentStoresKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSAddedPersistentStoresKey")]
		NSString AddedPersistentStoresKey { get; }

		/// <summary>Represents the value associated with the constant NSRemovedPersistentStoresKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSRemovedPersistentStoresKey")]
		NSString RemovedPersistentStoresKey { get; }

		/// <summary>Represents the value associated with the constant NSUUIDChangedPersistentStoresKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSUUIDChangedPersistentStoresKey")]
		NSString UUIDChangedPersistentStoresKey { get; }

		/// <summary>Represents the value associated with the constant NSReadOnlyPersistentStoreOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSReadOnlyPersistentStoreOption")]
		NSString ReadOnlyPersistentStoreOption { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("NSValidateXMLStoreOption")]
		NSString ValidateXMLStoreOption { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreTimeoutOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSPersistentStoreTimeoutOption")]
		NSString PersistentStoreTimeoutOption { get; }

		/// <summary>Represents the value associated with the constant NSSQLitePragmasOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSSQLitePragmasOption")]
		NSString SQLitePragmasOption { get; }

		/// <summary>Represents the value associated with the constant NSSQLiteAnalyzeOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSSQLiteAnalyzeOption")]
		NSString SQLiteAnalyzeOption { get; }

		/// <summary>Represents the value associated with the constant NSSQLiteManualVacuumOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSSQLiteManualVacuumOption")]
		NSString SQLiteManualVacuumOption { get; }

		/// <summary>Represents the value associated with the constant NSIgnorePersistentStoreVersioningOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSIgnorePersistentStoreVersioningOption")]
		NSString IgnorePersistentStoreVersioningOption { get; }

		/// <summary>Represents the value associated with the constant NSMigratePersistentStoresAutomaticallyOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSMigratePersistentStoresAutomaticallyOption")]
		NSString MigratePersistentStoresAutomaticallyOption { get; }

		/// <summary>Represents the value associated with the constant NSInferMappingModelAutomaticallyOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSInferMappingModelAutomaticallyOption")]
		NSString InferMappingModelAutomaticallyOption { get; }

		/// <summary>Represents the value associated with the constant NSStoreModelVersionHashesKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSStoreModelVersionHashesKey")]
		NSString StoreModelVersionHashesKey { get; }

		/// <summary>Represents the value associated with the constant NSStoreModelVersionIdentifiersKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSStoreModelVersionIdentifiersKey")]
		NSString StoreModelVersionIdentifiersKey { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreOSCompatibility</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSPersistentStoreOSCompatibility")]
		NSString PersistentStoreOSCompatibility { get; }

		/// <summary>Represents the value associated with the constant NSStoreTypeKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("NSStoreTypeKey")]
		NSString StoreTypeKey { get; }

		/// <include file="../docs/api/CoreData/NSPersistentStoreCoordinator.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSPersistentStoreCoordinator.StoresDidChangeNotification']/*" />
		[Notification]
		[Field ("NSPersistentStoreCoordinatorStoresDidChangeNotification")]
		NSString StoresDidChangeNotification { get; }

		/// <include file="../docs/api/CoreData/NSPersistentStoreCoordinator.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSPersistentStoreCoordinator.WillRemoveStoreNotification']/*" />
		[Notification]
		[Field ("NSPersistentStoreCoordinatorWillRemoveStoreNotification")]
		NSString WillRemoveStoreNotification { get; }

		// 5.0
		[Export ("executeRequest:withContext:error:")]
		[return: NullAllowed]
#if NET
		NSObject Execute (NSPersistentStoreRequest request, NSManagedObjectContext context, out NSError error);
#else
		NSObject ExecuteRequestwithContexterror (NSPersistentStoreRequest request, NSManagedObjectContext context, out NSError error);
#endif

		/// <include file="../docs/api/CoreData/NSPersistentStoreCoordinator.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSPersistentStoreCoordinator.DidImportUbiquitousContentChangesNotification']/*" />
		[NoTV]
		[Notification]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
		[Field ("NSPersistentStoreDidImportUbiquitousContentChangesNotification")]
		NSString DidImportUbiquitousContentChangesNotification { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreUbiquitousContentNameKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
		[Field ("NSPersistentStoreUbiquitousContentNameKey")]
		NSString PersistentStoreUbiquitousContentNameKey { get; }

		[NoTV]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
		[Field ("NSPersistentStoreUbiquitousContentURLKey")]
#if NET
		NSString PersistentStoreUbiquitousContentUrlKey { get; }
#else
		NSString PersistentStoreUbiquitousContentUrlLKey { get; }
#endif

		/// <summary>Represents the value associated with the constant NSPersistentStoreFileProtectionKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreFileProtectionKey")]
		NSString PersistentStoreFileProtectionKey { get; }

		// 7.0

		/// <summary>Represents the value associated with the constant NSPersistentStoreUbiquitousPeerTokenOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreUbiquitousPeerTokenOption")]
		NSString PersistentStoreUbiquitousPeerTokenOption { get; }

		/// <param name="storeUrl">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Static]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Please see the release notes and Core Data documentation.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
		[Export ("removeUbiquitousContentAndPersistentStoreAtURL:options:error:")]
		bool RemoveUbiquitousContentAndPersistentStore (NSUrl storeUrl, [NullAllowed] NSDictionary options, out NSError error);

		/// <include file="../docs/api/CoreData/NSPersistentStoreCoordinator.xml" path="/Documentation/Docs[@DocId='P:CoreData.NSPersistentStoreCoordinator.StoresWillChangeNotification']/*" />
		[MacCatalyst (13, 1)]
		[Notification (typeof (NSPersistentStoreCoordinatorStoreChangeEventArgs))]
		[Field ("NSPersistentStoreCoordinatorStoresWillChangeNotification")]
		NSString StoresWillChangeNotification { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreRebuildFromUbiquitousContentOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreRebuildFromUbiquitousContentOption")]
		NSString RebuildFromUbiquitousContentOption { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreRemoveUbiquitousMetadataOption</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreRemoveUbiquitousMetadataOption")]
		NSString RemoveUbiquitousMetadataOption { get; }

		/// <summary>Represents the value associated with the constant NSPersistentStoreUbiquitousContainerIdentifierKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreUbiquitousContainerIdentifierKey")]
		[Obsolete ("Use 'UbiquitousContainerIdentifierKey' instead.")]
		NSString eUbiquitousContainerIdentifierKey { get; }

		/// <summary>Represents the value that is associated with the NSPersistentStoreUbiquitousContainerIdentifierKey constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentStoreUbiquitousContainerIdentifierKey")]
		NSString UbiquitousContainerIdentifierKey { get; }

		// 11.0

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("NSCoreDataCoreSpotlightExporter")]
		NSString CoreSpotlightExporter { get; }

		/// <summary>Represents the value that is associated with the NSPersistentHistoryTrackingKey constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("NSPersistentHistoryTrackingKey")]
		NSString HistoryTrackingKey { get; }

		/// <summary>Gets the name of the persistent store coordinator.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("name")]
		string Name { get; set; }

		/// <param name="code">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("performBlock:")]
		void Perform (Action code);

		/// <param name="code">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("performBlockAndWait:")]
		void PerformAndWait (Action code);

		/// <param name="url">To be added.</param>
		///         <param name="storeType">To be added.</param>
		///         <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Destroys the persistent store that is located at the specified <paramref name="url" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("destroyPersistentStoreAtURL:withType:options:error:")]
		bool DestroyPersistentStore (NSUrl url, string storeType, [NullAllowed] NSDictionary options, out NSError error);

		/// <param name="destinationUrl">To be added.</param>
		///         <param name="destinationOptions">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="sourceUrl">To be added.</param>
		///         <param name="sourceOptions">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="storeType">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Replaces the persistent store at <paramref name="destinationUrl" /> with the one at <paramref name="sourceUrl" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("replacePersistentStoreAtURL:destinationOptions:withPersistentStoreFromURL:sourceOptions:storeType:error:")]
		bool ReplacePersistentStore (NSUrl destinationUrl, [NullAllowed] NSDictionary destinationOptions, NSUrl sourceUrl, [NullAllowed] NSDictionary sourceOptions, string storeType, out NSError error);


		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("currentPersistentHistoryTokenFromStores:")]
		[return: NullAllowed]
		NSPersistentHistoryToken GetCurrentPersistentHistoryToken ([NullAllowed] NSObject [] stores);

		[TV (14, 0), iOS (14, 0), MacCatalyst (14, 0)]
		[Export ("finishDeferredLightweightMigration:")]
		bool FinishDeferredLightweightMigration ([NullAllowed] out NSError error);

		[TV (14, 0), iOS (14, 0), MacCatalyst (14, 0)]
		[Export ("finishDeferredLightweightMigrationTask:")]
		bool FinishDeferredLightweightMigrationTask ([NullAllowed] out NSError error);

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("managedObjectIDFromUTF8String:length:")]
		NSManagedObjectID GetManagedObjectId (IntPtr utf8String, nuint length);
	}

	interface NSPersistentStoreCoordinatorStoreChangeEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Export ("NSPersistentStoreUbiquitousTransitionTypeKey")]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Please see the release notes and Core Data documentation.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Please see the release notes and Core Data documentation.")]
		NSPersistentStoreUbiquitousTransitionType EventType { get; }
	}

	/// <summary>Criteria used to retrieve data from or save data in a persistent store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSPersistentStoreRequest_Class/index.html">Apple documentation for <c>NSPersistentStoreRequest</c></related>
	[BaseType (typeof (NSObject))]
	interface NSPersistentStoreRequest : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requestType")]
		NSPersistentStoreRequestType RequestType { get; }

		//Detected properties
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("affectedStores", ArgumentSemantic.Retain)]
		NSPersistentStore [] AffectedStores { get; set; }
	}

	/// <summary>Class that represents the result of an asynchronous fetch request.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSAsynchronousFetchResult">Apple documentation for <c>NSAsynchronousFetchResult</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreAsynchronousResult))]
	interface NSAsynchronousFetchResult {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchRequest", ArgumentSemantic.Retain)]
		NSAsynchronousFetchRequest FetchRequest { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("finalResult", ArgumentSemantic.Retain)]
		[NullAllowed]
#if NET
		INSFetchRequestResult [] FinalResult { get; }
#else
		NSObject [] FinalResult { get; }
#endif
	}

	/// <summary>Class that represents the result of request that was made of a persistent data store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSPersistentStoreResult">Apple documentation for <c>NSPersistentStoreResult</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSPersistentStoreResult {

	}

	/// <summary>Class that represents the result of an batch update request.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSBatchUpdateResult">Apple documentation for <c>NSBatchUpdateResult</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreResult))]
	interface NSBatchUpdateResult {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("result", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSObject Result { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType")]
		NSBatchUpdateRequestResultType ResultType { get; }
	}

	/// <summary>Class that represents the results of an aysnchronous request that was made of a persistent data store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSPersistentStoreAsynchronousResult">Apple documentation for <c>NSPersistentStoreAsynchronousResult</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreResult))]
	interface NSPersistentStoreAsynchronousResult {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("managedObjectContext", ArgumentSemantic.Retain)]
		NSManagedObjectContext ManagedObjectContext { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("operationError", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSError OperationError { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("progress", ArgumentSemantic.Retain)]
		[NullAllowed]
		NSProgress Progress { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancel")]
		void Cancel ();
	}

	/// <summary>Class that represents an asynchronous fetch request.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSAsynchronousFetchRequest">Apple documentation for <c>NSAsynchronousFetchRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSAsynchronousFetchRequest {
		/// <param name="request">To be added.</param>
		/// <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFetchRequest:completionBlock:")]
		NativeHandle Constructor (NSFetchRequest request, [NullAllowed] Action<NSAsynchronousFetchResult> completion);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchRequest", ArgumentSemantic.Retain)]
		NSFetchRequest FetchRequest { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("estimatedResultCount")]
		nint EstimatedResultCount { get; set; }
	}

	/// <summary>Defines properties of an entity in a managed objectmodel. The equivalent of an instance property.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSPropertyDescription_Class/index.html">Apple documentation for <c>NSPropertyDescription</c></related>
	[BaseType (typeof (NSObject))]
	interface NSPropertyDescription : NSCoding, NSCopying {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entity")]
		NSEntityDescription Entity { get; }

		// by default this property is null, but docs mention possible 
		// exception on setter so allowing a null is not a good idea
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("optional")]
		bool Optional { [Bind ("isOptional")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transient")]
		bool Transient { [Bind ("isTransient")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("validationPredicates")]
		NSPredicate [] ValidationPredicates { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("validationWarnings")]
		string [] ValidationWarnings { get; }

		/// <param name="validationPredicates">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="validationWarnings">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setValidationPredicates:withValidationWarnings:")]
		void SetValidationPredicates ([NullAllowed] NSPredicate [] validationPredicates, [NullAllowed] string [] validationWarnings);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Retain)]
		NSDictionary UserInfo { get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'NSEntityDescription.Indexes' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexed")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'NSEntityDescription.Indexes' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSEntityDescription.Indexes' instead.")]
		bool Indexed { [Bind ("isIndexed")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("versionHash")]
		NSData VersionHash { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }

		/// <summary>Gets the receiver's renaming identifier.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renamingIdentifier")]
		[NullAllowed]
		string RenamingIdentifier { get; set; }

		// 5.0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexedBySpotlight")]
		bool IndexedBySpotlight { [Bind ("isIndexedBySpotlight")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("storedInExternalRecord")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CoreSpotlight' integration instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CoreSpotlight' integration instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'CoreSpotlight' integration instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CoreSpotlight' integration instead.")]
		bool StoredInExternalRecord { [Bind ("isStoredInExternalRecord")] get; set; }
	}

	/// <summary>Maps a property between source and destination entities.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/NSPropertyMapping_class/index.html">Apple documentation for <c>NSPropertyMapping</c></related>
	[BaseType (typeof (NSObject))]
	interface NSPropertyMapping {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("name")]
		string Name { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("valueExpression", ArgumentSemantic.Retain)]
		NSExpression ValueExpression { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("userInfo", ArgumentSemantic.Retain)]
		NSDictionary UserInfo { get; set; }
	}

	/// <summary>Describes the relationships of a <see cref="T:CoreData.NSEntityDescription" /> object.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSRelationshipDescription_Class/index.html">Apple documentation for <c>NSRelationshipDescription</c></related>
	[BaseType (typeof (NSPropertyDescription))]
	interface NSRelationshipDescription {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("destinationEntity")]
		NSEntityDescription DestinationEntity { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("inverseRelationship")]
		NSRelationshipDescription InverseRelationship { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maxCount")]
		nuint MaxCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minCount")]
		nuint MinCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("deleteRule")]
		NSDeleteRule DeleteRule { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("toMany")]
		bool IsToMany { [Bind ("isToMany")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("versionHash")]
		NSData VersionHash { get; }

		// 5.0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ordered")]
		bool Ordered { [Bind ("isOrdered")] get; set; }
	}

	/// <summary>A collection of changes to be made by an object store.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreData/Reference/NSSaveChangesRequest_Class/index.html">Apple documentation for <c>NSSaveChangesRequest</c></related>
	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSSaveChangesRequest {
		/// <param name="insertedObjects">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="updatedObjects">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="deletedObjects">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="lockedObjects">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithInsertedObjects:updatedObjects:deletedObjects:lockedObjects:")]
#if XAMCORE_5_0
		NativeHandle Constructor ([NullAllowed] NSSet<NSManagedObject> insertedObjects, [NullAllowed] NSSet<NSManagedObject> updatedObjects, [NullAllowed] NSSet<NSManagedObject> deletedObjects, [NullAllowed] NSSet<NSManagedObject> lockedObjects);
#else
		NativeHandle Constructor ([NullAllowed] NSSet insertedObjects, [NullAllowed] NSSet updatedObjects, [NullAllowed] NSSet deletedObjects, [NullAllowed] NSSet lockedObjects);
#endif

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("insertedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> InsertedObjects { get; }
#else
		NSSet InsertedObjects { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("updatedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> UpdatedObjects { get; }
#else
		NSSet UpdatedObjects { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("deletedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> DeletedObjects { get; }
#else
		NSSet DeletedObjects { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("lockedObjects", ArgumentSemantic.Strong)]
#if XAMCORE_5_0
		NSSet<NSManagedObject> LockedObjects { get; }
#else
		NSSet LockedObjects { get; }
#endif
	}

	/// <summary>Class that represents a request for a batch update.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSBatchUpdateRequest">Apple documentation for <c>NSBatchUpdateRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSBatchUpdateRequest {
		/// <param name="entityName">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithEntityName:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string entityName);

		/// <param name="entity">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithEntity:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSEntityDescription entity);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entityName")]
		string EntityName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("entity", ArgumentSemantic.Retain)]
		NSEntityDescription Entity { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("predicate", ArgumentSemantic.Retain)]
		NSPredicate Predicate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includesSubentities")]
		bool IncludesSubentities { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType", ArgumentSemantic.Assign)]
		NSBatchUpdateRequestResultType ResultType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("propertiesToUpdate", ArgumentSemantic.Copy)]
		NSDictionary PropertiesToUpdate { get; set; }

		/// <param name="entityName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("batchUpdateRequestWithEntityName:")]
		NSBatchUpdateRequest BatchUpdateRequestWithEntityName (string entityName);
	}

	/// <summary>A <see cref="T:CoreData.NSPersistentStoreRequest" /> that performs a batch delete.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSBatchDeleteRequest">Apple documentation for <c>NSBatchDeleteRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	[DisableDefaultCtor]
	interface NSBatchDeleteRequest {
		/// <param name="fetch">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFetchRequest:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSFetchRequest fetch);

		/// <param name="objects">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithObjectIDs:")]
		NativeHandle Constructor (NSManagedObjectID [] objects);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType", ArgumentSemantic.Assign)]
		NSBatchDeleteRequestResultType ResultType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fetchRequest", ArgumentSemantic.Copy)]
		NSFetchRequest FetchRequest { get; }
	}

	/// <summary>The results of a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Data%20NSBatch%20Delete%20Query&amp;scope=Xamarin" title="T:CoreData.NSBatchDeleteQuery">T:CoreData.NSBatchDeleteQuery</a></format>.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSBatchDeleteResult">Apple documentation for <c>NSBatchDeleteResult</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreResult))]
	interface NSBatchDeleteResult {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("result", ArgumentSemantic.Strong)]
		NSObject Result { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resultType")]
		NSBatchDeleteRequestResultType ResultType { get; }
	}

	/// <summary>To be added.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreData/NSConstraintConflict">Apple documentation for <c>NSConstraintConflict</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NSConstraintConflict {
		/// <param name="contraint">To be added.</param>
		/// <param name="databaseObject">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="databaseSnapshot">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="conflictingObjects">To be added.</param>
		/// <param name="conflictingSnapshots">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithConstraint:databaseObject:databaseSnapshot:conflictingObjects:conflictingSnapshots:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string [] contraint, [NullAllowed] NSManagedObject databaseObject, [NullAllowed] NSDictionary databaseSnapshot, NSManagedObject [] conflictingObjects, NSObject [] conflictingSnapshots);

#if MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("constraint", ArgumentSemantic.Copy)]
#else
		[Export ("constraint", ArgumentSemantic.Retain)]
#endif
		string [] Constraint { get; }

#if MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("constraintValues", ArgumentSemantic.Copy)]
#else
		[Export ("constraintValues", ArgumentSemantic.Retain)]
#endif
		NSDictionary<NSString, NSObject> ConstraintValues { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("databaseObject", ArgumentSemantic.Retain)]
		NSManagedObject DatabaseObject { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("databaseSnapshot", ArgumentSemantic.Retain)]
		NSDictionary<NSString, NSObject> DatabaseSnapshot { get; }

#if MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("conflictingObjects", ArgumentSemantic.Copy)]
#else
		[Export ("conflictingObjects", ArgumentSemantic.Retain)]
#endif
		NSManagedObject [] ConflictingObjects { get; }

#if MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("conflictingSnapshots", ArgumentSemantic.Copy)]
#else
		[Export ("conflictingSnapshots", ArgumentSemantic.Retain)]
#endif
		NSDictionary [] ConflictingSnapshots { get; }
	}

#if XAMCORE_5_0
	delegate bool NSBatchInsertRequestDictionaryHandler (NSMutableDictionary<NSString, NSObject> dictionary);
#else
	delegate bool NSBatchInsertRequestDictionaryHandler (NSMutableDictionary dictionary);
#endif
	delegate bool NSBatchInsertRequestManagedObjectHandler (NSManagedObject managedObject);

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	[DisableDefaultCtor] // NSInternalInconsistencyException Reason: -init results in undefined behavior for NSBatchInsertRequest
	interface NSBatchInsertRequest {

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use another constructor instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use another constructor instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 16, message: "Use another constructor instead.")]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use another constructor instead.")]
		[Export ("init")]
		NativeHandle Constructor ();

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithEntity:dictionaryHandler:")]
		NativeHandle Constructor (NSEntityDescription entity, NSBatchInsertRequestDictionaryHandler handler);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithEntity:managedObjectHandler:")]
		NativeHandle Constructor (NSEntityDescription entity, NSBatchInsertRequestManagedObjectHandler handler);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithEntityName:dictionaryHandler:")]
		NativeHandle Constructor (string entityName, NSBatchInsertRequestDictionaryHandler handler);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithEntityName:managedObjectHandler:")]
		NativeHandle Constructor (string entityName, NSBatchInsertRequestManagedObjectHandler handler);

		[Export ("entityName")]
		string EntityName { get; }

		[NullAllowed, Export ("entity", ArgumentSemantic.Strong)]
		NSEntityDescription Entity { get; }

		[NullAllowed, Export ("objectsToInsert", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> [] ObjectsToInsert { get; set; }

		[Export ("resultType", ArgumentSemantic.Assign)]
		NSBatchInsertRequestResultType ResultType { get; set; }

		[Static]
		[Export ("batchInsertRequestWithEntityName:objects:")]
		NSBatchInsertRequest BatchInsertRequest (string entityName, NSDictionary<NSString, NSObject> [] dictionaries);

		[DesignatedInitializer]
		[Export ("initWithEntityName:objects:")]
		NativeHandle Constructor (string entityName, NSDictionary<NSString, NSObject> [] dictionaries);

		[DesignatedInitializer]
		[Export ("initWithEntity:objects:")]
		NativeHandle Constructor (NSEntityDescription entity, NSDictionary<NSString, NSObject> [] dictionaries);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("dictionaryHandler", ArgumentSemantic.Copy)]
		NSBatchInsertRequestDictionaryHandler DictionaryHandler { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("managedObjectHandler", ArgumentSemantic.Copy)]
		NSBatchInsertRequestManagedObjectHandler ManagedObjectHandler { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("batchInsertRequestWithEntityName:dictionaryHandler:")]
		NSBatchInsertRequest CreateBatchInsertRequest (string entityName, NSBatchInsertRequestDictionaryHandler handler);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("batchInsertRequestWithEntityName:managedObjectHandler:")]
		NSBatchInsertRequest CreateBatchInsertRequest (string entityName, NSBatchInsertRequestManagedObjectHandler handler);
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentStoreResult))]
	interface NSBatchInsertResult {
		[NullAllowed, Export ("result", ArgumentSemantic.Strong)]
		NSObject Result { get; }

		[Export ("resultType")]
		NSBatchInsertRequestResultType ResultType { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSAttributeDescription))]
	interface NSDerivedAttributeDescription : NSSecureCoding {
		[NullAllowed, Export ("derivationExpression", ArgumentSemantic.Strong)]
		NSExpression DerivationExpression { get; set; }
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void NSPersistentCloudKitContainerShareManagedObjectsHandler (NSSet<NSManagedObjectID> sharedObjectIds, CKShare share, CKContainer container, NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void NSPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosHandler (NSArray<CKShareParticipant> fetchedParticipants, NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void NSPersistentCloudKitContainerPersistUpdatedShareHandler (CKShare persistedShare, NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void NSPersistentCloudKitContainerPurgeObjectsAndRecordsInZoneHandler (CKRecordZoneID purgedZoneId, NSError error);

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	delegate void NSPersistentCloudKitContainerAcceptShareInvitationsHandler (NSArray<CKShareMetadata> acceptedShareMetadatas, NSError error);

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSPersistentContainer))]
	[DisableDefaultCtor]
	interface NSPersistentCloudKitContainer {

		[Export ("initWithName:managedObjectModel:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, NSManagedObjectModel model);

		[Export ("initializeCloudKitSchemaWithOptions:error:")]
		bool Initialize (NSPersistentCloudKitContainerSchemaInitializationOptions options, [NullAllowed] out NSError error);

		[Export ("recordForManagedObjectID:")]
		[return: NullAllowed]
		CKRecord GetRecord (NSManagedObjectID managedObjectId);

		[Export ("recordsForManagedObjectIDs:")]
		NSDictionary<NSManagedObjectID, CKRecord> GetRecords (NSManagedObjectID [] managedObjectIds);

		[Export ("recordIDForManagedObjectID:")]
		[return: NullAllowed]
		CKRecordID GetRecordId (NSManagedObjectID managedObjectId);

		[Export ("recordIDsForManagedObjectIDs:")]
		NSDictionary<NSManagedObjectID, CKRecordID> GetRecordIds (NSManagedObjectID [] managedObjectIds);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("canUpdateRecordForManagedObjectWithID:")]
		bool CanUpdateRecord (NSManagedObjectID objectID);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("canDeleteRecordForManagedObjectWithID:")]
		bool CanDeleteRecord (NSManagedObjectID objectID);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("canModifyManagedObjectsInStore:")]
		bool CanModifyManagedObjects (NSPersistentStore store);

		// NSPersistentCloudKitContainer_Sharing
		[Async (ResultTypeName = "NSPersistentCloudKitContainerAcceptShareInvitationsResult")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("acceptShareInvitationsFromMetadata:intoPersistentStore:completion:")]
		void AcceptShareInvitations (CKShareMetadata [] metadata, NSPersistentStore persistentStore, [NullAllowed] NSPersistentCloudKitContainerAcceptShareInvitationsHandler handler);

		[Async (ResultTypeName = "NSPersistentCloudKitContainerPurgeObjectsAndRecordsInZone")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("purgeObjectsAndRecordsInZoneWithID:inPersistentStore:completion:")]
		void PurgeObjectsAndRecordsInZone (CKRecordZoneID zoneId, [NullAllowed] NSPersistentStore persistentStore, [NullAllowed] NSPersistentCloudKitContainerPurgeObjectsAndRecordsInZoneHandler handler);

		[Async (ResultTypeName = "NSPersistentCloudKitContainerPersistUpdatedShareResult")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("persistUpdatedShare:inPersistentStore:completion:")]
		void PersistUpdatedShare (CKShare share, NSPersistentStore persistentStore, [NullAllowed] NSPersistentCloudKitContainerPersistUpdatedShareHandler handler);

		[Async (ResultTypeName = "NSPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosResult")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("fetchParticipantsMatchingLookupInfos:intoPersistentStore:completion:")]
		void FetchParticipantsMatchingLookupInfos (CKUserIdentityLookupInfo [] lookupInfos, NSPersistentStore persistentStore, NSPersistentCloudKitContainerFetchParticipantsMatchingLookupInfosHandler handler);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("fetchSharesMatchingObjectIDs:error:")]
		[return: NullAllowed]
		NSDictionary<NSManagedObjectID, CKShare> FetchSharesMatchingObjectIds (NSManagedObjectID [] objectIDs, [NullAllowed] out NSError error);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("fetchSharesInPersistentStore:error:")]
		[return: NullAllowed]
		CKShare [] FetchSharesInPersistentStore ([NullAllowed] NSPersistentStore persistentStore, [NullAllowed] out NSError error);

		[Async (ResultTypeName = "NSPersistentCloudKitContainerShareManagedObjectsResult")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("shareManagedObjects:toShare:completion:")]
		void ShareManagedObjects (NSManagedObject [] managedObjects, [NullAllowed] CKShare share, NSPersistentCloudKitContainerShareManagedObjectsHandler handler);

	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSPersistentCloudKitContainerOptions {
		[Export ("containerIdentifier")]
		string ContainerIdentifier { get; }

		[Export ("initWithContainerIdentifier:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string containerIdentifier);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("databaseScope", ArgumentSemantic.Assign)]
		CKDatabaseScope DatabaseScope { get; set; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSPersistentCloudKitContainerEvent : NSCopying {
		[Export ("identifier", ArgumentSemantic.Strong)]
		NSUuid Identifier { get; }

		[Export ("storeIdentifier", ArgumentSemantic.Strong)]
		string StoreIdentifier { get; }

		[Export ("type")]
		NSPersistentCloudKitContainerEventType Type { get; }

		[Export ("startDate", ArgumentSemantic.Strong)]
		NSDate StartDate { get; }

		[NullAllowed, Export ("endDate", ArgumentSemantic.Strong)]
		NSDate EndDate { get; }

		[Export ("succeeded")]
		bool Succeeded { get; }

		[NullAllowed, Export ("error", ArgumentSemantic.Strong)]
		NSError Error { get; }

		[Notification]
		[Field ("NSPersistentCloudKitContainerEventChangedNotification")]
		NSString ChangedNotification { get; }

		[Field ("NSPersistentCloudKitContainerEventUserInfoKey")]
		NSString UserInfoKey { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSPersistentCloudKitContainerEventRequest {
		[Export ("resultType", ArgumentSemantic.Assign)]
		NSPersistentCloudKitContainerEventResultType ResultType { get; set; }

		[Static]
		[Export ("fetchEventsAfterDate:")]
		NSPersistentCloudKitContainerEventRequest FetchEventsAfter (NSDate date);

		[Static]
		[Export ("fetchEventsAfterEvent:")]
		NSPersistentCloudKitContainerEventRequest FetchEventsAfter ([NullAllowed] NSPersistentCloudKitContainerEvent @event);

		[Static]
		[Export ("fetchEventsMatchingFetchRequest:")]
		NSPersistentCloudKitContainerEventRequest FetchEvents (NSFetchRequest fetchRequest);

		[Static]
		[Export ("fetchRequestForEvents")]
		NSFetchRequest FetchRequest ();
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSPersistentStoreResult))]
	[DisableDefaultCtor]
	interface NSPersistentCloudKitContainerEventResult {
		[NullAllowed, Export ("result", ArgumentSemantic.Strong)]
		NSObject Result { get; }

		[Export ("resultType")]
		NSPersistentCloudKitContainerEventResultType ResultType { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSStagedMigrationManager {
		[Export ("stages", ArgumentSemantic.Strong)]
		NSMigrationStage [] Stages { get; }

		[NullAllowed, Export ("container", ArgumentSemantic.Strong)]
		NSPersistentContainer Container { get; }

		[Export ("initWithMigrationStages:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSMigrationStage [] stages);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface NSMigrationStage {
		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSManagedObjectModelReference {
		[Export ("resolvedModel", ArgumentSemantic.Strong)]
		NSManagedObjectModel ResolvedModel { get; }

		[Export ("versionChecksum", ArgumentSemantic.Strong)]
		string VersionChecksum { get; }

		[Export ("initWithModel:versionChecksum:")]
		NativeHandle Constructor (NSManagedObjectModel model, string versionChecksum);

		[Export ("initWithFileURL:versionChecksum:")]
		NativeHandle Constructor (NSUrl fileUrl, string versionChecksum);

		[Export ("initWithEntityVersionHashes:inBundle:versionChecksum:")]
		NativeHandle Constructor (NSDictionary versionHash, [NullAllowed] NSBundle bundle, string versionChecksum);

		[Export ("initWithName:inBundle:versionChecksum:")]
		NativeHandle Constructor (string modelName, [NullAllowed] NSBundle bundle, string versionChecksum);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSMigrationStage))]
	[DisableDefaultCtor]
	interface NSLightweightMigrationStage {
		[Export ("versionChecksums", ArgumentSemantic.Strong)]
		string [] VersionChecksums { get; }

		[Export ("initWithVersionChecksums:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string [] versionChecksums);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSAttributeDescription))]
	interface NSCompositeAttributeDescription {
		[Export ("elements", ArgumentSemantic.Copy)]
		NSAttributeDescription [] Elements { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSMigrationStage))]
	[DisableDefaultCtor]
	interface NSCustomMigrationStage {
		[Export ("currentModel", ArgumentSemantic.Strong)]
		NSManagedObjectModelReference CurrentModel { get; }

		[Export ("nextModel", ArgumentSemantic.Strong)]
		NSManagedObjectModelReference NextModel { get; }

		[NullAllowed]
		[Export ("willMigrateHandler", ArgumentSemantic.Copy)]
		Func<NSStagedMigrationManager, NSCustomMigrationStage, NSError, bool> WillMigrateHandler { get; set; }

		[NullAllowed]
		[Export ("didMigrateHandler", ArgumentSemantic.Copy)]
		Func<NSStagedMigrationManager, NSCustomMigrationStage, NSError, bool> DidMigrateHandler { get; set; }

		[Export ("initWithCurrentModelReference:nextModelReference:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSManagedObjectModelReference currentModel, NSManagedObjectModelReference nextModel);
	}
}

