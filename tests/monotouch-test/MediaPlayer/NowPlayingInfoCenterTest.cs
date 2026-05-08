// Copyright 2016 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using System.IO;
using MediaPlayer;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.MediaPlayer {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NowPlayingInfoCenterTest {
		MPNowPlayingInfo NowPlayingInfo;

		bool v8_0 = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 8, 0);
		bool v9_0 = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 9, 0);
		bool v10_0 = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 10, 0);
		bool v10_3 = TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 10, 3);

		[SetUp]
		public void SetUp ()
		{
			MPNowPlayingInfoLanguageOption languageOption = null;
			MPNowPlayingInfoLanguageOptionGroup languageOptionGroup = null;
			if (v9_0) {
				languageOption = new MPNowPlayingInfoLanguageOption (MPNowPlayingInfoLanguageOptionType.Audible, "en", null, "English", "en");
				languageOptionGroup = new MPNowPlayingInfoLanguageOptionGroup (new MPNowPlayingInfoLanguageOption [] { languageOption }, languageOption, false);
			}
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var img = UIImage.FromFile (file)) {
				NowPlayingInfo = new MPNowPlayingInfo {
					//MPNowPlayingInfoCenter
					ElapsedPlaybackTime = 1.0,
					PlaybackRate = 1.0,
					DefaultPlaybackRate = 1.0,
					PlaybackQueueIndex = 0,
					PlaybackQueueCount = 10,
					ChapterNumber = 1,
					ChapterCount = 10,
					AvailableLanguageOptions = v9_0 ? new MPNowPlayingInfoLanguageOptionGroup [] { languageOptionGroup } : null,
					CurrentLanguageOptions = v9_0 ? new MPNowPlayingInfoLanguageOption [] { new MPNowPlayingInfoLanguageOption (MPNowPlayingInfoLanguageOptionType.Audible, "en", null, "English", "en") } : null,
					CollectionIdentifier = "Collection",
					ExternalContentIdentifier = "ExternalContent",
					ExternalUserProfileIdentifier = "ExternalUserProfile",
					PlaybackProgress = 0.5f,
					MediaType = MPNowPlayingInfoMediaType.Audio,
					IsLiveStream = false,
					AssetUrl = new NSUrl ("https://developer.xamarin.com"),

					//MPMediaItem
					AlbumTitle = "AlbumTitle",
					AlbumTrackCount = 13,
					AlbumTrackNumber = 1,
					Artist = "Artist",
					Artwork = new MPMediaItemArtwork (img),
					Composer = "Composer",
					DiscCount = 1,
					DiscNumber = 1,
					Genre = "Genre",
					PersistentID = 1,
					PlaybackDuration = 100.0,
					Title = "Title",
				};
			}
		}

		[Test]
		public void NowPlaying ()
		{
			using (var dc = MPNowPlayingInfoCenter.DefaultCenter) {
				dc.NowPlaying = NowPlayingInfo; // internal NSDictionary ToDictionary ()
				var np = dc.NowPlaying; // internal MPNowPlayingInfo (NSDictionary source)

				Assert.That (typeof (double), Is.InstanceOf (), np.ElapsedPlaybackTime, "#1");
				Assert.That (typeof (double), Is.InstanceOf (), np.PlaybackRate, "#2");
				if (v8_0)
					Assert.That (typeof (double), Is.InstanceOf (), np.DefaultPlaybackRate, "#3");
				Assert.That (typeof (int), Is.InstanceOf (), np.PlaybackQueueIndex, "#4");
				Assert.That (typeof (int), Is.InstanceOf (), np.PlaybackQueueCount, "#5");
				Assert.That (typeof (int), Is.InstanceOf (), np.ChapterNumber, "#6");
				Assert.That (typeof (int), Is.InstanceOf (), np.ChapterCount, "#7");

				if (v9_0) {
					Assert.That (typeof (MPNowPlayingInfoLanguageOptionGroup []), Is.InstanceOf (), np.AvailableLanguageOptions, "#8");
					Assert.That (typeof (MPNowPlayingInfoLanguageOption []), Is.InstanceOf (), np.CurrentLanguageOptions, "#9");
				}
				if (v10_0) {
					Assert.That (typeof (string), Is.InstanceOf (), (object) np.CollectionIdentifier, "#10");
					Assert.That (typeof (string), Is.InstanceOf (), (object) np.ExternalContentIdentifier, "#11");
					Assert.That (typeof (string), Is.InstanceOf (), (object) np.ExternalUserProfileIdentifier, "#12");
					Assert.That (typeof (float), Is.InstanceOf (), np.PlaybackProgress, "#13");
					Assert.That (typeof (MPNowPlayingInfoMediaType), Is.InstanceOf (), np.MediaType, "#14");
					Assert.That (typeof (bool), Is.InstanceOf (), np.IsLiveStream, "#15");
				}

				Assert.That (typeof (string), Is.InstanceOf (), (object) np.AlbumTitle, "#16");
				Assert.That (typeof (int), Is.InstanceOf (), np.AlbumTrackCount, "#17");
				Assert.That (typeof (int), Is.InstanceOf (), np.AlbumTrackNumber, "#18");
				Assert.That (typeof (string), Is.InstanceOf (), (object) np.Artist, "#19");
				Assert.That (typeof (MPMediaItemArtwork), Is.InstanceOf (), np.Artwork, "#20");
				Assert.That (typeof (string), Is.InstanceOf (), (object) np.Composer, "#21");
				Assert.That (typeof (int), Is.InstanceOf (), np.DiscCount, "#22");
				Assert.That (typeof (int), Is.InstanceOf (), np.DiscNumber, "#23");
				Assert.That (typeof (string), Is.InstanceOf (), (object) np.Genre, "#24");
				Assert.That (typeof (ulong), Is.InstanceOf (), np.PersistentID, "#25");
				Assert.That (typeof (double), Is.InstanceOf (), np.PlaybackDuration, "#26");
				Assert.That (typeof (string), Is.InstanceOf (), (object) np.Title, "#27");

				if (v10_3)
					Assert.That (typeof (NSUrl), Is.InstanceOf (), np.AssetUrl, "#28");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
