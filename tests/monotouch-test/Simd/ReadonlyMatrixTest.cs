// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreGraphics;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ReadonlyMatrixTest {
		[Test]
		public void Matrix2 ()
		{
			var matrix = new NMatrix2 (1, 2, 3, 4);
			var expected = matrix;
			var identity = NMatrix2.Identity;
#if XAMCORE_5_0
			NMatrix2.Multiply (in matrix, in identity, out var product);
			NMatrix2.Transpose (in matrix, out var transposed);
#else
			NMatrix2.Multiply (ref matrix, ref identity, out var product);
			NMatrix2.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expected), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix2 (1, 3, 2, 4)), "Transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (identity, Is.EqualTo (NMatrix2.Identity), "Unchanged identity");
		}

		[Test]
		public void Matrix3 ()
		{
			var matrix = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
			var expected = matrix;
			var identity = NMatrix3.Identity;
#if XAMCORE_5_0
			NMatrix3.Multiply (in matrix, in identity, out var product);
			NMatrix3.Transpose (in matrix, out var transposed);
#else
			NMatrix3.Multiply (ref matrix, ref identity, out var product);
			NMatrix3.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expected), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix3 (1, 4, 7, 2, 5, 8, 3, 6, 9)), "Transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (identity, Is.EqualTo (NMatrix3.Identity), "Unchanged identity");
		}

		[Test]
		public void Matrix4 ()
		{
			var matrix = new NMatrix4 (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			var expected = matrix;
			var identity = NMatrix4.Identity;
#if XAMCORE_5_0
			NMatrix4.Multiply (in matrix, in identity, out var product);
			NMatrix4.Transpose (in matrix, out var transposed);
#else
			NMatrix4.Multiply (ref matrix, ref identity, out var product);
			NMatrix4.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expected), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix4 (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (identity, Is.EqualTo (NMatrix4.Identity), "Unchanged identity");
		}

		[Test]
		public void Matrix4d ()
		{
			var matrix = new NMatrix4d (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			var expected = matrix;
			var identity = NMatrix4d.Identity;
#if XAMCORE_5_0
			NMatrix4d.Multiply (in matrix, in identity, out var product);
			NMatrix4d.Transpose (in matrix, out var transposed);
#else
			NMatrix4d.Multiply (ref matrix, ref identity, out var product);
			NMatrix4d.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expected), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix4d (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (identity, Is.EqualTo (NMatrix4d.Identity), "Unchanged identity");
		}
	}
}
