// 
// CGShading.cs: Implements the managed CGShading
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace CoreGraphics {
	/// <summary>A type that represents a Quartz shading.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGShading.h
	public class CGShading : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGShading (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		protected internal override void Retain ()
		{
			CGShadingRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGShadingRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGShadingRef */ IntPtr CGShadingCreateAxial (/* CGColorSpaceRef */ IntPtr space,
			CGPoint start, CGPoint end, /* CGFunctionRef */ IntPtr functionHandle, byte extendStart, byte extendEnd);

		/// <summary>Create a new axial shading.</summary>
		/// <param name="colorspace">The colorspace to use for the new <see cref="CGShading" />.</param>
		/// <param name="start">The starting point for the axis.</param>
		/// <param name="end">The ending point for the axis</param>
		/// <param name="function">The shading function to use.</param>
		/// <param name="extendStart">Whether the shading will extend beyond the starting point of the axis or not.</param>
		/// <param name="extendEnd">Whether the shading will extend beyond the ending point of the axis or not.</param>
		/// <returns>A new <see cref="CGShading" /> if successful, <see langword="null" /> otherwise.</returns>
		public static CGShading? CreateAxial (CGColorSpace? colorspace, CGPoint start, CGPoint end, CGFunction? function, bool extendStart, bool extendEnd)
		{
			var handle = CGShadingCreateAxial (colorspace.GetHandle (), start, end, function.GetHandle (), extendStart.AsByte (), extendEnd.AsByte ());
			GC.KeepAlive (colorspace);
			GC.KeepAlive (function);
			if (handle == IntPtr.Zero)
				return null;
			return new CGShading (handle, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGShadingRef */ IntPtr CGShadingCreateRadial (/* CGColorSpaceRef */ IntPtr space,
			CGPoint start, /* CGFloat */ nfloat startRadius, CGPoint end, /* CGFloat */ nfloat endRadius,
			/* CGFunctionRef */ IntPtr function, byte extendStart, byte extendEnd);

		/// <summary>Create a new radial shading.</summary>
		/// <param name="colorspace">The colorspace to use for the new <see cref="CGShading" />.</param>
		/// <param name="start">The center point for the starting circle.</param>
		/// <param name="startRadius">The radius of the starting circle.</param>
		/// <param name="end">The center point for the ending circle</param>
		/// <param name="endRadius">The radius of the ending circle.</param>
		/// <param name="function">The shading function to use.</param>
		/// <param name="extendStart">Whether the shading will extend beyond the starting circle or not.</param>
		/// <param name="extendEnd">Whether the shading will extend beyond the ending circle or not.</param>
		/// <returns>A new <see cref="CGShading" /> if successful, <see langword="null" /> otherwise.</returns>
		public static CGShading? CreateRadial (CGColorSpace? colorspace, CGPoint start, nfloat startRadius, CGPoint end, nfloat endRadius,
							  CGFunction? function, bool extendStart, bool extendEnd)
		{
			var handle = CGShadingCreateRadial (colorspace.GetHandle (), start, startRadius, end, endRadius,
									 function.GetHandle (), extendStart.AsByte (), extendEnd.AsByte ());
			GC.KeepAlive (colorspace);
			GC.KeepAlive (function);
			if (handle == IntPtr.Zero)
				return null;
			return new CGShading (handle, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGShadingRef */ IntPtr CGShadingRelease (/* CGShadingRef */ IntPtr shading);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGShadingRef */ IntPtr CGShadingRetain (/* CGShadingRef */ IntPtr shading);

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr /* CGShadingRef __nullable */ CGShadingCreateAxialWithContentHeadroom (
				float headroom,
				IntPtr /* CGColorSpaceRef cg_nullable */ space,
				CGPoint start,
				CGPoint end,
				IntPtr /* CGFunctionRef cg_nullable */ function,
				byte /* bool */ extendStart,
				byte /* bool */ extendEnd);

		/// <summary>Create an axial shading with the specified content headroom.</summary>
		/// <param name="headroom">The content headroom for the new <see cref="CGShading" />.</param>
		/// <param name="colorSpace">The colorspace to use for the new <see cref="CGShading" />. This colorspace must support HDR.</param>
		/// <param name="start">The starting point for the axis.</param>
		/// <param name="end">The ending point for the axis</param>
		/// <param name="function">The shading function to use.</param>
		/// <param name="extendStart">Whether the shading will extend beyond the starting point of the axis or not.</param>
		/// <param name="extendEnd">Whether the shading will extend beyond the ending point of the axis or not.</param>
		/// <returns>A new <see cref="CGShading" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public static CGShading? CreateAxial (float headroom, CGColorSpace? colorSpace, CGPoint start, CGPoint end, CGFunction? function, bool extendStart, bool extendEnd)
		{
			var handle = CGShadingCreateAxialWithContentHeadroom (headroom, colorSpace.GetHandle (), start, end, function.GetHandle (), extendStart.AsByte (), extendEnd.AsByte ());
			GC.KeepAlive (colorSpace);
			GC.KeepAlive (function);
			if (handle == IntPtr.Zero)
				return null;
			return new CGShading (handle, true);
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGShadingRef */ IntPtr CGShadingCreateRadialWithContentHeadroom (
			float headroom,
			/* CGColorSpaceRef cg_nullable */ IntPtr space,
			CGPoint start,
			/* CGFloat */ nfloat startRadius,
			CGPoint end,
			/* CGFloat */ nfloat endRadius,
			/* CGFunctionRef cg_nullable */ IntPtr function,
			byte extendStart,
			byte extendEnd);

		/// <summary>Create a new radial shading with the specified content headroom.</summary>
		/// <param name="headroom">The content headroom for the new <see cref="CGShading" />.</param>
		/// <param name="colorspace">The colorspace to use for the new <see cref="CGShading" />. This colorspace must support HDR.</param>
		/// <param name="start">The center point for the starting circle.</param>
		/// <param name="startRadius">The radius of the starting circle.</param>
		/// <param name="end">The center point for the ending circle</param>
		/// <param name="endRadius">The radius of the ending circle.</param>
		/// <param name="function">The shading function to use.</param>
		/// <param name="extendStart">Whether the shading will extend beyond the starting circle or not.</param>
		/// <param name="extendEnd">Whether the shading will extend beyond the ending circle or not.</param>
		/// <returns>A new <see cref="CGShading" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public static CGShading? CreateRadial (float headroom, CGColorSpace? colorspace, CGPoint start, nfloat startRadius, CGPoint end, nfloat endRadius, CGFunction? function, bool extendStart, bool extendEnd)
		{
			var handle = CGShadingCreateRadialWithContentHeadroom (headroom, colorspace.GetHandle (), start, startRadius, end, endRadius, function.GetHandle (), extendStart.AsByte (), extendEnd.AsByte ());
			GC.KeepAlive (colorspace);
			GC.KeepAlive (function);
			if (handle == IntPtr.Zero)
				return null;
			return new CGShading (handle, true);
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGShadingGetContentHeadroom (IntPtr /* CGShadingRef gc_nullable */ shading);

		/// <summary>Get the content headroom for this shading.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public float ContentHeadroom {
			get => CGShadingGetContentHeadroom (GetCheckedHandle ());
		}
#endif // !COREBUILD
	}
}
