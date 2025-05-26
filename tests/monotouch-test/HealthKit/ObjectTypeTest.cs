//
// Unit tests for HKObjectType
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#if HAS_HEALTHKIT

using System;

using Foundation;
using HealthKit;
using NUnit.Framework;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.HealthKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ObjectTypeTest {

		[Test]
		public void Workout ()
		{
#if MONOMAC
			TestRuntime.AssertXcodeVersion (14, 0);
#else
			TestRuntime.AssertXcodeVersion (6, 0);
#endif


			using (var t = HKObjectType.WorkoutType) {
				Assert.That (t.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}
	}
}
#endif // HAS_HEALTHKIT
