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
			ClassicAssert.NotNull (shared.BookmarkCommand, "BookmarkCommand");
			ClassicAssert.NotNull (shared.ChangePlaybackRateCommand, "ChangePlaybackRateCommand");
			ClassicAssert.NotNull (shared.DislikeCommand, "DislikeCommand");
			ClassicAssert.NotNull (shared.LikeCommand, "LikeCommand");
			ClassicAssert.NotNull (shared.NextTrackCommand, "NextTrackCommand");
			ClassicAssert.NotNull (shared.PauseCommand, "PauseCommand");
			ClassicAssert.NotNull (shared.PlayCommand, "PlayCommand");
			ClassicAssert.NotNull (shared.PreviousTrackCommand, "PreviousTrackCommand");
			ClassicAssert.NotNull (shared.SeekBackwardCommand, "SeekBackwardCommand");
			ClassicAssert.NotNull (shared.SeekForwardCommand, "SeekForwardCommand");
			ClassicAssert.NotNull (shared.SkipBackwardCommand, "SkipBackwardCommand");
			ClassicAssert.NotNull (shared.SkipForwardCommand, "SkipForwardCommand");
			ClassicAssert.NotNull (shared.StopCommand, "StopCommand");
			ClassicAssert.NotNull (shared.TogglePlayPauseCommand, "TogglePlayPauseCommand");
		}

		[Test]
		public void Shared_8 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			ClassicAssert.NotNull (shared.ChangeRepeatModeCommand, "ChangeRepeatModeCommand");
			ClassicAssert.NotNull (shared.ChangeShuffleModeCommand, "ChangeShuffleModeCommand");
		}

		[Test]
		public void Shared_9 ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 9, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			ClassicAssert.NotNull (shared.EnableLanguageOptionCommand, "EnableLanguageOptionCommand");
		}
	}
}
