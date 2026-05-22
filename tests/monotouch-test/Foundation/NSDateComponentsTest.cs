
namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSDateComponentsTest {

		[Test]
		public void TestUndefinedComponent ()
		{
			// as per documentation:
			// "When a new instance of NSDateComponents is created, 
			// the date components are set to 
			// NSDateComponentUndefined." 
			// we simply test that the values are undefined
			var components = new NSDateComponents ();
			Assert.That (components.Year, Is.EqualTo (NSDateComponents.Undefined), $"Year");
			Assert.That (components.Month, Is.EqualTo (NSDateComponents.Undefined), "Month");
			Assert.That (components.Day, Is.EqualTo (NSDateComponents.Undefined), "Day");
			Assert.That (components.Hour, Is.EqualTo (NSDateComponents.Undefined), "Hour");
			Assert.That (components.Minute, Is.EqualTo (NSDateComponents.Undefined), "Minute");
			Assert.That (components.Second, Is.EqualTo (NSDateComponents.Undefined), "Second");
		}
	}
}
