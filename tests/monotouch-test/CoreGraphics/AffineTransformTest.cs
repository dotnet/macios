//
// Unit tests for CGAffineTransform
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation
//

using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreGraphics;
using CoreFoundation;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AffineTransformTest {
		[Test]
		public void Ctor ()
		{
			var transform = new CGAffineTransform ();
			Assert.AreEqual ((nfloat) 0, transform.A);
			Assert.AreEqual ((nfloat) 0, transform.B);
			Assert.AreEqual ((nfloat) 0, transform.C);
			Assert.AreEqual ((nfloat) 0, transform.D);
			Assert.AreEqual ((nfloat) 0, transform.Tx);
			Assert.AreEqual ((nfloat) 0, transform.Ty);

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			Assert.AreEqual ((nfloat) 1, transform.A);
			Assert.AreEqual ((nfloat) 2, transform.B);
			Assert.AreEqual ((nfloat) 3, transform.C);
			Assert.AreEqual ((nfloat) 4, transform.D);
			Assert.AreEqual ((nfloat) 5, transform.Tx);
			Assert.AreEqual ((nfloat) 6, transform.Ty);
		}

		[Test]
		public void MakeIdentity ()
		{
			var transform = CGAffineTransform.MakeIdentity ();

			Assert.AreEqual ((nfloat) 1, transform.A, "A");
			Assert.AreEqual ((nfloat) 0, transform.B, "B");
			Assert.AreEqual ((nfloat) 0, transform.C, "C");
			Assert.AreEqual ((nfloat) 1, transform.D, "D");
			Assert.AreEqual ((nfloat) 0, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) 0, transform.Ty, "Ty");

			Assert.IsTrue (transform.IsIdentity, "identity");
		}

		[Test]
		public void MakeRotation ()
		{
			var transform = CGAffineTransform.MakeRotation ((nfloat) Math.PI);

			Assert.AreEqual ((nfloat) (-1), transform.A, "A");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.B).Within (0.0000001), "B");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.C).Within (0.0000001), "C");
			Assert.AreEqual ((nfloat) (-1), transform.D, "D");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Tx).Within (0.0000001), "Tx");
			Assert.That ((double) 0, Is.EqualTo ((double) transform.Ty).Within (0.0000001), "Ty");
		}

		[Test]
		public void MakeScale ()
		{
			var transform = CGAffineTransform.MakeScale (314, 413);
			Assert.AreEqual ((nfloat) 314, transform.A);
			Assert.AreEqual ((nfloat) 0, transform.B);
			Assert.AreEqual ((nfloat) 0, transform.C);
			Assert.AreEqual ((nfloat) 413, transform.D);
			Assert.AreEqual ((nfloat) 0, transform.Tx);
			Assert.AreEqual ((nfloat) 0, transform.Ty);
		}

		[Test]
		public void MakeTranslation ()
		{
			var transform = CGAffineTransform.MakeTranslation (12, 23);

			Assert.AreEqual ((nfloat) 1, transform.A, "A");
			Assert.AreEqual ((nfloat) 0, transform.B, "B");
			Assert.AreEqual ((nfloat) 0, transform.C, "C");
			Assert.AreEqual ((nfloat) 1, transform.D, "D");
			Assert.AreEqual ((nfloat) 12, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) 23, transform.Ty, "Ty");
		}

		[Test]
		public void Multiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var transform = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			transform.Multiply (a);

			Assert.AreEqual ((nfloat) 33, transform.A, "A");
			Assert.AreEqual ((nfloat) 50, transform.B, "B");
			Assert.AreEqual ((nfloat) 25, transform.C, "C");
			Assert.AreEqual ((nfloat) 38, transform.D, "D");
			Assert.AreEqual ((nfloat) 22, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) 32, transform.Ty, "Ty");
		}

		[Test]
		public void StaticMultiply ()
		{
			var a = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var b = new CGAffineTransform (9, 8, 7, 6, 5, 4);
			var transform = CGAffineTransform.Multiply (a, b);

			Assert.AreEqual ((nfloat) 23, transform.A, "A");
			Assert.AreEqual ((nfloat) 20, transform.B, "B");
			Assert.AreEqual ((nfloat) 55, transform.C, "C");
			Assert.AreEqual ((nfloat) 48, transform.D, "D");
			Assert.AreEqual ((nfloat) 92, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) 80, transform.Ty, "Ty");
		}
		[Test]
		public void Scale ()
		{
			var transform1 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = t * [ sx 0 0 sy 0 0 ]
			transform1.Scale (3, 4); // MatrixOrder.Append by default

			Assert.AreEqual ((nfloat) 3, transform1.A);
			Assert.AreEqual ((nfloat) 0, transform1.B);
			Assert.AreEqual ((nfloat) 0, transform1.C);
			Assert.AreEqual ((nfloat) 4, transform1.D);
			Assert.AreEqual ((nfloat) 3, transform1.Tx);
			Assert.AreEqual ((nfloat) 8, transform1.Ty);

			var transform2 = CGAffineTransform.MakeTranslation (1, 2);
			// t' = [ sx 0 0 sy 0 0 ] * t – Swift equivalent
			transform2.Scale (3, 4, MatrixOrder.Prepend);

			Assert.AreEqual ((nfloat) 3, transform2.A);
			Assert.AreEqual ((nfloat) 0, transform2.B);
			Assert.AreEqual ((nfloat) 0, transform2.C);
			Assert.AreEqual ((nfloat) 4, transform2.D);
			Assert.AreEqual ((nfloat) 1, transform2.Tx);
			Assert.AreEqual ((nfloat) 2, transform2.Ty);
		}

		[Test]
		public void StaticScale ()
		{
			var transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);
			var transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (0, 200), 1, -1);

			Assert.IsTrue (transformM == transformN, "1");

			transformM = CGAffineTransform.Scale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);
			transformN = CGAffineTransformScale (CGAffineTransform.MakeTranslation (1, 2), -3, -4);

			Assert.IsTrue (transformM == transformN, "2");
		}

		[DllImport (global::ObjCRuntime.Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformScale (CGAffineTransform t, nfloat sx, nfloat sy);

		[Test]
		public void Translate ()
		{
			var transform = CGAffineTransform.MakeIdentity ();
			transform.Translate (1, -1); // MatrixOrder.Append by default

			Assert.AreEqual ((nfloat) 1, transform.A, "A");
			Assert.AreEqual ((nfloat) 0, transform.B, "B");
			Assert.AreEqual ((nfloat) 0, transform.C, "C");
			Assert.AreEqual ((nfloat) 1, transform.D, "D");
			Assert.AreEqual ((nfloat) 1, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) (-1), transform.Ty, "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3);

			Assert.AreEqual ((nfloat) 1, transform.A, "A");
			Assert.AreEqual ((nfloat) 2, transform.B, "B");
			Assert.AreEqual ((nfloat) 3, transform.C, "C");
			Assert.AreEqual ((nfloat) 4, transform.D, "D");
			Assert.AreEqual ((nfloat) 7, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) 3, transform.Ty, "Ty");

			transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transform.Translate (2, -3, MatrixOrder.Prepend);

			Assert.AreEqual ((nfloat) 1, transform.A, "A");
			Assert.AreEqual ((nfloat) 2, transform.B, "B");
			Assert.AreEqual ((nfloat) 3, transform.C, "C");
			Assert.AreEqual ((nfloat) 4, transform.D, "D");
			Assert.AreEqual ((nfloat) (-2), transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) (-2), transform.Ty, "Ty");
		}

		[Test]
		public void StaticTranslate ()
		{
			var origin = CGAffineTransform.MakeIdentity ();
			var transformM = CGAffineTransform.Translate (origin, 1, -1);
			var transformN = CGAffineTransformTranslate (origin, 1, -1);

			Assert.AreEqual ((nfloat) 1, transformM.A, "A");
			Assert.AreEqual ((nfloat) 0, transformM.B, "B");
			Assert.AreEqual ((nfloat) 0, transformM.C, "C");
			Assert.AreEqual ((nfloat) 1, transformM.D, "D");
			Assert.AreEqual ((nfloat) 1, transformM.Tx, "Tx");
			Assert.AreEqual ((nfloat) (-1), transformM.Ty, "Ty");
			Assert.IsTrue (transformN == transformM);

			origin = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			transformM = CGAffineTransform.Translate (origin, 2, -3);
			transformN = CGAffineTransformTranslate (origin, 2, -3);

			Assert.AreEqual ((nfloat) 1, transformM.A, "A");
			Assert.AreEqual ((nfloat) 2, transformM.B, "B");
			Assert.AreEqual ((nfloat) 3, transformM.C, "C");
			Assert.AreEqual ((nfloat) 4, transformM.D, "D");
			Assert.AreEqual ((nfloat) (-2), transformM.Tx, "Tx");
			Assert.AreEqual ((nfloat) (-2), transformM.Ty, "Ty");
			Assert.IsTrue (transformN == transformM);
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
			Assert.IsTrue (CGAffineTransform.MakeIdentity ().IsIdentity, "MakeIdentity");
			Assert.IsFalse (new CGAffineTransform (1, 2, 3, 4, 5, 6).IsIdentity, "123456");
		}

		[Test]
		public void TransformPoint ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var point = transform.TransformPoint (new CGPoint (4, 5));

			Assert.AreEqual ((nfloat) 24, point.X, "X");
			Assert.AreEqual ((nfloat) 34, point.Y, "Y");
		}

		[Test]
		public void TransformRect ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			var rect = transform.TransformRect (new CGRect (4, 5, 6, 7));

			Assert.AreEqual ((nfloat) 24, rect.X, "X");
			Assert.AreEqual ((nfloat) 34, rect.Y, "Y");
			Assert.AreEqual ((nfloat) 27, rect.Width, "Width");
			Assert.AreEqual ((nfloat) 40, rect.Height, "Height");
		}

		[Test]
		public void Invert ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6).Invert ();

			Assert.AreEqual ((nfloat) (-2), transform.A, "A");
			Assert.AreEqual ((nfloat) 1, transform.B, "B");
			Assert.AreEqual ((nfloat) 1.5, transform.C, "C");
			Assert.AreEqual ((nfloat) (-0.5), transform.D, "D");
			Assert.AreEqual ((nfloat) 1.0, transform.Tx, "Tx");
			Assert.AreEqual ((nfloat) (-2.0), transform.Ty, "Ty");
		}

		[Test]
		public void Decompose ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			var components = new CGAffineTransform (1, 2, 3, 4, 5, 6).Decompose ();
			Assert.AreNotEqual (0.0, components.Scale);
			Assert.AreNotEqual (0.0, components.HorizontalShear);
			Assert.AreNotEqual (0.0, components.Rotation);
			Assert.AreNotEqual (new CGVector ((nfloat) 0, (nfloat) 0), components.Translation);
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
			Assert.AreNotEqual (0.0, transform.A);
			Assert.AreNotEqual (0.0, transform.B);
			Assert.AreNotEqual (0.0, transform.C);
			Assert.AreNotEqual (0.0, transform.D);
			Assert.AreNotEqual (0.0, transform.Tx);
			Assert.AreNotEqual (0.0, transform.Ty);
		}

		[Test]
		public void NSValueRoundtrip ()
		{
			var transform = new CGAffineTransform (1, 2, 3, 4, 5, 6);
			// looks simplistic but that NSValue logic is implemented by "us" on macOS
			using (var nsv = NSValue.FromCGAffineTransform (transform)) {
				var tback = nsv.CGAffineTransformValue;
				Assert.AreEqual ((nfloat) 1, tback.A, "A");
				Assert.AreEqual ((nfloat) 2, tback.B, "B");
				Assert.AreEqual ((nfloat) 3, tback.C, "C");
				Assert.AreEqual ((nfloat) 4, tback.D, "D");
				Assert.AreEqual ((nfloat) 5, tback.Tx, "Tx");
				Assert.AreEqual ((nfloat) 6, tback.Ty, "Ty");
			}
		}

		[Test]
		public unsafe void SizeOfTest ()
		{
			Assert.AreEqual (sizeof (CGAffineTransform), Marshal.SizeOf<CGAffineTransform> ());
		}

		[Test]
		public void ToStringTest ()
		{
			var transform = new CGAffineTransform ((nfloat) 1, (nfloat) 2, (nfloat) 3, (nfloat) 4, (nfloat) 5, (nfloat) 6);
			Assert.AreEqual ("[1, 2, 3, 4, 5, 6]", transform.ToString (), "ToString");
		}
	}


}
