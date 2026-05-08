
using UserNotifications;

namespace MonoTouchFixtures.UserNotifications {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UNNotificationInterruptionLevelTest {

		[Test]
		public void EnumTest ()
		{
#if !XAMCORE_5_0
			/* Apple Docs order these enum values as:
			typedef NS_ENUM (NSUInteger, UNNotificationInterruptionLevel)
			{
				UNNotificationInterruptionLevelPassive,
				UNNotificationInterruptionLevelActive,
				UNNotificationInterruptionLevelTimeSensitive,
				UNNotificationInterruptionLevelCritical,
			} */
			ClassicAssert.AreEqual ((int) UNNotificationInterruptionLevel.Passive2, 0);
			ClassicAssert.AreEqual ((int) UNNotificationInterruptionLevel.Active2, 1);
			ClassicAssert.AreEqual ((int) UNNotificationInterruptionLevel.TimeSensitive2, 2);
			ClassicAssert.AreEqual ((int) UNNotificationInterruptionLevel.Critical2, 3);
#endif
		}
	}
}
