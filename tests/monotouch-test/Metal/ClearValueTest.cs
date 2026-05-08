
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

			ClassicAssert.AreEqual (0, value.Color.Alpha, "1-color-alpha");
			ClassicAssert.AreEqual (0, value.Color.Blue, "1-color-blue");
			ClassicAssert.AreEqual (0, value.Color.Green, "1-color-green");
			ClassicAssert.AreEqual (0, value.Color.Red, "1-color-red");
			ClassicAssert.AreEqual (0, value.Depth, "1-depth");
			ClassicAssert.AreEqual (0, value.Stencil, "1-stencil");

			value = new MTLClearValue (0.2f);

			ClassicAssert.AreEqual (0.2f, value.Depth, "2-depth");

			value = new MTLClearValue (123);

			ClassicAssert.AreEqual (123, value.Stencil, "3-stencil");

			value = new MTLClearValue (-2);

			ClassicAssert.AreEqual (-2, value.Depth, "4-depth");

			value = new MTLClearValue (new MTLClearColor (1, 2, 3, 4));

			ClassicAssert.AreEqual (4, value.Color.Alpha, "5-color-alpha");
			ClassicAssert.AreEqual (3, value.Color.Blue, "5-color-blue");
			ClassicAssert.AreEqual (2, value.Color.Green, "5-color-green");
			ClassicAssert.AreEqual (1, value.Color.Red, "5-color-red");
		}
	}
}
