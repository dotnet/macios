//
// Unit tests for MPRemoteCommandCenter
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

using System;
#if XAMCORE_2_0
using Foundation;
using MediaPlayer;
using ObjCRuntime;
#if !MONOMAC
using UIKit;
#endif
#else
using MonoTouch.Foundation;
using MonoTouch.MediaPlayer;
#if !MONOMAC
using MonoTouch.UIKit;
#endif
#endif
using NUnit.Framework;

namespace MonoTouchFixtures.MediaPlayer {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RemoteCommandCenterTest {

		[Test]
		public void Shared ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.iOS, 7, 1, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.NotNull (shared.BookmarkCommand, "BookmarkCommand");
			Assert.NotNull (shared.ChangePlaybackRateCommand, "ChangePlaybackRateCommand");
			Assert.NotNull (shared.DislikeCommand, "DislikeCommand");
			Assert.NotNull (shared.LikeCommand, "LikeCommand");
			Assert.NotNull (shared.NextTrackCommand, "NextTrackCommand");
			Assert.NotNull (shared.PauseCommand, "PauseCommand");
			Assert.NotNull (shared.PlayCommand, "PlayCommand");
			Assert.NotNull (shared.PreviousTrackCommand, "PreviousTrackCommand");
			Assert.NotNull (shared.SeekBackwardCommand, "SeekBackwardCommand");
			Assert.NotNull (shared.SeekForwardCommand, "SeekForwardCommand");
			Assert.NotNull (shared.SkipBackwardCommand, "SkipBackwardCommand");
			Assert.NotNull (shared.SkipForwardCommand, "SkipForwardCommand");
			Assert.NotNull (shared.StopCommand, "StopCommand");
			Assert.NotNull (shared.TogglePlayPauseCommand, "TogglePlayPauseCommand");
		}

		[Test]
		public void Shared_8 ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.NotNull (shared.ChangeRepeatModeCommand, "ChangeRepeatModeCommand");
			Assert.NotNull (shared.ChangeShuffleModeCommand, "ChangeShuffleModeCommand");
		}

		[Test]
		public void Shared_9 ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.iOS, 9, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 12, 2, throwIfOtherPlatform: false);

			MPRemoteCommandCenter shared = MPRemoteCommandCenter.Shared;
			Assert.NotNull (shared.EnableLanguageOptionCommand, "EnableLanguageOptionCommand");
		}
	}
}

#endif // !__WATCHOS__
