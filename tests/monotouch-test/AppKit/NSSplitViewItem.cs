#if __MACOS__
using System.Linq;

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSSplitViewItemTests {
		NSSplitViewItem item;

		[SetUp]
		public void SetUp ()
		{
			item = new NSSplitViewItem ();
		}

		[Test]
		public void NSSplitViewItemShouldCreateFromViewController ()
		{
			var viewController = new NSViewController ();
			var splitViewItem = NSSplitViewItem.FromViewController (viewController);

			Assert.That (splitViewItem is null, Is.False, "NSSplitViewItemShouldCreateFromViewController - Returned null");
			Assert.That (splitViewItem.ViewController == viewController, Is.True, "NSSplitViewItemShouldCreateFromViewController - ViewController property not set correctly");
		}

		[Test]
		public void NSSplitViewItemShouldChangeViewController ()
		{
			var viewController = item.ViewController;
			item.ViewController = new NSViewController ();

			Assert.That (item.ViewController == viewController, Is.False, "NSSplitViewItemShouldChangeViewController - Failed to set the ViewController property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeCollapsed ()
		{
			var collapsed = item.Collapsed;
			item.Collapsed = !collapsed;

			Assert.That (item.Collapsed == collapsed, Is.False, "NSSplitViewItemShouldChangeCollapsed - Failed to set the Collapsed property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeCanCollapse ()
		{
			var canCollapse = item.CanCollapse;
			item.CanCollapse = !canCollapse;

			Assert.That (item.CanCollapse == canCollapse, Is.False, "NSSplitViewItemShouldChangeCanCollapse - Failed to set the CanCollapse property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeHoldingPriority ()
		{
			var holdingPriority = item.HoldingPriority;
			item.HoldingPriority = 0.35f;

			Assert.That (item.HoldingPriority == holdingPriority, Is.False, "NSSplitViewItemShouldChangeHoldingPriority - Failed to set the HoldingPriority property");
		}
	}
}
#endif // __MACOS__
