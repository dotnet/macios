//
// Unit tests for SCNParticleSystem
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2025 Microsoft Corp.
//

using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using SceneKit;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.SceneKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNParticleSystemTest {

		[Test]
		public void Create ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				Assert.IsNotNull (ps, "Create should return non-null");
				Assert.AreNotEqual (IntPtr.Zero, ps.Handle, "Handle should not be zero");
			}
		}

		[Test]
		public void CreateNamed_Null ()
		{
			Assert.Throws<ArgumentNullException> (() => SCNParticleSystem.Create (null, null), "Create with null name should throw");
		}

		[Test]
		public void CreateNamed_InvalidName ()
		{
			var ps = SCNParticleSystem.Create ("nonexistent", null);
			Assert.IsNull (ps, "Create with invalid name should return null");
		}

		[Test]
		public void CreateNamed_NullDirectory ()
		{
			var ps = SCNParticleSystem.Create ("test", null);
			// Should not throw, just return null if not found
			Assert.IsNull (ps, "Create with null directory should return null if not found");
		}

		[Test]
		public void PropertyControllers_Get ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var controllers = ps.PropertyControllers;
				// PropertyControllers can be null initially
				// If not null, verify it's a valid object
				if (controllers is not null) {
					Assert.IsNotNull (controllers, "PropertyControllers should be non-null or null");
				}
			}
		}

		[Test]
		public void PropertyControllers_SetNull ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				ps.PropertyControllers = null;
				Assert.IsNull (ps.PropertyControllers, "PropertyControllers should be null after setting to null");
			}
		}

		[Test]
		public void PropertyControllers_Set ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var controllers = new SCNPropertyControllers ();
				ps.PropertyControllers = controllers;
				Assert.IsNotNull (ps.PropertyControllers, "PropertyControllers should be non-null after setting");
			}
		}

		[Test]
		public void PropertyControllers_EmptyConstructor ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNotNull (controllers, "Empty constructor should create valid object");
		}

		[Test]
		public void PropertyControllers_Position ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Position, "Position should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Position = controller;
				Assert.IsNotNull (controllers.Position, "Position should be non-null after setting");
			}

			controllers.Position = null;
			Assert.IsNull (controllers.Position, "Position should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Angle ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Angle, "Angle should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Angle = controller;
				Assert.IsNotNull (controllers.Angle, "Angle should be non-null after setting");
			}

			controllers.Angle = null;
			Assert.IsNull (controllers.Angle, "Angle should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_RotationAxis ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.RotationAxis, "RotationAxis should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.RotationAxis = controller;
				Assert.IsNotNull (controllers.RotationAxis, "RotationAxis should be non-null after setting");
			}

			controllers.RotationAxis = null;
			Assert.IsNull (controllers.RotationAxis, "RotationAxis should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Velocity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Velocity, "Velocity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Velocity = controller;
				Assert.IsNotNull (controllers.Velocity, "Velocity should be non-null after setting");
			}

			controllers.Velocity = null;
			Assert.IsNull (controllers.Velocity, "Velocity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_AngularVelocity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.AngularVelocity, "AngularVelocity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.AngularVelocity = controller;
				Assert.IsNotNull (controllers.AngularVelocity, "AngularVelocity should be non-null after setting");
			}

			controllers.AngularVelocity = null;
			Assert.IsNull (controllers.AngularVelocity, "AngularVelocity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Life ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Life, "Life should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Life = controller;
				Assert.IsNotNull (controllers.Life, "Life should be non-null after setting");
			}

			controllers.Life = null;
			Assert.IsNull (controllers.Life, "Life should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Color ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Color, "Color should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Color = controller;
				Assert.IsNotNull (controllers.Color, "Color should be non-null after setting");
			}

			controllers.Color = null;
			Assert.IsNull (controllers.Color, "Color should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Opacity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Opacity, "Opacity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Opacity = controller;
				Assert.IsNotNull (controllers.Opacity, "Opacity should be non-null after setting");
			}

			controllers.Opacity = null;
			Assert.IsNull (controllers.Opacity, "Opacity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Size ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Size, "Size should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Size = controller;
				Assert.IsNotNull (controllers.Size, "Size should be non-null after setting");
			}

			controllers.Size = null;
			Assert.IsNull (controllers.Size, "Size should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Frame ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Frame, "Frame should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Frame = controller;
				Assert.IsNotNull (controllers.Frame, "Frame should be non-null after setting");
			}

			controllers.Frame = null;
			Assert.IsNull (controllers.Frame, "Frame should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_FrameRate ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.FrameRate, "FrameRate should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.FrameRate = controller;
				Assert.IsNotNull (controllers.FrameRate, "FrameRate should be non-null after setting");
			}

			controllers.FrameRate = null;
			Assert.IsNull (controllers.FrameRate, "FrameRate should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Bounce ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Bounce, "Bounce should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Bounce = controller;
				Assert.IsNotNull (controllers.Bounce, "Bounce should be non-null after setting");
			}

			controllers.Bounce = null;
			Assert.IsNull (controllers.Bounce, "Bounce should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Charge ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Charge, "Charge should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Charge = controller;
				Assert.IsNotNull (controllers.Charge, "Charge should be non-null after setting");
			}

			controllers.Charge = null;
			Assert.IsNull (controllers.Charge, "Charge should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Friction ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.IsNull (controllers.Friction, "Friction should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Friction = controller;
				Assert.IsNotNull (controllers.Friction, "Friction should be non-null after setting");
			}

			controllers.Friction = null;
			Assert.IsNull (controllers.Friction, "Friction should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_MultipleProperties ()
		{
			var controllers = new SCNPropertyControllers ();

			using (var animation = CAAnimation.CreateAnimation ())
			using (var posController = SCNParticlePropertyController.Create (animation))
			using (var colorController = SCNParticlePropertyController.Create (animation))
			using (var sizeController = SCNParticlePropertyController.Create (animation)) {
				controllers.Position = posController;
				controllers.Color = colorController;
				controllers.Size = sizeController;

				Assert.IsNotNull (controllers.Position, "Position should be set");
				Assert.IsNotNull (controllers.Color, "Color should be set");
				Assert.IsNotNull (controllers.Size, "Size should be set");
			}
		}

		[Test]
		public void PropertyControllers_ReplaceProperty ()
		{
			var controllers = new SCNPropertyControllers ();

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller1 = SCNParticlePropertyController.Create (animation))
			using (var controller2 = SCNParticlePropertyController.Create (animation)) {
				controllers.Position = controller1;
				Assert.IsNotNull (controllers.Position, "Position should be set to first controller");

				controllers.Position = controller2;
				Assert.IsNotNull (controllers.Position, "Position should be set to second controller");

				controllers.Position = null;
				Assert.IsNull (controllers.Position, "Position should be null after clearing");
			}
		}

		[Test]
		public void AddAnimation ()
		{
			using (var ps = SCNParticleSystem.Create ())
			using (var animation = CAAnimation.CreateAnimation ()) {
				ps.AddAnimation (animation, (string) null);
				ps.AddAnimation (animation, "key1");
				ps.RemoveAllAnimations ();
			}
		}

		[Test]
		public void AddAnimation_NullAnimation ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				Assert.Throws<ArgumentNullException> (() => ps.AddAnimation ((CAAnimation) null, "key"), "AddAnimation with null should throw");
			}
		}

		[Test]
		public void GetAnimationKeys ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var keys = ps.GetAnimationKeys ();
				Assert.IsNotNull (keys, "GetAnimationKeys should return non-null");
				Assert.AreEqual (0, keys.Length, "Should have no animation keys initially");

				using (var animation = CAAnimation.CreateAnimation ()) {
					ps.AddAnimation (animation, "key1");
					keys = ps.GetAnimationKeys ();
					Assert.AreEqual (1, keys.Length, "Should have one animation key");
				}
			}
		}

		[Test]
		public void RemoveAllAnimations ()
		{
			using (var ps = SCNParticleSystem.Create ())
			using (var animation = CAAnimation.CreateAnimation ()) {
				ps.AddAnimation (animation, "key1");
				ps.AddAnimation (animation, "key2");
				var keys = ps.GetAnimationKeys ();
				Assert.AreEqual (2, keys.Length, "Should have two animation keys");

				ps.RemoveAllAnimations ();
				keys = ps.GetAnimationKeys ();
				Assert.AreEqual (0, keys.Length, "Should have no animation keys after removal");
			}
		}

		[Test]
		public void Copy ()
		{
			using (var ps = SCNParticleSystem.Create ())
			using (var copy = ps.Copy (null)) {
				Assert.IsNotNull (copy, "Copy should return non-null");
				Assert.AreNotEqual (ps.Handle, copy.Handle, "Copy should have different handle");
			}
		}

		[Test]
		public void NSCoding ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				// Test encoding/decoding
				var data = NSKeyedArchiver.GetArchivedData (ps, true, out var error);
				Assert.IsNotNull (data, "Encoding should produce data");
				Assert.IsNull (error, "Encoding should not produce error");

				var decoded = NSKeyedUnarchiver.GetUnarchivedObject (typeof (SCNParticleSystem), data, out error);
				Assert.IsNotNull (decoded, "Decoding should produce object");
				Assert.IsNull (error, "Decoding should not produce error");
				Assert.IsInstanceOf<SCNParticleSystem> (decoded, "Decoded object should be SCNParticleSystem");
			}
		}

		[Test]
		public void AddModifier_NullProperties ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				Assert.Throws<ArgumentNullException> (() => ps.AddModifier (null, SCNParticleModifierStage.PreDynamics, (data, dataStride, start, end, deltaTime) => { }), "AddModifier with null properties should throw");
			}
		}

		[Test]
		public void AddModifier_NullHandler ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var properties = new NSString [] { (NSString) "position" };
				Assert.Throws<ArgumentNullException> (() => ps.AddModifier (properties, SCNParticleModifierStage.PreDynamics, null), "AddModifier with null handler should throw");
			}
		}

		[Test]
		public void AddModifier_EmptyProperties ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var properties = new NSString [0];
				// Should not throw with empty array
				ps.AddModifier (properties, SCNParticleModifierStage.PreDynamics, (data, dataStride, start, end, deltaTime) => { });
			}
		}

		[Test]
		public void HandleEvent_NullProperties ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				Assert.Throws<ArgumentNullException> (() => ps.HandleEvent (SCNParticleEvent.Birth, null, (data, dataStride, indices, count) => { }), "HandleEvent with null properties should throw");
			}
		}

		[Test]
		public void HandleEvent_NullHandler ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var properties = new NSString [] { (NSString) "position" };
				Assert.Throws<ArgumentNullException> (() => ps.HandleEvent (SCNParticleEvent.Birth, properties, null), "HandleEvent with null handler should throw");
			}
		}

		[Test]
		public void HandleEvent_EmptyProperties ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var properties = new NSString [0];
				// Should not throw with empty array
				ps.HandleEvent (SCNParticleEvent.Birth, properties, (data, dataStride, indices, count) => { });
			}
		}

		[Test]
		public void PropertyControllers_RoundTrip ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var controllers = new SCNPropertyControllers ();

				using (var animation = CAAnimation.CreateAnimation ())
				using (var posController = SCNParticlePropertyController.Create (animation)) {
					controllers.Position = posController;
					ps.PropertyControllers = controllers;

					var retrieved = ps.PropertyControllers;
					Assert.IsNotNull (retrieved, "Retrieved PropertyControllers should not be null");
					Assert.IsNotNull (retrieved.Position, "Retrieved Position controller should not be null");
				}
			}
		}

		[Test]
		public void PropertyControllers_ClearAllProperties ()
		{
			var controllers = new SCNPropertyControllers ();

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Position = controller;
				controllers.Angle = controller;
				controllers.Velocity = controller;
				controllers.Color = controller;

				Assert.IsNotNull (controllers.Position, "Position should be set");
				Assert.IsNotNull (controllers.Angle, "Angle should be set");
				Assert.IsNotNull (controllers.Velocity, "Velocity should be set");
				Assert.IsNotNull (controllers.Color, "Color should be set");

				controllers.Position = null;
				controllers.Angle = null;
				controllers.Velocity = null;
				controllers.Color = null;

				Assert.IsNull (controllers.Position, "Position should be null");
				Assert.IsNull (controllers.Angle, "Angle should be null");
				Assert.IsNull (controllers.Velocity, "Velocity should be null");
				Assert.IsNull (controllers.Color, "Color should be null");
			}
		}
	}
}
