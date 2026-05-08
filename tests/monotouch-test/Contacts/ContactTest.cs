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
			ClassicAssert.True (keys.Description.StartsWith ("<CNAggregateKeyDescriptor:", StringComparison.Ordinal), "type");
			ClassicAssert.True (keys.Description.Contains (" kind=Formatter "), "kind");
			ClassicAssert.True (keys.Description.Contains (" style: 100"), "style"); // 1002 before iOS 10, 1003 after
		}

		[Test]
		public void Ctor ()
		{
			using (var contact = new CNContact ()) {
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
			}
		}
	}
}

#endif // !__TVOS__
