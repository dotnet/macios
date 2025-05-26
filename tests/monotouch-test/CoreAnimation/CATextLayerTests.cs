//
// Unit tests for CATextLayerTests
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System;
using NUnit.Framework;

using Foundation;
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

			Assert.AreEqual (CATextLayerTruncationMode.Middle, textLayer.TextTruncationMode, "TextTruncationMode");

			textLayer.TextTruncationMode = CATextLayerTruncationMode.End;
			Assert.AreEqual (CATextLayerTruncationMode.End, textLayer.TextTruncationMode, "TextTruncationMode 2");
		}

		[Test]
		public void CATextLayerAlignmentModeTest ()
		{
			var textLayer = new CATextLayer {
				String = "Hello",
				TextAlignmentMode = CATextLayerAlignmentMode.Justified
			};

			Assert.AreEqual (CATextLayerAlignmentMode.Justified, textLayer.TextAlignmentMode, "TextAlignmentMode");

			textLayer.TextAlignmentMode = CATextLayerAlignmentMode.Natural;
			Assert.AreEqual (CATextLayerAlignmentMode.Natural, textLayer.TextAlignmentMode, "TextAlignmentMode 2");
		}
	}
}
