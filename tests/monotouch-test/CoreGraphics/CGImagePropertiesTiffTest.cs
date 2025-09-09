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
	public class CGImagePropertiesTiffTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var tiff = new CGImagePropertiesTiff ();
			Assert.That (tiff, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting basic properties
			tiff.Software = "Test Software";
			Assert.That (tiff.Software, Is.EqualTo ("Test Software"), "Software property should be settable and gettable");

			tiff.XResolution = 300;
			Assert.That (tiff.XResolution, Is.EqualTo (300), "XResolution property should be settable and gettable");

			tiff.YResolution = 300;
			Assert.That (tiff.YResolution, Is.EqualTo (300), "YResolution property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var tiff = new CGImagePropertiesTiff (dict);
			Assert.That (tiff, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access TIFF properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// Note: The test image is PNG, so TIFF property might be null
			// This test mainly verifies the property access doesn't throw exceptions
			var tiff = imageProps.Tiff;
			// tiff may be null for PNG files, which is expected
			Assert.That (tiff, Is.Null, "TIFF data should be null for PNG files");
		}

		[Test]
		public void OrientationTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test orientation property
			tiff.Orientation = CIImageOrientation.TopLeft;
			Assert.That (tiff.Orientation, Is.EqualTo (CIImageOrientation.TopLeft), "Orientation should be settable to TopLeft");

			tiff.Orientation = CIImageOrientation.TopRight;
			Assert.That (tiff.Orientation, Is.EqualTo (CIImageOrientation.TopRight), "Orientation should be settable to TopRight");

			tiff.Orientation = CIImageOrientation.BottomLeft;
			Assert.That (tiff.Orientation, Is.EqualTo (CIImageOrientation.BottomLeft), "Orientation should be settable to BottomLeft");

			tiff.Orientation = CIImageOrientation.BottomRight;
			Assert.That (tiff.Orientation, Is.EqualTo (CIImageOrientation.BottomRight), "Orientation should be settable to BottomRight");
		}

		[Test]
		public void ResolutionValuesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test common resolution values
			tiff.XResolution = 72; // 72 DPI
			tiff.YResolution = 72;
			Assert.That (tiff.XResolution, Is.EqualTo (72), "Should handle 72 DPI");
			Assert.That (tiff.YResolution, Is.EqualTo (72), "Should handle 72 DPI");

			tiff.XResolution = 300; // 300 DPI
			tiff.YResolution = 300;
			Assert.That (tiff.XResolution, Is.EqualTo (300), "Should handle 300 DPI");
			Assert.That (tiff.YResolution, Is.EqualTo (300), "Should handle 300 DPI");

			// Different X and Y resolutions
			tiff.XResolution = 96;
			tiff.YResolution = 72;
			Assert.That (tiff.XResolution, Is.EqualTo (96), "X and Y resolutions can be different");
			Assert.That (tiff.YResolution, Is.EqualTo (72), "X and Y resolutions can be different");
		}

		[Test]
		public void SoftwarePropertyTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test various software strings
			tiff.Software = "Adobe Photoshop";
			Assert.That (tiff.Software, Is.EqualTo ("Adobe Photoshop"), "Should handle software name");

			tiff.Software = "GIMP 2.10";
			Assert.That (tiff.Software, Is.EqualTo ("GIMP 2.10"), "Should handle software with version");

			tiff.Software = "Test Software 1.0.0";
			Assert.That (tiff.Software, Is.EqualTo ("Test Software 1.0.0"), "Should handle detailed version");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test that nullable properties can be set to null
			tiff.Software = null;
			Assert.That (tiff.Software, Is.Null, "Software should be nullable");

			tiff.XResolution = null;
			Assert.That (tiff.XResolution, Is.Null, "XResolution should be nullable");

			tiff.YResolution = null;
			Assert.That (tiff.YResolution, Is.Null, "YResolution should be nullable");

			tiff.Orientation = null;
			Assert.That (tiff.Orientation, Is.Null, "Orientation should be nullable");
		}

		[Test]
		public void ZeroResolutionTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test edge case of zero resolution
			tiff.XResolution = 0;
			tiff.YResolution = 0;
			Assert.That (tiff.XResolution, Is.EqualTo (0), "Should handle zero resolution");
			Assert.That (tiff.YResolution, Is.EqualTo (0), "Should handle zero resolution");
		}

		[Test]
		public void LargeResolutionValuesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test large resolution values
			tiff.XResolution = 9999;
			tiff.YResolution = 9999;
			Assert.That (tiff.XResolution, Is.EqualTo (9999), "Should handle large resolution values");
			Assert.That (tiff.YResolution, Is.EqualTo (9999), "Should handle large resolution values");
		}

		[Test]
		public void EmptyStringTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test that empty string works correctly
			tiff.Software = "";
			Assert.That (tiff.Software, Is.EqualTo (""), "Software should accept empty strings");
		}

		[Test]
		public void UnicodeStringTest ()
		{
			var tiff = new CGImagePropertiesTiff ();

			// Test that unicode strings work correctly
			tiff.Software = "Test 測試 Software";
			Assert.That (tiff.Software, Is.EqualTo ("Test 測試 Software"), "Software should handle unicode");
		}
	}
}
