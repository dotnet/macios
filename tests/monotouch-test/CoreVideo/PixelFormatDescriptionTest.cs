//
// Unit tests for CVPixelFormatDescription
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.IO;
using System.Text;

using CoreGraphics;
using CoreVideo;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PixelFormatDescriptionTest {

		[Test]
		public void AllTypes ()
		{
			// https://bugzilla.xamarin.com/show_bug.cgi?id=13917
			ClassicAssert.NotNull (CVPixelFormatDescription.AllTypes);
		}

		[Test]
		public void Create ()
		{
			// 0 is not defined
			ClassicAssert.Null (CVPixelFormatDescription.Create (0), "0");

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV16Gray)) {
				ClassicAssert.NotNull (dict, "CV16Gray");
			}

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV32ARGB)) {
				ClassicAssert.NotNull (dict, "CV32ARGB");
			}
		}

		static bool registerDone;
		[Test]
		public void Register ()
		{
			if (registerDone)
				Assert.Ignore ("This test can only be executed once, it modifies global state.");
			registerDone = true;

			ClassicAssert.Null (CVPixelFormatDescription.Create ((CVPixelFormatType) 3), "3a");

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV24RGB)) {
				ClassicAssert.NotNull (dict, "CV24RGB");
				CVPixelFormatDescription.Register (dict, (CVPixelFormatType) 3);
			}

			ClassicAssert.NotNull (CVPixelFormatDescription.Create ((CVPixelFormatType) 3), "3b");
		}

		[Test]
		public void CV32ARGB ()
		{
			Assert.Multiple (() => {
				var pf = CVPixelFormatType.CV32ARGB;
				var desc = CVPixelFormatDescription.CreatePixelFormat (pf);
				ClassicAssert.IsNull (desc.Name, "Name");
				ClassicAssert.AreEqual (pf, desc.Constant ?? ((CVPixelFormatType) 0xFFFFFFFF), "Constant");
				ClassicAssert.IsNull (desc.CodecType, "CodecType");
				ClassicAssert.IsNull (desc.FourCC, "FourCC");
				ClassicAssert.AreEqual (true, desc.ContainsAlpha, "ContainsAlpha");
				ClassicAssert.AreEqual (false, desc.FormatContainsYCbCr, "FormatContainsYCbCr");
				ClassicAssert.AreEqual (true, desc.FormatContainsRgb, "FormatContainsRgb");
				ClassicAssert.AreEqual (false, desc.ContainsGrayscale, "ContainsGrayscale");
				if (TestRuntime.CheckXcodeVersion (14, 0))
					ClassicAssert.IsNull (desc.FormatContainsSenselArray, "FormatContainsSenselArray");
				if (TestRuntime.CheckXcodeVersion (16, 0))
					ClassicAssert.AreEqual (CVPixelFormatComponentRangeValues.FullRange, desc.ComponentRangeValue, "ComponentRangeValue");
				ClassicAssert.IsNull (desc.Planes, "Planes");
				ClassicAssert.IsNull (desc.BlockWidth, "BlockWidth");
				ClassicAssert.IsNull (desc.BlockHeight, "BlockHeight");
				ClassicAssert.AreEqual (32, desc.BitsPerBlock, "BitsPerBlock");
				ClassicAssert.IsNull (desc.BlockHorizontalAlignment, "BlockHorizontalAlignment");
				ClassicAssert.IsNull (desc.BlockVerticalAlignment, "BlockVerticalAlignment");
				ClassicAssert.IsNotNull (desc.BlackBlock, "BlackBlock");
				ClassicAssert.IsNull (desc.HorizontalSubsampling, "HorizontalSubsampling");
				ClassicAssert.IsNull (desc.VerticalSubsampling, "VerticalSubsampling");
#if (__IOS__ && !__MACCATALYST__) || __TVOS__
				ClassicAssert.IsNull (desc.OpenGLFormat, "OpenGLFormat");
				ClassicAssert.IsNull (desc.OpenGLType, "OpenGLType");
				ClassicAssert.IsNull (desc.OpenGLInternalFormat, "OpenGLInternalFormat");
				ClassicAssert.IsNull (desc.OpenGLCompatibility, "OpenGLCompatibility");
#else
				ClassicAssert.AreEqual (32993, desc.OpenGLFormat, "OpenGLFormat");
				ClassicAssert.AreEqual (32821, desc.OpenGLType, "OpenGLType");
				ClassicAssert.AreEqual (32856, desc.OpenGLInternalFormat, "OpenGLInternalFormat");
				ClassicAssert.AreEqual (true, desc.OpenGLCompatibility, "OpenGLCompatibility");
#endif
				ClassicAssert.AreEqual (CGBitmapFlags.ByteOrder32Big | CGBitmapFlags.First, desc.CGBitmapInfo, "CGBitmapInfo");
				ClassicAssert.AreEqual (true, desc.QDCompatibility, "QDCompatibility");
				ClassicAssert.AreEqual (true, desc.CGBitmapContextCompatibility, "CGBitmapContextCompatibility");
				ClassicAssert.AreEqual (true, desc.CGImageCompatibility, "CGImageCompatibility");
				ClassicAssert.IsNotNull (desc.FillExtendedPixelsCallback, "FillExtendedPixelsCallback");
				ClassicAssert.IsNotNull (desc.FillExtendedPixelsCallbackStruct, "FillExtendedPixelsCallbackStruct");
			});
		}
	}
}
