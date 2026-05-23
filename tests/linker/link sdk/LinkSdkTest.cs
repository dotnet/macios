
namespace LinkSdkTests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LinkSdkTest {
		static void Check (string calendarName, bool present)
		{
			var type = Type.GetType ("System.Globalization." + calendarName);
			if (present)
				Assert.That (type, Is.Not.Null, calendarName);
			else
				Assert.That (type, Is.Null, calendarName);
		}

		[Test]
		public void Calendars ()
		{
			Check ("GregorianCalendar", true);
			// because project options enabled them
			Check ("UmAlQuraCalendar", true);
			Check ("HijriCalendar", true);
			Check ("ThaiBuddhistCalendar", true);
		}
	}
}
