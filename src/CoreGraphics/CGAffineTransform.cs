// 
// CGAffineTransform.cs: Implements the managed side
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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace CoreGraphics {

	// CGAffineTransform.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CGAffineTransform {
		public /* CGFloat */ nfloat A;
		public /* CGFloat */ nfloat B;
		public /* CGFloat */ nfloat C;
		public /* CGFloat */ nfloat D;
		public /* CGFloat */ nfloat Tx;
		public /* CGFloat */ nfloat Ty;

#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'A' instead.")]
		public nfloat xx { get => A; set => A = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'B' instead.")]
		public nfloat yx { get => B; set => B = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'C' instead.")]
		public nfloat xy { get => C; set => C = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'D' instead.")]
		public nfloat yy { get => D; set => D = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'Tx' instead.")]
		public nfloat x0 { get => Tx; set => Tx = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'Ty' instead.")]
		public nfloat y0 { get => Ty; set => Ty = value; }
#endif // !XAMCORE_5_0

#if !COREBUILD
		//
		// Constructors
		//
		public CGAffineTransform (nfloat a, nfloat b, nfloat c, nfloat d, nfloat tx, nfloat ty)
		{
			this.A = a;
			this.B = b;
			this.C = c;
			this.D = d;
			this.Tx = tx;
			this.Ty = ty;
		}

		// Identity
		public static CGAffineTransform MakeIdentity ()
		{
			return new CGAffineTransform (1, 0, 0, 1, 0, 0);
		}

		public static CGAffineTransform MakeRotation (nfloat angle)
		{
			var cos = (nfloat) Math.Cos (angle);
			var sin = (nfloat) Math.Sin (angle);
			return new CGAffineTransform (
				cos, sin,
				-sin, cos,
				0, 0);
		}

		public static CGAffineTransform MakeScale (nfloat sx, nfloat sy)
		{
			return new CGAffineTransform (sx, 0, 0, sy, 0, 0);
		}

		public static CGAffineTransform MakeTranslation (nfloat tx, nfloat ty)
		{
			return new CGAffineTransform (1, 0, 0, 1, tx, ty);
		}

		//
		// Operations
		//
		public static CGAffineTransform Multiply (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.A * b.A + a.B * b.C,
							  a.A * b.B + a.B * b.D,
							  a.C * b.A + a.D * b.C,
							  a.C * b.B + a.D * b.D,
							  a.Tx * b.A + a.Ty * b.C + b.Tx,
							  a.Tx * b.B + a.Ty * b.D + b.Ty);
		}

		public void Multiply (CGAffineTransform b)
		{
			var a = this;
			A = a.A * b.A + a.B * b.C;
			B = a.A * b.B + a.B * b.D;
			C = a.C * b.A + a.D * b.C;
			D = a.C * b.B + a.D * b.D;
			Tx = a.Tx * b.A + a.Ty * b.C + b.Tx;
			Ty = a.Tx * b.B + a.Ty * b.D + b.Ty;
		}

		public void Scale (nfloat sx, nfloat sy, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeScale (sx, sy), this); // t' = [ sx 0 0 sy 0 0 ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeScale (sx, sy)); // t' = t * [ sx 0 0 sy 0 0 ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ sx 0 0 sy 0 0 ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Scale (nfloat, nfloat, MatrixOrder)'.")]
		public void Scale (nfloat sx, nfloat sy)
		{
			Scale (sx, sy, MatrixOrder.Append);
		}

		public static CGAffineTransform Scale (CGAffineTransform transform, nfloat sx, nfloat sy)
		{
			return new CGAffineTransform (
				sx * transform.A,
				sx * transform.B,
				sy * transform.C,
				sy * transform.D,
				transform.Tx,
				transform.Ty);
		}

		public void Translate (nfloat tx, nfloat ty, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeTranslation (tx, ty), this); // t' = [ 1 0 0 1 tx ty ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeTranslation (tx, ty)); // t' = t * [ 1 0 0 1 tx ty ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ 1 0 0 1 tx ty ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Translate (nfloat, nfloat, MatrixOrder)'.")]
		public void Translate (nfloat tx, nfloat ty)
		{
			Translate (tx, ty, MatrixOrder.Append);
		}

		public static CGAffineTransform Translate (CGAffineTransform transform, nfloat tx, nfloat ty)
		{
			return new CGAffineTransform (
				transform.A,
				transform.B,
				transform.C,
				transform.D,
				tx * transform.A + ty * transform.C + transform.Tx,
				tx * transform.B + ty * transform.D + transform.Ty);
		}

		public void Rotate (nfloat angle, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeRotation (angle), this); // t' = [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeRotation (angle)); // t' = t * [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Rotate (nfloat, MatrixOrder)'.")]
		public void Rotate (nfloat angle)
		{
			Rotate (angle, MatrixOrder.Append);
		}

		public static CGAffineTransform Rotate (CGAffineTransform transform, nfloat angle)
		{
			var cos = (nfloat) Math.Cos (angle);
			var sin = (nfloat) Math.Sin (angle);

			return new CGAffineTransform (
				cos * transform.A + sin * transform.C,
				cos * transform.B + sin * transform.D,
				cos * transform.C - sin * transform.A,
				cos * transform.D - sin * transform.B,
				transform.Tx,
				transform.Ty);
		}

		/// <summary>Determines if the affine corresponds to the identity affine.</summary>
		///         <value>Whether the affine is the identity.</value>
		///         <remarks>To be added.</remarks>
		public bool IsIdentity {
			get {
				return A == 1 && B == 0 && C == 0 && D == 1 && Tx == 0 && Ty == 0;
			}
		}

#if !MONOMAC
		// on macOS NSAffineTransform is an ObjC type
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString */ IntPtr NSStringFromCGAffineTransform (CGAffineTransform transform);
#endif

		public override String? ToString ()
		{
#if MONOMAC
			var s = $"[{A}, {B}, {C}, {D}, {Tx}, {Ty}]";
#else
			var s = CFString.FromHandle (NSStringFromCGAffineTransform (this));
#endif
			return s;
		}

		public static bool operator == (CGAffineTransform lhs, CGAffineTransform rhs)
		{
			return (lhs.A == rhs.A && lhs.C == rhs.C &&
				lhs.B == rhs.B && lhs.D == rhs.D &&
				lhs.Tx == rhs.Tx && lhs.Ty == rhs.Ty);
		}

		public static bool operator != (CGAffineTransform lhs, CGAffineTransform rhs)
		{
			return !(lhs == rhs);
		}

		public static CGAffineTransform operator * (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.A * b.A + a.B * b.C,
							  a.A * b.B + a.B * b.D,
							  a.C * b.A + a.D * b.C,
							  a.C * b.B + a.D * b.D,
							  a.Tx * b.A + a.Ty * b.C + b.Tx,
							  a.Tx * b.B + a.Ty * b.D + b.Ty);
		}

		public override bool Equals (object? o)
		{
			if (o is CGAffineTransform transform) {
				return this == transform;
			} else
				return false;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (A, C, B, D, Tx, Ty);
		}

		public CGPoint TransformPoint (CGPoint point)
		{
			return new CGPoint (A * point.X + C * point.Y + Tx,
						B * point.X + D * point.Y + Ty);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGRect CGRectApplyAffineTransform (CGRect rect, CGAffineTransform t);

		public CGRect TransformRect (CGRect rect)
		{
			return CGRectApplyAffineTransform (rect, this);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGSizeApplyAffineTransform (CGSize rect, CGAffineTransform t);

		public CGSize TransformSize (CGSize size)
		{
			return CGSizeApplyAffineTransform (size, this);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformInvert (CGAffineTransform t);

		public CGAffineTransform Invert ()
		{
			return CGAffineTransformInvert (this);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGAffineTransformComponents CGAffineTransformDecompose (CGAffineTransform transform);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGAffineTransformComponents Decompose ()
		{
			return CGAffineTransformDecompose (this);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGAffineTransform CGAffineTransformMakeWithComponents (CGAffineTransformComponents components);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public static CGAffineTransform MakeWithComponents (CGAffineTransformComponents components)
		{
			return CGAffineTransformMakeWithComponents (components);
		}
#endif // !COREBUILD
	}
}
