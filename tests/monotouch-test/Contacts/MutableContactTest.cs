//
// Unit tests for CNContact
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using Contacts;

namespace MonoTouchFixtures.Contacts {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MutableContactTest {

		[SetUp]
		public void MinimumSdkCheck ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void Properties ()
		{
			using (var contact = new CNMutableContact ()) {
				ClassicAssert.IsNull (contact.Birthday, "Birthday");
				ClassicAssert.AreEqual (0, contact.ContactRelations.Length, "ContactRelations");
				ClassicAssert.AreEqual (CNContactType.Person, contact.ContactType, "ContactType");
				ClassicAssert.AreEqual (0, contact.Dates.Length, "Dates");
				ClassicAssert.AreEqual (string.Empty, contact.DepartmentName, "DepartmentName");
				ClassicAssert.AreEqual (0, contact.EmailAddresses.Length, "EmailAddresses");
				ClassicAssert.AreEqual (string.Empty, contact.FamilyName, "FamilyName");
				ClassicAssert.AreEqual (string.Empty, contact.GivenName, "GivenName");
				ClassicAssert.AreNotEqual (string.Empty, contact.Identifier, "Identifier");
				ClassicAssert.IsNull (contact.ImageData, "ImageData");
				ClassicAssert.IsFalse (contact.ImageDataAvailable, "ImageDataAvailable");
				ClassicAssert.AreEqual (0, contact.InstantMessageAddresses.Length, "InstantMessageAddresses");
				ClassicAssert.AreEqual (string.Empty, contact.JobTitle, "JobTitle");
				ClassicAssert.AreEqual (string.Empty, contact.MiddleName, "MiddleName");
				ClassicAssert.AreEqual (string.Empty, contact.NamePrefix, "NamePrefix");
				ClassicAssert.AreEqual (string.Empty, contact.NameSuffix, "NameSuffix");
				ClassicAssert.AreEqual (string.Empty, contact.Nickname, "Nickname");
				ClassicAssert.IsNull (contact.NonGregorianBirthday, "NonGregorianBirthday");
				ClassicAssert.AreEqual (string.Empty, contact.Note, "Note");
				ClassicAssert.AreEqual (string.Empty, contact.OrganizationName, "OrganizationName");
				ClassicAssert.AreEqual (0, contact.PhoneNumbers.Length, "PhoneNumbers");
				ClassicAssert.AreEqual (string.Empty, contact.PhoneticFamilyName, "PhoneticFamilyName");
				ClassicAssert.AreEqual (string.Empty, contact.PhoneticGivenName, "PhoneticGivenName");
				ClassicAssert.AreEqual (string.Empty, contact.PhoneticMiddleName, "PhoneticMiddleName");
				ClassicAssert.AreEqual (0, contact.PostalAddresses.Length, "PostalAddresses");
				ClassicAssert.AreEqual (string.Empty, contact.PreviousFamilyName, "PreviousFamilyName");
				ClassicAssert.AreEqual (0, contact.SocialProfiles.Length, "SocialProfiles");
				ClassicAssert.IsNull (contact.ThumbnailImageData, "ThumbnailImageData");
				ClassicAssert.AreEqual (0, contact.UrlAddresses.Length, "UrlAddresses");

				contact.Birthday = new NSDateComponents () {
					Year = 1980
				};
				ClassicAssert.AreEqual ((nint) 1980, contact.Birthday.Year, "Birthday");

				contact.ContactRelations = new CNLabeledValue<CNContactRelation> [] {
					new CNLabeledValue<CNContactRelation> ("label", new CNContactRelation ("relation"))
				};
				ClassicAssert.AreEqual (1, contact.ContactRelations.Length, "ContactRelations");

				contact.ContactType = CNContactType.Organization;
				ClassicAssert.AreEqual (CNContactType.Organization, contact.ContactType, "ContactType");

				contact.Dates = new CNLabeledValue<NSDateComponents> [] {
					new CNLabeledValue<NSDateComponents> ("label", new NSDateComponents () {
						Month = 6
					})
				};
				ClassicAssert.AreEqual (1, contact.Dates.Length, "Dates");

				contact.DepartmentName = "department";
				ClassicAssert.AreEqual ("department", contact.DepartmentName, "DepartmentName");

				contact.EmailAddresses = new CNLabeledValue<NSString> [] {
					new CNLabeledValue<NSString> ("label", (NSString) "foo@bar.com")
				};
				ClassicAssert.AreEqual (1, contact.EmailAddresses.Length, "EmailAddresses");

				contact.FamilyName = "familyName";
				ClassicAssert.AreEqual ("familyName", contact.FamilyName, "FamilyName");

				contact.GivenName = "givenName";
				ClassicAssert.AreEqual ("givenName", contact.GivenName, "GivenName");

				ClassicAssert.AreNotEqual (string.Empty, contact.Identifier, "Identifier");

				contact.ImageData = new NSData ();
				ClassicAssert.IsNotNull (contact.ImageData, "ImageData-2");
				// iOS 10 (beta 1) fixed this bug (if not null then it's available)
				var avail = TestRuntime.CheckXcodeVersion (8, 0);
				Assert.That (contact.ImageDataAvailable, Is.EqualTo (avail), "ImageDataAvailable-2");

				contact.InstantMessageAddresses = new CNLabeledValue<CNInstantMessageAddress> [] {
					new CNLabeledValue<CNInstantMessageAddress> ("label", new CNInstantMessageAddress ("user", "service")),
				};
				ClassicAssert.AreEqual (1, contact.InstantMessageAddresses.Length, "InstantMessageAddresses");

				contact.JobTitle = "title";
				ClassicAssert.AreEqual ("title", contact.JobTitle, "JobTitle");

				contact.MiddleName = "middleName";
				ClassicAssert.AreEqual ("middleName", contact.MiddleName, "MiddleName");

				contact.NamePrefix = "namePrefix";
				ClassicAssert.AreEqual ("namePrefix", contact.NamePrefix, "NamePrefix");

				contact.NameSuffix = "nameSuffix";
				ClassicAssert.AreEqual ("nameSuffix", contact.NameSuffix, "NameSuffix");

				contact.Nickname = "nickname";
				ClassicAssert.AreEqual ("nickname", contact.Nickname, "Nickname");

				contact.NonGregorianBirthday = new NSDateComponents () {
					Year = 2099,
				};
				ClassicAssert.AreEqual ((nint) 2099, contact.NonGregorianBirthday.Year, "NonGregorianBirthday");

				contact.Note = "note";
				ClassicAssert.AreEqual ("note", contact.Note, "Note");

				contact.OrganizationName = "organizationName";
				ClassicAssert.AreEqual ("organizationName", contact.OrganizationName, "OrganizationName");

				contact.PhoneNumbers = new CNLabeledValue<CNPhoneNumber> [] {
					new CNLabeledValue<CNPhoneNumber> ("label", new CNPhoneNumber ("123-345-456"))
				};
				ClassicAssert.AreEqual (1, contact.PhoneNumbers.Length, "PhoneNumbers");

				contact.PhoneticFamilyName = "phoneticFamilyName";
				ClassicAssert.AreEqual ("phoneticFamilyName", contact.PhoneticFamilyName, "PhoneticFamilyName");

				contact.PhoneticGivenName = "phoneticGivenName";
				ClassicAssert.AreEqual ("phoneticGivenName", contact.PhoneticGivenName, "PhoneticGivenName");

				contact.PhoneticMiddleName = "phoneticMiddleName";
				ClassicAssert.AreEqual ("phoneticMiddleName", contact.PhoneticMiddleName, "PhoneticMiddleName");

				contact.PostalAddresses = new CNLabeledValue<CNPostalAddress> [] {
					new CNLabeledValue<CNPostalAddress> ("label", new CNMutablePostalAddress ()
						{
							Street = "my Street",
						})
				};
				ClassicAssert.AreEqual (1, contact.PostalAddresses.Length, "PostalAddresses");

				contact.PreviousFamilyName = "previousFamilyName";
				ClassicAssert.AreEqual ("previousFamilyName", contact.PreviousFamilyName, "PreviousFamilyName");

				contact.SocialProfiles = new CNLabeledValue<CNSocialProfile> [] {
					new CNLabeledValue<CNSocialProfile> ("label", new CNSocialProfile ("url", "username", "useridentifier", "service"))
				};
				ClassicAssert.AreEqual (1, contact.SocialProfiles.Length, "SocialProfiles");

				contact.UrlAddresses = new CNLabeledValue<NSString> [] {
					new CNLabeledValue<NSString> ("label", (NSString) "url@address.com")
				};
				ClassicAssert.AreEqual (1, contact.UrlAddresses.Length, "UrlAddresses");
			}
		}
	}
}

#endif // !__TVOS__
