// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using CoreMedia;
using CoreMediaIO;
using Xamarin.Utils;

namespace MonoTouchFixtures.CoreMediaIO {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ExtensionTest {
		[Test]
		public void PropertyState ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);
			using var min = NSNumber.FromInt32 (0);
			using var max = NSNumber.FromInt32 (10);
			using var attributes = new CMIOExtensionPropertyAttributes (min, max, [min, max], false);
			Assert.That (attributes.MinValue, Is.EqualTo (min), "Minimum");
			Assert.That (attributes.MaxValue, Is.EqualTo (max), "Maximum");
			Assert.That (attributes.ValidValues, Has.Length.EqualTo (2), "Valid values");
			Assert.That (attributes.IsReadOnly, Is.False, "Writable");
			using var state = new CMIOExtensionPropertyState (min, attributes);
			Assert.That (state.Value, Is.EqualTo (min), "Value");
			Assert.That (state.Attributes, Is.EqualTo (attributes), "Attributes");
			using var empty = CMIOExtensionPropertyState.Create (null);
			Assert.That (empty.Value, Is.Null, "Empty value");
			Assert.That (CMIOExtensionPropertyAttributes.ReadOnlyPropertyAttribute.IsReadOnly, Is.True, "Read-only");
		}

		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);
			using var dictionary = new NSDictionary<NSString, CMIOExtensionPropertyState> ();
			using var properties = new CMIOExtensionProviderProperties (dictionary);
			properties.Name = "Core Media I/O test provider";
			properties.Manufacturer = "Microsoft";
			Assert.That (properties.Name, Is.EqualTo ("Core Media I/O test provider"), "Name");
			Assert.That (properties.Manufacturer, Is.EqualTo ("Microsoft"), "Manufacturer");
			using var device = new CMIOExtensionDeviceProperties (dictionary);
			device.Model = "Test camera";
			Assert.That (device.Model, Is.EqualTo ("Test camera"), "Model");
			using var stream = new CMIOExtensionStreamProperties (dictionary);
			var duration = new CMTime (1, 30);
			using var timeDictionary = duration.ToDictionary ();
			stream.FrameDuration = timeDictionary;
			Assert.That (CMTime.FromDictionary (stream.FrameDuration), Is.EqualTo (duration), "Frame duration");
		}

		[Test]
		public void StreamFormat ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);
			using var description = new CMVideoFormatDescription (CMVideoCodecType.H264, new CMVideoDimensions (640, 480));
			var duration = new CMTime (1, 30);
			using var format = new CMIOExtensionStreamFormat (description, duration, duration, null);
			Assert.That (format.MinFrameDuration, Is.EqualTo (duration), "Minimum");
			Assert.That (format.MaxFrameDuration, Is.EqualTo (duration), "Maximum");
			Assert.That (format.FormatDescription.Handle, Is.EqualTo (description.Handle), "Format description");
			Assert.That (format.ValidFrameDurations, Is.Null, "Durations");
			using var output = new CMIOExtensionScheduledOutput (15, 1234);
			Assert.That (output.SequenceNumber, Is.EqualTo (15), "Sequence");
			Assert.That (output.HostTimeInNanoseconds, Is.EqualTo (1234), "Host time");
			using var identifier = new NSUuid ();
			using var configuration = new CMIOExtensionStreamCustomClockConfiguration ("Test clock", identifier, new CMTime (1, 1000), 10, 0);
			Assert.That (configuration.SourceIdentifier, Is.EqualTo (identifier), "Source");
			Assert.That (configuration.NumberOfEventsForRateSmoothing, Is.EqualTo (10), "Smoothing");
		}
	}
}

#endif
