//
// Unit tests for DispatchGroup
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.Threading.Tasks;

using Xamarin.Utils;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DispatchGroupTest {
		[Test]
		public void EmptyAndReusedGroup ()
		{
			using var group = new DispatchGroup ();
			Assert.That (group.Wait (DispatchTime.Now), Is.True, "Empty group");
			Assert.That (group.Wait (TimeSpan.Zero), Is.True, "Empty group relative wait");
			for (var i = 0; i < 2; i++) {
				group.Enter ();
				Assert.That (group.Wait (DispatchTime.Now), Is.False, $"Entered group {i}");
				Assert.That (group.Wait (TimeSpan.Zero), Is.False, $"Relative timeout {i}");
				group.Leave ();
				Assert.That (group.Wait (TimeSpan.FromSeconds (5)), Is.True, $"Completed group {i}");
			}
		}

		[TestCase (false)]
		[TestCase (true)]
		public void NotifyWaitsForCompletion (bool nativeBlock)
		{
			using var group = new DispatchGroup ();
			using var queue = new DispatchQueue ("NotifyWaitsForCompletion");
			var notified = new TaskCompletionSource<bool> ();
			var label = "";
			Action notification = () => {
				label = DispatchQueue.CurrentQueueLabel;
				notified.SetResult (true);
			};
			group.Enter ();
			try {
				if (nativeBlock) {
					using var block = new DispatchBlock (notification);
					group.Notify (queue, block);
				} else {
					group.Notify (queue, notification);
				}
				queue.DispatchSync (() => { });
				Assert.That (notified.Task.IsCompleted, Is.False, "Notification waits for outstanding work");
			} finally {
				group.Leave ();
			}
			Assert.That (notified.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Notification completed");
			Assert.That (label, Is.EqualTo (queue.Label), "Requested notification queue");
		}

		[Test]
		public void ArgumentValidation ()
		{
			using var group = new DispatchGroup ();
			using var queue = new DispatchQueue ("GroupArgumentValidation");
			using var block = new DispatchBlock (() => { });
			Assert.Throws<ArgumentNullException> (() => group.DispatchAsync (null, () => { }), "Async null queue");
			Assert.Throws<ArgumentNullException> (() => group.DispatchAsync (queue, null), "Async null action");
			Assert.Throws<ArgumentNullException> (() => group.Notify (null, (Action) (() => { })), "Notify action null queue");
			Assert.Throws<ArgumentNullException> (() => group.Notify (null, block), "Notify block null queue");
			Assert.Throws<ArgumentNullException> (() => group.Notify (queue, (Action) null), "Notify null action");
			Assert.Throws<ArgumentNullException> (() => group.Notify (queue, (DispatchBlock) null), "Notify null block");

			queue.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => group.DispatchAsync (queue, () => { }), "Async disposed queue");
			Assert.Throws<ObjectDisposedException> (() => group.Notify (queue, (Action) (() => { })), "Notify action disposed queue");
			Assert.Throws<ObjectDisposedException> (() => group.Notify (queue, block), "Notify block disposed queue");

			using var validQueue = new DispatchQueue ("ValidGroupQueue");
			block.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => group.Notify (validQueue, block), "Notify disposed block");
		}

		[Test]
		public void DisposedGroup ()
		{
			var group = new DispatchGroup ();
			group.Dispose ();
			using var queue = new DispatchQueue ("DisposedGroup");
			using var block = new DispatchBlock (() => { });
			Assert.Throws<ObjectDisposedException> (() => group.DispatchAsync (queue, () => { }), "Async");
			Assert.Throws<ObjectDisposedException> (() => group.Notify (queue, (Action) (() => { })), "Notify action");
			Assert.Throws<ObjectDisposedException> (() => group.Notify (queue, block), "Notify block");
			Assert.Throws<ObjectDisposedException> (() => group.Enter (), "Enter");
			Assert.Throws<ObjectDisposedException> (() => group.Leave (), "Leave");
			Assert.Throws<ObjectDisposedException> (() => group.Wait (DispatchTime.Now), "Wait deadline");
			Assert.Throws<ObjectDisposedException> (() => group.Wait (TimeSpan.Zero), "Wait interval");
		}

		[Test]
		public void WaitIntervalOverflow ()
		{
			using var group = new DispatchGroup ();
			Assert.Throws<OverflowException> (() => group.Wait (TimeSpan.MaxValue), "Positive overflow");
			Assert.Throws<OverflowException> (() => group.Wait (TimeSpan.MinValue), "Negative overflow");
		}

		[Test]
		public void WaitTest ()
		{
			using (var dg = DispatchGroup.Create ()) {
				var dq = DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Default);
				var called = new TaskCompletionSource<bool> ();

				dg.DispatchAsync (dq, delegate
				{
					called.SetResult (true);
				});

				Assert.That (dg.Wait (DispatchTime.Forever), Is.True);
				Assert.That (called.Task.Result, Is.True, "Called");
				dq.Dispose ();
			}
		}

		[Test]
		public void EnterLeaveTest ()
		{
			using (var dg = DispatchGroup.Create ()) {
				dg.Enter ();
				Assert.That (dg.Wait (new DispatchTime (1000 * 1000 * 1000)), Is.False, "#1");
				dg.Leave ();
				Assert.That (dg.Wait (DispatchTime.Forever), Is.True, "#2");
			}
		}

		[Test]
		public void NotifyWithDispatchBlock ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var dg = new DispatchGroup ()) {
				var called = false;
				var callback = new Action (() => called = true);
				using (var block = new DispatchBlock (callback)) {
					dg.Notify (DispatchQueue.MainQueue, block);
					TestRuntime.RunAsync (TimeSpan.FromSeconds (5), () => { }, () => called);
					Assert.That (called, Is.True, "Called");
				}
			}
		}

		[Test]
		public void NotifyWithAction ()
		{
			using (var dg = new DispatchGroup ()) {
				var called = false;
				var callback = new Action (() => called = true);
				dg.Notify (DispatchQueue.MainQueue, callback);
				TestRuntime.RunAsync (TimeSpan.FromSeconds (5), () => { }, () => called);
				Assert.That (called, Is.True, "Called");
			}
		}
	}
}
