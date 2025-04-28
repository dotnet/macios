// 
// CTRun.cs: Implements the managed CTFrame
//
// Authors: Mono Team
//          Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2010 Novell, Inc
// Copyright 2014 Xamarin Inc (http://www.xamarin.com)
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
using Foundation;
using CoreFoundation;
using CoreGraphics;

namespace CoreText {

	// defined as uint32_t - System/Library/Frameworks/CoreText.framework/Headers/CTRun.h
	/// <summary>An enumeration whose values describe the <see cref="P:CoreText.CTRun.Status" /> of a <see cref="T:CoreText.CTRun" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum CTRunStatus {
		/// <summary>To be added.</summary>
		NoStatus = 0,
		/// <summary>To be added.</summary>
		RightToLeft = (1 << 0),
		/// <summary>To be added.</summary>
		NonMonotonic = (1 << 1),
		/// <summary>To be added.</summary>
		HasNonIdentityMatrix = (1 << 2),
	}

	/// <summary>A glyph run. That is, a series of consecutive glyphs with the same attributes and direction.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTRun : NativeObject {
		[Preserve (Conditional = true)]
		internal CTRun (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunDraw (IntPtr h, IntPtr context, NSRange range);
		/// <param name="context">To be added.</param>
		///         <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Draw (CGContext context, NSRange range)
		{
			CTRunDraw (Handle, context.Handle, range);
			GC.KeepAlive (context);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetAdvances (IntPtr h, NSRange range, [In, Out] CGSize []? buffer);
		/// <param name="range">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize [] GetAdvances (NSRange range, CGSize []? buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetAdvances (Handle, range, buffer);

			return buffer;
		}

		T [] GetBuffer<T> (NSRange range, T []? buffer)
		{
			var glyphCount = GlyphCount;

			if (buffer is not null && range.Length != 0 && buffer.Length < range.Length)
				throw new ArgumentException ("buffer.Length must be >= range.Length.", nameof (buffer));
			if (buffer is not null && range.Length == 0 && buffer.Length < glyphCount)
				throw new ArgumentException ("buffer.Length must be >= GlyphCount.", nameof (buffer));

			return buffer ?? new T [range.Length == 0 ? glyphCount : range.Length];
		}

		/// <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize [] GetAdvances (NSRange range)
		{
			return GetAdvances (range, null);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize [] GetAdvances ()
		{
			return GetAdvances (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static IntPtr CTRunGetAttributes (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTStringAttributes? GetAttributes ()
		{
			var d = Runtime.GetNSObject<NSDictionary> (CTRunGetAttributes (Handle));
			return d is null ? null : new CTStringAttributes (d);
		}


		[DllImport (Constants.CoreTextLibrary)]
		extern static nint CTRunGetGlyphCount (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint GlyphCount {
			get {
				return CTRunGetGlyphCount (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetGlyphs (IntPtr h, NSRange range, [In, Out] ushort []? buffer);
		/// <param name="range">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ushort [] GetGlyphs (NSRange range, ushort []? buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetGlyphs (Handle, range, buffer);

			return buffer;
		}

		/// <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ushort [] GetGlyphs (NSRange range)
		{
			return GetGlyphs (range, null);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ushort [] GetGlyphs ()
		{
			return GetGlyphs (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static CGRect CTRunGetImageBounds (IntPtr h, IntPtr context, NSRange range);
		/// <param name="context">To be added.</param>
		///         <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect GetImageBounds (CGContext context, NSRange range)
		{
			CGRect bounds = CTRunGetImageBounds (Handle, context.Handle, range);
			GC.KeepAlive (context);
			return bounds;
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetPositions (IntPtr h, NSRange range, [In, Out] CGPoint []? buffer);
		/// <param name="range">To be added.</param>
		///         <param name="buffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint [] GetPositions (NSRange range, CGPoint []? buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetPositions (Handle, range, buffer);

			return buffer;
		}

		/// <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint [] GetPositions (NSRange range)
		{
			return GetPositions (range, null);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint [] GetPositions ()
		{
			return GetPositions (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static CTRunStatus CTRunGetStatus (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CTRunStatus Status {
			get {
				return CTRunGetStatus (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetStringIndices (IntPtr h, NSRange range, [In, Out] nint /* CFIndex */ []? buffer);
		public nint [] GetStringIndices (NSRange range, nint []? buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetStringIndices (Handle, range, buffer);

			return buffer;
		}

		/// <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public nint [] GetStringIndices (NSRange range)
		{
			return GetStringIndices (range, null);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public nint [] GetStringIndices ()
		{
			return GetStringIndices (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static NSRange CTRunGetStringRange (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSRange StringRange {
			get {
				return CTRunGetStringRange (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static CGAffineTransform CTRunGetTextMatrix (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGAffineTransform TextMatrix {
			get {
				return CTRunGetTextMatrix (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static double CTRunGetTypographicBounds (IntPtr h, NSRange range, out nfloat ascent, out nfloat descent, out nfloat leading);

		[DllImport (Constants.CoreTextLibrary)]
		extern static double CTRunGetTypographicBounds (IntPtr h, NSRange range, IntPtr ascent, IntPtr descent, IntPtr leading);
		public double GetTypographicBounds (NSRange range, out nfloat ascent, out nfloat descent, out nfloat leading)
		{
			return CTRunGetTypographicBounds (Handle, range, out ascent, out descent, out leading);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public double GetTypographicBounds ()
		{
			NSRange range = new NSRange () { Location = 0, Length = 0 };
			return CTRunGetTypographicBounds (Handle, range, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern void CTRunGetBaseAdvancesAndOrigins (/* CTRunRef */ IntPtr runRef, /* CFRange */ NSRange range, CGSize [] advancesBuffer, CGPoint [] originsBuffer);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void GetBaseAdvancesAndOrigins (NSRange range, out CGSize [] advancesBuffer, out CGPoint [] originsBuffer)
		{
			advancesBuffer = GetBuffer<CGSize> (range, null);
			originsBuffer = GetBuffer<CGPoint> (range, null);
			CTRunGetBaseAdvancesAndOrigins (Handle, range, advancesBuffer, originsBuffer);
		}
	}
}
