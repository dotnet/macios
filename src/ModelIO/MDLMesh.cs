//
// ModelIO/MIEnums.cs: enumerations and definitions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2015 Xamarin, Inc.
//
//
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using ObjCRuntime;

using Vector2i = global::CoreGraphics.NVector2i;
using Vector3i = global::CoreGraphics.NVector3i;

#nullable enable

namespace ModelIO {

	partial class MDLMesh {

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public enum MDLMeshVectorType {
			/// <summary>To be added.</summary>
			Dimensions,
			/// <summary>To be added.</summary>
			Extent,
		}

		internal MDLMesh (Vector3 extent, Vector2i segments, bool inwardNormals, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator, int? hemisphereSegments, bool? cap, bool? isCone)
		{
			if (hemisphereSegments.HasValue) {
				// initCapsule
				InitializeHandle (InitCapsule (extent, segments, hemisphereSegments.Value, inwardNormals, geometryType, allocator), "initCapsuleWithExtent:cylinderSegments:hemisphereSegments:inwardNormals:geometryType:allocator:");
			} else if (cap.HasValue && isCone.HasValue) {
				// initHemisphere || initCone
				if (isCone.Value)
					InitializeHandle (InitCone (extent, segments, inwardNormals, cap.Value, geometryType, allocator), "initConeWithExtent:segments:inwardNormals:cap:geometryType:allocator:");
				else
					InitializeHandle (InitHemisphere (extent, segments, inwardNormals, cap.Value, geometryType, allocator), "initHemisphereWithExtent:segments:inwardNormals:cap:geometryType:allocator:");
			} else {
				// initSphere
				InitializeHandle (InitSphere (extent, segments, inwardNormals, geometryType, allocator), "initSphereWithExtent:segments:inwardNormals:geometryType:allocator:");
			}
		}

		internal MDLMesh (Vector3 extent, Vector3i segments, bool inwardNormals, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			InitializeHandle (InitBox (extent, segments, inwardNormals, geometryType, allocator), "initBoxWithExtent:segments:inwardNormals:geometryType:allocator:");
		}

		internal MDLMesh (Vector3 extent, Vector2i segments, bool inwardNormals, bool topCap, bool bottomCap, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			InitializeHandle (InitCylinder (extent, segments, inwardNormals, topCap, bottomCap, geometryType, allocator), "initCylinderWithExtent:segments:inwardNormals:topCap:bottomCap:geometryType:allocator:");
		}

		internal MDLMesh (Vector3 extent, Vector2i segments, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			InitializeHandle (InitPlane (extent, segments, geometryType, allocator), "initPlaneWithExtent:segments:geometryType:allocator:");
		}

		internal MDLMesh (Vector3 extent, bool inwardNormals, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			InitializeHandle (InitIcosahedron (extent, inwardNormals, geometryType, allocator), "initIcosahedronWithExtent:inwardNormals:geometryType:allocator:");
		}

		internal MDLMesh (MDLMesh mesh, int submeshIndex, uint subdivisionLevels, IMDLMeshBufferAllocator allocator)
		{
			InitializeHandle (InitMesh (mesh, submeshIndex, subdivisionLevels, allocator), "initMeshBySubdividingMesh:submeshIndex:subdivisionLevels:allocator:");
		}

		// Note: we turn these constructors into static constructors because we don't want to lose the shape name. Also, the signatures of these constructors differ so it would not be possible to use an enum to differentiate the shapes.

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateBox (Vector3 dimensions, Vector3i segments, MDLGeometryType geometryType, bool inwardNormals, IMDLMeshBufferAllocator allocator)
		{
			return CreateBox (dimensions, segments, geometryType, inwardNormals, allocator, MDLMeshVectorType.Dimensions);
		}

