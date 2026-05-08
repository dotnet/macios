//
// Unit tests for DispatchGroup
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using Xamarin.Utils;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DispatchGroupTest {
		[Test]
		public void WaitTest ()
		{
			using (var dg = DispatchGroup.Create ()) {
				var dq = DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Default);

				dg.DispatchAsync (dq, delegate
				{
					Console.WriteLine ("Inside dispatch");
				});

				Assert.That (dg.Wait (DispatchTime.Forever), Is.True);
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
