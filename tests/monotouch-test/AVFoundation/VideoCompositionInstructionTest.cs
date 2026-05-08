//
// Unit tests for AVVideoCompositionInstruction
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using AVFoundation;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	[TestFixture]
	public class VideoCompositionInstructionTest {

		[Test]
		public void Defaults ()
		{
			using (var i = new AVVideoCompositionInstruction ()) {
				ClassicAssert.Null (i.BackgroundColor, "BackgroundColor");
				ClassicAssert.True (i.EnablePostProcessing, "EnablePostProcessing");
				ClassicAssert.Null (i.LayerInstructions, "LayerInstructions");
				ClassicAssert.True (i.TimeRange.Start.IsInvalid, "TimeRange.Start");
				ClassicAssert.True (i.TimeRange.Duration.IsInvalid, "TimeRange.Duration");
			}
		}

		[Test]
		public void Seven ()
		{
			TestRuntime.AssertXcodeVersion (5, 0, 1);

			using (var i = new AVVideoCompositionInstruction ()) {
				ClassicAssert.False (i.ContainsTweening, "ContainsTweening");
				Assert.That (i.PassthroughTrackID, Is.EqualTo (0), "PassthroughTrackID");
				Assert.That (i.RequiredSourceTrackIDs.Length, Is.EqualTo (0), "RequiredSourceTrackIDs");
			}
		}
	}
}
