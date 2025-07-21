//
// ARPlaneGeometry.cs: Nicer code for ARPlaneGeometry
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2018 Microsoft Inc. All rights reserved.
//

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using Vector3 = global::CoreGraphics.NVector3;

#nullable enable

namespace ARKit {
	public partial class ARPlaneGeometry {

		// Using GetXXX methods so it's similar to the ARFaceGeometry methods.
		/// <summary>The array of X,Y,Z coordinates defining the plane geometry.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe Vector3 [] GetVertices ()
		{
			var count = (int) VertexCount;
			var rv = new Vector3 [count];
			var ptr = (Vector3*) GetRawVertices ();
			for (int i = 0; i < count; i++)
				rv [i] = *ptr++;
			return rv;
		}

		// Using GetXXX methods so it's similar to the ARFaceGeometry methods.
		/// <summary>The UV texture coordinates for the corresponding vertex in the <see cref="ARKit.ARPlaneGeometry.GetVertices" /> array.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe Vector2 [] GetTextureCoordinates ()
		{
			var count = (int) TextureCoordinateCount;
			var rv = new Vector2 [count];
			var ptr = (Vector2*) GetRawTextureCoordinates ();
			for (int i = 0; i < count; i++)
				rv [i] = *ptr++;
			return rv;
		}

		// Using GetXXX methods so it's similar to the ARFaceGeometry methods.
		/// <summary>An array of indices into the <see cref="ARKit.ARPlaneGeometry.GetVertices" /> and <see cref="ARKit.ARPlaneGeometry.GetTextureCoordinates" /> arrays.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe short [] GetTriangleIndices ()
		{
			// There are always 3x more 'TriangleIndices' than 'TriangleCount' since 'TriangleIndices' represents Triangles (set of three indices).
			var count = (int) TriangleCount * 3;
			var rv = new short [count];
			var ptr = (short*) GetRawTriangleIndices ();
			for (int i = 0; i < count; i++)
				rv [i] = *ptr++;
			return rv;
		}

		// Using GetXXX methods so it's similar to the ARFaceGeometry methods.
		/// <summary>The vertices that lie along the plane's boundary.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe Vector3 [] GetBoundaryVertices ()
		{
			var count = (int) BoundaryVertexCount;
			var rv = new Vector3 [count];
			var ptr = (Vector3*) GetRawBoundaryVertices ();
			for (int i = 0; i < count; i++)
				rv [i] = *ptr++;
			return rv;
		}
	}
}
