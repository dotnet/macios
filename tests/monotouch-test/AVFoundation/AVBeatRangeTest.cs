using AVFoundation;
namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVBeatRangeTest {

		[Test]
		public void ConstructorTest ()
		{
			double start = 2.3;
			double length = 30.90;

			var range = new AVBeatRange (start, length);

			ClassicAssert.AreEqual (start, range.Start, "Wrong Start value.");
			ClassicAssert.AreEqual (length, range.Length, "Wrong Length value.");
		}

		[Test]
		public void AreEqualTrueTest ()
		{
			double start = 2.3;
			double length = 30.90;
			var range1 = new AVBeatRange (start, length);
			var range2 = new AVBeatRange (start, length);

			ClassicAssert.True (range1 == range2, "range1 == range2");
			ClassicAssert.True (range1.Equals (range2), "range1.Equals (range1)");
			ClassicAssert.False (range1 != range2, "range1 != range2");
		}

		[Test]
		public void AreEqualFalseTest ()
		{
			var range1 = new AVBeatRange (90, 12);
			var range2 = new AVBeatRange (range1.Start * 2, range1.Length * 2);
			ClassicAssert.False (range1 == range2, "range1 == range2");
			ClassicAssert.False (range1.Equals (range2), "range1.Equals (range2)");
			ClassicAssert.True (range1 != range2, "range1 != range2");
		}

		[Test]
		public void AreEqualDiffType ()
		{
			var range = new AVBeatRange (90, 12);
			var str = new NSString ("Foo");
			ClassicAssert.False (range.Equals ((object) str));
		}
	}
}
