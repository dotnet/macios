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

			ClassicAssert.IsNotNull (controller, "NSUserDefaultsControllerShouldGetDefaultController - SharedUserDefaultsController returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor ()
		{
			controller = new NSUserDefaultsController ();

			ClassicAssert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithDefaultConstructor - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters ()
		{
			controller = new NSUserDefaultsController (null, null);

			ClassicAssert.IsTrue (controller.Defaults == NSUserDefaults.StandardUserDefaults);
			ClassicAssert.IsTrue (controller.InitialValues is null);
			ClassicAssert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithNullParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldCreateNewControllerWithParameters ()
		{
			var initialValues = new NSDictionary ();
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, initialValues);

			ClassicAssert.IsTrue (controller.Defaults == NSUserDefaults.StandardUserDefaults);
			ClassicAssert.IsTrue (controller.InitialValues == initialValues);
			ClassicAssert.IsNotNull (controller, "NSUserDefaultsControllerShouldCreateNewControllerWithParameters - Constructor returned null");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeInitialValues ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var initialValues = controller.InitialValues;
			controller.InitialValues = new NSDictionary ();

			ClassicAssert.IsFalse (controller.InitialValues == initialValues, "NSUserDefaultsControllerShouldChangeInitialValues - Failed to set the InitialValues property");
		}

		[Test]
		public void NSUserDefaultsControllerShouldChangeAppliesImmediately ()
		{
			controller = new NSUserDefaultsController (NSUserDefaults.StandardUserDefaults, null);
			var appliesImmediately = controller.AppliesImmediately;
			controller.AppliesImmediately = !appliesImmediately;

			ClassicAssert.IsFalse (controller.AppliesImmediately == appliesImmediately, "NSUserDefaultsControllerShouldChangeAppliesImmediately - Failed to set the AppliesImmediately property");
		}
	}
}
#endif // __MACOS__
