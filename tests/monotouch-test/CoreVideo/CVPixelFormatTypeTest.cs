// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Drawing;
using Foundation;
using ObjCRuntime;
using CoreVideo;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreVideo;

[TestFixture]
[Preserve (AllMembers = true)]
public class CVPixelFormatTypeTest {
	[Test]
	public void Extensions ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);

		Assert.Multiple (() => {
			Assert.That (CVPixelFormatType.CV32BGRA.ToFourCharCodeString (), Is.EqualTo ("BGRA"));
			Assert.That (CVPixelFormatType.CV1Monochrome.ToFourCharCodeString (), Is.EqualTo ("1"));
			Assert.That (CVPixelFormatType.CV24RGB.ToFourCharCodeString (), Is.EqualTo ("24"));
		});
	}
}
