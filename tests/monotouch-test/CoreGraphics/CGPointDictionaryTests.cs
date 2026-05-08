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
			ClassicAssert.AreEqual (point.X, strongDict.X, "X");
			ClassicAssert.AreEqual (point.Y, strongDict.Y, "Y");

			var point2 = strongDict.ToPoint ();
			ClassicAssert.AreEqual (point, point2, "Point");

			strongDict = new CGPointDictionary ();
			strongDict.X = 3;
			ClassicAssert.AreEqual ((nfloat) 3, strongDict.X, "X 2");
			strongDict.Y = 4;
			ClassicAssert.AreEqual ((nfloat) 4, strongDict.Y, "Y 2");
			point2 = strongDict.ToPoint ();
			ClassicAssert.AreEqual (new CGPoint (3, 4), point2, "Point 2");
		});
	}

	[Test]
	public void Default ()
	{
		var strongDict = new CGPointDictionary ();
		ClassicAssert.IsNull (strongDict.X, "X");
		ClassicAssert.IsNull (strongDict.Y, "Y");
		var point = strongDict.ToPoint ();
		ClassicAssert.AreEqual (default (CGPoint), point, "Point");
	}

	[Test]
	public void ToStringTest1 ()
	{
		var strongDict = new CGPointDictionary ();
		ClassicAssert.AreEqual ("CoreGraphics.CGPointDictionary", strongDict.ToString (), "A");
		ClassicAssert.AreEqual ("{\n}", strongDict.Dictionary.ToString (), "B");
	}

	[Test]
	public void ToStringTest2 ()
	{
		var strongDict = new CGPointDictionary ();
		strongDict.X = 3;
		strongDict.Y = 4;
		ClassicAssert.AreEqual ("CoreGraphics.CGPointDictionary", strongDict.ToString (), "A");
		ClassicAssert.AreEqual ("{\n    X = 3;\n    Y = 4;\n}", strongDict.Dictionary.ToString (), "B");
	}
}
