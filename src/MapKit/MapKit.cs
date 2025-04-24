//
// MapKit definitions
//
// Author:
//   Miguel de Icaza
//
// Copyright 2009 Novell, Inc.
// Copyright 2014-2015 Xamarin Inc.
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CoreGraphics;
using CoreLocation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	// MKTileOverlay.h
#if NET
	/// <summary>Encapsulates the index values of a particular <see cref="MapKit.MKTileOverlay" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKTileOverlayPath {
		/// <summary>The index of the <see cref="MapKit.MKTileOverlay" /> along the X axis.</summary>
		///         <remarks>To be added.</remarks>
		public /* NSInteger */ nint X;
		/// <summary>The index of the <see cref="MapKit.MKTileOverlay" /> along the Y axis.</summary>
		///         <remarks>To be added.</remarks>
		public /* NSInteger */ nint Y;
		/// <summary>The Zoom level for the <see cref="MapKit.MKTileOverlay" />.</summary>
		///         <remarks>
		///           <list type="table">
		///             <listheader>
		///               <term>Zoom Level</term>
		///               <description>Number of tiles to map entire world</description>
		///             </listheader>
		///             <item>
		///               <term>0</term>
		///               <description>1</description>
		///             </item>
		///             <item>
		///               <term>1</term>
		///               <description>4</description>
		///             </item>
		///             <item>
		///               <term>2</term>
		///               <description>16</description>
		///             </item>
		///             <item>
		///               <term>n</term>
		///               <description>(2N)^2</description>
		///             </item>
		///           </list>
		///         </remarks>
		public /* NSInteger */ nint Z;
		/// <summary>The screen scaling factor for which the tile is intended. A value of 1.0 indicates standard resolution, a value of 2.0 indicates Retina displays.</summary>
		///         <remarks>To be added.</remarks>
		public /* CGFloat */ nfloat ContentScaleFactor;
	}

	// MKGeometry.h
	// note: CLLocationDegrees is double - see CLLocation.h
#if NET
	/// <summary>The area spanned by a region of the map.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKCoordinateSpan {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public /* CLLocationDegrees */ double LatitudeDelta;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public /* CLLocationDegrees */ double LongitudeDelta;

		// MKCoordinateSpanMake
		/// <param name="latitudeDelta">To be added.</param>
		///         <param name="longitudeDelta">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MKCoordinateSpan (double latitudeDelta, double longitudeDelta)
		{
			LatitudeDelta = latitudeDelta;
			LongitudeDelta = longitudeDelta;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return $"(LatitudeDelta={LatitudeDelta}, LongitudeDelta={LongitudeDelta}";
		}
	}

	// MKGeometry.h
#if NET
	/// <summary>Defines a region of the map to display.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKCoordinateRegion {
		/// <summary>The center for the region of the map to display.</summary>
		///         <remarks>
		///         </remarks>
		public CLLocationCoordinate2D Center;
		/// <summary>Represents the dimensions and zoom level to display.</summary>
		///         <remarks>
		///         </remarks>
		public MKCoordinateSpan Span;

		// MKCoordinateRegionMake
		/// <param name="center">The center of the region.</param>
		///         <param name="span">The dimensions of the region.</param>
		///         <summary>Creates a new object representing a region of the map to display.</summary>
		///         <remarks>
		///         </remarks>
		public MKCoordinateRegion (CLLocationCoordinate2D center, MKCoordinateSpan span)
		{
			this.Center = center;
			this.Span = span;
		}

		// note: CLLocationDistance is double - see CLLocation.h
		/// <param name="center">The center of the region</param>
		///         <param name="latitudinalMeters">The latitude expressed in meters (north to south).</param>
		///         <param name="longitudinalMeters">The longitudinal expressed in meters (east to west).</param>
		///         <summary>Creates a new object representing a region of the map to display using a center and a distance (represented in meters).</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateRegionMakeWithDistance")]
		extern static public MKCoordinateRegion FromDistance (CLLocationCoordinate2D center, /* CLLocationDistance */ double latitudinalMeters, /* CLLocationDistance */ double longitudinalMeters);

		/// <param name="rect">The MKMapRect source.</param>
		///         <summary>Returns a MKCoordinateRegion for the specified 2D-map rectangle.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateRegionForMapRect")]
		extern static public MKCoordinateRegion FromMapRect (MKMapRect rect);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return $"(Center={Center}, Span={Span}";
		}
	}

	// MKGeometry.h
