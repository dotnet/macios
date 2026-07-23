using System;

using AudioToolbox;
using AudioUnit;
using AVFoundation;
using Foundation;
using ObjCRuntime;

namespace AudioUnitExtensionTest {
	[Register ("TestAudioUnitFactory")]
	public class TestAudioUnitFactory : NSObject, IAUAudioUnitFactory {
		public TestAudioUnitFactory (NativeHandle handle) : base (handle)
		{
		}

		public AUAudioUnit CreateAudioUnit (AudioComponentDescription desc, out NSError error)
		{
			error = null;
			return new TestAudioUnit (desc, out error);
		}

		[Export ("beginRequestWithExtensionContext:")]
		public void BeginRequestWithExtensionContext (NSExtensionContext context)
		{
		}
	}

	[Register ("TestAudioUnit")]
	public class TestAudioUnit : AUAudioUnit {
		AUAudioUnitBusArray inputBusArray;
		AUAudioUnitBusArray outputBusArray;

		public TestAudioUnit (AudioComponentDescription componentDescription, out NSError error)
			: base (componentDescription, AudioComponentInstantiationOptions.OutOfProcess, out error)
		{
			var format = new AVAudioFormat (44100, 2);
			var inputBus = new AUAudioUnitBus (format, out error);
			var outputBus = new AUAudioUnitBus (format, out error);
			inputBusArray = new AUAudioUnitBusArray (this, AUAudioUnitBusType.Input, new [] { inputBus });
			outputBusArray = new AUAudioUnitBusArray (this, AUAudioUnitBusType.Output, new [] { outputBus });
		}

		public TestAudioUnit (NativeHandle handle) : base (handle)
		{
		}

		public override AUAudioUnitBusArray InputBusses => inputBusArray;

		public override AUAudioUnitBusArray OutputBusses => outputBusArray;

		public override AUInternalRenderBlock InternalRenderBlock {
			get {
				return (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp,
						uint frameCount, nint outputBusNumber, AudioBuffers outputData,
						AURenderEventEnumerator realtimeEventListHead, AURenderPullInputBlock pullInputBlock) => {
					if (pullInputBlock is null)
						return AudioUnitStatus.NoError;
					pullInputBlock (ref actionFlags, ref timestamp, frameCount, 0, outputData);
					return AudioUnitStatus.NoError;
				};
			}
		}
	}
}
