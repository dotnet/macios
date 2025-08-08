//
// ARKit bindings
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

#nullable enable

using System;
using System.ComponentModel;
using System.Numerics;

using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreLocation;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using ImageIO;
using Metal;
using QuickLook;
using SpriteKit;
using SceneKit;
using UIKit;

using Vector3 = global::CoreGraphics.NVector3;
using Matrix3 = global::CoreGraphics.NMatrix3;
using Matrix4 = global::CoreGraphics.NMatrix4;

namespace ARKit {

	/// <summary>Enumerates the quality of real-world tracking in an augmented-reality <see cref="ARKit.ARSession" />.</summary>
	[NoTV, NoMac]
	[Native]
	public enum ARTrackingState : long {
		/// <summary>World-tracking is not available.</summary>
		NotAvailable,
		/// <summary>World-tracking is at a reduced quality.</summary>
		Limited,
		/// <summary>World-tracking is at normal quality.</summary>
		Normal,
	}

	/// <summary>Enumerates the causes of <see cref="ARKit.ARTrackingState.Limited" />.</summary>
	[NoTV, NoMac]
	[Native]
	public enum ARTrackingStateReason : long {
		/// <summary>Either tracking is <see cref="ARKit.ARTrackingState.Normal" /> or the reason for poor tracking cannot be determined.</summary>
		None,
		/// <summary>ARKit is still starting up.</summary>
		Initializing,
		/// <summary>The camera is moving too quickly.</summary>
		ExcessiveMotion,
		/// <summary>Processing is not revealing sufficient high-contrast points in the field of view.</summary>
		InsufficientFeatures,
		/// <summary>The AR session was interrupted and is reorienting.</summary>
		Relocalizing,
	}

	/// <summary>Enumerate causes for an <see cref="ARKit.ARSession" /> failure.</summary>
	[NoTV, NoMac]
	[ErrorDomain ("ARErrorDomain")]
	[Native]
	public enum ARErrorCode : long {
		/// <summary>The requested <see cref="ARKit.ARSessionConfiguration" /> is not compatible with the device.</summary>
		UnsupportedConfiguration = 100,
		/// <summary>A suitable capture device could not be found.</summary>
		SensorUnavailable = 101,
		/// <summary>A failure relating to the capture device(s).</summary>
		SensorFailed = 102,
		/// <summary>Indicates that the user has not allowed access to the camera.</summary>
		CameraUnauthorized = 103,
		MicrophoneUnauthorized = 104,
		LocationUnauthorized = 105,
		HighResolutionFrameCaptureInProgress = 106,
		HighResolutionFrameCaptureFailed = 107,
		/// <summary>A failure relating to world tracking.</summary>
		WorldTrackingFailed = 200,
		GeoTrackingNotAvailableAtLocation = 201,
		GeoTrackingFailed = 202,
		/// <summary>To be added.</summary>
		InvalidReferenceImage = 300,
		/// <summary>To be added.</summary>
		InvalidReferenceObject = 301,
		/// <summary>To be added.</summary>
		InvalidWorldMap = 302,
		/// <summary>To be added.</summary>
		InvalidConfiguration = 303,
		InvalidCollaborationData = 304,
		/// <summary>To be added.</summary>
		InsufficientFeatures = 400,
		/// <summary>To be added.</summary>
		ObjectMergeFailed = 401,
		/// <summary>To be added.</summary>
		FileIOFailed = 500,
		RequestFailed = 501,
	}

