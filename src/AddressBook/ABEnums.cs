// 
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2011-2013, 2016 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//

#if !MONOMAC

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace AddressBook {
	/// <summary>Possible <see cref="T:AddressBook.ABAddressBook" /> errors.</summary>
	///     <remarks>
	///       When the <see cref="P:CoreFoundation.CFException.Domain" />
	///       property is set to
	///       <see cref="F:AddressBook.ABAddressBook.ErrorDomain" />,
	///       then <see cref="P:CoreFoundation.CFException.Code" />
	///       will be one of the
	///       <see cref="T:AddressBook.ABAddressBookError" /> values.
	///     </remarks>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use the 'Contacts' API instead.")]
	[ErrorDomain ("ABAddressBookErrorDomain")]
#if NET
	public enum ABAddressBookError {
#else
	[Native]
	public enum ABAddressBookError : long {
#endif
		/// <summary>The operation is not permitted.</summary>
		OperationNotPermittedByStore = 0,
		/// <summary>To be added.</summary>
		OperationNotPermittedByUserError,
	}

	/// <summary>An enumeration whose values specify the possible results of the <see cref="M:AddressBook.ABAddressBook.GetAuthorizationStatus" /> method.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use the 'Contacts' API instead.")]
	[Native]
	public enum ABAuthorizationStatus : long {
		/// <summary>The user has not made a decision regarding access.</summary>
		NotDetermined = 0,
		/// <summary>Access is denied and the user is not allowed to change permission.</summary>
		Restricted,
		/// <summary>The user has denied authorization to access address book data.</summary>
		Denied,
		/// <summary>The app is authorized to access address book data.</summary>
		Authorized,
	}

	/// <summary>How to sort records.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABPersonSortBy : uint /* uint32_t */ {
		/// <summary>Order by first name.</summary>
		FirstName = 0,
		/// <summary>Order by last name.</summary>
		LastName = 1,
	}

	/// <summary>
	///       The format to use for a person's composite name.
	///     </summary>
	///     <remarks>
	///       <para>
	///         The composite name controls the output of
	///         <see cref="M:AddressBook.ABRecord.ToString" />.
	///       </para>
	///     </remarks>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABPersonCompositeNameFormat : uint /* uint32_t */ {
		/// <summary>First name first.</summary>
		FirstNameFirst = 0,
		/// <summary>Last name first.</summary>
		LastNameFirst = 1,
	}

	/// <summary>
	///       The <see cref="T:AddressBook.ABPerson" />
	///       properties.
	///     </summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABPersonProperty {
		/// <summary>
		///           The
		///           <see cref="M:AddressBook.ABPerson.GetVCards(AddressBook.ABPerson[])" />
		///           multi-value property.
		///         </summary>
		Address,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Birthday" /> property.
		///         </summary>
		Birthday,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.CreationDate" /> property.
		///         </summary>
		CreationDate,
		/// <summary>
		///           The
		///           <see cref="M:AddressBook.ABPerson.GetDates" />
		///           multi-value property.
		///         </summary>
		Date,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Department" /> property.
		///         </summary>
		Department,
		/// <summary>
		///           The
		///           <see cref="M:AddressBook.ABPerson.GetEmails" />
		///           multi-value property.
		///         </summary>
		Email,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.FirstName" /> property.
		///         </summary>
		FirstName,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.FirstNamePhonetic" /> property.
		///         </summary>
		FirstNamePhonetic,
		/// <summary>
		///           The
		///           <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Address%20Book%20ABPerson%20Get%20Instant%20Messages&amp;scope=Xamarin" title="M:AddressBook.ABPerson.GetInstantMessages*">M:AddressBook.ABPerson.GetInstantMessages*</a></format>
		///           multi-value property.
		///         </summary>
		InstantMessage,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.JobTitle" /> property.
		///         </summary>
		JobTitle,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.PersonKind" /> property.
		///         </summary>
		Kind,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.LastName" /> property.
		///         </summary>
		LastName,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.LastNamePhonetic" /> property.
		///         </summary>
		LastNamePhonetic,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.MiddleName" /> property.
		///         </summary>
		MiddleName,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.MiddleNamePhonetic" /> property.
		///         </summary>
		MiddleNamePhonetic,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.ModificationDate" /> property.
		///         </summary>
		ModificationDate,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Nickname" /> property.
		///         </summary>
		Nickname,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Note" /> property.
		///         </summary>
		Note,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Organization" /> property.
		///         </summary>
		Organization,
		/// <summary>
		///           The
		///           <see cref="M:AddressBook.ABPerson.GetPhones" />
		///           multi-value property.
		///         </summary>
		Phone,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Prefix" /> property.
		///         </summary>
		Prefix,
		/// <summary>
		///           The
		///           <see cref="P:AddressBookUI.ABPersonPredicateKey.RelatedNames" />
		///           multi-value property.
		///         </summary>
		RelatedNames,
		/// <summary>
		///           The <see cref="P:AddressBook.ABPerson.Suffix" /> property.
		///         </summary>
		Suffix,
		/// <summary>
		///           The
		///           <see cref="M:AddressBook.ABPerson.GetUrls" />
		///           multi-value property.
		///         </summary>
		Url,
		/// <summary>To be added.</summary>
		SocialProfile,
	}

	/// <summary>An enumeration whose values specify whether the form of the image requested from the <see cref="M:AddressBook.ABPerson.GetImage(AddressBook.ABPersonImageFormat)" /> method.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
