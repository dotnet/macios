#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSUserDefaultsControllerTests {
		NSUserDefaultsController controller;

		[Test]
		public void NSUserDefaultsControllerShouldGetSharedController ()
		{
			controller = NSUserDefaultsController.SharedUserDefaultsController;

			Assert.That (controller, Is.Not.Null, "NSUserDefaultsControllerShouldGetDefaultController - SharedUserDefaultsController returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor ()
		{
			controller = new NSUserDefaultsController ();

			Assert.That (controller, Is.Not.Null, "NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters ()
		{
			controller = new NSUserDefaultsController (null, null);

			Assert.That (controller.Defaults, Is.EqualTo (NSUserDefaults.StandardUserDefaults));
			Assert.That (controller.InitialValues, Is.Null);
			Assert.That (controller, Is.Not.Null, "NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithParameters ()
		{
			var initialValues = new NSDictionary ();
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, initialValues);

			Assert.That (controller.Defaults, Is.EqualTo (NSUserDefaults.StandardUserDefaults));
			Assert.That (controller.InitialValues, Is.EqualTo (initialValues));
			Assert.That (controller, Is.Not.Null, "NSUserDefaultsControllerShouldCreateNewControllerWithParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeInitialValues ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var initialValues = controller.InitialValues;
			controller.InitialValues = new NSDictionary ();

			Assert.That (controller.InitialValues, Is.Not.EqualTo (initialValues), "NSUserDefaultsControllerShouldChangeInitialValues - Failed to set the InitialValues property");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeAppliesImmediately ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var appliesImmediately = controller.AppliesImmediately;
			controller.AppliesImmediately = !appliesImmediately;

			Assert.That (controller.AppliesImmediately, Is.Not.EqualTo (appliesImmediately), "NSUserDefaultsControllerShouldChangeAppliesImmediately - Failed to set the AppliesImmediately property");
		}
	}
}
#endif // __MACOS__
