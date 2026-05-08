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

			ClassicAssert.IsTrue (item.Title != title, "NSPathControlShouldSetTitle - Title value did not change.");
		}

		[Test]
		public void NSPathControlItemShouldSetAttributedTitle ()
		{
			var item = new NSPathControlItem ();
			var attributedTitle = item.AttributedTitle;
			item.AttributedTitle = new NSAttributedString ("Test");

			ClassicAssert.IsTrue (item.AttributedTitle != attributedTitle, "NSPathControlShouldSetAttributedTitle - AttributedTitle value did not change.");

		}

		[Test]
		public void NSPathControlItemShouldSetImage ()
		{
			var item = new NSPathControlItem ();
			ClassicAssert.IsTrue (item.Image is null, "NSPathControlItemShouldSetImage - Image did not start as null");

			item.Image = new NSImage ();
			ClassicAssert.IsTrue (item.Image is not null, "NSPathControlItemShouldSetImage - Failed to set Image property");
		}
	}
}

#endif // __MACOS__
