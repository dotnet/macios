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
	public class CGImagePropertiesGPSTest {
		[Test]
		public void LongitudeRefAndLatitudeRefTest ()
		{
			float expectedLatitude = 47.64248f;
			float expectedLongitude = 122.136986f;
			string expectedLatitudeRef = "N";
			string expectedLongitudeRef = "W";
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08_with_loc.png");

			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var gps = ci.Properties.Gps;
				Assert.AreEqual (expectedLatitude, gps.Latitude, 0.0001f, "Invalid or no Latitude value found.");
				Assert.AreEqual (expectedLongitude, gps.Longitude, 0.0001f, "Invalid or no Longitude value found.");
				Assert.AreEqual (expectedLatitudeRef, gps.LatitudeRef, "Invalid or no LatitudeRef value found.");
				Assert.AreEqual (expectedLongitudeRef, gps.LongitudeRef, "Invalid or no LongitudeRef value found.");
			}
		}

		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var gps = new CGImagePropertiesGps ();
			Assert.IsNotNull (gps, "Default constructor should create a valid instance");

			// Test setting and getting coordinate properties
			gps.Latitude = 37.7749f; // San Francisco latitude
			Assert.AreEqual (37.7749f, gps.Latitude, 0.0001f, "Latitude property should be settable and gettable");

			gps.Longitude = -122.4194f; // San Francisco longitude
			Assert.AreEqual (-122.4194f, gps.Longitude, 0.0001f, "Longitude property should be settable and gettable");

			gps.LatitudeRef = "N";
			Assert.AreEqual ("N", gps.LatitudeRef, "LatitudeRef property should be settable and gettable");

			gps.LongitudeRef = "W";
			Assert.AreEqual ("W", gps.LongitudeRef, "LongitudeRef property should be settable and gettable");

			gps.Altitude = 100;
			Assert.AreEqual (100, gps.Altitude, "Altitude property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var gps = new CGImagePropertiesGps (dict);
			Assert.IsNotNull (gps, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access GPS properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// Note: The regular PNG may not have GPS data, so Gps property could be null
				// This test mainly verifies the property access doesn't throw exceptions
				var gps = imageProps.Gps;
				// gps may be null for PNG files without GPS data, which is expected
			}
		}

		[Test]
		public void NegativeCoordinatesTest ()
		{
			var gps = new CGImagePropertiesGps ();
			
			// Test negative coordinates (southern hemisphere and western longitude)
			gps.Latitude = -33.8688f; // Sydney latitude
			gps.Longitude = 151.2093f; // Sydney longitude
			Assert.AreEqual (-33.8688f, gps.Latitude, 0.0001f, "Should handle negative latitude");
			Assert.AreEqual (151.2093f, gps.Longitude, 0.0001f, "Should handle positive longitude");
			
			gps.LatitudeRef = "S";
			gps.LongitudeRef = "E";
			Assert.AreEqual ("S", gps.LatitudeRef, "Should handle southern hemisphere");
			Assert.AreEqual ("E", gps.LongitudeRef, "Should handle eastern longitude");
		}

		[Test]
		public void AltitudeTest ()
		{
			var gps = new CGImagePropertiesGps ();
			
			// Test various altitude values
			gps.Altitude = 0; // Sea level
			Assert.AreEqual (0, gps.Altitude, "Should handle sea level altitude");
			
			gps.Altitude = 8849; // Mount Everest height in meters
			Assert.AreEqual (8849, gps.Altitude, "Should handle high altitude");
			
			gps.Altitude = -400; // Below sea level
			Assert.AreEqual (-400, gps.Altitude, "Should handle negative altitude");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var gps = new CGImagePropertiesGps ();
			
			// Test that nullable properties can be set to null
			gps.Latitude = null;
			Assert.IsNull (gps.Latitude, "Latitude should be nullable");
			
			gps.Longitude = null;
			Assert.IsNull (gps.Longitude, "Longitude should be nullable");
			
			gps.Altitude = null;
			Assert.IsNull (gps.Altitude, "Altitude should be nullable");
			
			gps.LatitudeRef = null;
			Assert.IsNull (gps.LatitudeRef, "LatitudeRef should be nullable");
			
			gps.LongitudeRef = null;
			Assert.IsNull (gps.LongitudeRef, "LongitudeRef should be nullable");
		}

		[Test]
		public void EdgeCaseCoordinatesTest ()
		{
			var gps = new CGImagePropertiesGps ();
			
			// Test edge case coordinates
			gps.Latitude = 90.0f; // North pole
			gps.Longitude = 180.0f; // International date line
			Assert.AreEqual (90.0f, gps.Latitude, 0.0001f, "Should handle north pole latitude");
			Assert.AreEqual (180.0f, gps.Longitude, 0.0001f, "Should handle international date line longitude");
			
			gps.Latitude = -90.0f; // South pole
			gps.Longitude = -180.0f; // International date line (west)
			Assert.AreEqual (-90.0f, gps.Latitude, 0.0001f, "Should handle south pole latitude");
			Assert.AreEqual (-180.0f, gps.Longitude, 0.0001f, "Should handle international date line longitude (west)");
		}
	}
}
