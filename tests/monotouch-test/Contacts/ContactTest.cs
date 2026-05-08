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
	public class ContactTest {

		[SetUp]
		public void MinimumSdkCheck ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void DescriptorForAllComparatorKeys ()
		{
			var keys = CNContact.GetDescriptorForAllComparatorKeys ();
			// while most input for ICNKeyDescriptor are done with NSString
			// the output is opaque and an internal type
			// note: this is not very robust - but I want to know if this changes during the next betas
			Assert.That (keys.Description.StartsWith ("<CNAggregateKeyDescriptor:", StringComparison.Ordinal), Is.True, "type");
			Assert.That (keys.Description.Contains (" kind=Formatter "), Is.True, "kind");
			Assert.That (keys.Description.Contains (" style: 100"), Is.True, "style"); // 1002 before iOS 10, 1003 after
		}

		[Test]
		public void Ctor ()
		{
			using (var contact = new CNContact ()) {
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
			}
		}
	}
}

#endif // !__TVOS__
