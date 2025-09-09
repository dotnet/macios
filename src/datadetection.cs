// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using ObjCRuntime;

namespace DataDetection {

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface DDMatch {
		[Export ("matchedString")]
		string MatchedString { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchLink {
		[Export ("URL")]
		NSUrl Url { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchPhoneNumber {
		[Export ("phoneNumber")]
		string PhoneNumber { get; }

		[NullAllowed, Export ("label")]
		string Label { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchEmailAddress {
		[Export ("emailAddress")]
		string EmailAddress { get; }

		[NullAllowed, Export ("label")]
		string Label { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchPostalAddress {
		[NullAllowed, Export ("street")]
		string Street { get; }

		[NullAllowed, Export ("city")]
		string City { get; }

		[NullAllowed, Export ("state")]
		string State { get; }

		[NullAllowed, Export ("postalCode")]
		string PostalCode { get; }

		[NullAllowed, Export ("country")]
		string Country { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchCalendarEvent {
		[Export ("allDay")]
		bool AllDay { [Bind ("isAllDay")] get; }

		[NullAllowed, Export ("startDate")]
		NSDate StartDate { get; }

		[NullAllowed, Export ("startTimeZone")]
		NSTimeZone StartTimeZone { get; }

		[NullAllowed, Export ("endDate")]
		NSDate EndDate { get; }

		[NullAllowed, Export ("endTimeZone")]
		NSTimeZone EndTimeZone { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchShipmentTrackingNumber {
		[Export ("carrier")]
		string Carrier { get; }

		[Export ("trackingNumber")]
		string TrackingNumber { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchFlightNumber {
		[Export ("airline")]
		string Airline { get; }

		[Export ("flightNumber")]
		string FlightNumber { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0), Mac (12, 0)]
	[BaseType (typeof (DDMatch))]
	interface DDMatchMoneyAmount {
		[Export ("currency")]
		string Currency { get; }

		[Export ("amount")]
		double Amount { get; }
	}
}
