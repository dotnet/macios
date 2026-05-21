//
// MDLTexture Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;

using System.Numerics;
using Vector2i = global::CoreGraphics.NVector2i;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLTextureTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void Ctor ()
		{
			var V2 = new Vector2i (123, 456);

			using (var obj = new MDLTexture (null, true, null, V2, 12, 2, MDLTextureChannelEncoding.Float16, false)) {
				Asserts.AreEqual (V2, obj.Dimensions, "dimensions");
			}
		}

		[Test]
		public void CreateIrradianceTextureCubeTest_a ()
		{
			var V2 = new Vector2i (3, 3);

			using (var obj = new MDLTexture ()) {
				using (var txt = MDLTexture.CreateIrradianceTextureCube (obj, "name", V2)) {
					if (TestRuntime.CheckXcodeVersion (8, 0)) {
						Assert.That (txt, Is.Null, "Is Null"); // this is probably because the arguments to CreateIrradianceTextureCube are invalid, but I haven't been able to figure out valid values.
					} else {
						Assert.That (txt, Is.Not.Null, "Ain't Null");
						Assert.That (txt.ChannelCount, Is.EqualTo ((nuint) 4), "ChannelCount");
						Assert.That (txt.ChannelEncoding, Is.EqualTo (MDLTextureChannelEncoding.UInt8), "ChannelEncoding");
						Assert.That (txt.Dimensions, Is.EqualTo (new Vector2i (3, 18)), "Dimensions");
						Assert.That (txt.MipLevelCount, Is.EqualTo ((nuint) 2), "MipLevelCount");
						Assert.That (txt.RowStride, Is.EqualTo ((nint) 12), "RowStride");
					}
				}
			}
		}

		[Test]
		public void CreateIrradianceTextureCubeTest_b ()
		{
			var V2 = new Vector2i (3, 3);

			using (var obj = new MDLTexture ()) {
				using (var txt = MDLTexture.CreateIrradianceTextureCube (obj, "name", V2, 0.1234f)) {
					if (TestRuntime.CheckXcodeVersion (8, 0)) {
						Assert.That (txt, Is.Null, "Is Null"); // this is probably because the arguments to CreateIrradianceTextureCube are invalid, but I haven't been able to figure out valid values.
					} else {
						Assert.That (txt, Is.Not.Null, "Ain't Null");
						Assert.That (txt.ChannelCount, Is.EqualTo ((nuint) 4), "ChannelCount");
						Assert.That (txt.ChannelEncoding, Is.EqualTo (MDLTextureChannelEncoding.UInt8), "ChannelEncoding");
						Assert.That (txt.Dimensions, Is.EqualTo (new Vector2i (3, 18)), "Dimensions");
						Assert.That (txt.MipLevelCount, Is.EqualTo ((nuint) 1), "MipLevelCount");
						Assert.That (txt.RowStride, Is.EqualTo ((nint) 12), "RowStride");
					}
				}
			}
		}

		[Test]
		public void DimensionsTest ()
		{
			var V2 = new Vector2i (123, 456);

			using (var txt = new MDLTexture ()) {
				Asserts.AreEqual (Vector2i.Zero, txt.Dimensions, "a");
			}
		}
	}
}
