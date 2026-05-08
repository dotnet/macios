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

			ClassicAssert.AreEqual (CATextLayerTruncationMode.Middle, textLayer.TextTruncationMode, "TextTruncationMode");

			textLayer.TextTruncationMode = CATextLayerTruncationMode.End;
			ClassicAssert.AreEqual (CATextLayerTruncationMode.End, textLayer.TextTruncationMode, "TextTruncationMode 2");
		}

		[Test]
		public void CATextLayerAlignmentModeTest ()
		{
			var textLayer = new CATextLayer {
				String = "Hello",
				TextAlignmentMode = CATextLayerAlignmentMode.Justified
			};

			ClassicAssert.AreEqual (CATextLayerAlignmentMode.Justified, textLayer.TextAlignmentMode, "TextAlignmentMode");

			textLayer.TextAlignmentMode = CATextLayerAlignmentMode.Natural;
			ClassicAssert.AreEqual (CATextLayerAlignmentMode.Natural, textLayer.TextAlignmentMode, "TextAlignmentMode 2");
		}
	}
}
