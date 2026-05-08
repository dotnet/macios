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

			Assert.That (size == control.ControlSize, Is.False);
			Assert.That (control.ControlSize == NSControlSize.Mini, Is.True);
		}

		[Test]
		public void NSControlShouldChangeHighlighted ()
		{
			var control = new NSButton ();
			var highlighted = control.Highlighted;
			control.Highlighted = !highlighted;

			Assert.That (highlighted == control.Highlighted, Is.False);
		}

		[Test]
		public void NSControlShouldChangeLineBreakMode ()
		{
			var control = new NSButton ();
			var lineBreak = control.LineBreakMode;
			control.LineBreakMode = NSLineBreakMode.Clipping;

			Assert.That (control.LineBreakMode == NSLineBreakMode.Clipping, Is.True);
			Assert.That (lineBreak == control.LineBreakMode, Is.False);
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

			Assert.That (firstHitCount == 1, Is.True, "NSControlShouldAddMultipleActivatedEventHandlers - Did not call first EventHandler");
			Assert.That (secondHitCount == 1, Is.True, "NSControlShouldAddMultipleActivatedEventHandlers - Did not call second EventHandler");
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

			Assert.That (firstHitCount == 0, Is.True, "NSControlShouldRemoveAndAddActivatedEventHandlers - Called first EventHandler after it was removed");
			Assert.That (secondHitCount == 1, Is.True, "NSControlShouldRemoveAndAddActivatedEventHandlers - Did not call second EventHandler");
		}
	}
}
#endif // __MACOS__
