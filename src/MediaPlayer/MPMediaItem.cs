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
	/// <include file="../../docs/api/MediaPlayer/MPMediaItem.xml" path="/Documentation/Docs[@DocId='T:MediaPlayer.MPMediaItem']/*" />
	public partial class MPMediaItem {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		ulong UInt64ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.UInt64Value;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		uint UInt32ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.UInt32Value;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		int Int32ForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.Int32Value;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		double DoubleForProperty (NSString property)
		{
			var prop = ValueForProperty (property) as NSNumber;
			if (prop is null)
				return 0;
			return prop.DoubleValue;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public bool IsCloudItem {
			get {
				return Int32ForProperty (IsCloudItemProperty) != 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool HasProtectedAsset {
			get {
				return Int32ForProperty (HasProtectedAssetProperty) != 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool IsExplicitItem {
			get {
				return Int32ForProperty (IsExplicitProperty) != 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSDate? DateAdded {
			get {
				return (ValueForProperty (DateAddedProperty) as NSDate);
			}
		}

		/// <summary>Gets the non-library ID.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSString? PlaybackStoreID {
			get {
				return (ValueForProperty (PlaybackStoreIDProperty) as NSString);
			}
		}

		[SupportedOSPlatform ("tvos14.5")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.5")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsPreorder {
			get {
				return BoolForProperty (IsPreorderProperty);
			}
		}
	}
}

#endif // !TVOS
