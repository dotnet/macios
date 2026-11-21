
namespace PhotosUI {

	/// <summary>Enumerates how live photo playback will occur in a <see cref="PhotosUI.PHLivePhotoView" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PHLivePhotoViewPlaybackStyle : long {
		/// <summary>This is an invalid value. Do not use.</summary>
		Undefined = 0,
		/// <summary>Indicates that the full Live Photo, with sound, will be played.</summary>
		Full,
		/// <summary>Indicates that a short teaser, without sound, will be played.</summary>
		Hint,
	}

#if MONOMAC
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum PHLivePhotoViewContentMode : long {
		/// <summary>To be added.</summary>
		AspectFit,
		/// <summary>To be added.</summary>
		AspectFill,
	}
#else
	/// <summary>Flagging enumeration that controls appearance of the playback badge on <see cref="PhotosUI.PHLivePhotoView" /> objects.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags] // NS_OPTIONS
	public enum PHLivePhotoBadgeOptions : ulong {
		/// <summary>Indicates no badge.</summary>
		None = 0,
		/// <summary>Indicates a badge that will be placed over animated content.</summary>
		OverContent = 1 << 0,
		/// <summary>Indicates a badge for assets for which Live Photo content is disabled.</summary>
		LiveOff = 1 << 1,
	}
#endif
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	public enum PHProjectCategory {
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryBook")]
		Book,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryCalendar")]
		Calendar,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryCard")]
		Card,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryPrints")]
		Prints,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategorySlideshow")]
		Slideshow,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryWallDecor")]
		WallDecor,
		/// <summary>To be added.</summary>
		[Field ("PHProjectCategoryOther")]
		Other,
		[NoMacCatalyst]
		[Field ("PHProjectCategoryUndefined")]
		Undefined,
	}

	[NoTV]
	[iOS (14, 0)]
	[Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum PHPickerConfigurationAssetRepresentationMode : long {
		Automatic = 0,
		Current = 1,
		Compatible = 2,
	}

	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum PHPickerMode : long {
		Default = 0,
		Compact = 1,
	}

	[Flags, NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum PHPickerCapabilities : ulong {
		None = 0,
		Search = 1 << 0,
		StagingArea = 1 << 1,
		CollectionNavigation = 1 << 2,
		SelectionActions = 1 << 3,
		SensitivityAnalysisIntervention = 1uL << 4,
		/// <summary>Automatically updates whether the picker can be dismissed interactively.</summary>
		[Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
		AutomaticModalInPresentation = 1uL << 5,
		/// <summary>Automatically updates the picker's preferred content size.</summary>
		[Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
		AutomaticPreferredContentSize = 1uL << 6,
		/// <summary>The sidebar used for navigation.</summary>
		[Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
		Sidebar = 1uL << 7,
	}

	[NoTV, Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
	[Flags]
	[Native]
	public enum PHPickerMetadataOptions : ulong {
		None = 0,
		RemoveLocation = 1 << 0,
		RemoveCaptions = 1 << 1,
		/// <summary>Removes metadata marking an asset as an Apple Reference Image.</summary>
		[Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
		RemoveAppleReferenceImage = 1uL << 2,
		/// <summary>Removes keyword metadata.</summary>
		RemoveKeywords = 1uL << 3,
	}

	/// <summary>Specifies the recommended minimum dimension of a picker result thumbnail.</summary>
	[NoTV, Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
	[Native]
	public enum PHPickerResultThumbnailSize : long {
		/// <summary>A minimum dimension of approximately 100 pixels.</summary>
		Small = 1,
		/// <summary>A minimum dimension of approximately 360 pixels.</summary>
		Standard = 2,
	}

	/// <summary>Specifies the primary button displayed by a photo picker.</summary>
	[NoTV, Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
	[Native]
	public enum PHPickerPrimaryButtonType : long {
		/// <summary>The system chooses the button based on context.</summary>
		Automatic = 0,
		/// <summary>No primary button is displayed.</summary>
		None = 1,
		/// <summary>An Add button.</summary>
		Add = 2,
		/// <summary>A Done button.</summary>
		Done = 3,
		/// <summary>An Edit button.</summary>
		Edit = 4,
		/// <summary>A Save button.</summary>
		Save = 5,
		/// <summary>A Next button.</summary>
		Next = 6,
		/// <summary>A Choose button.</summary>
		Choose = 7,
		/// <summary>A Continue button.</summary>
		Continue = 8,
	}

	/// <summary>Specifies the secondary button displayed by a photo picker.</summary>
	[NoTV, Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
	[Native]
	public enum PHPickerSecondaryButtonType : long {
		/// <summary>The system chooses the button based on context.</summary>
		Automatic = 0,
		/// <summary>No secondary button is displayed.</summary>
		None = 1,
		/// <summary>A Cancel button.</summary>
		Cancel = 2,
		/// <summary>A Back button.</summary>
		Back = 3,
		/// <summary>A Close button.</summary>
		Close = 4,
		/// <summary>A Clear button that clears the selection.</summary>
		Clear = 5,
	}

	/// <summary>Identifies errors reported by PhotosUI operations.</summary>
	[NoTV, Mac (27, 3), iOS (27, 3), MacCatalyst (27, 3)]
	[ErrorDomain ("PHPhotosUIErrorDomain")]
	[Native]
	public enum PHPhotosUIError : long {
		/// <summary>No album matches the supplied local identifier in the specified library.</summary>
		AssetCollectionNotFound = 1,
		/// <summary>The user cannot add content to the target album.</summary>
		AssetCollectionNotWritable = 2,
		/// <summary>Album creation did not produce an asset collection.</summary>
		AssetCollectionCreationFailed = 3,
		/// <summary>The shared album cannot accept content until migration completes.</summary>
		SharedAlbumMigrationInProgress = 100,
		/// <summary>The operation requires iCloud Photos, which is not enabled on the device.</summary>
		ICloudPhotosNotEnabled = 101,
	}

	[NoTV, Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
	[Native]
	public enum PHSharedAlbumCreationSharingPolicy : long {
		Private = 0,
		Public = 1,
	}
}
