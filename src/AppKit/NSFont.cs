#if !__MACCATALYST__
using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;
using CoreText;

using CGGlyph = System.UInt16;

#nullable enable

namespace AppKit {
	public partial class NSFont {
		/// <param name="font">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSFont? FromCTFont (CTFont? font)
		{
			if (font is null)
				return null;
			var result = new NSFont (font.Handle);
			GC.KeepAlive (font);
			return result;
		}

		/// <param name="glyphs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe CGRect [] GetBoundingRects (CGGlyph [] glyphs)
		{
			if (glyphs is null)
				throw new ArgumentNullException ("glyphs");
			if (glyphs.Length < 1)
				throw new ArgumentException ("glyphs array is empty");

			CGRect [] bounds = new CGRect [glyphs.Length];
			fixed (CGRect* boundsPtr = bounds) {
				fixed (CGGlyph* glyphsPtr = glyphs) {
					_GetBoundingRects ((IntPtr) boundsPtr, (IntPtr) glyphsPtr, (nuint) glyphs.Length);
				}
			}
			return bounds;
		}

		/// <param name="glyphs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe CGSize [] GetAdvancements (CGGlyph [] glyphs)
		{
			if (glyphs is null)
				throw new ArgumentNullException ("glyphs");
			if (glyphs.Length < 1)
				throw new ArgumentException ("glyphs array is empty");

			CGSize [] advancements = new CGSize [glyphs.Length];
			fixed (CGSize* advancementsPtr = advancements) {
				fixed (CGGlyph* glyphsPtr = glyphs) {
					_GetAdvancements ((IntPtr) advancementsPtr, (IntPtr) glyphsPtr, (nuint) glyphs.Length);
				}
			}
			return advancements;
		}

		public static NSFont? FromFontName (string fontName, nfloat fontSize)
		{
			var ptr = _FromFontName (fontName, fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? FromDescription (NSFontDescriptor fontDescriptor, nfloat fontSize)
		{
			var ptr = _FromDescription (fontDescriptor, fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		/// <param name="fontDescriptor">To be added.</param>
		///         <param name="textTransform">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSFont? FromDescription (NSFontDescriptor fontDescriptor, NSAffineTransform textTransform)
		{
			var ptr = _FromDescription (fontDescriptor, textTransform);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? UserFontOfSize (nfloat fontSize)
		{
			var ptr = _UserFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? UserFixedPitchFontOfSize (nfloat fontSize)
		{
			var ptr = _UserFixedPitchFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? SystemFontOfSize (nfloat fontSize)
		{
			var ptr = _SystemFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? BoldSystemFontOfSize (nfloat fontSize)
		{
			var ptr = _BoldSystemFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? LabelFontOfSize (nfloat fontSize)
		{
			var ptr = _LabelFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? TitleBarFontOfSize (nfloat fontSize)
		{
			var ptr = _TitleBarFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? MenuFontOfSize (nfloat fontSize)
		{
			var ptr = _MenuFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? MenuBarFontOfSize (nfloat fontSize)
		{
			var ptr = _MenuBarFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? MessageFontOfSize (nfloat fontSize)
		{
			var ptr = _MessageFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? PaletteFontOfSize (nfloat fontSize)
		{
			var ptr = _PaletteFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? ToolTipsFontOfSize (nfloat fontSize)
		{
			var ptr = _ToolTipsFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? ControlContentFontOfSize (nfloat fontSize)
		{
			var ptr = _ControlContentFontOfSize (fontSize);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.13")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public virtual NSFont? PrinterFont {
			get {
				var ptr = _PrinterFont;
				return ptr == IntPtr.Zero ? null : new NSFont (ptr);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.13")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public virtual NSFont? ScreenFont {
			get {
				var ptr = _ScreenFont;
				return ptr == IntPtr.Zero ? null : new NSFont (ptr);
			}
		}

		/// <param name="renderingMode">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.13")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public virtual NSFont? ScreenFontWithRenderingMode (NSFontRenderingMode renderingMode)
		{
			var ptr = _ScreenFontWithRenderingMode (renderingMode);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NSFont? GetVerticalFont ()
		{
			var ptr = _GetVerticalFont ();
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? SystemFontOfSize (nfloat fontSize, nfloat weight)
		{
			var ptr = _SystemFontOfSize (fontSize, weight);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? SystemFontOfSize (nfloat fontSize, nfloat weight, nfloat width)
		{
			var ptr = _SystemFontOfSize (fontSize, weight, width);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? MonospacedDigitSystemFontOfSize (nfloat fontSize, nfloat weight)
		{
			var ptr = _MonospacedDigitSystemFontOfSize (fontSize, weight);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}

		public static NSFont? MonospacedSystemFont (nfloat fontSize, nfloat weight)
		{
			var ptr = _MonospacedSystemFont (fontSize, weight);
			return ptr == IntPtr.Zero ? null : new NSFont (ptr);
		}
	}
}
#endif // !__MACCATALYST__
