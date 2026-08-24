// Copyright 2012-2013 Xamarin Inc. All rights reserved

using System.Drawing;
using System.Reflection;
using CoreGraphics;
using ImageIO;

namespace MonoTouchFixtures.ImageIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImageSourceTest {
		NSUrl fileUrl = NSBundle.MainBundle.GetUrlForResource ("xamarin2", "png");

		[Test]
		public void FromUrlTest ()
		{
			using (var img = CGImageSource.FromUrl (fileUrl)) {
				Assert.That (img, Is.Not.Null, "#a1");
			}

			using (var img = CGImageSource.FromUrl (fileUrl, new CGImageOptions ())) {
				Assert.That (img, Is.Not.Null, "#b1");
			}

			using (var img = CGImageSource.FromUrl (fileUrl, null)) {
				Assert.That (img, Is.Not.Null, "#c1");
			}
		}

		[Test]
		public void AllowableTypes ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 0);

			bool CanDecode (string [] allowableTypes)
			{
				using (var source = CGImageSource.FromUrl (fileUrl, new CGImageOptions { AllowableTypes = allowableTypes })) {
					if (source is null)
						return false;
					using (var image = source.CreateImage (0, null))
						return image is not null;
				}
			}

			Assert.That (CanDecode (new [] { "public.png" }), Is.True, "Allowed");
			Assert.That (CanDecode (new [] { "public.jpeg" }), Is.False, "Disallowed");
			Assert.That (CanDecode ([]), Is.False, "Empty");
		}

		[Test]
		public void PrioritizeQuality ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 0);

			var lib = Dlfcn.dlopen (Constants.ImageIOLibrary, 0);
			Assert.That (lib, Is.Not.EqualTo (IntPtr.Zero), "Library");
			try {
				var key = Dlfcn.GetStringConstant (lib, "kCGImageSourcePrioritizeQuality");
				Assert.That (key, Is.Not.Null, "Key");
				var toDictionary = typeof (CGImageOptions).GetMethod ("ToDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
				Assert.That (toDictionary, Is.Not.Null, "ToDictionary");
				using var dictionary = (NSMutableDictionary) toDictionary.Invoke (new CGImageOptions { PrioritizeQuality = true }, null);
				var value = dictionary [key] as NSNumber;
				Assert.That (value, Is.Not.Null, "Value");
				Assert.That (value.BoolValue, Is.True, "BoolValue");

				using var defaultDictionary = (NSMutableDictionary) toDictionary.Invoke (new CGImageOptions (), null);
				Assert.That (defaultDictionary [key], Is.Null, "Default");
			} finally {
				Dlfcn.dlclose (lib);
			}
		}

		[Test]
		public void FromDataProviderTest ()
		{
			var file = NSBundle.MainBundle.PathForResource ("xamarin2", "png");
			using (var dp = new CGDataProvider (file)) {
				using (var img = CGImageSource.FromDataProvider (dp)) {
					Assert.That (img, Is.Not.Null, "#a1");
				}
			}

			using (var dp = new CGDataProvider (file)) {
				using (var img = CGImageSource.FromDataProvider (dp, new CGImageOptions ())) {
					Assert.That (img, Is.Not.Null, "#b1");
				}
			}

			using (var dp = new CGDataProvider (file)) {
				using (var img = CGImageSource.FromDataProvider (dp, null)) {
					Assert.That (img, Is.Not.Null, "#c1");
				}
			}
		}

		[Test]
		public void FromDataTest ()
		{
			NSData data = NSData.FromFile (NSBundle.MainBundle.PathForResource ("xamarin2", "png"));

			using (var img = CGImageSource.FromData (data)) {
				Assert.That (img, Is.Not.Null, "#a1");
			}

			using (var img = CGImageSource.FromData (data, new CGImageOptions ())) {
				Assert.That (img, Is.Not.Null, "#b1");
			}

			using (var img = CGImageSource.FromData (data, null)) {
				Assert.That (img, Is.Not.Null, "#c1");
			}
		}

		[Test]
		public void CreateImageTest ()
		{
			using (var imgsrc = CGImageSource.FromUrl (fileUrl)) {
				using (var img = imgsrc.CreateImage (0, null)) {
					Assert.That (img, Is.Not.Null, "#a1");
				}
				using (var img = imgsrc.CreateImage (0, new CGImageOptions ())) {
					Assert.That (img, Is.Not.Null, "#b1");
				}
			}
		}

		[Test]
		public void CreateThumbnailTest ()
		{
			using (var imgsrc = CGImageSource.FromUrl (fileUrl)) {
				using (var img = imgsrc.CreateThumbnail (0, null)) {
					Assert.That (img, Is.Null.Or.Not.Null, "#a1"); // sometimes we get an image, and sometimes we don't 🤷‍♂️
				}
				using (var img = imgsrc.CreateThumbnail (0, new CGImageThumbnailOptions ())) {
					Assert.That (img, Is.Null.Or.Not.Null, "#b1"); // sometimes we get an image, and sometimes we don't 🤷‍♂️
				}
			}
		}

		[Test]
		public void CreateIncrementalTest ()
		{
			using (var img = CGImageSource.CreateIncremental (null)) {
				Assert.That (img, Is.Not.Null, "#a1");
			}

			using (var img = CGImageSource.CreateIncremental (new CGImageOptions ())) {
				Assert.That (img, Is.Not.Null, "#b1");
			}
		}

		[Test]
		public void CopyProperties ()
		{
			// what we had to answer with 5.2 for http://stackoverflow.com/q/10753108/220643
			IntPtr lib = Dlfcn.dlopen (Constants.ImageIOLibrary, 0);
			try {
				NSString kCGImageSourceShouldCache = Dlfcn.GetStringConstant (lib, "kCGImageSourceShouldCache");
				NSString kCGImagePropertyPixelWidth = Dlfcn.GetStringConstant (lib, "kCGImagePropertyPixelWidth");
				NSString kCGImagePropertyPixelHeight = Dlfcn.GetStringConstant (lib, "kCGImagePropertyPixelHeight");

				using (var imageSource = CGImageSource.FromUrl (fileUrl)) {
					using (var dict = new NSMutableDictionary ()) {
						dict [kCGImageSourceShouldCache] = NSNumber.FromBoolean (false);
						using (var props = imageSource.CopyProperties (dict)) {
							Assert.That (props.ValueForKey (kCGImagePropertyPixelWidth), Is.Null, "kCGImagePropertyPixelWidth");
							Assert.That (props.ValueForKey (kCGImagePropertyPixelHeight), Is.Null, "kCGImagePropertyPixelHeight");
							NSNumber n = (NSNumber) props ["FileSize"];
							// image is "optimized" for devices (and a lot bigger at 10351 bytes ;-)
							Assert.That ((int) n, Is.AtLeast (7318), "FileSize");
						}
					}
				}
			} finally {
				Dlfcn.dlclose (lib);
			}
		}

		[Test]
		public void GetProperties ()
		{
			using (var imageSource = CGImageSource.FromUrl (fileUrl)) {
				CGImageOptions options = new CGImageOptions () { ShouldCache = false };

				var props = imageSource.GetProperties (options);
				Assert.That (props.PixelWidth, Is.Null, "PixelHeight-0");
				Assert.That (props.PixelHeight, Is.Null, "PixelWidth-0");
				// image is "optimized" for devices (and a lot bigger at 10351 bytes ;-)
				Assert.That (props.FileSize, Is.AtLeast (7318), "FileSize");

				props = imageSource.GetProperties (0, options);
				Assert.That (props.PixelWidth, Is.EqualTo (57), "PixelHeight");
				Assert.That (props.PixelHeight, Is.EqualTo (57), "PixelWidth");
				Assert.That (props.ColorModel, Is.EqualTo (CGImageColorModel.RGB), "ColorModel");
				Assert.That (props.Depth, Is.EqualTo (8), "Depth");
			}
		}

#if !MONOMAC // CopyMetadata and RemoveCache not available on mac
		[Test]
		public void CopyMetadata ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			using (var imageSource = CGImageSource.FromUrl (fileUrl)) {
				CGImageOptions options = new CGImageOptions () { ShouldCacheImmediately = true };
				using (CGImageMetadata metadata = imageSource.CopyMetadata (0, options)) {
					Console.WriteLine ();
				}
			}
		}

		[Test]
		public void RemoveCache ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			using (var imageSource = CGImageSource.FromUrl (fileUrl)) {
				imageSource.RemoveCache (0);
			}
		}
#endif
	}
}
