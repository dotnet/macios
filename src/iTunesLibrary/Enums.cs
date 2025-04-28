// Copyright 2018, Microsoft Corp.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
using System;
using Foundation;
using ObjCRuntime;

namespace iTunesLibrary {

	[Native]
	public enum ITLibArtworkFormat : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Bitmap = 1,
		/// <summary>To be added.</summary>
		Jpeg = 2,
		/// <summary>To be added.</summary>
		Jpeg2000 = 3,
		/// <summary>To be added.</summary>
		Gif = 4,
		/// <summary>To be added.</summary>
		Png = 5,
		/// <summary>To be added.</summary>
		Bmp = 6,
		/// <summary>To be added.</summary>
		Tiff = 7,
		/// <summary>To be added.</summary>
		Pict = 8,
	}

	[Native]
	public enum ITLibMediaItemMediaKind : ulong {
		/// <summary>To be added.</summary>
		Unknown = 1,
		/// <summary>To be added.</summary>
		Song = 2,
		/// <summary>To be added.</summary>
		Movie = 3,
		/// <summary>To be added.</summary>
		Podcast = 4,
		/// <summary>To be added.</summary>
		Audiobook = 5,
		/// <summary>To be added.</summary>
		PdfBooklet = 6,
		/// <summary>To be added.</summary>
		MusicVideo = 7,
		/// <summary>To be added.</summary>
		TVShow = 8,
		/// <summary>To be added.</summary>
		InteractiveBooklet = 9,
		/// <summary>To be added.</summary>
		HomeVideo = 12,
		/// <summary>To be added.</summary>
		Ringtone = 14,
		/// <summary>To be added.</summary>
		DigitalBooklet = 15,
		/// <summary>To be added.</summary>
		iOSApplication = 16,
		/// <summary>To be added.</summary>
		VoiceMemo = 17,
		/// <summary>To be added.</summary>
		iTunesU = 18,
		/// <summary>To be added.</summary>
		Book = 19,
		/// <summary>To be added.</summary>
		PdfBook = 20,
		/// <summary>To be added.</summary>
		AlertTone = 21,
	}

	[Native]
	public enum ITLibMediaItemLyricsContentRating : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Explicit = 1,
		/// <summary>To be added.</summary>
		Clean = 2,
	}

	[Native]
	public enum ITLibMediaItemLocationType : ulong {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		File = 1,
		/// <summary>To be added.</summary>
		Url = 2,
		/// <summary>To be added.</summary>
		Remote = 3,
	}

	[Native]
	public enum ITLibMediaItemPlayStatus : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		PartiallyPlayed = 1,
		/// <summary>To be added.</summary>
		Unplayed = 2,
	}

	[Native]
	public enum ITLibDistinguishedPlaylistKind : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Movies = 1,
		/// <summary>To be added.</summary>
		TVShows = 2,
		/// <summary>To be added.</summary>
		Music = 3,
		/// <summary>To be added.</summary>
		Audiobooks = 4,
		/// <summary>To be added.</summary>
		Books = Audiobooks,
		/// <summary>To be added.</summary>
		Ringtones = 5,
		/// <summary>To be added.</summary>
		Podcasts = 7,
		/// <summary>To be added.</summary>
		VoiceMemos = 14,
		/// <summary>To be added.</summary>
		Purchases = 16,
		/// <summary>To be added.</summary>
		iTunesU = 26,
		/// <summary>To be added.</summary>
		NightiesMusic = 42,
		/// <summary>To be added.</summary>
		MyTopRated = 43,
		/// <summary>To be added.</summary>
		Top25MostPlayed = 44,
		/// <summary>To be added.</summary>
		RecentlyPlayed = 45,
		/// <summary>To be added.</summary>
		RecentlyAdded = 46,
		/// <summary>To be added.</summary>
		MusicVideos = 47,
		/// <summary>To be added.</summary>
		ClassicalMusic = 48,
		/// <summary>To be added.</summary>
		LibraryMusicVideos = 49,
		/// <summary>To be added.</summary>
		HomeVideos = 50,
		/// <summary>To be added.</summary>
		Applications = 51,
		/// <summary>To be added.</summary>
		LovedSongs = 52,
		/// <summary>To be added.</summary>
		MusicShowsAndMovies = 53,
	}

	[Native]
	public enum ITLibPlaylistKind : ulong {
		/// <summary>To be added.</summary>
		Regular,
		/// <summary>To be added.</summary>
		Smart,
		/// <summary>To be added.</summary>
		Genius,
		/// <summary>To be added.</summary>
		Folder,
		/// <summary>To be added.</summary>
		GeniusMix,
	}

	[Native]
	public enum ITLibExportFeature : ulong {
		/// <summary>To be added.</summary>
		ITLibExportFeatureNone = 0,
	}

	[Native]
	public enum ITLibInitOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		LazyLoadData = 1,
	}

	public enum MediaItemProperty {
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumTitle")]
		AlbumTitle,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySortAlbumTitle")]
		SortAlbumTitle,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumArtist")]
		AlbumArtist,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumRating")]
		AlbumRating,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumRatingComputed")]
		AlbumRatingComputed,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySortAlbumArtist")]
		SortAlbumArtist,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumIsGapless")]
		AlbumIsGapless,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumIsCompilation")]
		AlbumIsCompilation,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumDiscCount")]
		AlbumDiscCount,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumDiscNumber")]
		AlbumDiscNumber,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAlbumTrackCount")]
		AlbumTrackCount,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyArtistName")]
		ArtistName,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySortArtistName")]
		SortArtistName,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoIsHD")]
		VideoIsHD,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoWidth")]
		VideoWidth,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoHeight")]
		VideoHeight,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoSeries")]
		VideoSeries,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoSortSeries")]
		VideoSortSeries,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoSeason")]
		VideoSeason,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoEpisode")]
		VideoEpisode,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVideoEpisodeOrder")]
		VideoEpisodeOrder,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyHasArtwork")]
		HasArtwork,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyBitRate")]
		BitRate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyBeatsPerMinute")]
		BeatsPerMinute,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyCategory")]
		Category,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyComments")]
		Comments,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyComposer")]
		Composer,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySortComposer")]
		SortComposer,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyContentRating")]
		ContentRating,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyLyricsContentRating")]
		LyricsContentRating,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyAddedDate")]
		AddedDate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyModifiedDate")]
		ModifiedDate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyDescription")]
		Description,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyIsUserDisabled")]
		IsUserDisabled,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyFileType")]
		FileType,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyGenre")]
		Genre,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyGrouping")]
		Grouping,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyIsVideo")]
		IsVideo,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyKind")]
		Kind,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyTitle")]
		Title,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySortTitle")]
		SortTitle,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVolumeNormalizationEnergy")]
		VolumeNormalizationEnergy,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyPlayCount")]
		PlayCount,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyLastPlayDate")]
		LastPlayDate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyPlayStatus")]
		PlayStatus,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyIsDRMProtected")]
		IsDrmProtected,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyIsPurchased")]
		IsPurchased,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyMovementCount")]
		MovementCount,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyMovementName")]
		MovementName,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyMovementNumber")]
		MovementNumber,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyRating")]
		Rating,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyRatingComputed")]
		RatingComputed,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyReleaseDate")]
		ReleaseDate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySampleRate")]
		SampleRate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySize")]
		Size,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyFileSize")]
		FileSize,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyUserSkipCount")]
		UserSkipCount,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertySkipDate")]
		SkipDate,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyStartTime")]
		StartTime,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyStopTime")]
		StopTime,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyTotalTime")]
		TotalTime,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyTrackNumber")]
		TrackNumber,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyLocationType")]
		LocationType,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVoiceOverLanguage")]
		VoiceOverLanguage,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyVolumeAdjustment")]
		VolumeAdjustment,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyWork")]
		Work,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyYear")]
		Year,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyMediaKind")]
		MediaKind,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyLocation")]
		Location,
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaItemPropertyArtwork")]
		Artwork,
	}

	public enum ITLibPlaylistProperty {
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyName")]
		Name,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyAllItemsPlaylist")]
		AllItemsPlaylist,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyDistinguisedKind")]
		DistinguisedKind,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'Primary' instead.")]
		[Field ("ITLibPlaylistPropertyMaster")]
		Master,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyParentPersistentID")]
		ParentPersistentId,
		[Field ("ITLibPlaylistPropertyPrimary")]
		Primary,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyVisible")]
		Visible,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyItems")]
		Items,
		/// <summary>To be added.</summary>
		[Field ("ITLibPlaylistPropertyKind")]
		Kind,
	}

	public enum ITLibMediaEntityProperty {
		/// <summary>To be added.</summary>
		[Field ("ITLibMediaEntityPropertyPersistentID")]
		PersistentId,
	}
}
