using System;
using System.IO;
using System.Diagnostics;
using System.Linq;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[Preserve (AllMembers = true)]
	public class MTL4RenderPassDescriptorTest {

		[SetUp]
		public void SamplePositions ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			using var obj = new MTL4RenderPassDescriptor ();
			Assert.That (obj.SamplePositions, Is.Null, "#0");

			obj.SamplePositions = new MTLSamplePosition [] {
				new MTLSamplePosition () { X = 1, Y = 2 },
			};
			Assert.That (obj.SamplePositions, Is.Not.Null, "#1a");
			Assert.That (obj.SamplePositions.Length, Is.EqualTo (1), "#1b");
			Assert.That (obj.SamplePositions [0].X, Is.EqualTo (1), "#1x");
			Assert.That (obj.SamplePositions [0].Y, Is.EqualTo (2), "#1y");

			obj.SamplePositions = null;
			Assert.That (obj.SamplePositions, Is.Null, "#2");

			obj.SamplePositions = new MTLSamplePosition [0];
			Assert.That (obj.SamplePositions, Is.Null, "#3");

			obj.SamplePositions = new MTLSamplePosition [] {
				new MTLSamplePosition () { X = 3, Y = 4 },
				new MTLSamplePosition () { X = 1, Y = 2 },
			};
			Assert.That (obj.SamplePositions, Is.Not.Null, "#4a");
			Assert.That (obj.SamplePositions.Length, Is.EqualTo (2), "#4b");
			Assert.That (obj.SamplePositions [0].X, Is.EqualTo (3), "#4x0");
			Assert.That (obj.SamplePositions [0].Y, Is.EqualTo (4), "#4y0");
			Assert.That (obj.SamplePositions [1].X, Is.EqualTo (1), "#4x1");
			Assert.That (obj.SamplePositions [1].Y, Is.EqualTo (2), "#4y1");

			var samplePositions = new MTLSamplePosition [129];
			samplePositions [0] = new MTLSamplePosition () { X = 1, Y = 2 };
			samplePositions [127] = new MTLSamplePosition () { X = 100, Y = 200 };
			samplePositions [128] = new MTLSamplePosition () { X = 101, Y = 201 };
			obj.SamplePositions = samplePositions;
			Assert.That (obj.SamplePositions, Is.Not.Null, "#5a");
			Assert.That (obj.SamplePositions.Length, Is.EqualTo (128), "#5b");
			Assert.That (obj.SamplePositions [0].X, Is.EqualTo (1), "#5x0");
			Assert.That (obj.SamplePositions [0].Y, Is.EqualTo (2), "#5y0");
			Assert.That (obj.SamplePositions [127].X, Is.EqualTo (100), "#5x127");
			Assert.That (obj.SamplePositions [127].Y, Is.EqualTo (200), "#5y127");

			var rv = new MTLSamplePosition [129];
			Assert.That (obj.GetSamplePositions (rv), Is.EqualTo (rv.Length), "#6a");
			Assert.That (rv [0].X, Is.EqualTo (1), "#6x0");
			Assert.That (rv [0].Y, Is.EqualTo (2), "#6y0");
			Assert.That (rv [127].X, Is.EqualTo (100), "#6x127");
			Assert.That (rv [127].Y, Is.EqualTo (200), "#6y127");
			Assert.That (rv [128].X, Is.EqualTo (101), "#6x128");
			Assert.That (rv [128].Y, Is.EqualTo (201), "#6y128");

			rv = new MTLSamplePosition [128];
			Assert.That (obj.GetSamplePositions (rv), Is.EqualTo (rv.Length), "#7a");
			Assert.That (rv [0].X, Is.EqualTo (1), "#7x0");
			Assert.That (rv [0].Y, Is.EqualTo (2), "#7y0");
			Assert.That (rv [127].X, Is.EqualTo (100), "#7x127");
			Assert.That (rv [127].Y, Is.EqualTo (200), "#7y127");

			rv = new MTLSamplePosition [3];
			Assert.That (obj.GetSamplePositions (rv), Is.EqualTo (rv.Length), "#8a");
			Assert.That (rv [0].X, Is.EqualTo (1), "#8x0");
			Assert.That (rv [0].Y, Is.EqualTo (2), "#8y0");
		}
	}
}
