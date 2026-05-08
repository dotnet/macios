using CoreData;

namespace MonoTouchFixtures.CoreData {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PropertyDescriptionTest {

		[Test]
		public void WeakFramework ()
		{
			var pd = new NSPropertyDescription ();
			Assert.That (pd.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			// if CoreData is not linked then all related objects handle will be null
		}

		[Test]
		public void GetSetName ()
		{
			using (var pd = new NSPropertyDescription ()) {
				ClassicAssert.IsNull (pd.Name, "An unset Name should be null");
				pd.Name = "Name";
				ClassicAssert.AreEqual ("Name", pd.Name, "Name was not corretly set.");
			}
		}

		[Test]
		public void GetSetOpcional ()
		{
			using (var pd = new NSPropertyDescription ()) {
				ClassicAssert.IsTrue (pd.Optional, "A property should be Optional as default.");
				pd.Optional = false;
				ClassicAssert.IsFalse (pd.Optional, "Optional was not correctly set.");
			}
		}

		[Test]
		public void GetSetTransient ()
		{
			using (var pd = new NSPropertyDescription ()) {
				ClassicAssert.IsFalse (pd.Transient, "A property should not be Transient by default.");
				pd.Transient = true;
				ClassicAssert.IsTrue (pd.Transient, "Transient was not correctly set.");
			}
		}

		[Test]
		public void GetSetRenamingIdentifier ()
		{
			using (var pd = new NSPropertyDescription ()) {
				ClassicAssert.IsNull (pd.RenamingIdentifier,
							   "A property by default should have the RenamingIndentifier set to null");
				pd.RenamingIdentifier = "Foo";
				ClassicAssert.AreEqual ("Foo", pd.RenamingIdentifier,
								 "RenamingIdentifier was not correctly set.");
			}
		}
	}
}
