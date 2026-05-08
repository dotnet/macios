// Copyright 2011-2012 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DatePickerTest {

		[Test]
		public void Defaults ()
		{
			using (UIDatePicker dp = new UIDatePicker ()) {
				Assert.That (dp.MinimumDate, Is.Null, "MinimumDate");
				Assert.That (dp.MaximumDate, Is.Null, "MaximumDate");
				Assert.That (dp.TimeZone, Is.Null, "TimeZone");

				Assert.That (dp.Calendar, Is.Not.Null, "Calendar");
				Assert.That (dp.Date, Is.Not.Null, "Date");
			}
		}

		[Test]
		public void Locale ()
		{
			using (UIDatePicker dp = new UIDatePicker ()) {
				Assert.That (dp.Locale, Is.Not.Null, "Locale");
			}
		}
		[Test]
		public void Nulls ()
		{
			using (UIDatePicker dp = new UIDatePicker ()) {
				dp.Calendar = null;
				dp.Locale = null;
				dp.MinimumDate = null;
				dp.MaximumDate = null;
				dp.TimeZone = null;

				// some null checks are done, otherwise we end up with
				// Objective-C exception thrown.  Name: NSInternalInconsistencyException Reason: Invalid parameter not satisfying: date
				Assert.Throws<ArgumentNullException> (delegate
				{
					dp.Date = null;
				});
				Assert.Throws<ArgumentNullException> (delegate
				{
					dp.SetDate (null, true);
				});
			}
		}

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIDatePicker dp = new UIDatePicker (frame)) {
				Assert.That (dp.Frame.X, Is.EqualTo (frame.X), "X");
				Assert.That (dp.Frame.Y, Is.EqualTo (frame.Y), "Y");
				// Width and Height are set by the DatePicker (e.g. 320 x 216 for the iPhone)
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
