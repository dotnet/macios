//
// CGImageMetadata
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System;
#if XAMCORE_2_0
using Foundation;
using ImageIO;
using ObjCRuntime;
#else
using MonoTouch.Foundation;
using MonoTouch.ImageIO;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
#endif
using NUnit.Framework;

namespace MonoTouchFixtures.ImageIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ImageMetadataTest {

		NSString nspace = CGImageMetadataTagNamespaces.Exif;
		NSString prefix = CGImageMetadataTagPrefixes.Exif;
		NSString name = new NSString ("tagName");
		NSString path = new NSString ("exif:Flash.Fired");

		[Test]
		public void Defaults ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			Assert.Throws<ArgumentNullException> (delegate { new CGImageMetadata (null); }, "null");

			using (var mutable = new CGMutableImageMetadata ())
			using (var tag = new CGImageMetadataTag (nspace, prefix, name, CGImageMetadataType.Default, true)) {
				mutable.SetTag (null, path, tag);

				using (var meta = new CGImageMetadata (mutable.CreateXMPData ())) {
					// not surprising since it's all empty
					Assert.Null (meta.CopyTagMatchingImageProperty (CGImageProperties.ExifDictionary, CGImageProperties.ExifDateTimeOriginal), "CopyTagMatchingImageProperty");
				}
			}
		}
	}
}
