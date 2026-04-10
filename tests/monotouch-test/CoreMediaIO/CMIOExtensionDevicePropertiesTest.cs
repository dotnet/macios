#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionDevicePropertiesTest {

		[Test]
		public void Create_EmptyDictionary ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionDeviceProperties.Create (dict);
			Assert.IsNotNull (props, "Created properties");
		}

		[Test]
		public void Model_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionDeviceProperties.Create (dict);
			Assert.IsNull (props.Model, "Initial Model");

			props.Model = "TestModel";
			Assert.AreEqual ("TestModel", props.Model, "Updated Model");
		}

		[Test]
		public void Suspended_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionDeviceProperties.Create (dict);
			Assert.IsNull (props.Suspended, "Initial Suspended");

			props.Suspended = NSNumber.FromBoolean (true);
			Assert.IsNotNull (props.Suspended, "Updated Suspended");
			Assert.AreEqual (1, props.Suspended!.Int32Value, "Suspended value");
		}

		[Test]
		public void LinkedCoreAudioDeviceUid_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionDeviceProperties.Create (dict);
			Assert.IsNull (props.LinkedCoreAudioDeviceUid, "Initial LinkedCoreAudioDeviceUid");

			props.LinkedCoreAudioDeviceUid = "com.test.audio.device";
			Assert.AreEqual ("com.test.audio.device", props.LinkedCoreAudioDeviceUid, "Updated LinkedCoreAudioDeviceUid");
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
