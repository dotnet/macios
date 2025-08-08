using CoreGraphics;
using CoreLocation;
using ObjCRuntime;
using Foundation;
#if !MONOMAC
using UIKit;
using NSView = Foundation.NSObject;
using PHLivePhotoViewContentMode = Foundation.NSObject;
#else
using AppKit;
using UITouch = Foundation.NSObject;
using UIImage = AppKit.NSImage;
using UIColor = AppKit.NSColor;
using UIGestureRecognizer = Foundation.NSObject;
using PHLivePhotoBadgeOptions = Foundation.NSObject;
using UIViewController = AppKit.NSViewController;
#endif
using MapKit;
using Photos;
using System;

namespace PhotosUI {
	[NoTV]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface PHContentEditingController {

		/// <param name="adjustmentData">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("canHandleAdjustmentData:")]
		bool CanHandleAdjustmentData (PHAdjustmentData adjustmentData);

		/// <param name="contentEditingInput">To be added.</param>
		/// <param name="placeholderImage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("startContentEditingWithInput:placeholderImage:")]
		void StartContentEditing (PHContentEditingInput contentEditingInput, UIImage placeholderImage);

		/// <param name="completionHandler">To be added. This parameter can be <see langword="null" />.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishContentEditingWithCompletionHandler:")]
		void FinishContentEditing (Action<PHContentEditingOutput> completionHandler);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("cancelContentEditing")]
		void CancelContentEditing ();

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("shouldShowCancelConfirmation")]
		bool ShouldShowCancelConfirmation { get; }
	}

	/// <summary>A <see cref="UIKit.UIView" /> that displays a <see cref="Photo.PHLivePhoto" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/PhotosUI/PHLivePhotoView">Apple documentation for <c>PHLivePhotoView</c></related>
	[MacCatalyst (13, 1)]
#if MONOMAC
	[BaseType (typeof (NSView))]
#else
	[BaseType (typeof (UIView))]
