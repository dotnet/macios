#if __MACOS__
using System.Linq;

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSSplitViewControllerTests {
		NSSplitViewController controller;

		[SetUp]
		public void SetUp ()
		{
			controller = new NSSplitViewController ();
		}

		[Test]
		public void NSSplitViewControllerShouldChangeSplitView ()
		{
			var splitView = controller.SplitView;
			controller.SplitView = new NSSplitView ();

			Assert.That (controller.SplitView == splitView, Is.False, "NSSplitViewControllerShouldChangeSplitView - Failed to set the SplitView property");
		}

		[Test]
		public void NSSplitViewControllerShouldChangeSplitViewItems ()
		{
			var items = controller.SplitViewItems;
			controller.SplitViewItems = new NSSplitViewItem [] { new NSSplitViewItem { ViewController = new NSViewController () } };

			Assert.That (controller.SplitViewItems == items, Is.False, "NSSplitViewControllerShouldChangeSplitViewItems - Failed to set the SplitViewItems property");
		}

		[Test]
		public void NSSplitViewControllerShouldAddSplitViewItem ()
		{
			var item = new NSSplitViewItem { ViewController = new NSViewController () };
			controller.AddSplitViewItem (item);

			Assert.That (controller.SplitViewItems.Contains (item), Is.True, "NSSplitViewControllerShouldAddSplitViewItem - Failed to add item");
		}

		[Test]
		public void NSSplitViewControllerShouldRemoveSplitViewItem ()
		{
			var item = new NSSplitViewItem { ViewController = new NSViewController () };
			controller.AddSplitViewItem (item);

			Assert.That (controller.SplitViewItems.Contains (item), Is.True, "NSSplitViewControllerShouldRemoveSplitViewItem - Failed to add item");

			controller.RemoveSplitViewItem (item);

			Assert.That (controller.SplitViewItems.Contains (item), Is.False, "NSSplitViewControllerShouldRemoveSplitViewItem - Failed to remove item");
		}

		[Test]
		public void NSSplitViewControllerShouldInsertSplitViewItem ()
		{
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			var item = new NSSplitViewItem { ViewController = new NSViewController () };
			controller.InsertSplitViewItem (item, 1);

			Assert.That (controller.SplitViewItems [1] == item, Is.True, "NSSplitViewControllerShouldInsertSplitViewItem - Failed to insert the item at the given position.");
			Assert.That (controller.SplitViewItems [0] == item, Is.False, "NSSplitViewControllerShouldInsertSplitViewItem - Inserted the item in the wrong position.");
		}

		[Test]
		public void NSSplitViewControllerShouldGetSplitViewItem ()
		{
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			controller.AddSplitViewItem (new NSSplitViewItem { ViewController = new NSViewController () });
			var viewController = new NSViewController ();
			var item = new NSSplitViewItem { ViewController = viewController };
			controller.InsertSplitViewItem (item, 1);

			var retrievedItem = controller.GetSplitViewItem (viewController);

			Assert.That (retrievedItem == item, Is.True, "NSSplitViewControllerShouldGetSplitViewItem - Failed to get SplitViewItem from ViewController");
		}
	}
}
#endif // __MACOS__
