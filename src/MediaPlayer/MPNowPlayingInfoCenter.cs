//
// Bindings to the MPNowPlayingInfoCenter
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, Xamarin Inc
//

using System.Diagnostics.CodeAnalysis;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace MediaPlayer {
	/// <summary>Information relating to the <see cref="T:MediaPlayer.MPNowPlayingInfoCenter" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public class MPNowPlayingInfo {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MPNowPlayingInfo ()
		{
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double? ElapsedPlaybackTime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double? PlaybackRate;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? PlaybackQueueIndex;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? PlaybackQueueCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? ChapterNumber;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? ChapterCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? AlbumTrackCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? AlbumTrackNumber;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? DiscCount;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int? DiscNumber;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ulong? PersistentID;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double? PlaybackDuration;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public double? DefaultPlaybackRate;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public MPNowPlayingInfoLanguageOptionGroup []? AvailableLanguageOptions { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]

		public MPNowPlayingInfoLanguageOption []? CurrentLanguageOptions { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? CollectionIdentifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? ExternalContentIdentifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public string? ExternalUserProfileIdentifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public float? PlaybackProgress { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public MPNowPlayingInfoMediaType? MediaType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool? IsLiveStream { get; set; }

		/// <summary>Gets or sets the URL for the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSUrl? AssetUrl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public NSDate? CurrentPlaybackDate { get; set; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? AlbumTitle;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Artist;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public MPMediaItemArtwork? Artwork;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Composer;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Genre;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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
				AvailableLanguageOptions = NSArray.ArrayFromHandle<MPNowPlayingInfoLanguageOptionGroup> (result.Handle);
				GC.KeepAlive (result);
			}
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyCurrentLanguageOptions, out result)) {
				CurrentLanguageOptions = NSArray.ArrayFromHandle<MPNowPlayingInfoLanguageOption> (result.Handle);
				GC.KeepAlive (result);
			}
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyCollectionIdentifier, out result))
				CollectionIdentifier = (string) (result as NSString);
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyExternalContentIdentifier, out result))
				ExternalContentIdentifier = (string) (result as NSString);
			if (TryGetValue (source, MPNowPlayingInfoCenter.PropertyExternalUserProfileIdentifier, out result))
				ExternalUserProfileIdentifier = (string) (result as NSString);
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
				AlbumTitle = (string) (result as NSString);
			if (TryGetValue (source, MPMediaItem.ArtistProperty, out result))
				Artist = (string) (result as NSString);
			if (TryGetValue (source, MPMediaItem.ArtworkProperty, out result))
				Artwork = result as MPMediaItemArtwork;
			if (TryGetValue (source, MPMediaItem.ComposerProperty, out result))
				Composer = (string) (result as NSString);
			if (TryGetValue (source, MPMediaItem.GenreProperty, out result))
				Genre = (string) (result as NSString);
			if (TryGetValue (source, MPMediaItem.TitleProperty, out result))
				Title = (string) (result as NSString);
		}
	}

	/// <summary>A class that encapsulates data and functions relating to the "now-playing" information displayed on the device lock-screen, the television during AirPlay, or (potentially) on an external accessory such as a dock or car stereo.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPNowPlayingInfoCenter_Class/index.html">Apple documentation for <c>MPNowPlayingInfoCenter</c></related>
	public partial class MPNowPlayingInfoCenter {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
