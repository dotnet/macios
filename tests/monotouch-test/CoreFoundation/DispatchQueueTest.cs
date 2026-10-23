//
// Unit tests for DispatchQueue
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2018 Microsoft Corp. All rights reserved.
//

using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DispatchQueueTests {
#if !XAMCORE_5_0
		[TestCase (0, false)]
		[TestCase (1, false)]
		[TestCase (8, false)]
		[TestCase (8, true)]
		public void Submit (int iterations, bool concurrent)
		{
			var queue = new DispatchQueue ("Submit", concurrent);
			var reference = new WeakReference (queue);
			var visits = new int [iterations];
			var collected = 0;
			queue.Submit ((int index) => {
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				if (!reference.IsAlive)
					Interlocked.Increment (ref collected);
				Interlocked.Increment (ref visits [index]);
			}, iterations);
			Assert.That (collected, Is.EqualTo (0), "Queue must remain alive during all iterations");
			for (var i = 0; i < visits.Length; i++)
				Assert.That (visits [i], Is.EqualTo (1), $"Iteration {i} must run exactly once before Submit returns");
		}
#endif // !XAMCORE_5_0

		[TestCase (0, false)]
		[TestCase (1, false)]
		[TestCase (8, false)]
		[TestCase (8, true)]
		public void SubmitNative (int iterations, bool concurrent)
		{
			var queue = new DispatchQueue ("SubmitNative", concurrent);
			var reference = new WeakReference (queue);
			var visits = new int [iterations];
			var collected = 0;
			queue.Submit ((nint index) => {
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				if (!reference.IsAlive)
					Interlocked.Increment (ref collected);
				Interlocked.Increment (ref visits [index]);
			}, (nint) iterations);
			Assert.That (collected, Is.EqualTo (0), "Queue must remain alive during all iterations");
			for (var i = 0; i < visits.Length; i++)
				Assert.That (visits [i], Is.EqualTo (1), $"Iteration {i} must run exactly once before Submit returns");
		}

		[Test]
		public void SubmitOverloadResolution ()
		{
			using var queue = new DispatchQueue ("SubmitOverloadResolution");
			object index = null;
			queue.Submit (iteration => index = iteration, 1);
			Assert.That (index, Is.TypeOf<nint> (), "An untyped callback must select the native-sized overload");
		}

		[Test]
		public void SubmitNullAction ()
		{
			using var queue = new DispatchQueue ("SubmitNullAction");
			Assert.Throws<ArgumentNullException> (() => queue.Submit ((Action<nint>) null, (nint) 0), "Native");
#if !XAMCORE_5_0
			Assert.Throws<ArgumentNullException> (() => queue.Submit ((Action<int>) null, 0L), "Compatibility");
#endif // !XAMCORE_5_0
		}

		[TestCase (false)]
#if !XAMCORE_5_0
		[TestCase (true)]
#endif // !XAMCORE_5_0
		public void SubmitZeroReleasesCallbackState (bool compatibility)
		{
			var reference = SubmitZeroWithCapturedState (compatibility);
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			Assert.That (reference.IsAlive, Is.False, "Zero iterations must release the callback state");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference SubmitZeroWithCapturedState (bool compatibility)
		{
			using var queue = new DispatchQueue ("SubmitZeroWithCapturedState");
			var state = new int [1];
			var reference = new WeakReference (state);
#if !XAMCORE_5_0
			if (compatibility)
				queue.Submit ((int index) => state [0] = index, 0L);
			else
#endif // !XAMCORE_5_0
				queue.Submit ((nint index) => state [0] = (int) index, (nint) 0);
			return reference;
		}

		[TestCase (false, false)]
		[TestCase (true, false)]
#if !XAMCORE_5_0
		[TestCase (false, true)]
		[TestCase (true, true)]
#endif // !XAMCORE_5_0
		public void SubmitExceptions (bool concurrent, bool compatibility)
		{
			using var queue = new DispatchQueue ("SubmitExceptions", concurrent);
			var visits = new int [8];
			var failures = new Exception [visits.Length];
			for (var i = 0; i < failures.Length; i++)
				failures [i] = new InvalidOperationException ($"Iteration {i}");
			Action<nint> callback = index => {
				Interlocked.Increment (ref visits [(int) index]);
				ThrowFromCallback (failures [(int) index]);
			};
			var exception = Assert.Throws<AggregateException> (() => {
#if !XAMCORE_5_0
				if (compatibility)
					queue.Submit ((int index) => callback (index), visits.Length);
				else
#endif // !XAMCORE_5_0
					queue.Submit (callback, (nint) visits.Length);
			});
			Assert.That (exception.InnerExceptions, Is.EquivalentTo (failures), "Every callback failure is preserved");
			foreach (var failure in exception.InnerExceptions)
				Assert.That (failure.StackTrace, Does.Contain (nameof (ThrowFromCallback)), "Original throw site");
			for (var i = 0; i < visits.Length; i++)
				Assert.That (visits [i], Is.EqualTo (1), $"Iteration {i} completed exactly once");
			Assert.That (queue.DispatchSync (() => 42), Is.EqualTo (42), "Queue remains usable");
		}

		[Test]
		public void SubmitNegativeCount ()
		{
			using var queue = new DispatchQueue ("SubmitNegativeCount");
			var exception = Assert.Throws<ArgumentOutOfRangeException> (() => queue.Submit ((nint index) => { }, (nint) (-1)));
			Assert.That (exception.ParamName, Is.EqualTo ("times"), "Native parameter");
#if !XAMCORE_5_0
			exception = Assert.Throws<ArgumentOutOfRangeException> (() => queue.Submit ((int index) => { }, -1L));
			Assert.That (exception.ParamName, Is.EqualTo ("times"), "Compatibility parameter");
#endif // !XAMCORE_5_0
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ExceptionReleasesCallbackState (bool submit)
		{
			var reference = DispatchThrowingCapturedState (submit);
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			Assert.That (reference.IsAlive, Is.False, "Throwing callbacks must release captured state");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference DispatchThrowingCapturedState (bool submit)
		{
			using var queue = new DispatchQueue ("DispatchThrowingCapturedState");
			var state = new int [1];
			var reference = new WeakReference (state);
			Action callback = () => {
				state [0]++;
				throw new InvalidOperationException ();
			};
			if (submit)
				Assert.Throws<AggregateException> (() => queue.Submit ((nint index) => callback (), (nint) 1));
			else
				Assert.Throws<InvalidOperationException> (() => queue.DispatchSync (callback));
			return reference;
		}

		[TestCase (false)]
		[TestCase (true)]
		public void RejectedSubmissionReleasesCallbackState (bool groupSubmission)
		{
			var reference = RejectCapturedState (groupSubmission);
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			Assert.That (reference.IsAlive, Is.False, "Rejected submissions must not retain their callbacks");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference RejectCapturedState (bool groupSubmission)
		{
			var queue = new DispatchQueue ("RejectCapturedState");
			queue.Dispose ();
			var state = new int [1];
			var reference = new WeakReference (state);
			Action callback = () => {
				state [0]++;
			};
			if (groupSubmission) {
				using var group = new DispatchGroup ();
				Assert.Throws<ObjectDisposedException> (() => group.DispatchAsync (queue, callback));
				Assert.Throws<ObjectDisposedException> (() => group.Notify (queue, callback));
			} else {
				Assert.Throws<ObjectDisposedException> (() => queue.DispatchAsync (callback));
				Assert.Throws<ObjectDisposedException> (() => queue.DispatchAfter (DispatchTime.Now, callback));
				Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierAsync (callback));
			}
			return reference;
		}

		[TestCase (false)]
		[TestCase (true)]
		public void DispatchSyncExceptions (bool concurrent)
		{
			using var queue = new DispatchQueue ("DispatchSyncExceptions", concurrent);
			var expected = new InvalidOperationException ("Callback failure");
			var originalContext = SynchronizationContext.Current;
			var exception = Assert.Throws<InvalidOperationException> (() => queue.DispatchSync (() => {
				ThrowFromCallback (expected);
			}));
			Assert.That (exception, Is.SameAs (expected), "Action exception");
			Assert.That (exception.StackTrace, Does.Contain (nameof (ThrowFromCallback)), "Action throw site");
			Assert.That (SynchronizationContext.Current, Is.SameAs (originalContext), "Action caller context");

			exception = Assert.Throws<InvalidOperationException> (() => queue.DispatchSync<int> (() => {
				ThrowFromCallback (expected);
				return 0;
			}));
			Assert.That (exception, Is.SameAs (expected), "Func exception");
			Assert.That (exception.StackTrace, Does.Contain (nameof (ThrowFromCallback)), "Func throw site");

			exception = Assert.Throws<InvalidOperationException> (() => queue.DispatchBarrierSync (() => {
				ThrowFromCallback (expected);
			}));
			Assert.That (exception, Is.SameAs (expected), "Barrier exception");
			Assert.That (exception.StackTrace, Does.Contain (nameof (ThrowFromCallback)), "Barrier throw site");
			Assert.That (SynchronizationContext.Current, Is.SameAs (originalContext), "Barrier caller context");
			Assert.That (queue.DispatchSync (() => 42), Is.EqualTo (42), "Queue remains usable");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static void ThrowFromCallback (Exception exception)
		{
			throw exception;
		}

		[TestCase (false)]
		[TestCase (true)]
		public void SynchronizationContextRestored (bool customContext)
		{
			using var queue = new DispatchQueue ("SynchronizationContextRestored");
			var originalContext = SynchronizationContext.Current;
			var context = customContext ? new SynchronizationContext () : null;
			SynchronizationContext.SetSynchronizationContext (context);
			try {
				SynchronizationContext callbackContext = null;
				queue.DispatchSync (() => {
					callbackContext = SynchronizationContext.Current;
				});
				if (customContext)
					Assert.That (callbackContext, Is.SameAs (context), "User context is preserved");
				else
					Assert.That (callbackContext, Is.Not.Null, "Queue context is installed");
				Assert.That (SynchronizationContext.Current, Is.SameAs (context), "Context after success");
				Assert.Throws<InvalidOperationException> (() => queue.DispatchSync (() => {
					throw new InvalidOperationException ();
				}));
				Assert.That (SynchronizationContext.Current, Is.SameAs (context), "Context after failure");
			} finally {
				SynchronizationContext.SetSynchronizationContext (originalContext);
			}
		}

		[Test]
		public void NullCallbacks ()
		{
			using var queue = new DispatchQueue ("NullCallbacks");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchAsync ((Action) null), "Async action");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchAsync ((DispatchBlock) null), "Async block");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchSync ((Action) null), "Sync action");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchSync ((DispatchBlock) null), "Sync block");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchBarrierAsync ((Action) null), "Async barrier action");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchBarrierAsync ((DispatchBlock) null), "Async barrier block");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchBarrierSync ((Action) null), "Sync barrier action");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchBarrierSync ((DispatchBlock) null), "Sync barrier block");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchAfter (DispatchTime.Now, (Action) null), "After action");
			Assert.Throws<ArgumentNullException> (() => queue.DispatchAfter (DispatchTime.Now, (DispatchBlock) null), "After block");
		}

		[Test]
		public void DisposedQueue ()
		{
			var queue = new DispatchQueue ("DisposedQueue");
			queue.Dispose ();
			using var block = new DispatchBlock (() => { });
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAsync (() => { }), "Async action");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAsync (block), "Async block");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchSync (() => { }), "Sync action");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchSync (block), "Sync block");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchSync (() => 42), "Sync function");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierAsync (() => { }), "Async barrier action");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierAsync (block), "Async barrier block");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierSync (() => { }), "Sync barrier action");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierSync (block), "Sync barrier block");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAfter (DispatchTime.Now, () => { }), "After action");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAfter (DispatchTime.Now, block), "After block");
			Assert.Throws<ObjectDisposedException> (() => queue.Submit ((nint index) => { }, (nint) 0), "Submit");
