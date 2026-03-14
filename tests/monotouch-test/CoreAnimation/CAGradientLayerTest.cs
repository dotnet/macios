// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for CAGradientLayer
//

using CoreAnimation;
using CoreGraphics;

namespace MonoTouchFixtures.CoreAnimation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CAGradientLayerTest {

		[Test]
		public void Colors_GetSet ()
		{
			using var layer = new CAGradientLayer ();
			Assert.IsNull (layer.Colors, "Colors/default");

			var red = new CGColor (1, 0, 0);
			var green = new CGColor (0, 1, 0);
			var blue = new CGColor (0, 0, 1);
			layer.Colors = new CGColor [] { red, green, blue };
			var colors = layer.Colors;
			Assert.IsNotNull (colors, "Colors/assigned");
			Assert.AreEqual (3, colors!.Length, "Colors/length");

			layer.Colors = null;
			Assert.IsNull (layer.Colors, "Colors/null");
		}
	}
}
