//
// Contacts bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace Contacts {

	// NSInteger -> CNContact.h
	/// <summary>Enumerates whether a contact represents an individual or an organization.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNContactType : long {
		/// <summary>The <see cref="Contacts.CNContact" /> is a person.</summary>
		Person,
		/// <summary>The <see cref="Contacts.CNContact" /> is an organization.</summary>
		Organization,
	}

	// NSInteger -> CNContact.h
	/// <summary>Enumerates the manner in which contacts should be sorted.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNContactSortOrder : long {
		/// <summary>No sorting should be applied.</summary>
		None,
		/// <summary>Contacts should be sorted by the user's default preference.</summary>
		UserDefault,
		/// <summary>Contacts should be sorted by <see cref="Contacts.CNContact.GivenName" />..</summary>
		GivenName,
		/// <summary>Contacts should be sorted by <see cref="Contacts.CNContact.FamilyName" />.</summary>
		FamilyName,
	}

	// NSInteger -> CNContactFormatter.h
	/// <summary>Enumerates whether or not a contact name should be spelled out phonetically.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNContactFormatterStyle : long {
		/// <summary>Combines the name components into a full name.</summary>
		FullName,
		/// <summary>Combines the phonetic name components into a full name.</summary>
		PhoneticFullName,
	}

	// NSInteger -> CNContactFormatter.h
	/// <summary>Enumerates how contacts should be sorted for display.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNContactDisplayNameOrder : long {
		/// <summary>Display name order by the user-specified default.</summary>
		UserDefault,
		/// <summary>Display name order by given name first.</summary>
		GivenNameFirst,
		/// <summary>Display name order by family name first.</summary>
		FamilyNameFirst,
	}

	// NSInteger -> CNContactStore.h
	/// <summary>An enumeration whose only value (<see cref="Contacts.CNEntityType.Contacts" />) is used by some methods in <see cref="Contacts.CNContactStore" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNEntityType : long {
		/// <summary>Indicates that the user can grant access to the user's own contacts.</summary>
		Contacts,
	}

	// NSInteger -> CNContactStore.h
	/// <summary>Enumerates the application's current authorization to access the <see cref="Contacts.CNContactStore" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNAuthorizationStatus : long {
		/// <summary>The user has not yet chosen whether the app may access the Contacts database.</summary>
		NotDetermined = 0,
		/// <summary>The app may not access the Contacts database, although the reason may not be the user's choice, but rather something such as parental controls.</summary>
		Restricted,
		/// <summary>The app has been denied access to the Contacts database.</summary>
		Denied,
		/// <summary>The app is authorized to access the Contacts database.</summary>
		Authorized,
		[iOS (18, 0), NoMacCatalyst, NoMac]
		Limited,
	}

	// NSInteger -> CNContainer.h
	/// <summary>Enumerates known <see cref="Contacts.CNContainer" /> types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNContainerType : long {
		/// <summary>Indicates that the container type has not been assigned.</summary>
		Unassigned = 0,
		/// <summary>The local device container.</summary>
		Local,
		/// <summary>A Microsoft Exchange folder.</summary>
		Exchange,
		/// <summary>A CardDAV server, such as iCloud.</summary>
		CardDav,
	}

	// NSInteger -> CNError.h
	/// <summary>Enumerates kinds of error encountered while working with contacts.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("CNErrorDomain")]
	public enum CNErrorCode : long {
		/// <summary>An error occurred at the transport level.</summary>
		CommunicationError = 1,
		/// <summary>An error occurred while trying to access the data.</summary>
		DataAccessError = 2,
		/// <summary>The user has denied the app access to their contacts.</summary>
		AuthorizationDenied = 100,
		/// <summary>To be added.</summary>
		NoAccessibleWritableContainers = 101,
		UnauthorizedKeys = 102,
		FeatureDisabledByUser = 103,
		FeatureNotAvailable = 104,
		/// <summary>The <see cref="Contacts.CNContact" /> does not exist.</summary>
		RecordDoesNotExist = 200,
		/// <summary>The inserted record is a duplicate.</summary>
		InsertedRecordAlreadyExists = 201,
		/// <summary>Code containment cycle error.</summary>
		ContainmentCycle = 202,
		/// <summary>Code containment scope error.</summary>
		ContainmentScope = 203,
		/// <summary>The <see cref="Contacts.CNContact" /> does not exist.</summary>
		ParentRecordDoesNotExist = 204,
		/// <summary>To be added.</summary>
		RecordIdentifierInvalid = 205,
		RecordNotWritable = 206,
		ParentContainerNotWritable = 207,
		/// <summary>Multiple errors occurred relating to validation.</summary>
		ValidationMultipleErrors = 300,
		/// <summary>There was a type mismatch during validation.</summary>
		ValidationTypeMismatch = 301,
		/// <summary>The validation attempt was misconfigured.</summary>
		ValidationConfigurationError = 302,
		/// <summary>The <see cref="Contacts.CNPredicate" /> is invalid.</summary>
		PredicateInvalid = 400,
		/// <summary>A policy validation error.</summary>
		PolicyViolation = 500,
		/// <summary>To be added.</summary>
		ClientIdentifierInvalid = 600,
		/// <summary>To be added.</summary>
		ClientIdentifierDoesNotExist = 601,
		ClientIdentifierCollision = 602,
		ChangeHistoryExpired = 603,
		ChangeHistoryInvalidAnchor = 604,
		ChangeHistoryInvalidFetchRequest = 605,
		/// <summary>To be added.</summary>
		VCardMalformed = 700,
		/// <summary>To be added.</summary>
		VCardSummarizationError = 701,
	}

	// NSInteger -> CNPostalAddressFormatter.h
	/// <summary>Enumerates postal address formatter styles.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CNPostalAddressFormatterStyle : long {
		/// <summary>Indicates that the address should be formatted as a mailing address.</summary>
		MailingAddress,
	}

	/// <summary>Flagging enumeration that specifies keys that can be checked with <see cref="Contacts.CNContact.IsKeyAvailable(Foundation.NSString)" /> and <see cref="Contacts.CNContact.AreKeysAvailable{T}(T[])" />.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	public enum CNContactOptions : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Nickname = 1 << 0,
		/// <summary>To be added.</summary>
		PhoneticGivenName = 1 << 1,
		/// <summary>To be added.</summary>
		PhoneticMiddleName = 1 << 2,
		/// <summary>To be added.</summary>
		PhoneticFamilyName = 1 << 3,
		/// <summary>To be added.</summary>
		OrganizationName = 1 << 4,
		/// <summary>To be added.</summary>
		DepartmentName = 1 << 5,
		/// <summary>To be added.</summary>
		JobTitle = 1 << 6,
		/// <summary>To be added.</summary>
		Birthday = 1 << 7,
		/// <summary>To be added.</summary>
		NonGregorianBirthday = 1 << 8,
		/// <summary>To be added.</summary>
		Note = 1 << 9,
#if !MONOMAC
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		ImageData = 1 << 10,
#endif
		/// <summary>To be added.</summary>
		ThumbnailImageData = 1 << 11,
#if !MONOMAC
		/// <summary>To be added.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		ImageDataAvailable = 1 << 12,
#endif
		/// <summary>To be added.</summary>
		Type = 1 << 13,
		/// <summary>To be added.</summary>
		PhoneNumbers = 1 << 14,
		/// <summary>To be added.</summary>
		EmailAddresses = 1 << 15,
		/// <summary>To be added.</summary>
		PostalAddresses = 1 << 16,
		/// <summary>To be added.</summary>
		Dates = 1 << 17,
		/// <summary>To be added.</summary>
		UrlAddresses = 1 << 18,
		/// <summary>To be added.</summary>
		Relations = 1 << 19,
		/// <summary>To be added.</summary>
		SocialProfiles = 1 << 20,
		/// <summary>To be added.</summary>
		InstantMessageAddresses = 1 << 21,
	}
}
