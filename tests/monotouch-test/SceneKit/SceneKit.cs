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
			Assert.That (s is not null && s != (NSString) (string.Empty), Is.True);
		}

		[Test]
		public void SCNPhysicsTestKeys_SearchModeKeyTest ()
		{
			NSString s = SCNPhysicsTestKeys.SearchModeKey;
			Assert.That (s is not null && s != (NSString) (string.Empty), Is.True);
		}

		[Test]
		public void SCNSceneSourceLoading_AnimationImportPolicyKeyTest ()
		{
			NSString s = SCNSceneSourceLoading.AnimationImportPolicyKey;
			Assert.That (s is not null && s != (NSString) (string.Empty), Is.True);
		}
	}
}
#endif // __MACOS__
