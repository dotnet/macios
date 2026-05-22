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
				Assert.That (aig.ApertureMode, Is.Null, "ApertureMode");
				Assert.That (aig.AppliesPreferredTrackTransform, Is.False, "AppliesPreferredTrackTransform");
				Assert.That (aig.MaximumSize, Is.EqualTo (CGSize.Empty), "MaximumSize");
				Assert.That (aig.RequestedTimeToleranceAfter.IsPositiveInfinity, Is.True, "RequestedTimeToleranceAfter");
				Assert.That (aig.RequestedTimeToleranceBefore.IsPositiveInfinity, Is.True, "RequestedTimeToleranceBefore");
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
				Assert.That (aig.AppliesPreferredTrackTransform, Is.True, "AppliesPreferredTrackTransform");
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
				Assert.That (img, Is.Not.Null, "CopyCGImageAtTime");
				Assert.That (actual.IsInvalid, Is.False, "actual");
				Assert.That (error, Is.Null, "error");
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
				Assert.That (img, Is.Null, "missing");
				Assert.That (actual.IsInvalid, Is.True, "actual");
				Assert.That (error, Is.Not.Null, "error");
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
			Assert.That (mre.WaitOne (2000), Is.True, "wait");
			Assert.That (handled, Is.True, "handled");
		}

		void handler (CMTime requestedTime, IntPtr imageRef, CMTime actualTime, AVAssetImageGeneratorResult result, NSError error)
		{
			handled = true;
			mre.Set ();
		}
	}
}
