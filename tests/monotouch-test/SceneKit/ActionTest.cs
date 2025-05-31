//
// Unit tests for SCNAction
//

using System;
using Foundation;
using SceneKit;
using ObjCRuntime;
using NUnit.Framework;
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
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void TimingFunction_5072 ()
		{
			// API was introduced in iOS 8, but it's broken (it copies the block pointer instead of copying the block itself, which means that a block stored on the stack will not behave as expected when trying to invoke it later)
			TestRuntime.AssertXcodeVersion (7, 0);
			// https://github.com/dotnet/macios/issues/5072
			var a = new SCNAction ();
			a.TimingFunction = (float f) => {
				timeFunctionValue = f;
				return timeFunctionValue;
			};
			// Assert.Null (a.TimingFunction, "TimingFunction-end");
		}
	}
}
