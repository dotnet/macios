using System;
using ObjCRuntime;
using Foundation;

namespace Photos {
	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that control how images are displayed.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHImageContentMode : long {
		/// <summary>Scales the image to fill the target area and crops any excess.</summary>
		AspectFit = 0,
		/// <summary>Scales the image so that one dimension completely fills the target area in one direction. The other dimension may be padded, and the entire image will be visible.</summary>
		AspectFill = 1,
		/// <summary>The image is neither scaled nor cropped.</summary>
		Default = AspectFit,
	}

	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that control whether to retrieve edited or unedited versions of images.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHImageRequestOptionsVersion : long {
		/// <summary>Return the edited version of the image.</summary>
		Current = 0,
		/// <summary>Return the unadjusted image.</summary>
		Unadjusted,
		/// <summary>Return the highest quality version of the image.</summary>
		Original,
	}

	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that control the desired balance between speed and quality when retrieving image data.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHImageRequestOptionsDeliveryMode : long {
		/// <summary>Progressively obtain the highest available quality by repeatedly calling the image loading handler.</summary>
		Opportunistic = 0,
		/// <summary>Obtain the highest quality image available, regardless of delivery time.</summary>
		HighQualityFormat = 1,
		/// <summary>Obtain a low quality image if a high quality image is not immediately available.</summary>
		FastFormat = 2,
	}

	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that control the speed and accuracy of image resizing operations.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHImageRequestOptionsResizeMode : long {
		/// <summary>Do not resize the image.</summary>
		None = 0,
		/// <summary>Resize the image approximately, but quickly.</summary>
		Fast,
		/// <summary>Resize the image exactly.</summary>
		Exact,
	}

	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that control whether to return the edited or original version of a video asset.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHVideoRequestOptionsVersion : long {
		/// <summary>Return the current version, with edits.</summary>
		Current = 0,
		/// <summary>Return the original version, without edits.</summary>
		Original,
	}

