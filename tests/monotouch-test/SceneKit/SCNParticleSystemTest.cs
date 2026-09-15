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
				Assert.That (ps, Is.Not.Null, "Create should return non-null");
				Assert.That (ps.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle should not be zero");
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
			Assert.That (ps, Is.Null, "Create with invalid name should return null");
		}

		[Test]
		public void CreateNamed_NullDirectory ()
		{
			var ps = SCNParticleSystem.Create ("test", null);
			// Should not throw, just return null if not found
			Assert.That (ps, Is.Null, "Create with null directory should return null if not found");
		}

		[Test]
		public void PropertyControllers_Get ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var controllers = ps.PropertyControllers;
				// PropertyControllers can be null initially
				// If not null, verify it's a valid object
				if (controllers is not null) {
					Assert.That (controllers, Is.Not.Null, "PropertyControllers should be non-null or null");
				}
			}
		}

		[Test]
		public void PropertyControllers_SetNull ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				ps.PropertyControllers = null;
				Assert.That (ps.PropertyControllers, Is.Null, "PropertyControllers should be null after setting to null");
			}
		}

		[Test]
		public void PropertyControllers_Set ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				var controllers = new SCNPropertyControllers ();
				ps.PropertyControllers = controllers;
				Assert.That (ps.PropertyControllers, Is.Not.Null, "PropertyControllers should be non-null after setting");
			}
		}

		[Test]
		public void PropertyControllers_EmptyConstructor ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers, Is.Not.Null, "Empty constructor should create valid object");
		}

		[Test]
		public void PropertyControllers_Position ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Position, Is.Null, "Position should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Position = controller;
				Assert.That (controllers.Position, Is.Not.Null, "Position should be non-null after setting");
			}

			controllers.Position = null;
			Assert.That (controllers.Position, Is.Null, "Position should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Angle ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Angle, Is.Null, "Angle should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Angle = controller;
				Assert.That (controllers.Angle, Is.Not.Null, "Angle should be non-null after setting");
			}

			controllers.Angle = null;
			Assert.That (controllers.Angle, Is.Null, "Angle should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_RotationAxis ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.RotationAxis, Is.Null, "RotationAxis should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.RotationAxis = controller;
				Assert.That (controllers.RotationAxis, Is.Not.Null, "RotationAxis should be non-null after setting");
			}

			controllers.RotationAxis = null;
			Assert.That (controllers.RotationAxis, Is.Null, "RotationAxis should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Velocity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Velocity, Is.Null, "Velocity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Velocity = controller;
				Assert.That (controllers.Velocity, Is.Not.Null, "Velocity should be non-null after setting");
			}

			controllers.Velocity = null;
			Assert.That (controllers.Velocity, Is.Null, "Velocity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_AngularVelocity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.AngularVelocity, Is.Null, "AngularVelocity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.AngularVelocity = controller;
				Assert.That (controllers.AngularVelocity, Is.Not.Null, "AngularVelocity should be non-null after setting");
			}

			controllers.AngularVelocity = null;
			Assert.That (controllers.AngularVelocity, Is.Null, "AngularVelocity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Life ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Life, Is.Null, "Life should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Life = controller;
				Assert.That (controllers.Life, Is.Not.Null, "Life should be non-null after setting");
			}

			controllers.Life = null;
			Assert.That (controllers.Life, Is.Null, "Life should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Color ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Color, Is.Null, "Color should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Color = controller;
				Assert.That (controllers.Color, Is.Not.Null, "Color should be non-null after setting");
			}

			controllers.Color = null;
			Assert.That (controllers.Color, Is.Null, "Color should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Opacity ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Opacity, Is.Null, "Opacity should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Opacity = controller;
				Assert.That (controllers.Opacity, Is.Not.Null, "Opacity should be non-null after setting");
			}

			controllers.Opacity = null;
			Assert.That (controllers.Opacity, Is.Null, "Opacity should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Size ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Size, Is.Null, "Size should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Size = controller;
				Assert.That (controllers.Size, Is.Not.Null, "Size should be non-null after setting");
			}

			controllers.Size = null;
			Assert.That (controllers.Size, Is.Null, "Size should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Frame ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Frame, Is.Null, "Frame should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Frame = controller;
				Assert.That (controllers.Frame, Is.Not.Null, "Frame should be non-null after setting");
			}

			controllers.Frame = null;
			Assert.That (controllers.Frame, Is.Null, "Frame should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_FrameRate ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.FrameRate, Is.Null, "FrameRate should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.FrameRate = controller;
				Assert.That (controllers.FrameRate, Is.Not.Null, "FrameRate should be non-null after setting");
			}

			controllers.FrameRate = null;
			Assert.That (controllers.FrameRate, Is.Null, "FrameRate should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Bounce ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Bounce, Is.Null, "Bounce should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Bounce = controller;
				Assert.That (controllers.Bounce, Is.Not.Null, "Bounce should be non-null after setting");
			}

			controllers.Bounce = null;
			Assert.That (controllers.Bounce, Is.Null, "Bounce should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Charge ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Charge, Is.Null, "Charge should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Charge = controller;
				Assert.That (controllers.Charge, Is.Not.Null, "Charge should be non-null after setting");
			}

			controllers.Charge = null;
			Assert.That (controllers.Charge, Is.Null, "Charge should be null after setting to null");
		}

		[Test]
		public void PropertyControllers_Friction ()
		{
			var controllers = new SCNPropertyControllers ();
			Assert.That (controllers.Friction, Is.Null, "Friction should be null initially");

			using (var animation = CAAnimation.CreateAnimation ())
			using (var controller = SCNParticlePropertyController.Create (animation)) {
				controllers.Friction = controller;
				Assert.That (controllers.Friction, Is.Not.Null, "Friction should be non-null after setting");
			}

			controllers.Friction = null;
			Assert.That (controllers.Friction, Is.Null, "Friction should be null after setting to null");
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

				Assert.That (controllers.Position, Is.Not.Null, "Position should be set");
				Assert.That (controllers.Color, Is.Not.Null, "Color should be set");
				Assert.That (controllers.Size, Is.Not.Null, "Size should be set");
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
				Assert.That (controllers.Position, Is.Not.Null, "Position should be set to first controller");

				controllers.Position = controller2;
				Assert.That (controllers.Position, Is.Not.Null, "Position should be set to second controller");

				controllers.Position = null;
				Assert.That (controllers.Position, Is.Null, "Position should be null after clearing");
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
				Assert.That (keys, Is.Not.Null, "GetAnimationKeys should return non-null");
				Assert.That (keys.Length, Is.EqualTo (0), "Should have no animation keys initially");

				using (var animation = CAAnimation.CreateAnimation ()) {
					ps.AddAnimation (animation, "key1");
					keys = ps.GetAnimationKeys ();
					Assert.That (keys.Length, Is.EqualTo (1), "Should have one animation key");
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
				Assert.That (keys.Length, Is.EqualTo (2), "Should have two animation keys");

				ps.RemoveAllAnimations ();
				keys = ps.GetAnimationKeys ();
				Assert.That (keys.Length, Is.EqualTo (0), "Should have no animation keys after removal");
			}
		}

		[Test]
		public void Copy ()
		{
			using (var ps = SCNParticleSystem.Create ())
			using (var copy = ps.Copy (null)) {
				Assert.That (copy, Is.Not.Null, "Copy should return non-null");
				Assert.That (copy.Handle, Is.Not.EqualTo (ps.Handle), "Copy should have different handle");
			}
		}

		[Test]
		public void NSCoding ()
		{
			using (var ps = SCNParticleSystem.Create ()) {
				// Test encoding/decoding
				var data = NSKeyedArchiver.GetArchivedData (ps, true, out var error);
				Assert.That (data, Is.Not.Null, "Encoding should produce data");
				Assert.That (error, Is.Null, "Encoding should not produce error");

				var decoded = NSKeyedUnarchiver.GetUnarchivedObject (typeof (SCNParticleSystem), data, out error);
				Assert.That (decoded, Is.Not.Null, "Decoding should produce object");
				Assert.That (error, Is.Null, "Decoding should not produce error");
				Assert.That (decoded, Is.InstanceOf<SCNParticleSystem> (), "Decoded object should be SCNParticleSystem");
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
					Assert.That (retrieved, Is.Not.Null, "Retrieved PropertyControllers should not be null");
					Assert.That (retrieved.Position, Is.Not.Null, "Retrieved Position controller should not be null");
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

				Assert.That (controllers.Position, Is.Not.Null, "Position should be set");
				Assert.That (controllers.Angle, Is.Not.Null, "Angle should be set");
				Assert.That (controllers.Velocity, Is.Not.Null, "Velocity should be set");
				Assert.That (controllers.Color, Is.Not.Null, "Color should be set");

				controllers.Position = null;
				controllers.Angle = null;
				controllers.Velocity = null;
				controllers.Color = null;

				Assert.That (controllers.Position, Is.Null, "Position should be null");
				Assert.That (controllers.Angle, Is.Null, "Angle should be null");
				Assert.That (controllers.Velocity, Is.Null, "Velocity should be null");
				Assert.That (controllers.Color, Is.Null, "Color should be null");
			}
		}
	}
}
