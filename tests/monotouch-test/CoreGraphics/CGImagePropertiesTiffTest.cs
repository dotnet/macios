using System;
using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;
using CoreImage;
using NUnit.Framework;
using System.IO;

namespace monotouchtest.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImagePropertiesTiffTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var tiff = new CGImagePropertiesTiff ();
			Assert.IsNotNull (tiff, "Default constructor should create a valid instance");

			// Test setting and getting basic properties
			tiff.Software = "Test Software";
			Assert.AreEqual ("Test Software", tiff.Software, "Software property should be settable and gettable");

			tiff.XResolution = 300;
			Assert.AreEqual (300, tiff.XResolution, "XResolution property should be settable and gettable");

			tiff.YResolution = 300;
			Assert.AreEqual (300, tiff.YResolution, "YResolution property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var tiff = new CGImagePropertiesTiff (dict);
			Assert.IsNotNull (tiff, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access TIFF properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// Note: The test image is PNG, so TIFF property might be null
				// This test mainly verifies the property access doesn't throw exceptions
				var tiff = imageProps.Tiff;
				// tiff may be null for PNG files, which is expected
			}
		}

		[Test]
		public void OrientationTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test orientation property
			tiff.Orientation = CIImageOrientation.Up;
			Assert.AreEqual (CIImageOrientation.Up, tiff.Orientation, "Orientation should be settable to Up");
			
			tiff.Orientation = CIImageOrientation.Down;
			Assert.AreEqual (CIImageOrientation.Down, tiff.Orientation, "Orientation should be settable to Down");
			
			tiff.Orientation = CIImageOrientation.Left;
			Assert.AreEqual (CIImageOrientation.Left, tiff.Orientation, "Orientation should be settable to Left");
			
			tiff.Orientation = CIImageOrientation.Right;
			Assert.AreEqual (CIImageOrientation.Right, tiff.Orientation, "Orientation should be settable to Right");
		}

		[Test]
		public void ResolutionValuesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test common resolution values
			tiff.XResolution = 72; // 72 DPI
			tiff.YResolution = 72;
			Assert.AreEqual (72, tiff.XResolution, "Should handle 72 DPI");
			Assert.AreEqual (72, tiff.YResolution, "Should handle 72 DPI");
			
			tiff.XResolution = 300; // 300 DPI
			tiff.YResolution = 300;
			Assert.AreEqual (300, tiff.XResolution, "Should handle 300 DPI");
			Assert.AreEqual (300, tiff.YResolution, "Should handle 300 DPI");
			
			// Different X and Y resolutions
			tiff.XResolution = 96;
			tiff.YResolution = 72;
			Assert.AreEqual (96, tiff.XResolution, "X and Y resolutions can be different");
			Assert.AreEqual (72, tiff.YResolution, "X and Y resolutions can be different");
		}

		[Test]
		public void SoftwarePropertyTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test various software strings
			tiff.Software = "Adobe Photoshop";
			Assert.AreEqual ("Adobe Photoshop", tiff.Software, "Should handle software name");
			
			tiff.Software = "GIMP 2.10";
			Assert.AreEqual ("GIMP 2.10", tiff.Software, "Should handle software with version");
			
			tiff.Software = "Test Software 1.0.0";
			Assert.AreEqual ("Test Software 1.0.0", tiff.Software, "Should handle detailed version");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test that nullable properties can be set to null
			tiff.Software = null;
			Assert.IsNull (tiff.Software, "Software should be nullable");
			
			tiff.XResolution = null;
			Assert.IsNull (tiff.XResolution, "XResolution should be nullable");
			
			tiff.YResolution = null;
			Assert.IsNull (tiff.YResolution, "YResolution should be nullable");
			
			tiff.Orientation = null;
			Assert.IsNull (tiff.Orientation, "Orientation should be nullable");
		}

		[Test]
		public void ZeroResolutionTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test edge case of zero resolution
			tiff.XResolution = 0;
			tiff.YResolution = 0;
			Assert.AreEqual (0, tiff.XResolution, "Should handle zero resolution");
			Assert.AreEqual (0, tiff.YResolution, "Should handle zero resolution");
		}

		[Test]
		public void LargeResolutionValuesTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test large resolution values
			tiff.XResolution = 9999;
			tiff.YResolution = 9999;
			Assert.AreEqual (9999, tiff.XResolution, "Should handle large resolution values");
			Assert.AreEqual (9999, tiff.YResolution, "Should handle large resolution values");
		}

		[Test]
		public void EmptyStringTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test that empty string works correctly
			tiff.Software = "";
			Assert.AreEqual ("", tiff.Software, "Software should accept empty strings");
		}

		[Test]
		public void UnicodeStringTest ()
		{
			var tiff = new CGImagePropertiesTiff ();
			
			// Test that unicode strings work correctly
			tiff.Software = "Test 測試 Software";
			Assert.AreEqual ("Test 測試 Software", tiff.Software, "Software should handle unicode");
		}
	}
}