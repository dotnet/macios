//
// Unit tests for AUAudioUnitFactory
//
// Authors:
//	Oleg Demchenko (oleg.demchenko@xamarin.com)
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;

using NUnit.Framework;

using Foundation;
using AudioUnit;

namespace MonoTouchFixtures.AudioUnit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AUAudioUnitFactoryTest {
		[Test]
		public void CreateAudioUnit ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const string expectedManufacturer = "Apple";
			var desc = new AudioComponentDescription {
				ComponentType = AudioComponentType.Output,
#if MONOMAC
				ComponentSubType = AudioUnitSubType.VoiceProcessingIO,
#else
				ComponentSubType = (AudioUnitSubType) AudioTypeOutput.Remote,
#endif
				ComponentManufacturer = AudioComponentManufacturerType.Apple
			};

			using (var auFactory = new CustomAudioUnitFactory ()) {
				NSError error;
				using (var audioUnit = auFactory.CreateAudioUnit (desc, out error)) {
					Assert.True (audioUnit is not null, "CustomAudioUnitFactory returned null object for valid component description");
					Assert.True (audioUnit.ManufacturerName == expectedManufacturer,
						$"CustomAudioUnitFactory returned audio unit with incorrect manufacturer. Expected - {expectedManufacturer}, actual - {audioUnit.ManufacturerName}");
				}
			}
		}

		public class CustomAudioUnitFactory : NSObject, IAUAudioUnitFactory {
			public AUAudioUnit CreateAudioUnit (AudioComponentDescription desc, out NSError error)
			{
				var audioUnit = new AUAudioUnit (desc, out error);
				return audioUnit;
			}

			public void BeginRequestWithExtensionContext (NSExtensionContext context)
			{
				throw new NotImplementedException ();
			}
		}
	}
}
