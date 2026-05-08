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
			AudioComponent component = AudioComponent.FindComponent (ref cd);
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
			AudioComponent component = AudioComponent.FindComponent (ref cd);
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
