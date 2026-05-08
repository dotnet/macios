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

			ClassicAssert.IsFalse (view.Material == material, "NSVisualEffectViewShouldChangeMaterial - Failed to set the Material property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeBlendingMode ()
		{
			var blendingMode = view.BlendingMode;
			view.BlendingMode = NSVisualEffectBlendingMode.WithinWindow;

			ClassicAssert.IsFalse (view.BlendingMode == blendingMode, "NSVisualEffectViewShouldChangeBlendingMode - Failed to set the BlendingMode property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeState ()
		{
			var state = view.State;
			view.State = NSVisualEffectState.Inactive;

			ClassicAssert.IsFalse (view.State == state, "NSVisualEffectViewShouldChangeState - Failed to set the State property");
		}

		[Test]
		public void NSVisualEffectViewShouldChangeMaskImage ()
		{
			var image = view.MaskImage;
			view.MaskImage = new NSImage ();

			ClassicAssert.IsFalse (view.MaskImage == image, "NSVisualEffectViewShouldChangeMaskImage - Failed to set the MaskImage property");
		}
	}
}
#endif // __MACOS__
