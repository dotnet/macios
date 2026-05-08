
#if !__TVOS__

using MapKit;
using CoreGraphics;
using Xamarin.Utils;

namespace MonoTouchFixtures.MapKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MapRectTest {
		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);
		}

		[Test]
		public void Defaults ()
		{
			MKMapRect rect = new MKMapRect ();
			ClassicAssert.False (rect.IsNull, "IsNull");
			ClassicAssert.True (rect.IsEmpty, "IsEmpty");
			ClassicAssert.False (rect.Spans180thMeridian, "default");
			ClassicAssert.True (rect.Equals (rect), "Equals");
			Assert.That (rect.ToString (), Is.EqualTo (@"{{0, 0}, {0, 0}}"), "default");

			MKMapRect rect2 = new MKMapRect ();
			ClassicAssert.True (rect == rect2, "==");
			ClassicAssert.False (rect != rect2, "!=");
			Assert.That (rect.GetHashCode (), Is.EqualTo (rect2.GetHashCode ()), "GetHashCode");
		}

		[Test]
		public void ContainsPoint ()
		{
			MKMapRect rect = new MKMapRect ();
			MKMapPoint point = new MKMapPoint ();
			if (TestRuntime.CheckXcodeVersion (8, 0))
				ClassicAssert.True (rect.Contains (point), "default");
			else
				ClassicAssert.False (rect.Contains (point), "default");
		}

		[Test]
		public void ContainsRect ()
		{
			MKMapRect rect1 = new MKMapRect ();
			MKMapRect rect2 = new MKMapRect ();
			ClassicAssert.True (rect1.Contains (rect2), "default");
		}

		[Test]
		public void Union ()
		{
			MKMapRect rect1 = new MKMapRect (1, 2, 3, 4);
			MKMapRect rect2 = new MKMapRect (4, 3, 2, 1);
			MKMapRect union = MKMapRect.Union (rect1, rect2);
			Assert.That (union.ToString (), Is.EqualTo (@"{{1, 2}, {5, 4}}"), "ToString");
			Assert.That (MKMapRect.Union (rect2, rect1), Is.EqualTo (union), "==");
		}

		[Test]
		public void Intersection ()
		{
			MKMapRect rect1 = new MKMapRect (1, 2, 3, 4);
			MKMapRect rect2 = new MKMapRect (2, 3, 2, 1);
			ClassicAssert.True (MKMapRect.Intersects (rect1, rect2), "Intersects");

			MKMapRect n = MKMapRect.Intersection (rect1, rect2);
			Assert.That (n.ToString (), Is.EqualTo (@"{{2, 3}, {2, 1}}"), "ToString");

			MKMapRect rect3 = new MKMapRect (-2, -3, 2, 1);
			ClassicAssert.False (MKMapRect.Intersects (rect1, rect3), "!Intersects");
			n = MKMapRect.Intersection (rect1, rect3);
			ClassicAssert.True (n.IsNull, "IsNull");
		}

		[Test]
		public void Inset ()
		{
			MKMapRect rect = new MKMapRect (Double.PositiveInfinity, Double.PositiveInfinity, 0, 0);
			MKMapRect rectin = rect.Inset (-1, 1);
			ClassicAssert.True (rectin.IsNull, "IsNull");

			rect = new MKMapRect (1, 2, 3, 4);
			rectin = rect.Inset (-1, 1);
			Assert.That (rectin.ToString (), Is.EqualTo (@"{{0, 3}, {5, 2}}"), "ToString");
		}

		[Test]
		public void Offset ()
		{
			MKMapRect rect = new MKMapRect (Double.PositiveInfinity, Double.PositiveInfinity, 0, 0);
			MKMapRect rectoff = rect.Offset (-1, 1);
			ClassicAssert.True (rectoff.IsNull, "IsNull");

			rect = new MKMapRect (1, 2, 3, 4);
			rectoff = rect.Offset (1, -1);
			Assert.That (rectoff.ToString (), Is.EqualTo (@"{{2, 1}, {3, 4}}"), "ToString");
		}

		[Test]
		public void Remainder ()
		{
			MKMapRect rect = new MKMapRect ();
			ClassicAssert.False (rect.Spans180thMeridian, "default");
			MKMapRect remainder = rect.Remainder ();
			ClassicAssert.True (remainder.IsNull, "IsNull");

			rect = new MKMapRect (-90, -90, 90, 90);
			Assert.That (rect.Spans180thMeridian, Is.EqualTo (!TestRuntime.CheckXcodeVersion (5, 1)), rect.ToString ());
			remainder = rect.Remainder ();
			Assert.That (remainder.ToString (), Is.EqualTo (@"{{268435366, -90}, {90, 90}}"), "remainder");
		}

		[Test]
		public void Divide ()
		{
			MKMapRect rect = new MKMapRect (10, 20, 30, 40);
			MKMapRect remainder;
			MKMapRect slide = rect.Divide (2, CGRectEdge.MaxXEdge, out remainder);
			Assert.That (slide.ToString (), Is.EqualTo (@"{{38, 20}, {2, 40}}"), "slide");
			Assert.That (remainder.ToString (), Is.EqualTo (@"{{10, 20}, {28, 40}}"), "remainder");
		}

		[Test]
		public void NullRect ()
		{
			MKMapRect nullRect = MKMapRect.Null;
			MKMapRect expectedValue = new MKMapRect (double.PositiveInfinity, double.PositiveInfinity, 0, 0);
			ClassicAssert.AreEqual (expectedValue, nullRect, "NullRect equals (PositiveInfinity, PositiveInfinity, 0, 0)");
			ClassicAssert.IsTrue (nullRect.IsNull, "IsNull");
		}
	}
}

#endif // !__TVOS__
