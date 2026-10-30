// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

using CoreFoundation;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class OSWorkgroupTest {

		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern ulong mach_absolute_time ();

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
		}

		[Test]
		public void ParallelJoinLeaveCancel ()
		{
			using var wg = OSWorkgroupParallel.Create ("monotouch-test");
			Assert.That (wg, Is.Not.Null, "Create");
			Assert.That (wg.IsCancelled, Is.False, "IsCancelled 1");
			Assert.That (wg.MaxParallelThreads, Is.GreaterThan (0), "MaxParallelThreads");

			using (wg.Join ()) {
			}

			Assert.That (wg.TryJoin (out var token), Is.True, "TryJoin");
			token.Dispose ();

			// Leaving must work even if the workgroup instance has been disposed first.
			var wg2 = OSWorkgroupParallel.Create ("monotouch-test-2");
			var token2 = wg2.Join ();
			wg2.Dispose ();
			token2.Dispose ();

			wg.Cancel ();
			Assert.That (wg.IsCancelled, Is.True, "IsCancelled 2");
			Assert.That (wg.TryJoin (out var token3, out var error), Is.False, "TryJoin cancelled");
			Assert.That (token3, Is.Null, "token3");
			Assert.That (error, Is.Not.EqualTo (0), "error");
		}

		[Test]
		public void CreateFromWorkgroup ()
		{
			using var parent = OSWorkgroupParallel.Create ("monotouch-test-parent");
			using var child = OSWorkgroup.Create ("monotouch-test-child", parent);
			Assert.That (child, Is.Not.Null, "Create");
		}

		[Test]
		public void AudioWorkInterval ()
		{
			using var wg = global::AudioToolbox.AudioWorkInterval.Create ("monotouch-test");
			Assert.That (wg, Is.Not.Null, "Create");
			Assert.That (wg.IsCancelled, Is.False, "IsCancelled");
		}

		[Test]
		public void IntervalStartUpdateFinish ()
		{
			using var wg = global::AudioToolbox.AudioWorkInterval.Create ("monotouch-test-interval");
			Assert.That (wg, Is.Not.Null, "Create");
			using var token = wg.Join ();

			var now = mach_absolute_time ();
			var delta = 100_000_000UL;
			wg.Start (now, now + delta);
			wg.Update (now + 2 * delta);
			wg.Finish ();

			Assert.That (wg.TryStart (now, now + delta), Is.True, "TryStart");
			Assert.That (wg.TryUpdate (now + 2 * delta), Is.True, "TryUpdate");
			Assert.That (wg.TryFinish (), Is.True, "TryFinish");

			Assert.That (wg.TryStart (now, now + delta, out var error), Is.True, "TryStart error");
			Assert.That (error, Is.EqualTo (0), "TryStart error value");
			Assert.That (wg.TryUpdate (now + 2 * delta, out error), Is.True, "TryUpdate error");
			Assert.That (error, Is.EqualTo (0), "TryUpdate error value");
			Assert.That (wg.TryFinish (out error), Is.True, "TryFinish error");
			Assert.That (error, Is.EqualTo (0), "TryFinish error value");
		}

		[Test]
		public void JoinCancelled ()
		{
			using var wg = OSWorkgroupParallel.Create (null);
			Assert.That (wg, Is.Not.Null, "Create (null)");
			wg.Cancel ();
			Assert.That (wg.TryJoin (out var token), Is.False, "TryJoin");
			Assert.That (token, Is.Null, "token");
			Assert.Throws<InvalidOperationException> (() => wg.Join (), "Join");
		}

		[Test]
		public void CreateFromWorkgroupNullName ()
		{
			using var parent = OSWorkgroupParallel.Create ("monotouch-test-parent");
			using var child = OSWorkgroup.Create (null, parent);
			Assert.That (child, Is.Not.Null, "Create");
			Assert.Throws<ArgumentNullException> (() => OSWorkgroup.Create ("name", null!), "null workgroup");
		}

#if __MACOS__
		[Test]
		public void MachPort ()
		{
			using var wg = global::AudioToolbox.AudioWorkInterval.Create ("monotouch-test-port");
			Assert.That (wg.TryCopyPort (out var port), Is.True, "TryCopyPort");
			Assert.That (port, Is.Not.EqualTo (0u), "port");
			Assert.That (wg.TryCopyPort (out var port2, out var error), Is.True, "TryCopyPort error");
			Assert.That (error, Is.EqualTo (0), "error");
			Assert.That (port2, Is.Not.EqualTo (0u), "port2");

			using var fromPort = OSWorkgroup.Create ("monotouch-test-from-port", port);
			Assert.That (fromPort, Is.Not.Null, "Create from port");
		}
#endif
	}
}
