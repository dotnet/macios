// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.InteropServices;
using AVFoundation;
#if __MACOS__
using AppKit;
#endif
#if !__TVOS__
using WebKit;
#endif

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class EnumConversionTest {
		[Test]
		public void AVErrorParsingValues ()
		{
			Assert.That ((long) AVError.FileFailedToParse, Is.EqualTo (-11829), "File parsing");
#if XAMCORE_5_0
			Assert.That ((long) AVError.FailedToParse, Is.EqualTo (-11853), "Data parsing");
#else
			Assert.That ((long) AVError.FailedToParse, Is.EqualTo (-11829), "Legacy value");
			Assert.That ((long) AVError.FailedToParse2, Is.EqualTo (-11853), "Data parsing");
#endif
		}

#if __MACOS__
		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern nuint GetNativeValue (IntPtr handle, IntPtr selector);

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern void SetNativeValue (IntPtr handle, IntPtr selector, nuint value);

		[TestCase (NSImageInterpolation.Default, 0)]
		[TestCase (NSImageInterpolation.None, 1)]
		[TestCase (NSImageInterpolation.Low, 2)]
		[TestCase (NSImageInterpolation.Medium, 4)]
		[TestCase (NSImageInterpolation.High, 3)]
		[TestCase ((NSImageInterpolation) 42, 42)]
		public void ImageInterpolation (NSImageInterpolation managed, int native)
		{
			Assert.That (NSImageInterpolationExtensions.ToNative (managed), Is.EqualTo ((nuint) native), "ToNative");
			Assert.That (NSImageInterpolationExtensions.ToManaged ((nuint) native), Is.EqualTo (managed), "ToManaged");
		}

		[TestCase (NSImageInterpolation.Medium, 4)]
		[TestCase (NSImageInterpolation.High, 3)]
		public void ImageInterpolationProperty (NSImageInterpolation managed, int native)
		{
			using var bitmap = new NSBitmapImageRep (IntPtr.Zero, 10, 10, 8, 4, true, false, NSColorSpace.DeviceRGB, 0, 0);
			using var context = NSGraphicsContext.FromBitmap (bitmap);
			context.ImageInterpolation = managed;
			Assert.That (GetNativeValue (context.Handle, Selector.GetHandle ("imageInterpolation")), Is.EqualTo ((nuint) native), "Native getter");
			SetNativeValue (context.Handle, Selector.GetHandle ("setImageInterpolation:"), (nuint) native);
			Assert.That (context.ImageInterpolation, Is.EqualTo (managed), "Managed getter");
		}

		[TestCase (NSImageResizingMode.Stretch, 1)]
		[TestCase (NSImageResizingMode.Tile, 0)]
		public void ImageResizingMode (NSImageResizingMode managed, int arm64Native)
		{
			var native = (nint) arm64Native;
#if !XAMCORE_5_0
			if (!Runtime.IsARM64CallingConvention)
				native = (nint) (long) managed;
#endif
			Assert.That (NSImageResizingModeExtensions.ToNative (managed), Is.EqualTo (native), "ToNative");
			Assert.That (NSImageResizingModeExtensions.ToManaged (native), Is.EqualTo (managed), "ToManaged");
			using var image = new NSImage ();
			image.ResizingMode = managed;
			Assert.That (GetNativeValue (image.Handle, Selector.GetHandle ("resizingMode")), Is.EqualTo ((nuint) native), "Native getter");
			Assert.That (image.ResizingMode, Is.EqualTo (managed), "Managed getter");
		}

		[TestCase (NSTextAlignment.Left, 0)]
		[TestCase (NSTextAlignment.Center, 1)]
		[TestCase (NSTextAlignment.Right, 2)]
		[TestCase (NSTextAlignment.Justified, 3)]
		[TestCase (NSTextAlignment.Natural, 4)]
		public void TextAlignment (NSTextAlignment managed, int arm64Native)
		{
			var native = (nuint) arm64Native;
#if !XAMCORE_5_0
			if (!Runtime.IsARM64CallingConvention)
				native = (nuint) (ulong) managed;
#endif
			Assert.That (NSTextAlignmentExtensions.ToNative (managed), Is.EqualTo (native), "ToNative");
			Assert.That (NSTextAlignmentExtensions.ToManaged (native), Is.EqualTo (managed), "ToManaged");
			using var field = new NSTextField ();
			field.Alignment = managed;
			Assert.That (GetNativeValue (field.Handle, Selector.GetHandle ("alignment")), Is.EqualTo (native), "Native getter");
			Assert.That (field.Alignment, Is.EqualTo (managed), "Managed getter");
		}

		[TestCase (NSTickMarkPosition.Below, 0)]
		[TestCase (NSTickMarkPosition.Above, 1)]
		[TestCase (NSTickMarkPosition.Left, 1)]
		[TestCase (NSTickMarkPosition.Right, 0)]
		[TestCase (NSTickMarkPosition.Leading, 1)]
		[TestCase (NSTickMarkPosition.Trailing, 0)]
		public void TickMarkPosition (NSTickMarkPosition managed, int native)
		{
			Assert.That (NSTickMarkPositionExtensions.ToNative (managed), Is.EqualTo ((nuint) native), "ToNative");
			Assert.That (NSTickMarkPositionExtensions.ToManaged ((nuint) native), Is.EqualTo ((NSTickMarkPosition) native), "Canonical ToManaged");
			using var slider = new NSSlider ();
			slider.TickMarkPosition = managed;
			Assert.That (GetNativeValue (slider.Handle, Selector.GetHandle ("tickMarkPosition")), Is.EqualTo ((nuint) native), "Native getter");
			Assert.That (slider.TickMarkPosition, Is.EqualTo ((NSTickMarkPosition) native), "Canonical managed getter");
		}

		[Test]
		public void AppKitValues ()
		{
#if XAMCORE_5_0
			Assert.That ((ulong) NSImageInterpolation.Medium, Is.EqualTo (4), "Medium");
			Assert.That ((ulong) NSImageInterpolation.High, Is.EqualTo (3), "High");
			Assert.That ((long) NSImageResizingMode.Stretch, Is.EqualTo (1), "Stretch");
			Assert.That ((long) NSImageResizingMode.Tile, Is.EqualTo (0), "Tile");
			Assert.That ((ulong) NSTextAlignment.Center, Is.EqualTo (1), "Center");
			Assert.That ((ulong) NSTextAlignment.Right, Is.EqualTo (2), "Right alignment");
			Assert.That ((ulong) NSTickMarkPosition.Leading, Is.EqualTo (1), "Leading");
			Assert.That ((ulong) NSTickMarkPosition.Trailing, Is.EqualTo (0), "Trailing");
#else
			Assert.That ((ulong) NSImageInterpolation.Medium, Is.EqualTo (3), "Legacy Medium");
			Assert.That ((ulong) NSImageInterpolation.High, Is.EqualTo (4), "Legacy High");
			Assert.That ((long) NSImageResizingMode.Stretch, Is.EqualTo (0), "Legacy Stretch");
			Assert.That ((long) NSImageResizingMode.Tile, Is.EqualTo (1), "Legacy Tile");
			Assert.That ((ulong) NSTextAlignment.Center, Is.EqualTo (2), "Legacy Center");
			Assert.That ((ulong) NSTextAlignment.Right, Is.EqualTo (1), "Legacy Right alignment");
			Assert.That ((ulong) NSTickMarkPosition.Leading, Is.EqualTo (2), "Legacy Leading");
			Assert.That ((ulong) NSTickMarkPosition.Trailing, Is.EqualTo (3), "Legacy Trailing");
#endif
		}
#endif

#if !__TVOS__
		[TestCase (WKMediaPlaybackState.None, 0)]
		[TestCase (WKMediaPlaybackState.Playing, 1)]
		[TestCase (WKMediaPlaybackState.Paused, 2)]
		[TestCase (WKMediaPlaybackState.Suspended, 3)]
		[TestCase ((WKMediaPlaybackState) 42, 42)]
		public void MediaPlaybackState (WKMediaPlaybackState managed, int native)
		{
			TestRuntime.AssertXcodeVersion (12, 5);
			Assert.That (WKMediaPlaybackStateExtensions.ToNative (managed), Is.EqualTo ((nuint) native), "ToNative");
			Assert.That (WKMediaPlaybackStateExtensions.ToManaged ((nuint) native), Is.EqualTo (managed), "ToManaged");
		}

		[Test]
		public void MediaPlaybackStateValues ()
		{
			TestRuntime.AssertXcodeVersion (12, 5);
#if XAMCORE_5_0
			Assert.That ((ulong) WKMediaPlaybackState.Playing, Is.EqualTo (1), "Playing");
			Assert.That ((ulong) WKMediaPlaybackState.Paused, Is.EqualTo (2), "Paused");
			Assert.That ((ulong) WKMediaPlaybackState.Suspended, Is.EqualTo (3), "Suspended");
#else
			Assert.That ((ulong) WKMediaPlaybackState.Playing, Is.EqualTo (3), "Legacy Playing");
			Assert.That ((ulong) WKMediaPlaybackState.Paused, Is.EqualTo (1), "Legacy Paused");
			Assert.That ((ulong) WKMediaPlaybackState.Suspended, Is.EqualTo (2), "Legacy Suspended");
#endif
		}
#endif
	}
}
