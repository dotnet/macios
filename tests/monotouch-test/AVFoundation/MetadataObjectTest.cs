//
// Unit tests for AVMetadataObject
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__TVOS__

using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MetadataObjectTest {

		[Test]
		public void Defaults ()
		{
			TestRuntime.AssertXcodeVersion (6, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);

			using (var obj = new AVMetadataFaceObject ()) {
				Assert.That (obj.FaceID, Is.EqualTo ((nint) 0), "FaceID");
				Assert.That (obj.HasRollAngle, Is.EqualTo (false), "HasRollAngle");
				Assert.That (obj.HasYawAngle, Is.EqualTo (false), "HasYawAngle");
#if !MONOMAC // No Type property for Mac
				Assert.That (obj.Type, Is.EqualTo (AVMetadataObjectType.Face), "Type");
#endif
			}

#if !MONOMAC // iOS only
			using (var obj = new AVMetadataMachineReadableCodeObject ()) {
				Assert.That (obj.Corners, Is.Not.Null, "Corners");
				Assert.That (obj.Corners.Length, Is.EqualTo (0), "Corners");
				Assert.That (obj.StringValue, Is.Null, "StringValue");
				Assert.That (obj.Type, Is.EqualTo (AVMetadataObjectType.None), "Type");
				Assert.That (obj.WeakType, Is.Null, "WeakType");
			}
#endif
		}
	}
}

#endif // !__TVOS__
