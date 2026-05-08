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
			ClassicAssert.IsNotNull (item.Handle, "NSToolbarItem has handle");
			item.Label = TestLabel;
			ClassicAssert.AreEqual (item.Label, TestLabel, "NSToolbarItem has non null Label");

			NSToolbarItemGroup group = new NSToolbarItemGroup ();
			ClassicAssert.IsNotNull (group.Handle, "NSToolbarItemGroup has handle");
			ClassicAssert.AreEqual (group.Subitems.Length, 0, "NSToolbarItemGroup has zero items");
			group.Label = TestLabel;
			ClassicAssert.AreEqual (group.Label, TestLabel, "NSToolbarItemGroup has non null Label");
		}
	}
}
#endif // __MACOS__
