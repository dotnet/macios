﻿using NUnit.Framework;
using System;

#if !XAMCORE_2_0
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using CGRect = System.Drawing.RectangleF;
#else
using AppKit;
using CoreGraphics;
using ObjCRuntime;
#endif

namespace Xamarin.Mac.Tests
{
	[TestFixture]
	public class NSClipViewTests
	{
		[Test]
		public void NSClipViewConstrainBoundsRect ()
		{
			Asserts.EnsureMavericks ();

			var clipView = new NSClipView (new CGRect (0, 0, 50, 50));
			var rect = clipView.ConstrainBoundsRect (new CGRect (10, 10, 30, 30));

			Assert.IsTrue (rect.X == 0, "NSClipViewConstrainBoundsRect - X value was not 0");
			Assert.IsTrue (rect.Y == 0,  "NSClipViewConstrainBoundsRect - Y value was not 0");
			Assert.IsTrue (rect.Width == 30,  "NSClipViewConstrainBoundsRect - Width value was not 30");
			Assert.IsTrue (rect.Height == 30, "NSClipViewConstrainBoundsRect - Height value was not 30");
		}
	}
}