	/// <summary>Enumerates the kinds of objects detected by the <see cref="ARKit.ARFrame.HitTest(CoreGraphics.CGPoint,ARKit.ARHitTestResultType)" /> method.</summary>
	[NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARHitTestResultType : ulong {
		/// <summary>A point detected by image processing that appears to be on a continuous surface but does not have an <see cref="ARKit.ARAnchor" />.</summary>
		FeaturePoint = 1 << 0,
		/// <summary>A real-world horizontal plane detected by image analysis.</summary>
		EstimatedHorizontalPlane = 1 << 1,
		/// <summary>A real-world vertical plane detected by image analysis.</summary>
		EstimatedVerticalPlane = 1 << 2,
		/// <summary>An existing plane in the scene, detected without constraints on the plane's size.</summary>
		ExistingPlane = 1 << 3,
		/// <summary>An existing plane in the scene, detected only if the point is within the plane's extent.</summary>
		ExistingPlaneUsingExtent = 1 << 4,
		/// <summary>An existint plane in the scene, detected if the point is coplanar with the plane's geometry.</summary>
		ExistingPlaneUsingGeometry = 1 << 5,
	}

	/// <summary>The orientation of an <see cref="ARKit.ARPlaneAnchor" /> (Currently restricted to horizontal).</summary>
	[NoTV, NoMac]
	[Native]
	public enum ARPlaneAnchorAlignment : long {
		/// <summary>A plane perpendicular to the direction of gravity.</summary>
		Horizontal,
		/// <summary>A plane parallel to the direction of gravity.</summary>
		Vertical,
	}

	/// <summary>Enumerates options in calls to <see cref="ARKit.ARSession.Run(ARKit.ARConfiguration,ARKit.ARSessionRunOptions)" />.</summary>
	[NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARSessionRunOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>The <see cref="ARKit.ARSession" /> should reset its world-tracking.</summary>
		ResetTracking = 1 << 0,
		/// <summary>The <see cref="ARKit.ARSession" /> should remove any existing <see cref="ARKit.ARAnchor" /> objects.</summary>
		RemoveExistingAnchors = 1 << 1,
		StopTrackedRaycasts = 1 << 2,
		[iOS (13, 4)]
		ResetSceneReconstruction = (1 << 3),
	}

	/// <summary>Enumerates options for how the world coordinate system is created.</summary>
	[NoTV, NoMac]
	[Native]
	public enum ARWorldAlignment : long {
		/// <summary>The world coordinate system's Y-axis is perpendicular to gravity, with an origin at the original position of the device.</summary>
		Gravity,
		/// <summary>The world coordinate system's Y-axis is perpendicular to gravity, X- and Z- are oriented to a compass heading, and it's origin is the original position of the device.</summary>
		GravityAndHeading,
		/// <summary>The world coordinate system is locked to the orientation of the camera.</summary>
		Camera,
	}

	/// <summary>Enumerates the valid orientations for detected planes (currently, only horizontal).</summary>
	[NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARPlaneDetection : ulong {
		/// <summary>No planes should be detected.</summary>
		None = 0,
		/// <summary>Planes that are perpendicular to the direction of gravity.</summary>
		Horizontal = 1 << 0,
		/// <summary>Planes that are aligned to the direction of gravity.</summary>
		Vertical = 1 << 1,
	}

	/// <summary>Enumerates environmental texturing strategies used with <see cref="ARKit.ARWorldTrackingProbeAnchor" /> objects.</summary>
	[NoTV, NoMac]
	[Native]
	public enum AREnvironmentTexturing : long {
		/// <summary>Environmental texture-map generation is not in use.</summary>
		None,
		/// <summary>The developer creates and places <see cref="ARKit.ARWorldTrackingProbeAnchor" /> objects.</summary>
		Manual,
		/// <summary>The system automatically creates and places <see cref="ARKit.ARWorldTrackingProbeAnchor" /> objects. .</summary>
		Automatic,
	}

	/// <summary>Enumerates the states of a world-mapping session.</summary>
	[NoTV, NoMac]
	[Native]
	public enum ARWorldMappingStatus : long {
		/// <summary>No real-world map is available.</summary>
		NotAvailable,
		/// <summary>Not enough data has been gathered to accurately fix the device in space.</summary>
		Limited,
		/// <summary>Some areas have been mapped, but further mapping is still required.</summary>
		Extending,
		/// <summary>There is enough data to accurately track the device in the real world.</summary>
		Mapped,
	}

	[NoTV, NoMac]
	[Native]
	public enum ARPlaneClassificationStatus : long {
		/// <summary>To be added.</summary>
		NotAvailable = 0,
		/// <summary>To be added.</summary>
		Undetermined,
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Known,
	}

	[NoTV, NoMac]
	[Native]
	public enum ARPlaneClassification : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Wall,
		/// <summary>To be added.</summary>
		Floor,
		/// <summary>To be added.</summary>
		Ceiling,
		/// <summary>To be added.</summary>
		Table,
		/// <summary>To be added.</summary>
		Seat,
		[iOS (13, 0)]
		Window,
		[iOS (13, 0)]
		Door,
	}

	[iOS (13, 0)]
	[Native]
	public enum ARCoachingGoal : long {
		Tracking,
		HorizontalPlane,
		VerticalPlane,
		AnyPlane,
		[iOS (15, 0)]
		GeoTracking,
	}

	[iOS (13, 0)]
	[Flags]
	[Native]
	public enum ARFrameSemantics : long {
		None = 0x0,
		PersonSegmentation = 1 << 0,
		PersonSegmentationWithDepth = (1 << 1) | (1 << 0),
		BodyDetection = 1 << 2,
		[iOS (14, 0)]
		SceneDepth = (1 << 3),
		[iOS (14, 0)]
		SmoothedSceneDepth = (1 << 4),
	}

	[iOS (13, 0)]
	[Native]
	public enum ARMatteResolution : long {
		Full = 0,
		Half = 1,
	}

	[iOS (13, 0)]
	[Native]
	public enum ARRaycastTarget : long {
		ExistingPlaneGeometry,
		ExistingPlaneInfinite,
		EstimatedPlane,
	}

	[iOS (13, 0)]
	[Native]
	public enum ARRaycastTargetAlignment : long {
		Horizontal,
		Vertical,
		Any,
	}

	[iOS (13, 0)]
	public enum ARSegmentationClass : byte {
		None = 0,
		Person = 255,
	}

	[iOS (13, 0)]
	[Native]
	public enum ARCollaborationDataPriority : long {
		Critical,
		Optional,
	}


	[iOS (14, 0)]
	[Native]
	public enum ARAltitudeSource : long {
		Unknown,
		Coarse,
		Precise,
		UserDefined,
	}

	[iOS (14, 0)]
	[Native]
	public enum ARConfidenceLevel : long {
		Low,
		Medium,
		High,
	}

	[iOS (14, 0)]
	[Native]
	public enum ARGeoTrackingAccuracy : long {
		Undetermined,
		Low,
		Medium,
		High,
	}

	[iOS (14, 0)]
	[Native]
	public enum ARGeoTrackingState : long {
		NotAvailable,
		Initializing,
		Localizing,
		Localized,
	}

	[iOS (14, 0)]
	[Native]
	public enum ARGeoTrackingStateReason : long {
		None,
		NotAvailableAtLocation,
		NeedLocationPermissions,
		WorldTrackingUnstable,
		WaitingForLocation,
		WaitingForAvailabilityCheck,
		GeoDataNotLoaded,
		DevicePointedTooLow,
		VisualLocalizationFailed,
	}

	[iOS (14, 3)]
	[Native]
	public enum ARAppClipCodeUrlDecodingState : long {
		Decoding,
		Failed,
		Decoded,
	}

	[NoTV, NoMac]
	[Protocol]
	[Advice ("To conform to 'ARAnchorCopying' you need to implement:\n'[Export (\"initWithAnchor:\")]'\n'public YourConstructor (ARAnchor anchor)'")]
	interface ARAnchorCopying : NSCopying {
		// Constructors in interfaces are not possible in C#
		// @required -(instancetype _Nonnull)initWithAnchor:(ARAnchor * _Nonnull)anchor;
	}

	/// <summary>A position, orientation, and scale that is located in the real world to which augmented reality objects can be attached.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARAnchor : ARAnchorCopying, NSSecureCoding {

		[Export ("identifier")]
		NSUuid Identifier { get; }

		[NullAllowed, Export ("name")]
		string Name { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("sessionIdentifier")]
		NSUuid SessionIdentifier { get; }

		[Export ("transform")]
		Matrix4 Transform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("initWithTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (Matrix4 transform);

		[Export ("initWithName:transform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, Matrix4 transform);

		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);
	}

	/// <summary>Information about the location and orientation of the camera that captured an augmented-reality frame.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARCamera : NSCopying {

		[Export ("transform")]
		Matrix4 Transform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("eulerAngles")]
		Vector3 EulerAngles {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("trackingState")]
		ARTrackingState TrackingState { get; }

		[Export ("trackingStateReason")]
		ARTrackingStateReason TrackingStateReason { get; }

		[Export ("intrinsics")]
		Matrix3 Intrinsics {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("imageResolution")]
		CGSize ImageResolution { get; }

		[iOS (13, 0)]
		[Export ("exposureDuration")]
		double ExposureDuration { get; }

		[iOS (13, 0)]
		[Export ("exposureOffset")]
		float ExposureOffset { get; }

		[Export ("projectionMatrix")]
		Matrix4 ProjectionMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("projectPoint:orientation:viewportSize:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		CGPoint Project (Vector3 point, UIInterfaceOrientation orientation, CGSize viewportSize);

		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("unprojectPoint:ontoPlaneWithTransform:orientation:viewportSize:")]
		Vector3 Unproject (CGPoint point, Matrix4 planeTransform, UIInterfaceOrientation orientation, CGSize viewportSize);

		/// <param name="orientation">The camera orientation.</param>
		/// <param name="viewportSize">The viewport size, in points.</param>
		/// <param name="zNear">The distance to the near Z-clipping plane.</param>
		/// <param name="zFar">The distance to the far Z-clipping plane..</param>
		/// <summary>The projection matrix used to render 3D content so that it will match the real-world imagery.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("projectionMatrixForOrientation:viewportSize:zNear:zFar:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetProjectionMatrix (UIInterfaceOrientation orientation, CGSize viewportSize, nfloat zNear, nfloat zFar);

		[Export ("viewMatrixForOrientation:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetViewMatrix (UIInterfaceOrientation orientation);
	}

	/// <summary>A frame in an augmented-reality session.</summary>
	///     <remarks>
	///       <para>Developers can retrieve the current <see cref="ARKit.ARFrame" /> from <see cref="ARKit.ARSession.CurrentFrame" /> or by implementing <see cref="ARKit.ARSessionDelegate_Extensions.DidUpdateFrame(ARKit.IARSessionDelegate,ARKit.ARSession,ARKit.ARFrame)" />. </para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARFrame : NSCopying {

		[Export ("timestamp")]
		double Timestamp { get; }

		[Export ("capturedImage")]
		CVPixelBuffer CapturedImage { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("cameraGrainTexture")]
		IMTLTexture CameraGrainTexture { get; }

		[iOS (13, 0)]
		[Export ("cameraGrainIntensity")]
		float CameraGrainIntensity { get; }

		[NullAllowed, Export ("capturedDepthData", ArgumentSemantic.Strong)]
		AVDepthData CapturedDepthData { get; }

		[Export ("capturedDepthDataTimestamp")]
		double CapturedDepthDataTimestamp { get; }

		[Export ("camera", ArgumentSemantic.Copy)]
		ARCamera Camera { get; }

		[Export ("anchors", ArgumentSemantic.Copy)]
		ARAnchor [] Anchors { get; }

		[NullAllowed, Export ("lightEstimate", ArgumentSemantic.Strong)]
		ARLightEstimate LightEstimate { get; }

		[NullAllowed, Export ("rawFeaturePoints", ArgumentSemantic.Strong)]
		ARPointCloud RawFeaturePoints { get; }

		[Export ("worldMappingStatus")]
		ARWorldMappingStatus WorldMappingStatus { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("segmentationBuffer")]
		CVPixelBuffer SegmentationBuffer { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("estimatedDepthData")]
		CVPixelBuffer EstimatedDepthData { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("detectedBody")]
		ARBody2D DetectedBody { get; }

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'ARSession.Raycast' instead.")]
		[Export ("hitTest:types:")]
		ARHitTestResult [] HitTest (CGPoint point, ARHitTestResultType types);

		[iOS (13, 0)]
		[Export ("raycastQueryFromPoint:allowingTarget:alignment:")]
		ARRaycastQuery CreateRaycastQuery (CGPoint point, ARRaycastTarget target, ARRaycastTargetAlignment alignment);

		[Export ("displayTransformForOrientation:viewportSize:")]
		CGAffineTransform GetDisplayTransform (UIInterfaceOrientation orientation, CGSize viewportSize);

		[iOS (14, 0)]
		[NullAllowed, Export ("geoTrackingStatus", ArgumentSemantic.Strong)]
		ARGeoTrackingStatus GeoTrackingStatus { get; }

		[iOS (14, 0)]
		[NullAllowed, Export ("sceneDepth", ArgumentSemantic.Strong)]
		ARDepthData SceneDepth { get; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("smoothedSceneDepth", ArgumentSemantic.Strong)]
		ARDepthData SmoothedSceneDepth { get; }

		[iOS (16, 0)]
		[Export ("exifData", ArgumentSemantic.Strong)]
		NSDictionary<NSString, NSObject> ExifData { get; }
	}

	/// <summary>A result generated by the <see cref="ARKit.ARFrame.HitTest(CoreGraphics.CGPoint,ARKit.ARHitTestResultType)" /> method.</summary>
	[Deprecated (PlatformName.iOS, 14, 0, message: "Use Raycasting methods over HitTestResult ones.")]
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARHitTestResult {

		[Export ("type")]
		ARHitTestResultType Type { get; }

		[Export ("distance")]
		nfloat Distance { get; }

		[Export ("localTransform")]
		Matrix4 LocalTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("worldTransform")]
		Matrix4 WorldTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[NullAllowed, Export ("anchor", ArgumentSemantic.Strong)]
		ARAnchor Anchor { get; }
	}

	/// <summary>An estimate of the real-world lighting environment.</summary>
	///     <remarks>
	///       <para>Developers who are doing their own rendering can use this information to inform their augmented-reality lighting decisions. <see cref="ARKit.ARSCNView" /> objects that display SceneKit objects automatically take this value into account for their own rendering.</para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARLightEstimate {

		[Export ("ambientIntensity")]
		nfloat AmbientIntensity { get; }

