//
// Unit tests for CMClock
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using CoreMedia;
using Foundation;
using ObjCRuntime;
using NUnit.Framework;
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
				Assert.AreEqual (CMClockError.None, ce);
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
	}
}
