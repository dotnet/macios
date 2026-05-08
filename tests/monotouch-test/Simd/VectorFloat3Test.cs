
using System.Diagnostics;

using System.Numerics;

using VectorFloat3 = global::CoreGraphics.NVector3;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VectorFloat3Test {
		[Test]
		public void ElementConstructor ()
		{
			var expected = GetTestVector ();
			var actual = new VectorFloat3 (expected.X, expected.Y, expected.Z);
			Asserts.AreEqual (expected, actual, "ctor 1");
		}

		[Test]
		public void Equality_Operator ()
		{
			var inputL = GetTestVector ();
			var inputR = GetTestVector ();
			var inputSimdL = (VectorFloat3) inputL;
			var inputSimdR = (VectorFloat3) inputR;

			// matrices are different
			ClassicAssert.AreEqual (inputL == inputR, inputSimdL == inputSimdR, "inequality");
			ClassicAssert.IsFalse (inputL == inputR, "inequality 2 expected");
			ClassicAssert.IsFalse (inputSimdL == inputSimdR, "inequality 2 actual");

			inputL = inputR;
			inputSimdL = inputSimdR;
			// matrices are identical
			ClassicAssert.AreEqual (inputL == inputR, inputSimdL == inputSimdR, "equality");
			ClassicAssert.IsTrue (inputL == inputR, "equality 2 expected");
			ClassicAssert.IsTrue (inputSimdL == inputSimdR, "equality 2 actual");
		}

		[Test]
		public void Inequality_Operator ()
		{
			var inputL = GetTestVector ();
			var inputR = GetTestVector ();
			var inputSimdL = (VectorFloat3) inputL;
			var inputSimdR = (VectorFloat3) inputR;

			// matrices are different
			ClassicAssert.AreEqual (inputL != inputR, inputSimdL != inputSimdR, "inequality");
			ClassicAssert.IsTrue (inputL != inputR, "inequality 2 expected");
			ClassicAssert.IsTrue (inputSimdL != inputSimdR, "inequality 2 actual");

			inputL = inputR;
			inputSimdL = inputSimdR;
			// matrices are identical
			ClassicAssert.AreEqual (inputL != inputR, inputSimdL != inputSimdR, "equality");
			ClassicAssert.IsFalse (inputL != inputR, "equality 2 expected");
			ClassicAssert.IsFalse (inputSimdL != inputSimdR, "equality 2 actual");
		}

		[Test]
		public void Explicit_Operator_ToVector3 ()
		{
			var expected = GetTestVector ();
			var actual = (Vector3) expected;

			Asserts.AreEqual (expected, actual, "ToVector3");
		}

		[Test]
		public void Explicit_Operator_FromVector3 ()
		{
			var expected = new Vector3 (2, 3, 7.7f);
			var actual = (VectorFloat3) expected;

			Asserts.AreEqual (expected, actual, "FromVector3");
		}

		[Test]
		public void ToStringTest ()
		{
			var vector = new VectorFloat3 (1, 2, 3);

			ClassicAssert.AreEqual ("(1, 2, 3)", vector.ToString (), "tostring");
		}

		// GetHashCode doesn't have to be identical, so no need to test

		[Test]
		public void Equals_Object ()
		{
			var expectedA = GetTestVector ();
			var expectedB = GetTestVector ();
			var actualA = (VectorFloat3) expectedA;
			var actualB = (VectorFloat3) expectedB;

			ClassicAssert.IsTrue (actualA.Equals ((object) actualA), "self");
			ClassicAssert.IsFalse (actualA.Equals ((object) actualB), "other");
			ClassicAssert.IsFalse (actualA.Equals (null), "null");
			ClassicAssert.IsTrue (actualA.Equals (expectedA), "same type");
		}

		[Test]
		public void Equals_Vector ()
		{
			var expectedA = GetTestVector ();
			var expectedB = GetTestVector ();
			var actualA = (VectorFloat3) expectedA;
			var actualB = (VectorFloat3) expectedB;

			ClassicAssert.IsTrue (actualA.Equals (actualA), "self");
			ClassicAssert.IsFalse (actualA.Equals (actualB), "other");
		}

		static VectorFloat3 [] test_vectors = new [] {
			new VectorFloat3 (0.1532144f, 0.5451511f, 0.2004739f),
			new VectorFloat3 (0.7717745f, 0.559364f, 0.00918373f),
			new VectorFloat3 (0.2023053f, 0.4701468f, 0.6618567f),
			new VectorFloat3 (0.4904693f, 0.841727f, 0.2294401f),
			new VectorFloat3 (0.1252193f, 0.08986127f, 0.3407605f),
			new VectorFloat3 (0.006755914f, 0.07464754f, 0.287938f),
			new VectorFloat3 (9.799572E+08f, 1.64794E+09f, 1.117296E+09f),
			new VectorFloat3 (1.102396E+09f, 3.082477E+08f, 1.126484E+09f),
			new VectorFloat3 (2.263112E+08f, 8.79644E+08f, 1.303282E+09f),
			new VectorFloat3 (8.176959E+08f, 1.386156E+09f, 5.956444E+08f),
		};

		static int counter;
		internal static VectorFloat3 GetTestVector ()
		{
			counter++;
			if (counter == test_vectors.Length)
				counter = 0;
			return test_vectors [counter];
		}
	}
}
