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
				Assert.That (p.Width, Is.EqualTo ((nuint) 0u), "Width");
				Assert.That (p.Height, Is.EqualTo ((nuint) 0u), "Height");
				Assert.That (p.BytesPerPixel, Is.EqualTo ((nuint) 0u), "BytesPerPixel");
				Assert.That (p.AlignedBytesPerRow, Is.EqualTo ((nuint) 0u), "AlignedBytesPerRow");
				Assert.That (p.Component, Is.EqualTo (default (CGComponent)), "Component");
				Assert.That (p.Layout, Is.EqualTo (default (CGBitmapLayout)), "Layout");
				Assert.That (p.Format, Is.EqualTo (default (CGImagePixelFormatInfo)), "Format");
				Assert.That (p.ColorSpaceHandle, Is.EqualTo (IntPtr.Zero), "ColorSpaceHandle");
				Assert.That (p.HasPremultipliedAlpha, Is.EqualTo (false), "HasPremultipliedAlpha");
				Assert.That (p.ByteOrder, Is.EqualTo ((CFByteOrder) 0), "ByteOrder");
				Assert.That (p.EdrTargetHeadroom, Is.EqualTo (0f), "EdrTargetHeadroom");
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

				Assert.That (p.Width, Is.EqualTo ((nuint) 123u), "Width");
				Assert.That (p.Height, Is.EqualTo ((nuint) 456u), "Height");
				Assert.That (p.BytesPerPixel, Is.EqualTo ((nuint) 4u), "BytesPerPixel");
				Assert.That (p.AlignedBytesPerRow, Is.EqualTo ((nuint) 512u), "AlignedBytesPerRow");
				Assert.That (p.Component, Is.EqualTo ((CGComponent) 1), "Component");
				Assert.That (p.Layout, Is.EqualTo ((CGBitmapLayout) 2), "Layout");
				Assert.That (p.Format, Is.EqualTo ((CGImagePixelFormatInfo) 3), "Format");
				Assert.That (p.ColorSpaceHandle, Is.EqualTo (new IntPtr (0xDEADBEEF)), "ColorSpaceHandle");
				Assert.That (p.HasPremultipliedAlpha, Is.True, "HasPremultipliedAlpha");
				Assert.That (p.ByteOrder, Is.EqualTo (CFByteOrder.LittleEndian), "ByteOrder");
				Assert.That (p.EdrTargetHeadroom, Is.EqualTo (1.5f), "EdrTargetHeadroom");
			});
		}

		[Test]
		public void HasPremultipliedAlphaFalseTest ()
		{
			var p = new CGBitmapParameters ();
			p.HasPremultipliedAlpha = false;
			Assert.That (p.HasPremultipliedAlpha, Is.False, "HasPremultipliedAlpha");
		}

		[Test]
		public void ByteOrderTest ()
		{
			var p = new CGBitmapParameters ();
			p.ByteOrder = CFByteOrder.BigEndian;
			Assert.That (p.ByteOrder, Is.EqualTo (CFByteOrder.BigEndian), "ByteOrder");
			p.ByteOrder = CFByteOrder.LittleEndian;
			Assert.That (p.ByteOrder, Is.EqualTo (CFByteOrder.LittleEndian), "ByteOrder");
		}
	}
}
