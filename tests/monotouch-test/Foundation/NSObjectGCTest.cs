#if __MACOS__
using System.Collections.Generic;
using System.Threading;

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSObjectGCTest {
		// The runtime overrides release on the first managed subclass; observe it on the second.
		class DrainObjectBase : NSObject { }

		class DrainObject : DrainObjectBase {
			readonly Action released;
			bool queued;

			public DrainObject (Action released)
			{
				this.released = released;
			}

			public void QueueForFinalization ()
			{
				queued = true;
				Dispose (false);
			}

			[Export ("release")]
			void Release ()
			{
				if (queued)
					released ();
				Messaging.objc_super super;
				super.Handle = Handle;
				super.SuperHandle = ClassHandle;
				Messaging.void_objc_msgSendSuper (ref super, Selector.GetHandle ("release"));
			}
		}

		[TestCase (0, 600, 256)]
		[TestCase (20, 10, 1)]
		public void DrainYields (int releaseDelayMilliseconds, int objectCount, int maxBatchCount)
		{
			var released = 0;
			var releasedAtHeartbeat = 0;
			var heartbeat = false;
			var releasedOnMainThread = true;
			var objects = new List<DrainObject> ();

			for (var i = 0; i < objectCount; i++) {
				objects.Add (new DrainObject (() => {
					releasedOnMainThread &= NSThread.IsMain;
					released++;
					if (released == 1) {
						NSRunLoop.Main.BeginInvokeOnMainThread (() => {
							releasedAtHeartbeat = released;
							heartbeat = true;
						});
					}
					if (releaseDelayMilliseconds > 0)
						Thread.Sleep (releaseDelayMilliseconds);
				}));
			}
			foreach (var obj in objects)
				obj.QueueForFinalization ();

			Assert.That (TestRuntime.RunAsync (TimeSpan.FromSeconds (30), () => { }, () => heartbeat && released == objectCount), Is.True, "Drain timed out");
			Assert.That (releasedAtHeartbeat, Is.InRange (1, maxBatchCount), "Objects released before the run loop could process other work");
			Assert.That (released, Is.EqualTo (objectCount), "All objects released");
			Assert.That (releasedOnMainThread, Is.True, "Native releases must stay on the main thread");
			foreach (var obj in objects)
				Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Native handle cleared");
		}

		[Test]
		public void DrainReentrancyAndConcurrentEnqueue ()
		{
			var releaseOrder = new List<int> ();
			var depth = 0;
			var maxDepth = 0;
			var objects = new List<DrainObject> ();
			var additionalObjects = new List<DrainObject> ();
			var enqueueCompleted = false;
			var releasedOnMainThread = true;

			for (var i = 0; i < 30; i++) {
				var id = i;
				var obj = new DrainObject (() => {
					releasedOnMainThread &= NSThread.IsMain;
					depth++;
					maxDepth = Math.Max (maxDepth, depth);
					releaseOrder.Add (id);
					if (id == 0) {
						var thread = new Thread (() => {
							foreach (var additional in additionalObjects)
								additional.QueueForFinalization ();
						}) {
							IsBackground = true,
						};
						thread.Start ();
						enqueueCompleted = thread.Join (TimeSpan.FromSeconds (5));
						Messaging.void_objc_msgSend_IntPtr (Class.GetHandle ("__NSObject_Disposer"), Selector.GetHandle ("drain:"), IntPtr.Zero);
					}
					depth--;
				});
				if (i < 20)
					objects.Add (obj);
				else
					additionalObjects.Add (obj);
			}
			foreach (var obj in objects)
				obj.QueueForFinalization ();

			Assert.That (TestRuntime.RunAsync (TimeSpan.FromSeconds (30), () => { }, () => releaseOrder.Count == 30), Is.True, "Drain timed out");
			Assert.That (enqueueCompleted, Is.True, "Concurrent enqueue blocked during native release");
			Assert.That (maxDepth, Is.EqualTo (1), "Drain must not reenter native release");
			Assert.That (releaseOrder, Is.EqualTo (System.Linq.Enumerable.Range (0, 30)), "Objects released exactly once in queue order");
			Assert.That (releasedOnMainThread, Is.True, "Native releases must stay on the main thread");
		}

		[Test]
		public void NSObject_GCResurrectTest ()
		{
			// Run GC and pump run loop to ensure that NSObject_Disposer has processed everything
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			PumpLoop ();

			// Create a single object and immediately lose a reference to it, save the handle
			// to immitate getting the same handle from a native API at later point in time
			IntPtr nativeHandle = CreateNativeObject ();

			// Ensure that the GC is run and the one new object gets collected
			GC.Collect ();
			GC.WaitForPendingFinalizers ();

			// The object should now be resurrected through `NSObject_Disposer.Add(this)`
			// and so `GCHandle` will return its reference:
			var o = ObjCRuntime.Runtime.GetNSObject<NSString> (nativeHandle);

			Assert.That ((IntPtr) o.Handle, Is.Not.EqualTo (IntPtr.Zero));

			// Pump the run loop and thus drain the NSObject_Disposer
			PumpLoop ();

			// The object is invalid now
			Assert.That ((IntPtr) o.Handle, Is.Not.EqualTo (IntPtr.Zero));
		}

		[System.Runtime.CompilerServices.MethodImpl (System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		private static IntPtr CreateNativeObject ()
		{
			var foo = new NSString ("foo");
			foo.DangerousRetain ();
			return foo.Handle;
		}

		private static void PumpLoop ()
		{
			using (new NSAutoreleasePool ()) {
				NSEvent? evnt;
				while ((evnt = NSApplication.SharedApplication.NextEvent ((NSEventMask) NSEventMask.AnyEvent, NSDate.DistantPast, NSRunLoopMode.Default, true)) is not null) {
					NSApplication.SharedApplication.SendEvent (evnt);
				}
			}
		}
	}
}
#endif // __MACOS__
