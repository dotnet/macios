#if __MACOS__
using System.Threading.Tasks;

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSColorTests {
		[Test]
		public void NSColor_ComponentTests ()
		{
			NSColor c = NSColor.Blue;
			nfloat [] components;
			c.GetComponents (out components);
			Assert.That (0f == components [0], Is.True, "Red");
			Assert.That (0f == components [1], Is.True, "Green");
			Assert.That (1f == components [2], Is.True, "Blue");
		}

		[Test]
		public void SingleComponents ()
		{
			var c = NSColor.Red;
			nfloat [] components;
			c.GetComponents (out components);
			Assert.That (components [0], Is.EqualTo (c.RedComponent), "Red");
			Assert.That (components [1], Is.EqualTo (c.GreenComponent), "Green");
			Assert.That (components [2], Is.EqualTo (c.BlueComponent), "Blue");
		}

		[Test]
		public void FromColorSpace ()
		{
			var components = new nfloat [] { 0, 0.33f, 0.66f, 1 };
			using var color = NSColor.FromColorSpace (NSColorSpace.GenericRGBColorSpace, components);

			color.GetComponents (out var actualComponents);
			Assert.That (actualComponents [0], Is.EqualTo (components [0]), "Red");
			Assert.That (actualComponents [1], Is.EqualTo (components [1]), "Green");
			Assert.That (actualComponents [2], Is.EqualTo (components [2]), "Blue");
			Assert.That (actualComponents [3], Is.EqualTo (components [3]), "Alpha");
		}
	}
}

#endif // __MACOS__
