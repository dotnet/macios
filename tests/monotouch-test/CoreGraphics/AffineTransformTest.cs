//
// Unit tests for CGAffineTransform
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AffineTransformTest {
		[Test]
		public void Ctor ()
		{
			var transform = new CGAffineTransform ();
			ClassicAssert.AreEqual ((nfloat) 0, transform.A);
			ClassicAssert.AreEqual ((nfloat) 0, transform.B);
			ClassicAssert.AreEqual ((nfloat) 0, transform.C);
			ClassicAssert.AreEqual ((nfloat) 0, transform.D);
			ClassicAssert.AreEqual ((nfloat) 0, transform.Tx);
			ClassicAssert.AreEqual ((nfloat) 0, transform.Ty);

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			ClassicAssert.AreEqual ((nfloat) 1, transform.A);
			ClassicAssert.AreEqual ((nfloat) 2, transform.B);
			ClassicAssert.AreEqual ((nfloat) 3, transform.C);
			ClassicAssert.AreEqual ((nfloat) 4, transform.D);
			ClassicAssert.AreEqual ((nfloat) 5, transform.Tx);
			ClassicAssert.AreEqual ((nfloat) 6, transform.Ty);
		}

		[Test]
		public void MakeIdentity ()
		{
			var transform = CGAffineTransform.MakeIdentity ();

			ClassicAssert.AreEqual ((nfloat) 1, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 0, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 0, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 1, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 0, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) 0, transform.Ty, "Ty");

			ClassicAssert.IsTrue (transform.IsIdentity, "identity");
		}

		[Test]
		public void MakeRotation ()
		{
			var transform = CGAffineTransform.MakeRotation ((nfloat) Math.PI);

			ClassicAssert.AreEqual ((nfloat) (-1), transform.A, "A");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.B).Within (0.0000001), "B");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.C).Within (0.0000001), "C");
			ClassicAssert.AreEqual ((nfloat) (-1), transform.D, "D");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Tx).Within (0.0000001), "Tx");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Ty).Within (0.0000001), "Ty");
		}

		[Test]
		public void MakeScale ()
		{
			var transform = CGAffineTransform.MakeScale (314, 413);
			ClassicAssert.AreEqual ((nfloat) 314, transform.A);
			ClassicAssert.AreEqual ((nfloat) 0, transform.B);
			ClassicAssert.AreEqual ((nfloat) 0, transform.C);
			ClassicAssert.AreEqual ((nfloat) 413, transform.D);
			ClassicAssert.AreEqual ((nfloat) 0, transform.Tx);
			ClassicAssert.AreEqual ((nfloat) 0, transform.Ty);
		}

		[Test]
		public void MakeTranslation ()
		{
			var transform = CGAffineTransform.MakeTranslation (12, 23);

			ClassicAssert.AreEqual ((nfloat) 1, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 0, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 0, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 1, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 12, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) 23, transform.Ty, "Ty");
		}

		[Test]
		public void Multiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var transform = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			transform.Multiply (a);

			ClassicAssert.AreEqual ((nfloat) 33, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 50, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 25, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 38, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 22, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) 32, transform.Ty, "Ty");
		}

		[Test]
		public void StaticMultiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var b = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			var transform = CGAffineTransform.Multiply (a, b);

			ClassicAssert.AreEqual ((nfloat) 23, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 20, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 55, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 48, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 92, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) 80, transform.Ty, "Ty");
		}
		[Test]
		public void Scale ()
		{
			var transform1 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = t * [ sx 0 0 sy 0 0 ]
			transform1.Scale (3, 4); // MatrixOrder.Append by default

			ClassicAssert.AreEqual ((nfloat) 3, transform1.A);
			ClassicAssert.AreEqual ((nfloat) 0, transform1.B);
			ClassicAssert.AreEqual ((nfloat) 0, transform1.C);
			ClassicAssert.AreEqual ((nfloat) 4, transform1.D);
			ClassicAssert.AreEqual ((nfloat) 3, transform1.Tx);
			ClassicAssert.AreEqual ((nfloat) 8, transform1.Ty);

			var transform2 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = [ sx 0 0 sy 0 0 ] * t – Swift equivalent
			transform2.Scale (3, 4, MatrixOrder.Prepend);

			ClassicAssert.AreEqual ((nfloat) 3, transform2.A);
			ClassicAssert.AreEqual ((nfloat) 0, transform2.B);
			ClassicAssert.AreEqual ((nfloat) 0, transform2.C);
			ClassicAssert.AreEqual ((nfloat) 4, transform2.D);
			ClassicAssert.AreEqual ((nfloat) 1, transform2.Tx);
			ClassicAssert.AreEqual ((nfloat) 2, transform2.Ty);
		}

		[Test]
		public void StaticScale ()
		{
			var transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);
			var transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);

			ClassicAssert.IsTrue (transformM == transformN, "1");

			transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);
			transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);

			ClassicAssert.IsTrue (transformM == transformN, "2");
		}

		[DllImport (global::ObjCRuntime.Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformScale (CGAffineTransform t, nfloat sx, nfloat sy);

		[Test]
		public void Translate ()
		{
			var transform = CGAffineTransform.MakeIdentity ();
			transform.Translate (1, -1); // MatrixOrder.Append by default

			ClassicAssert.AreEqual ((nfloat) 1, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 0, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 0, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 1, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 1, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) (-1), transform.Ty, "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3);

			ClassicAssert.AreEqual ((nfloat) 1, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 2, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 3, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 4, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 7, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) 3, transform.Ty, "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3, MatrixOrder.Prepend);

			ClassicAssert.AreEqual ((nfloat) 1, transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 2, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 3, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) 4, transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) (-2), transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) (-2), transform.Ty, "Ty");
		}

		[Test]
		public void StaticTranslate ()
		{
			var origin = CGAffineTransform.MakeIdentity ();
			var transformM = CGAffineTransform.Translate (origin, 1, -1);
			var transformN = CGAffineTransformTranslate (origin, 1, -1);

			ClassicAssert.AreEqual ((nfloat) 1, transformM.A, "A");
			ClassicAssert.AreEqual ((nfloat) 0, transformM.B, "B");
			ClassicAssert.AreEqual ((nfloat) 0, transformM.C, "C");
			ClassicAssert.AreEqual ((nfloat) 1, transformM.D, "D");
			ClassicAssert.AreEqual ((nfloat) 1, transformM.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) (-1), transformM.Ty, "Ty");
			ClassicAssert.IsTrue (transformN == transformM);

			origin = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transformM = CGAffineTransform.Translate (origin, 2, -3);
			transformN = CGAffineTransformTranslate (origin, 2, -3);

			ClassicAssert.AreEqual ((nfloat) 1, transformM.A, "A");
			ClassicAssert.AreEqual ((nfloat) 2, transformM.B, "B");
			ClassicAssert.AreEqual ((nfloat) 3, transformM.C, "C");
			ClassicAssert.AreEqual ((nfloat) 4, transformM.D, "D");
			ClassicAssert.AreEqual ((nfloat) (-2), transformM.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) (-2), transformM.Ty, "Ty");
			ClassicAssert.IsTrue (transformN == transformM);
		}

		[DllImport (global::ObjCRuntime.Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformTranslate (CGAffineTransform t, nfloat sx, nfloat sy);

		[Test]
		public void Rotate ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Rotate ((nfloat) Math.PI); // MatrixOrder.Append by default

			Assert.That ((double) (-1), Is.EqualTo ((double) transform.A).Within (0.000001), "A");
			Assert.That ((double) (-2), Is.EqualTo ((double) transform.B).Within (0.000001), "B");
			Assert.That ((double) (-3), Is.EqualTo ((double) transform.C).Within (0.000001), "C");
			Assert.That ((double) (-4), Is.EqualTo ((double) transform.D).Within (0.000001), "D");
			Assert.That ((double) (-5), Is.EqualTo ((double) transform.Tx).Within (0.000001), "Tx");
			Assert.That ((double) (-6), Is.EqualTo ((double) transform.Ty).Within (0.000001), "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Rotate ((nfloat) Math.PI, MatrixOrder.Prepend);

			Assert.That ((double) (-1), Is.EqualTo ((double) transform.A).Within (0.000001), "A");
			Assert.That ((double) (-2), Is.EqualTo ((double) transform.B).Within (0.000001), "B");
			Assert.That ((double) (-3), Is.EqualTo ((double) transform.C).Within (0.000001), "C");
			Assert.That ((double) (-4), Is.EqualTo ((double) transform.D).Within (0.000001), "D");
			Assert.That ((double) 5, Is.EqualTo ((double) transform.Tx).Within (0.000001), "Tx");
			Assert.That ((double) 6, Is.EqualTo ((double) transform.Ty).Within (0.000001), "Ty");
		}

		[Test]
		public void StaticRotate ()
		{
			var transformM = CGAffineTransform.Rotate (new CGAffineTransform (1, 2, 3, 4, 5, 6), (nfloat) Math.PI);
			var transformN = CGAffineTransformRotate (new CGAffineTransform (1, 2, 3, 4, 5, 6), (nfloat) Math.PI);

			Assert.That ((double) (-1), Is.EqualTo ((double) transformM.A).Within (0.000001), "A");
			Assert.That ((double) (-2), Is.EqualTo ((double) transformM.B).Within (0.000001), "B");
			Assert.That ((double) (-3), Is.EqualTo ((double) transformM.C).Within (0.000001), "C");
			Assert.That ((double) (-4), Is.EqualTo ((double) transformM.D).Within (0.000001), "D");
			Assert.That ((double) 5, Is.EqualTo ((double) transformM.Tx).Within (0.000001), "Tx");
			Assert.That ((double) 6, Is.EqualTo ((double) transformM.Ty).Within (0.000001), "Ty");

			Assert.That ((double) transformN.A, Is.EqualTo ((double) transformM.A).Within (0.000001), "A");
			Assert.That ((double) transformN.B, Is.EqualTo ((double) transformM.B).Within (0.000001), "B");
			Assert.That ((double) transformN.C, Is.EqualTo ((double) transformM.C).Within (0.000001), "C");
			Assert.That ((double) transformN.D, Is.EqualTo ((double) transformM.D).Within (0.000001), "D");
			Assert.That ((double) 5, Is.EqualTo ((double) transformM.Tx).Within (0.000001), "Tx");
			Assert.That ((double) 6, Is.EqualTo ((double) transformM.Ty).Within (0.000001), "Ty");
		}

		[DllImport (global::ObjCRuntime.Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformRotate (CGAffineTransform t, nfloat angle);

		[Test]
		public void IsIdentity ()
		{
			ClassicAssert.IsTrue (CGAffineTransform.MakeIdentity ().IsIdentity, "MakeIdentity");
			ClassicAssert.IsFalse (new CGAffineTransform (1, 2, 3, 4, 5, 6).IsIdentity, "123456");
		}

		[Test]
		public void TransformPoint ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var point = transform.TransformPoint (new CGPoint (4, 5));

			ClassicAssert.AreEqual ((nfloat) 24, point.X, "X");
			ClassicAssert.AreEqual ((nfloat) 34, point.Y, "Y");
		}

		[Test]
		public void TransformRect ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var rect = transform.TransformRect (new CGRect (4, 5, 6, 7));

			ClassicAssert.AreEqual ((nfloat) 24, rect.X, "X");
			ClassicAssert.AreEqual ((nfloat) 34, rect.Y, "Y");
			ClassicAssert.AreEqual ((nfloat) 27, rect.Width, "Width");
			ClassicAssert.AreEqual ((nfloat) 40, rect.Height, "Height");
		}

		[Test]
		public void Invert ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6).Invert ();

			ClassicAssert.AreEqual ((nfloat) (-2), transform.A, "A");
			ClassicAssert.AreEqual ((nfloat) 1, transform.B, "B");
			ClassicAssert.AreEqual ((nfloat) 1.5, transform.C, "C");
			ClassicAssert.AreEqual ((nfloat) (-0.5), transform.D, "D");
			ClassicAssert.AreEqual ((nfloat) 1.0, transform.Tx, "Tx");
			ClassicAssert.AreEqual ((nfloat) (-2.0), transform.Ty, "Ty");
		}

		[Test]
		public void Decompose ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			var components = new CGAffineTransform (1, 2, 3, 4, 5, 6).Decompose ();
			ClassicAssert.AreNotEqual (0.0, components.Scale);
			ClassicAssert.AreNotEqual (0.0, components.HorizontalShear);
			ClassicAssert.AreNotEqual (0.0, components.Rotation);
			ClassicAssert.AreNotEqual (new CGVector ((nfloat) 0, (nfloat) 0), components.Translation);
		}

		[Test]
		public void MakeWithComponents ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			var components = new CGAffineTransformComponents () {
				Scale = new CGSize (1.0, 2.0),
				HorizontalShear = (nfloat) 3.0,
				Rotation = (nfloat) 4.0,
				Translation = new CGVector ((nfloat) 5.0, (nfloat) 6.0),
			};
			var transform = CGAffineTransform.MakeWithComponents (components);
			ClassicAssert.AreNotEqual (0.0, transform.A);
			ClassicAssert.AreNotEqual (0.0, transform.B);
			ClassicAssert.AreNotEqual (0.0, transform.C);
			ClassicAssert.AreNotEqual (0.0, transform.D);
			ClassicAssert.AreNotEqual (0.0, transform.Tx);
			ClassicAssert.AreNotEqual (0.0, transform.Ty);
		}

		[Test]
		public void NSValueRoundtrip ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			// looks simplistic but that NSValue logic is implemented by "us" on macOS
			using (var nsv = NSValue.FromCGAffineTransform (transform)) {
				var tback = nsv.CGAffineTransformValue;
				ClassicAssert.AreEqual ((nfloat) 1, tback.A, "A");
				ClassicAssert.AreEqual ((nfloat) 2, tback.B, "B");
				ClassicAssert.AreEqual ((nfloat) 3, tback.C, "C");
				ClassicAssert.AreEqual ((nfloat) 4, tback.D, "D");
				ClassicAssert.AreEqual ((nfloat) 5, tback.Tx, "Tx");
				ClassicAssert.AreEqual ((nfloat) 6, tback.Ty, "Ty");
			}
		}

		[Test]
		public unsafe void SizeOfTest ()
		{
			ClassicAssert.AreEqual (sizeof (CGAffineTransform), Marshal.SizeOf<CGAffineTransform> ());
		}

		[Test]
		public void ToStringTest ()
		{
			var transform = new CGAffineTransform ((nfloat) 1, (nfloat) 2, (nfloat) 3, (nfloat) 4, (nfloat) 5, (nfloat) 6);
			ClassicAssert.AreEqual ("[1, 2, 3, 4, 5, 6]", transform.ToString (), "ToString");
		}
	}


}
