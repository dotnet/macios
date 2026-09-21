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
				Assert.That (pd.Name, Is.Null, "An unset Name should be null");
				pd.Name = "Name";
				Assert.That (pd.Name, Is.EqualTo ("Name"), "Name was not corretly set.");
			}
		}

		[Test]
		public void GetSetOpcional ()
		{
			using (var pd = new NSPropertyDescription ()) {
				Assert.That (pd.Optional, Is.True, "A property should be Optional as default.");
				pd.Optional = false;
				Assert.That (pd.Optional, Is.False, "Optional was not correctly set.");
			}
		}

		[Test]
		public void GetSetTransient ()
		{
			using (var pd = new NSPropertyDescription ()) {
				Assert.That (pd.Transient, Is.False, "A property should not be Transient by default.");
				pd.Transient = true;
				Assert.That (pd.Transient, Is.True, "Transient was not correctly set.");
			}
		}

		[Test]
		public void GetSetRenamingIdentifier ()
		{
			using (var pd = new NSPropertyDescription ()) {
				Assert.That (pd.RenamingIdentifier, Is.Null, "A property by default should have the RenamingIndentifier set to null");
				pd.RenamingIdentifier = "Foo";
				Assert.That (pd.RenamingIdentifier, Is.EqualTo ("Foo"), "RenamingIdentifier was not correctly set.");
			}
		}
	}
}
