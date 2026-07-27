
namespace AddressBook {

	[NoMac, NoTV]
	[Partial]
	interface ABAddressBook {
#if !XAMCORE_5_0
		[Internal]
		[Field ("ABAddressBookErrorDomain")]
		NSString _ErrorDomain { get; }
#endif
#if XAMCORE_5_0
		/// <summary>Identifies the error domain under which Address Book errors are grouped.</summary>
		/// <remarks>
		///   <para>
		///     When an <see cref="CoreFoundation.CFException" /> is thrown from an <see cref="ABAddressBook" /> method,
		///     its <see cref="CoreFoundation.CFException.Domain" /> is equal to this value.
		///   </para>
		/// </remarks>
		[Field ("ABAddressBookErrorDomain")]
		NSString ErrorDomain { get; }
#endif
	}

	[NoMac, NoTV]
	[Internal]
	[Static, Partial]
	interface ABGroupProperty {
		[Field ("kABGroupNameProperty")]
		int Name { get; }
	}

	[NoMac, NoTV]
	[Internal]
	[Static, Partial]
	interface ABPersonPropertyId {
		[Field ("kABPersonAddressProperty")]
		int Address { get; }

		[Field ("kABPersonBirthdayProperty")]
		int Birthday { get; }

		[Field ("kABPersonCreationDateProperty")]
		int CreationDate { get; }

		[Field ("kABPersonDateProperty")]
		int Date { get; }

		[Field ("kABPersonDepartmentProperty")]
		int Department { get; }

		[Field ("kABPersonEmailProperty")]
		int Email { get; }

		[Field ("kABPersonFirstNameProperty")]
		int FirstName { get; }

		[Field ("kABPersonFirstNamePhoneticProperty")]
		int FirstNamePhonetic { get; }

		[Field ("kABPersonInstantMessageProperty")]
		int InstantMessage { get; }

		[Field ("kABPersonJobTitleProperty")]
		int JobTitle { get; }

		[Field ("kABPersonKindProperty")]
		int Kind { get; }

		[Field ("kABPersonLastNameProperty")]
		int LastName { get; }

		[Field ("kABPersonLastNamePhoneticProperty")]
		int LastNamePhonetic { get; }

		[Field ("kABPersonMiddleNameProperty")]
		int MiddleName { get; }

		[Field ("kABPersonMiddleNamePhoneticProperty")]
		int MiddleNamePhonetic { get; }

		[Field ("kABPersonModificationDateProperty")]
		int ModificationDate { get; }

		[Field ("kABPersonNicknameProperty")]
		int Nickname { get; }

		[Field ("kABPersonNoteProperty")]
		int Note { get; }

		[Field ("kABPersonOrganizationProperty")]
		int Organization { get; }

		[Field ("kABPersonPhoneProperty")]
		int Phone { get; }

		[Field ("kABPersonPrefixProperty")]
		int Prefix { get; }

		[Field ("kABPersonRelatedNamesProperty")]
		int RelatedNames { get; }

		[Field ("kABPersonSuffixProperty")]
		int Suffix { get; }

		[Field ("kABPersonURLProperty")]
		int Url { get; }

