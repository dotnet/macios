#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSVisualEffectViewTests {
		NSVisualEffectView view;

		[SetUp]
		public void SetUp ()
		{
			view = new NSVisualEffectView ();
		}

		[Test]
		public void NSVisualEffectViewShouldChangeMaterial ()
		{
			var material = view.Material;
			view.Material = NSVisualEffectMaterial.Titlebar;

			Assert.That (view.Material == material, Is.False, "NSVisualEffectViewShouldChangeMaterial - Failed to set the Material property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeBlendingMode ()
		{
			var blendingMode = view.BlendingMode;
			view.BlendingMode = NSVisualEffectBlendingMode.WithinWindow;

			Assert.That (view.BlendingMode == blendingMode, Is.False, "NSVisualEffectViewShouldChangeBlendingMode - Failed to set the BlendingMode property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeState ()
		{
			var state = view.State;
			view.State = NSVisualEffectState.Inactive;

			Assert.That (view.State == state, Is.False, "NSVisualEffectViewShouldChangeState - Failed to set the State property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeMaskImage ()
		{
			var image = view.MaskImage;
			view.MaskImage = new NSImage ();

			Assert.That (view.MaskImage == image, Is.False, "NSVisualEffectViewShouldChangeMaskImage - Failed to set the MaskImage property");
		}
	}
}
#endif // __MACOS__
