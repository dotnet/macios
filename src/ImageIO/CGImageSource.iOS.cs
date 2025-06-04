//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc
//

#nullable enable

#if !MONOMAC

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace ImageIO {

	/// <summary>Image Loader.</summary>
	///     <remarks>
	///     </remarks>
	public partial class CGImageSource {

		// CGImageSource.h
		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CGImageMetadataRef __nullable */ IntPtr CGImageSourceCopyMetadataAtIndex (
			/* CGImageSourceRef __nonnull */ IntPtr isrc, /* size_t */ nint idx,
			/* CFDictionaryRef __nullable */ IntPtr options);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public CGImageMetadata? CopyMetadata (nint index, NSDictionary? options)
		{
			var result = CGImageSourceCopyMetadataAtIndex (Handle, index, options.GetHandle ());
			GC.KeepAlive (options);
			return (result == IntPtr.Zero) ? null : new CGImageMetadata (result, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public CGImageMetadata? CopyMetadata (nint index, CGImageOptions? options)
		{
			using var o = options?.ToDictionary ();
			return CopyMetadata (index, o);
		}

		// CGImageSource.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageSourceRemoveCacheAtIndex (/* CGImageSourceRef __nonnull */ IntPtr isrc,
			/* size_t */ nint index);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public void RemoveCache (nint index)
		{
			CGImageSourceRemoveCacheAtIndex (Handle, index);
		}
	}
}

#endif // !MONOMAC
