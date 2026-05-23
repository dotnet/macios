using System.Drawing;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FormatterTests {

		NSDateComponentsFormatter dateComponentsFormatter;
		NSEnergyFormatter energyFormatter;

		void RequiresIos8 ()
		{
			TestRuntime.AssertXcodeVersion (6, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			if (dateComponentsFormatter is null)
				dateComponentsFormatter = new NSDateComponentsFormatter ();
			if (energyFormatter is null)
				energyFormatter = new NSEnergyFormatter ();
		}

		static void TestFormattedString (string formattedString, string testName)
		{
			Assert.That (formattedString, Is.Not.Null, testName);
			Assert.That (formattedString.Length > 0, Is.True, testName + " length");
		}

		public NSDateComponents NowComponents {
			get {
				return NSCalendar.CurrentCalendar.Components (NSCalendarUnit.Era | NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year | NSCalendarUnit.WeekOfMonth, NSDate.Now);
			}
		}

		#region NSDateComponentsFormatter

		[Test]
		public void DateTestProperties ()
		{
			RequiresIos8 ();

			dateComponentsFormatter.UnitsStyle = NSDateComponentsFormatterUnitsStyle.Full;
			Assert.That (dateComponentsFormatter.UnitsStyle, Is.EqualTo (NSDateComponentsFormatterUnitsStyle.Full), "UnitsStyle");

			dateComponentsFormatter.AllowedUnits = NSCalendarUnit.Month | NSCalendarUnit.Day;
			Assert.That (dateComponentsFormatter.AllowedUnits, Is.EqualTo (NSCalendarUnit.Month | NSCalendarUnit.Day), "AllowedUnits");

			dateComponentsFormatter.ZeroFormattingBehavior = NSDateComponentsFormatterZeroFormattingBehavior.Pad;
			Assert.That (dateComponentsFormatter.ZeroFormattingBehavior, Is.EqualTo (NSDateComponentsFormatterZeroFormattingBehavior.Pad), "ZeroFormattingBehavior");

			NSCalendar c = new NSCalendar (NSCalendarType.Buddhist);
			Assert.That (dateComponentsFormatter.Calendar, Is.Not.Null);
			dateComponentsFormatter.Calendar = c;
			Assert.That (dateComponentsFormatter.Calendar.Identifier, Is.EqualTo (c.Identifier), "Calendar");

			dateComponentsFormatter.AllowsFractionalUnits = true;
			Assert.That (dateComponentsFormatter.AllowsFractionalUnits, Is.True, "AllowsFractionalUnits");

			dateComponentsFormatter.MaximumUnitCount = 50;
			Assert.That (dateComponentsFormatter.MaximumUnitCount, Is.EqualTo ((nint) 50), "MaximumUnitCount");

			dateComponentsFormatter.CollapsesLargestUnit = true;
			Assert.That (dateComponentsFormatter.CollapsesLargestUnit, Is.True, "CollapsesLargestUnit");

			dateComponentsFormatter.IncludesApproximationPhrase = true;
			Assert.That (dateComponentsFormatter.IncludesApproximationPhrase, Is.True, "IncludesApproximationPhrase");

			dateComponentsFormatter.IncludesTimeRemainingPhrase = true;
			Assert.That (dateComponentsFormatter.IncludesTimeRemainingPhrase, Is.True, "IncludesTimeRemainingPhrase");

			Assert.That (dateComponentsFormatter.FormattingContext, Is.Not.Null);
			dateComponentsFormatter.FormattingContext = new NSFormattingContext ();
		}

		[Test]
		public void DateStringForObjectValue ()
		{
			RequiresIos8 ();

			string formattedString = dateComponentsFormatter.StringForObjectValue (NowComponents);
			TestFormattedString (formattedString, "StringForObjectValue");
		}

		[Test]
		public void DateStringFromDateComponents ()
		{
			RequiresIos8 ();

			string formattedString = dateComponentsFormatter.StringFromDateComponents (NowComponents);
			TestFormattedString (formattedString, "StringFromDateComponents");
		}

		[Test]
		public void DateStringFromDate ()
		{
			RequiresIos8 ();

			dateComponentsFormatter.AllowedUnits = NSCalendarUnit.Second; // New default allowed units won't give us second resolution
			string formattedString = dateComponentsFormatter.StringFromDate (NSDate.Now, NSDate.Now.AddSeconds (60));
			TestFormattedString (formattedString, "StringFromDate");
		}

		[Test]
		public void DateStringFromTimeInterval ()
		{
			RequiresIos8 ();

			string formattedString = dateComponentsFormatter.StringFromTimeInterval (1.5);
			TestFormattedString (formattedString, "StringFromTimeInterval");
		}

		[Test]
		public void DateLocalizedStringFromDateComponents ()
		{
			RequiresIos8 ();

			string formattedString = NSDateComponentsFormatter.LocalizedStringFromDateComponents (NowComponents, NSDateComponentsFormatterUnitsStyle.Full);
			TestFormattedString (formattedString, "LocalizedStringFromDateComponents");
		}

		[Test]
		public void DateGetObjectValueTest ()
		{
			RequiresIos8 ();

			// NSDateComponentsFormatter currently only implements formatting, not parsing. Until it implements parsing, this will always return NO.

			NSObject o;
			string e;
			bool value = dateComponentsFormatter.GetObjectValue (out o, string.Empty, out e);
			Assert.That (value, Is.False, "DateGetObjectValueTest"); // If this ever returns true, we need to write a better test
		}

		#endregion

		#region NSEnergyFormatter

		[Test]
		public void EnergyEnergyStringFromValue ()
		{
			RequiresIos8 ();

			string formattedString = energyFormatter.StringFromValue (2.0, NSEnergyFormatterUnit.Kilocalorie);
			TestFormattedString (formattedString, "EnergyStringFromValue");
		}

		[Test]
		public void EnergyEnergyStringFromJoules ()
		{
			RequiresIos8 ();

			string formattedString = energyFormatter.StringFromJoules (2.0);
			TestFormattedString (formattedString, "StringFromJoules");
		}

		[Test]
		public void EnergyUnitStringFromValue ()
		{
			RequiresIos8 ();

			string formattedString = energyFormatter.UnitStringFromValue (2.0, NSEnergyFormatterUnit.Kilojoule);
			TestFormattedString (formattedString, "UnitStringFromValue");
		}

		[Test]
		public void EnergyUnitStringFromJoules ()
		{
			RequiresIos8 ();

			NSEnergyFormatterUnit unit;
			string formattedString = energyFormatter.UnitStringFromJoules (2.0, out unit);
			TestFormattedString (formattedString, "UnitStringFromJoules");
			Assert.That ((int) unit > 0, Is.True); // We got some value from the API
		}

		[Test]
		public void EnergyGetObjectValue ()
		{
			RequiresIos8 ();
			// No parsing is supported. This method will return NO.

			NSObject o;
			string e;
			bool value = energyFormatter.GetObjectValue (out o, string.Empty, out e);
			Assert.That (value, Is.False, "EnergyGetObjectValue"); // If this ever returns true, we need to write a better test
		}

		#endregion

		#region NSDateIntervalFormatter

		[Test]
		public void DateIntervalStringFromDate ()
		{
			RequiresIos8 ();

			using (var dateIntervalFormatter = new NSDateIntervalFormatter ()) {
				string formattedString = dateIntervalFormatter.StringFromDate (NSDate.Now.AddSeconds (-60 * 60 * 24), NSDate.Now);
				TestFormattedString (formattedString, "DateIntervalStringFromDate");
			}
		}

		#endregion
	}
}

