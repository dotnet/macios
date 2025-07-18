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
	public class CGImagePropertiesJfifTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var jfif = new CGImagePropertiesJfif ();
			Assert.That (jfif, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting density properties
			jfif.XDensity = 72;
			Assert.That (jfif.XDensity, Is.EqualTo (72), "XDensity property should be settable and gettable");

			jfif.YDensity = 72;
			Assert.That (jfif.YDensity, Is.EqualTo (72), "YDensity property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var jfif = new CGImagePropertiesJfif (dict);
			Assert.That (jfif, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access JFIF properties
			var file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// Note: The test image is PNG, so JFIF property will likely be null
			// This test mainly verifies the property access doesn't throw exceptions
			var jfif = imageProps.Jfif;
			// jfif will be null for PNG files, which is expected
			Assert.That (jfif, Is.Null, "JFIF data should be null for PNG files");
		}

		[Test]
		public void DensityValuesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();

			// Test common DPI values
			jfif.XDensity = 300;
			jfif.YDensity = 300;
			Assert.That (jfif.XDensity, Is.EqualTo (300), "Should handle high DPI values");
			Assert.That (jfif.YDensity, Is.EqualTo (300), "Should handle high DPI values");

			// Test different X and Y densities
			jfif.XDensity = 96;
			jfif.YDensity = 72;
			Assert.That (jfif.XDensity, Is.EqualTo (96), "X and Y densities can be different");
			Assert.That (jfif.YDensity, Is.EqualTo (72), "X and Y densities can be different");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();

			// Test that nullable properties can be set to null
			jfif.XDensity = null;
			Assert.That (jfif.XDensity, Is.Null, "XDensity should be nullable");

			jfif.YDensity = null;
			Assert.That (jfif.YDensity, Is.Null, "YDensity should be nullable");
		}

		[Test]
		public void ZeroDensityTest ()
		{
			var jfif = new CGImagePropertiesJfif ();

			// Test edge case of zero density
			jfif.XDensity = 0;
			jfif.YDensity = 0;
			Assert.That (jfif.XDensity, Is.EqualTo (0), "Should handle zero density");
			Assert.That (jfif.YDensity, Is.EqualTo (0), "Should handle zero density");
		}

		[Test]
		public void LargeDensityValuesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();

			// Test large density values
			jfif.XDensity = 9999;
			jfif.YDensity = 9999;
			Assert.That (jfif.XDensity, Is.EqualTo (9999), "Should handle large density values");
			Assert.That (jfif.YDensity, Is.EqualTo (9999), "Should handle large density values");
		}
	}
}
