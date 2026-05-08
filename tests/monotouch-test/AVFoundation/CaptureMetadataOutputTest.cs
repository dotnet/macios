//
// Unit tests for AVMetadataObject
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using System.Reflection;
using CoreGraphics;
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CaptureMetadataOutputTest {

		[Test]
		public void Defaults ()
		{
			using (var obj = new AVCaptureMetadataOutput ()) {
				Assert.That (obj.AvailableMetadataObjectTypes, Is.EqualTo (AVMetadataObjectType.None), "AvailableMetadataObjectTypes");
				Assert.That (obj.MetadataObjectTypes, Is.EqualTo (AVMetadataObjectType.None), "MetadataObjectTypes");

				Assert.That (obj.WeakAvailableMetadataObjectTypes, Is.Not.Null, "WeakAvailableMetadataObjectTypes");
				Assert.That (obj.WeakAvailableMetadataObjectTypes.Length, Is.EqualTo (0), "WeakAvailableMetadataObjectTypes#");
				Assert.That (obj.WeakMetadataObjectTypes, Is.Not.Null, "WeakMetadataObjectTypes");
				Assert.That (obj.WeakMetadataObjectTypes.Length, Is.EqualTo (0), "WeakMetadataObjectTypes#");
				if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false))
					Assert.That (obj.RectOfInterest, Is.EqualTo (new CGRect (0, 0, 1, 1)), "RectOfInterest");

				if (TestRuntime.CheckXcodeVersion (13, 0)) {
					obj.WeakMetadataObjectTypes = null;
					Assert.That (obj.MetadataObjectTypes, Is.EqualTo (AVMetadataObjectType.None), "MetadataObjectTypes");
					obj.MetadataObjectTypes = AVMetadataObjectType.None;
					Assert.That (obj.MetadataObjectTypes, Is.EqualTo (AVMetadataObjectType.None), "MetadataObjectTypes");
					obj.SetDelegate (null, null);
				}
			}
		}

		[Test]
		public void Flags ()
		{
			// single
			var flags = AVMetadataObjectType.Face;
			var result = AVMetadataObjectTypeExtensions.ToFlags (new NSString [] { flags.GetConstant () });
			Assert.That (result, Is.EqualTo (flags), "a2e 1");

			var back = result.ToArray ();
			Assert.That (back.Length, Is.EqualTo (1), "l 1");
			Assert.That (back [0], Is.EqualTo (flags.GetConstant ()), "e2a 1");

			// constants are only available in recent xcode (and not on any 32bits OS)
			if (TestRuntime.CheckXcodeVersion (11, 0)) {
				// multiple (flags)
				flags = AVMetadataObjectType.CatBody | AVMetadataObjectType.DogBody | AVMetadataObjectType.HumanBody;
				var array = new NSString [] {
					AVMetadataObjectType.CatBody.GetConstant (),
					AVMetadataObjectType.DogBody.GetConstant (),
					AVMetadataObjectType.HumanBody.GetConstant ()
				};
				result = AVMetadataObjectTypeExtensions.ToFlags (array);
				Assert.That (result, Is.EqualTo (flags), "a2e 3");
				back = result.ToArray ();
				Assert.That (back.Length, Is.EqualTo (3), "l 3");
				Assert.That (back [0], Is.EqualTo (array [0]), "e2a 3a");
				Assert.That (back [1], Is.EqualTo (array [1]), "e2a 3b");
				Assert.That (back [2], Is.EqualTo (array [2]), "e2a 3c");
			}

			var all = (AVMetadataObjectType) ulong.MaxValue;
			var someArray = all.ToArray (); // converting all flags to an array will only return strings for flags that exist in the current OS.
			Assert.That (someArray.Length, Is.GreaterThan (1), "some back");
			var someFlags = AVMetadataObjectTypeExtensions.ToFlags (someArray);
			Assert.That (someFlags, Is.Not.EqualTo (AVMetadataObjectType.None), "Some, but not None");
			Assert.That (someFlags, Is.Not.EqualTo (all), "Some, but not all");
		}

		[Test]
		public void MetadataObjectTypesTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertDevice ("This test only runs on device (requires camera access)");
			TestRuntime.RequestCameraPermission (AVMediaTypes.Video.GetConstant (), true);

			using (var captureSession = new AVCaptureSession ()) {
				using (var videoDevice = AVCaptureDevice.GetDefaultDevice (AVMediaTypes.Video.GetConstant ())) {

					NSError error;
					using (var videoInput = new AVCaptureDeviceInput (videoDevice, out error)) {
						if (captureSession.CanAddInput (videoInput))
							captureSession.AddInput (videoInput);

						using (var metadataOutput = new AVCaptureMetadataOutput ()) {

							if (captureSession.CanAddOutput (metadataOutput))
								captureSession.AddOutput (metadataOutput);

							AVMetadataObjectType all = AVMetadataObjectType.None;
							foreach (var val in Enum.GetValues<AVMetadataObjectType> ()) {
								switch (val) {
								case AVMetadataObjectType.CatBody:
								case AVMetadataObjectType.DogBody:
								case AVMetadataObjectType.HumanBody:
								case AVMetadataObjectType.SalientObject:
									break;
								case AVMetadataObjectType.CodabarCode:
								case AVMetadataObjectType.GS1DataBarCode:
								case AVMetadataObjectType.GS1DataBarExpandedCode:
								case AVMetadataObjectType.GS1DataBarLimitedCode:
								case AVMetadataObjectType.MicroQRCode:
								case AVMetadataObjectType.MicroPdf417Code:
									if (!TestRuntime.CheckXcodeVersion (13, 3))
										continue;
									break;
								case AVMetadataObjectType.CatHead:
								case AVMetadataObjectType.DogHead:
									if (!TestRuntime.CheckXcodeVersion (26, 0))
										continue;
									break;
								}
								metadataOutput.MetadataObjectTypes = val;
								all |= val;
								Assert.That (metadataOutput.MetadataObjectTypes, Is.EqualTo (val), val.ToString ());
							}
							metadataOutput.MetadataObjectTypes = all;
							Assert.That (metadataOutput.MetadataObjectTypes, Is.EqualTo (all), all.ToString ());
						}
					}
				}
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
