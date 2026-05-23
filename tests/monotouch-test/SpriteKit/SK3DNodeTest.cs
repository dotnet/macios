#if !MONOMAC
using UIKit;
#endif
using SpriteKit;
using SceneKit;
using Xamarin.Utils;

using System.Numerics;

namespace MonoTouchFixtures.SpriteKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SK3DNodeTest {
		[SetUp]
		public void VersionCheck ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void ProjectPoint ()
		{
			if (TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("This doesn't seem to work properly in the iOS 9+ or macOS 10.11+");

			// SK3Node loads SCNRenderer dynamically, so make sure it's actually loaded.
			GC.KeepAlive (Class.GetHandle (typeof (SCNRenderer)));

			using (var node = new SK3DNode ()) {
				var v = node.ProjectPoint (new Vector3 (1, 2, 3));
				Assert.That (v.X, Is.EqualTo (1), "#x1");
				Assert.That (v.Y, Is.EqualTo (2), "#y1");
				Assert.That (v.Z, Is.EqualTo (3), "#z1");
			}
		}

		[Test]
		public void UnprojectPoint ()
		{
#if MONOMAC
			Assert.Ignore ("This doesn't seem to work properly in macOS 10.12");
#elif __IOS__ || __TVOS__
			if (TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("This doesn't seem to work properly in the iOS 9");
#endif

			using (var node = new SK3DNode ()) {
				var v = node.UnprojectPoint (new Vector3 (1, 2, 3));
				Assert.That (v.X, Is.EqualTo (1), "#x1");
				Assert.That (v.Y, Is.EqualTo (2), "#y1");
				Assert.That (v.Z, Is.EqualTo (3), "#z1");
			}
		}
	}
}
