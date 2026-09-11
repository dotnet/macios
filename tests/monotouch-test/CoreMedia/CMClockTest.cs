//
// Unit tests for CMClock
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc All rights reserved.
//

using CoreMedia;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreMedia {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMClockTest {

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static nint CFGetRetainCount (IntPtr handle);

#if !MONOMAC // The CMAudioClockCreate API is only available on iOS
		[Test]
		public void CreateAudioClock ()
		{
			CMClockError ce;
			using (var clock = CMClock.CreateAudioClock (out ce)) {
				if (ce == (CMClockError) (-101))
					TestRuntime.IgnoreInCI ("For unknown reasons, we might get -101 as an error code on the bots sometimes.");
				Assert.That (ce, Is.EqualTo (CMClockError.None));
			}
		}
#endif

		[Test]
		public void HostTimeClock ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var clock = CMClock.HostTimeClock) {
				Assert.That (clock.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
				Assert.That (CFGetRetainCount (clock.Handle), Is.GreaterThanOrEqualTo ((nint) 1), "RetainCount");
			}
		}

		[Test]
		public void PreferredStartTimePattern ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using (var clock = CMClock.HostTimeClock) {
				AssertPreferredStartTimePattern (clock);
			}
		}

		static void AssertPreferredStartTimePattern (CMClock clock)
		{
			var implements = clock.ImplementsGetPreferredStartTimePattern;
			var error = clock.GetPreferredStartTimePattern (out var clockStartTime, out var hostClockStartTime, out var delta);
			TestContext.WriteLine ($"Preferred start-time pattern: supported={implements}, status={error}");
			if (implements) {
				// kCMClockError_PreferredStartTimeNotAvailable is not exposed in CMClockError.
				const CMClockError preferredStartTimeNotAvailable = (CMClockError) (-12758);
				Assert.That (error, Is.EqualTo (CMClockError.None).Or.EqualTo (preferredStartTimeNotAvailable), "Error");
			} else {
				Assert.That (error, Is.EqualTo (CMClockError.UnsupportedOperation), "Error");
			}
			if (error == CMClockError.None) {
				Assert.That (clockStartTime.IsNumeric, Is.True, "ClockStartTime");
				Assert.That (hostClockStartTime.IsNumeric, Is.True, "HostClockStartTime");
				Assert.That (delta.IsNumeric, Is.True, "Delta");
			}
		}

		[Test]
		public void PreferredStartTimePatternDisposed ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using (var clock = CMClock.HostTimeClock) {
				clock.Dispose ();
				Assert.Throws<ObjectDisposedException> (() => { _ = clock.ImplementsGetPreferredStartTimePattern; }, "Implements");
				Assert.Throws<ObjectDisposedException> (() => clock.GetPreferredStartTimePattern (out _, out _, out _), "Pattern");
			}
		}

#if __MACOS__ || __MACCATALYST__
		[Test]
		public void CreateGenlockClock ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using (var clock = CMClock.CreateGenlockClock ()) {
				Assert.That (clock, Is.Not.Null, "Clock");
				Assert.That (clock.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle");
				Assert.That (CFGetRetainCount (clock.Handle), Is.GreaterThanOrEqualTo ((nint) 1), "RetainCount");
				Assert.That (clock.CurrentTime.IsNumeric, Is.True, "CurrentTime");
				AssertPreferredStartTimePattern (clock);
			}
		}

		[Test]
		public void IsAnyDisplaySynchronizedToLockedGenlockSignal ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			// The result depends on connected displays and genlock hardware.
			TestContext.WriteLine ($"Any display synchronized to genlock: {CMClock.IsAnyDisplaySynchronizedToLockedGenlockSignal}");
		}

		[Test]
		public void GenlockNotificationConstants ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			Assert.That (CMClock.DisplayGenlockModeChangedNotification, Is.Not.Null, "Notification");
			Assert.That (CMClock.AnyDisplayIsSynchronizedToLockedGenlockSignalKey, Is.Not.Null, "PayloadKey");
		}
#endif
	}
}
