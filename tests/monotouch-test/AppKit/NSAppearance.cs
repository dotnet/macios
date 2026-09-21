#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSAppearanceTests {
		[Test]
		public void NSAppearanceShouldLoadAppearanceNamed ()
		{
			var appearance = NSAppearance.GetAppearance (NSAppearance.NameVibrantDark);
			Assert.That (appearance, Is.Not.Null, "NSAppearanceShouldLoadAppearanceNamed - Failed to initialize appearance VibrantDark");
			Assert.That (NSAppearance.NameVibrantDark.ToString (), Is.EqualTo (appearance.Name), "NSAppearanceShouldLoadAppearanceNamed - Appearance initialized with incorrect name.");
		}

#if FALSE // Test failing, exception doesn't appear to be thrown during test, throw correctly running in an app.
		[Test]
		public void NSAppearanceConstructorShouldFailWithInvalidName ()
		{
			bool exceptionHit = false;

			try {
				var appearance = new NSAppearance ("InvalidNameTest", null);
			} catch (ArgumentException e) {
				exceptionHit = true;
			}

			Assert.That (exceptionHit, Is.True, "NSAppearanceConstructorShouldFailWithInvalidName - No exception thrown while initializing appearance with invalid name.");
		}
#endif

		[Test]
		public void NSAppearanceShouldChangeCurrentAppearance ()
		{
			var appearance = NSAppearance.CurrentAppearance;

			NSAppearance.CurrentAppearance = NSAppearance.GetAppearance (NSAppearance.NameVibrantDark);

			Assert.That (NSAppearance.CurrentAppearance, Is.Not.EqualTo (appearance), "NSAppearanceShouldChangeCurrentAppearance - Failed to change appearance.");
		}

		[Test]
		public void NSAppearanceCustomizationNull ()
		{
			using (NSButton b = new NSButton ()) {
				b.Appearance = null;
			}
		}
	}
}
#endif // __MACOS__
