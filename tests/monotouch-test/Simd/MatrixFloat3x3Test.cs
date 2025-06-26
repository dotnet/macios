
using System;
using System.Diagnostics;

using Foundation;
using ObjCRuntime;

using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;

using NUnit.Framework;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MatrixFloat3x3Test {
		[Test]
		public void Identity ()
		{
			var identity = new MatrixFloat3x3 {
				R0C0 = 1f,
				R1C1 = 1f,
				R2C2 = 1f,
			};
			Asserts.AreEqual (identity, MatrixFloat3x3.Identity, "identity");
		}

		[Test]
		public void ToStringTest ()
		{
			var actual = new MatrixFloat3x3 (1, 2, 3, 4, 5, 6, 7, 8, 9);

			Assert.AreEqual ("(1, 2, 3)\n(4, 5, 6)\n(7, 8, 9)", actual.ToString (), "tostring");
		}
	}
}
