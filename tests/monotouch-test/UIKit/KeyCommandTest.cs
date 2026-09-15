#if __IOS__

using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class KeyCommandTest {

		[Test]
		public void Create ()
		{
			using (var key = new NSString ("a")) {
				Assert.That (UIKeyCommand.Create (key, UIKeyModifierFlags.Command, new Selector ("foo")), Is.Not.Null, "Create");
			}
		}
	}
}

#endif
