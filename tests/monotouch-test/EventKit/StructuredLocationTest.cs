//
// Unit tests for EKStructuredLocation
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using EventKit;

namespace MonoTouchFixtures.EventKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StructureLocationTest {
		[Test]
		public void DefaultValues ()
		{
			if (!TestRuntime.CheckXcodeVersion (4, 5))
				Assert.Inconclusive ("EKStructuredLocation is new in 6.0");

			var sl = new EKStructuredLocation ();
			ClassicAssert.IsNull (sl.GeoLocation, "GeoLocation");
			ClassicAssert.AreEqual (0, sl.Radius, "Radius");
			ClassicAssert.IsNull (sl.Title, "Title");
		}

		[Test]
		public void FromTitle ()
		{
			if (!TestRuntime.CheckXcodeVersion (4, 5))
				Assert.Inconclusive ("EKStructuredLocation is new in 6.0");

			var sl = EKStructuredLocation.FromTitle ("my title");
			ClassicAssert.IsNull (sl.GeoLocation, "GeoLocation");
			ClassicAssert.AreEqual (0, sl.Radius, "Radius");
			ClassicAssert.AreEqual ("my title", sl.Title, "Title");
		}
	}
}

#endif // !__TVOS__
