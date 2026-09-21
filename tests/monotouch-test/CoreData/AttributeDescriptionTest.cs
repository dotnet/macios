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
				Assert.That (ad.DefaultValue, Is.SameAs (o), "DefaultValue");
			}
		}

		[Test]
		public void GetSetRenamingIdentifier ()
		{
			using (var ad = new NSAttributeDescription ()) {
				Assert.That (ad.RenamingIdentifier, Is.Null, "An unset RenamingIdentifier should be null.");
				ad.RenamingIdentifier = "Foo";
				Assert.That (ad.RenamingIdentifier, Is.EqualTo ("Foo"), "RenamingIndentifier was not corrently set.");
			}
		}
	}
}
