using SpriteKit;
using Xamarin.Utils;
using System.Numerics;

namespace MonoTouchFixtures.SpriteKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PhysicsWorldTest {
		[SetUp]
		public void VersionCheck ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void SampleFields ()
		{
			using (var scene = new SKScene ()) {
				using (var world = scene.PhysicsWorld) {
					var v = world.SampleFields (new Vector3 (1, 2, 3));
					Assert.That (v.X, Is.EqualTo (0), "#x1");
					Assert.That (v.Y, Is.EqualTo (0), "#y1");
					Assert.That (v.Z, Is.EqualTo (0), "#z1");
				}
			}
		}
	}
}

