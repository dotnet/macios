//
// Unit tests for EKRecurrenceRule
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using EventKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.EventKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RecurrenceRuleTest {
		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
		}

		[Test]
		public void DefaultProperties ()
		{
			using (var rule = new EKRecurrenceRule ()) {
				Assert.That (rule.CalendarIdentifier, Is.EqualTo ("gregorian"), "CalendarIdentifier");
				Assert.That (rule.RecurrenceEnd, Is.Null, "RecurrenceEnd");
				Assert.That (rule.Frequency, Is.EqualTo (EKRecurrenceFrequency.Weekly), "Frequency");
				Assert.That (rule.Interval, Is.EqualTo ((nint) 1), "Interval");
				Assert.That (rule.FirstDayOfTheWeek, Is.EqualTo (EKWeekday.Monday), "FirstDayOfTheWeek");
				Assert.That (rule.DaysOfTheWeek, Is.Null, "DaysOfTheWeek");
				Assert.That (rule.DaysOfTheMonth, Is.Null, "DaysOfTheMonth");
				Assert.That (rule.DaysOfTheYear, Is.Null, "DaysOfTheYear");
				Assert.That (rule.WeeksOfTheYear, Is.Null, "WeeksOfTheYear");
				Assert.That (rule.MonthsOfTheYear, Is.Null, "MonthsOfTheYear");
				Assert.That (rule.SetPositions, Is.Null, "SetPositions");
			}
		}

		[Test]
		public void Constructors ()
		{
			using (var rule = new EKRecurrenceRule (EKRecurrenceFrequency.Daily, 9, null)) {
			}
			using (var rule = new EKRecurrenceRule (EKRecurrenceFrequency.Yearly, 8, null, null, null, null, null, null, null)) {
			}
		}
	}
}

#endif // !__TVOS__