#if NET
	public enum ABPersonImageFormat {
#else
	[Native]
	public enum ABPersonImageFormat : long {
#endif
		/// <summary>To be added.</summary>
		Thumbnail = 0,
		/// <summary>To be added.</summary>
		OriginalSize = 2,
	}

	/// <summary>
	///       Specifies whether a <see cref="T:AddressBook.ABPerson" />
	///       represents a human being or an organization.
	///     </summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABPersonKind {
		/// <summary>
		///           It's unknown whether the
		///           <see cref="T:AddressBook.ABPerson" /> is a person or
		///           an organization.
		///         </summary>
		None,
		/// <summary>
		///           The <see cref="T:AddressBook.ABPerson" /> is an organization.
		///         </summary>
		Organization,
		/// <summary>
		///           The <see cref="T:AddressBook.ABPerson" /> is a person.
		///         </summary>
		Person,
	}

	/// <summary>Potential record types.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABRecordType : uint /* uint32_t */ {
		/// <summary>A <see cref="T:AddressBook.ABPerson" /> record.</summary>
		Person = 0,
		/// <summary>A <see cref="T:AddressBook.ABGroup" /> record.</summary>
		Group = 1,
		/// <summary>To be added.</summary>
		Source = 2,
	}

	/// <summary>Record property types.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABPropertyType : uint /* uint32_t */ {
		/// <summary>Invalid property type.</summary>
		Invalid = 0,
		/// <summary>
		///           The property holds a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> value.
		///         </summary>
		String = 0x1,
		/// <summary>
		///           The property holds a <see cref="T:Foundation.NSNumber" /> value.
		///         </summary>
		Integer = 0x2,
		/// <summary>
		///           The property holds a <see cref="T:Foundation.NSNumber" /> value.
		///         </summary>
		Real = 0x3,
		/// <summary>
		///           The property holds a <see cref="T:Foundation.NSDate" /> value.
		///         </summary>
		DateTime = 0x4,
		/// <summary>
		///           The property holds a <see cref="T:Foundation.NSDictionary" /> value.
		///         </summary>
		Dictionary = 0x5,
		/// <summary>
		///           The property holds a
		///           <see cref="T:AddressBook.ABMultiValue`1" />
		///           value.
		///         </summary>
		MultiString = MultiMask | String,
		/// <summary>
		///           The property holds a
		///           <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:AddressBook.ABMultiValue{Foundation.NSNumber}&amp;scope=Xamarin" title="T:AddressBook.ABMultiValue{Foundation.NSNumber}">T:AddressBook.ABMultiValue{Foundation.NSNumber}</a></format>
		///           value.
		///         </summary>
		MultiInteger = MultiMask | Integer,
		/// <summary>
		///           The property holds a
		///           <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:AddressBook.ABMultiValue{Foundation.NSNumber}&amp;scope=Xamarin" title="T:AddressBook.ABMultiValue{Foundation.NSNumber}">T:AddressBook.ABMultiValue{Foundation.NSNumber}</a></format>
		///           value.
		///         </summary>
		MultiReal = MultiMask | Real,
		/// <summary>
		///           The property holds a
		///           <see cref="T:AddressBook.ABMultiValue{Foundation.NSDate}" />
		///           value.
		///         </summary>
		MultiDateTime = MultiMask | DateTime,
		/// <summary>
		///           The property holds a
		///           <see cref="T:AddressBook.ABMultiValue{Foundation.NSDictionary}" />
		///           value.
		///         </summary>
		MultiDictionary = MultiMask | Dictionary,

		MultiMask = (1 << 8),
	}

	// note: not a true flag
	/// <summary>An enumeration whose values specify various kinds of <see cref="T:AddressBook.ABSourceType" />.</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABSourceType : int /* typedef int */ {
		/// <summary>To be added.</summary>
		Local = 0x0,
		/// <summary>To be added.</summary>
		Exchange = 0x1,
		/// <summary>To be added.</summary>
		ExchangeGAL = Exchange | SearchableMask,
		/// <summary>To be added.</summary>
		MobileMe = 0x2,
		/// <summary>To be added.</summary>
		LDAP = 0x3 | SearchableMask,
		/// <summary>To be added.</summary>
		CardDAV = 0x4,
		/// <summary>To be added.</summary>
		DAVSearch = CardDAV | SearchableMask,

		/// <summary>To be added.</summary>
		SearchableMask = 0x01000000,
	};

	/// <summary>For internal use.</summary>
	///     
	///     <!-- TODO: Unused? Can't find any references -->
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	public enum ABSourceProperty {
		/// <summary>To be added.</summary>
		Name,
		/// <summary>To be added.</summary>
		Type,
	}

}

#endif
