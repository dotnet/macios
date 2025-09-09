//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//   Whitney Schmidt
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2013 Xamarin Inc.
// Copyright 2019 Microsoft Corp.
//

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreLocation;
#if MONOMAC
using AppKit;
using UITraitCollection = System.Int32;
#else
using UIKit;
#endif
#if !TVOS
using Contacts;
#endif
using System;

#if MONOMAC
using UIImage = AppKit.NSImage;
using UIView = AppKit.NSView;
using UIEdgeInsets = AppKit.NSEdgeInsets;
using UIColor = AppKit.NSColor;
using UIScene = AppKit.NSColor;
using UIControl = AppKit.NSControl;
using UIBarButtonItem = Foundation.NSObject;
using UIViewController = AppKit.NSViewController;
#else
using NSAppearance = Foundation.NSObject;
#endif
#if TVOS
using CNPostalAddress = Foundation.NSObject;
#endif

namespace MapKit {

	/// <summary>Provides annotation information to the map view.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKAnnotation_Protocol/index.html">Apple documentation for <c>MKAnnotation</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	[MacCatalyst (13, 1)]
	interface MKAnnotation {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Export ("coordinate")]
		[Abstract]
		CLLocationCoordinate2D Coordinate { get; }

		[Export ("title", ArgumentSemantic.Copy)]
		[NullAllowed]
		string Title { get; }

		[Export ("subtitle", ArgumentSemantic.Copy)]
		[NullAllowed]
		string Subtitle { get; }

		/// <param name="value">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setCoordinate:")]
		[MacCatalyst (13, 1)]
		void SetCoordinate (CLLocationCoordinate2D value);

