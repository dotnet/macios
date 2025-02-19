// 
// MPMediaItem.cs: 
//
// Authors:
//   Geoff Norton.
//   Miguel de Icaza
//     
// Copyright 2011 Novell, Inc
// Copyright 2011-2012 Xamarin, Inc
//

#if !MONOMAC

using System;
using System.Collections;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace MediaPlayer {
	public partial class MPMediaItem {
		ulong UInt64ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.UInt64Value;
		}

		uint UInt32ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.UInt32Value;
		}

		int Int32ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.Int32Value;
		}

		double DoubleForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.DoubleValue;
		}

		bool BoolForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return false;
			return prop.BoolValue;
		}

		/// <summary>The Persistent ID </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong PersistentID {
			get {
				return UInt64ForProperty (PersistentIDProperty);
			}
		}

		/// <summary>The Album's persistent id </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong AlbumPersistentID {
			get {
				return UInt64ForProperty (AlbumPersistentIDProperty);
			}
		}

		/// <summary>The Artist Persistent ID </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong ArtistPersistentID {
			get {
				return UInt64ForProperty (ArtistPersistentIDProperty);
			}
		}

		/// <summary>The Album's Artist Persistent ID </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong AlbumArtistPersistentID {
			get {
				return UInt64ForProperty (AlbumArtistPersistentIDProperty);
			}
		}

		/// <summary>The Genre Persistent ID </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong GenrePersistentID {
			get {
				return UInt64ForProperty (GenrePersistentIDProperty);
			}
		}

		/// <summary>The Composer Persistent ID  </summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong ComposerPersistentID {
			get {
				return UInt64ForProperty (ComposerPersistentIDProperty);
			}
		}

		/// <summary>The Podcast Persistent ID.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		public ulong PodcastPersistentID {
			get {
				return UInt64ForProperty (PodcastPersistentIDProperty);
			}
		}

		/// <summary>Media type for this media item.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public MPMediaType MediaType {
			get {
				return (MPMediaType) Int32ForProperty (MediaTypeProperty);
			}
		}

		/// <summary>The title for this media item.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Title {
			get {
				return ValueForProperty (TitleProperty) as NSString;
			}
		}

		/// <summary>The album title  </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? AlbumTitle {
			get {
				return ValueForProperty (AlbumTitleProperty) as NSString;
			}
		}

		/// <summary>The artist </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Artist {
			get {
				return ValueForProperty (ArtistProperty) as NSString;
			}
		}

		/// <summary>The Album's artist </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? AlbumArtist {
			get {
				return ValueForProperty (AlbumArtistProperty) as NSString;
			}
		}

		/// <summary>The genre.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Genre {
			get {
				return ValueForProperty (GenreProperty) as NSString;
			}
		}

		/// <summary>The composer </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Composer {
			get {
				return ValueForProperty (ComposerProperty) as NSString;
			}
		}

		/// <summary>Playback duration in seconds.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public double PlaybackDuration {
			get {
				return DoubleForProperty (PlaybackDurationProperty);
			}
		}

		/// <summary>The album track </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int AlbumTrackNumber {
			get {
				return Int32ForProperty (AlbumTrackNumberProperty);
			}
		}

		/// <summary>The album track count </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int AlbumTrackCount {
			get {
				return Int32ForProperty (AlbumTrackCountProperty);
			}
		}

		/// <summary>The disc number </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int DiscNumber {
			get {
				return Int32ForProperty (DiscNumberProperty);
			}
		}

		/// <summary>The number of discs in the album that contains this media item.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int DiscCount {
			get {
				return Int32ForProperty (DiscCountProperty);
			}
		}

		/// <summary>The artwork </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public MPMediaItemArtwork? Artwork {
			get {
				return (ValueForProperty (ArtworkProperty) as MPMediaItemArtwork);
			}
		}

		/// <summary>The lyrics.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Lyrics {
			get {
				return ValueForProperty (LyricsProperty) as NSString;
			}
		}

		/// <summary>Set to true if this is media item belongs to a compilation.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public bool IsCompilation {
			get {
				return Int32ForProperty (IsCompilationProperty) != 0;
			}
		}

		/// <summary>The release date for this media item.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDate? ReleaseDate {
			get {
				return (ValueForProperty (ReleaseDateProperty) as NSDate);
			}
		}

		/// <summary>The beats per minute </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public uint BeatsPerMinute {
			get {
				return UInt32ForProperty (BeatsPerMinuteProperty);
			}
		}

		/// <summary>The comments </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? Comments {
			get {
				return ValueForProperty (CommentsProperty) as NSString;
			}
		}

		/// <summary>The asset URL </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSUrl? AssetURL {
			get {
				return ValueForProperty (AssetURLProperty) as NSUrl;
			}
		}

		/// <summary>Number of times this item has been played.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int PlayCount {
			get {
				return Int32ForProperty (PlayCountProperty);
			}
		}

		/// <summary>Number of times this item has been skipped from playing back.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public int SkipCount {
			get {
				return Int32ForProperty (SkipCountProperty);
			}
		}

		/// <summary>The user assigned rating in the range zero to five.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public uint Rating {
			get {
				return UInt32ForProperty (RatingProperty);
			}
		}

		/// <summary>The last date that this item was played.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDate? LastPlayedDate {
			get {
				return (ValueForProperty (LastPlayedDateProperty) as NSDate);
			}
		}

		/// <summary>User defined grouping for this media item.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? UserGrouping {
			get {
				return ValueForProperty (UserGroupingProperty) as NSString;
			}
		}

		/// <summary>Title for the podcast.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString? PodcastTitle {
			get {
				return ValueForProperty (PodcastTitleProperty) as NSString;
			}
		}

		/// <summary>The user bookmark time </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public double BookmarkTime {
			get {
				return DoubleForProperty (BookmarkTimeProperty);
			}
		}

		/// <summary>Determines if the item is an iCloud item or not.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public bool IsCloudItem {
			get {
				return Int32ForProperty (IsCloudItemProperty) != 0;
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public bool HasProtectedAsset {
			get {
				return Int32ForProperty (HasProtectedAssetProperty) != 0;
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public bool IsExplicitItem {
			get {
				return Int32ForProperty (IsExplicitProperty) != 0;
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSDate? DateAdded {
			get {
				return (ValueForProperty (DateAddedProperty) as NSDate);
			}
		}

#if NET
		/// <summary>Gets the non-library ID.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSString? PlaybackStoreID {
			get {
				return (ValueForProperty (PlaybackStoreIDProperty) as NSString);
			}
		}

#if NET
		[SupportedOSPlatform ("tvos14.5")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.5")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[TV (14, 5)]
		[iOS (14, 5)]
#endif
		public bool IsPreorder {
			get {
				return BoolForProperty (IsPreorderProperty);
			}
		}
	}
}

#endif // !TVOS
