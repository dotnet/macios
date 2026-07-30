//
// Unit tests for CGImageAnimation
//
// Authors:
//	Whitney Schmidt <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Corp. All rights reserved.
//
using System.IO;
using System.Threading.Tasks;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;
using ImageIO;

namespace MonoTouchFixtures.ImageIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImageAnimationTest {

		NSUrl imageUrl;
		NSData imageData;
		TaskCompletionSource<bool> tcs;
		int testValue;
		CGImageAnimationStatus status;

		void MyHandlerSetValueZero (nint index, CGImage image, out bool stop)
		{
			stop = true;
			testValue = 0;
			tcs.TrySetResult (true);
		}

		void MyHandlerSetValueOne (nint index, CGImage image, out bool stop)
		{
			stop = true;
			testValue = 1;
			tcs.TrySetResult (true);
		}

		[OneTimeSetUp]
		public void Init ()
		{
			TestRuntime.AssertXcodeVersion (11, 4);

			imageUrl = NSBundle.MainBundle.GetUrlForResource ("square", "gif");
			imageData = NSData.FromUrl (imageUrl);
		}

		[OneTimeTearDown]
		public void Cleanup ()
		{
			imageUrl?.Dispose ();
			imageData?.Dispose ();
		}

		[SetUp]
		public void InitPerTest ()
		{
			testValue = -1;
		}

		[TearDown]
		public void Dispose ()
		{
			testValue = -1;
		}

		void CallAnimateImage (bool useUrl, CGImageAnimation.CGImageSourceAnimationHandler handler)
		{
			tcs = new TaskCompletionSource<bool> ();
			status = (CGImageAnimationStatus) 1; /* CGImageAnimationStatus.Ok == 0 */
			bool done = false;

			TestRuntime.RunAsync (TimeSpan.FromSeconds (30), async () => {
				if (useUrl) {
					status = CGImageAnimation.AnimateImage (imageUrl, null, handler);
				} else {
					status = CGImageAnimation.AnimateImage (imageData, null, handler);
				}
				await tcs.Task;
				done = true;
			},
				() => done);

			tcs = null;
		}

		[Test]
		public void AnimateImageWithUrl ()
		{
			CallAnimateImage ( /* useUrl */ true, MyHandlerSetValueZero);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: handler called with url");
			Assert.That (testValue, Is.EqualTo (0), "handler called with url");
		}

		[Test]
		public void AnimateImageWithData ()
		{
			CallAnimateImage ( /* useUrl */ false, MyHandlerSetValueZero);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: handler called with data");
			Assert.That (testValue, Is.EqualTo (0), "handler called with data");
		}

		[Test]
		public void AnimateImageWithUrlChangeHandler ()
		{
			CallAnimateImage ( /* useUrl */ true, MyHandlerSetValueZero);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: first handler called with url");
			Assert.That (testValue, Is.EqualTo (0), "first handler called with url");

			CallAnimateImage ( /* useUrl */ true, MyHandlerSetValueOne);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: second handler called with url");
			Assert.That (testValue, Is.EqualTo (1), "second handler called with url");
		}

		[Test]
		public void AnimateImageWithDataChangeHandler ()
		{
			CallAnimateImage ( /* useUrl */ false, MyHandlerSetValueZero);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: first handler called with data");
			Assert.That (testValue, Is.EqualTo (0), "first handler called with data");

			CallAnimateImage ( /* useUrl */ false, MyHandlerSetValueOne);
			Assert.That (status, Is.EqualTo (CGImageAnimationStatus.Ok), "status ok: second handler called with data");
			Assert.That (testValue, Is.EqualTo (1), "second handler called with data");
		}

		[Test]
		public void AnimateImageWithUrlNullUrl ()
		{
			Assert.Throws<ArgumentNullException> (() => CGImageAnimation.AnimateImage ((NSUrl) null, null, MyHandlerSetValueZero), "null url");
		}

		[Test]
		public void AnimateImageWithUrlNullHandler ()
		{
			Assert.Throws<ArgumentNullException> (() => CGImageAnimation.AnimateImage (imageUrl, null, /* CGImageSourceAnimationHandler */ null), "null handler called with url");
		}

		[Test]
		public void AnimateImageWithDataNullData ()
		{
			Assert.Throws<ArgumentNullException> (() => CGImageAnimation.AnimateImage ((NSData) null, null, MyHandlerSetValueZero), "null data");
		}

		[Test]
		public void AnimateImageWithDataNullHandler ()
		{
			Assert.Throws<ArgumentNullException> (() => CGImageAnimation.AnimateImage (imageData, null, /* CGImageSourceAnimationHandler */ null), "null handler called with data");
		}

	}
}
