using System.Collections.Generic;
using System.Collections;

#nullable enable

namespace Foundation {
	public partial class NSOrderedCollectionChange
	{

		public NSOrderedCollectionChange (NSObject? anObject, NSCollectionChangeType type, nuint index)
			: this (anObject.GetHandle (), type, index)
		{
			GC.KeepAlive (anObject);
		}

		[DesignatedInitializer]
		public NSOrderedCollectionChange (NSObject? anObject, NSCollectionChangeType type, nuint index, nuint associatedIndex)
			: this (anObject.GetHandle (), type, index, associatedIndex)
		{
			GC.KeepAlive (anObject);
		}

		public static NSOrderedCollectionChange ChangeWithObject (NSObject? anObject, NSCollectionChangeType type, nuint index)
		{
			var change = new NSOrderedCollectionChange (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index));
			GC.KeepAlive (anObject);
			return change;
		}

		public static NSOrderedCollectionChange ChangeWithObject (NSObject? anObject, NSCollectionChangeType type, nuint index, nuint associatedIndex)
		{
			var change = new NSOrderedCollectionChange (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index, associatedIndex));
			GC.KeepAlive (anObject);
			return change;
		}

		public NSObject? Object => Runtime.GetNSObject<NSObject> (_Object);
	}
}
