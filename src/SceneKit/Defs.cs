//
// Defs.cs: Enumerations and core types
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2012-2014, 2016 Xamarin, Inc.
//

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace SceneKit {

	[MacCatalyst (13, 1)]
	[Native] // untyped enum (SceneKitTypes.h) but described as the value of `code` for `NSError` which is an NSInteger
	[ErrorDomain ("SCNErrorDomain")]
	public enum SCNErrorCode : long {
		/// <summary>To be added.</summary>
		ProgramCompilationError = 1,
	}

	/// <summary>Enumeration of 2D geometry primitives.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNGeometryPrimitiveType : long {
		/// <summary>To be added.</summary>
		Triangles,
		/// <summary>To be added.</summary>
		TriangleStrip,
		/// <summary>To be added.</summary>
		Line,
		/// <summary>To be added.</summary>
		Point,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Polygon,
	}

	/// <summary>Enumerates techniques for calculating transparency.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNTransparencyMode : long {
		/// <summary>Uses the alpha channel of colors. Alpha of 1.0 is opaque.</summary>
		AOne,
		/// <summary>Uses the luminance of colors. A luminance of 0.0 is opaque.</summary>
		RgbZero,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SingleLayer = 2,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		DualLayer = 3,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Default = AOne,
	}

	/// <summary>Enumeration determining which faces of a surface <see cref="T:SceneKit.SCNMaterial" /> are rendered.</summary>
	///     <remarks>
	///       <para>See <see cref="P:SceneKit.SCNMaterial.CullMode" />.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNCullMode : long {
		/// <summary>To be added.</summary>
		Back,
		/// <summary>To be added.</summary>
		Front,
	}

	/// <summary>Enumeration of texture filtering modes.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNFilterMode : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Nearest,
		/// <summary>To be added.</summary>
		Linear,
	}

	/// <summary>Enumerates texture-wrapping techniques.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNWrapMode : long {
		Clamp = 1,
		Repeat,
		// added in iOS 8, removed in 8.3 (mistake?) but added back in 9.0 betas
		ClampToBorder,
		Mirror,
	}

	/// <summary>Enumerates the states of an SCNSceneSource.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNSceneSourceStatus : long {
		/// <summary>An error occurred during loading.</summary>
		Error = -1,
		/// <summary>Deserializing and parsing has begun.</summary>
		Parsing = 4,
		/// <summary>The source's format is being validated.</summary>
		Validating = 8,
		/// <summary>Scene graph objects are being created.</summary>
		Processing = 12,
		/// <summary>Successfully completed.</summary>
		Complete = 16,
	}

	/// <summary>Enumerates the ways a <see cref="T:SceneKit.SCNShape" /> can be chamfered; on its front, back, or both sides.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNChamferMode : long {
		/// <summary>To be added.</summary>
		Both,
		/// <summary>To be added.</summary>
		Front,
		/// <summary>To be added.</summary>
		Back,
	}

	/// <summary>Enumeration of valid interpolation formulae for <see cref="P:SceneKit.SCNMorpher.CalculationMode" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNMorpherCalculationMode : long {
		Normalized,
		Additive,
	}

	/// <summary>Enumerates rate curves for use with <see cref="T:SceneKit.SCNAction" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNActionTimingMode : long {
		/// <summary>To be added.</summary>
		Linear,
		/// <summary>To be added.</summary>
		EaseIn,
		/// <summary>To be added.</summary>
		EaseOut,
		/// <summary>To be added.</summary>
		EaseInEaseOut,
	}

	/// <summary>Enumeration controlling when shadows are calculated.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNShadowMode : long {
		/// <summary>Shadows are calculated during lighting computations.</summary>
		Forward,
		/// <summary>Shadows are calculated during post-processing.</summary>
		Deferred,
		/// <summary>To be added.</summary>
		Modulated,
	}

	/// <summary>An enumeration specifying whether the <see cref="T:SceneKit.SCNPhysicsBody" /> is dynamic, kinematic, or static. Used with <see cref="M:SceneKit.SCNPhysicsBody.CreateBody(SceneKit.SCNPhysicsBodyType,SceneKit.SCNPhysicsShape)" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNPhysicsBodyType : long {
		/// <summary>To be added.</summary>
		Static,
		/// <summary>To be added.</summary>
		Dynamic,
		/// <summary>To be added.</summary>
		Kinematic,
	}

	/// <summary>Enumerates values specifying whether an <see cref="T:SceneKit.SCNPhysicsField" /> affects objects inside or outside its border.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNPhysicsFieldScope : long {
		/// <summary>To be added.</summary>
		InsideExtent,
		/// <summary>To be added.</summary>
		OutsideExtent,
	}

	/// <summary>Enumeration specifying the order in which particles emitted by a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Scene%20Kit%20SCNParticle%20Scene&amp;scope=Xamarin" title="T:SceneKit.SCNParticleScene">T:SceneKit.SCNParticleScene</a></format> are rendered.</summary>
	///     <remarks>
	///       <para>Along with <see cref="P:SceneKit.SCNParticleSystem.BlendMode" />, <see cref="P:SceneKit.SCNParticleSystem.SortingMode" /> affects the appearance of overlapping particles.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleSortingMode : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ProjectedDepth,
		/// <summary>To be added.</summary>
		Distance,
		/// <summary>To be added.</summary>
		OldestFirst,
		/// <summary>To be added.</summary>
		YoungestFirst,
	}

	/// <summary>Enumeration of the ways in which overlapping particles emitted by a <see cref="T:SceneKit.SCNParticleSystem" /> will be rendered.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleBlendMode : long {
		/// <summary>To be added.</summary>
		Additive,
		/// <summary>To be added.</summary>
		Subtract,
		/// <summary>To be added.</summary>
		Multiply,
		/// <summary>To be added.</summary>
		Screen,
		/// <summary>To be added.</summary>
		Alpha,
		/// <summary>To be added.</summary>
		Replace,
	}

	/// <summary>Enumerates the alignment of particles emitted by a <see cref="T:SceneKit.SCNParticleSystem" />. Used with <see cref="P:SceneKit.SCNParticleSystem.OrientationMode" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleOrientationMode : long {
		/// <summary>To be added.</summary>
		BillboardScreenAligned,
		/// <summary>To be added.</summary>
		BillboardViewAligned,
		/// <summary>To be added.</summary>
		Free,
		/// <summary>To be added.</summary>
		BillboardYAligned,
	}

	/// <summary>Enumeration of the initial location of particles emitted by a <see cref="T:SceneKit.SCNParticleSystem" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleBirthLocation : long {
		/// <summary>To be added.</summary>
		Surface,
		/// <summary>To be added.</summary>
		Volume,
		/// <summary>To be added.</summary>
		Vertex,
	}

	/// <summary>Enumerates the initial direction of particles emitted by a <see cref="T:SceneKit.SCNParticleSystem" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleBirthDirection : long {
		/// <summary>To be added.</summary>
		Constant,
		/// <summary>To be added.</summary>
		SurfaceNormal,
		/// <summary>To be added.</summary>
		Random,
	}

	/// <summary>Enumeration of playing modes for <see cref="T:SceneKit.SCNParticleSystem" />'s whose particles are rendered as a sequence of images.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleImageSequenceAnimationMode : long {
		/// <summary>To be added.</summary>
		Repeat,
		/// <summary>To be added.</summary>
		Clamp,
		/// <summary>To be added.</summary>
		AutoReverse,
	}

	/// <summary>Enumerates how a particle property is animated (over the lifetime of the particle, as the particle travels over a distance, or based on another property). Used with <see cref="P:SceneKit.SCNParticlePropertyController.InputMode" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleInputMode : long {
		/// <summary>To be added.</summary>
		OverLife,
		/// <summary>To be added.</summary>
		OverDistance,
		/// <summary>To be added.</summary>
		OverOtherProperty,
	}

	/// <summary>Enumerates moments when the modifier specified in <see cref="M:SceneKit.SCNParticleSystem.AddModifier(Foundation.NSString[],SceneKit.SCNParticleModifierStage,SceneKit.SCNParticleModifierHandler)" /> should be applied.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleModifierStage : long {
		/// <summary>To be added.</summary>
		PreDynamics,
		/// <summary>To be added.</summary>
		PostDynamics,
		/// <summary>To be added.</summary>
		PreCollision,
		/// <summary>To be added.</summary>
		PostCollision,
	}

	/// <summary>Enumeration of lifecycle events for particles emitted by a <see cref="T:SceneKit.SCNParticleSystem" />. Used with <see cref="M:SceneKit.SCNParticleSystem.HandleEvent(SceneKit.SCNParticleEvent,Foundation.NSString[],SceneKit.SCNParticleEventHandler)" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNParticleEvent : long {
		/// <summary>
		/// 	    This event is raised when a new particle is created.
		/// 	  </summary>
		Birth,
		/// <summary>
		/// 	    This event is raised when a new particle will be destroyed.
		/// 	  </summary>
		Death,
		/// <summary>
		/// 	    This event is raised when two particles collide with the scene geometry.
		/// 	  </summary>
		Collision,
	}

	// Utility enum
	/// <summary>Enumerates semantics for geometry data.</summary>
	public enum SCNGeometrySourceSemantics {
		/// <summary>To be added.</summary>
		Vertex,
		/// <summary>To be added.</summary>
		Normal,
		/// <summary>To be added.</summary>
		Color,
		/// <summary>To be added.</summary>
		Texcoord,
		/// <summary>To be added.</summary>
		VertexCrease,
		/// <summary>To be added.</summary>
		EdgeCrease,
		/// <summary>To be added.</summary>
		BoneWeights,
		/// <summary>To be added.</summary>
		BoneIndices,
	}

	// Utility enum
	/// <summary>Enumerates animation import policies.</summary>
	public enum SCNAnimationImportPolicy {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Play,
		/// <summary>To be added.</summary>
		PlayRepeatedly,
		/// <summary>To be added.</summary>
		DoNotPlay,
		/// <summary>To be added.</summary>
		PlayUsingSceneTimeBase,
	}

	// Utility enum
	/// <summary>Enumerates values that control which physics search results are returned.</summary>
	public enum SCNPhysicsSearchMode {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>Return the first matching result.</summary>
		Any,
		/// <summary>Return the closest matching result.</summary>
		Closest,
		/// <summary>Return all results that match the search criteria.</summary>
		All,
	}

	/// <summary>Enumerates values that control antialiasing behavior.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNAntialiasingMode : ulong {
		/// <summary>Do not antialias.</summary>
		None,
		/// <summary>Use two samples for each pixel.</summary>
		Multisampling2X,
		/// <summary>Use four samples for each pixel..</summary>
		Multisampling4X,
