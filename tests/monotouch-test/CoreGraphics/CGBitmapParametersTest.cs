// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using CoreFoundation;

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGBitmapParametersTest {
		[Test]
		public void DefaultValuesTest ()
		{
			Assert.Multiple (() => {
				var p = new CGBitmapParameters ();
				Assert.AreEqual ((nuint) 0u, p.Width, "Width");
				Assert.AreEqual ((nuint) 0u, p.Height, "Height");
				Assert.AreEqual ((nuint) 0u, p.BytesPerPixel, "BytesPerPixel");
				Assert.AreEqual ((nuint) 0u, p.AlignedBytesPerRow, "AlignedBytesPerRow");
				Assert.AreEqual (default (CGComponent), p.Component, "Component");
				Assert.AreEqual (default (CGBitmapLayout), p.Layout, "Layout");
				Assert.AreEqual (default (CGImagePixelFormatInfo), p.Format, "Format");
				Assert.AreEqual (IntPtr.Zero, p.ColorSpaceHandle, "ColorSpaceHandle");
				Assert.AreEqual (false, p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
				Assert.AreEqual ((CFByteOrder) 0, p.ByteOrder, "ByteOrder");
				Assert.AreEqual (0f, p.EdrTargetHeadroom, "EdrTargetHeadroom");
			});
		}

		[Test]
		public void PropertySetGetTest ()
		{
			Assert.Multiple (() => {
				var p = new CGBitmapParameters ();
				p.Width = 123u;
				p.Height = 456u;
				p.BytesPerPixel = 4u;
				p.AlignedBytesPerRow = 512u;
				p.Component = (CGComponent) 1;
				p.Layout = (CGBitmapLayout) 2;
				p.Format = (CGImagePixelFormatInfo) 3;
				p.ColorSpaceHandle = new IntPtr (0xDEADBEEF);
				p.HasPremultipliedAlpha = true;
				p.ByteOrder = CFByteOrder.LittleEndian;
				p.EdrTargetHeadroom = 1.5f;

				Assert.AreEqual ((nuint) 123u, p.Width, "Width");
				Assert.AreEqual ((nuint) 456u, p.Height, "Height");
				Assert.AreEqual ((nuint) 4u, p.BytesPerPixel, "BytesPerPixel");
				Assert.AreEqual ((nuint) 512u, p.AlignedBytesPerRow, "AlignedBytesPerRow");
				Assert.AreEqual ((CGComponent) 1, p.Component, "Component");
				Assert.AreEqual ((CGBitmapLayout) 2, p.Layout, "Layout");
				Assert.AreEqual ((CGImagePixelFormatInfo) 3, p.Format, "Format");
				Assert.AreEqual (new IntPtr (0xDEADBEEF), p.ColorSpaceHandle, "ColorSpaceHandle");
				Assert.IsTrue (p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
				Assert.AreEqual (CFByteOrder.LittleEndian, p.ByteOrder, "ByteOrder");
				Assert.AreEqual (1.5f, p.EdrTargetHeadroom, "EdrTargetHeadroom");
			});
		}

		[Test]
		public void HasPremultipliedAlphaFalseTest ()
		{
			var p = new CGBitmapParameters ();
			p.HasPremultipliedAlpha = false;
			Assert.IsFalse (p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
		}

		[Test]
		public void ByteOrderTest ()
		{
			var p = new CGBitmapParameters ();
			p.ByteOrder = CFByteOrder.BigEndian;
			Assert.AreEqual (CFByteOrder.BigEndian, p.ByteOrder, "ByteOrder");
			p.ByteOrder = CFByteOrder.LittleEndian;
			Assert.AreEqual (CFByteOrder.LittleEndian, p.ByteOrder, "ByteOrder");
		}
	}
}
