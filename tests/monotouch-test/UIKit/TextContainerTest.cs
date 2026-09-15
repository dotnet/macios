//
// NSTextContainer Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TextContainerTest {

		[Test]
		public void Layout ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var tc = new NSTextContainer ()) {
				Assert.That (tc.LayoutManager, Is.Null, "LayoutManager");
			}
		}
	}
}

#endif // !MONOMAC
