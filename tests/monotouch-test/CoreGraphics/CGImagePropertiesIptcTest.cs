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
	public class CGImagePropertiesIptcTest {
		[Test]
		public void ConstructorAndBasicPropertiesTest ()
		{
			// Test default constructor
			var iptc = new CGImagePropertiesIptc ();
			Assert.IsNotNull (iptc, "Default constructor should create a valid instance");

			// Test setting and getting basic string properties
			iptc.Byline = "Test Photographer";
			Assert.AreEqual ("Test Photographer", iptc.Byline, "Byline property should be settable and gettable");

			iptc.BylineTitle = "Staff Photographer";
			Assert.AreEqual ("Staff Photographer", iptc.BylineTitle, "BylineTitle property should be settable and gettable");

			iptc.CaptionAbstract = "Test image caption";
			Assert.AreEqual ("Test image caption", iptc.CaptionAbstract, "CaptionAbstract property should be settable and gettable");

			iptc.City = "San Francisco";
			Assert.AreEqual ("San Francisco", iptc.City, "City property should be settable and gettable");
		}

		[Test]
		public void ConstructorWithDictionaryTest ()
		{
			var dict = new NSMutableDictionary ();
			var iptc = new CGImagePropertiesIptc (dict);
			Assert.IsNotNull (iptc, "Constructor with dictionary should create a valid instance");
		}

		[Test]
		public void IntegrationWithCGImagePropertiesTest ()
		{
			// Test that CGImageProperties can access IPTC properties
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url)) {
				var imageProps = ci.Properties;
				Assert.IsNotNull (imageProps, "Image properties should be available");
				
				// Note: The test image may not have IPTC data, so Iptc property could be null
				// This test mainly verifies the property access doesn't throw exceptions
				var iptc = imageProps.Iptc;
				// iptc may be null for PNG files without IPTC data, which is expected
			}
		}

		[Test]
		public void LocationPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();
			
			// Test location-related properties
			iptc.ContentLocationName = "Golden Gate Bridge";
			Assert.AreEqual ("Golden Gate Bridge", iptc.ContentLocationName, "ContentLocationName should be settable");
			
			iptc.CountryPrimaryLocationName = "United States";
			Assert.AreEqual ("United States", iptc.CountryPrimaryLocationName, "CountryPrimaryLocationName should be settable");
		}

		[Test]
		public void CopyrightAndCreditPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();
			
			// Test copyright and credit properties
			iptc.CopyrightNotice = "© 2023 Test Photographer";
			Assert.AreEqual ("© 2023 Test Photographer", iptc.CopyrightNotice, "CopyrightNotice should be settable");
			
			iptc.Credit = "Test News Agency";
			Assert.AreEqual ("Test News Agency", iptc.Credit, "Credit should be settable");
			
			iptc.Source = "Test Photo Source";
			Assert.AreEqual ("Test Photo Source", iptc.Source, "Source should be settable");
			
			iptc.WriterEditor = "Test Editor";
			Assert.AreEqual ("Test Editor", iptc.WriterEditor, "WriterEditor should be settable");
		}

		[Test]
		public void NullablePropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();
			
			// Test that nullable string properties can be set to null
			iptc.Byline = null;
			Assert.IsNull (iptc.Byline, "Byline should be nullable");
			
			iptc.CaptionAbstract = null;
			Assert.IsNull (iptc.CaptionAbstract, "CaptionAbstract should be nullable");
			
			iptc.City = null;
			Assert.IsNull (iptc.City, "City should be nullable");
		}

		[Test]
		public void EmptyStringPropertiesTest ()
		{
			var iptc = new CGImagePropertiesIptc ();
			
			// Test that empty strings work correctly
			iptc.Byline = "";
			Assert.AreEqual ("", iptc.Byline, "Byline should accept empty strings");
			
			iptc.CopyrightNotice = "";
			Assert.AreEqual ("", iptc.CopyrightNotice, "CopyrightNotice should accept empty strings");
		}
	}
}