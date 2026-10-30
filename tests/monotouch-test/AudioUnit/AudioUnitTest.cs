//
// Unit tests for AudioUnit
//

using AudioUnit;
using AudioToolbox;

namespace MonoTouchFixtures.AudioUnit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioUnitTest {
		[Test]
		public unsafe void ReadonlyRenderInputs ()
		{
			using var graph = new AUGraph ();
			var node = graph.AddNode (AudioComponentDescription.CreateConverter (AudioTypeConverter.AU));
			graph.Open ();
			using var converter = graph.GetNodeInfo (node);
			var format = AudioStreamBasicDescription.CreateLinearPCM (44100, 1, 32);
			format.FormatFlags = AudioFormatFlags.IsFloat | AudioFormatFlags.IsPacked;
			format.BytesPerPacket = format.BytesPerFrame = sizeof (float);
			var expectedFormat = format;
#if XAMCORE_5_0
			Assert.That (converter.SetFormat (in format, AudioUnitScopeType.Input), Is.EqualTo (AudioUnitStatus.OK), "Input format");
			Assert.That (converter.SetFormat (in format, AudioUnitScopeType.Output), Is.EqualTo (AudioUnitStatus.OK), "Output format");
#else
			Assert.That (converter.SetFormat (format, AudioUnitScopeType.Input), Is.EqualTo (AudioUnitStatus.OK), "Input format");
			Assert.That (converter.SetFormat (format, AudioUnitScopeType.Output), Is.EqualTo (AudioUnitStatus.OK), "Output format");
#endif
			Assert.That (converter.GetAudioFormat (AudioUnitScopeType.Output), Is.EqualTo (format), "Format roundtrip");
			Assert.That (format, Is.EqualTo (expectedFormat), "Format input");
			var timestamp = new AudioTimeStamp {
				SampleTime = 1234,
				Flags = AudioTimeStamp.AtsFlags.SampleTimeValid,
			};
			var expectedTimestamp = timestamp;
			var called = false;
			AudioTimeStamp receivedTimestamp = default;
#if XAMCORE_5_0
			RenderDelegate callback = (AudioUnitRenderActionFlags flags, in AudioTimeStamp time, uint bus, uint frames, AudioBuffers data) => {
#else
			RenderDelegate callback = (AudioUnitRenderActionFlags flags, AudioTimeStamp time, uint bus, uint frames, AudioBuffers data) => {
#endif
				called = true;
				receivedTimestamp = time;
				for (int b = 0; b < data.Count; b++) {
					var samples = new Span<float> ((void*) data [b].Data, (int) (data [b].DataByteSize / sizeof (float)));
					samples.Fill (0.25f);
				}
				return AudioUnitStatus.OK;
			};
			Assert.That (converter.SetRenderCallback (callback, AudioUnitScopeType.Input), Is.EqualTo (AudioUnitStatus.OK), "Callback");
			Assert.That (converter.Initialize (), Is.EqualTo (AudioUnitStatus.OK), "Initialize");
			try {
				float* samples = stackalloc float [32];
				using var buffers = new AudioBuffers (1);
				buffers [0] = new AudioBuffer {
					NumberChannels = 1,
					DataByteSize = 32 * sizeof (float),
					Data = (IntPtr) samples,
				};
				var flags = (AudioUnitRenderActionFlags) 0;
#if XAMCORE_5_0
				var status = converter.Render (ref flags, in timestamp, 0, 32, buffers);
#else
				var status = converter.Render (ref flags, timestamp, 0, 32, buffers);
#endif
				Assert.That (status, Is.EqualTo (AudioUnitStatus.OK), "Render");
				Assert.That (called, Is.True, "Called");
				Assert.That (receivedTimestamp.SampleTime, Is.EqualTo (timestamp.SampleTime), "Callback timestamp");
				for (int i = 0; i < 32; i++)
					Assert.That (samples [i], Is.EqualTo (0.25f).Within (0.0001f), $"Sample {i}");
				Assert.That (timestamp, Is.EqualTo (expectedTimestamp), "Timestamp input");
			} finally {
				converter.Uninitialize ();
			}

#if XAMCORE_5_0
			InputDelegate input = (AudioUnitRenderActionFlags flags, in AudioTimeStamp time, uint bus, uint frames, global::AudioUnit.AudioUnit unit) => {
#else
			InputDelegate input = (AudioUnitRenderActionFlags flags, AudioTimeStamp time, uint bus, uint frames, global::AudioUnit.AudioUnit unit) => {
#endif
				Assert.That (time, Is.EqualTo (expectedTimestamp), "Input callback timestamp");
				Assert.That (unit, Is.SameAs (converter), "Input callback unit");
				return AudioUnitStatus.OK;
			};
#if XAMCORE_5_0
			Assert.That (input (0, in timestamp, 0, 32, converter), Is.EqualTo (AudioUnitStatus.OK), "Input callback");
#else
			Assert.That (input (0, timestamp, 0, 32, converter), Is.EqualTo (AudioUnitStatus.OK), "Input callback");
#endif
			Assert.That (timestamp, Is.EqualTo (expectedTimestamp), "Input callback input");
		}

		[Test]
		public void ScheduleTimestampInput ()
		{
			var description = AudioComponentDescription.CreateGenerator (AudioTypeGenerator.ScheduledSoundPlayer);
#if XAMCORE_5_0
			var component = AudioComponent.FindComponent (in description);
#else
			var component = AudioComponent.FindComponent (ref description);
#endif
			using var unit = component.CreateAudioUnit ();
			var timestamp = new AudioTimeStamp {
				SampleTime = -1,
				Flags = AudioTimeStamp.AtsFlags.SampleTimeValid,
			};
			var expected = timestamp;
#if XAMCORE_5_0
			var status = unit.SetScheduleStartTimeStamp (in timestamp);
#else
			var status = unit.SetScheduleStartTimeStamp (timestamp);
#endif
			Assert.That (status, Is.EqualTo (AudioUnitStatus.OK), "Schedule");
			Assert.That (timestamp, Is.EqualTo (expected), "Input");
		}

		[Test]
		public void DisposeMethodTest ()
		{
			// Test case from bxc #5410

			// Create instance of AudioUnit object
			AudioComponentDescription cd = new AudioComponentDescription () {
				ComponentType = AudioComponentType.Output,
#if MONOMAC
				ComponentSubType = AudioUnitSubType.VoiceProcessingIO,
#else
				ComponentSubType = (AudioUnitSubType) AudioTypeOutput.Remote,
#endif
				ComponentManufacturer = AudioComponentManufacturerType.Apple
			};
#if XAMCORE_5_0
			AudioComponent component = AudioComponent.FindComponent (in cd);
#else
			AudioComponent component = AudioComponent.FindComponent (ref cd);
#endif
			var audioUnit = component.CreateAudioUnit ();

			audioUnit.Dispose ();
		}

		[Test]
		public void GetElementCount ()
		{
			var graph = new AUGraph ();
			var mixerNode = graph.AddNode (AudioComponentDescription.CreateMixer (AudioTypeMixer.MultiChannel));
			graph.Open ();
			var mixer = graph.GetNodeInfo (mixerNode);
			Assert.That (mixer.GetElementCount (AudioUnitScopeType.Global), Is.EqualTo (1));
		}

		[Test]
		public void CopyIconTest ()
		{
			TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);
			AudioComponentDescription cd = new AudioComponentDescription () {
				ComponentType = AudioComponentType.Output,
#if MONOMAC
				ComponentSubType = AudioUnitSubType.VoiceProcessingIO,
#else
				ComponentSubType = (AudioUnitSubType) AudioTypeOutput.Remote,
#endif
				ComponentManufacturer = AudioComponentManufacturerType.Apple
			};
#if XAMCORE_5_0
			AudioComponent component = AudioComponent.FindComponent (in cd);
#else
			AudioComponent component = AudioComponent.FindComponent (ref cd);
#endif
			Assert.DoesNotThrow (() => {
				var icon = component.CopyIcon (); // ensuring that the manual binding does not throw, we do not care about the result
			});
		}

		[Test]
		public unsafe void TestSizeOf ()
		{
			Assert.That (Marshal.SizeOf<AudioFormat> (), Is.EqualTo (sizeof (AudioFormat)));
			Assert.That (Marshal.SizeOf<AudioValueRange> (), Is.EqualTo (sizeof (AudioValueRange)));
			Assert.That (Marshal.SizeOf<AudioClassDescription> (), Is.EqualTo (sizeof (AudioClassDescription)));
		}
	}
}
