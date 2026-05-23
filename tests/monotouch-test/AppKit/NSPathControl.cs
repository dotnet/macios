#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSPathControlTests {
		[Test]
		public void NSPathControlShouldSetEditable ()
		{
			var control = new NSPathControl ();
			var editable = control.Editable;
			control.Editable = !editable;

			Assert.That (control.Editable != editable, Is.True, "NSPathControlShouldSetEditable - Failed to change the Editable property");
		}

		[Test]
		public void NSPathControlShouldSetAllowedTypes ()
		{
			var control = new NSPathControl ();
			var allowedTypes = control.AllowedTypes;
			control.AllowedTypes = new [] { (NSString) "exe", (NSString) "jpg" };

			Assert.That (control.AllowedTypes != allowedTypes, Is.True, "NSPathControlShouldSetAllowedTypes - Failed to change AllowedTypes property");
		}

		[Test]
		public void NSPathControlShouldSetPlaceholderString ()
		{
			var control = new NSPathControl ();
			var placeholderString = control.PlaceholderString;
			control.PlaceholderString = "Test Placeholder";

			Assert.That (control.PlaceholderString != placeholderString, Is.True, "NSPathControlShouldSetPlaceholderString - Failed to change PlaceholderString property");
		}

		[Test]
		public void NSPathControlShouldSetPlaceholderAttributedString ()
		{
			var control = new NSPathControl ();
			var placeholderAttributedString = control.PlaceholderAttributedString;
			control.PlaceholderAttributedString = new NSAttributedString ("Test Placeholder");

			Assert.That (control.PlaceholderAttributedString != placeholderAttributedString, Is.True, "NSPathControlShouldSetPlaceholderAttributedString - Failed to change PlaceholderAttributedString property");
		}

		[Test]
		public void NSPathControlShouldSetPathItems ()
		{
			var control = new NSPathControl ();
			var pathItems = control.PathItems;
			control.PathItems = new [] { new NSPathControlItem () };

			Assert.That (control.PathItems != pathItems, Is.True, "NSPathControlShouldSetPathItems - Failed to set PathItems property");
		}
	}
}
#endif // __MACOS__
