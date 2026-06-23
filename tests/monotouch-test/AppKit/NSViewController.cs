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

			Assert.That (controller.ChildViewControllers.Length == 1, Is.True, "NSViewControllerShouldAddChildViewControllers - Failed to add child view controller");
		}

		[Test]
		public void NSViewControllerShouldRemoveChildViewController ()
		{
			var child = new NSViewController ();
			controller.AddChildViewController (child);

			Assert.That (controller.ChildViewControllers.Length == 1, Is.True, "NSViewControllerShouldRemoveChildViewControllers - Failed to add child view controller");

			controller.RemoveChildViewController (0);

			Assert.That (controller.ChildViewControllers.Length == 0, Is.True, "NSViewControllerShouldRemoveChildViewController - Failed to remove child view controller");
		}

		[Test]
		public void NSViewControllerShouldInsertChildViewController ()
		{
			controller.AddChildViewController (new NSViewController ());
			controller.AddChildViewController (new NSViewController ());

			Assert.That (controller.ChildViewControllers.Length == 2, Is.True, "NSViewControllerShouldInsertChildViewController - Failed to add child view controller");

			var child = new NSViewController ();
			controller.InsertChildViewController (child, 1);

			Assert.That (controller.ChildViewControllers.Length == 3, Is.True, "NSViewControllerShouldInsertChildViewController - Failed to insert child view controller");
			Assert.That (controller.ChildViewControllers [1] == child, Is.True, "NSViewControllerShouldInsertChildViewController - Inserted child view controller at the wrong index.");
		}
	}
}
#endif // __MACOS__
