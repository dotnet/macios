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
	public class CGImagePropertiesPngTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var png = new CGImagePropertiesPng ();
			Assert.IsNotNull (png, "Default constructor should create a valid instance");

			// Test setting and getting string properties
			png.Author = "Test Author";
			Assert.AreEqual ("Test Author", png.Author, "Author property should be settable and gettable");

			png.Description = "Test PNG image";
			Assert.AreEqual ("Test PNG image", png.Description, "Description property should be settable and gettable");

			png.Software = "Test Software";
			Assert.AreEqual ("Test Software", png.Software, "Software property should be settable and gettable");

			png.Title = "Test Title";
			Assert.AreEqual ("Test Title", png.Title, "Title property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var png = new CGImagePropertiesPng (dict);
			Assert.IsNotNull (png, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access PNG properties from a real PNG file
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// For a PNG file, the Png property should be accessible
				var png = imageProps.Png;
				// png may be null or contain properties depending on the PNG file
			}
		}

		[Test]
		public void NumericPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test numeric properties
			png.Gamma = 2.2f;
			Assert.AreEqual (2.2f, png.Gamma, 0.001f, "Gamma should be settable and gettable");
			
			png.XPixelsPerMeter = 3780; // ~96 DPI
			Assert.AreEqual (3780, png.XPixelsPerMeter, "XPixelsPerMeter should be settable");
			
			png.YPixelsPerMeter = 3780; // ~96 DPI
			Assert.AreEqual (3780, png.YPixelsPerMeter, "YPixelsPerMeter should be settable");
		}

		[Test]
		public void GammaValuesTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test common gamma values
			png.Gamma = 1.0f;
			Assert.AreEqual (1.0f, png.Gamma, 0.001f, "Should handle gamma of 1.0");
			
			png.Gamma = 2.2f;
			Assert.AreEqual (2.2f, png.Gamma, 0.001f, "Should handle gamma of 2.2");
			
			png.Gamma = 1.8f;
			Assert.AreEqual (1.8f, png.Gamma, 0.001f, "Should handle gamma of 1.8");
		}

		[Test]
		public void PixelsPerMeterTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test various DPI equivalents
			// 72 DPI = ~2835 pixels per meter
			png.XPixelsPerMeter = 2835;
			png.YPixelsPerMeter = 2835;
			Assert.AreEqual (2835, png.XPixelsPerMeter, "Should handle 72 DPI equivalent");
			Assert.AreEqual (2835, png.YPixelsPerMeter, "Should handle 72 DPI equivalent");
			
			// Different X and Y resolutions
			png.XPixelsPerMeter = 3780; // ~96 DPI
			png.YPixelsPerMeter = 2835; // ~72 DPI
			Assert.AreEqual (3780, png.XPixelsPerMeter, "X and Y resolutions can be different");
			Assert.AreEqual (2835, png.YPixelsPerMeter, "X and Y resolutions can be different");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test that nullable properties can be set to null
			png.Author = null;
			Assert.IsNull (png.Author, "Author should be nullable");
			
			png.Description = null;
			Assert.IsNull (png.Description, "Description should be nullable");
			
			png.Gamma = null;
			Assert.IsNull (png.Gamma, "Gamma should be nullable");
			
			png.XPixelsPerMeter = null;
			Assert.IsNull (png.XPixelsPerMeter, "XPixelsPerMeter should be nullable");
		}

		[Test]
		public void EmptyStringPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test that empty strings work correctly
			png.Author = "";
			Assert.AreEqual ("", png.Author, "Author should accept empty strings");
			
			png.Title = "";
			Assert.AreEqual ("", png.Title, "Title should accept empty strings");
			
			png.Software = "";
			Assert.AreEqual ("", png.Software, "Software should accept empty strings");
		}

		[Test]
		public void UnicodeStringPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();
			
			// Test that unicode strings work correctly
			png.Author = "Test Author 測試";
			Assert.AreEqual ("Test Author 測試", png.Author, "Author should handle unicode");
			
			png.Title = "Тест Title";
			Assert.AreEqual ("Тест Title", png.Title, "Title should handle unicode");
		}
	}
}