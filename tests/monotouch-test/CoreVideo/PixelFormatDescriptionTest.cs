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
			Assert.That (CVPixelFormatDescription.AllTypes, Is.Not.Null);
		}

		[Test]
		public void Create ()
		{
			// 0 is not defined
			Assert.That (CVPixelFormatDescription.Create (0), Is.Null, "0");

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV16Gray)) {
				Assert.That (dict, Is.Not.Null, "CV16Gray");
			}

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV32ARGB)) {
				Assert.That (dict, Is.Not.Null, "CV32ARGB");
			}
		}

		static bool registerDone;
		[Test]
		public void Register ()
		{
			if (registerDone)
				Assert.Ignore ("This test can only be executed once, it modifies global state.");
			registerDone = true;

			Assert.That (CVPixelFormatDescription.Create ((CVPixelFormatType) 3), Is.Null, "3a");

			using (var dict = CVPixelFormatDescription.Create (CVPixelFormatType.CV24RGB)) {
				Assert.That (dict, Is.Not.Null, "CV24RGB");
				CVPixelFormatDescription.Register (dict, (CVPixelFormatType) 3);
			}

			Assert.That (CVPixelFormatDescription.Create ((CVPixelFormatType) 3), Is.Not.Null, "3b");
		}

		[Test]
		public void CV32ARGB ()
		{
			Assert.Multiple (() => {
				var pf = CVPixelFormatType.CV32ARGB;
				var desc = CVPixelFormatDescription.CreatePixelFormat (pf);
				Assert.That (desc.Name, Is.Null, "Name");
				Assert.That (desc.Constant ?? ((CVPixelFormatType) 0xFFFFFFFF), Is.EqualTo (pf), "Constant");
				Assert.That (desc.CodecType, Is.Null, "CodecType");
				Assert.That (desc.FourCC, Is.Null, "FourCC");
				Assert.That (desc.ContainsAlpha, Is.EqualTo (true), "ContainsAlpha");
				Assert.That (desc.FormatContainsYCbCr, Is.EqualTo (false), "FormatContainsYCbCr");
				Assert.That (desc.FormatContainsRgb, Is.EqualTo (true), "FormatContainsRgb");
				Assert.That (desc.ContainsGrayscale, Is.EqualTo (false), "ContainsGrayscale");
				if (TestRuntime.CheckXcodeVersion (14, 0))
					Assert.That (desc.FormatContainsSenselArray, Is.Null, "FormatContainsSenselArray");
				if (TestRuntime.CheckXcodeVersion (16, 0))
					Assert.That (desc.ComponentRangeValue, Is.EqualTo (CVPixelFormatComponentRangeValues.FullRange), "ComponentRangeValue");
				Assert.That (desc.Planes, Is.Null, "Planes");
				Assert.That (desc.BlockWidth, Is.Null, "BlockWidth");
				Assert.That (desc.BlockHeight, Is.Null, "BlockHeight");
				Assert.That (desc.BitsPerBlock, Is.EqualTo (32), "BitsPerBlock");
				Assert.That (desc.BlockHorizontalAlignment, Is.Null, "BlockHorizontalAlignment");
				Assert.That (desc.BlockVerticalAlignment, Is.Null, "BlockVerticalAlignment");
				Assert.That (desc.BlackBlock, Is.Not.Null, "BlackBlock");
				Assert.That (desc.HorizontalSubsampling, Is.Null, "HorizontalSubsampling");
				Assert.That (desc.VerticalSubsampling, Is.Null, "VerticalSubsampling");
#if (__IOS__ && !__MACCATALYST__) || __TVOS__
				Assert.That (desc.OpenGLFormat, Is.Null, "OpenGLFormat");
				Assert.That (desc.OpenGLType, Is.Null, "OpenGLType");
				Assert.That (desc.OpenGLInternalFormat, Is.Null, "OpenGLInternalFormat");
				Assert.That (desc.OpenGLCompatibility, Is.Null, "OpenGLCompatibility");
#else
				Assert.That (desc.OpenGLFormat, Is.EqualTo (32993), "OpenGLFormat");
				Assert.That (desc.OpenGLType, Is.EqualTo (32821), "OpenGLType");
				Assert.That (desc.OpenGLInternalFormat, Is.EqualTo (32856), "OpenGLInternalFormat");
				Assert.That (desc.OpenGLCompatibility, Is.EqualTo (true), "OpenGLCompatibility");
#endif
				Assert.That (desc.CGBitmapInfo, Is.EqualTo (CGBitmapFlags.ByteOrder32Big | CGBitmapFlags.First), "CGBitmapInfo");
				Assert.That (desc.QDCompatibility, Is.EqualTo (true), "QDCompatibility");
				Assert.That (desc.CGBitmapContextCompatibility, Is.EqualTo (true), "CGBitmapContextCompatibility");
				Assert.That (desc.CGImageCompatibility, Is.EqualTo (true), "CGImageCompatibility");
				Assert.That (desc.FillExtendedPixelsCallback, Is.Not.Null, "FillExtendedPixelsCallback");
				Assert.That (desc.FillExtendedPixelsCallbackStruct, Is.Not.Null, "FillExtendedPixelsCallbackStruct");
			});
		}
	}
}
