// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

using Foundation;
using CoreGraphics;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGToneMappingOptionsTest {
		[Test]
		public void DefaultOptions ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var defaultOptions = CGToneMappingOptions.GetDefaultExrToneMappingGammaOptions ();
			Assert.That (defaultOptions, Is.Not.Null, "Default");
		}
	}
}
