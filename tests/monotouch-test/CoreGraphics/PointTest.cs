//
// Unit tests for CGPoint
//
// Authors:
//	Sebastien Pouliot < sebastien.pouliot@gmail.com>
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PointTest {

		[Test]
		public void ToStringTest ()
		{
			var point = new CGPoint ((nfloat) 1, (nfloat) 2);
			Assert.AreEqual ("{1, 2}", point.ToString (), "ToString");
		}
	}
}
