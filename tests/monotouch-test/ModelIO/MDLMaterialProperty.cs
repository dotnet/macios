//
// MDLMaterialProperty Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

using CoreGraphics;
#if MONOMAC
using AppKit;
using UIColor = AppKit.NSColor;
#else
using UIKit;
#endif
#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;
using System.Numerics;
using Matrix4 = global::System.Numerics.Matrix4x4;
using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using VectorFloat3 = global::CoreGraphics.NVector3;
using Bindings.Test;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLMaterialPropertyTest {
		[OneTimeSetUp]
		public void Setup ()
		{

			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
		}


		[Test]
		public void Ctors ()
		{
			Vector2 V2;
			Vector3 V3;
			Vector4 V4;
			NMatrix4 M4;
			MDLTextureSampler tsv;
			NSUrl url;

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion)) {
				Assert.That (obj.Semantic, Is.EqualTo (MDLMaterialSemantic.AmbientOcclusion), "1 Semantic");
				Assert.That (obj.Color, Is.Null, "1 Color");
				Asserts.AreEqual (Vector2.Zero, obj.Float2Value, "1 Float2Value");
				Asserts.AreEqual (Vector3.Zero, obj.Float3Value, "1 Float3Value");
				Asserts.AreEqual (Vector4.Zero, obj.Float4Value, "1 Float4Value");
				Assert.That (obj.FloatValue, Is.EqualTo (0.0f), "1 FloatValue");
				Asserts.AreEqual (Matrix4.Identity, obj.Matrix4x4, "1 Matrix4x4");
				Assert.That (obj.Name, Is.EqualTo ("name"), "1 Name");
				Assert.That (obj.StringValue, Is.Null, "1 StringValue");
				Assert.That (obj.TextureSamplerValue, Is.Null, "1 TextureSamplerValue");
				Assert.That (obj.Type, Is.EqualTo (MDLMaterialPropertyType.Float), "1 Type");
				Assert.That (obj.UrlValue, Is.Null, "1 UrlValue");

				V2 = new Vector2 (1, 2);
				V3 = new Vector3 (3, 4, 5);
				V4 = new Vector4 (6, 7, 8, 9);
				M4 = new NMatrix4 (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
				tsv = new MDLTextureSampler ();
				url = new NSUrl ("http://xamarin.com");

				obj.Semantic = MDLMaterialSemantic.Anisotropic;
				Assert.That (obj.Semantic, Is.EqualTo (MDLMaterialSemantic.Anisotropic), "2 Semantic");

				obj.Color = UIColor.Blue.CGColor;
				Assert.That (obj.Color.ToString (), Is.EqualTo (UIColor.Blue.CGColor.ToString ()), "2 Color");

				obj.Float2Value = V2;
				Asserts.AreEqual (V2, obj.Float2Value, "2 Float2Value");

				obj.Float3Value = V3;
				Asserts.AreEqual (V3, obj.Float3Value, "2 Float3Value");

				obj.Float4Value = V4;
				Asserts.AreEqual (V4, obj.Float4Value, "2 Float4Value");

				obj.FloatValue = 3.14f;
				Assert.That (obj.FloatValue, Is.EqualTo (3.14f), "2 FloatValue");

				obj.Matrix4x4 = M4;
				// It looks like the Matrix4 setter is ignored, assigning a matrix
				// doesn't work in Xcode either.
				Asserts.AreEqual (Matrix4.Identity, obj.Matrix4x4, "2 Matrix4x4");

				obj.Name = "new name";
				Assert.That (obj.Name, Is.EqualTo ("new name"), "2 Name");

				obj.StringValue = "string value";
				Assert.That (obj.StringValue, Is.EqualTo ("string value"), "2 StringValue");

				obj.TextureSamplerValue = tsv;
				Assert.That (obj.TextureSamplerValue.Handle, Is.EqualTo (tsv.Handle), "2 TextureSamplerValue");

				Assert.That (obj.Type, Is.EqualTo (MDLMaterialPropertyType.Texture), "2 Type");

				// Looks like the URLValue can't change after construction
				obj.UrlValue = url;
				if (TestRuntime.CheckXcodeVersion (9, 0)) {
					Assert.That (obj.UrlValue, Is.SameAs (url), "2 UrlValue");
				} else {
					Assert.That (obj.UrlValue, Is.Null, "2 UrlValue");
				}
			}


			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, url)) {
				Assert.That (obj.UrlValue.Handle, Is.EqualTo (url.Handle), "3 UrlValue");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, V3)) {
				Asserts.AreEqual (V3, obj.Float3Value, "4 Float3Value");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, tsv)) {
				Assert.That (obj.TextureSamplerValue.Handle, Is.EqualTo (tsv.Handle), "5 TextureSamplerValue");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, "string value")) {
				Assert.That (obj.StringValue, Is.EqualTo ("string value"), "6 StringValue");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, M4)) {
				Asserts.AreEqual (M4, obj.Matrix4x4, "7 Matrix4x4");
				Asserts.AreEqual (CFunctions.GetMatrixFloat4x4 (obj, "matrix4x4"), obj.Matrix4x4, "7b MatrixFloat4x4");
				Asserts.AreEqual (M4, obj.Matrix4x4, "7c MatrixFloat4x4");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, V4)) {
				Asserts.AreEqual (V4, obj.Float4Value, "8 Float4Value");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, UIColor.Red.CGColor)) {
				Assert.That (obj.Color.ToString (), Is.EqualTo (UIColor.Blue.CGColor.ToString ()), "9 Color");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, V2)) {
				Asserts.AreEqual (V2, obj.Float2Value, "10 Float2Value");
			}

			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion, 3.1415f)) {
				Assert.That (obj.FloatValue, Is.EqualTo (3.1415f), "11 FloatValue");
			}
		}

		[Test]
		public void Copy ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
			using (var obj = new MDLMaterialProperty ("name", MDLMaterialSemantic.AmbientOcclusion)) {
				Assert.That (obj.Copy (), Is.Not.Null);
			}
		}
	}
}
