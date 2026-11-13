// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using CoreMedia;
using CoreMediaIO;

namespace MonoTouchFixtures.CoreMediaIO {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StreamClockTest {
		[Test]
		public void TimingEvents ()
		{
			Assert.That (CMIOStreamClock.Create ("Core Media I/O test clock", new IntPtr (1), new CMTime (1, 1000), 10, 0, out var clock), Is.Zero, "Create");
			if (clock is null)
				throw new AssertionException ("No clock returned.");
			using (clock) {
				try {
					using var hostClock = CMClock.HostTimeClock;
					var hostTime = CMClock.ConvertHostTimeToSystemUnits (hostClock.CurrentTime);
					Assert.That (CMIOStreamClock.PostTimingEvent (clock, CMTime.Zero, hostTime, true), Is.Zero, "Post");
					Assert.That (CMIOStreamClock.ConvertHostTimeToDeviceTime (clock, hostTime).IsInvalid, Is.False, "Convert");
				} finally {
					Assert.That (CMIOStreamClock.Invalidate (clock), Is.Zero, "Invalidate");
				}
			}
			Assert.Throws<ObjectDisposedException> (() => CMIOStreamClock.Invalidate (clock), "Disposed");
		}
	}
}

#endif
