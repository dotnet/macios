//
// SCNGeometrySource.cs: extensions to provide an array-based API that
// we pass as pointers
//
// Authors:
//   MIguel de Icaza (miguel@xamarin.com)
//
// Copyright Xamarin Inc
//
using System;

using CoreGraphics;
using Foundation;
using Metal;

#nullable enable

namespace SceneKit {
	public partial class SCNGeometrySource {

		/// <param name="vertices">To be added.</param>
		///         <summary>Factory method to create a source for vertex data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static unsafe SCNGeometrySource FromVertices (SCNVector3 [] vertices)
		{
			if (vertices is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vertices));

			fixed (SCNVector3* ptr = vertices)
				return FromVertices ((IntPtr) ptr, vertices.Length);
		}

		/// <param name="normals">To be added.</param>
		///         <summary>Factory method that creates a source for vertex normals.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The <paramref name="normals" /> must correspond directly to their associated vertices (in another <see cref="SceneKit.SCNGeometrySource" />).</para>
		///         </remarks>
		public static unsafe SCNGeometrySource FromNormals (SCNVector3 [] normals)
		{
			if (normals is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (normals));

			fixed (SCNVector3* ptr = normals)
				return FromNormals ((IntPtr) ptr, normals.Length);
		}

		/// <param name="texcoords">To be added.</param>
		///         <summary>Factory method that creates a source for texture coordinates.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The <paramref name="texcoords" /> must correspond directly to their associated vertices (in another <see cref="SceneKit.SCNGeometrySource" />).</para>
		///           <para>For non-tiling textures, texture coordinates are values between 0 and 1 that describe the mapping between a texture location and a geometry location. A value of [0,0] represents the origin of the texture while [1,1] represents the point at its furthest extent.</para>
		///         </remarks>
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

		/// <param name="data">To be added.</param>
		/// <param name="semantic">To be added.</param>
		/// <param name="vectorCount">To be added.</param>
		/// <param name="floatComponents">To be added.</param>
		/// <param name="componentsPerVector">To be added.</param>
		/// <param name="bytesPerComponent">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="stride">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static SCNGeometrySource FromData (NSData data, SCNGeometrySourceSemantics semantic, nint vectorCount, bool floatComponents, nint componentsPerVector, nint bytesPerComponent, nint offset, nint stride)
		{
			return FromData (data, SemanticToToken (semantic), vectorCount, floatComponents, componentsPerVector, bytesPerComponent, offset, stride);
		}

		/// <param name="mtlBuffer">To be added.</param>
		/// <param name="vertexFormat">To be added.</param>
		/// <param name="semantic">To be added.</param>
		/// <param name="vertexCount">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="stride">To be added.</param>
		/// <summary>Factory method to create a new <see cref="SceneKit.SCNGeometrySource" /> from a data buffer.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static SCNGeometrySource FromMetalBuffer (IMTLBuffer mtlBuffer, MTLVertexFormat vertexFormat, SCNGeometrySourceSemantics semantic, nint vertexCount, nint offset, nint stride)
		{
			return FromMetalBuffer (mtlBuffer, vertexFormat, SemanticToToken (semantic), vertexCount, offset, stride);
		}
	}

}
