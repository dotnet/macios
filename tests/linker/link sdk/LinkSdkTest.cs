
namespace LinkSdkTests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LinkSdkTest {
		static void Check (string calendarName, bool present)
		{
			var type = Type.GetType ("System.Globalization." + calendarName);
			bool success = present == (type is not null);
			Assert.That (type is not null, Is.EqualTo (present).Within (calendarName));
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
