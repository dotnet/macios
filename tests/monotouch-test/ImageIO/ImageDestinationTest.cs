//
// Unit tests for CGImageDestination
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

using System.IO;
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
	public class ImageDestinationTest {

		const string GoodUti = "public.jpeg"; // correct
		const string BadUti = "public.Jpeg"; // wrong 'J' is an upper letter

		[Test]
		public void FromData_BadITU ()
		{
			using (NSMutableData destData = new NSMutableData ()) {
				Assert.That (CGImageDestination.Create (destData, BadUti, 1), Is.Null, "FromData-1");
				Assert.That (CGImageDestination.Create (destData, BadUti, 1, new CGImageDestinationOptions ()), Is.Null, "FromData-2");
			}
		}

		[Test]
		public void FromData_GoodITU ()
		{
			using (NSMutableData destData = new NSMutableData ()) {
				using (var id = CGImageDestination.Create (destData, GoodUti, 1)) {
					Assert.That (id.Handle, Is.Not.EqualTo (IntPtr.Zero), "handle-1");
				}
				using (var id = CGImageDestination.Create (destData, GoodUti, 1, new CGImageDestinationOptions ())) {
					Assert.That (id.Handle, Is.Not.EqualTo (IntPtr.Zero), "handle-2");
				}
			}
		}

		[Test]
		public void Create_DataConsumer_BadUTI ()
		{
			using (NSMutableData destData = new NSMutableData ())
			using (var consumer = new CGDataConsumer (destData)) {
				Assert.That (CGImageDestination.Create (consumer, BadUti, 1), Is.Null, "Create-1");
				Assert.That (CGImageDestination.Create (consumer, BadUti, 1, new CGImageDestinationOptions ()), Is.Null, "Create-2");
			}
		}

		[Test]
		public void Create_DataConsumer_GoodUTI ()
		{
			using (NSMutableData destData = new NSMutableData ())
			using (var consumer = new CGDataConsumer (destData)) {
				using (var id = CGImageDestination.Create (consumer, GoodUti, 1)) {
					Assert.That (id.Handle, Is.Not.EqualTo (IntPtr.Zero), "handle-1");
				}
				using (var id = CGImageDestination.Create (consumer, GoodUti, 1, new CGImageDestinationOptions ())) {
					Assert.That (id.Handle, Is.Not.EqualTo (IntPtr.Zero), "handle-2");
				}
			}
		}

		[Test]
		public void FromUrl_BadITU ()
		{
			using (NSUrl url = NSUrl.FromString ("file://local")) {
				Assert.That (CGImageDestination.Create (url, BadUti, 1), Is.Null, "FromUrl-1");
			}
		}

		[Test]
		public void AddImage ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (NSMutableData destData = new NSMutableData ())
#if MONOMAC
			using (var uiimg = new NSImage (file))
#else
			using (var uiimg = UIImage.FromFile (file))
#endif
			using (var img = uiimg.CGImage)
			using (var id = CGImageDestination.Create (destData, GoodUti, 1)) {
				id.AddImage (img, (NSDictionary) null);
			}
		}

		[Test]
		public void AddImageAndMetadata ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (NSMutableData destData = new NSMutableData ())
#if MONOMAC
			using (var uiimg = new NSImage (file))
#else
			using (var uiimg = UIImage.FromFile (file))
#endif
			using (var img = uiimg.CGImage)
			using (var id = CGImageDestination.Create (destData, GoodUti, 1))
			using (var mutable = new CGMutableImageMetadata ()) {
				id.AddImageAndMetadata (img, mutable, (NSDictionary) null);
			}
		}

		[Test]
		public void CopyImageSource ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			using (NSData data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("xamarin2", "png")))
			using (var source = CGImageSource.FromData (data))
			using (NSMutableData destData = new NSMutableData ())
			using (var id = CGImageDestination.Create (destData, GoodUti, 1)) {
				NSError err;
				// testing that null is allowed (no crash) so the fact that is return false and an error does not matter
				Assert.That (id.CopyImageSource (source, (NSDictionary) null, out err), Is.False, "CopyImageSource");
				Assert.That (err, Is.Not.Null, "NSError");
			}
		}

		[Test]
		public void TypeIdentifiers ()
		{
			Assert.That (CGImageDestination.TypeIdentifiers, Is.Not.Null, "TypeIdentifiers");
		}
	}
}
