// 
// CGPath.cs: Implements the managed CGPath
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

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using CoreFoundation;

namespace CoreGraphics {

	// untyped enum -> CGPath.h
	/// <summary>The type of an element in a CGPath.</summary>
	///     <remarks>This is used by the callback function invoked by the CGPath.Apply method.</remarks>
	public enum CGPathElementType {
		/// <summary>This is a MoveTo operation, one point parameter.</summary>
		MoveToPoint,
		/// <summary>This is a LineTo operation, one point parameter.</summary>
		AddLineToPoint,
		/// <summary>This is an AddQuadCurveTo operation, two point parameters.</summary>
		AddQuadCurveToPoint,
		/// <summary>This is an AddCurveTo operation, three point parameters.</summary>
		AddCurveToPoint,
		/// <summary>This is a close subpath operation.</summary>
		CloseSubpath,
	}

	/// <summary>An individual element on a CGPath.</summary>
	///     <remarks>
	///       <para>
	/// Depending on the value of Type, you will use the values in Point1, Point2 and Point3.
	/// </para>
	///       <para>
	///         <list type="table">
	///           <listheader>
	///             <term>CGPathElementType</term>
	///             <description>Description</description>
	///           </listheader>
	///           <item>
	///             <term>CloseSubpath</term>
	///             <description>The end of a subpath.</description>
	///           </item>
	///           <item>
	///             <term>MoveToPoint, AddLineToPoint</term>
	///             <description>Use the Point1 value.</description>
	///           </item>
	///           <item>
	///             <term>AddQuadCurveToPoint</term>
	///             <description>Use the Point1 and Point2 values.</description>
	///           </item>
	///           <item>
	///             <term>AddCurveToPoint</term>
	///             <description>Use the Point1, Point2 and Point3 values.</description>
	///           </item>
	///         </list>
	///       </para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPath.h
	public struct CGPathElement {
		/// <summary>Determines the type of path element.</summary>
		///         <remarks>Depending on the value, the values of Point1, Point2 and Point3 will be valid.</remarks>
		public CGPathElementType Type;

		/// <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPathElement (int t)
		{
			Type = (CGPathElementType) t;
			Point1 = Point2 = Point3 = CGPoint.Empty;
		}

		// Set for MoveToPoint, AddLineToPoint, AddQuadCurveToPoint, AddCurveToPoint
		/// <summary>First point.</summary>
		///         <remarks>Used by MoveToPoint, AddLineToPoint, AddQuadCurveToPoint, AddCurveToPoint.</remarks>
		public CGPoint Point1;

		// Set for AddQuadCurveToPoint, AddCurveToPoint
		/// <summary>Second point.</summary>
		///         <remarks>AddQuadCurveToPoint, AddCurveToPoint.</remarks>
		public CGPoint Point2;

		// Set for AddCurveToPoint
		/// <summary>Third point.</summary>
		///         <remarks>Used by AddCurveToPoint.</remarks>
		public CGPoint Point3;
	}

	/// <summary>A drawing path is made up of lines, arcs, beziers that can be used to paint.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/WeatherMap/">WeatherMap</related>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/CoreAnimation/">Example_CoreAnimation</related>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/Drawing/">Example_Drawing</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGPath : NativeObject {
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGMutablePathRef */ IntPtr CGPathCreateMutable ();

