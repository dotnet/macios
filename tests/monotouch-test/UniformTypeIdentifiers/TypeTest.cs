
using UniformTypeIdentifiers;

namespace MonoTouchFixtures.UniformTypeIdentifiers {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UTTypeTests {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);
		}

		[Test]
		public void Archive ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var a = UTTypes.Archive;
			Assert.That (a.Dynamic, Is.False, "Dynamic");
			var z = UTTypes.Zip;
			Assert.That (z.IsSubtypeOf (a), Is.True, "IsSubtypeOf");
			Assert.That (a.IsSupertypeOf (z), Is.True, "IsSupertypeOf");
		}
	}
}
