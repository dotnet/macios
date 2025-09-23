//
// MapKit enumerations
//
// Author:
//   Miguel de Icaza
//
// Copyright 2009 Novell, Inc.
// Copyright 2014-2016 Xamarin Inc.
//

using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using CoreLocation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	// NSUInteger -> MKDirectionsTypes.h
	/// <summary>An enumeration whose values specify the routing type for directions requests.</summary>
	/// <remarks>
	///       <para>The <see cref="MapKit.MKDirectionsTransportType" /> used as the <see cref="MapKit.MKDirectionsRequest.TransportType" /> property of a <see cref="MapKit.MKDirectionsRequest" /> must match the values specified in the application's <c>info.plist</c> (see <see cref="MapKit.MKDirections" />).</para>
	///     </remarks>
	[Native]
	[MacCatalyst (13, 1)]
	public enum MKDirectionsTransportType : ulong {
		/// <summary>Routing for automobiles.</summary>
		Automobile = 1 << 0,
		/// <summary>Routing for walking.</summary>
		Walking = 1 << 1,
		/// <summary>Routing for public transport.</summary>
		Transit = 1 << 2,
		/// <summary>Routing for cycling.</summary>
		Cycling = 1 << 3,
		/// <summary>The routing type is not specified.</summary>
		Any = 0x0FFFFFFF,
	}

	// NSUInteger -> MKTypes.h
	/// <summary>The type of map.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKMapType : ulong {
		/// <summary>Standard cartographic imagery.</summary>
		Standard = 0,
		/// <summary>Photos taken from space.</summary>
		Satellite,
		/// <summary>A combination of satellite and cartographic imagery.</summary>
		Hybrid,
		/// <summary>A flyover using satellite images.</summary>
		SatelliteFlyover,
		/// <summary>A flyover that combines satellite and cartographic imagery.</summary>
		HybridFlyover,
		/// <summary>A muted map that emphasized developer data.</summary>
		[MacCatalyst (13, 1)]
		MutedStandard,
	}

	// NSUInteger -> MKDistanceFormatter.h
	/// <summary>An enumeration whose values specify the units used with <see cref="MapKit.MKDistanceFormatter" />.</summary>
	/// <remarks>To be added.</remarks>
	[Native]
	[MacCatalyst (13, 1)]
	public enum MKDistanceFormatterUnits : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Metric,
		/// <summary>To be added.</summary>
		Imperial,
		/// <summary>To be added.</summary>
		ImperialWithYards,
	}

	// NSUInteger -> MKDistanceFormatter.h
	/// <summary>An enumeration whose values specify the length of a <see cref="MapKit.MKDistanceFormatter" /> string.</summary>
	/// <remarks>To be added.</remarks>
	[Native]
	[MacCatalyst (13, 1)]
	public enum MKDistanceFormatterUnitStyle : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Abbreviated,
		/// <summary>To be added.</summary>
		Full,
	}

	// NSInteger -> MKMapView.h
	/// <summary>An enumeration whose value specify whether the overlay should render above roads, but beneath labels, etc..</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKOverlayLevel : long {
		/// <summary>The overlay should render above roads, but below labels, points of interest, and annotation views.</summary>
		AboveRoads = 0,
		/// <summary>The overlay should render above all map content except for annotation views.</summary>
		AboveLabels,
	}

	// NSUInteger -> MKTypes.h
	/// <summary>An enumeration whose values represent various errors that can occur with <see cref="MapKit.MKDirections.CalculateDirections" /> and <see cref="MapKit.MKDirections.CalculateETA(MapKit.MKETAHandler)" />.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("MKErrorDomain")]
	public enum MKErrorCode : ulong {
		/// <summary>To be added.</summary>
		Unknown = 1,
		/// <summary>To be added.</summary>
		ServerFailure,
		/// <summary>To be added.</summary>
		LoadingThrottled,
		/// <summary>To be added.</summary>
		PlacemarkNotFound,
		/// <summary>To be added.</summary>
		DirectionsNotFound,
		DecodingFailed,
	}

	// NSUInteger -> MKTypes.h
	/// <summary>An enumeration of valid states for a dragged <see cref="MapKit.MKAnnotationView" />.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKAnnotationViewDragState : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Starting,
		/// <summary>To be added.</summary>
		Dragging,
		/// <summary>To be added.</summary>
		Canceling,
		/// <summary>To be added.</summary>
		Ending,
	}

	// NSUInteger -> MKTypes.h
	/// <summary>Color for map pins.</summary>
	[NoTV]
	[Native]
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'MKPinAnnotationView.PinTintColor' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKPinAnnotationView.PinTintColor' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'MKPinAnnotationView.PinTintColor' instead.")]
	public enum MKPinAnnotationColor : ulong {
		/// <summary>To be added.</summary>
		Red,
		/// <summary>To be added.</summary>
		Green,
		/// <summary>To be added.</summary>
		Purple,
	}

	// NSUInteger -> MKTypes.h
	/// <summary>An enumeration of valid tracking modes.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKUserTrackingMode : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Follow,
