
using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StructTest {

		[Test]
		public void MTLQuadTessellationFactorsHalfStructSize ()
		{
			// tested with a native iOS app
			Assert.That (Marshal.SizeOf<MTLQuadTessellationFactorsHalf> (), Is.EqualTo (12), $"Reported size was {Marshal.SizeOf<MTLQuadTessellationFactorsHalf> ()}");
		}

		[Test]
		public void MTLTriangleTessellationFactorsHalfStructSize ()
		{
			// tested with a native iOS app
			Assert.That (Marshal.SizeOf<MTLTriangleTessellationFactorsHalf> (), Is.EqualTo (8), $"Reported size was {Marshal.SizeOf<MTLTriangleTessellationFactorsHalf> ()}");
		}
	}
}
