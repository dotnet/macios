// Copyright 2015 Xamarin Inc.

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using CoreMedia;
using ObjCRuntime;

namespace MediaToolbox {

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	static public class MTFormatNames {

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.MediaToolboxLibrary)]
		static extern /* CFStringRef CM_NULLABLE */ IntPtr MTCopyLocalizedNameForMediaType (
			CMMediaType mediaType);

#if NET
		/// <param name="mediaType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		static public string? GetLocalizedName (this CMMediaType mediaType)
		{
			return CFString.FromHandle (MTCopyLocalizedNameForMediaType (mediaType), releaseHandle: true);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.MediaToolboxLibrary)]
		static extern /* CFStringRef CM_NULLABLE */ IntPtr MTCopyLocalizedNameForMediaSubType (
			CMMediaType mediaType, uint mediaSubType);

#if NET
		/// <param name="mediaType">To be added.</param>
		///         <param name="mediaSubType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		static public string? GetLocalizedName (this CMMediaType mediaType, uint mediaSubType)
		{
			return CFString.FromHandle (MTCopyLocalizedNameForMediaSubType (mediaType, mediaSubType), releaseHandle: true);
		}
	}
}
