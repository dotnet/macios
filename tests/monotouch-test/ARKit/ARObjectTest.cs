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
using Foundation;
using ObjCRuntime;
using Xamarin.Utils;

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

		[Test]
		public void ARWorldTrackingProvider_IsSupported ()
		{
			// Just verify the P/Invoke doesn't crash
			var supported = ARWorldTrackingProvider.IsSupported;
			// We can't assert the value since it depends on hardware
			Assert.IsNotNull (supported.ToString ());
		}

		[Test]
		public void ARWorldTrackingProvider_RequiredAuthorizationType ()
		{
			var authType = ARWorldTrackingProvider.RequiredAuthorizationType;
			// The required auth type should be a valid flags value
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
			// Provider starts in initialized state before being run in a session
			Assert.AreEqual (ARDataProviderState.Initialized, provider.State, "State");
		}

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
		public void ARDeviceAnchor_Create ()
		{
			using var anchor = new ARDeviceAnchor ();
			Assert.AreNotEqual (IntPtr.Zero, anchor.Handle, "Handle");
		}

		[Test]
		public void ARDeviceAnchor_Identifier ()
		{
			using var anchor = new ARDeviceAnchor ();
			// Freshly created device anchor should have a valid (non-default) identifier
			var id = anchor.Identifier;
			Assert.IsNotNull (id.ToString ());
		}

		[Test]
		public void ARDeviceAnchor_TrackingState ()
		{
			using var anchor = new ARDeviceAnchor ();
			// A newly created device anchor hasn't been queried yet
			var state = anchor.TrackingState;
			Assert.That ((long) state, Is.GreaterThanOrEqualTo (0).And.LessThanOrEqualTo (2),
				"TrackingState should be a valid enum value");
		}

		[Test]
		public void ARError_ErrorDomain ()
		{
			var domain = ARError.ErrorDomain;
			// ErrorDomain should return a non-null string constant
			Assert.IsNotNull (domain, "ErrorDomain");
			Assert.AreNotEqual (0, domain!.Length, "ErrorDomain.Length");
		}

		[Test]
		public void ARAuthorizationType_Flags ()
		{
			// Verify flag values match the Apple header definitions
			Assert.AreEqual ((ulong) 0, (ulong) ARAuthorizationType.None, "None");
			Assert.AreEqual ((ulong) 1, (ulong) ARAuthorizationType.HandTracking, "HandTracking");
			Assert.AreEqual ((ulong) 2, (ulong) ARAuthorizationType.WorldSensing, "WorldSensing");
			Assert.AreEqual ((ulong) 8, (ulong) ARAuthorizationType.CameraAccess, "CameraAccess");
		}

		[Test]
		public void ARDataProviderState_Values ()
		{
			// Verify enum values match Apple header definitions
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
	}
}

#endif // __MACOS__
