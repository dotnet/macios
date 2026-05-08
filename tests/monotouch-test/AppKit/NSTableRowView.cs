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

			ClassicAssert.IsFalse (view.PreviousRowSelected == selected, "NSTableRowViewShouldChangePreviousRowSelected - Failed to set the PreviousRowSelected property");
		}

		[Test]
		public void NSTableRowViewShouldChangeNextRowSelected ()
		{
			var selected = view.NextRowSelected;
			view.NextRowSelected = !selected;

			ClassicAssert.IsFalse (view.NextRowSelected == selected, "NSTableRowViewShouldChangeNextRowSelected - Failed to set the NextRowSelected property");
		}
	}
}

#endif // __MACOS__