		[Notification, Field ("MKAnnotationCalloutInfoDidChangeNotification")]
		NSString CalloutInfoDidChangeNotification { get; }
	}

	interface IMKAnnotation { }

	/// <include file="../docs/api/MapKit/MKOverlay.xml" path="/Documentation/Docs[@DocId='T:MapKit.MKOverlay']/*" />
	[BaseType (typeof (MKAnnotation))]
	[Model]
	[Protocol]
	[MacCatalyst (13, 1)]
	interface MKOverlay {
		// There's a 'coordinate' readonly property defined in the header for MKOverlay,
		// but the MKAnnotation protocol (which this protocol subclasses), also defines
		// a readonly 'coordinate' property, so there's no need to re-declare it here
		// (in fact it causes numerous build problems).

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("boundingMapRect")]
		MKMapRect BoundingMapRect { get; }

		/// <param name="rect">The area being checked for intersection with this <see cref="MapKit.MKOverlay" />.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("intersectsMapRect:")]
		bool Intersects (MKMapRect rect);

		// optional, not implemented by MKPolygon, MKPolyline and MKCircle
		// implemented by MKTileOverlay (and defined there)
		[OptionalImplementation]
		[Export ("canReplaceMapContent")]
		bool CanReplaceMapContent { get; }
	}

	interface IMKOverlay { }

	[BaseType (typeof (UIView))]
	[MacCatalyst (13, 1)]
	interface MKAnnotationView {
		[DesignatedInitializer]
		[Export ("initWithAnnotation:reuseIdentifier:")]
		[PostGet ("Annotation")]
		NativeHandle Constructor ([NullAllowed] IMKAnnotation annotation, [NullAllowed] string reuseIdentifier);

		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKAnnotationView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKAnnotationView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("reuseIdentifier")]
		[NullAllowed]
		string ReuseIdentifier { get; }

		[Export ("prepareForReuse")]
		void PrepareForReuse ();

		[Export ("annotation", ArgumentSemantic.Retain)]
		[ThreadSafe] // Sometimes iOS will request the annotation from a non-UI thread (see https://bugzilla.xamarin.com/show_bug.cgi?27609)
		[NullAllowed]
		IMKAnnotation Annotation { get; set; }

		[Export ("image", ArgumentSemantic.Retain)]
		[NullAllowed]
		UIImage Image { get; set; }

		[Export ("centerOffset")]
		CGPoint CenterOffset { get; set; }

		[Export ("calloutOffset")]
		CGPoint CalloutOffset { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the annotation is enabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the annotation view is highlighetd.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the annotation view is selected.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("selected")]
		bool Selected { [Bind ("isSelected")] get; set; }

		[Export ("setSelected:animated:")]
		void SetSelected (bool selected, bool animated);

		[Export ("canShowCallout")]
		bool CanShowCallout { get; set; }

		[Export ("leftCalloutAccessoryView", ArgumentSemantic.Retain)]
		[NullAllowed]
		UIView LeftCalloutAccessoryView { get; set; }

		[Export ("rightCalloutAccessoryView", ArgumentSemantic.Retain)]
		[NullAllowed]
		UIView RightCalloutAccessoryView { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("setDragState:animated:")]
		void SetDragState (MKAnnotationViewDragState newDragState, bool animated);

		[Export ("dragState")]
		[NoTV]
		[MacCatalyst (13, 1)]
		MKAnnotationViewDragState DragState { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the user can drag the annotation view.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("draggable")]
		bool Draggable { [Bind ("isDraggable")] get; set; }

		[MacCatalyst (13, 1)]
		[Export ("detailCalloutAccessoryView")]
		[NullAllowed]
		UIView DetailCalloutAccessoryView { get; set; }

		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("leftCalloutOffset")]
		CGPoint LeftCalloutOffset { get; set; }

		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("rightCalloutOffset")]
		CGPoint RightCallpoutOffset { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("clusteringIdentifier")]
		string ClusteringIdentifier { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("clusterAnnotationView", ArgumentSemantic.Weak)]
		MKAnnotationView ClusterAnnotationView { get; }

		[MacCatalyst (13, 1)]
		[Advice ("Pre-defined constants are available from 'MKFeatureDisplayPriority'.")]
		[Export ("displayPriority")]
		float DisplayPriority { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("collisionMode", ArgumentSemantic.Assign)]
		MKAnnotationViewCollisionMode CollisionMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("prepareForDisplay")]
		[RequiresSuper]
		void PrepareForDisplay ();

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("zPriority")]
		float ZPriority { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("selectedZPriority")]
		float SelectedZPriority { get; set; }

		[iOS (18, 0), Mac (15, 0), MacCatalyst (18, 0), NoTV]
		[Export ("accessoryOffset", ArgumentSemantic.Assign)]
		CGPoint AccessoryOffset { get; set; }
	}

	[ThreadSafe]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKShape))]
	interface MKCircle : MKOverlay {
		[Export ("radius")]
		double Radius { get; }

		[Static]
		[Export ("circleWithCenterCoordinate:radius:")]
		MKCircle Circle (CLLocationCoordinate2D withcenterCoordinate, double radius);

		[Static]
		[Export ("circleWithMapRect:")]
		MKCircle CircleWithMapRect (MKMapRect mapRect);

		#region MKAnnotation
		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }
		// note: setCoordinate: is not mandatory and is not implemented for MKCircle
		#endregion
	}

	/// <summary>The visual representation of a <see cref="MapKit.MKCircle" />. Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKCircleRenderer" />.</summary>
	///     <remarks>Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKCircleRenderer" />.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKCircleView_class/index.html">Apple documentation for <c>MKCircleView</c></related>
	[NoMac]
	[NoTV]
	[BaseType (typeof (MKOverlayPathView))]
	[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKCircleRenderer' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKCircleRenderer' instead.")]
	interface MKCircleView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKCircleView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKCircleView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("circle")]
		MKCircle Circle { get; }

		[Export ("initWithCircle:")]
		[PostGet ("Circle")]
		NativeHandle Constructor (MKCircle circle);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MKDirectionsRequest {
		[NullAllowed] // by default this property is null
		[Export ("destination")]
		MKMapItem Destination { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("source")]
		MKMapItem Source { get; set; }

		[Export ("initWithContentsOfURL:")]
		NativeHandle Constructor (NSUrl url);

		[Static]
		[Export ("isDirectionsRequestURL:")]
		bool IsDirectionsRequestUrl (NSUrl url);

		[Export ("transportType")]
		MKDirectionsTransportType TransportType { get; set; }

		[Export ("requestsAlternateRoutes")]
		bool RequestsAlternateRoutes { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("departureDate", ArgumentSemantic.Copy)]
		NSDate DepartureDate { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("arrivalDate", ArgumentSemantic.Copy)]
		NSDate ArrivalDate { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("tollPreference", ArgumentSemantic.Assign)]
		MKDirectionsRoutePreference TollPreference { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("highwayPreference", ArgumentSemantic.Assign)]
		MKDirectionsRoutePreference HighwayPreference { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	interface MKMapItem : NSSecureCoding
#if IOS // #if TARGET_OS_IOS
		, NSItemProviderReading, NSItemProviderWriting
#endif
	{
		[Export ("placemark", ArgumentSemantic.Retain)]
		MKPlacemark Placemark { get; }

		[Export ("isCurrentLocation")]
		bool IsCurrentLocation { get; }

		[NullAllowed] // it's null by default on iOS 6.1
		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("phoneNumber", ArgumentSemantic.Copy)]
		string PhoneNumber { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("url", ArgumentSemantic.Retain)]
		NSUrl Url { get; set; }

		[Static]
		[Export ("mapItemForCurrentLocation")]
		MKMapItem MapItemForCurrentLocation ();

		[Export ("initWithPlacemark:")]
		NativeHandle Constructor (MKPlacemark placemark);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("openInMapsWithLaunchOptions:"), Internal]
		bool _OpenInMaps ([NullAllowed] NSDictionary launchOptions);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("openMapsWithItems:launchOptions:"), Internal]
		bool _OpenMaps (MKMapItem [] mapItems, [NullAllowed] NSDictionary launchOptions);

		[iOS (13, 2), NoMac, NoTV]
		[Introduced (PlatformName.MacCatalyst, 13, 2)]
		[Async]
		[Export ("openInMapsWithLaunchOptions:fromScene:completionHandler:")]
		void OpenInMaps ([NullAllowed] NSDictionary launchOptions, [NullAllowed] UIScene fromScene, [NullAllowed] Action<NSError> completionHandler);

		[iOS (13, 2), NoMac, NoTV]
		[Introduced (PlatformName.MacCatalyst, 13, 2)]
		[Static]
		[Async]
		[Export ("openMapsWithItems:launchOptions:fromScene:completionHandler:")]
		void OpenMaps (MKMapItem [] mapItems, [NullAllowed] NSDictionary launchOptions, [NullAllowed] UIScene fromScene, [NullAllowed] Action<NSError> completionHandler);

		[NoTV, NoiOS, NoMacCatalyst, Mac (14, 4)]
		[Async]
		[Export ("openInMapsWithLaunchOptions:completionHandler:")]
		void OpenInMaps ([NullAllowed] NSDictionary launchOptions, [NullAllowed] Action<bool> completion);

		[NoTV, NoiOS, NoMacCatalyst, Mac (14, 4)]
		[Static]
		[Async]
		[Export ("openMapsWithItems:launchOptions:completionHandler:")]
		void OpenMaps (MKMapItem [] mapItems, [NullAllowed] NSDictionary launchOptions, [NullAllowed] Action<bool> completion);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsDirectionsModeKey"), Internal]
		NSString MKLaunchOptionsDirectionsModeKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsMapTypeKey"), Internal]
		NSString MKLaunchOptionsMapTypeKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsMapCenterKey"), Internal]
		NSString MKLaunchOptionsMapCenterKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsMapSpanKey"), Internal]
		NSString MKLaunchOptionsMapSpanKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsShowsTrafficKey"), Internal]
		NSString MKLaunchOptionsShowsTrafficKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsCameraKey"), Internal]
		NSString MKLaunchOptionsCameraKey { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsDirectionsModeDriving"), Internal]
		NSString MKLaunchOptionsDirectionsModeDriving { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsDirectionsModeWalking"), Internal]
		NSString MKLaunchOptionsDirectionsModeWalking { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsDirectionsModeTransit"), Internal]
		NSString MKLaunchOptionsDirectionsModeTransit { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MKLaunchOptionsDirectionsModeDefault"), Internal]
		NSString MKLaunchOptionsDirectionsModeDefault { get; }

		[Export ("timeZone")]
		[MacCatalyst (13, 1)]
		[NullAllowed]
		NSTimeZone TimeZone { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MKMapItemTypeIdentifier")]
		NSString TypeIdentifier { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointOfInterestCategory")]
		string PointOfInterestCategory { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("identifier"), NullAllowed]
		MKMapItemIdentifier Identifier { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("alternateIdentifiers")]
		NSSet<MKMapItemIdentifier> AlternateIdentifiers { get; }
	}

	[BaseType (typeof (UIView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (MKMapViewDelegate) })]
	[MacCatalyst (13, 1)]
	interface MKMapView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKMapView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKMapView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the MapKit.IMKMapViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MapKit.IMKMapViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IMKMapViewDelegate Delegate { get; set; }

		[Export ("mapType")]
		MKMapType MapType { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("preferredConfiguration", ArgumentSemantic.Copy)]
		MKMapConfiguration PreferredConfiguration { get; set; }

		[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("selectableMapFeatures", ArgumentSemantic.Assign)]
		MKMapFeatureOptions SelectableMapFeatures { get; set; }

		[Export ("region")]
		MKCoordinateRegion Region { get; set; }

		[Export ("setRegion:animated:")]
		void SetRegion (MKCoordinateRegion region, bool animated);

		[Export ("centerCoordinate")]
		CLLocationCoordinate2D CenterCoordinate { get; set; }

		[Export ("setCenterCoordinate:animated:")]
		void SetCenterCoordinate (CLLocationCoordinate2D coordinate, bool animated);

		[Export ("regionThatFits:")]
		MKCoordinateRegion RegionThatFits (MKCoordinateRegion region);

		[Export ("convertCoordinate:toPointToView:")]
		CGPoint ConvertCoordinate (CLLocationCoordinate2D coordinate, [NullAllowed] UIView toPointToView);

		[Export ("convertPoint:toCoordinateFromView:")]
		CLLocationCoordinate2D ConvertPoint (CGPoint point, [NullAllowed] UIView toCoordinateFromView);

		[Export ("convertRegion:toRectToView:")]
		CGRect ConvertRegion (MKCoordinateRegion region, [NullAllowed] UIView toRectToView);

		[Export ("convertRect:toRegionFromView:")]
		MKCoordinateRegion ConvertRect (CGRect rect, [NullAllowed] UIView toRegionFromView);

		/// <summary>Controls if the user can zoom in and out of the map using a pinch gesture.</summary>
		///         <value>Default is <see langword="true" />.</value>
		///         <remarks>The default value of <see langword="true" /> allows zooming. This property only controls if the user can interactively zoom. It has no affect on zooming the map in code, which can be done by changing either the <see cref="MapKit.MKMapView.Region" /> or <see cref="MapKit.MKMapView.VisibleMapRect" /> properties.</remarks>
		[Export ("zoomEnabled")]
		bool ZoomEnabled { [Bind ("isZoomEnabled")] get; set; }

		/// <summary>Control if the map can be scrolled with a pan gesture.</summary>
		///         <value>
		///         </value>
		///         <remarks>Only controls interactive scrolling. The region can be changed in code regardless of this property.</remarks>
		[Export ("scrollEnabled")]
		bool ScrollEnabled { [Bind ("isScrollEnabled")] get; set; }

		[Export ("showsUserLocation")]
		bool ShowsUserLocation { get; set; }

		[Export ("userLocation")]
		MKUserLocation UserLocation { get; }

		/// <summary>Returns if the user's location is currently visible on the map.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("userLocationVisible")]
		bool UserLocationVisible { [Bind ("isUserLocationVisible")] get; }

		[Export ("addAnnotation:")]
		[PostGet ("Annotations")]
		void AddAnnotation (IMKAnnotation annotation);

		[Export ("addAnnotations:")]
		[PostGet ("Annotations")]
		void AddAnnotations ([Params] IMKAnnotation [] annotations);

		[Export ("removeAnnotation:")]
		[PostGet ("Annotations")]
		void RemoveAnnotation (IMKAnnotation annotation);

		[Export ("removeAnnotations:")]
		[PostGet ("Annotations")]
		void RemoveAnnotations ([Params] IMKAnnotation [] annotations);

		[Export ("annotations")]
		IMKAnnotation [] Annotations { get; }

		[Export ("viewForAnnotation:")]
		[return: NullAllowed]
		MKAnnotationView ViewForAnnotation (IMKAnnotation annotation);

		[Export ("dequeueReusableAnnotationViewWithIdentifier:")]
		[return: NullAllowed]
		MKAnnotationView DequeueReusableAnnotation (string withViewIdentifier);

		[MacCatalyst (13, 1)]
		[Export ("dequeueReusableAnnotationViewWithIdentifier:forAnnotation:")]
		MKAnnotationView DequeueReusableAnnotation (string identifier, IMKAnnotation annotation);

		[MacCatalyst (13, 1)]
		[Export ("registerClass:forAnnotationViewWithReuseIdentifier:")]
		void Register ([NullAllowed] Class viewClass, string identifier);

		/// <param name="viewType">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("Register (viewType is null ? null : new Class (viewType), identifier)")]
		void Register ([NullAllowed] Type viewType, string identifier);

		[Export ("selectAnnotation:animated:")]
		[PostGet ("SelectedAnnotations")]
		void SelectAnnotation (IMKAnnotation annotation, bool animated);

		[Export ("deselectAnnotation:animated:")]
		[PostGet ("SelectedAnnotations")]
		void DeselectAnnotation ([NullAllowed] IMKAnnotation annotation, bool animated);

		[Export ("selectedAnnotations", ArgumentSemantic.Copy)]
		IMKAnnotation [] SelectedAnnotations { get; set; }

		[Export ("annotationVisibleRect")]
		CGRect AnnotationVisibleRect { get; }

		[Export ("addOverlay:")]
		[PostGet ("Overlays")]
		void AddOverlay (IMKOverlay overlay);

		[Export ("addOverlays:")]
		[PostGet ("Overlays")]
		void AddOverlays (IMKOverlay [] overlays);

		[Export ("removeOverlay:")]
		[PostGet ("Overlays")]
		void RemoveOverlay (IMKOverlay overlay);

		[Export ("removeOverlays:")]
		[PostGet ("Overlays")]
		void RemoveOverlays ([Params] IMKOverlay [] overlays);

		[Export ("overlays")]
		IMKOverlay [] Overlays { get; }

		/// <param name="overlay">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("insertOverlay:atIndex:")]
		[PostGet ("Overlays")]
		void InsertOverlay (IMKOverlay overlay, nint index);

		[Export ("insertOverlay:aboveOverlay:")]
		[PostGet ("Overlays")]
		void InsertOverlayAbove (IMKOverlay overlay, IMKOverlay sibling);

		[Export ("insertOverlay:belowOverlay:")]
		[PostGet ("Overlays")]
		void InsertOverlayBelow (IMKOverlay overlay, IMKOverlay sibling);

		/// <param name="index1">The index of the first overlay.</param>
		/// <param name="index2">The index of the second overlay.</param>
		/// <summary>Swaps the index positions of two overlays.</summary>
		/// <remarks>Changing the index positions of the overlays will swap their z-order on the map.</remarks>
		[Export ("exchangeOverlayAtIndex:withOverlayAtIndex:")]
		void ExchangeOverlays (nint index1, nint index2);

		[Export ("mapRectThatFits:")]
		MKMapRect MapRectThatFits (MKMapRect mapRect);

		[Export ("setVisibleMapRect:edgePadding:animated:")]
		void SetVisibleMapRect (MKMapRect mapRect, UIEdgeInsets edgePadding, bool animate);

		[Export ("setVisibleMapRect:animated:")]
		void SetVisibleMapRect (MKMapRect mapRect, bool animate);

		[Export ("mapRectThatFits:edgePadding:")]
		MKMapRect MapRectThatFits (MKMapRect mapRect, UIEdgeInsets edgePadding);

		[NoMac]
		[NoTV]
		[Export ("viewForOverlay:")]
		[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKOverlayRenderer.RendererForOverlay' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKOverlayRenderer.RendererForOverlay' instead.")]
		MKOverlayView ViewForOverlay (IMKOverlay overlay);

		[Export ("visibleMapRect")]
		MKMapRect VisibleMapRect { get; set; }

		[Export ("annotationsInMapRect:")]
		NSSet GetAnnotations (MKMapRect mapRect);

		[MacCatalyst (13, 1)]
		[Export ("userTrackingMode")]
		MKUserTrackingMode UserTrackingMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("setUserTrackingMode:animated:")]
		void SetUserTrackingMode (MKUserTrackingMode trackingMode, bool animated);

		[Export ("camera", ArgumentSemantic.Copy)]
		MKMapCamera Camera { get; set; }

		[Export ("setCamera:animated:")]
		void SetCamera (MKMapCamera camera, bool animated);

		/// <summary>Whether the view uses the heading defined by the <see cref="MapKit.MKMapView.Camera" />.</summary>
		///         <value>The default value is <see langword="true" />.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("rotateEnabled")]
		bool RotateEnabled { [Bind ("isRotateEnabled")] get; set; }

		/// <summary>Whether the view uses the angle defined by the <see cref="MapKit.MKMapView.Camera" />.</summary>
		///         <value>The default value is <see langword="true" />.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("pitchEnabled")]
		bool PitchEnabled { [Bind ("isPitchEnabled")] get; set; }

		[Export ("showAnnotations:animated:")]
		void ShowAnnotations (IMKAnnotation [] annotations, bool animated);

		[Export ("addOverlay:level:")]
		[PostGet ("Overlays")]
		void AddOverlay (IMKOverlay overlay, MKOverlayLevel level);

		[Export ("addOverlays:level:")]
		[PostGet ("Overlays")]
		void AddOverlays (IMKOverlay [] overlays, MKOverlayLevel level);

		[Export ("exchangeOverlay:withOverlay:")]
		[PostGet ("Overlays")]
		void ExchangeOverlay (IMKOverlay overlay1, IMKOverlay overlay2);

		/// <param name="overlay">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <param name="level">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("insertOverlay:atIndex:level:")]
		[PostGet ("Overlays")]
		void InsertOverlay (IMKOverlay overlay, nuint index, MKOverlayLevel level);

		[Export ("overlaysInLevel:")]
		IMKOverlay [] OverlaysInLevel (MKOverlayLevel level);

		[Export ("rendererForOverlay:")]
		[return: NullAllowed]
		MKOverlayRenderer RendererForOverlay (IMKOverlay overlay);

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PointOfInterestFilter' instead.")]
		[Export ("showsPointsOfInterest")]
		bool ShowsPointsOfInterest { get; set; }

		[Export ("showsBuildings")]
		bool ShowsBuildings { get; set; }

		// MKMapView.h headers says "To be used in testing only" which means it's likely won't be accepted in the appstore
		//		
		//		[Export ("_handleSelectionAtPoint:")]
		//		void _HandleSelectionAtPoint (CGPoint locationInView);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("showsCompass")]
		bool ShowsCompass { get; set; }

		[Export ("showsScale")]
		[MacCatalyst (13, 1)]
		bool ShowsScale { get; set; }

		[Export ("showsTraffic")]
		[MacCatalyst (13, 1)]
		bool ShowsTraffic { get; set; }

		[NoiOS]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("showsZoomControls")]
		bool ShowsZoomControls { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setCameraZoomRange:animated:")]
		void SetCameraZoomRange ([NullAllowed] MKMapCameraZoomRange cameraZoomRange, bool animated);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("cameraZoomRange", ArgumentSemantic.Copy)]
		[NullAllowed]
		MKMapCameraZoomRange CameraZoomRange { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("cameraBoundary", ArgumentSemantic.Copy)]
		MKMapCameraBoundary CameraBoundary { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setCameraBoundary:animated:")]
		void SetCameraBoundary ([NullAllowed] MKMapCameraBoundary cameraBoundary, bool animated);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[NoTV, NoiOS]
		[Export ("showsPitchControl")]
		bool ShowsPitchControl { get; set; }

		[TV (17, 0), MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
		[Export ("pitchButtonVisibility", ArgumentSemantic.Assign)]
		MKFeatureVisibility PitchButtonVisibility { get; set; }

		[TV (17, 0), MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
		[Export ("showsUserTrackingButton")]
		bool ShowsUserTrackingButton { get; set; }
	}

	[Static]
	[MacCatalyst (13, 1)]
	interface MKMapViewDefault {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MKMapViewDefaultAnnotationViewReuseIdentifier")]
		NSString AnnotationViewReuseIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MKMapViewDefaultClusterAnnotationViewReuseIdentifier")]
		NSString ClusterAnnotationViewReuseIdentifier { get; }
	}

	interface IMKMapViewDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	[MacCatalyst (13, 1)]
	interface MKMapViewDelegate {
		/// <param name="mapView">To be added.</param>
		/// <param name="animated">To be added.</param>
		/// <summary>Indicates the region displayed by <paramref name="mapView" /> is about to change.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:regionWillChangeAnimated:"), EventArgs ("MKMapViewChange", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void RegionWillChange (MKMapView mapView, bool animated);

		/// <param name="mapView">To be added.</param>
		/// <param name="animated">To be added.</param>
		/// <summary>Indicates the region displayed by <paramref name="mapView" /> has changed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:regionDidChangeAnimated:"), EventArgs ("MKMapViewChange", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void RegionChanged (MKMapView mapView, bool animated);

		/// <param name="mapView">To be added.</param>
		/// <summary>Indicates that loading of map data is about to begin.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapViewWillStartLoadingMap:")]
		void WillStartLoadingMap (MKMapView mapView);

		/// <param name="mapView">To be added.</param>
		/// <summary>Indicates that loading of map data has completed.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapViewDidFinishLoadingMap:")]
		void MapLoaded (MKMapView mapView);

		/// <param name="mapView">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Indicates an <paramref name="error" /> caused loading to fail.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapViewDidFailLoadingMap:withError:"), EventArgs ("NSError", true, XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void LoadingMapFailed (MKMapView mapView, NSError error);

		/// <param name="mapView">To be added.</param>
		/// <param name="annotation">To be added.</param>
		/// <summary>Returns the <see cref="MapKit.MKAnnotationView" /> associated with the <paramref name="annotation" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapView:viewForAnnotation:"), DelegateName ("MKMapViewAnnotation"), DefaultValue (null)]
		[return: NullAllowed]
		MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation);

		/// <param name="mapView">To be added.</param>
		/// <param name="views">To be added.</param>
		/// <summary>Called when an annotation view (or views) have been added to <paramref name="mapView" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didAddAnnotationViews:"), EventArgs ("MKMapViewAnnotation", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidAddAnnotationViews (MKMapView mapView, MKAnnotationView [] views);

		/// <param name="mapView">To be added.</param>
		/// <param name="view">To be added.</param>
		/// <param name="control">To be added.</param>
		/// <summary>Called when the callout accessory <paramref name="control" /> has been tapped.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("mapView:annotationView:calloutAccessoryControlTapped:"), EventArgs ("MKMapViewAccessoryTapped", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control);

		/// <param name="mapView">To be added.</param>
		/// <param name="annotationView">To be added.</param>
		/// <param name="newState">To be added.</param>
		/// <param name="oldState">To be added.</param>
		/// <summary>Called when the drag state has changed from <paramref name="oldState" /> to <paramref name="newState" />.</summary>
		/// <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("mapView:annotationView:didChangeDragState:fromOldState:"), EventArgs ("MKMapViewDragState", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void ChangedDragState (MKMapView mapView, MKAnnotationView annotationView, MKAnnotationViewDragState newState, MKAnnotationViewDragState oldState);

		/// <param name="mapView">To be added.</param>
		/// <param name="overlay">To be added.</param>
		/// <summary>Use MKOverlayRenderer.RendererForOverlay instead</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[NoMac]
		[NoTV]
		[Export ("mapView:viewForOverlay:"), DelegateName ("MKMapViewOverlay"), DefaultValue (null)]
		[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKOverlayRenderer.RendererForOverlay' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKOverlayRenderer.RendererForOverlay' instead.")]
		MKOverlayView GetViewForOverlay (MKMapView mapView, IMKOverlay overlay);

		/// <param name="mapView">To be added.</param>
		/// <param name="overlayViews">To be added.</param>
		/// <summary>Developers should not use this deprecated method. Developers should use 'DidAddOverlayRenderers' instead.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[NoTV]
		[Export ("mapView:didAddOverlayViews:"), EventArgs ("MKOverlayViews", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'DidAddOverlayRenderers' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidAddOverlayRenderers' instead.")]
		void DidAddOverlayViews (MKMapView mapView, MKOverlayView overlayViews);

		/// <param name="mapView">To be added.</param>
		/// <param name="view">To be added.</param>
		/// <summary>Indicates that the specified <see cref="MapKit.MKAnnotationView" /> has been selected.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didSelectAnnotationView:"), EventArgs ("MKAnnotationView", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectAnnotationView (MKMapView mapView, MKAnnotationView view);

		/// <param name="mapView">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Indicates that the attempt to locate the current user has failed due to <paramref name="error" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didFailToLocateUserWithError:"), EventArgs ("NSError", true, XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidFailToLocateUser (MKMapView mapView, NSError error);

		/// <param name="mapView">To be added.</param>
		/// <param name="view">To be added.</param>
		/// <summary>Indicates that <paramref name="view" /> has been deselected.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didDeselectAnnotationView:"), EventArgs ("MKAnnotationView", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidDeselectAnnotationView (MKMapView mapView, MKAnnotationView view);

		[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("mapView:didSelectAnnotation:"), EventArgs ("MKAnnotation")]
		void DidSelectAnnotation (MKMapView mapView, IMKAnnotation annotation);

		[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("mapView:didDeselectAnnotation:"), EventArgs ("MKAnnotation")]
		void DidDeselectAnnotation (MKMapView mapView, IMKAnnotation annotation);

		/// <param name="mapView">To be added.</param>
		/// <summary>Indicates that the system will start attempting to locate the user.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapViewWillStartLocatingUser:")]
		void WillStartLocatingUser (MKMapView mapView);

		/// <param name="mapView">To be added.</param>
		/// <summary>Indicates the system has stopped attemptig to locate the user.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapViewDidStopLocatingUser:")]
		void DidStopLocatingUser (MKMapView mapView);

		/// <param name="mapView">To be added.</param>
		/// <param name="userLocation">To be added.</param>
		/// <summary>Indicates the system has provided an update to the user's location.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didUpdateUserLocation:"), EventArgs ("MKUserLocation", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation);

		/// <param name="mapView">To be added.</param>
		/// <param name="mode">To be added.</param>
		/// <param name="animated">To be added.</param>
		/// <summary>Indicates a change in the active <see cref="MapKit.MKUserTrackingMode" />.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("mapView:didChangeUserTrackingMode:animated:"), EventArgs ("MMapViewUserTracking", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidChangeUserTrackingMode (MKMapView mapView, MKUserTrackingMode mode, bool animated);

		/// <param name="mapView">The <see cref="MapKit.MKMapView" /> being rendered.</param>
		/// <param name="overlay">The overlay requiring a renderer.</param>
		/// <summary>Calculates he <see cref="MapKit.MKOverlayRenderer" /> appropriate to the <paramref name="overlay" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapView:rendererForOverlay:"), DelegateName ("MKRendererForOverlayDelegate"), DefaultValue (null)]
		MKOverlayRenderer OverlayRenderer (MKMapView mapView, IMKOverlay overlay);

		/// <param name="mapView">To be added.</param>
		/// <param name="renderers">To be added.</param>
		/// <summary>Called when an overlay renderer (or renderers) have been added to <paramref name="mapView" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapView:didAddOverlayRenderers:"), EventArgs ("MKDidAddOverlayRenderers", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidAddOverlayRenderers (MKMapView mapView, MKOverlayRenderer [] renderers);

		/// <param name="mapView">To be added.</param>
		/// <summary>Indicates that rendering of <paramref name="mapView" /> is about to begin.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("mapViewWillStartRenderingMap:")]
		void WillStartRenderingMap (MKMapView mapView);

		/// <param name="mapView">To be added.</param>
		/// <param name="fullyRendered">To be added.</param>
		/// <summary>Indicates that rendering of <paramref name="mapView" /> has completed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapViewDidFinishRenderingMap:fullyRendered:"), EventArgs ("MKDidFinishRenderingMap", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidFinishRenderingMap (MKMapView mapView, bool fullyRendered);

		/// <param name="mapView">To be added.</param>
		/// <param name="memberAnnotations">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[MacCatalyst (13, 1)]
		[Export ("mapView:clusterAnnotationForMemberAnnotations:"), DelegateName ("MKCreateClusterAnnotation"), DefaultValue (null)]
		MKClusterAnnotation CreateClusterAnnotation (MKMapView mapView, IMKAnnotation [] memberAnnotations);

		/// <param name="mapView">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		[MacCatalyst (13, 1)]
		[Export ("mapViewDidChangeVisibleRegion:")]
		void DidChangeVisibleRegion (MKMapView mapView);

		[return: NullAllowed]
		[Export ("mapView:selectionAccessoryForAnnotation:"), DelegateName ("MKMapViewDelegateGetSelectionAccessory"), DefaultValue (null)]
		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		MKSelectionAccessory GetSelectionAccessory (MKMapView mapView, IMKAnnotation annotation);
	}

	[BaseType (typeof (MKAnnotationView))]
	// crash on Dispose when created from 'init'
	[DisableDefaultCtor]
	[Deprecated (PlatformName.MacOSX, 13, 0, message: "Use MKMarkerAnnotationView instead.")]
	[Deprecated (PlatformName.iOS, 16, 0, message: "Use MKMarkerAnnotationView instead.")]
	[Deprecated (PlatformName.MacCatalyst, 16, 0, message: "Use MKMarkerAnnotationView instead.")]
	[Deprecated (PlatformName.TvOS, 16, 0, message: "Use MKMarkerAnnotationView instead.")]
	[MacCatalyst (13, 1)]
	interface MKPinAnnotationView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKPinAnnotationView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKPinAnnotationView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("initWithAnnotation:reuseIdentifier:")]
		NativeHandle Constructor ([NullAllowed] IMKAnnotation annotation, [NullAllowed] string reuseIdentifier);

		[NoTV]
		[Export ("pinColor")]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'PinTintColor' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'PinTintColor' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PinTintColor' instead.")]
		MKPinAnnotationColor PinColor { get; set; }

		[Export ("animatesDrop")]
		bool AnimatesDrop { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Appearance]
		[Export ("pinTintColor")]
		[NullAllowed]
		UIColor PinTintColor { get; set; }

		[MacCatalyst (13, 1)]
		[Static, Export ("redPinColor")]
		UIColor RedPinColor { get; }

		[MacCatalyst (13, 1)]
		[Static, Export ("greenPinColor")]
		UIColor GreenPinColor { get; }

		[MacCatalyst (13, 1)]
		[Static, Export ("purplePinColor")]
		UIColor PurplePinColor { get; }
	}

	// This requires the AddressBook framework, which afaict isn't bound on Mac, tvOS and watchOS yet
	/// <summary>Class that contains address data for a place mark.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[StrongDictionary ("global::AddressBook.ABPersonAddressKey")]
	interface MKPlacemarkAddress {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("City")]
		string City { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Country")]
		string Country { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("CountryCode")]
		string CountryCode { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("State")]
		string State { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Street")]
		string Street { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Zip")]
		string Zip { get; set; }
	}

	[BaseType (typeof (CLPlacemark))]
	// crash (at least) when calling 'description' when instance is created by 'init'
	[DisableDefaultCtor]
	[MacCatalyst (13, 1)]
	interface MKPlacemark : MKAnnotation, NSCopying {
		[Export ("initWithCoordinate:addressDictionary:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate, [NullAllowed] NSDictionary addressDictionary);

		// This requires the AddressBook framework, which afaict isn't bound on Mac, tvOS and watchOS yet
		/// <param name="coordinate">To be added.</param>
		/// <param name="addressDictionary">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Wrap ("this (coordinate, addressDictionary.GetDictionary ())")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate, MKPlacemarkAddress addressDictionary);

		[MacCatalyst (13, 1)]
		[Export ("initWithCoordinate:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("initWithCoordinate:postalAddress:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate, CNPostalAddress postalAddress);

		[Export ("countryCode")]
		[NullAllowed]
		string CountryCode { get; }
	}

	/// <summary>Provides conversion between a specific location to information about that location (e.g., street address).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKReverseGeocoder_Class/index.html">Apple documentation for <c>MKReverseGeocoder</c></related>
	[NoMac]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[Deprecated (PlatformName.iOS, 5, 0, message: "Use 'CoreLocation.CLGeocoder' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CoreLocation.CLGeocoder' instead.")]
	// crash (at least) at Dispose time when instance is created by 'init'
	[DisableDefaultCtor]
	interface MKReverseGeocoder {
		[Export ("initWithCoordinate:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate);

		[Export ("delegate", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the MapKit.IMKReverseGeocoderDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MapKit.IMKReverseGeocoderDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IMKReverseGeocoderDelegate Delegate { get; set; }

#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Obsolete ("Use the 'Coordinate' property instead.")]
		[Wrap ("Coordinate", IsVirtual = true)]
		CLLocationCoordinate2D coordinate { get; }
#endif

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }

		[Export ("start")]
		void Start ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("querying")]
		bool Querying { [Bind ("isQuerying")] get; }

		[Export ("cancel")]
		void Cancel ();

		[Export ("placemark")]
		MKPlacemark Placemark { get; }
	}

	interface IMKReverseGeocoderDelegate { }

#pragma warning disable 618
	/// <summary>When overridden, allows customization of events relating to the lifecycle of a <see cref="MapKit.MKReverseGeocoder" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKReverseGeocoderDelegate_Protocol/index.html">Apple documentation for <c>MKReverseGeocoderDelegate</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.iOS, 5, 0)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MKReverseGeocoderDelegate {
		/// <param name="geocoder">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("reverseGeocoder:didFailWithError:")]
		void FailedWithError (MKReverseGeocoder geocoder, NSError error);

		/// <param name="geocoder">To be added.</param>
		/// <param name="placemark">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("reverseGeocoder:didFindPlacemark:")]
		void FoundWithPlacemark (MKReverseGeocoder geocoder, MKPlacemark placemark);
	}
#pragma warning restore 618

	/// <summary>The visual representation of a <see cref="MapKit.MKOverlay" />. <see cref="MapKit.MKOverlayRenderer" /> should be used instead of this class in apps targeting iOS 7 and later.</summary>
	///     <remarks>
	///       <para>Application developers targeting iOS 7 and later should prefer to use <see cref="MapKit.MKOverlayRenderer" />. That class has higher performance and places overlays beneath labels.</para>
	///     </remarks>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKOverlayView_class/index.html">Apple documentation for <c>MKOverlayView</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKOverlayRenderer' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKOverlayRenderer' instead.")]
	[BaseType (typeof (UIView))]
	interface MKOverlayView {
		[Export ("overlay")]
		IMKOverlay Overlay { get; }

		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKOverlayView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKOverlayView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[DesignatedInitializer]
		[Export ("initWithOverlay:")]
		NativeHandle Constructor (IMKOverlay overlay);

		[Export ("pointForMapPoint:")]
		[ThreadSafe]
		CGPoint PointForMapPoint (MKMapPoint mapPoint);

		[Export ("mapPointForPoint:")]
		[ThreadSafe]
		MKMapPoint MapPointForPoint (CGPoint point);

		[Export ("rectForMapRect:")]
		[ThreadSafe]
		CGRect RectForMapRect (MKMapRect mapRect);

		[Export ("mapRectForRect:")]
		[ThreadSafe]
		MKMapRect MapRectForRect (CGRect rect);

		/// <include file="../docs/api/MapKit/MKOverlayView.xml" path="/Documentation/Docs[@DocId='M:MapKit.MKOverlayView.CanDrawMapRect(MapKit.MKMapRect,System.Runtime.InteropServices.NFloat)']/*" />
		[Export ("canDrawMapRect:zoomScale:")]
		bool CanDrawMapRect (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale);

		/// <param name="mapRect">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <param name="context">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[Export ("drawMapRect:zoomScale:inContext:")]
		[ThreadSafe]
		void DrawMapRect (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale, CGContext context);

		[Export ("setNeedsDisplayInMapRect:")]
		void SetNeedsDisplay (MKMapRect mapRect);

		/// <param name="mapRect">The <see cref="MapKit.MKMapRect" /> to invalidate.</param>
		/// <param name="zoomScale">The zoom scale to invalidate.</param>
		/// <summary>Invalidates the view in the specified <paramref name="mapRect" /> at the specified <paramref name="zoomScale" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setNeedsDisplayInMapRect:zoomScale:")]
		void SetNeedsDisplay (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale);
	}

	/// <summary>A <see cref="MapKit.MKOverlayView" /> whose visual reprepresentation is a <see cref="CoreGraphics.CGPath" />. Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKOverlayPathRenderer" />.</summary>
	///     <remarks>Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKOverlayPathRenderer" />.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKOverlayPathView_class/index.html">Apple documentation for <c>MKOverlayPathView</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKOverlayPathRenderer' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKOverlayPathRenderer' instead.")]
	[BaseType (typeof (MKOverlayView))]
	interface MKOverlayPathView {
		[Export ("initWithOverlay:")]
		NativeHandle Constructor (IMKOverlay overlay);

		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKOverlayPathView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKOverlayPathView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[NullAllowed] // by default this property is null
		[Export ("fillColor", ArgumentSemantic.Retain)]
		UIColor FillColor { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("lineWidth")]
		nfloat LineWidth { get; set; }

		[Export ("lineJoin")]
		CGLineJoin LineJoin { get; set; }

		[Export ("lineCap")]
		CGLineCap Linecap { get; set; }

		[Export ("miterLimit")]
		nfloat MiterLimit { get; set; }

		[Export ("lineDashPhase")]
		nfloat LineDashPhase { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("lineDashPattern", ArgumentSemantic.Copy)]
		NSNumber [] LineDashPattern { get; set; }

		[NullAllowed]
		[Export ("path")]
		CGPath Path { get; set; }

		[Export ("createPath")]
		void CreatePath ();

		[Export ("invalidatePath")]
		void InvalidatePath ();

		/// <param name="context">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("applyStrokePropertiesToContext:atZoomScale:")]
		void ApplyStrokeProperties (CGContext context, /* MKZoomScale */ nfloat zoomScale);

		/// <param name="context">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("applyFillPropertiesToContext:atZoomScale:")]
		void ApplyFillProperties (CGContext context, /* MKZoomScale */ nfloat zoomScale);

		[Export ("strokePath:inContext:")]
		void StrokePath (CGPath path, CGContext context);

		[Export ("fillPath:inContext:")]
		void FillPath (CGPath path, CGContext context);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Abstract]
	interface MKShape : MKAnnotation {
		[NullAllowed] // by default this property is null
		[Export ("title", ArgumentSemantic.Copy)]
		new string Title { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("subtitle", ArgumentSemantic.Copy)]
		new string Subtitle { get; set; }
	}

	[MacCatalyst (13, 1)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (MKShape))]
	interface MKPointAnnotation : MKGeoJsonObject {
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithCoordinate:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithCoordinate:title:subtitle:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate, [NullAllowed] string title, [NullAllowed] string subtitle);

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; set; }
	}

	/// <summary>The visual representation of an <see cref="MapKit.MKPolygon" /> annotation. Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKPolygonRenderer" />.</summary>
	///     <remarks>Developers targeting iOS 7 and later should instead use <see cref="MapKit.MKPolygonRenderer" />.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKPolygonView_class/index.html">Apple documentation for <c>MKPolygonView</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKPolygonRenderer' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKPolygonRenderer' instead.")]
	[BaseType (typeof (MKOverlayPathView))]
	interface MKPolygonView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKPolygonView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKPolygonView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("initWithPolygon:")]
		[PostGet ("Polygon")]
		NativeHandle Constructor (MKPolygon polygon);

		[Export ("polygon")]
		MKPolygon Polygon { get; }
	}

	[ThreadSafe]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKMultiPoint))]
	interface MKPolygon : MKOverlay, MKGeoJsonObject {
		[Export ("interiorPolygons")]
		[NullAllowed]
		MKPolygon [] InteriorPolygons { get; }

		[Static]
		[Internal]
		[Export ("polygonWithPoints:count:")]
		MKPolygon _FromPoints (IntPtr points, nint count);

		[Static]
		[Internal]
		[Export ("polygonWithPoints:count:interiorPolygons:")]
		MKPolygon _FromPoints (IntPtr points, nint count, [NullAllowed] MKPolygon [] interiorPolygons);

		[Static]
		[Export ("polygonWithCoordinates:count:"), Internal]
		MKPolygon _FromCoordinates (IntPtr coords, nint count);

		[Static]
		[Internal]
		[Export ("polygonWithCoordinates:count:interiorPolygons:")]
		MKPolygon _FromCoordinates (IntPtr coords, nint count, [NullAllowed] MKPolygon [] interiorPolygons);

		#region MKAnnotation
		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }
		// note: setCoordinate: is not mandatory and is not implemented for MKPolygon (see unit tests)
		#endregion
	}

	[ThreadSafe]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKMultiPoint))]
	interface MKPolyline : MKOverlay, MKGeoJsonObject {
		[Static]
		[Export ("polylineWithCoordinates:count:")]
		[Internal]
		MKPolyline _FromCoordinates (IntPtr coords, nint count);

		[Static]
		[Internal]
		[Export ("polylineWithPoints:count:")]
		MKPolyline _FromPoints (IntPtr points, nint count);

		#region MKAnnotation
		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }
		// note: setCoordinate: is not mandatory and is not implemented for MKPolygon (see unit tests)
		#endregion
	}

	/// <summary>The visual representation of an <see cref="MapKit.MKPolyline" />. Application developers targeting iOS 7 and later should prefer to use <see cref="MapKit.MKPolylineRenderer" />.</summary>
	///     <remarks>
	///       <para>Application developers targeting iOS 7 and later should use the higher-performing, better-displaying <see cref="MapKit.MKPolylineRenderer" /> class.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKPolylineView_class/index.html">Apple documentation for <c>MKPolylineView</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MKPolylineRenderer' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKPolylineRenderer' instead.")]
	[BaseType (typeof (MKOverlayPathView))]
	interface MKPolylineView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the MKPolylineView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of MKPolylineView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("initWithPolyline:")]
		[PostGet ("Polyline")]
		NativeHandle Constructor (MKPolyline polyline);

		[Export ("polyline")]
		MKPolyline Polyline { get; }
	}

	[BaseType (typeof (MKShape))]
	[MacCatalyst (13, 1)]
	interface MKMultiPoint : MKGeoJsonObject {
		[Export ("points"), Internal]
		IntPtr _Points { get; }

		[Export ("pointCount")]
		nint PointCount { get; }

		[Export ("getCoordinates:range:"), Internal]
		void GetCoords (IntPtr dest, NSRange range);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("locationAtPointIndex:")]
		nfloat GetLocation (nuint pointIndex);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[return: BindAs (typeof (nfloat []))]
		[Export ("locationsAtPointIndexes:")]
		NSNumber [] GetLocations (NSIndexSet indexes);
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
#if XAMCORE_5_0
	interface MKUserLocation : MKAnnotation {
#else
	interface MKUserLocation : IMKAnnotation { // This is wrong. It should be MKAnnotation but we can't due to API compat. When you fix this remove hack in generator.cs to enable warning again. In the meantime, we're stating that MKUserLocation implements the IMKAnnotation protocol by using a manual binding.
#endif
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("updating")]
		bool Updating { [Bind ("isUpdating")] get; }

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("location", ArgumentSemantic.Retain)]
		[NullAllowed]
		CLLocation Location { get; }

		[Export ("title", ArgumentSemantic.Copy)]
		[NullAllowed]
		string Title { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("subtitle", ArgumentSemantic.Copy)]
		string Subtitle { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("heading", ArgumentSemantic.Retain)]
		[NullAllowed]
		CLHeading Heading { get; }
	}

	/// <summary>A specialized <see cref="UIKit.UIBarButtonItem" /> that allows the user to see and control the active <see cref="MapKit.MKUserTrackingMode" /> .</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MapKit/Reference/MKUserTrackingBarButtonItemClassRef/index.html">Apple documentation for <c>MKUserTrackingBarButtonItem</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIBarButtonItem))]
	[DisableDefaultCtor]
	interface MKUserTrackingBarButtonItem {
		[NullAllowed] // by default this property is null
		[Export ("mapView", ArgumentSemantic.Retain)]
		MKMapView MapView { get; set; }

		[DesignatedInitializer]
		[Export ("initWithMapView:")]
		[PostGet ("MapView")]
		NativeHandle Constructor ([NullAllowed] MKMapView mapView);
	}

	/// <param name="response">To be added.</param>
	/// <param name="error">To be added.</param>
	/// <summary>A delegate that is used to handle the results of a map-based search.</summary>
	/// <remarks>To be added.</remarks>
	/// <altmember cref="MapKit.MKLocalSearch" />
	delegate void MKLocalSearchCompletionHandler ([NullAllowed] MKLocalSearchResponse response, [NullAllowed] NSError error);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[ThreadSafe]
	[DisableDefaultCtor] // crash on iOS8 beta
	interface MKLocalSearch {

		[DesignatedInitializer]
		[Export ("initWithRequest:")]
		NativeHandle Constructor (MKLocalSearchRequest request);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithPointsOfInterestRequest:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKLocalPointsOfInterestRequest request);

		[Export ("startWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>To be added.</summary>
			<returns>
			          <para>A task that represents the asynchronous Start operation.   The value of the TResult parameter is a <see cref="MapKit.MKLocalSearchCompletionHandler" />.</para>
			        </returns>
			<remarks>
			          <para>(More documentation for this node is coming)</para>
			          <para tool="threads">This can be used from a background thread.</para>
			          <para copied="true">The StartAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para tool="threads" copied="true">This can be used from a background thread.</para>
			        </remarks>
			""")]
		void Start (MKLocalSearchCompletionHandler completionHandler);

		[Export ("cancel")]
		void Cancel ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[Export ("searching")]
		bool IsSearching { [Bind ("isSearching")] get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[ThreadSafe]
	[DesignatedDefaultCtor]
	interface MKLocalSearchRequest : NSCopying {

		[DesignatedInitializer]
		[MacCatalyst (13, 1)]
		[Export ("initWithCompletion:")]
		NativeHandle Constructor (MKLocalSearchCompletion completion);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithNaturalLanguageQuery:")]
		NativeHandle Constructor (string naturalLanguageQuery);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithNaturalLanguageQuery:region:")]
		NativeHandle Constructor (string naturalLanguageQuery, MKCoordinateRegion region);

		[Export ("naturalLanguageQuery", ArgumentSemantic.Copy)]
		[NullAllowed]
		string NaturalLanguageQuery { get; set; }

		[Export ("region", ArgumentSemantic.Assign)]
		MKCoordinateRegion Region { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("resultTypes", ArgumentSemantic.Assign)]
		MKLocalSearchResultType ResultTypes { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("regionPriority", ArgumentSemantic.Assign)]
		MKLocalSearchRegionPriority RegionPriority { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addressFilter", ArgumentSemantic.Copy), NullAllowed]
		MKAddressFilter AddressFilter { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[ThreadSafe]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** setObjectForKey: object cannot be nil (key: mapItems)
	[DisableDefaultCtor]
	interface MKLocalSearchResponse {

		[Export ("boundingRegion")]
		MKCoordinateRegion Region { get; }

		[Export ("mapItems")]
		MKMapItem [] MapItems { get; }
	}

	[BaseType (typeof (MKOverlayPathRenderer))]
	[MacCatalyst (13, 1)]
	partial interface MKCircleRenderer {

		[Export ("initWithCircle:")]
		NativeHandle Constructor (MKCircle circle);

		[Export ("circle")]
		MKCircle Circle { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeStart")]
		nfloat StrokeStart { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeEnd")]
		nfloat StrokeEnd { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: Cannot initialize MKDirections with nil request
	partial interface MKDirections {

		[DesignatedInitializer]
		[Export ("initWithRequest:")]
		NativeHandle Constructor (MKDirectionsRequest request);

		[Export ("calculateDirectionsWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Requests a directions calculation from Apple's servers and runs a completion handler when the request is complete.</summary>
			<returns>
			          <para>A task that represents the asynchronous CalculateDirections operation.   The value of the TResult parameter is a <see cref="MapKit.MKDirectionsHandler" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void CalculateDirections (MKDirectionsHandler completionHandler);

		[Export ("cancel")]
		void Cancel ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("calculating")]
		bool Calculating { [Bind ("isCalculating")] get; }

		[Export ("calculateETAWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Requests an ETA calculation from Apple's servers and runs a completion handler when the request is complete.</summary>
			<returns>
			          <para>A task that represents the asynchronous CalculateETA operation.   The value of the TResult parameter is a <see cref="MapKit.MKETAHandler" />.</para>
			        </returns>
			<remarks>
			          <para copied="true">The CalculateETAAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void CalculateETA (MKETAHandler completionHandler);
	}

	/// <param name="response">Returned if the routing request was successful</param>
	/// <param name="error">If not <see langword="null" />, an error occurred with the request.</param>
	/// <summary>The completion handler for calls to <see cref="MapKit.MKDirections.CalculateDirections(MapKit.MKDirectionsHandler)" />.</summary>
	/// <remarks>To be added.</remarks>
	/// <altmember cref="MapKit.MKDirectionsRequest" />
	delegate void MKDirectionsHandler ([NullAllowed] MKDirectionsResponse response, [NullAllowed] NSError error);

	/// <param name="response">Returned if the request was successful.</param>
	/// <param name="error">If not <see langword="null" />, an error occurred with the request.</param>
	/// <summary>The completion handler for calls to <see cref="MapKit.MKDirections.CalculateETA(MapKit.MKETAHandler)" />.</summary>
	/// <remarks>To be added.</remarks>
	delegate void MKETAHandler ([NullAllowed] MKETAResponse response, [NullAllowed] NSError error);

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKETAResponse {
		[Export ("source")]
		MKMapItem Source { get; }

		[Export ("destination")]
		MKMapItem Destination { get; }

		[Export ("expectedTravelTime")]
		double ExpectedTravelTime { get; }

		[MacCatalyst (13, 1)]
		[Export ("distance")]
		double /* CLLocationDistance */ Distance { get; }

		[Export ("transportType")]
		[MacCatalyst (13, 1)]
		MKDirectionsTransportType TransportType { get; }

		[Export ("expectedArrivalDate")]
		[MacCatalyst (13, 1)]
		NSDate ExpectedArrivalDate { get; }

		[Export ("expectedDepartureDate")]
		[MacCatalyst (13, 1)]
		NSDate ExpectedDepartureDate { get; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKDirectionsResponse {

		[Export ("source")]
		MKMapItem Source { get; }

		[Export ("destination")]
		MKMapItem Destination { get; }

		[Export ("routes")]
		MKRoute [] Routes { get; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKRoute {

		[Export ("name")]
		string Name { get; }

		[Export ("advisoryNotices")]
		string [] AdvisoryNotices { get; }

		[Export ("distance")]
		double Distance { get; }

		[Export ("expectedTravelTime")]
		double ExpectedTravelTime { get; }

		[Export ("transportType")]
		MKDirectionsTransportType TransportType { get; }

		[Export ("polyline")]
		MKPolyline Polyline { get; }

		[Export ("steps")]
		MKRouteStep [] Steps { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("hasTolls")]
		bool HasTolls { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("hasHighways")]
		bool HasHighways { get; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKRouteStep {

		[Export ("instructions")]
		string Instructions { get; }

		[Export ("notice")]
		[NullAllowed]
		string Notice { get; }

		[Export ("polyline")]
		MKPolyline Polyline { get; }

		[Export ("distance")]
		double Distance { get; }

		[Export ("transportType")]
		MKDirectionsTransportType TransportType { get; }
	}

	[BaseType (typeof (NSFormatter))]
	[MacCatalyst (13, 1)]
	partial interface MKDistanceFormatter {

		[Export ("stringFromDistance:")]
		string StringFromDistance (double distance);

		[Export ("distanceFromString:")]
		double DistanceFromString (string distance);

		[Export ("locale", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSLocale Locale { get; set; }

		[Export ("units", ArgumentSemantic.Assign)]
		MKDistanceFormatterUnits Units { get; set; }

		[Export ("unitStyle", ArgumentSemantic.Assign)]
		MKDistanceFormatterUnitStyle UnitStyle { get; set; }
	}

	[BaseType (typeof (MKPolyline))]
	[MacCatalyst (13, 1)]
	partial interface MKGeodesicPolyline {

		[Static, Export ("polylineWithPoints:count:")]
		[Internal]
		MKGeodesicPolyline PolylineWithPoints (IntPtr points, nint count);

		[Static, Export ("polylineWithCoordinates:count:")]
		[Internal]
		MKGeodesicPolyline PolylineWithCoordinates (IntPtr coords, nint count);
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKMapCamera : NSCopying, NSSecureCoding {

		[Export ("centerCoordinate")]
		CLLocationCoordinate2D CenterCoordinate { get; set; }

		[Export ("heading")]
		double Heading { get; set; }

		[Export ("pitch")]
		nfloat Pitch { get; set; }

		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'CenterCoordinateDistance' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'CenterCoordinateDistance' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'CenterCoordinateDistance' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CenterCoordinateDistance' instead.")]
		[Export ("altitude")]
		double Altitude { get; set; }

		[Static, Export ("camera")]
		MKMapCamera Camera { get; }

		[Static, Export ("cameraLookingAtCenterCoordinate:fromEyeCoordinate:eyeAltitude:")]
		MKMapCamera CameraLookingAtCenterCoordinate (CLLocationCoordinate2D centerCoordinate, CLLocationCoordinate2D eyeCoordinate, double eyeAltitude);

		/// <param name="centerCoordinate">To be added.</param>
		/// <param name="locationDistance">To be added.</param>
		/// <param name="pitch">To be added.</param>
		/// <param name="locationDirectionHeading">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static]
		[MacCatalyst (13, 1)]
		[Export ("cameraLookingAtCenterCoordinate:fromDistance:pitch:heading:")]
		MKMapCamera CameraLookingAtCenterCoordinate (CLLocationCoordinate2D centerCoordinate, double locationDistance, nfloat pitch, double locationDirectionHeading);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("centerCoordinateDistance")]
		double CenterCoordinateDistance { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Static]
		[Export ("cameraLookingAtMapItem:forViewSize:allowPitch:")]
		MKMapCamera CameraLookingAt (MKMapItem mapItem, CGSize viewSize, bool allowPitch);
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKMapSnapshot {

		[Export ("image")]
		UIImage Image { get; }

		[Export ("pointForCoordinate:")]
		CGPoint PointForCoordinate (CLLocationCoordinate2D coordinate);

		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		[Export ("appearance")]
		NSAppearance Appearance { get; }

		[TV (13, 0), NoMac, iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("traitCollection")]
		UITraitCollection TraitCollection { get; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKMapSnapshotOptions : NSCopying {

		[Export ("camera", ArgumentSemantic.Copy)]
		MKMapCamera Camera { get; set; }

		[Export ("mapRect", ArgumentSemantic.Assign)]
		MKMapRect MapRect { get; set; }

		[Export ("region", ArgumentSemantic.Assign)]
		MKCoordinateRegion Region { get; set; }

		[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use 'PreferredConfiguration' instead.")]
		[Deprecated (PlatformName.iOS, 17, 0, message: "Use 'PreferredConfiguration' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use 'PreferredConfiguration' instead.")]
		[Deprecated (PlatformName.TvOS, 17, 0, message: "Use 'PreferredConfiguration' instead.")]
		[Export ("mapType", ArgumentSemantic.Assign)]
		MKMapType MapType { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TraitCollection.DisplayScale' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TraitCollection.DisplayScale' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'TraitCollection.DisplayScale' instead.")]
		[Export ("scale", ArgumentSemantic.Assign)]
		nfloat Scale { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'PointOfInterestFilter' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PointOfInterestFilter' instead.")]
		[Export ("showsPointsOfInterest")]
		bool ShowsPointsOfInterest { get; set; }

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Export ("showsBuildings")]
		bool ShowsBuildings { get; set; }

		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		[NullAllowed, Export ("appearance", ArgumentSemantic.Strong)]
		NSAppearance Appearance { get; set; }

		[Deprecated (PlatformName.MacOSX, 14, 0)]
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[TV (13, 0), NoMac, iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("traitCollection", ArgumentSemantic.Copy)]
		UITraitCollection TraitCollection { get; set; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("preferredConfiguration", ArgumentSemantic.Copy)]
		MKMapConfiguration PreferredConfiguration { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKMapSnapshotter {

		[DesignatedInitializer]
		[Export ("initWithOptions:")]
		NativeHandle Constructor (MKMapSnapshotOptions options);

		[Export ("startWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>To be added.</summary>
			<returns>
			          <para>A task that represents the asynchronous Start operation.   The value of the TResult parameter is a <see cref="MapKit.MKMapSnapshotCompletionHandler" />.</para>
			        </returns>
			<remarks>
			          <para copied="true">The StartAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void Start (MKMapSnapshotCompletionHandler completionHandler);

		[Export ("startWithQueue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="queue">The dispatch queue to which to add the request.</param>
			<summary>Puts a request that a snapshot be generated on the provided dispatch queue, returning a task that provides the snapshot when it is ready.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		void Start (DispatchQueue queue, MKMapSnapshotCompletionHandler completionHandler);

		[Export ("cancel")]
		void Cancel ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("loading")]
		bool Loading { [Bind ("isLoading")] get; }
	}

	/// <param name="snapshot">The newly-created <see cref="MapKit.MKMapSnapshot" /></param>
	/// <param name="error">If not <see langword="null" />, an error occurred with the request.</param>
	/// <summary>The completion handler for <see cref="MapKit.MKMapSnapshotter.StartAsync(CoreFoundation.DispatchQueue)" />.</summary>
	/// <remarks>To be added.</remarks>
	delegate void MKMapSnapshotCompletionHandler ([NullAllowed] MKMapSnapshot snapshot, [NullAllowed] NSError error);

	[BaseType (typeof (MKOverlayRenderer))]
	[MacCatalyst (13, 1)]
	[ThreadSafe]
	partial interface MKOverlayPathRenderer {

		[Export ("initWithOverlay:")]
		NativeHandle Constructor (IMKOverlay overlay);

		[NullAllowed] // by default this property is null
		[Export ("fillColor", ArgumentSemantic.Retain)]
		UIColor FillColor { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("lineWidth")]
		nfloat LineWidth { get; set; }

		[Export ("lineJoin")]
		CGLineJoin LineJoin { get; set; }

		[Export ("lineCap")]
		CGLineCap LineCap { get; set; }

		[Export ("miterLimit")]
		nfloat MiterLimit { get; set; }

		[Export ("lineDashPhase")]
		nfloat LineDashPhase { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("lineDashPattern", ArgumentSemantic.Copy)]
		NSNumber [] LineDashPattern { get; set; }

		[Export ("createPath")]
		void CreatePath ();

		[NullAllowed]
		[Export ("path")]
		CGPath Path { get; set; }

		[Export ("invalidatePath")]
		void InvalidatePath ();

		/// <param name="context">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[Export ("applyStrokePropertiesToContext:atZoomScale:")]
		void ApplyStrokePropertiesToContext (CGContext context, /* MKZoomScale */ nfloat zoomScale);

		/// <param name="context">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[Export ("applyFillPropertiesToContext:atZoomScale:")]
		void ApplyFillPropertiesToContext (CGContext context, /* MKZoomScale */ nfloat zoomScale);

		[Export ("strokePath:inContext:")]
		void StrokePath (CGPath path, CGContext context);

		[Export ("fillPath:inContext:")]
		void FillPath (CGPath path, CGContext context);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("shouldRasterize")]
		bool ShouldRasterize { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[MacCatalyst (13, 1)]
	partial interface MKOverlayRenderer {

		[DesignatedInitializer]
		[Export ("initWithOverlay:")]
		NativeHandle Constructor (IMKOverlay overlay);

		[Export ("overlay")]
		IMKOverlay Overlay { get; }

		[ThreadSafe]
		[Export ("pointForMapPoint:")]
		CGPoint PointForMapPoint (MKMapPoint mapPoint);

		[ThreadSafe]
		[Export ("mapPointForPoint:")]
		MKMapPoint MapPointForPoint (CGPoint point);

		[ThreadSafe]
		[Export ("rectForMapRect:")]
		CGRect RectForMapRect (MKMapRect mapRect);

		[ThreadSafe]
		[Export ("mapRectForRect:")]
		MKMapRect MapRectForRect (CGRect rect);

		/// <param name="mapRect">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("canDrawMapRect:zoomScale:")]
		bool CanDrawMapRect (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale);

		/// <param name="mapRect">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <param name="context">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[ThreadSafe]
		[Export ("drawMapRect:zoomScale:inContext:")]
		void DrawMapRect (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale, CGContext context);

		[Export ("setNeedsDisplay")]
		void SetNeedsDisplay ();

		[Export ("setNeedsDisplayInMapRect:")]
		void SetNeedsDisplay (MKMapRect mapRect);

		/// <param name="mapRect">To be added.</param>
		/// <param name="zoomScale">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setNeedsDisplayInMapRect:zoomScale:")]
		void SetNeedsDisplay (MKMapRect mapRect, /* MKZoomScale */ nfloat zoomScale);

		[Export ("alpha")]
		nfloat Alpha { get; set; }

		[Export ("contentScaleFactor")]
		nfloat ContentScaleFactor { get; }

		[NoMac, iOS (16, 0), NoMacCatalyst, TV (16, 0)]
		[Export ("blendMode", ArgumentSemantic.Assign)]
		CGBlendMode BlendMode { get; set; }
	}

	[BaseType (typeof (MKOverlayPathRenderer))]
	[MacCatalyst (13, 1)]
	partial interface MKPolygonRenderer {

		[Export ("initWithPolygon:")]
		NativeHandle Constructor (MKPolygon polygon);

		[Export ("polygon")]
		MKPolygon Polygon { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeStart")]
		nfloat StrokeStart { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeEnd")]
		nfloat StrokeEnd { get; set; }
	}

	[BaseType (typeof (MKOverlayPathRenderer))]
	[MacCatalyst (13, 1)]
	partial interface MKPolylineRenderer {

		[Export ("initWithPolyline:")]
		NativeHandle Constructor (MKPolyline polyline);

		[Export ("polyline")]
		MKPolyline Polyline { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeStart")]
		nfloat StrokeStart { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokeEnd")]
		nfloat StrokeEnd { get; set; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (MKPolylineRenderer))]
	partial interface MKGradientPolylineRenderer {
		[Export ("locations", ArgumentSemantic.Copy)]
		[BindAs (typeof (nfloat []))]
		NSNumber [] Locations { get; }

		[Export ("colors", ArgumentSemantic.Copy)]
		UIColor [] Colors { get; }

		[Export ("setColors:atLocations:")]
		void SetColors (UIColor [] colors, [BindAs (typeof (nfloat []))] NSNumber [] locations);
	}

	[ThreadSafe]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MKTileOverlay : MKOverlay {
		[DesignatedInitializer]
		[Export ("initWithURLTemplate:")]
		NativeHandle Constructor ([NullAllowed] string URLTemplate);

		[Export ("tileSize")]
		CGSize TileSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[Export ("geometryFlipped")]
		bool GeometryFlipped { [Bind ("isGeometryFlipped")] get; set; }

		[Export ("minimumZ")]
		nint MinimumZ { get; set; }

		[Export ("maximumZ")]
		nint MaximumZ { get; set; }

		[Export ("URLTemplate")]
		[NullAllowed]
		string URLTemplate { get; }

#pragma warning disable 0109 // warning CS0109: The member 'MKTileOverlay.CanReplaceMapContent' does not hide an accessible member. The new keyword is not required.
		[Export ("canReplaceMapContent")]
		new bool CanReplaceMapContent { get; set; }
#pragma warning restore

		[Export ("URLForTilePath:")]
		NSUrl URLForTilePath (MKTileOverlayPath path);

		[Export ("loadTileAtPath:result:")]
		void LoadTileAtPath (MKTileOverlayPath path, MKTileOverlayLoadTileCompletionHandler result);

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }
	}

	/// <param name="tileData">To be added.</param>
	/// <param name="error">To be added.</param>
	/// <summary>The completion handler for <see cref="MapKit.MKTileOverlay.LoadTileAtPath(MapKit.MKTileOverlayPath,MapKit.MKTileOverlayLoadTileCompletionHandler)" />.</summary>
	/// <remarks>To be added.</remarks>
	delegate void MKTileOverlayLoadTileCompletionHandler ([NullAllowed] NSData tileData, [NullAllowed] NSError error);

	[BaseType (typeof (MKOverlayRenderer))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: Expected a MKTileOverlay but got (null)
	[DisableDefaultCtor] // throw in iOS8 beta 1 ^
	[MacCatalyst (13, 1)]
	partial interface MKTileOverlayRenderer {
		// This ctor is not allowed: NSInvalidArgumentEception Expected a MKTileOverlay
		//		[Export ("initWithOverlay:")]
		//		NativeHandle Constructor (IMKOverlay toverlay);

		[Export ("initWithTileOverlay:")]
		NativeHandle Constructor (MKTileOverlay overlay);

		[Export ("reloadData")]
		void ReloadData ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MKLocalSearchCompleter {
		[Export ("queryFragment")]
		string QueryFragment { get; set; }

		[Export ("region", ArgumentSemantic.Assign)]
		MKCoordinateRegion Region { get; set; }

		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'ResultTypes' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'ResultTypes' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'ResultTypes' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ResultTypes' instead.")]
		[Export ("filterType", ArgumentSemantic.Assign)]
		MKSearchCompletionFilterType FilterType { get; set; }

		/// <summary>An instance of the MapKit.IMKLocalSearchCompleterDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MapKit.IMKLocalSearchCompleterDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IMKLocalSearchCompleterDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("results", ArgumentSemantic.Strong)]
		MKLocalSearchCompletion [] Results { get; }

		/// <summary>Gets a <see langword="bool" /> that tells whether a search is in progress.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("searching")]
		bool Searching { [Bind ("isSearching")] get; }

		[Export ("cancel")]
		void Cancel ();

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("resultTypes", ArgumentSemantic.Assign)]
		MKLocalSearchCompleterResultType ResultTypes { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("regionPriority", ArgumentSemantic.Assign)]
		MKLocalSearchRegionPriority RegionPriority { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addressFilter", ArgumentSemantic.Copy), NullAllowed]
		MKAddressFilter AddressFilter { get; set; }
	}

	interface IMKLocalSearchCompleterDelegate { }

	[MacCatalyst (13, 1)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface MKLocalSearchCompleterDelegate {
		/// <param name="completer">The search completer to which this delegate belongs.</param>
		/// <summary>The search completer updated the results with new search completions.</summary>
		/// <remarks>
		///           <para>After this method is called, developers can check the <see cref="MapKit.MKLocalSearchCompleter.Results" /> property for the newest results.</para>
		///         </remarks>
		[Export ("completerDidUpdateResults:")]
		void DidUpdateResults (MKLocalSearchCompleter completer);

		/// <param name="completer">The search completer to which this delegate belongs.</param>
		/// <param name="error">The error that occured.</param>
		/// <summary>The search completer encountered an error while searching for completions.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("completer:didFailWithError:")]
		void DidFail (MKLocalSearchCompleter completer, NSError error);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLocalSearchCompletion {
		[Export ("title", ArgumentSemantic.Strong)]
		string Title { get; }

		// NSValue-wrapped NSRanges
		[Export ("titleHighlightRanges", ArgumentSemantic.Strong)]
		NSValue [] TitleHighlightRanges { get; }

		[Export ("subtitle", ArgumentSemantic.Strong)]
		string Subtitle { get; }

		// NSValue-wrapped NSRanges
		[Export ("subtitleHighlightRanges", ArgumentSemantic.Strong)]
		NSValue [] SubtitleHighlightRanges { get; }
	}

	/// <summary>Extension class for getting and setting map items on a <see cref="Foundation.NSUserActivity" /> object.</summary>
	/// <remarks>To be added.</remarks>
	[Category]
	[BaseType (typeof (NSUserActivity))]
	interface NSUserActivity_MKMapItem {
		/// <summary>Gets the mapkit item.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("mapItem")]
		MKMapItem GetMapItem ();

		/// <param name="item">The new mapkit item.</param>
		/// <summary>Sets the mapkit item to <paramref name="item" />.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setMapItem:")]
		void SetMapItem (MKMapItem item);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKClusterAnnotation : MKAnnotation {
		[NullAllowed, Export ("title")]
		new string Title { get; set; }

		[NullAllowed, Export ("subtitle")]
		new string Subtitle { get; set; }

		[Export ("memberAnnotations")]
		IMKAnnotation [] MemberAnnotations { get; }

		[Export ("initWithMemberAnnotations:")]
		[DesignatedInitializer]
		NativeHandle Constructor (IMKAnnotation [] memberAnnotations);
	}

	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	[DisableDefaultCtor]
	interface MKCompassButton {
		[Static]
		[Export ("compassButtonWithMapView:")]
		MKCompassButton FromMapView ([NullAllowed] MKMapView mapView);

		[NullAllowed, Export ("mapView", ArgumentSemantic.Weak)]
		MKMapView MapView { get; set; }

		[Export ("compassVisibility", ArgumentSemantic.Assign)]
		MKFeatureVisibility CompassVisibility { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKAnnotationView))]
	interface MKMarkerAnnotationView {

		// inlined from base type
		[Export ("initWithAnnotation:reuseIdentifier:")]
		[PostGet ("Annotation")]
		NativeHandle Constructor ([NullAllowed] IMKAnnotation annotation, [NullAllowed] string reuseIdentifier);

		[Export ("titleVisibility", ArgumentSemantic.Assign)]
		MKFeatureVisibility TitleVisibility { get; set; }

		[Export ("subtitleVisibility", ArgumentSemantic.Assign)]
		MKFeatureVisibility SubtitleVisibility { get; set; }

		/// <summary>Gets or sets the background color of the balloon.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Appearance]
		[NullAllowed, Export ("markerTintColor", ArgumentSemantic.Copy)]
		UIColor MarkerTintColor { get; set; }

		/// <summary>Gets or sets the tint to apply to the image or text of the glyph.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Appearance]
		[NullAllowed, Export ("glyphTintColor", ArgumentSemantic.Copy)]
		UIColor GlyphTintColor { get; set; }

		/// <summary>Gets or sets the text to display in the marker balloon.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Appearance]
		[NullAllowed, Export ("glyphText")]
		string GlyphText { get; set; }

		/// <summary>Gets or sets the image to display in the marker balloon.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Appearance]
		[NullAllowed, Export ("glyphImage", ArgumentSemantic.Copy)]
		UIImage GlyphImage { get; set; }

		/// <summary>Gets or sets the image to display when the marker is selected.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Appearance]
		[NullAllowed, Export ("selectedGlyphImage", ArgumentSemantic.Copy)]
		UIImage SelectedGlyphImage { get; set; }

		[Export ("animatesWhenAdded")]
		bool AnimatesWhenAdded { get; set; }
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	[DisableDefaultCtor]
	interface MKScaleView {

		[Static]
		[Export ("scaleViewWithMapView:")]
		MKScaleView FromMapView ([NullAllowed] MKMapView mapView);

		[NullAllowed, Export ("mapView", ArgumentSemantic.Weak)]
		MKMapView MapView { get; set; }

		[Export ("scaleVisibility", ArgumentSemantic.Assign)]
		MKFeatureVisibility ScaleVisibility { get; set; }

		[Export ("legendAlignment", ArgumentSemantic.Assign)]
		MKScaleViewAlignment LegendAlignment { get; set; }
	}

	/// <summary>A button for toggling the tracking mode.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	[DisableDefaultCtor]
	interface MKUserTrackingButton {
		[Static]
		[Export ("userTrackingButtonWithMapView:")]
		MKUserTrackingButton FromMapView ([NullAllowed] MKMapView mapView);

		[NullAllowed, Export ("mapView", ArgumentSemantic.Weak)]
		MKMapView MapView { get; set; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MKPointOfInterestFilter : NSSecureCoding, NSCopying {
		[Static]
		[Export ("filterIncludingAllCategories")]
		MKPointOfInterestFilter FilterIncludingAllCategories { get; }

		[Static]
		[Export ("filterExcludingAllCategories")]
		MKPointOfInterestFilter FilterExcludingAllCategories { get; }

		[Internal]
		[Export ("initIncludingCategories:")]
		IntPtr _InitIncludingCategories ([BindAs (typeof (MKPointOfInterestCategory []))] NSString [] categories);

		[Internal]
		[Export ("initExcludingCategories:")]
		IntPtr _InitExcludingCategories ([BindAs (typeof (MKPointOfInterestCategory []))] NSString [] categories);

		[Export ("includesCategory:")]
		bool IncludesCategory ([BindAs (typeof (MKPointOfInterestCategory))] NSString category);

		[Export ("excludesCategory:")]
		bool ExcludesCategory ([BindAs (typeof (MKPointOfInterestCategory))] NSString category);
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol (Name = "MKGeoJSONObject")]
	interface MKGeoJsonObject { }

	interface IMKGeoJsonObject { }

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "MKGeoJSONDecoder")]
	interface MKGeoJsonDecoder {
		[Export ("geoJSONObjectsWithData:error:")]
		[return: NullAllowed]
		IMKGeoJsonObject [] GeoJsonObjects (NSData data, [NullAllowed] out NSError error);
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "MKGeoJSONFeature")]
	interface MKGeoJsonFeature : MKGeoJsonObject {
		[NullAllowed, Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("properties")]
		NSData Properties { get; }

		[Export ("geometry")]
		IMKGeoJsonObject [] Geometry { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MKMapCameraZoomRange : NSSecureCoding, NSCopying {
		[Export ("initWithMinCenterCoordinateDistance:maxCenterCoordinateDistance:")]
		[DesignatedInitializer]
		NativeHandle Constructor (double minDistance, double maxDistance);

		[Internal]
		[Export ("initWithMinCenterCoordinateDistance:")]
		IntPtr _InitWithMinCenterCoordinateDistance (double minDistance);

		[Internal]
		[Export ("initWithMaxCenterCoordinateDistance:")]
		IntPtr _InitWithMaxCenterCoordinateDistance (double maxDistance);

		[Export ("minCenterCoordinateDistance")]
		double MinCenterCoordinateDistance { get; }

		[Export ("maxCenterCoordinateDistance")]
		double MaxCenterCoordinateDistance { get; }

		[Field ("MKMapCameraZoomDefault")]
		double ZoomDefault { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MKMapCameraBoundary : NSSecureCoding, NSCopying {
		[Export ("initWithMapRect:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapRect mapRect);

		[Export ("initWithCoordinateRegion:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKCoordinateRegion region);

		[Export ("mapRect")]
		MKMapRect MapRect { get; }

		[Export ("region")]
		MKCoordinateRegion Region { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKShape))]
	interface MKMultiPolygon : MKOverlay, MKGeoJsonObject {
		[Export ("initWithPolygons:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKPolygon [] polygons);

		[Export ("polygons", ArgumentSemantic.Copy)]
		MKPolygon [] Polygons { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKOverlayPathRenderer))]
	interface MKMultiPolygonRenderer {
		[Export ("initWithMultiPolygon:")]
		NativeHandle Constructor (MKMultiPolygon multiPolygon);

		[Export ("multiPolygon")]
		MKMultiPolygon MultiPolygon { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKShape))]
	interface MKMultiPolyline : MKOverlay, MKGeoJsonObject {
		[Export ("initWithPolylines:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKPolyline [] polylines);

		[Export ("polylines", ArgumentSemantic.Copy)]
		MKPolyline [] Polylines { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MKOverlayPathRenderer))]
	interface MKMultiPolylineRenderer {
		[Export ("initWithMultiPolyline:")]
		NativeHandle Constructor (MKMultiPolyline multiPolyline);

		[Export ("multiPolyline")]
		MKMultiPolyline MultiPolyline { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (MKAnnotationView))]
	interface MKUserLocationView {
		[DesignatedInitializer]
		[Export ("initWithAnnotation:reuseIdentifier:")]
		NativeHandle Constructor ([NullAllowed] IMKAnnotation annotation, [NullAllowed] string reuseIdentifier);

		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLocalPointsOfInterestRequest : NSCopying {
		[Field ("MKPointsOfInterestRequestMaxRadius")]
		double RequestMaxRadius { get; }

		[Export ("initWithCenterCoordinate:radius:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CLLocationCoordinate2D centerCoordinate, double radius);

		[Export ("initWithCoordinateRegion:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKCoordinateRegion region);

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }

		[Export ("radius")]
		double Radius { get; }

		[Export ("region")]
		MKCoordinateRegion Region { get; }

		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV, NoiOS]
	[DisableDefaultCtor]
	[BaseType (typeof (UIView))]
	interface MKPitchControl {
		[DesignatedInitializer]
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		[Static]
		[Export ("pitchControlWithMapView:")]
		MKPitchControl Create ([NullAllowed] MKMapView mapView);

		[NullAllowed, Export ("mapView", ArgumentSemantic.Weak)]
		MKMapView MapView { get; set; }
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV, NoiOS]
	[DisableDefaultCtor]
	[BaseType (typeof (UIView))]
	interface MKZoomControl {

		[DesignatedInitializer]
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		[Static]
		[Export ("zoomControlWithMapView:")]
		MKZoomControl Create ([NullAllowed] MKMapView mapView);

		[NullAllowed, Export ("mapView", ArgumentSemantic.Weak)]
		MKMapView MapView { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[BaseType (typeof (MKMapConfiguration))]
	[DesignatedDefaultCtor]
	interface MKHybridMapConfiguration {
		[Export ("initWithElevationStyle:")]
		NativeHandle Constructor (MKMapElevationStyle elevationStyle);

		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[Export ("showsTraffic")]
		bool ShowsTraffic { get; set; }
	}

	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKIconStyle {
		[Export ("backgroundColor")]
		UIColor BackgroundColor { get; }

		[Export ("image")]
		UIImage Image { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[BaseType (typeof (MKMapConfiguration))]
	[DesignatedDefaultCtor]
	interface MKImageryMapConfiguration {
		[Export ("initWithElevationStyle:")]
		NativeHandle Constructor (MKMapElevationStyle elevationStyle);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLookAroundScene : NSCopying { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLookAroundSceneRequest {
		[Export ("initWithCoordinate:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate);

		[Export ("initWithMapItem:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapItem mapItem);

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }

		[NullAllowed, Export ("mapItem")]
		MKMapItem MapItem { get; }

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("isLoading")]
		bool IsLoading { get; }

		[Async]
		[Export ("getSceneWithCompletionHandler:")]
		void GetScene (Action<MKLookAroundScene, NSError> completionHandler);

		[Export ("cancel")]
		void Cancel ();
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLookAroundSnapshot {
		[Export ("image")]
		UIImage Image { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	interface MKLookAroundSnapshotOptions {
		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[Export ("size", ArgumentSemantic.Assign)]
		CGSize Size { get; set; }

		[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("traitCollection", ArgumentSemantic.Copy)]
		UITraitCollection TraitCollection { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKLookAroundSnapshotter {
		[Export ("initWithScene:options:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKLookAroundScene scene, MKLookAroundSnapshotOptions options);

		[Async]
		[Export ("getSnapshotWithCompletionHandler:")]
		void GetSnapshot (Action<MKLookAroundSnapshot, NSError> completionHandler);

		[Export ("cancel")]
		void Cancel ();

		[Export ("isLoading")]
		bool IsLoading { get; }
	}

	interface IMKLookAroundViewControllerDelegate { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface MKLookAroundViewControllerDelegate {
		[Export ("lookAroundViewControllerWillUpdateScene:")]
		void WillUpdateScene (MKLookAroundViewController viewController);

		[Export ("lookAroundViewControllerDidUpdateScene:")]
		void DidUpdateScene (MKLookAroundViewController viewController);

		[Export ("lookAroundViewControllerWillPresentFullScreen:")]
		void WillPresentFullScreen (MKLookAroundViewController viewController);

		[Export ("lookAroundViewControllerDidPresentFullScreen:")]
		void DidPresentFullScreen (MKLookAroundViewController viewController);

		[Export ("lookAroundViewControllerWillDismissFullScreen:")]
		void WillDismissFullScreen (MKLookAroundViewController viewController);

		[Export ("lookAroundViewControllerDidDismissFullScreen:")]
		void DidDismissFullScreen (MKLookAroundViewController viewController);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (UIViewController))]
	interface MKLookAroundViewController : NSSecureCoding, NSCoding {
		[Export ("initWithScene:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKLookAroundScene scene);

		[Export ("initWithNibName:bundle:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle nibBundle);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IMKLookAroundViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("scene", ArgumentSemantic.Copy)]
		MKLookAroundScene Scene { get; set; }

		[Export ("navigationEnabled")]
		bool NavigationEnabled { [Bind ("isNavigationEnabled")] get; set; }

		[Export ("showsRoadLabels")]
		bool ShowsRoadLabels { get; set; }

		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[Export ("badgePosition", ArgumentSemantic.Assign)]
		MKLookAroundBadgePosition BadgePosition { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapConfiguration : NSSecureCoding, NSCopying {
		[Export ("elevationStyle", ArgumentSemantic.Assign)]
		MKMapElevationStyle ElevationStyle { get; set; }
	}

	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapFeatureAnnotation : MKAnnotation {
		[Export ("featureType")]
		MKMapFeatureType FeatureType { get; }

		[NullAllowed, Export ("iconStyle")]
		MKIconStyle IconStyle { get; }

		[BindAs (typeof (MKPointOfInterestCategory))]
		[NullAllowed, Export ("pointOfInterestCategory")]
		NSString PointOfInterestCategory { get; }
	}

	[Mac (15, 0), iOS (16, 0), MacCatalyst (16, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapItemRequest {
		[NoMac, NoTV]
		[Export ("initWithMapFeatureAnnotation:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapFeatureAnnotation mapFeatureAnnotation);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("initWithMapItemIdentifier:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapItemIdentifier identifier);

		[Async]
		[Export ("getMapItemWithCompletionHandler:")]
		void GetMapItem (Action<MKMapItem, NSError> completionHandler);

		[Export ("cancel")]
		void Cancel ();

		[NoMac, NoTV]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'MapFeatureAnnotation' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'MapFeatureAnnotation' instead.")]
		[Export ("featureAnnotation")]
		MKMapFeatureAnnotation FeatureAnnotation { get; }

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("isLoading")]
		bool IsLoading { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("mapItemIdentifier"), NullAllowed]
		MKMapItemIdentifier MapItemIdentifier { get; }

		[NoTV, NoMac, iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("mapFeatureAnnotation"), NullAllowed]
		MKMapFeatureAnnotation MapFeatureAnnotation { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[BaseType (typeof (MKMapConfiguration))]
	[DesignatedDefaultCtor]
	interface MKStandardMapConfiguration {
		[Export ("initWithElevationStyle:")]
		NativeHandle Constructor (MKMapElevationStyle elevationStyle);

		[Export ("initWithElevationStyle:emphasisStyle:")]
		NativeHandle Constructor (MKMapElevationStyle elevationStyle, MKStandardMapEmphasisStyle emphasisStyle);

		[Export ("initWithEmphasisStyle:")]
		NativeHandle Constructor (MKStandardMapEmphasisStyle emphasisStyle);

		[Export ("emphasisStyle", ArgumentSemantic.Assign)]
		MKStandardMapEmphasisStyle EmphasisStyle { get; set; }

		[NullAllowed, Export ("pointOfInterestFilter", ArgumentSemantic.Copy)]
		MKPointOfInterestFilter PointOfInterestFilter { get; set; }

		[Export ("showsTraffic")]
		bool ShowsTraffic { get; set; }
	}

	[Flags]
	[Native]
	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MKAddressFilterOption : ulong {
		Country = 1 << 0,
		AdministrativeArea = 1 << 1,
		SubAdministrativeArea = 1 << 2,
		Locality = 1 << 3,
		SubLocality = 1 << 4,
		PostalCode = 1 << 5,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKAddressFilter : NSSecureCoding, NSCopying {
		[Static]
		[Export ("filterIncludingAll")]
		MKAddressFilter IncludingAll { get; }

		[Static]
		[Export ("filterExcludingAll")]
		MKAddressFilter ExcludingAll { get; }

		[Export ("initIncludingOptions:")]
		// [DesignatedInitializer] // this isn't a constructor, so [DesignatedInitializer] doesn't work
		[Internal]
		NativeHandle _InitIncludingOptions (MKAddressFilterOption options);

		[Export ("initExcludingOptions:")]
		// [DesignatedInitializer] // this isn't a constructor, so [DesignatedInitializer] doesn't work
		[Internal]
		NativeHandle _InitExcludingOptions (MKAddressFilterOption options);

		[Export ("includesOptions:")]
		bool Includes (MKAddressFilterOption options);

		[Export ("excludesOptions:")]
		bool Excludes (MKAddressFilterOption options);
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapItemAnnotation : MKAnnotation {
		[Export ("initWithMapItem:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapItem mapItem);

		[Export ("mapItem", ArgumentSemantic.Strong)]
		MKMapItem MapItem { get; }
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface MKMapItemDetailViewControllerDelegate {
		[Abstract]
		[Export ("mapItemDetailViewControllerDidFinish:")]
		void DidFinish (MKMapItemDetailViewController detailViewController);
	}

	interface IMKMapItemDetailViewControllerDelegate { }

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface MKMapItemDetailViewController {
		[Export ("mapItem", ArgumentSemantic.Strong), NullAllowed]
		MKMapItem MapItem { get; set; }

		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IMKMapItemDetailViewControllerDelegate Delegate { get; set; }

		[Export ("initWithMapItem:displaysMap:")]
		NativeHandle Constructor ([NullAllowed] MKMapItem mapItem, bool displaysMap);

		[Export ("initWithMapItem:")]
		NativeHandle Constructor ([NullAllowed] MKMapItem mapItem);
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapItemIdentifier : NSCopying, NSSecureCoding {
		[Export ("initWithIdentifierString:")]
		NativeHandle Constructor (string identifier);

		[Export ("identifierString")]
		string IdentifierString { get; }
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKSelectionAccessory {
		[Static]
		[Export ("mapItemDetailWithPresentationStyle:")]
		MKSelectionAccessory Create (MKMapItemDetailSelectionAccessoryPresentationStyle presentationStyle);
	}

	[Native]
	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MKMapItemDetailSelectionAccessoryCalloutStyle : long {
		Automatic,
		Full,
		Compact,
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MKMapItemDetailSelectionAccessoryPresentationStyle {
		[Static]
		[Export ("automaticWithPresentationViewController:")]
		MKMapItemDetailSelectionAccessoryPresentationStyle CreateAutomatic ([NullAllowed] UIViewController presentationViewController);

		[Static]
		[Export ("callout", ArgumentSemantic.Strong)]
		MKMapItemDetailSelectionAccessoryPresentationStyle Callout { get; }

		[Static]
		[Export ("calloutWithCalloutStyle:")]
		MKMapItemDetailSelectionAccessoryPresentationStyle CreateCallout (MKMapItemDetailSelectionAccessoryCalloutStyle style);

		[Static]
		[Export ("sheetPresentedFromViewController:")]
		MKMapItemDetailSelectionAccessoryPresentationStyle CreateSheet (UIViewController viewController);

		[Static]
		[Export ("openInMaps", ArgumentSemantic.Strong)]
		MKMapItemDetailSelectionAccessoryPresentationStyle OpenInMaps { get; }
	}

	[Native]
	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MKLocalSearchRegionPriority : long {
		Default = 0,
		Required,
	}
}
