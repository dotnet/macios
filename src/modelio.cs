//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2015 Xamarin, Inc.
//
//

using System;
using System.ComponentModel;

using AudioUnit;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreGraphics;
using SceneKit;
#if NET
using Vector2 = global::System.Numerics.Vector2;
using Vector2d = global::CoreGraphics.NVector2d;
using Vector2i = global::CoreGraphics.NVector2i;
using NVector3d = global::CoreGraphics.NVector3d;
using NVector3 = global::CoreGraphics.NVector3;
using Vector3 = global::System.Numerics.Vector3;
using Vector3i = global::CoreGraphics.NVector3i;
using Vector4 = global::System.Numerics.Vector4;
using Vector4d = global::CoreGraphics.NVector4d;
using Vector4i = global::CoreGraphics.NVector4i;
using Matrix2 = global::CoreGraphics.NMatrix2;
using Matrix3 = global::CoreGraphics.NMatrix3;
using Matrix4 = global::CoreGraphics.NMatrix4;
using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using NMatrix4 = global::CoreGraphics.NMatrix4;
using NMatrix4d = global::CoreGraphics.NMatrix4d;
using Quaterniond = global::CoreGraphics.NQuaterniond;
using Quaternion = global::System.Numerics.Quaternion;
#else
using Vector2 = global::OpenTK.Vector2;
using Vector2d = global::OpenTK.Vector2d;
using Vector2i = global::OpenTK.Vector2i;
using NVector3d = global::OpenTK.NVector3d;
using NVector3 = global::OpenTK.NVector3;
using Vector3 = global::OpenTK.Vector3;
using Vector3i = global::OpenTK.Vector3i;
using Vector4 = global::OpenTK.Vector4;
using Vector4d = global::OpenTK.Vector4d;
using Vector4i = global::OpenTK.Vector4i;
using Matrix2 = global::OpenTK.Matrix2;
using Matrix3 = global::OpenTK.Matrix3;
using Matrix4 = global::OpenTK.Matrix4;
using MatrixFloat4x4 = global::OpenTK.NMatrix4;
using NMatrix4 = global::OpenTK.NMatrix4;
using NMatrix4d = global::OpenTK.NMatrix4d;
using Quaterniond = global::OpenTK.Quaterniond;
using Quaternion = global::OpenTK.Quaternion;
#endif

