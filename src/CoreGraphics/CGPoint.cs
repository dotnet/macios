#nullable enable

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreGraphics {
	/// <summary>Structure defining a 2D point.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[Serializable]
	public struct CGPoint : IEquatable<CGPoint> {
		nfloat x;
		nfloat y;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly CGPoint Empty;

#if !COREBUILD
		public static bool operator == (CGPoint l, CGPoint r)
		{
			// the following version of Equals cannot be removed by the linker, while == can be
			return l.Equals (r);
		}

		public static bool operator != (CGPoint l, CGPoint r)
		{
			return l.x != r.x || l.y != r.y;
		}

		public static CGPoint operator + (CGPoint l, CGSize r)
		{
			return new CGPoint (l.x + r.Width, l.y + r.Height);
		}

		public static CGPoint operator - (CGPoint l, CGSize r)
		{
			return new CGPoint (l.x - r.Width, l.y - r.Height);
		}

#if !NO_SYSTEM_DRAWING
		public static implicit operator CGPoint (PointF point)
		{
			return new CGPoint (point.X, point.Y);
		}

		public static implicit operator CGPoint (Point point)
		{
			return new CGPoint (point.X, point.Y);
		}

		public static explicit operator PointF (CGPoint point)
		{
			return new PointF ((float) point.X, (float) point.Y);
		}

		public static explicit operator Point (CGPoint point)
		{
			return new Point ((int) point.X, (int) point.Y);
		}
#endif

		/// <param name="point">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGPoint Add (CGPoint point, CGSize size)
		{
			return point + size;
		}

		/// <param name="point">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGPoint Subtract (CGPoint point, CGSize size)
		{
			return point - size;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat X {
			get { return x; }
			set { x = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat Y {
			get { return y; }
			set { y = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get { return x == 0.0 && y == 0.0; }
		}
#endif // !COREBUILD

		public CGPoint (nfloat x, nfloat y)
		{
			this.x = x;
			this.y = y;
		}

#if !COREBUILD
		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPoint (double x, double y)
		{
			this.x = (nfloat) x;
			this.y = (nfloat) y;
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPoint (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPoint (CGPoint point)
		{
			this.x = point.x;
			this.y = point.y;
		}

		/// <param name="dictionaryRepresentation">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool TryParse (NSDictionary? dictionaryRepresentation, out CGPoint point)
		{
			if (dictionaryRepresentation is null) {
				point = Empty;
				return false;
			}
			unsafe {
				point = default;
				bool result = NativeDrawingMethods.CGPointMakeWithDictionaryRepresentation (dictionaryRepresentation.Handle, (CGPoint*) Unsafe.AsPointer<CGPoint> (ref point)) != 0;
				GC.KeepAlive (dictionaryRepresentation);
				return result;
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (NativeDrawingMethods.CGPointCreateDictionaryRepresentation (this));
		}
#endif // !COREBUILD

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			return (obj is CGPoint t) && Equals (t);
		}

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (CGPoint point)
		{
			return point.x == x && point.y == y;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (x, y);
		}

#if !COREBUILD
		public void Deconstruct (out nfloat x, out nfloat y)
		{
			x = X;
			y = Y;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string? ToString ()
		{
			return CFString.FromHandle (NSStringFromCGPoint (this));
		}

#if MONOMAC
		// <quote>When building for 64 bit systems, or building 32 bit like 64 bit, NSPoint is typedef’d to CGPoint.</quote>
		// https://developer.apple.com/documentation/foundation/nspoint?language=objc
		[DllImport (Constants.FoundationLibrary, EntryPoint = "NSStringFromPoint")]
		extern static /* NSString* */ IntPtr NSStringFromCGPoint (/* NSPoint */ CGPoint point);
#else
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString* */ IntPtr NSStringFromCGPoint (CGPoint point);
#endif // MONOMAC
#endif // !COREBUILD
	}
}
