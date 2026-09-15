
namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSUrlSessionConfigurationTest {

		[Test]
		public void TestSessionTypeDefault ()
		{
			using (var config = NSUrlSessionConfiguration.DefaultSessionConfiguration)
				Assert.That (config.SessionType, Is.EqualTo (NSUrlSessionConfiguration.SessionConfigurationType.Default));
		}

		[Test]
		public void TestSessionTypeBackground ()
		{
			using (var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration ("my.identifier.test"))
				Assert.That (config.SessionType, Is.EqualTo (NSUrlSessionConfiguration.SessionConfigurationType.Background));
		}

		[Test]
		public void TestSessionTypeEphemeral ()
		{
			using (var config = NSUrlSessionConfiguration.EphemeralSessionConfiguration)
				Assert.That (config.SessionType, Is.EqualTo (NSUrlSessionConfiguration.SessionConfigurationType.Ephemeral));
		}
	}
}
