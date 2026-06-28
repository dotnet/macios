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
public class CGPointDictionaryTests {

	[Test]
	public void PropertiesTest ()
	{
		Assert.Multiple (() => {
			var point = new CGPoint ((nfloat) 1, (nfloat) 2);
			using var dict = point.ToDictionary ();
			var strongDict = new CGPointDictionary (dict);
			Assert.That (strongDict.X, Is.EqualTo (point.X), "X");
			Assert.That (strongDict.Y, Is.EqualTo (point.Y), "Y");

			var point2 = strongDict.ToPoint ();
			Assert.That (point2, Is.EqualTo (point), "Point");

			strongDict = new CGPointDictionary ();
			strongDict.X = 3;
			Assert.That (strongDict.X, Is.EqualTo ((nfloat) 3), "X 2");
			strongDict.Y = 4;
			Assert.That (strongDict.Y, Is.EqualTo ((nfloat) 4), "Y 2");
			point2 = strongDict.ToPoint ();
			Assert.That (point2, Is.EqualTo (new CGPoint (3, 4)), "Point 2");
		});
	}

	[Test]
	public void Default ()
	{
		var strongDict = new CGPointDictionary ();
		Assert.That (strongDict.X, Is.Null, "X");
		Assert.That (strongDict.Y, Is.Null, "Y");
		var point = strongDict.ToPoint ();
		Assert.That (point, Is.EqualTo (default (CGPoint)), "Point");
	}

	[Test]
	public void ToStringTest1 ()
	{
		var strongDict = new CGPointDictionary ();
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGPointDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "B");
	}

	[Test]
	public void ToStringTest2 ()
	{
		var strongDict = new CGPointDictionary ();
		strongDict.X = 3;
		strongDict.Y = 4;
		Assert.That (strongDict.ToString (), Is.EqualTo ("CoreGraphics.CGPointDictionary"), "A");
		Assert.That (strongDict.Dictionary.ToString (), Is.EqualTo ("{\n    X = 3;\n    Y = 4;\n}"), "B");
	}
}
