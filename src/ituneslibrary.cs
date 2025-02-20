// Copyright 2018-2019, Microsoft, Corp.
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

using System;
using System.ComponentModel;
using AppKit;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace iTunesLibrary {

	[BaseType (typeof (NSObject))]
	interface ITLibAlbum {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("title")]
		string Title { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortTitle")]
		string SortTitle { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("compilation")]
		bool Compilation { [Bind ("isCompilation")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("artist", ArgumentSemantic.Retain)]
		ITLibArtist Artist { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("discCount")]
		nuint DiscCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("discNumber")]
		nuint DiscNumber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rating")]
		nint Rating { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ratingComputed")]
		bool RatingComputed { [Bind ("isRatingComputed")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("gapless")]
		bool Gapless { [Bind ("isGapless")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("trackCount")]
		nuint TrackCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("albumArtist")]
		string AlbumArtist { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortAlbumArtist")]
		string SortAlbumArtist { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentID", ArgumentSemantic.Retain)]
		NSNumber PersistentId { get; }
	}

	[BaseType (typeof (NSObject))]
	interface ITLibArtist {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortName")]
		string SortName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentID", ArgumentSemantic.Retain)]
		NSNumber PersistentId { get; }
	}

	[BaseType (typeof (NSObject))]
	interface ITLibArtwork {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("image", ArgumentSemantic.Retain)]
		NSImage Image { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("imageData", ArgumentSemantic.Retain)]
		NSData ImageData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageDataFormat", ArgumentSemantic.Assign)]
		ITLibArtworkFormat ImageDataFormat { get; }
	}

	delegate void ITLibMediaEntityEnumerateValuesHandler (NSString property, NSObject value, out bool stop);

	[BaseType (typeof (NSObject))]
	interface ITLibMediaEntity {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentID", ArgumentSemantic.Retain)]
		NSNumber PersistentId { get; }

		[Export ("valueForProperty:")]
		[return: NullAllowed]
		NSObject GetValue (string property);

		[Export ("enumerateValuesForProperties:usingBlock:")]
		void EnumerateValues ([NullAllowed] NSSet<NSString> properties, ITLibMediaEntityEnumerateValuesHandler handler);

		[Export ("enumerateValuesExceptForProperties:usingBlock:")]
		void EnumerateValuesExcept ([NullAllowed] NSSet<NSString> properties, ITLibMediaEntityEnumerateValuesHandler handler);
	}

	[BaseType (typeof (ITLibMediaEntity))]
	interface ITLibMediaItem {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("title")]
		string Title { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortTitle")]
		string SortTitle { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("artist", ArgumentSemantic.Retain)]
		ITLibArtist Artist { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("composer")]
		string Composer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortComposer")]
		string SortComposer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rating")]
		nint Rating { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ratingComputed")]
		bool RatingComputed { [Bind ("isRatingComputed")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startTime")]
		nuint StartTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stopTime")]
		nuint StopTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("album", ArgumentSemantic.Retain)]
		ITLibAlbum Album { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("genre")]
		string Genre { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("kind")]
		string Kind { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaKind", ArgumentSemantic.Assign)]
		ITLibMediaItemMediaKind MediaKind { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fileSize")]
		ulong FileSize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("size")]
		nuint Size { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("totalTime")]
		nuint TotalTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("trackNumber")]
		nuint TrackNumber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("category")]
		string Category { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("description")]
		string Description { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lyricsContentRating", ArgumentSemantic.Assign)]
		ITLibMediaItemLyricsContentRating LyricsContentRating { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("contentRating")]
		string ContentRating { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("modifiedDate", ArgumentSemantic.Retain)]
		NSDate ModifiedDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("addedDate", ArgumentSemantic.Retain)]
		NSDate AddedDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bitrate")]
		nuint Bitrate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sampleRate")]
		nuint SampleRate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("beatsPerMinute")]
		nuint BeatsPerMinute { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playCount")]
		nuint PlayCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("lastPlayedDate", ArgumentSemantic.Retain)]
		NSDate LastPlayedDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playStatus", ArgumentSemantic.Assign)]
		ITLibMediaItemPlayStatus PlayStatus { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("location", ArgumentSemantic.Retain)]
		NSUrl Location { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("artworkAvailable")]
		bool ArtworkAvailable { [Bind ("hasArtworkAvailable")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("artwork", ArgumentSemantic.Retain)]
		ITLibArtwork Artwork { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("comments")]
		string Comments { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("purchased")]
		bool Purchased { [Bind ("isPurchased")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cloud")]
		bool Cloud { [Bind ("isCloud")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drmProtected")]
		bool DrmProtected { [Bind ("isDRMProtected")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("video")]
		bool Video { [Bind ("isVideo")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("videoInfo", ArgumentSemantic.Retain)]
		ITLibMediaItemVideoInfo VideoInfo { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("releaseDate", ArgumentSemantic.Retain)]
		NSDate ReleaseDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("year")]
		nuint Year { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fileType")]
		nuint FileType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("skipCount")]
		nuint SkipCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("skipDate", ArgumentSemantic.Retain)]
		NSDate SkipDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("voiceOverLanguage")]
		string VoiceOverLanguage { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("volumeAdjustment")]
		nint VolumeAdjustment { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("volumeNormalizationEnergy")]
		nuint VolumeNormalizationEnergy { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userDisabled")]
		bool UserDisabled { [Bind ("isUserDisabled")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("grouping")]
		string Grouping { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("locationType", ArgumentSemantic.Assign)]
		ITLibMediaItemLocationType LocationType { get; }
	}

	[BaseType (typeof (NSObject))]
	interface ITLibMediaItemVideoInfo {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("series")]
		string Series { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sortSeries")]
		string SortSeries { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("season")]
		nuint Season { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("episode")]
		string Episode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("episodeOrder")]
		nint EpisodeOrder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hd")]
		bool HD { [Bind ("isHD")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("videoWidth")]
		nuint VideoWidth { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("videoHeight")]
		nuint VideoHeight { get; }
	}

	[BaseType (typeof (ITLibMediaEntity))]
	interface ITLibPlaylist {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'Primary' instead.")]
		[Export ("master")]
		bool Master { [Bind ("isMaster")] get; }

		[Export ("primary")]
		bool Primary { [Bind ("isPrimary")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("parentID", ArgumentSemantic.Retain)]
		NSNumber ParentId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("visible")]
		bool Visible { [Bind ("isVisible")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allItemsPlaylist")]
		bool AllItemsPlaylist { [Bind ("isAllItemsPlaylist")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("items", ArgumentSemantic.Retain)]
		ITLibMediaItem [] Items { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("distinguishedKind", ArgumentSemantic.Assign)]
		ITLibDistinguishedPlaylistKind DistinguishedKind { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("kind", ArgumentSemantic.Assign)]
		ITLibPlaylistKind Kind { get; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ITLibrary {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("applicationVersion")]
		string ApplicationVersion { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("features", ArgumentSemantic.Assign)]
		ITLibExportFeature Features { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("apiMajorVersion")]
		nuint ApiMajorVersion { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("apiMinorVersion")]
		nuint ApiMinorVersion { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaFolderLocation", ArgumentSemantic.Copy)]
		NSUrl MediaFolderLocation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("musicFolderLocation", ArgumentSemantic.Copy)]
		NSUrl MusicFolderLocation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showContentRating")]
		bool ShowContentRating { [Bind ("shouldShowContentRating")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allMediaItems", ArgumentSemantic.Retain)]
		ITLibMediaItem [] AllMediaItems { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allPlaylists", ArgumentSemantic.Retain)]
		ITLibPlaylist [] AllPlaylists { get; }

		[Static]
		[Export ("libraryWithAPIVersion:error:")]
		[return: NullAllowed]
		ITLibrary GetLibrary (string requestedAPIVersion, [NullAllowed] out NSError error);

		[Static]
		[Export ("libraryWithAPIVersion:options:error:")]
		[return: NullAllowed]
		ITLibrary GetLibrary (string requestedAPIVersion, ITLibInitOptions options, [NullAllowed] out NSError error);

		[Export ("initWithAPIVersion:error:")]
		NativeHandle Constructor (string requestedAPIVersion, [NullAllowed] out NSError error);

		[DesignatedInitializer]
		[Export ("initWithAPIVersion:options:error:")]
		NativeHandle Constructor (string requestedAPIVersion, ITLibInitOptions options, [NullAllowed] out NSError error);

		[Export ("artworkForMediaFile:")]
		[return: NullAllowed]
		ITLibArtwork GetArtwork (NSUrl mediaFileUrl);

		[Export ("reloadData")]
		bool ReloadData ();

		[Export ("unloadData")]
		void UnloadData ();
	}

	[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
	[Static]
	interface ITLibraryNotifications {
		[Field ("ITLibraryDidChangeNotification")]
		[Notification]
		NSString DidChangeNotification { get; }
	}
}
