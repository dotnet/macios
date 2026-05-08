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
				ClassicAssert.IsNull (def.FireDate, "FireDate");
				ClassicAssert.IsNull (def.TimeZone, "TimeZone");
				Assert.That ((nuint) (ulong) def.RepeatInterval, Is.EqualTo ((nuint) 0), "RepeatInterval"); // documented to be 0, which is not in the enum.
				ClassicAssert.IsNull (def.RepeatCalendar, "RepeatCalendar");
				ClassicAssert.IsNull (def.AlertBody, "AlertBody");
				ClassicAssert.IsTrue (def.HasAction, "HasAction");
				ClassicAssert.IsNull (def.AlertAction, "AlertAction");
				ClassicAssert.IsNull (def.AlertLaunchImage, "AlertLaunchImage");
				ClassicAssert.IsNull (def.SoundName, "SoundName");
				Assert.That (def.ApplicationIconBadgeNumber, Is.EqualTo ((nint) 0), "ApplicationIconBadgeNumber");
				ClassicAssert.IsNull (def.UserInfo, "UserInfo");
			}
		}

		[Test]
		public void NullValues ()
		{
			using (var def = new UILocalNotification ()) {
				def.FireDate = null;
				def.FireDate = new NSDate ();
				ClassicAssert.IsNotNull (def.FireDate, "FireDate NN");
				def.FireDate = null;
				ClassicAssert.IsNull (def.FireDate, "FireDate N");

				def.TimeZone = null;
				def.TimeZone = new NSTimeZone ("GMT");
				ClassicAssert.IsNotNull (def.TimeZone, "TimeZone NN");
				def.TimeZone = null;
				ClassicAssert.IsNull (def.TimeZone, "TimeZone N");

				def.RepeatInterval = NSCalendarUnit.Calendar;
				Assert.That (def.RepeatInterval, Is.EqualTo (NSCalendarUnit.Calendar), "RepeatInterval 1");
				def.RepeatInterval = (NSCalendarUnit) 0;
				Assert.That (def.RepeatInterval, Is.EqualTo ((NSCalendarUnit) 0), "RepeatInterval 2");

				def.RepeatCalendar = null;
				def.RepeatCalendar = new NSCalendar (NSCalendarType.Hebrew);
				ClassicAssert.IsNotNull (def.RepeatCalendar, "RepeatCalendar NN");
				def.RepeatCalendar = null;
				ClassicAssert.IsNull (def.RepeatCalendar, "RepeatCalendar N");

				def.AlertBody = null;
				def.AlertBody = "body";
				ClassicAssert.AreEqual ("body", def.AlertBody, "AlertBody NN");
				def.AlertBody = null;
				ClassicAssert.IsNull (def.AlertBody, "AlertBody N");

				def.AlertAction = null;
				def.AlertAction = "action";
				ClassicAssert.AreEqual ("action", def.AlertAction, "AlertAction NN");
				def.AlertAction = null;
				ClassicAssert.IsNull (def.AlertAction, "AlertAction N");

				def.AlertLaunchImage = null;
				def.AlertLaunchImage = "image";
				ClassicAssert.AreEqual ("image", def.AlertLaunchImage, "AlertLaunchImage NN");
				def.AlertLaunchImage = null;
				ClassicAssert.IsNull (def.AlertLaunchImage, "AlertLaunchImage N");

				def.SoundName = null;
				def.SoundName = "sound";
				ClassicAssert.AreEqual ("sound", def.SoundName, "SoundName NN");
				def.SoundName = null;
				ClassicAssert.IsNull (def.SoundName, "SoundName N");

				def.UserInfo = null;
				def.UserInfo = new NSDictionary ();
				ClassicAssert.IsNotNull (def.UserInfo, "UserInfo NN");
				def.UserInfo = null;
				ClassicAssert.IsNull (def.UserInfo, "UserInfo N");
			}
		}
	}
}

#endif // !__TVOS__
