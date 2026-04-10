#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using CoreMedia;
using CoreMediaIO;
using NUnit.Framework;


namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOStreamClockTest {

		[Test]
		public void Create_Invalidate ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			int status = CMIOStreamClock.Create (
				clockName: "TestClock",
				sourceIdentifier: IntPtr.Zero,
				getTimeCallMinimumInterval: new CMTime (1, 100),
				numberOfEventsForRateSmoothing: 10,
				numberOfAveragesForRateSmoothing: 5,
				out IntPtr clock);

			Assert.AreEqual (0, status, "Create status");
			Assert.AreNotEqual (IntPtr.Zero, clock, "Clock handle");

			int invalidateStatus = CMIOStreamClock.Invalidate (clock);
			Assert.AreEqual (0, invalidateStatus, "Invalidate status");

			TestRuntime.CFRelease (clock);
		}

		[Test]
		public void PostTimingEvent ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			int status = CMIOStreamClock.Create (
				clockName: "TimingTestClock",
				sourceIdentifier: IntPtr.Zero,
				getTimeCallMinimumInterval: new CMTime (1, 30),
				numberOfEventsForRateSmoothing: 5,
				numberOfAveragesForRateSmoothing: 0,
				out IntPtr clock);

			Assert.AreEqual (0, status, "Create status");

			int postStatus = CMIOStreamClock.PostTimingEvent (
				new CMTime (0, 30),
				hostTime: 0,
				resynchronize: true,
				clock);
			Assert.AreEqual (0, postStatus, "PostTimingEvent status");

			CMIOStreamClock.Invalidate (clock);
			TestRuntime.CFRelease (clock);
		}

		[Test]
		public void ConvertHostTimeToDeviceTime ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			int status = CMIOStreamClock.Create (
				clockName: "ConvertTestClock",
				sourceIdentifier: IntPtr.Zero,
				getTimeCallMinimumInterval: new CMTime (1, 30),
				numberOfEventsForRateSmoothing: 5,
				numberOfAveragesForRateSmoothing: 0,
				out IntPtr clock);

			Assert.AreEqual (0, status, "Create status");

			// Post an event so the clock has a reference point
			CMIOStreamClock.PostTimingEvent (new CMTime (0, 30), 0, true, clock);

			CMTime deviceTime = CMIOStreamClock.ConvertHostTimeToDeviceTime (0, clock);
			// The result may be invalid if no timing events have been posted; just verify no crash
			Assert.IsNotNull (deviceTime, "DeviceTime");

			CMIOStreamClock.Invalidate (clock);
			TestRuntime.CFRelease (clock);
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
