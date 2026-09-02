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
			Assert.That (sl.GeoLocation, Is.Null, "GeoLocation");
			Assert.That (sl.Radius, Is.EqualTo (0), "Radius");
			Assert.That (sl.Title, Is.Null, "Title");
		}

		[Test]
		public void FromTitle ()
		{
			if (!TestRuntime.CheckXcodeVersion (4, 5))
				Assert.Inconclusive ("EKStructuredLocation is new in 6.0");

			var sl = EKStructuredLocation.FromTitle ("my title");
			Assert.That (sl.GeoLocation, Is.Null, "GeoLocation");
			Assert.That (sl.Radius, Is.EqualTo (0), "Radius");
			Assert.That (sl.Title, Is.EqualTo ("my title"), "Title");
		}
	}
}

#endif // !__TVOS__
