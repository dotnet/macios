//
// NSLayoutManager.cs: 
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2013, Xamarin Inc
//

using System;
#if IOS
using System.Drawing;
#endif
using System.Runtime.InteropServices;
using System.Collections;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

#if MONOMAC
using UIFont = AppKit.NSFont;
#endif

#if MONOMAC
namespace AppKit {
#else
namespace UIKit {
#endif
	partial class NSLayoutManager {
		/// <param name="glyphRange">To be added.</param>
		/// <param name="glyphBuffer">To be added.</param>
		/// <param name="props">To be added.</param>
		/// <param name="charIndexBuffer">To be added.</param>
		/// <param name="bidiLevelBuffer">To be added.</param>
		/// <summary>Fills <paramref name="glyphBuffer" /> with the glyphs in <paramref name="glyphRange" />.</summary>
		/// <returns>The number of glyphs in <paramref name="glyphBuffer" />.</returns>
		/// <remarks>To be added.</remarks>
		public unsafe nuint GetGlyphs (
			NSRange glyphRange,
			short [] /* CGGlyph* = CGFontIndex* = unsigned short* */ glyphBuffer,
			NSGlyphProperty [] /* NSGlyphProperty* = nint* */ props,
			nuint [] /* NSUInteger */ charIndexBuffer,
			byte [] /* (unsigned char *) */ bidiLevelBuffer)
		{
			if (glyphBuffer is not null && glyphBuffer.Length < glyphRange.Length)
				throw new ArgumentOutOfRangeException (string.Format ("glyphBuffer must have at least {0} elements", glyphRange.Length));

			if (props is not null && props.Length < glyphRange.Length)
				throw new ArgumentOutOfRangeException (string.Format ("props must have at least {0} elements", glyphRange.Length));

			if (charIndexBuffer is not null && charIndexBuffer.Length < glyphRange.Length)
				throw new ArgumentOutOfRangeException (string.Format ("props must have at least {0} elements", glyphRange.Length));

			if (bidiLevelBuffer is not null && bidiLevelBuffer.Length < glyphRange.Length)
				throw new ArgumentOutOfRangeException (string.Format ("bidiLevelBuffer must have at least {0} elements", glyphRange.Length));

			fixed (short* glyphs = glyphBuffer) {
				nuint rv;
				// Unified/64 + Classic: the input array is the correct size
				var tmpArray = props;
				fixed (void* properties = tmpArray) {
					fixed (nuint* charIBuffer = charIndexBuffer) {
						fixed (byte* bidi = bidiLevelBuffer) {
							rv = GetGlyphs (glyphRange, (IntPtr) glyphs, (IntPtr) properties, (IntPtr) charIBuffer, (IntPtr) bidi);
						}
					}
				}

				return rv;
			}
		}

		/// <summary>Renders <paramref name="glyphs" /> at <paramref name="positions" /> into <paramref name="graphicsContext" />.</summary>
		/// <param name="glyphs">To be added.</param>
		/// <param name="positions">To be added.</param>
		/// <param name="glyphCount">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="textMatrix">To be added.</param>
		/// <param name="attributes">To be added.</param>
		/// <param name="graphicsContext">To be added.</param>
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe void ShowGlyphs (
			short [] /* const CGGlyph* = CGFontIndex* = unsigned short* */ glyphs,
			CGPoint [] /* const CGPoint* */ positions,
			nint /* NSInteger */ glyphCount,
			UIFont font,
			CGAffineTransform textMatrix,
			NSDictionary attributes,
			CGContext graphicsContext)
		{
			fixed (short* gl = glyphs) {
				fixed (CGPoint* pos = positions) {
					ShowGlyphs ((IntPtr) gl, (IntPtr) pos, glyphCount, font, textMatrix, attributes, graphicsContext);
				}
			}
		}

		/// <param name="charIndex">To be added.</param>
		/// <param name="alternatePosition">To be added.</param>
		/// <param name="inDisplayOrder">To be added.</param>
		/// <param name="positions">To be added.</param>
		/// <param name="charIndexes">To be added.</param>
		/// <summary>Fills <paramref name="positions" /> and <paramref name="charIndexes" /> with the positions and indices of the insertion points for a line fragment.</summary>
		/// <returns>The number of insertion points returned in <paramref name="positions" /> and <paramref name="charIndexes" />.</returns>
		/// <remarks>To be added.</remarks>
		public unsafe nuint GetLineFragmentInsertionPoints (
			nuint /* NSUInteger */ charIndex,
			bool /* BOOL */ alternatePosition,
			bool /* BOOL */ inDisplayOrder,
			nfloat [] /* CGFloat* */ positions,
			nint [] /* NSUInteger* */ charIndexes)
		{
			fixed (nfloat* p = positions) {
				fixed (nint* c = charIndexes) {
					var rv = GetLineFragmentInsertionPoints (charIndex, alternatePosition, inDisplayOrder, (IntPtr) p, (IntPtr) c);

					// I can't find an API to check this before the call :(

					if (positions is not null && (ulong) positions.Length < (ulong) rv)
						throw new ArgumentException (string.Format ("Memory corruption: the 'positions' array was not big enough to hold the number of insertion points. {0} insertion points were returned, while the array's Length is only {1}", rv, positions.Length));

					if (charIndexes is not null && (ulong) charIndexes.Length < (ulong) rv)
						throw new ArgumentException (string.Format ("Memory corruption: the 'charIndexes' array was not big enough to hold the number of insertion points. {0} insertion points were returned, while the array's Length is only {1}", rv, charIndexes.Length));

					return rv;
				}
			}
		}
	}
}
