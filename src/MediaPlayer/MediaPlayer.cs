//
// This file contains definitions used in the MediaPlayer namespace
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2015 Xamarin, Inc.
//

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MediaPlayer {
	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration of possible states in which the <see cref="MediaPlayer.MPMoviePlayerController" /> may be. Used with the <see cref="MediaPlayer.MPMovieCotnroller.PlaybackState" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMoviePlaybackState : long {
		/// <summary>To be added.</summary>
		Stopped,
		/// <summary>To be added.</summary>
		Playing,
		/// <summary>To be added.</summary>
		Paused,
		/// <summary>To be added.</summary>
		Interrupted,
		/// <summary>To be added.</summary>
		SeekingForward,
		/// <summary>To be added.</summary>
		SeekingBackward,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration whose values reflect a movie's load state. Used in the <see cref="MediaPlayer.MPMoviePlayerController.LoadState" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieLoadState : long {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Playable = 1 << 0,
		/// <summary>To be added.</summary>
		PlaythroughOK = 1 << 1,
		/// <summary>To be added.</summary>
		Stalled = 1 << 2,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration that specifies whether a movie should repeat or not. Used with the <see cref="MediaPlayer.MPMoviePlayerController.RepeatMode" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieRepeatMode : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		One,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration whose values specify various modes for the <see cref="MediaPlayer.MPMoviePlayerController.ControlStyle" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieControlStyle : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Embedded,
		/// <summary>To be added.</summary>
		Fullscreen,
		/// <summary>To be added.</summary>
		Default = Embedded,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration whose values specify various ways a movie may have finished.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieFinishReason : long {
		/// <summary>To be added.</summary>
		PlaybackEnded,
		/// <summary>To be added.</summary>
		PlaybackError,
		/// <summary>To be added.</summary>
		UserExited,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration that specifies the movie's media types. Used with the <see cref="MediaPlayer.MPMoviePlayerController.MovieMediaTypes" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native ("MPMovieMediaTypeMask")]
	[Flags]
	public enum MPMovieMediaType : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Video = 1 << 0,
		/// <summary>To be added.</summary>
		Audio = 1 << 1,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration that specifies whether a movie's data is provided by a file or streaming. Used with the <see cref="MediaPlayer.MPMoviePlayerController.SourceType" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieSourceType : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		File,
		/// <summary>To be added.</summary>
		Streaming,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration that specifies which frame to use when generating thumbnails.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieTimeOption : long {
		/// <summary>To be added.</summary>
		NearestKeyFrame,
		/// <summary>To be added.</summary>
		Exact,
	}

	// NSUInteger -> MPMediaItem.h
	/// <summary>An enumeration whose values specify various types of media.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MPMediaType : ulong {
		/// <summary>To be added.</summary>
		Music = 1 << 0,
		/// <summary>To be added.</summary>
		Podcast = 1 << 1,
		/// <summary>To be added.</summary>
		AudioBook = 1 << 2,
		/// <summary>To be added.</summary>
		AudioITunesU = 1 << 3,
		/// <summary>To be added.</summary>
		AnyAudio = 0x00ff,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Movie = 1 << 8,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		TVShow = 1 << 9,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		VideoPodcast = 1 << 10,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MusicVideo = 1 << 11,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		VideoITunesU = 1 << 12,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		HomeVideo = 1 << 13,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		TypeAnyVideo = 0xff00,
		/// <summary>To be added.</summary>
		Any = 0xFFFFFFFFFFFFFFFF,
	}

	// NSInteger -> MPMediaPlaylist.h
	/// <summary>An enumeration whose values specify various types of playlist.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MPMediaPlaylistAttribute : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		OnTheGo = (1 << 0), // if set, the playlist was created on a device rather than synced from iTunes
		/// <summary>To be added.</summary>
		Smart = (1 << 1),
		/// <summary>To be added.</summary>
		Genius = (1 << 2),
	};

	// NSInteger -> MPMediaQuery.h
	/// <summary>An enumeration whose values specify various ways in which media can be logically grouped.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMediaGrouping : long {
		/// <summary>To be added.</summary>
		Title,
		/// <summary>To be added.</summary>
		Album,
		/// <summary>To be added.</summary>
		Artist,
		/// <summary>To be added.</summary>
		AlbumArtist,
		/// <summary>To be added.</summary>
		Composer,
		/// <summary>To be added.</summary>
		Genre,
		/// <summary>To be added.</summary>
		Playlist,
		/// <summary>To be added.</summary>
		PodcastTitle,
	}

	// NSInteger -> MPMediaQuery.h
	/// <summary>An enumeration whose values specifies a comparison-type to be used with a <see cref="MediaPlayer.MPMediaPredicate" />.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMediaPredicateComparison : long {
		/// <summary>To be added.</summary>
		EqualsTo,
		/// <summary>To be added.</summary>
		Contains,
	}

	// NSInteger -> MPMoviePlayerController.h
	/// <summary>An enumeration of video scaling modes. Used with the <see cref="MediaPlayer.MPMoviePlayerController.ScalingMode" /> property.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum MPMovieScalingMode : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		AspectFit,
		/// <summary>To be added.</summary>
		AspectFill,
		/// <summary>To be added.</summary>
		Fill,
	}

	// untyped enum -> MPMoviePlayerController.h
	/// <summary>Application developers should not use this deprecated class, but instead use <see cref="MediaPlayer.MPMovieControlStyle" />.</summary>
	/// <remarks>To be added.</remarks>
	[NoMac]
	[MacCatalyst (13, 1)]
	public enum MPMovieControlMode {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		VolumeOnly,
		/// <summary>To be added.</summary>
		Hidden,
	}

	// NSInteger -> /MPMusicPlayerController.h
	/// <summary>An enumeration of states in which the <see cref="MediaPlayer.MPMusicPlayerController" /> may be. Used with the <see cref="MediaPlayer.MPMusicPlayerController.PlaybackState" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMusicPlaybackState : long {
		/// <summary>To be added.</summary>
		Stopped,
		/// <summary>To be added.</summary>
		Playing,
		/// <summary>To be added.</summary>
		Paused,
		/// <summary>To be added.</summary>
		Interrupted,
		/// <summary>To be added.</summary>
		SeekingForward,
		/// <summary>To be added.</summary>
		SeekingBackward,
	}

	// NSInteger -> /MPMusicPlayerController.h
	/// <summary>An enumeration of music repeat modes. Used with the <see cref="MediaPlayer.MPMusicPlayerController.RepeatMode" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMusicRepeatMode : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		One,
		/// <summary>To be added.</summary>
		All,
	}

	// NSInteger -> /MPMusicPlayerController.h
	/// <summary>An enumeration of shuffle modes for use with the <see cref="MediaPlayer.MPMusicPlayerController.ShuffleMode" /> property.</summary>
	/// <remarks>To be added.</remarks>
	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMusicShuffleMode : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Off,
		/// <summary>To be added.</summary>
		Songs,
		/// <summary>To be added.</summary>
		Albums,
	}

	/// <param name="property">The property kind.</param>
	/// <param name="value">The value associated with the property</param>
	/// <param name="stop">Reference value, can be used to stop the enumeration.</param>
	/// <summary>The delegate to be used as the <c>enumerator</c> argument to <see cref="MediaPlayer.MPMediaItemCollection.EnumerateValues(Foundation.NSSet,MediaPlayer.MPMediaItemEnumerator)" />.</summary>
	/// <remarks>
	///     </remarks>
	public delegate void MPMediaItemEnumerator (string property, NSObject value, ref bool stop);

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPShuffleType : long {
		/// <summary>To be added.</summary>
		Off,
		/// <summary>To be added.</summary>
		Items,
		/// <summary>To be added.</summary>
		Collections,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPRepeatType : long {
		/// <summary>To be added.</summary>
		Off,
		/// <summary>To be added.</summary>
		One,
		/// <summary>To be added.</summary>
		All,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPChangeLanguageOptionSetting : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		NowPlayingItemOnly,
		/// <summary>To be added.</summary>
		Permanent,
	}

	// NSInteger -> MPRemoteCommand.h
	/// <summary>Enumerates values that indicate whether a command succeeded, failed, or cannot play the kind of media requested.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPRemoteCommandHandlerStatus : long {
		/// <summary>Indicates that the command succeeded.</summary>
		Success = 0,
		/// <summary>Indicates that there was no such content to play.</summary>
		NoSuchContent = 100,
		/// <summary>Indicates that no item was playing, so the command could not be meaningfuly received.</summary>
		[MacCatalyst (13, 1)]
		NoActionableNowPlayingItem = 110,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		DeviceNotFound = 120,
		/// <summary>Indicates that the command failed.</summary>
		CommandFailed = 200,
	}

	// NSUInteger -> MPRemoteCommandEvent.h
	/// <summary>Enumerates values that indicate whether the command began or ended a seek operation.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSeekCommandEventType : ulong {
		/// <summary>To be added.</summary>
		BeginSeeking,
		/// <summary>To be added.</summary>
		EndSeeking,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPNowPlayingInfoLanguageOptionType : ulong {
		/// <summary>To be added.</summary>
		Audible,
		/// <summary>To be added.</summary>
		Legible,
	}

	/// <summary>Enumerates error codes in the Media Player domain.</summary>
	/// <remarks>To be added.</remarks>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("MPErrorDomain")]
	public enum MPErrorCode : long {
		/// <summary>An unknown error occurred.</summary>
		Unknown,
		/// <summary>Permission was denied.</summary>
		PermissionDenied,
		/// <summary>iCloud services were not enabled.</summary>
		CloudServiceCapabilityMissing,
		/// <summary>A connection to the network could not be established.</summary>
		NetworkConnectionFailed,
		/// <summary>The identifer was not found in the store front.</summary>
		NotFound,
		/// <summary>The operation was not supported.</summary>
		NotSupported,
		/// <summary>The operation was cancelled.</summary>
		Cancelled,
		/// <summary>A request timed out.</summary>
		RequestTimedOut,
	}

	/// <summary>Enumerates the status of the application's permission to access the media library.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPMediaLibraryAuthorizationStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined = 0,
		/// <summary>The user has denied permission.</summary>
		Denied,
		/// <summary>Access is denied and the user may not change permission.</summary>
		Restricted,
		/// <summary>The user has granted permission.</summary>
		Authorized,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPNowPlayingInfoMediaType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Audio,
		/// <summary>To be added.</summary>
		Video,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPNowPlayingPlaybackState : ulong {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Playing,
		/// <summary>To be added.</summary>
		Paused,
		/// <summary>To be added.</summary>
		Stopped,
		/// <summary>To be added.</summary>
		Interrupted,
	}
}
