// Copyright 2011-2013 Xamarin Inc. All rights reserved

#if HAS_UIKIT || HAS_APPKIT

using System.Drawing;

#if HAS_APPKIT
using AppKit;
#endif
using CoreGraphics;
#if HAS_UIKIT
using UIKit;
#endif

namespace MonoTouchFixtures.XKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTextStorageTest {
		[Test]
		public void InitWithString ()
		{
			using var obj = new NSTextStorage ("Hello World");
		}
	}
}

#endif // HAS_UIKIT || HAS_APPKIT
