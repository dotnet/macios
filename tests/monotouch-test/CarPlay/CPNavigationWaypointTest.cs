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

			Assert.IsNotNull (waypoint, "waypoint");
			Assert.AreEqual ("Test", waypoint.Name, "Name");
			Assert.AreEqual ("123 Main St", waypoint.Address, "Address");
			Assert.AreEqual ((nuint) 2, waypoint.EntryPointsCount, "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.AreEqual (2, result.Length, "EntryPoints.Length");
			Assert.AreEqual (37.7750, result [0].Latitude, 0.0001, "EntryPoints[0].Latitude");
			Assert.AreEqual (-122.4195, result [0].Longitude, 0.0001, "EntryPoints[0].Longitude");
			Assert.AreEqual (5.0, result [0].Altitude, 0.0001, "EntryPoints[0].Altitude");
			Assert.AreEqual (37.7751, result [1].Latitude, 0.0001, "EntryPoints[1].Latitude");
			Assert.AreEqual (-122.4196, result [1].Longitude, 0.0001, "EntryPoints[1].Longitude");
			Assert.AreEqual (15.0, result [1].Altitude, 0.0001, "EntryPoints[1].Altitude");
		}

		[Test]
		public void CreateWithNullEntryPoints ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 40.7128, Longitude = -74.0060, Altitude = 0.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, "NYC", null, null, null);

			Assert.IsNotNull (waypoint, "waypoint");
			Assert.AreEqual ("NYC", waypoint.Name, "Name");
			Assert.AreEqual ((nuint) 0, waypoint.EntryPointsCount, "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.AreEqual (0, result.Length, "EntryPoints.Length");
		}

		[Test]
		public void CreateWithEmptyEntryPoints ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 51.5074, Longitude = -0.1278, Altitude = 11.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, "London", null, new CPLocationCoordinate3D [0], null);

			Assert.IsNotNull (waypoint, "waypoint");
			Assert.AreEqual ((nuint) 0, waypoint.EntryPointsCount, "EntryPointsCount");
			Assert.AreEqual (0, waypoint.EntryPoints.Length, "EntryPoints.Length");
		}

		[Test]
		public void CreateWithSingleEntryPoint ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = 48.8566, Longitude = 2.3522, Altitude = 35.0 };
			var entryPoints = new CPLocationCoordinate3D [] {
				new CPLocationCoordinate3D { Latitude = 48.8567, Longitude = 2.3523, Altitude = 36.0 },
			};

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, null, null, entryPoints, null);

			Assert.IsNotNull (waypoint, "waypoint");
			Assert.AreEqual ((nuint) 1, waypoint.EntryPointsCount, "EntryPointsCount");

			var result = waypoint.EntryPoints;
			Assert.AreEqual (1, result.Length, "EntryPoints.Length");
			Assert.AreEqual (48.8567, result [0].Latitude, 0.0001, "EntryPoints[0].Latitude");
		}

		[Test]
		public void CenterPointRoundTrip ()
		{
			var centerPoint = new CPLocationCoordinate3D { Latitude = -33.8688, Longitude = 151.2093, Altitude = 58.0 };

			var waypoint = CPNavigationWaypoint.Create (centerPoint, null, null, null, null, null);

			Assert.AreEqual (-33.8688, waypoint.CenterPoint.Latitude, 0.0001, "CenterPoint.Latitude");
			Assert.AreEqual (151.2093, waypoint.CenterPoint.Longitude, 0.0001, "CenterPoint.Longitude");
			Assert.AreEqual (58.0, waypoint.CenterPoint.Altitude, 0.0001, "CenterPoint.Altitude");
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

			Assert.IsNotNull (segment, "segment");
			Assert.AreEqual ((nint) 3, segment.CoordinatesCount, "CoordinatesCount");

			var result = segment.Coordinates;
			Assert.AreEqual (3, result.Length, "Coordinates.Length");
			Assert.AreEqual (37.0, result [0].Latitude, 0.0001, "Coordinates[0].Latitude");
			Assert.AreEqual (-122.0, result [0].Longitude, 0.0001, "Coordinates[0].Longitude");
			Assert.AreEqual (0.0, result [0].Altitude, 0.0001, "Coordinates[0].Altitude");
			Assert.AreEqual (36.0, result [1].Latitude, 0.0001, "Coordinates[1].Latitude");
			Assert.AreEqual (-121.0, result [1].Longitude, 0.0001, "Coordinates[1].Longitude");
			Assert.AreEqual (100.0, result [1].Altitude, 0.0001, "Coordinates[1].Altitude");
			Assert.AreEqual (35.0, result [2].Latitude, 0.0001, "Coordinates[2].Latitude");
			Assert.AreEqual (-120.0, result [2].Longitude, 0.0001, "Coordinates[2].Longitude");
			Assert.AreEqual (200.0, result [2].Altitude, 0.0001, "Coordinates[2].Altitude");
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

			Assert.IsNotNull (segment.Origin, "Origin");
			Assert.IsNotNull (segment.Destination, "Destination");
			Assert.IsNotNull (segment.Identifier, "Identifier");
		}
	}
}

#endif // HAS_CARPLAY
