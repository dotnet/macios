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

	/// <summary>Structure defining a rectangle in terms of location and size.</summary>
	///     <remarks>
	///       <para>
	/// 	CGRects structures define a rectangle using floating point
	/// 	values of type <see cref="T:System.nfloat" /> and are defined
	/// 	using an initial location (X,Y) as well as a size (Width,
	/// 	Height). 
	///       </para>
	///       <para>
	/// 	You can save the CGRect into an <see cref="T:Foundation.NSDictionary" /> by calling the
	/// 	<see cref="M:CoreGraphics.CGRect.ToDictionary" />
	/// 	method.   You can also get an CGRect out a serialized
	/// 	dictionary by using the <see cref="M:CoreGraphics.CGRect.TryParse(Foundation.NSDictionary,CoreGraphics.CGRect@)" /> method. 
	///       </para>
	///     </remarks>
	[Serializable]
	public struct CGRect : IEquatable<CGRect> {
		nfloat x;
		nfloat y;
		nfloat width;
		nfloat height;

#if !COREBUILD
		/// <summary>Represents a <see cref="T:CoreGraphics.CGRect" /> structure with its properties left uninitialized.</summary>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>A rectangle is defined by its width, height and upper-left corner.</para>
		///         </remarks>
		[Field ("CGRectZero", "CoreGraphics")] // unused but helps xtro
#endif
		public static readonly CGRect Empty;

#if !COREBUILD
		/// <summary>Gets an invalid, or null, rectangle.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CGRectNull", "CoreGraphics")] // unused but helps xtro
		public static CGRect Null {
			get { return Dlfcn.GetCGRect (Libraries.CoreGraphics.Handle, "CGRectNull"); }
		}

		/// <summary>Gets an infinitely large rectangle.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CGRectInfinite", "CoreGraphics")] // unused but helps xtro
		public static CGRect Infinite {
			get { return Dlfcn.GetCGRect (Libraries.CoreGraphics.Handle, "CGRectInfinite"); }
		}

		public static bool operator == (CGRect left, CGRect right)
		{
			// the following version of Equals cannot be removed by the linker, while == can be
			return left.Equals (right);
		}

		public static bool operator != (CGRect left, CGRect right)
		{
			return
				left.X != right.X ||
				left.Y != right.Y ||
				left.Width != right.Width ||
				left.Height != right.Height;
		}

#if !NO_SYSTEM_DRAWING
		public static implicit operator CGRect (RectangleF rect)
		{
			return new CGRect (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static implicit operator CGRect (Rectangle rect)
		{
			return new CGRect (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static explicit operator RectangleF (CGRect rect)
		{
			return new RectangleF ((float) rect.X, (float) rect.Y, (float) rect.Width, (float) rect.Height);
		}

		public static explicit operator Rectangle (CGRect rect)
		{
			return new Rectangle ((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
		}
#endif

		/// <param name="a">
		///           <attribution license="cc4" from="Microsoft" modified="false" />A rectangle to intersect. </param>
		///         <param name="b">
		///           <attribution license="cc4" from="Microsoft" modified="false" />A rectangle to intersect. </param>
		///         <summary>Returns a third <see cref="T:CoreGraphics.CGRect" /> structure that represents the intersection of two other <see cref="T:CoreGraphics.CGRect" /> structures. If there is no intersection, an empty <see cref="T:CoreGraphics.CGRect" /> is returned.</summary>
		///         <returns>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>A <see cref="T:CoreGraphics.CGRect" /> that represents the intersection of <paramref name="a" /> and <paramref name="b" />.</para>
		///         </returns>
		///         <remarks>To be added</remarks>
		public static CGRect Intersect (CGRect a, CGRect b)
		{
			// MS.NET returns a non-empty rectangle if the two rectangles
			// touch each other
			if (!a.IntersectsWithInclusive (b)) {
				return Empty;
			}

			return FromLTRB (
				(nfloat) Math.Max (a.Left, b.Left),
				(nfloat) Math.Max (a.Top, b.Top),
				(nfloat) Math.Min (a.Right, b.Right),
				(nfloat) Math.Min (a.Bottom, b.Bottom)
			);
		}

		/// <param name="rect">
		///           <attribution license="cc4" from="Microsoft" modified="false" />The <see cref="T:CoreGraphics.CGRect" /> with which to intersect. </param>
		///         <summary>Replaces this <see cref="T:CoreGraphics.CGRect" /> with the intersection of itself and the specified <see cref="T:CoreGraphics.CGRect" />.</summary>
		///         <remarks>To be added</remarks>
		public void Intersect (CGRect rect)
		{
			this = CGRect.Intersect (this, rect);
		}

		/// <include file="../../docs/api/CoreGraphics/CGRect.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGRect.Union(CoreGraphics.CGRect,CoreGraphics.CGRect)']/*" />
		public static CGRect Union (CGRect a, CGRect b)
		{
			return FromLTRB (
				(nfloat) Math.Min (a.Left, b.Left),
				(nfloat) Math.Min (a.Top, b.Top),
				(nfloat) Math.Max (a.Right, b.Right),
				(nfloat) Math.Max (a.Bottom, b.Bottom)
			);
		}

		public static CGRect FromLTRB (nfloat left, nfloat top, nfloat right, nfloat bottom)
		{
			return new CGRect (left, top, right - left, bottom - top);
		}

		public static CGRect Inflate (CGRect rect, nfloat x, nfloat y)
		{
			var inflated = new CGRect (rect.X, rect.Y, rect.Width, rect.Height);
			inflated.Inflate (x, y);
			return inflated;
		}

		/// <summary>Tests whether all numeric properties of this <see cref="T:CoreGraphics.CGRect" /> have values of zero.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added</remarks>
		public bool IsEmpty {
			get { return width == 0.0 || height == 0.0; }
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>Changing the <see cref="P:CoreGraphics.CGRect.X" /> property will also cause a change in the <see cref="P:CoreGraphics.CGRect.Right" /> property of the <see cref="T:CoreGraphics.CGRect" />.</para>
		///         </remarks>
		public nfloat X {
			get { return x; }
			set { x = value; }
		}

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>Changing the <see cref="P:CoreGraphics.CGRect.Y" /> property will also cause a change in the <see cref="P:CoreGraphics.CGRect.Bottom" /> property of the <see cref="T:CoreGraphics.CGRect" />.</para>
		///         </remarks>
		public nfloat Y {
			get { return y; }
			set { y = value; }
		}

		/// <include file="../../docs/api/CoreGraphics/CGRect.xml" path="/Documentation/Docs[@DocId='P:CoreGraphics.CGRect.Width']/*" />
		public nfloat Width {
			get { return width; }
			set { width = value; }
		}

		/// <include file="../../docs/api/CoreGraphics/CGRect.xml" path="/Documentation/Docs[@DocId='P:CoreGraphics.CGRect.Height']/*" />
		public nfloat Height {
			get { return height; }
			set { height = value; }
		}

		/// <summary>The Y coordinate of the top of the rectangle.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat Top {
			get { return Y; }
		}

		/// <summary>Gets the y-coordinate that is the sum of the <see cref="P:CoreGraphics.CGRect.Y" /> and <see cref="P:CoreGraphics.CGRect.Height" /> property values of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>The value of the <see cref="P:CoreGraphics.CGRect.Bottom" /> property represents the y-coordinate of the first point at the bottom edge of the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:CoreGraphics.CGRect.Rectangle&amp;scope=Xamarin" title="T:CoreGraphics.CGRect.Rectangle">T:CoreGraphics.CGRect.Rectangle</a></format> that is not contained in the <see cref="T:CoreGraphics.CGRect" />.</para>
		///         </remarks>
		public nfloat Bottom {
			get { return Y + Height; }
		}

		/// <summary>Gets the x-coordinate of the left edge of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added</remarks>
		public nfloat Left {
			get { return X; }
		}

		/// <summary>Gets the x-coordinate that is the sum of <see cref="P:CoreGraphics.CGRect.X" /> and <see cref="P:CoreGraphics.CGRect.Width" /> property values of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>The value of the <see cref="P:CoreGraphics.CGRect.Right" /> property represents the x-coordinate of the first point at the right edge of the rectangle that is not contained in the rectangle.</para>
		///         </remarks>
		public nfloat Right {
			get { return X + Width; }
		}

		/// <summary>Gets or sets the coordinates of the upper-left corner of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added</remarks>
		public CGPoint Location {
			get { return new CGPoint (x, y); }
			set {
				x = value.X;
				y = value.Y;
			}
		}

		/// <summary>Gets or sets the size of this <see cref="T:CoreGraphics.CGRect" />.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added</remarks>
		public CGSize Size {
			get { return new CGSize (width, height); }
			set {
				width = value.Width;
				height = value.Height;
			}
		}

		/// <param name="location">Rectangle location.</param>
		///         <param name="size">Dimensions for the rectangle.</param>
		///         <summary>Initializes a CGRect structure from a rectangle and a size parameters.</summary>
		///         <remarks>
		///         </remarks>
		public CGRect (CGPoint location, CGSize size)
		{
			x = location.X;
			y = location.Y;
			width = size.Width;
			height = size.Height;
		}
#endif // !COREBUILD

		public CGRect (nfloat x, nfloat y, nfloat width, nfloat height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

#if !COREBUILD
		/// <param name="x">X component for the rectangle.</param>
		///         <param name="y">Y component for the rectangle.</param>
		///         <param name="width">Width component for the rectangle.</param>
		///         <param name="height">Height component for the rectangle.</param>
		///         <summary>Initializes a CGRect structure from a double
		/// 	precision floating point values, with potential truncation on
		/// 	32 bit systems.</summary>
		///         <remarks>
		///           <para>
		/// 	    This initializes the structure with the given parameters.
		/// 	    On 32-bit systems, the values will be explicitly cast to
		/// 	    single precision floating point values.
		/// 	  </para>
		///         </remarks>
		public CGRect (double x, double y, double width, double height)
		{
			this.x = (nfloat) x;
			this.y = (nfloat) y;
			this.width = (nfloat) width;
			this.height = (nfloat) height;
		}


		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>Constructs a rectangle with the specified dimensions.</summary>
		///         <remarks>To be added.</remarks>
		public CGRect (float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public bool Contains (nfloat x, nfloat y)
		{
			return
				x >= Left &&
				x < Right &&
				y >= Top &&
				y < Bottom;
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>Determines if the specified point is contained within this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (float x, float y)
		{
			return Contains ((nfloat) x, (nfloat) y);
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>
		///           <see langword="true" /> if the point [<paramref name="x" />, <paramref name="y" />] is within the rectangle.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (double x, double y)
		{
			return Contains ((nfloat) x, (nfloat) y);
		}

		/// <param name="point">
		///           <attribution license="cc4" from="Microsoft" modified="false" />The <see cref="T:System.Drawing.Point" /> to test. </param>
		///         <summary>Determines if the specified point is contained within this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <returns>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>This method returns true if the point represented by <paramref name="point" /> is contained within this <see cref="T:CoreGraphics.CGRect" /> structure; otherwise false.</para>
		///         </returns>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>The containing rectangle must be normalized for this method to return accurate results.</para>
		///         </remarks>
		public bool Contains (CGPoint point)
		{
			return Contains (point.X, point.Y);
		}

		/// <param name="rect">
		///           <attribution license="cc4" from="Microsoft" modified="false" />The <see cref="T:CoreGraphics.CGRect" /> to test. </param>
		///         <summary>Determines if the rectangular region represented by <paramref name="rect" /> is entirely contained within this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <returns>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>This method returns true if the rectangular region represented by <paramref name="rect" /> is entirely contained within this <see cref="T:CoreGraphics.CGRect" /> structure; otherwise false.</para>
		///         </returns>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>The containing rectangle must be normalized for this method to return accurate results.</para>
		///         </remarks>
		public bool Contains (CGRect rect)
		{
			return
				X <= rect.X &&
				Right >= rect.Right &&
				Y <= rect.Y &&
				Bottom >= rect.Bottom;
		}

		public void Inflate (nfloat x, nfloat y)
		{
			this.x -= x;
			this.y -= y;
			width += x * 2;
			height += y * 2;
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Inflate (float x, float y)
		{
			Inflate ((nfloat) x, (nfloat) y);
		}

		/// <param name="x">The amount to add to both horizontal sides.</param>
		///         <param name="y">The amount to add to both vertical sides.</param>
		///         <summary>Increases the size of the rectangle by adding the specified amounts along both directions of each axis.</summary>
		///         <remarks>
		///           <para>Inflating a rectangle that is of size [1,1] and centered on [1,1] results in a rectangle of size [,3,3] centered on the same spot, since the <paramref name="x" /> and <paramref name="y" /> inflations are applied to all sides.</para>
		///         </remarks>
		public void Inflate (double x, double y)
		{
			Inflate ((nfloat) x, (nfloat) y);
		}

		/// <param name="size">
		///           <attribution license="cc4" from="Microsoft" modified="false" />The amount to inflate this rectangle. </param>
		///         <summary>Enlarges this <see cref="T:CoreGraphics.CGRect" /> by the specified amount.</summary>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>This method enlarges this rectangle, not a copy of it. The rectangle is enlarged in both directions along an axis. For example, if a 50 by 50 rectangle is enlarged by 50 in the x-axis, the resultant rectangle will be 150 units long (the original 50, the 50 in the minus direction, and the 50 in the plus direction) maintaining the rectangle's geometric center.</para>
		///           <para>If either element of the <paramref name="size" /> parameter is negative, the <see cref="T:CoreGraphics.CGRect" /> structure is deflated in the corresponding direction.</para>
		///         </remarks>
		public void Inflate (CGSize size)
		{
			Inflate (size.Width, size.Height);
		}

		public void Offset (nfloat x, nfloat y)
		{
			X += x;
			Y += y;
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		///         <remarks>To be added.</remarks>
		public void Offset (float x, float y)
		{
			Offset ((nfloat) x, (nfloat) y);
		}

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		///         <remarks>To be added.</remarks>
		public void Offset (double x, double y)
		{
			Offset ((nfloat) x, (nfloat) y);
		}

		/// <param name="pos">
		///           <attribution license="cc4" from="Microsoft" modified="false" />Amount to offset the location. </param>
		///         <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		///         <remarks>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>This method adjusts the location of the upper-left corner horizontally by the x-coordinate of the specified point, and vertically by the y-coordinate of the specified point.</para>
		///         </remarks>
		public void Offset (CGPoint pos)
		{
			Offset (pos.X, pos.Y);
		}

		/// <param name="rect">
		///           <attribution license="cc4" from="Microsoft" modified="false" />The rectangle to test. </param>
		///         <summary>Determines if this rectangle intersects with <paramref name="rect" />.</summary>
		///         <returns>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>This method returns true if there is any intersection, otherwise false.</para>
		///         </returns>
		///         <remarks>To be added</remarks>
		public bool IntersectsWith (CGRect rect)
		{
			return !(
				Left >= rect.Right ||
				Right <= rect.Left ||
				Top >= rect.Bottom ||
				Bottom <= rect.Top
			);
		}

		private bool IntersectsWithInclusive (CGRect r)
		{
			return !(
				Left > r.Right ||
				Right < r.Left ||
				Top > r.Bottom ||
				Bottom < r.Top
			);
		}
#endif // !COREBUILD

		/// <include file="../../docs/api/CoreGraphics/CGRect.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGRect.Equals(System.Object)']/*" />
		public override bool Equals (object? obj)
		{
			return (obj is CGRect rect) && Equals (rect);
		}

		/// <include file="../../docs/api/CoreGraphics/CGRect.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGRect.Equals(CoreGraphics.CGRect)']/*" />
		public bool Equals (CGRect rect)
		{
			return
				x == rect.x &&
				y == rect.y &&
				width == rect.width &&
				height == rect.height;
		}

		/// <summary>Returns the hash code for this <see cref="T:CoreGraphics.CGRect" /> structure. For information about the use of hash codes, see <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20Object%20Get%20Hash%20Code&amp;scope=Xamarin" title="M:System.Object.GetHashCode*">M:System.Object.GetHashCode*</a></format> .</summary>
		///         <returns>
		///           <attribution license="cc4" from="Microsoft" modified="false" />
		///           <para>An integer that represents the hash code for this rectangle.</para>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (x, y, width, height);
		}

#if !COREBUILD
		/// <summary>Gets the y-coordinate of the top edge of this <see cref="T:CoreGraphics.CGRect" /> structure.</summary>
		///         <returns />
		///         <remarks>
		///         </remarks>
		public override string? ToString ()
		{
			return CFString.FromHandle (NSStringFromCGRect (this));
		}

		public void Deconstruct (out nfloat x, out nfloat y, out nfloat width, out nfloat height)
		{
			x = X;
			y = Y;
			width = Width;
			height = Height;
		}

		public void Deconstruct (out CGPoint location, out CGSize size)
		{
			location = Location;
			size = Size;
		}

		/// <param name="dictionaryRepresentation">Dictionary containing
		/// 	a serialized CGRect.</param>
		///         <param name="rect">The rectangle value with the contents if
		/// 	the return value is true.</param>
		///         <summary>To be added.</summary>
		///         <returns>True if the NSDictionary contained a serialized
		/// 	CGRect and the initialized <paramref name="rect" />  with the
		/// 	contents on return.   False on failure, and the contents of
		/// 	<paramref name="rect" /> are set to Empty in that case.</returns>
		///         <remarks>Used to create a CGRect from a dictionary containing
		/// 	keys for X, Y, Widht and Height.</remarks>
		public static bool TryParse (NSDictionary? dictionaryRepresentation, out CGRect rect)
		{
			if (dictionaryRepresentation is null) {
				rect = Empty;
				return false;
			}
			rect = default;
			unsafe {
				bool result = NativeDrawingMethods.CGRectMakeWithDictionaryRepresentation (dictionaryRepresentation.Handle, (CGRect*) Unsafe.AsPointer<CGRect> (ref rect)) != 0;
				GC.KeepAlive (dictionaryRepresentation);
				return result;
			}
		}

		/// <summary>Serializes the state of the rectangle into an NSDictionary.</summary>
		///         <returns>An NSDictionary representing the rectangle.</returns>
		///         <remarks>
		///           <para>
		/// 	    The returned dictionary conforms to the serialization
		/// 	    standard of Cocoa and CocoaTouch and can be used to serialize
		/// 	    the state into objects that can be parsed by other Apple APIs.
		/// 	  </para>
		///           <para>
		/// 	    It is possible to create CGRect from a Dictionary using
		/// 	    the <see cref="M:CoreGraphics.CGRect.TryParse(Foundation.NSDictionary,CoreGraphics.CGRect@)" />
		/// 	    method. 
		/// 	  </para>
		///         </remarks>
		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (NativeDrawingMethods.CGRectCreateDictionaryRepresentation (this));
		}

#if MONOMAC
		// <quote>When building for 64 bit systems, or building 32 bit like 64 bit, NSRect is typedef’d to CGRect.</quote>
		// https://developer.apple.com/documentation/foundation/nsrect?language=objc
		[DllImport (Constants.FoundationLibrary, EntryPoint = "NSStringFromRect")]
		extern static /* NSString* */ IntPtr NSStringFromCGRect (/* NSRect */ CGRect rect);
#else
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString* */ IntPtr NSStringFromCGRect (CGRect rect);
#endif // MONOMAC
#endif // !COREBUILD
	}
}
