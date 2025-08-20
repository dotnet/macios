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
			Assert.AreEqual (default (CGComponent), c.DeepestImageComponent, "DeepestImageComponent");
			Assert.AreEqual (default (CGColorModel), c.ContentColorModels, "ContentColorModels");
			Assert.IsFalse (c.HasWideGamut, "HasWideGamut");
			Assert.IsFalse (c.HasTransparency, "HasTransparency");
			Assert.AreEqual (0f, c.LargestContentHeadroom, "LargestContentHeadroom");
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

			Assert.AreEqual ((CGComponent) 2, c.DeepestImageComponent, "DeepestImageComponent");
			Assert.AreEqual ((CGColorModel) 3, c.ContentColorModels, "ContentColorModels");
			Assert.IsTrue (c.HasWideGamut, "HasWideGamut");
			Assert.IsTrue (c.HasTransparency, "HasTransparency");
			Assert.AreEqual (1.25f, c.LargestContentHeadroom, "LargestContentHeadroom");
		}

		[Test]
		public void HasWideGamutFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasWideGamut = false;
			Assert.IsFalse (c.HasWideGamut, "HasWideGamut");
		}

		[Test]
		public void HasTransparencyFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasTransparency = false;
			Assert.IsFalse (c.HasTransparency, "HasTransparency");
		}
	}
}
