
using System.Diagnostics;

using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MatrixFloat4x4Test {
		[Test]
		public void Identity ()
		{
			var identity = new MatrixFloat4x4 {
				M11 = 1f,
				M22 = 1f,
				M33 = 1f,
				M44 = 1f,
			};
			Asserts.AreEqual (identity, MatrixFloat4x4.Identity, "identity");
		}
	}
}
