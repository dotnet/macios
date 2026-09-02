#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using CoreAnimation;
using CoreGraphics;
using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SceneKitTests // Generic one off tests
	{
		[Test]
		public void SCNGeometrySourceSemantic_ColorKeyTest ()
		{
			NSString s = SCNGeometrySourceSemantic.Color;
			Assert.That (s, Is.Not.Null);
			Assert.That (s, Is.Not.EqualTo ((NSString) (string.Empty)));
		}

		[Test]
		public void SCNPhysicsTestKeys_SearchModeKeyTest ()
		{
			NSString s = SCNPhysicsTestKeys.SearchModeKey;
			Assert.That (s, Is.Not.Null);
			Assert.That (s, Is.Not.EqualTo ((NSString) (string.Empty)));
		}

		[Test]
		public void SCNSceneSourceLoading_AnimationImportPolicyKeyTest ()
		{
			NSString s = SCNSceneSourceLoading.AnimationImportPolicyKey;
			Assert.That (s, Is.Not.Null);
			Assert.That (s, Is.Not.EqualTo ((NSString) (string.Empty)));
		}
	}
}
#endif // __MACOS__
