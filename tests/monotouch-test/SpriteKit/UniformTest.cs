using System;
using Foundation;
using SpriteKit;
#if !MONOMAC
using UIKit;
#endif
using ObjCRuntime;

using CoreGraphics;
using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using VectorFloat3 = global::CoreGraphics.NVector3;

using NUnit.Framework;
using Bindings.Test;

namespace MonoTouchFixtures.SpriteKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UniformTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void Ctors ()
		{
			SKTexture texture;

			using (var obj = new SKUniform ("name")) {
				var N4Zero = default (NMatrix4);
				var N3Zero = default (NMatrix3);
				var N2Zero = default (NMatrix2);

				Assert.AreEqual ("name", obj.Name, "1 Name");
				Assert.AreEqual (SKUniformType.None, obj.UniformType, "1 UniformType");
				Assert.IsNull (obj.TextureValue, "1 TextureValue");
				Assert.AreEqual (0.0f, obj.FloatValue, "1 FloatValue");
				Asserts.AreEqual (N2Zero, obj.MatrixFloat2x2Value, "1 MatrixFloat2x2Value");
				Asserts.AreEqual (N3Zero, obj.MatrixFloat3x3Value, "1 MatrixFloat3x3Value");
				Asserts.AreEqual (N4Zero, obj.MatrixFloat4x4Value, "1 MatrixFloat4x4Value");

				texture = SKTexture.FromImageNamed ("basn3p08.png");
				obj.TextureValue = texture;
				Assert.AreEqual (texture, obj.TextureValue, "2 TextureValue");

				obj.FloatValue = 0.5f;
				Assert.AreEqual (0.5f, obj.FloatValue, "2 FloatValue");
			}

			bool hasSimdConstructors = TestRuntime.CheckXcodeVersion (8, 0);
			using (var obj = new SKUniform ("name", texture)) {
				Assert.AreEqual (texture, obj.TextureValue, "3 TextureValue");
			}

			using (var obj = new SKUniform ("name", 3.1415f)) {
				Assert.AreEqual (3.1415f, obj.FloatValue, "4 FloatValue");
			}
		}

		[Test]
		public void Create ()
		{
			var M2x2 = new MatrixFloat2x2 (1, 2, 3, 4);
			var M3x3 = new MatrixFloat3x3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
			var M4x4 = new MatrixFloat4x4 (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

			using (var obj = SKUniform.Create ("name", M2x2)) {
				Asserts.AreEqual (M2x2, obj.MatrixFloat2x2Value, "11 MatrixFloat2x2Value");
				Asserts.AreEqual (M2x2, CFunctions.GetMatrixFloat2x2 (obj, "matrixFloat2x2Value"), "11b MatrixFloat2x2Value");
				var tmp2 = new MatrixFloat2x2 (9, 8, 7, 6);
				obj.MatrixFloat2x2Value = tmp2;
				Asserts.AreEqual (tmp2, obj.MatrixFloat2x2Value, "11 MatrixFloat2x2Value second");
				Asserts.AreEqual (tmp2, CFunctions.GetMatrixFloat2x2 (obj, "matrixFloat2x2Value"), "11b MatrixFloat2x2Value second");
			}

			using (var obj = SKUniform.Create ("name", M3x3)) {
				Asserts.AreEqual (M3x3, obj.MatrixFloat3x3Value, "12 MatrixFloat3x3Value");
				Asserts.AreEqual (M3x3, CFunctions.GetMatrixFloat3x3 (obj, "matrixFloat3x3Value"), "12b MatrixFloat3x3Value");
				var tmp3 = new MatrixFloat3x3 (9, 8, 7, 6, 5, 4, 3, 2, 1);
				obj.MatrixFloat3x3Value = tmp3;
				Asserts.AreEqual (tmp3, obj.MatrixFloat3x3Value, "12 MatrixFloat3x3Value second");
				Asserts.AreEqual (tmp3, CFunctions.GetMatrixFloat3x3 (obj, "matrixFloat3x3Value"), "12b MatrixFloat3x3Value second");
			}

			using (var obj = SKUniform.Create ("name", M4x4)) {
				Asserts.AreEqual (M4x4, obj.MatrixFloat4x4Value, "13  MatrixFloat4x4Value");
				Asserts.AreEqual (M4x4, CFunctions.GetMatrixFloat4x4 (obj, "matrixFloat4x4Value"), "13b FloatMatrix4Value");
				var tmp4 = new MatrixFloat4x4 (9, 8, 7, 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6);
				obj.MatrixFloat4x4Value = tmp4;
				Asserts.AreEqual (tmp4, obj.MatrixFloat4x4Value, "13 MatrixFloat4x4Value second");
				Asserts.AreEqual (tmp4, CFunctions.GetMatrixFloat4x4 (obj, "matrixFloat4x4Value"), "13b MatrixFloat4x4Value second");
			}
		}
	}
}
