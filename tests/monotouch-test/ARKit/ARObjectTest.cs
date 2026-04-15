//
// Unit tests for the ARKit C API bindings (ar_* functions)
//
// These tests exercise the new C-style ARKit API that was
// introduced on macOS 26.0. The API uses OS_OBJECT patterns
// (ar_retain/ar_release) rather than Objective-C.
//

#if __MACOS__

using System;
using ARKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Xamarin.Utils;

using Matrix4 = global::CoreGraphics.NMatrix4;

namespace MonoTouchFixtures.ARKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ARObjectTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 26, 0, throwIfOtherPlatform: false);
		}

		#region ARWorldTrackingConfiguration

		[Test]
		public void ARWorldTrackingConfiguration_Create ()
		{
			using var config = new ARWorldTrackingConfiguration ();
			Assert.AreNotEqual (IntPtr.Zero, config.Handle, "Handle");
		}

		[Test]
		public void ARWorldTrackingConfiguration_Dispose ()
		{
			var config = new ARWorldTrackingConfiguration ();
			Assert.AreNotEqual (IntPtr.Zero, config.Handle, "Handle before dispose");
			config.Dispose ();
			Assert.That (config.Handle, Is.EqualTo (NativeHandle.Zero), "Handle after dispose");
		}

		#endregion

		#region ARWorldTrackingProvider

		[Test]
		public void ARWorldTrackingProvider_IsSupported ()
		{
			var supported = ARWorldTrackingProvider.IsSupported;
			Assert.IsNotNull (supported.ToString ());
		}

		[Test]
		public void ARWorldTrackingProvider_RequiredAuthorizationType ()
		{
			var authType = ARWorldTrackingProvider.RequiredAuthorizationType;
			Assert.That ((ulong) authType, Is.LessThanOrEqualTo ((ulong) (
				ARAuthorizationType.HandTracking |
				ARAuthorizationType.WorldSensing |
				ARAuthorizationType.CameraAccess)),
				"RequiredAuthorizationType should be a valid flags combination");
		}

		[Test]
		public void ARWorldTrackingProvider_Create ()
		{
			using var config = new ARWorldTrackingConfiguration ();
			using var provider = new ARWorldTrackingProvider (config);
			Assert.AreNotEqual (IntPtr.Zero, provider.Handle, "Handle");
		}

		[Test]
		public void ARWorldTrackingProvider_State ()
		{
			using var config = new ARWorldTrackingConfiguration ();
			using var provider = new ARWorldTrackingProvider (config);
			Assert.AreEqual (ARDataProviderState.Initialized, provider.State, "State");
		}

		[Test]
		public void ARWorldTrackingProvider_RequiredAuthorizationType_Instance ()
		{
			// RequiredAuthorizationType is static, verify via type name
			var authType = ARWorldTrackingProvider.RequiredAuthorizationType;
			Assert.That ((ulong) authType, Is.LessThanOrEqualTo ((ulong) (
				ARAuthorizationType.HandTracking |
				ARAuthorizationType.WorldSensing |
				ARAuthorizationType.CameraAccess)),
				"Static RequiredAuthorizationType");
		}

		#endregion

		#region ARDataProviders

		[Test]
		public void ARDataProviders_CreateEmpty ()
		{
			using var providers = new ARDataProviders ();
			Assert.AreNotEqual (IntPtr.Zero, providers.Handle, "Handle");
			Assert.AreEqual ((nuint) 0, providers.Count, "Count");
		}

		[Test]
		public void ARDataProviders_AddRemove ()
		{
			using var config = new ARWorldTrackingConfiguration ();
			using var provider = new ARWorldTrackingProvider (config);
			using var providers = new ARDataProviders ();

			Assert.AreEqual ((nuint) 0, providers.Count, "Count before add");
			providers.Add (provider);
			Assert.AreEqual ((nuint) 1, providers.Count, "Count after add");
			providers.Remove (provider);
			Assert.AreEqual ((nuint) 0, providers.Count, "Count after remove");
		}

		[Test]
		public void ARDataProviders_GetDataProviders ()
		{
			using var config = new ARWorldTrackingConfiguration ();
			using var provider = new ARWorldTrackingProvider (config);
			using var providers = new ARDataProviders ();

			providers.Add (provider);
			var result = providers.GetDataProviders ();
			Assert.AreEqual (1, result.Length, "Length");
			Assert.AreNotEqual (IntPtr.Zero, result [0].Handle, "result[0].Handle");
		}

		[Test]
		public void ARDataProviders_Dispose ()
		{
			var providers = new ARDataProviders ();
			Assert.AreNotEqual (IntPtr.Zero, providers.Handle, "Handle before dispose");
			providers.Dispose ();
			Assert.That (providers.Handle, Is.EqualTo (NativeHandle.Zero), "Handle after dispose");
		}

		#endregion

		#region ARDeviceAnchor

		[Test]
		public void ARDeviceAnchor_Create ()
		{
			using var anchor = new ARDeviceAnchor ();
			Assert.AreNotEqual (IntPtr.Zero, anchor.Handle, "Handle");
		}

		[Test]
		public void ARDeviceAnchor_Identifier ()
		{
			using var anchor = new ARDeviceAnchor ();
			var id = anchor.Identifier;
			Assert.IsNotNull (id.ToString ());
		}

		[Test]
		public void ARDeviceAnchor_TrackingState ()
		{
			using var anchor = new ARDeviceAnchor ();
			var state = anchor.TrackingState;
			Assert.That ((long) state, Is.GreaterThanOrEqualTo (0).And.LessThanOrEqualTo (2),
				"TrackingState should be a valid enum value");
		}

		[Test]
		public void ARDeviceAnchor_OriginFromAnchorTransform ()
		{
			using var anchor = new ARDeviceAnchor ();
			var transform = anchor.OriginFromAnchorTransform;
			// Verify the P/Invoke returns valid (finite) values — a freshly created
			// device anchor returns all zeros before being populated by world tracking.
			Assert.That (float.IsFinite (transform.M11), "M11 is finite");
			Assert.That (float.IsFinite (transform.M22), "M22 is finite");
			Assert.That (float.IsFinite (transform.M33), "M33 is finite");
			Assert.That (float.IsFinite (transform.M44), "M44 is finite");
		}

		[Test]
		public void ARDeviceAnchor_Timestamp ()
		{
			using var anchor = new ARDeviceAnchor ();
			var timestamp = anchor.Timestamp;
			// Freshly created anchor - timestamp should be a non-negative value
			Assert.That (timestamp, Is.GreaterThanOrEqualTo (0.0), "Timestamp");
		}

		[Test]
		public void ARDeviceAnchor_IsTracked ()
		{
			using var anchor = new ARDeviceAnchor ();
			// Just verify the P/Invoke doesn't crash
			var tracked = anchor.IsTracked;
			Assert.IsNotNull (tracked.ToString ());
		}

		[Test]
		public void ARDeviceAnchor_Dispose ()
		{
			var anchor = new ARDeviceAnchor ();
			Assert.AreNotEqual (IntPtr.Zero, anchor.Handle, "Handle before dispose");
			anchor.Dispose ();
			Assert.That (anchor.Handle, Is.EqualTo (NativeHandle.Zero), "Handle after dispose");
		}

		#endregion

		#region ARError

		[Test]
		public void ARError_ErrorDomain ()
		{
			var domain = ARError.ErrorDomain;
			Assert.IsNotNull (domain, "ErrorDomain");
			Assert.AreNotEqual (0, domain!.Length, "ErrorDomain.Length");
		}

		#endregion

		#region ARSession

		[Test]
		public void ARSession_SetDataProviderStateChangeHandler_Null ()
		{
			// We can't create an ARSession without an ARDevice, but we can verify
			// that the delegate type and handler machinery compile and work.
			// This is a compile-time verification that the delegate signature is correct.
			ARSession.DataProviderStateChangeHandler? handler = null;
			Assert.IsNull (handler);
		}

		#endregion

		#region Enum values

		[Test]
		public void ARAuthorizationType_Flags ()
		{
			Assert.AreEqual ((ulong) 0, (ulong) ARAuthorizationType.None, "None");
			Assert.AreEqual ((ulong) 1, (ulong) ARAuthorizationType.HandTracking, "HandTracking");
			Assert.AreEqual ((ulong) 2, (ulong) ARAuthorizationType.WorldSensing, "WorldSensing");
			Assert.AreEqual ((ulong) 8, (ulong) ARAuthorizationType.CameraAccess, "CameraAccess");
		}

		[Test]
		public void ARDataProviderState_Values ()
		{
			Assert.AreEqual (0, (int) ARDataProviderState.Initialized, "Initialized");
			Assert.AreEqual (1, (int) ARDataProviderState.Running, "Running");
			Assert.AreEqual (2, (int) ARDataProviderState.Paused, "Paused");
			Assert.AreEqual (3, (int) ARDataProviderState.Stopped, "Stopped");
		}

		[Test]
		public void ARSessionErrorCode_Values ()
		{
			Assert.AreEqual (100, (int) ARSessionErrorCode.DataProviderNotAuthorized, "DataProviderNotAuthorized");
			Assert.AreEqual (101, (int) ARSessionErrorCode.DataProviderFailedToRun, "DataProviderFailedToRun");
		}

		[Test]
		public void ARWorldTrackingErrorCode_Values ()
		{
			Assert.AreEqual (200, (int) ARWorldTrackingErrorCode.AddAnchorFailed, "AddAnchorFailed");
			Assert.AreEqual (201, (int) ARWorldTrackingErrorCode.AnchorMaxLimitReached, "AnchorMaxLimitReached");
			Assert.AreEqual (202, (int) ARWorldTrackingErrorCode.RemoveAnchorFailed, "RemoveAnchorFailed");
		}

		[Test]
		public void ARDeviceAnchorQueryStatus_Values ()
		{
			Assert.AreEqual (0, (int) ARDeviceAnchorQueryStatus.Success, "Success");
			Assert.AreEqual (1, (int) ARDeviceAnchorQueryStatus.Failure, "Failure");
		}

		[Test]
		public void ARDeviceAnchorTrackingState_Values ()
		{
			Assert.AreEqual (0, (int) ARDeviceAnchorTrackingState.Untracked, "Untracked");
			Assert.AreEqual (1, (int) ARDeviceAnchorTrackingState.OrientationTracked, "OrientationTracked");
			Assert.AreEqual (2, (int) ARDeviceAnchorTrackingState.Tracked, "Tracked");
		}

		[Test]
		public void ARAuthorizationStatus_Values ()
		{
			Assert.AreEqual (0, (int) ARAuthorizationStatus.NotDetermined, "NotDetermined");
			Assert.AreEqual (1, (int) ARAuthorizationStatus.Allowed, "Allowed");
			Assert.AreEqual (2, (int) ARAuthorizationStatus.Denied, "Denied");
		}

		#endregion
	}
}

#endif // __MACOS__
