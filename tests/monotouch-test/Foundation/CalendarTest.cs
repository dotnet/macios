using Xamarin.Utils;

using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;
using PointF = CoreGraphics.CGPoint;

using PlatformException = ObjCRuntime.ObjCException;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CalendarTest {

		[Test]
		public void DateComponentsTest ()
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var now = DateTime.UtcNow;
			NSDateComponents comps;

			cal.TimeZone = NSTimeZone.FromName ("UTC");
			comps = cal.Components (NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day, (NSDate) now);
			ClassicAssert.AreEqual ((nint) now.Year, comps.Year, "a year");
			ClassicAssert.AreEqual ((nint) now.Month, comps.Month, "a month");
			ClassicAssert.AreEqual ((nint) now.Day, comps.Day, "a day");

			var dayCompare = now;

			comps = cal.Components (NSCalendarUnit.Hour, (NSDate) dayCompare.AddHours (-1), (NSDate) dayCompare, NSCalendarOptions.None);
			ClassicAssert.AreEqual ((nint) 1, comps.Hour, "c hour");
		}

		[Test]
		public void DateByAddingComponentsTest ()
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var now = (NSDate) DateTime.Now;
			var comps = new NSDateComponents ();
			NSDate date;

			comps.Day = 2;
			comps.TimeZone = NSTimeZone.FromAbbreviation ("UTC");
			cal.TimeZone = comps.TimeZone;

			date = cal.DateByAddingComponents (comps, now, NSCalendarOptions.None);
			ClassicAssert.AreEqual (now.SecondsSinceReferenceDate + 3600 * 24 * 2, date.SecondsSinceReferenceDate, "b");
		}

		[Test]
		public void DateFromComponents ()
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var comps = new NSDateComponents ();
			comps.Year = 1965;
			comps.Month = 1;
			comps.Day = 6;
			comps.Hour = 14;
			comps.Minute = 10;
			comps.Second = 0;
			comps.TimeZone = new NSTimeZone ("Europe/Madrid");
			var date = cal.DateFromComponents (comps);
			ClassicAssert.AreEqual (-1135594200d, date.SecondsSinceReferenceDate, "a");
		}

		static void RequiresIos8 ()
		{
			TestRuntime.AssertXcodeVersion (6, 0);
		}

		public NSDate Yesterday { get { return NSDate.FromTimeIntervalSinceNow (-60 * 60 * 24); } }
		public NSDate Tomorrow { get { return NSDate.FromTimeIntervalSinceNow (60 * 60 * 24); } }
		public NSDate NowPlusTenSeconds { get { return NSDate.FromTimeIntervalSinceNow (10); } }
		public NSDate NowPlusOneHour { get { return NSDate.FromTimeIntervalSinceNow (60 * 60); } }
		public NSDate NowMinusTenSeconds { get { return NSDate.FromTimeIntervalSinceNow (-10); } }


		[Test]
		public void GetAllCalendarIdentifiers ()
		{
			RequiresIos8 ();

			foreach (var t in Enum.GetValues<NSCalendarType> ()) {
				switch (t) {
				case NSCalendarType.Bangla:
				case NSCalendarType.Gujarati:
				case NSCalendarType.Kannada:
				case NSCalendarType.Malayalam:
				case NSCalendarType.Marathi:
				case NSCalendarType.Odia:
				case NSCalendarType.Tamil:
				case NSCalendarType.Telugu:
				case NSCalendarType.Vikram:
				case NSCalendarType.Dangi:
				case NSCalendarType.Vietnamese:
					if (!TestRuntime.CheckXcodeVersion (26, 0))
						continue;
					break;
				}
				using var c = new NSCalendar (t);
				ClassicAssert.IsNotNull (c.Identifier, "Can't find identifier: " + t.ToString ());
			}
		}

		[Test]
		public void TestCalendarSymbols ()
		{
			RequiresIos8 ();

			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.EraSymbols.Length > 0, "EraSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.LongEraSymbols.Length > 0, "LongEraSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.MonthSymbols.Length > 0, "MonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortMonthSymbols.Length > 0, "ShortMonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.VeryShortMonthSymbols.Length > 0, "VeryShortMonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.StandaloneMonthSymbols.Length > 0, "StandaloneMonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortStandaloneMonthSymbols.Length > 0, "ShortStandaloneMonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.VeryShortStandaloneMonthSymbols.Length > 0, "VeryShortStandaloneMonthSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.WeekdaySymbols.Length > 0, "WeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortWeekdaySymbols.Length > 0, "ShortWeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.VeryShortWeekdaySymbols.Length > 0, "VeryShortWeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.StandaloneWeekdaySymbols.Length > 0, "StandaloneWeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortStandaloneWeekdaySymbols.Length > 0, "ShortStandaloneWeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.VeryShortStandaloneWeekdaySymbols.Length > 0, "VeryShortStandaloneWeekdaySymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.QuarterSymbols.Length > 0, "QuarterSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortQuarterSymbols.Length > 0, "ShortQuarterSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.StandaloneQuarterSymbols.Length > 0, "StandaloneQuarterSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.ShortStandaloneQuarterSymbols.Length > 0, "ShortStandaloneQuarterSymbols not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.AMSymbol.Length > 0, "AMSymbol not found");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.PMSymbol.Length > 0, "PMSymbol not found");
		}

		[Test]
		public void TestCalendarComparision ()
		{
			RequiresIos8 ();

			NSDate today = NSDate.Now;
			NSDate todayPlusSeconds = NowPlusTenSeconds;
			if (NSCalendar.CurrentCalendar.CompareDate (today, todayPlusSeconds, NSCalendarUnit.Day) != NSComparisonResult.Same)
				Assert.Inconclusive ("Now plus 10 seconds isn't the same day, either a bug or run < 10 seconds before midnight");
			ClassicAssert.IsFalse (NSCalendar.CurrentCalendar.CompareDate (today, todayPlusSeconds, NSCalendarUnit.Second) == NSComparisonResult.Same, "Now plus 10 seconds shouldn't be the same second");
		}

		[Test]
		public void TestCalendarComponents ()
		{
			RequiresIos8 ();

			nint era, year, month, day = -1;
			NSCalendar.CurrentCalendar.GetComponentsFromDate (out era, out year, out month, out day, NSDate.Now);
			ClassicAssert.IsTrue (era >= 0, "GetComponentsFromDate - era");
			ClassicAssert.IsTrue (year >= 0, "GetComponentsFromDate - year");
			ClassicAssert.IsTrue (month >= 0, "GetComponentsFromDate - month");
			ClassicAssert.IsTrue (day >= 0, "GetComponentsFromDate - day");

			nint weekOfYear, weekday = -1;
			era = year = -1;
			NSCalendar.CurrentCalendar.GetComponentsFromDateForWeekOfYear (out era, out year, out weekOfYear, out weekday, NSDate.Now);
			ClassicAssert.IsTrue (era >= 0, "GetComponentsFromDateForWeekOfYear - era");
			ClassicAssert.IsTrue (year >= 0, "GetComponentsFromDateForWeekOfYear - year");
			ClassicAssert.IsTrue (weekOfYear >= 0, "GetComponentsFromDateForWeekOfYear - weekOfYear");
			ClassicAssert.IsTrue (weekday >= 0, "GetComponentsFromDateForWeekOfYear - weekday");

			nint hour, minute, second, nanosecond = -1;
			NSCalendar.CurrentCalendar.GetHourComponentsFromDate (out hour, out minute, out second, out nanosecond, NSDate.Now);
			ClassicAssert.IsTrue (hour >= 0, "GetHourComponentsFromDate - hour");
			ClassicAssert.IsTrue (minute >= 0, "GetHourComponentsFromDate - minute");
			ClassicAssert.IsTrue (second >= 0, "GetHourComponentsFromDate - second");
			ClassicAssert.IsTrue (nanosecond >= 0, "GetHourComponentsFromDate - nanosecond");

			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Day, NSDate.Now) > 0, "GetComponentFromDate - day");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Week, NSDate.Now) > 0, "GetComponentFromDate - week");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Month, NSDate.Now) > 0, "GetComponentFromDate - month");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Year, NSDate.Now) > 0, "GetComponentFromDate - year");
		}

		[Test]
		public void TestComponentsFromDateToDate ()
		{
			RequiresIos8 ();

			NSDateComponents todayComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, NSDate.Now);
			NSDateComponents tomorrowComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, Tomorrow);

			NSDateComponents components = NSCalendar.CurrentCalendar.ComponentsFromDateToDate (NSCalendarUnit.Day | NSCalendarUnit.Month, todayComponents, tomorrowComponents, NSCalendarOptions.None);
			ClassicAssert.AreEqual ((nint) 1, components.Day, "One day passed between today and tomorrow");
		}

		[Test]
		public void TestComponentsInTimeZone ()
		{
			RequiresIos8 ();

			NSTimeZone currentZone = NSTimeZone.LocalTimeZone;
			NSTimeZone otherZone = new NSTimeZone ("Asia/Bangkok");

			if (currentZone.SecondsFromGMT (NSDate.Now) == otherZone.SecondsFromGMT (NSDate.Now))
				Assert.Inconclusive ("Same time zone, change Asia/Bangkok");

			NSDateComponents components = NSCalendar.CurrentCalendar.ComponentsInTimeZone (otherZone, NSDate.Now);
			ClassicAssert.IsTrue (components.Hour != NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Hour, NSDate.Now).Hour, "Different time zones should have different hours");
		}

		[Test]
		public void TestMatchesComponents ()
		{
			RequiresIos8 ();

			NSDateComponents todayComponent = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day, NSDate.Now);
			bool futureMatch = NSCalendar.CurrentCalendar.Matches (NowPlusTenSeconds, todayComponent);
			bool pastMatch = NSCalendar.CurrentCalendar.Matches (NowMinusTenSeconds, todayComponent);
			if (futureMatch ^ pastMatch)    // While unlikley, if you run it within 10 seconds of a day boundry, we can get inconclusive results. Better this than a random failure
				Assert.Inconclusive ("Test was run with 10 seconds of a day switchover (unlikely) or malfunctioned.");

			ClassicAssert.IsTrue (futureMatch && pastMatch, "10 seconds on both side of us should both be on same day or Inconclusive");
		}

		[Test]
		public void TestAddingByComponents ()
		{
			RequiresIos8 ();
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			NSDate now = NSDate.Now;
			NSDate oneDayFromNow = NSCalendar.CurrentCalendar.DateByAddingUnit (NSCalendarUnit.Day, 1, now, NSCalendarOptions.None);
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsEqualToUnitGranularity (Tomorrow, oneDayFromNow, NSCalendarUnit.Day), $"oneDayFromNow: DateByAddingUnit - One day from now should be tomorrow {Tomorrow} != {oneDayFromNow}");

			var todayDayNumber = NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Day, NSDate.Now);
			NSDate todayPlusADay = NSCalendar.CurrentCalendar.DateBySettingUnit (NSCalendarUnit.Day, todayDayNumber + 1, now, NSCalendarOptions.None);
			if (todayPlusADay is null) { // Work on the last day of a month
				var todayMonthNumber = NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Month, now);
				todayPlusADay = NSCalendar.CurrentCalendar.DateBySettingUnit (NSCalendarUnit.Month, todayMonthNumber + 1, now, NSCalendarOptions.None);
			}
			if (todayPlusADay is null) { // Work on the last day of a year
				var todayYearNumber = NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Year, now);
				todayPlusADay = NSCalendar.CurrentCalendar.DateBySettingUnit (NSCalendarUnit.Year, todayYearNumber + 1, now, NSCalendarOptions.None);
			}
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsEqualToUnitGranularity (Tomorrow, todayPlusADay, NSCalendarUnit.Day), $"todayPlusADay: lDateBySettingUnit - One day from now should be tomorrow {Tomorrow} != {todayPlusADay}");
		}

		[Test]
		public void TestSettingHourComponent ()
		{
			RequiresIos8 ();

			var currentHour = NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Hour, NSDate.Now);
			NSDate oneHourFromNow = NSCalendar.CurrentCalendar.DateBySettingsHour (currentHour + 1, 0, 0, NSDate.Now, NSCalendarOptions.None);
			if (oneHourFromNow is null)
				Assert.Inconclusive ("Test does not handle day change");
			ClassicAssert.IsTrue ((currentHour + 1) == NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Hour, oneHourFromNow), "DateBySettingsHour - One hour from now should be one hour");
		}

		[Test]
		public void TestSignUpForDateNotification ()
		{
			RequiresIos8 ();

			// Just make sure signing up and canceling for event doesn't crash
			using (NSCalendar.Notifications.ObserveDayChanged ((o, e) => {
			})) {
			}
		}

		[Test]
		public void TestNSCalendarConstructors ()
		{
			RequiresIos8 ();

			NSDate date1 = NSCalendar.CurrentCalendar.Date (1, 2, 3, 4, 5, 6, 7, 8);
			ClassicAssert.IsNotNull (date1, "Date constructor 1");
			NSDate date2 = NSCalendar.CurrentCalendar.DateForWeekOfYear (1, 2, 3, 4, 5, 6, 7, 8);
			ClassicAssert.IsNotNull (date2, "Date constructor 2");
		}

		[Test]
		public void TestIsDateMethods ()
		{
			RequiresIos8 ();

			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsDateInToday (NSDate.Now), "IsDateInToday positive");
			ClassicAssert.IsFalse (NSCalendar.CurrentCalendar.IsDateInToday (Tomorrow), "IsDateInToday negative");

			ClassicAssert.IsFalse (NSCalendar.CurrentCalendar.IsDateInTomorrow (NSDate.Now), "IsDateInTomorrow negative");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsDateInTomorrow (Tomorrow), "IsDateInTomorrow positive");

			ClassicAssert.IsFalse (NSCalendar.CurrentCalendar.IsDateInYesterday (NSDate.Now), "IsDateInYesterday negative");
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsDateInYesterday (Yesterday), "IsDateInYesterday positive");

			ClassicAssert.IsFalse (NSCalendar.CurrentCalendar.IsInSameDay (NSDate.Now, Tomorrow), "IsInSameDay negative");

			NSDate weekend;
			double length;
			NSCalendar.CurrentCalendar.FindNextWeekend (out weekend, out length, NSCalendarOptions.None, NSDate.Now);
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.IsDateInWeekend (weekend), "IsDateInWeekend positive");
		}

		[Test]
		public void TestRangeOfWeekendContainingDate ()
		{
			RequiresIos8 ();

			NSDate weekend;
			double length;
			NSCalendar.CurrentCalendar.FindNextWeekend (out weekend, out length, NSCalendarOptions.None, NSDate.Now);

			NSDate weekend2;
			double length2;
			NSCalendar.CurrentCalendar.RangeOfWeekendContainingDate (out weekend2, out length2, weekend);

			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.CompareDate (weekend, weekend2, NSCalendarUnit.Day) == NSComparisonResult.Same, "Weekend test");
		}

		[Test]
		public void TestStartOfDay ()
		{
			RequiresIos8 ();

			NSDate firstMomentOfToday = NSCalendar.CurrentCalendar.StartOfDayForDate (NSDate.Now);
			ClassicAssert.IsTrue (NSCalendar.CurrentCalendar.CompareDate (firstMomentOfToday, NSDate.Now, NSCalendarUnit.Day) == NSComparisonResult.Same, "StartOfDayForDate");
		}

		[Test]
		public void TestFindNextDate ()
		{
			RequiresIos8 ();

			NSDateComponents nextYearComponent = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, NSDate.Now);
			nextYearComponent.Year++;

			NSDate nextYear = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, nextYearComponent, NSCalendarOptions.MatchNextTime);
			ClassicAssert.IsNotNull (nextYear, "FindNextDateAfterDateMatching");

			NSDate nextNoon = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, 12, 0, 0, NSCalendarOptions.MatchNextTime);
			ClassicAssert.IsNotNull (nextNoon, "FindNextDateAfterDateMatching 2");

			NSDate nextNoonAgain = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, NSCalendarUnit.Hour, 12, NSCalendarOptions.MatchNextTime);
			ClassicAssert.IsNotNull (nextNoonAgain, "FindNextDateAfterDateMatching 3");
		}

		[Test]
		public void TestEnumerateDates ()
		{
			RequiresIos8 ();

			NSDateComponents nextYearComponent = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, NSDate.Now);
			nextYearComponent.Year++;
			bool delegateHit = false;
			NSCalendar.CurrentCalendar.EnumerateDatesStartingAfterDate (NSDate.Now, nextYearComponent, NSCalendarOptions.MatchNextTime, (NSDate d, bool exactMatch, ref bool stop) => {
				delegateHit = true;
				stop = true;
			});
			ClassicAssert.IsTrue (delegateHit, "EnumerateDatesStartingAfterDate delegate called");
		}

		[Test]
		public void TestNSDateComponentNewAPIs ()
		{
			RequiresIos8 ();

			NSDateComponents todayComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year | NSCalendarUnit.Era | NSCalendarUnit.Calendar, NSDate.Now);
			var nano = todayComponents.Nanosecond;
			ClassicAssert.IsTrue (todayComponents.IsValidDate, "IsValidDate");
			ClassicAssert.IsTrue (todayComponents.IsValidDateInCalendar (NSCalendar.CurrentCalendar), "IsValidDateInCalendar");

			todayComponents.SetValueForComponent (12, NSCalendarUnit.Day);
			ClassicAssert.AreEqual ((nint) 12, todayComponents.GetValueForComponent (NSCalendarUnit.Day), "GetValueForComponent\\SetValueForComponent");
		}

		[Test]
		public void TestFindNextDateAfterDateMatching ()
		{
			RequiresIos8 ();

			NSDateComponents nextYearComponent = new NSDateComponents ();
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				ClassicAssert.IsNull (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, nextYearComponent, NSCalendarOptions.None), "nextYearComponent");

				ClassicAssert.NotNull (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, NSCalendarUnit.Day, 8, NSCalendarOptions.None), "Unit");

				ClassicAssert.NotNull (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, 1, 2, 3, NSCalendarOptions.None), "components");
			} else {
				Assert.Throws<PlatformException> (() =>
					NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, nextYearComponent, NSCalendarOptions.None), "nextYearComponent");

				Assert.Throws<PlatformException> (() =>
					NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, NSCalendarUnit.Day, 8, NSCalendarOptions.None), "Unit");

				Assert.Throws<PlatformException> (() =>
					NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, 1, 2, 3, NSCalendarOptions.None), "Components");
			}
		}

		[TestCase (1, 12, NSCalendarUnit.Month)]
		[TestCase (1, 28, NSCalendarUnit.Day)]
		[TestCase (0, 24, NSCalendarUnit.Hour)]
		public void TestMinimumRange (int location, int length, NSCalendarUnit unit)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var range = cal.MinimumRange (unit);
			ClassicAssert.AreEqual ((nint) location, range.Location);
			ClassicAssert.AreEqual ((nint) length, range.Length);
		}

		[TestCase (1, 12, NSCalendarUnit.Month)]
		[TestCase (1, 31, NSCalendarUnit.Day)]
		[TestCase (0, 24, NSCalendarUnit.Hour)]
		public void TestMaximumRange (int location, int length, NSCalendarUnit unit)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var range = cal.MaximumRange (unit);
			ClassicAssert.AreEqual ((nint) length, range.Length);
			ClassicAssert.AreEqual ((nint) location, range.Location);
		}

		[TestCase (2010, 1, 11, 1, 31, NSCalendarUnit.Day, NSCalendarUnit.Month)]
		[TestCase (2010, 2, 11, 1, 28, NSCalendarUnit.Day, NSCalendarUnit.Month)]
		[TestCase (2010, 3, 11, 1, 31, NSCalendarUnit.Day, NSCalendarUnit.Month)]
		[TestCase (1999, 1, 11, 0, 24, NSCalendarUnit.Hour, NSCalendarUnit.Day)]
		public void TestRange (int year, int month, int day, int location, int length, NSCalendarUnit smaller, NSCalendarUnit larger)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var date = new DateTime (year, month, day);
			date = DateTime.SpecifyKind (date, DateTimeKind.Utc);
			var range = cal.Range (smaller, larger, (NSDate) date);
			ClassicAssert.AreEqual ((nint) location, range.Location);
			ClassicAssert.AreEqual ((nint) length, range.Length);
		}

		[TestCase (2010, 1, 11, NSCalendarUnit.Day, NSCalendarUnit.Month, 11)]
		[TestCase (2010, 4, 15, NSCalendarUnit.Day, NSCalendarUnit.Month, 15)]
		public void TestOrdinality (int year, int month, int day, NSCalendarUnit smaller, NSCalendarUnit larger, int expected)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var date = new DateTime (year, month, day, 0, 0, 0, DateTimeKind.Utc);
			var dt = (NSDate) date;
			cal.TimeZone = NSTimeZone.FromName ("Europe/Madrid");
			ClassicAssert.AreEqual ((nuint) expected, cal.Ordinality (smaller, larger, dt), $"Ordinality");
		}

		[TestCase (2010, 1, 11, NSCalendarUnit.Day, 86400.0)]
		[TestCase (2010, 2, 11, NSCalendarUnit.Hour, 3600.0)]
		[TestCase (2015, 2, 11, NSCalendarUnit.Month, 2419200.0)]
		public void TestRangeOrUnitInterval (int year, int month, int day, NSCalendarUnit unit, double expectedInterval)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var date = new DateTime (year, month, day);
			date = DateTime.SpecifyKind (date, DateTimeKind.Utc);
			NSDate outDate = null;
			double outInterval;
			var success = cal.Range (unit, out outDate, out outInterval, (NSDate) date);
			ClassicAssert.AreEqual (expectedInterval, outInterval);
		}

		[TestCase (2010, 1, 11, NSCalendarUnit.Day, 86400.0)]
		[TestCase (2010, 2, 11, NSCalendarUnit.Hour, 3600.0)]
		[TestCase (2015, 2, 11, NSCalendarUnit.Month, 2419200.0)]
		public void TestRangeOrUnitIntervalNotNull (int year, int month, int day, NSCalendarUnit unit, double expectedInterval)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var date = new DateTime (year, month, day);
			date = DateTime.SpecifyKind (date, DateTimeKind.Utc);
			var outDate = (NSDate) DateTime.Now;
			double outInterval;
			var success = cal.Range (unit, out outDate, out outInterval, (NSDate) date);
			ClassicAssert.AreEqual (expectedInterval, outInterval);
		}

	}
}
