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

			Assert.That (control.ControlSize, Is.Not.EqualTo (size));
			Assert.That (control.ControlSize, Is.EqualTo (NSControlSize.Mini));
		}

		[Test]
		public void NSControlShouldChangeHighlighted ()
		{
			var control = new NSButton ();
			var highlighted = control.Highlighted;
			control.Highlighted = !highlighted;

			Assert.That (control.Highlighted, Is.Not.EqualTo (highlighted));
		}

		[Test]
		public void NSControlShouldChangeLineBreakMode ()
		{
			var control = new NSButton ();
			var lineBreak = control.LineBreakMode;
			control.LineBreakMode = NSLineBreakMode.Clipping;

			Assert.That (control.LineBreakMode, Is.EqualTo (NSLineBreakMode.Clipping));
			Assert.That (control.LineBreakMode, Is.Not.EqualTo (lineBreak));
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

			Assert.That (firstHitCount, Is.EqualTo (1), "NSControlShouldAddMultipleActivatedEventHandlers - Did not call first EventHandler");
			Assert.That (secondHitCount, Is.EqualTo (1), "NSControlShouldAddMultipleActivatedEventHandlers - Did not call second EventHandler");
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

			Assert.That (firstHitCount, Is.EqualTo (0), "NSControlShouldRemoveAndAddActivatedEventHandlers - Called first EventHandler after it was removed");
			Assert.That (secondHitCount, Is.EqualTo (1), "NSControlShouldRemoveAndAddActivatedEventHandlers - Did not call second EventHandler");
		}
	}
}
#endif // __MACOS__
