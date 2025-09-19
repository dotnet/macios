// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AVFoundation;
using Foundation;
using ObjCRuntime;

namespace BrowserEngineCore {
	[NoMacCatalyst, NoTV, NoMac, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BEAudioSession {
		[Export ("initWithAudioSession:")]
		NativeHandle Constructor (AVAudioSession audioSession);

		[NullAllowed, Export ("availableOutputs")]
		AVAudioSessionPortDescription [] AvailableOutputs { get; }

		[Export ("setPreferredOutput:error:")]
		bool SetPreferredOutput ([NullAllowed] AVAudioSessionPortDescription outPort, [NullAllowed] out NSError outError);

		[NullAllowed, Export ("preferredOutput")]
		AVAudioSessionPortDescription PreferredOutput { get; }
	}
}
