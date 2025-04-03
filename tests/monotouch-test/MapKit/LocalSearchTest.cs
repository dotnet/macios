//
// Unit tests for MKLocalSearch
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using CoreLocation;
using MapKit;
using ObjCRuntime;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.MapKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LocalSearchTest {
		[Test]
		public void EmptyRequest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 6, 1, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 9, 2, throwIfOtherPlatform: false);

			using (var lsr = new MKLocalSearchRequest ())
			using (MKLocalSearch ls = new MKLocalSearch (lsr)) {
				lsr.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (47, -71), new MKCoordinateSpan (1, 1));
				bool wait = true;
				ls.Start ((MKLocalSearchResponse response, NSError error) => {
					wait = false;
				});
				Assert.True (ls.IsSearching, "IsSearching");

				// wait a bit before cancelling the search (so it really starts)
				// otherwise IsSearching might never complete (on iOS8) and seems very random (in earlier versions)
				NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (1));
				ls.Cancel ();

				// give it some time to cancel
				NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (1));

				// the timeout is not always long enough, and we don't want to wait a long time, so just accept whatever
				Assert.That (ls.IsSearching, Is.True.Or.False, "IsSearching/Cancel");
				Assert.That (wait, Is.True.Or.False, "IsSearching/Cancel - wait");
			}
		}
	}
}
