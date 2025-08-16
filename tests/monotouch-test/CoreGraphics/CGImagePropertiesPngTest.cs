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
	public class CGImagePropertiesPngTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var png = new CGImagePropertiesPng ();
			Assert.That (png, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting string properties
			png.Author = "Test Author";
			Assert.That (png.Author, Is.EqualTo ("Test Author"), "Author property should be settable and gettable");

			png.Description = "Test PNG image";
			Assert.That (png.Description, Is.EqualTo ("Test PNG image"), "Description property should be settable and gettable");

			png.Software = "Test Software";
			Assert.That (png.Software, Is.EqualTo ("Test Software"), "Software property should be settable and gettable");

			png.Title = "Test Title";
			Assert.That (png.Title, Is.EqualTo ("Test Title"), "Title property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var png = new CGImagePropertiesPng (dict);
			Assert.That (png, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access PNG properties from a real PNG file
			var file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// For a PNG file, the Png property should be accessible
			var png = imageProps.Png;
			// png may be null or contain properties depending on the PNG file
			Assert.That (png, Is.Not.Null, "PNG data should not be null for PNG files");
		}

		[Test]
		public void NumericPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test numeric properties
			png.Gamma = 2.2f;
			Assert.That (png.Gamma, Is.EqualTo (2.2f).Within (0.001f), "Gamma should be settable and gettable");

			png.XPixelsPerMeter = 3780; // ~96 DPI
			Assert.That (png.XPixelsPerMeter, Is.EqualTo (3780), "XPixelsPerMeter should be settable");

			png.YPixelsPerMeter = 3780; // ~96 DPI
			Assert.That (png.YPixelsPerMeter, Is.EqualTo (3780), "YPixelsPerMeter should be settable");
		}

		[Test]
		public void GammaValuesTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test common gamma values
			png.Gamma = 1.0f;
			Assert.That (png.Gamma, Is.EqualTo (1.0f).Within (0.001f), "Should handle gamma of 1.0");

			png.Gamma = 2.2f;
			Assert.That (png.Gamma, Is.EqualTo (2.2f).Within (0.001f), "Should handle gamma of 2.2");

			png.Gamma = 1.8f;
			Assert.That (png.Gamma, Is.EqualTo (1.8f).Within (0.001f), "Should handle gamma of 1.8");
		}

		[Test]
		public void PixelsPerMeterTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test various DPI equivalents
			// 72 DPI = ~2835 pixels per meter
			png.XPixelsPerMeter = 2835;
			png.YPixelsPerMeter = 2835;
			Assert.That (png.XPixelsPerMeter, Is.EqualTo (2835), "Should handle 72 DPI equivalent");
			Assert.That (png.YPixelsPerMeter, Is.EqualTo (2835), "Should handle 72 DPI equivalent");

			// Different X and Y resolutions
			png.XPixelsPerMeter = 3780; // ~96 DPI
			png.YPixelsPerMeter = 2835; // ~72 DPI
			Assert.That (png.XPixelsPerMeter, Is.EqualTo (3780), "X and Y resolutions can be different");
			Assert.That (png.YPixelsPerMeter, Is.EqualTo (2835), "X and Y resolutions can be different");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test that nullable properties can be set to null
			png.Author = null;
			Assert.That (png.Author, Is.Null, "Author should be nullable");

			png.Description = null;
			Assert.That (png.Description, Is.Null, "Description should be nullable");

			png.Gamma = null;
			Assert.That (png.Gamma, Is.Null, "Gamma should be nullable");

			png.XPixelsPerMeter = null;
			Assert.That (png.XPixelsPerMeter, Is.Null, "XPixelsPerMeter should be nullable");
		}

		[Test]
		public void EmptyStringPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test that empty strings work correctly
			png.Author = "";
			Assert.That (png.Author, Is.EqualTo (""), "Author should accept empty strings");

			png.Title = "";
			Assert.That (png.Title, Is.EqualTo (""), "Title should accept empty strings");

			png.Software = "";
			Assert.That (png.Software, Is.EqualTo (""), "Software should accept empty strings");
		}

		[Test]
		public void UnicodeStringPropertiesTest ()
		{
			var png = new CGImagePropertiesPng ();

			// Test that unicode strings work correctly
			png.Author = "Test Author 測試";
			Assert.That (png.Author, Is.EqualTo ("Test Author 測試"), "Author should handle unicode");

			png.Title = "Тест Title";
			Assert.That (png.Title, Is.EqualTo ("Тест Title"), "Title should handle unicode");
		}
	}
}
