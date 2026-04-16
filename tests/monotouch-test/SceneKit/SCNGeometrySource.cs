#if __MACOS__
using System.Threading.Tasks;

using SceneKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SCNGeometrySourceTests {
		[SetUp]
		public void SetUp ()
		{
			Asserts.Ensure64Bit ();
		}

		[Test]
		public void SCNGeometrySourceSemanticTest ()
		{
			Asserts.EnsureMountainLion ();
			Assert.IsNotNull (SCNGeometrySourceSemantic.Color, "Color");
		}

		private bool isValidEnumForPlatform (SCNGeometrySourceSemantics value)
		{
			return true;
		}

		[Test]
		public void SCNGeometrySource_FromDataTest ()
		{
			Asserts.EnsureMountainLion ();
#pragma warning disable 0219
			SCNGeometrySource d = SCNGeometrySource.FromData (new NSData (), SCNGeometrySourceSemantic.Color, 1, false, 1, 1, 1, 1);
			foreach (var s in Enum.GetValues<SCNGeometrySourceSemantics> ()) {
				if (!isValidEnumForPlatform (s))
					continue;
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
