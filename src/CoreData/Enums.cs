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
// Copyright 2011-2014 Xamarin Inc.

using System;

using ObjCRuntime;

#nullable enable

namespace CoreData {

	// NUInteger -> NSEntityMapping.h
	/// <summary>Enumerates specific types of entity mapping.</summary>
	[Native]
	public enum NSEntityMappingType : ulong {
		/// <summary>To be added.</summary>
		Undefined = 0x00,
		/// <summary>To be added.</summary>
		Custom = 0x01,
		/// <summary>To be added.</summary>
		Add = 0x02,
		/// <summary>To be added.</summary>
		Remove = 0x03,
		/// <summary>To be added.</summary>
		Copy = 0x04,
		/// <summary>To be added.</summary>
		Transform = 0x05,
	}

	// NUInteger -> NSAttributeDescription.h
	/// <summary>An enumeration whose values define the valid types for <see cref="CoreData.NSAttributeType" /> objects.</summary>
	[Native]
	public enum NSAttributeType : ulong {
		/// <summary>Indicates an undefined type.</summary>
		Undefined = 0,
		/// <summary>Indicates a signed 16-bit integer.</summary>
		Integer16 = 100,
		/// <summary>Indicates a signed 32-bit integer..</summary>
		Integer32 = 200,
		/// <summary>Indicates a signed 64-bit integer.</summary>
		Integer64 = 300,
		/// <summary>Indicates an NSDecimalNumber.</summary>
		Decimal = 400,
		/// <summary>Indicates a double.</summary>
		Double = 500,
		/// <summary>Indicates a float.</summary>
		Float = 600,
		/// <summary>Indicates an NSString.</summary>
		String = 700,
		/// <summary>Indicates a Boolean value</summary>
		Boolean = 800,
		/// <summary>Indicates an NSDate.</summary>
		Date = 900,
		/// <summary>Indicates NSData.</summary>
		Binary = 1000,
		/// <summary>Indicates a UUID.</summary>
		[MacCatalyst (13, 1)]
		Uuid = 1100,
		/// <summary>Indicates a URI.</summary>
		[MacCatalyst (13, 1)]
		Uri = 1200,
		/// <summary>Indicates a type with an available transformer.</summary>
		Transformable = 1800,
		/// <summary>Indicates an Object ID.</summary>
		ObjectID = 2000,
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		CompositeAttributeType = 2100,
	}

	// NUInteger -> NSFetchRequest.h
	/// <summary>Enumerates possible result types from a fetch.</summary>
	[Flags]
	[Native]
	public enum NSFetchRequestResultType : ulong {
		/// <summary>To be added.</summary>
		ManagedObject = 0x00,
		/// <summary>To be added.</summary>
		ManagedObjectID = 0x01,
		/// <summary>To be added.</summary>
		DictionaryResultType = 0x02,
		/// <summary>To be added.</summary>
		NSCountResultType = 0x04,
	}

	// NUInteger -> NSRelationshipDescription.h
	/// <summary>An enumeration whose values specify cascade options after deletion of an object.</summary>
	[Native]
	public enum NSDeleteRule : ulong {
		/// <summary>To be added.</summary>
		NoAction,
		/// <summary>To be added.</summary>
		Nullify,
		/// <summary>To be added.</summary>
		Cascade,
		/// <summary>To be added.</summary>
		Deny,
	}

	// NUInteger -> NSPersistentStoreRequest.h
	/// <summary>Constants defining the types of a fetch request.</summary>
	[Native]
	public enum NSPersistentStoreRequestType : ulong {
		/// <summary>Indicates that the fetch request was for retrieving objects.</summary>
		Fetch = 1,
		/// <summary>Indicates that the fetch request was to save managed objects.</summary>
		Save,
		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		BatchInsert = 5,
		/// <summary>Indicates that the fetch request was for a batch update.</summary>
		BatchUpdate = 6,
		/// <summary>Indicates that the fetch request was for a batch delete.</summary>
		BatchDelete = 7,
	}

