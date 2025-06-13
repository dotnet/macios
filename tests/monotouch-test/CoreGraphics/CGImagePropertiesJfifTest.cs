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
	public class CGImagePropertiesJfifTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var jfif = new CGImagePropertiesJfif ();
			Assert.IsNotNull (jfif, "Default constructor should create a valid instance");

			// Test setting and getting density properties
			jfif.XDensity = 72;
			Assert.AreEqual (72, jfif.XDensity, "XDensity property should be settable and gettable");

			jfif.YDensity = 72;
			Assert.AreEqual (72, jfif.YDensity, "YDensity property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var jfif = new CGImagePropertiesJfif (dict);
			Assert.IsNotNull (jfif, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access JFIF properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// Note: The test image is PNG, so JFIF property will likely be null
				// This test mainly verifies the property access doesn't throw exceptions
				var jfif = imageProps.Jfif;
				// jfif will be null for PNG files, which is expected
			}
		}

		[Test]
		public void DensityValuesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();
			
			// Test common DPI values
			jfif.XDensity = 300;
			jfif.YDensity = 300;
			Assert.AreEqual (300, jfif.XDensity, "Should handle high DPI values");
			Assert.AreEqual (300, jfif.YDensity, "Should handle high DPI values");
			
			// Test different X and Y densities
			jfif.XDensity = 96;
			jfif.YDensity = 72;
			Assert.AreEqual (96, jfif.XDensity, "X and Y densities can be different");
			Assert.AreEqual (72, jfif.YDensity, "X and Y densities can be different");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();
			
			// Test that nullable properties can be set to null
			jfif.XDensity = null;
			Assert.IsNull (jfif.XDensity, "XDensity should be nullable");
			
			jfif.YDensity = null;
			Assert.IsNull (jfif.YDensity, "YDensity should be nullable");
		}

		[Test]
		public void ZeroDensityTest ()
		{
			var jfif = new CGImagePropertiesJfif ();
			
			// Test edge case of zero density
			jfif.XDensity = 0;
			jfif.YDensity = 0;
			Assert.AreEqual (0, jfif.XDensity, "Should handle zero density");
			Assert.AreEqual (0, jfif.YDensity, "Should handle zero density");
		}

		[Test]
		public void LargeDensityValuesTest ()
		{
			var jfif = new CGImagePropertiesJfif ();
			
			// Test large density values
			jfif.XDensity = 9999;
			jfif.YDensity = 9999;
			Assert.AreEqual (9999, jfif.XDensity, "Should handle large density values");
			Assert.AreEqual (9999, jfif.YDensity, "Should handle large density values");
		}
	}
}