// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;

#if __MACOS__ && !XAMCORE_5_0

namespace AppKit {
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
	[Obsolete ("This class does not form part of the public API in macOS, and will be removed in the future.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public partial class NSPrintPreviewGraphicsContext : NSGraphicsContext {

		[EditorBrowsable (EditorBrowsableState.Never)]
		public override NativeHandle ClassHandle { get { return default; } }

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected NSPrintPreviewGraphicsContext (NSObjectFlag t) : base (t)
		{
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected internal NSPrintPreviewGraphicsContext (NativeHandle handle) : base (handle)
		{
		}
	} /* class NSPrintPreviewGraphicsContext */
}

#endif // __MACOS__ && !XAMCORE_5_0