	// NUInteger -> NSManagedObjectContext.h
	/// <summary>Enumerates types of concurrenty patterns that can be used by a <see cref="CoreData.NSManagedObjectContext" />.</summary>
	[Native]
	public enum NSManagedObjectContextConcurrencyType : ulong {
		/// <summary>To be added.</summary>
		Confinement,
		/// <summary>To be added.</summary>
		PrivateQueue,
		/// <summary>To be added.</summary>
		MainQueue,
	}

	// NUInteger -> NSManagedObjectContext.h
	/// <summary>Enumerates policies for merging.</summary>
	[Native]
	public enum NSMergePolicyType : ulong {
		/// <summary>To be added.</summary>
		Error,
		/// <summary>To be added.</summary>
		PropertyStoreTrump,
		/// <summary>To be added.</summary>
		PropertyObjectTrump,
		/// <summary>To be added.</summary>
		Overwrite,
		/// <summary>To be added.</summary>
		RollbackMerge,
	}

	// NUInteger -> NSFetchedResultsController.h
	/// <summary>Enumerates kinds of batch requests.</summary>
	[Native]
	public enum NSFetchedResultsChangeType : ulong {
		/// <summary>To be added.</summary>
		Insert = 1,
		/// <summary>To be added.</summary>
		Delete = 2,
		/// <summary>To be added.</summary>
		Move = 3,
		/// <summary>To be added.</summary>
		Update = 4,
	}

	/// <summary>Enumerates the kind of results that can be returned from a batched update request.</summary>
	[Native]
	public enum NSBatchUpdateRequestResultType : ulong {
		/// <summary>To be added.</summary>
		StatusOnly = 0,
		/// <summary>To be added.</summary>
		UpdatedObjectIDs = 1,
		/// <summary>To be added.</summary>
		UpdatedObjectsCount = 2,
	}

