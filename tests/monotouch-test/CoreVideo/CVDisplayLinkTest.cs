#if __MACOS__
using CoreGraphics;
using CoreVideo;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CVDisplayLinkTest {
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
