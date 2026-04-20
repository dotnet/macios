#if __MACOS__

using AppKit;
using Foundation;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSToolbarTests {
		NSToolbar toolbar;

		[SetUp]
		public void SetUp ()
		{
			toolbar = new NSToolbar (NSToolbar.NSToolbarSeparatorItemIdentifier);
		}

		[Test]
		public void NSToolbarShouldChangeAllowsExtensionItems ()
		{
			var allows = toolbar.AllowsExtensionItems;
			toolbar.AllowsExtensionItems = !allows;

			Assert.IsFalse (toolbar.AllowsExtensionItems == allows, "NSToolbarShouldChangeAllowsExtensionItems - Failed to set the AllowsExtensionItems property");
		}

		[Test]
		public void ItemEventArgs ()
		{
			TestRuntime.AssertXcodeVersion (14, 0); // macOS 13.0

			using (var item = new NSToolbarItem ())
			using (var userInfo = new NSMutableDictionary ()) {
				userInfo.Add (NSToolbar.NSToolbarItemKey, item);

				using (var notification = NSNotification.FromName (NSToolbar.NSToolbarWillAddItemNotification, toolbar, userInfo)) {
					var args = new NSToolbarItemEventArgs (notification);
					var actual = args.Item;

					Assert.That (actual, Is.Not.Null, "WillAddItem");
					if (actual is null)
						return;

					Assert.That (actual.Handle, Is.EqualTo (item.Handle), "WillAddItemHandle");
				}

				using (var notification = NSNotification.FromName (NSToolbar.NSToolbarDidRemoveItemNotification, toolbar, userInfo)) {
					var args = new NSToolbarItemEventArgs (notification);
					var actual = args.Item;

					Assert.That (actual, Is.Not.Null, "DidRemoveItem");
					if (actual is null)
						return;

					Assert.That (actual.Handle, Is.EqualTo (item.Handle), "DidRemoveItemHandle");
				}
			}
		}
	}
}
#endif // __MACOS__
