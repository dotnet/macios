//
// Unit tests for CGSize
//
// Authors:
//	Sebastien Pouliot < sebastien.pouliot@gmail.com>
//

using System;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using NUnit.Framework;

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
