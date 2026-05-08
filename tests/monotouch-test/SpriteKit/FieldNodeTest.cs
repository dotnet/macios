
using SpriteKit;
using Xamarin.Utils;
using System.Numerics;

namespace MonoTouchFixtures.SpriteKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FieldNodeTest {
		[SetUp]
		public void VersionCheck ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void CreateRadialGravityField ()
		{
			using (SKFieldNode node = SKFieldNode.CreateRadialGravityField ()) {

				var v = new Vector4 (1, 2, 3, 4);
				node.Direction = v;
				Assert.That (1, Is.EqualTo (node.Direction.X), "#x1");
				Assert.That (2, Is.EqualTo (node.Direction.Y), "#y1");
				Assert.That (3, Is.EqualTo (node.Direction.Z), "#z1");
				Assert.That (0, Is.EqualTo (node.Direction.W), "#w1");

				v = node.Direction;
				Assert.That (1, Is.EqualTo (v.X), "#x2");
				Assert.That (2, Is.EqualTo (v.Y), "#y2");
				Assert.That (3, Is.EqualTo (v.Z), "#z2");
				Assert.That (0, Is.EqualTo (v.W), "#w2");
			}
		}

		[Test]
		public void CreateLinearGravityField ()
		{
			using (var node = SKFieldNode.CreateLinearGravityField (new Vector4 (1, 2, 3, 4))) {

				Assert.That (node.MinimumRadius, Is.EqualTo (0.00457763672f), "#minimum radius");
			}
		}

		[Test]
		public void CreateVelocityField ()
		{
			using (var node = SKFieldNode.CreateVelocityField (new Vector4 (1, 2, 3, 4))) {

				Assert.That (node.MinimumRadius, Is.EqualTo (0.00457763672f), "#minimum radius");
			}
		}

		[Test]
		public void CreateCustomField ()
		{
			using (var node = SKFieldNode.CreateCustomField ((Vector4 position, Vector4 velocity, float mass, float charge, double time) => {
				return new Vector3 (3, 1, 4);
			})) {

				// FIXME: the code below doesn't end up calling the anonymous delegate above.
				var v = node.Direction;
				Assert.That (v.X, Is.EqualTo (0), "#x2");
				Assert.That (v.Y, Is.EqualTo (0), "#y2");
				Assert.That (v.Z, Is.EqualTo (0), "#z2");
				Assert.That (v.W, Is.EqualTo (0), "#w2");
			}
		}
	}
}
