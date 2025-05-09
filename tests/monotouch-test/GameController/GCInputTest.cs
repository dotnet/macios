using System;

using Foundation;
using GameController;

using NUnit.Framework;

namespace MonoTouchFixtures.GameController {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GCInputTest {
		[Test]
		public void ButtonNames ()
		{
			Assert.Multiple (() => {
				if (TestRuntime.CheckXcodeVersion (15, 3)) {
					AssertButtonName (GCInput.GetBackLeftButtonName (0), "Back Left Button 0", "BackLeftButtonName");
					AssertButtonName (GCInput.GetBackRightButtonName (0), "Back Right Button 0", "BackRightButtonName");
				}
				if (TestRuntime.CheckXcodeVersion (15, 0))
					AssertButtonName (GCInput.GetArcadeButtonName (0, 0), "Arcade Button 0, 0", "ArcadeButtonName");
			});
		}

		void AssertButtonName (NSString? name, string expected, string message)
		{
			Assert.That (name, Is.Not.Null, $"{message}: null");
			Assert.That (name.ToString (), Is.EqualTo (expected), $"{message}: value");
		}
	}
}
