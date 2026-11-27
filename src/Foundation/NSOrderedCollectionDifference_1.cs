//
// This file contains a generic version of NSOrderedCollectionDifference
//
#nullable enable
using System.Collections.Generic;
using System.Collections;

namespace Foundation {
	public delegate NSOrderedCollectionChange<TKey>? NSOrderedCollectionDifferenceGetDifferenceHandler<TKey> (NSOrderedCollectionChange<TKey> collectionChange)
		where TKey : class, INativeObject;

	[SupportedOSPlatform ("ios"), SupportedOSPlatform ("tvos"), SupportedOSPlatform ("macos"), SupportedOSPlatform ("maccatalyst")]
	[Register (SkipRegistration = true)]
	public sealed partial class NSOrderedCollectionDifference<TKey> : NSOrderedCollectionDifference
		where TKey : class, INativeObject {

		internal NSOrderedCollectionDifference (NativeHandle handle)
			: base (handle) { }

		public NSOrderedCollectionDifference (NSOrderedCollectionChange<TKey> [] changes)
			: base (changes) { }

		[DesignatedInitializer]
		public NSOrderedCollectionDifference (NSIndexSet inserts, TKey []? insertedObjects, NSIndexSet removes, TKey []? removedObjects, NSOrderedCollectionChange<TKey> [] changes)
			: base (NSObjectFlag.Empty)
		{
			using var insertedArray = insertedObjects is null ? null : NSArray.FromNSObjects (insertedObjects);
			using var removedArray = removedObjects is null ? null : NSArray.FromNSObjects (removedObjects);
			InitializeHandle (_Init (inserts, insertedArray, removes, removedArray, changes), "initWithInsertIndexes:insertedObjects:removeIndexes:removedObjects:additionalChanges:");
		}

		public NSOrderedCollectionDifference (NSIndexSet inserts, TKey []? insertedObjects, NSIndexSet removes, TKey []? removedObjects)
			: base (NSObjectFlag.Empty)
		{
			using var insertedArray = insertedObjects is null ? null : NSArray.FromNSObjects (insertedObjects);
			using var removedArray = removedObjects is null ? null : NSArray.FromNSObjects (removedObjects);
			InitializeHandle (_Init (inserts, insertedArray, removes, removedArray), "initWithInsertIndexes:insertedObjects:removeIndexes:removedObjects:");
		}

		public new NSOrderedCollectionChange<TKey> [] Insertions => NSArray.NonNullArrayFromHandleDropNullElements<NSOrderedCollectionChange<TKey>> (_Insertions);

		public new NSOrderedCollectionChange<TKey> [] Removals => NSArray.NonNullArrayFromHandleDropNullElements<NSOrderedCollectionChange<TKey>> (_Removals);

		public NSOrderedCollectionDifference<TKey>? GetDifference (NSOrderedCollectionDifferenceGetDifferenceHandler<TKey> callback)
		{
			if (callback is null)
				throw new ArgumentNullException (nameof (callback));

			unsafe {
				using var block = CreateTransformBlock (change => callback (Runtime.GetNSObject<NSOrderedCollectionChange<TKey>> (change) ?? throw new InvalidOperationException ("The native change is null.")));
				return Runtime.GetNSObject<NSOrderedCollectionDifference<TKey>> (_GetDifference (&block));
			}
		}

		public new NSOrderedCollectionDifference<TKey>? GetInverseDifference ()
			=> Runtime.GetNSObject<NSOrderedCollectionDifference<TKey>> (_InverseDifference ());

	}
}
