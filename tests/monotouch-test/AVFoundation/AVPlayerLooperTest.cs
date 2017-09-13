﻿#if !__WATCHOS__
using System;
using System.IO;
#if XAMCORE_2_0
using Foundation;
using AVFoundation;
#else
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
#endif
using NUnit.Framework;
namespace monotouchtest.AVFoundation
{
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVPlayerLooperTest
	{
#if !XAMCORE_4_0
		public void TestLoopingEnabled ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand.wav");
			Assert.True (File.Exists (file), file);
			using (var url = new NSUrl (file))
			using (var playerItem = AVPlayerItem.FromUrl (url))
			using (AVQueuePlayer player = AVQueuePlayer.FromItems (new[] { playerItem }))
			using (var playerLooper = AVPlayerLooper.FromPlayer (player, playerItem)) {
				Assert.True (playerLooper.LoopingEnabled, "The default value should be true.");
				playerLooper.DisableLooping ();
				Assert.False (playerLooper.LoopingEnabled, "Looping enabled should return false after 'DisableLooping'");
			}
		}
#endif
	}
}
#endif