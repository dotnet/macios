// 
// CGContext.cs: Implements the managed CGContext
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

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace CoreGraphics {
	/// <include file="../../docs/api/CoreGraphics/CGContext.xml" path="/Documentation/Docs[@DocId='T:CoreGraphics.CGContext']/*" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGContext : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGContext (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRelease (/* CGContextRef */ IntPtr c);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGContextRef */ IntPtr CGContextRetain (/* CGContextRef */ IntPtr c);

		protected internal override void Retain ()
		{
			CGContextRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGContextRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSaveGState (/* CGContextRef */ IntPtr c);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRestoreGState (/* CGContextRef */ IntPtr c);

		/// <summary>Stores the state of the <see cref="T:CoreGraphics.CGContext" />. (See <see cref="M:CoreGraphics.CGContext.RestoreState" />.)</summary>
		///         <remarks>To be added.</remarks>
		public void SaveState ()
		{
			CGContextSaveGState (Handle);
		}

		/// <summary>Sets the state of the <see cref="T:CoreGraphics.CGContext" /> to what it was when <see cref="M:CoreGraphics.CGContext.SaveState" /> was last called.</summary>
		///         <remarks>To be added.</remarks>
		public void RestoreState ()
		{
			CGContextRestoreGState (Handle);
		}

		//
		// Transformation matrix
		//

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextScaleCTM (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat sx, /* CGFloat */ nfloat sy);

		public void ScaleCTM (nfloat sx, nfloat sy)
		{
			CGContextScaleCTM (Handle, sx, sy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextTranslateCTM (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat tx, /* CGFloat */ nfloat ty);

		public void TranslateCTM (nfloat tx, nfloat ty)
		{
			CGContextTranslateCTM (Handle, tx, ty);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRotateCTM (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat angle);

		public void RotateCTM (nfloat angle)
		{
			CGContextRotateCTM (Handle, angle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextConcatCTM (/* CGContextRef */ IntPtr c, CGAffineTransform transform);

		/// <param name="transform">The <see cref="T:CoreGraphics.CGAffineTransform" /> to concatenate onto the current context transformation matrix.</param>
		///         <summary>Concatenates the specified <paramref name="transform" /> onto the current transformation matrix.</summary>
		///         <remarks>
		///           <para>The <paramref name="transform" /> is concatenated to the current context transformation matrix to create the new CTM. (The example in the discussion of <see cref="M:CoreGraphics.CGContext.GetCTM" /> illustrates <see cref="M:CoreGraphics.CGContext.ConcatCTM(CoreGraphics.CGAffineTransform)" />.)</para>
		///         </remarks>
		public void ConcatCTM (CGAffineTransform transform)
		{
			CGContextConcatCTM (Handle, transform);
		}

		// Settings
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetLineWidth (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat width);

		public void SetLineWidth (nfloat w)
		{
			CGContextSetLineWidth (Handle, w);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetLineCap (/* CGContextRef */ IntPtr c, CGLineCap cap);

		/// <param name="cap">The desired <see cref="T:CoreGraphics.CGLineCap" />.</param>
		///         <summary>Sets the style for the ends of lines.</summary>
		///         <remarks>To be added.</remarks>
		public void SetLineCap (CGLineCap cap)
		{
			CGContextSetLineCap (Handle, cap);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetLineJoin (/* CGContextRef */  IntPtr c, CGLineJoin join);

		/// <param name="join">The desired <see cref="T:CoreGraphics.CGLineJoin" />.</param>
		///         <summary>Sets the way lines are joined.</summary>
		///         <remarks>To be added.</remarks>
		public void SetLineJoin (CGLineJoin join)
		{
			CGContextSetLineJoin (Handle, join);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetMiterLimit (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat limit);

		public void SetMiterLimit (nfloat limit)
		{
			CGContextSetMiterLimit (Handle, limit);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe void CGContextSetLineDash (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat phase, /* CGFloat[] */ nfloat* lengths, /* size_t */ nint count);

		public void SetLineDash (nfloat phase, nfloat []? lengths)
		{
			int n = lengths is null ? 0 : lengths.Length;
			unsafe {
				fixed (nfloat* lengthsPtr = lengths) {
					CGContextSetLineDash (Handle, phase, lengthsPtr, n);
				}
			}
		}

		public void SetLineDash (nfloat phase, nfloat []? lengths, int n)
		{
			if (lengths is null)
				n = 0;
			else if (n < 0 || n > lengths.Length)
				throw new ArgumentException (nameof (n));
			unsafe {
				fixed (nfloat* lengthsPtr = lengths) {
					CGContextSetLineDash (Handle, phase, lengthsPtr, n);
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFlatness (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat flatness);

		public void SetFlatness (nfloat flatness)
		{
			CGContextSetFlatness (Handle, flatness);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAlpha (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat alpha);

		public void SetAlpha (nfloat alpha)
		{
			CGContextSetAlpha (Handle, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetBlendMode (/* CGContextRef */ IntPtr c, CGBlendMode mode);

		/// <param name="mode">The desired <see cref="T:CoreGraphics.CGBlendMode" />.</param>
		///         <summary>Specifies the compositing mode.</summary>
		///         <remarks>To be added.</remarks>
		public void SetBlendMode (CGBlendMode mode)
		{
			CGContextSetBlendMode (Handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetCTM (/* CGContextRef */ IntPtr c);

		/// <include file="../../docs/api/CoreGraphics/CGContext.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGContext.GetCTM']/*" />
		public CGAffineTransform GetCTM ()
		{
			return CGContextGetCTM (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginPath (/* CGContextRef */ IntPtr c);

		/// <summary>Starts a new path in the graphics context.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginPath ()
		{
			CGContextBeginPath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextMoveToPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public void MoveTo (nfloat x, nfloat y)
		{
			CGContextMoveToPoint (Handle, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLineToPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public void AddLineToPoint (nfloat x, nfloat y)
		{
			CGContextAddLineToPoint (Handle, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddCurveToPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat cp1x, /* CGFloat */ nfloat cp1y, /* CGFloat */ nfloat cp2x, /* CGFloat */ nfloat cp2y, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public void AddCurveToPoint (nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
		{
			CGContextAddCurveToPoint (Handle, cp1x, cp1y, cp2x, cp2y, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddQuadCurveToPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat cpx, /* CGFloat */ nfloat cpy, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public void AddQuadCurveToPoint (nfloat cpx, nfloat cpy, nfloat x, nfloat y)
		{
			CGContextAddQuadCurveToPoint (Handle, cpx, cpy, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClosePath (/* CGContextRef */ IntPtr c);

		/// <summary>Closes and completes the current path.</summary>
		///         <remarks>To be added.</remarks>
		public void ClosePath ()
		{
			CGContextClosePath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">A rectangle.</param>
		///         <summary>Adds a rectangular path to the current path.</summary>
		///         <remarks>To be added.</remarks>
		public void AddRect (CGRect rect)
		{
			CGContextAddRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRects (/* CGContextRef */ IntPtr c, CGRect [] rects, /* size_t */ nint count);

		/// <param name="rects">An array of rectangles.</param>
		///         <summary>Adds an array of rectangular paths to the current path.</summary>
		///         <remarks>To be added.</remarks>
		public void AddRects (CGRect [] rects)
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			CGContextAddRects (Handle, rects, rects.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLines (/* CGContextRef */ IntPtr c, CGPoint [] points, /* size_t */ nint count);
		/// <include file="../../docs/api/CoreGraphics/CGContext.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGContext.AddLines(CoreGraphics.CGPoint[])']/*" />
		public void AddLines (CGPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			CGContextAddLines (Handle, points, points.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddEllipseInRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">The bounding rectangle of the ellipse.</param>
		///         <summary>Adds an ellipse that fits in the specified <paramref name="rect" />.</summary>
		///         <remarks>
		///           <para>The ellipse is centered in the <paramref name="rect" />, with major and minor axes defined such that the ellipse touches the <paramref name="rect" />'s edges. The ellipse is a complete subpath, with control points specified in clockwise order.</para>
		///         </remarks>
		public void AddEllipseInRect (CGRect rect)
		{
			CGContextAddEllipseInRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArc (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y, /* CGFloat */ nfloat radius, /* CGFloat */ nfloat startAngle, /* CGFloat */ nfloat endAngle, /* int */ int clockwise);

		public void AddArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
		{
			CGContextAddArc (Handle, x, y, radius, startAngle, endAngle, clockwise ? 1 : 0);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArcToPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat x1, /* CGFloat */ nfloat y1, /* CGFloat */ nfloat x2, /* CGFloat */ nfloat y2, /* CGFloat */ nfloat radius);

		public void AddArcToPoint (nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
		{
			CGContextAddArcToPoint (Handle, x1, y1, x2, y2, radius);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddPath (/* CGContextRef */ IntPtr c, /* CGPathRef */ IntPtr path);

		/// <param name="path">The <see cref="T:CoreGraphics.CGPath" /> to be added.</param>
		///         <summary>Adds the specified path to the current path.</summary>
		///         <remarks>To be added.</remarks>
		public void AddPath (CGPath path)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			CGContextAddPath (Handle, path.Handle);
			GC.KeepAlive (path);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextReplacePathWithStrokedPath (/* CGContextRef */ IntPtr c);

		/// <summary>Replaces the current path with the stroked version of the path, based on the stroke paremeters.</summary>
		///         <remarks>To be added.</remarks>
		public void ReplacePathWithStrokedPath ()
		{
			CGContextReplacePathWithStrokedPath (Handle);
		}


		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGContextIsPathEmpty (/* CGContextRef */ IntPtr context);

		/// <summary>Whether the current path contains any subpaths.</summary>
		///         <returns>
		///           <see langword="true" /> if the current path does not contain any subpaths.</returns>
		///         <remarks>To be added.</remarks>
		public bool IsPathEmpty ()
		{
			return CGContextIsPathEmpty (Handle) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextGetPathCurrentPoint (/* CGContextRef */ IntPtr context);

		/// <summary>The current point in the <see cref="T:CoreGraphics.CGContext" />'s path.</summary>
		///         <returns>The current point in the <see cref="T:CoreGraphics.CGContext" />'s path.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint GetPathCurrentPoint ()
		{
			return CGContextGetPathCurrentPoint (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextGetPathBoundingBox (/* CGContextRef */ IntPtr context);

		/// <summary>Returns the bounding box for the current path.</summary>
		///         <returns>The bounding box for the current path.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect GetPathBoundingBox ()
		{
			return CGContextGetPathBoundingBox (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGContextPathContainsPoint (/* CGContextRef */ IntPtr context, CGPoint point, CGPathDrawingMode mode);

		/// <param name="point">To be added.</param>
		///         <param name="mode">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool PathContainsPoint (CGPoint point, CGPathDrawingMode mode)
		{
			return CGContextPathContainsPoint (Handle, point, mode) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPath (/* CGContextRef */ IntPtr context, CGPathDrawingMode mode);

		/// <param name="mode">The <see cref="T:CoreGraphics.CGPathDrawingMode" /> for the the path.</param>
		///         <summary>Draws the <see cref="T:CoreGraphics.CGContext" />'s current path.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawPath (CGPathDrawingMode mode)
		{
			CGContextDrawPath (Handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillPath (/* CGContextRef */ IntPtr c);

		/// <summary>Fills the current path, using Non-Zero Winding rule.</summary>
		///         <remarks>
		///           <para>For an explanation of the Even-Odd and Non-Zero Winding rule, see <see cref="T:CoreGraphics.CGPathDrawingMode" />.</para>
		///         </remarks>
		public void FillPath ()
		{
			CGContextFillPath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOFillPath (/* CGContextRef */ IntPtr c);

		/// <summary>Fills the current path, using the Even-Odd rule.</summary>
		///         <remarks>
		///           <para>For an explanation of the Even-Odd and Non-Zero Winding rule, see <see cref="T:CoreGraphics.CGPathDrawingMode" />.</para>
		///         </remarks>
		public void EOFillPath ()
		{
			CGContextEOFillPath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokePath (/* CGContextRef */ IntPtr c);

		/// <summary>Strokes the current path. Afterwards, the current path is reset.</summary>
		///         <remarks>To be added.</remarks>
		public void StrokePath ()
		{
			CGContextStrokePath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">The <see cref="T:System.Drawing.RectangleF" /> to be filled.</param>
		///         <summary>Paints the specified <paramref name="rect" />.</summary>
		///         <remarks>To be added.</remarks>
		public void FillRect (CGRect rect)
		{
			CGContextFillRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRects (/* CGContextRef */ IntPtr c, CGRect [] rects, /* size_t */ nint count);

		/// <param name="rects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void ContextFillRects (CGRect [] rects)
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			CGContextFillRects (Handle, rects, rects.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">The rectangle to be stroked.</param>
		///         <summary>Strokes the specified <paramref name="rect" />. Afterwards, the current path is reset.</summary>
		///         <remarks>To be added.</remarks>
		public void StrokeRect (CGRect rect)
		{
			CGContextStrokeRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRectWithWidth (/* CGContextRef */ IntPtr c, CGRect rect, /* GCFloat */ nfloat width);

		public void StrokeRectWithWidth (CGRect rect, nfloat width)
		{
			CGContextStrokeRectWithWidth (Handle, rect, width);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClearRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">The rectangle to clear.</param>
		///         <summary>Paints the rectangle transparently.</summary>
		///         <remarks>
		///           <para>This method should only be used in window and bitmap contexts. In those situations, it effectively clears the <paramref name="rect" />.</para>
		///         </remarks>
		public void ClearRect (CGRect rect)
		{
			CGContextClearRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillEllipseInRect (/* CGContextRef */ IntPtr context, CGRect rect);

		/// <param name="rect">The <see cref="T:System.Drawing.RectangleF" /> defining the ellipse's extent.</param>
		///         <summary>Paints the ellipse defined by <paramref name="rect" />. Afterwards, the current path is reset.</summary>
		///         <remarks>To be added.</remarks>
		public void FillEllipseInRect (CGRect rect)
		{
			CGContextFillEllipseInRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeEllipseInRect (/* CGContextRef */ IntPtr context, CGRect rect);

		/// <param name="rect">The <see cref="T:System.Drawing.RectangleF" /> defining the ellipse's extent.</param>
		///         <summary>Strokes the ellipse defined by <paramref name="rect" />. Afterwards, the current path is reset.</summary>
		///         <remarks>To be added.</remarks>
		public void StrokeEllipseInRect (CGRect rect)
		{
			CGContextStrokeEllipseInRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeLineSegments (/* CGContextRef __nullable */ IntPtr c,
			/* const CGPoint* __nullable */ CGPoint []? points,
			/* size_t */ nint count);

		/// <param name="points">An array of points, defining starting and ending positions of the lines. The array must contain an even number of points.</param>
		///         <summary>Strokes the lines defined by the pairs in <paramref name="points" />. Afterwards, the current path is reset.</summary>
		///         <remarks>To be added.</remarks>
		public void StrokeLineSegments (CGPoint []? points)
		{
			CGContextStrokeLineSegments (Handle, points, points is null ? 0 : points.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClip (/* CGContextRef */ IntPtr c);

		/// <summary>Sets the current path of the graphics context to be the clipping path.</summary>
		///         <remarks>
		///           <para>The current path is changed to become the current clipping path, with interiors determined by the "non-zero winding rule" (see <see cref="T:CoreGraphics.CGPathDrawingMode" />). Any open subpaths are closed, as if the developer had called <see cref="M:CoreGraphics.CGContext.ClosePath" />. The current path of the <see cref="T:CoreGraphics.CGContext" /> is then reset.</para>
		///         </remarks>
		public void Clip ()
		{
			CGContextClip (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOClip (/* CGContextRef */ IntPtr c);

		/// <summary>Modifies the current clipping path, using the Even-Odd rule.</summary>
		///         <remarks>
		///           <para>For an explanation of the Even-Odd and Non-Zero Winding rule, see <see cref="T:CoreGraphics.CGPathDrawingMode" />.</para>
		///         </remarks>
		public void EOClip ()
		{
			CGContextEOClip (Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextResetClip (/* CGContextRef */ IntPtr c);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public void ResetClip ()
		{
			CGContextResetClip (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToMask (/* CGContextRef */ IntPtr c, CGRect rect,
			/* CGImageRef __nullable */ IntPtr mask);

		/// <param name="rect">To be added.</param>
		///         <param name="mask">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void ClipToMask (CGRect rect, CGImage? mask)
		{
			CGContextClipToMask (Handle, rect, mask.GetHandle ());
			GC.KeepAlive (mask);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextGetClipBoundingBox (/* CGContextRef */ IntPtr c);

		/// <summary>The bounding box of the current clipping path.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect GetClipBoundingBox ()
		{
			return CGContextGetClipBoundingBox (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRect (/* CGContextRef */ IntPtr c, CGRect rect);

		/// <param name="rect">A rectangle.</param>
		///         <summary>Modifies the clipping path to be the intersection of the current path and the supplied rectangle.</summary>
		///         <remarks>To be added.</remarks>
		public void ClipToRect (CGRect rect)
		{
			CGContextClipToRect (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRects (/* CGContextRef */ IntPtr c, CGRect [] rects, /* size_t */ nint count);

		/// <param name="rects">An array of rectangles.</param>
		///         <summary>Modifies the current clipping path to be the insersection of the current clipping path and the region specified by the supplied rectangles.</summary>
		///         <remarks>To be added.</remarks>
		public void ClipToRects (CGRect [] rects)
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			CGContextClipToRects (Handle, rects, rects.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorWithColor (/* CGContextRef */ IntPtr c,
			/* CGColorRef __nullable */ IntPtr color);

		/// <param name="color">The desired <see cref="T:CoreGraphics.CGColor" />.</param>
		///         <summary>Sets the fill color to the specified <see cref="T:CoreGraphics.CGColor" />.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFillColor (CGColor? color)
		{
			CGContextSetFillColorWithColor (Handle, color.GetHandle ());
			GC.KeepAlive (color);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorWithColor (/* CGContextRef */ IntPtr c,
			/* CGColorRef __nullable */ IntPtr color);

		/// <param name="color">The desired <see cref="T:CoreGraphics.CGColor" />.</param>
		///         <summary>Sets the stroke color.</summary>
		///         <remarks>To be added.</remarks>
		public void SetStrokeColor (CGColor? color)
		{
			CGContextSetStrokeColorWithColor (Handle, color.GetHandle ());
			GC.KeepAlive (color);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorSpace (/* CGContextRef */ IntPtr context,
			/* CGColorSpaceRef __nullable */ IntPtr space);

		/// <param name="space">The desired <see cref="T:CoreGraphics.CGColorSpace" />.</param>
		///         <summary>Specifies the <see cref="T:CoreGraphics.CGColorSpace" /> to be used in the context.</summary>
		///         <remarks>
		///           <para>This method must be called prior to using <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=M:CoreGraphics.CGContext.SetFillColor(float[])&amp;scope=Xamarin" title="M:CoreGraphics.CGContext.SetFillColor(float[])">M:CoreGraphics.CGContext.SetFillColor(float[])</a></format>.</para>
		///         </remarks>
		public void SetFillColorSpace (CGColorSpace? space)
		{
			CGContextSetFillColorSpace (Handle, space.GetHandle ());
			GC.KeepAlive (space);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorSpace (/* CGContextRef */ IntPtr context,
			/* CGColorSpaceRef __nullable */ IntPtr space);

		/// <param name="space">The desired <see cref="T:CoreGraphics.CGColorSpace" />.</param>
		///         <summary>Sets the <see cref="T:CoreGraphics.CGColorSpace" /> to be used with <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=M:CoreGraphics.CGContext.SetStrokeColor(float[])&amp;scope=Xamarin" title="M:CoreGraphics.CGContext.SetStrokeColor(float[])">M:CoreGraphics.CGContext.SetStrokeColor(float[])</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		public void SetStrokeColorSpace (CGColorSpace? space)
		{
			CGContextSetStrokeColorSpace (Handle, space.GetHandle ());
			GC.KeepAlive (space);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe void CGContextSetFillColor (/* CGContextRef */ IntPtr context,
			/* const CGFloat * __nullable */ nfloat* components);

		public void SetFillColor (nfloat []? components)
		{
			unsafe {
				fixed (nfloat* componentsPtr = components) {
					CGContextSetFillColor (Handle, componentsPtr);
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe void CGContextSetStrokeColor (/* CGContextRef */ IntPtr context,
			/* const CGFloat * __nullable */ nfloat* components);

		public void SetStrokeColor (nfloat []? components)
		{
			unsafe {
				fixed (nfloat* componentsPtr = components) {
					CGContextSetStrokeColor (Handle, componentsPtr);
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe void CGContextSetFillPattern (/* CGContextRef */ IntPtr context,
			/* CGPatternRef __nullable */ IntPtr pattern, /* const CGFloat * __nullable */ nfloat* components);

		public void SetFillPattern (CGPattern pattern, nfloat []? components)
		{
			unsafe {
				fixed (nfloat* componentsPtr = components) {
					CGContextSetFillPattern (Handle, pattern.GetHandle (), componentsPtr);
					GC.KeepAlive (pattern);
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe void CGContextSetStrokePattern (/* CGContextRef */ IntPtr context,
			/* CGPatternRef __nullable */ IntPtr pattern, /* const CGFloat * __nullable */ nfloat* components);

		public void SetStrokePattern (CGPattern? pattern, nfloat []? components)
		{
			unsafe {
				fixed (nfloat* componentsPtr = components) {
					CGContextSetStrokePattern (Handle, pattern.GetHandle (), componentsPtr);
					GC.KeepAlive (pattern);
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetPatternPhase (/* CGContextRef */ IntPtr context, CGSize phase);

		/// <param name="phase">The pattern's origin, in user space.</param>
		///         <summary>Translates the pattern prior to beginning to tile it.</summary>
		///         <remarks>
		///           <para>The default <paramref name="phase" /> is [0,0]. The <paramref name="phase" /> is specified in user space and translates the pattern in X and Y before the pattern tiling begins.</para>
		///         </remarks>
		public void SetPatternPhase (CGSize phase)
		{
			CGContextSetPatternPhase (Handle, phase);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayFillColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat gray, /* CGFloat */ nfloat alpha);

		public void SetFillColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayFillColor (Handle, gray, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayStrokeColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat gray, /* CGFloat */ nfloat alpha);

		public void SetStrokeColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayStrokeColor (Handle, gray, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBFillColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat red, /* CGFloat */ nfloat green, /* CGFloat */ nfloat blue, /* CGFloat */ nfloat alpha);

		public void SetFillColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBFillColor (Handle, red, green, blue, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBStrokeColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat red, /* CGFloat */ nfloat green, /* CGFloat */ nfloat blue, /* CGFloat */ nfloat alpha);

		public void SetStrokeColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBStrokeColor (Handle, red, green, blue, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKFillColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat cyan, /* CGFloat */ nfloat magenta, /* CGFloat */ nfloat yellow, /* CGFloat */ nfloat black, /* CGFloat */ nfloat alpha);

		public void SetFillColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKFillColor (Handle, cyan, magenta, yellow, black, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKStrokeColor (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat cyan, /* CGFloat */ nfloat magenta, /* CGFloat */ nfloat yellow, /* CGFloat */ nfloat black, /* CGFloat */ nfloat alpha);

		public void SetStrokeColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKStrokeColor (Handle, cyan, magenta, yellow, black, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRenderingIntent (/* CGContextRef */ IntPtr context, CGColorRenderingIntent intent);

		/// <param name="intent">The desired <see cref="T:CoreGraphics.CGColorRenderingIntent" />.</param>
		///         <summary>How colors outside the destination color space are handled.</summary>
		///         <remarks>
		///           <para>The default rendering intent is <see cref="F:CoreGraphics.CGColorRenderingIntent.RelativeColorimetric" /> for everyting but samples images, which are rendered with <see cref="F:CoreGraphics.CGColorRenderingIntent.Perceptual" />.</para>
		///         </remarks>
		public void SetRenderingIntent (CGColorRenderingIntent intent)
		{
			CGContextSetRenderingIntent (Handle, intent);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawImage (/* CGContextRef */ IntPtr c, CGRect rect,
			/* CGImageRef __nullable */ IntPtr image);

		/// <param name="rect">To be added.</param>
		///         <param name="image">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawImage (CGRect rect, CGImage? image)
		{
			CGContextDrawImage (Handle, rect, image.GetHandle ());
			GC.KeepAlive (image);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawTiledImage (/* CGContextRef */ IntPtr c, CGRect rect,
			/* CGImageRef __nullable */ IntPtr image);

		/// <param name="rect">To be added.</param>
		///         <param name="image">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawTiledImage (CGRect rect, CGImage? image)
		{
			CGContextDrawTiledImage (Handle, rect, image.GetHandle ());
			GC.KeepAlive (image);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGInterpolationQuality CGContextGetInterpolationQuality (/* CGContextRef */ IntPtr context);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetInterpolationQuality (/* CGContextRef */ IntPtr context, CGInterpolationQuality quality);

		/// <summary>A hint for the level of quality used when interpolating images (for example, when scaling).</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>
		///             <see cref="P:CoreGraphics.CGContext.InterpolationQuality" /> is only a hint. Not all contexts support all <see cref="T:CoreGraphics.CGInterpolationQuality" /> values.</para>
		///         </remarks>
		public CGInterpolationQuality InterpolationQuality {
			get {
				return CGContextGetInterpolationQuality (Handle);
			}

			set {
				CGContextSetInterpolationQuality (Handle, value);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadowWithColor (/* CGContextRef __nullable */ IntPtr context, CGSize offset,
			/* CGFloat */ nfloat blur, /* CGColorRef __nullable */ IntPtr color);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadow (IntPtr context, CGSize offset, nfloat blur);

		public void SetShadow (CGSize offset, nfloat blur, CGColor? color = null)
		{
			if (color is null)
				CGContextSetShadow (Handle, offset, blur);
			else {
				CGContextSetShadowWithColor (Handle, offset, blur, color.Handle);
				GC.KeepAlive (color);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLinearGradient (/* CGContextRef __nullable */ IntPtr context,
			/* CGGradientRef __nullable */ IntPtr gradient, CGPoint startPoint, CGPoint endPoint,
			CGGradientDrawingOptions options);

		/// <param name="gradient">To be added.</param>
		///         <param name="startPoint">To be added.</param>
		///         <param name="endPoint">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawLinearGradient (CGGradient? gradient, CGPoint startPoint, CGPoint endPoint, CGGradientDrawingOptions options)
		{
			CGContextDrawLinearGradient (Handle, gradient.GetHandle (), startPoint, endPoint, options);
			GC.KeepAlive (gradient);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawRadialGradient (/* CGContextRef __nullable */ IntPtr context,
			/* CGGradientRef __nullable */ IntPtr gradient,
			CGPoint startCenter, /* CGFloat */ nfloat startRadius,
			CGPoint endCenter, /* CGFloat */ nfloat endRadius, CGGradientDrawingOptions options);

		public void DrawRadialGradient (CGGradient? gradient, CGPoint startCenter, nfloat startRadius, CGPoint endCenter, nfloat endRadius, CGGradientDrawingOptions options)
		{
			CGContextDrawRadialGradient (Handle, gradient.GetHandle (), startCenter, startRadius, endCenter, endRadius, options);
			GC.KeepAlive (gradient);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawShading (/* CGContextRef */ IntPtr context,
			/* CGShadingRef __nullable */ IntPtr shading);

		/// <param name="shading">The <see cref="T:CoreGraphics.CGShading" /> to be drawn.</param>
		///         <summary>Renders the specified <paramref name="shading" />.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawShading (CGShading? shading)
		{
			CGContextDrawShading (Handle, shading.GetHandle ());
			GC.KeepAlive (shading);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCharacterSpacing (/* CGContextRef */ IntPtr context, /* GCFloat */ nfloat spacing);

		public void SetCharacterSpacing (nfloat spacing)
		{
			CGContextSetCharacterSpacing (Handle, spacing);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextPosition (/* CGContextRef */ IntPtr c, /* GCFloat */ nfloat x, /* GCFloat */ nfloat y);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextGetTextPosition (/* CGContextRef */ IntPtr context);

		/// <summary>The location, in user space coordinates, at which to draw text.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGPoint TextPosition {
			get {
				return CGContextGetTextPosition (Handle);
			}
			set {
				CGContextSetTextPosition (Handle, value.X, value.Y);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextMatrix (/* CGContextRef */ IntPtr c, CGAffineTransform t);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetTextMatrix (/* CGContextRef */ IntPtr c);

		/// <summary>Defines the transform between text space and user space. Independent of the <see cref="T:CoreGraphics.CGContext" />'s state.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGAffineTransform TextMatrix {
			get {
				return CGContextGetTextMatrix (Handle);
			}
			set {
				CGContextSetTextMatrix (Handle, value);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextDrawingMode (/* CGContextRef */ IntPtr c, CGTextDrawingMode mode);

		/// <param name="mode">The desired <see cref="T:CoreGraphics.CGTextDrawingMode" />.</param>
		///         <summary>Specifies how glyphs should be rendered.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTextDrawingMode (CGTextDrawingMode mode)
		{
			CGContextSetTextDrawingMode (Handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFont (/* CGContextRef */ IntPtr c, /* CGFontRef __nullable */ IntPtr font);

		/// <param name="font">The <see cref="T:CoreGraphics.CGFont" /> used for the context.</param>
		///         <summary>Sets the <see cref="T:CoreGraphics.CGFont" /> used to render text.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFont (CGFont? font)
		{
			CGContextSetFont (Handle, font.GetHandle ());
			GC.KeepAlive (font);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFontSize (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat size);

		public void SetFontSize (nfloat size)
		{
			CGContextSetFontSize (Handle, size);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSelectFont (/* CGContextRef */ IntPtr c,
			/* const char* __nullable */ IntPtr name, /* CGFloat */ nfloat size, CGTextEncoding textEncoding);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void SelectFont (string? name, nfloat size, CGTextEncoding textEncoding)
		{
			using var namePtr = new TransientString (name);
			CGContextSelectFont (Handle, namePtr, size, textEncoding);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPositions (/* CGContextRef __nullable */ IntPtr context,
			/* const CGGlyph * __nullable */ ushort []? glyphs,
			/* const CGPoint * __nullable */ CGPoint []? positions, /* size_t */ nint count);

		/// <param name="glyphs">To be added.</param>
		///         <param name="positions">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void ShowGlyphsAtPositions (ushort []? glyphs, CGPoint []? positions, int count = -1)
		{
			if (glyphs is null)
				count = 0;
			else if (count < 0)
				count = glyphs.Length;
			CGContextShowGlyphsAtPositions (Handle, glyphs, positions, count);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText (/* CGContextRef */ IntPtr c, /* const char* __nullable */ IntPtr s, /* size_t */ nint length);

		/// <param name="str">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowText (string? str, int count)
		{
			if (str is null)
				count = 0;
			else if (count > str.Length)
				throw new ArgumentException (nameof (count));
			using var strPtr = new TransientString (str);
			CGContextShowText (Handle, strPtr, count);
		}

		/// <param name="str">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowText (string? str)
		{
			using var strPtr = new TransientString (str);
			CGContextShowText (Handle, strPtr, str is null ? 0 : str.Length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText (/* CGContextRef */ IntPtr c, /* const char* __nullable */ byte []? bytes, /* size_t */ nint length);

		/// <param name="bytes">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowText (byte []? bytes, int count)
		{
			if (bytes is null)
				count = 0;
			else if (count > bytes.Length)
				throw new ArgumentException (nameof (count));
			CGContextShowText (Handle, bytes, count);
		}

		/// <param name="bytes">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowText (byte []? bytes)
		{
			CGContextShowText (Handle, bytes, bytes is null ? 0 : bytes.Length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint (/* CGContextRef __nullable */ IntPtr c, /* CGFloat */ nfloat x,
			/* CGFloat */ nfloat y, /* const char* __nullable */ IntPtr str, /* size_t */ nint length);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		public void ShowTextAtPoint (nfloat x, nfloat y, string? str, int length)
		{
			using var strPtr = new TransientString (str);
			CGContextShowTextAtPoint (Handle, x, y, strPtr, length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		public void ShowTextAtPoint (nfloat x, nfloat y, string? str)
		{
			using var strPtr = new TransientString (str);
			CGContextShowTextAtPoint (Handle, x, y, strPtr, str is null ? 0 : str.Length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint (/* CGContextRef */ IntPtr c, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y, /* const char* */ byte []? bytes, /* size_t */ nint length);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		public void ShowTextAtPoint (nfloat x, nfloat y, byte []? bytes, int length)
		{
			CGContextShowTextAtPoint (Handle, x, y, bytes, length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the 'CoreText' API instead.")]
		public void ShowTextAtPoint (nfloat x, nfloat y, byte []? bytes)
		{
			CGContextShowTextAtPoint (Handle, x, y, bytes, bytes is null ? 0 : bytes.Length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphs (/* CGContextRef __nullable */ IntPtr c,
			/* const CGGlyph * __nullable */ ushort []? glyphs, /* size_t */ nint count);

		/// <param name="glyphs">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowGlyphs (ushort []? glyphs)
		{
			CGContextShowGlyphs (Handle, glyphs, glyphs is null ? 0 : glyphs.Length);
		}

		/// <param name="glyphs">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>This method has been deprecated in favor of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Core%20Text&amp;scope=Xamarin" title="N:CoreText">N:CoreText</a></format>.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowGlyphs (ushort []? glyphs, int count)
		{
			if (glyphs is null)
				count = 0;
			else if (count > glyphs.Length)
				throw new ArgumentException (nameof (count));
			CGContextShowGlyphs (Handle, glyphs, count);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPoint (/* CGContextRef */ IntPtr context, /* CGFloat */ nfloat x,
			/* CGFloat */ nfloat y, /* const CGGlyph * __nullable */ ushort []? glyphs, /* size_t */ nint count);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowGlyphsAtPoint (nfloat x, nfloat y, ushort []? glyphs, int count)
		{
			if (glyphs is null)
				count = 0;
			else if (count > glyphs.Length)
				throw new ArgumentException (nameof (count));
			CGContextShowGlyphsAtPoint (Handle, x, y, glyphs, count);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowGlyphsAtPoint (nfloat x, nfloat y, ushort []? glyphs)
		{
			CGContextShowGlyphsAtPoint (Handle, x, y, glyphs, glyphs is null ? 0 : glyphs.Length);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9")]
		[ObsoletedOSPlatform ("ios7.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsWithAdvances (/* CGContextRef __nullable */ IntPtr c,
			/* const CGGlyph * __nullable */ ushort []? glyphs,
			/* const CGSize * __nullable */ CGSize []? advances, /* size_t */ nint count);

		/// <param name="glyphs">To be added.</param>
		///         <param name="advances">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.9", "Use the 'CoreText' API instead.")]
		[ObsoletedOSPlatform ("ios7.0", "Use the 'CoreText' API instead.")]
		public void ShowGlyphsWithAdvances (ushort []? glyphs, CGSize []? advances, int count)
		{
			if (glyphs is null)
				count = 0;
			if (count > (glyphs?.Length ?? 0) || count > (advances?.Length ?? 0))
				throw new ArgumentException (nameof (count));
			CGContextShowGlyphsWithAdvances (Handle, glyphs, advances, count);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPDFPage (/* CGContextRef __nullable */ IntPtr c,
			/* CGPDFPageRef __nullable */ IntPtr page);

		/// <include file="../../docs/api/CoreGraphics/CGContext.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGContext.DrawPDFPage(CoreGraphics.CGPDFPage)']/*" />
		public void DrawPDFPage (CGPDFPage? page)
		{
			CGContextDrawPDFPage (Handle, page.GetHandle ());
			GC.KeepAlive (page);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage (/* CGContextRef __nullable */ IntPtr c,
			/* const CGRect * __nullable */ CGRect* mediaBox);

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void BeginPage (CGRect? rect)
		{
			if (rect.HasValue) {
				CGRect v = rect.Value;
				CGContextBeginPage (Handle, &v);
			} else {
				CGContextBeginPage (Handle, null);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEndPage (/* CGContextRef __nullable */ IntPtr c);

		/// <summary>Called to indicate the end of a page in a page-based context.</summary>
		///         <remarks>To be added.</remarks>
		public void EndPage ()
		{
			CGContextEndPage (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFlush (/* CGContextRef __nullable */ IntPtr c);

		/// <summary>Forces all pending drawing to be rendered.</summary>
		///         <remarks>
		///           <para>Calling this method is not necessary under normal circumstances. Calling this method frequently may harm performance.</para>
		///         </remarks>
		public void Flush ()
		{
			CGContextFlush (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSynchronize (/* CGContextRef __nullable */ IntPtr c);

		/// <summary>Marks a <see cref="T:CoreGraphics.CGContext" /> for update.</summary>
		///         <remarks>
		///           <para>Flushes all drawing operations since the last update. Does nothing for PDF and bitmap contexts. App devs do not typically need to call this method.</para>
		///         </remarks>
		public void Synchronize ()
		{
			CGContextSynchronize (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldAntialias (/* CGContextRef */ IntPtr context, byte shouldAntialias);

		/// <param name="shouldAntialias">
		///           <see langword="true" /> if antialiasing should be used.</param>
		///         <summary>Used in conjunction with <see cref="M:CoreGraphics.CGContext.SetAllowsAntialiasing(System.Boolean)" /> to enable or disable antialiasing.</summary>
		///         <remarks>To be added.</remarks>
		public void SetShouldAntialias (bool shouldAntialias)
		{
			CGContextSetShouldAntialias (Handle, shouldAntialias.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsAntialiasing (/* CGContextRef */ IntPtr context, byte allowsAntialiasing);
		/// <param name="allowsAntialiasing">
		///           <see langword="true" /> if antialiasing should be allowed.</param>
		///         <summary>Whether the context allows antialiasing.</summary>
		///         <remarks>
		///           <para>This property works in conjunction with <see cref="M:CoreGraphics.CGContext.SetShouldAntialias(System.Boolean)" />. Only if both values are <see langword="true" /> will antialiasing occur.</para>
		///         </remarks>
		public void SetAllowsAntialiasing (bool allowsAntialiasing)
		{
			CGContextSetAllowsAntialiasing (Handle, allowsAntialiasing.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSmoothFonts (/* CGContextRef */ IntPtr context, byte shouldSmoothFonts);

		/// <param name="shouldSmoothFonts">
		///           <see langword="true" /> if fonts should be smoothed.</param>
		///         <summary>Used in conjunction with <see cref="M:CoreGraphics.CGContext.SetAllowsFontSmoothing(System.Boolean)" /> to enable or disable font smoothing.</summary>
		///         <remarks>To be added.</remarks>
		public void SetShouldSmoothFonts (bool shouldSmoothFonts)
		{
			CGContextSetShouldSmoothFonts (Handle, shouldSmoothFonts.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetUserSpaceToDeviceSpaceTransform (/* CGContextRef */ IntPtr context);

		/// <summary>The <see cref="T:CoreGraphics.CGAffineTransform" /> that maps user-space coordinates into device-space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGAffineTransform GetUserSpaceToDeviceSpaceTransform ()
		{
			return CGContextGetUserSpaceToDeviceSpaceTransform (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextConvertPointToDeviceSpace (/* CGContextRef */ IntPtr context, CGPoint point);

		/// <param name="point">A point in user-space coordinates.</param>
		///         <summary>Returns a new <see cref="T:System.Drawing.PointF" /> that converts the user-space <paramref name="point" /> into device space.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint PointToDeviceSpace (CGPoint point)
		{
			return CGContextConvertPointToDeviceSpace (Handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextConvertPointToUserSpace (/* CGContextRef */ IntPtr context, CGPoint point);

		/// <param name="point">A point.</param>
		///         <summary>Converts a point from device space coordinates to user space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint ConvertPointToUserSpace (CGPoint point)
		{
			return CGContextConvertPointToUserSpace (Handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGContextConvertSizeToDeviceSpace (/* CGContextRef */ IntPtr context, CGSize size);

		/// <param name="size">A size.</param>
		///         <summary>Converts a size from user space coordinates to device space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize ConvertSizeToDeviceSpace (CGSize size)
		{
			return CGContextConvertSizeToDeviceSpace (Handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGContextConvertSizeToUserSpace (/* CGContextRef */ IntPtr context, CGSize size);

		/// <param name="size">A size.</param>
		///         <summary>Converts a size from device space coordinates to user space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize ConvertSizeToUserSpace (CGSize size)
		{
			return CGContextConvertSizeToUserSpace (Handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextConvertRectToDeviceSpace (/* CGContextRef */ IntPtr context, CGRect rect);

		/// <param name="rect">A rectangle.</param>
		///         <summary>Converts a rectangle from user space coordinates to device space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect ConvertRectToDeviceSpace (CGRect rect)
		{
			return CGContextConvertRectToDeviceSpace (Handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextConvertRectToUserSpace (/* CGContextRef */ IntPtr context, CGRect rect);

		/// <param name="rect">A rectangle</param>
		///         <summary>Converts a rectangle from device space coordinates to user space coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect ConvertRectToUserSpace (CGRect rect)
		{
			return CGContextConvertRectToUserSpace (Handle, rect);
		}

		// CGLayer.h
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerInRect (/* CGContextRef */ IntPtr context, CGRect rect, /* CGLayerRef */ IntPtr layer);

		/// <param name="layer">The layer to draw.</param>
		///         <param name="rect">The bounding box in user space in which to draw the layer.</param>
		///         <summary>Draws a layer into the graphics context bounded by the specified rectangle.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawLayer (CGLayer layer, CGRect rect)
		{
			if (layer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (layer));
			CGContextDrawLayerInRect (Handle, rect, layer.Handle);
			GC.KeepAlive (layer);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerAtPoint (/* CGContextRef */ IntPtr context, CGPoint rect, /* CGLayerRef */ IntPtr layer);

		/// <param name="layer">The layer to draw.</param>
		///         <param name="point">The point in user space where to draw the layer.</param>
		///         <summary>Draws a layer into the graphics context at the specified point.</summary>
		///         <remarks>To be added.</remarks>
		public void DrawLayer (CGLayer layer, CGPoint point)
		{
			if (layer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (layer));
			CGContextDrawLayerAtPoint (Handle, point, layer.Handle);
			GC.KeepAlive (layer);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPathRef */ IntPtr CGContextCopyPath (/* CGContextRef */ IntPtr context);

		/// <summary>Returns a deep copy of the current path in the current context.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPath CopyPath ()
		{
			var r = CGContextCopyPath (Handle);
			return new CGPath (r, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsFontSmoothing (/* CGContextRef */ IntPtr context, byte shouldSubpixelPositionFonts);

		/// <param name="allows">
		///           <see langword="true" /> if font smoothing should be allowed.</param>
		///         <summary>Whether the context allows font smoothing.</summary>
		///         <remarks>
		///           <para>This property works in conjunction with <see cref="M:CoreGraphics.CGContext.SetShouldSmoothFonts(System.Boolean)" />. Only if both values are <see langword="true" /> will font smoothing occur.</para>
		///         </remarks>
		public void SetAllowsFontSmoothing (bool allows)
		{
			CGContextSetAllowsFontSmoothing (Handle, allows.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsFontSubpixelPositioning (/* CGContextRef */ IntPtr context, byte allowsFontSubpixelPositioning);

		/// <param name="allows">
		///           <see langword="true" /> if glyphs need not be aligned to pixel boundaries.</param>
		///         <summary>Whether the context allows for glyphs to be aligned other than to pixel boundaries.</summary>
		///         <remarks>This property works in conjunction with <see cref="M:CoreGraphics.CGContext.SetShouldSubpixelPositionFonts(System.Boolean)" />. Only if both values are <see langword="true" /> will glyphs not be aligned to pixel boundaries.</remarks>
		public void SetAllowsSubpixelPositioning (bool allows)
		{
			CGContextSetAllowsFontSubpixelPositioning (Handle, allows.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsFontSubpixelQuantization (/* CGContextRef */ IntPtr context, byte shouldSubpixelQuantizeFonts);

		/// <param name="allows">
		///           <see langword="true" /> if subpixel quantization should be allowed</param>
		///         <summary>Whether the context allows for glyphs to be drawn at subpixel locations.</summary>
		///         <remarks>
		///           <para>This property works in conjunction with <see cref="M:CoreGraphics.CGContext.ShouldSubpixelQuantizeFonts(System.Boolean)" />, <see cref="M:CoreGraphics.CGContext.SetAllowsSubpixelPositioning(System.Boolean)" />, and <see cref="M:CoreGraphics.CGContext.SetShouldSubpixelPositionFonts(System.Boolean)" />. Only if all these values are <see langword="true" /> will subpixel quantization be allowed.</para>
		///         </remarks>
		public void SetAllowsFontSubpixelQuantization (bool allows)
		{
			CGContextSetAllowsFontSubpixelQuantization (Handle, allows.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSubpixelPositionFonts (/* CGContextRef */ IntPtr context, byte shouldSubpixelPositionFonts);

		/// <param name="shouldSubpixelPositionFonts">
		///           <see langword="true" /> if glyphs need not be aligned to pixels.</param>
		///         <summary>Used in conjunction with <see cref="M:CoreGraphics.CGContext.SetAllowsSubpixelPositioning(System.Boolean)" /> to enable or disable glyph alignment with pixels.</summary>
		///         <remarks>To be added.</remarks>
		public void SetShouldSubpixelPositionFonts (bool shouldSubpixelPositionFonts)
		{
			CGContextSetShouldSubpixelPositionFonts (Handle, shouldSubpixelPositionFonts.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSubpixelQuantizeFonts (/* CGContextRef */ IntPtr context, byte shouldSubpixelQuantizeFonts);

		/// <param name="shouldSubpixelQuantizeFonts">
		///           <see langword="true" /> if fonts should be drawn at subpixel positions.</param>
		///         <summary>With <see cref="M:CoreGraphics.CGContext.SetAllowsFontSubpixelQuantization(System.Boolean)" />, determines whether fonts should be drawn at subpixel locations.</summary>
		///         <remarks>
		///           <para>Subpixel quantization requires <see cref="M:CoreGraphics.CGContext.SetAllowsFontSubpixelQuantization(System.Boolean)" />, <see cref="M:CoreGraphics.CGContext.SetShouldSubpixelPositionFonts(System.Boolean)" />, and <see cref="M:CoreGraphics.CGContext.SetAllowsSubpixelPositioning(System.Boolean)" /> all to be <see langword="true" />.</para>
		///         </remarks>
		public void ShouldSubpixelQuantizeFonts (bool shouldSubpixelQuantizeFonts)
		{
			CGContextSetShouldSubpixelQuantizeFonts (Handle, shouldSubpixelQuantizeFonts.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginTransparencyLayer (/* CGContextRef */ IntPtr context, /* CFDictionaryRef __nullable */ IntPtr auxiliaryInfo);

		/// <param name="auxiliaryInfo">A dictionary of auxiliary information. May be <see langword="null" />.</param>
		///         <summary>With <see cref="M:CoreGraphics.CGContext.EndTransparencyLayer" />, encloses operations on a fully transparent layer.</summary>
		///         <remarks>To be added.</remarks>
		///         <altmember cref="M:CoreGraphics.CGContext.BeginTransparencyLayer" />
		public void BeginTransparencyLayer (NSDictionary? auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayer (Handle, auxiliaryInfo.GetHandle ());
			GC.KeepAlive (auxiliaryInfo);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginTransparencyLayerWithRect (/* CGContextRef */ IntPtr context, CGRect rect, /* CFDictionaryRef __nullable */ IntPtr auxiliaryInfo);

		/// <param name="rectangle">To be added.</param>
		///         <param name="auxiliaryInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginTransparencyLayer (CGRect rectangle, NSDictionary? auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayerWithRect (Handle, rectangle, auxiliaryInfo.GetHandle ());
			GC.KeepAlive (auxiliaryInfo);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEndTransparencyLayer (/* CGContextRef */ IntPtr context);

		/// <summary>Indicates the end of a transparency layer.</summary>
		///         <remarks>To be added.</remarks>
		public void EndTransparencyLayer ()
		{
			CGContextEndTransparencyLayer (Handle);
		}

		/// <include file="../../docs/api/CoreGraphics/CGContext.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGContext.AsBitmapContext']/*" />
		public CGBitmapContext AsBitmapContext ()
		{
			return new CGBitmapContext (Handle, false);
		}

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGContextDrawConicGradient (/* CGContext */ IntPtr context, /*[NullAllowed] CGGradient*/ IntPtr gradient, CGPoint center, nfloat angle);

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		public void DrawConicGradient (CGGradient? gradient, CGPoint point, nfloat angle)
		{
			CGContextDrawConicGradient (Handle, gradient.GetHandle (), point, angle);
			GC.KeepAlive (gradient);
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern byte CGContextSetEDRTargetHeadroom (/* CGContextRef __nonnull */ IntPtr context, /* float */ float headroom);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGContextGetEDRTargetHeadroom (/* CGContextRef __nonnull */ IntPtr context);

		/// <summary>Get the EDR headroom that's used when rendering HDR content.</summary>
		/// <returns>The EDR headroom.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public float GetEdrTargetHeadroom ()
		{
			return CGContextGetEDRTargetHeadroom (Handle);
		}

		/// <summary>Get the EDR headroom that's used when rendering HDR content.</summary>
		/// <param name="value">The value to set. Must be greater than 1.0f.</param>
		/// <returns>Returns true if successful, false otherwise.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public bool SetEdrTargetHeadroom (float value)
		{
			return CGContextSetEDRTargetHeadroom (Handle, value) != 0;
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* bool */ byte CGContextDrawImageApplyingToneMapping (/* CGContextRef __nonnull */ IntPtr context, CGRect r, /* CGImageRef */ IntPtr image, CGToneMapping method, /* CFDictionaryRef  __nullable */ IntPtr options);

		/// <summary>Draw the specified image onto the CGContext, applying the tone mapping and any tone mapping options.</summary>
		/// <param name="rect">The rectangle where the image is rendered.</param>
		/// <param name="image">The image to render.</param>
		/// <param name="method">The tone mapping method to use.</param>
		/// <param name="options">The tone mapping options to use.</param>
		/// <returns>Returns true if successful, otherwise false.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public bool DrawImageApplyingToneMapping (CGRect rect, CGImage image, CGToneMapping method, NSDictionary? options)
		{
			var rv = CGContextDrawImageApplyingToneMapping (Handle, rect, image.Handle, method, options.GetHandle ());
			GC.KeepAlive (image);
			GC.KeepAlive (options);
			return rv != 0;
		}

		/// <summary>Draw the specified image onto the CGContext, applying the tone mapping and any tone mapping options.</summary>
		/// <param name="rect">The rectangle where the image is rendered.</param>
		/// <param name="image">The image to render.</param>
		/// <param name="method">The tone mapping method to use.</param>
		/// <param name="options">The tone mapping options to use.</param>
		/// <returns>Returns true if successful, otherwise false.</returns>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public bool DrawImageApplyingToneMapping (CGRect rect, CGImage image, CGToneMapping method, CGToneMappingOptions? options)
		{
			var optionsDictionary = options?.Dictionary;
			var rv = CGContextDrawImageApplyingToneMapping (Handle, rect, image.Handle, method, optionsDictionary?.GetHandle () ?? IntPtr.Zero);
			GC.KeepAlive (image);
			GC.KeepAlive (optionsDictionary);
			return rv != 0;
		}
#endif // !COREBUILD
	}
}
