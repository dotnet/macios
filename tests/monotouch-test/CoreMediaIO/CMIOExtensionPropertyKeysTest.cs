#if HAS_COREMEDIAIO
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionPropertyKeysTest {

		[Test]
		public void ProviderName ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.ProviderName;
			Assert.IsNotNull (key, "ProviderName");
			Assert.IsNotEmpty (key.ToString (), "ProviderName value");
		}

		[Test]
		public void ProviderManufacturer ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.ProviderManufacturer;
			Assert.IsNotNull (key, "ProviderManufacturer");
			Assert.IsNotEmpty (key.ToString (), "ProviderManufacturer value");
		}

		[Test]
		public void DeviceModel ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.DeviceModel;
			Assert.IsNotNull (key, "DeviceModel");
		}

		[Test]
		public void DeviceIsSuspended ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.DeviceIsSuspended;
			Assert.IsNotNull (key, "DeviceIsSuspended");
		}

		[Test]
		public void StreamActiveFormatIndex ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.StreamActiveFormatIndex;
			Assert.IsNotNull (key, "StreamActiveFormatIndex");
		}

		[Test]
		public void StreamSinkBufferQueueSize ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var key = CMIOExtensionPropertyKeys.StreamSinkBufferQueueSize;
			Assert.IsNotNull (key, "StreamSinkBufferQueueSize");
		}

		[Test]
		public void AllPropertyKeys_NotNull ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			Assert.IsNotNull (CMIOExtensionPropertyKeys.DeviceTransportType, "DeviceTransportType");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.DeviceLinkedCoreAudioDeviceUid, "DeviceLinkedCoreAudioDeviceUid");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.DeviceCanBeDefaultInputDevice, "DeviceCanBeDefaultInputDevice");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.DeviceCanBeDefaultOutputDevice, "DeviceCanBeDefaultOutputDevice");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.StreamFrameDuration, "StreamFrameDuration");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.StreamMaxFrameDuration, "StreamMaxFrameDuration");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.StreamSinkBuffersRequiredForStartup, "StreamSinkBuffersRequiredForStartup");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.StreamSinkBufferUnderrunCount, "StreamSinkBufferUnderrunCount");
			Assert.IsNotNull (CMIOExtensionPropertyKeys.StreamSinkEndOfData, "StreamSinkEndOfData");
		}
	}
}
#endif // HAS_COREMEDIAIO
