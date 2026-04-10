#if HAS_COREMEDIAIO
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
public void TryCreate_Dispose ()
{
TestRuntime.AssertXcodeVersion (13, 3);

bool created = CMIOStreamClock.TryCreate (
clockName: "TestClock",
sourceIdentifier: IntPtr.Zero,
getTimeCallMinimumInterval: new CMTime (1, 100),
numberOfEventsForRateSmoothing: 10,
numberOfAveragesForRateSmoothing: 5,
out int status,
out var clock);

Assert.IsTrue (created, "Created");
Assert.AreEqual (0, status, "Status");
Assert.IsNotNull (clock, "Clock");
clock?.Dispose ();
}

[Test]
public void PostTimingEvent ()
{
TestRuntime.AssertXcodeVersion (13, 3);

CMIOStreamClock.TryCreate (
clockName: "TimingTestClock",
sourceIdentifier: IntPtr.Zero,
getTimeCallMinimumInterval: new CMTime (1, 30),
numberOfEventsForRateSmoothing: 5,
numberOfAveragesForRateSmoothing: 0,
out _,
out var clock);

Assert.IsNotNull (clock, "Clock");
using (clock!) {
int postStatus = clock.PostTimingEvent (
new CMTime (0, 30),
hostTime: 0,
resynchronize: true);
Assert.AreEqual (0, postStatus, "PostTimingEvent status");
}
}

[Test]
public void ConvertHostTimeToDeviceTime ()
{
TestRuntime.AssertXcodeVersion (13, 3);

CMIOStreamClock.TryCreate (
clockName: "ConvertTestClock",
sourceIdentifier: IntPtr.Zero,
getTimeCallMinimumInterval: new CMTime (1, 30),
numberOfEventsForRateSmoothing: 5,
numberOfAveragesForRateSmoothing: 0,
out _,
out var clock);

Assert.IsNotNull (clock, "Clock");
using (clock!) {
clock.PostTimingEvent (new CMTime (0, 30), 0, true);

CMTime deviceTime = clock.ConvertHostTimeToDeviceTime (0);
Assert.IsNotNull (deviceTime, "DeviceTime");
}
}
}
}
#endif // HAS_COREMEDIAIO
