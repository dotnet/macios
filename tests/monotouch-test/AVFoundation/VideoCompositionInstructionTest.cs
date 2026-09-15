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
				Assert.That (i.BackgroundColor, Is.Null, "BackgroundColor");
				Assert.That (i.EnablePostProcessing, Is.True, "EnablePostProcessing");
				Assert.That (i.LayerInstructions, Is.Null, "LayerInstructions");
				Assert.That (i.TimeRange.Start.IsInvalid, Is.True, "TimeRange.Start");
				Assert.That (i.TimeRange.Duration.IsInvalid, Is.True, "TimeRange.Duration");
			}
		}

		[Test]
		public void Seven ()
		{
			TestRuntime.AssertXcodeVersion (5, 0, 1);

			using (var i = new AVVideoCompositionInstruction ()) {
				Assert.That (i.ContainsTweening, Is.False, "ContainsTweening");
				Assert.That (i.PassthroughTrackID, Is.EqualTo (0), "PassthroughTrackID");
				Assert.That (i.RequiredSourceTrackIDs.Length, Is.EqualTo (0), "RequiredSourceTrackIDs");
			}
		}
	}
}
