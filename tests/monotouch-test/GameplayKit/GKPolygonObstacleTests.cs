//
// Unit tests for GKPolygonObstacle
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using GameplayKit;

using System.Numerics;

namespace MonoTouchFixtures.GamePlayKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKPolygonObstacleTests {

		Vector2 [] points = new Vector2 [] {
			new Vector2 (0,0), new Vector2 (0,1), new Vector2 (0,2), new Vector2 (0,3),
			new Vector2 (1,0), new Vector2 (1,1), new Vector2 (1,2), new Vector2 (1,3)
		};

		[Test]
		public void FromPointsTest ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var obstacle = GKPolygonObstacle.FromPoints (points);
			Assert.That (obstacle, Is.Not.Null, "GKPolygonObstacle.FromPoints should not be null");

			var count = obstacle.VertexCount;
			Assert.That ((int) count, Is.EqualTo (points.Length), "GKPolygonObstacle lengt should be equal");

			for (nuint i = 0; i < count; i++)
				Assert.That (obstacle.GetVertex (i), Is.EqualTo (points [(int) i]), "GKPolygonObstacle vectors should be equal");
		}

		[Test]
		public void InitWithPointsTest ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var obstacle = new GKPolygonObstacle (points);
			Assert.That (obstacle, Is.Not.Null, "GKPolygonObstacle ctor should not be null");

			var count = obstacle.VertexCount;
			Assert.That ((int) count, Is.EqualTo (points.Length), "GKPolygonObstacle lengt should be equal");

			for (nuint i = 0; i < count; i++)
				Assert.That (obstacle.GetVertex (i), Is.EqualTo (points [(int) i]), "GKPolygonObstacle vectors should be equal");
		}
	}
}