		/// <summary>Creates an empty <see cref="CoreGraphics.CGPath" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGPath ()
			: base (CGPathCreateMutable (), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static /* CGMutablePathRef */ IntPtr CGPathCreateMutableCopyByTransformingPath (/* CGPathRef */ IntPtr path, /* const CGAffineTransform* */ CGAffineTransform* transform);

		/// <param name="reference">To be added.</param>
		///         <param name="transform">To be added.</param>
		///         <summary>Creates an new <see cref="CoreGraphics.CGPath" /> from the provided <paramref name="reference" /> path by applying the provided <paramref name="transform" /></summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe CGPath (CGPath reference, in CGAffineTransform transform)
#else
		public unsafe CGPath (CGPath reference, CGAffineTransform transform)
#endif
			: base (CreateMutableCopy (reference, in transform), true)
		{
			GC.KeepAlive (reference);
		}

		static unsafe IntPtr CreateMutableCopy (CGPath reference, in CGAffineTransform transform)
		{
			fixed (CGAffineTransform* transformPtr = &transform)
				return CGPathCreateMutableCopyByTransformingPath (reference.GetNonNullHandle (nameof (reference)), transformPtr);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGMutablePathRef */ IntPtr CGPathCreateMutableCopy (/* CGPathRef */ IntPtr path);

		/// <param name="basePath">To be added.</param>
		///         <summary>Creates an new <see cref="CoreGraphics.CGPath" /> from the provided <paramref name="basePath" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGPath (CGPath basePath)
			: base (CGPathCreateMutableCopy (basePath.GetNonNullHandle (nameof (basePath))), true)
		{
			GC.KeepAlive (basePath);
		}

		[Preserve (Conditional = true)]
		internal CGPath (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathRelease (/* CGPathRef */ IntPtr path);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPathRef */ IntPtr CGPathRetain (/* CGPathRef */ IntPtr path);

		protected internal override void Retain ()
		{
			CGPathRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGPathRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPathEqualToPath (/* CGPathRef */ IntPtr path1, /* CGPathRef */ IntPtr path2);

		public static bool operator == (CGPath? path1, CGPath? path2)
		{
			if (path1 is null)
				return path2 is null;
			return path1.Equals (path2);
		}

		public static bool operator != (CGPath? path1, CGPath? path2)
		{
			if (path1 is null)
				return path2 is not null;
			return !path1.Equals (path2);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			// looks weird but it's valid
			// using the Handle property would not be since there's a special function for equality
			// see Remarks in https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=net-6.0
			return 0;
		}

		/// <param name="o">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? o)
		{
			var other = o as CGPath;
			if (other is null)
				return false;

			bool result = CGPathEqualToPath (this.Handle, other.Handle) != 0;
			GC.KeepAlive (other);
			return result;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathMoveToPoint (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public unsafe void MoveToPoint (nfloat x, nfloat y)
		{
			CGPathMoveToPoint (Handle, null, x, y);
		}

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void MoveToPoint (CGPoint point)
		{
			CGPathMoveToPoint (Handle, null, point.X, point.Y);
		}

#if XAMCORE_5_0
		public unsafe void MoveToPoint (in CGAffineTransform transform, nfloat x, nfloat y)
#else
		public unsafe void MoveToPoint (CGAffineTransform transform, nfloat x, nfloat y)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathMoveToPoint (Handle, transformPtr, x, y);
		}

		/// <param name="transform">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void MoveToPoint (in CGAffineTransform transform, CGPoint point)
#else
		public unsafe void MoveToPoint (CGAffineTransform transform, CGPoint point)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathMoveToPoint (Handle, transformPtr, point.X, point.Y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddLineToPoint (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public unsafe void AddLineToPoint (nfloat x, nfloat y)
		{
			CGPathAddLineToPoint (Handle, null, x, y);
		}

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddLineToPoint (CGPoint point)
		{
			CGPathAddLineToPoint (Handle, null, point.X, point.Y);
		}

#if XAMCORE_5_0
		public unsafe void AddLineToPoint (in CGAffineTransform transform, nfloat x, nfloat y)
#else
		public unsafe void AddLineToPoint (CGAffineTransform transform, nfloat x, nfloat y)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddLineToPoint (Handle, transformPtr, x, y);
		}

		/// <param name="transform">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddLineToPoint (in CGAffineTransform transform, CGPoint point)
#else
		public unsafe void AddLineToPoint (CGAffineTransform transform, CGPoint point)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddLineToPoint (Handle, transformPtr, point.X, point.Y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddQuadCurveToPoint (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat cpx, /* CGFloat */ nfloat cpy, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

		public unsafe void AddQuadCurveToPoint (nfloat cpx, nfloat cpy, nfloat x, nfloat y)
		{
			CGPathAddQuadCurveToPoint (Handle, null, cpx, cpy, x, y);
		}

#if XAMCORE_5_0
		public unsafe void AddQuadCurveToPoint (in CGAffineTransform transform, nfloat cpx, nfloat cpy, nfloat x, nfloat y)
#else
		public unsafe void AddQuadCurveToPoint (CGAffineTransform transform, nfloat cpx, nfloat cpy, nfloat x, nfloat y)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddQuadCurveToPoint (Handle, transformPtr, cpx, cpy, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddCurveToPoint (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat cp1x, /* CGFloat */ nfloat cp1y, /* CGFloat */ nfloat cp2x, /* CGFloat */ nfloat cp2y, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y);

#if XAMCORE_5_0
		public unsafe void AddCurveToPoint (in CGAffineTransform transform, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
#else
		public unsafe void AddCurveToPoint (CGAffineTransform transform, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddCurveToPoint (Handle, transformPtr, cp1x, cp1y, cp2x, cp2y, x, y);
		}

		/// <param name="transform">To be added.</param>
		///         <param name="cp1">To be added.</param>
		///         <param name="cp2">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddCurveToPoint (in CGAffineTransform transform, CGPoint cp1, CGPoint cp2, CGPoint point)
#else
		public unsafe void AddCurveToPoint (CGAffineTransform transform, CGPoint cp1, CGPoint cp2, CGPoint point)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddCurveToPoint (Handle, transformPtr, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}

		public unsafe void AddCurveToPoint (nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
		{
			CGPathAddCurveToPoint (Handle, null, cp1x, cp1y, cp2x, cp2y, x, y);
		}

		/// <param name="cp1">To be added.</param>
		///         <param name="cp2">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddCurveToPoint (CGPoint cp1, CGPoint cp2, CGPoint point)
		{
			CGPathAddCurveToPoint (Handle, null, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathCloseSubpath (/* CGMutablePathRef */ IntPtr path);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void CloseSubpath ()
		{
			CGPathCloseSubpath (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddRect (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, CGRect rect);

		/// <param name="transform">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddRect (in CGAffineTransform transform, CGRect rect)
#else
		public unsafe void AddRect (CGAffineTransform transform, CGRect rect)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddRect (Handle, transformPtr, rect);
		}

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddRect (CGRect rect)
		{
			CGPathAddRect (Handle, null, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddRects (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, CGRect [] rects, /* size_t */ nint count);

		/// <param name="m">To be added.</param>
		///         <param name="rects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddRects (in CGAffineTransform m, CGRect [] rects)
#else
		public unsafe void AddRects (CGAffineTransform m, CGRect [] rects)
#endif
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddRects (Handle, transformPtr, rects, rects.Length);
		}

		/// <param name="m">To be added.</param>
		///         <param name="rects">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddRects (in CGAffineTransform m, CGRect [] rects, int count)
#else
		public unsafe void AddRects (CGAffineTransform m, CGRect [] rects, int count)
#endif
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			if (count > rects.Length)
				throw new ArgumentException (nameof (count));
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddRects (Handle, transformPtr, rects, count);
		}

		/// <param name="rects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddRects (CGRect [] rects)
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			CGPathAddRects (Handle, null, rects, rects.Length);
		}

		/// <param name="rects">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddRects (CGRect [] rects, int count)
		{
			if (rects is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (rects));
			if (count > rects.Length)
				throw new ArgumentException (nameof (count));
			CGPathAddRects (Handle, null, rects, count);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddLines (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, CGPoint [] points, /* size_t */ nint count);

		/// <param name="m">To be added.</param>
		///         <param name="points">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddLines (in CGAffineTransform m, CGPoint [] points)
#else
		public unsafe void AddLines (CGAffineTransform m, CGPoint [] points)
#endif
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddLines (Handle, transformPtr, points, points.Length);
		}

		/// <param name="m">To be added.</param>
		///         <param name="points">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddLines (in CGAffineTransform m, CGPoint [] points, int count)
#else
		public unsafe void AddLines (CGAffineTransform m, CGPoint [] points, int count)
#endif
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (count > points.Length)
				throw new ArgumentException (nameof (count));
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddLines (Handle, transformPtr, points, count);
		}

		/// <param name="points">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddLines (CGPoint [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			CGPathAddLines (Handle, null, points, points.Length);
		}

		/// <param name="points">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddLines (CGPoint [] points, int count)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (count > points.Length)
				throw new ArgumentException (nameof (count));
			CGPathAddLines (Handle, null, points, count);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddEllipseInRect (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, CGRect rect);

		/// <param name="m">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddEllipseInRect (in CGAffineTransform m, CGRect rect)
#else
		public unsafe void AddEllipseInRect (CGAffineTransform m, CGRect rect)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddEllipseInRect (Handle, transformPtr, rect);
		}

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddEllipseInRect (CGRect rect)
		{
			CGPathAddEllipseInRect (Handle, null, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddArc (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y, /* CGFloat */ nfloat radius, /* CGFloat */ nfloat startAngle, /* CGFloat */ nfloat endAngle, byte clockwise);

#if XAMCORE_5_0
		public unsafe void AddArc (in CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
#else
		public unsafe void AddArc (CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddArc (Handle, transformPtr, x, y, radius, startAngle, endAngle, clockwise.AsByte ());
		}

		public unsafe void AddArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
		{
			CGPathAddArc (Handle, null, x, y, radius, startAngle, endAngle, clockwise.AsByte ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddArcToPoint (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat x1, /* CGFloat */ nfloat y1, /* CGFloat */ nfloat x2, /* CGFloat */ nfloat y2, /* CGFloat */ nfloat radius);

#if XAMCORE_5_0
		public unsafe void AddArcToPoint (in CGAffineTransform m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
#else
		public unsafe void AddArcToPoint (CGAffineTransform m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddArcToPoint (Handle, transformPtr, x1, y1, x2, y2, radius);
		}

		public unsafe void AddArcToPoint (nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
		{
			CGPathAddArcToPoint (Handle, null, x1, y1, x2, y2, radius);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddRelativeArc (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* m, /* CGFloat */ nfloat x, /* CGFloat */ nfloat y, /* CGFloat */ nfloat radius, /* CGFloat */ nfloat startAngle, /* CGFloat */ nfloat delta);

#if XAMCORE_5_0
		public unsafe void AddRelativeArc (in CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
#else
		public unsafe void AddRelativeArc (CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			CGPathAddRelativeArc (Handle, transformPtr, x, y, radius, startAngle, delta);
		}

		public unsafe void AddRelativeArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
		{
			CGPathAddRelativeArc (Handle, null, x, y, radius, startAngle, delta);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddPath (/* CGMutablePathRef */ IntPtr path1, CGAffineTransform* m, /* CGMutablePathRef */ IntPtr path2);

		/// <param name="t">To be added.</param>
		///         <param name="path2">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe void AddPath (in CGAffineTransform t, CGPath path2)
#else
		public unsafe void AddPath (CGAffineTransform t, CGPath path2)
#endif
		{
			if (path2 is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path2));
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &t)
#else
			CGAffineTransform* transformPtr = &t;
#endif
			CGPathAddPath (Handle, transformPtr, path2.Handle);
			GC.KeepAlive (path2);
		}

		/// <param name="path2">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void AddPath (CGPath path2)
		{
			if (path2 is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path2));
			CGPathAddPath (Handle, null, path2.Handle);
			GC.KeepAlive (path2);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPathIsEmpty (/* CGPathRef */ IntPtr path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get {
				return CGPathIsEmpty (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPathIsRect (/* CGPathRef */ IntPtr path, CGRect* rect);

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool IsRect (out CGRect rect)
		{
			unsafe {
				rect = default;
				fixed (CGRect* rectPtr = &rect) {
					return CGPathIsRect (Handle, rectPtr) != 0;
				}
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGPathGetCurrentPoint (/* CGPathRef */ IntPtr path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGPoint CurrentPoint {
			get {
				return CGPathGetCurrentPoint (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPathGetBoundingBox (/* CGPathRef */IntPtr path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect BoundingBox {
			get {
				return CGPathGetBoundingBox (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPathGetPathBoundingBox (/* CGPathRef */ IntPtr path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect PathBoundingBox {
			get {
				return CGPathGetPathBoundingBox (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPathContainsPoint (IntPtr path, CGAffineTransform* m, CGPoint point, byte eoFill);

		/// <param name="m">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <param name="eoFill">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public unsafe bool ContainsPoint (in CGAffineTransform m, CGPoint point, bool eoFill)
#else
		public unsafe bool ContainsPoint (CGAffineTransform m, CGPoint point, bool eoFill)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &m)
#else
			CGAffineTransform* transformPtr = &m;
#endif
			return CGPathContainsPoint (Handle, transformPtr, point, eoFill.AsByte ()) != 0;
		}

		/// <param name="point">To be added.</param>
		///         <param name="eoFill">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe bool ContainsPoint (CGPoint point, bool eoFill)
		{
			return CGPathContainsPoint (Handle, null, point, eoFill.AsByte ()) != 0;
		}

		/// <param name="element">
		/// The element to process.
		///   	   </param>
		///     <summary>A function that can make changes to a CGPathElement.</summary>
		///     <remarks>This is the function signature that is called back from CGPath.Apply for each element in a path. </remarks>
		public delegate void ApplierFunction (CGPathElement element);

		delegate void CGPathApplierFunction (/* void* */ IntPtr info, /* const CGPathElement* */ IntPtr element);
		[UnmanagedCallersOnly]
		static void ApplierCallback (IntPtr info, IntPtr element_ptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (info);
			// note: CGPathElementType is an untyped enum, always 32bits
			CGPathElement element = new CGPathElement (Marshal.ReadInt32 (element_ptr, 0));
			var func = gch.Target as ApplierFunction;
			if (func is null)
				return;

			IntPtr ptr = Marshal.ReadIntPtr (element_ptr, IntPtr.Size);
			int ptsize = Marshal.SizeOf<CGPoint> ();

			switch (element.Type) {
			case CGPathElementType.CloseSubpath:
				break;

			case CGPathElementType.MoveToPoint:
			case CGPathElementType.AddLineToPoint:
				element.Point1 = Marshal.PtrToStructure<CGPoint> (ptr)!;
				break;

			case CGPathElementType.AddQuadCurveToPoint:
				element.Point1 = Marshal.PtrToStructure<CGPoint> (ptr)!;
				element.Point2 = Marshal.PtrToStructure<CGPoint> (((IntPtr) (((long) ptr) + ptsize)))!;
				break;

			case CGPathElementType.AddCurveToPoint:
				element.Point1 = Marshal.PtrToStructure<CGPoint> (ptr)!;
				element.Point2 = Marshal.PtrToStructure<CGPoint> (((IntPtr) (((long) ptr) + ptsize)))!;
				element.Point3 = Marshal.PtrToStructure<CGPoint> (((IntPtr) (((long) ptr) + (2 * ptsize))))!;
				break;
			}

			func (element);
		}


		[DllImport (Constants.CoreGraphicsLibrary)]
		extern unsafe static void CGPathApply (/* CGPathRef */ IntPtr path, /* void* */ IntPtr info, delegate* unmanaged<IntPtr, IntPtr, void> function);

		/// <param name="func">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Apply (ApplierFunction func)
		{
			GCHandle gch = GCHandle.Alloc (func);
			unsafe {
				CGPathApply (Handle, GCHandle.ToIntPtr (gch), &ApplierCallback);
				GC.KeepAlive (this);
			}
			gch.Free ();
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyByNormalizing (IntPtr path, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateByNormalizing (bool evenOddFillRule)
		{
			return Runtime.GetINativeObject<CGPath> (CGPathCreateCopyByNormalizing (Handle, evenOddFillRule.AsByte ()), owns: true);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyByUnioningPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateByUnioningPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyByUnioningPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyByIntersectingPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateByIntersectingPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyByIntersectingPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyBySubtractingPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateBySubtractingPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyBySubtractingPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyBySymmetricDifferenceOfPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateBySymmetricDifferenceOfPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyBySymmetricDifferenceOfPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyOfLineBySubtractingPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateLineBySubtractingPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyOfLineBySubtractingPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyOfLineByIntersectingPath (IntPtr path, IntPtr maskPath, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateLineByIntersectingPath (CGPath? maskPath, bool evenOddFillRule)
		{
			CGPath? result = Runtime.GetINativeObject<CGPath> (CGPathCreateCopyOfLineByIntersectingPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()), owns: true);
			GC.KeepAlive (maskPath);
			return result;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern unsafe /* CFArrayRef __nullable */ IntPtr CGPathCreateSeparateComponents (IntPtr path, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath [] GetSeparateComponents (bool evenOddFillRule)
		{
			var cfArrayRef = CGPathCreateSeparateComponents (Handle, evenOddFillRule.AsByte ());
			return NSArray.NonNullArrayFromHandleDropNullElements<CGPath> (cfArrayRef);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPathCreateCopyByFlattening (IntPtr path, nfloat flatteningThreshold);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGPath? CreateByFlattening (nfloat flatteningThreshold)
		{
			return Runtime.GetINativeObject<CGPath> (CGPathCreateCopyByFlattening (Handle, flatteningThreshold), owns: true);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern byte CGPathIntersectsPath (IntPtr path1, IntPtr path2, byte evenOddFillRule);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public bool DoesIntersect (CGPath? maskPath, bool evenOddFillRule)
		{
			bool result = CGPathIntersectsPath (Handle, maskPath.GetHandle (), evenOddFillRule.AsByte ()) != 0;
			GC.KeepAlive (maskPath);
			return result;
		}

		static CGPath MakeMutable (IntPtr source, bool owns)
		{
			var mutable = CGPathCreateMutableCopy (source);
			if (owns)
				CGPathRelease (source);
			return new CGPath (mutable, owns: true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern unsafe static IntPtr CGPathCreateCopyByDashingPath (
			/* CGPathRef */ IntPtr path,
			/* const CGAffineTransform * */ CGAffineTransform* transform,
			/* CGFloat */ nfloat phase,
			/* CGFloat */ nfloat* lengths,
			/* size_t */ nint count);

#if XAMCORE_5_0
		public CGPath CopyByDashingPath (in CGAffineTransform transform, nfloat [] lengths)
#else
		public CGPath CopyByDashingPath (CGAffineTransform transform, nfloat [] lengths)
#endif
		{
			return CopyByDashingPath (transform, lengths, 0);
		}

#if XAMCORE_5_0
		public unsafe CGPath CopyByDashingPath (in CGAffineTransform transform, nfloat [] lengths, nfloat phase)
#else
		public unsafe CGPath CopyByDashingPath (CGAffineTransform transform, nfloat [] lengths, nfloat phase)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			fixed (nfloat* lengthsPtr = lengths) {
				return MakeMutable (CGPathCreateCopyByDashingPath (Handle, transformPtr, phase, lengthsPtr, lengths is null ? 0 : lengths.Length), true);
			}
		}

		public CGPath CopyByDashingPath (nfloat [] lengths)
		{
			return CopyByDashingPath (lengths, 0);
		}

		public unsafe CGPath CopyByDashingPath (nfloat [] lengths, nfloat phase)
		{
			fixed (nfloat* lengthsPtr = lengths) {
				var path = CGPathCreateCopyByDashingPath (Handle, null, phase, lengthsPtr, lengths is null ? 0 : lengths.Length);
				return MakeMutable (path, true);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe CGPath Copy ()
		{
			return MakeMutable (Handle, false);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static IntPtr CGPathCreateCopyByStrokingPath (/* CGPathRef */ IntPtr path, CGAffineTransform* transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, /* CGFloat */ nfloat miterLimit);

#if XAMCORE_5_0
		public unsafe CGPath CopyByStrokingPath (in CGAffineTransform transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
#else
		public unsafe CGPath CopyByStrokingPath (CGAffineTransform transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			return MakeMutable (CGPathCreateCopyByStrokingPath (Handle, transformPtr, lineWidth, lineCap, lineJoin, miterLimit), true);
		}

		public unsafe CGPath CopyByStrokingPath (nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
		{
			return MakeMutable (CGPathCreateCopyByStrokingPath (Handle, null, lineWidth, lineCap, lineJoin, miterLimit), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static IntPtr CGPathCreateCopyByTransformingPath (/* CGPathRef */ IntPtr path, CGAffineTransform* transform);

		/// <param name="transform">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		public CGPath CopyByTransformingPath (in CGAffineTransform transform)
#else
		public CGPath CopyByTransformingPath (CGAffineTransform transform)
#endif
		{
			unsafe {
#if XAMCORE_5_0
				fixed (CGAffineTransform* transformPtr = &transform)
#else
				CGAffineTransform* transformPtr = &transform;
#endif
				return MakeMutable (CGPathCreateCopyByTransformingPath (Handle, transformPtr), true);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static IntPtr CGPathCreateWithEllipseInRect (CGRect boundingRect, CGAffineTransform* transform);

		/// <param name="boundingRect">To be added.</param>
		///         <param name="transform">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		static public unsafe CGPath EllipseFromRect (CGRect boundingRect, in CGAffineTransform transform)
#else
		static public unsafe CGPath EllipseFromRect (CGRect boundingRect, CGAffineTransform transform)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			return MakeMutable (CGPathCreateWithEllipseInRect (boundingRect, transformPtr), true);
		}

		/// <param name="boundingRect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public unsafe CGPath EllipseFromRect (CGRect boundingRect)
		{
			return MakeMutable (CGPathCreateWithEllipseInRect (boundingRect, null), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static IntPtr CGPathCreateWithRect (CGRect boundingRect, CGAffineTransform* transform);

		/// <param name="rectangle">To be added.</param>
		///         <param name="transform">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
#if XAMCORE_5_0
		static public unsafe CGPath FromRect (CGRect rectangle, in CGAffineTransform transform)
#else
		static public unsafe CGPath FromRect (CGRect rectangle, CGAffineTransform transform)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			return MakeMutable (CGPathCreateWithRect (rectangle, transformPtr), true);
		}

		/// <param name="rectangle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public unsafe CGPath FromRect (CGRect rectangle)
		{
			return MakeMutable (CGPathCreateWithRect (rectangle, null), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static /* CGPathRef */ IntPtr CGPathCreateWithRoundedRect (CGRect rect, /* CGFloat */ nfloat cornerWidth, /* CGFloat */ nfloat cornerHeight, CGAffineTransform* transform);

		static unsafe CGPath _FromRoundedRect (CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight, CGAffineTransform* transform)
		{
			if ((cornerWidth < 0) || (2 * cornerWidth > rectangle.Width))
				throw new ArgumentException (nameof (cornerWidth));
			if ((cornerHeight < 0) || (2 * cornerHeight > rectangle.Height))
				throw new ArgumentException (nameof (cornerHeight));
			return MakeMutable (CGPathCreateWithRoundedRect (rectangle, cornerWidth, cornerHeight, transform), true);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static unsafe public CGPath FromRoundedRect (CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight)
		{
			return _FromRoundedRect (rectangle, cornerWidth, cornerHeight, null);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#if XAMCORE_5_0
		static public unsafe CGPath FromRoundedRect (CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight, in CGAffineTransform transform)
#else
		static public unsafe CGPath FromRoundedRect (CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight, CGAffineTransform transform)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			return _FromRoundedRect (rectangle, cornerWidth, cornerHeight, transformPtr);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPathAddRoundedRect (/* CGMutablePathRef */ IntPtr path, CGAffineTransform* transform, CGRect rect, /* CGFloat */ nfloat cornerWidth, /* CGFloat */ nfloat cornerHeight);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#if XAMCORE_5_0
		public unsafe void AddRoundedRect (in CGAffineTransform transform, CGRect rect, nfloat cornerWidth, nfloat cornerHeight)
#else
		public unsafe void AddRoundedRect (CGAffineTransform transform, CGRect rect, nfloat cornerWidth, nfloat cornerHeight)
#endif
		{
#if XAMCORE_5_0
			fixed (CGAffineTransform* transformPtr = &transform)
#else
			CGAffineTransform* transformPtr = &transform;
#endif
			CGPathAddRoundedRect (Handle, transformPtr, rect, cornerWidth, cornerHeight);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public unsafe void AddRoundedRect (CGRect rect, nfloat cornerWidth, nfloat cornerHeight)
		{
			CGPathAddRoundedRect (Handle, null, rect, cornerWidth, cornerHeight);
		}
#endif // !COREBUILD
	}
}
