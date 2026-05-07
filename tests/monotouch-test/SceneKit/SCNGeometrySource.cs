#if __MACOS__
using System.Threading.Tasks;

using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNGeometrySourceTests {
		[Test]
		public void SCNGeometrySourceSemanticTest ()
		{
			Assert.IsNotNull (SCNGeometrySourceSemantic.Color, "Color");
		}

		[Test]
		public void SCNGeometrySource_FromDataTest ()
		{
#pragma warning disable 0219
			SCNGeometrySource d = SCNGeometrySource.FromData (new NSData (), SCNGeometrySourceSemantic.Color, 1, false, 1, 1, 1, 1);
			foreach (var s in Enum.GetValues<SCNGeometrySourceSemantics> ()) {
				d = SCNGeometrySource.FromData (new NSData (), s, 1, false, 1, 1, 1, 1);
			}
#pragma warning restore 0219
		}

		[Test]
		public void SCNGeometrySource_BoneStringTests () // These were radar://17782603
		{
#pragma warning disable 0219
			SCNGeometrySource d = SCNGeometrySource.FromData (new NSData (), SCNGeometrySourceSemantic.BoneWeights, 1, false, 1, 1, 1, 1);
			d = SCNGeometrySource.FromData (new NSData (), SCNGeometrySourceSemantic.BoneIndices, 1, false, 1, 1, 1, 1);
#pragma warning restore 0219
		}
	}
}
#endif // __MACOS__
