//
// Unit tests for CPNavigationWaypoint and CPRouteSegment
//
// Copyright (c) Microsoft Corporation.
//

#if HAS_CARPLAY

using System;
using CarPlay;
using Foundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.CarPlay {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CPNavigationWaypointTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (26, 4);
		}

		[Test]
		public void CreateWithCenterPointAndEntryPoints ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 37.7749, Longitude = -122.4194, Altitude = 10.0 };
			var entryPoints = new CPLocationCoordinate3D [] {
				new CPLocationCoordinate3D { Latitude = 37.7750, Longitude = -122.4195, Altitude = 5.0 },
				new CPLocationCoordinate3D { Latitude = 37.7751, Longitude = -122.4196, Altitude = 15.0 },
			};

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, "Test", "123 Main St", entryPoints, null);

			Assert.That (waypoint, Is.Not.Null, "waypoint");
			Assert.That (waypoint.Name, Is.EqualTo ("Test"), "Name");
			Assert.That (waypoint.Address, Is.EqualTo ("123 Main St"), "Address");
			Assert.That (waypoint.EntryPointsCount, Is.EqualTo ((nuint) 2), "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.That (result.Length, Is.EqualTo (2), "EntryPoints.Length");
			Assert.That (result [0].Latitude, Is.EqualTo (37.7750).Within (0.0001), "EntryPoints[0].Latitude");
			Assert.That (result [0].Longitude, Is.EqualTo (-122.4195).Within (0.0001), "EntryPoints[0].Longitude");
			Assert.That (result [0].Altitude, Is.EqualTo (5.0).Within (0.0001), "EntryPoints[0].Altitude");
			Assert.That (result [1].Latitude, Is.EqualTo (37.7751).Within (0.0001), "EntryPoints[1].Latitude");
			Assert.That (result [1].Longitude, Is.EqualTo (-122.4196).Within (0.0001), "EntryPoints[1].Longitude");
			Assert.That (result [1].Altitude, Is.EqualTo (15.0).Within (0.0001), "EntryPoints[1].Altitude");
		}

		[Test]
		public void CreateWithNullEntryPoints ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 40.7128, Longitude = -74.0060, Altitude = 0.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, "NYC", null, null, null);

			Assert.That (waypoint, Is.Not.Null, "waypoint");
			Assert.That (waypoint.Name, Is.EqualTo ("NYC"), "Name");
			Assert.That (waypoint.EntryPointsCount, Is.EqualTo ((nuint) 0), "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.That (result.Length, Is.EqualTo (0), "EntryPoints.Length");
		}

		[Test]
		public void CreateWithEmptyEntryPoints ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 51.5074, Longitude = -0.1278, Altitude = 11.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, "London", null, new CPLocationCoordinate3D [0], null);

			Assert.That (waypoint, Is.Not.Null, "waypoint");
			Assert.That (waypoint.EntryPointsCount, Is.EqualTo ((nuint) 0), "EntryPointsCount");
			Assert.That (waypoint.EntryPoints.Length, Is.EqualTo (0), "EntryPoints.Length");
		}

		[Test]
		public void CreateWithSingleEntryPoint ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 48.8566, Longitude = 2.3522, Altitude = 35.0 };
			var entryPoints = new CPLocationCoordinate3D [] {
				new CPLocationCoordinate3D { Latitude = 48.8567, Longitude = 2.3523, Altitude = 36.0 },
			};

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, null, null, entryPoints, null);

			Assert.That (waypoint, Is.Not.Null, "waypoint");
			Assert.That (waypoint.EntryPointsCount, Is.EqualTo ((nuint) 1), "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.That (result.Length, Is.EqualTo (1), "EntryPoints.Length");
			Assert.That (result [0].Latitude, Is.EqualTo (48.8567).Within (0.0001), "EntryPoints[0].Latitude");
		}

		[Test]
		public void CenterPointRoundTrip ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = -33.8688, Longitude = 151.2093, Altitude = 58.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, null, null, null, null);

			Assert.That (waypoint.CenterPoint.Latitude, Is.EqualTo (-33.8688).Within (0.0001), "CenterPoint.Latitude");
			Assert.That (waypoint.CenterPoint.Longitude, Is.EqualTo (151.2093).Within (0.0001), "CenterPoint.Longitude");
			Assert.That (waypoint.CenterPoint.Altitude, Is.EqualTo (58.0).Within (0.0001), "CenterPoint.Altitude");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CPRouteSegmentTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (26, 4);
		}

		[Test]
		public void CreateWithCoordinates ()
		{
			var origin = CPNavigationWaypoint.Create (
				new CPLocationCoordinate3D { Latitude = 37.7749, Longitude = -122.4194, Altitude = 0.0 },
				null, "Origin", null, null, null);
			var destination = CPNavigationWaypoint.Create (
				new CPLocationCoordinate3D { Latitude = 34.0522, Longitude = -118.2437, Altitude = 0.0 },
				null, "Destination", null, null, null);

			var distance = new NSMeasurement<NSUnitLength> (100.0, NSUnitLength.Miles);
			var estimates = new CPTravelEstimates (distance, 3600.0);

			var coordinates = new CPLocationCoordinate3D [] {
				new CPLocationCoordinate3D { Latitude = 37.0, Longitude = -122.0, Altitude = 0.0 },
				new CPLocationCoordinate3D { Latitude = 36.0, Longitude = -121.0, Altitude = 100.0 },
				new CPLocationCoordinate3D { Latitude = 35.0, Longitude = -120.0, Altitude = 200.0 },
			};

			var segment = CPRouteSegment.Create (
				origin, destination,
				new CPManeuver [] { new CPManeuver () },
				new CPLaneGuidance [] { new CPLaneGuidance () },
				new CPManeuver [] { new CPManeuver () },
				new CPLaneGuidance (),
				estimates, estimates,
				coordinates);

			Assert.That (segment, Is.Not.Null, "segment");
			Assert.That (segment.CoordinatesCount, Is.EqualTo ((nint) 3), "CoordinatesCount");

			var result = segment.Coordinates;
			Assert.That (result.Length, Is.EqualTo (3), "Coordinates.Length");
			Assert.That (result [0].Latitude, Is.EqualTo (37.0).Within (0.0001), "Coordinates[0].Latitude");
			Assert.That (result [0].Longitude, Is.EqualTo (-122.0).Within (0.0001), "Coordinates[0].Longitude");
			Assert.That (result [0].Altitude, Is.EqualTo (0.0).Within (0.0001), "Coordinates[0].Altitude");
			Assert.That (result [1].Latitude, Is.EqualTo (36.0).Within (0.0001), "Coordinates[1].Latitude");
			Assert.That (result [1].Longitude, Is.EqualTo (-121.0).Within (0.0001), "Coordinates[1].Longitude");
			Assert.That (result [1].Altitude, Is.EqualTo (100.0).Within (0.0001), "Coordinates[1].Altitude");
			Assert.That (result [2].Latitude, Is.EqualTo (35.0).Within (0.0001), "Coordinates[2].Latitude");
			Assert.That (result [2].Longitude, Is.EqualTo (-120.0).Within (0.0001), "Coordinates[2].Longitude");
			Assert.That (result [2].Altitude, Is.EqualTo (200.0).Within (0.0001), "Coordinates[2].Altitude");
		}

		[Test]
		public void OriginAndDestination ()
		{
			var origin = CPNavigationWaypoint.Create (
				new CPLocationCoordinate3D { Latitude = 37.7749, Longitude = -122.4194, Altitude = 0.0 },
				null, "Start", null, null, null);
			var destination = CPNavigationWaypoint.Create (
				new CPLocationCoordinate3D { Latitude = 34.0522, Longitude = -118.2437, Altitude = 0.0 },
				null, "End", null, null, null);

			var distance = new NSMeasurement<NSUnitLength> (50.0, NSUnitLength.Kilometers);
			var estimates = new CPTravelEstimates (distance, 1800.0);

			var coordinates = new CPLocationCoordinate3D [] {
				new CPLocationCoordinate3D { Latitude = 37.0, Longitude = -122.0, Altitude = 0.0 },
			};

			var segment = CPRouteSegment.Create (
				origin, destination,
				new CPManeuver [] { new CPManeuver () },
				new CPLaneGuidance [] { new CPLaneGuidance () },
				new CPManeuver [] { new CPManeuver () },
				new CPLaneGuidance (),
				estimates, estimates,
				coordinates);

			Assert.That (segment.Origin, Is.Not.Null, "Origin");
			Assert.That (segment.Destination, Is.Not.Null, "Destination");
			Assert.That (segment.Identifier, Is.Not.Null, "Identifier");
		}
	}
}

#endif // HAS_CARPLAY
