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
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKTileOverlayPath {
		/// <summary>The index of the <see cref="T:MapKit.MKTileOverlay" /> along the X axis.</summary>
		///         <remarks>To be added.</remarks>
		public /* NSInteger */ nint X;
		/// <summary>The index of the <see cref="T:MapKit.MKTileOverlay" /> along the Y axis.</summary>
		///         <remarks>To be added.</remarks>
		public /* NSInteger */ nint Y;
		/// <summary>The Zoom level for the <see cref="T:MapKit.MKTileOverlay" />.</summary>
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
		public MKCoordinateSpan (double latitudeDelta, double longitudeDelta)
		{
			LatitudeDelta = latitudeDelta;
			LongitudeDelta = longitudeDelta;
		}

		public override string ToString ()
		{
			return $"(LatitudeDelta={LatitudeDelta}, LongitudeDelta={LongitudeDelta}";
		}
	}

	// MKGeometry.h
#if NET
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
		public MKCoordinateRegion (CLLocationCoordinate2D center, MKCoordinateSpan span)
		{
			this.Center = center;
			this.Span = span;
		}

		// note: CLLocationDistance is double - see CLLocation.h
		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateRegionMakeWithDistance")]
		extern static public MKCoordinateRegion FromDistance (CLLocationCoordinate2D center, /* CLLocationDistance */ double latitudinalMeters, /* CLLocationDistance */ double longitudinalMeters);

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateRegionForMapRect")]
		extern static public MKCoordinateRegion FromMapRect (MKMapRect rect);

		public override string ToString ()
		{
			return $"(Center={Center}, Span={Span}";
		}
	}

	// MKGeometry.h
#if NET
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

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapPointForCoordinate")]
		public extern static MKMapPoint FromCoordinate (CLLocationCoordinate2D coordinate);

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKCoordinateForMapPoint")]
		public extern static CLLocationCoordinate2D ToCoordinate (MKMapPoint mapPoint);

		// MKMapPointMake
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

		public override bool Equals (object? other)
		{
			if (other is MKMapPoint) {
				var omap = (MKMapPoint) other;

				return omap.X == X && omap.Y == Y;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (X, Y);
		}

		// MKStringFromMapPoint does not really exists, it's inlined in MKGeometry.h
		public override string ToString ()
		{
			return String.Format ("{{{0}, {1}}}", X, Y);
		}
	}

	// MKGeometry.h
#if NET
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

		public override bool Equals (object? other)
		{
			if (other is MKMapSize) {
				var omap = (MKMapSize) other;

				return omap.Width == Width && omap.Height == Height;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (Width, Height);
		}

		// MKStringFromMapSize does not really exists, it's inlined in MKGeometry.h
		public override string ToString ()
		{
			return String.Format ("{{{0}, {1}}}", Width, Height);
		}
	}

	// MKGeometry.h
#if NET
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct MKMapRect {
#if NET
		/// <summary>A known-invalid <see cref="T:MapKit.MKMapRect" /> (see <see cref="P:MapKit.MKMapRect.IsNull" />).</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static readonly MKMapRect Null = new MKMapRect (double.PositiveInfinity, double.PositiveInfinity, 0, 0);

		/// <summary>The starting location of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <remarks>
		///           <para>Note that a <see cref="T:MapKit.MKMapRect" /> may have negative <see cref="P:MapKit.MKMapRect.Height" /> or <see cref="P:MapKit.MKMapRect.Width" />, so <see cref="F:MapKit.MKMapRect.Origin" /> is not necessarily located at [<see cref="P:MapKit.MKMapRect.MinX" />, <see cref="P:MapKit.MKMapRect.MinY" />].</para>
		///         </remarks>
		public MKMapPoint Origin;
		/// <summary>The extent of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <remarks>To be added.</remarks>
		public MKMapSize Size;

		public MKMapRect (MKMapPoint origin, MKMapSize size)
		{
			Origin = origin;
			Size = size;
		}

		// MKMapRectMake
		public MKMapRect (double x, double y, double width, double height)
		{
			Origin.X = x;
			Origin.Y = y;
			Size.Width = width;
			Size.Height = height;
		}

		// MKMapRectGetMinX
		/// <summary>The minimum X-axis value of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MinX {
			get {
				return Origin.X;
			}
		}

		// MKMapRectGetMinY
		/// <summary>The minimum Y-axis value of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MinY {
			get {
				return Origin.Y;
			}
		}

		// MKMapRectGetMaxX
		/// <summary>The maximum X-axis value of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MaxX {
			get {
				return Origin.X + Size.Width;
			}
		}

		// MKMapRectGetMaxY
		/// <summary>The maximum Y-axis value of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MaxY {
			get {
				return Origin.Y + Size.Height;
			}
		}

		// MKMapRectGetMidX
		/// <summary>The X-axis midpoint of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MidX {
			get {
				return Origin.X + Size.Width / 2.0;
			}
		}

		// MKMapRectGetMidY
		/// <summary>The Y-axis midpoint of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double MidY {
			get {
				return Origin.Y + Size.Height / 2.0;
			}
		}

		// MKMapRectGetWidth
		/// <summary>The extent along the X-axis of the <see cref="T:MapKit.MKMapRect" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Width {
			get {
				return Size.Width;
			}
		}

		// MKMapRectGetHeight
		/// <summary>The height of this <see cref="T:MapKit.MKMapRect" />, in degrees.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Height {
			get {
				return Size.Height;
			}
		}

		// MKMapRectIsNull
		/// <summary>Whether the <see cref="T:MapKit.MKMapRect" /> is the known-invalid rectangle <see cref="F:MapKit.MKMapRect.Null" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsNull {
			get {
				return Double.IsInfinity (Origin.X) || Double.IsInfinity (Origin.Y);
			}
		}

		// MKMapRectIsEmpty
		/// <summary>Whether the <see cref="T:MapKit.MKMapRect" /> has a positive area.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty {
			get {
				return IsNull || Size.Width == 0 || Size.Height == 0;
			}
		}

