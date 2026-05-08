
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
			Assert.That (0, Is.EqualTo ((int) UNNotificationInterruptionLevel.Passive2));
			Assert.That (1, Is.EqualTo ((int) UNNotificationInterruptionLevel.Active2));
			Assert.That (2, Is.EqualTo ((int) UNNotificationInterruptionLevel.TimeSensitive2));
			Assert.That (3, Is.EqualTo ((int) UNNotificationInterruptionLevel.Critical2));
#endif
		}
	}
}
