//
// NSLayoutManager.cs: 
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2013, Xamarin Inc
//

#if IOS
using System.Drawing;
#endif
using System.Collections;

using CoreGraphics;

#if MONOMAC
using UIFont = AppKit.NSFont;
#endif

#if MONOMAC
namespace AppKit {
#else
namespace UIKit {
#endif
	partial class NSLayoutManager {
		/// <summary>Fills <paramref name="glyphBuffer" /> with the glyphs in <paramref name="glyphRange" />.</summary>
		/// <param name="glyphRange">The range of glyphs to retrieve.</param>
		/// <param name="glyphBuffer">An output buffer that receives the glyph identifiers for the specified range.</param>
		/// <param name="props">An output buffer that receives the glyph properties for each glyph.</param>
		/// <param name="charIndexBuffer">An output buffer that receives the character index corresponding to each glyph.</param>
		/// <param name="bidiLevelBuffer">An output buffer that receives the bidirectional embedding level for each glyph.</param>
		/// <returns>The number of glyphs in <paramref name="glyphBuffer" />.</returns>
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
		/// <param name="glyphs">The glyphs.</param>
		/// <param name="positions">The positions.</param>
		/// <param name="glyphCount">The glyph count.</param>
		/// <param name="font">The font.</param>
		/// <param name="textMatrix">The text matrix.</param>
		/// <param name="attributes">The attributes.</param>
		/// <param name="graphicsContext">The graphics context.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
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

		/// <summary>Fills <paramref name="positions" /> and <paramref name="charIndexes" /> with the positions and indices of the insertion points for a line fragment.</summary>
		/// <param name="charIndex">A character index within the line fragment.</param>
		/// <param name="alternatePosition">Whether to use the alternate insertion point position.</param>
		/// <param name="inDisplayOrder">Whether to return the insertion points in display order rather than logical order.</param>
		/// <param name="positions">An output buffer that receives the horizontal positions of each insertion point.</param>
		/// <param name="charIndexes">An output buffer that receives the character index for each insertion point.</param>
		/// <returns>The number of insertion points returned in <paramref name="positions" /> and <paramref name="charIndexes" />.</returns>
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
