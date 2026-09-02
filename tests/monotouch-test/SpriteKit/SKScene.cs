#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using CoreGraphics;
using SpriteKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SKSceneTests {
		[Test]
		public void SKScene_InitWithSize ()
		{
			if (IntPtr.Size != 8) // SpriteKit is 64-bit only on mac
				return;

			SKNode c = new SKNode ();
			//SKScene c = new SKScene (new CGSize (50, 50));
			Assert.That (c, Is.Not.Null);
		}

		[Test]
		public void SKScene_InitWithSizeSuper ()
		{
			if (IntPtr.Size != 8) // SpriteKit is 64-bit only on mac
				return;

			MyScene c = new MyScene (new CGSize (50, 50));
			Assert.That (c, Is.Not.Null);
		}

		class MyScene : SKScene {
			public MyScene (CGSize size) : base (size)
			{
			}
		}
	}
}
#endif // __MACOS__
