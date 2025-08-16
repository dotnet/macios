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
	public class CGImagePropertiesIptcTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var iptc = new CGImagePropertiesIptc ();
			Assert.That (iptc, Is.Not.Null, "Default constructor should create a valid instance");

			// Test setting and getting basic string properties
			iptc.Byline = "Test Photographer";
			Assert.That (iptc.Byline, Is.EqualTo ("Test Photographer"), "Byline property should be settable and gettable");

			iptc.BylineTitle = "Staff Photographer";
			Assert.That (iptc.BylineTitle, Is.EqualTo ("Staff Photographer"), "BylineTitle property should be settable and gettable");

			iptc.CaptionAbstract = "Test image caption";
			Assert.That (iptc.CaptionAbstract, Is.EqualTo ("Test image caption"), "CaptionAbstract property should be settable and gettable");

			iptc.City = "San Francisco";
			Assert.That (iptc.City, Is.EqualTo ("San Francisco"), "City property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			using var dict = new NSMutableDictionary ();
			var iptc = new CGImagePropertiesIptc (dict);
			Assert.That (iptc, Is.Not.Null, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access IPTC properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");

			using var url = NSUrl.FromFilename (file);
			using var ci = CIImage.FromUrl (url);
			var imageProps = ci.Properties;
			Assert.That (imageProps, Is.Not.Null, "Image properties should be available");

			// Note: The test image may not have IPTC data, so Iptc property could be null
			// This test mainly verifies the property access doesn't throw exceptions
			var iptc = imageProps.Iptc;
			// iptc may be null for PNG files without IPTC data, which is expected
			Assert.That (iptc, Is.Null, "IPTC data should be null for PNG files without IPTC data");
		}

		[Test]
		public void LocationPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();

			// Test location-related properties
			iptc.ContentLocationName = "Golden Gate Bridge";
			Assert.That (iptc.ContentLocationName, Is.EqualTo ("Golden Gate Bridge"), "ContentLocationName should be settable");

			iptc.CountryPrimaryLocationName = "United States";
			Assert.That (iptc.CountryPrimaryLocationName, Is.EqualTo ("United States"), "CountryPrimaryLocationName should be settable");
		}

		[Test]
		public void CopyrightAndCreditPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();

			// Test copyright and credit properties
			iptc.CopyrightNotice = "© 2023 Test Photographer";
			Assert.That (iptc.CopyrightNotice, Is.EqualTo ("© 2023 Test Photographer"), "CopyrightNotice should be settable");

			iptc.Credit = "Test News Agency";
			Assert.That (iptc.Credit, Is.EqualTo ("Test News Agency"), "Credit should be settable");

			iptc.Source = "Test Photo Source";
			Assert.That (iptc.Source, Is.EqualTo ("Test Photo Source"), "Source should be settable");

			iptc.WriterEditor = "Test Editor";
			Assert.That (iptc.WriterEditor, Is.EqualTo ("Test Editor"), "WriterEditor should be settable");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();

			// Test that nullable string properties can be set to null
			iptc.Byline = null;
			Assert.That (iptc.Byline, Is.Null, "Byline should be nullable");

			iptc.CaptionAbstract = null;
			Assert.That (iptc.CaptionAbstract, Is.Null, "CaptionAbstract should be nullable");

			iptc.City = null;
			Assert.That (iptc.City, Is.Null, "City should be nullable");
		}

		[Test]
		public void EmptyStringPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();

			// Test that empty strings work correctly
			iptc.Byline = "";
			Assert.That (iptc.Byline, Is.EqualTo (""), "Byline should accept empty strings");

			iptc.CopyrightNotice = "";
			Assert.That (iptc.CopyrightNotice, Is.EqualTo (""), "CopyrightNotice should accept empty strings");
		}
	}
}
