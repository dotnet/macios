
using Metal;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ClearValueTest {
		[Test]
		public void Constructor ()
		{
			MTLClearValue value;

			value = new MTLClearValue ();

			Assert.That (value.Color.Alpha, Is.EqualTo (0), "1-color-alpha");
			Assert.That (value.Color.Blue, Is.EqualTo (0), "1-color-blue");
			Assert.That (value.Color.Green, Is.EqualTo (0), "1-color-green");
			Assert.That (value.Color.Red, Is.EqualTo (0), "1-color-red");
			Assert.That (value.Depth, Is.EqualTo (0), "1-depth");
			Assert.That (value.Stencil, Is.EqualTo (0), "1-stencil");

			value = new MTLClearValue (0.2f);

			Assert.That (value.Depth, Is.EqualTo (0.2f), "2-depth");

			value = new MTLClearValue (123);

			Assert.That (value.Stencil, Is.EqualTo (123), "3-stencil");

			value = new MTLClearValue (-2);

			Assert.That (value.Depth, Is.EqualTo (-2), "4-depth");

			value = new MTLClearValue (new MTLClearColor (1, 2, 3, 4));

			Assert.That (value.Color.Alpha, Is.EqualTo (4), "5-color-alpha");
			Assert.That (value.Color.Blue, Is.EqualTo (3), "5-color-blue");
			Assert.That (value.Color.Green, Is.EqualTo (2), "5-color-green");
			Assert.That (value.Color.Red, Is.EqualTo (1), "5-color-red");
		}
	}
}
