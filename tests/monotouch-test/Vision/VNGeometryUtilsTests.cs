//
// Unit tests for VNGeometryUtils
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

using System.Collections;

using CoreGraphics;
using Vision;

using System.Numerics;

namespace MonoTouchFixtures.Vision {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VNGeometryUtilsTests {

		[SetUp]
		public void Setup () => TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);

		[Test]
		public void CreateBoundingCircleTest ()
		{
			var nvectors = new [] {
				new VNPoint (1,1),
				new VNPoint (-1,1),
				new VNPoint (-1,-1),
				new VNPoint (1,-1),
			};

			var ncircle = VNGeometryUtils.CreateBoundingCircle (nvectors, out var nerror);
			Assert.That (nerror, Is.Null, "nerror was not null");
			Assert.That (ncircle, Is.Not.Null, "ncircle was null");

			var vectors = new [] {
				new Vector2 (1,1),
				new Vector2 (-1,1),
				new Vector2 (-1,-1),
				new Vector2 (1,-1),
			};

			var circle = VNGeometryUtils.CreateBoundingCircle (vectors, out var error);
			Assert.That (error, Is.Null, "Error was not null");
			Assert.That (circle, Is.Not.Null, "circle was null");

			Assert.That (circle.Diameter, Is.EqualTo (ncircle.Diameter), "Diameter");
			Assert.That (circle.Radius, Is.EqualTo (ncircle.Radius), "Radius");
		}
	}
}
