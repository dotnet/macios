#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSPathControlItemTests {
		[Test]
		public void NSPathControlItemShouldSetTitle ()
		{
			var item = new NSPathControlItem ();
			var title = item.Title;
			item.Title = "Test";

			Assert.That (item.Title != title, Is.True, "NSPathControlShouldSetTitle - Title value did not change.");
		}

		[Test]
		public void NSPathControlItemShouldSetAttributedTitle ()
		{
			var item = new NSPathControlItem ();
			var attributedTitle = item.AttributedTitle;
			item.AttributedTitle = new NSAttributedString ("Test");

			Assert.That (item.AttributedTitle != attributedTitle, Is.True, "NSPathControlShouldSetAttributedTitle - AttributedTitle value did not change.");

		}

		[Test]
		public void NSPathControlItemShouldSetImage ()
		{
			var item = new NSPathControlItem ();
			Assert.That (item.Image is null, Is.True, "NSPathControlItemShouldSetImage - Image did not start as null");

			item.Image = new NSImage ();
			Assert.That (item.Image is not null, Is.True, "NSPathControlItemShouldSetImage - Failed to set Image property");
		}
	}
}

#endif // __MACOS__
