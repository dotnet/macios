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
			ClassicAssert.AreEqual (default (CGComponent), c.DeepestImageComponent, "DeepestImageComponent");
			ClassicAssert.AreEqual (default (CGColorModel), c.ContentColorModels, "ContentColorModels");
			ClassicAssert.IsFalse (c.HasWideGamut, "HasWideGamut");
			ClassicAssert.IsFalse (c.HasTransparency, "HasTransparency");
			ClassicAssert.AreEqual (0f, c.LargestContentHeadroom, "LargestContentHeadroom");
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

			ClassicAssert.AreEqual ((CGComponent) 2, c.DeepestImageComponent, "DeepestImageComponent");
			ClassicAssert.AreEqual ((CGColorModel) 3, c.ContentColorModels, "ContentColorModels");
			ClassicAssert.IsTrue (c.HasWideGamut, "HasWideGamut");
			ClassicAssert.IsTrue (c.HasTransparency, "HasTransparency");
			ClassicAssert.AreEqual (1.25f, c.LargestContentHeadroom, "LargestContentHeadroom");
		}

		[Test]
		public void HasWideGamutFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasWideGamut = false;
			ClassicAssert.IsFalse (c.HasWideGamut, "HasWideGamut");
		}

		[Test]
		public void HasTransparencyFalseTest ()
		{
			var c = new CGContentInfo ();
			c.HasTransparency = false;
			ClassicAssert.IsFalse (c.HasTransparency, "HasTransparency");
		}
	}
}
