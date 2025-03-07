using System;
using Foundation;
using ObjCRuntime;

namespace PhotosUI {

	/// <summary>Enumerates how live photo playback will occur in a <see cref="T:PhotosUI.PHLivePhotoView" />.</summary>
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
	/// <summary>Flagging enumeration that controls appearance of the playback badge on <see cref="T:PhotosUI.PHLivePhotoView" /> objects.</summary>
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
		[Field ("PHProjectCategoryBook")]
		Book,
		[Field ("PHProjectCategoryCalendar")]
		Calendar,
		[Field ("PHProjectCategoryCard")]
		Card,
		[Field ("PHProjectCategoryPrints")]
		Prints,
		[Field ("PHProjectCategorySlideshow")]
		Slideshow,
		[Field ("PHProjectCategoryWallDecor")]
		WallDecor,
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
	}
}
