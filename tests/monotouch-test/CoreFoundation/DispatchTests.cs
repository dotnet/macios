//
// Unit tests for Dispatch
//
// Authors:
//	Miguel de Icaza  <miguel@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.IO;
using CoreGraphics;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DispatchTests {
#if !MONOMAC // UIKitThreadAccessException and NSStringDrawing.WeakDrawString don't exist on mac
		[Test]
		public void MainQueueDispatch ()
		{
#if !DEBUG || OPTIMIZEALL
			Assert.Ignore ("UIKitThreadAccessException is not throw, by default, on release builds (removed by the linker)");
#endif
			bool hit = false;
			// We need to check the UIKitThreadAccessException, but there are very few API
			// with that check on WatchOS. NSStringDrawing.WeakDrawString is one example, here we pass
			// it null for the parameter so that it immediately returns with a NullReferenceException
			// instead of trying to load an image (which is not what we're testing). There
			// is also a test to ensure UIKitThreadAccessException is thrown if not on
			// the UI thread (so that we'll notice if the UIKitThreadAccessException is ever
			// removed from NSStringDrawing.WeakDrawString).
			var uiThread = Thread.CurrentThread;
			Thread queueThread = null;
			Thread mainQthread = null;
			Exception ex = null;
			Exception queue_ex = null;

			var defaultQ = DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Default);
			defaultQ.DispatchAsync (delegate
			{
				try {
					NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
				} catch (Exception e) {
					queue_ex = e;
				}

				queueThread = Thread.CurrentThread;
				var mainQ = DispatchQueue.MainQueue;
				mainQ.DispatchAsync (delegate
				{
					mainQthread = Thread.CurrentThread;
					try {
						NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
					} catch (Exception e) {
						ex = e;
					} finally {
						hit = true;
					}
				});

			});

			// Now wait for the above to actually run
			while (hit == false) {
				NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.5));
			}
			Assert.That (ex, Is.Not.Null, "main ex");
			Assert.That (ex.GetType (), Is.SameAs (typeof (NullReferenceException)), "no thread check hit");
			Assert.That (queue_ex, Is.Not.Null, "queue ex");
			Assert.That (queue_ex.GetType (), Is.SameAs (typeof (UIKitThreadAccessException)), "thread check hit");
			Assert.That (uiThread, Is.EqualTo (mainQthread), "mainq thread is equal to uithread");
			Assert.That (queueThread, Is.Not.EqualTo (mainQthread), "queueThread is not the same as the UI thread");
		}

		[Test]
		public void MainQueueDispatchQualityOfService ()
		{
#if !DEBUG || OPTIMIZEALL
			Assert.Ignore ("UIKitThreadAccessException is not throw, by default, on release builds (removed by the linker)");
#endif
			bool hit = false;
			// We need to check the UIKitThreadAccessException, but there are very few API
			// with that check on WatchOS. NSStringDrawing.WeakDrawString is one example, here we pass
			// it null for the parameter so that it immediately returns with a NullReferenceException
			// instead of trying to load an image (which is not what we're testing). There
			// is also a test to ensure UIKitThreadAccessException is thrown if not on
			// the UI thread (so that we'll notice if the UIKitThreadAccessException is ever
			// removed from NSStringDrawing.WeakDrawString).
			var uiThread = Thread.CurrentThread;
			Thread queueThread = null;
			Thread mainQthread = null;
			Exception ex = null;
			Exception queue_ex = null;

			var defaultQ = DispatchQueue.GetGlobalQueue (DispatchQualityOfService.Default);
			defaultQ.DispatchAsync (delegate
			{
				try {
					NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
				} catch (Exception e) {
					queue_ex = e;
				}

				queueThread = Thread.CurrentThread;
				var mainQ = DispatchQueue.MainQueue;
				mainQ.DispatchAsync (delegate
				{
					mainQthread = Thread.CurrentThread;
					try {
						NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
					} catch (Exception e) {
						ex = e;
					} finally {
						hit = true;
					}
				});

			});

			// Now wait for the above to actually run
			while (hit == false) {
				NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.5));
			}
			Assert.That (ex, Is.Not.Null, "main ex");
			Assert.That (ex.GetType (), Is.SameAs (typeof (NullReferenceException)), "no thread check hit");
			Assert.That (queue_ex, Is.Not.Null, "queue ex");
			Assert.That (queue_ex.GetType (), Is.SameAs (typeof (UIKitThreadAccessException)), "thread check hit");
			Assert.That (uiThread, Is.EqualTo (mainQthread), "mainq thread is equal to uithread");
			Assert.That (queueThread, Is.Not.EqualTo (mainQthread), "queueThread is not the same as the UI thread");
		}
