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
				ClassicAssert.AreEqual ("gregorian", rule.CalendarIdentifier, "CalendarIdentifier");
				ClassicAssert.IsNull (rule.RecurrenceEnd, "RecurrenceEnd");
				ClassicAssert.AreEqual (EKRecurrenceFrequency.Weekly, rule.Frequency, "Frequency");
				ClassicAssert.AreEqual ((nint) 1, rule.Interval, "Interval");
				ClassicAssert.AreEqual (EKWeekday.Monday, rule.FirstDayOfTheWeek, "FirstDayOfTheWeek");
				ClassicAssert.IsNull (rule.DaysOfTheWeek, "DaysOfTheWeek");
				ClassicAssert.IsNull (rule.DaysOfTheMonth, "DaysOfTheMonth");
				ClassicAssert.IsNull (rule.DaysOfTheYear, "DaysOfTheYear");
				ClassicAssert.IsNull (rule.WeeksOfTheYear, "WeeksOfTheYear");
				ClassicAssert.IsNull (rule.MonthsOfTheYear, "MonthsOfTheYear");
				ClassicAssert.IsNull (rule.SetPositions, "SetPositions");
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
