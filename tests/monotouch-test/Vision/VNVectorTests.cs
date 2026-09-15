//
// Unit tests for VNVector
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
	public class VNVectorTests {

		[SetUp]
		public void Setup () => TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);

		[Test]
		public void VNVectorCreateTest ()
		{
			var vector = VNVector.Create (r: 10, theta: 0.5);
			Assert.That (vector, Is.Not.Null, "vector not null");
			Assert.That (vector.R, Is.EqualTo (10), "R");
			Assert.That (0.5, Is.EqualTo (vector.Theta), "Theta");
			Assert.That (vector.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}

		[Test]
		public void VNVectorCtorTest ()
		{
			using var vector = new VNVector ((R: 10, Theta: 0.5));
			Assert.That (vector, Is.Not.Null, "vector not null");
			Assert.That (vector.R, Is.EqualTo (10), "R");
			Assert.That (0.5, Is.EqualTo (vector.Theta), "Theta");
			Assert.That (vector.RetainCount, Is.EqualTo ((nuint) 1), "RetainCount");
		}
	}
}
