// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !__TVOS__

using System.Runtime.InteropServices;
using CoreGraphics;
#if __MACOS__
using AppKit;
#endif
using WebKit;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class EnumConversionTest {
#if __MACOS__
		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern nuint GetNativeValue (IntPtr handle, IntPtr selector);

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern void SetNativeValue (IntPtr handle, IntPtr selector, nuint value);

#if !XAMCORE_5_0
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
#endif

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
			Assert.That (NSImageResizingModeExtensions.ToNative (managed), Is.EqualTo (native), "ToNative");
			Assert.That (NSImageResizingModeExtensions.ToManaged (native), Is.EqualTo (managed), "ToManaged");
#endif
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
			Assert.That (NSTextAlignmentExtensions.ToNative (managed), Is.EqualTo (native), "ToNative");
			Assert.That (NSTextAlignmentExtensions.ToManaged (native), Is.EqualTo (managed), "ToManaged");
#endif
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
#if !XAMCORE_5_0
			Assert.That (NSTickMarkPositionExtensions.ToNative (managed), Is.EqualTo ((nuint) native), "ToNative");
			Assert.That (NSTickMarkPositionExtensions.ToManaged ((nuint) native), Is.EqualTo ((NSTickMarkPosition) native), "Canonical ToManaged");
#endif
			using var slider = new NSSlider ();
			slider.TickMarkPosition = managed;
			Assert.That (GetNativeValue (slider.Handle, Selector.GetHandle ("tickMarkPosition")), Is.EqualTo ((nuint) native), "Native getter");
			Assert.That (slider.TickMarkPosition, Is.EqualTo ((NSTickMarkPosition) native), "Canonical managed getter");
		}
#endif

#if !XAMCORE_5_0
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
#endif

		[TestCase (1, WKMediaPlaybackState.Playing)]
		[TestCase (2, WKMediaPlaybackState.Paused)]
		[TestCase (3, WKMediaPlaybackState.Suspended)]
		public void MediaPlaybackStateCompletion (int native, WKMediaPlaybackState expected)
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using var configuration = new WKWebViewConfiguration ();
			using var view = new MediaPlaybackStateWebView (configuration);
			view.NativeState = (nuint) native;
			WKMediaPlaybackState? actual = null;
			view.RequestState (state => actual = state);
			Assert.That (actual, Is.EqualTo (expected), "Completion handler state");
		}

		[Preserve (AllMembers = true)]
		class MediaPlaybackStateWebView : WKWebView {
			public nuint NativeState { get; set; }

			public MediaPlaybackStateWebView (WKWebViewConfiguration configuration)
				: base (CGRect.Empty, configuration)
			{
			}

			public void RequestState (Action<WKMediaPlaybackState> completion)
			{
				// Dispatch through the generated binding to our raw native completion-block implementation.
				IsDirectBinding = true;
				try {
					base.RequestMediaPlaybackState (completion);
				} finally {
					IsDirectBinding = false;
				}
			}

			[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
			delegate void NativeCompletion (IntPtr block, nuint state);

			[Export ("requestMediaPlaybackStateWithCompletionHandler:")]
			public unsafe void RequestNativeState (IntPtr completion)
			{
				var block = (BlockLiteral*) completion;
				block->GetDelegateForBlock<NativeCompletion> () (completion, NativeState);
			}
		}
	}
}

#endif
