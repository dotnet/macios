#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionStreamPropertiesTest {

		[Test]
		public void Create_EmptyDictionary ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionStreamProperties.Create (dict);
			Assert.IsNotNull (props, "Created properties");
			Assert.IsNotNull (props.PropertiesDictionary, "PropertiesDictionary");
		}

		[Test]
		public void ActiveFormatIndex_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionStreamProperties.Create (dict);
			Assert.IsNull (props.ActiveFormatIndex, "Initial ActiveFormatIndex");

			props.ActiveFormatIndex = NSNumber.FromInt32 (2);
			Assert.IsNotNull (props.ActiveFormatIndex, "Updated ActiveFormatIndex");
			Assert.AreEqual (2, props.ActiveFormatIndex!.Int32Value, "ActiveFormatIndex value");
		}

		[Test]
		public void SinkBufferQueueSize_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionStreamProperties.Create (dict);
			Assert.IsNull (props.SinkBufferQueueSize, "Initial SinkBufferQueueSize");

			props.SinkBufferQueueSize = NSNumber.FromInt32 (10);
			Assert.IsNotNull (props.SinkBufferQueueSize, "Updated SinkBufferQueueSize");
			Assert.AreEqual (10, props.SinkBufferQueueSize!.Int32Value, "SinkBufferQueueSize value");
		}

		[Test]
		public void SetPropertyState ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			using var dict = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			var props = CMIOExtensionStreamProperties.Create (dict);

			using var value = NSNumber.FromInt32 (5);
			var state = CMIOExtensionPropertyState.Create (value);

			Assert.DoesNotThrow (() =>
				props.SetPropertyState (state, CMIOExtensionPropertyKeys.StreamActiveFormatIndex),
				"SetPropertyState should not throw");
		}
	}
}
#endif // HAS_COREMEDIAIO
