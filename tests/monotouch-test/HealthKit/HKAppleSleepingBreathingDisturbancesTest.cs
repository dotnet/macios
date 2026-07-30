#if HAS_HEALTHKIT

using HealthKit;

namespace MonoTouchFixtures.HealthKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class HKAppleSleepingBreathingDisturbancesTest {
		[Test]
		public void RoundtripTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var minimum = HKAppleSleepingBreathingDisturbances.GetMinimumQuantity (HKAppleSleepingBreathingDisturbancesClassification.Elevated);
			var classification = HKAppleSleepingBreathingDisturbances.GetClassification (minimum);
			Assert.That (classification, Is.EqualTo (HKAppleSleepingBreathingDisturbancesClassification.Elevated), "Classification");
		}
	}
}

#endif // HAS_HEALTHKIT
