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
			Assert.That (comps.Year, Is.EqualTo ((nint) now.Year), "a year");
			Assert.That (comps.Month, Is.EqualTo ((nint) now.Month), "a month");
			Assert.That (comps.Day, Is.EqualTo ((nint) now.Day), "a day");

			var dayCompare = now;

			comps = cal.Components (NSCalendarUnit.Hour, (NSDate) dayCompare.AddHours (-1), (NSDate) dayCompare, NSCalendarOptions.None);
			Assert.That (comps.Hour, Is.EqualTo ((nint) 1), "c hour");
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
			Assert.That (date.SecondsSinceReferenceDate, Is.EqualTo (now.SecondsSinceReferenceDate + 3600 * 24 * 2), "b");
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
			Assert.That (date.SecondsSinceReferenceDate, Is.EqualTo (-1135594200d), "a");
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
				Assert.That (c.Identifier, Is.Not.Null, "Can't find identifier: " + t.ToString ());
			}
		}

		[Test]
		public void TestCalendarSymbols ()
		{
			RequiresIos8 ();

			Assert.That (NSCalendar.CurrentCalendar.EraSymbols.Length > 0, Is.True, "EraSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.LongEraSymbols.Length > 0, Is.True, "LongEraSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.MonthSymbols.Length > 0, Is.True, "MonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortMonthSymbols.Length > 0, Is.True, "ShortMonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.VeryShortMonthSymbols.Length > 0, Is.True, "VeryShortMonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.StandaloneMonthSymbols.Length > 0, Is.True, "StandaloneMonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortStandaloneMonthSymbols.Length > 0, Is.True, "ShortStandaloneMonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.VeryShortStandaloneMonthSymbols.Length > 0, Is.True, "VeryShortStandaloneMonthSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.WeekdaySymbols.Length > 0, Is.True, "WeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortWeekdaySymbols.Length > 0, Is.True, "ShortWeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.VeryShortWeekdaySymbols.Length > 0, Is.True, "VeryShortWeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.StandaloneWeekdaySymbols.Length > 0, Is.True, "StandaloneWeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortStandaloneWeekdaySymbols.Length > 0, Is.True, "ShortStandaloneWeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.VeryShortStandaloneWeekdaySymbols.Length > 0, Is.True, "VeryShortStandaloneWeekdaySymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.QuarterSymbols.Length > 0, Is.True, "QuarterSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortQuarterSymbols.Length > 0, Is.True, "ShortQuarterSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.StandaloneQuarterSymbols.Length > 0, Is.True, "StandaloneQuarterSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.ShortStandaloneQuarterSymbols.Length > 0, Is.True, "ShortStandaloneQuarterSymbols not found");
			Assert.That (NSCalendar.CurrentCalendar.AMSymbol.Length > 0, Is.True, "AMSymbol not found");
			Assert.That (NSCalendar.CurrentCalendar.PMSymbol.Length > 0, Is.True, "PMSymbol not found");
		}

		[Test]
		public void TestCalendarComparision ()
		{
			RequiresIos8 ();

			NSDate today = NSDate.Now;
			NSDate todayPlusSeconds = NowPlusTenSeconds;
			if (NSCalendar.CurrentCalendar.CompareDate (today, todayPlusSeconds, NSCalendarUnit.Day) != NSComparisonResult.Same)
				Assert.Inconclusive ("Now plus 10 seconds isn't the same day, either a bug or run < 10 seconds before midnight");
			Assert.That (NSCalendar.CurrentCalendar.CompareDate (today, todayPlusSeconds, NSCalendarUnit.Second) == NSComparisonResult.Same, Is.False, "Now plus 10 seconds shouldn't be the same second");
		}

		[Test]
		public void TestCalendarComponents ()
		{
			RequiresIos8 ();

			nint era, year, month, day = -1;
			NSCalendar.CurrentCalendar.GetComponentsFromDate (out era, out year, out month, out day, NSDate.Now);
			Assert.That (era >= 0, Is.True, "GetComponentsFromDate - era");
			Assert.That (year >= 0, Is.True, "GetComponentsFromDate - year");
			Assert.That (month >= 0, Is.True, "GetComponentsFromDate - month");
			Assert.That (day >= 0, Is.True, "GetComponentsFromDate - day");

			nint weekOfYear, weekday = -1;
			era = year = -1;
			NSCalendar.CurrentCalendar.GetComponentsFromDateForWeekOfYear (out era, out year, out weekOfYear, out weekday, NSDate.Now);
			Assert.That (era >= 0, Is.True, "GetComponentsFromDateForWeekOfYear - era");
			Assert.That (year >= 0, Is.True, "GetComponentsFromDateForWeekOfYear - year");
			Assert.That (weekOfYear >= 0, Is.True, "GetComponentsFromDateForWeekOfYear - weekOfYear");
			Assert.That (weekday >= 0, Is.True, "GetComponentsFromDateForWeekOfYear - weekday");

			nint hour, minute, second, nanosecond = -1;
			NSCalendar.CurrentCalendar.GetHourComponentsFromDate (out hour, out minute, out second, out nanosecond, NSDate.Now);
			Assert.That (hour >= 0, Is.True, "GetHourComponentsFromDate - hour");
			Assert.That (minute >= 0, Is.True, "GetHourComponentsFromDate - minute");
			Assert.That (second >= 0, Is.True, "GetHourComponentsFromDate - second");
			Assert.That (nanosecond >= 0, Is.True, "GetHourComponentsFromDate - nanosecond");

			Assert.That (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Day, NSDate.Now) > 0, Is.True, "GetComponentFromDate - day");
			Assert.That (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Week, NSDate.Now) > 0, Is.True, "GetComponentFromDate - week");
			Assert.That (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Month, NSDate.Now) > 0, Is.True, "GetComponentFromDate - month");
			Assert.That (NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Year, NSDate.Now) > 0, Is.True, "GetComponentFromDate - year");
		}

		[Test]
		public void TestComponentsFromDateToDate ()
		{
			RequiresIos8 ();

			NSDateComponents todayComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, NSDate.Now);
			NSDateComponents tomorrowComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, Tomorrow);

			NSDateComponents components = NSCalendar.CurrentCalendar.ComponentsFromDateToDate (NSCalendarUnit.Day | NSCalendarUnit.Month, todayComponents, tomorrowComponents, NSCalendarOptions.None);
			Assert.That (components.Day, Is.EqualTo ((nint) 1), "One day passed between today and tomorrow");
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
			Assert.That (components.Hour != NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Hour, NSDate.Now).Hour, Is.True, "Different time zones should have different hours");
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

			Assert.That (futureMatch && pastMatch, Is.True, "10 seconds on both side of us should both be on same day or Inconclusive");
		}

		[Test]
		public void TestAddingByComponents ()
		{
			RequiresIos8 ();
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			NSDate now = NSDate.Now;
			NSDate oneDayFromNow = NSCalendar.CurrentCalendar.DateByAddingUnit (NSCalendarUnit.Day, 1, now, NSCalendarOptions.None);
			Assert.That (NSCalendar.CurrentCalendar.IsEqualToUnitGranularity (Tomorrow, oneDayFromNow, NSCalendarUnit.Day), Is.True, $"oneDayFromNow: DateByAddingUnit - One day from now should be tomorrow {Tomorrow} != {oneDayFromNow}");

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
			Assert.That (NSCalendar.CurrentCalendar.IsEqualToUnitGranularity (Tomorrow, todayPlusADay, NSCalendarUnit.Day), Is.True, $"todayPlusADay: lDateBySettingUnit - One day from now should be tomorrow {Tomorrow} != {todayPlusADay}");
		}

		[Test]
		public void TestSettingHourComponent ()
		{
			RequiresIos8 ();

			var currentHour = NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Hour, NSDate.Now);
			NSDate oneHourFromNow = NSCalendar.CurrentCalendar.DateBySettingsHour (currentHour + 1, 0, 0, NSDate.Now, NSCalendarOptions.None);
			if (oneHourFromNow is null)
				Assert.Inconclusive ("Test does not handle day change");
			Assert.That ((currentHour + 1) == NSCalendar.CurrentCalendar.GetComponentFromDate (NSCalendarUnit.Hour, oneHourFromNow), Is.True, "DateBySettingsHour - One hour from now should be one hour");
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
			Assert.That (date1, Is.Not.Null, "Date constructor 1");
			NSDate date2 = NSCalendar.CurrentCalendar.DateForWeekOfYear (1, 2, 3, 4, 5, 6, 7, 8);
			Assert.That (date2, Is.Not.Null, "Date constructor 2");
		}

		[Test]
		public void TestIsDateMethods ()
		{
			RequiresIos8 ();

			Assert.That (NSCalendar.CurrentCalendar.IsDateInToday (NSDate.Now), Is.True, "IsDateInToday positive");
			Assert.That (NSCalendar.CurrentCalendar.IsDateInToday (Tomorrow), Is.False, "IsDateInToday negative");

			Assert.That (NSCalendar.CurrentCalendar.IsDateInTomorrow (NSDate.Now), Is.False, "IsDateInTomorrow negative");
			Assert.That (NSCalendar.CurrentCalendar.IsDateInTomorrow (Tomorrow), Is.True, "IsDateInTomorrow positive");

			Assert.That (NSCalendar.CurrentCalendar.IsDateInYesterday (NSDate.Now), Is.False, "IsDateInYesterday negative");
			Assert.That (NSCalendar.CurrentCalendar.IsDateInYesterday (Yesterday), Is.True, "IsDateInYesterday positive");

			Assert.That (NSCalendar.CurrentCalendar.IsInSameDay (NSDate.Now, Tomorrow), Is.False, "IsInSameDay negative");

			NSDate weekend;
			double length;
			NSCalendar.CurrentCalendar.FindNextWeekend (out weekend, out length, NSCalendarOptions.None, NSDate.Now);
			Assert.That (NSCalendar.CurrentCalendar.IsDateInWeekend (weekend), Is.True, "IsDateInWeekend positive");
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

			Assert.That (NSCalendar.CurrentCalendar.CompareDate (weekend, weekend2, NSCalendarUnit.Day) == NSComparisonResult.Same, Is.True, "Weekend test");
		}

		[Test]
		public void TestStartOfDay ()
		{
			RequiresIos8 ();

			NSDate firstMomentOfToday = NSCalendar.CurrentCalendar.StartOfDayForDate (NSDate.Now);
			Assert.That (NSCalendar.CurrentCalendar.CompareDate (firstMomentOfToday, NSDate.Now, NSCalendarUnit.Day) == NSComparisonResult.Same, Is.True, "StartOfDayForDate");
		}

		[Test]
		public void TestFindNextDate ()
		{
			RequiresIos8 ();

			NSDateComponents nextYearComponent = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, NSDate.Now);
			nextYearComponent.Year++;

			NSDate nextYear = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, nextYearComponent, NSCalendarOptions.MatchNextTime);
			Assert.That (nextYear, Is.Not.Null, "FindNextDateAfterDateMatching");

			NSDate nextNoon = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, 12, 0, 0, NSCalendarOptions.MatchNextTime);
			Assert.That (nextNoon, Is.Not.Null, "FindNextDateAfterDateMatching 2");

			NSDate nextNoonAgain = NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, NSCalendarUnit.Hour, 12, NSCalendarOptions.MatchNextTime);
			Assert.That (nextNoonAgain, Is.Not.Null, "FindNextDateAfterDateMatching 3");
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
			Assert.That (delegateHit, Is.True, "EnumerateDatesStartingAfterDate delegate called");
		}

		[Test]
		public void TestNSDateComponentNewAPIs ()
		{
			RequiresIos8 ();

			NSDateComponents todayComponents = NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year | NSCalendarUnit.Era | NSCalendarUnit.Calendar, NSDate.Now);
			var nano = todayComponents.Nanosecond;
			Assert.That (todayComponents.IsValidDate, Is.True, "IsValidDate");
			Assert.That (todayComponents.IsValidDateInCalendar (NSCalendar.CurrentCalendar), Is.True, "IsValidDateInCalendar");

			todayComponents.SetValueForComponent (12, NSCalendarUnit.Day);
			Assert.That (todayComponents.GetValueForComponent (NSCalendarUnit.Day), Is.EqualTo ((nint) 12), "GetValueForComponent\\SetValueForComponent");
		}

		[Test]
		public void TestFindNextDateAfterDateMatching ()
		{
			RequiresIos8 ();

			NSDateComponents nextYearComponent = new NSDateComponents ();
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				Assert.That (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, nextYearComponent, NSCalendarOptions.None), Is.Null, "nextYearComponent");

				Assert.That (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, NSCalendarUnit.Day, 8, NSCalendarOptions.None), Is.Not.Null, "Unit");

				Assert.That (NSCalendar.CurrentCalendar.FindNextDateAfterDateMatching (NSDate.Now, 1, 2, 3, NSCalendarOptions.None), Is.Not.Null, "components");
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
			Assert.That (range.Location, Is.EqualTo ((nint) location));
			Assert.That (range.Length, Is.EqualTo ((nint) length));
		}

		[TestCase (1, 12, NSCalendarUnit.Month)]
		[TestCase (1, 31, NSCalendarUnit.Day)]
		[TestCase (0, 24, NSCalendarUnit.Hour)]
		public void TestMaximumRange (int location, int length, NSCalendarUnit unit)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var range = cal.MaximumRange (unit);
			Assert.That (range.Length, Is.EqualTo ((nint) length));
			Assert.That (range.Location, Is.EqualTo ((nint) location));
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
			Assert.That (range.Location, Is.EqualTo ((nint) location));
			Assert.That (range.Length, Is.EqualTo ((nint) length));
		}

		[TestCase (2010, 1, 11, NSCalendarUnit.Day, NSCalendarUnit.Month, 11)]
		[TestCase (2010, 4, 15, NSCalendarUnit.Day, NSCalendarUnit.Month, 15)]
		public void TestOrdinality (int year, int month, int day, NSCalendarUnit smaller, NSCalendarUnit larger, int expected)
		{
			var cal = new NSCalendar (NSCalendarType.Gregorian);
			var date = new DateTime (year, month, day, 0, 0, 0, DateTimeKind.Utc);
			var dt = (NSDate) date;
			cal.TimeZone = NSTimeZone.FromName ("Europe/Madrid");
			Assert.That (cal.Ordinality (smaller, larger, dt), Is.EqualTo ((nuint) expected), $"Ordinality");
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
			Assert.That (outInterval, Is.EqualTo (expectedInterval));
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
			Assert.That (outInterval, Is.EqualTo (expectedInterval));
		}

	}
}
