// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;
using CoreGraphics;
using NUnit.Framework;
using ObjCRuntime;

namespace MonoTouchFixtures.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGContentInfoTest {
		[Test]
		public void DefaultValuesTest ()
		{
			var c = new CGContentInfo ();
			Assert.That (c.DeepestImageComponent, Is.EqualTo (default (CGComponent)), "DeepestImageComponent");
			Assert.That (c.ContentColorModels, Is.EqualTo (default (CGColorModel)), "ContentColorModels");
			Assert.That (c.HasWideGamut, Is.False, "HasWideGamut");
			Assert.That (c.HasTransparency, Is.False, "HasTransparency");
			Assert.That (c.LargestContentHeadroom, Is.EqualTo (0f), "LargestContentHeadroom");
		}

		[Test]
		public void PropertySetGetTest ()
		{
			var c = new CGContentInfo ();
			c.DeepestImageComponent = (CGComponent) 2;
			c.ContentColorModels = (CGColorModel) 3;
			c.HasWideGamut = true;
			c.HasTransparency = true;
			c.LargestContentHeadroom = 1.25f;

			Assert.That (c.DeepestImageComponent, Is.EqualTo ((CGComponent) 2), "DeepestImageComponent");
			Assert.That (c.ContentColorModels, Is.EqualTo ((CGColorModel) 3), "ContentColorModels");
			Assert.That (c.HasWideGamut, Is.True, "HasWideGamut");
			Assert.That (c.HasTransparency, Is.True, "HasTransparency");
			Assert.That (c.LargestContentHeadroom, Is.EqualTo (1.25f), "LargestContentHeadroom");
		}

		[Test]
		public void HasWideGamutFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasWideGamut = false;
			Assert.That (c.HasWideGamut, Is.False, "HasWideGamut");
		}

		[Test]
		public void HasTransparencyFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasTransparency = false;
			Assert.That (c.HasTransparency, Is.False, "HasTransparency");
		}
	}
}
