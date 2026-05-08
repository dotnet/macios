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
				Assert.That (contact.Birthday, Is.Null, "Birthday");
				Assert.That (contact.ContactRelations.Length, Is.EqualTo (0), "ContactRelations");
				Assert.That (contact.ContactType, Is.EqualTo (CNContactType.Person), "ContactType");
				Assert.That (contact.Dates.Length, Is.EqualTo (0), "Dates");
				Assert.That (contact.DepartmentName, Is.EqualTo (string.Empty), "DepartmentName");
				Assert.That (contact.EmailAddresses.Length, Is.EqualTo (0), "EmailAddresses");
				Assert.That (contact.FamilyName, Is.EqualTo (string.Empty), "FamilyName");
				Assert.That (contact.GivenName, Is.EqualTo (string.Empty), "GivenName");
				Assert.That (contact.Identifier, Is.Not.EqualTo (string.Empty), "Identifier");
				Assert.That (contact.ImageData, Is.Null, "ImageData");
				Assert.That (contact.ImageDataAvailable, Is.False, "ImageDataAvailable");
				Assert.That (contact.InstantMessageAddresses.Length, Is.EqualTo (0), "InstantMessageAddresses");
				Assert.That (contact.JobTitle, Is.EqualTo (string.Empty), "JobTitle");
				Assert.That (contact.MiddleName, Is.EqualTo (string.Empty), "MiddleName");
				Assert.That (contact.NamePrefix, Is.EqualTo (string.Empty), "NamePrefix");
				Assert.That (contact.NameSuffix, Is.EqualTo (string.Empty), "NameSuffix");
				Assert.That (contact.Nickname, Is.EqualTo (string.Empty), "Nickname");
				Assert.That (contact.NonGregorianBirthday, Is.Null, "NonGregorianBirthday");
				Assert.That (contact.Note, Is.EqualTo (string.Empty), "Note");
				Assert.That (contact.OrganizationName, Is.EqualTo (string.Empty), "OrganizationName");
				Assert.That (contact.PhoneNumbers.Length, Is.EqualTo (0), "PhoneNumbers");
				Assert.That (contact.PhoneticFamilyName, Is.EqualTo (string.Empty), "PhoneticFamilyName");
				Assert.That (contact.PhoneticGivenName, Is.EqualTo (string.Empty), "PhoneticGivenName");
				Assert.That (contact.PhoneticMiddleName, Is.EqualTo (string.Empty), "PhoneticMiddleName");
				Assert.That (contact.PostalAddresses.Length, Is.EqualTo (0), "PostalAddresses");
				Assert.That (contact.PreviousFamilyName, Is.EqualTo (string.Empty), "PreviousFamilyName");
				Assert.That (contact.SocialProfiles.Length, Is.EqualTo (0), "SocialProfiles");
				Assert.That (contact.ThumbnailImageData, Is.Null, "ThumbnailImageData");
				Assert.That (contact.UrlAddresses.Length, Is.EqualTo (0), "UrlAddresses");

				contact.Birthday = new NSDateComponents () {
					Year = 1980
				};
				Assert.That (contact.Birthday.Year, Is.EqualTo ((nint) 1980), "Birthday");

				contact.ContactRelations = new CNLabeledValue<CNContactRelation> [] {
					new CNLabeledValue<CNContactRelation> ("label", new CNContactRelation ("relation"))
				};
				Assert.That (contact.ContactRelations.Length, Is.EqualTo (1), "ContactRelations");

				contact.ContactType = CNContactType.Organization;
				Assert.That (contact.ContactType, Is.EqualTo (CNContactType.Organization), "ContactType");

				contact.Dates = new CNLabeledValue<NSDateComponents> [] {
					new CNLabeledValue<NSDateComponents> ("label", new NSDateComponents () {
						Month = 6
					})
				};
				Assert.That (contact.Dates.Length, Is.EqualTo (1), "Dates");

				contact.DepartmentName = "department";
				Assert.That (contact.DepartmentName, Is.EqualTo ("department"), "DepartmentName");

				contact.EmailAddresses = new CNLabeledValue<NSString> [] {
					new CNLabeledValue<NSString> ("label", (NSString) "foo@bar.com")
				};
				Assert.That (contact.EmailAddresses.Length, Is.EqualTo (1), "EmailAddresses");

				contact.FamilyName = "familyName";
				Assert.That (contact.FamilyName, Is.EqualTo ("familyName"), "FamilyName");

				contact.GivenName = "givenName";
				Assert.That (contact.GivenName, Is.EqualTo ("givenName"), "GivenName");

				Assert.That (contact.Identifier, Is.Not.EqualTo (string.Empty), "Identifier");

				contact.ImageData = new NSData ();
				Assert.That (contact.ImageData, Is.Not.Null, "ImageData-2");
				// iOS 10 (beta 1) fixed this bug (if not null then it's available)
				var avail = TestRuntime.CheckXcodeVersion (8, 0);
				Assert.That (contact.ImageDataAvailable, Is.EqualTo (avail), "ImageDataAvailable-2");

				contact.InstantMessageAddresses = new CNLabeledValue<CNInstantMessageAddress> [] {
					new CNLabeledValue<CNInstantMessageAddress> ("label", new CNInstantMessageAddress ("user", "service")),
				};
				Assert.That (contact.InstantMessageAddresses.Length, Is.EqualTo (1), "InstantMessageAddresses");

				contact.JobTitle = "title";
				Assert.That (contact.JobTitle, Is.EqualTo ("title"), "JobTitle");

				contact.MiddleName = "middleName";
				Assert.That (contact.MiddleName, Is.EqualTo ("middleName"), "MiddleName");

				contact.NamePrefix = "namePrefix";
				Assert.That (contact.NamePrefix, Is.EqualTo ("namePrefix"), "NamePrefix");

				contact.NameSuffix = "nameSuffix";
				Assert.That (contact.NameSuffix, Is.EqualTo ("nameSuffix"), "NameSuffix");

				contact.Nickname = "nickname";
				Assert.That (contact.Nickname, Is.EqualTo ("nickname"), "Nickname");

				contact.NonGregorianBirthday = new NSDateComponents () {
					Year = 2099,
				};
				Assert.That (contact.NonGregorianBirthday.Year, Is.EqualTo ((nint) 2099), "NonGregorianBirthday");

				contact.Note = "note";
				Assert.That (contact.Note, Is.EqualTo ("note"), "Note");

				contact.OrganizationName = "organizationName";
				Assert.That (contact.OrganizationName, Is.EqualTo ("organizationName"), "OrganizationName");

				contact.PhoneNumbers = new CNLabeledValue<CNPhoneNumber> [] {
					new CNLabeledValue<CNPhoneNumber> ("label", new CNPhoneNumber ("123-345-456"))
				};
				Assert.That (contact.PhoneNumbers.Length, Is.EqualTo (1), "PhoneNumbers");

				contact.PhoneticFamilyName = "phoneticFamilyName";
				Assert.That (contact.PhoneticFamilyName, Is.EqualTo ("phoneticFamilyName"), "PhoneticFamilyName");

				contact.PhoneticGivenName = "phoneticGivenName";
				Assert.That (contact.PhoneticGivenName, Is.EqualTo ("phoneticGivenName"), "PhoneticGivenName");

				contact.PhoneticMiddleName = "phoneticMiddleName";
				Assert.That (contact.PhoneticMiddleName, Is.EqualTo ("phoneticMiddleName"), "PhoneticMiddleName");

				contact.PostalAddresses = new CNLabeledValue<CNPostalAddress> [] {
					new CNLabeledValue<CNPostalAddress> ("label", new CNMutablePostalAddress ()
						{
							Street = "my Street",
						})
				};
				Assert.That (contact.PostalAddresses.Length, Is.EqualTo (1), "PostalAddresses");

				contact.PreviousFamilyName = "previousFamilyName";
				Assert.That (contact.PreviousFamilyName, Is.EqualTo ("previousFamilyName"), "PreviousFamilyName");

				contact.SocialProfiles = new CNLabeledValue<CNSocialProfile> [] {
					new CNLabeledValue<CNSocialProfile> ("label", new CNSocialProfile ("url", "username", "useridentifier", "service"))
				};
				Assert.That (contact.SocialProfiles.Length, Is.EqualTo (1), "SocialProfiles");

				contact.UrlAddresses = new CNLabeledValue<NSString> [] {
					new CNLabeledValue<NSString> ("label", (NSString) "url@address.com")
				};
				Assert.That (contact.UrlAddresses.Length, Is.EqualTo (1), "UrlAddresses");
			}
		}
	}
}

#endif // !__TVOS__