#endif
	interface PHLivePhotoView {

		// inlined (designated initializer)
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the PHLivePhotoView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of PHLivePhotoView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("livePhotoBadgeImageWithOptions:")]
		UIImage GetLivePhotoBadgeImage (PHLivePhotoBadgeOptions badgeOptions);

		/// <summary>An instance of the PhotosUI.IPHLivePhotoViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PhotosUI.IPHLivePhotoViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IPHLivePhotoViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("livePhoto", ArgumentSemantic.Strong)]
		PHLivePhoto LivePhoto { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("playbackGestureRecognizer", ArgumentSemantic.Strong)]
		UIGestureRecognizer PlaybackGestureRecognizer { get; }

		/// <summary>Gets or sets a Boolean value that controls whether sound is muted for the Live Photo. Default is <see langword="false" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("muted")]
		bool Muted { [Bind ("isMuted")] get; set; }

		[Export ("startPlaybackWithStyle:")]
		void StartPlayback (PHLivePhotoViewPlaybackStyle playbackStyle);

		[Export ("stopPlayback")]
		void StopPlayback ();

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("stopPlaybackAnimated:")]
		void StopPlayback (bool animated);

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("contentMode", ArgumentSemantic.Assign)]
		PHLivePhotoViewContentMode ContentMode { get; set; }

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("audioVolume")]
		float AudioVolume { get; set; }

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[NullAllowed, Export ("livePhotoBadgeView", ArgumentSemantic.Strong)]
		NSView LivePhotoBadgeView { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("contentsRect", ArgumentSemantic.Assign)]
		CGRect ContentsRect { get; set; }
	}

	interface IPHLivePhotoViewDelegate { }

	/// <summary>Delegate object for <see cref="PhotosUI.PHLivePhotoView" /> objects that adds methods for responding to playback beginning and ending.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/PhotosUI/PHLivePhotoViewDelegate">Apple documentation for <c>PHLivePhotoViewDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PHLivePhotoViewDelegate {
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("livePhotoView:canBeginPlaybackWithStyle:")]
		bool CanBeginPlayback (PHLivePhotoView livePhotoView, PHLivePhotoViewPlaybackStyle playbackStyle);

		/// <param name="livePhotoView">To be added.</param>
		/// <param name="playbackStyle">To be added.</param>
		/// <summary>Method that is called just before playback begins.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("livePhotoView:willBeginPlaybackWithStyle:")]
		void WillBeginPlayback (PHLivePhotoView livePhotoView, PHLivePhotoViewPlaybackStyle playbackStyle);

		/// <param name="livePhotoView">To be added.</param>
		/// <param name="playbackStyle">To be added.</param>
		/// <summary>Method that is called aftr playback ends.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("livePhotoView:didEndPlaybackWithStyle:")]
		void DidEndPlayback (PHLivePhotoView livePhotoView, PHLivePhotoViewPlaybackStyle playbackStyle);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("livePhotoView:extraMinimumTouchDurationForTouch:withStyle:")]
		double GetExtraMinimumTouchDuration (PHLivePhotoView livePhotoView, UITouch touch, PHLivePhotoViewPlaybackStyle playbackStyle);
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Static]
	interface PHProjectType {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PHProjectTypeUndefined")]
		NSString Undefined { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (NSExtensionContext))]
	interface PHProjectExtensionContext : NSSecureCoding, NSCopying {

		[Export ("photoLibrary")]
		PHPhotoLibrary PhotoLibrary { get; }

		[Export ("project")]
		PHProject Project { get; }

		[NoMacCatalyst]
		[Export ("showEditorForAsset:")]
		void ShowEditor (PHAsset asset);

		[NoMacCatalyst]
		[Export ("updatedProjectInfoFromProjectInfo:completion:")]
		NSProgress UpdatedProjectInfo ([NullAllowed] PHProjectInfo existingProjectInfo, Action<PHProjectInfo> completionHandler);
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (PHProjectElement))]
	interface PHProjectJournalEntryElement : NSSecureCoding {

		[Export ("date")]
		NSDate Date { get; }

		[NullAllowed, Export ("assetElement")]
		PHProjectAssetElement AssetElement { get; }

		[NullAllowed, Export ("textElement")]
		PHProjectTextElement TextElement { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (PHProjectElement))]
	interface PHProjectTextElement : NSSecureCoding {

		[Export ("text")]
		string Text { get; }

		[NullAllowed, Export ("attributedText")]
		NSAttributedString AttributedText { get; }

		[Export ("textElementType")]
		PHProjectTextElementType TextElementType { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Protocol]
	interface PHProjectExtensionController {

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		[Export ("supportedProjectTypes", ArgumentSemantic.Copy)]
		PHProjectTypeDescription [] GetSupportedProjectTypes ();

		/// <param name="extensionContext">To be added.</param>
		/// <param name="projectInfo">To be added.</param>
		/// <param name="completion">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("beginProjectWithExtensionContext:projectInfo:completion:")]
		void BeginProject (PHProjectExtensionContext extensionContext, PHProjectInfo projectInfo, Action<NSError> completion);

		/// <param name="extensionContext">To be added.</param>
		/// <param name="completion">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("resumeProjectWithExtensionContext:completion:")]
		void ResumeProject (PHProjectExtensionContext extensionContext, Action<NSError> completion);

		/// <param name="completion">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishProjectWithCompletionHandler:")]
		void FinishProject (Action completion);

		/// <param name="category">To be added.</param>
		/// <param name="invalidator">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Protected]
		[NoMacCatalyst]
		[Export ("typeDescriptionDataSourceForCategory:invalidator:")]
		IPHProjectTypeDescriptionDataSource GetTypeDescriptionDataSource (NSString category, IPHProjectTypeDescriptionInvalidator invalidator);

		[Wrap ("GetTypeDescriptionDataSource (category.GetConstant(), invalidator)")]
		IPHProjectTypeDescriptionDataSource GetTypeDescriptionDataSource (PHProjectCategory category, IPHProjectTypeDescriptionInvalidator invalidator);
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectTypeDescription : NSSecureCoding {

		[Export ("projectType")]
		NSString ProjectType { get; }

		[Export ("localizedTitle")]
		string LocalizedTitle { get; }

		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Copy)]
		UIImage Image { get; }

		[Export ("subtypeDescriptions", ArgumentSemantic.Copy)]
		PHProjectTypeDescription [] SubtypeDescriptions { get; }

		[Export ("initWithProjectType:title:description:image:subtypeDescriptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSString projectType, string localizedTitle, [NullAllowed] string localizedDescription, [NullAllowed] UIImage image, PHProjectTypeDescription [] subtypeDescriptions);

		[Export ("initWithProjectType:title:description:image:")]
		NativeHandle Constructor (NSString projectType, string localizedTitle, [NullAllowed] string localizedDescription, [NullAllowed] UIImage image);

		[NoMacCatalyst]
		[Export ("initWithProjectType:title:attributedDescription:image:subtypeDescriptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSString projectType, string localizedTitle, [NullAllowed] NSAttributedString localizedAttributedDescription, [NullAllowed] UIImage image, PHProjectTypeDescription [] subtypeDescriptions);

		[NoMacCatalyst]
		[Export ("initWithProjectType:title:attributedDescription:image:canProvideSubtypes:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSString projectType, string localizedTitle, [NullAllowed] NSAttributedString localizedAttributedDescription, [NullAllowed] UIImage image, bool canProvideSubtypes);

		[NoMacCatalyst]
		[Export ("initWithProjectType:title:description:image:canProvideSubtypes:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSString projectType, string localizedTitle, [NullAllowed] string localizedDescription, [NullAllowed] UIImage image, bool canProvideSubtypes);

		[NoMacCatalyst]
		[Export ("canProvideSubtypes")]
		bool CanProvideSubtypes { get; }

		[NoMacCatalyst]
		[NullAllowed, Export ("localizedAttributedDescription", ArgumentSemantic.Copy)]
		NSAttributedString LocalizedAttributedDescription { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectRegionOfInterest : NSSecureCoding {

		[Export ("rect")]
		CGRect Rect { get; }

		[Export ("weight")]
		double Weight { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[NoMacCatalyst]
		[Export ("quality")]
		double Quality { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectElement : NSSecureCoding {

		[Export ("weight")]
		double Weight { get; }

		[Export ("placement")]
		CGRect Placement { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (PHProjectElement))]
	interface PHProjectAssetElement : NSSecureCoding {

		[Export ("cloudAssetIdentifier")]
		PHCloudIdentifier CloudAssetIdentifier { get; }

		[Export ("annotation")]
		string Annotation { get; }

		[Export ("cropRect")]
		CGRect CropRect { get; }

		[Export ("regionsOfInterest")]
		PHProjectRegionOfInterest [] RegionsOfInterest { get; }

		[NoMacCatalyst]
		[Export ("horizontallyFlipped")]
		bool HorizontallyFlipped { get; }

		[NoMacCatalyst]
		[Export ("verticallyFlipped")]
		bool VerticallyFlipped { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectInfo : NSSecureCoding {

		[Export ("creationSource")]
		PHProjectCreationSource CreationSource { get; }

		[Export ("projectType")]
		NSString ProjectType { get; }

		[Export ("sections")]
		PHProjectSection [] Sections { get; }

		[NoMacCatalyst]
		[Export ("brandingEnabled")]
		bool BrandingEnabled { get; }

		[NoMacCatalyst]
		[Export ("pageNumbersEnabled")]
		bool PageNumbersEnabled { get; }

		[NoMacCatalyst]
		[NullAllowed, Export ("productIdentifier")]
		string ProductIdentifier { get; }

		[NoMacCatalyst]
		[NullAllowed, Export ("themeIdentifier")]
		string ThemeIdentifier { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectSection : NSSecureCoding {

		[Export ("sectionContents")]
		PHProjectSectionContent [] SectionContents { get; }

		[Export ("sectionType")]
		PHProjectSectionType SectionType { get; }

		[Export ("title")]
		string Title { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PHProjectSectionContent : NSSecureCoding {

		[Export ("elements")]
		PHProjectElement [] Elements { get; }

		[Export ("numberOfColumns")]
		nint NumberOfColumns { get; }

		[Export ("aspectRatio")]
		double AspectRatio { get; }

		[Export ("cloudAssetIdentifiers")]
		PHCloudIdentifier [] CloudAssetIdentifiers { get; }

		[NoMacCatalyst]
		[NullAllowed, Export ("backgroundColor")]
		UIColor BackgroundColor { get; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (PHProjectElement))]
	interface PHProjectMapElement : NSSecureCoding {
		[Export ("mapType")]
		MKMapType MapType { get; }

		[Export ("centerCoordinate")]
		CLLocationCoordinate2D CenterCoordinate { get; }

		[Export ("heading")]
		double Heading { get; }

		[Export ("pitch")]
		nfloat Pitch { get; }

		[Export ("altitude")]
		double Altitude { get; }

		[Export ("annotations", ArgumentSemantic.Copy)]
		IMKAnnotation [] Annotations { get; }
	}

	interface IPHProjectTypeDescriptionDataSource { }
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PHProjectTypeDescriptionDataSource {
		/// <param name="projectType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("subtypesForProjectType:")]
		PHProjectTypeDescription [] GetSubtypes (NSString projectType);

		/// <param name="projectType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("typeDescriptionForProjectType:")]
		[return: NullAllowed]
		PHProjectTypeDescription GetTypeDescription (NSString projectType);

		/// <param name="projectType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("footerTextForSubtypesOfProjectType:")]
		[return: NullAllowed]
		NSAttributedString GetFooterTextForSubtypes (NSString projectType);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("extensionWillDiscardDataSource")]
		void WillDiscardDataSource ();
	}

	interface IPHProjectTypeDescriptionInvalidator { }
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Protocol]
	interface PHProjectTypeDescriptionInvalidator {
		/// <param name="projectType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("invalidateTypeDescriptionForProjectType:")]
		void InvalidateTypeDescription (NSString projectType);

		/// <param name="projectType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("invalidateFooterTextForSubtypesOfProjectType:")]
		void InvalidateFooterTextForSubtypes (NSString projectType);
	}

	[NoMac]
	[NoTV]
	[DisableDefaultCtor]
	[NoMacCatalyst]
	[Obsoleted (PlatformName.iOS, 14, 0)] // Removed from headers completely
	[BaseType (typeof (NSExtensionContext))]
	interface PHEditingExtensionContext {
	}

	interface IPHPickerViewControllerDelegate { }

	[NoTV, Mac (13, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PHPickerViewControllerDelegate {
		[Abstract]
		[Export ("picker:didFinishPicking:")]
		void DidFinishPicking (PHPickerViewController picker, PHPickerResult [] results);
	}

	[NoTV, Mac (13, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIViewController))]
	[Advice ("This type should not be subclassed.")]
	[DisableDefaultCtor]
	interface PHPickerViewController {
		[Export ("configuration", ArgumentSemantic.Copy)]
		PHPickerConfiguration Configuration { get; }

		[Wrap ("WeakDelegate")]
		IPHPickerViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("initWithConfiguration:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PHPickerConfiguration configuration);

		[NoTV, Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Export ("deselectAssetsWithIdentifiers:")]
		void DeselectAssets (string [] identifiers);

		[NoTV, Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Export ("moveAssetWithIdentifier:afterAssetWithIdentifier:")]
		void MoveAsset (string identifier, [NullAllowed] string afterIdentifier);

		[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("updatePickerUsingConfiguration:")]
		void UpdatePicker (PHPickerUpdateConfiguration configuration);

		[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("scrollToInitialPosition")]
		void ScrollToInitialPosition ();

		[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("zoomIn")]
		void ZoomIn ();

		[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("zoomOut")]
		void ZoomOut ();
	}

	[NoTV, Mac (13, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[Advice ("This type should not be subclassed.")]
	interface PHPickerConfiguration : NSCopying {
		[Export ("preferredAssetRepresentationMode", ArgumentSemantic.Assign)]
		PHPickerConfigurationAssetRepresentationMode PreferredAssetRepresentationMode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("selection", ArgumentSemantic.Assign)]
		PHPickerConfigurationSelection Selection { get; set; }

		[Export ("selectionLimit")]
		nint SelectionLimit { get; set; }

		[NullAllowed, Export ("filter", ArgumentSemantic.Copy)]
		PHPickerFilter Filter { get; set; }

		[Export ("initWithPhotoLibrary:")]
		NativeHandle Constructor (PHPhotoLibrary photoLibrary);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("preselectedAssetIdentifiers", ArgumentSemantic.Copy)]
		string [] PreselectedAssetIdentifiers { get; set; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("mode", ArgumentSemantic.Assign)]
		PHPickerMode Mode { get; set; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("edgesWithoutContentMargins", ArgumentSemantic.Assign)]
		NSDirectionalRectEdge EdgesWithoutContentMargins { get; set; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("disabledCapabilities", ArgumentSemantic.Assign)]
		PHPickerCapabilities DisabledCapabilities { get; set; }
	}

	[NoTV, Mac (13, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[Advice ("This type should not be subclassed.")]
	[DisableDefaultCtor]
	interface PHPickerFilter : NSCopying {
		[Static]
		[Export ("imagesFilter")]
		PHPickerFilter ImagesFilter { get; }

		[Static]
		[Export ("videosFilter")]
		PHPickerFilter VideosFilter { get; }

		[Static]
		[Export ("livePhotosFilter")]
		PHPickerFilter LivePhotosFilter { get; }

		[Static]
		[Export ("anyFilterMatchingSubfilters:")]
		PHPickerFilter GetAnyFilterMatchingSubfilters (PHPickerFilter [] subfilters);

		[NoTV, Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Static]
		[Export ("depthEffectPhotosFilter")]
		PHPickerFilter DepthEffectPhotosFilter { get; }

		[NoTV, Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Static]
		[Export ("burstsFilter")]
		PHPickerFilter BurstsFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("panoramasFilter")]
		PHPickerFilter PanoramasFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("screenshotsFilter")]
		PHPickerFilter ScreenshotsFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("screenRecordingsFilter")]
		PHPickerFilter ScreenRecordingsFilter { get; }

		[NoTV, Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Static]
		[Export ("cinematicVideosFilter")]
		PHPickerFilter CinematicVideosFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("slomoVideosFilter")]
		PHPickerFilter SlomoVideosFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("timelapseVideosFilter")]
		PHPickerFilter TimelapseVideosFilter { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Static]
		[Export ("spatialMediaFilter")]
		PHPickerFilter SpatialMediaFilter { get; }

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("playbackStyleFilter:")]
		PHPickerFilter GetPlaybackStyleFilter (PHAssetPlaybackStyle playbackStyle);

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("allFilterMatchingSubfilters:")]
		PHPickerFilter GetAllFilterMatchingSubfilters (PHPickerFilter [] subfilters);

		[NoTV, Mac (13, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("notFilterOfSubfilter:")]
		PHPickerFilter GetNotFilterOfSubfilter (PHPickerFilter subfilter);
	}

	[NoTV, Mac (13, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[Advice ("This type should not be subclassed.")]
	[DisableDefaultCtor]
	interface PHPickerResult {
		[Export ("itemProvider")]
		NSItemProvider ItemProvider { get; }

		[NullAllowed, Export ("assetIdentifier")]
		string AssetIdentifier { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (PHPhotoLibrary))]
	interface PHPhotoLibrary_PhotosUISupport {
		[Export ("presentLimitedLibraryPickerFromViewController:")]
		void PresentLimitedLibraryPicker (UIViewController controller);

		[Async]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("presentLimitedLibraryPickerFromViewController:completionHandler:")]
		void PresentLimitedLibraryPicker (UIViewController controller, Action<string []> completionHandler);
	}

	[NoTV, Mac (13, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum PHPickerConfigurationSelection : long {
		Default = 0,
		Ordered = 1,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		Continuous = 2,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		ContinuousAndOrdered = 3,
	}

	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface PHPickerUpdateConfiguration : NSCopying, NSSecureCoding {
		[Export ("selectionLimit")]
		nint SelectionLimit { get; set; }

		[Export ("edgesWithoutContentMargins", ArgumentSemantic.Assign)]
		NSDirectionalRectEdge EdgesWithoutContentMargins { get; set; }
	}
}
