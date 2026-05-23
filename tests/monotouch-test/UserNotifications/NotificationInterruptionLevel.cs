
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
			Assert.That ((int) UNNotificationInterruptionLevel.Passive2, Is.EqualTo (0));
			Assert.That ((int) UNNotificationInterruptionLevel.Active2, Is.EqualTo (1));
			Assert.That ((int) UNNotificationInterruptionLevel.TimeSensitive2, Is.EqualTo (2));
			Assert.That ((int) UNNotificationInterruptionLevel.Critical2, Is.EqualTo (3));
#endif
		}
	}
}
