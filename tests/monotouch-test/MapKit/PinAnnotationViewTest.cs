//
// MKPinAnnotationView Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012, 2015 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !__WATCHOS__

using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using MapKit;
using ObjCRuntime;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using NUnit.Framework;

namespace MonoTouchFixtures.MapKit {
	
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PinAnnotationViewTest {
		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 9, throwIfOtherPlatform: false);
		}
		
		[Test]
		public void Ctor_Annotation ()
		{
			using (var a = new MKPolyline ())
			using (MKPinAnnotationView av = new MKPinAnnotationView (a, "reuse")) {
				Assert.AreSame (a, av.Annotation, "Annotation");

#if !MONOMAC
				if (TestRuntime.CheckSystemVersion (PlatformName.iOS, 7, 0)) // Crashes with EXC_BAD_ACCESS (SIGABRT) if < iOS 7.0
					Assert.False (av.AnimatesDrop, "AnimatesDrop");

				if (!TestRuntime.CheckSystemVersion (PlatformName.iOS, 9, 0))
					return;
#endif

				Assert.That (av.PinColor, Is.EqualTo (MKPinAnnotationColor.Red), "PinColor");

				if (TestRuntime.CheckXcodeVersion (7, 0))
					Assert.NotNull (av.PinTintColor, "PinTintColor");
			}
		}

		[Test]
		public void InitWithFrame ()
		{
#if !MONOMAC
			// Crashes with EXC_BAD_ACCESS (SIGABRT) if < iOS 7.0
			TestRuntime.AssertSystemVersion (PlatformName.iOS, 7, 0, throwIfOtherPlatform: false);
#endif

			var frame = new CGRect (10, 10, 100, 100);
			using (var av = new MKPinAnnotationView (frame)) {
				// broke in xcode 12 beta 1
				if (!TestRuntime.CheckXcodeVersion (12, 0))
					Assert.That (av.Frame.ToString (), Is.EqualTo (frame.ToString ()), "Frame"); // fp comparison fails
				Assert.Null (av.Annotation, "Annotation");
				Assert.False (av.AnimatesDrop, "AnimatesDrop");

				if (!TestRuntime.CheckXcodeVersion (7, 0))
					return;
				
				Assert.That (av.PinColor, Is.EqualTo (MKPinAnnotationColor.Red), "PinColor");
#if MONOMAC
				if (TestRuntime.CheckSystemVersion (PlatformName.MacOSX, 10, 12)) {
					Assert.That (av.PinTintColor, Is.EqualTo (NSColor.SystemRedColor), "PinTintColor");
				} else {
					Assert.Null (av.PinTintColor, "PinTintColor"); // differs from the other init call
				}
#else
				bool not_null = TestRuntime.CheckSystemVersion (PlatformName.iOS, 10, 0);
				if (not_null && TestRuntime.CheckSystemVersion (PlatformName.iOS, 14, 0))
					not_null = (Runtime.Arch == Arch.DEVICE);
				if (not_null)
					Assert.NotNull (av.PinTintColor, "PinTintColor");
				else
					Assert.Null (av.PinTintColor, "PinTintColor"); // differs from the other init call
#endif
			}
		}
	}
}

#endif // !__TVOS__ && !__WATCHOS__