#if !XAMCORE_5_0
			Assert.Throws<ObjectDisposedException> (() => queue.Submit ((int index) => { }, 0L), "Compatibility submit");
#endif // !XAMCORE_5_0
			Assert.Throws<ObjectDisposedException> (() => queue.Activate (), "Activate");
			Assert.Throws<ObjectDisposedException> (() => queue.Suspend (), "Suspend");
			Assert.Throws<ObjectDisposedException> (() => queue.Resume (), "Resume");
			Assert.Throws<ObjectDisposedException> (() => queue.SetTargetQueue (null), "Target");
			Assert.Throws<ObjectDisposedException> (() => queue.SetSpecific ((IntPtr) 1, new object ()), "SetSpecific");
			Assert.Throws<ObjectDisposedException> (() => queue.GetSpecific ((IntPtr) 1), "GetSpecific");
			Assert.Throws<ObjectDisposedException> (() => queue.GetQualityOfService (out var priority), "GetQualityOfService");
			Assert.Throws<ObjectDisposedException> (() => { var value = queue.QualityOfService; }, "QualityOfService");
			Assert.Throws<ObjectDisposedException> (() => { var value = queue.Label; }, "Label");
			Assert.Throws<ObjectDisposedException> (() => { var value = queue.Context; }, "Get context");
			Assert.Throws<ObjectDisposedException> (() => queue.Context = IntPtr.Zero, "Set context");
			Assert.Throws<ObjectDisposedException> (() => new DispatchQueue ("DisposedTarget", new DispatchQueue.Attributes (), queue), "Constructor target");
		}

		[Test]
		public void DisposedBlock ()
		{
			using var queue = new DispatchQueue ("DisposedBlock");
			var block = new DispatchBlock (() => { });
			block.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAsync (block), "Async");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchSync (block), "Sync");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierAsync (block), "Async barrier");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchBarrierSync (block), "Sync barrier");
			Assert.Throws<ObjectDisposedException> (() => queue.DispatchAfter (DispatchTime.Now, block), "After");
		}

		[Test]
		public void Context ()
		{
			using var queue = new DispatchQueue ("Context");
			Assert.That (queue.Context, Is.EqualTo (IntPtr.Zero), "Initial context");
			queue.Context = (IntPtr) 42;
			Assert.That (queue.Context, Is.EqualTo ((IntPtr) 42), "Assigned context");
			queue.Context = IntPtr.Zero;
			Assert.That (queue.Context, Is.EqualTo (IntPtr.Zero), "Cleared context");
		}

		[Test]
		public void SuspendResume ()
		{
			using var queue = new DispatchQueue ("SuspendResume");
			var completed = new TaskCompletionSource<bool> ();
			queue.Suspend ();
			queue.Suspend ();
			try {
				queue.DispatchAsync (() => completed.SetResult (true));
				Assert.That (completed.Task.IsCompleted, Is.False, "Suspended twice");
			} finally {
				queue.Resume ();
				try {
					Assert.That (completed.Task.IsCompleted, Is.False, "Still suspended once");
				} finally {
					queue.Resume ();
				}
			}
			Assert.That (completed.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Resumed");
		}

		[TestCase (false, DispatchQueue.AutoreleaseFrequency.Inherit)]
		[TestCase (true, DispatchQueue.AutoreleaseFrequency.WorkItem)]
		[TestCase (true, DispatchQueue.AutoreleaseFrequency.Never)]
		public void Attributes (bool concurrent, DispatchQueue.AutoreleaseFrequency frequency)
		{
			var attributes = new DispatchQueue.Attributes ();
			Assert.That (attributes.Concurrent, Is.False, "Default concurrency");
			Assert.That (attributes.IsInitiallyInactive, Is.False, "Default activation");
			Assert.That (attributes.AutoreleaseFrequency, Is.Null, "Default autorelease frequency");
			Assert.That (attributes.QualityOfService, Is.Null, "Default quality of service");
			Assert.That (attributes.RelativePriority, Is.EqualTo (0), "Default priority");
			attributes.Concurrent = concurrent;
			attributes.IsInitiallyInactive = true;
			attributes.AutoreleaseFrequency = frequency;
			attributes.QualityOfService = DispatchQualityOfService.Utility;
			attributes.RelativePriority = -2;
			Assert.That (attributes.Concurrent, Is.EqualTo (concurrent), "Concurrency");
			Assert.That (attributes.IsInitiallyInactive, Is.True, "Activation");
			Assert.That (attributes.AutoreleaseFrequency, Is.EqualTo (frequency), "Autorelease frequency");
			Assert.That (attributes.QualityOfService, Is.EqualTo (DispatchQualityOfService.Utility), "Quality of service");
			Assert.That (attributes.RelativePriority, Is.EqualTo (-2), "Priority");
			using var queue = new DispatchQueue ("Attributes", attributes);
			queue.Activate ();
			queue.Activate ();
			attributes.QualityOfService = DispatchQualityOfService.Background;
			attributes.RelativePriority = -3;
			Assert.That (queue.GetQualityOfService (out var priority), Is.EqualTo (DispatchQualityOfService.Utility), "Queue quality of service");
			Assert.That (priority, Is.EqualTo (-2), "Queue priority");
			Assert.That (queue.DispatchSync (() => 42), Is.EqualTo (42), "Activated queue executes work");
		}

		[Test]
		public void UnspecifiedQualityOfService ()
		{
			using var queue = new DispatchQueue ("UnspecifiedQualityOfService");
			Assert.That (queue.QualityOfService, Is.EqualTo (DispatchQualityOfService.Unspecified), "No explicit quality of service");
			Assert.That (queue.GetQualityOfService (out var priority), Is.EqualTo (DispatchQualityOfService.Unspecified), "No explicit class");
			Assert.That (priority, Is.EqualTo (0), "No relative priority");
		}

		[Test]
		public void CtorWithAttributes ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			using (var queue = new DispatchQueue ("1", new DispatchQueue.Attributes {
				AutoreleaseFrequency = DispatchQueue.AutoreleaseFrequency.Inherit,
			})) {
				Assert.That (queue.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle 1");
			}

			using (var queue = new DispatchQueue ("2", new DispatchQueue.Attributes {
				IsInitiallyInactive = true,
			})) {
				queue.Activate (); // must activate the queue before it can be released according to Apple's documentation
				Assert.That (queue.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle 2");
			}

			using (var queue = new DispatchQueue ("3", new DispatchQueue.Attributes {
				QualityOfService = DispatchQualityOfService.Utility,
			})) {
				Assert.That (queue.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle 3");
				Assert.That (queue.QualityOfService, Is.EqualTo (DispatchQualityOfService.Utility), "QualityOfService 3");
			}

			using (var target_queue = new DispatchQueue ("4 - target")) {
				using (var queue = new DispatchQueue ("4", new DispatchQueue.Attributes {
					QualityOfService = DispatchQualityOfService.Background,
					AutoreleaseFrequency = DispatchQueue.AutoreleaseFrequency.WorkItem,
					RelativePriority = -1,
				}, target_queue)) {
					Assert.That (queue.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle 4");
					Assert.That (queue.GetQualityOfService (out var relative_priority), Is.EqualTo (DispatchQualityOfService.Background), "QualityOfService 4");
					Assert.That (relative_priority, Is.EqualTo (-1), "RelativePriority 4");
				}
			}
		}

		[Test]
		public void Specific ()
		{
			using (var queue = new DispatchQueue ("Specific")) {
				var key = (IntPtr) 0x31415926;
				Assert.That (queue.GetSpecific (key), Is.Null, "Unset key");
				Assert.That (queue.GetSpecific (IntPtr.Zero), Is.Null, "Zero key");
				queue.SetSpecific (key, "hello world");
				Assert.That (queue.GetSpecific (key), Is.EqualTo ("hello world"), "Key");
				var replacement = new object ();
				queue.SetSpecific (key, replacement);
				Assert.That (queue.GetSpecific (key), Is.SameAs (replacement), "Replaced key");
				Assert.Throws<ArgumentException> (() => queue.SetSpecific (IntPtr.Zero, replacement), "Zero key is rejected");
			}
		}

		[Test]
		public void SpecificDoesNotSearchTarget ()
		{
			var key = (IntPtr) 0x31415926;
			using var target = new DispatchQueue ("SpecificTarget");
			target.SetSpecific (key, "target");
			using var queue = new DispatchQueue ("SpecificChild", new DispatchQueue.Attributes (), target);
			Assert.That (queue.GetSpecific (key), Is.Null, "Only searches the specified queue");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void DispatchAfter (bool nativeBlock)
		{
			using var queue = new DispatchQueue ("DispatchAfter");
			var completed = new TaskCompletionSource<bool> ();
			var clock = System.Diagnostics.Stopwatch.StartNew ();
			var deadline = new DispatchTime (DispatchTime.Now, TimeSpan.FromMilliseconds (50));
			var elapsed = TimeSpan.Zero;
			var label = "";
			Action callback = () => {
				elapsed = clock.Elapsed;
				label = DispatchQueue.CurrentQueueLabel;
				completed.SetResult (true);
			};
			if (nativeBlock) {
				using var block = new DispatchBlock (callback);
				queue.DispatchAfter (deadline, block);
			} else {
				queue.DispatchAfter (deadline, callback);
			}
			Assert.That (completed.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Callback completed");
			Assert.That (elapsed, Is.GreaterThanOrEqualTo (TimeSpan.FromMilliseconds (50)), "Not executed before the deadline");
			Assert.That (label, Is.EqualTo (queue.Label), "Executes on the requested queue");
		}

		[TestCase (false, false)]
		[TestCase (false, true)]
		[TestCase (true, false)]
		[TestCase (true, true)]
		public void BarrierOrdering (bool asynchronous, bool nativeBlock)
		{
			using var queue = new DispatchQueue ("BarrierOrdering", true);
			var before = 0;
			var atBarrier = -1;
			var after = -1;
			for (var i = 0; i < 8; i++)
				queue.DispatchAsync (() => {
					Interlocked.Increment (ref before);
				});
			Action barrier = () => {
				atBarrier = before;
			};
			if (nativeBlock) {
				using var block = new DispatchBlock (barrier);
				if (asynchronous)
					queue.DispatchBarrierAsync (block);
				else
					queue.DispatchBarrierSync (block);
			} else if (asynchronous) {
				queue.DispatchBarrierAsync (barrier);
			} else {
				queue.DispatchBarrierSync (barrier);
			}
			queue.DispatchAsync (() => {
				after = atBarrier;
			});
			queue.DispatchBarrierSync (() => { });
			Assert.That (atBarrier, Is.EqualTo (8), "Earlier work finished before the barrier");
			Assert.That (after, Is.EqualTo (8), "Later work starts after the barrier");
		}

		[TestCase (DispatchQueuePriority.High)]
		[TestCase (DispatchQueuePriority.Default)]
		[TestCase (DispatchQueuePriority.Low)]
		[TestCase (DispatchQueuePriority.Background)]
		public void GlobalPriority (DispatchQueuePriority priority)
		{
			using var queue = DispatchQueue.GetGlobalQueue (priority);
			Assert.That (queue.Label, Does.StartWith ("com.apple.root."), "Global label");
			Assert.That (queue.DispatchSync (() => 42), Is.EqualTo (42), "Queue executes work");
		}

		[TestCase (DispatchQualityOfService.UserInteractive)]
		[TestCase (DispatchQualityOfService.UserInitiated)]
		[TestCase (DispatchQualityOfService.Default)]
		[TestCase (DispatchQualityOfService.Utility)]
		[TestCase (DispatchQualityOfService.Background)]
		public void GlobalQualityOfService (DispatchQualityOfService service)
		{
			using var queue = DispatchQueue.GetGlobalQueue (service);
			Assert.That (queue.QualityOfService, Is.EqualTo (service), "Assigned quality of service");
			Assert.That (queue.GetQualityOfService (out var priority), Is.EqualTo (service), "Quality of service method");
			Assert.That (priority, Is.EqualTo (0), "Relative priority");
			Assert.That (queue.DispatchSync (() => 42), Is.EqualTo (42), "Queue executes work");
		}

#if MONOMAC
		[Test]
		public void MainIteration ()
		{
			using var process = new System.Diagnostics.Process ();
			process.StartInfo = new System.Diagnostics.ProcessStartInfo {
				FileName = NSBundle.MainBundle.ExecutablePath,
				Arguments = "--test-dispatch-main",
				UseShellExecute = false,
			};
			Assert.That (process.Start (), Is.True, "Start child");
			var exited = process.WaitForExit (30000);
			if (!exited) {
				process.Kill ();
				process.WaitForExit ();
			}
			Assert.That (exited, Is.True, "Main dispatch callback exited the child before the timeout");
			Assert.That (process.ExitCode, Is.EqualTo (0), "MainIteration serviced the main queue");
		}
#endif

		[Test]
		public void DispatchSync ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var queue = new DispatchQueue ("DispatchSync")) {
				var called = false;
				var callback = new Action (() => called = true);
				queue.DispatchSync (callback);
				Assert.That (called, Is.True, "Called");

				called = false;
				using (var dg = new DispatchBlock (callback))
					queue.DispatchSync (dg);
				Assert.That (called, Is.True, "Called DispatchBlock");
			}
		}

		[Test]
		public void DispatchSyncNullFunc ()
		{
			using var queue = new DispatchQueue ("DispatchSyncNullFunc");
			var exception = Assert.Throws<ArgumentNullException> (() => queue.DispatchSync ((Func<int>) null));
			Assert.That (exception.ParamName, Is.EqualTo ("func"), "Parameter name");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void DispatchSyncResult (bool concurrent)
		{
			using var queue = new DispatchQueue ("DispatchSyncResult", concurrent);
			var called = 0;
			var result = queue.DispatchSync (() => {
				called++;
				return 42;
			});
			Assert.That (called, Is.EqualTo (1), "Function must run exactly once before DispatchSync returns");
			Assert.That (result, Is.EqualTo (42), "Value type");
			Assert.That (queue.DispatchSync (() => 0), Is.EqualTo (0), "Default value");

			var expected = new object ();
			Assert.That (queue.DispatchSync (() => expected), Is.SameAs (expected), "Reference type");
			Assert.That (queue.DispatchSync<string> (() => null), Is.Null, "Null reference");
			Assert.That (queue.DispatchSync<int?> (() => 42), Is.EqualTo (42), "Nullable value");
			Assert.That (queue.DispatchSync<int?> (() => null), Is.Null, "Null nullable value");
		}

		[Test]
		public void DispatchSyncResultQueueOrder ()
		{
			using var queue = new DispatchQueue ("DispatchSyncResultQueueOrder");
			var value = 0;
			queue.DispatchAsync (() => {
				value = 42;
			});
			Assert.That (queue.DispatchSync (() => value), Is.EqualTo (42), "Previously submitted work must finish first");
		}

		[Test]
		public void DispatchBarrierSync ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var queue = new DispatchQueue ("DispatchBarrierSync")) {
				var called = false;
				var callback = new Action (() => called = true);
				queue.DispatchBarrierSync (callback);
				Assert.That (called, Is.True, "Called");

				called = false;
				using (var dg = new DispatchBlock (callback))
					queue.DispatchBarrierSync (dg);
				Assert.That (called, Is.True, "Called DispatchBlock");
			}
		}

		[Test]
		public void DispatchAsync ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var queue = new DispatchQueue ("DispatchAsync")) {
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					queue.DispatchAsync (callback);
					TestRuntime.RunAsync (TimeSpan.FromSeconds (5), called.Task);
					Assert.That (called.Task.Result, Is.True, "Called");
				}
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					using (var dg = new DispatchBlock (callback)) {
						queue.DispatchAsync (dg);
						dg.Wait (TimeSpan.FromSeconds (5));
					}
					Assert.That (called.Task.Result, Is.True, "Called DispatchBlock");
				}
			}
		}

		[Test]
		public void DispatchBarrierAsync ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var queue = new DispatchQueue ("DispatchBarrierAsync")) {
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					queue.DispatchBarrierAsync (callback);
					TestRuntime.RunAsync (TimeSpan.FromSeconds (5), called.Task);
					Assert.That (called.Task.Result, Is.True, "Called");
				}
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					using (var dg = new DispatchBlock (callback)) {
						queue.DispatchBarrierAsync (dg);
						dg.Wait (TimeSpan.FromSeconds (5));
					}
					Assert.That (called.Task.Result, Is.True, "Called DispatchBlock");
				}
			}
		}

		[Test]
		public void MainQueue ()
		{
			Assert.That (DispatchQueue.MainQueue, Is.EqualTo (DispatchQueue.CurrentQueue), "MainQueue");
		}

#if __MACOS__
		[Test]
		public void GlobalQueueAfterAsyncWindows ()
		{
			// Exceed the 64-worker dispatch limit observed when window animations stall on Tahoe.
			for (var i = 0; i < 128; i++)
				Assert.That (TestRuntime.RunAsync (TimeSpan.FromSeconds (5), Task.CompletedTask), Is.True, $"Window {i}");

			var completed = new TaskCompletionSource<bool> ();
			DispatchQueue.DefaultGlobalQueue.DispatchAsync (() => completed.SetResult (true));
			Assert.That (completed.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Global queue remained responsive after closing async-test windows");
		}
#endif
	}
}
