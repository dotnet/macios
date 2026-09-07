// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

using Bindings.Test;

using NUnit.Framework;

namespace MonoTouchFixtures.ObjCRuntime {
	// Probe tests for the issue #25861 redesign. They capture the current (baseline)
	// behavior of two scenarios that any redesigned object registration must preserve:
	//
	//  1. A user type whose native 'init' calls a method overridden in managed code:
	//     the overridden managed method must be invoked on the correct instance while
	//     'init' is still executing.
	//  2. A directly-bound (platform-like, no gchandle ivar) type whose native 'init'
	//     synchronously surfaces 'self' to managed code: managed code must resolve
	//     'self' to the very wrapper being constructed (no duplicate wrapper).
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class InitCallbackProbeTest {
		// --- Scenario 1: init calls an overridden managed method --------------------

		class OverridingSubclass : InitCallsVirtualMethod {
			public bool OverrideCalled;
			public NSObject InstanceAtCall;
			public int ValueReceived;

			public override void VirtualMethodCalledDuringInit (int value)
			{
				OverrideCalled = true;
				InstanceAtCall = this;
				ValueReceived = value;
			}
		}

		[Test]
		public void InitCallsOverriddenManagedMethod ()
		{
			var obj = new OverridingSubclass ();

			Assert.IsTrue (obj.OverrideCalled, "overridden managed method was called during init");
			Assert.AreSame (obj, obj.InstanceAtCall, "overridden method was called on the instance being constructed");
			Assert.AreEqual (0x1234, obj.ValueReceived, "the argument was marshalled correctly");
		}

		// --- Scenario 2: platform-like init surfaces self to managed ----------------

		[DllImport ("__Internal")]
		unsafe static extern void x_set_init_self_callback (delegate* unmanaged<IntPtr, void> callback);

		static NSObject resolvedDuringInit;

		[UnmanagedCallersOnly]
		static void OnInitSelf (IntPtr self)
		{
			resolvedDuringInit = Runtime.GetNSObject (self);
		}

		[Test]
		public unsafe void PlatformInitSurfacingSelfResolvesToSameWrapper ()
		{
			resolvedDuringInit = null;
			x_set_init_self_callback (&OnInitSelf);
			try {
				var obj = new InitSurfacesSelfToManaged ();

				Assert.IsNotNull (resolvedDuringInit, "self was resolved to a managed wrapper during init");
				Assert.AreSame (obj, resolvedDuringInit, "self resolved to the wrapper being constructed (no duplicate wrapper)");
			} finally {
				x_set_init_self_callback (null);
				resolvedDuringInit = null;
			}
		}
	}
}