#if NET
	/// <include file="../../docs/api/MapKit/MKMapPoint.xml" path="/Documentation/Docs[@DocId='T:MapKit.MKMapPoint']/*" />
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKMapPoint {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double X;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Y;

		/// <param name="coordinate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapPointForCoordinate")]
		public extern static MKMapPoint FromCoordinate (CLLocationCoordinate2D coordinate);

		/// <param name="mapPoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateForMapPoint")]
		public extern static CLLocationCoordinate2D ToCoordinate (MKMapPoint mapPoint);

		// MKMapPointMake
		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapPoint (double x, double y)
		{
			X = x;
			Y = y;
		}

		// MKMapPointEqualToPoint
		public static bool operator == (MKMapPoint a, MKMapPoint b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator != (MKMapPoint a, MKMapPoint b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (other is MKMapPoint) {
				var omap = (MKMapPoint) other;

				return omap.X == X && omap.Y == Y;
			}
			return false;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (X, Y);
		}

		// MKStringFromMapPoint does not really exists, it's inlined in MKGeometry.h
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("{{{0}, {1}}}", X, Y);
		}
	}

	// MKGeometry.h
#if NET
	/// <summary>The extent of a 2D map projection as measured in map points.</summary>
	///     <remarks>
	///     </remarks>
	///     <altmember cref="MapKit.MKMapPoint" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKMapSize {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Width;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double Height;

		// MKMapSizeMake
		/// <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapSize (double width, double height)
		{
			Width = width;
			Height = height;
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
#endif
		public static MKMapSize World { get { return new MKMapSize (0x10000000, 0x10000000); } }

		// MKMapSizeEqualToSize
		public static bool operator == (MKMapSize a, MKMapSize b)
		{
			return a.Width == b.Width && a.Height == b.Height;
		}

		public static bool operator != (MKMapSize a, MKMapSize b)
		{
			return a.Width != b.Width || a.Height != b.Height;
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (other is MKMapSize) {
				var omap = (MKMapSize) other;

				return omap.Width == Width && omap.Height == Height;
			}
			return false;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Width, Height);
		}

		// MKStringFromMapSize does not really exists, it's inlined in MKGeometry.h
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("{{{0}, {1}}}", Width, Height);
		}
	}

	// MKGeometry.h
#if NET
	/// <summary>A rectangular area in a 2D map projection, measured in map points.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKMapRect {
#if NET
		/// <summary>A known-invalid <see cref="MapKit.MKMapRect" /> (see <see cref="MapKit.MKMapRect.IsNull" />).</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static readonly MKMapRect Null = new MKMapRect (double.PositiveInfinity, double.PositiveInfinity, 0, 0);

		/// <summary>The starting location of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <remarks>
		///           <para>Note that a <see cref="MapKit.MKMapRect" /> may have negative <see cref="MapKit.MKMapRect.Height" /> or <see cref="MapKit.MKMapRect.Width" />, so <see cref="MapKit.MKMapRect.Origin" /> is not necessarily located at [<see cref="MapKit.MKMapRect.MinX" />, <see cref="MapKit.MKMapRect.MinY" />].</para>
		///         </remarks>
		public MKMapPoint Origin;
		/// <summary>The extent of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapSize Size;

		/// <param name="origin">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <summary>Creates a new <see cref="MapKit.MKMapPoint" /> struct with the specified <paramref name="origin" /> and <paramref name="size" />.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapRect (MKMapPoint origin, MKMapSize size)
		{
			Origin = origin;
			Size = size;
		}

		// MKMapRectMake
		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <param name="width">To be added.</param>
		///         <param name="height">To be added.</param>
		///         <summary>Creates a new <see cref="MapKit.MKMapRect" /> originating at [<paramref name="x" />, <paramref name="y" />] and of the specified <paramref name="height" /> and <paramref name="width" />.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapRect (double x, double y, double width, double height)
		{
			Origin.X = x;
			Origin.Y = y;
			Size.Width = width;
			Size.Height = height;
		}

		// MKMapRectGetMinX
		/// <summary>The minimum X-axis value of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MinX {
			get {
				return Origin.X;
			}
		}

		// MKMapRectGetMinY
		/// <summary>The minimum Y-axis value of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MinY {
			get {
				return Origin.Y;
			}
		}

		// MKMapRectGetMaxX
		/// <summary>The maximum X-axis value of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MaxX {
			get {
				return Origin.X + Size.Width;
			}
		}

		// MKMapRectGetMaxY
		/// <summary>The maximum Y-axis value of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MaxY {
			get {
				return Origin.Y + Size.Height;
			}
		}

		// MKMapRectGetMidX
		/// <summary>The X-axis midpoint of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MidX {
			get {
				return Origin.X + Size.Width / 2.0;
			}
		}

		// MKMapRectGetMidY
		/// <summary>The Y-axis midpoint of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MidY {
			get {
				return Origin.Y + Size.Height / 2.0;
			}
		}

		// MKMapRectGetWidth
		/// <summary>The extent along the X-axis of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Width {
			get {
				return Size.Width;
			}
		}

		// MKMapRectGetHeight
		/// <summary>The height of this <see cref="MapKit.MKMapRect" />, in degrees.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Height {
			get {
				return Size.Height;
			}
		}

		// MKMapRectIsNull
		/// <summary>Whether the <see cref="MapKit.MKMapRect" /> is the known-invalid rectangle <see cref="MapKit.MKMapRect.Null" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsNull {
			get {
				return Double.IsInfinity (Origin.X) || Double.IsInfinity (Origin.Y);
			}
		}

		// MKMapRectIsEmpty
		/// <summary>Whether the <see cref="MapKit.MKMapRect" /> has a positive area.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get {
				return IsNull || Size.Width == 0 || Size.Height == 0;
			}
		}

