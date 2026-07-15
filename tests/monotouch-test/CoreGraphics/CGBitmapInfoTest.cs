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
			Assert.That (((CGBitmapInfo) CGImageAlphaInfo.PremultipliedLast).GetAlphaInfo (), Is.EqualTo (CGImageAlphaInfo.PremultipliedLast), "GetAlphaInfo");
			Assert.That (((CGBitmapInfo) CGImageComponentInfo.Float).GetComponentInfo (), Is.EqualTo (CGImageComponentInfo.Float), "CGImageComponentInfo");
			Assert.That (((CGBitmapInfo) CGImageByteOrderInfo.ByteOrder32Little).GetByteOrderInfo (), Is.EqualTo (CGImageByteOrderInfo.ByteOrder32Little), "CGImageByteOrderInfo");
			Assert.That (((CGBitmapInfo) CGImagePixelFormatInfo.Rgb101010).GetPixelFormatInfo (), Is.EqualTo (CGImagePixelFormatInfo.Rgb101010), "CGImagePixelFormatInfo");
		}
	}
}