#if MONOMAC || __MACCATALYST__
		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Multisampling8X,
		/// <summary>To be added.</summary>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Multisampling16X,
#endif
	}

	/// <summary>Defaults for the collision properties of a <see cref="T:SceneKit.SCNPhysicsBody" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNPhysicsCollisionCategory : ulong {
		/// <summary>A category that does not collide.</summary>
		None = 0,
		/// <summary>The category for kinematic and dynamic bodies.</summary>
		Default = 1 << 0,
		/// <summary>The category for static bodies.</summary>
		Static = 1 << 1,
		/// <summary>A category that indicates that the physics body collides with all bodies that have a nonzero bitmask.</summary>
		All = UInt64.MaxValue,
	}

	/// <summary>Enumeration of axes' locks available to nodes constrained by <see cref="T:SceneKit.SCNBillboardConstraint" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNBillboardAxis : ulong {
		/// <summary>Allow the billboard to rotate about the X axis.</summary>
		X = 1 << 0,
		/// <summary>Allow the billboard to rotate about the Y axis.</summary>
		Y = 1 << 1,
		/// <summary>Allow the billboard to rotate about the Z axis.</summary>
		Z = 1 << 2,
		/// <summary>Align the billboard with the view.</summary>
		All = (X | Y | Z),
	}

	/// <summary>Enumerates possible loading policies for <see cref="T:SceneKit.SCNReferenceNode" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNReferenceLoadingPolicy : long {
		/// <summary>The scene is loaded as soon as the <see cref="P:SceneKit.SCNReferenceNode.ReferenceUrl" /> is initialized.</summary>
		Immediate = 0,
		/// <summary>The scene is loaded only when the <see cref="T:SceneKit.SCNReferenceNode" /> is first about to be displayed.</summary>
		OnDemand = 1,
	}

	/// <summary>Enumeration of the ways SceneKit can blend colors from a material with colors that already exist in the render target.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNBlendMode : long {
		/// <summary>Indicates that colors will be blended by multiplying the source and destination values by their corresponding alpha values</summary>
		Alpha = 0,
		/// <summary>Indicates that colors will be blended by adding their values.</summary>
		Add = 1,
		/// <summary>Indicates that colors will be blended by subtracting the source from the destination.</summary>
		Subtract = 2,
		/// <summary>Indicates that colors will be blended by multiplying their corresponding components.</summary>
		Multiply = 3,
		/// <summary>Indicates that colors will be blended by multiplying their corresponding inverses.</summary>
		Screen = 4,
		/// <summary>Indicates that colors will be blended by replacing the destination with the source and ignoring the alpha channel.</summary>
		Replace = 5,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Max = 6,
	}

	/// <summary>Enumerates debug overlay options.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum SCNDebugOptions : ulong {
		/// <summary>Indicates that no debugging overlays will be displayed.</summary>
		None = 0,
		/// <summary>Indicates that physics shapes will be shown.</summary>
		ShowPhysicsShapes = 1 << 0,
		/// <summary>Indicates that bounding boxes will be shown for nodes that have content.</summary>
		ShowBoundingBoxes = 1 << 1,
		/// <summary>Indicates that each Scene Kit light location will be shown.</summary>
		ShowLightInfluences = 1 << 2,
		/// <summary>Indicates that the regions that each Scene Kit light affects will be shown.</summary>
		ShowLightExtents = 1 << 3,
		/// <summary>Indicates that physics fields will be shown.</summary>
		ShowPhysicsFields = 1 << 4,
		/// <summary>Indicates that scene geometry will be rendered as wireframes.</summary>
		ShowWireframe = 1 << 5,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		RenderAsWireframe = 1 << 6,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ShowSkeletons = 1 << 7,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ShowCreases = 1 << 8,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ShowConstraints = 1 << 9,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ShowCameras = 1 << 10,
	}

	/// <summary>Enumerates values that signify the Metal or OpenGLES2 APIs.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNRenderingApi : ulong {
		Metal,