#if NET
		/// <summary>The <see cref="MapKit.MKMapRect" /> that represents the world in the 2D map projection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif

#if XAMCORE_5_0
		public static MKMapRect World {
			get {
				return new MKMapRect (0, 0, 0x10000000, 0x10000000);
			}
		}
#else
		public MKMapRect World {
			get {
				return new MKMapRect (0, 0, 0x10000000, 0x10000000);
			}
		}
#endif

		// MKMapRectEqualToRect
		public static bool operator == (MKMapRect a, MKMapRect b)
		{
			return a.Origin == b.Origin && a.Size == b.Size;
		}

		public static bool operator != (MKMapRect a, MKMapRect b)
		{
			return a.Origin != b.Origin || a.Size != b.Size;
		}

		/// <param name="other">To be added.</param>
		///         <summary>Whether this <see cref="MapKit.MKMapRect" /> has the same <see cref="MapKit.MKMapRect.Origin" /> and <see cref="MapKit.MKMapRect.Size" /> values as the <paramref name="other" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (other is MKMapRect) {
				var omap = (MKMapRect) other;

				return omap.Origin == Origin && omap.Size == Size;
			}
			return false;
		}

		/// <summary>Returns a hash of this <see cref="MapKit.MKMapRect" /> struct's value.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Origin, Size);
		}

		// MKStringFromMapRect does not really exists, it's inlined in MKGeometry.h
		/// <summary>A brief representation of the origin and size of the <see cref="MapKit.MKMapRect" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return string.Format ("{{{0}, {1}}}", Origin, Size);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectContainsPoint")]
		static extern byte MKMapRectContainsPoint (MKMapRect rect, MKMapPoint point);

		/// <param name="point">To be added.</param>
		///         <summary>Whether the <see cref="MapKit.MKMapRect" /> contains <paramref name="point" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (MKMapPoint point)
		{
			return MKMapRectContainsPoint (this, point) != 0;
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectContainsRect")]
		static extern byte MKMapRectContainsRect (MKMapRect rect1, MKMapRect rect2);

		/// <param name="rect">To be added.</param>
		///         <summary>Whether <paramref name="rect" /> is entirely within the bounds of this <see cref="MapKit.MKMapRect" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (MKMapRect rect)
		{
			return MKMapRectContainsRect (this, rect) != 0;
		}

		/// <param name="rect1">To be added.</param>
		///         <param name="rect2">To be added.</param>
		///         <summary>Returns the rectangle covering both <paramref name="rect1" /> and <paramref name="rect2" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectUnion")]
		static public extern MKMapRect Union (MKMapRect rect1, MKMapRect rect2);

		/// <param name="rect1">To be added.</param>
		///         <param name="rect2">To be added.</param>
		///         <summary>Static method returning the intersection of <paramref name="rect1" /> with <paramref name="rect2" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectIntersection")]
		static public extern MKMapRect Intersection (MKMapRect rect1, MKMapRect rect2);

		[DllImport (Constants.MapKitLibrary)]
		static extern byte MKMapRectIntersectsRect (MKMapRect rect1, MKMapRect rect2);

		/// <param name="rect1">To be added.</param>
		///         <param name="rect2">To be added.</param>
		///         <summary>Whether <paramref name="rect1" /> and <paramref name="rect2" /> overlap.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool Intersects (MKMapRect rect1, MKMapRect rect2)
		{
			return MKMapRectIntersectsRect (rect1, rect2) != 0;
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectInset")]
		static extern MKMapRect MKMapRectInset (MKMapRect rect, double dx, double dy);

		/// <param name="dx">To be added.</param>
		///         <param name="dy">To be added.</param>
		///         <summary>Returns a new <see cref="MapKit.MKMapRect" /> based on <c>this</c>, offset by <paramref name="dx" /> and <paramref name="dy" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public MKMapRect Inset (double dx, double dy)
		{
			return MKMapRectInset (this, dx, dy);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectOffset")]
		static extern MKMapRect MKMapRectOffset (MKMapRect rect, double dx, double dy);

		/// <param name="dx">To be added.</param>
		///         <param name="dy">To be added.</param>
		///         <summary>A new <see cref="MapKit.MKMapRect" /> whose origin is shifted by <paramref name="dx" /> and <paramref name="dy" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public MKMapRect Offset (double dx, double dy)
		{
			return MKMapRectOffset (this, dx, dy);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectDivide")]
		unsafe static extern void MKMapRectDivide (MKMapRect rect, MKMapRect* slice, MKMapRect* remainder, double amount, CGRectEdge edge);

#if !COREBUILD
		/// <param name="amount">To be added.</param>
		///         <param name="edge">To be added.</param>
		///         <param name="remainder">To be added.</param>
		///         <summary>Splits this <see cref="MapKit.MKMapRect" /> into a two smaller rectangle (returned value and <paramref name="remainder" />).</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var rect = new MKMapRect(new MKMapPoint(19, -122), new MKMapSize(10, 10));
		///
		/// //Cannot make null because MKMapRect is a value type
		/// var r2 = new MKMapRect();
		///
		/// //Divide into smaller rectangles
		/// var slice = rect.Divide (5.0, new CGRectEdge(), out r2);
		/// // slice = {{19,-122},{5, 10}} , r2 = {{24, -122},{5, 10}}
		///     ]]></code>
		///           </example>
		///         </remarks>
		public MKMapRect Divide (double amount, CGRectEdge edge, out MKMapRect remainder)
		{
			MKMapRect slice;
			remainder = default;
			unsafe {
				MKMapRectDivide (this, &slice, (MKMapRect*) Unsafe.AsPointer<MKMapRect> (ref remainder), amount, edge);
			}
			return slice;
		}
#endif

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectSpans180thMeridian")]
		static extern byte MKMapRectSpans180thMeridian (MKMapRect rect);

		/// <summary>Whether the <see cref="MapKit.MKMapRect" /> crosses the ante-meridian.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Spans180thMeridian {
			get { return MKMapRectSpans180thMeridian (this) != 0; }
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectRemainder")]
		static extern MKMapRect MKMapRectRemainder (MKMapRect rect);

		/// <summary>A new <see cref="MapKit.MKMapRect" /> that has been normalized to remove areas outside the world map's boundaries.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public MKMapRect Remainder ()
		{
			return MKMapRectRemainder (this);
		}
	}

	// MKGeometry.h
#if NET
	/// <summary>Helper class containing methods for calculating distances and latitude-dependent scales.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	public static class MKGeometry {

		/// <param name="latitude">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapPointsPerMeterAtLatitude")]
		static extern public double MapPointsPerMeterAtLatitude (/* CLLocationDegrees */ double latitude);

		/// <param name="latitude">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMetersPerMapPointAtLatitude")]
		static extern public /* CLLocationDistance */ double MetersPerMapPointAtLatitude (/* CLLocationDegrees */ double latitude);

		/// <param name="a">To be added.</param>
		///         <param name="b">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMetersBetweenMapPoints")]
		static extern public /* CLLocationDistance */ double MetersBetweenMapPoints (MKMapPoint a, MKMapPoint b);
	}

#if COREBUILD
	public partial class MKMapLaunchOptions : NSObject {
	}
#endif
}
