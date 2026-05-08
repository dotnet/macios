#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSViewControllerTests {
		NSViewController controller;

		[SetUp]
		public void SetUp ()
		{
			controller = new NSViewController ();
		}

		[Test]
		public void NSViewControllerShouldAddChildViewController ()
		{
			var child = new NSViewController ();
			controller.AddChildViewController (child);

			ClassicAssert.IsTrue (controller.ChildViewControllers.Length == 1, "NSViewControllerShouldAddChildViewControllers - Failed to add child view controller");
		}

		[Test]
		public void NSViewControllerShouldRemoveChildViewController ()
		{
			var child = new NSViewController ();
			controller.AddChildViewController (child);

			ClassicAssert.IsTrue (controller.ChildViewControllers.Length == 1, "NSViewControllerShouldRemoveChildViewControllers - Failed to add child view controller");

			controller.RemoveChildViewController (0);

			ClassicAssert.IsTrue (controller.ChildViewControllers.Length == 0, "NSViewControllerShouldRemoveChildViewController - Failed to remove child view controller");
		}

		[Test]
		public void NSViewControllerShouldInsertChildViewController ()
		{
			controller.AddChildViewController (new NSViewController ());
			controller.AddChildViewController (new NSViewController ());

			ClassicAssert.IsTrue (controller.ChildViewControllers.Length == 2, "NSViewControllerShouldInsertChildViewController - Failed to add child view controller");

			var child = new NSViewController ();
			controller.InsertChildViewController (child, 1);

			ClassicAssert.IsTrue (controller.ChildViewControllers.Length == 3, "NSViewControllerShouldInsertChildViewController - Failed to insert child view controller");
			ClassicAssert.IsTrue (controller.ChildViewControllers [1] == child, "NSViewControllerShouldInsertChildViewController - Inserted child view controller at the wrong index.");
		}
	}
}
#endif // __MACOS__