#endif

		[Test]
		public void Current ()
		{
			Assert.That (DispatchQueue.CurrentQueue.Label, Is.EqualTo ("com.apple.main-thread"), "Current");
		}

		[Test]
		public void Default ()
		{
			var qname = "com.apple.root.default-priority";
#if __IOS__
			if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 8, 0))
				qname = "com.apple.root.default-qos";
#elif __TVOS__
			qname = "com.apple.root.default-qos";
#elif __MACOS__
			qname = "com.apple.root.default-qos";
#endif
			Assert.That (DispatchQueue.DefaultGlobalQueue.Label, Is.EqualTo (qname), "Default");
		}

		[Test]
		public void SetTargetQueue ()
		{
			using var target = new DispatchQueue ("Target");
			var attributes = new DispatchQueue.Attributes { IsInitiallyInactive = true };
			using var queue = new DispatchQueue ("Child", attributes);
			try {
				queue.SetTargetQueue (target);
			} finally {
				queue.Activate ();
			}
			Assert.That (queue.DispatchSync (() => DispatchQueue.CurrentQueueLabel), Is.EqualTo ("Child"), "Child label");

			target.Suspend ();
			var completed = new TaskCompletionSource<bool> ();
			try {
				queue.DispatchAsync (() => completed.SetResult (true));
				Assert.That (completed.Task.IsCompleted, Is.False, "Child work cannot run on a suspended target");
			} finally {
				target.Resume ();
			}
			Assert.That (completed.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Target resumed");
		}

		[Test]
		public void ResetTargetQueue ()
		{
			using var target = new DispatchQueue ("InitialTarget");
			using var queue = new DispatchQueue ("ResetTarget", new DispatchQueue.Attributes { IsInitiallyInactive = true }, target);
			try {
				queue.SetTargetQueue (null);
			} finally {
				queue.Activate ();
			}
			target.Suspend ();
			try {
				var completed = new TaskCompletionSource<bool> ();
				queue.DispatchAsync (() => completed.SetResult (true));
				Assert.That (completed.Task.Wait (TimeSpan.FromSeconds (5)), Is.True, "Reset removes the previous target");
			} finally {
				target.Resume ();
			}
		}

		[Test]
		public void DisposedTargetQueue ()
		{
			var target = new DispatchQueue ("DisposedTarget");
			target.Dispose ();
			using var queue = new DispatchQueue ("DisposedTargetChild", new DispatchQueue.Attributes { IsInitiallyInactive = true });
			try {
				Assert.Throws<ObjectDisposedException> (() => queue.SetTargetQueue (target), "Disposed target");
			} finally {
				queue.Activate ();
			}
		}

		[Test]
		public void Main ()
		{
			Assert.That (DispatchQueue.MainQueue.Label, Is.EqualTo ("com.apple.main-thread"), "Main");
		}

		[Test]
		public void GetGlobalQueue_Priority ()
		{
			// values changes in OS versions (and even in arch) but we only want to make sure we get a valid string so the prefix is enough
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Default).Label.StartsWith ("com.apple.root."), Is.True, "Default");
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Low).Label.StartsWith ("com.apple.root."), Is.True, "Low");
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQueuePriority.High).Label.StartsWith ("com.apple.root."), Is.True, "High");
		}

		[Test]
		public void GetGlobalQueue_QualityOfService ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			// values changes in OS versions (and even in arch) but we only want to make sure we get a valid string so the prefix is enough
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQualityOfService.Default).Label.StartsWith ("com.apple.root."), Is.True, "Default");
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQualityOfService.Utility).Label.StartsWith ("com.apple.root."), Is.True, "Low");
			Assert.That (DispatchQueue.GetGlobalQueue (DispatchQualityOfService.UserInitiated).Label.StartsWith ("com.apple.root."), Is.True, "High");
		}

		[Test]
		public void NeverTooLate ()
		{
			Assert.That (DispatchTime.Now.Nanoseconds, Is.EqualTo (0), "Now");
			Assert.That (DispatchTime.Forever.Nanoseconds, Is.EqualTo (unchecked((ulong) ~0)), "Forever");

			var dt = new DispatchTime (1);
			Assert.That (dt.Nanoseconds, Is.EqualTo (1), "1");

			dt = new DispatchTime (DispatchTime.Now, 0);
			Assert.That (dt.Nanoseconds, Is.Not.EqualTo (0), "!0");

			var dt2 = new DispatchTime (dt, Int32.MaxValue);
			Assert.That (dt2.Nanoseconds, Is.GreaterThan (dt.Nanoseconds), "later");
		}

		[TestCase (0L)]
		[TestCase (1L)]
		[TestCase (-1L)]
		[TestCase (10000000L)]
		[TestCase (-10000000L)]
		public void DispatchTimeInterval (long ticks)
		{
			var reference = new DispatchTime (DispatchTime.Now, TimeSpan.FromSeconds (5));
			var actual = new DispatchTime (reference, TimeSpan.FromTicks (ticks));
			var expected = new DispatchTime (reference, ticks * 100);
			Assert.That (actual.Nanoseconds, Is.EqualTo (expected.Nanoseconds), "TimeSpan ticks converted to nanoseconds");
		}

		[Test]
		public void DispatchTimeIntervalOverflow ()
		{
			Assert.Throws<OverflowException> (() => new DispatchTime (DispatchTime.Now, TimeSpan.MaxValue), "Positive overflow");
			Assert.Throws<OverflowException> (() => new DispatchTime (DispatchTime.Now, TimeSpan.MinValue), "Negative overflow");
			Assert.Throws<OverflowException> (() => new DispatchTime (DispatchTime.Now, TimeSpan.FromTicks (long.MaxValue / 100 + 1)), "Positive boundary");
			Assert.Throws<OverflowException> (() => new DispatchTime (DispatchTime.Now, TimeSpan.FromTicks (long.MinValue / 100 - 1)), "Negative boundary");
			Assert.DoesNotThrow (() => new DispatchTime (DispatchTime.Now, TimeSpan.FromTicks (long.MaxValue / 100)), "Largest representable delta");
			Assert.DoesNotThrow (() => new DispatchTime (DispatchTime.Now, TimeSpan.FromTicks (long.MinValue / 100)), "Smallest representable delta");
		}

		[Test]
		public void DispatchWallTime ()
		{
			var before = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds ();
			var wallTime = DispatchTime.Now.WallTime;
			var after = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds ();
			var milliseconds = -(long) wallTime.Nanoseconds / 1000000;
			Assert.That (milliseconds, Is.InRange (before, after), "Wall-clock deadline encodes the current Unix time");
			var later = new DispatchTime (wallTime, TimeSpan.FromSeconds (1));
			Assert.That (-(long) later.Nanoseconds, Is.EqualTo (-(long) wallTime.Nanoseconds + 1000000000), "Wall-clock offset");
			Assert.That (new DispatchTime (42).Nanoseconds, Is.EqualTo (42UL), "Raw native time");
		}

