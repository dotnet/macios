//
// CGMutableImageMetadata
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using ImageIO;

using CGImageProperties = ImageIO.CGImageProperties;

namespace MonoTouchFixtures.ImageIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MutableImageMetadataTest {

		NSString nspace = CGImageMetadataTagNamespaces.Exif;
		NSString prefix = CGImageMetadataTagPrefixes.Exif;
		NSString name = new NSString ("tagName");
		NSString path = new NSString ("exif:Flash.Fired");

		[Test]
		public void Defaults ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			using (var meta = new CGMutableImageMetadata ()) {
				Console.WriteLine (meta);
				NSError err;
				Assert.That (meta.RegisterNamespace (CGImageMetadataTagNamespaces.Exif, CGImageMetadataTagPrefixes.Exif, out err), Is.True, "RegisterNamespace");
				Assert.That (err, Is.Null, "NSError");

				// nothing to see at this stage
				using (var data = meta.CreateXMPData ()) {
					Assert.That (data, Is.Null, "CreateXMPData-1");
				}

				using (var tag = new CGImageMetadataTag (nspace, prefix, name, CGImageMetadataType.Default, true)) {
					Assert.That (meta.SetTag (null, path, tag), Is.True, "SetTag");
				}

				// now we're talking
				using (var data = meta.CreateXMPData ()) {
					Assert.That (data, Is.Not.Null, "CreateXMPData-2");
				}

				Assert.That (meta.SetValue (null, path, false), Is.True, "SetValue");

				Assert.That (meta.SetValueMatchingImageProperty (CGImageProperties.ExifDictionary, CGImageProperties.ExifDateTimeOriginal, (NSDate) DateTime.Now), Is.True, "SetValueMatchingImageProperty");
			}
		}
	}
}
