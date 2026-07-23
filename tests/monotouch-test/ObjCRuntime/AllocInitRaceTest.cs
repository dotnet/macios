// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using Foundation;
using ObjCRuntime;

using Bindings.Test;

using NUnit.Framework;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AllocInitRaceTest {
		// Reproduction for issue #25861 (and #9478).
		//
		// When a native 'init' method returns a different handle than the one 'alloc'
		// created, there's a window during which the freed 'alloc' address is still
		// registered in the runtime's object_map (mapping that address to the object
		// being constructed). If, during that window, another thread allocates a managed
		// object that happens to reuse the freed address, the object_map entry is
		// overwritten to point at the new object; when the first object then rebinds its
		// handle to the value returned by 'init', it unregisters the (reused) address,
		// removing the second object from the object_map. A later native->managed marshal
		// of the second object then fails with:
		//
		//   Failed to marshal the Objective-C object 0x... (type: __MonoMac_NSAsyncActionDispatcher).
		//   Could not find an existing managed instance for this object [...]
		//
		// This test stresses that code path: one thread constantly constructs objects
		// whose native 'init' frees the 'alloc' address (widening the reuse window with a
		// tiny sleep), while another thread constantly schedules NSAsyncActionDispatcher
		// instances (via BeginInvokeOnMainThread) whose native addresses may reuse those
		// freed addresses. The main run loop, pumped below, services the dispatchers; if
		// the race triggers, marshalling the dispatcher back to managed code throws.
		[Test]
		public void RegistrationSurvivesAllocInitAddressReuse ()
		{
			var duration = TimeSpan.FromSeconds (10);

			var stop = false;
			Exception rebindException = null;
			Exception dispatcherException = null;

			// Thread that constantly constructs objects whose native 'init' returns a
			// different handle than 'alloc', freeing the 'alloc' address so it can be
			// reused by another thread.
			var rebindThread = new Thread (() => {
				try {
					while (!Volatile.Read (ref stop)) {
						using (var obj = new InitReturnsDifferentObjectAfterSleep ()) {
						}
					}
				} catch (Exception e) {
					rebindException = e;
				}
			}) {
				IsBackground = true,
				Name = "alloc-init-rebind",
			};

			// Thread that constantly schedules NSAsyncActionDispatcher instances on the
			// main run loop. These are allocated on this (background) thread and may reuse
			// the addresses freed by the rebind thread.
			var pump = new NSObject ();
			var dispatcherThread = new Thread (() => {
				try {
					while (!Volatile.Read (ref stop)) {
						for (var i = 0; i < 100; i++)
							pump.BeginInvokeOnMainThread (() => { });
						Thread.Yield ();
					}
				} catch (Exception e) {
					dispatcherException = e;
				}
			}) {
				IsBackground = true,
				Name = "dispatcher-scheduler",
			};

			rebindThread.Start ();
			dispatcherThread.Start ();

			Exception runLoopException = null;
			var watch = Stopwatch.StartNew ();
			try {
				// Pump the main run loop, servicing the scheduled dispatchers. If the race
				// triggers, marshalling a dispatcher back to managed code throws here.
				while (watch.Elapsed < duration)
					NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (0.01));
			} catch (Exception e) {
				runLoopException = e;
			} finally {
				Volatile.Write (ref stop, true);
				rebindThread.Join ();
				dispatcherThread.Join ();
			}

			Assert.IsNull (rebindException, "rebind thread exception");
			Assert.IsNull (dispatcherException, "dispatcher thread exception");
			Assert.IsNull (runLoopException, "run loop marshalling exception (issue #25861)");
		}

		// Deterministic reproduction for issue #25861 (and #9478).
		//
		// While ReuseSlotClassA is executing its native 'init', it frees the alloc'd
		// instance and forces the next allocation to reuse that exact address, then calls
		// back into managed code which allocates a ReuseSlotClassB that deterministically
		// lands on that address. The managed runtime now maps the address to the
		// ReuseSlotClassB. ReuseSlotClassA's 'init' then returns a different instance, so
		// the ReuseSlotClassA wrapper rebinds its handle; a non-ownership-aware unregister
		// would, at that point, remove the ReuseSlotClassB (which now occupies the old
		// address) from the object_map. This makes the race deterministic, so we can assert
		// the outcome directly instead of stress-testing.
		[DllImport ("__Internal")]
		unsafe static extern void x_set_reuse_alloc_callback (delegate* unmanaged<IntPtr, void> callback);

		static ReuseSlotClassB reuseCreatedB;
		static IntPtr reuseReusedAddress;
		static IntPtr reuseOriginalAddress;

		[UnmanagedCallersOnly]
		static void OnReuseAlloc (IntPtr originalPtr)
		{
			// Allocate a ReuseSlotClassB; it deterministically reuses 'originalPtr'.
			reuseOriginalAddress = originalPtr;
			var b = new ReuseSlotClassB ();
			reuseCreatedB = b;
			reuseReusedAddress = b.Handle;
		}

		static void RunClobberScenario ()
		{
			reuseCreatedB = null;
			reuseReusedAddress = IntPtr.Zero;
			reuseOriginalAddress = IntPtr.Zero;
			unsafe {
				x_set_reuse_alloc_callback (&OnReuseAlloc);
			}
			try {
				GC.KeepAlive (new ReuseSlotClassA ());
			} finally {
				unsafe {
					x_set_reuse_alloc_callback (null);
				}
			}
		}

		[Test]
		public void ReusedAddressSurvivesAllocInitClobber ()
		{
			RunClobberScenario ();

			Assert.IsNotNull (reuseCreatedB, "the reused-address object was created");
			Assert.AreEqual (reuseOriginalAddress, reuseReusedAddress, "the ReuseSlotClassB reused the freed address");

			// The object_map entry for the reused address must still point at the very
			// ReuseSlotClassB we created (it must not have been clobbered when
			// ReuseSlotClassA rebound its handle).
			var resolved = Runtime.GetNSObject (reuseReusedAddress);
			Assert.AreSame (reuseCreatedB, resolved, "the reused address still resolves to the original object (issue #25861)");
		}

		[Test]
		public void ReusedAddressClobberedWithLegacySwitch ()
		{
			// With the legacy behavior (unconditional unregister on handle rebind), the
			// same scenario removes the reused-address object from the object_map, so it
			// no longer resolves to the original instance. This documents the bug and
			// proves the scenario genuinely exercises the code path the fix changes.
			var switchName = "ObjCRuntime.Runtime.RegisterObjectsBeforeInit";
			var hadSwitch = AppContext.TryGetSwitch (switchName, out var previous);
			AppContext.SetSwitch (switchName, true);
			try {
				RunClobberScenario ();

				Assert.IsNotNull (reuseCreatedB, "the reused-address object was created");
				Assert.AreEqual (reuseOriginalAddress, reuseReusedAddress, "the ReuseSlotClassB reused the freed address");
				var resolved = Runtime.GetNSObject (reuseReusedAddress);
				Assert.AreNotSame (reuseCreatedB, resolved, "with the legacy switch, the reused address was clobbered and resolves to a different (duplicate) wrapper");
			} finally {
				if (hadSwitch)
					AppContext.SetSwitch (switchName, previous);
				else
					AppContext.SetSwitch (switchName, false);
			}
		}
		[Test]
		public void InitReturnsNilDoesNotCrashOnGC ()
		{
			// Issue #23679: a failed 'init' (returns nil) that throws must not leave a
			// dangling managed reference that crashes during a later garbage collection.
			for (var i = 0; i < 100; i++) {
				Assert.Catch (() => GC.KeepAlive (new InitReturnsNilClass ()), "failed init should throw");
			}
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			// Getting here without crashing is the assertion (issue #23679).
			Assert.Pass ();
		}
	}
}
