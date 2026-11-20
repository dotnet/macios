#if __MACOS__
using System.Threading;

using CoreGraphics;
using CoreVideo;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CVDisplayLinkTest {
		[Test]
		public void OutputCallbackInputs ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.IgnoreIfLockedScreen ();
			using var displayLink = new CVDisplayLink ();
			var now = new CVTimeStamp { VideoTimeScale = 600, VideoTime = 1234, HostTime = 5678 };
			var output = new CVTimeStamp { VideoTimeScale = 600, VideoTime = 1244, HostTime = 5688 };
			var expectedNow = now;
			var expectedOutput = output;
			var flagsOut = CVOptionFlags.None;
#if XAMCORE_5_0
			CVDisplayLink.DisplayLinkOutputCallback callback = (CVDisplayLink link, in CVTimeStamp current, in CVTimeStamp next, CVOptionFlags flags, ref CVOptionFlags resultFlags) => {
#else
			CVDisplayLink.DisplayLinkOutputCallback callback = (CVDisplayLink link, ref CVTimeStamp current, ref CVTimeStamp next, CVOptionFlags flags, ref CVOptionFlags resultFlags) => {
#endif
				Assert.That (link, Is.SameAs (displayLink), "Display link");
				Assert.That (current, Is.EqualTo (expectedNow), "Current time");
				Assert.That (next, Is.EqualTo (expectedOutput), "Output time");
				Assert.That (flags, Is.EqualTo ((CVOptionFlags) 1), "Input flags");
				resultFlags = (CVOptionFlags) 2;
				return CVReturn.Success;
			};
#if XAMCORE_5_0
			var status = callback (displayLink, in now, in output, (CVOptionFlags) 1, ref flagsOut);
#else
			var status = callback (displayLink, ref now, ref output, (CVOptionFlags) 1, ref flagsOut);
#endif
			Assert.That (status, Is.EqualTo (CVReturn.Success), "Result");
			Assert.That (flagsOut, Is.EqualTo ((CVOptionFlags) 2), "Output flags");
			Assert.That (now, Is.EqualTo (expectedNow), "Current input");
			Assert.That (output, Is.EqualTo (expectedOutput), "Output input");
		}

		[Test]
		public void NativeOutputCallback ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.IgnoreIfLockedScreen ();
			using var displayLink = new CVDisplayLink ();
			using var called = new ManualResetEventSlim ();
			CVTimeStamp receivedNow = default;
			CVTimeStamp receivedOutput = default;
			IntPtr receivedLink = IntPtr.Zero;
			int callbackCount = 0;
#if XAMCORE_5_0
			CVDisplayLink.DisplayLinkOutputCallback callback = (CVDisplayLink link, in CVTimeStamp current, in CVTimeStamp next, CVOptionFlags flags, ref CVOptionFlags resultFlags) => {
#else
			CVDisplayLink.DisplayLinkOutputCallback callback = (CVDisplayLink link, ref CVTimeStamp current, ref CVTimeStamp next, CVOptionFlags flags, ref CVOptionFlags resultFlags) => {
#endif
				resultFlags = CVOptionFlags.None;
				if (Interlocked.Increment (ref callbackCount) == 1) {
					receivedNow = current;
					receivedOutput = next;
					receivedLink = link.Handle;
					called.Set ();
				}
				return CVReturn.Success;
			};
			Assert.That (displayLink.SetOutputCallback (callback), Is.EqualTo (CVReturn.Success), "Set callback");
			Assert.That (displayLink.Start (), Is.EqualTo (CVReturn.Success), "Start");
			try {
				Assert.That (called.Wait (TimeSpan.FromSeconds (5)), Is.True, "Called");
				Assert.That (receivedLink, Is.EqualTo ((IntPtr) displayLink.Handle), "Native display link");
				Assert.That (receivedNow.VideoTimeScale, Is.GreaterThan (0), "Current time scale");
				Assert.That (receivedNow.HostTime, Is.GreaterThan (0), "Current host time");
				Assert.That (receivedOutput.VideoTimeScale, Is.GreaterThan (0), "Output time scale");
				Assert.That (receivedOutput.HostTime, Is.GreaterThan (0), "Output host time");
			} finally {
				displayLink.Stop ();
			}
		}

		[Test]
		public void CreateFromDisplayIdValidIdTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			Assert.DoesNotThrow (() => {
				using var displayLink = CVDisplayLink.CreateFromDisplayId ((uint) CGDisplay.MainDisplayID);
				Assert.That (displayLink, Is.Not.Null, "Not null");
				Assert.That (displayLink.GetCurrentDisplay (), Is.EqualTo (CGDisplay.MainDisplayID), "DisplayId");
			}, "Throws");
		}

		[Test]
		public void CreateFromDisplayWrongIdTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			Assert.DoesNotThrow (() => {
				using var displayLink = CVDisplayLink.CreateFromDisplayId (UInt32.MaxValue);
				Assert.That (displayLink, Is.Null, "null");
			}, "Throws");
		}

		[Test]
		public void CreateFromDisplayIdsTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			// we might not have more than one display, therefore we will use an array 
			// with a single one, there is nothing in the docs that say that we cannot do that
			Assert.DoesNotThrow (() => {
				using var displayLink = CVDisplayLink.CreateFromDisplayIds (new [] { (uint) CGDisplay.MainDisplayID });
				Assert.That (displayLink, Is.Not.Null, "Not null");
			}, "Throws");
		}

		[Test]
		public void CreateFromOpenGLMaskTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			var openGLMask = CGDisplay.GetOpenGLDisplayMask (CGDisplay.MainDisplayID);
			Assert.DoesNotThrow (() => {
				using var displayLink = CVDisplayLink.CreateFromOpenGLMask ((uint) openGLMask);
				Assert.That (displayLink, Is.Not.Null, "Not null");
			}, "Throws");
		}

		[Test]
		public void DefaultConstructorTest ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.IgnoreIfLockedScreen ();
			Assert.DoesNotThrow (() => {
				using var displayLink = new CVDisplayLink ();
			});
		}

		[Test]
		public void SetCurrentDisplayOpenGLTest ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.IgnoreIfLockedScreen ();
			Assert.DoesNotThrow (() => {
				using var displayLink = new CVDisplayLink ();
				displayLink.SetCurrentDisplay (CGDisplay.MainDisplayID);
			});
		}

		[Test]
		public void GetCurrentDisplayTest ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.IgnoreIfLockedScreen ();
			Assert.DoesNotThrow (() => {
				using var displayLink = new CVDisplayLink ();
				Assert.That (displayLink.GetCurrentDisplay (), Is.EqualTo (CGDisplay.MainDisplayID));
			});
		}

		[Test]
		public void GetTypeIdTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			Assert.DoesNotThrow (() => {
				CVDisplayLink.GetTypeId ();
			}, "Throws");
		}

		[Test]
		public void TryTranslateTimeValidTest ()
		{
			TestRuntime.AssertNotVSTS ();
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 12, 0);
			TestRuntime.IgnoreIfLockedScreen ();
			var outTime = new CVTimeStamp {
				Version = 0,
				Flags = (1L << 0) | (1L << 1), // kCVTimeStampVideoTimeValid | kCVTimeStampHostTimeValid
			};
			using var displayLink = new CVDisplayLink ();
			// it has to be running else you will get a crash
			if (displayLink.Start () == 0) {
				displayLink.GetCurrentTime (out var timeStamp);
				Assert.That (displayLink.TryTranslateTime (timeStamp, ref outTime), Is.True);
				displayLink.Stop ();
			}
		}
	}
}
#endif
