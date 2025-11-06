//
// Unit tests for CGSize
//
// Authors:
//	Sebastien Pouliot < sebastien.pouliot@gmail.com>
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SizeTest {

		[Test]
		public void ToStringTest ()
		{
			var size = new CGSize ((nfloat) 1, (nfloat) 2);
			Assert.AreEqual ("{1, 2}", size.ToString (), "ToString");
		}
	}
}
