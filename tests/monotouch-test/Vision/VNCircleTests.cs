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
			Assert.That (circle, Is.Not.Null, "Circle not null");
			Assert.That (10, Is.EqualTo (circle.Radius), "Radius");
			Assert.That (10, Is.EqualTo (circle.Center.X), "X");
			Assert.That (10, Is.EqualTo (circle.Center.Y), "Y");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingDiameterTest ()
		{
			var circle = VNCircle.CreateUsingDiameter (new VNPoint (5, 6), diameter: 7);
			Assert.That (circle, Is.Not.Null, "Circle not null");
			Assert.That (7, Is.EqualTo (circle.Diameter), "Diameter");
			Assert.That (6, Is.EqualTo (circle.Center.Y), "Y");
			Assert.That (5, Is.EqualTo (circle.Center.X), "X");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingRadiusCtorTest ()
		{
			using var circle = new VNCircle (new VNPoint (10, 10), radiusOrDiameter: 10, option: VNCircleInitializationOption.Radius);
			Assert.That (circle, Is.Not.Null, "Circle not null");
			Assert.That (10, Is.EqualTo (circle.Radius), "Radius");
			Assert.That (10, Is.EqualTo (circle.Center.X), "X");
			Assert.That (10, Is.EqualTo (circle.Center.Y), "Y");
			Assert.That (circle.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void CreateUsingDiameterCtorTest ()
		{
			using var circle = new VNCircle (new VNPoint (5, 6), radiusOrDiameter: 7, option: VNCircleInitializationOption.Diameter);
			Assert.That (circle, Is.Not.Null, "Circle not null");
			Assert.That (7, Is.EqualTo (circle.Diameter), "Diameter");
			Assert.That (6, Is.EqualTo (circle.Center.Y), "Y");
			Assert.That (5, Is.EqualTo (circle.Center.X), "X");
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
