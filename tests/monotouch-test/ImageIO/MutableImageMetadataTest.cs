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
				ClassicAssert.True (meta.RegisterNamespace (CGImageMetadataTagNamespaces.Exif, CGImageMetadataTagPrefixes.Exif, out err), "RegisterNamespace");
				ClassicAssert.Null (err, "NSError");

				// nothing to see at this stage
				using (var data = meta.CreateXMPData ()) {
					ClassicAssert.Null (data, "CreateXMPData-1");
				}

				using (var tag = new CGImageMetadataTag (nspace, prefix, name, CGImageMetadataType.Default, true)) {
					ClassicAssert.True (meta.SetTag (null, path, tag), "SetTag");
				}

				// now we're talking
				using (var data = meta.CreateXMPData ()) {
					ClassicAssert.NotNull (data, "CreateXMPData-2");
				}

				ClassicAssert.True (meta.SetValue (null, path, false), "SetValue");

				ClassicAssert.True (meta.SetValueMatchingImageProperty (CGImageProperties.ExifDictionary, CGImageProperties.ExifDateTimeOriginal, (NSDate) DateTime.Now), "SetValueMatchingImageProperty");
			}
		}
	}
}
