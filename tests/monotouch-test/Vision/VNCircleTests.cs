//
// Unit tests for VNRequestTests
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

using System.Collections;

using CoreGraphics;
using Vision;

namespace MonoTouchFixtures.Vision {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VNCircleTests {

		[SetUp]
		public void Setup () => TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);

		[Test]
		public void CreateUsingRadiusTest ()
		{
			var circle = VNCircle.CreateUsingRadius (new VNPoint (10, 10), radius: 10);
			ClassicAssert.NotNull (circle, "Circle not null");
			ClassicAssert.AreEqual (circle.Radius, 10, "Radius");
			ClassicAssert.AreEqual (circle.Center.X, 10, "X");
			ClassicAssert.AreEqual (circle.Center.Y, 10, "Y");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingDiameterTest ()
		{
			var circle = VNCircle.CreateUsingDiameter (new VNPoint (5, 6), diameter: 7);
			ClassicAssert.NotNull (circle, "Circle not null");
			ClassicAssert.AreEqual (circle.Diameter, 7, "Diameter");
			ClassicAssert.AreEqual (circle.Center.Y, 6, "Y");
			ClassicAssert.AreEqual (circle.Center.X, 5, "X");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingRadiusCtorTest ()
		{
			using var circle = new VNCircle (new VNPoint (10, 10), radiusOrDiameter: 10, option: VNCircleInitializationOption.Radius);
			ClassicAssert.NotNull (circle, "Circle not null");
			ClassicAssert.AreEqual (circle.Radius, 10, "Radius");
			ClassicAssert.AreEqual (circle.Center.X, 10, "X");
			ClassicAssert.AreEqual (circle.Center.Y, 10, "Y");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingDiameterCtorTest ()
		{
			using var circle = new VNCircle (new VNPoint (5, 6), radiusOrDiameter: 7, option: VNCircleInitializationOption.Diameter);
			ClassicAssert.NotNull (circle, "Circle not null");
			ClassicAssert.AreEqual (circle.Diameter, 7, "Diameter");
			ClassicAssert.AreEqual (circle.Center.Y, 6, "Y");
			ClassicAssert.AreEqual (circle.Center.X, 5, "X");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingInvalidOptionCtorTest ()
		{
			Assert.Throws<ArgumentOutOfRangeException> (() => {
				using (var circle = new VNCircle (new VNPoint (5, 6), radiusOrDiameter: 7, option: (VNCircleInitializationOption) (-1))) {
				}
			});
		}
	}
}
