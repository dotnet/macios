using System;
using System.Threading.Tasks;
using NUnit.Framework;

#if !XAMCORE_2_0
using MonoMac.AppKit;
using MonoMac.AudioUnit;
using MonoMac.AudioToolbox;
#else
using AppKit;
using AudioUnit;
using AudioToolbox;
#endif

namespace Xamarin.Mac.Tests
{
	[TestFixture]
	public class AUGraphTests
	{
		int graphRenderCallbackCount = 0;
		int mixerRenderCallbackCount = 0;
		AUGraph graph;
#if XAMCORE_2_0
		AudioUnit.AudioUnit mMixer;
#else
		AudioUnit mMixer;
#endif

		void SetupAUGraph ()
		{
			graph = new AUGraph ();

			AudioComponentDescription mixerDescription = new AudioComponentDescription ();
			mixerDescription.ComponentType = AudioComponentType.Mixer;
			mixerDescription.ComponentSubType = (int)AudioTypeMixer.MultiChannel;
			mixerDescription.ComponentFlags = 0;
			mixerDescription.ComponentFlagsMask = 0;
			mixerDescription.ComponentManufacturer = AudioComponentManufacturerType.Apple;

			AudioComponentDescription outputDesciption = new AudioComponentDescription ();
			outputDesciption.ComponentType = AudioComponentType.Output;
			outputDesciption.ComponentSubType = (int)AudioTypeOutput.System;
			outputDesciption.ComponentFlags = 0;
			outputDesciption.ComponentFlagsMask = 0;
			outputDesciption.ComponentManufacturer = AudioComponentManufacturerType.Apple;

			int mixerNode = graph.AddNode (mixerDescription);
			int outputNode = graph.AddNode (outputDesciption);

			AUGraphError error = graph.ConnnectNodeInput (mixerNode, 0, outputNode, 0);
			Assert.AreEqual (AUGraphError.OK, error);

			graph.Open ();

			mMixer = graph.GetNodeInfo (mixerNode);

			AudioUnitStatus status = mMixer.SetElementCount (AudioUnitScopeType.Input, 0);
			Assert.AreEqual (AudioUnitStatus.OK, status);
		}

		[Test]
		public async Task DoTest ()
		{
			SetupAUGraph ();

			// One of these has to be commented out depending on old\new build
			graph.AddRenderNotify (GraphRenderCallback);
			//graph.RenderCallback += HandleRenderCallback;

			AudioUnitStatus status = mMixer.SetRenderCallback (MixerRenderCallback);
			Assert.AreEqual (AudioUnitStatus.OK, status );

			await WaitOnGraphAndMixerCallbacks ();
		}

#if !XAMCORE_2_0
#pragma warning disable 0612
		void HandleRenderCallback (object sender, AudioGraphEventArgs e)
		{
			graphRenderCallbackCount++;
		}
#pragma warning restore 0612
#endif

		AudioUnitStatus GraphRenderCallback (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data)
		{
			graphRenderCallbackCount++;
			return AudioUnitStatus.NoError;
		}

		AudioUnitStatus MixerRenderCallback (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data)
		{
			mixerRenderCallbackCount++;
			return AudioUnitStatus.NoError;
		}

		async Task WaitOnGraphAndMixerCallbacks ()
		{
			graph.Initialize ();
			graph.Start ();

			// Wait for 1 second, then give up
			try {
				for (int i = 0; i < 100; ++i) {
					if (graphRenderCallbackCount > 0 && mixerRenderCallbackCount > 0)
						return;
					await Task.Delay (10);
				}
				Assert.Fail ("Did not see events after 1 second");
			}
			finally {
				graph.Stop ();
			}
		}
	}
}

