//
// ARFaceGeometry.cs: Nicer code for ARFaceGeometry
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using Vector3 = global::CoreGraphics.NVector3;

#nullable enable

namespace ARKit {
	public partial class ARFaceGeometry {

		// Going for GetXXX methods so we can keep the same name as the matching obsoleted property 'Vertices'.
		/// <summary>The array of X,Y,Z coordinates defining the facial geometry.</summary>
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

		// Going for GetXXX methods so we can keep the same name as the matching obsoleted property 'TextureCoordinates'.
		/// <summary>The UV texture coordinates for the corresponding vertex in the <see cref="ARKit.ARFaceGeometry.GetVertices" /> array.</summary>
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

		// Going for GetXXX methods so we can keep the same name as the matching obsoleted property 'TriangleIndices'.
		/// <summary>An array of indices into the <see cref="ARKit.ARFaceGeometry.GetVertices" /> and <see cref="ARKit.ARFaceGeometry.GetTextureCoordinates" /> arrays.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>Every three subsequent values define the indices of a single triangle. </para>
		///         </remarks>
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
	}
}
