
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StructTest {

		[Test]
		public void MTLQuadTessellationFactorsHalfStructSize ()
		{
			// tested with a native iOS app
			Assert.AreEqual (12, Marshal.SizeOf<MTLQuadTessellationFactorsHalf> (), $"Reported size was {Marshal.SizeOf<MTLQuadTessellationFactorsHalf> ()}");
		}

		[Test]
		public void MTLTriangleTessellationFactorsHalfStructSize ()
		{
			// tested with a native iOS app
			Assert.AreEqual (8, Marshal.SizeOf<MTLTriangleTessellationFactorsHalf> (), $"Reported size was {Marshal.SizeOf<MTLTriangleTessellationFactorsHalf> ()}");
		}
	}
}
