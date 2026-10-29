using System.Collections.Generic;
using System.Collections;

#nullable enable

namespace Foundation {
	[SupportedOSPlatform ("ios"), SupportedOSPlatform ("tvos"), SupportedOSPlatform ("macos"), SupportedOSPlatform ("maccatalyst")]
	[Register (SkipRegistration = true)]
	public sealed partial class NSOrderedCollectionChange<TKey> : NSOrderedCollectionChange
		where TKey : class, INativeObject {

		public static NSOrderedCollectionChange<TKey> ChangeWithObject (TKey? anObject, NSCollectionChangeType type, nuint index)
		{
			var change = new NSOrderedCollectionChange<TKey> (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index));
			GC.KeepAlive (anObject);
			return change;
		}

		public static NSOrderedCollectionChange<TKey> ChangeWithObject (TKey? anObject, NSCollectionChangeType type, nuint index, nuint associatedIndex)
		{
			var change = new NSOrderedCollectionChange<TKey> (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index, associatedIndex));
			GC.KeepAlive (anObject);
			return change;
		}

		internal NSOrderedCollectionChange (NativeHandle handle) : base (handle) { }

		public NSOrderedCollectionChange (TKey? anObject, NSCollectionChangeType type, nuint index) 
			: base (anObject.GetHandle (), type, index)
		{
			GC.KeepAlive (anObject);
		}

		[DesignatedInitializer]
		public NSOrderedCollectionChange (TKey? anObject, NSCollectionChangeType type, nuint index, nuint associatedIndex)
			: base (anObject.GetHandle (), type, index, associatedIndex)
		{
			GC.KeepAlive (anObject);
		}

		public new TKey? Object => Runtime.GetINativeObject<TKey> (_Object, false);
	}
}
