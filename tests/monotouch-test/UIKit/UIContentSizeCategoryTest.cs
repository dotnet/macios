//
// Unit tests for UIContentSizeCategory
//
// Authors:
//	Vincent Dondain <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft. All rights reserved.
//

#if !MONOMAC

using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UIContentSizeCategoryTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
		}

		[Test]
		public void IsAccessibilityCategory ()
		{
			var isAccessible = UIContentSizeCategory.AccessibilityMedium.IsAccessibilityCategory ();
			ClassicAssert.IsTrue (isAccessible, "AccessibilityMedium");
			isAccessible = UIContentSizeCategory.Medium.IsAccessibilityCategory ();
			ClassicAssert.IsFalse (isAccessible, "Medium");
		}

		[Test]
		public void Compare ()
		{
			var small = UIContentSizeCategory.Small;
			var large = UIContentSizeCategory.Large;
			ClassicAssert.True (UIContentSizeCategoryExtensions.Compare (small, large) == NSComparisonResult.Ascending, "small < large");
			Assert.Throws<ArgumentException> (() => UIContentSizeCategoryExtensions.Compare ((UIContentSizeCategory) 31415, large));
			Assert.Throws<ArgumentException> (() => UIContentSizeCategoryExtensions.Compare (small, (UIContentSizeCategory) 271828));
			Assert.Throws<ArgumentException> (() => ((UIContentSizeCategory) 1234).IsAccessibilityCategory ());
		}

		[Test]
		public void GetPreferredContentSizeCategoryTest ()
		{
			var sizeNSString = UIApplication.SharedApplication.PreferredContentSizeCategory;
			var sizeEnum = UIContentSizeCategoryExtensions.GetValue (sizeNSString);
			var size = UIApplication.SharedApplication.GetPreferredContentSizeCategory ();
			ClassicAssert.AreEqual (sizeEnum, size, "String");
			var sizeReverse = size.GetConstant ();
			ClassicAssert.AreEqual (sizeNSString, sizeReverse, "NSString");
		}
	}
}

#endif // !MONOMAC
