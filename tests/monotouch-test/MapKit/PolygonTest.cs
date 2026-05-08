// Copyright 2011 Xamarin Inc. All rights reserved

#if !__TVOS__

using System.Drawing;
using CoreLocation;
using MapKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.MapKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PolygonTest {
		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);
		}

		[Test]
		public void FromPoints_Null ()
		{
			Assert.Throws<ArgumentNullException> (() => MKPolygon.FromPoints (null));
		}

		[Test]
		public void FromPoints_Interior_Null ()
		{
			MKPolygon pg = MKPolygon.FromPoints (new MKMapPoint [] { }, null);
			CheckEmpty (pg);
		}

		void CheckEmpty (MKPolygon pg)
		{
			// MKAnnotation
			Assert.That (pg.Coordinate.Longitude, Is.NaN, "Coordinate.Longitude");
			Assert.That (pg.Coordinate.Latitude, Is.NaN, "Coordinate.Latitude");
			Assert.That (pg.Title, Is.Null, "Title");
			Assert.That (pg.Subtitle, Is.Null, "Subtitle");
			// MKOverlay
			Assert.That (Double.IsPositiveInfinity (pg.BoundingMapRect.Origin.X), Is.True, "BoundingMapRect.Origin.X");
			Assert.That (Double.IsPositiveInfinity (pg.BoundingMapRect.Origin.Y), Is.True, "BoundingMapRect.Origin.Y");
			Assert.That (Double.IsNegativeInfinity (pg.BoundingMapRect.Size.Height), Is.True, "BoundingMapRect.Size.Height");
			Assert.That (Double.IsNegativeInfinity (pg.BoundingMapRect.Size.Width), Is.True, "BoundingMapRect.Size.Width");
			Assert.That (pg.Intersects (pg.BoundingMapRect), Is.False, "Intersect/Self");
			MKMapRect rect = new MKMapRect (0, 0, 0, 0);
			Assert.That (pg.Intersects (rect), Is.False, "Intersect/Empty");

			ShapeTest.CheckShape (pg);
		}

		[Test]
		public void FromPoints_Empty ()
		{
			MKPolygon pg = MKPolygon.FromPoints (new MKMapPoint [] { });
			CheckEmpty (pg);
		}

		[Test]
		public void FromPoints_Interior_Empty ()
		{
			MKPolygon pg = MKPolygon.FromPoints (new MKMapPoint [] { }, new MKPolygon [] { });
			CheckEmpty (pg);
		}

		[Test]
		public void FromCoordinates_Null ()
		{
			Assert.Throws<ArgumentNullException> (() => MKPolygon.FromCoordinates (null));
		}

		[Test]
		public void FromCoordinates_Interior_Null ()
		{
			MKPolygon pg = MKPolygon.FromCoordinates (new CLLocationCoordinate2D [] { }, null);
			CheckEmpty (pg);
		}

		[Test]
		public void FromCoordinates_Empty ()
		{
			MKPolygon pg = MKPolygon.FromCoordinates (new CLLocationCoordinate2D [] { });
			CheckEmpty (pg);
		}

		[Test]
		public void FromCoordinates_Interior_Empty ()
		{
			MKPolygon pg = MKPolygon.FromCoordinates (new CLLocationCoordinate2D [] { }, new MKPolygon [] { });
			CheckEmpty (pg);
		}

#if false
		// Annotations that support dragging should implement this method to update the position of the annotation.
		// keyword is SHOULD - it's not working for MKPolygon
		// http://developer.apple.com/library/ios/#documentation/MapKit/Reference/MKAnnotation_Protocol/Reference/Reference.html#//apple_ref/occ/intf/MKAnnotation
		[Test]
		public void setCoordinate_Selector ()
		{
			MKPolygon pg = MKPolygon.FromPoints (new MKMapPoint [] { });
			try {
				pg.Coordinate = new CLLocationCoordinate2D (10, 20);
			}
			catch (ObjCException mte) {
				Assert.That (mte.Message.Contains ("unrecognized selector sent to instance"), Is.True);
			}
			catch {
				Assert.Fail ("API could be working/implemented");
			}
		}
#endif
	}
}

#endif // !__TVOS__
