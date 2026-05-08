// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGBitmapInfoTest {
		[Test]
		public void Extensions ()
		{
			ClassicAssert.AreEqual (CGImageAlphaInfo.PremultipliedLast, ((CGBitmapInfo) CGImageAlphaInfo.PremultipliedLast).GetAlphaInfo (), "GetAlphaInfo");
			ClassicAssert.AreEqual (CGImageComponentInfo.Float, ((CGBitmapInfo) CGImageComponentInfo.Float).GetComponentInfo (), "CGImageComponentInfo");
			ClassicAssert.AreEqual (CGImageByteOrderInfo.ByteOrder32Little, ((CGBitmapInfo) CGImageByteOrderInfo.ByteOrder32Little).GetByteOrderInfo (), "CGImageByteOrderInfo");
			ClassicAssert.AreEqual (CGImagePixelFormatInfo.Rgb101010, ((CGBitmapInfo) CGImagePixelFormatInfo.Rgb101010).GetPixelFormatInfo (), "CGImagePixelFormatInfo");
		}
	}
}
