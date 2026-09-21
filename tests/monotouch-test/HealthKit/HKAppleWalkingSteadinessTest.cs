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
			Assert.That (HKAppleWalkingSteadiness.TryGetClassification (max, out var classification, out var error), Is.True);
			Assert.That (error, Is.Null, "error");
			Assert.That (HKAppleWalkingSteadinessClassification.Ok, Is.EqualTo (classification), "classification");
		}

		[Test]
		public void GetMinimumQuantityTest ()
			=> Assert.That (HKAppleWalkingSteadiness.GetMinimumQuantity (HKAppleWalkingSteadinessClassification.Ok), Is.Not.Null);

		[Test]
		public void GetMaximumQuantityTest ()
			=> Assert.That (HKAppleWalkingSteadiness.GetMaximumQuantity (HKAppleWalkingSteadinessClassification.Ok), Is.Not.Null);
	}
}

#endif // HAS_HEALTHKIT
