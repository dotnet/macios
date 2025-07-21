using System;

using Foundation;
using GameController;

using NUnit.Framework;

namespace MonoTouchFixtures.GameController {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GCControllerTest {
		[Test]
		public void TheTest ()
		{
			var controllers = GCController.Controllers;
			Assert.That (controllers, Is.Not.Null, $"Null");
			if (controllers.Length == 0)
				Assert.Ignore ("No controllers!");
			for (var i = 0; i < controllers.Length; i++) {
				Assert.That (controllers [i], Is.Not.Null, $"#{i} NotNull");
			}
		}
	}
}
