//
// Unit tests for CATextLayerTests
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using CoreAnimation;

namespace MonoTouchFixtures.CoreAnimation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CATextLayerTests {

		[Test]
		public void CATextLayerTruncationModeTest ()
		{
			var textLayer = new CATextLayer {
				String = "Hello",
				TextTruncationMode = CATextLayerTruncationMode.Middle
			};

			Assert.That (textLayer.TextTruncationMode, Is.EqualTo (CATextLayerTruncationMode.Middle), "TextTruncationMode");

			textLayer.TextTruncationMode = CATextLayerTruncationMode.End;
			Assert.That (textLayer.TextTruncationMode, Is.EqualTo (CATextLayerTruncationMode.End), "TextTruncationMode 2");
		}

		[Test]
		public void CATextLayerAlignmentModeTest ()
		{
			var textLayer = new CATextLayer {
				String = "Hello",
				TextAlignmentMode = CATextLayerAlignmentMode.Justified
			};

			Assert.That (textLayer.TextAlignmentMode, Is.EqualTo (CATextLayerAlignmentMode.Justified), "TextAlignmentMode");

			textLayer.TextAlignmentMode = CATextLayerAlignmentMode.Natural;
			Assert.That (textLayer.TextAlignmentMode, Is.EqualTo (CATextLayerAlignmentMode.Natural), "TextAlignmentMode 2");
		}
	}
}
