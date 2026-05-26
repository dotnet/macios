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
			Assert.That (transform.A, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.B, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.C, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.D, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 0));

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			Assert.That (transform.A, Is.EqualTo ((nfloat) 1));
			Assert.That (transform.B, Is.EqualTo ((nfloat) 2));
			Assert.That (transform.C, Is.EqualTo ((nfloat) 3));
			Assert.That (transform.D, Is.EqualTo ((nfloat) 4));
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 5));
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 6));
		}

		[Test]
		public void MakeIdentity ()
		{
			var transform = CGAffineTransform.MakeIdentity ();

			Assert.That (transform.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 0), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 0), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 1), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 0), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 0), "Ty");

			Assert.That (transform.IsIdentity, Is.True, "identity");
		}

		[Test]
		public void MakeRotation ()
		{
			var transform = CGAffineTransform.MakeRotation ((nfloat) Math.PI);

			Assert.That (transform.A, Is.EqualTo ((nfloat) (-1)), "A");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.B).Within (0.0000001), "B");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.C).Within (0.0000001), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) (-1)), "D");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Tx).Within (0.0000001), "Tx");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Ty).Within (0.0000001), "Ty");
		}

		[Test]
		public void MakeScale ()
		{
			var transform = CGAffineTransform.MakeScale (314, 413);
			Assert.That (transform.A, Is.EqualTo ((nfloat) 314));
			Assert.That (transform.B, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.C, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.D, Is.EqualTo ((nfloat) 413));
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 0));
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 0));
		}

		[Test]
		public void MakeTranslation ()
		{
			var transform = CGAffineTransform.MakeTranslation (12, 23);

			Assert.That (transform.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 0), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 0), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 1), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 12), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 23), "Ty");
		}

		[Test]
		public void Multiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var transform = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			transform.Multiply (a);

			Assert.That (transform.A, Is.EqualTo ((nfloat) 33), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 50), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 25), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 38), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 22), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 32), "Ty");
		}

		[Test]
		public void StaticMultiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var b = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			var transform = CGAffineTransform.Multiply (a, b);

			Assert.That (transform.A, Is.EqualTo ((nfloat) 23), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 20), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 55), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 48), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 92), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 80), "Ty");
		}
		[Test]
		public void Scale ()
		{
			var transform1 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = t * [ sx 0 0 sy 0 0 ]
			transform1.Scale (3, 4); // MatrixOrder.Append by default

			Assert.That (transform1.A, Is.EqualTo ((nfloat) 3));
			Assert.That (transform1.B, Is.EqualTo ((nfloat) 0));
			Assert.That (transform1.C, Is.EqualTo ((nfloat) 0));
			Assert.That (transform1.D, Is.EqualTo ((nfloat) 4));
			Assert.That (transform1.Tx, Is.EqualTo ((nfloat) 3));
			Assert.That (transform1.Ty, Is.EqualTo ((nfloat) 8));

			var transform2 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = [ sx 0 0 sy 0 0 ] * t – Swift equivalent
			transform2.Scale (3, 4, MatrixOrder.Prepend);

			Assert.That (transform2.A, Is.EqualTo ((nfloat) 3));
			Assert.That (transform2.B, Is.EqualTo ((nfloat) 0));
			Assert.That (transform2.C, Is.EqualTo ((nfloat) 0));
			Assert.That (transform2.D, Is.EqualTo ((nfloat) 4));
			Assert.That (transform2.Tx, Is.EqualTo ((nfloat) 1));
			Assert.That (transform2.Ty, Is.EqualTo ((nfloat) 2));
		}

		[Test]
		public void StaticScale ()
		{
			var transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);
			var transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);

			Assert.That (transformM == transformN, Is.True, "1");

			transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);
			transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);

			Assert.That (transformM == transformN, Is.True, "2");
		}

		[DllImport (global::ObjCRuntime.Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformScale (CGAffineTransform t, nfloat sx, nfloat sy);

		[Test]
		public void Translate ()
		{
			var transform = CGAffineTransform.MakeIdentity ();
			transform.Translate (1, -1); // MatrixOrder.Append by default

			Assert.That (transform.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 0), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 0), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 1), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 1), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) (-1)), "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3);

			Assert.That (transform.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 2), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 3), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 4), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 7), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) 3), "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3, MatrixOrder.Prepend);

			Assert.That (transform.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 2), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 3), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) 4), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) (-2)), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) (-2)), "Ty");
		}

		[Test]
		public void StaticTranslate ()
		{
			var origin = CGAffineTransform.MakeIdentity ();
			var transformM = CGAffineTransform.Translate (origin, 1, -1);
			var transformN = CGAffineTransformTranslate (origin, 1, -1);

			Assert.That (transformM.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transformM.B, Is.EqualTo ((nfloat) 0), "B");
			Assert.That (transformM.C, Is.EqualTo ((nfloat) 0), "C");
			Assert.That (transformM.D, Is.EqualTo ((nfloat) 1), "D");
			Assert.That (transformM.Tx, Is.EqualTo ((nfloat) 1), "Tx");
			Assert.That (transformM.Ty, Is.EqualTo ((nfloat) (-1)), "Ty");
			Assert.That (transformN == transformM, Is.True);

			origin = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transformM = CGAffineTransform.Translate (origin, 2, -3);
			transformN = CGAffineTransformTranslate (origin, 2, -3);

			Assert.That (transformM.A, Is.EqualTo ((nfloat) 1), "A");
			Assert.That (transformM.B, Is.EqualTo ((nfloat) 2), "B");
			Assert.That (transformM.C, Is.EqualTo ((nfloat) 3), "C");
			Assert.That (transformM.D, Is.EqualTo ((nfloat) 4), "D");
			Assert.That (transformM.Tx, Is.EqualTo ((nfloat) (-2)), "Tx");
			Assert.That (transformM.Ty, Is.EqualTo ((nfloat) (-2)), "Ty");
			Assert.That (transformN == transformM, Is.True);
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
			Assert.That (CGAffineTransform.MakeIdentity ().IsIdentity, Is.True, "MakeIdentity");
			Assert.That (new CGAffineTransform (1, 2, 3, 4, 5, 6).IsIdentity, Is.False, "123456");
		}

		[Test]
		public void TransformPoint ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var point = transform.TransformPoint (new CGPoint (4, 5));

			Assert.That (point.X, Is.EqualTo ((nfloat) 24), "X");
			Assert.That (point.Y, Is.EqualTo ((nfloat) 34), "Y");
		}

		[Test]
		public void TransformRect ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var rect = transform.TransformRect (new CGRect (4, 5, 6, 7));

			Assert.That (rect.X, Is.EqualTo ((nfloat) 24), "X");
			Assert.That (rect.Y, Is.EqualTo ((nfloat) 34), "Y");
			Assert.That (rect.Width, Is.EqualTo ((nfloat) 27), "Width");
			Assert.That (rect.Height, Is.EqualTo ((nfloat) 40), "Height");
		}

		[Test]
		public void Invert ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6).Invert ();

			Assert.That (transform.A, Is.EqualTo ((nfloat) (-2)), "A");
			Assert.That (transform.B, Is.EqualTo ((nfloat) 1), "B");
			Assert.That (transform.C, Is.EqualTo ((nfloat) 1.5), "C");
			Assert.That (transform.D, Is.EqualTo ((nfloat) (-0.5)), "D");
			Assert.That (transform.Tx, Is.EqualTo ((nfloat) 1.0), "Tx");
			Assert.That (transform.Ty, Is.EqualTo ((nfloat) (-2.0)), "Ty");
		}

		[Test]
		public void Decompose ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			var components = new CGAffineTransform (1, 2, 3, 4, 5, 6).Decompose ();
			Assert.That (components.Scale, Is.Not.EqualTo (0.0));
			Assert.That (components.HorizontalShear, Is.Not.EqualTo (0.0));
			Assert.That (components.Rotation, Is.Not.EqualTo (0.0));
			Assert.That (components.Translation, Is.Not.EqualTo (new CGVector ((nfloat) 0, (nfloat) 0)));
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
			Assert.That (transform.A, Is.Not.EqualTo (0.0));
			Assert.That (transform.B, Is.Not.EqualTo (0.0));
			Assert.That (transform.C, Is.Not.EqualTo (0.0));
			Assert.That (transform.D, Is.Not.EqualTo (0.0));
			Assert.That (transform.Tx, Is.Not.EqualTo (0.0));
			Assert.That (transform.Ty, Is.Not.EqualTo (0.0));
		}

		[Test]
		public void NSValueRoundtrip ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			// looks simplistic but that NSValue logic is implemented by "us" on macOS
			using (var nsv = NSValue.FromCGAffineTransform (transform)) {
				var tback = nsv.CGAffineTransformValue;
				Assert.That (tback.A, Is.EqualTo ((nfloat) 1), "A");
				Assert.That (tback.B, Is.EqualTo ((nfloat) 2), "B");
				Assert.That (tback.C, Is.EqualTo ((nfloat) 3), "C");
				Assert.That (tback.D, Is.EqualTo ((nfloat) 4), "D");
				Assert.That (tback.Tx, Is.EqualTo ((nfloat) 5), "Tx");
				Assert.That (tback.Ty, Is.EqualTo ((nfloat) 6), "Ty");
			}
		}

		[Test]
		public unsafe void SizeOfTest ()
		{
			Assert.That (Marshal.SizeOf<CGAffineTransform> (), Is.EqualTo (sizeof (CGAffineTransform)));
		}

		[Test]
		public void ToStringTest ()
		{
			var transform = new CGAffineTransform ((nfloat) 1, (nfloat) 2, (nfloat) 3, (nfloat) 4, (nfloat) 5, (nfloat) 6);
			Assert.That (transform.ToString (), Is.EqualTo ("[1, 2, 3, 4, 5, 6]"), "ToString");
		}
	}


}
