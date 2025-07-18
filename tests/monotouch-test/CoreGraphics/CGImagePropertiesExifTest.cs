using System;
using System.IO;

using CoreGraphics;
using CoreImage;
using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

using NUnit.Framework;

namespace monotouchtest.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImagePropertiesExifTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var exif = new CGImagePropertiesExif ();
			Assert.That (exif, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting basic properties
			exif.Aperture = 2.8f;
			Assert.That (exif.Aperture, Is.EqualTo (2.8f).Within (0.001f), "Aperture property should be settable and gettable");

			exif.ExposureTime = 0.125f;
			Assert.That (exif.ExposureTime, Is.EqualTo (0.125f).Within (0.001f), "ExposureTime property should be settable and gettable");

			exif.Flash = true;
			Assert.That (exif.Flash, Is.EqualTo (true), "Flash property should be settable and gettable");

			exif.PixelXDimension = 1920;
			Assert.That (exif.PixelXDimension, Is.EqualTo (1920), "PixelXDimension property should be settable and gettable");

			exif.PixelYDimension = 1080;
			Assert.That (exif.PixelYDimension, Is.EqualTo (1080), "PixelYDimension property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var exif = new CGImagePropertiesExif (dict);
			Assert.That (exif, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access Exif properties
			var file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// Note: The test image may not have EXIF data, so Exif property could be null
			// This test mainly verifies the property access doesn't throw exceptions
			var exif = imageProps.Exif;
			// exif may be null for PNG files without EXIF data, which is expected
			Assert.That (exif, Is.Null, "EXIF data should be null for PNG files without EXIF data");
		}

		[Test]
		public void ExposureProgramTest ()
		{
			var exif = new CGImagePropertiesExif ();

			exif.ExposureProgram = 1; // Manual mode
			Assert.That (exif.ExposureProgram, Is.EqualTo (1), "ExposureProgram should be settable and gettable");

			exif.ExposureProgram = 2; // Aperture priority
			Assert.That (exif.ExposureProgram, Is.EqualTo (2), "ExposureProgram should accept different values");
		}

		[Test]
		public void FloatingPointPropertiesTest ()
		{
			var exif = new CGImagePropertiesExif ();

			// Test various floating point properties
			exif.Brightness = 0.5f;
			Assert.That (exif.Brightness, Is.EqualTo (0.5f).Within (0.001f), "Brightness should be settable");

			exif.DigitalZoomRatio = 2.0f;
			Assert.That (exif.DigitalZoomRatio, Is.EqualTo (2.0f).Within (0.001f), "DigitalZoomRatio should be settable");

			exif.ExposureBias = -1.5f;
			Assert.That (exif.ExposureBias, Is.EqualTo (-1.5f).Within (0.001f), "ExposureBias should accept negative values");

			exif.FlashEnergy = 10.0f;
			Assert.That (exif.FlashEnergy, Is.EqualTo (10.0f).Within (0.001f), "FlashEnergy should be settable");

			exif.SubjectDistance = 5.2f;
			Assert.That (exif.SubjectDistance, Is.EqualTo (5.2f).Within (0.001f), "SubjectDistance should be settable");
		}

		[Test]
		public void ISOSpeedRatingsTest ()
		{
			var exif = new CGImagePropertiesExif ();

			// ISOSpeedRatings is read-only in the current implementation
			// This test verifies it doesn't throw when accessed
			var isoRatings = exif.ISOSpeedRatings;
			// Should not throw, may be null initially
			Assert.That (isoRatings, Is.Null, "ISOSpeedRatings should be null initially");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var exif = new CGImagePropertiesExif ();

			// Test that nullable properties can be set to null
			exif.Aperture = null;
			Assert.That (exif.Aperture, Is.Null, "Aperture should be nullable");

			exif.ExposureTime = null;
			Assert.That (exif.ExposureTime, Is.Null, "ExposureTime should be nullable");

			exif.Flash = null;
			Assert.That (exif.Flash, Is.Null, "Flash should be nullable");
		}
	}
}
