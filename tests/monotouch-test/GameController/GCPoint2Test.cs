using System;

using Foundation;
using GameController;

using NUnit.Framework;

namespace MonoTouchFixtures.GameController {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GCPoint2Test {
		[Test]
		public void TheTest ()
		{
			Assert.Multiple (() => {
				var pnt = new GCPoint2 (1, 2);
				Assert.That (pnt.X, Is.EqualTo (1), "X");
				Assert.That (pnt.Y, Is.EqualTo (2), "Y");

				pnt.X = 3;
				Assert.That (pnt.X, Is.EqualTo (3), "X#2");
				Assert.That (pnt.Y, Is.EqualTo (2), "Y#2");

				pnt.Y = 4;
				Assert.That (pnt.X, Is.EqualTo (3), "X#3");
				Assert.That (pnt.Y, Is.EqualTo (4), "Y#3");

				Assert.That (GCPoint2.Zero.X, Is.EqualTo (0), "Z.X");
				Assert.That (GCPoint2.Zero.Y, Is.EqualTo (0), "Z.Y");

				Assert.That (pnt == GCPoint2.Zero, Is.EqualTo (false), "== A");
				Assert.That (pnt != GCPoint2.Zero, Is.EqualTo (true), "!= A");

				var pnt2 = new GCPoint2 (3, 4);
				Assert.That (pnt == pnt2, Is.EqualTo (true), "== B");
				Assert.That (pnt != pnt2, Is.EqualTo (false), "!= B");

				Assert.That (pnt.IsEmpty, Is.EqualTo (false), "IsEmpty A");
				Assert.That (GCPoint2.Zero.IsEmpty, Is.EqualTo (true), "IsEmpty B");

				var pnt3 = new GCPoint2 (pnt2);
				Assert.That (pnt3.X, Is.EqualTo (3), "X#4");
				Assert.That (pnt3.Y, Is.EqualTo (4), "Y#4");

				Assert.That (pnt.Equals ((object) pnt3), Is.EqualTo (true), "Equals A");
				Assert.That (pnt.Equals ((object) GCPoint2.Zero), Is.EqualTo (false), "Equals B");

				Assert.That (pnt.Equals (pnt3), Is.EqualTo (true), "Equals C");
				Assert.That (pnt.Equals (GCPoint2.Zero), Is.EqualTo (false), "Equals D");

				pnt.Deconstruct (out var x, out var y);
				Assert.That (x, Is.EqualTo ((nfloat) 3), "X#5");
				Assert.That (y, Is.EqualTo ((nfloat) 4), "Y#5");

				Assert.AreEqual (pnt.ToString (), "{3, 4}", "ToString A");
				Assert.AreEqual (GCPoint2.Zero.ToString (), "{0, 0}", "ToString B");
			});
		}
	}
}
