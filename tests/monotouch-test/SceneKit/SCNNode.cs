#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNNodeTests {
		[Test]
		public void SCNNode_AddAnimation ()
		{
			SCNNode c = new SCNNode ();
			CABasicAnimation a = CABasicAnimation.FromKeyPath ("hidden");
			NSString key = new NSString ("MyKey");
			c.AddAnimation (a, key);
			CAPropertyAnimation cur = (CAPropertyAnimation) c.GetAnimation (key);
			Assert.That (cur, Is.Not.Null);
			Assert.That (cur.KeyPath, Is.EqualTo ("hidden"));
			c.RemoveAnimation (key);
			cur = (CAPropertyAnimation) c.GetAnimation (key);
			Assert.That (cur, Is.Null);
		}

		[Test]
		public void SCNNode_SetPhysicsBodyTest ()
		{
			if (IntPtr.Size == 8) {
				// Create a new empty scene
				var Scene = new SCNScene ();

				var floorNode = SCNNode.Create ();
				Scene.RootNode.AddChildNode (floorNode);

				floorNode.PhysicsBody = SCNPhysicsBody.CreateStaticBody ();
				Scene.PhysicsWorld.Speed = 0;
			}
		}

		[Test]
		public void SCNNode_GeometryNullTest ()
		{
			var floorNode = SCNNode.Create ();
			floorNode.Geometry = null;
		}
	}
}
#endif // __MACOS__
