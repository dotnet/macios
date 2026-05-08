#if __IOS__ || MONOMAC

using CoreGraphics;
using Phase;

using Vector2d = global::CoreGraphics.NVector2d;

#nullable enable

namespace MonoTouchFixtures.Phase {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class PhaseEnvelopeSegmentTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			// not supported in simulator
			TestRuntime.AssertDevice ();
		}

		[Test]
		public void ConstructorTest ()
		{
			var endPoint = new Vector2d (1, 2);
			using (var segment = new PhaseEnvelopeSegment (endPoint, PhaseCurveType.Cubed)) {
				Assert.That (segment.EndPoint, Is.EqualTo (endPoint));
				var newEndPoint = new Vector2d (2, 1);
				segment.EndPoint = newEndPoint;
				Assert.That (segment.EndPoint, Is.EqualTo (newEndPoint));
			}
		}

	}
}

#endif // __IOS__ || MONOMAC
