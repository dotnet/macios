//
// Unit tests for SCNAction
//

using SceneKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.SceneKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ActionTest {

		float timeFunctionValue;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (6, 0);
		}

		[Test]
		public void TimingFunction_5072 ()
		{
			// API was introduced in iOS 8, but it's broken (it copies the block pointer instead of copying the block itself, which means that a block stored on the stack will not behave as expected when trying to invoke it later)
			TestRuntime.AssertXcodeVersion (7, 0);
			// https://github.com/dotnet/macios/issues/5072
			using var a = new SCNAction ();
			Assert.That (a.TimingFunction, Is.Null, "TimingFunction-initial");

			a.TimingFunction = null;
			Assert.That (a.TimingFunction, Is.Null, "TimingFunction-null");

			timeFunctionValue = float.NaN;
			a.TimingFunction = (float f) => {
				timeFunctionValue = f;
				return timeFunctionValue;
			};
			var timingFunction = a.TimingFunction;
			Assert.That (timingFunction, Is.Not.Null, "TimingFunction-assigned");
			Assert.That (timingFunction (0.5f), Is.EqualTo (0.5f), "TimingFunction-return");
			Assert.That (timeFunctionValue, Is.EqualTo (0.5f), "TimingFunction-invoked");

			a.TimingFunction = null;
			Assert.That (a.TimingFunction, Is.Null, "TimingFunction-end");
		}
	}
}
