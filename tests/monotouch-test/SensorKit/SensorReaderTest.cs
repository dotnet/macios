// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if HAS_SENSORKIT

using SensorKit;

namespace MonoTouchFixtures.SensorKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SensorReaderTest {
		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
		}

		[TestCase (null)]
		public void NullSensors (SRSensor [] sensors)
		{
			Assert.Throws<ArgumentNullException> (() => SRSensorReader.RequestAuthorization (sensors, _ => { }), "Callback");
			Assert.Throws<ArgumentNullException> (() => SRSensorReader.RequestAuthorizationAsync (sensors), "Async");
		}

		[TestCase (null)]
		public void NullCompletion (Action<NSError?> completion)
		{
			Assert.Throws<ArgumentNullException> (() => SRSensorReader.RequestAuthorization (new SRSensor [] { SRSensor.MessagesUsageReport }, completion), "Completion");
		}

		[TestCase (SRSensor.Invalid)]
		[TestCase ((SRSensor) int.MaxValue)]
		public void InvalidSensor (SRSensor sensor)
		{
			Assert.Throws<ArgumentOutOfRangeException> (() => SRSensorReader.RequestAuthorization ([sensor], _ => { }), "Callback");
			Assert.Throws<ArgumentOutOfRangeException> (() => SRSensorReader.RequestAuthorizationAsync (sensor), "Async");
		}
	}
}

#endif // HAS_SENSORKIT