#if NET
		/// <summary>The <see cref="T:MapKit.MKMapRect" /> that represents the world in the 2D map projection.</summary>
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

		public override bool Equals (object? other)
		{
			if (other is MKMapRect) {
				var omap = (MKMapRect) other;

				return omap.Origin == Origin && omap.Size == Size;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (Origin, Size);
		}

		// MKStringFromMapRect does not really exists, it's inlined in MKGeometry.h
		public override string ToString ()
		{
			return string.Format ("{{{0}, {1}}}", Origin, Size);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectContainsPoint")]
		static extern byte MKMapRectContainsPoint (MKMapRect rect, MKMapPoint point);

		public bool Contains (MKMapPoint point)
		{
			return MKMapRectContainsPoint (this, point) != 0;
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectContainsRect")]
		static extern byte MKMapRectContainsRect (MKMapRect rect1, MKMapRect rect2);

		public bool Contains (MKMapRect rect)
		{
			return MKMapRectContainsRect (this, rect) != 0;
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectUnion")]
		static public extern MKMapRect Union (MKMapRect rect1, MKMapRect rect2);

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectIntersection")]
		static public extern MKMapRect Intersection (MKMapRect rect1, MKMapRect rect2);

		[DllImport (Constants.MapKitLibrary)]
		static extern byte MKMapRectIntersectsRect (MKMapRect rect1, MKMapRect rect2);

		public static bool Intersects (MKMapRect rect1, MKMapRect rect2)
		{
			return MKMapRectIntersectsRect (rect1, rect2) != 0;
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectInset")]
		static extern MKMapRect MKMapRectInset (MKMapRect rect, double dx, double dy);

		public MKMapRect Inset (double dx, double dy)
		{
			return MKMapRectInset (this, dx, dy);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectOffset")]
		static extern MKMapRect MKMapRectOffset (MKMapRect rect, double dx, double dy);

		public MKMapRect Offset (double dx, double dy)
		{
			return MKMapRectOffset (this, dx, dy);
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectDivide")]
		unsafe static extern void MKMapRectDivide (MKMapRect rect, MKMapRect* slice, MKMapRect* remainder, double amount, CGRectEdge edge);

#if !COREBUILD
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

		/// <summary>Whether the <see cref="T:MapKit.MKMapRect" /> crosses the ante-meridian.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Spans180thMeridian {
			get { return MKMapRectSpans180thMeridian (this) != 0; }
		}

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapRectRemainder")]
		static extern MKMapRect MKMapRectRemainder (MKMapRect rect);

		public MKMapRect Remainder ()
		{
			return MKMapRectRemainder (this);
		}
	}

	// MKGeometry.h
#if NET
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	public static class MKGeometry {

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMapPointsPerMeterAtLatitude")]
		static extern public double MapPointsPerMeterAtLatitude (/* CLLocationDegrees */ double latitude);

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMetersPerMapPointAtLatitude")]
		static extern public /* CLLocationDistance */ double MetersPerMapPointAtLatitude (/* CLLocationDegrees */ double latitude);

		[DllImport (Constants.MapKitLibrary, EntryPoint = "MKMetersBetweenMapPoints")]
		static extern public /* CLLocationDistance */ double MetersBetweenMapPoints (MKMapPoint a, MKMapPoint b);
	}

#if COREBUILD
	public partial class MKMapLaunchOptions : NSObject {
	}
#endif
}
