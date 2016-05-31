﻿//
// MDLTexture Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

#if !__WATCHOS__

using System;
#if XAMCORE_2_0
using Foundation;
using UIKit;
#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;
using ObjCRuntime;
#else
using MonoTouch.Foundation;
#if !__TVOS__
using MonoTouch.MultipeerConnectivity;
#endif
using MonoTouch.UIKit;
using MonoTouch.ModelIO;
using MonoTouch.ObjCRuntime;
#endif
using OpenTK;
using NUnit.Framework;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLTextureTest {
		[TestFixtureSetUp]
		public void Setup ()
		{
			if (!UIDevice.CurrentDevice.CheckSystemVersion (9, 0))
				Assert.Ignore ("Requires iOS9+");

			if (Runtime.Arch == Arch.SIMULATOR && IntPtr.Size == 4) {
				// There's a bug in the i386 version of objc_msgSend where it doesn't preserve SIMD arguments
				// when resizing the cache of method selectors for a type. So here we call all selectors we can
				// find, so that the subsequent tests don't end up producing any cache resize (radar #21630410).
				object dummy;
				using (var obj = new MDLTexture (null, true, null, Vector2i.Zero, 12, 2, MDLTextureChannelEncoding.Float16, false)) {
					dummy = obj.ChannelCount;
					dummy = obj.ChannelEncoding;
					dummy = obj.Dimensions;
					dummy = obj.IsCube;
					dummy = obj.MipLevelCount;
					dummy = obj.Name;
					dummy = obj.RowStride;
					obj.GetTexelDataWithBottomLeftOrigin ();
					obj.GetTexelDataWithBottomLeftOrigin (1, false);
					obj.GetTexelDataWithTopLeftOrigin ();
					obj.GetTexelDataWithTopLeftOrigin (1, false);
				}
				using (var obj = new MDLTexture ()) {
				}
			}
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
					Assert.IsNotNull (txt, "Ain't Null");
					Assert.AreEqual (4, txt.ChannelCount, "ChannelCount");
					Assert.AreEqual (MDLTextureChannelEncoding.UInt8, txt.ChannelEncoding, "ChannelEncoding");
					Assert.AreEqual (new Vector2i (3, 18), txt.Dimensions, "Dimensions");
					Assert.AreEqual (2, txt.MipLevelCount, "MipLevelCount");
					Assert.AreEqual (12, txt.RowStride, "RowStride");
				}
			}
		}

		[Test]
		public void CreateIrradianceTextureCubeTest_b ()
		{
			var V2 = new Vector2i (3, 3);

			using (var obj = new MDLTexture ()) {
				using (var txt = MDLTexture.CreateIrradianceTextureCube (obj, "name", V2, 0.1234f)) {
					Assert.IsNotNull (txt, "Ain't Null");
					Assert.AreEqual (4, txt.ChannelCount, "ChannelCount");
					Assert.AreEqual (MDLTextureChannelEncoding.UInt8, txt.ChannelEncoding, "ChannelEncoding");
					Assert.AreEqual (new Vector2i (3, 18), txt.Dimensions, "Dimensions");
					Assert.AreEqual (1, txt.MipLevelCount, "MipLevelCount");
					Assert.AreEqual (12, txt.RowStride, "RowStride");
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

#endif // !__WATCHOS__
