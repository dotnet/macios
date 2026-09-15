#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTableRowViewTests {
		NSTableRowView view;

		[SetUp]
		public void SetUp ()
		{
			view = new NSTableRowView ();
		}

		[Test]
		public void NSTableRowViewShouldChangePreviousRowSelected ()
		{
			var selected = view.PreviousRowSelected;
			view.PreviousRowSelected = !selected;

			Assert.That (view.PreviousRowSelected == selected, Is.False, "NSTableRowViewShouldChangePreviousRowSelected - Failed to set the PreviousRowSelected property");
		}

		[Test]
		public void NSTableRowViewShouldChangeNextRowSelected ()
		{
			var selected = view.NextRowSelected;
			view.NextRowSelected = !selected;

			Assert.That (view.NextRowSelected == selected, Is.False, "NSTableRowViewShouldChangeNextRowSelected - Failed to set the NextRowSelected property");
		}
	}
}

#endif // __MACOS__
