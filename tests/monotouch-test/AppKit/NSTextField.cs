#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTextFieldTests {
		NSTextField textField;

		[SetUp]
		public void SetUp ()
		{
			textField = new NSTextField ();
		}

		[Test]
		public void NSTextFieldShouldChangePlaceholderString ()
		{
			var placeholder = textField.PlaceholderString;
			textField.PlaceholderString = "Test";

			Assert.That (textField.PlaceholderString == placeholder, Is.False, "NSTextFieldShouldChangePlaceholderString - Failed to set the PlaceholderString property");
		}

		[Test]
		public void NSTextFieldShouldChangePlaceholderAttributedString ()
		{
			var placeholder = textField.PlaceholderAttributedString;
			textField.PlaceholderAttributedString = new NSAttributedString ("Test");

			Assert.That (textField.PlaceholderAttributedString == placeholder, Is.False, "NSTextFieldShouldChangePlaceholderAttributedString - Failed to set the PlaceholderAttributedString property");
		}
	}
}
#endif // __MACOS__
