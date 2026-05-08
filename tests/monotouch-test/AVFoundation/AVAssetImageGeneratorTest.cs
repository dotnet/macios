//
// Unit tests for AVAssetImageGenerator
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.IO;
using System.Threading;
using CoreGraphics;
using AVFoundation;
using CoreMedia;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAssetImageGeneratorTest {

		[Test]
		public void Defaults ()
		{
			using (NSUrl video_url = NSUrl.FromFilename (video_asset_path))
			using (AVAsset video_asset = AVAsset.FromUrl (video_url))
			using (AVAssetImageGenerator aig = new AVAssetImageGenerator (video_asset)) {
				ClassicAssert.Null (aig.ApertureMode, "ApertureMode");
				ClassicAssert.False (aig.AppliesPreferredTrackTransform, "AppliesPreferredTrackTransform");
				Assert.That (aig.MaximumSize, Is.EqualTo (CGSize.Empty), "MaximumSize");
				ClassicAssert.True (aig.RequestedTimeToleranceAfter.IsPositiveInfinity, "RequestedTimeToleranceAfter");
				ClassicAssert.True (aig.RequestedTimeToleranceBefore.IsPositiveInfinity, "RequestedTimeToleranceBefore");
			}
		}

		[Test]
		public void AppliesPreferredTrackTransform ()
		{
			using (NSUrl video_url = NSUrl.FromFilename (video_asset_path))
			using (AVAsset video_asset = AVAsset.FromUrl (video_url))
			using (AVAssetImageGenerator aig = new AVAssetImageGenerator (video_asset)) {
				// setter was missing see https://bugzilla.xamarin.com/show_bug.cgi?id=5216
				aig.AppliesPreferredTrackTransform = true;
				ClassicAssert.True (aig.AppliesPreferredTrackTransform, "AppliesPreferredTrackTransform");
			}
		}

		[Test]
		public void CopyCGImageAtTime ()
		{
			// Mp4 file is supported by CopyCGImageAtTime so we can test out actual param
			using (NSUrl video_url = NSUrl.FromFilename (video_asset_path))
			using (AVAsset video_asset = AVAsset.FromUrl (video_url))
			using (AVAssetImageGenerator aig = new AVAssetImageGenerator (video_asset)) {
				// signature errors see https://bugzilla.xamarin.com/show_bug.cgi?id=5218
				CMTime actual;
				NSError error;
				var img = aig.CopyCGImageAtTime (CMTime.Zero, out actual, out error);
				ClassicAssert.NotNull (img, "CopyCGImageAtTime");
				ClassicAssert.False (actual.IsInvalid, "actual");
				ClassicAssert.Null (error, "error");
			}
		}

		[Test]
		public void CopyCGImageAtTime_Invalid ()
		{
			// Mov file is not supported by CopCGImageAtTime so we can test out error param
			using (NSUrl video_url = NSUrl.FromFilename (does_not_exists_asset_path))
			using (AVAsset video_asset = AVAsset.FromUrl (video_url))
			using (AVAssetImageGenerator aig = new AVAssetImageGenerator (video_asset)) {
				// signature errors see https://bugzilla.xamarin.com/show_bug.cgi?id=5218
				CMTime actual;
				NSError error;
				var img = aig.CopyCGImageAtTime (CMTime.Zero, out actual, out error);
				ClassicAssert.Null (img, "missing");
				ClassicAssert.True (actual.IsInvalid, "actual");
				ClassicAssert.NotNull (error, "error");
			}
		}

		string does_not_exists_asset_path = Path.Combine (NSBundle.MainBundle.BundlePath, "xamarin.mov");
		string video_asset_path = Path.Combine (NSBundle.MainBundle.ResourcePath, "xamvideotest.mp4");
		bool handled;
		ManualResetEvent mre;

		[Test]
		public void GenerateCGImagesAsynchronously ()
		{
			// This test deadlocks on Mountain Lion (but works on Lion)
			// https://gist.github.com/rolfbjarne/1190d97af79e554c298f2c133dfd8e87
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);

			handled = false;
			mre = new ManualResetEvent (false);
			ThreadStart main = () => {
				using (NSUrl video_url = NSUrl.FromFilename (video_asset_path))
				using (AVAsset video_asset = AVAsset.FromUrl (video_url))
				using (AVAssetImageGenerator aig = new AVAssetImageGenerator (video_asset)) {
					NSValue [] values = new NSValue [] { NSValue.FromCMTime (CMTime.Zero) };
					aig.GenerateCGImagesAsynchronously (values, handler);
					mre.WaitOne ();
				}
			};
			var thread = new Thread (main) {
				IsBackground = true,
			};
			thread.Start ();
			ClassicAssert.True (mre.WaitOne (2000), "wait");
			ClassicAssert.True (handled, "handled");
		}

		void handler (CMTime requestedTime, IntPtr imageRef, CMTime actualTime, AVAssetImageGeneratorResult result, NSError error)
		{
			handled = true;
			mre.Set ();
		}
	}
}