		[Export ("ambientColorTemperature")]
		nfloat AmbientColorTemperature { get; }
	}

	/// <summary>A subclass of <see cref="ARKit.ARAnchor" /> used to represent real-world flat surfaces.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARPlaneAnchor {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		// [Export ("initWithTransform:")] marked as NS_UNAVAILABLE

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("classificationSupported")]
		bool ClassificationSupported { [Bind ("isClassificationSupported")] get; }

		[Export ("alignment")]
		ARPlaneAnchorAlignment Alignment { get; }

		[Export ("center")]
		Vector3 Center {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Deprecated (PlatformName.iOS, 16, 0, message: "Use 'PlaneExtent' instead.")]
		[Export ("extent")]
		Vector3 Extent {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("geometry", ArgumentSemantic.Strong)]
		ARPlaneGeometry Geometry { get; }

		[Export ("classificationStatus", ArgumentSemantic.Assign)]
		ARPlaneClassificationStatus ClassificationStatus { get; }

		[Export ("classification", ArgumentSemantic.Assign)]
		ARPlaneClassification Classification { get; }

		[iOS (16, 0)]
		[Export ("planeExtent")]
		ARPlaneExtent PlaneExtent { get; }
	}

	/// <summary>Geometry representing a plane detected in the real world.</summary>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARPlaneGeometry : NSSecureCoding {
		[Export ("vertexCount")]
		nuint VertexCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("vertices")]
		IntPtr GetRawVertices ();

		[Export ("textureCoordinateCount")]
		nuint TextureCoordinateCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("textureCoordinates")]
		IntPtr GetRawTextureCoordinates ();

		[Export ("triangleCount")]
		nuint TriangleCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("triangleIndices")]
		IntPtr GetRawTriangleIndices ();

		[Export ("boundaryVertexCount")]
		nuint BoundaryVertexCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("boundaryVertices")]
		IntPtr GetRawBoundaryVertices ();
	}

	[BaseType (typeof (SCNGeometry))]
	[DisableDefaultCtor]
	interface ARSCNPlaneGeometry {
		[Static]
		[Export ("planeGeometryWithDevice:")]
		[return: NullAllowed]
		ARSCNPlaneGeometry Create (IMTLDevice device);

		[Export ("updateFromPlaneGeometry:")]
		void Update (ARPlaneGeometry planeGeometry);
	}

	/// <summary>A set of 3-dimensional points, indicating image-processing's belief in a fixed real-world point on a physical surface.</summary>
	///     <remarks>
	///       <para>Developers can visualize this data in a <see cref="ARKit.ARSCNView" /> by setting <see cref="ARKit.ARSCNDebugOptions.ShowFeaturePoints" /> to <see langword="true" />.</para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARPointCloud : NSSecureCoding {

		[Export ("count")]
		nuint Count { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("points")]
		IntPtr GetRawPoints ();

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("identifiers")]
		IntPtr GetRawIdentifiers ();
	}

	/// <summary>An image resource that contains pre-processed images to be recognized in the real-world.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARReferenceImage : NSCopying {
		[NullAllowed, Export ("name")]
		string Name { get; set; }

		[Export ("physicalSize")]
		CGSize PhysicalSize { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("resourceGroupName", ArgumentSemantic.Strong)]
		string ResourceGroupName { get; }

		[iOS (13, 0)]
		[Async]
		[Export ("validateWithCompletionHandler:")]
		void Validate (Action<NSError?> completionHandler);

		/// <param name="image">To be added.</param>
		/// <param name="orientation">To be added.</param>
		/// <param name="physicalWidth">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithCGImage:orientation:physicalWidth:")]
		NativeHandle Constructor (CGImage image, CGImagePropertyOrientation orientation, nfloat physicalWidth);

		/// <param name="pixelBuffer">To be added.</param>
		/// <param name="orientation">To be added.</param>
		/// <param name="physicalWidth">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPixelBuffer:orientation:physicalWidth:")]
		NativeHandle Constructor (CVPixelBuffer pixelBuffer, CGImagePropertyOrientation orientation, nfloat physicalWidth);

		[Static]
		[Export ("referenceImagesInGroupNamed:bundle:")]
		[return: NullAllowed]
		NSSet<ARReferenceImage> GetReferenceImagesInGroup (string name, [NullAllowed] NSBundle bundle);
	}

	/// <summary>Summary information about the video feed used in the AR simulation.</summary>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARVideoFormat : NSCopying {

		[iOS (13, 0)]
		[Export ("captureDevicePosition")]
		AVCaptureDevicePosition CaptureDevicePosition { get; }

		[Export ("imageResolution")]
		CGSize ImageResolution { get; }

		[Export ("framesPerSecond")]
		nint FramesPerSecond { get; }

		[iOS (14, 5)]
		[Export ("captureDeviceType")]
		[BindAs (typeof (AVCaptureDeviceType))]
		NSString CaptureDeviceType { get; }

		[iOS (16, 0)]
		[Export ("isRecommendedForHighResolutionFrameCapturing")]
		bool IsRecommendedForHighResolutionFrameCapturing { get; }

		[iOS (16, 0)]
		[Export ("videoHDRSupported")]
		bool IsVideoHdrSupported { [Bind ("isVideoHDRSupported")] get; }
	}

	/// <summary>A subclass of <see cref="SceneKit.SCNView" /> that supports augmented-reality content.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (SCNView))]
	interface ARSCNView : ARSessionProviding {

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSCNViewDelegate Delegate { get; set; }

		// We get the Session property from ARSessionProviding, but only the getter, so we must redefine the property here.
		[Export ("session", ArgumentSemantic.Strong)]
		new ARSession Session { get; set; }

		[Export ("scene", ArgumentSemantic.Strong)]
		SCNScene Scene { get; set; }

		[Export ("automaticallyUpdatesLighting")]
		bool AutomaticallyUpdatesLighting { get; set; }

		[iOS (13, 0)]
		[Export ("rendersCameraGrain")]
		bool RendersCameraGrain { get; set; }

		[iOS (13, 0)]
		[Export ("rendersMotionBlur")]
		bool RendersMotionBlur { get; set; }

		[Export ("anchorForNode:")]
		[return: NullAllowed]
		ARAnchor GetAnchor (SCNNode node);

		[Export ("nodeForAnchor:")]
		[return: NullAllowed]
		SCNNode GetNode (ARAnchor anchor);

		[Export ("hitTest:types:")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CreateRaycastQuery' instead.")]
		ARHitTestResult [] HitTest (CGPoint point, ARHitTestResultType types);

		[Export ("unprojectPoint:ontoPlaneWithTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 Unproject (CGPoint point, Matrix4 planeTransform);

		[iOS (13, 0)]
		[Export ("raycastQueryFromPoint:allowingTarget:alignment:")]
		[return: NullAllowed]
		ARRaycastQuery CreateRaycastQuery (CGPoint point, ARRaycastTarget target, ARRaycastTargetAlignment alignment);
	}

	interface IARSCNViewDelegate { }

	/// <summary>Delegate object for <see cref="ARKit.ARSCNView" /> objects.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSCNViewDelegate : SCNSceneRendererDelegate, ARSessionObserver {

		/// <param name="renderer">The renderer for the scene.</param>
		/// <param name="anchor">The anchor for the node to get.</param>
		/// <summary>Retrieves the <see cref="SceneKit.SCNNode" /> corresponding to the specified <paramref name="anchor" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("renderer:nodeForAnchor:")]
		[return: NullAllowed]
		SCNNode GetNode (ISCNSceneRenderer renderer, ARAnchor anchor);

		/// <param name="renderer">The renderer for the event.</param>
		/// <param name="node">The node that was added.</param>
		/// <param name="anchor">The anchor for the node that was added.</param>
		/// <summary>Developers may override this method to react to the adding of a <see cref="SceneKit.SCNNode" /> that corresponds to a new <see cref="ARKit.ARAnchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("renderer:didAddNode:forAnchor:")]
		void DidAddNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		/// <param name="renderer">The renderer for the scene.</param>
		/// <param name="node">The node that will be updated.</param>
		/// <param name="anchor">The anchor for the node that will be updated.</param>
		/// <summary>This method is called shortly before the properties of <paramref name="node" /> are updated to reflect the state of <paramref name="anchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("renderer:willUpdateNode:forAnchor:")]
		void WillUpdateNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		/// <param name="renderer">The renderer for the scene.</param>
		/// <param name="node">The node that was updated.</param>
		/// <param name="anchor">The anchor for the node that was updated.</param>
		/// <summary>This method is called shortly after <paramref name="node" /> has been updated to reflect the current state of <paramref name="anchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("renderer:didUpdateNode:forAnchor:")]
		void DidUpdateNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		/// <param name="renderer">The renderer for the scene.</param>
		/// <param name="node">The node that was removed.</param>
		/// <param name="anchor">The anchor for the node that was removed.</param>
		/// <summary>Developers may override this method to react to the removal of <paramref name="node" />, which was removed after <paramref name="anchor" /> was removed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("renderer:didRemoveNode:forAnchor:")]
		void DidRemoveNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);
	}

	/// <summary>A subclass of <see cref="SpriteKit.SKView" /> that places Sprite Kit objects in an augmented-reality session.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (SKView))]
	interface ARSKView : ARSessionProviding {

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSKViewDelegate Delegate { get; set; }

		// We get the Session property from ARSessionProviding, but only the getter, so we must redefine the property here.
		[Export ("session", ArgumentSemantic.Strong)]
		new ARSession Session { get; set; }

		[Export ("anchorForNode:")]
		[return: NullAllowed]
		ARAnchor GetAnchor (SKNode node);

		[Export ("nodeForAnchor:")]
		[return: NullAllowed]
		SKNode GetNode (ARAnchor anchor);

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use Raycasting methods instead.")]
		[Export ("hitTest:types:")]
		ARHitTestResult [] HitTest (CGPoint point, ARHitTestResultType types);
	}

	interface IARSKViewDelegate { }

	/// <summary>Delegate object allowing the developer to respond to events relating to a <see cref="ARKit.ARSKView" />.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSKViewDelegate : SKViewDelegate, ARSessionObserver {

		/// <param name="view">The view that is rendering the scene.</param>
		/// <param name="anchor">The anchor for the node to get.</param>
		/// <summary>Retrieves the <see cref="SpriteKit.SKNode" /> corresponding to the specified <paramref name="anchor" />. If no corresponding node exists, returns <see langword="null" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("view:nodeForAnchor:")]
		[return: NullAllowed]
		SKNode GetNode (ARSKView view, ARAnchor anchor);

		/// <param name="view">The view that is rendering the scene.</param>
		/// <param name="node">The node that was added.</param>
		/// <param name="anchor">The anchor for the node that was added.</param>
		/// <summary>Developers may override this method to react to the adding of a <see cref="SpriteKit.SKNode" /> that corresponds to a new <see cref="ARKit.ARAnchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("view:didAddNode:forAnchor:")]
		void DidAddNode (ARSKView view, SKNode node, ARAnchor anchor);

		/// <param name="view">The view that is rendering the scene.</param>
		/// <param name="node">The node that will be updated.</param>
		/// <param name="anchor">The anchor for the node that will be updated.</param>
		/// <summary>This method is called shortly before the properties of <paramref name="node" /> are updated to reflect the state of <paramref name="anchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("view:willUpdateNode:forAnchor:")]
		void WillUpdateNode (ARSKView view, SKNode node, ARAnchor anchor);

		/// <param name="view">The view that is rendering the scene.</param>
		/// <param name="node">The node that was updated.</param>
		/// <param name="anchor">The anchor for the node that was updated.</param>
		/// <summary>This method is called shortly after <paramref name="node" /> has been updated to reflect the current state of <paramref name="anchor" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("view:didUpdateNode:forAnchor:")]
		void DidUpdateNode (ARSKView view, SKNode node, ARAnchor anchor);

		/// <param name="view">The view that is rendering the scene.</param>
		/// <param name="node">The node that was removed.</param>
		/// <param name="anchor">The anchor for the node that was removed.</param>
		/// <summary>Developers may override this method to react to the removal of <paramref name="node" />, which was removed after <paramref name="anchor" /> was removed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("view:didRemoveNode:forAnchor:")]
		void DidRemoveNode (ARSKView view, SKNode node, ARAnchor anchor);
	}

	delegate void GetGeolocationCallback (CLLocationCoordinate2D coordinate, double altitude, [NullAllowed] NSError error);

	/// <include file="../docs/api/ARKit/ARSession.xml" path="/Documentation/Docs[@DocId='T:ARKit.ARSession']/*" />
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface ARSession {

		[iOS (13, 0)]
		[Export ("identifier", ArgumentSemantic.Strong)]
		NSUuid Identifier { get; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSessionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegateQueue", ArgumentSemantic.Strong)]
		DispatchQueue DelegateQueue { get; set; }

		[NullAllowed, Export ("currentFrame", ArgumentSemantic.Copy)]
		ARFrame CurrentFrame { get; }

		[NullAllowed, Export ("configuration", ArgumentSemantic.Copy)]
		ARConfiguration Configuration { get; }

		[Export ("runWithConfiguration:")]
		void Run (ARConfiguration configuration);

		[Export ("runWithConfiguration:options:")]
		void Run (ARConfiguration configuration, ARSessionRunOptions options);

		[Export ("pause")]
		void Pause ();

		[Export ("addAnchor:")]
		void AddAnchor (ARAnchor anchor);

		[Export ("removeAnchor:")]
		void RemoveAnchor (ARAnchor anchor);

		[Export ("setWorldOrigin:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetWorldOrigin (Matrix4 relativeTransform);

		[Async (XmlDocs = """
			<summary>Asynchronously returns a task that contains the current world map.</summary>
			<returns>A task that contains the current world map.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("getCurrentWorldMapWithCompletionHandler:")]
		void GetCurrentWorldMap (Action<ARWorldMap?, NSError?> completionHandler);

		[Async (XmlDocs = """
			<param name="transform">The transform to the position and orientation of the region from which to create a reference object.</param>
			<param name="center">The center of the region.</param>
			<param name="extent">The exent of the region, in the <paramref name="transform" /> coordinate space.</param>
			<summary>Asynchronously creates a reference object from a region in space and returns a task that contains the resulting object.</summary>
			<returns>A task that receives the created object, if present.</returns>
			<remarks>To be added.</remarks>
			""")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("createReferenceObjectWithTransform:center:extent:completionHandler:")]
		void CreateReferenceObject (Matrix4 transform, Vector3 center, Vector3 extent, Action<ARReferenceObject?, NSError?> completionHandler);

		[iOS (13, 0)]
		[Export ("raycast:")]
		ARRaycastResult [] Raycast (ARRaycastQuery query);

		[iOS (13, 0)]
		[Async]
		[Export ("trackedRaycast:updateHandler:")]
		[return: NullAllowed]
		ARTrackedRaycast TrackedRaycast (ARRaycastQuery query, Action<ARRaycastResult []> updateHandler);

		[iOS (13, 0)]
		[Export ("updateWithCollaborationData:")]
		void Update (ARCollaborationData collaborationData);

		[iOS (14, 0)]
		[Async (ResultTypeName = "GeoLocationForPoint")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("getGeoLocationForPoint:completionHandler:")]
		void GetGeoLocation (Vector3 position, GetGeolocationCallback completionHandler);

		[iOS (16, 0)]
		[Async]
		[Export ("captureHighResolutionFrameWithCompletion:")]
		void CaptureHighResolutionFrame (Action<ARFrame?, NSError?> handler);
	}

	/// <summary>Interface defining methods that respond to events in an <see cref="ARKit.ARSession" />.</summary>
	/// <remarks>To be added.</remarks>
	[NoTV, NoMac]
	[Protocol]
	interface ARSessionObserver {

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="error">The error that occurred.</param>
		/// <summary>Called when the <paramref name="session" /> stops running due to an error.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:didFailWithError:")]
		void DidFail (ARSession session, NSError error);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="camera">The camera whose tracking state changed.</param>
		/// <summary>Called when the <see cref="ARKit.ARCamera.TrackingState" /> changes, indicating a change in tracking quality.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:cameraDidChangeTrackingState:")]
		void CameraDidChangeTrackingState (ARSession session, ARCamera camera);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <summary>Developers may override this method to stop frame processing and device tracking when an interruption occurs.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("sessionWasInterrupted:")]
		void WasInterrupted (ARSession session);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <summary>Developers may override this method to begin frame processing and device tracking after an interruption.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("sessionInterruptionEnded:")]
		void InterruptionEnded (ARSession session);

		/// <param name="session">The session in question.</param>
		/// <summary>Returns a Boolean value that tells whether the session should attempt to reorient after an interruption.</summary>
		/// <returns>A Boolean value that tells whether the session should attempt to reorient after an interruption.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("sessionShouldAttemptRelocalization:")]
		bool ShouldAttemptRelocalization (ARSession session);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="audioSampleBuffer">The audio buffer that was played.</param>
		/// <summary>Developers may implement this method that is called shortly after an audio buffer has been played.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:didOutputAudioSampleBuffer:")]
		void DidOutputAudioSampleBuffer (ARSession session, CMSampleBuffer audioSampleBuffer);

		[iOS (13, 0)]
		[Export ("session:didOutputCollaborationData:")]
		void DidOutputCollaborationData (ARSession session, ARCollaborationData data);

		[iOS (14, 0)]
		[Export ("session:didChangeGeoTrackingStatus:")]
		void DidChangeGeoTrackingStatus (ARSession session, ARGeoTrackingStatus geoTrackingStatus);
	}

	interface IARSessionDelegate { }

	/// <summary>Delegate object for the <see cref="ARKit.ARSession" /> object, allowing the developer to respond to events relating to the augmented-reality session.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSessionDelegate : ARSessionObserver {

		/// <include file="../docs/api/ARKit/ARSessionDelegate_Extensions.xml" path="/Documentation/Docs[@DocId='M:ARKit.ARSessionDelegate_Extensions.DidUpdateFrame(ARKit.IARSessionDelegate,ARKit.ARSession,ARKit.ARFrame)']/*" />
		[Export ("session:didUpdateFrame:")]
		void DidUpdateFrame (ARSession session, ARFrame frame);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="anchors">The anchors that were added.</param>
		/// <summary>Called when <paramref name="anchors" /> are added to the <paramref name="session" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:didAddAnchors:")]
		void DidAddAnchors (ARSession session, ARAnchor [] anchors);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="anchors">The anchors that were updated.</param>
		/// <summary>Indicates that <paramref name="anchors" /> have been updated due to tracking.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:didUpdateAnchors:")]
		void DidUpdateAnchors (ARSession session, ARAnchor [] anchors);

		/// <param name="session">The session that is supplying the information for the event.</param>
		/// <param name="anchors">The anchors that were removed.</param>
		/// <summary>Called when <paramref name="anchors" /> have been removed from the <paramref name="session" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("session:didRemoveAnchors:")]
		void DidRemoveAnchors (ARSession session, ARAnchor [] anchors);
	}

	/// <summary>Configuration information for augmented reality sessions.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[Abstract]
	[DisableDefaultCtor]
	interface ARConfiguration : NSCopying {

		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[Export ("videoFormat", ArgumentSemantic.Strong)]
		ARVideoFormat VideoFormat { get; set; }

		[Export ("worldAlignment", ArgumentSemantic.Assign)]
		ARWorldAlignment WorldAlignment { get; set; }

		/// <summary>Gets or sets a Boolean value that tells whether ARKit estimates ambient lighting to render augmented reality elements.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lightEstimationEnabled")]
		bool LightEstimationEnabled { [Bind ("isLightEstimationEnabled")] get; set; }

		[Export ("providesAudioData")]
		bool ProvidesAudioData { get; set; }

		[iOS (13, 0)]
		[Export ("frameSemantics", ArgumentSemantic.Assign)]
		ARFrameSemantics FrameSemantics { get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);

		[iOS (16, 0)]
		[Static]
		[NullAllowed, Export ("configurableCaptureDeviceForPrimaryCamera")]
		AVCaptureDevice ConfigurableCaptureDeviceForPrimaryCamera { get; }

		[iOS (16, 0)]
		[Static]
		[NullAllowed, Export ("recommendedVideoFormatFor4KResolution")]
		ARVideoFormat RecommendedVideoFormatFor4KResolution { get; }

		[iOS (16, 0)]
		[Static]
		[NullAllowed, Export ("recommendedVideoFormatForHighResolutionFrameCapturing")]
		ARVideoFormat RecommendedVideoFormatForHighResolutionFrameCapturing { get; }

		[iOS (16, 0)]
		[NullAllowed, Export ("videoHDRAllowed")]
		bool VideoHdrAllowed { get; set; }
	}

	/// <summary>Configuration for a session that tracks the device position and orientation, and optionally detects horizontal surfaces.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface ARWorldTrackingConfiguration {

		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		/// <summary>Gets or sets a value that controls whether autofocus is enabled on the device camera.</summary>
		///         <value>A value that controls whether autofocus is enabled on the device camera.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoFocusEnabled")]
		bool AutoFocusEnabled { [Bind ("isAutoFocusEnabled")] get; set; }

		[Export ("environmentTexturing", ArgumentSemantic.Assign)]
		AREnvironmentTexturing EnvironmentTexturing { get; set; }

		[iOS (13, 0)]
		[Export ("wantsHDREnvironmentTextures")]
		bool WantsHdrEnvironmentTextures { get; set; }

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }

		[NullAllowed, Export ("initialWorldMap", ArgumentSemantic.Strong)]
		ARWorldMap InitialWorldMap { get; set; }

		[NullAllowed] //null_resettable
		[Export ("detectionImages", ArgumentSemantic.Copy)]
		NSSet<ARReferenceImage> DetectionImages { get; set; }

		[iOS (13, 0)]
		[Export ("automaticImageScaleEstimationEnabled")]
		bool AutomaticImageScaleEstimationEnabled { get; set; }

		[Export ("maximumNumberOfTrackedImages")]
		nint MaximumNumberOfTrackedImages { get; set; }

		[Export ("detectionObjects", ArgumentSemantic.Copy)]
		NSSet<ARReferenceObject> DetectionObjects { get; set; }

		[iOS (13, 0)]
		[Export ("collaborationEnabled")]
		bool CollaborationEnabled { [Bind ("isCollaborationEnabled")] get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsUserFaceTracking")]
		bool SupportsUserFaceTracking { get; }

		[iOS (13, 0)]
		[Export ("userFaceTrackingEnabled")]
		bool UserFaceTrackingEnabled { [Bind ("userFaceTrackingEnabled")] get; set; }

		[iOS (14, 3)]
		[Export ("appClipCodeTrackingEnabled")]
		bool AppClipCodeTrackingEnabled { get; set; }

		[iOS (14, 3)]
		[Static]
		[Export ("supportsAppClipCodeTracking")]
		bool SupportsAppClipCodeTracking { get; }

		[iOS (13, 4)]
		[Static]
		[Export ("supportsSceneReconstruction:")]
		bool SupportsSceneReconstruction (ARSceneReconstruction sceneReconstruction);

		[iOS (13, 4)]
		[Export ("sceneReconstruction", ArgumentSemantic.Assign)]
		ARSceneReconstruction SceneReconstruction { get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	/// <summary>An <see cref="ARKit.ARConfiguration" /> that only tracks the device orientation and uses the device's rear-facing cameras.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface AROrientationTrackingConfiguration {

		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		/// <summary>Gets or sets a Boolean value that tells whether the device camera is currently autofocusing.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoFocusEnabled")]
		bool AutoFocusEnabled { [Bind ("isAutoFocusEnabled")] get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	/// <summary>Visualization options for use with the <see cref="SceneKit.SCNView.DebugOptions" /> property of <see cref="ARKit.ARSCNView" />.</summary>
	[NoTV, NoMac]
	[Static]
	interface ARSCNDebugOptions {

		/// <summary>The [0, 0, 0] coordinate of the world coordinate system will be displayed as a large heavy + shape.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("ARSCNDebugOptionShowWorldOrigin")]
		SCNDebugOptions ShowWorldOrigin { get; }

		/// <summary>The point cloud being used by image processing will be shown as a series of + shapes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("ARSCNDebugOptionShowFeaturePoints")]
		SCNDebugOptions ShowFeaturePoints { get; }
	}

	/// <summary>Interface for real-world objects that can be tracked by ARKit.</summary>
	[NoTV, NoMac]
	[Protocol]
	interface ARTrackable {
		/// <summary>Whether the ARKit-calculated transform matches the real-world position and rotation.</summary>
		/// <value>
		///           <see langword="true" /> if the transform accurately represents the real-world position and rotation of the detected object.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("isTracked")]
		bool IsTracked { get; }
	}

	/// <summary>An <see cref="ARKit.ARConfiguration" />  for recognizing and tracking faces.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface ARFaceTrackingConfiguration {
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		[iOS (13, 0)]
		[Static]
		[Export ("supportedNumberOfTrackedFaces")]
		nint SupportedNumberOfTrackedFaces { get; }

		[iOS (13, 0)]
		[Export ("maximumNumberOfTrackedFaces")]
		nint MaximumNumberOfTrackedFaces { get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsWorldTracking")]
		bool SupportsWorldTracking { get; }

		[iOS (13, 0)]
		[Export ("worldTrackingEnabled")]
		bool WorldTrackingEnabled { [Bind ("isWorldTrackingEnabled")] get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	/// <summary>A <see cref="Foundation.DictionaryContainer" /> that defines the features available in <see cref="ARKit.ARBlendShapeLocation" /> .</summary>
	///     <remarks>
	///       <para>All values are in the range [0..1], where 0.0 indicates the neutral state and 1.0 indicates maximum displacement.</para>
	///     </remarks>
	[NoTV, NoMac]
	[StrongDictionary ("ARBlendShapeLocationKeys")]
	interface ARBlendShapeLocationOptions {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float BrowDownLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float BrowDownRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float BrowInnerUp { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float BrowOuterUpLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float BrowOuterUpRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float CheekPuff { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float CheekSquintLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float CheekSquintRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeBlinkLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeBlinkRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookDownLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookDownRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookInLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookInRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookOutLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookOutRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookUpLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeLookUpRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeSquintLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeSquintRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeWideLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float EyeWideRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float JawForward { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float JawLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float JawOpen { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float JawRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthClose { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthDimpleLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthDimpleRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthFrownLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthFrownRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthFunnel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthLowerDownLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthLowerDownRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthPressLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthPressRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthPucker { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthRollLower { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthRollUpper { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthShrugLower { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthShrugUpper { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthSmileLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthSmileRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthStretchLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthStretchRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthUpperUpLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float MouthUpperUpRight { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float NoseSneerLeft { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float NoseSneerRight { get; set; }

		/// <summary>Gets or sets the extension of a tongue.</summary>
		///         <value>The extension of a tongue.</value>
		///         <remarks>To be added.</remarks>
		float TongueOut { get; set; }
	}

	[NoTV, NoMac]
	[Static]
	[Internal]
	interface ARBlendShapeLocationKeys {

		[Field ("ARBlendShapeLocationBrowDownLeft")]
		NSString BrowDownLeftKey { get; }

		[Field ("ARBlendShapeLocationBrowDownRight")]
		NSString BrowDownRightKey { get; }

		[Field ("ARBlendShapeLocationBrowInnerUp")]
		NSString BrowInnerUpKey { get; }

		[Field ("ARBlendShapeLocationBrowOuterUpLeft")]
		NSString BrowOuterUpLeftKey { get; }

		[Field ("ARBlendShapeLocationBrowOuterUpRight")]
		NSString BrowOuterUpRightKey { get; }

		[Field ("ARBlendShapeLocationCheekPuff")]
		NSString CheekPuffKey { get; }

		[Field ("ARBlendShapeLocationCheekSquintLeft")]
		NSString CheekSquintLeftKey { get; }

		[Field ("ARBlendShapeLocationCheekSquintRight")]
		NSString CheekSquintRightKey { get; }

		[Field ("ARBlendShapeLocationEyeBlinkLeft")]
		NSString EyeBlinkLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeBlinkRight")]
		NSString EyeBlinkRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookDownLeft")]
		NSString EyeLookDownLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookDownRight")]
		NSString EyeLookDownRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookInLeft")]
		NSString EyeLookInLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookInRight")]
		NSString EyeLookInRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookOutLeft")]
		NSString EyeLookOutLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookOutRight")]
		NSString EyeLookOutRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookUpLeft")]
		NSString EyeLookUpLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookUpRight")]
		NSString EyeLookUpRightKey { get; }

		[Field ("ARBlendShapeLocationEyeSquintLeft")]
		NSString EyeSquintLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeSquintRight")]
		NSString EyeSquintRightKey { get; }

		[Field ("ARBlendShapeLocationEyeWideLeft")]
		NSString EyeWideLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeWideRight")]
		NSString EyeWideRightKey { get; }

		[Field ("ARBlendShapeLocationJawForward")]
		NSString JawForwardKey { get; }

		[Field ("ARBlendShapeLocationJawLeft")]
		NSString JawLeftKey { get; }

		[Field ("ARBlendShapeLocationJawOpen")]
		NSString JawOpenKey { get; }

		[Field ("ARBlendShapeLocationJawRight")]
		NSString JawRightKey { get; }

		[Field ("ARBlendShapeLocationMouthClose")]
		NSString MouthCloseKey { get; }

		[Field ("ARBlendShapeLocationMouthDimpleLeft")]
		NSString MouthDimpleLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthDimpleRight")]
		NSString MouthDimpleRightKey { get; }

		[Field ("ARBlendShapeLocationMouthFrownLeft")]
		NSString MouthFrownLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthFrownRight")]
		NSString MouthFrownRightKey { get; }

		[Field ("ARBlendShapeLocationMouthFunnel")]
		NSString MouthFunnelKey { get; }

		[Field ("ARBlendShapeLocationMouthLeft")]
		NSString MouthLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthLowerDownLeft")]
		NSString MouthLowerDownLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthLowerDownRight")]
		NSString MouthLowerDownRightKey { get; }

		[Field ("ARBlendShapeLocationMouthPressLeft")]
		NSString MouthPressLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthPressRight")]
		NSString MouthPressRightKey { get; }

		[Field ("ARBlendShapeLocationMouthPucker")]
		NSString MouthPuckerKey { get; }

		[Field ("ARBlendShapeLocationMouthRight")]
		NSString MouthRightKey { get; }

		[Field ("ARBlendShapeLocationMouthRollLower")]
		NSString MouthRollLowerKey { get; }

		[Field ("ARBlendShapeLocationMouthRollUpper")]
		NSString MouthRollUpperKey { get; }

		[Field ("ARBlendShapeLocationMouthShrugLower")]
		NSString MouthShrugLowerKey { get; }

		[Field ("ARBlendShapeLocationMouthShrugUpper")]
		NSString MouthShrugUpperKey { get; }

		[Field ("ARBlendShapeLocationMouthSmileLeft")]
		NSString MouthSmileLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthSmileRight")]
		NSString MouthSmileRightKey { get; }

		[Field ("ARBlendShapeLocationMouthStretchLeft")]
		NSString MouthStretchLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthStretchRight")]
		NSString MouthStretchRightKey { get; }

		[Field ("ARBlendShapeLocationMouthUpperUpLeft")]
		NSString MouthUpperUpLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthUpperUpRight")]
		NSString MouthUpperUpRightKey { get; }

		[Field ("ARBlendShapeLocationNoseSneerLeft")]
		NSString NoseSneerLeftKey { get; }

		[Field ("ARBlendShapeLocationNoseSneerRight")]
		NSString NoseSneerRightKey { get; }

		[Field ("ARBlendShapeLocationTongueOut")]
		NSString TongueOutKey { get; }
	}

	/// <summary>An <see cref="ARKit.ARAnchor" /> that locates a detected face in the AR session's world coordinates.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARFaceAnchor : ARTrackable {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[Export ("geometry")]
		ARFaceGeometry Geometry { get; }

		[Export ("leftEyeTransform")]
		Matrix4 LeftEyeTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("rightEyeTransform")]
		Matrix4 RightEyeTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("lookAtPoint")]
		Vector3 LookAtPoint {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("blendShapes")]
		NSDictionary WeakBlendShapes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakBlendShapes")]
		ARBlendShapeLocationOptions BlendShapes { get; }
	}

	/// <summary>A mesh that represents a recognized face, including shape and expression.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARFaceGeometry : NSCopying, NSSecureCoding {
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("initWithBlendShapes:")]
		NativeHandle Constructor (NSDictionary blendShapes);

		/// <param name="blendShapes">To be added.</param>
		/// <summary>Constructor that instantiates facial geometry with the expression specified in s<paramref name="blendShapes" />. Requires hardware support for face-tracking.</summary>
		/// <remarks>
		///           <para>This constructor will throw an <see cref="ObjCRuntime.Class.ThrowOnInitFailure" /> exception if run on a device that does not support face-tracking.</para>
		///         </remarks>
		[Wrap ("this (blendShapes.GetDictionary ()!)")]
		NativeHandle Constructor (ARBlendShapeLocationOptions blendShapes);

		[Export ("vertexCount")]
		nuint VertexCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("vertices")]
		IntPtr GetRawVertices ();

		[Export ("textureCoordinateCount")]
		nuint TextureCoordinateCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("textureCoordinates")]
		IntPtr GetRawTextureCoordinates ();

		[Export ("triangleCount")]
		nuint TriangleCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("triangleIndices")]
		IntPtr GetRawTriangleIndices ();
	}

	/// <summary>SceneKit geometry that represents a face.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (SCNGeometry))]
	[DisableDefaultCtor]
	interface ARSCNFaceGeometry {
		[Static]
		[Export ("faceGeometryWithDevice:")]
		[return: NullAllowed]
		ARSCNFaceGeometry Create (IMTLDevice device);

		[Static]
		[Export ("faceGeometryWithDevice:fillMesh:")]
		[return: NullAllowed]
		ARSCNFaceGeometry Create (IMTLDevice device, bool fillMesh);

		[Export ("updateFromFaceGeometry:")]
		void Update (ARFaceGeometry faceGeometry);
	}

	/// <summary>A <see cref="ARKit.ARAnchor" /> that tracks an image detected in the real world.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARImageAnchor : ARTrackable {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[Export ("referenceImage", ArgumentSemantic.Strong)]
		ARReferenceImage ReferenceImage { get; }

		[iOS (13, 0)]
		[Export ("estimatedScaleFactor")]
		nfloat EstimatedScaleFactor { get; }
	}

	/// <summary>Estimates real-world illumination falling on a face.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARLightEstimate))]
	[DisableDefaultCtor]
	interface ARDirectionalLightEstimate {
		[Export ("sphericalHarmonicsCoefficients", ArgumentSemantic.Copy)]
		NSData SphericalHarmonicsCoefficients { get; }

		[Export ("primaryLightDirection")]
		Vector3 PrimaryLightDirection {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("primaryLightIntensity")]
		nfloat PrimaryLightIntensity { get; }
	}

	/// <summary>
	///       <see cref="ARKit.ARConfiguration" /> subclass that uses a recognized image as a basis for world-tracking.</summary>
	///     <remarks>
	///       <para>Developers should use this subclass of <see cref="ARKit.ARConfiguration" /> in scenarios where a known image, viewable by the rear-facing camera, is a more reliable anchor than real-world features from a broader field of view. For instance, a poster or advetisement located in a moving train car. </para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface ARImageTrackingConfiguration {
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		/// <summary>Gets or sets whether the camera is using autofocus. If <see langword="false" />, fixed focus is used.</summary>
		///         <value>The default value is <see langword="true" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoFocusEnabled")]
		bool AutoFocusEnabled { [Bind ("isAutoFocusEnabled")] get; set; }

		[Export ("trackingImages", ArgumentSemantic.Copy)]
		NSSet<ARReferenceImage> TrackingImages { get; set; }

		[Export ("maximumNumberOfTrackedImages")]
		nint MaximumNumberOfTrackedImages { get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	/// <summary>A resource-intensive <see cref="ARKit.ARConfiguration" /> used during development to create <see cref="ARKit.ARReferenceObject" /> data.</summary>
	///     <remarks>
	///       <para>This <see cref="ARKit.ARConfiguration" /> is intended to be used during development only. It is expensive in terms of computation, power, and memory. Developers can use the higher-fidelity data generated in sessions using this configuration to create <see cref="ARKit.ARReferenceObject" /> instances that can be used later.</para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface ARObjectScanningConfiguration {
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		/// <summary>Gets or sets whether the camera should use autofocusing. If <see langword="false" />, manual focus is used.</summary>
		///         <value>The default value is <see langword="true" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoFocusEnabled")]
		bool AutoFocusEnabled { [Bind ("isAutoFocusEnabled")] get; set; }

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }

		[iOS (13, 0)]
		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	/// <summary>Source for environmentally-based lighting.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface AREnvironmentProbeAnchor {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[Export ("initWithTransform:extent:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (Matrix4 transform, Vector3 extent);

		[Export ("initWithName:transform:extent:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, Matrix4 transform, Vector3 extent);

		[NullAllowed, Export ("environmentTexture", ArgumentSemantic.Strong)]
		IMTLTexture EnvironmentTexture { get; }

		[Export ("extent")]
		Vector3 Extent {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
	}

	/// <summary>Digital representation of a 3D object to be detected in the real world.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARReferenceObject : NSSecureCoding {
		[Export ("initWithArchiveURL:error:")]
		NativeHandle Constructor (NSUrl archiveUrl, [NullAllowed] out NSError error);

		[NullAllowed, Export ("name")]
		string Name { get; set; }

		[Export ("center")]
		Vector3 Center {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("extent")]
		Vector3 Extent {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("scale")]
		Vector3 Scale {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[iOS (13, 0)]
		[NullAllowed, Export ("resourceGroupName", ArgumentSemantic.Strong)]
		string ResourceGroupName { get; }

		[Export ("rawFeaturePoints", ArgumentSemantic.Strong)]
		ARPointCloud RawFeaturePoints { get; }

		[Static]
		[Export ("referenceObjectsInGroupNamed:bundle:")]
		[return: NullAllowed]
		NSSet<ARReferenceObject> GetReferenceObjects (string resourceGroupName, [NullAllowed] NSBundle bundle);

		[Export ("exportObjectToURL:previewImage:error:")]
		bool Export (NSUrl url, [NullAllowed] UIImage previewImage, [NullAllowed] out NSError error);

		[Export ("referenceObjectByApplyingTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		ARReferenceObject ApplyTransform (Matrix4 transform);

		[Export ("referenceObjectByMergingObject:error:")]
		[return: NullAllowed]
		ARReferenceObject Merge (ARReferenceObject @object, [NullAllowed] out NSError error);

		/// <summary>The file extension used for archived 3D objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("ARReferenceObjectArchiveExtension")]
		NSString ArchiveExtension { get; }
	}

	/// <summary>
	///       <see cref="ARKit.ARAnchor" /> subclass that tracks a recognized real-world 3D object.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARObjectAnchor {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[Export ("referenceObject", ArgumentSemantic.Strong)]
		ARReferenceObject ReferenceObject { get; }
	}

	/// <summary>A serializable and shareable combination of real-world spatial data points and mixed-reality anchors.</summary>
	///     <remarks>
	///       <para>Developers can use an <see cref="ARKit.ARWorldMap" /> to serialize a mixed-reality scene. The serialized data can be used at a later time to recreate a scene or shared with another user to create a shared experience.</para>
	///     </remarks>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARWorldMap : NSCopying, NSSecureCoding {
		[Export ("center")]
		Vector3 Center {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("extent")]
		Vector3 Extent {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("anchors", ArgumentSemantic.Copy)]
		ARAnchor [] Anchors { get; set; }

		[Export ("rawFeaturePoints", ArgumentSemantic.Strong)]
		ARPointCloud RawFeaturePoints { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARBody2D {

		[Export ("skeleton")]
		ARSkeleton2D Skeleton { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARBodyAnchor : ARTrackable {

		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		// [Export ("initWithTransform:")] marked as NS_UNAVAILABLE
		// [Export ("initWithName:")] marked as NS_UNAVAILABLE

		[Export ("skeleton", ArgumentSemantic.Strong)]
		ARSkeleton3D Skeleton { get; }

		[Export ("estimatedScaleFactor")]
		nfloat EstimatedScaleFactor { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (UIView))]
	interface ARCoachingOverlayView {

		// inherited from UIView
		[DesignatedInitializer]
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IARCoachingOverlayViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("sessionProvider", ArgumentSemantic.Weak)]
		IARSessionProviding SessionProvider { get; set; }

		[NullAllowed, Export ("session", ArgumentSemantic.Strong)]
		ARSession Session { get; set; }

		[Export ("goal", ArgumentSemantic.Assign)]
		ARCoachingGoal Goal { get; set; }

		[Export ("activatesAutomatically")]
		bool ActivatesAutomatically { get; set; }

		[Export ("isActive")]
		bool IsActive { get; }

		[Export ("setActive:animated:")]
		void SetActive (bool active, bool animated);
	}

	interface IARCoachingOverlayViewDelegate { }

	[iOS (13, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARCoachingOverlayViewDelegate {

		[Export ("coachingOverlayViewDidRequestSessionReset:")]
		void DidRequestSessionReset (ARCoachingOverlayView coachingOverlayView);

		[Export ("coachingOverlayViewWillActivate:")]
		void WillActivate (ARCoachingOverlayView coachingOverlayView);

		[Export ("coachingOverlayViewDidDeactivate:")]
		void DidDeactivate (ARCoachingOverlayView coachingOverlayView);
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARCollaborationData : NSSecureCoding {

		[Export ("priority")]
		ARCollaborationDataPriority Priority { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARConfiguration))]
	interface ARBodyTrackingConfiguration {

		// From the parent, needed in all subclasses
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		[Export ("autoFocusEnabled")]
		bool AutoFocusEnabled { [Bind ("isAutoFocusEnabled")] get; set; }

		[NullAllowed, Export ("initialWorldMap", ArgumentSemantic.Strong)]
		ARWorldMap InitialWorldMap { get; set; }

		[Export ("environmentTexturing", ArgumentSemantic.Assign)]
		AREnvironmentTexturing EnvironmentTexturing { get; set; }

		[Export ("wantsHDREnvironmentTextures")]
		bool WantsHdrEnvironmentTextures { get; set; }

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }

		[Export ("detectionImages", ArgumentSemantic.Copy)]
		NSSet<ARReferenceImage> DetectionImages { get; set; }

		[Export ("automaticImageScaleEstimationEnabled")]
		bool AutomaticImageScaleEstimationEnabled { get; set; }

		[Export ("automaticSkeletonScaleEstimationEnabled")]
		bool AutomaticSkeletonScaleEstimationEnabled { get; set; }

		[Export ("maximumNumberOfTrackedImages")]
		nint MaximumNumberOfTrackedImages { get; set; }

		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);

		[iOS (14, 3)]
		[Export ("appClipCodeTrackingEnabled")]
		bool AppClipCodeTrackingEnabled { get; set; }

		[iOS (14, 3)]
		[Static]
		[Export ("supportsAppClipCodeTracking")]
		bool SupportsAppClipCodeTracking { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARConfiguration))]
	interface ARPositionalTrackingConfiguration {

		// From the parent, needed in all subclasses
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }

		[NullAllowed, Export ("initialWorldMap", ArgumentSemantic.Strong)]
		ARWorldMap InitialWorldMap { get; set; }

		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARMatteGenerator {

		[DesignatedInitializer]
		[Export ("initWithDevice:matteResolution:")]
		NativeHandle Constructor (IMTLDevice device, ARMatteResolution matteResolution);

		[Export ("generateMatteFromFrame:commandBuffer:")]
		IMTLTexture GenerateMatte (ARFrame frame, IMTLCommandBuffer commandBuffer);

		[Export ("generateDilatedDepthFromFrame:commandBuffer:")]
		IMTLTexture GenerateDilatedDepth (ARFrame frame, IMTLCommandBuffer commandBuffer);
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARQuickLookPreviewItem : QLPreviewItem {

		[Export ("initWithFileAtURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url);

		[NullAllowed, Export ("canonicalWebPageURL", ArgumentSemantic.Strong)]
		NSUrl CanonicalWebPageUrl { get; set; }

		[Export ("allowsContentScaling")]
		bool AllowsContentScaling { get; set; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARRaycastQuery {

		[Export ("initWithOrigin:direction:allowingTarget:alignment:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (Vector3 origin, Vector3 direction, ARRaycastTarget target, ARRaycastTargetAlignment alignment);

		[Export ("origin")]
		Vector3 Origin {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("direction")]
		Vector3 Direction {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("target")]
		ARRaycastTarget Target { get; }

		[Export ("targetAlignment")]
		ARRaycastTargetAlignment TargetAlignment { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARRaycastResult {

		[Export ("worldTransform")]
		Matrix4 WorldTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("target", ArgumentSemantic.Assign)]
		ARRaycastTarget Target { get; }

		[Export ("targetAlignment", ArgumentSemantic.Assign)]
		ARRaycastTargetAlignment TargetAlignment { get; }

		[NullAllowed, Export ("anchor", ArgumentSemantic.Strong)]
		ARAnchor Anchor { get; }
	}

	interface IARSessionProviding { }

	[iOS (13, 0)]
	[Protocol]
	interface ARSessionProviding {

		[Abstract]
		[Export ("session")]
		ARSession Session { get; }
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARSkeleton {

		[Export ("definition")]
		ARSkeletonDefinition Definition { get; }

		[Export ("jointCount")]
		nuint JointCount { get; }

		[Export ("isJointTracked:")]
		bool IsJointTracked (nint jointIndex);
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARSkeleton))]
	[DisableDefaultCtor]
	interface ARSkeleton3D {

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("jointModelTransforms")]
		IntPtr RawJointModelTransforms { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("jointLocalTransforms")]
		IntPtr RawJointLocalTransforms { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("modelTransformForJointName:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetModelTransform (NSString jointName);

		[Wrap ("GetModelTransform (jointName.GetConstant()!)", IsVirtual = true)]
		Matrix4 GetModelTransform (ARSkeletonJointName jointName);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("localTransformForJointName:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetLocalTransform (NSString jointName);

		[Wrap ("GetLocalTransform (jointName.GetConstant()!)", IsVirtual = true)]
		Matrix4 GetLocalTransform (ARSkeletonJointName jointName);
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARSkeleton))]
	[DisableDefaultCtor]
	interface ARSkeleton2D {

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("jointLandmarks")]
		IntPtr RawJointLandmarks { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("landmarkForJointNamed:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector2 GetLandmarkPoint (NSString jointName);

		[Wrap ("GetLandmarkPoint (jointName.GetConstant()!)", IsVirtual = true)]
		Vector2 GetLandmarkPoint (ARSkeletonJointName jointName);
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARSkeletonDefinition {

		[Static]
		[Export ("defaultBody3DSkeletonDefinition")]
		ARSkeletonDefinition DefaultBody3DSkeletonDefinition { get; }

		[Static]
		[Export ("defaultBody2DSkeletonDefinition")]
		ARSkeletonDefinition DefaultBody2DSkeletonDefinition { get; }

		[Export ("jointCount")]
		nuint JointCount { get; }

		[Export ("jointNames")]
		string [] JointNames { get; }

		[Export ("parentIndices")]
		NSNumber [] ParentIndices { get; }

		[NullAllowed, Export ("neutralBodySkeleton3D")]
		ARSkeleton3D NeutralBodySkeleton3D { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("indexForJointName:")]
		nuint GetJointIndex (NSString jointName);

		[Wrap ("GetJointIndex (jointName.GetConstant()!)")]
		nuint GetJointIndex (ARSkeletonJointName jointName);
	}

	[iOS (13, 0)]
	enum ARSkeletonJointName {

		[Field ("ARSkeletonJointNameRoot")]
		Root,

		[Field ("ARSkeletonJointNameHead")]
		Head,

		[Field ("ARSkeletonJointNameLeftHand")]
		LeftHand,

		[Field ("ARSkeletonJointNameRightHand")]
		RightHand,

		[Field ("ARSkeletonJointNameLeftFoot")]
		LeftFoot,

		[Field ("ARSkeletonJointNameRightFoot")]
		RightFoot,

		[Field ("ARSkeletonJointNameLeftShoulder")]
		LeftShoulder,

		[Field ("ARSkeletonJointNameRightShoulder")]
		RightShoulder,
	}

	[iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARTrackedRaycast {

		[Export ("stopTracking")]
		void StopTracking ();
	}

	[iOS (13, 0)]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARParticipantAnchor {

		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		// [Export ("initWithTransform:")] marked as NS_UNAVAILABLE
		// [Export ("initWithName:")] marked as NS_UNAVAILABLE
	}

	[iOS (13, 4)]
	[Native]
	[Flags]
	enum ARSceneReconstruction : ulong {
		None = 0,
		Mesh = 1,
		MeshWithClassification = (1 << 1) | (1 << 0),
	}

	[iOS (13, 4)]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARMeshAnchor {

		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		// [Export ("initWithTransform:")] marked as NS_UNAVAILABLE
		// [Export ("initWithName:")] marked as NS_UNAVAILABLE

		[Export ("geometry")]
		ARMeshGeometry Geometry { get; }
	}

	[iOS (13, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARGeometrySource : NSSecureCoding {

		[Export ("buffer", ArgumentSemantic.Strong)]
		IMTLBuffer Buffer { get; }

		[Export ("count")]
		nint Count { get; }

		[Export ("format", ArgumentSemantic.Assign)]
		MTLVertexFormat Format { get; }

		[Export ("componentsPerVector")]
		nint ComponentsPerVector { get; }

		[Export ("offset")]
		nint Offset { get; }

		[Export ("stride")]
		nint Stride { get; }
	}

	[iOS (13, 4)]
	[Native]
	enum ARGeometryPrimitiveType : long {
		Line,
		Triangle,
	}

	[iOS (13, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARGeometryElement : NSSecureCoding {

		[Export ("buffer", ArgumentSemantic.Strong)]
		IMTLBuffer Buffer { get; }

		[Export ("count")]
		nint Count { get; }

		[Export ("bytesPerIndex")]
		nint BytesPerIndex { get; }

		[Export ("indexCountPerPrimitive")]
		nint IndexCountPerPrimitive { get; }

		[Export ("primitiveType", ArgumentSemantic.Assign)]
		ARGeometryPrimitiveType PrimitiveType { get; }
	}

	[iOS (13, 4)]
	[Native]
	enum ARMeshClassification : long {
		None,
		Wall,
		Floor,
		Ceiling,
		Table,
		Seat,
		Window,
		Door,
	}

	[iOS (13, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARMeshGeometry : NSSecureCoding {

		[Export ("vertices", ArgumentSemantic.Strong)]
		ARGeometrySource Vertices { get; }

		[Export ("normals", ArgumentSemantic.Strong)]
		ARGeometrySource Normals { get; }

		[Export ("faces", ArgumentSemantic.Strong)]
		ARGeometryElement Faces { get; }

		[Export ("classification", ArgumentSemantic.Strong)]
		[NullAllowed]
		ARGeometrySource Classification { get; }
	}

	[iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARDepthData {
		[Export ("depthMap", ArgumentSemantic.Assign)]
		CVPixelBuffer DepthMap { get; }

		[NullAllowed, Export ("confidenceMap", ArgumentSemantic.Assign)]
		CVPixelBuffer ConfidenceMap { get; }
	}

	[iOS (14, 0)]
	[BaseType (typeof (ARAnchor))]
	interface ARGeoAnchor : ARTrackable {
		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get; }

		[Export ("altitude")]
		double Altitude { get; }

		[Export ("altitudeSource", ArgumentSemantic.Assign)]
		ARAltitudeSource AltitudeSource { get; }

		[Export ("initWithCoordinate:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate);

		[Export ("initWithCoordinate:altitude:")]
		NativeHandle Constructor (CLLocationCoordinate2D coordinate, double altitude);

		[Export ("initWithName:coordinate:")]
		NativeHandle Constructor (string name, CLLocationCoordinate2D coordinate);

		[Export ("initWithName:coordinate:altitude:")]
		NativeHandle Constructor (string name, CLLocationCoordinate2D coordinate, double altitude);
	}

	[iOS (14, 0)]
	[BaseType (typeof (ARConfiguration))]
	interface ARGeoTrackingConfiguration {
		[Static]
		[Export ("supportedVideoFormats")]
		ARVideoFormat [] GetSupportedVideoFormats ();

		[Export ("environmentTexturing", ArgumentSemantic.Assign)]
		AREnvironmentTexturing EnvironmentTexturing { get; set; }

		[Export ("wantsHDREnvironmentTextures")]
		bool WantsHdrEnvironmentTextures { get; set; }

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }

		[NullAllowed, Export ("detectionImages", ArgumentSemantic.Copy)]
		NSSet<ARReferenceImage> DetectionImages { get; set; }

		[Export ("automaticImageScaleEstimationEnabled")]
		bool AutomaticImageScaleEstimationEnabled { get; set; }

		[Export ("maximumNumberOfTrackedImages")]
		nint MaximumNumberOfTrackedImages { get; set; }

		[iOS (14, 3)]
		[Export ("appClipCodeTrackingEnabled")]
		bool AppClipCodeTrackingEnabled { get; set; }

		[iOS (14, 3)]
		[Static]
		[Export ("supportsAppClipCodeTracking")]
		bool SupportsAppClipCodeTracking { get; }

		[Export ("detectionObjects", ArgumentSemantic.Copy)]
		NSSet<ARReferenceObject> DetectionObjects { get; set; }

		[Async]
		[Static]
		[Export ("checkAvailabilityWithCompletionHandler:")]
		void CheckAvailability (Action<bool, NSError?> completionHandler);

		[Async]
		[Static]
		[Export ("checkAvailabilityAtCoordinate:completionHandler:")]
		void CheckAvailability (CLLocationCoordinate2D coordinate, Action<bool, NSError?> completionHandler);

		[Static]
		[Export ("new")]
		[return: Release]
		ARGeoTrackingConfiguration Create ();

		[Static]
		[Export ("supportsFrameSemantics:")]
		bool SupportsFrameSemantics (ARFrameSemantics frameSemantics);
	}

	[iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARGeoTrackingStatus : NSCopying, NSSecureCoding {
		[Export ("state")]
		ARGeoTrackingState State { get; }

		[Export ("accuracy")]
		ARGeoTrackingAccuracy Accuracy { get; }

		[Export ("stateReason")]
		ARGeoTrackingStateReason StateReason { get; }
	}

	[iOS (14, 3)]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARAppClipCodeAnchor : ARTrackable {

		// Inlined from 'ARAnchorCopying' protocol (we can't have constructors in interfaces)
		[Export ("initWithAnchor:")]
		NativeHandle Constructor (ARAnchor anchor);

		[NullAllowed, Export ("url", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		[Export ("urlDecodingState", ArgumentSemantic.Assign)]
		ARAppClipCodeUrlDecodingState UrlDecodingState { get; }

		[Export ("radius")]
		float Radius { get; }
	}

	[iOS (16, 0)]
	[BaseType (typeof (NSObject))]
	interface ARPlaneExtent : NSSecureCoding {
		[Export ("rotationOnYAxis")]
		float RotationOnYAxis { get; }

		[Export ("width")]
		float Width { get; }

		[Export ("height")]
		float Height { get; }
	}


}
