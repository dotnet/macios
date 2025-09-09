
using System;
using System.Diagnostics;

using Foundation;
using ObjCRuntime;

using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;

using NUnit.Framework;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MatrixFloat2x2Test {
		[Test]
		public void Identity ()
		{
			var identity = new MatrixFloat2x2 {
				R0C0 = 1f,
				R1C1 = 1f,
			};
			Asserts.AreEqual (identity, MatrixFloat2x2.Identity, "identity");
		}

		[Test]
		public void ToStringTest ()
		{
			var actual = new MatrixFloat2x2 (1, 2, 3, 4);

			Assert.AreEqual ("(1, 2)\n(3, 4)", actual.ToString (), "tostring");
		}
	}
}
