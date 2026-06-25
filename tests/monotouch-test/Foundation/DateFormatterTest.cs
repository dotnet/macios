//
// Unit tests for NSDateFormatter
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DateFormatterTest {

		[Test]
		public void ToLocalizedStringTest ()
		{
			var str = NSDateFormatter.ToLocalizedString (NSDate.Now, NSDateFormatterStyle.Full, NSDateFormatterStyle.Full);
			Assert.That (str, Is.Not.Null);
		}

		[Test]
		public void GetDateFormatFromTemplateTest ()
		{
			var us_locale = new NSLocale ("en_US");
			var gb_locale = new NSLocale ("en_GB");
			const string dateComponents = "yMMMMd";

			var dateFormat = NSDateFormatter.GetDateFormatFromTemplate (dateComponents, 0, us_locale);
			Assert.That (dateFormat, Is.EqualTo ("MMMM d, y"), "#US");

			dateFormat = NSDateFormatter.GetDateFormatFromTemplate (dateComponents, 0, gb_locale);
			Assert.That (dateFormat, Is.EqualTo ("d MMMM y"), "GB");
		}
	}
}
