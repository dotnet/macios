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
			var other = new NMatrix3 (9, 8, 7, 6, 5, 4, 3, 2, 1);
			var originalOther = other;
			var expectedProduct = new NMatrix3 (30, 24, 18, 84, 69, 54, 138, 114, 90);
#if XAMCORE_5_0
			NMatrix3.Multiply (in matrix, in other, out var product);
			NMatrix3.Transpose (in matrix, out var transposed);
			Assert.That (NMatrix3.Multiply (in matrix, in other), Is.EqualTo (expectedProduct), "Readonly product");
			Assert.That (NMatrix3.Transpose (in matrix), Is.EqualTo (new NMatrix3 (1, 4, 7, 2, 5, 8, 3, 6, 9)), "Readonly transpose");
#else
			NMatrix3.Multiply (ref matrix, ref other, out var product);
			NMatrix3.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expectedProduct), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix3 (1, 4, 7, 2, 5, 8, 3, 6, 9)), "Transpose");
			Assert.That (NMatrix3.Multiply (matrix, other), Is.EqualTo (expectedProduct), "Value product");
			Assert.That (matrix * other, Is.EqualTo (expectedProduct), "Operator product");
			Assert.That (NMatrix3.Transpose (matrix), Is.EqualTo (transposed), "Value transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (other, Is.EqualTo (originalOther), "Unchanged other");
		}

		[Test]
		public void Matrix4 ()
		{
			var matrix = new NMatrix4 (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			var expected = matrix;
			var other = new NMatrix4 (16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
			var originalOther = other;
			var expectedProduct = new NMatrix4 (80, 70, 60, 50, 240, 214, 188, 162, 400, 358, 316, 274, 560, 502, 444, 386);
#if XAMCORE_5_0
			NMatrix4.Multiply (in matrix, in other, out var product);
			NMatrix4.Transpose (in matrix, out var transposed);
			Assert.That (NMatrix4.Multiply (in matrix, in other), Is.EqualTo (expectedProduct), "Readonly product");
			Assert.That (NMatrix4.Transpose (in matrix), Is.EqualTo (new NMatrix4 (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Readonly transpose");
#else
			NMatrix4.Multiply (ref matrix, ref other, out var product);
			NMatrix4.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expectedProduct), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix4 (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Transpose");
			Assert.That (NMatrix4.Multiply (matrix, other), Is.EqualTo (expectedProduct), "Value product");
			Assert.That (matrix * other, Is.EqualTo (expectedProduct), "Operator product");
			Assert.That (NMatrix4.Transpose (matrix), Is.EqualTo (transposed), "Value transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (other, Is.EqualTo (originalOther), "Unchanged other");
		}

		[Test]
		public void Matrix4d ()
		{
			var matrix = new NMatrix4d (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			var expected = matrix;
			var other = new NMatrix4d (16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
			var originalOther = other;
			var expectedProduct = new NMatrix4d (80, 70, 60, 50, 240, 214, 188, 162, 400, 358, 316, 274, 560, 502, 444, 386);
#if XAMCORE_5_0
			NMatrix4d.Multiply (in matrix, in other, out var product);
			NMatrix4d.Transpose (in matrix, out var transposed);
			Assert.That (NMatrix4d.Multiply (in matrix, in other), Is.EqualTo (expectedProduct), "Readonly product");
			Assert.That (NMatrix4d.Transpose (in matrix), Is.EqualTo (new NMatrix4d (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Readonly transpose");
#else
			NMatrix4d.Multiply (ref matrix, ref other, out var product);
			NMatrix4d.Transpose (ref matrix, out var transposed);
#endif
			Assert.That (product, Is.EqualTo (expectedProduct), "Product");
			Assert.That (transposed, Is.EqualTo (new NMatrix4d (1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16)), "Transpose");
			Assert.That (NMatrix4d.Multiply (matrix, other), Is.EqualTo (expectedProduct), "Value product");
			Assert.That (matrix * other, Is.EqualTo (expectedProduct), "Operator product");
			Assert.That (NMatrix4d.Transpose (matrix), Is.EqualTo (transposed), "Value transpose");
			Assert.That (matrix, Is.EqualTo (expected), "Unchanged matrix");
			Assert.That (other, Is.EqualTo (originalOther), "Unchanged other");
		}

		[Test]
		public void RowMatrix3 ()
		{
			var matrix = new RMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
			var other = new RMatrix3 (9, 8, 7, 6, 5, 4, 3, 2, 1);
			var original = matrix;
			var originalOther = other;
			var transposed = new RMatrix3 (1, 4, 7, 2, 5, 8, 3, 6, 9);
			var product = new RMatrix3 (30, 24, 18, 84, 69, 54, 138, 114, 90);
			var scaled = new RMatrix3 (2, 4, 6, 8, 10, 12, 14, 16, 18);
			var sum = new RMatrix3 (10, 10, 10, 10, 10, 10, 10, 10, 10);
			var difference = new RMatrix3 (-8, -6, -4, -2, 0, 2, 4, 6, 8);
#if XAMCORE_5_0
			Assert.That (RMatrix3.Transpose (in matrix), Is.EqualTo (transposed), "Readonly transpose");
			Assert.That (RMatrix3.Multiply (in matrix, in other), Is.EqualTo (product), "Readonly product");
			Assert.That (RMatrix3.Multiply (in matrix, 2), Is.EqualTo (scaled), "Readonly scalar product");
			Assert.That (RMatrix3.Add (in matrix, in other), Is.EqualTo (sum), "Readonly sum");
			Assert.That (RMatrix3.Subtract (in matrix, in other), Is.EqualTo (difference), "Readonly difference");
#endif
			Assert.That (RMatrix3.Transpose (matrix), Is.EqualTo (transposed), "Value transpose");
			Assert.That (RMatrix3.Multiply (matrix, other), Is.EqualTo (product), "Value product");
			Assert.That (RMatrix3.Multiply (matrix, 2), Is.EqualTo (scaled), "Value scalar product");
			Assert.That (RMatrix3.Add (matrix, other), Is.EqualTo (sum), "Value sum");
			Assert.That (RMatrix3.Subtract (matrix, other), Is.EqualTo (difference), "Value difference");
			Assert.That (matrix * other, Is.EqualTo (product), "Operator product");
			Assert.That (matrix, Is.EqualTo (original), "Unchanged matrix");
			Assert.That (other, Is.EqualTo (originalOther), "Unchanged other");
		}
	}
}
