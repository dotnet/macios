// 
// ABPerson.cs: Implements the managed ABPerson
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright (C) 2009 Novell, Inc
// Copyright (C) 2012-2013 Xamarin Inc.
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

#nullable enable

#if !MONOMAC

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace AddressBook {

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class ABPersonPropertyId {

		public static int Address { get; private set; }
		public static int Birthday { get; private set; }
		public static int CreationDate { get; private set; }
		public static int Date { get; private set; }
		public static int Department { get; private set; }
		public static int Email { get; private set; }
		public static int FirstName { get; private set; }
		public static int FirstNamePhonetic { get; private set; }
		public static int InstantMessage { get; private set; }
		public static int JobTitle { get; private set; }
		public static int Kind { get; private set; }
		public static int LastName { get; private set; }
		public static int LastNamePhonetic { get; private set; }
		public static int MiddleName { get; private set; }
		public static int MiddleNamePhonetic { get; private set; }
		public static int ModificationDate { get; private set; }
		public static int Nickname { get; private set; }
		public static int Note { get; private set; }
		public static int Organization { get; private set; }
		public static int Phone { get; private set; }
		public static int Prefix { get; private set; }
		public static int RelatedNames { get; private set; }
		public static int Suffix { get; private set; }
		public static int Url { get; private set; }
		public static int SocialProfile { get; private set; }

		static ABPersonPropertyId ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Address = Dlfcn.GetInt32 (handle, "kABPersonAddressProperty");
			Birthday = Dlfcn.GetInt32 (handle, "kABPersonBirthdayProperty");
			CreationDate = Dlfcn.GetInt32 (handle, "kABPersonCreationDateProperty");
			Date = Dlfcn.GetInt32 (handle, "kABPersonDateProperty");
			Department = Dlfcn.GetInt32 (handle, "kABPersonDepartmentProperty");
			Email = Dlfcn.GetInt32 (handle, "kABPersonEmailProperty");
			FirstName = Dlfcn.GetInt32 (handle, "kABPersonFirstNameProperty");
			FirstNamePhonetic = Dlfcn.GetInt32 (handle, "kABPersonFirstNamePhoneticProperty");
			InstantMessage = Dlfcn.GetInt32 (handle, "kABPersonInstantMessageProperty");
			JobTitle = Dlfcn.GetInt32 (handle, "kABPersonJobTitleProperty");
			Kind = Dlfcn.GetInt32 (handle, "kABPersonKindProperty");
			LastName = Dlfcn.GetInt32 (handle, "kABPersonLastNameProperty");
			LastNamePhonetic = Dlfcn.GetInt32 (handle, "kABPersonLastNamePhoneticProperty");
			MiddleName = Dlfcn.GetInt32 (handle, "kABPersonMiddleNameProperty");
			MiddleNamePhonetic = Dlfcn.GetInt32 (handle, "kABPersonMiddleNamePhoneticProperty");
			ModificationDate = Dlfcn.GetInt32 (handle, "kABPersonModificationDateProperty");
			Nickname = Dlfcn.GetInt32 (handle, "kABPersonNicknameProperty");
			Note = Dlfcn.GetInt32 (handle, "kABPersonNoteProperty");
			Organization = Dlfcn.GetInt32 (handle, "kABPersonOrganizationProperty");
			Phone = Dlfcn.GetInt32 (handle, "kABPersonPhoneProperty");
			Prefix = Dlfcn.GetInt32 (handle, "kABPersonPrefixProperty");
			RelatedNames = Dlfcn.GetInt32 (handle, "kABPersonRelatedNamesProperty");
			Suffix = Dlfcn.GetInt32 (handle, "kABPersonSuffixProperty");
			Url = Dlfcn.GetInt32 (handle, "kABPersonURLProperty");
			SocialProfile = Dlfcn.GetInt32 (handle, "kABPersonSocialProfileProperty");
		}

		public static int ToId (ABPersonProperty property)
		{
			switch (property) {
			case ABPersonProperty.Address: return Address;
			case ABPersonProperty.Birthday: return Birthday;
			case ABPersonProperty.CreationDate: return CreationDate;
			case ABPersonProperty.Date: return Date;
			case ABPersonProperty.Department: return Department;
			case ABPersonProperty.Email: return Email;
			case ABPersonProperty.FirstName: return FirstName;
			case ABPersonProperty.FirstNamePhonetic: return FirstNamePhonetic;
			case ABPersonProperty.InstantMessage: return InstantMessage;
			case ABPersonProperty.JobTitle: return JobTitle;
			case ABPersonProperty.Kind: return Kind;
			case ABPersonProperty.LastName: return LastName;
			case ABPersonProperty.LastNamePhonetic: return LastNamePhonetic;
			case ABPersonProperty.MiddleName: return MiddleName;
			case ABPersonProperty.MiddleNamePhonetic: return MiddleNamePhonetic;
			case ABPersonProperty.ModificationDate: return ModificationDate;
			case ABPersonProperty.Nickname: return Nickname;
			case ABPersonProperty.Note: return Note;
			case ABPersonProperty.Organization: return Organization;
			case ABPersonProperty.Phone: return Phone;
			case ABPersonProperty.Prefix: return Prefix;
			case ABPersonProperty.RelatedNames: return RelatedNames;
			case ABPersonProperty.Suffix: return Suffix;
			case ABPersonProperty.Url: return Url;
			case ABPersonProperty.SocialProfile: return SocialProfile;
			}
			throw new NotSupportedException ("Invalid ABPersonProperty value: " + property);
		}

		public static ABPersonProperty ToPersonProperty (int id)
		{
			if (id == Address) return ABPersonProperty.Address;
			if (id == Birthday) return ABPersonProperty.Birthday;
			if (id == CreationDate) return ABPersonProperty.CreationDate;
			if (id == Date) return ABPersonProperty.Date;
			if (id == Department) return ABPersonProperty.Department;
			if (id == Email) return ABPersonProperty.Email;
			if (id == FirstName) return ABPersonProperty.FirstName;
			if (id == FirstNamePhonetic) return ABPersonProperty.FirstNamePhonetic;
			if (id == InstantMessage) return ABPersonProperty.InstantMessage;
			if (id == JobTitle) return ABPersonProperty.JobTitle;
			if (id == Kind) return ABPersonProperty.Kind;
			if (id == LastName) return ABPersonProperty.LastName;
			if (id == LastNamePhonetic) return ABPersonProperty.LastNamePhonetic;
			if (id == MiddleName) return ABPersonProperty.MiddleName;
			if (id == MiddleNamePhonetic) return ABPersonProperty.MiddleNamePhonetic;
			if (id == ModificationDate) return ABPersonProperty.ModificationDate;
			if (id == Nickname) return ABPersonProperty.Nickname;
			if (id == Note) return ABPersonProperty.Note;
			if (id == Organization) return ABPersonProperty.Organization;
			if (id == Phone) return ABPersonProperty.Phone;
			if (id == Prefix) return ABPersonProperty.Prefix;
			if (id == RelatedNames) return ABPersonProperty.RelatedNames;
			if (id == Suffix) return ABPersonProperty.Suffix;
			if (id == Url) return ABPersonProperty.Url;
			if (id == SocialProfile) return ABPersonProperty.SocialProfile;
			throw new NotSupportedException ("Invalid ABPersonPropertyId value: " + id);
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonAddressKey.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonAddressKey']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonAddressKey {

		/// <summary>Represents the value associated with the constant kABPersonAddressCityKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the City portion of the address.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? City { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonAddressCountryKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the Country portion of the address.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Country { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonAddressCountryCodeKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the CountryCode portion of the address.
		///         </value>
		///         <remarks>
		///           <para>
		///             <format type="text/html">
		///               <span>
		///             See <a href="https://developer.apple.com/iphone/library/documentation/AddressBook/Reference/ABPersonRef_iPhoneOS/Reference/reference.html#//apple_ref/doc/uid/TP40007210-CH991-SW33">kABPersonAddresCountryCodeKey</a> documentation for a list of supported values.
		///             </span>
		///             </format>
		///           </para>
		///         </remarks>
		public static NSString? CountryCode { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonAddressStateKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the State portion of the address.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? State { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonAddressStreetKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the Street portion of the address.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Street { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonAddressZIPKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the Zip portion of the address.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Zip { get; private set; }

		static ABPersonAddressKey ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			City = Dlfcn.GetStringConstant (handle, "kABPersonAddressCityKey");
			Country = Dlfcn.GetStringConstant (handle, "kABPersonAddressCountryKey");
			CountryCode = Dlfcn.GetStringConstant (handle, "kABPersonAddressCountryCodeKey");
			State = Dlfcn.GetStringConstant (handle, "kABPersonAddressStateKey");
			Street = Dlfcn.GetStringConstant (handle, "kABPersonAddressStreetKey");
			Zip = Dlfcn.GetStringConstant (handle, "kABPersonAddressZIPKey");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonDateLabel.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonDateLabel']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonDateLabel {
		/// <summary>Represents the value associated with the constant kABPersonAnniversaryLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the "Birthdate" label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Anniversary { get; private set; }

		static ABPersonDateLabel ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			Anniversary = Dlfcn.GetStringConstant (Libraries.AddressBook.Handle, "kABPersonAnniversaryLabel");
		}
	}

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class ABPersonKindId {
		public static NSNumber? Organization { get; private set; }
		public static NSNumber? Person { get; private set; }

		static ABPersonKindId ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Organization = Dlfcn.GetNSNumber (handle, "kABPersonKindOrganization");
			Person = Dlfcn.GetNSNumber (handle, "kABPersonKindPerson");
		}

		public static ABPersonKind ToPersonKind (NSNumber value)
		{
			if (object.ReferenceEquals (Organization, value))
				return ABPersonKind.Organization;
			if (object.ReferenceEquals (Person, value))
				return ABPersonKind.Person;
			return ABPersonKind.None;
		}

		public static NSNumber? FromPersonKind (ABPersonKind value)
		{
			switch (value) {
			case ABPersonKind.Organization: return Organization;
			case ABPersonKind.Person: return Person;
			}
			return null;
		}
	}

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class ABPersonSocialProfile {
		public static readonly NSString? URLKey;
		public static readonly NSString? ServiceKey;
		public static readonly NSString? UsernameKey;
		public static readonly NSString? UserIdentifierKey;

		static ABPersonSocialProfile ()
		{
			var handle = Libraries.AddressBook.Handle;
			URLKey = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileURLKey");
			ServiceKey = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceKey");
			UsernameKey = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileUsernameKey");
			UserIdentifierKey = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileUserIdentifierKey");
		}
	}

	/// <summary>A class whose static members define constant names for various social networks.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonSocialProfileService {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? Twitter;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? GameCenter;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? Facebook;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? Myspace;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? LinkedIn;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? Flickr;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString? SinaWeibo;

		static ABPersonSocialProfileService ()
		{
			var handle = Libraries.AddressBook.Handle;
			Twitter = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceTwitter");
			GameCenter = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceGameCenter");
			Facebook = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceFacebook");
			Myspace = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceMyspace");
			LinkedIn = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceLinkedIn");
			Flickr = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceFlickr");
			SinaWeibo = Dlfcn.GetStringConstant (handle, "kABPersonSocialProfileServiceSinaWeibo");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonPhoneLabel.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonPhoneLabel']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonPhoneLabel {
		/// <summary>Represents the value associated with the constant kABPersonPhoneHomeFAXLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Home fax phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? HomeFax { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhoneIPhoneLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the iPhone phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? iPhone { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhoneMainLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Main phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Main { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhoneMobileLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Mobile phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Mobile { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhonePagerLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Pager phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Pager { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhoneWorkFAXLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Work fax phone number label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? WorkFax { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPhoneOtherFAXLabel</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? OtherFax { get; private set; }

		static ABPersonPhoneLabel ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			HomeFax = Dlfcn.GetStringConstant (handle, "kABPersonPhoneHomeFAXLabel");
			iPhone = Dlfcn.GetStringConstant (handle, "kABPersonPhoneIPhoneLabel");
			Main = Dlfcn.GetStringConstant (handle, "kABPersonPhoneMainLabel");
			Mobile = Dlfcn.GetStringConstant (handle, "kABPersonPhoneMobileLabel");
			Pager = Dlfcn.GetStringConstant (handle, "kABPersonPhonePagerLabel");
			WorkFax = Dlfcn.GetStringConstant (handle, "kABPersonPhoneWorkFAXLabel");
			OtherFax = Dlfcn.GetStringConstant (handle, "kABPersonPhoneOtherFAXLabel");
		}
	}

	/// <summary>
	///       Possible
	///       <see cref="P:AddressBook.ABPersonInstantMessageKey.Service" />
	///       key values.
	///     </summary>
	///     <remarks>
	///       <para>
	///         Instant message information is stored within
	///         <see cref="T:Foundation.NSDictionary" /> instances where
	///         the the
	///         <see cref="P:AddressBook.ABPersonInstantMessageKey.Service" />
	///         key is used to store the service name, and the
	///         <see cref="P:AddressBook.ABPersonInstantMessageKey.Username" />
	///         key is used to store the service login name.
	///       </para>
	///       <para>
	///         The <c>ABPersonInstantMessageService</c> stores predefined
	///         <see cref="P:AddressBook.ABPersonInstantMessageKey.Service" />
	///         values.
	///       </para>
	///     </remarks>
	///     <altmember cref="M:AddressBook.ABPerson.GetInstantMessages" />
	///     <altmember cref="M:AddressBook.ABPerson.SetInstantMessages(AddressBook.ABMultiValue{Foundation.NSDictionary})" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonInstantMessageService {
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceAIM</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the AIM instant message service.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Aim { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceICQ</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the ICQ instant message service.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Icq { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceJabber</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Jabber instant message service.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Jabber { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceMSN</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the MSN instant message service.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Msn { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceYahoo</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Yahoo instant message service.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Yahoo { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceQQ</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? QQ { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceGoogleTalk</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? GoogleTalk { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceSkype</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? Skype { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceFacebook</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? Facebook { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceGaduGadu</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString? GaduGadu { get; private set; }

		static ABPersonInstantMessageService ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Aim = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceAIM");
			Icq = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceICQ");
			Jabber = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceJabber");
			Msn = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceMSN");
			Yahoo = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceYahoo");
			QQ = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceQQ");
			GoogleTalk = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceGoogleTalk");
			Skype = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceSkype");
			Facebook = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceFacebook");
			GaduGadu = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceGaduGadu");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonInstantMessageKey.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonInstantMessageKey']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonInstantMessageKey {
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageServiceKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the Service portion of the instant message information.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Service { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonInstantMessageUsernameKey</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing the
		///           key to use for the Username portion of the instant message information.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Username { get; private set; }

		static ABPersonInstantMessageKey ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Service = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageServiceKey");
			Username = Dlfcn.GetStringConstant (handle, "kABPersonInstantMessageUsernameKey");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonUrlLabel.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonUrlLabel']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonUrlLabel {
		/// <summary>Represents the value associated with the constant kABPersonHomePageLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Home page URL label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? HomePage { get; private set; }

		static ABPersonUrlLabel ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			HomePage = Dlfcn.GetStringConstant (Libraries.AddressBook.Handle, "kABPersonHomePageLabel");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPersonRelatedNamesLabel.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPersonRelatedNamesLabel']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABPersonRelatedNamesLabel {
		/// <summary>Represents the value associated with the constant kABPersonAssistantLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Assistant related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Assistant { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonBrotherLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Brother related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Brother { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonChildLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Child related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Child { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonFatherLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Father related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Father { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonFriendLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Friend related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Friend { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonManagerLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Manager related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Manager { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonMotherLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Mother related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Mother { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonParentLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Parent related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Parent { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonPartnerLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Partner related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Partner { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonSisterLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Sister related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Sister { get; private set; }
		/// <summary>Represents the value associated with the constant kABPersonSpouseLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the Spouse related name label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Spouse { get; private set; }

		static ABPersonRelatedNamesLabel ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Assistant = Dlfcn.GetStringConstant (handle, "kABPersonAssistantLabel");
			Brother = Dlfcn.GetStringConstant (handle, "kABPersonBrotherLabel");
			Child = Dlfcn.GetStringConstant (handle, "kABPersonChildLabel");
			Father = Dlfcn.GetStringConstant (handle, "kABPersonFatherLabel");
			Friend = Dlfcn.GetStringConstant (handle, "kABPersonFriendLabel");
			Manager = Dlfcn.GetStringConstant (handle, "kABPersonManagerLabel");
			Mother = Dlfcn.GetStringConstant (handle, "kABPersonMotherLabel");
			Parent = Dlfcn.GetStringConstant (handle, "kABPersonParentLabel");
			Partner = Dlfcn.GetStringConstant (handle, "kABPersonPartnerLabel");
			Sister = Dlfcn.GetStringConstant (handle, "kABPersonSisterLabel");
			Spouse = Dlfcn.GetStringConstant (handle, "kABPersonSpouseLabel");
		}
	}

	/// <summary>
	///       Generic property labels.
	///     </summary>
	///     <remarks>
	///       <para>
	///         Labels are used with
	///         <see cref="M:AddressBook.ABAddressBook.LocalizedLabel(Foundation.NSString)" />,
	///         <see cref="P:AddressBook.ABMultiValueEntry`1.Label" />,
	///         <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=M:AddressBook.ABMultiValue`1.Add(`0,Foundation.NSString)&amp;scope=Xamarin" title="M:AddressBook.ABMultiValue`1.Add(`0,Foundation.NSString)">M:AddressBook.ABMultiValue`1.Add(`0,Foundation.NSString)</a></format>, and
	///         <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=M:AddressBook.ABMultiValue`1.Insert(System.Int32,`0,Foundation.NSString)&amp;scope=Xamarin" title="M:AddressBook.ABMultiValue`1.Insert(System.Int32,`0,Foundation.NSString)">M:AddressBook.ABMultiValue`1.Insert(System.Int32,`0,Foundation.NSString)</a></format>.
	///       </para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class ABLabel {
		/// <summary>Represents the value associated with the constant kABHomeLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the "Home" label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Home { get; private set; }
		/// <summary>Represents the value associated with the constant kABOtherLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the "Other" label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Other { get; private set; }
		/// <summary>Represents the value associated with the constant kABWorkLabel</summary>
		///         <value>
		///           A <see cref="T:Foundation.NSString" /> containing
		///           the "Work" label.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static NSString? Work { get; private set; }

		static ABLabel ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Home = Dlfcn.GetStringConstant (handle, "kABHomeLabel");
			Other = Dlfcn.GetStringConstant (handle, "kABOtherLabel");
			Work = Dlfcn.GetStringConstant (handle, "kABWorkLabel");
		}
	}

	/// <include file="../../docs/api/AddressBook/ABPerson.xml" path="/Documentation/Docs[@DocId='T:AddressBook.ABPerson']/*" />
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABPerson : ABRecord, IComparable, IComparable<ABPerson> {
		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCreate ();

		/// <summary>
		///           Constructs and initializes a
		///           <see cref="T:AddressBook.ABPerson" /> instance.
		///         </summary>
		///         <remarks>
		///         </remarks>
		public ABPerson ()
			: base (ABPersonCreate (), true)
		{
			InitConstants.Init ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCreateInSource (IntPtr source);

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ABPerson (ABRecord source)
			: base (ABPersonCreateInSource (source.GetNonNullHandle (nameof (source))), true)
		{
			GC.KeepAlive (source);
		}

		[Preserve (Conditional = true)]
		internal ABPerson (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		internal ABPerson (NativeHandle handle, ABAddressBook? addressbook)
			: base (handle, false)
		{
			AddressBook = addressbook;
		}

		/// <include file="../../docs/api/AddressBook/ABPerson.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABPerson.System#IComparable#CompareTo(System.Object)']/*" />
		int IComparable.CompareTo (object? o)
		{
			var other = o as ABPerson;
			if (other is null)
				throw new ArgumentException ("Can only compare to other ABPerson instances.", nameof (o));
			return CompareTo (other);
		}

		/// <include file="../../docs/api/AddressBook/ABPerson.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABPerson.CompareTo(AddressBook.ABPerson)']/*" />
		public int CompareTo (ABPerson? other)
		{
			return CompareTo (other!, ABPersonSortBy.LastName);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static int ABPersonComparePeopleByName (IntPtr person1, IntPtr person2, ABPersonSortBy ordering);
		/// <include file="../../docs/api/AddressBook/ABPerson.xml" path="/Documentation/Docs[@DocId='M:AddressBook.ABPerson.CompareTo(AddressBook.ABPerson,AddressBook.ABPersonSortBy)']/*" />
		public int CompareTo (ABPerson other, ABPersonSortBy ordering)
		{
			if (other is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (other));
			if (ordering != ABPersonSortBy.FirstName && ordering != ABPersonSortBy.LastName)
				throw new ArgumentException ("Invalid ordering value: " + ordering, "ordering");
			int result = ABPersonComparePeopleByName (Handle, other.Handle, ordering);
			GC.KeepAlive (other);
			return result;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopyLocalizedPropertyName (int /* ABPropertyID = int32_t */ propertyId);
		/// <param name="property">
		///           A <see cref="T:AddressBook.ABPersonProperty" />
		///           containing the property to get the localized name of.
		///         </param>
		///         <summary>
		///           Gets the localized name of a <paramref name="property" />.
		///         </summary>
		///         <returns>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing the localized
		///           name of a <paramref name="property" />.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public static string? LocalizedPropertyName (ABPersonProperty property)
		{
			return CFString.FromHandle (ABPersonCopyLocalizedPropertyName (ABPersonPropertyId.ToId (property)));
		}

		/// <param name="propertyId">A value that corresponds to one of the low-level kABPersonProperty fields.
		///         </param>
		///         <summary>
		///           Gets the localized name of a <paramref name="propertyId" />.
		///         </summary>
		///         <returns>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing the localized
		///           name of a <paramref name="propertyId" />.
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    Unlike the overload that takes a ABPersonProperty, the value
		/// 	    of the <paramref name="propertyId" /> is actually not a
		/// 	    constant and can vary at runtime (this is the native C
		/// 	    interface).
		/// 	  </para>
		///           <para>
		/// 	    This method is typically used on callbacks that provide an
		/// 	    "int propertyId" as a parameter.
		/// 	  </para>
		///         </remarks>
		public static string? LocalizedPropertyName (int propertyId)
		{
			return CFString.FromHandle (ABPersonCopyLocalizedPropertyName (propertyId));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static ABPropertyType ABPersonGetTypeOfProperty (int /* ABPropertyID = int32_t */ propertyId);
		/// <param name="property">
		///           A <see cref="T:AddressBook.ABPersonProperty" />
		///           specifying which property to query.
		///         </param>
		///         <summary>
		///           Gets the type of the property <paramref name="property" />.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABPropertyType" /> value containing
		///           the type of the property <paramref name="property" />.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public static ABPropertyType GetPropertyType (ABPersonProperty property)
		{
			return ABPersonGetTypeOfProperty (ABPersonPropertyId.ToId (property));
		}

		/// <param name="propertyId">A value that corresponds to one of
		/// 	the low-level kABPersonProperty fields.</param>
		///         <summary>Gets the type of the property <paramref name="propertyId" />.</summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABPropertyType" /> value containing
		///           the type of the property <paramref name="propertyId" />.
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    Unlike the overload that takes a ABPersonProperty, the value
		/// 	    of the <paramref name="propertyId" /> is actually not a
		/// 	    constant and can vary at runtime (this is the native C
		/// 	    interface).
		/// 	  </para>
		///           <para>
		/// 	    This method is typically used on callbacks that provide an
		/// 	    "int propertyId" as a parameter.
		/// 	  </para>
		///         </remarks>
		public static ABPropertyType GetPropertyType (int propertyId)
		{
			return ABPersonGetTypeOfProperty (propertyId);
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABPersonSetImageData (IntPtr person, IntPtr imageData, IntPtr* error);
		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopyImageData (IntPtr person);

		/// <summary>
		///           Gets or sets the
		///           <see cref="T:AddressBook.ABPerson" />'s picture.
		///         </summary>
		///         <value>
		///           A <see cref="T:Foundation.NSData" /> containing
		///           the picture data.
		///         </value>
		///         <remarks>
		///         </remarks>
		///         <exception cref="T:CoreFoundation.CFException">
		///           The reason the picture couldn't be set.
		///         </exception>
		///         <altmember cref="P:AddressBook.ABPerson.HasImage" />
		///         <altmember cref="P:AddressBook.ABPerson.RemoveImage" />
		public NSData? Image {
			get { return Runtime.GetNSObject<NSData> (ABPersonCopyImageData (Handle)); }
			set {
				IntPtr error;
				unsafe {
					if (ABPersonSetImageData (Handle, value.GetHandle (), &error) == 0)
						throw CFException.FromCFError (error);
					GC.KeepAlive (value);
				}
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static byte ABPersonHasImageData (IntPtr person);
		/// <summary>
		///           Gets a value indicating whether the
		///           <see cref="T:AddressBook.ABPerson" /> has a picture.
		///         </summary>
		///         <value>
		///           <see langword="true" /> if the
		///           <see cref="T:AddressBook.ABPerson" /> has a picture;
		///           otherwise, <see langword="false" />.
		///         </value>
		///         <remarks>
		///         </remarks>
		public bool HasImage {
			get { return ABPersonHasImageData (Handle) != 0; }
		}

		[DllImport (Constants.AddressBookLibrary)]
		unsafe extern static byte ABPersonRemoveImageData (IntPtr person, IntPtr* error);
		/// <summary>
		///           Removes a <see cref="T:AddressBook.ABPerson" />'s picture.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <exception cref="T:CoreFoundation.CFException">
		///           The reason the picture couldn't be removed.
		///         </exception>
		///         <altmember cref="P:AddressBook.ABPerson.Image" />
		public void RemoveImage ()
		{
			IntPtr error;
			unsafe {
				if (ABPersonRemoveImageData (Handle, &error) == 0)
					throw CFException.FromCFError (error);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static ABPersonCompositeNameFormat ABPersonGetCompositeNameFormat ();

		/// <summary>Developers should not use this deprecated property. Developers should use GetCompositeNameFormat (null) instead</summary>
		///         <value>
		///           A <see cref="T:AddressBook.ABPersonCompositeNameFormat" />
		///           which controls the format used for the person's composite name.
		///         </value>
		///         <remarks>
		///         </remarks>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[ObsoletedOSPlatform ("ios", "Use 'GetCompositeNameFormat (null)' instead.")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'GetCompositeNameFormat (null)' instead.")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public static ABPersonCompositeNameFormat CompositeNameFormat {
			get { return ABPersonGetCompositeNameFormat (); }
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static ABPersonCompositeNameFormat ABPersonGetCompositeNameFormatForRecord (IntPtr record);

		/// <param name="record">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ABPersonCompositeNameFormat GetCompositeNameFormat (ABRecord? record)
		{
			var result = ABPersonGetCompositeNameFormatForRecord (record.GetHandle ());
			GC.KeepAlive (record);
			return result;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopyCompositeNameDelimiterForRecord (IntPtr record);

		/// <param name="record">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? GetCompositeNameDelimiter (ABRecord? record)
		{
			var handle = ABPersonCopyCompositeNameDelimiterForRecord (record.GetHandle ());
			GC.KeepAlive (record);
			return CFString.FromHandle (handle, true);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static ABPersonSortBy ABPersonGetSortOrdering ();
		/// <summary>
		///           Gets the user's sort ordering preference for lists of persons.
		///         </summary>
		///         <value>
		///           A <see cref="T:AddressBook.ABPersonSortBy" /> which
		///           is the user's sort ordering preference for lists of persons.
		///         </value>
		///         <remarks>
		///         </remarks>
		public static ABPersonSortBy SortOrdering {
			get { return ABPersonGetSortOrdering (); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s first name.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s first name.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? FirstName {
			get { return PropertyToString (ABPersonPropertyId.FirstName); }
			set { SetValue (ABPersonPropertyId.FirstName, value); }
		}

		/// <summary>
		///           Gets or sets <see cref="T:AddressBook.ABPerson" />'s first name
		///           phonetic pronounciation.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s first name
		///           phonetic pronounciation.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? FirstNamePhonetic {
			get { return PropertyToString (ABPersonPropertyId.FirstNamePhonetic); }
			set { SetValue (ABPersonPropertyId.FirstNamePhonetic, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s last name.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s last name.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? LastName {
			get { return PropertyToString (ABPersonPropertyId.LastName); }
			set { SetValue (ABPersonPropertyId.LastName, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s last name
		///           phonetic pronounciation.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s last name phonetic pronounciation.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? LastNamePhonetic {
			get { return PropertyToString (ABPersonPropertyId.LastNamePhonetic); }
			set { SetValue (ABPersonPropertyId.LastNamePhonetic, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s middle name.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s middle name.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? MiddleName {
			get { return PropertyToString (ABPersonPropertyId.MiddleName); }
			set { SetValue (ABPersonPropertyId.MiddleName, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s middle name
		///           phonetic pronounciation.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s middle name phonetic pronounciation.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? MiddleNamePhonetic {
			get { return PropertyToString (ABPersonPropertyId.MiddleNamePhonetic); }
			set { SetValue (ABPersonPropertyId.MiddleNamePhonetic, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s prefix.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s prefix.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Prefix {
			get { return PropertyToString (ABPersonPropertyId.Prefix); }
			set { SetValue (ABPersonPropertyId.Prefix, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s suffix.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s suffix.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Suffix {
			get { return PropertyToString (ABPersonPropertyId.Suffix); }
			set { SetValue (ABPersonPropertyId.Suffix, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s nickname.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s nickname.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Nickname {
			get { return PropertyToString (ABPersonPropertyId.Nickname); }
			set { SetValue (ABPersonPropertyId.Nickname, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s organization.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s organization.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Organization {
			get { return PropertyToString (ABPersonPropertyId.Organization); }
			set { SetValue (ABPersonPropertyId.Organization, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s job title.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s job title.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? JobTitle {
			get { return PropertyToString (ABPersonPropertyId.JobTitle); }
			set { SetValue (ABPersonPropertyId.JobTitle, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s department.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s department.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Department {
			get { return PropertyToString (ABPersonPropertyId.Department); }
			set { SetValue (ABPersonPropertyId.Department, value); }
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopySource (IntPtr group);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABRecord? Source {
			get {
				var h = ABPersonCopySource (Handle);
				if (h == IntPtr.Zero)
					return null;

				return FromHandle (h, null);
			}
		}

		internal static string ToString (NativeHandle value)
		{
			return CFString.FromHandle (value)!;
		}

		/// <summary>
		///           Gets the <see cref="T:AddressBook.ABPerson" />'s email addresses.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s email addresses.
		///         </returns>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="M:AddressBook.ABPerson.SetEmails(AddressBook.ABMultiValue`1)" />
		public ABMultiValue<string>? GetEmails ()
		{
			return CreateStringMultiValue (CopyValue (ABPersonPropertyId.Email));
		}

		static ABMultiValue<string>? CreateStringMultiValue (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;
			return new ABMultiValue<string> (handle, ABPerson.ToString, CFString.CreateNative, true);
		}

		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new email addresses
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new email addresses.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPerson.GetEmails" />
		public void SetEmails (ABMultiValue<string>? value)
		{
			SetValue (ABPersonPropertyId.Email, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s birthday.
		///         </summary>
		///         <value>
		///           A <see cref="T:Foundation.NSDate" /> containing
		///           the person's birthday.
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDate? Birthday {
			get { return PropertyTo<NSDate> (ABPersonPropertyId.Birthday); }
			set { SetValue (ABPersonPropertyId.Birthday, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s note.
		///         </summary>
		///         <value>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s note.
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Note {
			get { return PropertyToString (ABPersonPropertyId.Note); }
			set { SetValue (ABPersonPropertyId.Note, value); }
		}

		/// <summary>
		///           Gets or sets the record's creation date.
		///         </summary>
		///         <value>
		///           A <see cref="T:Foundation.NSDate" /> containing
		///           the record's creation date.
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDate? CreationDate {
			get { return PropertyTo<NSDate> (ABPersonPropertyId.CreationDate); }
			set { SetValue (ABPersonPropertyId.CreationDate, value); }
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s modification date.
		///         </summary>
		///         <value>
		///           A <see cref="T:Foundation.NSDate" /> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s modification date.
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDate? ModificationDate {
			get { return PropertyTo<NSDate> (ABPersonPropertyId.ModificationDate); }
			set { SetValue (ABPersonPropertyId.ModificationDate, value); }
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABMultiValue<PersonAddress>? GetAllAddresses ()
		{
			return CreateDictionaryMultiValue<PersonAddress> (CopyValue (ABPersonPropertyId.Address), l => new PersonAddress (l));
		}

		// Obsolete
		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue{Foundation.NSDictionary}" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new addresses.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new addresses.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPersonAddressKey" />
		///         <altmember cref="T:AddressBook.ABPerson.GetAddresses" />
		public void SetAddresses (ABMultiValue<NSDictionary>? value)
		{
			SetValue (ABPersonPropertyId.Address, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <param name="addresses">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetAddresses (ABMultiValue<PersonAddress>? addresses)
		{
			SetValue (ABPersonPropertyId.Address, addresses.GetHandle ());
			GC.KeepAlive (addresses);
		}

		// Obsolete
		static ABMultiValue<NSDictionary>? CreateDictionaryMultiValue (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;
			return new ABMultiValue<NSDictionary> (handle, true);
		}

		static ABMultiValue<T>? CreateDictionaryMultiValue<T> (NativeHandle handle, Func<NSDictionary, T> factory) where T : DictionaryContainer
		{
			if (handle == NativeHandle.Zero)
				return null;

			return new ABMultiValue<T> (handle,
				l => factory ((NSDictionary) (object) Runtime.GetNSObject (l)!),
				l => l.Dictionary.Handle,
				false);
		}

		/// <summary>
		///           Gets the <see cref="T:AddressBook.ABPerson" />'s dates.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABMultiValue{Foundation.NSDate}" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s dates.
		///         </returns>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPersonDateLabel" />
		///         <altmember cref="M:AddressBook.ABPerson.SetDates(AddressBook.ABMultiValue{Foundation.NSDate})" />
		public ABMultiValue<NSDate>? GetDates ()
		{
			return CreateDateMultiValue (CopyValue (ABPersonPropertyId.Date));
		}

		static ABMultiValue<NSDate>? CreateDateMultiValue (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;
			return new ABMultiValue<NSDate> (handle, true);
		}

		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue{Foundation.NSDate}" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new dates.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new dates.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPersonDateLabel" />
		///         <altmember cref="T:AddressBook.ABPerson.GetDates" />
		public void SetDates (ABMultiValue<NSDate>? value)
		{
			SetValue (ABPersonPropertyId.Date, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <summary>
		///           Gets or sets the <see cref="T:AddressBook.ABPerson" />'s
		///           <see cref="T:AddressBook.ABPersonKind" />.
		///         </summary>
		///         <value>
		///           A <see cref="T:AddressBook.ABPersonKind" /> containing
		///           the <see cref="T:AddressBook.ABPerson" />'s kind.
		///         </value>
		///         <remarks>
		///           <para>
		///             The <see cref="T:AddressBook.ABPersonKind" />
		///             controls whether the instance is a person or an organization.
		///           </para>
		///         </remarks>
		public ABPersonKind PersonKind {
			get { return ABPersonKindId.ToPersonKind (PropertyTo<NSNumber> (ABPersonPropertyId.Kind!)!); }
			set { SetValue (ABPersonPropertyId.Kind!, ABPersonKindId.FromPersonKind (value)); }
		}

		/// <summary>
		///           Gets the <see cref="T:AddressBook.ABPerson" />'s phone numbers.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s phone numbers.
		///         </returns>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="M:AddressBook.ABPerson.SetPhones(AddressBook.ABMultiValue`1)" />
		public ABMultiValue<string>? GetPhones ()
		{
			return CreateStringMultiValue (CopyValue (ABPersonPropertyId.Phone));
		}

		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new phone numbers.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new phone numbers.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPerson.GetPhones" />
		public void SetPhones (ABMultiValue<string>? value)
		{
			SetValue (ABPersonPropertyId.Phone, value.GetHandle ());
			GC.KeepAlive (value);
		}

		[Advice ("Use GetInstantMessageServices.")]
		ABMultiValue<NSDictionary>? GetInstantMessages ()
		{
			return CreateDictionaryMultiValue (CopyValue (ABPersonPropertyId.InstantMessage));
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABMultiValue<InstantMessageService>? GetInstantMessageServices ()
		{
			return CreateDictionaryMultiValue<InstantMessageService> (CopyValue (ABPersonPropertyId.InstantMessage), l => new InstantMessageService (l));
		}

		// Obsolete
		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue{Foundation.NSDictionary}" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new instant messaging services.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new instant messaging services.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPerson.GetInstantMessages" />
		public void SetInstantMessages (ABMultiValue<NSDictionary>? value)
		{
			SetValue (ABPersonPropertyId.InstantMessage, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <param name="services">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetInstantMessages (ABMultiValue<InstantMessageService>? services)
		{
			SetValue (ABPersonPropertyId.InstantMessage, services.GetHandle ());
			GC.KeepAlive (services);
		}

		[Advice ("Use GetSocialProfiles.")]
		ABMultiValue<NSDictionary>? GetSocialProfile ()
		{
			return CreateDictionaryMultiValue (CopyValue (ABPersonPropertyId.SocialProfile));
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABMultiValue<SocialProfile>? GetSocialProfiles ()
		{
			return CreateDictionaryMultiValue<SocialProfile> (CopyValue (ABPersonPropertyId.SocialProfile), l => new SocialProfile (l));
		}

		// Obsolete
		/// <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetSocialProfile (ABMultiValue<NSDictionary>? value)
		{
			SetValue (ABPersonPropertyId.SocialProfile, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <param name="profiles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetSocialProfile (ABMultiValue<SocialProfile>? profiles)
		{
			SetValue (ABPersonPropertyId.SocialProfile, profiles.GetHandle ());
			GC.KeepAlive (profiles);
		}

		/// <summary>
		///           Gets the <see cref="T:AddressBook.ABPerson" />'s URLs.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s URLs.
		///         </returns>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="M:AddressBook.ABPerson.SetUrls(AddressBook.ABMultiValue`1)" />
		public ABMultiValue<string>? GetUrls ()
		{
			return CreateStringMultiValue (CopyValue (ABPersonPropertyId.Url));
		}

		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new URLs.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new URLs.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPerson.GetUrls" />
		public void SetUrls (ABMultiValue<string>? value)
		{
			SetValue (ABPersonPropertyId.Url, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <summary>
		///           Gets the <see cref="T:AddressBook.ABPerson" />'s related names.
		///         </summary>
		///         <returns>
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s related names.
		///         </returns>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="M:AddressBook.ABPerson.SetRelatedNames(AddressBook.ABMultiValue`1)" />
		public ABMultiValue<string>? GetRelatedNames ()
		{
			return CreateStringMultiValue (CopyValue (ABPersonPropertyId.RelatedNames));
		}

		/// <param name="value">
		///           A <see cref="T:AddressBook.ABMultiValue`1" />
		///           containing the <see cref="T:AddressBook.ABPerson" />'s new related names.
		///         </param>
		///         <summary>
		///           Sets the <see cref="T:AddressBook.ABPerson" />'s new related names.
		///         </summary>
		///         <remarks>
		///         </remarks>
		///         <altmember cref="T:AddressBook.ABPerson.GetRelatedNames" />
		public void SetRelatedNames (ABMultiValue<string>? value)
		{
			SetValue (ABPersonPropertyId.RelatedNames, value.GetHandle ());
			GC.KeepAlive (value);
		}

		/// <param name="property">
		///           A <see cref="T:AddressBook.ABPersonProperty" />
		///           specifying which property to return.
		///         </param>
		///         <summary>
		///           Gets the specified property.
		///         </summary>
		///         <returns>
		///           A <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20Object&amp;scope=Xamarin" title="T:System.Object">T:System.Object</a></format> containing the value of the specified property.
		///         </returns>
		///         <remarks>
		///         </remarks>
		public object? GetProperty (ABPersonProperty property)
		{
			switch (property) {
			case ABPersonProperty.Address: return GetAllAddresses ();
			case ABPersonProperty.Birthday: return Birthday;
			case ABPersonProperty.CreationDate: return CreationDate;
			case ABPersonProperty.Date: return GetDates ();
			case ABPersonProperty.Department: return Department;
			case ABPersonProperty.Email: return GetEmails ();
			case ABPersonProperty.FirstName: return FirstName;
			case ABPersonProperty.FirstNamePhonetic: return FirstNamePhonetic;
			case ABPersonProperty.InstantMessage: return GetInstantMessages ();
			case ABPersonProperty.JobTitle: return JobTitle;
			case ABPersonProperty.Kind: return PersonKind;
			case ABPersonProperty.LastName: return LastName;
			case ABPersonProperty.LastNamePhonetic: return LastNamePhonetic;
			case ABPersonProperty.MiddleName: return MiddleName;
			case ABPersonProperty.MiddleNamePhonetic: return MiddleNamePhonetic;
			case ABPersonProperty.ModificationDate: return ModificationDate;
			case ABPersonProperty.Nickname: return Nickname;
			case ABPersonProperty.Note: return Note;
			case ABPersonProperty.Organization: return Organization;
			case ABPersonProperty.Phone: return GetPhones ();
			case ABPersonProperty.Prefix: return Prefix;
			case ABPersonProperty.RelatedNames: return GetRelatedNames ();
			case ABPersonProperty.Suffix: return Suffix;
			case ABPersonProperty.Url: return GetUrls ();
			case ABPersonProperty.SocialProfile: return GetSocialProfile ();
			}
			throw new ArgumentException ("Invalid property value: " + property);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopyArrayOfAllLinkedPeople (IntPtr person);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ABPerson? []? GetLinkedPeople ()
		{
			var linked = ABPersonCopyArrayOfAllLinkedPeople (Handle);
			return NSArray.ArrayFromHandle (linked, l => new ABPerson (l, null));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCopyImageDataWithFormat (IntPtr handle, nint format);

		/// <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? GetImage (ABPersonImageFormat format)
		{
			return Runtime.GetNSObject<NSData> (ABPersonCopyImageDataWithFormat (Handle, (nint) (long) format));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCreateVCardRepresentationWithPeople (IntPtr people);

		/// <param name="people">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSData? GetVCards (params ABPerson [] people)
		{
			if (people is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (people));

			var ptrs = new NativeHandle [people.Length];
			for (int i = 0; i < people.Length; ++i) {
				ptrs [i] = people [i].Handle;
			}

			var ptr = ABPersonCreateVCardRepresentationWithPeople (CFArray.Create (ptrs));
			return Runtime.GetNSObject<NSData> (ptr, true);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABPersonCreatePeopleInSourceWithVCardRepresentation (IntPtr source, IntPtr vCardData);

		/// <param name="source">To be added.</param>
		///         <param name="vCardData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static ABPerson? []? CreateFromVCard (ABRecord? source, NSData vCardData)
		{
			if (vCardData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vCardData));

			// TODO: SIGSEGV when source is not null
			var res = ABPersonCreatePeopleInSourceWithVCardRepresentation (source.GetHandle (), vCardData.Handle);

			GC.KeepAlive (source);
			GC.KeepAlive (vCardData);

			return NSArray.ArrayFromHandle (res, l => new ABPerson (l, null));
		}
	}

	/// <summary>Manages social profile configuration.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class SocialProfile : DictionaryContainer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SocialProfile ()
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SocialProfile (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>Social profile service name.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonSocialProfileServiceKey value to access the underlying dictionary.</remarks>
		public string? ServiceName {
			get {
				return GetStringValue (ABPersonSocialProfile.ServiceKey!);
			}
			set {
				SetStringValue (ABPersonSocialProfile.ServiceKey!, value);
			}
		}

		/// <summary>Represents the social profile username.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonSocialProfileUsernameKey value to access the underlying dictionary.</remarks>
		public string? Username {
			get {
				return GetStringValue (ABPersonSocialProfile.UsernameKey!);
			}
			set {
				SetStringValue (ABPersonSocialProfile.UsernameKey!, value);
			}
		}

		/// <summary>Represents the social profile user identifier.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonSocialProfileUserIdentifierKey value to access the underlying dictionary.</remarks>
		public string? UserIdentifier {
			get {
				return GetStringValue (ABPersonSocialProfile.UserIdentifierKey!);
			}
			set {
				SetStringValue (ABPersonSocialProfile.UserIdentifierKey!, value);
			}
		}

		/// <summary>Represents the social profile URL.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonSocialProfileURLKey value to access the underlying dictionary.</remarks>
		public string? Url {
			get {
				return GetStringValue (ABPersonSocialProfile.URLKey!);
			}
			set {
				SetStringValue (ABPersonSocialProfile.URLKey!, value);
			}
		}
	}

	/// <summary>Manages instance message service configuration.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class InstantMessageService : DictionaryContainer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public InstantMessageService ()
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public InstantMessageService (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>Instant message service name.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonInstantMessageServiceKey value to access the underlying dictionary.</remarks>
		public string? ServiceName {
			get {
				// TODO: It does not return ABPersonInstantMessageService value. Underlying
				// value is custom string, it coould be MT bug because this makes
				// ABPersonInstantMessageService constants useless
				return GetStringValue (ABPersonInstantMessageKey.Service!);
			}
			set {
				SetStringValue (ABPersonInstantMessageKey.Service!, value);
			}
		}

		/// <summary>Instant message service user name.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonInstantMessageUsernameKey value to access the underlying dictionary.</remarks>
		public string? Username {
			get {
				return GetStringValue (ABPersonInstantMessageKey.Username!);
			}
			set {
				SetStringValue (ABPersonInstantMessageKey.Username!, value);
			}
		}
	}

	/// <summary>Manages the person address.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class PersonAddress : DictionaryContainer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PersonAddress ()
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PersonAddress (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>City</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonAddressProperty value to access the underlying dictionary.</remarks>
		public string? City {
			get {
				return GetStringValue (ABPersonAddressKey.City!);
			}
			set {
				SetStringValue (ABPersonAddressKey.City!, value);
			}
		}

		/// <summary>Represents country name.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonAddressCountryKey value to access the underlying dictionary.</remarks>
		public string? Country {
			get {
				return GetStringValue (ABPersonAddressKey.Country!);
			}
			set {
				SetStringValue (ABPersonAddressKey.Country!, value);
			}
		}

		/// <summary>Represents country code.</summary>
		///         <value>The value must be in the form of 2 character ISO-3166 country codes.</value>
		///         <remarks>The property uses constant kABPersonAddressCountryCodeKey value to access the underlying dictionary.</remarks>
		public string? CountryCode {
			get {
				return GetStringValue (ABPersonAddressKey.CountryCode!);
			}
			set {
				SetStringValue (ABPersonAddressKey.CountryCode!, value);
			}
		}

		/// <summary>State</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonAddressStateKey value to access the underlying dictionary.</remarks>
		public string? State {
			get {
				return GetStringValue (ABPersonAddressKey.State!);
			}
			set {
				SetStringValue (ABPersonAddressKey.State!, value);
			}
		}

		/// <summary>Street</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonAddressStreetKey value to access the underlying dictionary.</remarks>
		public string? Street {
			get {
				return GetStringValue (ABPersonAddressKey.Street!);
			}
			set {
				SetStringValue (ABPersonAddressKey.Street!, value);
			}
		}

		/// <summary>ZIP</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kABPersonAddressZIPKey value to access the underlying dictionary.</remarks>
		public string? Zip {
			get {
				return GetStringValue (ABPersonAddressKey.Zip!);
			}
			set {
				SetStringValue (ABPersonAddressKey.Zip!, value);
			}
		}
	}
}

#endif // !MONOMAC