		/// <summary>Creates a right rectangular box from the <paramref name="vector" />, with the specified number of segments and geometry kind.</summary>
		/// <param name="vector">To be added.</param>
		/// <param name="segments">The number of divisions to create in each dimension.</param>
		/// <param name="geometryType">Whether to create triangles, quadrilaterals, or lines.</param>
		/// <param name="inwardNormals">Whether to generate inward-pointing normals.</param>
		/// <param name="allocator">The allocator to use instead of the default, internal allocator.</param>
		/// <param name="type">The mesh vector type.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateBox (Vector3 vector, Vector3i segments, MDLGeometryType geometryType, bool inwardNormals, IMDLMeshBufferAllocator allocator, MDLMeshVectorType type = MDLMeshVectorType.Dimensions)
		{
			switch (type) {
			case MDLMeshVectorType.Dimensions:
				return NewBoxWithDimensions (vector, segments, geometryType, inwardNormals, allocator);
			case MDLMeshVectorType.Extent:
				return new MDLMesh (vector, segments, inwardNormals, geometryType, allocator);
			default:
				throw new ArgumentException ("The 'MDLMeshVectorType type' argument needs a value.");
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateSphere (Vector3 dimensions, Vector2i segments, MDLGeometryType geometryType, bool inwardNormals, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (dimensions, segments, inwardNormals, geometryType, allocator, null, null, null);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateHemisphere (Vector3 dimensions, Vector2i segments, MDLGeometryType geometryType, bool inwardNormals, bool cap, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (dimensions, segments, inwardNormals, geometryType, allocator, null, cap, false);
		}

		/// <param name="extent">The extent of the cylinder.</param>
		/// <param name="segments">The number of divisions to create in each dimension.</param>
		/// <param name="inwardNormals">Whether to generate inward-pointing normals.</param>
		/// <param name="topCap">Whether to put a top cap on the cylinder.</param>
		/// <param name="bottomCap">Whether to put a bottom cap on the cylinder.</param>
		/// <param name="geometryType">Whether to create triangles, quadrilaterals, or lines.</param>
		/// <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a cylinder from the specified parameters.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateCylinder (Vector3 extent, Vector2i segments, bool inwardNormals, bool topCap, bool bottomCap, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (extent, segments, inwardNormals, topCap, bottomCap, geometryType, allocator);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateCapsule (Vector3 dimensions, Vector2i segments, MDLGeometryType geometryType, bool inwardNormals, int hemisphereSegments, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (dimensions, segments, inwardNormals, geometryType, allocator, hemisphereSegments, null, null);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateCone (Vector3 dimensions, Vector2i segments, MDLGeometryType geometryType, bool inwardNormals, bool cap, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (dimensions, segments, inwardNormals, geometryType, allocator, null, cap, true);
		}

		/// <param name="extent">The extent of the plane.</param>
		/// <param name="segments">The number of divisions to create in each dimension.</param>
		/// <param name="geometryType">Whether to create triangles, quadrilaterals, or lines.</param>
		/// <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a planar region centered at the origin, aligned with the X-Z plane, with the specified dimensions.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreatePlane (Vector3 extent, Vector2i segments, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (extent, segments, geometryType, allocator);
		}

		/// <param name="extent">The extents of the icosahedron.</param>
		/// <param name="inwardNormals">Whether to generate inward-pointing normals.</param>
		/// <param name="geometryType">Whether to create triangles, quadrilaterals, or lines.</param>
		/// <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a regular icosahedron from the specified parameters.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MDLMesh CreateIcosahedron (Vector3 extent, bool inwardNormals, MDLGeometryType geometryType, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (extent, inwardNormals, geometryType, allocator);
		}

		/// <param name="mesh">To be added.</param>
		///         <param name="submeshIndex">To be added.</param>
		///         <param name="subdivisionLevels">To be added.</param>
		///         <param name="allocator">
		///           <para>The allocator to use instead of the default, internal allocator.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static MDLMesh CreateSubdividedMesh (MDLMesh mesh, int submeshIndex, uint subdivisionLevels, IMDLMeshBufferAllocator allocator)
		{
			return new MDLMesh (mesh, submeshIndex, subdivisionLevels, allocator);
		}

		/// <summary>Gets the visual anisotropy of the surface, if present. This data is associated with the <see cref="ModelIO.MDLVertexAttributes.Anisotropy" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? AnisotropyVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Anisotropy);
			}
		}

		/// <summary>Gets the binormal data at each vertex. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Binormal" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? BinormalVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Binormal);
			}
		}

		/// <summary>Gets the bitangent vectors for each vertex. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Bitangent" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? BitangentVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Bitangent);
			}
		}

		/// <summary>Gets color data for each vertex. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Color" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? ColorVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Color);
			}
		}

		/// <summary>Gets data that describe which edges that should not be modified when subdividing the surface. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.EdgeCrease" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? EdgeCreaseVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.EdgeCrease);
			}
		}

		/// <summary>Gets bone and joint indices. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.JointIndices" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? JointIndicesVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.JointIndices);
			}
		}

		/// <summary>Gets bone and joint weight information that is used to render a mesh that is associated with a skeleton. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.JointWeights" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? JointWeightsVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.JointWeights);
			}
		}

		/// <summary>Gets normal vectors for the mesh vertices. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Normal" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? NormalVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Normal);
			}
		}

		/// <summary>Gets occlusion data for the mesh vertices. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.OcclusionValue" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? OcclusionValueVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.OcclusionValue);
			}
		}

		/// <summary>Gets the positions of the mesh vertices. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Position" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? PositionVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Position);
			}
		}

		/// <summary>Gets the U basis vectors that are used for shading. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.ShadingBasisU" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? ShadingBasisUVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.ShadingBasisU);
			}
		}

		/// <summary>Gets the V basis vectors that are used for shading. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.ShadingBasisV" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? ShadingBasisVVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.ShadingBasisV);
			}
		}

		/// <summary>Gets the data that decide which neighboring vertices are used for subdivision calculations. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.SubdivisionStencil" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? SubdivisionStencilVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.SubdivisionStencil);
			}
		}

		/// <summary>Gets the vertex tangent data. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.Tangent" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? TangentVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.Tangent);
			}
		}

		/// <summary>Gets the coordinate system for texture operations on vertices. These data are associated with the <see cref="ModelIO.MDLVertexAttributes.TextureCoordinate" /> key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MDLVertexAttributeData? TextureCoordinateVertexData {
			get {
				return GetVertexAttributeDataForAttribute (MDLVertexAttributes.TextureCoordinate);
			}
		}
	}
}
