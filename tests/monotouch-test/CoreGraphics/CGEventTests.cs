using System.Diagnostics;
using System.Text;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGEventTests {
#if MONOMAC
		bool tapCalled = false;

		IntPtr callBack (IntPtr tapProxyEvent, CGEventType eventType,
			IntPtr eventRef, IntPtr userInfo)
		{
			tapCalled = true;
			return eventRef;
		}

		[Test]
		public void CreateTap ()
		{
			tapCalled = false;
			using var tapPort = CGEvent.CreateTap (CGEventTapLocation.AnnotatedSession, CGEventTapPlacement.HeadInsert, CGEventTapOptions.Default, CGEventMask.KeyDown, callBack, IntPtr.Zero);
			Assert.That (tapCalled, Is.False, "tap was mistakenly called.");
		}

		[Test]
		public void CreateTapPSN ()
		{
			tapCalled = false;
			var psn = (IntPtr) 2; // kCurrentProcess
			using var tapPort = CGEvent.CreateTap (psn, CGEventTapPlacement.HeadInsert, CGEventTapOptions.Default, CGEventMask.KeyDown, callBack, IntPtr.Zero);
			Assert.That (tapCalled, Is.False, "tap was mistakenly called.");
		}
#endif

#if __MACOS__ || __MACCATALYST__
		[Test]
		public void Constructor_CGEventSourceStateID_0 ()
		{
			var ex = Assert.Throws<ArgumentException> (() => new CGEvent (null, CGScrollEventUnit.Pixel), "ArgumentException");
			Assert.That (ex.Message, Is.EqualTo ("At least one wheel must be provided"), "Message");
		}

		[Test]
		public void Constructor_CGEventSourceStateID_1 ()
		{
			using var evt = new CGEvent (null, CGScrollEventUnit.Pixel, 0);
			Assert.That (evt.EventType, Is.EqualTo (CGEventType.ScrollWheel), "EventType");
			Assert.That (evt.Timestamp, Is.EqualTo (0), "Timestamp");
			// There doesn't seem to be any way to validate any creation
			// arguments, except using CGEvent.ToData which returns an opaque
			// byte array. Unfortunately the byte array changes randomly
			// (timestamps in it maybe?), so it's not reliable enough for a
			// test.
		}

		[Test]
		public void Constructor_CGEventSourceStateID_2 ()
		{
			using var evt = new CGEvent (null, CGScrollEventUnit.Pixel, 0, 3);
			Assert.That (evt.EventType, Is.EqualTo (CGEventType.ScrollWheel), "EventType");
			Assert.That (evt.Timestamp, Is.EqualTo (0), "Timestamp");
			// There doesn't seem to be any way to validate any creation
			// arguments, except using CGEvent.ToData which returns an opaque
			// byte array. Unfortunately the byte array changes randomly
			// (timestamps in it maybe?), so it's not reliable enough for a
			// test.
		}

		[Test]
		public void Constructor_CGEventSourceStateID_3 ()
		{
			using var evt = new CGEvent (null, CGScrollEventUnit.Pixel, 0, 3, 9);
			Assert.That (evt.EventType, Is.EqualTo (CGEventType.ScrollWheel), "EventType");
			Assert.That (evt.Timestamp, Is.EqualTo (0), "Timestamp");
			// There doesn't seem to be any way to validate any creation
			// arguments, except using CGEvent.ToData which returns an opaque
			// byte array. Unfortunately the byte array changes randomly
			// (timestamps in it maybe?), so it's not reliable enough for a
			// test.
		}

		[Test]
		public void Constructor_CGEventSourceStateID_4 ()
		{
			var ex = Assert.Throws<ArgumentException> (() => new CGEvent (null, CGScrollEventUnit.Pixel, 0, 3, 9, 42), "ArgumentException");
			Assert.That (ex.Message, Is.EqualTo ("Only one to three wheels are supported on this constructor"), "Message");
		}

		public void PostToPid ()
		{
			var pid = Process.GetCurrentProcess ().Id;
			using var evt = new CGEvent (null, (ushort) 1, true);
			evt.PostToPid (pid);
			CGEvent.PostToPid (evt, pid);
		}

		[Test]
		public void PostToPSN ()
		{
			var pid = Process.GetCurrentProcess ().Id;
			Assert.That (GetProcessForPID (pid, out var psn), Is.EqualTo (0), "GetProcessForPID");
			using var evt = new CGEvent (null, (ushort) 1, true);
			unsafe {
				IntPtr* psnPtr = &psn;
				evt.PostToPSN ((IntPtr) psnPtr);
				CGEvent.PostToPSN (evt, (IntPtr) psnPtr);
			}
		}

		[DllImport ("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/ApplicationServices")]
		static extern int GetProcessForPID (int pid, out IntPtr psn);

		[Test]
		public void ToData ()
		{
			using var evt = new CGEvent (null, CGScrollEventUnit.Pixel, 0);
			using var data = evt.ToData ();
			Assert.That (data, Is.Not.Null, "data");
			Assert.That (data.Length, Is.GreaterThan ((nuint) 0), "Length");
		}
#endif // __MACOS__ || __MACCATALYST__
	}
}
