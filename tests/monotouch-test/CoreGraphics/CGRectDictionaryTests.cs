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
public class CGRectDictionaryTests {

	[Test]
	public void PropertiesTest ()
	{
		Assert.Multiple (() => {
			var rect = new CGRect (1, 2, 3, 4);
			using var dict = rect.ToDictionary ();
			var strongDict = new CGRectDictionary (dict);
			Assert.That (strongDict.X, Is.EqualTo (rect.X), "X");
			Assert.That (strongDict.Y, Is.EqualTo (rect.Y), "Y");
			Assert.That (strongDict.Height, Is.EqualTo (rect.Height), "Height");
			Assert.That (strongDict.Width, Is.EqualTo (rect.Width), "Width");

			var rect2 = strongDict.ToRect ();
			Assert.That (rect2, Is.EqualTo (rect), "Rect");

			strongDict = new CGRectDictionary ();
			strongDict.X = 3;
			Assert.That (strongDict.X, Is.EqualTo ((nfloat) 3), "X 2");
			strongDict.Y = 4;
			Assert.That (strongDict.Y, Is.EqualTo ((nfloat) 4), "Y 2");
			strongDict.Width = 5;
			Assert.That (strongDict.Width, Is.EqualTo ((nfloat) 5), "Width 2");
			strongDict.Height = 6;
			Assert.That (strongDict.Height, Is.EqualTo ((nfloat) 6), "Height 2");
			rect2 = strongDict.ToRect ();
			Assert.That (rect2, Is.EqualTo (new CGRect (3, 4, 5, 6)), "Rect 2");
		});
	}

	[Test]
	public void Default ()
	{
		Assert.Multiple (() => {
			var strongDict = new CGRectDictionary ();
			Assert.That (strongDict.X, Is.Null, "X");
			Assert.That (strongDict.Y, Is.Null, "Y");
			Assert.That (strongDict.Width, Is.Null, "Width");
			Assert.That (strongDict.Height, Is.Null, "Height");
			var rect = strongDict.ToRect ();
			Assert.That (rect, Is.EqualTo (default (CGRect)), "Rect");
		});
	}

	[Test]
	public void ToStringTest1 ()
	{
		var strongDict = new CGRectDictionary ();
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGRectDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "B");
	}

	[Test]
	public void ToStringTest2 ()
	{
		var strongDict = new CGRectDictionary ();
		strongDict.X = 3;
		strongDict.Y = 4;
		strongDict.Width = 5;
		strongDict.Height = 6;
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGRectDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n    Height = 6;\n    Width = 5;\n    X = 3;\n    Y = 4;\n}"), "B");
	}
}
