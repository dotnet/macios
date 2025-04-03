// 
// CGColor.cs: Implements the managed CGColor
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2014 Xamarin Inc.
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

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreGraphics {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGColor.h
	public class CGColor : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGColor (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		protected internal override void Retain ()
		{
			CGColorRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGColorRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern unsafe static /* CGColorRef */ IntPtr CGColorCreate (/* CGColorSpaceRef */ IntPtr space, /* CGFloat */ nfloat* components);

		static IntPtr Create (CGColorSpace colorspace, nfloat [] components)
		{
			if (components is null)
				global::ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (components));

			unsafe {
				fixed (nfloat* componentsPtr = components) {
					var colorspace_handle = colorspace.GetNonNullHandle (nameof (colorspace));
					IntPtr result = CGColorCreate (colorspace_handle, componentsPtr);
					GC.KeepAlive (colorspace);
					return result;
				}
			}
		}

		public CGColor (CGColorSpace colorspace, nfloat [] components)
			: base (Create (colorspace, components), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorRef */ IntPtr CGColorCreateGenericGray (/* CGFloat */ nfloat gray, /* CGFloat */ nfloat alpha);

		public CGColor (nfloat gray, nfloat alpha)
			: base (CGColorCreateGenericGray (gray, alpha), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorRef */ IntPtr CGColorCreateGenericRGB (/* CGFloat */ nfloat red, /* CGFloat */ nfloat green, /* CGFloat */ nfloat blue, /* CGFloat */ nfloat alpha);

		public CGColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
			: base (CGColorCreateGenericRGB (red, green, blue, alpha), true)
		{
		}

		public CGColor (nfloat red, nfloat green, nfloat blue)
			: base (CGColorCreateGenericRGB (red, green, blue, 1.0f), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorRef */ IntPtr CGColorGetConstantColor (/* CFStringRef */ IntPtr colorName);

		static IntPtr Create (string name)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var nameHandle = CFString.CreateNative (name);
			try {
				var handle = CGColorGetConstantColor (nameHandle);
				if (handle == IntPtr.Zero)
					throw new ArgumentException (nameof (name));
				CGColorRetain (handle);
				return handle;
			} finally {
				CFString.ReleaseNative (nameHandle);
			}

		}

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGColor (string name)
			: base (Create (name), true)
		{
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		static IntPtr Create (CGConstantColor color)
		{
			var constant = color.GetConstant ();
			if (constant is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (color));
			var handle = CGColorGetConstantColor (constant.Handle);
			GC.KeepAlive (constant);
			if (handle == IntPtr.Zero)
				throw new ArgumentException (nameof (color));
			CGColorRetain (handle);
			return handle;
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public CGColor (CGConstantColor color)
			: base (Create (color), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern unsafe static /* CGColorRef */ IntPtr CGColorCreateWithPattern (/* CGColorSpaceRef */ IntPtr space, /* CGPatternRef */ IntPtr pattern, /* const CGFloat[] */ nfloat* components);

		static IntPtr Create (CGColorSpace colorspace, CGPattern pattern, nfloat [] components)
		{
			if (components is null)
				global::ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (components));

			unsafe {
				fixed (nfloat* componentsPtr = components) {
					var colorspace_handle = colorspace.GetNonNullHandle (nameof (colorspace));
					var pattern_handle = pattern.GetNonNullHandle (nameof (pattern));
					var handle = CGColorCreateWithPattern (colorspace_handle, pattern_handle, componentsPtr);
					if (handle == IntPtr.Zero)
						throw new ArgumentException ();
					GC.KeepAlive (colorspace);
					GC.KeepAlive (pattern);
					return handle;
				}
			}
		}

		public CGColor (CGColorSpace colorspace, CGPattern pattern, nfloat [] components)
			: base (Create (colorspace, pattern, components), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorRef */ IntPtr CGColorCreateCopyWithAlpha (/* CGColorRef */ IntPtr color, nfloat alpha);

		static IntPtr Create (CGColor source, nfloat alpha)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			IntPtr result = CGColorCreateCopyWithAlpha (source.GetCheckedHandle (), alpha);
			GC.KeepAlive (source);
			return result;
		}

		public CGColor (CGColor source, nfloat alpha)
			: base (Create (source, alpha), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGColorEqualToColor (/* CGColorRef */ IntPtr color1, /* CGColorRef */ IntPtr color2);

		public static bool operator == (CGColor color1, CGColor color2)
		{
			if (color1 is null)
				return color2 is null;
			return color1.Equals (color2);
		}

		public static bool operator != (CGColor color1, CGColor color2)
		{
			if (color1 is null)
				return color2 is not null;
			return !color1.Equals (color2);
		}

		/// <summary>Get the hashcode for this color.</summary>
		///         <returns>The hashcode.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			// looks weird but it's valid
			// using the Handle property would not be since there's a special function for equality
			// see Remarks in https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=net-6.0
			return 0;
		}

		/// <param name="o">The other object.</param>
		///         <summary>Determines if the objects are equal.</summary>
		///         <returns>
		///           <see langword="true" /> if this color is equal to the specified object.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? o)
		{
			var other = o as CGColor;
			if (other is null)
				return false;

			bool result = CGColorEqualToColor (this.Handle, other.Handle) != 0;
			GC.KeepAlive (other);
			return result;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGColorGetNumberOfComponents (/* CGColorRef */ IntPtr color);

		/// <summary>The number of components in this color.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public nint NumberOfComponents {
			get {
				return CGColorGetNumberOfComponents (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe /* CGFloat* */ nfloat* CGColorGetComponents (/* CGColorRef */ IntPtr color);

		/// <summary>The components for this color.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public nfloat [] Components {
			get {
				int n = (int) NumberOfComponents;
				nfloat [] result = new nfloat [n];
				unsafe {
					nfloat* cptr = CGColorGetComponents (Handle);

					for (int i = 0; i < n; i++) {
						result [i] = cptr [i];
					}
				}
				return result;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGFloat */ nfloat CGColorGetAlpha (/* CGColorRef */ IntPtr color);

		/// <summary>The alpha channel value.</summary>
		///         <value>A value in the 0.0 to 1.0f range.</value>
		///         <remarks>To be added.</remarks>
		public nfloat Alpha {
			get {
				return CGColorGetAlpha (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorSpaceRef */ IntPtr CGColorGetColorSpace (/* CGColorRef */ IntPtr color);

		/// <summary>The color's colorspace.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public CGColorSpace? ColorSpace {
			get {
				var ptr = CGColorGetColorSpace (Handle);
				return ptr == IntPtr.Zero ? null : new CGColorSpace (ptr, false);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPatternRef */ IntPtr CGColorGetPattern (/* CGColorRef */ IntPtr color);
		/// <summary>If present, the pattern for this color.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGPattern? Pattern {
			get {
				var h = CGColorGetPattern (Handle);
				// return `null`, not an invalid instance, if there's no pattern
				return h == IntPtr.Zero ? null : new CGPattern (h, false);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGColorRef */ IntPtr CGColorRetain (/* CGColorRef */ IntPtr color);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGColorRelease (/* CGColorRef */ IntPtr color);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGColorRef __nullable */ IntPtr CGColorCreateCopyByMatchingToColorSpace (
			/* __nullable CGColorSpaceRef* */ IntPtr space, CGColorRenderingIntent intent,
			/* CGColorRef __nullable */ IntPtr color, /* __nullable CFDictionaryRef */ IntPtr options);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public CGColor? CreateByMatchingToColorSpace (CGColorSpace space, CGColorRenderingIntent intent,
			CGColor color, NSDictionary options)
		{
			var h = CGColorCreateCopyByMatchingToColorSpace (space.GetHandle (), intent, color.GetHandle (), options.GetHandle ());
			GC.KeepAlive (space);
			GC.KeepAlive (color);
			GC.KeepAlive (options);
			return h == IntPtr.Zero ? null : new CGColor (h, owns: true);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGColorRef* */ IntPtr CGColorCreateSRGB (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public CGColor? CreateSrgb (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			var h = CGColorCreateSRGB (red, green, blue, alpha);
			return h == IntPtr.Zero ? null : new CGColor (h, owns: true);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGColorRef* */ IntPtr CGColorCreateGenericGrayGamma2_2 (nfloat gray, nfloat alpha);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public CGColor? CreateGenericGrayGamma2_2 (nfloat gray, nfloat alpha)
		{
			var h = CGColorCreateGenericGrayGamma2_2 (gray, alpha);
			return h == IntPtr.Zero ? null : new CGColor (h, owns: true);
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGColorRef */ IntPtr CGColorCreateGenericCMYK (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha);

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		static public CGColor? CreateCmyk (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			var h = CGColorCreateGenericCMYK (cyan, magenta, yellow, black, alpha);
			return h == IntPtr.Zero ? null : new CGColor (h, owns: true);
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.AccessibilityLibrary)]
		static extern /* NSString */ IntPtr AXNameFromColor (/* CGColorRef */ IntPtr color);

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public string? AXName => CFString.FromHandle (AXNameFromColor (Handle));


#endif // !COREBUILD
	}
}
