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
			ClassicAssert.AreEqual (size.Width, strongDict.Width, "Width");
			ClassicAssert.AreEqual (size.Height, strongDict.Height, "Height");

			var size2 = strongDict.ToSize ();
			ClassicAssert.AreEqual (size, size2, "Size");

			strongDict = new CGSizeDictionary ();
			strongDict.Width = 3;
			ClassicAssert.AreEqual ((nfloat) 3, strongDict.Width, "Width 2");
			strongDict.Height = 4;
			ClassicAssert.AreEqual ((nfloat) 4, strongDict.Height, "Height 2");
			size2 = strongDict.ToSize ();
			ClassicAssert.AreEqual (new CGSize (3, 4), size2, "Size 2");
		});
	}

	[Test]
	public void Default ()
	{
		Assert.Multiple (() => {
			var strongDict = new CGSizeDictionary ();
			ClassicAssert.IsNull (strongDict.Width, "Width");
			ClassicAssert.IsNull (strongDict.Height, "Height");
			var size = strongDict.ToSize ();
			ClassicAssert.AreEqual (default (CGSize), size, "Size");
		});
	}

	[Test]
	public void ToStringTest1 ()
	{
		var strongDict = new CGSizeDictionary ();
		ClassicAssert.AreEqual ("CoreGraphics.CGSizeDictionary", strongDict.ToString (), "A");
		ClassicAssert.AreEqual ("{\n}", strongDict.Dictionary.ToString (), "B");
	}

	[Test]
	public void ToStringTest2 ()
	{
		var strongDict = new CGSizeDictionary ();
		strongDict.Width = 3;
		strongDict.Height = 4;
		ClassicAssert.AreEqual ("CoreGraphics.CGSizeDictionary", strongDict.ToString (), "A");
		ClassicAssert.AreEqual ("{\n    Height = 4;\n    Width = 3;\n}", strongDict.Dictionary.ToString (), "B");
	}
}
