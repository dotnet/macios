#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionProviderPropertiesTest {

		[Test]
		public void Create_EmptyDictionary ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionProviderProperties.Create (dict);
			Assert.IsNotNull (props, "Created properties");
			Assert.IsNotNull (props.PropertiesDictionary, "PropertiesDictionary");
		}

		[Test]
		public void Name_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionProviderProperties.Create (dict);
			Assert.IsNull (props.Name, "Initial Name");

			props.Name = "TestProvider";
			Assert.AreEqual ("TestProvider", props.Name, "Updated Name");
		}

		[Test]
		public void Manufacturer_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionProviderProperties.Create (dict);
			Assert.IsNull (props.Manufacturer, "Initial Manufacturer");

			props.Manufacturer = "TestManufacturer";
			Assert.AreEqual ("TestManufacturer", props.Manufacturer, "Updated Manufacturer");
		}
	}
}
#endif // HAS_COREMEDIAIO