	// NSInteger -> PHImageManager.h
	/// <summary>Enumerates values that balance the load time and quality of video when requesting video data.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHVideoRequestOptionsDeliveryMode : long {
		/// <summary>Photos returns the data that is most appropriate for the request and network conditions.</summary>
		Automatic = 0,
		/// <summary>Photos provides high quality, but possibly larger video data..</summary>
		HighQualityFormat = 1,
		/// <summary>Photos provides medium quality image data.</summary>
		MediumQualityFormat = 2,
		/// <summary>Photos provides fast, lower quality video data.</summary>
		FastFormat = 3,
	}

	// NSInteger -> PhotosTypes.h
	/// <summary>Enumerates values that indicate whether a collection is a moment list, folder, or smart folder.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHCollectionListType : long {
		/// <summary>The collection is a moment list.</summary>
		[Deprecated (PlatformName.iOS, 13, 0)]
		[Deprecated (PlatformName.TvOS, 13, 0)]
		[Unavailable (PlatformName.MacOSX)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		MomentList = 1,
		/// <summary>The collection is a folder.</summary>
		Folder = 2,
		/// <summary>The collection is a smart folder.</summary>
		SmartFolder = 3,
	}

	/// <summary>Enumerates values that indicate the subtype of the collection.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHCollectionListSubtype : long {
		/// <summary>The collection is a moment list cluster.</summary>
		[Deprecated (PlatformName.iOS, 13, 0)]
		[Deprecated (PlatformName.TvOS, 13, 0)]
		[Unavailable (PlatformName.MacOSX)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		MomentListCluster = 1,

		/// <summary>The collection is a moment list year.</summary>
		[Deprecated (PlatformName.iOS, 13, 0)]
		[Deprecated (PlatformName.TvOS, 13, 0)]
		[Unavailable (PlatformName.MacOSX)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		MomentListYear = 2,

		/// <summary>The collection is a regular folder.</summary>
		RegularFolder = 100,

		/// <summary>The collection is a smart folder of events.</summary>
		SmartFolderEvents = 200,
		/// <summary>The collection is a smart folder of faces.</summary>
		SmartFolderFaces = 201,

		/// <summary>The collection subtype is unspecified.</summary>
		Any = Int64.MaxValue,
	}

	// NSUInteger -> PhotosTypes.h
	/// <summary>Enumerates values that describe the editing operations that can be performed on a collection.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHCollectionEditOperation : long {
		/// <summary>No capabilities are specified.</summary>
		None = 0,
		/// <summary>Content can be deleted from the collection.</summary>
		DeleteContent = 1,
		/// <summary>Content can be removed from the collection without being permanently deleted.</summary>
		RemoveContent = 2,
		/// <summary>Content can be added to the collection.</summary>
		AddContent = 3,
		/// <summary>The collection can create new content or duplicate existing content.</summary>
		CreateContent = 4,
		/// <summary>Content can be rearranged within the collection.</summary>
		RearrangeContent = 5,
		/// <summary>The collection can be deleted.</summary>
		Delete = 6,
		/// <summary>Content within the collection can be renamed.</summary>
		Rename = 7,
	}

	// NSInteger -> PhotosTypes.h
	/// <summary>Enumerates varieties of <see cref="T:Photos.PHAssetCollection" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetCollectionType : long {
		/// <summary>A collection of related songs.</summary>
		Album = 1,
		/// <summary>A collection of songs whose relatedness was algorithmically determined.</summary>
		SmartAlbum = 2,

		/// <summary>A collection of photos taken at a particular time.</summary>
		[Deprecated (PlatformName.iOS, 13, 0)]
		[Deprecated (PlatformName.TvOS, 13, 0)]
		[Unavailable (PlatformName.MacOSX)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		Moment = 3,
	}

	// NSInteger -> PhotosTypes.h
	/// <summary>Enumerates values that describe the particular subtype (For example, time lapses, bursts, shared collections in the cloud, and etc.) of an asset collection.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetCollectionSubtype : long {
		/// <summary>Album created in Photos app.</summary>
		AlbumRegular = 2,
		/// <summary>An event synced to the device from iPhoto.</summary>
		AlbumSyncedEvent = 3,
		/// <summary>A faces group synced from iPhoto.</summary>
		AlbumSyncedFaces = 4,
		/// <summary>An album synced to the device from iPhoto.</summary>
		AlbumSyncedAlbum = 5,
		/// <summary>Album imported from a camera or other external device.</summary>
		AlbumImported = 6,
		/// <summary>The user's iCloud Photo Stream.</summary>
		AlbumMyPhotoStream = 100,
		/// <summary>A shared iCloud album.</summary>
		AlbumCloudShared = 101,
		/// <summary>A smart album of no particular subtype.</summary>
		SmartAlbumGeneric = 200,
		/// <summary>A smart album that holds all the panoramas in the library.</summary>
		SmartAlbumPanoramas = 201,
		/// <summary>A smart album that holds all the videos in the library.</summary>
		SmartAlbumVideos = 202,
		/// <summary>A smart album that holds all assets marked as a favorite.</summary>
		SmartAlbumFavorites = 203,
		/// <summary>A smart album that contains all the timelapse videos in the library.</summary>
		SmartAlbumTimelapses = 204,
		/// <summary>A smart album that holds all assets hidden from Moments view.</summary>
		SmartAlbumAllHidden = 205,
		/// <summary>A smart album that holds recently added assets.</summary>
		SmartAlbumRecentlyAdded = 206,
		/// <summary>A smart album that holds all the burst sequences in the library.</summary>
		SmartAlbumBursts = 207,
		/// <summary>A smart album that contains all the slow-motion videos in the library.</summary>
		SmartAlbumSlomoVideos = 208,
		/// <summary>A smart album that holds all the assets created by the user (as opposed to, for instance, iCloud Shared Albums).</summary>
		SmartAlbumUserLibrary = 209,
		/// <summary>A smart album that holds self portraits.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumSelfPortraits = 210,
		/// <summary>A smart album that holds screenshots.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumScreenshots = 211,
		/// <summary>A smart album that groups Depth Effect images.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumDepthEffect = 212,
		/// <summary>A smart album that groups Live Photo images.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumLivePhotos = 213,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumAnimated = 214,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SmartAlbumLongExposures = 215,
		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		SmartAlbumUnableToUpload = 216,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		SmartAlbumRAW = 217,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		SmartAlbumCinematic = 218,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		SmartAlbumSpatial = 219,

		/// <summary>A bitmask of all possible subtypes.</summary>
		Any = Int64.MaxValue,
	}

	// NSUInteger -> PhotosTypes.h
	/// <summary>Enumerates values that indicate whether an operation edits or deletes an asset, changes its properties, or performs no action on the asset.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetEditOperation : long {
		/// <summary>The edit performs no operation on the asset.</summary>
		None = 0,
		/// <summary>The edit deletes the asset.</summary>
		Delete = 1,
		/// <summary>The edit changes the content of the asset.</summary>
		Content = 2,
		/// <summary>The edit changes the asset's properties.</summary>
		Properties = 3,
	}

	// NSInteger -> PhotosTypes.h
	/// <summary>Enumerates the forms of <see cref="T:Photos.PHAsset" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetMediaType : long {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Image = 1,
		/// <summary>To be added.</summary>
		Video = 2,
		/// <summary>To be added.</summary>
		Audio = 3,
	}

	// NSUInteger -> PhotosTypes.h
	/// <summary>Enumerates values that describe media subtypes. (HDR, panorama, streaming video, and etc.)</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum PHAssetMediaSubtype : ulong {
		/// <summary>No specific subtype.</summary>
		None = 0,
		/// <summary>A panoramic photo.</summary>
		PhotoPanorama = (1 << 0),
		/// <summary>A high-dynamic-range photo.</summary>
		PhotoHDR = (1 << 1),
		/// <summary>A screenshot.</summary>
		[MacCatalyst (13, 1)]
		Screenshot = (1 << 2),
		/// <summary>A Live Photo.</summary>
		[MacCatalyst (13, 1)]
		PhotoLive = (1 << 3),
		/// <summary>A Depth Effect photo.</summary>
		[MacCatalyst (13, 1)]
		PhotoDepthEffect = (1 << 4),
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		SmartAlbumSpatial = (1 << 10),
		/// <summary>A streaming video.</summary>
		VideoStreamed = (1 << 16),
		/// <summary>A video recorded at a frame rate greater than 30FPS.</summary>
		VideoHighFrameRate = (1 << 17),
		/// <summary>A timelapse video.</summary>
		VideoTimelapse = (1 << 18),
		VideoCinematic = (1 << 21),
	}

	// NSUInteger -> PhotosTypes.h
	/// <summary>Indicates whether the Photos app or the user selected an asset as a favorite.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum PHAssetBurstSelectionType : ulong {
		/// <summary>The asset was not picked as a favorite.</summary>
		None = 0,
		/// <summary>The Photos app picked the asset as a favorite.</summary>
		AutoPick = (1 << 0),
		/// <summary>The user picked the asset as a favorite.</summary>
		UserPick = (1 << 1),
	}

	/// <summary>Enumerates the current authorization allowed by the application user.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAuthorizationStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined,
		/// <summary>Access is denied and the user cannot change permission.</summary>
		Restricted,
		/// <summary>The user has denied access.</summary>
		Denied,
		/// <summary>The user has granted access.</summary>
		Authorized,
		[iOS (14, 0)]
		[NoTV]
		[NoMac]
		[MacCatalyst (14, 0)]
		Limited,
	}

	/// <summary>Enumerates types of <see cref="T:Photos.PHAssetResource" /> data.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetResourceType : long {
		/// <summary>Photo data.</summary>
		Photo = 1,
		/// <summary>Video data.</summary>
		Video = 2,
		/// <summary>Audio data.</summary>
		Audio = 3,
		/// <summary>Photo data in an alternate format (such as JPEG for a RAW photo).</summary>
		AlternatePhoto = 4,
		/// <summary>Photo data in the highest quality and size available.</summary>
		FullSizePhoto = 5,
		/// <summary>Video data in the highest quality and size available.</summary>
		FullSizeVideo = 6,
		/// <summary>Data used to reconstruct edits to an asset.</summary>
		AdjustmentData = 7,
		/// <summary>An unaltered copy of the original photo.</summary>
		AdjustmentBasePhoto = 8,
		/// <summary>Original video data for a Live Photo.</summary>
		[MacCatalyst (13, 1)]
		PairedVideo = 9,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		FullSizePairedVideo = 10,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		AdjustmentBasePairedVideo = 11,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		AdjustmentBaseVideo = 12,
		[iOS (17, 0), TV (17, 0)]
		[MacCatalyst (17, 0), Mac (14, 0)]
		PhotoProxy = 19,
	}

	/// <summary>Enumerates the means by which an asset entered the Photos library.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetSourceType : ulong {
		/// <summary>Unknown.</summary>
		None = 0,
		/// <summary>From the user's manin Photos library.</summary>
		UserLibrary = (1 << 0),
		/// <summary>An iCloud shared album.</summary>
		CloudShared = (1 << 1),
		/// <summary>Synced via iTunes.</summary>
		iTunesSynced = (1 << 2),
	}

	/// <summary>Enumerates Live Photo frame types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHLivePhotoFrameType : long {
		/// <summary>Indicates that the frame is a still photo.</summary>
		Photo,
		/// <summary>Indicates that the frame is a video frame.</summary>
		Video,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum PHAssetPlaybackStyle : long {
		/// <summary>To be added.</summary>
		Unsupported = 0,
		/// <summary>To be added.</summary>
		Image = 1,
		/// <summary>To be added.</summary>
		ImageAnimated = 2,
		/// <summary>To be added.</summary>
		LivePhoto = 3,
		/// <summary>To be added.</summary>
		Video = 4,
		/// <summary>To be added.</summary>
		VideoLooping = 5,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum PHProjectTextElementType : long {
		/// <summary>To be added.</summary>
		Body = 0,
		/// <summary>To be added.</summary>
		Title,
		/// <summary>To be added.</summary>
		Subtitle,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum PHProjectCreationSource : long {
		/// <summary>To be added.</summary>
		Undefined = 0,
		/// <summary>To be added.</summary>
		UserSelection = 1,
		/// <summary>To be added.</summary>
		Album = 2,
		/// <summary>To be added.</summary>
		Memory = 3,
		/// <summary>To be added.</summary>
		Moment = 4,
		/// <summary>To be added.</summary>
		Project = 20,
		/// <summary>To be added.</summary>
		ProjectBook = 21,
		/// <summary>To be added.</summary>
		ProjectCalendar = 22,
		/// <summary>To be added.</summary>
		ProjectCard = 23,
		/// <summary>To be added.</summary>
		ProjectPrintOrder = 24,
		/// <summary>To be added.</summary>
		ProjectSlideshow = 25,
		/// <summary>To be added.</summary>
		ProjectExtension = 26,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum PHProjectSectionType : long {
		/// <summary>To be added.</summary>
		Undefined = 0,
		/// <summary>To be added.</summary>
		Cover = 1,
		/// <summary>To be added.</summary>
		Content = 2,
		/// <summary>To be added.</summary>
		Auxiliary = 3,
	}

	[NoMacCatalyst]
	[NoiOS]
	[NoTV]
	[Native ("PHLivePhotoEditingErrorCode")]
	[ErrorDomain ("PHLivePhotoEditingErrorDomain")]
	public enum PHLivePhotoEditingError : long {
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'PHPhotosError.InternalError' instead.")]
		Unknown,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'PHPhotosError.UserCancelled' instead.")]
		Aborted,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	public enum FigExifCustomRenderedValue : short {
		/// <summary>To be added.</summary>
		NotCustom = 0,
		/// <summary>To be added.</summary>
		Custom = 1,
		/// <summary>To be added.</summary>
		HdrImage = 2,
		/// <summary>To be added.</summary>
		HdrPlusEV0_HdrImage = 3,
		/// <summary>To be added.</summary>
		HdrPlusEV0_EV0Image = 4,
		/// <summary>To be added.</summary>
		PanoramaImage = 6,
		/// <summary>To be added.</summary>
		SdofImage = 7,
		/// <summary>To be added.</summary>
		SdofPlusOriginal_SdofImage = 8,
		/// <summary>To be added.</summary>
		SdofPlusOriginal_OriginalImage = 9,
	}

	[ErrorDomain ("PHPhotosErrorDomain")]
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHPhotosError : long {
#if !NET
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'InternalError' instead.")]
		Invalid = -1,
#endif
		InternalError = -1,
		UserCancelled = 3072,
		LibraryVolumeOffline = 3114,
		RelinquishingLibraryBundleToWriter = 3142,
		SwitchingSystemPhotoLibrary = 3143,
		NetworkAccessRequired = 3164,
		NetworkError = 3169,
		IdentifierNotFound = 3201,
		MultipleIdentifiersFound = 3202,
		ChangeNotSupported = 3300,
		OperationInterrupted = 3301,
		InvalidResource = 3302,
		MissingResource = 3303,
		NotEnoughSpace = 3305,
		RequestNotSupportedForAsset = 3306,
		AccessRestricted = 3310,
		AccessUserDenied = 3311,
		LibraryInFileProviderSyncRoot = 5423,
		PersistentChangeTokenExpired = 3105,
		PersistentChangeDetailsUnavailable = 3210,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum PHAccessLevel : long {
		AddOnly = 1,
		ReadWrite = 2,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0)]
	[MacCatalyst (16, 0)]
	[Native]
	public enum PHObjectType : long {
		Asset = 1,
		AssetCollection = 2,
		CollectionList = 3,
	}
}
