//
// Unit tests for STWebHistory
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

#if HAS_SCREENTIME

using System;

using Foundation;
using ScreenTime;

using NUnit.Framework;

namespace MonoTouchFixtures.ScreenTime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class STWebHistoryTest {

		[Test]
		public void Create_WithBundleIdentifier ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);
			using var obj = STWebHistory.Create ("com.xamarin.monotouch-test", out var error);
			Assert.IsNotNull (obj, "Object");
			Assert.IsNull (error, "Error");
		}

		[Test]
		public void Create_WithBundleIdentifierAndProfile ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);
			using var obj = STWebHistory.Create ("com.xamarin.monotouch-test", (NSString) "profile", out var error);
			Assert.IsNotNull (obj, "Object");
			Assert.IsNull (error, "Error");
		}
	}
}

#endif
