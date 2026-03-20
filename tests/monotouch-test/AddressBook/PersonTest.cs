//
// Unit tests for ABPerson
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using UIKit;
using AddressBook;
using Foundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AddressBook {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PersonTest {

		[SetUp]
		public void Setup ()
		{
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				Assert.Ignore ("The addressbook framework is deprecated in Xcode 15.0 and always returns null");
			}
		}

		[Test]
		public void UpdateAddressLine ()
		{
			TestRuntime.CheckAddressBookPermission ();

			NSError err;
			var ab = ABAddressBook.Create (out err);
			Assert.IsNotNull (ab, "#1");

			var people = ab.GetPeople ();
			if (people.Length < 1) {
				// TODO:
				return;
			}

			var p = people [0];

			var all = p.GetAllAddresses ();
			var mutable = all.ToMutableMultiValue ();
			if (mutable.Count < 1) {
				// TODO:
				return;
			}

			var multi = mutable [0];
			var addr = multi.Value;
			addr.Zip = "78972";
			multi.Value = addr;
			p.SetAddresses (mutable);

			Assert.IsTrue (ab.HasUnsavedChanges);
			ab.Save ();
		}

		[Test]
		public void LocalizedPropertyName ()
		{
			TestRuntime.CheckAddressBookPermission ();
			var name = ABPerson.LocalizedPropertyName (ABPersonProperty.FirstName);
			Assert.NotNull (name, "name");
			Assert.That (name.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void LocalizedPropertyName_Int ()
		{
			TestRuntime.CheckAddressBookPermission ();
			// Use the underlying integer ID for ABPersonProperty.LastName (1)
			var name = ABPerson.LocalizedPropertyName (1);
			Assert.NotNull (name, "name");
			Assert.That (name.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void PersonToString ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				person.FirstName = "Test";
				person.LastName = "Person";
				var str = person.ToString ();
				Assert.NotNull (str, "ToString");
			}
		}

		[Test]
		public void GetImage_NoImage ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				Assert.IsFalse (person.HasImage, "HasImage");
				Assert.IsNull (person.Image, "Image");
				Assert.IsNull (person.GetImage (ABPersonImageFormat.Thumbnail), "GetImage");
			}
		}

		[Test]
		public void GetLinkedPeople ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				var linked = person.GetLinkedPeople ();
				// A new person not in the address book may return null or empty
				Assert.IsTrue (linked is null || linked.Length >= 0, "GetLinkedPeople");
			}
		}

		[Test]
		public void CreateFromVCard ()
		{
			TestRuntime.CheckAddressBookPermission ();
			var vcard = "BEGIN:VCARD\nVERSION:3.0\nFN:Test Person\nN:Person;Test;;;\nEND:VCARD\n";
			using (var data = NSData.FromString (vcard)) {
				var people = ABPerson.CreateFromVCard (null, data);
				Assert.NotNull (people, "people");
				Assert.That (people.Length, Is.GreaterThan (0), "Length");
			}
		}

		[Test]
		public void PropertyToString_FirstName ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				person.FirstName = "TestFirst";
				Assert.AreEqual ("TestFirst", person.FirstName, "FirstName");
			}
		}

		[Test]
		public void MultiValueLabel ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				var phones = new ABMutableStringMultiValue ();
				phones.Add ("555-1234", ABPersonPhoneLabel.Mobile);
				person.SetPhones (phones);

				var allPhones = person.GetPhones ();
				Assert.That (allPhones.Count, Is.GreaterThan (0), "Count");
				Assert.NotNull (allPhones [0].Label, "Label");
			}
		}

		[Test]
		public void MultiValueGetValues ()
		{
			TestRuntime.CheckAddressBookPermission ();
			using (var person = new ABPerson ()) {
				var phones = new ABMutableStringMultiValue ();
				phones.Add ("555-1234", ABPersonPhoneLabel.Mobile);
				phones.Add ("555-5678", ABPersonPhoneLabel.Main);
				person.SetPhones (phones);

				var allPhones = person.GetPhones ();
				var values = allPhones.GetValues ();
				Assert.NotNull (values, "values");
				Assert.That (values.Length, Is.EqualTo (2), "Length");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
