// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using ObjCRuntime;
using CoreGraphics;

using NUnit.Framework;

#if __MONOMAC__ || __MACCATALYST__

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGDisplayTest {

		[Test]
		public void GetRefreshRate ()
		{
			Assert.That (CGDisplay.GetRefreshRate (CGDisplay.MainDisplayID), Is.Not.EqualTo (0), "RefreshRate");
		}
	}
}

#endif // __MONOMAC__ || __MACCATALYST__
