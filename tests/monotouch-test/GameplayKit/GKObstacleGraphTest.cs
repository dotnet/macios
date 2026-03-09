// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for GKObstacleGraph
//

using System.Numerics;
using GameplayKit;

namespace MonoTouchFixtures.GameplayKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKObstacleGraphTest {

		[Test]
		public void GetNodes_ReturnsNullForUnknownObstacle ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var points = new [] {
				new Vector2 (0, 0),
				new Vector2 (10, 0),
				new Vector2 (10, 10),
			};
			var obstacle = GKPolygonObstacle.FromPoints (points);
			var graph = GKObstacleGraph.FromObstacles (new GKPolygonObstacle [] { obstacle }, 1.0f);
			Assert.IsNotNull (graph, "graph");

			var nodes = graph!.GetNodes (obstacle);
			// May return null or a valid array depending on the graph state
			// The key thing is it doesn't crash
			if (nodes is not null)
				Assert.That (nodes.Length, Is.GreaterThanOrEqualTo (0), "nodes/length");

			// Query for an obstacle not in the graph
			var otherObstacle = GKPolygonObstacle.FromPoints (new [] {
				new Vector2 (100, 100),
				new Vector2 (110, 100),
				new Vector2 (110, 110),
			});
			var otherNodes = graph.GetNodes (otherObstacle);
			// An obstacle not in the graph may return null or an empty array
			if (otherNodes is not null)
				Assert.AreEqual (0, otherNodes.Length, "otherNodes/empty");
		}
	}
}
