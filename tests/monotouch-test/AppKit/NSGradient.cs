#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSGradientTests {
		[Test]
		public void NSGradientConstructorTests ()
		{
			NSColorSpace colorSpace = NSColorSpace.GenericRGBColorSpace;
			NSGradient g = new NSGradient (new [] { NSColor.Black, NSColor.White, NSColor.Black }, new [] { 0f, .5f, 1.0f }, colorSpace);
			Assert.That (g, Is.Not.Null);
			Assert.That (g.ColorSpace, Is.EqualTo (colorSpace));
			Assert.That (g.ColorStopsCount, Is.EqualTo ((nint) 3));

			// Since we are asking for colors on a gradient, there will be some color blending, even with just black and white.
			const float closeEnough = .05f;
			NSColor black = NSColor.Black.UsingColorSpace (NSColorSpace.CalibratedRGB);
			NSColor white = NSColor.White.UsingColorSpace (NSColorSpace.CalibratedRGB);

			NSColor color;
			nfloat location;

			g.GetColor (out color, out location, 0);
			color = color.UsingColorSpace (NSColorSpace.CalibratedRGB);
			Assert.That (black.RedComponent - color.RedComponent < closeEnough, Is.True);
			Assert.That (black.BlueComponent - color.BlueComponent < closeEnough, Is.True);
			Assert.That (black.GreenComponent - color.GreenComponent < closeEnough, Is.True);
			Assert.That ((float) location, Is.EqualTo (0.0f));

			g.GetColor (out color, out location, 1);
			color = color.UsingColorSpace (NSColorSpace.CalibratedRGB);
			Assert.That (white.RedComponent - color.RedComponent < closeEnough, Is.True);
			Assert.That (white.BlueComponent - color.BlueComponent < closeEnough, Is.True);
			Assert.That (white.GreenComponent - color.GreenComponent < closeEnough, Is.True);
			Assert.That ((float) location, Is.EqualTo (0.5f));

			g.GetColor (out color, out location, 2);
			color = color.UsingColorSpace (NSColorSpace.CalibratedRGB);
			Assert.That (black.RedComponent - color.RedComponent < closeEnough, Is.True);
			Assert.That (black.BlueComponent - color.BlueComponent < closeEnough, Is.True);
			Assert.That (black.GreenComponent - color.GreenComponent < closeEnough, Is.True);
			Assert.That ((float) location, Is.EqualTo (1.0f));
		}
	}
}

#endif // __MACOS__
