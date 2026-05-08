#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTextViewTests {
		NSTextView view;

		[SetUp]
		public void SetUp ()
		{
			view = new NSTextView ();
		}

		[Test]
		public void NSTextViewShouldChangeUsesRolloverButtonForSelection ()
		{
			var usesRollover = view.UsesRolloverButtonForSelection;
			view.UsesRolloverButtonForSelection = !usesRollover;

			Assert.That (view.UsesRolloverButtonForSelection == usesRollover, Is.False, "NSTextViewShouldChangeUsesRolloverButtonForSelection - Failed to set the UsesRolloverButtonForSelection property");
		}
	}
}
#endif // __MACOS__
