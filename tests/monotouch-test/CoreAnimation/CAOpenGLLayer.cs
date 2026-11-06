#if __MACOS__

using AppKit;
using CoreAnimation;
using CoreVideo;
using OpenGL;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CAOpenGLLayerTest {
		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
		static extern byte CanDraw (IntPtr layer, IntPtr selector, IntPtr context, IntPtr pixelFormat, double time, ref CVTimeStamp timestamp);

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
		static extern void Draw (IntPtr layer, IntPtr selector, IntPtr context, IntPtr pixelFormat, double time, ref CVTimeStamp timestamp);

		static void CheckDisplayTime (CALayer layer, string canDrawSelector, string drawSelector, Func<(CVTimeStamp Timestamp, double Time)> received)
		{
			var timestamp = new CVTimeStamp { VideoTimeScale = 600, VideoTime = 1234, HostTime = 5678, Flags = 3 };
			var expected = timestamp;
			Assert.That (CanDraw (layer.Handle, Selector.GetHandle (canDrawSelector), IntPtr.Zero, IntPtr.Zero, 3, ref timestamp), Is.EqualTo (1), "CanDraw");
			Assert.That (received ().Timestamp, Is.EqualTo (expected), "CanDraw timestamp");
			Assert.That (received ().Time, Is.EqualTo (3), "CanDraw time");
			Assert.That (timestamp, Is.EqualTo (expected), "CanDraw input");

			timestamp.VideoTime += 10;
			expected = timestamp;
			Draw (layer.Handle, Selector.GetHandle (drawSelector), IntPtr.Zero, IntPtr.Zero, 4, ref timestamp);
			Assert.That (received ().Timestamp, Is.EqualTo (expected), "Draw timestamp");
			Assert.That (received ().Time, Is.EqualTo (4), "Draw time");
			Assert.That (timestamp, Is.EqualTo (expected), "Draw input");
		}

		[Test]
		public void CGLDisplayTimeInputs ()
		{
			using var layer = new TimestampOpenGLLayer ();
			CheckDisplayTime (layer,
				"canDrawInCGLContext:pixelFormat:forLayerTime:displayTime:",
				"drawInCGLContext:pixelFormat:forLayerTime:displayTime:",
				() => (layer.Timestamp, layer.Time));
		}

		[Test]
		public void OpenGLDisplayTimeInputs ()
		{
			using var layer = new TimestampNSOpenGLLayer ();
			CheckDisplayTime (layer,
				"canDrawInOpenGLContext:pixelFormat:forLayerTime:displayTime:",
				"drawInOpenGLContext:pixelFormat:forLayerTime:displayTime:",
				() => (layer.Timestamp, layer.Time));
		}

#if !DYNAMIC_REGISTRAR
		[Ignore ("https://github.com/dotnet/macios/issues/10284")]
#endif
		[Test]
		public void SubclassedTest ()
		{
			TestRuntime.AssertNotVirtualMachine ();

			using (var layer = new OpenGLLayer ()) {
				Messaging.IntPtr_objc_msgSend (layer.Handle, Selector.GetHandle ("copyCGLPixelFormatForDisplayMask:"));
			}
		}
	}

	[Preserve (AllMembers = true)]
	public class TimestampOpenGLLayer : CAOpenGLLayer {
		public CVTimeStamp Timestamp;
		public double Time;

#if XAMCORE_5_0
		public override bool CanDrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, in CVTimeStamp timeStamp)
#else
		public override bool CanDrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp)
#endif
		{
			Timestamp = timeStamp;
			Time = timeInterval;
			return true;
		}

#if XAMCORE_5_0
		public override void DrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, in CVTimeStamp timeStamp)
#else
		public override void DrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp)
#endif
		{
			Timestamp = timeStamp;
			Time = timeInterval;
		}
	}

	[Preserve (AllMembers = true)]
	public class TimestampNSOpenGLLayer : NSOpenGLLayer {
		public CVTimeStamp Timestamp;
		public double Time;

#if XAMCORE_5_0
		public override bool CanDraw (NSOpenGLContext context, NSOpenGLPixelFormat pixelFormat, double t, in CVTimeStamp ts)
#else
		public override bool CanDraw (NSOpenGLContext context, NSOpenGLPixelFormat pixelFormat, double t, ref CVTimeStamp ts)
#endif
		{
			Timestamp = ts;
			Time = t;
			return true;
		}

#if XAMCORE_5_0
		public override void Draw (NSOpenGLContext context, NSOpenGLPixelFormat pixelFormat, double t, in CVTimeStamp ts)
#else
		public override void Draw (NSOpenGLContext context, NSOpenGLPixelFormat pixelFormat, double t, ref CVTimeStamp ts)
#endif
		{
			Timestamp = ts;
			Time = t;
		}
	}

	public partial class OpenGLLayer : CAOpenGLLayer {
		public override CGLPixelFormat CopyCGLPixelFormatForDisplayMask (uint mask)
		{
			var attribs = new object [] {
				CGLPixelFormatAttribute.Accelerated,
				CGLPixelFormatAttribute.DoubleBuffer,
				CGLPixelFormatAttribute.ColorSize, 24,
				CGLPixelFormatAttribute.DepthSize, 16 };

			CGLPixelFormat pixelFormat = new CGLPixelFormat (attribs);
			return pixelFormat;
		}
	}
}
#endif // __MACOS__
