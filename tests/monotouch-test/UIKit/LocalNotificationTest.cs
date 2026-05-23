//
// Unit tests for UILocalNotification
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LocalNotificationTest {

		[Test]
		public void DefaultValues ()
		{
			using (var def = new UILocalNotification ()) {
				Assert.That (def.FireDate, Is.Null, "FireDate");
				Assert.That (def.TimeZone, Is.Null, "TimeZone");
				Assert.That ((nuint) (ulong) def.RepeatInterval, Is.EqualTo ((nuint) 0), "RepeatInterval"); // documented to be 0, which is not in the enum.
				Assert.That (def.RepeatCalendar, Is.Null, "RepeatCalendar");
				Assert.That (def.AlertBody, Is.Null, "AlertBody");
				Assert.That (def.HasAction, Is.True, "HasAction");
				Assert.That (def.AlertAction, Is.Null, "AlertAction");
				Assert.That (def.AlertLaunchImage, Is.Null, "AlertLaunchImage");
				Assert.That (def.SoundName, Is.Null, "SoundName");
				Assert.That (def.ApplicationIconBadgeNumber, Is.EqualTo ((nint) 0), "ApplicationIconBadgeNumber");
				Assert.That (def.UserInfo, Is.Null, "UserInfo");
			}
		}

		[Test]
		public void NullValues ()
		{
			using (var def = new UILocalNotification ()) {
				def.FireDate = null;
				def.FireDate = new NSDate ();
				Assert.That (def.FireDate, Is.Not.Null, "FireDate NN");
				def.FireDate = null;
				Assert.That (def.FireDate, Is.Null, "FireDate N");

				def.TimeZone = null;
				def.TimeZone = new NSTimeZone ("GMT");
				Assert.That (def.TimeZone, Is.Not.Null, "TimeZone NN");
				def.TimeZone = null;
				Assert.That (def.TimeZone, Is.Null, "TimeZone N");

				def.RepeatInterval = NSCalendarUnit.Calendar;
				Assert.That (def.RepeatInterval, Is.EqualTo (NSCalendarUnit.Calendar), "RepeatInterval 1");
				def.RepeatInterval = (NSCalendarUnit) 0;
				Assert.That (def.RepeatInterval, Is.EqualTo ((NSCalendarUnit) 0), "RepeatInterval 2");

				def.RepeatCalendar = null;
				def.RepeatCalendar = new NSCalendar (NSCalendarType.Hebrew);
				Assert.That (def.RepeatCalendar, Is.Not.Null, "RepeatCalendar NN");
				def.RepeatCalendar = null;
				Assert.That (def.RepeatCalendar, Is.Null, "RepeatCalendar N");

				def.AlertBody = null;
				def.AlertBody = "body";
				Assert.That (def.AlertBody, Is.EqualTo ("body"), "AlertBody NN");
				def.AlertBody = null;
				Assert.That (def.AlertBody, Is.Null, "AlertBody N");

				def.AlertAction = null;
				def.AlertAction = "action";
				Assert.That (def.AlertAction, Is.EqualTo ("action"), "AlertAction NN");
				def.AlertAction = null;
				Assert.That (def.AlertAction, Is.Null, "AlertAction N");

				def.AlertLaunchImage = null;
				def.AlertLaunchImage = "image";
				Assert.That (def.AlertLaunchImage, Is.EqualTo ("image"), "AlertLaunchImage NN");
				def.AlertLaunchImage = null;
				Assert.That (def.AlertLaunchImage, Is.Null, "AlertLaunchImage N");

				def.SoundName = null;
				def.SoundName = "sound";
				Assert.That (def.SoundName, Is.EqualTo ("sound"), "SoundName NN");
				def.SoundName = null;
				Assert.That (def.SoundName, Is.Null, "SoundName N");

				def.UserInfo = null;
				def.UserInfo = new NSDictionary ();
				Assert.That (def.UserInfo, Is.Not.Null, "UserInfo NN");
				def.UserInfo = null;
				Assert.That (def.UserInfo, Is.Null, "UserInfo N");
			}
		}
	}
}

#endif // !__TVOS__
