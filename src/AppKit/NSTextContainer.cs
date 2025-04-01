#if !__MACCATALYST__ // there's a version in UIKit, use that one instead
using System;
using CoreGraphics;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	public partial class NSTextContainer {
#if !NET
		[Obsoleted (PlatformName.MacOSX, 10, 11, message: "Use NSTextContainer.FromSize instead.")]
		public NSTextContainer (CGSize size)
		{
			Handle = InitWithContainerSize (size);
		}
#endif // !NET

		internal NSTextContainer (CGSize size, bool isContainer)
		{
			if (isContainer)
				Handle = InitWithContainerSize (size);
			else
				Handle = InitWithSize (size);
		}

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSTextContainer FromSize (CGSize size)
		{
			return new NSTextContainer (size, false);
		}

		[UnsupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos", "Use NSTextContainer.FromSize instead.")]
		public static NSTextContainer FromContainerSize (CGSize containerSize)
		{
			return new NSTextContainer (containerSize, true);
		}
	}
}
#endif // !__MACCATALYST__
