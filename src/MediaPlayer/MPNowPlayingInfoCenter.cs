//
// Bindings to the MPNowPlayingInfoCenter
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, Xamarin Inc
//

using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace MediaPlayer {
	/// <summary>Provides strongly-typed access to "now playing" media information displayed on the lock screen, during AirPlay, or on external accessories.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPNowPlayingInfoCenter_Class/index.html">Apple documentation for <c>MPNowPlayingInfoCenter</c></related>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public class MPNowPlayingInfo {
		/// <summary>Creates a new empty <see cref="MPNowPlayingInfo" /> instance.</summary>
		public MPNowPlayingInfo ()
		{
		}

		/// <summary>Gets or sets the elapsed playback time, in seconds.</summary>
		public double? ElapsedPlaybackTime;
		/// <summary>Gets or sets the playback rate (1.0 = normal speed).</summary>
		public double? PlaybackRate;
		/// <summary>Gets or sets the index of the current item in the playback queue.</summary>
		public int? PlaybackQueueIndex;
		/// <summary>Gets or sets the total number of items in the playback queue.</summary>
		public int? PlaybackQueueCount;
		/// <summary>Gets or sets the current chapter number.</summary>
		public int? ChapterNumber;
		/// <summary>Gets or sets the total number of chapters.</summary>
		public int? ChapterCount;
		/// <summary>Gets or sets the total number of tracks in the album.</summary>
		public int? AlbumTrackCount;
		/// <summary>Gets or sets the track number within the album.</summary>
		public int? AlbumTrackNumber;
		/// <summary>Gets or sets the total number of discs in the album.</summary>
		public int? DiscCount;
		/// <summary>Gets or sets the disc number within the album.</summary>
		public int? DiscNumber;
		/// <summary>Gets or sets the persistent identifier for the media item.</summary>
		public ulong? PersistentID;
		/// <summary>Gets or sets the total duration of the media item, in seconds.</summary>
		public double? PlaybackDuration;

		/// <summary>Gets or sets the default playback rate.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public double? DefaultPlaybackRate;

		/// <summary>Gets or sets the available language options for the media item.</summary>
		/// <value>An array of language option groups, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public MPNowPlayingInfoLanguageOptionGroup []? AvailableLanguageOptions { get; set; }

		/// <summary>Gets or sets the currently selected language options for the media item.</summary>
		/// <value>An array of language options, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]

		public MPNowPlayingInfoLanguageOption []? CurrentLanguageOptions { get; set; }

		/// <summary>Gets or sets the identifier for the collection containing this media item.</summary>
		/// <value>The collection identifier string, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? CollectionIdentifier { get; set; }

		/// <summary>Gets or sets the external content identifier for the media item.</summary>
		/// <value>The external content identifier string, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? ExternalContentIdentifier { get; set; }

		/// <summary>Gets or sets the external user profile identifier associated with the media item.</summary>
		/// <value>The external user profile identifier string, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? ExternalUserProfileIdentifier { get; set; }

		/// <summary>Gets or sets the playback progress as a value between 0.0 and 1.0.</summary>
		/// <value>The playback progress, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public float? PlaybackProgress { get; set; }

		/// <summary>Gets or sets the media type of the now-playing item.</summary>
		/// <value>The media type, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public MPNowPlayingInfoMediaType? MediaType { get; set; }

		/// <summary>Gets or sets a value indicating whether the media item is a live stream.</summary>
		/// <value><see langword="true" /> if the item is a live stream; otherwise, <see langword="false" />. <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool? IsLiveStream { get; set; }

		/// <summary>Gets or sets the URL for the currently playing asset.</summary>
		/// <value>The asset URL, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSUrl? AssetUrl { get; set; }

		/// <summary>Gets or sets the date corresponding to the current playback position in the media item.</summary>
		/// <value>The current playback date, or <see langword="null" /> if not set.</value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public NSDate? CurrentPlaybackDate { get; set; }

		/// <summary>Gets or sets the title of the album.</summary>
		public string? AlbumTitle;
		/// <summary>Gets or sets the name of the artist.</summary>
		public string? Artist;
		/// <summary>Gets or sets the artwork image for the media item.</summary>
		public MPMediaItemArtwork? Artwork;
		/// <summary>Gets or sets the name of the composer.</summary>
		public string? Composer;
		/// <summary>Gets or sets the genre of the media item.</summary>
		public string? Genre;
		/// <summary>Gets or sets the title of the media item.</summary>
		public string? Title;

		internal NSDictionary ToDictionary ()
		{
			var dict = new NSMutableDictionary ();

			if (ElapsedPlaybackTime.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyElapsedPlaybackTime, new NSNumber (ElapsedPlaybackTime.Value));
			if (PlaybackRate.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyPlaybackRate, new NSNumber (PlaybackRate.Value));
			if (PlaybackQueueIndex.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyPlaybackQueueIndex, new NSNumber (PlaybackQueueIndex.Value));
			if (PlaybackQueueCount.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyPlaybackQueueCount, new NSNumber (PlaybackQueueCount.Value));
			if (ChapterNumber.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyChapterNumber, new NSNumber (ChapterNumber.Value));
			if (ChapterCount.HasValue)
				dict.Add (MPNowPlayingInfoCenter.PropertyChapterCount, new NSNumber (ChapterCount.Value));
			if (DefaultPlaybackRate.HasValue)
				Add (dict, MPNowPlayingInfoCenter.PropertyDefaultPlaybackRate, new NSNumber (DefaultPlaybackRate.Value));

			if (AvailableLanguageOptions is not null && AvailableLanguageOptions.Length != 0)
				Add (dict, MPNowPlayingInfoCenter.PropertyAvailableLanguageOptions, NSArray.FromObjects (AvailableLanguageOptions));
			if (CurrentLanguageOptions is not null && CurrentLanguageOptions.Length != 0)
				Add (dict, MPNowPlayingInfoCenter.PropertyCurrentLanguageOptions, NSArray.FromObjects (CurrentLanguageOptions));
			if (CollectionIdentifier is not null)
				Add (dict, MPNowPlayingInfoCenter.PropertyCollectionIdentifier, new NSString (CollectionIdentifier));
			if (ExternalContentIdentifier is not null)
				Add (dict, MPNowPlayingInfoCenter.PropertyExternalContentIdentifier, new NSString (ExternalContentIdentifier));
			if (ExternalUserProfileIdentifier is not null)
				Add (dict, MPNowPlayingInfoCenter.PropertyExternalUserProfileIdentifier, new NSString (ExternalUserProfileIdentifier));
			if (PlaybackProgress.HasValue)
				Add (dict, MPNowPlayingInfoCenter.PropertyPlaybackProgress, new NSNumber (PlaybackProgress.Value));
			if (MediaType.HasValue)
				Add (dict, MPNowPlayingInfoCenter.PropertyMediaType, new NSNumber ((int) MediaType.Value));
			if (IsLiveStream.HasValue)
				Add (dict, MPNowPlayingInfoCenter.PropertyIsLiveStream, new NSNumber (IsLiveStream.Value));
			if (AssetUrl is not null)
				Add (dict, MPNowPlayingInfoCenter.PropertyAssetUrl, AssetUrl);
			if (CurrentPlaybackDate is not null)
				Add (dict, MPNowPlayingInfoCenter.PropertyCurrentPlaybackDate, CurrentPlaybackDate);

			if (AlbumTrackCount.HasValue)
				dict.Add (MPMediaItem.AlbumTrackCountProperty, new NSNumber (AlbumTrackCount.Value));
			if (AlbumTrackNumber.HasValue)
				dict.Add (MPMediaItem.AlbumTrackNumberProperty, new NSNumber (AlbumTrackNumber.Value));
			if (DiscCount.HasValue)
				dict.Add (MPMediaItem.DiscCountProperty, new NSNumber (DiscCount.Value));
			if (DiscNumber.HasValue)
				dict.Add (MPMediaItem.DiscNumberProperty, new NSNumber (DiscNumber.Value));
			if (PersistentID.HasValue)
				dict.Add (MPMediaItem.PersistentIDProperty, new NSNumber (PersistentID.Value));
			if (PlaybackDuration.HasValue)
				dict.Add (MPMediaItem.PlaybackDurationProperty, new NSNumber (PlaybackDuration.Value));

			if (AlbumTitle is not null)
				dict.Add (MPMediaItem.AlbumTitleProperty, new NSString (AlbumTitle));
			if (Artist is not null)
				dict.Add (MPMediaItem.ArtistProperty, new NSString (Artist));
			if (Artwork is not null)
				dict.Add (MPMediaItem.ArtworkProperty, Artwork);
			if (Composer is not null)
				dict.Add (MPMediaItem.ComposerProperty, new NSString (Composer));
			if (Genre is not null)
				dict.Add (MPMediaItem.GenreProperty, new NSString (Genre));
			if (Title is not null)
				dict.Add (MPMediaItem.TitleProperty, new NSString (Title));

			return dict;
		}

		void Add (NSMutableDictionary dictionary, NSObject key, NSObject value)
		{
			if (key is not null)
				dictionary.Add (key, value);
		}

		bool TryGetValue (NSDictionary source, NSObject? key, [NotNullWhen (true)] out NSObject? result)
		{
			if (key is not null)
				return source.TryGetValue (key, out result);
			result = null;
			return false;
		}

		internal MPNowPlayingInfo (NSDictionary? source)
		{
			if (source is null)
				return;

			NSObject? result;

			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyElapsedPlaybackTime, out result))
				ElapsedPlaybackTime = (result as NSNumber)?.DoubleValue;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyPlaybackRate, out result))
				PlaybackRate = (result as NSNumber)?.DoubleValue;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyPlaybackQueueIndex, out result))
				PlaybackQueueIndex = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyPlaybackQueueCount, out result))
				PlaybackQueueCount = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyChapterNumber, out result))
				ChapterNumber = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyChapterCount, out result))
				ChapterCount = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyDefaultPlaybackRate, out result))
				DefaultPlaybackRate = (result as NSNumber)?.DoubleValue;

			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyAvailableLanguageOptions, out result)) {
				AvailableLanguageOptions = NSArray.ArrayFromHandleDropNullElements<MPNowPlayingInfoLanguageOptionGroup> (result.Handle);
				GC.KeepAlive (result);
			}
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyCurrentLanguageOptions, out result)) {
				CurrentLanguageOptions = NSArray.ArrayFromHandleDropNullElements<MPNowPlayingInfoLanguageOption> (result.Handle);
				GC.KeepAlive (result);
			}
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyCollectionIdentifier, out result))
				CollectionIdentifier = (string?) (result as NSString);
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyExternalContentIdentifier, out result))
				ExternalContentIdentifier = (string?) (result as NSString);
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyExternalUserProfileIdentifier, out result))
				ExternalUserProfileIdentifier = (string?) (result as NSString);
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyPlaybackProgress, out result))
				PlaybackProgress = (result as NSNumber)?.FloatValue;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyMediaType, out result))
				MediaType = (MPNowPlayingInfoMediaType?) (result as NSNumber)?.UInt32Value;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyIsLiveStream, out result))
				IsLiveStream = (result as NSNumber)?.BoolValue;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyAssetUrl, out result))
				AssetUrl = result as NSUrl;
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyCurrentPlaybackDate, out result))
				CurrentPlaybackDate = result as NSDate;

			if (TryGetValue (source, MPMediaItem.AlbumTrackCountProperty, out result))
				AlbumTrackCount = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPMediaItem.AlbumTrackNumberProperty, out result))
				AlbumTrackNumber = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPMediaItem.DiscCountProperty, out result))
				DiscCount = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPMediaItem.DiscNumberProperty, out result))
				DiscNumber = (result as NSNumber)?.Int32Value;
			if (TryGetValue (source, MPMediaItem.PersistentIDProperty, out result))
				PersistentID = (result as NSNumber)?.UInt64Value;
			if (TryGetValue (source, MPMediaItem.PlaybackDurationProperty, out result))
				PlaybackDuration = (result as NSNumber)?.DoubleValue;

			if (TryGetValue (source, MPMediaItem.AlbumTitleProperty, out result))
				AlbumTitle = (string?) (result as NSString);
			if (TryGetValue (source, MPMediaItem.ArtistProperty, out result))
				Artist = (string?) (result as NSString);
			if (TryGetValue (source, MPMediaItem.ArtworkProperty, out result))
				Artwork = result as MPMediaItemArtwork;
			if (TryGetValue (source, MPMediaItem.ComposerProperty, out result))
				Composer = (string?) (result as NSString);
			if (TryGetValue (source, MPMediaItem.GenreProperty, out result))
				Genre = (string?) (result as NSString);
			if (TryGetValue (source, MPMediaItem.TitleProperty, out result))
				Title = (string?) (result as NSString);
		}
	}

	/// <summary>Provides access to the "now playing" information displayed on the device lock screen, during AirPlay, or on external accessories such as a dock or car stereo.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPNowPlayingInfoCenter_Class/index.html">Apple documentation for <c>MPNowPlayingInfoCenter</c></related>
	public partial class MPNowPlayingInfoCenter {

		/// <summary>Gets or sets the strongly-typed now-playing information.</summary>
		/// <value>The current <see cref="MPNowPlayingInfo" />, or an empty instance if no information is set.</value>
		public MPNowPlayingInfo NowPlaying {
			get {
				return new MPNowPlayingInfo (_NowPlayingInfo);
			}
			set {
				_NowPlayingInfo = value?.ToDictionary ();
			}
		}
	}
}
