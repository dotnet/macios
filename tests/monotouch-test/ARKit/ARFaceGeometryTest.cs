//
// Unit tests for ARFaceGeometry
//
// Authors:
//	Vincent Dondain <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft. All rights reserved.
//

#if HAS_ARKIT

using System.Threading.Tasks;
using ARKit;
using Xamarin.Utils;

using VectorFloat2 = global::System.Numerics.Vector2;
using VectorFloat3 = global::CoreGraphics.NVector3;

namespace MonoTouchFixtures.ARKit {

	class ARFaceGeometryPoker : ARFaceGeometry {

		GCHandle verticesArrayHandle;
		GCHandle textureCoordinatesArrayHandle;
		GCHandle indicesArrayHandle;
		VectorFloat3 [] vertices;
		VectorFloat2 [] textureCoordinates;
		short [] indices;

		public ARFaceGeometryPoker () : base (IntPtr.Zero)
		{
		}

		public override nuint VertexCount => 2;

		public override nuint TextureCoordinateCount => 2;

		// There are always 3x more 'TriangleIndices' than 'TriangleCount' since 'TriangleIndices' represents Triangles (set of three indices).
		// So 2 'TriangleCount' = 6 'TriangleIndices'.
		public override nuint TriangleCount => 2;

		public override IntPtr GetRawVertices ()
		{
			vertices = new VectorFloat3 [] { new VectorFloat3 (1, 2, 3), new VectorFloat3 (4, 5, 6) };
			if (!verticesArrayHandle.IsAllocated)
				verticesArrayHandle = GCHandle.Alloc (vertices, GCHandleType.Pinned);
			return verticesArrayHandle.AddrOfPinnedObject ();
		}

		public override IntPtr GetRawTextureCoordinates ()
		{
			textureCoordinates = new VectorFloat2 [] { new VectorFloat2 (1, 2), new VectorFloat2 (3, 4) };
			if (!textureCoordinatesArrayHandle.IsAllocated)
				textureCoordinatesArrayHandle = GCHandle.Alloc (textureCoordinates, GCHandleType.Pinned);
			return textureCoordinatesArrayHandle.AddrOfPinnedObject ();
		}

		public override IntPtr GetRawTriangleIndices ()
		{
			// Two triangles (set of 3 indices)
			indices = new short [] { 1, 2, 3, 4, 5, 6 };
			if (!indicesArrayHandle.IsAllocated)
				indicesArrayHandle = GCHandle.Alloc (indices, GCHandleType.Pinned);
			return indicesArrayHandle.AddrOfPinnedObject ();
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (verticesArrayHandle.IsAllocated)
				verticesArrayHandle.Free ();
			if (textureCoordinatesArrayHandle.IsAllocated)
				textureCoordinatesArrayHandle.Free ();
			if (indicesArrayHandle.IsAllocated)
				indicesArrayHandle.Free ();
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ARFaceGeometryTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
		}

		[Test]
		public void VerticesTest ()
		{
			var face = new ARFaceGeometryPoker ();
			var vertices = face.GetVertices ();
			Assert.That (vertices [0], Is.EqualTo (new VectorFloat3 (1, 2, 3)), "Vertex 1");
			Assert.That (vertices [1], Is.EqualTo (new VectorFloat3 (4, 5, 6)), "Vertex 2");
		}

		[Test]
		public void TextureCoordinatesTest ()
		{
			var face = new ARFaceGeometryPoker ();
			var textureCoordinates = face.GetTextureCoordinates ();
			Assert.That (textureCoordinates [0], Is.EqualTo (new VectorFloat2 (1, 2)), "Texture Coordinates 1");
			Assert.That (textureCoordinates [1], Is.EqualTo (new VectorFloat2 (3, 4)), "Texture Coordinates 2");
		}

		[Test]
		public void TriangleIndicesTest ()
		{
			var face = new ARFaceGeometryPoker ();
			Assert.That (face.GetTriangleIndices (), Is.EqualTo (new short [] { 1, 2, 3, 4, 5, 6 }));
		}
	}
}

#endif // HAS_ARKIT
