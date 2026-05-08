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

			ClassicAssert.IsFalse (splitViewItem is null, "NSSplitViewItemShouldCreateFromViewController - Returned null");
			ClassicAssert.IsTrue (splitViewItem.ViewController == viewController, "NSSplitViewItemShouldCreateFromViewController - ViewController property not set correctly");
		}

		[Test]
		public void NSSplitViewItemShouldChangeViewController ()
		{
			var viewController = item.ViewController;
			item.ViewController = new NSViewController ();

			ClassicAssert.IsFalse (item.ViewController == viewController, "NSSplitViewItemShouldChangeViewController - Failed to set the ViewController property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeCollapsed ()
		{
			var collapsed = item.Collapsed;
			item.Collapsed = !collapsed;

			ClassicAssert.IsFalse (item.Collapsed == collapsed, "NSSplitViewItemShouldChangeCollapsed - Failed to set the Collapsed property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeCanCollapse ()
		{
			var canCollapse = item.CanCollapse;
			item.CanCollapse = !canCollapse;

			ClassicAssert.IsFalse (item.CanCollapse == canCollapse, "NSSplitViewItemShouldChangeCanCollapse - Failed to set the CanCollapse property");
		}

		[Test]
		public void NSSplitViewItemShouldChangeHoldingPriority ()
		{
			var holdingPriority = item.HoldingPriority;
			item.HoldingPriority = 0.35f;

			ClassicAssert.IsFalse (item.HoldingPriority == holdingPriority, "NSSplitViewItemShouldChangeHoldingPriority - Failed to set the HoldingPriority property");
		}
	}
}
#endif // __MACOS__
