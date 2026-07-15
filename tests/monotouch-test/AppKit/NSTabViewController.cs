#if __MACOS__
using System.Linq;

using AppKit;
using Xamarin.Utils;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTabViewControllerTests {
		NSTabViewController controller;

		[SetUp]
		public void SetUp ()
		{
			controller = new NSTabViewController ();
		}

		[Test]
		public void NSTabViewControllerShouldChangeTabStyle ()
		{
			var tabStyle = controller.TabStyle;
			controller.TabStyle = NSTabViewControllerTabStyle.Toolbar;

			Assert.That (controller.TabStyle == tabStyle, Is.False, "NSTabViewControllerShouldChangeTabStyle - Failed to set the TabStyle property");
		}

		//		[Test]
		//		public void NSTabViewControllerShouldChangeTabView ()
		//		{
		//			var tabView = controller.TabView;
		//			controller.TabView = new NSTabView ();
		//
		//			Assert.That (controller.TabView == tabView, Is.False, "NSTabViewControllerShouldChangeTabView - Failed to set the TabView property");
		//		}

		[Test]
		public void NSTabViewControllerShouldChangeTransitionOptions ()
		{
			var options = controller.TransitionOptions;
			controller.TransitionOptions = NSViewControllerTransitionOptions.Crossfade | NSViewControllerTransitionOptions.SlideRight;

			Assert.That (controller.TransitionOptions == options, Is.False, "NSTabViewControllerShouldChangeTransitionOptions - Failed to set the TransitionOptions property");
		}

		[Test]
		public void NSTabViewControllerShouldChangeCanPropagateSelectedChildViewControllerTitle ()
		{
			var canPropogate = controller.CanPropagateSelectedChildViewControllerTitle;
			controller.CanPropagateSelectedChildViewControllerTitle = !canPropogate;

			Assert.That (controller.CanPropagateSelectedChildViewControllerTitle == canPropogate, Is.False, "NSTabViewControllerShouldChangeCanPropagateSelectedChildViewControllerTitle - Failed to set the CanPropagateSelectedChildViewControllerTitle property");
		}

		[Test]
		public void NSTabViewControllerShouldChangeTabViewItems ()
		{
			var items = controller.TabViewItems;
			controller.TabViewItems = new NSTabViewItem [] { new NSTabViewItem { ViewController = new NSViewController () } };

			Assert.That (controller.TabViewItems == items, Is.False, "NSTabViewControllerShouldChangeTabViewItems - Failed to set the TabViewItems property");
		}

		[Test]
		public void NSTabViewControllerShouldChangeSelectedTabViewItemIndex ()
		{
			controller.TabViewItems = new NSTabViewItem [] {
				new NSTabViewItem { ViewController = new NSViewController () },
				new NSTabViewItem { ViewController = new NSViewController () },
				new NSTabViewItem { ViewController = new NSViewController () }
			};

			var index = controller.SelectedTabViewItemIndex;
			controller.SelectedTabViewItemIndex = (index + 1) % 3;

			Assert.That (controller.SelectedTabViewItemIndex == index, Is.False, "NSTabViewControllerShouldChangeSelectedTabViewItemIndex - Failed to set the SelectedTabViewItemIndex property");
		}

		[Test]
		public void NSTabViewControllerShouldAddTabViewItem ()
		{
			var item = new NSTabViewItem { ViewController = new NSViewController () };
			controller.AddTabViewItem (item);

			Assert.That (controller.TabViewItems.Contains (item), Is.True, "NSTabViewControllerShouldAddTabViewItem - Failed to add TabViewItem");
		}

		[Test]
		public void NSTabViewControllerShouldRemoveTabViewItem ()
		{
			var item = new NSTabViewItem { ViewController = new NSViewController () };
			controller.AddTabViewItem (item);

			Assert.That (controller.TabViewItems.Contains (item), Is.True, "NSTabViewControllerShouldRemoveTabViewItem - Failed to add item");

			controller.RemoveTabViewItem (item);

			Assert.That (controller.TabViewItems.Contains (item), Is.False, "NSTabViewControllerShouldRemoveTabViewItem - Failed to remove item");
		}

		[Test]
		public void NSTabViewControllerShouldInsertTabViewItem ()
		{
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			var item = new NSTabViewItem { ViewController = new NSViewController () };
			controller.InsertTabViewItem (item, 1);

			Assert.That (controller.TabViewItems [1] == item, Is.True, "NSTabViewControllerShouldInsertTabViewItem - Failed to insert the item at the given position.");
			Assert.That (controller.TabViewItems [0] == item, Is.False, "NSTabViewControllerShouldInsertTabViewItem - Inserted the item in the wrong position.");
		}

		[Test]
		public void NSTabViewControllerShouldGetTabViewItem ()
		{
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			controller.AddTabViewItem (new NSTabViewItem { ViewController = new NSViewController () });
			var viewController = new NSViewController ();
			var item = new NSTabViewItem { ViewController = viewController };
			controller.InsertTabViewItem (item, 1);

			var retrievedItem = controller.GetTabViewItem (viewController);

			Assert.That (retrievedItem == item, Is.True, "NSTabViewControllerShouldGetTabViewItem - Failed to get TabViewItem from ViewController");
		}
	}
}
#endif // __MACOS__
