
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
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Year, $"Year");
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Month, "Month");
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Day, "Day");
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Hour, "Hour");
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Minute, "Minute");
			ClassicAssert.AreEqual (NSDateComponents.Undefined, components.Second, "Second");
		}
	}
}
