#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSControlTests {
		[Test]
		public void NSControlShouldChangeControlSize ()
		{
			var control = new NSButton ();
			var size = control.ControlSize;
			control.ControlSize = NSControlSize.Mini;

			ClassicAssert.IsFalse (size == control.ControlSize);
			ClassicAssert.IsTrue (control.ControlSize == NSControlSize.Mini);
		}

		[Test]
		public void NSControlShouldChangeHighlighted ()
		{
			var control = new NSButton ();
			var highlighted = control.Highlighted;
			control.Highlighted = !highlighted;

			ClassicAssert.IsFalse (highlighted == control.Highlighted);
		}

		[Test]
		public void NSControlShouldChangeLineBreakMode ()
		{
			var control = new NSButton ();
			var lineBreak = control.LineBreakMode;
			control.LineBreakMode = NSLineBreakMode.Clipping;

			ClassicAssert.IsTrue (control.LineBreakMode == NSLineBreakMode.Clipping);
			ClassicAssert.IsFalse (lineBreak == control.LineBreakMode);
		}

		[Test]
		public void NSControlShouldAddMultipleActivatedEventHandlers ()
		{
			var control = new NSButton ();

			int firstHitCount = 0;
			int secondHitCount = 0;

			control.Activated += (sender, e) => firstHitCount++;
			control.Activated += (sender, e) => secondHitCount++;

			control.PerformClick (control);

			ClassicAssert.IsTrue (firstHitCount == 1, "NSControlShouldAddMultipleActivatedEventHandlers - Did not call first EventHandler");
			ClassicAssert.IsTrue (secondHitCount == 1, "NSControlShouldAddMultipleActivatedEventHandlers - Did not call second EventHandler");
		}

		[Test]
		public void NSControlShouldRemoveAndAddActivatedEventHandlers ()
		{
			var control = new NSButton ();

			int firstHitCount = 0;
			int secondHitCount = 0;

			EventHandler firstDelegate = (object sender, EventArgs e) => firstHitCount++;

			control.Activated += firstDelegate;
			control.Activated -= firstDelegate;
			control.Activated += (sender, e) => secondHitCount++;

			control.PerformClick (control);

			ClassicAssert.IsTrue (firstHitCount == 0, "NSControlShouldRemoveAndAddActivatedEventHandlers - Called first EventHandler after it was removed");
			ClassicAssert.IsTrue (secondHitCount == 1, "NSControlShouldRemoveAndAddActivatedEventHandlers - Did not call second EventHandler");
		}
	}
}
#endif // __MACOS__
