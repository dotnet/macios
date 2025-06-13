using System;
using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;
using NUnit.Framework;
using System.IO;
using CoreImage;

namespace monotouchtest.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImagePropertiesExifTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var exif = new CGImagePropertiesExif ();
			Assert.IsNotNull (exif, "Default constructor should create a valid instance");

			// Test setting and getting basic properties
			exif.Aperture = 2.8f;
			Assert.AreEqual (2.8f, exif.Aperture, 0.001f, "Aperture property should be settable and gettable");

			exif.ExposureTime = 0.125f;
			Assert.AreEqual (0.125f, exif.ExposureTime, 0.001f, "ExposureTime property should be settable and gettable");

			exif.Flash = true;
			Assert.AreEqual (true, exif.Flash, "Flash property should be settable and gettable");

			exif.PixelXDimension = 1920;
			Assert.AreEqual (1920, exif.PixelXDimension, "PixelXDimension property should be settable and gettable");

			exif.PixelYDimension = 1080;
			Assert.AreEqual (1080, exif.PixelYDimension, "PixelYDimension property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var exif = new CGImagePropertiesExif (dict);
			Assert.IsNotNull (exif, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access Exif properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// Note: The test image may not have EXIF data, so Exif property could be null
				// This test mainly verifies the property access doesn't throw exceptions
				var exif = imageProps.Exif;
				// exif may be null for PNG files without EXIF data, which is expected
			}
		}

		[Test]
		public void ExposureProgramTest ()
		{
			var exif = new CGImagePropertiesExif ();
			
			exif.ExposureProgram = 1; // Manual mode
			Assert.AreEqual (1, exif.ExposureProgram, "ExposureProgram should be settable and gettable");
			
			exif.ExposureProgram = 2; // Aperture priority
			Assert.AreEqual (2, exif.ExposureProgram, "ExposureProgram should accept different values");
		}

		[Test]
		public void FloatingPointPropertiesTest ()
		{
			var exif = new CGImagePropertiesExif ();
			
			// Test various floating point properties
			exif.Brightness = 0.5f;
			Assert.AreEqual (0.5f, exif.Brightness, 0.001f, "Brightness should be settable");
			
			exif.DigitalZoomRatio = 2.0f;
			Assert.AreEqual (2.0f, exif.DigitalZoomRatio, 0.001f, "DigitalZoomRatio should be settable");
			
			exif.ExposureBias = -1.5f;
			Assert.AreEqual (-1.5f, exif.ExposureBias, 0.001f, "ExposureBias should accept negative values");
			
			exif.FlashEnergy = 10.0f;
			Assert.AreEqual (10.0f, exif.FlashEnergy, 0.001f, "FlashEnergy should be settable");
			
			exif.SubjectDistance = 5.2f;
			Assert.AreEqual (5.2f, exif.SubjectDistance, 0.001f, "SubjectDistance should be settable");
		}

		[Test]
		public void ISOSpeedRatingsTest ()
		{
			var exif = new CGImagePropertiesExif ();
			
			// ISOSpeedRatings is read-only in the current implementation
			// This test verifies it doesn't throw when accessed
			var isoRatings = exif.ISOSpeedRatings;
			// Should not throw, may be null initially
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var exif = new CGImagePropertiesExif ();
			
			// Test that nullable properties can be set to null
			exif.Aperture = null;
			Assert.IsNull (exif.Aperture, "Aperture should be nullable");
			
			exif.ExposureTime = null;
			Assert.IsNull (exif.ExposureTime, "ExposureTime should be nullable");
			
			exif.Flash = null;
			Assert.IsNull (exif.Flash, "Flash should be nullable");
		}
	}
}