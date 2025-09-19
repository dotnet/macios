// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using NUnit.Framework;

#if HAS_UIKIT
using UIKit;
#endif

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGShadingTest {
#if HAS_UIKIT
		[Test]
		public void CreateAxialWithContentHeadroom ()
		{
			CreateShadingWithContentHeadroomTest ((size, hdrCapableColorspace, function) => {
				var start = new CGPoint (0, 0);
				var end = new CGPoint (size.Height, size.Width);

				return CGShading.CreateAxial (0.5f, hdrCapableColorspace, start, end, function, true, false);
			});
		}

		[Test]
		public void CreateRadialWithContentHeadroom ()
		{
			CreateShadingWithContentHeadroomTest ((size, hdrCapableColorspace, function) => {
				var start = new CGPoint (0, 0);
				var startRadius = 16.0f;
				var end = new CGPoint (size.Height, size.Width);
				var endRadius = 32.0f;

				return CGShading.CreateRadial (0.5f, hdrCapableColorspace, start, startRadius, end, endRadius, function, true, false);
			});
		}

		void CreateShadingWithContentHeadroomTest (Func<CGSize, CGColorSpace, CGFunction, CGShading> createShading)
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var size = new CGSize (128, 128);
			var functionCalled = false;
			Exception? ex = null;

			using var renderer = new UIGraphicsImageRenderer (size);
			using var img = renderer.CreateImage ((context) => {
				try {
					using var hdrCapableColorspace = CGColorSpace.CreateWithName (CGColorSpaceNames.DisplayP3_PQ);
					Assert.IsTrue (hdrCapableColorspace.IsHdr, "IsHdr");

					using var slopedFunction = FunctionTest.CreateSlopedFunction (() => functionCalled = true, 1, hdrCapableColorspace.Components + 1);

					using var shading = createShading (size, hdrCapableColorspace, slopedFunction);
					context.CGContext.DrawShading (shading);
				} catch (Exception e) {
					ex = e;
				}
			});

			Assert.IsTrue (functionCalled, "Function called");
			Assert.IsNull (ex, "Exception");
		}
#endif
	}
}
