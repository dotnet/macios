#if __IOS__ || MONOMAC

using CoreGraphics;
using Phase;

#nullable enable

namespace MonoTouchFixtures.Phase {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class PhaseObjectTest {
		PhaseEngine? engine;
		PhaseObject? phaseObject;

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			// not supported in simulator
			TestRuntime.AssertDevice ();
			engine = new PhaseEngine (PhaseUpdateMode.Automatic);
			phaseObject = new PhaseObject (engine);
		}

		[TearDown]
		public void TearDown ()
		{
			phaseObject?.Dispose ();
			engine?.Dispose ();
		}

		[Test]
		public void RightTest ()
		{
			var right = PhaseObject.Right;
			Assert.That (right, Is.Not.Null, "not null");
			Assert.That (right.Length (), Is.EqualTo (1), "length");
		}

		[Test]
		public void UpTest ()
		{
			var up = PhaseObject.Up;
			Assert.That (up, Is.Not.Null, "not null");
			Assert.That (up.Length (), Is.EqualTo (1), "length");
		}

		[Test]
		public void ForwardTest ()
		{
			var fwd = PhaseObject.Forward;
			Assert.That (fwd, Is.Not.Null, "not null");
			Assert.That (fwd.Length (), Is.EqualTo (1), "length");
		}

		[Test]
		public void TransformTest ()
		{
			var matrix = phaseObject.Transform;
			Assert.That (matrix, Is.Not.Null, "not null");
			Assert.That (matrix.M11, Is.EqualTo (1), "11");
		}

		[Test]
		public void WorldTransform ()
		{
			var matrix = phaseObject.WorldTransform;
			Assert.That (matrix, Is.Not.Null, "not null");
			Assert.That (matrix.M11, Is.EqualTo (1), "11");
		}

	}
}

#endif // __IOS__ || MONOMAC
