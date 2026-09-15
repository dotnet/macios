// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using CoreGraphics;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics;

[TestFixture]
[Preserve (AllMembers = true)]
public class CGSizeDictionaryTests {

	[Test]
	public void PropertiesTest ()
	{
		Assert.Multiple (() => {
			var size = new CGSize ((nfloat) 1, (nfloat) 2);
			using var dict = size.ToDictionary ();
			var strongDict = new CGSizeDictionary (dict);
			Assert.That (strongDict.Width, Is.EqualTo (size.Width), "Width");
			Assert.That (strongDict.Height, Is.EqualTo (size.Height), "Height");

			var size2 = strongDict.ToSize ();
			Assert.That (size2, Is.EqualTo (size), "Size");

			strongDict = new CGSizeDictionary ();
			strongDict.Width = 3;
			Assert.That (strongDict.Width, Is.EqualTo ((nfloat) 3), "Width 2");
			strongDict.Height = 4;
			Assert.That (strongDict.Height, Is.EqualTo ((nfloat) 4), "Height 2");
			size2 = strongDict.ToSize ();
			Assert.That (size2, Is.EqualTo (new CGSize (3, 4)), "Size 2");
		});
	}

	[Test]
	public void Default ()
	{
		Assert.Multiple (() => {
			var strongDict = new CGSizeDictionary ();
			Assert.That (strongDict.Width, Is.Null, "Width");
			Assert.That (strongDict.Height, Is.Null, "Height");
			var size = strongDict.ToSize ();
			Assert.That (size, Is.EqualTo (default (CGSize)), "Size");
		});
	}

	[Test]
	public void ToStringTest1 ()
	{
		var strongDict = new CGSizeDictionary ();
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGSizeDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "B");
	}

	[Test]
	public void ToStringTest2 ()
	{
		var strongDict = new CGSizeDictionary ();
		strongDict.Width = 3;
		strongDict.Height = 4;
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGSizeDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n    Height = 4;\n    Width = 3;\n}"), "B");
	}
}
