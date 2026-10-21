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

			using (var token = wg.Join ()) {
				Assert.That (token, Is.Not.Null, "token");
			}

			wg.Cancel ();
			Assert.That (wg.IsCancelled, Is.True, "IsCancelled 2");
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
