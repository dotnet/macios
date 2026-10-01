//
// SCNGeometrySource.cs: extensions to provide an array-based API that
// we pass as pointers
//
// Authors:
//   MIguel de Icaza (miguel@xamarin.com)
//
// Copyright Xamarin Inc
//

using CoreGraphics;
using Metal;

#nullable enable

namespace SceneKit {
	public partial class SCNGeometrySource {

		/// <summary>Factory method to create a source for vertex data.</summary>
		/// <param name="vertices">The array of vertices to use as the geometry source.</param>
		public static unsafe SCNGeometrySource FromVertices (SCNVector3 [] vertices)
		{
			if (vertices is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vertices));

			fixed (SCNVector3* ptr = vertices)
				return FromVertices ((IntPtr) ptr, vertices.Length);
		}

		/// <summary>Factory method that creates a source for vertex normals.</summary>
		/// <param name="normals">The array of normal vectors for the geometry source.</param>
		/// <remarks>
		///   <para>The <paramref name="normals" /> must correspond directly to their associated vertices (in another <see cref="SceneKit.SCNGeometrySource" />).</para>
		/// </remarks>
		public static unsafe SCNGeometrySource FromNormals (SCNVector3 [] normals)
		{
			if (normals is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (normals));

			fixed (SCNVector3* ptr = normals)
				return FromNormals ((IntPtr) ptr, normals.Length);
		}

		/// <summary>Factory method that creates a source for texture coordinates.</summary>
		/// <param name="texcoords">The array of texture coordinates for the geometry source.</param>
		/// <remarks>
		///   <para>The <paramref name="texcoords" /> must correspond directly to their associated vertices (in another <see cref="SceneKit.SCNGeometrySource" />).</para>
		///   <para>For non-tiling textures, texture coordinates are values between 0 and 1 that describe the mapping between a texture location and a geometry location. A value of [0,0] represents the origin of the texture while [1,1] represents the point at its furthest extent.</para>
		/// </remarks>
		public static unsafe SCNGeometrySource FromTextureCoordinates (CGPoint [] texcoords)
		{
			if (texcoords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (texcoords));

			fixed (CGPoint* ptr = texcoords)
				return FromTextureCoordinates ((IntPtr) ptr, texcoords.Length);
		}

		static NSString SemanticToToken (SCNGeometrySourceSemantics geometrySourceSemantic)
		{
			switch (geometrySourceSemantic) {
			case SCNGeometrySourceSemantics.Vertex:
				return SCNGeometrySourceSemantic.Vertex;
			case SCNGeometrySourceSemantics.Normal:
				return SCNGeometrySourceSemantic.Normal;
			case SCNGeometrySourceSemantics.Color:
				return SCNGeometrySourceSemantic.Color;
			case SCNGeometrySourceSemantics.Texcoord:
				return SCNGeometrySourceSemantic.Texcoord;
			case SCNGeometrySourceSemantics.VertexCrease:
				return SCNGeometrySourceSemantic.VertexCrease;
			case SCNGeometrySourceSemantics.EdgeCrease:
				return SCNGeometrySourceSemantic.EdgeCrease;
			case SCNGeometrySourceSemantics.BoneWeights:
				return SCNGeometrySourceSemantic.BoneWeights;
			case SCNGeometrySourceSemantics.BoneIndices:
				return SCNGeometrySourceSemantic.BoneIndices;
			default:
				throw new System.ArgumentException ("geometrySourceSemantic");
			}
		}

		/// <summary>Creates a geometry source from raw data with the specified layout.</summary>
		/// <param name="data">The data buffer containing the geometry source values.</param>
		/// <param name="semantic">The semantic type of the geometry source data.</param>
		/// <param name="vectorCount">The number of vectors in the data.</param>
		/// <param name="floatComponents">Whether the component values are floating-point.</param>
		/// <param name="componentsPerVector">The number of scalar components in each vector.</param>
		/// <param name="bytesPerComponent">The size, in bytes, of each component.</param>
		/// <param name="offset">The offset, in bytes, from the beginning of the data to the first vector.</param>
		/// <param name="stride">The number of bytes between vectors in the data.</param>
		public static SCNGeometrySource FromData (NSData data, SCNGeometrySourceSemantics semantic, nint vectorCount, bool floatComponents, nint componentsPerVector, nint bytesPerComponent, nint offset, nint stride)
		{
			return FromData (data, SemanticToToken (semantic), vectorCount, floatComponents, componentsPerVector, bytesPerComponent, offset, stride);
		}

		/// <summary>Factory method to create a new <see cref="SceneKit.SCNGeometrySource" /> from a Metal buffer.</summary>
		/// <param name="mtlBuffer">The Metal buffer containing the vertex data.</param>
		/// <param name="vertexFormat">The vertex format describing the data layout.</param>
		/// <param name="semantic">The semantic type of the geometry source data.</param>
		/// <param name="vertexCount">The number of vertices in the buffer.</param>
		/// <param name="offset">The offset, in bytes, to the first vertex in the buffer.</param>
		/// <param name="stride">The number of bytes between vertices in the buffer.</param>
		public static SCNGeometrySource FromMetalBuffer (IMTLBuffer mtlBuffer, MTLVertexFormat vertexFormat, SCNGeometrySourceSemantics semantic, nint vertexCount, nint offset, nint stride)
		{
			return FromMetalBuffer (mtlBuffer, vertexFormat, SemanticToToken (semantic), vertexCount, offset, stride);
		}
	}

}
