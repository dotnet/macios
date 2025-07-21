//
// CPCompat.cs
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

#nullable enable

using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System.ComponentModel;

namespace CarPlay {
#if !XAMCORE_5_0 && __IOS__
	public partial class CPListItem {
		/// <summary>Gets the maximum dimensions allowable for the <see cref="CarPlay.CPListItem.Image" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[ObsoletedOSPlatform ("ios14.0", "Do not use; this API was removed.")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static CGSize MaximumListItemImageSize {
			get {
				return default (CGSize);
			}
		}
	}
#endif // !XAMCORE_5_0 && __IOS__
}
