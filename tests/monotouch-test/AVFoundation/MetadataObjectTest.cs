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
				ClassicAssert.AreEqual ((nint) 0, obj.FaceID, "FaceID");
				ClassicAssert.AreEqual (false, obj.HasRollAngle, "HasRollAngle");
				ClassicAssert.AreEqual (false, obj.HasYawAngle, "HasYawAngle");
#if !MONOMAC // No Type property for Mac
				ClassicAssert.AreEqual (AVMetadataObjectType.Face, obj.Type, "Type");
#endif
			}

#if !MONOMAC // iOS only
			using (var obj = new AVMetadataMachineReadableCodeObject ()) {
				ClassicAssert.IsNotNull (obj.Corners, "Corners");
				ClassicAssert.AreEqual (0, obj.Corners.Length, "Corners");
				ClassicAssert.IsNull (obj.StringValue, "StringValue");
				ClassicAssert.AreEqual (AVMetadataObjectType.None, obj.Type, "Type");
				ClassicAssert.IsNull (obj.WeakType, "WeakType");
			}
#endif
		}
	}
}

#endif // !__TVOS__
