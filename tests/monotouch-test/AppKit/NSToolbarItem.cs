#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSToolbarItemTests {
		[Test]
		public void InitTests ()
		{
			const string TestLabel = "NSToolbarItemTests.Label";
			NSToolbarItem item = new NSToolbarItem ();
			Assert.That (item.Handle, Is.Not.Null, "NSToolbarItem has handle");
			item.Label = TestLabel;
			Assert.That (TestLabel, Is.EqualTo (item.Label), "NSToolbarItem has non null Label");

			NSToolbarItemGroup group = new NSToolbarItemGroup ();
			Assert.That (group.Handle, Is.Not.Null, "NSToolbarItemGroup has handle");
			Assert.That (group.Subitems.Length, Is.EqualTo (0), "NSToolbarItemGroup has zero items");
			group.Label = TestLabel;
			Assert.That (TestLabel, Is.EqualTo (group.Label), "NSToolbarItemGroup has non null Label");
		}
	}
}
#endif // __MACOS__
