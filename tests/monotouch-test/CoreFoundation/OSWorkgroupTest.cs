// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreFoundation;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class OSWorkgroupTest {

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
	}
}
