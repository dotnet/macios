﻿#if !__WATCHOS__

using System;
#if XAMCORE_2_0
using Foundation;
#if !MONOMAC
using UIKit;
#endif
using SpriteKit;
using ObjCRuntime;
#else
using MonoTouch.Foundation;
using MonoTouch.SpriteKit;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
#endif
using OpenTK;
using NUnit.Framework;

namespace MonoTouchFixtures.SpriteKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SKTransformNodeTest {
		[SetUp]
		public void VersionCheck ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
		}

		[Test]
		public void EulerAngles ()
		{
			Vector3 V3;

			using (var obj = new SKTransformNode ()) {
				Asserts.AreEqual (Vector3.Zero, obj.EulerAngles, "1 EulerAngles");
				V3 = new Vector3 (1, 2, 3);
				obj.EulerAngles = V3;
				// The values bellow match what the same code in Swift returns.
				Assert.AreEqual (-2.14159298f, obj.EulerAngles.X, "#x1");
				Assert.AreEqual (1.14159274f, obj.EulerAngles.Y, "#y1");
				Assert.AreEqual (-0.141592711f, obj.EulerAngles.Z, "#z1");
			}
		}

		[Test]
		public void RotationMatrix ()
		{
			using (var obj = new SKTransformNode ()) {
				obj.RotationMatrix = Matrix3.Zero;
				// In Swift, a rotated zero matrice also becomes the identity matrice.
				Asserts.AreEqual (Matrix3.Identity, obj.RotationMatrix, "RotationMatrix");
			}
		}

		[Test]
		public void QuaternionTest ()
		{
			Quaternion Q;

			using (var obj = new SKTransformNode ()) {
				Asserts.AreEqual (Quaternion.Identity, obj.Quaternion, "1 Quaternion");
				Q = new Quaternion (new Vector3 (1, 2, 3), 4);
				obj.Quaternion = Q;
				Asserts.AreEqual (Q, obj.Quaternion, "2 Quaternion");
			}
		}
	}
}

#endif // !__WATCHOS__