#if !MONOMAC
		[Unavailable (PlatformName.MacCatalyst)]
		[NoMac]
		OpenGLES2,
#else
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		OpenGLLegacy,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		OpenGLCore32,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		OpenGLCore41,
#endif
	}

	/// <summary>Enumerates values that control whether handlers are invoked per frame, per node per frame, or per node per frame per shaded renderable.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNBufferFrequency : long {
		/// <summary>Indicates that handlers are invoked per frame.</summary>
		Frame = 0,
		/// <summary>ndicates that handlers are invoked per node.</summary>
		Node = 1,
		/// <summary>ndicates that handlers are invoked per frame, node, material, and geometry for the shader.</summary>
		Shadable = 2,
	}

	/// <summary>Enumerates values that tell SceneKit whether nodes are expected to move over time.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNMovabilityHint : long {
		Fixed,
		Movable,
	}

	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum SCNColorMask : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Red = 1 << 3,
		/// <summary>To be added.</summary>
		Green = 1 << 2,
		/// <summary>To be added.</summary>
		Blue = 1 << 1,
		/// <summary>To be added.</summary>
		Alpha = 1 << 0,
		/// <summary>To be added.</summary>
		All = 15,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNInteractionMode : long {
		/// <summary>To be added.</summary>
		Fly,
		/// <summary>To be added.</summary>
		OrbitTurntable,
		/// <summary>To be added.</summary>
		OrbitAngleMapping,
		/// <summary>To be added.</summary>
		OrbitCenteredArcball,
		/// <summary>To be added.</summary>
		OrbitArcball,
		/// <summary>To be added.</summary>
		Pan,
		/// <summary>To be added.</summary>
		Truck,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNFillMode : ulong {
		/// <summary>To be added.</summary>
		Fill = 0,
		/// <summary>To be added.</summary>
		Lines = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNTessellationSmoothingMode : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		PNTriangles,
		/// <summary>To be added.</summary>
		Phong,
	}
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNHitTestSearchMode : long {
		/// <summary>To be added.</summary>
		Closest = 0,
		/// <summary>To be added.</summary>
		All = 1,
		/// <summary>To be added.</summary>
		Any = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNCameraProjectionDirection : long {
		/// <summary>To be added.</summary>
		Vertical = 0,
		/// <summary>To be added.</summary>
		Horizontal = 1,
	}

	/// <summary>Enumerates the focusable states of a <see cref="T:SceneKit.SCNNode" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNNodeFocusBehavior : long {
		None = 0,
		Occluding,
		Focusable,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNLightProbeType : long {
		Irradiance = 0,
		Radiance = 1,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNLightProbeUpdateType : long {
		Never = 0,
		Realtime = 1,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SCNLightAreaType : long {
		Rectangle = 1,
		Polygon = 4,
	}

	/// <summary>Enumeration of categories for <see cref="T:SceneKit.SCNPhysicsShape" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum SCNPhysicsShapeType {
		/// <summary>To be added.</summary>
		ConvexHull,
		/// <summary>To be added.</summary>
		BoundingBox,
		/// <summary>To be added.</summary>
		ConcavePolyhedron,
	}
}
