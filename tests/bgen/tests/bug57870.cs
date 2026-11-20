using System;
using Foundation;

namespace Bug57870 {

	enum PersonRelationship {
		[Field (null)]
		None,

		[Field ("INPersonRelationshipFather", "__Internal")]
		Father,

		[Field ("INPersonRelationshipMother", "__Internal")]
		Mother
	}

	[BaseType (typeof (NSObject))]
	interface Wrappers {

		// SmartEnum -- Normal Wrap getter Property

		[Export ("presenceType")]
		NSString _PresenceType { get; }

		[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType)")]
		PersonRelationship PresenceType { get; }

		// SmartEnum -- getter Wrap + NotImplemented setter

		[Export ("presenceType2")]
		NSString _PresenceType2 { get; [NotImplemented] set; }

		PersonRelationship PresenceType2 {
			[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType2)")]
			get;
			[NotImplemented ("Nope nope nope")]
			set;
		}

		// SmartEnum -- getter Wrap Only

		[Export ("presenceType3")]
		NSString _PresenceType3 { get; }

		PersonRelationship PresenceType3 {
			[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType3)")]
			get;
		}

		// SmartEnum -- Wrap getter and setter

		[Export ("presenceType4")]
		NSString _PresenceType4 { get; set; }

		PersonRelationship PresenceType4 {
			[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType4)")]
			get;
			[Wrap ("_PresenceType4 = value.GetConstant ()")]
			set;
		}

		[Export ("presenceType5")]
		NSString _PresenceType5 { get; [NotImplemented ("Custom read-only message")] set; }

		PersonRelationship PresenceType5 {
			[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType5)")]
			get;
			[NotImplemented]
			set;
		}

		[Export ("presenceType6")]
		NSString _PresenceType6 { get; [NotImplemented ("Use \"mutable\" instead.\nPath: C:\\bindings")] set; }

		PersonRelationship PresenceType6 {
			[Wrap ("PersonRelationshipExtensions.GetValue (_PresenceType6)")]
			get;
			[NotImplemented ("Use \"mutable\" instead.\nPath: C:\\bindings")]
			set;
		}
	}
}
