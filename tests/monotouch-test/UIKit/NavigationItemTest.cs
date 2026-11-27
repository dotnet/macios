//
// Unit tests for UINavigationItem
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using System.Drawing;
using System.Reflection;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NavigationItemTest {
		[Test]
		public void AccessibilityLabel ()
		{
			using var item = new UINavigationItem ("Title");
			item.AccessibilityLabel = "Accessible title";
			Assert.That (item.AccessibilityLabel, Is.EqualTo ("Accessible title"), "Label");
			Assert.That (item.Title, Is.EqualTo ("Title"), "Title");

			item.AccessibilityLabel = null;
			Assert.That (item.AccessibilityLabel, Is.Null, "Cleared label");
		}
	}
}

#endif // !MONOMAC
