using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Metal;

namespace Cinematic {

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[ErrorDomain ("CNCinematicErrorDomain")]
	[Native]
	public enum CNCinematicErrorCode : long {
		Unknown = 1,
		Unreadable = 2,
		Incomplete = 3,
		Malformed = 4,
		Unsupported = 5,
		Incompatible = 6,
		Cancelled = 7,
		/// <summary>Required Cinematic resources could not be downloaded.</summary>
		DownloadFailed = 8,
	}

	/// <summary>Identifies a version of Cinematic resources.</summary>
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[Native]
	public enum CNCinematicResourceVersion : long {
		/// <summary>Version 1 of the Cinematic resources.</summary>
		Version1 = 1,
	}

	/// <summary>Describes the Cinematic capabilities of an asset.</summary>
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[Native]
	public enum CNCinematicCapability : long {
		/// <summary>The asset has no Cinematic capabilities.</summary>
		None = 0,
		/// <summary>The asset can be rendered without preprocessing.</summary>
		Renderable = 1,
		/// <summary>The asset requires preprocessing.</summary>
		NeedsPreprocessing = 2,
	}

	/// <summary>Describes the availability of resources required to process a Cinematic asset.</summary>
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[Native]
	public enum CNResourceStatus : long {
		/// <summary>The configuration is supported and its required resources are available.</summary>
		Ready,
		/// <summary>The configuration is supported, but required resources must be downloaded.</summary>
		NeedsDownloading,
		/// <summary>The device lacks the required hardware capabilities.</summary>
		UnsupportedDevice,
		/// <summary>The asset is unsupported by the current system build.</summary>
		UnsupportedAsset,
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CNRenderingQuality : long {
		Thumbnail,
		Preview,
		Export,
		ExportHigh,
	}

	[TV (17, 0), iOS (17, 0)]
	[Native]
	public enum CNDetectionType : long {
		Unknown = 0,
		HumanFace = 1,
		HumanHead = 2,
		HumanTorso = 3,
		CatBody = 4,
		DogBody = 5,
		CatHead = 9,
		DogHead = 10,
		SportsBall = 11,
		AutoFocus = 100,
		FixedFocus = 101,
		Custom = 102,
	}

	[TV (26, 0), MacCatalyst (26, 0), Mac (26, 0), iOS (26, 0)]
	[Native]
	public enum CNSpatialAudioRenderingStyle : long {
		Cinematic = 0,
		Studio = 1,
		InFrame = 2,
		CinematicBackgroundStem = 3,
		CinematicForegroundStem = 4,
		StudioForegroundStem = 5,
		InFrameForegroundStem = 6,
		Standard = 7,
		StudioBackgroundStem = 8,
		InFrameBackgroundStem = 9,
	}

	[TV (26, 0), MacCatalyst (26, 0), Mac (26, 0), iOS (26, 0)]
	[Native]
	public enum CNSpatialAudioContentType : long {
		Stereo,
		Spatial,
	}