		[Field ("kABPersonSocialProfileProperty")]
		int SocialProfile { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonAddressKey {
		/// <summary>Gets the key for the city component of an address.</summary>
		[Field ("kABPersonAddressCityKey")]
		NSString City { get; }

		/// <summary>Gets the key for the country component of an address.</summary>
		[Field ("kABPersonAddressCountryKey")]
		NSString Country { get; }

		/// <summary>Gets the key for the country-code component of an address.</summary>
		[Field ("kABPersonAddressCountryCodeKey")]
		NSString CountryCode { get; }

		/// <summary>Gets the key for the state or province component of an address.</summary>
		[Field ("kABPersonAddressStateKey")]
		NSString State { get; }

		/// <summary>Gets the key for the street component of an address.</summary>
		[Field ("kABPersonAddressStreetKey")]
		NSString Street { get; }

		/// <summary>Gets the key for the postal-code component of an address.</summary>
		[Field ("kABPersonAddressZIPKey")]
		NSString Zip { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonDateLabel {
		/// <summary>Gets the label used for anniversary dates.</summary>
		[Field ("kABPersonAnniversaryLabel")]
		NSString Anniversary { get; }
	}

	[NoMac, NoTV]
	[Internal]
	[Static, Partial]
	interface ABPersonKindId {
		[Field ("kABPersonKindOrganization")]
		NSNumber Organization { get; }

		[Field ("kABPersonKindPerson")]
		NSNumber Person { get; }
	}

	[NoMac, NoTV]
	[Internal]
	[Static, Partial]
	interface ABPersonSocialProfile {
		[Field ("kABPersonSocialProfileURLKey")]
		NSString URLKey { get; }

		[Field ("kABPersonSocialProfileServiceKey")]
		NSString ServiceKey { get; }

		[Field ("kABPersonSocialProfileUsernameKey")]
		NSString UsernameKey { get; }

		[Field ("kABPersonSocialProfileUserIdentifierKey")]
		NSString UserIdentifierKey { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonSocialProfileService {
#if !XAMCORE_5_0
		[Internal]
		[Field ("kABPersonSocialProfileServiceTwitter")]
		NSString _Twitter { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceGameCenter")]
		NSString _GameCenter { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceFacebook")]
		NSString _Facebook { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceMyspace")]
		NSString _Myspace { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceLinkedIn")]
		NSString _LinkedIn { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceFlickr")]
		NSString _Flickr { get; }

		[Internal]
		[Field ("kABPersonSocialProfileServiceSinaWeibo")]
		NSString _SinaWeibo { get; }
#endif
#if XAMCORE_5_0
		/// <summary>Identifies the Twitter social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceTwitter")]
		NSString Twitter { get; }

		/// <summary>Identifies the Game Center social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceGameCenter")]
		NSString GameCenter { get; }

		/// <summary>Identifies the Facebook social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceFacebook")]
		NSString Facebook { get; }

		/// <summary>Identifies the Myspace social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceMyspace")]
		NSString Myspace { get; }

		/// <summary>Identifies the LinkedIn social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceLinkedIn")]
		NSString LinkedIn { get; }

		/// <summary>Identifies the Flickr social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceFlickr")]
		NSString Flickr { get; }

		/// <summary>Identifies the Sina Weibo social-profile service.</summary>
		[Field ("kABPersonSocialProfileServiceSinaWeibo")]
		NSString SinaWeibo { get; }
#endif
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonPhoneLabel {
		/// <summary>Gets the label used for a home fax number.</summary>
		[Field ("kABPersonPhoneHomeFAXLabel")]
		NSString HomeFax { get; }

		/// <summary>Gets the label used for an iPhone number.</summary>
		[Field ("kABPersonPhoneIPhoneLabel")]
		NSString iPhone { get; }

		/// <summary>Gets the label used for a main phone number.</summary>
		[Field ("kABPersonPhoneMainLabel")]
		NSString Main { get; }

		/// <summary>Gets the label used for a mobile phone number.</summary>
		[Field ("kABPersonPhoneMobileLabel")]
		NSString Mobile { get; }

		/// <summary>Gets the label used for a pager number.</summary>
		[Field ("kABPersonPhonePagerLabel")]
		NSString Pager { get; }

		/// <summary>Gets the label used for a work fax number.</summary>
		[Field ("kABPersonPhoneWorkFAXLabel")]
		NSString WorkFax { get; }

		/// <summary>Gets the label used for another fax number.</summary>
		[Field ("kABPersonPhoneOtherFAXLabel")]
		NSString OtherFax { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonInstantMessageService {
		/// <summary>Gets the AIM instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceAIM")]
		NSString Aim { get; }

		/// <summary>Gets the ICQ instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceICQ")]
		NSString Icq { get; }

		/// <summary>Gets the Jabber instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceJabber")]
		NSString Jabber { get; }

		/// <summary>Gets the MSN instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceMSN")]
		NSString Msn { get; }

		/// <summary>Gets the Yahoo instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceYahoo")]
		NSString Yahoo { get; }

		/// <summary>Gets the QQ instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceQQ")]
		NSString QQ { get; }

		/// <summary>Gets the Google Talk instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceGoogleTalk")]
		NSString GoogleTalk { get; }

		/// <summary>Gets the Skype instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceSkype")]
		NSString Skype { get; }

		/// <summary>Gets the Facebook instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceFacebook")]
		NSString Facebook { get; }

		/// <summary>Gets the Gadu-Gadu instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceGaduGadu")]
		NSString GaduGadu { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonInstantMessageKey {
		/// <summary>Gets the key for an instant-messaging service identifier.</summary>
		[Field ("kABPersonInstantMessageServiceKey")]
		NSString Service { get; }

		/// <summary>Gets the key for an instant-messaging user name.</summary>
		[Field ("kABPersonInstantMessageUsernameKey")]
		NSString Username { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonUrlLabel {
		/// <summary>Gets the label used for a home-page URL.</summary>
		[Field ("kABPersonHomePageLabel")]
		NSString HomePage { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABPersonRelatedNamesLabel {
		/// <summary>Gets the label used for an assistant.</summary>
		[Field ("kABPersonAssistantLabel")]
		NSString Assistant { get; }

		/// <summary>Gets the label used for a brother.</summary>
		[Field ("kABPersonBrotherLabel")]
		NSString Brother { get; }

		/// <summary>Gets the label used for a child.</summary>
		[Field ("kABPersonChildLabel")]
		NSString Child { get; }

		/// <summary>Gets the label used for a father.</summary>
		[Field ("kABPersonFatherLabel")]
		NSString Father { get; }

		/// <summary>Gets the label used for a friend.</summary>
		[Field ("kABPersonFriendLabel")]
		NSString Friend { get; }

		/// <summary>Gets the label used for a manager.</summary>
		[Field ("kABPersonManagerLabel")]
		NSString Manager { get; }

		/// <summary>Gets the label used for a mother.</summary>
		[Field ("kABPersonMotherLabel")]
		NSString Mother { get; }

		/// <summary>Gets the label used for a parent.</summary>
		[Field ("kABPersonParentLabel")]
		NSString Parent { get; }

		/// <summary>Gets the label used for a partner.</summary>
		[Field ("kABPersonPartnerLabel")]
		NSString Partner { get; }

		/// <summary>Gets the label used for a sister.</summary>
		[Field ("kABPersonSisterLabel")]
		NSString Sister { get; }

		/// <summary>Gets the label used for a spouse.</summary>
		[Field ("kABPersonSpouseLabel")]
		NSString Spouse { get; }
	}

	[NoMac, NoTV]
	[Static, Partial]
	interface ABLabel {
		/// <summary>Gets the generic home label.</summary>
		[Field ("kABHomeLabel")]
		NSString Home { get; }

		/// <summary>Gets the generic other label.</summary>
		[Field ("kABOtherLabel")]
		NSString Other { get; }

		/// <summary>Gets the generic work label.</summary>
		[Field ("kABWorkLabel")]
		NSString Work { get; }
	}

	[NoMac, NoTV]
	[Internal]
	[Static, Partial]
	interface ABSourcePropertyId {
		[Field ("kABSourceNameProperty")]
		int Name { get; }

		[Field ("kABSourceTypeProperty")]
		int Type { get; }
	}
}
