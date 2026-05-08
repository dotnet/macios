//
// Unit tests for DispatchQueue
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2018 Microsoft Corp. All rights reserved.
//

using System.IO;
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
		[Test]
		public void CtorWithAttributes ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			using (var queue = new DispatchQueue ("1", new DispatchQueue.Attributes {
				AutoreleaseFrequency = DispatchQueue.AutoreleaseFrequency.Inherit,
			})) {
				ClassicAssert.AreNotEqual (IntPtr.Zero, queue.Handle, "Handle 1");
			}

			using (var queue = new DispatchQueue ("2", new DispatchQueue.Attributes {
				IsInitiallyInactive = true,
			})) {
				queue.Activate (); // must activate the queue before it can be released according to Apple's documentation
				ClassicAssert.AreNotEqual (IntPtr.Zero, queue.Handle, "Handle 2");
			}

			using (var queue = new DispatchQueue ("3", new DispatchQueue.Attributes {
				QualityOfService = DispatchQualityOfService.Utility,
			})) {
				ClassicAssert.AreNotEqual (IntPtr.Zero, queue.Handle, "Handle 3");
				ClassicAssert.AreEqual (DispatchQualityOfService.Utility, queue.QualityOfService, "QualityOfService 3");
			}

			using (var target_queue = new DispatchQueue ("4 - target")) {
				using (var queue = new DispatchQueue ("4", new DispatchQueue.Attributes {
					QualityOfService = DispatchQualityOfService.Background,
					AutoreleaseFrequency = DispatchQueue.AutoreleaseFrequency.WorkItem,
					RelativePriority = -1,
				}, target_queue)) {
					ClassicAssert.AreNotEqual (IntPtr.Zero, queue.Handle, "Handle 4");
					ClassicAssert.AreEqual (DispatchQualityOfService.Background, queue.GetQualityOfService (out var relative_priority), "QualityOfService 4");
					ClassicAssert.AreEqual (-1, relative_priority, "RelativePriority 4");
				}
			}
		}

		[Test]
		public void Specific ()
		{
			using (var queue = new DispatchQueue ("Specific")) {
				var key = (IntPtr) 0x31415926;
				queue.SetSpecific (key, "hello world");
				ClassicAssert.AreEqual ("hello world", queue.GetSpecific (key), "Key");
			}
		}

		[Test]
		public void DispatchSync ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var queue = new DispatchQueue ("DispatchSync")) {
				var called = false;
				var callback = new Action (() => called = true);
				queue.DispatchSync (callback);
				ClassicAssert.IsTrue (called, "Called");

				called = false;
				using (var dg = new DispatchBlock (callback))
					queue.DispatchSync (dg);
				ClassicAssert.IsTrue (called, "Called DispatchBlock");
			}
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
				ClassicAssert.IsTrue (called, "Called");

				called = false;
				using (var dg = new DispatchBlock (callback))
					queue.DispatchBarrierSync (dg);
				ClassicAssert.IsTrue (called, "Called DispatchBlock");
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
					ClassicAssert.IsTrue (called.Task.Result, "Called");
				}
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					using (var dg = new DispatchBlock (callback)) {
						queue.DispatchAsync (dg);
						dg.Wait (TimeSpan.FromSeconds (5));
					}
					ClassicAssert.IsTrue (called.Task.Result, "Called DispatchBlock");
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
					ClassicAssert.IsTrue (called.Task.Result, "Called");
				}
				{
					var called = new TaskCompletionSource<bool> ();
					var callback = new Action (() => called.SetResult (true));
					using (var dg = new DispatchBlock (callback)) {
						queue.DispatchBarrierAsync (dg);
						dg.Wait (TimeSpan.FromSeconds (5));
					}
					ClassicAssert.IsTrue (called.Task.Result, "Called DispatchBlock");
				}
			}
		}

		[Test]
		public void MainQueue ()
		{
			ClassicAssert.AreEqual (DispatchQueue.CurrentQueue, DispatchQueue.MainQueue, "MainQueue");
		}
	}
}
