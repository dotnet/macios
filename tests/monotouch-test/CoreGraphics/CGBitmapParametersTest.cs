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
				ClassicAssert.AreEqual ((nuint) 0u, p.Width, "Width");
				ClassicAssert.AreEqual ((nuint) 0u, p.Height, "Height");
				ClassicAssert.AreEqual ((nuint) 0u, p.BytesPerPixel, "BytesPerPixel");
				ClassicAssert.AreEqual ((nuint) 0u, p.AlignedBytesPerRow, "AlignedBytesPerRow");
				ClassicAssert.AreEqual (default (CGComponent), p.Component, "Component");
				ClassicAssert.AreEqual (default (CGBitmapLayout), p.Layout, "Layout");
				ClassicAssert.AreEqual (default (CGImagePixelFormatInfo), p.Format, "Format");
				ClassicAssert.AreEqual (IntPtr.Zero, p.ColorSpaceHandle, "ColorSpaceHandle");
				ClassicAssert.AreEqual (false, p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
				ClassicAssert.AreEqual ((CFByteOrder) 0, p.ByteOrder, "ByteOrder");
				ClassicAssert.AreEqual (0f, p.EdrTargetHeadroom, "EdrTargetHeadroom");
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

				ClassicAssert.AreEqual ((nuint) 123u, p.Width, "Width");
				ClassicAssert.AreEqual ((nuint) 456u, p.Height, "Height");
				ClassicAssert.AreEqual ((nuint) 4u, p.BytesPerPixel, "BytesPerPixel");
				ClassicAssert.AreEqual ((nuint) 512u, p.AlignedBytesPerRow, "AlignedBytesPerRow");
				ClassicAssert.AreEqual ((CGComponent) 1, p.Component, "Component");
				ClassicAssert.AreEqual ((CGBitmapLayout) 2, p.Layout, "Layout");
				ClassicAssert.AreEqual ((CGImagePixelFormatInfo) 3, p.Format, "Format");
				ClassicAssert.AreEqual (new IntPtr (0xDEADBEEF), p.ColorSpaceHandle, "ColorSpaceHandle");
				ClassicAssert.IsTrue (p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
				ClassicAssert.AreEqual (CFByteOrder.LittleEndian, p.ByteOrder, "ByteOrder");
				ClassicAssert.AreEqual (1.5f, p.EdrTargetHeadroom, "EdrTargetHeadroom");
			});
		}

		[Test]
		public void HasPremultipliedAlphaFalseTest ()
		{
			var p = new CGBitmapParameters ();
			p.HasPremultipliedAlpha = false;
			ClassicAssert.IsFalse (p.HasPremultipliedAlpha, "HasPremultipliedAlpha");
		}

		[Test]
		public void ByteOrderTest ()
		{
			var p = new CGBitmapParameters ();
			p.ByteOrder = CFByteOrder.BigEndian;
			ClassicAssert.AreEqual (CFByteOrder.BigEndian, p.ByteOrder, "ByteOrder");
			p.ByteOrder = CFByteOrder.LittleEndian;
			ClassicAssert.AreEqual (CFByteOrder.LittleEndian, p.ByteOrder, "ByteOrder");
		}
	}
}