	/// <summary>Enumerates the form of the result of a <see cref="CoreData.NSBatchDeleteRequest" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSBatchDeleteRequestResultType : ulong {
		/// <summary>To be added.</summary>
		StatusOnly = 0,
		/// <summary>To be added.</summary>
		ObjectIDs = 1,
		/// <summary>To be added.</summary>
		Count = 2,
	}

	/// <summary>Enumerates errors that can be encountered during validation.</summary>
	[Native]
	public enum ValidationErrorType : ulong {
		/// <summary>Indicates an error with a managed object.</summary>
		ManagedObjectValidation = 1550,
		/// <summary>Indicates that multiple errors occurred.</summary>
		MultipleErrors = 1560,
		/// <summary>Indicates that a mandatory property was missing.</summary>
		MissingMandatoryProperty = 1570,
		/// <summary>Indicates that a to-many relationship was bound to too few objects.</summary>
		RelationshipLacksMinimumCount = 1580,
		/// <summary>Indicates that a to-many relationship was bound to too many objects.</summary>
		RelationshipExceedsMaximumCount = 1590,
		/// <summary>Indicates that an attempt was made to delete a relationship that has a delete rule that denies deletion.</summary>
		RelationshipDeniedDelete = 1600,
		/// <summary>Indicates that a number was too large.</summary>
		NumberTooLarge = 1610,
		/// <summary>Indicates that a number was too small.</summary>
		NumberTooSmall = 1620,
		/// <summary>Indicates that a date was too late.</summary>
		DateTooLate = 1630,
		/// <summary>Indicates that a date was too soon.</summary>
		DateTooSoon = 1640,
		/// <summary>Indicates that a date was invalid.</summary>
		InvalidDate = 1650,
		/// <summary>Indicates that a string was too long.</summary>
		StringTooLong = 1660,
		/// <summary>Indicates that a string was too short.</summary>
		StringTooShort = 1670,
		/// <summary>Indicates that a pattern failed to match a string.</summary>
		StringPatternMatching = 1680,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		InvalidUri = 1690,
	}

	/// <summary>Enumerates error codes that can be encountered while working with object graphs.</summary>
	[Native]
	public enum ObjectGraphManagementErrorType : ulong {
		/// <summary>Indicates that a lock could not be obtained on a managed object context.</summary>
		ManagedObjectContextLocking = 132000,
		/// <summary>Indicates that a lock could not be obtained on a persistent store.</summary>
		PersistentStoreCoordinatorLocking = 132010,
		/// <summary>Indicates that an attempt was made to fire a fault at an object that does not exist.</summary>
		ManagedObjectReferentialIntegrity = 133000,
		/// <summary>Indicates that an object being saved contains a relationship to an object in another store.</summary>
		ManagedObjectExternalRelationship = 133010,
		/// <summary>Indicates that a merge failed due to a merge policy.</summary>
		ManagedObjectMerge = 133020,
	}

	/// <summary>Enumerates error codes that can be encountered while working with persistent stores.</summary>
	[Native]
	public enum PersistentStoreErrorType : ulong {
		/// <summary>Indicates that an invalid type was encountered.</summary>
		InvalidType = 134000,
		/// <summary>Indicates that a store was encountered that did not match the specified type.</summary>
		TypeMismatch = 134010,
		/// <summary>Indicates that an incompatible schema was encountered, for example, one that specifies a table that is not present.</summary>
		IncompatibleSchema = 134020,
		/// <summary>Indicates that an error, such as a permissions error, was encountered while saving a persistent store.</summary>
		Save = 134030,
		/// <summary>Indicates that an error occurred while saving one or more stores in a container.</summary>
		IncompleteSave = 134040,
		/// <summary>Indicates that an unresolved merge conflict was encountered while saving.</summary>
		SaveConflicts = 134050,
		/// <summary>Indicates that an error occurred while performing an operation on persistent store.</summary>
		Operation = 134070,
		/// <summary>Indicates that an error occurred while trying to open a persistent store.</summary>
		Open = 134080,
		/// <summary>Indicates that connecting to a persistent store took too long.</summary>
		Timeout = 134090,
		/// <summary>Indicates that the entity version hashes in the persistent store are not compatible with the managed object model.</summary>
		IncompatibleVersionHash = 134100,
	}

	/// <summary>Enumerates error codes that can be encountered while migrating stores.</summary>
	[Native]
	public enum MigrationErrorType {
		/// <summary>An unspecified error occurred.</summary>
		Migration = 134110,
		/// <summary>The migration was canceled.</summary>
		MigrationCancelled = 134120,
		/// <summary>The source data model was missing.</summary>
		MigrationMissingSourceModel = 134130,
		/// <summary>The mapping model was missing.</summary>
		MigrationMissingMappingModel = 134140,
		/// <summary>There was a problem with the source store.</summary>
		MigrationManagerSourceStore = 134150,
		/// <summary>There was a problem with the destination store.</summary>
		MigrationManagerDestinationStore = 134160,
		/// <summary>An error occurred while processing an entity migration policy.</summary>
		EntityMigrationPolicy = 134170,
		/// <summary>An error occurred while creating an inferred mapping model.</summary>
		InferredMappingModel = 134190,
		/// <summary>An error occurred while importing an external record.</summary>
		ExternalRecordImport = 134200,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		HistoryTokenExpired = 134301,
	}

	/// <summary>Enumerates index types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSFetchIndexElementType : ulong {
		/// <summary>Indicates indices into a binary tree.</summary>
		Binary,
		/// <summary>Indicates indices into an R-tree.</summary>
		RTree,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSPersistentHistoryChangeType : long {
		/// <summary>To be added.</summary>
		Insert,
		/// <summary>To be added.</summary>
		Update,
		/// <summary>To be added.</summary>
		Delete,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSPersistentHistoryResultType : long {
		/// <summary>To be added.</summary>
		StatusOnly = 0,
		/// <summary>To be added.</summary>
		ObjectIds = 1,
		/// <summary>To be added.</summary>
		Count = 2,
		/// <summary>To be added.</summary>
		TransactionsOnly = 3,
		/// <summary>To be added.</summary>
		ChangesOnly = 4,
		/// <summary>To be added.</summary>
		TransactionsAndChanges = 5,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSBatchInsertRequestResultType : ulong {
		StatusOnly = 0,
		ObjectIds = 1,
		Count = 2,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum NSPersistentCloudKitContainerSchemaInitializationOptions : ulong {
		None = 0x0,
		DryRun = 1 << 1,
		PrintSchema = 1 << 2,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum NSPersistentCloudKitContainerEventResultType : long {
		Events = 0,
		CountEvents,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum NSPersistentCloudKitContainerEventType : long {
		Setup,
		Import,
		Export,
	}

}
