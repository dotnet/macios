using System.Collections.Generic;
using System.Collections;

#nullable enable

namespace Foundation {
#if false // https://github.com/dotnet/macios/issues/15577
	[SupportedOSPlatform ("ios"), SupportedOSPlatform ("tvos"), SupportedOSPlatform ("macos")]
	public partial class NSOrderedCollectionChange
	{

		public static NSOrderedCollectionChange ChangeWithObject (NSObject? anObject, NSCollectionChangeType type, nuint index)
			=> new NSOrderedCollectionChange (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index));

		public static NSOrderedCollectionChange ChangeWithObject (NSObject? anObject, NSCollectionChangeType type, nuint index, nuint associatedIndex)
			=> new NSOrderedCollectionChange (NSOrderedCollectionChange._ChangeWithObject (anObject.GetHandle (), type, index, associatedIndex));

		public NSObject? Object => Runtime.GetNSObject<NSObject> (_Object);
	}
#endif
}
