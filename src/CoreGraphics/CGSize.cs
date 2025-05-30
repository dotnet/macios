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

	/// <summary>Structure containing height and width values.</summary>
	///     <remarks>To be added.</remarks>
	[Serializable]
	public struct CGSize : IEquatable<CGSize> {
		nfloat width;
		nfloat height;

		/// <summary>Represents an empty size.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly CGSize Empty;

#if !COREBUILD
		public static bool operator == (CGSize l, CGSize r)
		{
			// the following version of Equals cannot be removed by the linker, while == can be
			return l.Equals (r);
		}

		public static bool operator != (CGSize l, CGSize r)
		{
			return l.width != r.width || l.height != r.height;
		}

		public static CGSize operator + (CGSize l, CGSize r)
		{
			return new CGSize (l.width + r.Width, l.height + r.Height);
		}

		public static CGSize operator - (CGSize l, CGSize r)
		{
			return new CGSize (l.width - r.Width, l.height - r.Height);
		}

#if !NO_SYSTEM_DRAWING
		public static implicit operator CGSize (SizeF size)
		{
			return new CGSize (size.Width, size.Height);
		}

		public static implicit operator CGSize (Size size)
		{
			return new CGSize (size.Width, size.Height);
		}

		public static explicit operator SizeF (CGSize size)
		{
			return new SizeF ((float) size.Width, (float) size.Height);
		}

		public static explicit operator Size (CGSize size)
		{
			return new Size ((int) size.Width, (int) size.Height);
		}
#endif

		public static explicit operator CGPoint (CGSize size)
		{
			return new CGPoint (size.Width, size.Height);
		}

		/// <param name="size1">To be added.</param>
		///         <param name="size2">To be added.</param>
		///         <summary>Adds two CGSize objects and returns the result.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGSize Add (CGSize size1, CGSize size2)
		{
			return size1 + size2;
		}

		/// <param name="size1">To be added.</param>
		///         <param name="size2">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGSize Subtract (CGSize size1, CGSize size2)
		{
			return size1 - size2;
		}

		/// <summary>The Width component of the CGSize.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat Width {
			get { return width; }
			set { width = value; }
		}

		/// <summary>The height component of the CGSize.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat Height {
			get { return height; }
			set { height = value; }
		}

		/// <summary>Returns true if the size is empty</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get { return width == 0.0 && height == 0.0; }
		}
#endif // !COREBUILD

		public CGSize (nfloat width, nfloat height)
		{
			this.width = width;
			this.height = height;
		}

#if !COREBUILD
		/// <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGSize (double width, double height)
		{
			this.width = (nfloat) width;
			this.height = (nfloat) height;
		}

		/// <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGSize (float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		/// <param name="size">To be added.</param>
		///         <summary>Initializes a CGSize object from another CGSize.</summary>
		///         <remarks>To be added.</remarks>
		public CGSize (CGSize size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		/// <param name="dictionaryRepresentation">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>Attempts to parse the contents of an NSDictionary with a serialized CGSize into a CGSize.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool TryParse (NSDictionary? dictionaryRepresentation, out CGSize size)
		{
			if (dictionaryRepresentation is null) {
				size = Empty;
				return false;
			}
			unsafe {
				size = default;
				bool result = NativeDrawingMethods.CGSizeMakeWithDictionaryRepresentation (dictionaryRepresentation.Handle, (CGSize*) Unsafe.AsPointer<CGSize> (ref size)) != 0;
				GC.KeepAlive (dictionaryRepresentation);
				return result;
			}
		}

		/// <summary>Serializes a CGSize into an <see cref="Foundation.NSDictionary" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (NativeDrawingMethods.CGSizeCreateDictionaryRepresentation (this));
		}

		/// <param name="point">To be added.</param>
		///         <summary>Initializes a CGSize object from a CGPoint.</summary>
		///         <remarks>To be added.</remarks>
		public CGSize (CGPoint point)
		{
			this.width = point.X;
			this.height = point.Y;
		}
#endif // !COREBUILD

		/// <param name="obj">To be added.</param>
		///         <summary>Compares the CGSize with another object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			return (obj is CGSize t) && Equals (t);
		}

		/// <param name="size">To be added.</param>
		///         <summary>Compares the size with the specified size.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (CGSize size)
		{
			return size.width == width && size.height == height;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (width, height);
		}

#if !COREBUILD
		public void Deconstruct (out nfloat width, out nfloat height)
		{
			width = Width;
			height = Height;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize ToRoundedCGSize ()
		{
			return new CGSize ((nfloat) Math.Round (width), (nfloat) Math.Round (height));
		}

		/// <summary>Converts the CGSize to a CGPOint.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPoint ToCGPoint ()
		{
			return (CGPoint) this;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string? ToString ()
		{
			return CFString.FromHandle (NSStringFromCGSize (this));
		}

#if MONOMAC
		// <quote>When building for 64 bit systems, or building 32 bit like 64 bit, NSSize is typedef’d to CGSize.</quote>
		// https://developer.apple.com/documentation/foundation/nssize?language=objc
		[DllImport (Constants.FoundationLibrary, EntryPoint = "NSStringFromSize")]
		extern static /* NSString* */ IntPtr NSStringFromCGSize (/* NSRect */ CGSize size);
#else
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString* */ IntPtr NSStringFromCGSize (CGSize size);
#endif // MONOMAC
#endif // !COREBUILD
	}
}
