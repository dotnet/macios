#if !__MACCATALYST__ // there's a version in UIKit, use that one instead
using System;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	public partial class NSTextContainer {
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos", "Use 'new NSTextContainer (CGSize)' instead.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		internal NSTextContainer (CGSize size, bool isContainer)
			: base (NSObjectFlag.Empty)
		{
			if (isContainer) {
				InitializeHandle (_InitWithContainerSize (size), "initWithContainerSize:");
			} else {
				InitializeHandle (_InitWithSize (size), "initWithSize:");
			}
		}

		/// <summary>Create a new <see cref="NSTextContainer" /> with the specified size.</summary>
		/// <param name="size">The size of the new <see cref="NSTextContainer" />.</param>
		/// <returns>A new <see cref="NSTextContainer" /> with the specified size.</returns>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSTextContainer FromSize (CGSize size)
		{
			return new NSTextContainer (size);
		}

		/// <summary>Create a new <see cref="NSTextContainer" /> with the specified size.</summary>
		/// <param name="containerSize">The size of the new <see cref="NSTextContainer" />.</param>
		/// <returns>A new <see cref="NSTextContainer" /> with the specified size.</returns>
		/// <remarks>This method is deprecated, use <see cref="FromSize" /> instead.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos", "Use 'new NSTextContainer (CGSize)' instead.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSTextContainer FromContainerSize (CGSize containerSize)
		{
			return new NSTextContainer (containerSize, true);
		}
	}
}
#endif // !__MACCATALYST__
