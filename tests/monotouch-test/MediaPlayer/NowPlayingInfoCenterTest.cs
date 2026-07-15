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

				Assert.That (np.ElapsedPlaybackTime, Is.InstanceOf (typeof (double)), "#1");
				Assert.That (np.PlaybackRate, Is.InstanceOf (typeof (double)), "#2");
				if (v8_0)
					Assert.That (np.DefaultPlaybackRate, Is.InstanceOf (typeof (double)), "#3");
				Assert.That (np.PlaybackQueueIndex, Is.InstanceOf (typeof (int)), "#4");
				Assert.That (np.PlaybackQueueCount, Is.InstanceOf (typeof (int)), "#5");
				Assert.That (np.ChapterNumber, Is.InstanceOf (typeof (int)), "#6");
				Assert.That (np.ChapterCount, Is.InstanceOf (typeof (int)), "#7");

				if (v9_0) {
					Assert.That (np.AvailableLanguageOptions, Is.InstanceOf (typeof (MPNowPlayingInfoLanguageOptionGroup [])), "#8");
					Assert.That (np.CurrentLanguageOptions, Is.InstanceOf (typeof (MPNowPlayingInfoLanguageOption [])), "#9");
				}
				if (v10_0) {
					Assert.That ((object) np.CollectionIdentifier, Is.InstanceOf (typeof (string)), "#10");
					Assert.That ((object) np.ExternalContentIdentifier, Is.InstanceOf (typeof (string)), "#11");
					Assert.That ((object) np.ExternalUserProfileIdentifier, Is.InstanceOf (typeof (string)), "#12");
					Assert.That (np.PlaybackProgress, Is.InstanceOf (typeof (float)), "#13");
					Assert.That (np.MediaType, Is.InstanceOf (typeof (MPNowPlayingInfoMediaType)), "#14");
					Assert.That (np.IsLiveStream, Is.InstanceOf (typeof (bool)), "#15");
				}

				Assert.That ((object) np.AlbumTitle, Is.InstanceOf (typeof (string)), "#16");
				Assert.That (np.AlbumTrackCount, Is.InstanceOf (typeof (int)), "#17");
				Assert.That (np.AlbumTrackNumber, Is.InstanceOf (typeof (int)), "#18");
				Assert.That ((object) np.Artist, Is.InstanceOf (typeof (string)), "#19");
				Assert.That (np.Artwork, Is.InstanceOf (typeof (MPMediaItemArtwork)), "#20");
				Assert.That ((object) np.Composer, Is.InstanceOf (typeof (string)), "#21");
				Assert.That (np.DiscCount, Is.InstanceOf (typeof (int)), "#22");
				Assert.That (np.DiscNumber, Is.InstanceOf (typeof (int)), "#23");
				Assert.That ((object) np.Genre, Is.InstanceOf (typeof (string)), "#24");
				Assert.That (np.PersistentID, Is.InstanceOf (typeof (ulong)), "#25");
				Assert.That (np.PlaybackDuration, Is.InstanceOf (typeof (double)), "#26");
				Assert.That ((object) np.Title, Is.InstanceOf (typeof (string)), "#27");

				if (v10_3)
					Assert.That (np.AssetUrl, Is.InstanceOf (typeof (NSUrl)), "#28");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
