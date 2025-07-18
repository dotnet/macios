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
	public class CGImagePropertiesGPSTest {
		[Test]
		public void LongitudeRefAndLatitudeRefTest ()
		{
			float expectedLatitude = 47.64248f;
			float expectedLongitude = 122.136986f;
			string expectedLatitudeRef = "N";
			string expectedLongitudeRef = "W";
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08_with_loc.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var gps = ci.Properties.Gps;
			Assert.That (gps.Latitude, Is.EqualTo (expectedLatitude).Within (0.0001f), "Invalid or no Latitude value found.");
			Assert.That (gps.Longitude, Is.EqualTo (expectedLongitude).Within (0.0001f), "Invalid or no Longitude value found.");
			Assert.That (gps.LatitudeRef, Is.EqualTo (expectedLatitudeRef), "Invalid or no LatitudeRef value found.");
			Assert.That (gps.LongitudeRef, Is.EqualTo (expectedLongitudeRef), "Invalid or no LongitudeRef value found.");
		}

		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var gps = new CGImagePropertiesGps ();
			Assert.That (gps, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting coordinate properties
			gps.Latitude = 37.7749f; // San Francisco latitude
			Assert.That (gps.Latitude, Is.EqualTo (37.7749f).Within (0.0001f), "Latitude property should be settable and gettable");

			gps.Longitude = -122.4194f; // San Francisco longitude
			Assert.That (gps.Longitude, Is.EqualTo (-122.4194f).Within (0.0001f), "Longitude property should be settable and gettable");

			gps.LatitudeRef = "N";
			Assert.That (gps.LatitudeRef, Is.EqualTo ("N"), "LatitudeRef property should be settable and gettable");

			gps.LongitudeRef = "W";
			Assert.That (gps.LongitudeRef, Is.EqualTo ("W"), "LongitudeRef property should be settable and gettable");

			gps.Altitude = 100;
			Assert.That (gps.Altitude, Is.EqualTo (100), "Altitude property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var gps = new CGImagePropertiesGps (dict);
			Assert.That (gps, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access GPS properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// Note: The regular PNG may not have GPS data, so Gps property could be null
			// This test mainly verifies the property access doesn't throw exceptions
			var gps = imageProps.Gps;
			// gps may be null for PNG files without GPS data, which is expected
			Assert.That (gps, Is.Null, "GPS data should be null for PNG files without GPS data");
		}

		[Test]
		public void NegativeCoordinatesTest ()
		{
			var gps = new CGImagePropertiesGps ();

			// Test negative coordinates (southern hemisphere and western longitude)
			gps.Latitude = -33.8688f; // Sydney latitude
			gps.Longitude = 151.2093f; // Sydney longitude
			Assert.That (gps.Latitude, Is.EqualTo (-33.8688f).Within (0.0001f), "Should handle negative latitude");
			Assert.That (gps.Longitude, Is.EqualTo (151.2093f).Within (0.0001f), "Should handle positive longitude");

			gps.LatitudeRef = "S";
			gps.LongitudeRef = "E";
			Assert.That (gps.LatitudeRef, Is.EqualTo ("S"), "Should handle southern hemisphere");
			Assert.That (gps.LongitudeRef, Is.EqualTo ("E"), "Should handle eastern longitude");
		}

		[Test]
		public void AltitudeTest ()
		{
			var gps = new CGImagePropertiesGps ();

			// Test various altitude values
			gps.Altitude = 0; // Sea level
			Assert.That (gps.Altitude, Is.EqualTo (0), "Should handle sea level altitude");

			gps.Altitude = 8849; // Mount Everest height in meters
			Assert.That (gps.Altitude, Is.EqualTo (8849), "Should handle high altitude");

			gps.Altitude = -400; // Below sea level
			Assert.That (gps.Altitude, Is.EqualTo (-400), "Should handle negative altitude");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var gps = new CGImagePropertiesGps ();

			// Test that nullable properties can be set to null
			gps.Latitude = null;
			Assert.That (gps.Latitude, Is.Null, "Latitude should be nullable");

			gps.Longitude = null;
			Assert.That (gps.Longitude, Is.Null, "Longitude should be nullable");

			gps.Altitude = null;
			Assert.That (gps.Altitude, Is.Null, "Altitude should be nullable");

			gps.LatitudeRef = null;
			Assert.That (gps.LatitudeRef, Is.Null, "LatitudeRef should be nullable");

			gps.LongitudeRef = null;
			Assert.That (gps.LongitudeRef, Is.Null, "LongitudeRef should be nullable");
		}

		[Test]
		public void EdgeCaseCoordinatesTest ()
		{
			var gps = new CGImagePropertiesGps ();

			// Test edge case coordinates
			gps.Latitude = 90.0f; // North pole
			gps.Longitude = 180.0f; // International date line
			Assert.That (gps.Latitude, Is.EqualTo (90.0f).Within (0.0001f), "Should handle north pole latitude");
			Assert.That (gps.Longitude, Is.EqualTo (180.0f).Within (0.0001f), "Should handle international date line longitude");

			gps.Latitude = -90.0f; // South pole
			gps.Longitude = -180.0f; // International date line (west)
			Assert.That (gps.Latitude, Is.EqualTo (-90.0f).Within (0.0001f), "Should handle south pole latitude");
			Assert.That (gps.Longitude, Is.EqualTo (-180.0f).Within (0.0001f), "Should handle international date line longitude (west)");
		}
	}
}

