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
		/// <summary>To be added.</summary>
		Xyz = 1,
		/// <summary>To be added.</summary>
		Xzy,
		/// <summary>To be added.</summary>
		Yxz,
		/// <summary>To be added.</summary>
		Yzx,
		/// <summary>To be added.</summary>
		Zxy,
		/// <summary>To be added.</summary>
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
		/// <summary>Gets or sets the base linear dimension from which light shapes are calculated.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("areaRadius")]
		float AreaRadius { get; set; }

		/// <summary>Gets or sets the power of a super-elliptically-shaped light.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("superEllipticPower", ArgumentSemantic.Assign)]
		Vector2 SuperEllipticPower {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the aspect ratio of the light.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Exports the data that is contained in the asset to the file at the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("exportAssetToURL:error:")]
		bool ExportAssetToUrl (NSUrl url, out NSError error);

		/// <param name="atPath">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("objectAtPath:")]
		MDLObject GetObject (string atPath);

		/// <param name="extension">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the asset can import information from files with a format that corresponds to the specified extension. Otherwise, returns <see langword="false" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canImportFileExtension:")]
		bool CanImportFileExtension (string extension);

		/// <param name="extension">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the asset can export information to files with a format that corresponds to the specified extension. Otherwise, returns <see langword="false" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canExportFileExtension:")]
		bool CanExportFileExtension (string extension);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("components", ArgumentSemantic.Copy)]
		IMDLComponent [] Components { get; }

		/// <param name="component">To be added.</param>
		///         <param name="protocol">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("setComponent:forProtocol:")]
		void SetComponent (IMDLComponent component, Protocol protocol);

		/// <param name="component">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("SetComponent (component, new Protocol (type))")]
		void SetComponent (IMDLComponent component, Type type);

		/// <param name="protocol">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("componentConformingToProtocol:")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Protocol protocol);

		/// <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("GetComponent (new Protocol (type!))")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Type type);

		/// <param name="objectClass">To be added.</param>
		///         <summary>Gets the asset's child assets.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("childObjectsOfClass:")]
		MDLObject [] GetChildObjects (Class objectClass);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("loadTextures")]
		void LoadTextures ();

		/// <param name="atTime">To be added.</param>
		///         <summary>Gets the bounding box of the asset at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="object">To be added.</param>
		///         <summary>Adds the specified <see cref="T:ModelIO.MDLObject" />, which may be a <see cref="T:ModelIO.MDLCamera" />, <see cref="T:ModelIO.MDLMesh" />, or <see cref="T:ModelIO.MDLLight" />, to the end of the indexed list of objects for this <see cref="T:ModelIO.MDLAsset" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addObject:")]
		void AddObject (MDLObject @object);

		/// <param name="object">To be added.</param>
		///         <summary>Removes the specified <see cref="T:ModelIO.MDLObject" />.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="scene">To be added.</param>
		///         <summary>Creates and returns a new Model IO asset from the provided Scene Kit scene.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("assetWithSCNScene:")]
		MDLAsset FromScene (SCNScene scene);

		/// <param name="scene">To be added.</param>
		///         <param name="bufferAllocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates and returns a new Model IO asset from the provided Scene Kit scene, using the specified buffer allocator.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("assetWithSCNScene:bufferAllocator:")]
		MDLAsset FromScene (SCNScene scene, [NullAllowed] IMDLMeshBufferAllocator bufferAllocator);

		// MDLAsset_MDLLightBaking (category)

		/// <param name="density">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="dataSource">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("boundingBox", ArgumentSemantic.Assign)]
		MDLAxisAlignedBoundingBox BoundingBox { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <param name="boundingBox">To be added.</param>
		///         <param name="setNearAndFar">To be added.</param>
		///         <summary>Moves the camera to view <paramref name="boundingBox" /> looking parallel to the Z axis in a negative direction, and sets the near and far clipping planes to the bounding box if <paramref name="setNearAndFar" /> is <see langword="true" />.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="sceneCamera">To be added.</param>
		///         <summary>Creates a new MDLCamera from the specified Scene Kit camera.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the number of squares in each direction.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("divisions")]
		float Divisions { get; set; }

		/// <summary>Gets or sets the first color. This is the color of the top left corner of the checkerboard pattern</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("color1", ArgumentSemantic.Assign)]
		CGColor Color1 { get; set; }

		/// <summary>Gets or sets the second color.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the light type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lightType")]
		MDLLightType LightType { get; set; }

		/// <summary>Gets or sets the light's color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("colorSpace")]
		// No documentation to confirm but this should be a constant (hence NSString).
		NSString ColorSpace { get; set; }

		/// <param name="sceneLight">To be added.</param>
		///         <summary>Creates a new MDLLight instance from the specified <see cref="T:SceneKit.SCNLight" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets the cube map that represents the scene as seen from the viewpoint of the light probe.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("reflectiveTexture", ArgumentSemantic.Retain)]
		MDLTexture ReflectiveTexture { get; }

		/// <summary>Gets the irradiance map.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("irradianceTexture", ArgumentSemantic.Retain)]
		MDLTexture IrradianceTexture { get; }

		/// <summary>Gets a value from <c>0</c> to <c>3</c> that represents the number of harmonics that are represented by  <see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsCoefficients" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>There are 3 values in each coefficient. <see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsLevel" /> represent the highest index level in <see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsCoefficients" />. There are 2^(<see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsLevel" /> + 1) sets of 3-valued coefficients in the array.</remarks>
		[Export ("sphericalHarmonicsLevel")]
		nuint SphericalHarmonicsLevel { get; }

		/// <summary>Gets an array of 32-bit floating-point coefficients that are used to calculate the light probes illumination in space.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>There are 3 values in each coefficient. <see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsLevel" /> represent the highest index level in <see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsCoefficients" />. There are 2^(<see cref="P:ModelIO.MDLLightProbe.SphericalHarmonicsLevel" /> + 1) sets of 3-valued coefficients in the array.</remarks>
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

		/// <param name="property">To be added.</param>
		///         <summary>Updates or adds the specified property.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setProperty:")]
		void SetProperty (MDLMaterialProperty property);

		/// <param name="property">To be added.</param>
		///         <summary>Removes <paramref name="property" /> from the material.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeProperty:")]
		void RemoveProperty (MDLMaterialProperty property);

		/// <param name="name">To be added.</param>
		///         <summary>Returns the property with the specifed name, if it exists. Otherwise, returns <see langword="null" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("propertyNamed:")]
		[return: NullAllowed]
		MDLMaterialProperty GetProperty (string name);

		/// <param name="semantic">To be added.</param>
		///         <summary>Returns the property value for the specifed semantic, if it exists. Otherwise, returns <see langword="null" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("propertyWithSemantic:")]
		[return: NullAllowed]
		MDLMaterialProperty GetProperty (MDLMaterialSemantic semantic);

		/// <param name="semantic">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("propertiesWithSemantic:")]
		MDLMaterialProperty [] GetProperties (MDLMaterialSemantic semantic);

		/// <summary>Removes all properties from the material.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeAllProperties")]
		void RemoveAllProperties ();

		/// <param name="resolver">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("resolveTexturesWithResolver:")]
		void ResolveTextures (IMDLAssetResolver resolver);

		/// <param name="resolver">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("loadTexturesUsingResolver:")]
		void LoadTextures (IMDLAssetResolver resolver);

		/// <summary>Gets the scattering function for the material.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scatteringFunction", ArgumentSemantic.Retain)]
		MDLScatteringFunction ScatteringFunction { get; }

		/// <summary>Gets or sets a material from which to derive the properties of this amterial.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Uninitialized properties on this object are filled in with their correspoding values from the base material.</remarks>
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

		/// <summary>Gets the number of properties that belong to this material.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nuint Count { get; }

		/// <summary>Gets or sets a value that determines whether the material is a back, front, or double-sided face.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("materialFace", ArgumentSemantic.Assign)]
		MDLMaterialFace MaterialFace { get; set; }

		/// <param name="material">To be added.</param>
		///         <summary>Creates a new MDLMaterial from the specified SCNMaterial.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="property">To be added.</param>
		///         <summary>Sets the values of this MDLMaterialProperty to match those of <paramref name="property" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setProperties:")]
		void SetProperties (MDLMaterialProperty property);

		/// <summary>Gets or sets the semantic for the property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("semantic", ArgumentSemantic.Assign)]
		MDLMaterialSemantic Semantic { get; set; }

		/// <summary>Gets the property type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type", ArgumentSemantic.Assign)]
		MDLMaterialPropertyType Type { get; }

		/// <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setType:")]
		void SetType (MDLMaterialPropertyType type);

		/// <summary>Gets or sets the string value.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("stringValue")]
		string StringValue { get; set; }

		/// <summary>Gets or sets the URL value.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URLValue", ArgumentSemantic.Copy)]
		NSUrl UrlValue { get; set; }

		/// <summary>Gets or sets the texture sampler value.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("textureSamplerValue", ArgumentSemantic.Retain)]
		MDLTextureSampler TextureSamplerValue { get; set; }

		/// <summary>Gets or sets the color of the material property.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("color", ArgumentSemantic.Assign)]
		CGColor Color { get; set; }

		/// <summary>Gets or sets the floating-point value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("floatValue")]
		float FloatValue { get; set; }

		/// <summary>Gets or sets the 2-part floating-point value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("float2Value", ArgumentSemantic.Assign)]
		Vector2 Float2Value {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the 3-part floating-point value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("float3Value", ArgumentSemantic.Assign)]
		Vector3 Float3Value {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		/// <summary>Gets or sets the 4-part floating-point value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets or sets the 4x4 matrix of floating-point values.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the luminance of the material.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("output", ArgumentSemantic.Weak)]
		MDLMaterialProperty Output { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("evaluationFunction", ArgumentSemantic.Copy)]
		Action<MDLMaterialPropertyNode> EvaluationFunction { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputs")]
		MDLMaterialProperty [] Inputs { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("evaluate")]
		void Evaluate ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("nodes")]
		MDLMaterialPropertyNode [] Nodes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <param name="attributeName">To be added.</param>
		///         <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("vertexAttributeDataForAttributeNamed:asFormat:")]
		[return: NullAllowed]
		MDLVertexAttributeData GetVertexAttributeData (string attributeName, MDLVertexFormat format);

		/// <summary>Gets the smallest box that contains all of the vertices in the mesh.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("boundingBox")]
		MDLAxisAlignedBoundingBox BoundingBox {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		/// <summary>Gets or sets the description of the in-memory layout of vertices in a mesh.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexDescriptor", ArgumentSemantic.Copy)]
		MDLVertexDescriptor VertexDescriptor { get; set; }

		/// <summary>Gets the number of vertices in the entire mesh.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexCount")]
		nuint VertexCount {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		/// <summary>Gets the buffers that contain the vertex data for the mesh.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexBuffers", ArgumentSemantic.Retain)]
		IMDLMeshBuffer [] VertexBuffers {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		/// <summary>Gets the array of submeshes for this mesh.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("submeshes", ArgumentSemantic.Copy)]
		NSMutableArray<MDLSubmesh> Submeshes {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allocator", ArgumentSemantic.Retain)]
		IMDLMeshBufferAllocator Allocator { get; }

		// MDLMesh_Modifiers (category)

		/// <param name="name">To be added.</param>
		///         <param name="format">To be added.</param>
		///         <summary>Adds the <paramref name="format" /> attribute, indexed by the keyword <paramref name="name" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addAttributeWithName:format:")]
		void AddAttribute (string name, MDLVertexFormat format);

		[MacCatalyst (13, 1)]
		[Export ("addAttributeWithName:format:type:data:stride:")]
		void AddAttribute (string name, MDLVertexFormat format, string type, NSData data, nint stride);

		[MacCatalyst (13, 1)]
		[Export ("addAttributeWithName:format:type:data:stride:time:")]
		void AddAttribute (string name, MDLVertexFormat format, string type, NSData data, nint stride, double time);

		/// <param name="name">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="creaseThreshold">To be added.</param>
		///         <summary>Generates surface normals for a mesh, interpolating between adjacent faces when the dot product of their unit normals is greater than <paramref name="creaseThreshold" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addNormalsWithAttributeNamed:creaseThreshold:")]
		void AddNormals ([NullAllowed] string name, float creaseThreshold);

		/// <param name="textureCoordinateAttributeName">To be added.</param>
		///         <param name="tangentAttributeName">To be added.</param>
		///         <param name="bitangentAttributeName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Reads texture coordinates from the buffer that is specified by <paramref name="textureCoordinateAttributeName" />, calculates tangents and bitangents, and stores them in the specified buffers.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addTangentBasisForTextureCoordinateAttributeNamed:tangentAttributeNamed:bitangentAttributeNamed:")]
		void AddTangentBasis (string textureCoordinateAttributeName, string tangentAttributeName, [NullAllowed] string bitangentAttributeName);

		/// <param name="textureCoordinateAttributeName">To be added.</param>
		///         <param name="normalAttributeName">To be added.</param>
		///         <param name="tangentAttributeName">To be added.</param>
		///         <summary>Reads surface normals from the buffer that is specified by <paramref name="textureCoordinateAttributeName" />, calculates tangents and bitangents, and stores them in the specified buffers.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addTangentBasisForTextureCoordinateAttributeNamed:normalAttributeNamed:tangentAttributeNamed:")]
		void AddTangentBasisWithNormals (string textureCoordinateAttributeName, string normalAttributeName, string tangentAttributeName);

		/// <param name="textureCoordinateAttributeName">To be added.</param>
		///         <param name="normalAttributeName">To be added.</param>
		///         <param name="tangentAttributeName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("addOrthTanBasisForTextureCoordinateAttributeNamed:normalAttributeNamed:tangentAttributeNamed:")]
		void AddOrthTanBasis (string textureCoordinateAttributeName, string normalAttributeName, string tangentAttributeName);

		/// <param name="textureCoordinateAttributeName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("addUnwrappedTextureCoordinatesForAttributeNamed:")]
		void AddUnwrappedTextureCoordinates (string textureCoordinateAttributeName);

		/// <param name="inTextureCoordinateAttributeNamed">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("flipTextureCoordinatesInAttributeNamed:")]
		void FlipTextureCoordinates (string inTextureCoordinateAttributeNamed);

		/// <summary>Developers should not use this deprecated method. Developers should use the 'NSError' overload.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use the 'NSError' overload.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'NSError' overload.")]
		[Export ("makeVerticesUnique")]
		void MakeVerticesUnique ();

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("makeVerticesUniqueAndReturnError:")]
		bool MakeVerticesUnique (out NSError error);

		/// <param name="name">To be added.</param>
		///         <param name="newData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("replaceAttributeNamed:withData:")]
		void ReplaceAttribute (string name, MDLVertexAttributeData newData);

		/// <param name="name">To be added.</param>
		///         <param name="newData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("updateAttributeNamed:withData:")]
		void UpdateAttribute (string name, MDLVertexAttributeData newData);

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="radius">The radius of the icosahedron.</param>
		///         <param name="inwardNormals">Whether to generate inward-pointing normals.</param>
		///         <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a regular icosohedron with the specified radius.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("newIcosahedronWithRadius:inwardNormals:allocator:")]
		MDLMesh CreateIcosahedron (float radius, bool inwardNormals, [NullAllowed] IMDLMeshBufferAllocator allocator);

		/// <param name="radius">The radius of the icosahedron.</param>
		///         <param name="inwardNormals">Whether to generate inward-pointing normals.</param>
		///         <param name="geometryType">Whether to create triangles, quadrilaterals, or lines.</param>
		///         <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a regular icosahedron from the specified parameters.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="bakeQuality">To be added.</param>
		///         <param name="attenuationFactor">To be added.</param>
		///         <param name="objectsToConsider">To be added.</param>
		///         <param name="vertexAttributeName">To be added.</param>
		///         <param name="materialPropertyName">To be added.</param>
		///         <summary>Generates a texture that is used to simulate the occlusion of ambient light from recesses in the mesh.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("generateAmbientOcclusionTextureWithQuality:attenuationFactor:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		bool GenerateAmbientOcclusionTexture (float bakeQuality, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		[Export ("generateAmbientOcclusionVertexColorsWithRaysPerSample:attenuationFactor:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateAmbientOcclusionVertexColors (nint raysPerSample, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName);

		/// <param name="bakeQuality">To be added.</param>
		///         <param name="attenuationFactor">To be added.</param>
		///         <param name="objectsToConsider">To be added.</param>
		///         <param name="vertexAttributeName">To be added.</param>
		///         <summary>Generates vertex color data that is used to simulate the occlusion of ambient light from recesses in the mesh.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("generateAmbientOcclusionVertexColorsWithQuality:attenuationFactor:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateAmbientOcclusionVertexColors (float bakeQuality, float attenuationFactor, MDLObject [] objectsToConsider, string vertexAttributeName);


		[Export ("generateLightMapTextureWithTextureSize:lightsToConsider:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		bool GenerateLightMapTexture (Vector2i textureSize, MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		/// <param name="bakeQuality">To be added.</param>
		///         <param name="lightsToConsider">To be added.</param>
		///         <param name="objectsToConsider">To be added.</param>
		///         <param name="vertexAttributeName">To be added.</param>
		///         <param name="materialPropertyName">To be added.</param>
		///         <summary>Generates a map that represents the computed result of shading from the specified lights, obstructed by the specified objects.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("generateLightMapTextureWithQuality:lightsToConsider:objectsToConsider:vertexAttributeNamed:materialPropertyNamed:")]
		bool GenerateLightMapTexture (float bakeQuality, MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName, string materialPropertyName);

		/// <param name="lightsToConsider">To be added.</param>
		///         <param name="objectsToConsider">To be added.</param>
		///         <param name="vertexAttributeName">To be added.</param>
		///         <summary>Generates vertex color data that represent the computed result of shading from the specified lights, obstructed by the specified objects.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("generateLightMapVertexColorsWithLightsToConsider:objectsToConsider:vertexAttributeNamed:")]
		bool GenerateLightMapVertexColors (MDLLight [] lightsToConsider, MDLObject [] objectsToConsider, string vertexAttributeName);

		/// <param name="geometry">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("meshWithSCNGeometry:")]
		MDLMesh FromGeometry (SCNGeometry geometry);

		/// <param name="geometry">To be added.</param>
		///         <param name="bufferAllocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets an NSData object that contains the data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets the array of components that belong to this Model I/O object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("components", ArgumentSemantic.Copy)]
		IMDLComponent [] Components { get; }

		/// <param name="component">The component to associate with a protocol.</param>
		///         <param name="protocol">The protocol to associate with the component.</param>
		///         <summary>Associates <paramref name="component" /> with this MDLOBject for the specified protocol.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setComponent:forProtocol:")]
		void SetComponent (IMDLComponent component, Protocol protocol);

		/// <param name="component">The component to associate with a type.</param>
		///         <param name="type">The type to associate with the component.</param>
		///         <summary>Makes <paramref name="component" /> the object to provide for the specified <paramref name="type" />.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("SetComponent (component, new Protocol (type!))")]
		void SetComponent (IMDLComponent component, Type type);

#if !NET
		[Obsolete ("Use 'GetComponent (Type type)'.")]
		[Export ("componentConformingToProtocol:")]
		[return: NullAllowed]
		IMDLComponent IsComponentConforming (Protocol protocol);
#endif

		/// <param name="protocol">The protocol for which to get the component.</param>
		///         <summary>Gets this object's component that conforms to <paramref name="protocol" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
#if NET
		[Export ("componentConformingToProtocol:")]
#else
		[Wrap ("IsComponentConforming (protocol!)")]
#endif
		[return: NullAllowed]
		IMDLComponent GetComponent (Protocol protocol);

		/// <param name="type">The type to filter by.</param>
		///         <summary>Gets this object's component that matches the supplied <paramref name="type" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("GetComponent (new Protocol (type!))")]
		[return: NullAllowed]
		IMDLComponent GetComponent (Type type);

		/// <summary>Gets or sets this <see cref="T:ModelIO.MDLObject" /> object's parent, or <see langword="null" /> if this is a top-level object.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("parent", ArgumentSemantic.Weak)]
		MDLObject Parent { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("instance", ArgumentSemantic.Retain)]
		MDLObject Instance { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("path")]
		string Path { get; }

		/// <param name="path">To be added.</param>
		///         <summary>Returns the Model IO object at the specified path.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("objectAtPath:")]
		MDLObject GetObject (string path);

		/// <param name="objectClass">The class of child objects to enumerate.</param>
		///         <param name="root">The root object whose children will be enumerated.</param>
		///         <param name="handler">A handler to run on each child object.</param>
		///         <param name="stop">Developers set this value to <see langword="true" /> to stop enumeration.</param>
		///         <summary>Runs the provided <paramref name="handler" /> on each component in <paramref name="root" />'s object hierarchy that matches the specified <paramref name="objectClass" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("enumerateChildObjectsOfClass:root:usingBlock:stopPointer:")]
		void EnumerateChildObjects (Class objectClass, MDLObject root, MDLObjectHandler handler, ref bool stop);

		/// <summary>Gets or sets the possibly time-dependent transform that establishes this <see cref="T:ModelIO.MDLObject" /> object's coordinate system in relation to its parent's coordinate system.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("transform", ArgumentSemantic.Retain)]
		IMDLTransformComponent Transform { get; set; }

		/// <summary>Gets or sets the list of the objects of type <see cref="T:ModelIO.MDLObject" /> that belong to this <see cref="T:ModelIO.MDLObject" />.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("children", ArgumentSemantic.Retain)]
		IMDLObjectContainerComponent Children { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the Model IO object is hidden.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("hidden")]
		bool Hidden { get; set; }

		/// <param name="child">To be added.</param>
		///         <summary>Adds <paramref name="child" /> to this <see cref="T:ModelIO.MDLObject" /> object's <see cref="P:ModelIO.MDLObject.Children" /> property, creating <see cref="P:ModelIO.MDLObject.Children" />, if necessary.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addChild:")]
		void AddChild (MDLObject child);

		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the bounding box of the Model IO object at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("boundingBoxAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		MDLAxisAlignedBoundingBox GetBoundingBox (double atTime);

		/// <param name="node">To be added.</param>
		///         <summary>Creates a new MDLObject from the specified Scene Kit node.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("objectWithSCNNode:")]
		MDLObject FromNode (SCNNode node);

		/// <param name="node">To be added.</param>
		///         <param name="bufferAllocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates and returns a new Model IO object from the provided node.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets the light cube map that describes the directional intensity of the light.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Gets a cube map that represents the light's intensity in all directions, if present.</remarks>
		[NullAllowed, Export ("lightCubeMap", ArgumentSemantic.Retain)]
		MDLTexture LightCubeMap { get; }

		/// <summary>Gets a value from <c>0</c> to <c>3</c> that represents the number of harmonics that are represented by  <see cref="P:ModelIO.MDLPhotometricLight.SphericalHarmonicsCoefficients" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>There are 3 values in each coefficient. <see cref="P:ModelIO.MDLPhotometricLight.SphericalHarmonicsLevel" /> represent the highest index level in <see cref="P:ModelIO.MDLPhotometricLight.SphericalHarmonicsCoefficients" />. There are 2^(<see cref="P:ModelIO.MDLPhotometricLight.SphericalHarmonicsLevel" /> + 1) sets of 3-valued coefficients in the array.</remarks>
		[Export ("sphericalHarmonicsLevel")]
		nuint SphericalHarmonicsLevel { get; }

		/// <summary>Gets the spherical harmonics that control the light's directional intensity, if present.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("sphericalHarmonicsCoefficients", ArgumentSemantic.Copy)]
		NSData SphericalHarmonicsCoefficients { get; }
	}

	/// <summary>Represents a light that plausibly attenutates and can represent black-body incandescence.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLPhysicallyPlausibleLight_Class/index.html">Apple documentation for <c>MDLPhysicallyPlausibleLight</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLLight))]
	interface MDLPhysicallyPlausibleLight {
		/// <param name="temperature">To be added.</param>
		///         <summary>Sets the color of light by modeling black-body radiation at the specified temperature in °K.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setColorByTemperature:")]
		void SetColor (float temperature);

		/// <summary>Gets or sets the color of the light.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("color", ArgumentSemantic.Assign)]
		CGColor Color { get; set; }

		/// <summary>Gets or sets the light's intensity. The default is <c>1000.0</c> lumens.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lumens")]
		float Lumens { get; set; }

		/// <summary>Gets or sets the angle, in radians, of the cone of full illumination.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("innerConeAngle")]
		float InnerConeAngle { get; set; }

		/// <summary>Gets or sets the angle, in radians, of the cone of partial illumination, outside of which the illumination is zero.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outerConeAngle")]
		float OuterConeAngle { get; set; }

		/// <summary>Gets or sets the distance before which the light provides full illumination.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attenuationStartDistance")]
		float AttenuationStartDistance { get; set; }

		/// <summary>Gets or sets the distance after which the light provides no illumination.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attenuationEndDistance")]
		float AttenuationEndDistance { get; set; }
	}

	/// <summary>Represents a bidirectional reflectance distribution function to use when rendering a <see cref="T:ModelIO.MDLMaterialTextureWrapMode" />.</summary>
	///     <remarks>The properties on this object, except for <see cref="P:ModelIO.MDLPhysicallyPlausibleScatteringFunction.Version" /> have valid values only in the range [0.0, 1.0].</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLPhysicallyPlausibleScatteringFunction_Class/index.html">Apple documentation for <c>MDLPhysicallyPlausibleScatteringFunction</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MDLScatteringFunction))]
	interface MDLPhysicallyPlausibleScatteringFunction {
		/// <summary>Gets the scattering function version.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("version")]
		nint Version { get; }

		/// <summary>Gets the degree to which light is scattered immediately beneath the surface. The default is <c>0.0</c>, which indicates very strong scattering.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subsurface", ArgumentSemantic.Retain)]
		MDLMaterialProperty Subsurface { get; }

		/// <summary>Gets the degree to which the surface appears metallic. The default is <c>0.0</c>, which indicates a nonmetallic surface.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("metallic", ArgumentSemantic.Retain)]
		MDLMaterialProperty Metallic { get; }

		/// <summary>Gets the relative strength of specular highlights. The default is <c>0.0</c>, which indicates no specular highlights are visible.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("specularAmount", ArgumentSemantic.Retain)]
		MDLMaterialProperty SpecularAmount { get; }

		/// <summary>Gets the degree to which specular reflections are tinted by the surface color. The default is <c>0.0</c>, which indicates that specular reflections are identical to the incident light color.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("specularTint", ArgumentSemantic.Retain)]
		MDLMaterialProperty SpecularTint { get; }

		/// <summary>Gets the surface roughness. The default is <c>0.9</c>, which indicates a matte surface.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("roughness", ArgumentSemantic.Retain)]
		MDLMaterialProperty Roughness { get; }

		/// <summary>Gets the degree to which reflection varies as a function of the angle from the tangential vector. The default is <c>0.0</c>, which indicates isotropic reflection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("anisotropic", ArgumentSemantic.Retain)]
		MDLMaterialProperty Anisotropic { get; }

		/// <summary>Gets the angular deviation from which anisotropic effects are calculated, relative to the tangential basis. The default is <c>0.0</c>, which indicates no deviation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("anisotropicRotation", ArgumentSemantic.Retain)]
		MDLMaterialProperty AnisotropicRotation { get; }

		/// <summary>Gets or sets the degree of added low-angle reflection. The default is <c>0.05</c>, which indicates a highlight that is just detectable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sheen", ArgumentSemantic.Retain)]
		MDLMaterialProperty Sheen { get; }

		/// <summary>Gets the degree to which low-angle reflections are tinted by the surface color. The default is <c>0.0</c>, which indicates that low-angle reflections are identical to the incident light color.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sheenTint", ArgumentSemantic.Retain)]
		MDLMaterialProperty SheenTint { get; }

		/// <summary>Gets the intensity of secondary highlights. The default is <c>0.0</c>, which indicates no specular highlights.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("clearcoat", ArgumentSemantic.Retain)]
		MDLMaterialProperty Clearcoat { get; }

		/// <summary>Gets the sharpness of secondary highlights. The default is <c>0.0</c>, which indicates blurry specular highlights.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("clearcoatGloss", ArgumentSemantic.Retain)]
		MDLMaterialProperty ClearcoatGloss { get; }
	}

	/// <summary>Represents a Bidirectional Reflectance Distribution Funcion (BRDF) that defines a material's response to lighting conditions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLScatteringFunction_Class/index.html">Apple documentation for <c>MDLScatteringFunction</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLScatteringFunction : MDLNamed {
		/// <summary>Gets the base color of the material.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("baseColor", ArgumentSemantic.Retain)]
		MDLMaterialProperty BaseColor { get; }

		/// <summary>Gets the radiance color of the material, which provides a minimum illumination when the object is darkened by the rest of the lighting and shading model.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emission", ArgumentSemantic.Retain)]
		MDLMaterialProperty Emission { get; }

		/// <summary>Gets the intensity of specular highlighting.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("specular", ArgumentSemantic.Retain)]
		MDLMaterialProperty Specular { get; }

		/// <summary>Gets the n2 term in Schlick's approximation of the Fresnel factor in specular relection..</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("materialIndexOfRefraction", ArgumentSemantic.Retain)]
		MDLMaterialProperty MaterialIndexOfRefraction { get; }

		/// <summary>Gets the n1 term in Schlick's approximation of the Fresnel factor in specular relection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interfaceIndexOfRefraction", ArgumentSemantic.Retain)]
		MDLMaterialProperty InterfaceIndexOfRefraction { get; }

		/// <summary>Gets the deviation of surface normals in model coordinate space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("normal", ArgumentSemantic.Retain)]
		MDLMaterialProperty Normal { get; }

		/// <summary>Gets the degree to which ambient light is blocked by neighboring geometry.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ambientOcclusion", ArgumentSemantic.Retain)]
		MDLMaterialProperty AmbientOcclusion { get; }

		/// <summary>Gets a multiplier for the degree to which ambient light is blocked by neighboring geometry. The default value is <c>1.0</c>.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Regenerates the sky to match the current property values of this MDLSkyCubeTexture object.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("updateTexture")]
		void UpdateTexture ();

		/// <summary>Gets or sets the haziness of the simulated sky, on a scale from <c>0.0</c> to <c>1.0</c>.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("turbidity")]
		float Turbidity { get; set; }

		/// <summary>Gets or sets the angular position, from the zenith, of the Sun's position.</summary>
		///         <value>To be added.</value>
		///         <remarks>To control the horizontal position of the sun, app developers should rotate the scene within the sky cube.</remarks>
		[Export ("sunElevation")]
		float SunElevation { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("sunAzimuth")]
		float SunAzimuth { get; set; }

		/// <summary>Gets or sets the scattering present in the upper regions of the simulated sky, on a scale from <c>0.0</c> (similar to illumination at dawn or dusk) to <c>1.0</c> (similar to midday illumination).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("upperAtmosphereScattering")]
		float UpperAtmosphereScattering { get; set; }

		/// <summary>Gets or sets the relative amount of light, from <c>0.0</c> to <c>10.0</c>, that reflects off the ground. Lower values produce an effect similar to a clear sky.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("groundAlbedo")]
		float GroundAlbedo { get; set; }

		/// <summary>Gets or sets the angular height, in radians, below which the ground color affects the rendering outcome.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("horizonElevation")]
		float HorizonElevation { get; set; }

		/// <summary>Gets or sets the ground color.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("groundColor", ArgumentSemantic.Assign)]
		CGColor GroundColor { get; set; }

		/// <summary>Gets or sets a gamma correction factor to use when tone mapping.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("gamma")]
		float Gamma { get; set; }

		/// <summary>Gets or sets a the exposure to use when tone mapping.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("exposure")]
		float Exposure { get; set; }

		/// <summary>Gets or sets a brightness multiplier to use when tone mapping.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("brightness")]
		float Brightness { get; set; }

		/// <summary>Gets or sets a contrast multiplier to use when tone mapping.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contrast")]
		float Contrast { get; set; }

		/// <summary>Gets or sets the saturation to use when tone mapping.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("saturation")]
		float Saturation { get; set; }

		/// <summary>Gets or sets a vector, that filters and clamps color values for HDR compression.</summary>
		///         <value>To be added.</value>
		///         <remarks>Color values that are below the X value of this property's vector value are ignored. Thos above the Y value are clamped to the maximum display brightness.</remarks>
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
		/// <summary>Gets or sets the distance, in millimeters, between the centers of the camera viewpoints.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interPupillaryDistance")]
		float InterPupillaryDistance { get; set; }

		/// <summary>Gets or sets the angle in degrees at which the left viewpoint looks toward the centerline.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("leftVergence")]
		float LeftVergence { get; set; }

		/// <summary>Gets or sets the angle in degrees at which the right viewpoint looks toward the centerline.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rightVergence")]
		float RightVergence { get; set; }

		/// <summary>The fraction of the image width by which the left and right images overlap.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("overlap")]
		float Overlap { get; set; }

#if !NET
		[Obsolete ("Use 'LeftViewMatrix4x4' instead.")]
#endif
		/// <summary>Gets the view matrix for the left viewpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets the view matrix for the right viewpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets the projection matrix for the left viewpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets the projection matrix for the right viewpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets the buffer whose indices sequence the vertex data into interpretable geometry.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMDLMeshBuffer IndexBuffer { get; }

		/// <param name="indexType">To be added.</param>
		///         <summary>Returns the index buffer for the submesh with the specified bit depth.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("indexBufferAsIndexType:")]
		IMDLMeshBuffer GetIndexBuffer (MDLIndexBitDepth indexType);

		/// <summary>Gets the number of indices in the index buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexCount")]
		nuint IndexCount { get; }

		/// <summary>Gets the numeric data type of the values in the index buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexType")]
		MDLIndexBitDepth IndexType { get; }

		/// <summary>Gets or sets the geometry type of the submesh.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("geometryType")]
		MDLGeometryType GeometryType { get; }

		/// <summary>Gets or sets the material to use when rendering the submesh.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("material", ArgumentSemantic.Retain)]
		MDLMaterial Material { get; set; }

		/// <summary>Gets a value that controls how the vertices in the submesh define the geometry of the mesh.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Gets or sets an object that describes the submesh's topology.</remarks>
		[NullAllowed, Export ("topology", ArgumentSemantic.Retain)]
		MDLSubmeshTopology Topology {
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		/// <param name="element">To be added.</param>
		///         <summary>Creates a new MDLSubmesh object from the specified Scene Kit geometry element.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("submeshWithSCNGeometryElement:")]
		MDLSubmesh FromGeometryElement (SCNGeometryElement element);

		/// <param name="element">To be added.</param>
		///         <param name="bufferAllocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a new submesh from the provided Scene Kit element.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="name">To be added.</param>
		///         <param name="bundleOrNil">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("textureNamed:bundle:")]
		[return: NullAllowed]
		MDLTexture CreateTexture (string name, [NullAllowed] NSBundle bundleOrNil);

		/// <param name="name">To be added.</param>
		///         <param name="resolver">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("textureNamed:assetResolver:")]
		[return: NullAllowed]
		MDLTexture CreateTexture (string name, IMDLAssetResolver resolver);

		/// <param name="imageNames">To be added.</param>
		///         <summary>Creates a texture cube from the named images in the default application bundle.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("textureCubeWithImagesNamed:")]
		[return: NullAllowed]
		MDLTexture CreateTextureCube (string [] imageNames);

		/// <param name="imageNames">To be added.</param>
		///         <param name="bundleOrNil">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a texture cube from the named images in the specified application bundle.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="url">To be added.</param>
		///         <summary>Writes the texture data to the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("writeToURL:")]
		bool WriteToUrl (NSUrl url);

		[MacCatalyst (13, 1)]
		[Export ("writeToURL:level:")]
		bool WriteToUrl (NSUrl url, nuint level);

		/// <param name="url">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>Writes the texture data to the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("writeToURL:type:")]
		bool WriteToUrl (NSUrl url, string type);

		[MacCatalyst (13, 1)]
		[Export ("writeToURL:type:level:")]
		bool WriteToUrl (NSUrl nsurl, string type, nuint level);

		/// <summary>Returns an image created from the texture data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("imageFromTexture")]
		[return: NullAllowed]
		CGImage GetImageFromTexture ();

		[MacCatalyst (13, 1)]
		[Export ("imageFromTextureAtLevel:")]
		[return: NullAllowed]
		CGImage GetImageFromTexture (nuint level);

		/// <summary>Gets the texel data such that the first texel represents the top left corner of the texture.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("texelDataWithTopLeftOrigin")]
		[return: NullAllowed]
		NSData GetTexelDataWithTopLeftOrigin ();

		/// <summary>Gets the texel data such that the first texel represents the bottom left corner of the texture.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("texelDataWithBottomLeftOrigin")]
		[return: NullAllowed]
		NSData GetTexelDataWithBottomLeftOrigin ();

		[Export ("texelDataWithTopLeftOriginAtMipLevel:create:")]
		[return: NullAllowed]
		NSData GetTexelDataWithTopLeftOrigin (nint mipLevel, bool create);

		[Export ("texelDataWithBottomLeftOriginAtMipLevel:create:")]
		[return: NullAllowed]
		NSData GetTexelDataWithBottomLeftOrigin (nint mipLevel, bool create);

		/// <summary>Gets the width and height of the texture, in texels.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dimensions")]
		Vector2i Dimensions {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		/// <summary>Gets the row stride length in bytes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rowStride")]
		nint RowStride { get; }

		/// <summary>Gets the number of channels per texel.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("channelCount")]
		nuint ChannelCount { get; }

		/// <summary>Gets the maximum number of mipmap levels for the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mipLevelCount")]
		nuint MipLevelCount { get; }

		/// <summary>Gets a value that represents the encoding for texels in the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("channelEncoding")]
		MDLTextureChannelEncoding ChannelEncoding { get; }

		/// <summary>Gets or sets a value that determines whether the texture should be interpreted as a cube.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isCube")]
		bool IsCube { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>Gets or sets the wrapping mode for S-coordinates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode SWrapMode { get; set; }

		/// <summary>Gets or sets the wrapping mode for T-coordinates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("tWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode TWrapMode { get; set; }

		/// <summary>Gets or sets the wrapping mode for R-coordinates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rWrapMode", ArgumentSemantic.Assign)]
		MDLMaterialTextureWrapMode RWrapMode { get; set; }

		/// <summary>Gets or sets the filtering mode for rendering shrunken versions of the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minFilter", ArgumentSemantic.Assign)]
		MDLMaterialTextureFilterMode MinFilter { get; set; }

		/// <summary>Gets or sets the filtering mode for rendering magnified versions of the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("magFilter", ArgumentSemantic.Assign)]
		MDLMaterialTextureFilterMode MagFilter { get; set; }

		/// <summary>Gets or sets the filtering mode for rendering with mipmaps.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mipFilter", ArgumentSemantic.Assign)]
		MDLMaterialMipMapFilterMode MipFilter { get; set; }
	}

	/// <summary>Represents a texture and sampling parameters to use when rendering the texture.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ModelIO/Reference/MDLTextureSampler_Class/index.html">Apple documentation for <c>MDLTextureSampler</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTextureSampler {
		/// <summary>Gets or sets the texture to sample.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("texture", ArgumentSemantic.Retain)]
		MDLTexture Texture { get; set; }

		/// <summary>Gets or sets the filter to use to perform hardware filtering.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("hardwareFilter", ArgumentSemantic.Retain)]
		MDLTextureFilter HardwareFilter { get; set; }

		/// <summary>Gets or sets the pre-sample transform.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Gets or sets a transfomation to apply to coordinates before sampling the texture.</remarks>
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

		/// <summary>Makes the transform identical to the identity transform.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setIdentity")]
		void SetIdentity ();

		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the shear of the transform at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shearAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetShear (double atTime);

		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the scale of the transform at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("scaleAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetScale (double atTime);

		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the translation of the transform at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("translationAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetTranslation (double atTime);

		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the rotation of the transform at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("rotationAtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector3 GetRotation (double atTime);

#if !NET
		[Obsolete ("Use 'GetRotationMatrix4x4' instead.")]
#endif
		/// <param name="atTime">To be added.</param>
		///         <summary>Returns the rotation of the transform at the specified time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the URL for the MDLURLTexture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the name of the attribute. Application developers should use values from <see cref="T:ModelIO.MDLVertexAttributes" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; set; }

		/// <summary>Gets or sets a descriptor for the physical data format of the attribute.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("format", ArgumentSemantic.Assign)]
		MDLVertexFormat Format { get; set; }

		/// <summary>Gets the byte offset that locates this attribute relative to the start of a vertex datum in the vertex buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("offset", ArgumentSemantic.Assign)]
		nuint Offset { get; set; }

		/// <summary>Gets or sets the index into the array of vertex buffers that specifies the buffer that is described by this vertex attribute.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bufferIndex", ArgumentSemantic.Assign)]
		nuint BufferIndex { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("time")]
		double Time { get; set; }

		/// <summary>Gets or sets a default value for the vertex data for the attribute.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("map", ArgumentSemantic.Retain)]
		MDLMeshBufferMap Map { get; set; }

		/// <summary>Gets or sets the intra-vertex byte offset for the start of attribute data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dataStart", ArgumentSemantic.Assign)]
		IntPtr DataStart { get; set; }

		/// <summary>Gets or sets the stride, in bytes, between vertex attributes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stride", ArgumentSemantic.Assign)]
		nuint Stride { get; set; }

		/// <summary>Gets or sets the vertex attribute format.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("format", ArgumentSemantic.Assign)]
		MDLVertexFormat Format { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Returns a pointer to the mesh buffer data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <param name="name">To be added.</param>
		///         <summary>Returns the attribute that is identified by <paramref name="name" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("attributeNamed:")]
		[return: NullAllowed]
		MDLVertexAttribute AttributeNamed (string name);

		/// <param name="attribute">To be added.</param>
		///         <summary>Adds <paramref name="attribute" /> to the descriptor, or updates it if it is already present.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addOrReplaceAttribute:")]
		void AddOrReplaceAttribute (MDLVertexAttribute attribute);

		/// <param name="name">To be added.</param>
		///         <summary>Removes the attribute that has the specified name.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Clears all data from this vertex descriptor do that it contains a single default attribute and single default vertex buffer layout.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reset")]
		void Reset ();

		/// <summary>Sets the per-vertex stride to produce the most compact vertex buffer for the current vertex offsets.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setPackedStrides")]
		void SetPackedStrides ();

		/// <summary>Sets vertex attribute offsets to produce the most compact vertices.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="allocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Returns a mesh that encloses the voxels in the array.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("meshUsingAllocator:")]
		[return: NullAllowed]
		MDLMesh CreateMesh ([NullAllowed] IMDLMeshBufferAllocator allocator);

		[Export ("voxelExistsAtIndex:allowAnyX:allowAnyY:allowAnyZ:allowAnyShell:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		bool VoxelExists (Vector4i atIndex, bool allowAnyX, bool allowAnyY, bool allowAnyZ, bool allowAnyShell);

		[Export ("setVoxelAtIndex:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		void SetVoxel (Vector4i index);

		/// <param name="mesh">To be added.</param>
		///         <param name="divisions">To be added.</param>
		///         <param name="patchRadius">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setVoxelsForMesh:divisions:patchRadius:")]
		void SetVoxels (MDLMesh mesh, int divisions, float patchRadius);

		/// <param name="mesh">To be added.</param>
		///         <param name="divisions">To be added.</param>
		///         <param name="interiorShells">To be added.</param>
		///         <param name="exteriorShells">To be added.</param>
		///         <param name="patchRadius">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="mesh">To be added.</param>
		///         <param name="divisions">To be added.</param>
		///         <param name="interiorNBWidth">To be added.</param>
		///         <param name="exteriorNBWidth">To be added.</param>
		///         <param name="patchRadius">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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
		/// <param name="withinExtent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Returns a list of all the voxel indices as an array of 4-component integer arrays.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("voxelIndices")]
		[return: NullAllowed]
		NSData GetVoxelIndices ();

		/// <param name="voxels">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("unionWithVoxels:")]
		void UnionWith (MDLVoxelArray voxels);

		/// <param name="voxels">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("differenceWithVoxels:")]
		void DifferenceWith (MDLVoxelArray voxels);

		/// <param name="voxels">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets the number of voxels in the array.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nuint Count { get; }

#if !NET
		[Obsolete ("Use 'VoxelIndexExtent2' instead.")]
#endif
		/// <summary>Gets the allowable ranges for the four components of a voxel index.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets the smallest box that contains all the voxels.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("boundingBox")]
		MDLAxisAlignedBoundingBox BoundingBox {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("convertToSignedShellField")]
		void ConvertToSignedShellField ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isValidSignedShellField")]
		bool IsValidSignedShellField { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shellFieldInteriorThickness")]
		float ShellFieldInteriorThickness { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shellFieldExteriorThickness")]
		float ShellFieldExteriorThickness { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("coarseMesh")]
		[return: NullAllowed]
		MDLMesh GetCoarseMesh ();

		/// <param name="allocator">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("coarseMeshUsingAllocator:")]
		[return: NullAllowed]
		MDLMesh GetCoarseMeshUsingAllocator ([NullAllowed] IMDLMeshBufferAllocator allocator);
	}

	/// <summary>Contains valid vertex attribute names.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface MDLVertexAttributes {
		/// <summary>The vertex attribute data describe the degree to which the surface changes appearance with rotation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeAnisotropy")]
		NSString Anisotropy { get; }

		/// <summary>The vertex attribute data describe binormal vector data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeBinormal")]
		NSString Binormal { get; }

		/// <summary>The vertex attribute data describe bitangent vector data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeBitangent")]
		NSString Bitangent { get; }

		/// <summary>The vertex attribute data describe color data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeColor")]
		NSString Color { get; }

		/// <summary>The vertex attribute data describe edges that should not be modified during subdivision.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeEdgeCrease")]
		NSString EdgeCrease { get; }

		/// <summary>The vertex attribute data describe joint index data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeJointIndices")]
		NSString JointIndices { get; }

		/// <summary>The vertex attribute data describe joint weight data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeJointWeights")]
		NSString JointWeights { get; }

		/// <summary>The vertex attribute data describe a normal vector.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeNormal")]
		NSString Normal { get; }

		/// <summary>The vertex attribute data describe ambient occlusion data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeOcclusionValue")]
		NSString OcclusionValue { get; }

		/// <summary>The vertex attribute data describe position data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributePosition")]
		NSString Position { get; }

		/// <summary>The vertex attribute data describe the U component of a shading basis.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeShadingBasisU")]
		NSString ShadingBasisU { get; }

		/// <summary>The vertex attribute data describe the V component of a shading basis.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeShadingBasisV")]
		NSString ShadingBasisV { get; }

		/// <summary>The vertex attribute data describe data that control which neighboring vertices control subdivision.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeSubdivisionStencil")]
		NSString SubdivisionStencil { get; }

		/// <summary>The vertex attribute data describe tangent vectors.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MDLVertexAttributeTangent")]
		NSString Tangent { get; }

		/// <summary>The vertex attribute data describe texture coordinates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets the stride of the data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>Gets or sets a mesh buffer that contains the number of faces for each corresponding face in the mesh's index buffer.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("faceTopology", ArgumentSemantic.Retain)]
		IMDLMeshBuffer FaceTopology { get; set; }

		/// <summary>Gets or sets the number of faces in the submesh.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("faceCount", ArgumentSemantic.Assign)]
		nuint FaceCount { get; set; }

		/// <summary>Gets or sets the indices of vertices that are to be treated as creases.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("vertexCreaseIndices", ArgumentSemantic.Retain)]
		IMDLMeshBuffer VertexCreaseIndices { get; set; }

		/// <summary>Gets or sets the sparse mesh buffer that contains vertex smoothness data that are indexed by <see cref="P:ModelIO.MDLSubmeshTopology.VertexCreaseIndices" /> property</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("vertexCreases", ArgumentSemantic.Retain)]
		IMDLMeshBuffer VertexCreases { get; set; }

		/// <summary>Gets or sets the number of values in the <see cref="P:ModelIO.MDLSubmeshTopology.VertexCreaseIndices" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexCreaseCount", ArgumentSemantic.Assign)]
		nuint VertexCreaseCount { get; set; }

		/// <summary>Gets or sets the mesh buffer that contains the indices to the vertices that define the edges that crease during subdivision.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("edgeCreaseIndices", ArgumentSemantic.Retain)]
		IMDLMeshBuffer EdgeCreaseIndices { get; set; }

		/// <summary>Gets or sets the mesh buffer that contains edge smoothness data that correlates with the vertex pairs that are contained in <see cref="P:ModelIO.MDLSubmeshTopology.EdgeCreaseIndices" /> property.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("edgeCreases", ArgumentSemantic.Retain)]
		IMDLMeshBuffer EdgeCreases { get; set; }

		/// <summary>Gets the number of creases that are contained in the edge crease buffers.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("edgeCreaseCount", ArgumentSemantic.Assign)]
		nuint EdgeCreaseCount { get; set; }

		/// <summary>Gets or sets a mesh buffer that contains indices into the submesh's mesh buffer, thus indicating which faces are to be treated as holes when rendering.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("holes", ArgumentSemantic.Retain)]
		IMDLMeshBuffer Holes { get; set; }

		/// <summary>Gets or sets the number of holes in the hole buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <param name="value">To be added.</param>
		///         <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setFloat:atTime:")]
		void SetValue (float value, double time);

		/// <param name="value">To be added.</param>
		///         <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setDouble:atTime:")]
		void SetValue (double value, double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("floatAtTime:")]
		float GetFloat (double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("float2AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector2 GetVector2Value (double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("float3AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NVector3 GetNVector3Value (double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("float4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Vector4 GetVector4Value (double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("float4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4 GetNMatrix4Value (double time);

		/// <param name="time">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("jointPaths")]
		string [] JointPaths { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("jointBindTransforms")]
		MDLMatrix4x4Array JointBindTransforms { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("jointPaths")]
		string [] JointPaths { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("translations")]
		MDLAnimatedVector3Array Translations { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotations")]
		MDLAnimatedQuaternionArray Rotations { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("asset", ArgumentSemantic.Weak)]
		MDLAsset Asset { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLPathAssetResolver : MDLAssetResolver {

		[Export ("initWithPath:")]
		NativeHandle Constructor (string path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("path")]
		string Path { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MDLBundleAssetResolver : MDLAssetResolver {

		[Export ("initWithBundle:")]
		NativeHandle Constructor (string path);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateYOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateZOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedScalar AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformRotateOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformTranslateOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformScaleOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValue")]
		MDLAnimatedVector3 AnimatedValue { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MDLTransformMatrixOp : MDLTransformOp {

		// From MDLTransformOp Protocol
		//[Export ("name")]
		//string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addTranslateOp:inverse:")]
		MDLTransformTranslateOp AddTranslateOp (string animatedValueName, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addRotateXOp:inverse:")]
		MDLTransformRotateXOp AddRotateXOp (string animatedValueName, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addRotateYOp:inverse:")]
		MDLTransformRotateYOp AddRotateYOp (string animatedValueName, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addRotateZOp:inverse:")]
		MDLTransformRotateZOp AddRotateZOp (string animatedValueName, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="order">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addRotateOp:order:inverse:")]
		MDLTransformRotateOp AddRotateOp (string animatedValueName, MDLTransformOpRotationOrder order, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addScaleOp:inverse:")]
		MDLTransformScaleOp AddScaleOp (string animatedValueName, bool inverse);

		/// <param name="animatedValueName">To be added.</param>
		///         <param name="inverse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addMatrixOp:inverse:")]
		MDLTransformMatrixOp AddMatrixOp (string animatedValueName, bool inverse);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("addOrientOp:inverse:")]
		MDLTransformOrientOp AddOrientOp (string animatedValueName, bool inverse);

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("animatedValueWithName:")]
		MDLAnimatedValue GetAnimatedValue (string name);

		/// <param name="atTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("float4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4 GetNMatrix4 (double atTime);

		/// <param name="atTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("double4x4AtTime:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		NMatrix4d GetNMatrix4d (double atTime);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nuint Count { get; }

		// Comes from MDLTransformComponent protocol
		//[Export ("keyTimes", ArgumentSemantic.Copy)]
		//NSNumber [] KeyTimes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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
