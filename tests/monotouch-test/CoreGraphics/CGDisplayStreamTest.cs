// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using System;
using CoreFoundation;
using CoreGraphics;
using CoreVideo;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGDisplayStreamTest {
		[Test]
		public void TypeIdentifiers ()
		{
			Assert.That (CGDisplayStream.GetTypeId (), Is.Not.EqualTo (0), "Stream");
			Assert.That (CGDisplayStreamUpdate.GetTypeId (), Is.Not.EqualTo (0), "Update");
			Assert.That (CGDisplayStream.GetTypeId (), Is.Not.EqualTo (CGDisplayStreamUpdate.GetTypeId ()), "Distinct types");
		}

		[Test]
		public void MergeNullUpdates ()
		{
			using var update = CGDisplayStreamUpdate.CreateMergedUpdate (null, null);
			Assert.That (update, Is.Null);
		}

		[Test]
		public void InvalidDimensions ()
		{
			Assert.Throws<ArgumentOutOfRangeException> (() => CGDisplayStream.Create (0, 0, 1, CVPixelFormatType.CV32BGRA, null, null), "Width");
			Assert.Throws<ArgumentOutOfRangeException> (() => CGDisplayStream.Create (0, 1, 0, CVPixelFormatType.CV32BGRA, null, null), "Height");
			using var queue = new DispatchQueue ("CGDisplayStreamTest");
			Assert.Throws<ArgumentOutOfRangeException> (() => CGDisplayStream.Create (0, 0, 1, CVPixelFormatType.CV32BGRA, null, queue, null), "Queue width");
			Assert.Throws<ArgumentOutOfRangeException> (() => CGDisplayStream.Create (0, 1, 0, CVPixelFormatType.CV32BGRA, null, queue, null), "Queue height");
		}

		[Test]
		public void NullQueue ()
		{
			Assert.Throws<ArgumentNullException> (() => CGDisplayStream.Create (0, 1, 1, CVPixelFormatType.CV32BGRA, null, null, null));
		}
	}
}

#endif // __MACOS__ || __MACCATALYST__
