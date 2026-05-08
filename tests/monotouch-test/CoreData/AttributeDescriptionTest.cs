//
// Unit tests for NSAttributeDescriptionTest
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2011-2012 Xamarin Inc. All rights reserved.
//

using CoreData;

namespace MonoTouchFixtures.CoreData {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AttributeDescription {

		[Test]
		public void WeakFramework ()
		{
			NSAttributeDescription ad = new NSAttributeDescription ();
			Assert.That (ad.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			// if CoreData is not linked then all related objects handle will be null
		}

		[Test]
		public void DefaultValue ()
		{
			using (var ad = new NSAttributeDescription ())
			using (var o = new NSObject ()) {
				ad.DefaultValue = o;
				ClassicAssert.AreSame (o, ad.DefaultValue, "DefaultValue");
			}
		}

		[Test]
		public void GetSetRenamingIdentifier ()
		{
			using (var ad = new NSAttributeDescription ()) {
				ClassicAssert.IsNull (ad.RenamingIdentifier, "An unset RenamingIdentifier should be null.");
				ad.RenamingIdentifier = "Foo";
				ClassicAssert.AreEqual ("Foo", ad.RenamingIdentifier,
								 "RenamingIndentifier was not corrently set.");
			}
		}
	}
}
