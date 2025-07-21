using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;

#nullable enable

namespace Foundation {
#if false // https://github.com/dotnet/macios/issues/15577
	[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
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
