// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace QuartzComposer;

partial class QCComposition {
#if !XAMCORE_5_0
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
	[SupportedOSPlatform ("macos")]
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This field is always null.")]
	public static NSString? InputRSSArticleDurationKey {
		[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
		[SupportedOSPlatform ("macos")]
		get => null;
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
	[SupportedOSPlatform ("macos")]
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This field is always null.")]
	public static NSString? InputRSSFeedURLKey {
		[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
		[SupportedOSPlatform ("macos")]
		get => null;
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
	[SupportedOSPlatform ("macos")]
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This field is always null.")]
	public static NSString? ProtocolRSSVisualizer {
		[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
		[SupportedOSPlatform ("macos")]
		get => null;
	}
#endif // !XAMCORE_5_0
}
