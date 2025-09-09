// 
// CoreVideo.cs
//
// Authors: Mono Team
//     
// Copyright 2011 Novell, Inc
// Copyright 2011-2014 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreVideo {

	// CVPixelBuffer.h
	/// <summary>A struct that describes planar components.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVPlanarComponentInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public /* int32_t */ int Offset;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public /* uint32_t */ uint RowBytes;
	}

	// CVPixelBuffer.h
	/// <summary>A struct that holds the <see cref="CoreVideo.CVPlanarComponentInfo" />s of a planar buffer.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVPlanarPixelBufferInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo [] ComponentInfo;
	}

	// CVPixelBuffer.h
	/// <summary>A struct that defines the <see cref="CoreVideo.CVPlanarComponentInfo" />s of a YCbCr planar buffer.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVPlanarPixelBufferInfo_YCbCrPlanar {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo ComponentInfoY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo ComponentInfoCb;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo ComponentInfoCr;
	}

	/// <summary>Implements a YCbCr biplanar buffer description.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVPlanarPixelBufferInfo_YCbCrBiPlanar {
		/// <summary>A descriptor for the Y buffer.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo ComponentInfoY;
		/// <summary>A descriptor for the Cb/CR buffer.</summary>
		///         <remarks>To be added.</remarks>
		public CVPlanarComponentInfo ComponentInfoCbCr;
	}

	/// <summary>A struct that describes a display timestamp.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVTimeStamp {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt32 Version;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int32 VideoTimeScale;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int64 VideoTime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt64 HostTime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double RateScalar;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int64 VideoRefreshPeriod;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVSMPTETime SMPTETime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt64 Flags;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt64 Reserved;
	}

	/// <summary>Encodes an SMPTE timestamp.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVSMPTETime {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 Subframes;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 SubframeDivisor;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt32 Counter;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt32 Type;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UInt32 Flags;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 Hours;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 Minutes;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 Seconds;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Int16 Frames;
	}

#if !XAMCORE_5_0
	/// <summary>Encapsulates the description of a custom extended-pixel fill algorithm.</summary>
	///     <remarks>To be added.</remarks>
	///     <altmember cref="CoreVideo.CVFillExtendedPixelsCallBack" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct CVFillExtendedPixelsCallBackData {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint /* CFIndex */ Version;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CVFillExtendedPixelsCallBack FillCallBack;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr UserInfo;
	}

	/// <param name="pixelBuffer">To be added.</param>
	///     <param name="refCon">To be added.</param>
	///     <summary>A delegate that defines the function used to pad buffers that use a custom pixel format.</summary>
	///     <returns>To be added.</returns>
	///     <remarks>To be added.</remarks>
	///     <altmember cref="CoreVideo.CVFillExtendedPixelsCallBackData" />
	public delegate bool CVFillExtendedPixelsCallBack (IntPtr pixelBuffer, IntPtr refCon);
#endif // !XAMCORE_5_0

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#if XAMCORE_5_0
	public unsafe struct CVFillExtendedPixelsCallBackData {
#else
	[NativeName ("CVFillExtendedPixelsCallBackData")]
	public unsafe struct CVFillExtendedPixelsCallBackDataStruct {
#endif
		public nint /* CFIndex */ Version;
		public delegate* unmanaged<IntPtr, IntPtr, byte> FillCallBack;
		public IntPtr UserInfo;
#if !COREBUILD
		public unsafe bool CallFillCallback (CVPixelBuffer buffer)
		{
			if (FillCallBack is null)
				throw new InvalidOperationException ($"The callback is null.");
			var rv = FillCallBack (buffer.GetCheckedHandle (), UserInfo);
			GC.KeepAlive (buffer);
			return rv != 0;
		}
#endif
	}

}
