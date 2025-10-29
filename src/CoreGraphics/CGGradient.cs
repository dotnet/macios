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

using CoreFoundation;

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

#if __MACOS__
	/// <summary>Gradient definitions.</summary>
	/// <remarks>
	///   <para>A <see cref="CoreGraphics.CGGradient" /> defines a smooth transition between colors. </para>
	///   <para>To use a <see cref="CoreGraphics.CGGradient" />, application developers will typically have to create a custom <see cref="AppKit.NSView" /> and override its <see cref="AppKit.NSView.DrawRect(CoreGraphics.CGRect)" /> method. Application developers should consider a <see cref="CoreAnimation.CAGradientLayer" /> as a possible easier-to-use alternative.</para>
	/// </remarks>
	/// <altmember cref="CoreGraphics.CGContext.DrawLinearGradient" />
	/// <altmember cref="CoreGraphics.CGContext.DrawRadialGradient" />
	/// <altmember cref="CoreAnimation.CAGradientLayer" />
#else
	/// <summary>Gradient definitions.</summary>
	/// <remarks>
	///   <para>A <see cref="CoreGraphics.CGGradient" /> defines a smooth transition between colors. </para>
	///   <para>To use a <see cref="CoreGraphics.CGGradient" />, application developers will typically have to create a custom <see cref="UIKit.UIView" /> and override its <see cref="UIKit.UIView.Draw(CoreGraphics.CGRect)" /> method. Application developers should consider a <see cref="CoreAnimation.CAGradientLayer" /> as a possible easier-to-use alternative.</para>
	/// </remarks>
	/// <altmember cref="CoreGraphics.CGContext.DrawLinearGradient" />
	/// <altmember cref="CoreGraphics.CGContext.DrawRadialGradient" />
	/// <altmember cref="CoreAnimation.CAGradientLayer" />
	/// <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/QuartzSample/">QuartzSample</related>
#endif
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

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern IntPtr /* CGGradientRef __nullable */ CGGradientCreateWithContentHeadroom (
			float headroom,
			IntPtr /* CGColorSpaceRef cg_nullable */ space,
			nfloat* /* const CGFloat * cg_nullable */ components,
			nfloat* /* const CGFloat * cg_nullable */ locations,
			nint count);

		/// <summary>Create a <see cref="CGGradient" /> with the specified content headroom.</summary>
		/// <param name="headroom">The content headroom for the new <see cref="CGGradient" />.</param>
		/// <param name="colorSpace">The colorspace to use for the gradient. This colorspace must support HDR.</param>
		/// <param name="components">The color components to map into the new <see cref="CGGradient" />.</param>
		/// <param name="locations">An array of values that determines where, in the range from 0.0 to 1.0, should each color be located in the new <see cref="CGGradient" />.</param>
		/// <returns>A new <see cref="CGGradient" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public static CGGradient? Create (float headroom, CGColorSpace? colorSpace, nfloat []? components, nfloat []? locations)
		{
			// "The number of locations is specified by `count'"
			// "The number of color components is the product of `count' and the number of color components of `space'."
			var locationLength = locations?.Length ?? 0;
			var colorComponentsCount = colorSpace?.Components ?? 0;
			var expectedColorComponents = (locations?.Length ?? 0) * (colorSpace?.Components ?? 0);
			if (expectedColorComponents > 0 && (components is null || components.Length < expectedColorComponents))
				throw new ArgumentException (nameof (components), string.Format ("Must have at least {0} color components when the {1} array has {2} and the color space {3} has {4} color components.", expectedColorComponents, nameof (locations), locationLength, nameof (colorSpace), colorComponentsCount));

			unsafe {
				fixed (nfloat* componentsPtr = components) {
					fixed (nfloat* locationsPtr = locations) {
						var result = CGGradientCreateWithContentHeadroom (headroom, colorSpace.GetHandle (), componentsPtr, locationsPtr, locationLength);
						GC.KeepAlive (colorSpace);
						if (result == IntPtr.Zero)
							return null;
						return new CGGradient (result, true);
					}
				}
			}
		}

		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGGradientGetContentHeadroom (IntPtr /* CGGradientRef gc_nullable */ gradient);

		/// <summary>Get the content headroom for this gradient.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public float ContentHeadroom {
			get => CGGradientGetContentHeadroom (GetCheckedHandle ());
		}

#endif // !COREBUILD
	}
}