#if !MONOMAC // UIKitThreadAccessException and NSStringDrawing.WeakDrawString don't exist on mac
		[Test]
		public void EverAfter ()
		{
#if !DEBUG || OPTIMIZEALL
			Assert.Ignore ("UIKitThreadAccessException is not throw, by default, on release builds (removed by the linker)");
#endif
			bool hit = false;
			// We need to check the UIKitThreadAccessException, but there are very few API
			// with that check on WatchOS. NSStringDrawing.WeakDrawString is one example, here we pass
			// it null for the parameter so that it immediately returns with a NullReferenceException
			// instead of trying to load an image (which is not what we're testing). There
			// is also a test to ensure UIKitThreadAccessException is thrown if not on
			// the UI thread (so that we'll notice if the UIKitThreadAccessException is ever
			// removed from NSStringDrawing.WeakDrawString).
			var uiThread = Thread.CurrentThread;
			Thread queueThread = null;
			Thread mainQthread = null;
			Exception ex = null;
			Exception queue_ex = null;

			var defaultQ = DispatchQueue.GetGlobalQueue (DispatchQueuePriority.Default);
			defaultQ.DispatchAfter (new DispatchTime (DispatchTime.Now, 1000), delegate
			{
				try {
					NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
				} catch (Exception e) {
					queue_ex = e;
				}

				queueThread = Thread.CurrentThread;
				var mainQ = DispatchQueue.MainQueue;
				mainQ.DispatchAfter (DispatchTime.Now, delegate
				{
					mainQthread = Thread.CurrentThread;
					try {
						NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
					} catch (Exception e) {
						ex = e;
					} finally {
						hit = true;
					}
				});
			});

			// Now wait for the above to actually run
			while (hit == false) {
				NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.5));
			}
			Assert.That (ex, Is.Not.Null, "main ex");
			Assert.That (ex.GetType (), Is.SameAs (typeof (NullReferenceException)), "no thread check hit");
			Assert.That (queue_ex, Is.Not.Null, "queue ex");
			Assert.That (queue_ex.GetType (), Is.SameAs (typeof (UIKitThreadAccessException)), "thread check hit");
			Assert.That (uiThread, Is.EqualTo (mainQthread), "mainq thread is equal to uithread");
			Assert.That (queueThread, Is.Not.EqualTo (mainQthread), "queueThread is not the same as the UI thread");
		}

		[Test]
		public void EverAfterQualityOfService ()
		{
#if !DEBUG || OPTIMIZEALL
			Assert.Ignore ("UIKitThreadAccessException is not throw, by default, on release builds (removed by the linker)");
#endif
			bool hit = false;
			// We need to check the UIKitThreadAccessException, but there are very few API
			// with that check on WatchOS. NSStringDrawing.WeakDrawString is one example, here we pass
			// it null for the parameter so that it immediately returns with a NullReferenceException
			// instead of trying to load an image (which is not what we're testing). There
			// is also a test to ensure UIKitThreadAccessException is thrown if not on
			// the UI thread (so that we'll notice if the UIKitThreadAccessException is ever
			// removed from NSStringDrawing.WeakDrawString).
			var uiThread = Thread.CurrentThread;
			Thread queueThread = null;
			Thread mainQthread = null;
			Exception ex = null;
			Exception queue_ex = null;

			var defaultQ = DispatchQueue.GetGlobalQueue (DispatchQualityOfService.Default);
			defaultQ.DispatchAfter (new DispatchTime (DispatchTime.Now, 1000), delegate
			{
				try {
					NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
				} catch (Exception e) {
					queue_ex = e;
				}

				queueThread = Thread.CurrentThread;
				var mainQ = DispatchQueue.MainQueue;
				mainQ.DispatchAfter (DispatchTime.Now, delegate
				{
					mainQthread = Thread.CurrentThread;
					try {
						NSStringDrawing.WeakDrawString (null, CGPoint.Empty, null);
					} catch (Exception e) {
						ex = e;
					} finally {
						hit = true;
					}
				});
			});

			// Now wait for the above to actually run
			while (hit == false) {
				NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.5));
			}
			Assert.That (ex, Is.Not.Null, "main ex");
			Assert.That (ex.GetType (), Is.SameAs (typeof (NullReferenceException)), "no thread check hit");
			Assert.That (queue_ex, Is.Not.Null, "queue ex");
			Assert.That (queue_ex.GetType (), Is.SameAs (typeof (UIKitThreadAccessException)), "thread check hit");
			Assert.That (uiThread, Is.EqualTo (mainQthread), "mainq thread is equal to uithread");
			Assert.That (queueThread, Is.Not.EqualTo (mainQthread), "queueThread is not the same as the UI thread");
		}
#endif
	}
}