	[TV (26, 0), MacCatalyst (26, 0), Mac (26, 0), iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNAssetSpatialAudioInfo {
		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[Async]
		[Static]
		[Export ("checkIfContainsSpatialAudio:completionHandler:")]
		void CheckIfContainsSpatialAudio (AVAsset asset, CNAssetSpatialAudioInfoCheckIfContainsSpatialAudioCallback completionHandler);

		[Async]
		[Static]
		[Export ("loadFromAsset:completionHandler:")]
		void Load (AVAsset asset, CNAssetSpatialAudioInfoLoadCallback completionHandler);

		// From the CNAssetSpatialAudioInfo (Properties) category
		[Export ("defaultSpatialAudioTrack")]
		AVAssetTrack DefaultSpatialAudioTrack { get; }

		// From the CNAssetSpatialAudioInfo (Properties) category
		[Export ("defaultEffectIntensity")]
		float DefaultEffectIntensity { get; }

		// From the CNAssetSpatialAudioInfo (Properties) category
		[Export ("defaultRenderingStyle")]
		CNSpatialAudioRenderingStyle DefaultRenderingStyle { get; }

		// From the CNAssetSpatialAudioInfo (Properties) category
		[Export ("spatialAudioMixMetadata")]
		NSData SpatialAudioMixMetadata { get; }

		// From the CNAssetSpatialAudioInfo (SynthesizeAVFoundationObjects) category
		[Export ("audioMixWithEffectIntensity:renderingStyle:")]
		AVAudioMix CreateAudioMix (float effectIntensity, CNSpatialAudioRenderingStyle renderingStyle);

		// From the CNAssetSpatialAudioInfo (SynthesizeAVFoundationObjects) category
		[Export ("assetReaderOutputSettingsForContentType:")]
		NSDictionary<NSString, NSObject> GetAssetReaderOutputSettings (CNSpatialAudioContentType contentType);

		// From the CNAssetSpatialAudioInfo (SynthesizeAVFoundationObjects) category
		[Export ("assetWriterInputSettingsForContentType:")]
		NSDictionary<NSString, NSObject> GetAssetWriterInputSettingsFor (CNSpatialAudioContentType contentType);
	}

	delegate void CNAssetSpatialAudioInfoCheckIfContainsSpatialAudioCallback (bool result);
	delegate void CNAssetSpatialAudioInfoLoadCallback ([NullAllowed] CNAssetSpatialAudioInfo assetInfo, [NullAllowed] NSError error);

	/// <summary>Configures preprocessing of a Cinematic asset.</summary>
	[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNAssetPreprocessConfiguration {
		/// <summary>Creates a preprocessing configuration with the specified output location.</summary>
		/// <param name="destinationAssetUrl">The URL where the preprocessed asset will be written.</param>
		[Export ("initWithDestinationAssetURL:")]
		NativeHandle Constructor (NSUrl destinationAssetUrl);

		/// <summary>Gets or sets whether the output references the source asset's color and audio tracks instead of embedding them.</summary>
		/// <value><see langword="true" /> to reference the source tracks; otherwise, <see langword="false" />. The default is <see langword="false" />.</value>
		/// <remarks>Referencing the source produces a smaller output, but requires the source asset to remain at its original location. Disparity and metadata tracks are always embedded.</remarks>
		[Export ("referenceSourceAssetTracks")]
		bool ReferenceSourceAssetTracks { get; set; }

		/// <summary>Gets the URL where the preprocessed asset will be written.</summary>
		[Export ("destinationAssetURL")]
		NSUrl DestinationAssetUrl { get; }
	}

	/// <summary>Handles the result of checking an asset's Cinematic capabilities.</summary>
	/// <param name="capability">The asset's Cinematic capability.</param>
	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	delegate void CNAssetInfoCheckCinematicCapabilityCallback (CNCinematicCapability capability);

	/// <summary>Handles completion of a download of Cinematic resource versions.</summary>
	/// <param name="error">The error, or <see langword="null" /> if the download succeeded.</param>
	[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	delegate void CNAssetInfoDownloadResourcesForVersionsCallback ([NullAllowed] NSError error);

	/// <summary>Handles completion of a download of resources for a Cinematic asset.</summary>
	/// <param name="assetInfo">Refreshed asset information with the downloaded resources available, or <see langword="null" /> on failure.</param>
	/// <param name="error">The error, or <see langword="null" /> if the download succeeded.</param>
	[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	delegate void CNAssetInfoDownloadResourcesCallback ([NullAllowed] CNAssetInfo assetInfo, [NullAllowed] NSError error);

	/// <summary>Handles completion of preprocessing a Cinematic asset.</summary>
	/// <param name="assetInfo">Information for the new, renderable, preprocessed asset, or <see langword="null" /> on failure.</param>
	/// <param name="error">The error, or <see langword="null" /> if preprocessing succeeded.</param>
	[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	delegate void CNAssetInfoPreprocessAssetCallback ([NullAllowed] CNAssetInfo assetInfo, [NullAllowed] NSError error);

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNAssetInfo {
		/// <summary>Checks the Cinematic capabilities of an asset.</summary>
		/// <param name="asset">The asset to inspect.</param>
		/// <param name="completionHandler">The callback that receives the asset's Cinematic capability.</param>
		[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Async (XmlDocs = """
			<summary>Asynchronously checks the Cinematic capabilities of an asset.</summary>
			<param name="asset">The asset to inspect.</param>
			<returns>A task whose result is the asset's Cinematic capability.</returns>
			""")]
		[Static]
		[Export ("checkCinematicCapabilityForAsset:completionHandler:")]
		void CheckCinematicCapability (AVAsset asset, CNAssetInfoCheckCinematicCapabilityCallback completionHandler);

		[Deprecated (PlatformName.iOS, 27, 0, message: "Use 'CheckCinematicCapability' instead.")]
		[Deprecated (PlatformName.TvOS, 27, 0, message: "Use 'CheckCinematicCapability' instead.")]
		[Deprecated (PlatformName.MacOSX, 27, 0, message: "Use 'CheckCinematicCapability' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 27, 0, message: "Use 'CheckCinematicCapability' instead.")]
		[Async]
		[Static]
		[Export ("checkIfCinematic:completionHandler:")]
		void CheckIfCinematic (AVAsset asset, Action<bool> completionHandler);

		[Async]
		[Static]
		[Export ("loadFromAsset:completionHandler:")]
		void LoadFromAsset (AVAsset asset, Action<CNAssetInfo, NSError> completionHandler);

		[Export ("asset", ArgumentSemantic.Strong)]
		AVAsset Asset { get; }

		[Export ("allCinematicTracks", ArgumentSemantic.Strong)]
		AVAssetTrack [] AllCinematicTracks { get; }

		[Export ("cinematicVideoTrack", ArgumentSemantic.Strong)]
		AVAssetTrack CinematicVideoTrack { get; }

		[Export ("cinematicDisparityTrack", ArgumentSemantic.Strong)]
		AVAssetTrack CinematicDisparityTrack { get; }

		[Export ("cinematicMetadataTrack", ArgumentSemantic.Strong)]
		AVAssetTrack CinematicMetadataTrack { get; }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; }

		[Export ("naturalSize")]
		CGSize NaturalSize { get; }

		[Export ("preferredSize")]
		CGSize PreferredSize { get; }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; }

		// from @interface AbstractTracks (CNAssetInfo)

		[Export ("frameTimingTrack", ArgumentSemantic.Strong)]
		AVAssetTrack FrameTimingTrack { get; }

		[Export ("videoCompositionTracks", ArgumentSemantic.Strong)]
		AVAssetTrack [] VideoCompositionTracks { get; }

		[Export ("videoCompositionTrackIDs", ArgumentSemantic.Strong)]
		NSNumber [] VideoCompositionTrackIds { get; }

		[Export ("sampleDataTrackIDs", ArgumentSemantic.Strong)]
		NSNumber [] SampleDataTrackIds { get; }

		// From the CNAssetWithoutDisparity category

		/// <summary>Gets the status of the requested Cinematic resource versions on the current device.</summary>
		/// <param name="resourceVersions">A set of <see cref="T:Cinematic.CNCinematicResourceVersion" /> values boxed as <see cref="T:Foundation.NSNumber" /> objects, or an empty set to check all available versions.</param>
		/// <returns>The first non-ready status encountered, or <see cref="F:Cinematic.CNResourceStatus.Ready" /> if all requested versions are ready.</returns>
		[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Static]
		[Export ("resourceStatusForVersions:")]
		CNResourceStatus GetResourceStatus (NSSet<NSNumber> resourceVersions);

		/// <summary>Downloads the requested Cinematic resource versions.</summary>
		/// <param name="resourceVersions">A set of <see cref="T:Cinematic.CNCinematicResourceVersion" /> values boxed as <see cref="T:Foundation.NSNumber" /> objects, or an empty set to download all available versions.</param>
		/// <param name="downloadTimeout">The maximum download time, in seconds. Use <see cref="P:Cinematic.CNAssetInfo.DefaultResourceDownloadTimeout" /> for the default timeout.</param>
		/// <param name="completionHandler">The callback invoked when downloading finishes.</param>
		/// <returns>An object that reports download progress.</returns>
		/// <remarks>The downloaded resources are device-wide and cached for subsequent use.</remarks>
		[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Async (XmlDocs = """
			<summary>Asynchronously downloads the requested Cinematic resource versions.</summary>
			<param name="resourceVersions">A set of <see cref="T:Cinematic.CNCinematicResourceVersion" /> values boxed as <see cref="T:Foundation.NSNumber" /> objects, or an empty set to download all available versions.</param>
			<param name="downloadTimeout">The maximum download time, in seconds.</param>
			<returns>A task that completes when downloading finishes.</returns>
			<remarks>The downloaded resources are device-wide and cached for subsequent use.</remarks>
			""",
			XmlDocsWithOutParameter = """
			<summary>Asynchronously downloads the requested Cinematic resource versions and provides download progress.</summary>
			<param name="resourceVersions">A set of <see cref="T:Cinematic.CNCinematicResourceVersion" /> values boxed as <see cref="T:Foundation.NSNumber" /> objects, or an empty set to download all available versions.</param>
			<param name="downloadTimeout">The maximum download time, in seconds.</param>
			<param name="result">An object that reports download progress.</param>
			<returns>A task that completes when downloading finishes.</returns>
			<remarks>The downloaded resources are device-wide and cached for subsequent use.</remarks>
			""")]
		[Static]
		[Export ("downloadResourcesForVersions:timeout:completionHandler:")]
		NSProgress DownloadResourcesForVersions (NSSet<NSNumber> resourceVersions, double downloadTimeout, CNAssetInfoDownloadResourcesForVersionsCallback completionHandler);

		/// <summary>Downloads the resources required by this asset.</summary>
		/// <param name="downloadTimeout">The maximum download time, in seconds. Use <see cref="P:Cinematic.CNAssetInfo.DefaultResourceDownloadTimeout" /> for the default timeout.</param>
		/// <param name="completionHandler">The callback that receives refreshed asset information when downloading finishes.</param>
		/// <returns>An object that reports download progress.</returns>
		/// <remarks>Use the refreshed asset information supplied to the callback after a successful download.</remarks>
		[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Async (XmlDocs = """
			<summary>Asynchronously downloads the resources required by this asset.</summary>
			<param name="downloadTimeout">The maximum download time, in seconds.</param>
			<returns>A task whose result is refreshed asset information with the downloaded resources available.</returns>
			""",
			XmlDocsWithOutParameter = """
			<summary>Asynchronously downloads the resources required by this asset and provides download progress.</summary>
			<param name="downloadTimeout">The maximum download time, in seconds.</param>
			<param name="result">An object that reports download progress.</param>
			<returns>A task whose result is refreshed asset information with the downloaded resources available.</returns>
			""")]
		[Export ("downloadResourcesWithTimeout:completionHandler:")]
		NSProgress DownloadResources (double downloadTimeout, CNAssetInfoDownloadResourcesCallback completionHandler);

		/// <summary>Preprocesses the asset by generating a disparity track and writing a new asset.</summary>
		/// <param name="configuration">The preprocessing configuration, including the destination asset URL.</param>
		/// <param name="completionHandler">The callback that receives information for the new, renderable, preprocessed asset.</param>
		/// <returns>An object that reports preprocessing progress.</returns>
		/// <remarks>Ensure <see cref="P:Cinematic.CNAssetInfo.ResourceStatus" /> is <see cref="F:Cinematic.CNResourceStatus.Ready" /> before preprocessing. Download any required resources first.</remarks>
		[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Async (XmlDocs = """
			<summary>Asynchronously preprocesses the asset by generating a disparity track and writing a new asset.</summary>
			<param name="configuration">The preprocessing configuration, including the destination asset URL.</param>
			<returns>A task whose result describes the new, renderable, preprocessed asset.</returns>
			<remarks>Ensure <see cref="P:Cinematic.CNAssetInfo.ResourceStatus" /> is <see cref="F:Cinematic.CNResourceStatus.Ready" /> before preprocessing. Download any required resources first.</remarks>
			""",
			XmlDocsWithOutParameter = """
			<summary>Asynchronously preprocesses the asset and provides preprocessing progress.</summary>
			<param name="configuration">The preprocessing configuration, including the destination asset URL.</param>
			<param name="result">An object that reports preprocessing progress.</param>
			<returns>A task whose result describes the new, renderable, preprocessed asset.</returns>
			<remarks>Ensure <see cref="P:Cinematic.CNAssetInfo.ResourceStatus" /> is <see cref="F:Cinematic.CNResourceStatus.Ready" /> before preprocessing. Download any required resources first.</remarks>
			""")]
		[Export ("preprocessAssetWithConfiguration:completionHandler:")]
		NSProgress PreprocessAsset (CNAssetPreprocessConfiguration configuration, CNAssetInfoPreprocessAssetCallback completionHandler);

		/// <summary>Gets the default timeout, in seconds, for Cinematic resource downloads.</summary>
		[NoTV, MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Static]
		[Export ("defaultResourceDownloadTimeout")]
		double DefaultResourceDownloadTimeout { get; }

		/// <summary>Gets whether the asset has been preprocessed.</summary>
		[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Export ("preprocessed")]
		bool Preprocessed { [Bind ("isPreprocessed")] get; }

		/// <summary>Gets the asset's Cinematic capability.</summary>
		[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Export ("cinematicCapability")]
		CNCinematicCapability CinematicCapability { get; }

		/// <summary>Gets the status of the resources required by the asset on the current device.</summary>
		[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
		[Export ("resourceStatus")]
		CNResourceStatus ResourceStatus { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (CNAssetInfo))]
	interface CNCompositionInfo {
		[Export ("insertTimeRange:ofCinematicAssetInfo:atTime:error:")]
		bool InsertTimeRange (CMTimeRange timeRange, CNAssetInfo assetInfo, CMTime startTime, [NullAllowed] out NSError outError);
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNRenderingSessionAttributes {
		[Async]
		[Static]
		[Export ("loadFromAsset:completionHandler:")]
		void Load (AVAsset asset, Action<CNRenderingSessionAttributes, NSError> completionHandler);

		[Export ("renderingVersion")]
		nint RenderingVersion { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNRenderingSessionFrameAttributes : NSCopying, NSMutableCopying {
		[Export ("initWithSampleBuffer:sessionAttributes:")]
		NativeHandle Constructor (CMSampleBuffer sampleBuffer, CNRenderingSessionAttributes sessionAttributes);

		[Export ("initWithTimedMetadataGroup:sessionAttributes:")]
		NativeHandle Constructor (AVTimedMetadataGroup metadataGroup, CNRenderingSessionAttributes sessionAttributes);

		[Export ("focusDisparity")]
		float FocusDisparity { get; set; }

		[Export ("fNumber")]
		float FNumber { get; set; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNRenderingSession {
		[Export ("initWithCommandQueue:sessionAttributes:preferredTransform:quality:")]
		NativeHandle Constructor (IMTLCommandQueue commandQueue, CNRenderingSessionAttributes sessionAttributes, CGAffineTransform preferredTransform, CNRenderingQuality quality);

		[Export ("commandQueue", ArgumentSemantic.Strong)]
		IMTLCommandQueue CommandQueue { get; }

		[Export ("sessionAttributes", ArgumentSemantic.Strong)]
		CNRenderingSessionAttributes SessionAttributes { get; }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; }

		[Export ("quality")]
		CNRenderingQuality Quality { get; }

		/// <summary>Encodes rendering of a Cinematic frame into a pixel buffer.</summary>
		/// <param name="commandBuffer">The command buffer to encode into.</param>
		/// <param name="frameAttributes">The rendering attributes for the frame.</param>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="sourceDisparity">The source disparity buffer, or <see langword="null" /> only when previewing an asset whose capability is <see cref="F:Cinematic.CNCinematicCapability.NeedsPreprocessing" /> before preprocessing.</param>
		/// <param name="destinationImage">The destination image buffer.</param>
		/// <returns><see langword="true" /> if rendering was encoded; otherwise, <see langword="false" />.</returns>
		/// <remarks>When the disparity buffer is <see langword="null" />, source disparity and focus disparity are computed internally. A <see langword="null" /> disparity buffer for any other asset type causes this method to return <see langword="false" />.</remarks>
		[Export ("encodeRenderToCommandBuffer:frameAttributes:sourceImage:sourceDisparity:destinationImage:")]
		bool EncodeRender (IMTLCommandBuffer commandBuffer, CNRenderingSessionFrameAttributes frameAttributes, CVPixelBuffer sourceImage, [NullAllowed] CVPixelBuffer sourceDisparity, CVPixelBuffer destinationImage);

		/// <summary>Encodes rendering of a Cinematic frame into an RGBA texture.</summary>
		/// <param name="commandBuffer">The command buffer to encode into.</param>
		/// <param name="frameAttributes">The rendering attributes for the frame.</param>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="sourceDisparity">The source disparity buffer, or <see langword="null" /> only when previewing an asset whose capability is <see cref="F:Cinematic.CNCinematicCapability.NeedsPreprocessing" /> before preprocessing.</param>
		/// <param name="destinationRgba">The destination RGBA texture.</param>
		/// <returns><see langword="true" /> if rendering was encoded; otherwise, <see langword="false" />.</returns>
		/// <remarks>When the disparity buffer is <see langword="null" />, source disparity and focus disparity are computed internally. A <see langword="null" /> disparity buffer for any other asset type causes this method to return <see langword="false" />.</remarks>
		[Export ("encodeRenderToCommandBuffer:frameAttributes:sourceImage:sourceDisparity:destinationRGBA:")]
		bool EncodeRender (IMTLCommandBuffer commandBuffer, CNRenderingSessionFrameAttributes frameAttributes, CVPixelBuffer sourceImage, [NullAllowed] CVPixelBuffer sourceDisparity, IMTLTexture destinationRgba);

		/// <summary>Encodes rendering of a Cinematic frame into separate luma and chroma textures.</summary>
		/// <param name="commandBuffer">The command buffer to encode into.</param>
		/// <param name="frameAttributes">The rendering attributes for the frame.</param>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="sourceDisparity">The source disparity buffer, or <see langword="null" /> only when previewing an asset whose capability is <see cref="F:Cinematic.CNCinematicCapability.NeedsPreprocessing" /> before preprocessing.</param>
		/// <param name="destinationLuma">The destination luma texture.</param>
		/// <param name="destinationChroma">The destination chroma texture.</param>
		/// <returns><see langword="true" /> if rendering was encoded; otherwise, <see langword="false" />.</returns>
		/// <remarks>When the disparity buffer is <see langword="null" />, source disparity and focus disparity are computed internally. A <see langword="null" /> disparity buffer for any other asset type causes this method to return <see langword="false" />.</remarks>
		[Export ("encodeRenderToCommandBuffer:frameAttributes:sourceImage:sourceDisparity:destinationLuma:destinationChroma:")]
		bool EncodeRender (IMTLCommandBuffer commandBuffer, CNRenderingSessionFrameAttributes frameAttributes, CVPixelBuffer sourceImage, [NullAllowed] CVPixelBuffer sourceDisparity, IMTLTexture destinationLuma, IMTLTexture destinationChroma);

		[Static]
		[Export ("sourcePixelFormatTypes", ArgumentSemantic.Strong)]
		NSNumber [] SourcePixelFormatTypes { get; }

		[Static]
		[Export ("destinationPixelFormatTypes", ArgumentSemantic.Strong)]
		NSNumber [] DestinationPixelFormatTypes { get; }
	}

	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNImageRenderingSessionConfiguration {
		[Export ("initWithQuality:")]
		NativeHandle Constructor (CNRenderingQuality quality);

		[Export ("initWithQuality:renderingVersion:")]
		[return: NullAllowed]
		NativeHandle Constructor (CNRenderingQuality quality, nint renderingVersion);

		[Export ("quality")]
		CNRenderingQuality Quality { get; }

		[Export ("renderingVersion")]
		nint RenderingVersion { get; }

		[Static]
		[Export ("latestRenderingVersion")]
		nint LatestRenderingVersion { get; }

		[Static]
		[Export ("isRenderingVersionSupported:")]
		bool IsRenderingVersionSupported (nint renderingVersion);
	}

	[TV (27, 0), MacCatalyst (27, 0), Mac (27, 0), iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNImageRenderingSession {
		[Export ("initWithConfiguration:")]
		NativeHandle Constructor (CNImageRenderingSessionConfiguration configuration);

		[Export ("configuration", ArgumentSemantic.Strong)]
		CNImageRenderingSessionConfiguration Configuration { get; }

		[Export ("encodeRenderToCommandBuffer:sourceRGBA:sourceDisparity:destinationRGBA:fNumber:focusDisparity:")]
		bool EncodeRender (IMTLCommandBuffer commandBuffer, IMTLTexture sourceRgba, IMTLTexture sourceDisparity, IMTLTexture destinationRgba, float fNumber, float focusDisparity);

		[Export ("encodeTileRenderToCommandBuffer:sourceTileRGBA:sourceDisparity:destinationTileRGBA:fNumber:focusDisparity:sourceRGBASize:tileOffset:tileExtendOffset:")]
		bool EncodeTileRender (IMTLCommandBuffer commandBuffer, IMTLTexture sourceTileRgba, IMTLTexture sourceDisparity, IMTLTexture destinationTileRgba, float fNumber, float focusDisparity, CGSize sourceRgbaSize, CGPoint tileOffset, CGPoint tileExtendOffset);

		[Static]
		[Export ("minimumTileExtendRectForTileRect:sourceRGBASize:")]
		CGRect GetMinimumTileExtendRect (CGRect tileRect, CGSize sourceRgbaSize);
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNDetection : NSCopying {
		[Export ("initWithTime:detectionType:normalizedRect:focusDisparity:")]
		NativeHandle Constructor (CMTime time, CNDetectionType detectionType, CGRect normalizedRect, float focusDisparity);

		[Export ("time")]
		CMTime Time { get; }

		[Export ("detectionType")]
		CNDetectionType DetectionType { get; }

		[Export ("normalizedRect")]
		CGRect NormalizedRect { get; }

		[Export ("focusDisparity")]
		float FocusDisparity { get; }

		[Export ("detectionID")]
		long DetectionId { get; }

		[Export ("detectionGroupID")]
		long DetectionGroupId { get; }

		[Static]
		[Export ("isValidDetectionID:")]
		bool IsValidDetectionId (long detectionId);

		[Static]
		[Export ("isValidDetectionGroupID:")]
		bool IsValidDetectionGroupId (long detectionGroupId);

		[Static]
		[Export ("accessibilityLabelForDetectionType:")]
		string AccessibilityLabelForDetectionType (CNDetectionType detectionType);

		[Static]
		[Export ("disparityInNormalizedRect:sourceDisparity:detectionType:priorDisparity:")]
		float DisparityInNormalizedRect (CGRect normalizedRect, CVPixelBuffer sourceDisparity, CNDetectionType detectionType, float priorDisparity);
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNDecision : NSCopying {
		[Internal]
		[Export ("initWithTime:detectionID:strong:")]
		NativeHandle _InitWithSingleIdentifier (CMTime time, long detectionId, bool isStrong);

		[Internal]
		[Export ("initWithTime:detectionGroupID:strong:")]
		NativeHandle _InitWithGroupIdentifier (CMTime time, long detectionGroupId, bool isStrong);

		[Export ("time")]
		CMTime Time { get; }

		[Export ("detectionID")]
		long DetectionId { get; }

		[Export ("detectionGroupID")]
		long DetectionGroupId { get; }

		[Export ("userDecision")]
		bool UserDecision { [Bind ("isUserDecision")] get; }

		[Export ("groupDecision")]
		bool GroupDecision { [Bind ("isGroupDecision")] get; }

		[Export ("strongDecision")]
		bool StrongDecision { [Bind ("isStrongDecision")] get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNDetectionTrack : NSCopying {
		[Export ("detectionType")]
		CNDetectionType DetectionType { get; }

		[Export ("detectionID")]
		long DetectionId { get; }

		[Export ("detectionGroupID")]
		long DetectionGroupId { get; }

		[Export ("userCreated")]
		bool UserCreated { [Bind ("isUserCreated")] get; }

		[Export ("discrete")]
		bool Discrete { [Bind ("isDiscrete")] get; }

		[Export ("detectionAtOrBeforeTime:")]
		[return: NullAllowed]
		CNDetection GetDetectionAtOrBeforeTime (CMTime time);

		[Export ("detectionNearestTime:")]
		[return: NullAllowed]
		CNDetection GetDetectionNearestTime (CMTime time);

		[Export ("detectionsInTimeRange:")]
		CNDetection [] GetDetectionsInTimeRange (CMTimeRange timeRange);
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (CNDetectionTrack))]
	interface CNFixedDetectionTrack {
		[Export ("initWithFocusDisparity:")]
		NativeHandle Constructor (float focusDisparity);

		[Export ("initWithOriginalDetection:")]
		NativeHandle Constructor (CNDetection originalDetection);

		[Export ("focusDisparity")]
		float FocusDisparity { get; }

		[NullAllowed, Export ("originalDetection", ArgumentSemantic.Strong)]
		CNDetection OriginalDetection { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (CNDetectionTrack))]
	interface CNCustomDetectionTrack {
		[Export ("initWithDetections:smooth:")]
		NativeHandle Constructor (CNDetection [] detections, bool applySmoothing);

		[Export ("allDetections", ArgumentSemantic.Strong)]
		CNDetection [] AllDetections { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNScript {
		[Async]
		[Static]
		[Export ("loadFromAsset:changes:progress:completionHandler:")]
		void Load (AVAsset asset, [NullAllowed] CNScriptChanges changes, [NullAllowed] NSProgress progress, Action<CNScript, NSError> completionHandler);

		[Export ("reloadWithChanges:")]
		void Reload ([NullAllowed] CNScriptChanges changes);

		[Export ("changes")]
		CNScriptChanges Changes { get; }

		[Export ("changesTrimmedByTimeRange:")]
		CNScriptChanges GetChangesTrimmed (CMTimeRange timeRange);

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; }

		[Export ("frameAtTime:tolerance:")]
		[return: NullAllowed]
		CNScriptFrame GetFrame (CMTime time, CMTime tolerance);

		[Export ("framesInTimeRange:")]
		CNScriptFrame [] GetFrames (CMTimeRange timeRange);

		[Export ("decisionAtTime:tolerance:")]
		[return: NullAllowed]
		CNDecision GetDecision (CMTime time, CMTime tolerance);

		[Export ("decisionsInTimeRange:")]
		CNDecision [] GetDecisions (CMTimeRange timeRange);

		[Export ("decisionAfterTime:")]
		[return: NullAllowed]
		CNDecision GetDecisionAfterTime (CMTime time);

		[Export ("decisionBeforeTime:")]
		[return: NullAllowed]
		CNDecision GetDecisionBeforeTime (CMTime time);

		[Export ("primaryDecisionAtTime:")]
		[return: NullAllowed]
		CNDecision GetPrimaryDecision (CMTime time);

		[Export ("secondaryDecisionAtTime:")]
		[return: NullAllowed]
		CNDecision GetSecondaryDecision (CMTime time);

		[Export ("timeRangeOfTransitionAfterDecision:")]
		CMTimeRange GetTimeRangeOfTransitionAfterDecision (CNDecision decision);

		[Export ("timeRangeOfTransitionBeforeDecision:")]
		CMTimeRange GetTimeRangeOfTransitionBeforeDecision (CNDecision decision);

		[Export ("userDecisionsInTimeRange:")]
		CNDecision [] GetUserDecisions (CMTimeRange timeRange);

		[Export ("baseDecisionsInTimeRange:")]
		CNDecision [] GetBaseDecisions (CMTimeRange timeRange);

		[Export ("detectionTrackForID:")]
		[return: NullAllowed]
		CNDetectionTrack GetDetectionTrackForId (long detectionId);

		[Export ("detectionTrackForDecision:")]
		[return: NullAllowed]
		CNDetectionTrack GetDetectionTrack (CNDecision decision);

		[Export ("fNumber")]
		float FNumber { get; set; }

		[Export ("addUserDecision:")]
		bool AddUserDecision (CNDecision decision);

		[Export ("removeUserDecision:")]
		bool RemoveUserDecision (CNDecision decision);

		[Export ("removeAllUserDecisions")]
		void RemoveAllUserDecisions ();

		[Export ("addDetectionTrack:")]
		long AddDetectionTrack (CNDetectionTrack detectionTrack);

		[Export ("removeDetectionTrack:")]
		bool RemoveDetectionTrack (CNDetectionTrack detectionTrack);

		[Export ("addedDetectionTracks", ArgumentSemantic.Strong)]
		CNDetectionTrack [] AddedDetectionTracks { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNScriptChanges {
		[Export ("initWithDataRepresentation:")]
		NativeHandle Constructor (NSData dataRepresentation);

		[Export ("dataRepresentation")]
		NSData DataRepresentation { get; }

		[Export ("fNumber")]
		float FNumber { get; }

		[Export ("userDecisions")]
		CNDecision [] UserDecisions { get; }

		[Export ("addedDetectionTracks")]
		CNDetectionTrack [] AddedDetectionTracks { get; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNScriptFrame : NSCopying {
		[Export ("time")]
		CMTime Time { get; }

		[Export ("focusDisparity")]
		float FocusDisparity { get; }

		[Export ("focusDetection", ArgumentSemantic.Strong)]
		CNDetection FocusDetection { get; }

		[Export ("allDetections", ArgumentSemantic.Strong)]
		CNDetection [] AllDetections { get; }

		[Export ("detectionForID:")]
		[return: NullAllowed]
		CNDetection GetDetectionForId (long detectionId);

		[Export ("bestDetectionForGroupID:")]
		[return: NullAllowed]
		CNDetection GetBestDetectionForGroupId (long detectionGroupId);
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface CNBoundsPrediction : NSCopying, NSMutableCopying {
		[Export ("normalizedBounds", ArgumentSemantic.Assign)]
		CGRect NormalizedBounds { get; set; }

		[Export ("confidence")]
		float Confidence { get; set; }
	}

	[TV (17, 0), iOS (17, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNObjectTracker {
		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[Export ("initWithCommandQueue:")]
		NativeHandle Constructor (IMTLCommandQueue commandQueue);

		[Export ("findObjectAtPoint:sourceImage:")]
		[return: NullAllowed]
		CNBoundsPrediction FindObject (CGPoint point, CVPixelBuffer sourceImage);

		[Export ("startTrackingAt:within:sourceImage:sourceDisparity:")]
		bool StartTracking (CMTime atTime, CGRect normalizedBounds, CVPixelBuffer sourceImage, CVPixelBuffer sourceDisparity);

		[Export ("continueTrackingAt:sourceImage:sourceDisparity:")]
		[return: NullAllowed]
		CNBoundsPrediction ContinueTracking (CMTime atTime, CVPixelBuffer sourceImage, CVPixelBuffer sourceDisparity);

		[Export ("finishDetectionTrack")]
		CNDetectionTrack FinishDetectionTrack { get; }

		[Export ("resetDetectionTrack")]
		void ResetDetectionTrack ();
	}

}
