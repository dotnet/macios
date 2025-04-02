// 
// CoreMedia.cs: Basic definitions for CoreMedia
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012-2014 Xamarin Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreMedia {

	// CMSampleBuffer.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMSampleTimingInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTime Duration;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTime PresentationTimeStamp;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTime DecodeTimeStamp;
	}

	// CMTimeRange.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMTimeRange {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTime Start;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTime Duration;
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly CMTimeRange Zero;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly CMTimeRange InvalidRange;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static readonly CMTimeRange InvalidMapping;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static NSString? TimeMappingSourceKey { get; private set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static NSString? TimeMappingTargetKey { get; private set; }

		static CMTimeRange ()
		{
			var lib = Libraries.CoreMedia.Handle;
			var retZero = Dlfcn.dlsym (lib, "kCMTimeRangeZero");
			Zero = Marshal.PtrToStructure<CMTimeRange> (retZero)!;

			var retInvalid = Dlfcn.dlsym (lib, "kCMTimeRangeInvalid");
			InvalidRange = Marshal.PtrToStructure<CMTimeRange> (retInvalid)!;

			var retMappingInvalid = Dlfcn.dlsym (lib, "kCMTimeMappingInvalid");
			if (retMappingInvalid != IntPtr.Zero)
				InvalidMapping = Marshal.PtrToStructure<CMTimeRange> (retMappingInvalid)!;

			TimeMappingSourceKey = Dlfcn.GetStringConstant (lib, "kCMTimeMappingSourceKey");
			TimeMappingTargetKey = Dlfcn.GetStringConstant (lib, "kCMTimeMappingTargetKey");
		}
#endif // !COREBUILD
	}

	// CMTimeRange.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMTimeMapping {
		/// <summary>The source time range.</summary>
		///         <remarks>To be added.</remarks>
		public CMTimeRange Source;
		/// <summary>The target time range.</summary>
		///         <remarks>To be added.</remarks>
		public CMTimeRange Target;

#if !COREBUILD
		/// <param name="source">To be added.</param>
		///         <param name="target">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMTimeMapping Create (CMTimeRange source, CMTimeRange target)
		{
			return CMTimeMappingMake (source, target);
		}

		/// <param name="target">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMTimeMapping CreateEmpty (CMTimeRange target)
		{
			return CMTimeMappingMakeEmpty (target);
		}

		/// <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMTimeMapping CreateFromDictionary (NSDictionary dict)
		{
			CMTimeMapping result = CMTimeMappingMakeFromDictionary (dict.Handle);
			GC.KeepAlive (dict);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSDictionary AsDictionary ()
		{
			return new NSDictionary (CMTimeMappingCopyAsDictionary (this, IntPtr.Zero), true);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? Description {
			get {
				return CFString.FromHandle (CMTimeMappingCopyDescription (IntPtr.Zero, this), true);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern CMTimeMapping CMTimeMappingMake (CMTimeRange source, CMTimeRange target);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern CMTimeMapping CMTimeMappingMakeEmpty (CMTimeRange target);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern IntPtr /* CFDictionaryRef* */ CMTimeMappingCopyAsDictionary (CMTimeMapping mapping, IntPtr allocator);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern CMTimeMapping CMTimeMappingMakeFromDictionary (/* CFDictionaryRef* */ IntPtr dict);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern IntPtr /* CFStringRef* */ CMTimeMappingCopyDescription (IntPtr allocator, CMTimeMapping mapping);
#endif // !COREBUILD
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMTimeScale {
		// CMTime.h
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly CMTimeScale MaxValue = new CMTimeScale (0x7fffffff);

		// int32_t -> CMTime.h
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int Value;

		/// <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTimeScale (int value)
		{
			if (value < 0 || value > 0x7fffffff)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Between 0 and 0x7fffffff");

			this.Value = value;
		}
	}

	// CMVideoDimensions => int32_t width + int32_t height
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CMVideoDimensions {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int Height;

		/// <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMVideoDimensions (int width, int height)
		{
			Width = width;
			Height = height;
		}
	}
}
