//
// VTUtilities.cs
//
// Authors: 
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using CoreGraphics;
using CoreMedia;
using CoreVideo;

#nullable enable

namespace VideoToolbox {
	/// <summary>Extensions class for <see cref="CoreVideo.CVPixelBuffer" />.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class VTUtilities {
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTCreateCGImageFromCVPixelBuffer (
			/* CM_NONNULL CVPixelBufferRef */ IntPtr pixelBuffer,
			/* CM_NULLABLE CFDictionaryRef */ IntPtr options,
			/* CM_RETURNS_RETAINED_PARAMETER CM_NULLABLE CGImageRef * CM_NONNULL */ IntPtr* imageOut);

		// intentionally not exposing the (NSDictionary options) argument
		// since header docs indicate that there are no options available
		// as of 9.0/10.11 and to always pass NULL
		/// <summary>Creates a Core Graphics image from a pixel buffer.</summary>
		/// <param name="pixelBuffer">The pixel buffer to convert.</param>
		/// <param name="image">On success, the created image; otherwise, <see langword="null" />.</param>
		/// <returns>A status code that indicates whether the conversion succeeded.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="pixelBuffer" /> is <see langword="null" />.</exception>
		public static VTStatus ToCGImage (this CVPixelBuffer pixelBuffer, out CGImage? image)
		{
			if (pixelBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (pixelBuffer));

			VTStatus ret;
			IntPtr imagePtr;
			unsafe {
				ret = VTCreateCGImageFromCVPixelBuffer (pixelBuffer.GetCheckedHandle (),
				IntPtr.Zero, // no options as of 9.0/10.11 - always pass NULL
				&imagePtr);
				GC.KeepAlive (pixelBuffer);
			}

			image = Runtime.GetINativeObject<CGImage> (imagePtr, true); // This is already retained CM_RETURNS_RETAINED_PARAMETER

			return ret;
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[DllImport (Constants.VideoToolboxLibrary)]
		static extern void VTRegisterSupplementalVideoDecoderIfAvailable (uint codecType);

		/// <summary>Registers the supplemental video decoder for a codec if one is available.</summary>
		/// <param name="codecType">The video codec for which to register a supplemental decoder.</param>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		public static void RegisterSupplementalVideoDecoder (CMVideoCodecType codecType)
			=> VTRegisterSupplementalVideoDecoderIfAvailable ((uint) codecType);

#if __MACOS__
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos15.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe static extern VTStatus VTCopyVideoDecoderExtensionProperties (
			IntPtr /* CMFormatDescriptionRef CM_NONNULL */ formatDesc,
			IntPtr* /* CM_RETURNS_RETAINED_PARAMETER CM_NULLABLE CFDictionaryRef * CM_NONNULL */ mediaExtensionPropertiesOut
		);

		/// <summary>Determine whether a Media Extension video decoder will be used to decode the specified format, and return information about the Media Extension.</summary>
		/// <param name="formatDescription">The format description for the video format to analyze.</param>
		/// <param name="error">An error code if the operation was unsuccessful, otherwise <see cref="VTStatus.Ok" />. If a Media Extension encoder won't be used to decode this format, <see cref="VTStatus.CouldNotFindExtensionErr" /> will be returned.</param>
		/// <returns>A dictionary with the properties for the Media Extension that will be used to decode this format, or null in case of failure.</returns>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos15.0")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSDictionary? CopyVideoDecoderExtensionProperties (CMFormatDescription formatDescription, out VTStatus error)
		{
			IntPtr handle;
			unsafe {
				error = VTCopyVideoDecoderExtensionProperties (formatDescription.GetNonNullHandle (nameof (formatDescription)), &handle);
				GC.KeepAlive (formatDescription);
			}
			return Runtime.GetNSObject<NSDictionary> (handle, owns: true);
		}
#endif // __MACOS__

#if __MACOS__
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos15.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe static extern VTStatus VTCopyRAWProcessorExtensionProperties (
			IntPtr /* CMFormatDescriptionRef CM_NONNULL */ formatDesc,
			IntPtr* /* CM_RETURNS_RETAINED_PARAMETER CM_NULLABLE CFDictionaryRef * CM_NONNULL */ mediaExtensionPropertiesOut
		);

		/// <summary>Determine whether a Media Extension RAW processor will be used to process the specified format, and return information about the Media Extension.</summary>
		/// <param name="formatDescription">The format description for the video format to analyze.</param>
		/// <param name="error">An error code if the operation was unsuccessful, otherwise <see cref="VTStatus.Ok" />. If a Media Extension RAW processor won't be used to decode this format, <see cref="VTStatus.CouldNotFindExtensionErr" /> will be returned.</param>
		/// <returns>A dictionary with the properties for the Media Extension RAW processor that will be used to decode this format, or null in case of failure.</returns>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos15.0")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSDictionary? CopyRawProcessorExtensionProperties (CMFormatDescription formatDescription, out VTStatus error)
		{
			IntPtr handle;
			unsafe {
				error = VTCopyRAWProcessorExtensionProperties (formatDescription.GetNonNullHandle (nameof (formatDescription)), &handle);
				GC.KeepAlive (formatDescription);
			}
			return Runtime.GetNSObject<NSDictionary> (handle, owns: true);
		}
#endif // __MACOS__

	}
}
