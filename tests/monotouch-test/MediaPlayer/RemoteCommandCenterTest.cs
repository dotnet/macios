//
// Unit tests for MPRemoteCommandCenter
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using MediaPlayer;
#if !MONOMAC
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.MediaPlayer {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RemoteCommandCenterTest {

		[Test]
		public void Shared ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 1, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.That (shared.BookmarkCommand, Is.Not.Null, "BookmarkCommand");
			Assert.That (shared.ChangePlaybackRateCommand, Is.Not.Null, "ChangePlaybackRateCommand");
			Assert.That (shared.DislikeCommand, Is.Not.Null, "DislikeCommand");
			Assert.That (shared.LikeCommand, Is.Not.Null, "LikeCommand");
			Assert.That (shared.NextTrackCommand, Is.Not.Null, "NextTrackCommand");
			Assert.That (shared.PauseCommand, Is.Not.Null, "PauseCommand");
			Assert.That (shared.PlayCommand, Is.Not.Null, "PlayCommand");
			Assert.That (shared.PreviousTrackCommand, Is.Not.Null, "PreviousTrackCommand");
			Assert.That (shared.SeekBackwardCommand, Is.Not.Null, "SeekBackwardCommand");
			Assert.That (shared.SeekForwardCommand, Is.Not.Null, "SeekForwardCommand");
			Assert.That (shared.SkipBackwardCommand, Is.Not.Null, "SkipBackwardCommand");
			Assert.That (shared.SkipForwardCommand, Is.Not.Null, "SkipForwardCommand");
			Assert.That (shared.StopCommand, Is.Not.Null, "StopCommand");
			Assert.That (shared.TogglePlayPauseCommand, Is.Not.Null, "TogglePlayPauseCommand");
		}

		[Test]
		public void Shared_8 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.That (shared.ChangeRepeatModeCommand, Is.Not.Null, "ChangeRepeatModeCommand");
			Assert.That (shared.ChangeShuffleModeCommand, Is.Not.Null, "ChangeShuffleModeCommand");
		}

		[Test]
		public void Shared_9 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 9, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.That (shared.EnableLanguageOptionCommand, Is.Not.Null, "EnableLanguageOptionCommand");
		}
	}
}
