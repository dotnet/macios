// Copyright 2014 Xamarin Inc. All rights reserved

#if !MONOMAC

using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CollectionViewTransitionLayoutTest {

		[Test]
		public void Ctor ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var l1 = new UICollectionViewLayout ())
			using (var l2 = new UICollectionViewLayout ())
			using (var tl = new UICollectionViewTransitionLayout (l1, l2)) {
				// interesting ctor for the linker (two [PostGet])
				Assert.That (l1, Is.SameAs (tl.CurrentLayout), "CurrentLayout");
				Assert.That (l2, Is.SameAs (tl.NextLayout), "NextLayout");
			}
		}
	}
}

#endif // !MONOMAC