#if !XAMCORE_5_0 && !(IOS || MACCATALYST)
		[Obsolete ("This is only available on iOS and MacCatalyst.")]
		FollowWithHeading,
#elif IOS || MACCATALYST
		/// <summary>To be added.</summary>
		FollowWithHeading,
#endif
	}

	/// <summary>Enumerates values that control whether search queries, in addition to place results, are included in completion lists.</summary>
	/// <remarks>To be added.</remarks>
	[Native]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'MKLocalSearchCompleterResultType' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'MKLocalSearchCompleterResultType' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'MKLocalSearchCompleterResultType' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MKLocalSearchCompleterResultType' instead.")]
	public enum MKSearchCompletionFilterType : long {
		/// <summary>Include related search queries in the completion results, in addition to place names.</summary>
		AndQueries = 0,
		/// <summary>Only include place names in the completion results.</summary>
		Only,
	}

	/// <summary>Enumerates collision detection modes.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKAnnotationViewCollisionMode : long {
		/// <summary>Indicates that the entire frame rectangle should be used for collision detection.</summary>
		Rectangle,
		/// <summary>Indicates that a circle inscribed within the frame rectangle should be used for collision detection.</summary>
		Circle,
		[TV (14, 0)]
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		None,
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKScaleViewAlignment : long {
		/// <summary>To be added.</summary>
		Leading,
		/// <summary>To be added.</summary>
		Trailing,
		[TV (26, 0), NoMac, iOS (26, 0), MacCatalyst (26, 0)]
		Center,
	}

	/// <summary>Enumerates visibility behavior for marker titles.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKFeatureVisibility : long {
		/// <summary>Indicates that the title text is shown or hidden in response to the map state.</summary>
		Adaptive,
		/// <summary>Indicates that the title text is hidden.</summary>
		Hidden,
		/// <summary>Indicates that the title text is always visible.</summary>
		Visible,
	}

	[Flags]
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKLocalSearchCompleterResultType : ulong {
		Address = 1 << 0,
		PointOfInterest = 1 << 1,
		Query = 1 << 2,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		PhysicalFeature = 1 << 3,
	}

	[Flags]
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MKLocalSearchResultType : ulong {
		Address = 1 << 0,
		PointOfInterest = 1 << 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		PhysicalFeature = 1 << 2,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MKDirectionsRoutePreference : long {
		Any = 0,
		Avoid,
	}

	[Flags]
	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum MKMapFeatureOptions : long {
		PointsOfInterest = 1 << (int) MKMapFeatureType.PointOfInterest,
		Territories = 1 << (int) MKMapFeatureType.Territory,
		PhysicalFeatures = 1 << (int) MKMapFeatureType.PhysicalFeature,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum MKLookAroundBadgePosition : long {
		TopLeading = 0,
		TopTrailing,
		BottomTrailing,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MKMapElevationStyle : long {
		Flat = 0,
		Realistic,
	}

	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum MKMapFeatureType : long {
		PointOfInterest = 0,
		Territory,
		PhysicalFeature,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MKStandardMapEmphasisStyle : long {
		Default = 0,
		Muted,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum MKPointOfInterestCategory {

		[Field ("MKPointOfInterestCategoryAirport")]
		Airport,

		[Field ("MKPointOfInterestCategoryAmusementPark")]
		AmusementPark,

		[Field ("MKPointOfInterestCategoryAquarium")]
		Aquarium,

		[Field ("MKPointOfInterestCategoryATM")]
		Atm,

		[Field ("MKPointOfInterestCategoryBakery")]
		Bakery,

		[Field ("MKPointOfInterestCategoryBank")]
		Bank,

		[Field ("MKPointOfInterestCategoryBeach")]
		Beach,

		[Field ("MKPointOfInterestCategoryBrewery")]
		Brewery,

		[Field ("MKPointOfInterestCategoryCafe")]
		Cafe,

		[Field ("MKPointOfInterestCategoryCampground")]
		Campground,

		[Field ("MKPointOfInterestCategoryCarRental")]
		CarRental,

		[Field ("MKPointOfInterestCategoryEVCharger")]
		EVCharger,

		[Field ("MKPointOfInterestCategoryFireStation")]
		FireStation,

		[Field ("MKPointOfInterestCategoryFitnessCenter")]
		FitnessCenter,

		[Field ("MKPointOfInterestCategoryFoodMarket")]
		FoodMarket,

		[Field ("MKPointOfInterestCategoryGasStation")]
		GasStation,

		[Field ("MKPointOfInterestCategoryHospital")]
		Hospital,

		[Field ("MKPointOfInterestCategoryHotel")]
		Hotel,

		[Field ("MKPointOfInterestCategoryLaundry")]
		Laundry,

		[Field ("MKPointOfInterestCategoryLibrary")]
		Library,

		[Field ("MKPointOfInterestCategoryMarina")]
		Marina,

		[Field ("MKPointOfInterestCategoryMovieTheater")]
		MovieTheater,

		[Field ("MKPointOfInterestCategoryMuseum")]
		Museum,

		[Field ("MKPointOfInterestCategoryNationalPark")]
		NationalPark,

		[Field ("MKPointOfInterestCategoryNightlife")]
		Nightlife,

		[Field ("MKPointOfInterestCategoryPark")]
		Park,

		[Field ("MKPointOfInterestCategoryParking")]
		Parking,

		[Field ("MKPointOfInterestCategoryPharmacy")]
		Pharmacy,

		[Field ("MKPointOfInterestCategoryPolice")]
		Police,

		[Field ("MKPointOfInterestCategoryPostOffice")]
		PostOffice,

		[Field ("MKPointOfInterestCategoryPublicTransport")]
		PublicTransport,

		[Field ("MKPointOfInterestCategoryRestaurant")]
		Restaurant,

		[Field ("MKPointOfInterestCategoryRestroom")]
		Restroom,

		[Field ("MKPointOfInterestCategorySchool")]
		School,

		[Field ("MKPointOfInterestCategoryStadium")]
		Stadium,

		[Field ("MKPointOfInterestCategoryStore")]
		Store,

		[Field ("MKPointOfInterestCategoryTheater")]
		Theater,

		[Field ("MKPointOfInterestCategoryUniversity")]
		University,

		[Field ("MKPointOfInterestCategoryWinery")]
		Winery,

		[Field ("MKPointOfInterestCategoryZoo")]
		Zoo,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryAnimalService")]
		AnimalService,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryAutomotiveRepair")]
		AutomotiveRepair,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryBaseball")]
		Baseball,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryBasketball")]
		Basketball,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryBeauty")]
		Beauty,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryBowling")]
		Bowling,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryCastle")]
		Castle,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryConventionCenter")]
		ConventionCenter,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryDistillery")]
		Distillery,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryFairground")]
		Fairground,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryFishing")]
		Fishing,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryFortress")]
		Fortress,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryGolf")]
		Golf,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryGoKart")]
		GoKart,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryHiking")]
		Hiking,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryKayaking")]
		Kayaking,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryLandmark")]
		Landmark,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryMailbox")]
		Mailbox,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryMiniGolf")]
		MiniGolf,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryMusicVenue")]
		MusicVenue,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryNationalMonument")]
		NationalMonument,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryPlanetarium")]
		Planetarium,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryRockClimbing")]
		RockClimbing,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryRVPark")]
		RVPark,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySkatePark")]
		SkatePark,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySkating")]
		Skating,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySkiing")]
		Skiing,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySoccer")]
		Soccer,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySpa")]
		Spa,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySurfing")]
		Surfing,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategorySwimming")]
		Swimming,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryTennis")]
		Tennis,

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MKPointOfInterestCategoryVolleyball")]
		Volleyball,
	}
}