#if MONOMAC
using AppKit;
using AUViewControllerBase = AppKit.NSViewController;
#else
using UIKit;
using AUViewControllerBase = UIKit.UIViewController;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace ModelIO {

	[MacCatalyst (13, 1)]
	[Native]
	enum MDLAnimatedValueInterpolation : ulong {
		/// <summary>To be added.</summary>
		Constant,
		/// <summary>To be added.</summary>
		Linear,
	}

	[MacCatalyst (13, 1)]
	[Native]
	enum MDLTransformOpRotationOrder : ulong {
		Xyz = 1,
		Xzy,
		Yxz,
		Yzx,
		Zxy,
		Zyx,
	}

	[MacCatalyst (13, 1)]
	[Native]
	enum MDLDataPrecision : ulong {
		/// <summary>To be added.</summary>
		Undefined,
		/// <summary>To be added.</summary>
		Float,
		/// <summary>To be added.</summary>
		Double,
	}

	delegate void MDLObjectHandler (MDLObject mdlObject, ref bool stop);

	/// <summary>A basic high-performance light for illuminating scenes from a lit shape.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLAreaLight_Class/index.html">Apple documentation for <c>MDLAreaLight</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLPhysicallyPlausibleLight))]
	[DisableDefaultCtor]
	interface MDLAreaLight {
		[Export ("areaRadius")]
		float AreaRadius { get; set; }

		[Export ("superEllipticPower", ArgumentSemantic.Assign)]
		Vector2 SuperEllipticPower {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Export ("aspect")]
		float Aspect { get; set; }
	}

	/// <summary>A container that stores 3D assets as an indexed collection of <see cref="T:ModelIO.MDLObject" /> trees.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLAsset_Class/index.html">Apple documentation for <c>MDLAsset</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLAsset : NSCopying {
		[Export ("initWithURL:")]
		NativeHandle Constructor (NSUrl url);

		[Export ("initWithURL:vertexDescriptor:bufferAllocator:")]
		NativeHandle Constructor ([NullAllowed] NSUrl url, [NullAllowed] MDLVertexDescriptor vertexDescriptor, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);

		[MacCatalyst (13, 1)]
		[Export ("initWithBufferAllocator:")]
		NativeHandle Constructor ([NullAllowed] IMDLMeshBufferAllocator bufferAllocator);

		[Export ("initWithURL:vertexDescriptor:bufferAllocator:preserveTopology:error:")]
		NativeHandle Constructor (NSUrl url, [NullAllowed] MDLVertexDescriptor vertexDescriptor, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator, bool preserveTopology, out NSError error);

		// note: by choice we do not export "exportAssetToURL:"
		[Export ("exportAssetToURL:error:")]
		bool ExportAssetToUrl (NSUrl url, out NSError error);

		[MacCatalyst (13, 1)]
		[Export ("objectAtPath:")]
		MDLObject GetObject (string atPath);

		[Static]
		[Export ("canImportFileExtension:")]
		bool CanImportFileExtension (string extension);

		[Static]
		[Export ("canExportFileExtension:")]
		bool CanExportFileExtension (string extension);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("components", ArgumentSemantic.Copy)]
		IMDLComponent [] Components { get; }

		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("setComponent:forProtocol:")]
		void SetComponent (IMDLComponent component, Protocol protocol);

		[MacCatalyst (13, 1)]
		[Wrap ("SetComponent (component, new Protocol (type))")]
		void SetComponent (IMDLComponent component, Type type);

		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("componentConformingToProtocol:")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Protocol protocol);

		[MacCatalyst (13, 1)]
		[Wrap ("GetComponent (new Protocol (type!))")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Type type);

		[MacCatalyst (13, 1)]
		[Export ("childObjectsOfClass:")]
		MDLObject [] GetChildObjects (Class objectClass);

		[MacCatalyst (13, 1)]
		[Export ("loadTextures")]
		void LoadTextures ();

		[Export ("boundingBoxAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLAxisAlignedBoundingBox GetBoundingBox (double atTime);

		/// <summary>Gets the smallest box that contains all of the objects in the asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>If any of the asset's contents contain timed information, application developers should use <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Model%20IOMDLObject%20Bounding%20Box%20At%20Time&amp;scope=Xamarin" title="M:ModelIO.MDLObject.BoundingBoxAtTime*">M:ModelIO.MDLObject.BoundingBoxAtTime*</a></format>, instead.</remarks>
		[Export ("boundingBox")]
		MDLAxisAlignedBoundingBox BoundingBox {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		/// <summary>Gets or sets the frame interval for timed data in the asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("frameInterval")]
		double FrameInterval { get; set; }

		/// <summary>Gets the timestamp of the first timed data item in the asset, which is the lowest value to which all requested timed sample data are clamped.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startTime")]
		double StartTime { get; set; }

		/// <summary>Gets the timestamp of the last timed data item in the asset, which is the highest value to which all requested timed sample data are clamped.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endTime")]
		double EndTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("upAxis", ArgumentSemantic.Assign)]
		NVector3 UpAxis {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets the URL from which the asset was loaded, or <see langword="null" /> if the asset was not loaded from a URL.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URL", ArgumentSemantic.Retain)]
		NSUrl Url { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("resolver", ArgumentSemantic.Retain)]
		IMDLAssetResolver Resolver { get; set; }

		/// <summary>Gets the allocator that the application developer specified when constructing the asset, or an internal allocator if none was specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bufferAllocator", ArgumentSemantic.Retain)]
		IMDLMeshBufferAllocator BufferAllocator { get; }

		/// <summary>Gets or sets the description of in-memory vertex data.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("vertexDescriptor", ArgumentSemantic.Retain)]
		MDLVertexDescriptor VertexDescriptor { get; }

		[Export ("addObject:")]
		void AddObject (MDLObject @object);

		[Export ("removeObject:")]
		void RemoveObject (MDLObject @object);

		/// <summary>Gets the number of top-level <see cref="T:ModelIO.MDLObject" /> nodes that are indexed by the asset, each of which contains zero or more child <see cref="T:ModelIO.MDLObject" /> nodes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nuint Count { get; }

		[Export ("objectAtIndexedSubscript:")]
		[return: NullAllowed]
		MDLObject GetObjectAtIndexedSubscript (nuint index);

		[Export ("objectAtIndex:")]
		MDLObject GetObject (nuint index);

		/// <summary>Gets or sets the underlying object that contains the asset's object hierarchy.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the 'Originals' property instead.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use the 'Originals' property instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the 'Originals' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the 'Originals' property instead.")]
		[MacCatalyst (13, 1)]
		[Export ("masters", ArgumentSemantic.Retain)]
		IMDLObjectContainerComponent Masters { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("originals", ArgumentSemantic.Retain)]
		IMDLObjectContainerComponent Originals { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("animations", ArgumentSemantic.Retain)]
		IMDLObjectContainerComponent Animations { get; set; }

		[Static]
		[Export ("assetWithSCNScene:")]
		MDLAsset FromScene (SCNScene scene);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("assetWithSCNScene:bufferAllocator:")]
		MDLAsset FromScene (SCNScene scene, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);

		// MDLAsset_MDLLightBaking (category)

		[Static]
		[Export ("placeLightProbesWithDensity:heuristic:usingIrradianceDataSource:")]
		[MacCatalyst (13, 1)]
		MDLLightProbe [] PlaceLightProbes (float density, MDLProbePlacement type, IMDLLightProbeIrradianceDataSource dataSource);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.MDLLightProbeIrradianceDataSource" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.MDLLightProbeIrradianceDataSource" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.MDLLightProbeIrradianceDataSource" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:ModelIO.MDLLightProbeIrradianceDataSource_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMDLLightProbeIrradianceDataSource { }

	// Added in iOS 10 SDK but it is supposed to be present in iOS 9.
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ModelIO/MDLLightProbeIrradianceDataSource">Apple documentation for <c>MDLLightProbeIrradianceDataSource</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface MDLLightProbeIrradianceDataSource {
		[Abstract]
		[Export ("boundingBox", ArgumentSemantic.Assign)]
		MDLAxisAlignedBoundingBox BoundingBox { get; set; }

		[Export ("sphericalHarmonicsLevel")]
		nuint SphericalHarmonicsLevel { get; set; }

		[Export ("sphericalHarmonicsCoefficientsAtPosition:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NSData GetSphericalHarmonicsCoefficients (Vector3 position);
	}

	/// <summary>A point of view that looks into a 3D scence, and that can simulate certain aspects of a physical camera.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLCamera_Class/index.html">Apple documentation for <c>MDLCamera</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	interface MDLCamera {
		/// <summary>Gets or sets the transformation that determines what portion of a scene that the camera can view.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("projectionMatrix")]
#if !NET
		[Obsolete ("Use 'ProjectionMatrix4x4' instead.")]
#endif
		Matrix4 ProjectionMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

#if !NET
		[Sealed]
		[Export ("projectionMatrix")]
		MatrixFloat4x4 ProjectionMatrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif

		/// <summary>Gets or sets a value that controls whether the projection is perspective or orthographic.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("projection", ArgumentSemantic.Assign)]
		MDLCameraProjection Projection { get; set; }

		[Export ("frameBoundingBox:setNearAndFar:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void FrameBoundingBox (MDLAxisAlignedBoundingBox boundingBox, bool setNearAndFar);

		[Export ("lookAt:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void LookAt (Vector3 focusPosition);

		[Export ("lookAt:from:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void LookAt (Vector3 focusPosition, Vector3 cameraPosition);

		[Export ("rayTo:forViewPort:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 RayTo (Vector2i pixel, Vector2i size);

		/// <summary>Gets or sets the near clipping plane.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("nearVisibilityDistance")]
		float NearVisibilityDistance { get; set; }

		/// <summary>Gets or sets the back clipping plane.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("farVisibilityDistance")]
		float FarVisibilityDistance { get; set; }

		/// <summary>Gets or sets the coefficient of the quadratic term in the formula that is used to calculate radial distortion.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("barrelDistortion")]
		float BarrelDistortion { get; set; }

		/// <summary>Gets or sets the scale factor that transforms world coordinates into meters.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("worldToMetersConversionScale")]
		float WorldToMetersConversionScale { get; set; }

		/// <summary>Gets or sets the coefficient of the fourth-power term in the formula that is used to calculate radial distortion.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fisheyeDistortion")]
		float FisheyeDistortion { get; set; }

		/// <summary>Gets or sets a value that controls the degree to which image brightness is reduced as the edge of the simulated physical aperture is approached.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("opticalVignetting")]
		float OpticalVignetting { get; set; }

		/// <summary>Gets or sets the degree to which different colors are focussed differently by the camera. 0.0 is no chromatic aberration. 1.0 is the maximum amount.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("chromaticAberration")]
		float ChromaticAberration { get; set; }

		/// <summary>Gets or sets the focal length and updates the field of view accordingly.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("focalLength")]
		float FocalLength { get; set; }

		/// <summary>Gets or sets the focal distance, in meters.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("focusDistance")]
		float FocusDistance { get; set; }

		/// <summary>Gets or sets the field of view, and updates the focal length of the camera accordingly.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fieldOfView")]
		float FieldOfView { get; set; }

		/// <summary>Gets or sets the focal ratio of the camera.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fStop")]
		float FStop { get; set; }

		/// <summary>Gets or sets the number of blades to use when simulating bokeh in out-of-focus elements in the scene.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("apertureBladeCount", ArgumentSemantic.Assign)]
		nuint ApertureBladeCount { get; set; }

		/// <summary>Gets or sets the largest size of bokeh from point sources of light.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumCircleOfConfusion")]
		float MaximumCircleOfConfusion { get; set; }

		[Export ("bokehKernelWithSize:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLTexture BokehKernelWithSize (Vector2i size);

		/// <summary>Gets or sets the time, in seconds, for which the simulated shutter is open per frame.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shutterOpenInterval")]
		double ShutterOpenInterval { get; set; }

		/// <summary>Gets or sets the height, in mm, of the sensor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sensorVerticalAperture")]
		float SensorVerticalAperture { get; set; }

		/// <summary>Gets or sets the aspect ratio of a simulated camera sensor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sensorAspect")]
		float SensorAspect { get; set; }

		/// <summary>Gets or sets the scale factor by which to expand the central region of the region. The image is cropped to the original extent of the sensor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sensorEnlargement", ArgumentSemantic.Assign)]
		Vector2 SensorEnlargement {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the directed distance, in mm, by which to offset the sensor from the center line of the simulated optical system.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sensorShift", ArgumentSemantic.Assign)]
		Vector2 SensorShift {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets a vector whose signed components are added to the rendered image in order to simulate the effect of a flash.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("flash", ArgumentSemantic.Assign)]
		Vector3 Flash {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets a vector whose X and Y components clamp the low and high values of color components when calculating exposure levels for a simulated physical camera.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("exposureCompression", ArgumentSemantic.Assign)]
		Vector2 ExposureCompression {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets a vector whose components are multiplied by the color components of the rendered image prior to simulation of the physical lens.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("exposure", ArgumentSemantic.Assign)]
		Vector3 Exposure {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Static]
		[Export ("cameraWithSCNCamera:")]
		MDLCamera FromSceneCamera (SCNCamera sceneCamera);
	}

	/// <summary>Procedurally generates a checkerboard texture on demand, and caches the result.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLCheckerboardTexture_Class/index.html">Apple documentation for <c>MDLCheckerboardTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture))]
	[DisableDefaultCtor]
	interface MDLCheckerboardTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		// -(instancetype __nonnull)initWithDivisions:(float)divisions name:(NSString * __nullable)name dimensions:(vector_int2)dimensions channelCount:(int)channelCount channelEncoding:(MDLTextureChannelEncoding)channelEncoding color1:(CGColorRef __nonnull)color1 color2:(CGColorRef __nonnull)color2;
		[Export ("initWithDivisions:name:dimensions:channelCount:channelEncoding:color1:color2:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (float divisions, [NullAllowed] string name, Vector2i dimensions, int channelCount, MDLTextureChannelEncoding channelEncoding, CGColor color1, CGColor color2);

		[Export ("divisions")]
		float Divisions { get; set; }

		[NullAllowed]
		[Export ("color1", ArgumentSemantic.Assign)]
		CGColor Color1 { get; set; }

		[NullAllowed]
		[Export ("color2", ArgumentSemantic.Assign)]
		CGColor Color2 { get; set; }
	}

	/// <summary>Texture that contains a gradient between two colors.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLColorSwatchTexture_Class/index.html">Apple documentation for <c>MDLColorSwatchTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture))]
	[DisableDefaultCtor]
	interface MDLColorSwatchTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Export ("initWithColorTemperatureGradientFrom:toColorTemperature:name:textureDimensions:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (float colorTemperature1, float colorTemperature2, [NullAllowed] string name, Vector2i textureDimensions);

		[Export ("initWithColorGradientFrom:toColor:name:textureDimensions:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (CGColor color1, CGColor color2, [NullAllowed] string name, Vector2i textureDimensions);
	}


	/// <summary>Base class for light sources.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLLight_Class/index.html">Apple documentation for <c>MDLLight</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	interface MDLLight {
		[Export ("irradianceAtPoint:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		CGColor GetIrradiance (Vector3 point);

		[Export ("irradianceAtPoint:colorSpace:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		CGColor GetIrradiance (Vector3 point, CGColorSpace colorSpace);

		[Export ("lightType")]
		MDLLightType LightType { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("colorSpace")]
		// No documentation to confirm but this should be a constant (hence NSString).
		NSString ColorSpace { get; set; }

		[Static]
		[Export ("lightWithSCNLight:")]
		MDLLight FromSceneLight (SCNLight sceneLight);
	}

	/// <summary>A light source that defines the color and intensity of its light in all directions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLLightProbe_Class/index.html">Apple documentation for <c>MDLLightProbe</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLLight))]
	interface MDLLightProbe {
		[Export ("initWithReflectiveTexture:irradianceTexture:")]
		NativeHandle Constructor ([NullAllowed] MDLTexture reflectiveTexture, [NullAllowed] MDLTexture irradianceTexture);

		[Export ("generateSphericalHarmonicsFromIrradiance:")]
		void GenerateSphericalHarmonicsFromIrradiance (nuint sphericalHarmonicsLevel);

		[NullAllowed, Export ("reflectiveTexture", ArgumentSemantic.Retain)]
		MDLTexture ReflectiveTexture { get; }

		[NullAllowed, Export ("irradianceTexture", ArgumentSemantic.Retain)]
		MDLTexture IrradianceTexture { get; }

		[Export ("sphericalHarmonicsLevel")]
		nuint SphericalHarmonicsLevel { get; }

		[NullAllowed, Export ("sphericalHarmonicsCoefficients", ArgumentSemantic.Copy)]
		NSData SphericalHarmonicsCoefficients { get; }

		// inlined from MDLLightBaking (MDLLightProbe)
		// reason: static protocol members made very bad extensions methods

		[Static]
		[Export ("lightProbeWithTextureSize:forLocation:lightsToConsider:objectsToConsider:reflectiveCubemap:irradianceCubemap:")]
		[return: NullAllowed]
		MDLLightProbe Create (nint textureSize, MDLTransform transform, MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, [NullAllowed] MDLTexture reflectiveCubemap, [NullAllowed] MDLTexture irradianceCubemap);
	}

	/// <summary>Defines how a surface should appear when rendered.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMaterial_Class/index.html">Apple documentation for <c>MDLMaterial</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLMaterial : MDLNamed, INSFastEnumeration {
		[Export ("initWithName:scatteringFunction:")]
		NativeHandle Constructor (string name, MDLScatteringFunction scatteringFunction);

		[Export ("setProperty:")]
		void SetProperty (MDLMaterialProperty property);

		[Export ("removeProperty:")]
		void RemoveProperty (MDLMaterialProperty property);

		[Export ("propertyNamed:")]
		[return: NullAllowed]
		MDLMaterialProperty GetProperty (string name);

		[Export ("propertyWithSemantic:")]
		[return: NullAllowed]
		MDLMaterialProperty GetProperty (MDLMaterialSemantic semantic);

		[MacCatalyst (13, 1)]
		[Export ("propertiesWithSemantic:")]
		MDLMaterialProperty [] GetProperties (MDLMaterialSemantic semantic);

		[Export ("removeAllProperties")]
		void RemoveAllProperties ();

		[MacCatalyst (13, 1)]
		[Export ("resolveTexturesWithResolver:")]
		void ResolveTextures (IMDLAssetResolver resolver);

		[MacCatalyst (13, 1)]
		[Export ("loadTexturesUsingResolver:")]
		void LoadTextures (IMDLAssetResolver resolver);

		[Export ("scatteringFunction", ArgumentSemantic.Retain)]
		MDLScatteringFunction ScatteringFunction { get; }

		[NullAllowed, Export ("baseMaterial", ArgumentSemantic.Retain)]
		MDLMaterial BaseMaterial { get; set; }

		[Export ("objectAtIndexedSubscript:")]
		[Internal]
		[return: NullAllowed]
		MDLMaterialProperty ObjectAtIndexedSubscript (nuint idx);

		[Export ("objectForKeyedSubscript:")]
		[Internal]
		[return: NullAllowed]
		MDLMaterialProperty ObjectForKeyedSubscript (string name);

		[Export ("count")]
		nuint Count { get; }

		[MacCatalyst (13, 1)]
		[Export ("materialFace", ArgumentSemantic.Assign)]
		MDLMaterialFace MaterialFace { get; set; }

		[Static]
		[Export ("materialWithSCNMaterial:")]
		MDLMaterial FromSceneMaterial (SCNMaterial material);
	}

	/// <summary>Defines a specific parameter that controls the rendering of a material surface.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMaterialProperty_Class/index.html">Apple documentation for <c>MDLMaterialProperty</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLMaterialProperty : MDLNamed, NSCopying {
		[DesignatedInitializer]
		[Export ("initWithName:semantic:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic);

		[Export ("initWithName:semantic:float:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, float value);

		[Export ("initWithName:semantic:float2:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, Vector2 value);

		[Export ("initWithName:semantic:float3:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, Vector3 value);

		[Export ("initWithName:semantic:float4:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, Vector4 value);

		[Export ("initWithName:semantic:matrix4x4:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
#if !NET
		[Obsolete ("Use the '(string, MDLMaterialSemantic, MatrixFloat4x4)' overload instead.")]
#endif
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, Matrix4 value);


#if !NET
		[Sealed]
		[Export ("initWithName:semantic:matrix4x4:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, MatrixFloat4x4 value);
#endif

		[Export ("initWithName:semantic:URL:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, [NullAllowed] NSUrl url);

		[Export ("initWithName:semantic:string:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, [NullAllowed] string stringValue);

		[Export ("initWithName:semantic:textureSampler:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, [NullAllowed] MDLTextureSampler textureSampler);

		[Export ("initWithName:semantic:color:")]
		NativeHandle Constructor (string name, MDLMaterialSemantic semantic, CGColor color);

		[Export ("setProperties:")]
		void SetProperties (MDLMaterialProperty property);

		[Export ("semantic", ArgumentSemantic.Assign)]
		MDLMaterialSemantic Semantic { get; set; }

		[Export ("type", ArgumentSemantic.Assign)]
		MDLMaterialPropertyType Type { get; }

		[MacCatalyst (13, 1)]
		[Export ("setType:")]
		void SetType (MDLMaterialPropertyType type);

		[NullAllowed, Export ("stringValue")]
		string StringValue { get; set; }

		[NullAllowed, Export ("URLValue", ArgumentSemantic.Copy)]
		NSUrl UrlValue { get; set; }

		[NullAllowed, Export ("textureSamplerValue", ArgumentSemantic.Retain)]
		MDLTextureSampler TextureSamplerValue { get; set; }

		[NullAllowed]
		[Export ("color", ArgumentSemantic.Assign)]
		CGColor Color { get; set; }

		[Export ("floatValue")]
		float FloatValue { get; set; }

		[Export ("float2Value", ArgumentSemantic.Assign)]
		Vector2 Float2Value {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Export ("float3Value", ArgumentSemantic.Assign)]
		Vector3 Float3Value {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Export ("float4Value", ArgumentSemantic.Assign)]
		Vector4 Float4Value {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

#if !NET
		[Obsolete ("Use 'MatrixFloat4x4' instead.")]
#endif
		[Export ("matrix4x4", ArgumentSemantic.Assign)]
		Matrix4 Matrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

#if !NET
		[Sealed]
		[Export ("matrix4x4", ArgumentSemantic.Assign)]
		MatrixFloat4x4 MatrixFloat4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}
#endif

		[MacCatalyst (13, 1)]
		[Export ("luminance")]
		float Luminance { get; set; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ModelIO/MDLMaterialPropertyConnection">Apple documentation for <c>MDLMaterialPropertyConnection</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLMaterialPropertyConnection : MDLNamed {
		[Export ("initWithOutput:input:")]
		NativeHandle Constructor (MDLMaterialProperty output, MDLMaterialProperty input);

		[NullAllowed, Export ("output", ArgumentSemantic.Weak)]
		MDLMaterialProperty Output { get; }

		[NullAllowed, Export ("input", ArgumentSemantic.Weak)]
		MDLMaterialProperty Input { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ModelIO/MDLMaterialPropertyNode">Apple documentation for <c>MDLMaterialPropertyNode</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLMaterialPropertyNode : MDLNamed {
		[Export ("initWithInputs:outputs:evaluationFunction:")]
		NativeHandle Constructor (MDLMaterialProperty [] inputs, MDLMaterialProperty [] outputs, Action<MDLMaterialPropertyNode> function);

		[Export ("evaluationFunction", ArgumentSemantic.Copy)]
		Action<MDLMaterialPropertyNode> EvaluationFunction { get; set; }

		[Export ("inputs")]
		MDLMaterialProperty [] Inputs { get; }

		[Export ("outputs")]
		MDLMaterialProperty [] Outputs { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ModelIO/MDLMaterialPropertyGraph">Apple documentation for <c>MDLMaterialPropertyGraph</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLMaterialPropertyNode))]
	[DisableDefaultCtor]
	interface MDLMaterialPropertyGraph {
		[Export ("initWithNodes:connections:")]
		NativeHandle Constructor (MDLMaterialPropertyNode [] nodes, MDLMaterialPropertyConnection [] connections);

		[Export ("evaluate")]
		void Evaluate ();

		[Export ("nodes")]
		MDLMaterialPropertyNode [] Nodes { get; }

		[Export ("connections")]
		MDLMaterialPropertyConnection [] Connections { get; }
	}

	/// <summary>A collection of <see cref="T:ModelIO.MDLSubmesh" /> objects that, together, define the geometric and material properties for rendering a 3D object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMesh_Class/index.html">Apple documentation for <c>MDLMesh</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	interface MDLMesh {
		[MacCatalyst (13, 1)]
		[Export ("initWithBufferAllocator:")]
		NativeHandle Constructor ([NullAllowed] IMDLMeshBufferAllocator bufferAllocator);

		[Export ("initWithVertexBuffer:vertexCount:descriptor:submeshes:")]
		NativeHandle Constructor (IMDLMeshBuffer vertexBuffer, nuint vertexCount, MDLVertexDescriptor descriptor, MDLSubmesh [] submeshes);

		[Export ("initWithVertexBuffers:vertexCount:descriptor:submeshes:")]
		NativeHandle Constructor (IMDLMeshBuffer [] vertexBuffers, nuint vertexCount, MDLVertexDescriptor descriptor, MDLSubmesh [] submeshes);

		[Internal]
		[Export ("vertexAttributeDataForAttributeNamed:")]
		[return: NullAllowed]
		MDLVertexAttributeData GetVertexAttributeDataForAttribute (string attributeName);

		[MacCatalyst (13, 1)]
		[Export ("vertexAttributeDataForAttributeNamed:asFormat:")]
		[return: NullAllowed]
		MDLVertexAttributeData GetVertexAttributeData (string attributeName, MDLVertexFormat format);

		[Export ("boundingBox")]
		MDLAxisAlignedBoundingBox BoundingBox {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("vertexDescriptor", ArgumentSemantic.Copy)]
		MDLVertexDescriptor VertexDescriptor { get; set; }

		[Export ("vertexCount")]
		nuint VertexCount {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		[Export ("vertexBuffers", ArgumentSemantic.Retain)]
		IMDLMeshBuffer [] VertexBuffers {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		[NullAllowed]
		[Export ("submeshes", ArgumentSemantic.Copy)]
		NSMutableArray<MDLSubmesh> Submeshes {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		[MacCatalyst (13, 1)]
		[Export ("allocator", ArgumentSemantic.Retain)]
		IMDLMeshBufferAllocator Allocator { get; }

		// MDLMesh_Modifiers (category)

		[Export ("addAttributeWithName:format:")]
		void AddAttribute (string name, MDLVertexFormat format);

		[MacCatalyst (13, 1)]
		[Export ("addAttributeWithName:format:type:data:stride:")]
		void AddAttribute (string name, MDLVertexFormat format, string type, NSData data, nint stride);

		[MacCatalyst (13, 1)]
		[Export ("addAttributeWithName:format:type:data:stride:time:")]
		void AddAttribute (string name, MDLVertexFormat format, string type, NSData data, nint stride, double time);

		[Export ("addNormalsWithAttributeNamed:creaseThreshold:")]
		void AddNormals ([NullAllowed] string name, float creaseThreshold);

		[Export ("addTangentBasisForTextureCoordinateAttributeNamed:tangentAttributeNamed:bitangentAttributeNamed:")]
		void AddTangentBasis (string textureCoordinateAttributeName, string tangentAttributeName, [NullAllowed] string bitangentAttributeName);

		[Export ("addTangentBasisForTextureCoordinateAttributeNamed:normalAttributeNamed:tangentAttributeNamed:")]
		void AddTangentBasisWithNormals (string textureCoordinateAttributeName, string normalAttributeName, string tangentAttributeName);

		[MacCatalyst (13, 1)]
		[Export ("addOrthTanBasisForTextureCoordinateAttributeNamed:normalAttributeNamed:tangentAttributeNamed:")]
		void AddOrthTanBasis (string textureCoordinateAttributeName, string normalAttributeName, string tangentAttributeName);

		[MacCatalyst (13, 1)]
		[Export ("addUnwrappedTextureCoordinatesForAttributeNamed:")]
		void AddUnwrappedTextureCoordinates (string textureCoordinateAttributeName);

		[MacCatalyst (13, 1)]
		[Export ("flipTextureCoordinatesInAttributeNamed:")]
		void FlipTextureCoordinates (string inTextureCoordinateAttributeNamed);

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'NSError' overload.")]
		[Export ("makeVerticesUnique")]
		void MakeVerticesUnique ();

		[MacCatalyst (13, 1)]
		[Export ("makeVerticesUniqueAndReturnError:")]
		bool MakeVerticesUnique (out NSError error);

		[MacCatalyst (13, 1)]
		[Export ("replaceAttributeNamed:withData:")]
		void ReplaceAttribute (string name, MDLVertexAttributeData newData);

		[MacCatalyst (13, 1)]
		[Export ("updateAttributeNamed:withData:")]
		void UpdateAttribute (string name, MDLVertexAttributeData newData);

		[MacCatalyst (13, 1)]
		[Export ("removeAttributeNamed:")]
		void RemoveAttribute (string name);

		// MDLMesh_Generators (category)

		// Note: we turn these constructors into static constructors because we don't want to lose the shape name. Also, the signatures of these constructors differ so it would not be possible to use an enum to differentiate the shapes.

		[Internal]
		[Export ("initBoxWithExtent:segments:inwardNormals:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitBox (Vector3 extent, Vector3i segments, bool inwardNormals, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initSphereWithExtent:segments:inwardNormals:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitSphere (Vector3 extent, Vector2i segments, bool inwardNormals, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initHemisphereWithExtent:segments:inwardNormals:cap:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitHemisphere (Vector3 extent, Vector2i segments, bool inwardNormals, bool cap, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initCylinderWithExtent:segments:inwardNormals:topCap:bottomCap:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitCylinder (Vector3 extent, Vector2i segments, bool inwardNormals, bool topCap, bool bottomCap, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initCapsuleWithExtent:cylinderSegments:hemisphereSegments:inwardNormals:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitCapsule (Vector3 extent, Vector2i segments, int hemisphereSegments, bool inwardNormals, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initConeWithExtent:segments:inwardNormals:cap:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitCone (Vector3 extent, Vector2i segments, bool inwardNormals, bool cap, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initPlaneWithExtent:segments:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitPlane (Vector3 extent, Vector2i segments, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initIcosahedronWithExtent:inwardNormals:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitIcosahedron (Vector3 extent, bool inwardNormals, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Export ("initMeshBySubdividingMesh:submeshIndex:subdivisionLevels:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitMesh (MDLMesh mesh, int submeshIndex, uint subdivisionLevels, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Internal]
		[Static]
		[Export ("newBoxWithDimensions:segments:geometryType:inwardNormals:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh NewBoxWithDimensions (Vector3 dimensions, Vector3i segments, MDLGeometryType geometryType, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newPlaneWithDimensions:segments:geometryType:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh CreatePlane (Vector2 dimensions, Vector2i segments, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newEllipsoidWithRadii:radialSegments:verticalSegments:geometryType:inwardNormals:hemisphere:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh CreateEllipsoid (Vector3 radii, nuint radialSegments, nuint verticalSegments, MDLGeometryType geometryType, bool inwardNormals, bool hemisphere, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newCylinderWithHeight:radii:radialSegments:verticalSegments:geometryType:inwardNormals:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh CreateCylindroid (float height, Vector2 radii, nuint radialSegments, nuint verticalSegments, MDLGeometryType geometryType, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[MacCatalyst (13, 1)]
		[Export ("newCapsuleWithHeight:radii:radialSegments:verticalSegments:hemisphereSegments:geometryType:inwardNormals:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh CreateCapsule (float height, Vector2 radii, nuint radialSegments, nuint verticalSegments, nuint hemisphereSegments, MDLGeometryType geometryType, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newEllipticalConeWithHeight:radii:radialSegments:verticalSegments:geometryType:inwardNormals:allocator:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLMesh CreateEllipticalCone (float height, Vector2 radii, nuint radialSegments, nuint verticalSegments, MDLGeometryType geometryType, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newIcosahedronWithRadius:inwardNormals:allocator:")]
		MDLMesh CreateIcosahedron (float radius, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[MacCatalyst (13, 1)]
		[Export ("newIcosahedronWithRadius:inwardNormals:geometryType:allocator:")]
		MDLMesh CreateIcosahedron (float radius, bool inwardNormals, MDLGeometryType geometryType, [NullAllowed] IMDLMeshBufferAllocator allocator);

		[Static]
		[Export ("newSubdividedMesh:submeshIndex:subdivisionLevels:")]
		[return: NullAllowed]
		MDLMesh CreateSubdividedMesh (MDLMesh mesh, nuint submeshIndex, nuint subdivisionLevels);

		[Export ("generateAmbientOcclusionTextureWithSize:raysPerSample:attenuationFactor:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		bool GenerateAmbientOcclusionTexture (Vector2i textureSize, nint raysPerSample, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		[Export ("generateAmbientOcclusionTextureWithQuality:attenuationFactor:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		bool GenerateAmbientOcclusionTexture (float bakeQuality, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		[Export ("generateAmbientOcclusionVertexColorsWithRaysPerSample:attenuationFactor:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateAmbientOcclusionVertexColors (nint raysPerSample, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName);

		[Export ("generateAmbientOcclusionVertexColorsWithQuality:attenuationFactor:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateAmbientOcclusionVertexColors (float bakeQuality, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName);


		[Export ("generateLightMapTextureWithTextureSize:lightsToConsider:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		bool GenerateLightMapTexture (Vector2i textureSize, MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		[Export ("generateLightMapTextureWithQuality:lightsToConsider:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		bool GenerateLightMapTexture (float bakeQuality, MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		[Export ("generateLightMapVertexColorsWithLightsToConsider:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateLightMapVertexColors (MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName);

		[Static]
		[Export ("meshWithSCNGeometry:")]
		MDLMesh FromGeometry (SCNGeometry geometry);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("meshWithSCNGeometry:bufferAllocator:")]
		MDLMesh FromGeometry (SCNGeometry geometry, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.IMDLMeshBuffer" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.IMDLMeshBuffer" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.IMDLMeshBuffer" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:ModelIO.MDLMeshBuffer_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	/// <summary>Extension methods to the <see cref="T:ModelIO.IMDLMeshBuffer" /> interface to support all the methods from the <see cref="T:ModelIO.IMDLMeshBuffer" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="T:ModelIO.IMDLMeshBuffer" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="T:ModelIO.IMDLMeshBuffer" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	interface IMDLMeshBuffer { }
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLMeshBuffer : NSCopying {
		[Abstract]
		[Export ("fillData:offset:")]
		void FillData (NSData data, nuint offset);

		[Abstract]
		[Export ("map")]
		MDLMeshBufferMap Map { get; }

#if NET
		[Abstract]
#endif
		[Export ("length")]
		nuint Length { get; }

#if NET
		[Abstract]
#endif
		[Export ("allocator", ArgumentSemantic.Retain)]
		IMDLMeshBufferAllocator Allocator { get; }

#if NET
		[Abstract]
#endif
		[Export ("zone", ArgumentSemantic.Retain)]
		[NullAllowed]
		IMDLMeshBufferZone Zone { get; }

#if NET
		[Abstract]
#endif
		[Export ("type")]
		MDLMeshBufferType Type { get; }
	}

	/// <include file="../docs/api/ModelIO/IMDLMeshBufferAllocator.xml" path="/Documentation/Docs[@DocId='T:ModelIO.IMDLMeshBufferAllocator']/*" />
	interface IMDLMeshBufferAllocator { }
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLMeshBufferAllocator {
		[Abstract]
		[Export ("newZone:")]
		IMDLMeshBufferZone CreateZone (nuint capacity);

		[Abstract]
		[Export ("newZoneForBuffersWithSize:andType:")]
		IMDLMeshBufferZone CreateZone (NSNumber [] sizes, NSNumber [] types);

		[Abstract]
		[Export ("newBuffer:type:")]
		IMDLMeshBuffer CreateBuffer (nuint length, MDLMeshBufferType type);

		[Abstract]
		[Export ("newBufferWithData:type:")]
		IMDLMeshBuffer CreateBuffer (NSData data, MDLMeshBufferType type);

		[Abstract]
		[Export ("newBufferFromZone:length:type:")]
		[return: NullAllowed]
		IMDLMeshBuffer CreateBuffer ([NullAllowed] IMDLMeshBufferZone zone, nuint length, MDLMeshBufferType type);

		[Abstract]
		[Export ("newBufferFromZone:data:type:")]
		[return: NullAllowed]
		IMDLMeshBuffer CreateBuffer ([NullAllowed] IMDLMeshBufferZone zone, NSData data, MDLMeshBufferType type);
	}

	/// <summary>An implementation of <see cref="T:ModelIO.IMDLMeshBufferAllocator" /> that allocates mesh buffer data space within main memory, not GPU memory.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMeshBufferDataAllocator_Class/index.html">Apple documentation for <c>MDLMeshBufferDataAllocator</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLMeshBufferDataAllocator : MDLMeshBufferAllocator {

	}

	/// <summary>A default implementation of a <see cref="T:ModelIO.IMDLMeshBufferZone" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMeshBufferZoneDefault_Class/index.html">Apple documentation for <c>MDLMeshBufferZoneDefault</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLMeshBufferZoneDefault : MDLMeshBufferZone {
		// We get Capacity and Allocator from MDLMeshBufferZone
		// [Export ("capacity")]
		// nuint Capacity { get; }

		// [Export ("allocator", ArgumentSemantic.Retain)]
		// IMDLMeshBufferAllocator Allocator { get; }
	}

	/// <summary>Provides storage for vertex and index data for a <see cref="T:ModelIO.MDLMesh" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMeshBufferData_Class/index.html">Apple documentation for <c>MDLMeshBufferData</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLMeshBufferData : MDLMeshBuffer, NSCopying {
		[Export ("initWithType:length:")]
		NativeHandle Constructor (MDLMeshBufferType type, nuint length);

		[Export ("initWithType:data:")]
		NativeHandle Constructor (MDLMeshBufferType type, [NullAllowed] NSData data);

		[Export ("data", ArgumentSemantic.Retain)]
		NSData Data { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.IMDLMeshBufferZone" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.IMDLMeshBufferZone" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.IMDLMeshBufferZone" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:ModelIO.MDLMeshBufferZone_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	/// <summary>Extension methods to the <see cref="T:ModelIO.IMDLMeshBufferZone" /> interface to support all the methods from the <see cref="T:ModelIO.IMDLMeshBufferZone" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="T:ModelIO.IMDLMeshBufferZone" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="T:ModelIO.IMDLMeshBufferZone" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	interface IMDLMeshBufferZone { }
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLMeshBufferZone {
#if NET
		[Abstract]
#endif
		[Export ("capacity")]
		nuint Capacity { get; }

#if NET
		[Abstract]
#endif
		[Export ("allocator")]
		IMDLMeshBufferAllocator Allocator { get; }
	}

	/// <summary>Interface whose implementers define a <see cref="P:ModelIO.IMDLNamed.Name" /> property.</summary>
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLNamed {
		[Abstract]
		[Export ("name")]
		string Name { get; set; }
	}

	/// <summary>Class that generates a texture that consists of random color or direction noise.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLNoiseTexture_Class/index.html">Apple documentation for <c>MDLNoiseTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture))]
	[DisableDefaultCtor]
	interface MDLNoiseTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Internal]
		[Export ("initVectorNoiseWithSmoothness:name:textureDimensions:channelEncoding:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitVectorNoiseWithSmoothness (float smoothness, [NullAllowed] string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding);

		[Export ("initScalarNoiseWithSmoothness:name:textureDimensions:channelCount:channelEncoding:grayscale:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (float smoothness, [NullAllowed] string name, Vector2i textureDimensions, int channelCount, MDLTextureChannelEncoding channelEncoding, bool grayscale);

		[Internal]
		[MacCatalyst (13, 1)]
		[Export ("initCellularNoiseWithFrequency:name:textureDimensions:channelEncoding:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr InitCellularNoiseWithFrequency (float frequency, [NullAllowed] string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding);
	}

	/// <summary>Class that generates a texture that contains surface normal data.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLNormalMapTexture_Class/index.html">Apple documentation for <c>MDLNormalMapTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture))]
	[DisableDefaultCtor]
	interface MDLNormalMapTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Export ("initByGeneratingNormalMapWithTexture:name:smoothness:contrast:")]
		NativeHandle Constructor (MDLTexture sourceTexture, [NullAllowed] string name, float smoothness, float contrast);
	}

	/// <summary>Base class for objects in a <see cref="T:ModelIO.MDLAsset" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLObject_Class/index.html">Apple documentation for <c>MDLObject</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLObject : MDLNamed {
		[MacCatalyst (13, 1)]
		[Export ("components", ArgumentSemantic.Copy)]
		IMDLComponent [] Components { get; }

		[Export ("setComponent:forProtocol:")]
		void SetComponent (IMDLComponent component, Protocol protocol);

		[Wrap ("SetComponent (component, new Protocol (type!))")]
		void SetComponent (IMDLComponent component, Type type);

#if !NET
		[Obsolete ("Use 'GetComponent (Type type)'.")]
		[Export ("componentConformingToProtocol:")]
		[return: NullAllowed]
		IMDLComponent IsComponentConforming (Protocol protocol);
#endif

		[EditorBrowsable (EditorBrowsableState.Advanced)]
#if NET
		[Export ("componentConformingToProtocol:")]
#else
		[Wrap ("IsComponentConforming (protocol!)")]
#endif
		[return: NullAllowed]
		IMDLComponent GetComponent (Protocol protocol);

		[Wrap ("GetComponent (new Protocol (type!))")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Type type);

		[NullAllowed, Export ("parent", ArgumentSemantic.Weak)]
		MDLObject Parent { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("instance", ArgumentSemantic.Retain)]
		MDLObject Instance { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("path")]
		string Path { get; }

		[MacCatalyst (13, 1)]
		[Export ("objectAtPath:")]
		MDLObject GetObject (string path);

		[MacCatalyst (13, 1)]
		[Export ("enumerateChildObjectsOfClass:root:usingBlock:stopPointer:")]
		void EnumerateChildObjects (Class objectClass, MDLObject root, MDLObjectHandler handler, ref bool stop);

		[NullAllowed, Export ("transform", ArgumentSemantic.Retain)]
		IMDLTransformComponent Transform { get; set; }

		[Export ("children", ArgumentSemantic.Retain)]
		IMDLObjectContainerComponent Children { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("hidden")]
		bool Hidden { get; set; }

		[Export ("addChild:")]
		void AddChild (MDLObject child);

		[Export ("boundingBoxAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLAxisAlignedBoundingBox GetBoundingBox (double atTime);

		[Static]
		[Export ("objectWithSCNNode:")]
		MDLObject FromNode (SCNNode node);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("objectWithSCNNode:bufferAllocator:")]
		MDLObject FromNode (SCNNode node, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);
	}

	/// <summary>A default implementation of the abstract <see cref="T:ModelIO.IMDLObjectContainerComponent" /> class.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLObjectContainer_Class/index.html">Apple documentation for <c>MDLObjectContainer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLObjectContainer : MDLObjectContainerComponent {
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.IMDLObjectContainerComponent" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.IMDLObjectContainerComponent" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.IMDLObjectContainerComponent" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:ModelIO.MDLObjectContainerComponent_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	/// <summary>Extension methods to the <see cref="T:ModelIO.IMDLObjectContainerComponent" /> interface to support all the methods from the <see cref="T:ModelIO.IMDLObjectContainerComponent" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="T:ModelIO.IMDLObjectContainerComponent" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="T:ModelIO.IMDLObjectContainerComponent" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	interface IMDLObjectContainerComponent { }
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLObjectContainerComponent : MDLComponent, INSFastEnumeration {
		[Abstract]
		[Export ("addObject:")]
		void AddObject (MDLObject @object);

		[Abstract]
		[Export ("removeObject:")]
		void RemoveObject (MDLObject @object);

#if NET
		[Abstract]
#endif
		[MacCatalyst (13, 1)]
		[Export ("objectAtIndexedSubscript:")]
		MDLObject GetObject (nuint index);

#if NET
		[Abstract]
#endif
		[MacCatalyst (13, 1)]
		[Export ("count")]
		nuint Count { get; }

		[Abstract]
		[Export ("objects", ArgumentSemantic.Retain)]
		MDLObject [] Objects { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.IMDLComponent" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.IMDLComponent" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.IMDLComponent" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Model%20IOMDLComponent_%20Extensions&amp;scope=Xamarin" title="T:ModelIO.MDLComponent_Extensions">T:ModelIO.MDLComponent_Extensions</a></format> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMDLComponent { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLComponent {
	}

	/// <summary>Represents a light that is derived from photometric data.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLPhotometricLight_Class/index.html">Apple documentation for <c>MDLPhotometricLight</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLPhysicallyPlausibleLight))]
	interface MDLPhotometricLight {
		[Export ("initWithIESProfile:")]
		NativeHandle Constructor (NSUrl url);

		[Export ("generateSphericalHarmonicsFromLight:")]
		void GenerateSphericalHarmonics (nuint sphericalHarmonicsLevel);

		[Export ("generateCubemapFromLight:")]
		void GenerateCubemap (nuint textureSize);

		[MacCatalyst (13, 1)]
		[Export ("generateTexture:")]
		MDLTexture GenerateTexture (nuint textureSize);

		[NullAllowed, Export ("lightCubeMap", ArgumentSemantic.Retain)]
		MDLTexture LightCubeMap { get; }

		[Export ("sphericalHarmonicsLevel")]
		nuint SphericalHarmonicsLevel { get; }

		[NullAllowed, Export ("sphericalHarmonicsCoefficients", ArgumentSemantic.Copy)]
		NSData SphericalHarmonicsCoefficients { get; }
	}

	/// <summary>Represents a light that plausibly attenutates and can represent black-body incandescence.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLPhysicallyPlausibleLight_Class/index.html">Apple documentation for <c>MDLPhysicallyPlausibleLight</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLLight))]
	interface MDLPhysicallyPlausibleLight {
		[Export ("setColorByTemperature:")]
		void SetColor (float temperature);

		[NullAllowed, Export ("color", ArgumentSemantic.Assign)]
		CGColor Color { get; set; }

		[Export ("lumens")]
		float Lumens { get; set; }

		[Export ("innerConeAngle")]
		float InnerConeAngle { get; set; }

		[Export ("outerConeAngle")]
		float OuterConeAngle { get; set; }

		[Export ("attenuationStartDistance")]
		float AttenuationStartDistance { get; set; }

		[Export ("attenuationEndDistance")]
		float AttenuationEndDistance { get; set; }
	}

	/// <summary>Represents a bidirectional reflectance distribution function to use when rendering a <see cref="T:ModelIO.MDLMaterialTextureWrapMode" />.</summary>
	///     <remarks>The properties on this object, except for <see cref="P:ModelIO.MDLPhysicallyPlausibleScatteringFunction.Version" /> have valid values only in the range [0.0, 1.0].</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLPhysicallyPlausibleScatteringFunction_Class/index.html">Apple documentation for <c>MDLPhysicallyPlausibleScatteringFunction</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLScatteringFunction))]
	interface MDLPhysicallyPlausibleScatteringFunction {
		[Export ("version")]
		nint Version { get; }

		[Export ("subsurface", ArgumentSemantic.Retain)]
		MDLMaterialProperty Subsurface { get; }

		[Export ("metallic", ArgumentSemantic.Retain)]
		MDLMaterialProperty Metallic { get; }

		[Export ("specularAmount", ArgumentSemantic.Retain)]
		MDLMaterialProperty SpecularAmount { get; }

		[Export ("specularTint", ArgumentSemantic.Retain)]
		MDLMaterialProperty SpecularTint { get; }

		[Export ("roughness", ArgumentSemantic.Retain)]
		MDLMaterialProperty Roughness { get; }

		[Export ("anisotropic", ArgumentSemantic.Retain)]
		MDLMaterialProperty Anisotropic { get; }

		[Export ("anisotropicRotation", ArgumentSemantic.Retain)]
		MDLMaterialProperty AnisotropicRotation { get; }

		[Export ("sheen", ArgumentSemantic.Retain)]
		MDLMaterialProperty Sheen { get; }

		[Export ("sheenTint", ArgumentSemantic.Retain)]
		MDLMaterialProperty SheenTint { get; }

		[Export ("clearcoat", ArgumentSemantic.Retain)]
		MDLMaterialProperty Clearcoat { get; }

		[Export ("clearcoatGloss", ArgumentSemantic.Retain)]
		MDLMaterialProperty ClearcoatGloss { get; }
	}

	/// <summary>Represents a Bidirectional Reflectance Distribution Funcion (BRDF) that defines a material's response to lighting conditions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLScatteringFunction_Class/index.html">Apple documentation for <c>MDLScatteringFunction</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLScatteringFunction : MDLNamed {
		[Export ("baseColor", ArgumentSemantic.Retain)]
		MDLMaterialProperty BaseColor { get; }

		[Export ("emission", ArgumentSemantic.Retain)]
		MDLMaterialProperty Emission { get; }

		[Export ("specular", ArgumentSemantic.Retain)]
		MDLMaterialProperty Specular { get; }

		[Export ("materialIndexOfRefraction", ArgumentSemantic.Retain)]
		MDLMaterialProperty MaterialIndexOfRefraction { get; }

		[Export ("interfaceIndexOfRefraction", ArgumentSemantic.Retain)]
		MDLMaterialProperty InterfaceIndexOfRefraction { get; }

		[Export ("normal", ArgumentSemantic.Retain)]
		MDLMaterialProperty Normal { get; }

		[Export ("ambientOcclusion", ArgumentSemantic.Retain)]
		MDLMaterialProperty AmbientOcclusion { get; }

		[Export ("ambientOcclusionScale", ArgumentSemantic.Retain)]
		MDLMaterialProperty AmbientOcclusionScale { get; }
	}

	/// <summary>A texture that represents a realistic sunlit sky.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLSkyCubeTexture_Class/index.html">Apple documentation for <c>MDLSkyCubeTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture))]
	[DisableDefaultCtor]
	interface MDLSkyCubeTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Export ("initWithName:channelEncoding:textureDimensions:turbidity:sunElevation:upperAtmosphereScattering:groundAlbedo:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] string name, MDLTextureChannelEncoding channelEncoding, Vector2i textureDimensions, float turbidity, float sunElevation, float upperAtmosphereScattering, float groundAlbedo);

		[MacCatalyst (13, 1)]
		[Export ("initWithName:channelEncoding:textureDimensions:turbidity:sunElevation:sunAzimuth:upperAtmosphereScattering:groundAlbedo:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] string name, MDLTextureChannelEncoding channelEncoding, Vector2i textureDimensions, float turbidity, float sunElevation, float sunAzimuth, float upperAtmosphereScattering, float groundAlbedo);

		[Export ("updateTexture")]
		void UpdateTexture ();

		[Export ("turbidity")]
		float Turbidity { get; set; }

		[Export ("sunElevation")]
		float SunElevation { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("sunAzimuth")]
		float SunAzimuth { get; set; }

		[Export ("upperAtmosphereScattering")]
		float UpperAtmosphereScattering { get; set; }

		[Export ("groundAlbedo")]
		float GroundAlbedo { get; set; }

		[Export ("horizonElevation")]
		float HorizonElevation { get; set; }

		[NullAllowed]
		[Export ("groundColor", ArgumentSemantic.Assign)]
		CGColor GroundColor { get; set; }

		[Export ("gamma")]
		float Gamma { get; set; }

		[Export ("exposure")]
		float Exposure { get; set; }

		[Export ("brightness")]
		float Brightness { get; set; }

		[Export ("contrast")]
		float Contrast { get; set; }

		[Export ("saturation")]
		float Saturation { get; set; }

		[Export ("highDynamicRangeCompression", ArgumentSemantic.Assign)]
		Vector2 HighDynamicRangeCompression {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}
	}

	/// <summary>A camera that can be used to render stereoscopic views of a scene.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLStereoscopicCamera_Class/index.html">Apple documentation for <c>MDLStereoscopicCamera</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLCamera))]
	interface MDLStereoscopicCamera {
		[Export ("interPupillaryDistance")]
		float InterPupillaryDistance { get; set; }

		[Export ("leftVergence")]
		float LeftVergence { get; set; }

		[Export ("rightVergence")]
		float RightVergence { get; set; }

		[Export ("overlap")]
		float Overlap { get; set; }

#if !NET
		[Obsolete ("Use 'LeftViewMatrix4x4' instead.")]
#endif
		[Export ("leftViewMatrix")]
		Matrix4 LeftViewMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

#if !NET
		[Sealed]
		[Export ("leftViewMatrix")]
		MatrixFloat4x4 LeftViewMatrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif

#if !NET
		[Obsolete ("Use 'RightViewMatrix4x4' instead.")]
#endif
		[Export ("rightViewMatrix")]
		Matrix4 RightViewMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

#if !NET
		[Sealed]
		[Export ("rightViewMatrix")]
		MatrixFloat4x4 RightViewMatrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif

#if !NET
		[Obsolete ("Use 'LeftProjectionMatrix4x4' instead.")]
#endif
		[Export ("leftProjectionMatrix")]
		Matrix4 LeftProjectionMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

#if !NET
		[Sealed]
		[Export ("leftProjectionMatrix")]
		MatrixFloat4x4 LeftProjectionMatrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif

#if !NET
		[Obsolete ("Use 'RightProjectionMatrix4x4' instead.")]
#endif
		[Export ("rightProjectionMatrix")]
		Matrix4 RightProjectionMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

#if !NET
		[Sealed]
		[Export ("rightProjectionMatrix")]
		MatrixFloat4x4 RightProjectionMatrix4x4 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif
	}

	/// <summary>Indexes and identifies a portion of an MDLMesh, and contains material information about the indexed portion.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLSubmesh_Class/index.html">Apple documentation for <c>MDLSubmesh</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLSubmesh : MDLNamed {
		[Export ("initWithName:indexBuffer:indexCount:indexType:geometryType:material:")]
		NativeHandle Constructor (string name, IMDLMeshBuffer indexBuffer, nuint indexCount, MDLIndexBitDepth indexType, MDLGeometryType geometryType, [NullAllowed] MDLMaterial material);

		[Export ("initWithIndexBuffer:indexCount:indexType:geometryType:material:")]
		NativeHandle Constructor (IMDLMeshBuffer indexBuffer, nuint indexCount, MDLIndexBitDepth indexType, MDLGeometryType geometryType, [NullAllowed] MDLMaterial material);

		[Export ("initWithName:indexBuffer:indexCount:indexType:geometryType:material:topology:")]
		NativeHandle Constructor (string name, IMDLMeshBuffer indexBuffer, nuint indexCount, MDLIndexBitDepth indexType, MDLGeometryType geometryType, [NullAllowed] MDLMaterial material, [NullAllowed] MDLSubmeshTopology topology);

		[Export ("initWithMDLSubmesh:indexType:geometryType:")]
		NativeHandle Constructor (MDLSubmesh indexBuffer, MDLIndexBitDepth indexType, MDLGeometryType geometryType);

		[Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMDLMeshBuffer IndexBuffer { get; }

		[MacCatalyst (13, 1)]
		[Export ("indexBufferAsIndexType:")]
		IMDLMeshBuffer GetIndexBuffer (MDLIndexBitDepth indexType);

		[Export ("indexCount")]
		nuint IndexCount { get; }

		[Export ("indexType")]
		MDLIndexBitDepth IndexType { get; }

		[Export ("geometryType")]
		MDLGeometryType GeometryType { get; }

		[NullAllowed, Export ("material", ArgumentSemantic.Retain)]
		MDLMaterial Material { get; set; }

		[NullAllowed, Export ("topology", ArgumentSemantic.Retain)]
		MDLSubmeshTopology Topology {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		[Static]
		[Export ("submeshWithSCNGeometryElement:")]
		MDLSubmesh FromGeometryElement (SCNGeometryElement element);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("submeshWithSCNGeometryElement:bufferAllocator:")]
		MDLSubmesh FromGeometryElement (SCNGeometryElement element, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);
	}

	/// <summary>Represents texel data for surface rendering.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLTexture_Class/index.html">Apple documentation for <c>MDLTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface MDLTexture : MDLNamed {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

#if !NET
		[Static]
		[Obsolete ("Use 'CreateTexture' instead.")]
		[Wrap ("CreateTexture (name)")]
		[return: NullAllowed]
		MDLTexture FromBundle (string name);
#endif

		[Static]
		[Export ("textureNamed:")]
		[return: NullAllowed]
		MDLTexture CreateTexture (string name);

#if !NET
		[Static]
		[Obsolete ("Use 'CreateTexture' instead.")]
		[Wrap ("CreateTexture (name, bundleOrNil)")]
		[return: NullAllowed]
		MDLTexture FromBundle (string name, [NullAllowed] NSBundle bundleOrNil);
#endif

		[Static]
		[Export ("textureNamed:bundle:")]
		[return: NullAllowed]
		MDLTexture CreateTexture (string name, [NullAllowed] NSBundle bundleOrNil);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("textureNamed:assetResolver:")]
		[return: NullAllowed]
		MDLTexture CreateTexture (string name, IMDLAssetResolver resolver);

		[Static]
		[Export ("textureCubeWithImagesNamed:")]
		[return: NullAllowed]
		MDLTexture CreateTextureCube (string [] imageNames);

		[Static]
		[Export ("textureCubeWithImagesNamed:bundle:")]
		[return: NullAllowed]
		MDLTexture CreateTextureCube (string [] imageNames, [NullAllowed] NSBundle bundleOrNil);

		[Static]
		[Export ("irradianceTextureCubeWithTexture:name:dimensions:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLTexture CreateIrradianceTextureCube (MDLTexture texture, [NullAllowed] string name, Vector2i dimensions);

		[Static]
		[Export ("irradianceTextureCubeWithTexture:name:dimensions:roughness:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLTexture CreateIrradianceTextureCube (MDLTexture reflectiveTexture, [NullAllowed] string name, Vector2i dimensions, float roughness);

		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[DesignatedInitializer]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Export ("writeToURL:")]
		bool WriteToUrl (NSUrl url);

		[MacCatalyst (13, 1)]
		[Export ("writeToURL:level:")]
		bool WriteToUrl (NSUrl url, nuint level);

		[Export ("writeToURL:type:")]
		bool WriteToUrl (NSUrl url, string type);

		[MacCatalyst (13, 1)]
		[Export ("writeToURL:type:level:")]
		bool WriteToUrl (NSUrl nsurl, string type, nuint level);

		[Export ("imageFromTexture")]
		[return: NullAllowed]
		CGImage GetImageFromTexture ();

		[MacCatalyst (13, 1)]
		[Export ("imageFromTextureAtLevel:")]
		[return: NullAllowed]
		CGImage GetImageFromTexture (nuint level);

		[Export ("texelDataWithTopLeftOrigin")]
		[return: NullAllowed]
		NSData GetTexelDataWithTopLeftOrigin ();

		[Export ("texelDataWithBottomLeftOrigin")]
		[return: NullAllowed]
		NSData GetTexelDataWithBottomLeftOrigin ();

		[Export ("texelDataWithTopLeftOriginAtMipLevel:create:")]
		[return: NullAllowed]
		NSData GetTexelDataWithTopLeftOrigin (nint mipLevel, bool create);

		[Export ("texelDataWithBottomLeftOriginAtMipLevel:create:")]
		[return: NullAllowed]
		NSData GetTexelDataWithBottomLeftOrigin (nint mipLevel, bool create);

		[Export ("dimensions")]
		Vector2i Dimensions {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("rowStride")]
		nint RowStride { get; }

		[Export ("channelCount")]
		nuint ChannelCount { get; }

		[Export ("mipLevelCount")]
		nuint MipLevelCount { get; }

		[Export ("channelEncoding")]
		MDLTextureChannelEncoding ChannelEncoding { get; }

		[Export ("isCube")]
		bool IsCube { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("hasAlphaValues")]
		bool HasAlphaValues { get; set; }
	}

	/// <summary>Describes filtering modes for rendering by sampling a texture.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLTextureFilter_Class/index.html">Apple documentation for <c>MDLTextureFilter</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTextureFilter {
		[Export ("sWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode SWrapMode { get; set; }

		[Export ("tWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode TWrapMode { get; set; }

		[Export ("rWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode RWrapMode { get; set; }

		[Export ("minFilter", ArgumentSemantic.Assign)]
		MDLMaterialTextureFilterMode MinFilter { get; set; }

		[Export ("magFilter", ArgumentSemantic.Assign)]
		MDLMaterialTextureFilterMode MagFilter { get; set; }

		[Export ("mipFilter", ArgumentSemantic.Assign)]
		MDLMaterialMipMapFilterMode MipFilter { get; set; }
	}

	/// <summary>Represents a texture and sampling parameters to use when rendering the texture.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLTextureSampler_Class/index.html">Apple documentation for <c>MDLTextureSampler</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTextureSampler {
		[NullAllowed, Export ("texture", ArgumentSemantic.Retain)]
		MDLTexture Texture { get; set; }

		[NullAllowed, Export ("hardwareFilter", ArgumentSemantic.Retain)]
		MDLTextureFilter HardwareFilter { get; set; }

		[NullAllowed, Export ("transform", ArgumentSemantic.Retain)]
		MDLTransform Transform { get; set; }
	}

	/// <summary>Represents static and animated coordinate transformations.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLTransform_Class/index.html">Apple documentation for <c>MDLTransform</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface MDLTransform : MDLTransformComponent, NSCopying {

		[Export ("initWithTransformComponent:")]
		NativeHandle Constructor (IMDLTransformComponent component);

		[MacCatalyst (13, 1)]
		[Export ("initWithTransformComponent:resetsTransform:")]
		NativeHandle Constructor (IMDLTransformComponent component, bool resetsTransform);

#if !NET
		[Obsolete ("Use the '(MatrixFloat4x4)' overload instead.")]
#endif
		[Export ("initWithMatrix:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (Matrix4 matrix);

#if !NET
		[Sealed]
		[Export ("initWithMatrix:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (MatrixFloat4x4 matrix);
#endif

#if !NET
		[Obsolete ("Use the '(MatrixFloat4x4, bool)' overload instead.")]
#endif
		[MacCatalyst (13, 1)]
		[Export ("initWithMatrix:resetsTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (Matrix4 matrix, bool resetsTransform);

#if !NET
		[Sealed]
		[Export ("initWithMatrix:resetsTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (MatrixFloat4x4 matrix, bool resetsTransform);
#endif

		[Export ("setIdentity")]
		void SetIdentity ();

		[Export ("shearAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetShear (double atTime);

		[Export ("scaleAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetScale (double atTime);

		[Export ("translationAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetTranslation (double atTime);

		[Export ("rotationAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetRotation (double atTime);

#if !NET
		[Obsolete ("Use 'GetRotationMatrix4x4' instead.")]
#endif
		[Export ("rotationMatrixAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetRotationMatrix (double atTime);

#if !NET
		[Sealed]
		[Export ("rotationMatrixAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MatrixFloat4x4 GetRotationMatrix4x4 (double atTime);
#endif

		[Export ("setShear:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetShear (Vector3 scale, double time);

		[Export ("setScale:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetScale (Vector3 scale, double time);

		[Export ("setTranslation:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetTranslation (Vector3 translation, double time);

		[Export ("setRotation:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetRotation (Vector3 rotation, double time);

		[MacCatalyst (13, 1)]
		[Export ("setMatrix:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
#if !NET
		[Obsolete ("Use 'SetMatrix4x4' instead.")]
#endif
		void SetMatrix (Matrix4 matrix, double time);

#if !NET
		[Sealed]
		[Export ("setMatrix:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetMatrix4x4 (MatrixFloat4x4 matrix, double time);
#endif

		/// <summary>Gets or sets the shear of the transform.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shear", ArgumentSemantic.Assign)]
		Vector3 Shear {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the componentwise scale factors of the transform.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scale", ArgumentSemantic.Assign)]
		Vector3 Scale {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the translation of the transform.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("translation", ArgumentSemantic.Assign)]
		Vector3 Translation {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the rotation aspect of the transformation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotation", ArgumentSemantic.Assign)]
		Vector3 Rotation {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:ModelIO.IMDLTransformComponent" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:ModelIO.IMDLTransformComponent" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:ModelIO.IMDLTransformComponent" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:ModelIO.MDLTransformComponent_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMDLTransformComponent { }
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLTransformComponent : MDLComponent {
		[Abstract]
		[Export ("matrix", ArgumentSemantic.Assign)]
		Matrix4 Matrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[MacCatalyst (13, 1)]
#if NET
		[Abstract]
#endif
		[Export ("resetsTransform")]
		bool ResetsTransform { get; set; }

		[Abstract]
		[Export ("minimumTime")]
		double MinimumTime { get; }

		[Abstract]
		[Export ("maximumTime")]
		double MaximumTime { get; }

		// Added in iOS 10 SDK but it is supposed to be present in iOS 9.
		[MacCatalyst (13, 1)]
#if NET
		[Abstract]
#endif
		[Export ("keyTimes", ArgumentSemantic.Copy)]
		NSNumber [] KeyTimes { get; }

		[Export ("setLocalTransform:forTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetLocalTransform (Matrix4 transform, double time);

		[Export ("setLocalTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetLocalTransform (Matrix4 transform);

		[Export ("localTransformAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetLocalTransform (double atTime);

#if !NET
		[Obsolete ("Use 'CreateGlobalTransform4x4' instead.")]
#endif
		[Static]
		[Export ("globalTransformWithObject:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 CreateGlobalTransform (MDLObject obj, double atTime);
	}

	/// <summary>Encapsulates on-demand loading of a texture at a URL.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLURLTexture_Class/index.html">Apple documentation for <c>MDLURLTexture</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLTexture), Name = "MDLURLTexture")]
	[DisableDefaultCtor]
	interface MDLUrlTexture {
		[Export ("initWithData:topLeftOrigin:name:dimensions:rowStride:channelCount:channelEncoding:isCube:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor ([NullAllowed] NSData pixelData, bool topLeftOrigin, [NullAllowed] string name, Vector2i dimensions, nint rowStride, nuint channelCount, MDLTextureChannelEncoding channelEncoding, bool isCube);

		[Export ("initWithURL:name:")]
		NativeHandle Constructor (NSUrl url, [NullAllowed] string name);

		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }
	}

	/// <summary>Describes the physical layout and semantic content of vertex data in an index buffer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLVertexAttribute_Class/index.html">Apple documentation for <c>MDLVertexAttribute</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLVertexAttribute : NSCopying {
		[Export ("initWithName:format:offset:bufferIndex:")]
		NativeHandle Constructor (string name, MDLVertexFormat format, nuint offset, nuint bufferIndex);

		[Export ("name")]
		string Name { get; set; }

		[Export ("format", ArgumentSemantic.Assign)]
		MDLVertexFormat Format { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		nuint Offset { get; set; }

		[Export ("bufferIndex", ArgumentSemantic.Assign)]
		nuint BufferIndex { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("time")]
		double Time { get; set; }

		[Export ("initializationValue", ArgumentSemantic.Assign)]
		Vector4 InitializationValue {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}
	}

	/// <summary>Provides convenient access to vertex data for a specific <see cref="T:ModelIO.MDLMesh" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLVertexAttributeData_Class/index.html">Apple documentation for <c>MDLVertexAttributeData</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // apple headers: created by MDLMesh's vertexAttributeData selector
	interface MDLVertexAttributeData {
		[Export ("map", ArgumentSemantic.Retain)]
		MDLMeshBufferMap Map { get; set; }

		[Export ("dataStart", ArgumentSemantic.Assign)]
		IntPtr DataStart { get; set; }

		[Export ("stride", ArgumentSemantic.Assign)]
		nuint Stride { get; set; }

		[Export ("format", ArgumentSemantic.Assign)]
		MDLVertexFormat Format { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("bufferSize", ArgumentSemantic.Assign)]
		nuint BufferSize { get; set; }
	}

	/// <summary>Class that provides temporary access to mesh data in shared memory.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLMeshBufferMap_Class/index.html">Apple documentation for <c>MDLMeshBufferMap</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLMeshBufferMap {
		// FIXME: provide better API.
		[Export ("initWithBytes:deallocator:")]
		NativeHandle Constructor (IntPtr bytes, [NullAllowed] Action deallocator);

		[Export ("bytes")]
		IntPtr Bytes { get; }
	}

	/// <summary>Describes the format of vertices in a mesh buffer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLVertexDescriptor_Class/index.html">Apple documentation for <c>MDLVertexDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLVertexDescriptor : NSCopying {
		[Export ("initWithVertexDescriptor:")]
		NativeHandle Constructor (MDLVertexDescriptor vertexDescriptor);

		[Export ("attributeNamed:")]
		[return: NullAllowed]
		MDLVertexAttribute AttributeNamed (string name);

		[Export ("addOrReplaceAttribute:")]
		void AddOrReplaceAttribute (MDLVertexAttribute attribute);

		[MacCatalyst (13, 1)]
		[Export ("removeAttributeNamed:")]
		void RemoveAttribute (string name);

		/// <summary>Gets or sets the list of attributes that are described by this descriptor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributes", ArgumentSemantic.Retain)]
		NSMutableArray<MDLVertexAttribute> Attributes { get; set; }

		/// <summary>Gets or sets the vertex buffer layouts that describe the corresponding attributes that are contained in the <see cref="P:ModelIO.MDLVertexDescriptor.Attributes" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("layouts", ArgumentSemantic.Retain)]
		NSMutableArray<MDLVertexBufferLayout> Layouts { get; set; }

		[Export ("reset")]
		void Reset ();

		[Export ("setPackedStrides")]
		void SetPackedStrides ();

		[Export ("setPackedOffsets")]
		void SetPackedOffsets ();
	}

	/// <summary>Models a 3D object as a collection of volumetric elements, called voxels.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLVoxelArray_Class/index.html">Apple documentation for <c>MDLVoxelArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	[DisableDefaultCtor]
	interface MDLVoxelArray {

		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
#if NET
		[NoiOS]
#if XAMCORE_5_0
		[NoTV]
#else
		[Obsoleted (PlatformName.TvOS, 10, 0, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
		[NoMacCatalyst]
#endif
#else
		[Obsoleted (PlatformName.iOS, 10, 0, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
#endif
		[Export ("initWithAsset:divisions:interiorShells:exteriorShells:patchRadius:")]
		NativeHandle Constructor (MDLAsset asset, int divisions, int interiorShells, int exteriorShells, float patchRadius);

		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
#if NET
		[NoiOS]
#if XAMCORE_5_0
		[NoTV]
#else
		[Obsoleted (PlatformName.TvOS, 10, 0, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
		[NoMacCatalyst]
#endif
#else
		[Obsoleted (PlatformName.iOS, 10, 0, message: "Use 'new MDLVoxelArray (MDLAsset, int, float)'.")]
#endif
		[Export ("initWithAsset:divisions:interiorNBWidth:exteriorNBWidth:patchRadius:")]
		NativeHandle Constructor (MDLAsset asset, int divisions, float interiorNBWidth, float exteriorNBWidth, float patchRadius);

		[MacCatalyst (13, 1)]
		[Export ("initWithAsset:divisions:patchRadius:")]
		NativeHandle Constructor (MDLAsset asset, int divisions, float patchRadius);

		[Export ("initWithData:boundingBox:voxelExtent:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NativeHandle Constructor (NSData voxelData, MDLAxisAlignedBoundingBox boundingBox, float voxelExtent);

		[Export ("meshUsingAllocator:")]
		[return: NullAllowed]
		MDLMesh CreateMesh ([NullAllowed] IMDLMeshBufferAllocator allocator);

		[Export ("voxelExistsAtIndex:allowAnyX:allowAnyY:allowAnyZ:allowAnyShell:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		bool VoxelExists (Vector4i atIndex, bool allowAnyX, bool allowAnyY, bool allowAnyZ, bool allowAnyShell);

		[Export ("setVoxelAtIndex:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetVoxel (Vector4i index);

		[MacCatalyst (13, 1)]
		[Export ("setVoxelsForMesh:divisions:patchRadius:")]
		void SetVoxels (MDLMesh mesh, int divisions, float patchRadius);

		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
#if NET
		[NoiOS]
#if XAMCORE_5_0
		[NoTV]
#else
		[Obsoleted (PlatformName.TvOS, 10, 0, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
		[NoMacCatalyst]
#endif
#else
		[Obsoleted (PlatformName.iOS, 10, 0, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
#endif
		[Export ("setVoxelsForMesh:divisions:interiorShells:exteriorShells:patchRadius:")]
		void SetVoxels (MDLMesh mesh, int divisions, int interiorShells, int exteriorShells, float patchRadius);

		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
#if NET
		[NoiOS]
#if XAMCORE_5_0
		[NoTV]
#else
		[Obsoleted (PlatformName.TvOS, 10, 0, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
		[NoMacCatalyst]
#endif
#else
		[Obsoleted (PlatformName.iOS, 10, 0, message: "Use 'SetVoxels (MDLMesh, int, float)' instead.")]
#endif
		[Export ("setVoxelsForMesh:divisions:interiorNBWidth:exteriorNBWidth:patchRadius:")]
		void SetVoxels (MDLMesh mesh, int divisions, float interiorNBWidth, float exteriorNBWidth, float patchRadius);

#if !NET
		[Obsolete ("Use 'GetVoxels (MDLVoxelIndexExtent2)' instead.")]
#else
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
#endif
		[Export ("voxelsWithinExtent:")]
		[return: NullAllowed]
		NSData GetVoxels (MDLVoxelIndexExtent withinExtent);

#if !NET
		[Sealed]
		[Export ("voxelsWithinExtent:")]
		[return: NullAllowed]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NSData GetVoxels (MDLVoxelIndexExtent2 withinExtent);
#endif

		[Export ("voxelIndices")]
		[return: NullAllowed]
		NSData GetVoxelIndices ();

		[Export ("unionWithVoxels:")]
		void UnionWith (MDLVoxelArray voxels);

		[Export ("differenceWithVoxels:")]
		void DifferenceWith (MDLVoxelArray voxels);

		[Export ("intersectWithVoxels:")]
		void IntersectWith (MDLVoxelArray voxels);

		[Export ("indexOfSpatialLocation:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector4i GetIndex (Vector3 spatiallocation);

		[Export ("spatialLocationOfIndex:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetSpatialLocation (Vector4i index);

		[Export ("voxelBoundingBoxAtIndex:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLAxisAlignedBoundingBox GetVoxelBoundingBox (Vector4i index);

		[Export ("count")]
		nuint Count { get; }

#if !NET
		[Obsolete ("Use 'VoxelIndexExtent2' instead.")]
#endif
		[Export ("voxelIndexExtent")]
		MDLVoxelIndexExtent VoxelIndexExtent {
#if NET
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
#endif
			get;
		}

#if !NET
		[Export ("voxelIndexExtent")]
		[Sealed]
		MDLVoxelIndexExtent2 VoxelIndexExtent2 {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
#endif

		[Export ("boundingBox")]
		MDLAxisAlignedBoundingBox BoundingBox {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[MacCatalyst (13, 1)]
		[Export ("convertToSignedShellField")]
		void ConvertToSignedShellField ();

		[MacCatalyst (13, 1)]
		[Export ("isValidSignedShellField")]
		bool IsValidSignedShellField { get; }

		[MacCatalyst (13, 1)]
		[Export ("shellFieldInteriorThickness")]
		float ShellFieldInteriorThickness { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("shellFieldExteriorThickness")]
		float ShellFieldExteriorThickness { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("coarseMesh")]
		[return: NullAllowed]
		MDLMesh GetCoarseMesh ();

		[MacCatalyst (13, 1)]
		[Export ("coarseMeshUsingAllocator:")]
		[return: NullAllowed]
		MDLMesh GetCoarseMeshUsingAllocator ([NullAllowed] IMDLMeshBufferAllocator allocator);
	}

	/// <summary>Contains valid vertex attribute names.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface MDLVertexAttributes {
		[Field ("MDLVertexAttributeAnisotropy")]
		NSString Anisotropy { get; }

		[Field ("MDLVertexAttributeBinormal")]
		NSString Binormal { get; }

		[Field ("MDLVertexAttributeBitangent")]
		NSString Bitangent { get; }

		[Field ("MDLVertexAttributeColor")]
		NSString Color { get; }

		[Field ("MDLVertexAttributeEdgeCrease")]
		NSString EdgeCrease { get; }

		[Field ("MDLVertexAttributeJointIndices")]
		NSString JointIndices { get; }

		[Field ("MDLVertexAttributeJointWeights")]
		NSString JointWeights { get; }

		[Field ("MDLVertexAttributeNormal")]
		NSString Normal { get; }

		[Field ("MDLVertexAttributeOcclusionValue")]
		NSString OcclusionValue { get; }

		[Field ("MDLVertexAttributePosition")]
		NSString Position { get; }

		[Field ("MDLVertexAttributeShadingBasisU")]
		NSString ShadingBasisU { get; }

		[Field ("MDLVertexAttributeShadingBasisV")]
		NSString ShadingBasisV { get; }

		[Field ("MDLVertexAttributeSubdivisionStencil")]
		NSString SubdivisionStencil { get; }

		[Field ("MDLVertexAttributeTangent")]
		NSString Tangent { get; }

		[Field ("MDLVertexAttributeTextureCoordinate")]
		NSString TextureCoordinate { get; }
	}

	/// <summary>Describes the layout of vertices in a mesh buffer object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLVertexBufferLayout_Class/index.html">Apple documentation for <c>MDLVertexBufferLayout</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLVertexBufferLayout : NSCopying {
		[MacCatalyst (13, 1)]
		[Export ("initWithStride:")]
		NativeHandle Constructor (nuint stride);

		[Export ("stride", ArgumentSemantic.Assign)]
		nuint Stride { get; set; }
	}

	/// <summary>Describes the topology of the vertices that are indexed by a submesh's index buffers.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLSubmeshTopology_Class/index.html">Apple documentation for <c>MDLSubmeshTopology</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLSubmeshTopology {
		[MacCatalyst (13, 1)]
		[Export ("initWithSubmesh:")]
		NativeHandle Constructor (MDLSubmesh submesh);

		[NullAllowed, Export ("faceTopology", ArgumentSemantic.Retain)]
		IMDLMeshBuffer FaceTopology { get; set; }

		[Export ("faceCount", ArgumentSemantic.Assign)]
		nuint FaceCount { get; set; }

		[NullAllowed, Export ("vertexCreaseIndices", ArgumentSemantic.Retain)]
		IMDLMeshBuffer VertexCreaseIndices { get; set; }

		[NullAllowed, Export ("vertexCreases", ArgumentSemantic.Retain)]
		IMDLMeshBuffer VertexCreases { get; set; }

		[Export ("vertexCreaseCount", ArgumentSemantic.Assign)]
		nuint VertexCreaseCount { get; set; }

		[NullAllowed, Export ("edgeCreaseIndices", ArgumentSemantic.Retain)]
		IMDLMeshBuffer EdgeCreaseIndices { get; set; }

		[NullAllowed, Export ("edgeCreases", ArgumentSemantic.Retain)]
		IMDLMeshBuffer EdgeCreases { get; set; }

		[Export ("edgeCreaseCount", ArgumentSemantic.Assign)]
		nuint EdgeCreaseCount { get; set; }

		[NullAllowed, Export ("holes", ArgumentSemantic.Retain)]
		IMDLMeshBuffer Holes { get; set; }

		[Export ("holeCount", ArgumentSemantic.Assign)]
		nuint HoleCount { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLAnimatedValue : NSCopying {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isAnimated")]
		bool IsAnimated { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("precision")]
		MDLDataPrecision Precision { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timeSampleCount")]
		nuint TimeSampleCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minimumTime")]
		double MinimumTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumTime")]
		double MaximumTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interpolation", ArgumentSemantic.Assign)]
		MDLAnimatedValueInterpolation Interpolation { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("keyTimes")]
		NSNumber [] WeakKeyTimes { get; }

		[Export ("clear")]
		void Clear ();

		[Internal]
		[Export ("getTimes:maxCount:")]
		nuint _GetTimes (IntPtr timesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedScalarArray {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementCount")]
		nuint ElementCount { get; }

		[Export ("initWithElementCount:")]
		NativeHandle Constructor (nuint arrayElementCount);

		[Internal]
		[Export ("setFloatArray:count:atTime:")]
		void _SetFloatArray (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("setDoubleArray:count:atTime:")]
		void _SetDoubleArray (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("getFloatArray:maxCount:atTime:")]
		nuint _GetFloatArray (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("getDoubleArray:maxCount:atTime:")]
		nuint _GetDoubleArray (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("resetWithFloatArray:count:atTimes:count:")]
		void _ResetWithFloatArray (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("resetWithDoubleArray:count:atTimes:count:")]
		void _ResetWithDoubleArray (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("getFloatArray:maxCount:")]
		nuint _GetFloatArray (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDoubleArray:maxCount:")]
		nuint _GetDoubleArray (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedVector3Array {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementCount")]
		nuint ElementCount { get; }

		[Export ("initWithElementCount:")]
		NativeHandle Constructor (nuint arrayElementCount);

		[Internal]
		[Export ("setFloat3Array:count:atTime:")]
		void _SetFloat3Array (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("setDouble3Array:count:atTime:")]
		void _SetDouble3Array (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("getFloat3Array:maxCount:atTime:")]
		nuint _GetFloat3Array (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("getDouble3Array:maxCount:atTime:")]
		nuint _GetDouble3Array (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("resetWithFloat3Array:count:atTimes:count:")]
		void _ResetWithFloat3Array (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("resetWithDouble3Array:count:atTimes:count:")]
		void _ResetWithDouble3Array (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("getFloat3Array:maxCount:")]
		nuint _GetFloat3Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble3Array:maxCount:")]
		nuint _GetDouble3Array (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedQuaternionArray {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementCount")]
		nuint ElementCount { get; }

		[Export ("initWithElementCount:")]
		NativeHandle Constructor (nuint arrayElementCount);

		[Internal]
		[Export ("setFloatQuaternionArray:count:atTime:")]
		void _SetFloatQuaternionArray (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("setDoubleQuaternionArray:count:atTime:")]
		void _SetDoubleQuaternionArray (IntPtr array, nuint count, double time);

		[Internal]
		[Export ("getFloatQuaternionArray:maxCount:atTime:")]
		nuint _GetFloatQuaternionArray (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("getDoubleQuaternionArray:maxCount:atTime:")]
		nuint _GetDoubleQuaternionArray (IntPtr array, nuint maxCount, double time);

		[Internal]
		[Export ("resetWithFloatQuaternionArray:count:atTimes:count:")]
		void _ResetWithFloatQuaternionArray (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("resetWithDoubleQuaternionArray:count:atTimes:count:")]
		void _ResetWithDoubleQuaternionArray (IntPtr valuesArray, nuint valuesCount, IntPtr timesArray, nuint timesCount);

		[Internal]
		[Export ("getFloatQuaternionArray:maxCount:")]
		nuint _GetFloatQuaternionArray (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDoubleQuaternionArray:maxCount:")]
		nuint _GetDoubleQuaternionArray (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedScalar {

		[Export ("setFloat:atTime:")]
		void SetValue (float value, double time);

		[Export ("setDouble:atTime:")]
		void SetValue (double value, double time);

		[Export ("floatAtTime:")]
		float GetFloat (double time);

		[Export ("doubleAtTime:")]
		double GetDouble (double time);

		[Internal]
		[Export ("resetWithFloatArray:atTimes:count:")]
		void _ResetWithFloatArray (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("resetWithDoubleArray:atTimes:count:")]
		void _ResetWithDoubleArray (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("getFloatArray:maxCount:")]
		nuint _GetFloatArray (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDoubleArray:maxCount:")]
		nuint _GetDoubleArray (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedVector2 {

		[Export ("setFloat2:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (Vector2 value, double time);

		[Export ("setDouble2:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (Vector2d value, double time);

		[Export ("float2AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector2 GetVector2Value (double time);

		[Export ("double2AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector2d GetVector2dValue (double time);

		[Internal]
		[Export ("resetWithFloat2Array:atTimes:count:")]
		void _ResetWithFloat2Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("resetWithDouble2Array:atTimes:count:")]
		void _ResetWithDouble2Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("getFloat2Array:maxCount:")]
		nuint _GetFloat2Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble2Array:maxCount:")]
		nuint _GetDouble2Array (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedVector3 {

		[Export ("setFloat3:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (NVector3 value, double time);

		[Export ("setDouble3:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (NVector3d value, double time);

		[Export ("float3AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NVector3 GetNVector3Value (double time);

		[Export ("double3AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NVector3d GetNVector3dValue (double time);

		[Internal]
		[Export ("resetWithFloat3Array:atTimes:count:")]
		void _ResetWithFloat3Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("resetWithDouble3Array:atTimes:count:")]
		void _ResetWithDouble3Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("getFloat3Array:maxCount:")]
		nuint _GetFloat3Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble3Array:maxCount:")]
		nuint _GetDouble3Array (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedVector4 {

		[Export ("setFloat4:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (Vector4 value, double time);

		[Export ("setDouble4:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (Vector4d value, double time);

		[Export ("float4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector4 GetVector4Value (double time);

		[Export ("double4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector4d GetVector4dValue (double time);

		[Internal]
		[Export ("resetWithFloat4Array:atTimes:count:")]
		void _ResetWithFloat4Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("resetWithDouble4Array:atTimes:count:")]
		void _ResetWithDouble4Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("getFloat4Array:maxCount:")]
		nuint _GetFloat4Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble4Array:maxCount:")]
		nuint _GetDouble4Array (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedMatrix4x4 {

		[Export ("setFloat4x4:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (NMatrix4 value, double time);

		[Export ("setDouble4x4:atTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetValue (NMatrix4d value, double time);

		[Export ("float4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4 GetNMatrix4Value (double time);

		[Export ("double4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4d GetNMatrix4dValue (double time);

		[Internal]
		[Export ("resetWithFloat4x4Array:atTimes:count:")]
		void _ResetWithFloat4x4Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("resetWithDouble4x4Array:atTimes:count:")]
		void _ResetWithDouble4x4Array (IntPtr valuesArray, IntPtr timesArray, nuint count);

		[Internal]
		[Export ("getFloat4x4Array:maxCount:")]
		nuint _GetFloat4x4Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble4x4Array:maxCount:")]
		nuint _GetDouble4x4Array (IntPtr valuesArray, nuint maxCount);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	[DisableDefaultCtor]
	interface MDLSkeleton : NSCopying {

		[Export ("jointPaths")]
		string [] JointPaths { get; }

		[Export ("jointBindTransforms")]
		MDLMatrix4x4Array JointBindTransforms { get; }

		[MacCatalyst (13, 1)]
		[Export ("jointRestTransforms")]
		MDLMatrix4x4Array JointRestTransforms { get; }

		[Export ("initWithName:jointPaths:")]
		NativeHandle Constructor (string name, string [] jointPaths);
	}

	interface IMDLJointAnimation { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLJointAnimation {
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLObject))]
	[DisableDefaultCtor]
	interface MDLPackedJointAnimation : NSCopying, MDLJointAnimation {

		[Export ("jointPaths")]
		string [] JointPaths { get; }

		[Export ("translations")]
		MDLAnimatedVector3Array Translations { get; }

		[Export ("rotations")]
		MDLAnimatedQuaternionArray Rotations { get; }

		[Export ("scales")]
		MDLAnimatedVector3Array Scales { get; }

		[Export ("initWithName:jointPaths:")]
		NativeHandle Constructor (string name, string [] jointPaths);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLAnimationBindComponent : NSCopying, MDLComponent {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("skeleton", ArgumentSemantic.Retain)]
		MDLSkeleton Skeleton { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("jointAnimation", ArgumentSemantic.Retain)]
		IMDLJointAnimation JointAnimation { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("jointPaths", ArgumentSemantic.Retain)]
		string [] JointPaths { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("geometryBindTransform", ArgumentSemantic.Assign)]
		NMatrix4d GeometryBindTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}
	}

	interface IMDLAssetResolver { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLAssetResolver {

		[Abstract]
		[Export ("canResolveAssetNamed:")]
		bool CanResolveAsset (string name);

		[Abstract]
		[Export ("resolveAssetNamed:")]
		NSUrl ResolveAsset (string name);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLRelativeAssetResolver : MDLAssetResolver {

		[Export ("initWithAsset:")]
		NativeHandle Constructor (MDLAsset asset);

		[NullAllowed, Export ("asset", ArgumentSemantic.Weak)]
		MDLAsset Asset { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLPathAssetResolver : MDLAssetResolver {

		[Export ("initWithPath:")]
		NativeHandle Constructor (string path);

		[Export ("path")]
		string Path { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLBundleAssetResolver : MDLAssetResolver {

		[Export ("initWithBundle:")]
		NativeHandle Constructor (string path);

		[Export ("path")]
		string Path { get; set; }
	}

	interface IMDLTransformOp { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MDLTransformOp {

		[Abstract]
		[Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("float4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4 GetNMatrix4 (double atTime);

		[Abstract]
		[Export ("double4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4d GetNMatrix4d (double atTime);

		[Abstract]
		[Export ("IsInverseOp")]
		bool IsInverseOp { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateXOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateYOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateZOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformTranslateOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformScaleOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformMatrixOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedMatrix4x4 AnimatedValue { get; }
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformOrientOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		// [Export ("name")]
		// string Name { get; }

		[Export ("animatedValue")]
		MDLAnimatedQuaternion AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformStack : NSCopying, MDLTransformComponent {

		[Export ("addTranslateOp:inverse:")]
		MDLTransformTranslateOp AddTranslateOp (string animatedValueName, bool inverse);

		[Export ("addRotateXOp:inverse:")]
		MDLTransformRotateXOp AddRotateXOp (string animatedValueName, bool inverse);

		[Export ("addRotateYOp:inverse:")]
		MDLTransformRotateYOp AddRotateYOp (string animatedValueName, bool inverse);

		[Export ("addRotateZOp:inverse:")]
		MDLTransformRotateZOp AddRotateZOp (string animatedValueName, bool inverse);

		[Export ("addRotateOp:order:inverse:")]
		MDLTransformRotateOp AddRotateOp (string animatedValueName, MDLTransformOpRotationOrder order, bool inverse);

		[Export ("addScaleOp:inverse:")]
		MDLTransformScaleOp AddScaleOp (string animatedValueName, bool inverse);

		[Export ("addMatrixOp:inverse:")]
		MDLTransformMatrixOp AddMatrixOp (string animatedValueName, bool inverse);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("addOrientOp:inverse:")]
		MDLTransformOrientOp AddOrientOp (string animatedValueName, bool inverse);

		[Export ("animatedValueWithName:")]
		MDLAnimatedValue GetAnimatedValue (string name);

		[Export ("float4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4 GetNMatrix4 (double atTime);

		[Export ("double4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4d GetNMatrix4d (double atTime);

		[Export ("count")]
		nuint Count { get; }

		// Comes from MDLTransformComponent protocol
		//[Export ("keyTimes", ArgumentSemantic.Copy)]
		//NSNumber [] KeyTimes { get; }

		[Export ("transformOps", ArgumentSemantic.Copy)]
		IMDLTransformOp [] TransformOps { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLMatrix4x4Array : NSCopying {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementCount")]
		nuint ElementCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("precision")]
		MDLDataPrecision Precision { get; }

		[Export ("clear")]
		void Clear ();

		[Export ("initWithElementCount:")]
		NativeHandle Constructor (nuint arrayElementCount);

		[Internal]
		[Export ("setFloat4x4Array:count:")]
		void _SetFloat4x4Array (IntPtr valuesArray, nuint count);

		[Internal]
		[Export ("setDouble4x4Array:count:")]
		void _SetDouble4x4Array (IntPtr valuesArray, nuint count);

		[Internal]
		[Export ("getFloat4x4Array:maxCount:")]
		nuint _GetFloat4x4Array (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDouble4x4Array:maxCount:")]
		nuint _GetDouble4x4Array (IntPtr valuesArray, nuint maxCount);
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLAnimatedValue))]
	interface MDLAnimatedQuaternion {
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("setFloatQuaternion:atTime:")]
		void SetQuaternion (Quaternion value, double atTime);

		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("setDoubleQuaternion:atTime:")]
		void SetQuaternion (Quaterniond value, double atTime);

		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("floatQuaternionAtTime:")]
		Quaternion GetFloatQuaternion (double atTime);

		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		[Export ("doubleQuaternionAtTime:")]
		Quaterniond GetDoubleQuaternion (double atTime);

		[Internal]
		[Export ("resetWithFloatQuaternionArray:atTimes:count:")]
		void _ResetWithFloatQuaternionArray (IntPtr valuesArray, IntPtr timesArray, nuint times);

		[Internal]
		[Export ("resetWithDoubleQuaternionArray:atTimes:count:")]
		void _ResetWithDoubleQuaternionArray (IntPtr valuesArray, IntPtr timesArray, nuint times);

		[Internal]
		[Export ("getFloatQuaternionArray:maxCount:")]
		nuint _GetFloatQuaternionArray (IntPtr valuesArray, nuint maxCount);

		[Internal]
		[Export ("getDoubleQuaternionArray:maxCount:")]
		nuint _GetDoubleQuaternionArray (IntPtr valuesArray, nuint maxCount);
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MDLUtility {
		[Static]
		[Export ("convertToUSDZ:writeToURL:")]
		void ConvertToUsdz (NSUrl inputUrl, NSUrl outputUrl);
	}
}
