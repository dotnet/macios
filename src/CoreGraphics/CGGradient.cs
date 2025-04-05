// 
// CGGradient.cs: Implements the managed CGGradient
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2012-2014 Xamarin Inc.
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

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreGraphics {

	// uint32_t -> CGGradient.h
	/// <summary>Drawing location for gradients.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum CGGradientDrawingOptions : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>The fill will draw before the start location.</summary>
		DrawsBeforeStartLocation = (1 << 0),
		/// <summary>The fill will extend beyond the end location.</summary>
		DrawsAfterEndLocation = (1 << 1),
	}

	/// <summary>Gradient definitions.</summary>
	///     <remarks>
	///       <para>A <see cref="T:CoreGraphics.CGGradient" /> defines a smooth transition between colors. </para>
	///       <para>To use a <see cref="T:CoreGraphics.CGGradient" />, application developers will typically have to create a custom <see cref="T:UIKit.UIView" /> and override its <see cref="M:UIKit.UIView.Draw(CoreGraphics.CGRect)" /> method. Application developers should consider a <see cref="T:CoreAnimation.CAGradientLayer" /> as a possible easier-to-use alternative.</para>
	///     </remarks>
	///     <altmember cref="M:CoreGraphics.CGContext.DrawLinearGradient" />
	///     <altmember cref="M:CoreGraphics.CGContext.DrawRadialGradient" />
	///     <altmember cref="T:CoreAnimation.CAGradientLayer" />
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/QuartzSample/">QuartzSample</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGGradient : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGGradient (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGGradientRef */ IntPtr CGGradientRetain (/* CGGradientRef */ IntPtr gradient);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGGradientRelease (/* CGGradientRef */ IntPtr gradient);

		protected internal override void Retain ()
		{
			CGGradientRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGGradientRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe /* CGGradientRef __nullable */ IntPtr CGGradientCreateWithColorComponents (
			/* CGColorSpaceRef __nullable */ IntPtr colorspace, /* const CGFloat* __nullable */ nfloat* components,
			/* const CGFloat* __nullable */ nfloat* locations, /* size_t */ nint count);

		static IntPtr Create (CGColorSpace colorspace, nfloat [] components, nfloat []? locations)
		{
			// those parameters are __nullable but would return a `nil` instance back,
			// which is not something we can handle nicely from a .NET constructor
			if (colorspace is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (colorspace));
			if (components is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (components));

			unsafe {
				fixed (nfloat* componentsPtr = components, locationsPtr = locations) {
					IntPtr result = CGGradientCreateWithColorComponents (colorspace.GetCheckedHandle (), componentsPtr, locationsPtr, components.Length / (colorspace.Components + 1));
					GC.KeepAlive (colorspace);
					return result;
				}
			}
		}

		public CGGradient (CGColorSpace colorspace, nfloat [] components, nfloat []? locations)
			: base (Create (colorspace, components, locations), true)
		{
		}

		static IntPtr Create (CGColorSpace colorspace, nfloat [] components)
		{
			// those parameters are __nullable but would return a `nil` instance back,
			// which is not something we can handle nicely from a .NET constructor
			if (colorspace is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (colorspace));
			if (components is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (components));

			unsafe {
				fixed (nfloat* componentsPtr = components) {
					IntPtr result = CGGradientCreateWithColorComponents (colorspace.GetCheckedHandle (), componentsPtr, null, components.Length / (colorspace.Components + 1));
					GC.KeepAlive (colorspace);
					return result;
				}
			}
		}

		public CGGradient (CGColorSpace colorspace, nfloat [] components)
			: base (Create (colorspace, components), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe /* CGGradientRef __nullable */ IntPtr CGGradientCreateWithColors (
			/* CGColorSpaceRef __nullable */ IntPtr space, /* CFArrayRef __nullable */ IntPtr colors,
			/* const CGFloat* __nullable */ nfloat* locations);

		static IntPtr Create (CGColorSpace? colorspace, CGColor [] colors, nfloat []? locations)
		{
			// colors is __nullable but would return a `nil` instance back,
			// which is not something we can handle nicely from a .NET constructor
			if (colors is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (colors));

			using (var array = CFArray.FromNativeObjects (colors)) {
				unsafe {
					fixed (nfloat* locationsPtr = locations) {
						IntPtr result = CGGradientCreateWithColors (colorspace.GetHandle (), array.Handle, locationsPtr);
						GC.KeepAlive (colorspace);
						return result;
					}
				}
			}
		}

		public CGGradient (CGColorSpace colorspace, CGColor [] colors, nfloat []? locations)
			: base (Create (colorspace, colors, locations), true)
		{
		}

		static IntPtr Create (CGColorSpace? colorspace, CGColor [] colors)
		{
			if (colors is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (colors));

			using (var array = CFArray.FromNativeObjects (colors)) {
				unsafe {
					IntPtr result = CGGradientCreateWithColors (colorspace.GetHandle (), array.Handle, null);
					GC.KeepAlive (colorspace);
					return result;
				}
			}
		}

		/// <param name="colorspace">To be added.</param>
		///         <param name="colors">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGGradient (CGColorSpace? colorspace, CGColor [] colors)
			: base (Create (colorspace, colors), true)
		{
		}
#endif // !COREBUILD
	}
}
