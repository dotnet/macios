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
				ClassicAssert.Null (dp.MinimumDate, "MinimumDate");
				ClassicAssert.Null (dp.MaximumDate, "MaximumDate");
				ClassicAssert.Null (dp.TimeZone, "TimeZone");

				ClassicAssert.NotNull (dp.Calendar, "Calendar");
				ClassicAssert.NotNull (dp.Date, "Date");
			}
		}

		[Test]
		public void Locale ()
		{
			using (UIDatePicker dp = new UIDatePicker ()) {
				ClassicAssert.NotNull (dp.Locale, "Locale");
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
