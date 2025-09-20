using System;
using System.Diagnostics;

using Foundation;
using ObjCRuntime;

using VectorByte16 = global::CoreGraphics.NVector16b;

using NUnit.Framework;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VectorByte16Test {
		[Test]
		public void DefaultConstructor ()
		{
			var vector = new VectorByte16 ();
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (0, vector [i], $"default constructor element {i}");
			}
		}

		[Test]
		public void ArrayConstructor ()
		{
			var expected = GetTestByteArray ();
			var actual = new VectorByte16 (expected);
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (expected [i], actual [i], $"array ctor element {i}");
			}
		}

		[Test]
		public void ArrayConstructor_Null ()
		{
			var actual = new VectorByte16 (null);
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (0, actual [i], $"null array ctor element {i}");
			}
		}

		[Test]
		public void ArrayConstructor_PartialArray ()
		{
			var partialArray = new byte [] { 1, 2, 3, 4, 5 };
			var actual = new VectorByte16 (partialArray);
			for (int i = 0; i < 5; i++) {
				Assert.AreEqual (partialArray [i], actual [i], $"partial array ctor element {i}");
			}
			for (int i = 5; i < 16; i++) {
				Assert.AreEqual (0, actual [i], $"partial array ctor uninitialized element {i}");
			}
		}

		[Test]
		public void ArrayConstructor_LargeArray ()
		{
			var largeArray = new byte [20];
			for (int i = 0; i < 20; i++) {
				largeArray [i] = (byte) (i + 10);
			}
			var actual = new VectorByte16 (largeArray);
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (largeArray [i], actual [i], $"large array ctor element {i}");
			}
		}

		[Test]
		public void Indexer_Get ()
		{
			var expected = GetTestByteArray ();
			var vector = new VectorByte16 (expected);
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (expected [i], vector [i], $"indexer get element {i}");
			}
		}

		[Test]
		public void Indexer_Set ()
		{
			var vector = new VectorByte16 ();
			var expected = GetTestByteArray ();
			for (int i = 0; i < 16; i++) {
				vector [i] = expected [i];
			}
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (expected [i], vector [i], $"indexer set element {i}");
			}
		}

		[Test]
		public void Indexer_OutOfRange_Negative ()
		{
			var vector = new VectorByte16 ();
			Assert.Throws<ArgumentOutOfRangeException> (() => { var x = vector [-1]; }, "negative index get");
			Assert.Throws<ArgumentOutOfRangeException> (() => { vector [-1] = 1; }, "negative index set");
		}

		[Test]
		public void Indexer_OutOfRange_TooLarge ()
		{
			var vector = new VectorByte16 ();
			Assert.Throws<ArgumentOutOfRangeException> (() => { var x = vector [16]; }, "too large index get");
			Assert.Throws<ArgumentOutOfRangeException> (() => { vector [16] = 1; }, "too large index set");
		}

		[Test]
		public void Equality_Operator ()
		{
			var inputL = GetTestVector ();
			var inputR = GetTestVector ();

			// vectors are different
			Assert.IsFalse (inputL == inputR, "inequality");

			inputL = inputR;
			// vectors are identical
			Assert.IsTrue (inputL == inputR, "equality");
		}

		[Test]
		public void Inequality_Operator ()
		{
			var inputL = GetTestVector ();
			var inputR = GetTestVector ();

			// vectors are different
			Assert.IsTrue (inputL != inputR, "inequality");

			inputL = inputR;
			// vectors are identical
			Assert.IsFalse (inputL != inputR, "equality");
		}

		[Test]
		public void ToStringTest ()
		{
			var vector = new VectorByte16 (new byte [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
			Assert.AreEqual ("(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)", vector.ToString (), "toString");
		}

		[Test]
		public void Equals_Object ()
		{
			var expectedA = GetTestVector ();
			var expectedB = GetTestVector ();

			Assert.IsTrue (expectedA.Equals ((object) expectedA), "self");
			Assert.IsFalse (expectedA.Equals ((object) expectedB), "other");
			Assert.IsFalse (expectedA.Equals (null), "null");
			Assert.IsFalse (expectedA.Equals ("string"), "wrong type");
		}

		[Test]
		public void Equals_Vector ()
		{
			var expectedA = GetTestVector ();
			var expectedB = GetTestVector ();

			Assert.IsTrue (expectedA.Equals (expectedA), "self");
			Assert.IsFalse (expectedA.Equals (expectedB), "other");
		}

		[Test]
		public void AsSpan ()
		{
			var expected = GetTestByteArray ();
			var vector = new VectorByte16 (expected);
			var span = vector.AsSpan ();

			Assert.AreEqual (16, span.Length, "span length");
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (expected [i], span [i], $"span element {i}");
			}
		}

		[Test]
		public void GetHashCode_SameVectors ()
		{
			var byteArray = GetTestByteArray ();
			var vector1 = new VectorByte16 (byteArray);
			var vector2 = new VectorByte16 (byteArray);

			Assert.AreEqual (vector1.GetHashCode (), vector2.GetHashCode (), "same vectors should have same hash code");
		}

		[Test]
		public void GetHashCode_DifferentVectors ()
		{
			var vector1 = GetTestVector ();
			var vector2 = GetTestVector ();

			Assert.AreNotEqual (vector1.GetHashCode (), vector2.GetHashCode (), "different vectors should have different hash codes");
		}

		[Test]
		public void Zero_Property ()
		{
			var zero = VectorByte16.Zero;
			for (int i = 0; i < 16; i++) {
				Assert.AreEqual (0, zero [i], $"Zero property element {i}");
			}
		}

		static byte [] [] test_byte_arrays = new [] {
			new byte [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
			new byte [] { 255, 254, 253, 252, 251, 250, 249, 248, 247, 246, 245, 244, 243, 242, 241, 240 },
			new byte [] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
			new byte [] { 128, 127, 126, 125, 124, 123, 122, 121, 120, 119, 118, 117, 116, 115, 114, 113 },
			new byte [] { 17, 34, 51, 68, 85, 102, 119, 136, 153, 170, 187, 204, 221, 238, 255, 0 },
			new byte [] { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 },
			new byte [] { 100, 200, 50, 150, 75, 175, 25, 125, 225, 15, 115, 215, 65, 165, 35, 135 },
			new byte [] { 0, 0, 0, 0, 255, 255, 255, 255, 128, 128, 128, 128, 64, 64, 64, 64 },
			new byte [] { 7, 14, 21, 28, 35, 42, 49, 56, 63, 70, 77, 84, 91, 98, 105, 112 },
			new byte [] { 199, 198, 197, 196, 195, 194, 193, 192, 191, 190, 189, 188, 187, 186, 185, 184 }
		};

		static int counter;
		internal static VectorByte16 GetTestVector ()
		{
			counter++;
			if (counter >= test_byte_arrays.Length)
				counter = 0;
			return new VectorByte16 (test_byte_arrays [counter]);
		}

		internal static byte [] GetTestByteArray ()
		{
			counter++;
			if (counter >= test_byte_arrays.Length)
				counter = 0;
			return test_byte_arrays [counter];
		}
	}
}
