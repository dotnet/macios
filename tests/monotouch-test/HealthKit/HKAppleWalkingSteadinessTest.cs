#if HAS_HEALTHKIT

using HealthKit;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.HealthKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class HKAppleWalkingSteadinessTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertNotDesktop (); // Only runs on iOS Devices or Simulators, which makes sense `Apple Walking Steadiness`.
			TestRuntime.AssertXcodeVersion (13, 0);
		}

		[Test]
		public void TryGetClassificationTest ()
		{
			var max = HKAppleWalkingSteadiness.GetMaximumQuantity (HKAppleWalkingSteadinessClassification.Ok);
			ClassicAssert.True (HKAppleWalkingSteadiness.TryGetClassification (max, out var classification, out var error));
			ClassicAssert.Null (error, "error");
			ClassicAssert.AreEqual (classification, HKAppleWalkingSteadinessClassification.Ok, "classification");
		}

		[Test]
		public void GetMinimumQuantityTest ()
			=> ClassicAssert.NotNull (HKAppleWalkingSteadiness.GetMinimumQuantity (HKAppleWalkingSteadinessClassification.Ok));

		[Test]
		public void GetMaximumQuantityTest ()
			=> ClassicAssert.NotNull (HKAppleWalkingSteadiness.GetMaximumQuantity (HKAppleWalkingSteadinessClassification.Ok));
	}
}

#endif // HAS_HEALTHKIT
