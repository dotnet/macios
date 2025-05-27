// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Classes;

public class ClassGenerationTests : BaseGeneratorTestClass {

	public class TestDataGenerator : BaseTestDataGenerator, IEnumerable<object []> {
		readonly List<(ApplePlatform Platform, string ClassName, string BindingFile, string OutputFile, string? LibraryText, string? TrampolinesText)> _data = new ()
		{
			//(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			//(ApplePlatform.TVOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			//(ApplePlatform.MacCatalyst, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			//(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			//(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs", null, null),
			//(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs", null, null),
			//(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs", null, null),
			//(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs", null, null),
			//(ApplePlatform.iOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			//(ApplePlatform.TVOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			//(ApplePlatform.MacCatalyst, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			//(ApplePlatform.iOS, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs", null, null),
			//(ApplePlatform.TVOS, "PropertyTests", "PropertyTests.cs", "tvOSExpectedPropertyTests.cs", null, null),
			//(ApplePlatform.MacCatalyst, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs", null, null),
			//(ApplePlatform.MacOSX, "PropertyTests", "PropertyTests.cs", "macOSExpectedPropertyTests.cs", null, null),
			//(ApplePlatform.iOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.TVOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.MacCatalyst, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.iOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.TVOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.MacCatalyst, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			//(ApplePlatform.MacOSX, "AppKitPropertyTests", "AppKitPropertyTests.cs", "ExpectedAppKitPropertyTests.cs", null, null),
			//(ApplePlatform.MacOSX, "ThreadSafeAppKitPropertyTests", "ThreadSafeAppKitPropertyTests.cs", "ExpectedThreadSafeAppKitPropertyTests.cs", null, null),

			//(ApplePlatform.iOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			//(ApplePlatform.TVOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			//(ApplePlatform.MacCatalyst, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			//(ApplePlatform.MacOSX, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),

			// trampoline tests
			//(ApplePlatform.iOS, "TrampolinePropertyTests", "Trampolines/TrampolinePropertyTests.cs", "Trampolines/ExpectedTrampolinePropertyTests.cs", null, "Trampolines/ExpectedTrampolinePropertyTestsTrampolines.cs"),
			//(ApplePlatform.TVOS, "TrampolinePropertyTests", "Trampolines/TrampolinePropertyTests.cs", "Trampolines/ExpectedTrampolinePropertyTests.cs", null, "Trampolines/ExpectedTrampolinePropertyTestsTrampolines.cs"),
			//(ApplePlatform.MacCatalyst, "TrampolinePropertyTests", "Trampolines/TrampolinePropertyTests.cs", "Trampolines/ExpectedTrampolinePropertyTests.cs", null, "Trampolines/ExpectedTrampolinePropertyTestsTrampolines.cs"),
			//(ApplePlatform.MacOSX, "TrampolinePropertyTests", "Trampolines/TrampolinePropertyTests.cs", "Trampolines/ExpectedTrampolinePropertyTests.cs", null, "Trampolines/ExpectedTrampolinePropertyTestsTrampolines.cs"),
			
			//(ApplePlatform.iOS, "ARKitTrampolines", "Trampolines/ARKitTrampolines.cs", "Trampolines/ExpectedARKitTrampolinesProperties.cs", null, "Trampolines/ExpectedARKitTrampolinesTrampolines.cs"),
			(ApplePlatform.iOS, "AVFoundationTrampolines", "Trampolines/AVFoundationTrampolines.cs", "Trampolines/ExpectedAVFoundationTrampolinesProperties.cs", null, "Trampolines/ExpectedAVFoundationTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "AVKitTrampolines", "Trampolines/AVKitTrampolines.cs", "Trampolines/ExpectedAVKitTrampolinesProperties.cs", null, "Trampolines/ExpectedAVKitTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "AccessibilityTrampolines", "Trampolines/AccessibilityTrampolines.cs", "Trampolines/ExpectedAccessibilityTrampolinesProperties.cs", null, "Trampolines/ExpectedAccessibilityTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "AccountsTrampolines", "Trampolines/AccountsTrampolines.cs", "Trampolines/ExpectedAccountsTrampolinesProperties.cs", null, "Trampolines/ExpectedAccountsTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "AudioUnitTrampolines", "Trampolines/AudioUnitTrampolines.cs", "Trampolines/ExpectedAudioUnitTrampolinesProperties.cs", null, "Trampolines/ExpectedAudioUnitTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "AuthenticationServicesTrampolines", "Trampolines/AuthenticationServicesTrampolines.cs", "Trampolines/ExpectedAuthenticationServicesTrampolinesProperties.cs", null, "Trampolines/ExpectedAuthenticationServicesTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "BrowserEngineKitTrampolines", "Trampolines/BrowserEngineKitTrampolines.cs", "Trampolines/ExpectedBrowserEngineKitTrampolinesProperties.cs", null, "Trampolines/ExpectedBrowserEngineKitTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CarPlayTrampolines", "Trampolines/CarPlayTrampolines.cs", "Trampolines/ExpectedCarPlayTrampolinesProperties.cs", null, "Trampolines/ExpectedCarPlayTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CloudKitTrampolines", "Trampolines/CloudKitTrampolines.cs", "Trampolines/ExpectedCloudKitTrampolinesProperties.cs", null, "Trampolines/ExpectedCloudKitTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "ContactsTrampolines", "Trampolines/ContactsTrampolines.cs", "Trampolines/ExpectedContactsTrampolinesProperties.cs", null, "Trampolines/ExpectedContactsTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreDataTrampolines", "Trampolines/CoreDataTrampolines.cs", "Trampolines/ExpectedCoreDataTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreDataTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreImageTrampolines", "Trampolines/CoreImageTrampolines.cs", "Trampolines/ExpectedCoreImageTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreImageTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreLocationTrampolines", "Trampolines/CoreLocationTrampolines.cs", "Trampolines/ExpectedCoreLocationTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreLocationTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreMLTrampolines", "Trampolines/CoreMLTrampolines.cs", "Trampolines/ExpectedCoreMLTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreMLTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreMidiTrampolines", "Trampolines/CoreMidiTrampolines.cs", "Trampolines/ExpectedCoreMidiTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreMidiTrampolinesTrampolines.cs"),
			// (ApplePlatform.iOS, "CoreMotionTrampolines", "Trampolines/CoreMotionTrampolines.cs", "Trampolines/ExpectedCoreMotionTrampolinesProperties.cs", null, "Trampolines/ExpectedCoreMotionTrampolinesTrampolines.cs"),
		};

		public IEnumerator<object []> GetEnumerator ()
		{
			foreach (var testData in _data) {
				var libraryText = string.IsNullOrEmpty (value: testData.LibraryText) ?
					null : ReadFileAsString (file: testData.LibraryText);
				var trampolineText = string.IsNullOrEmpty (value: testData.TrampolinesText) ?
					null : ReadFileAsString (file: testData.TrampolinesText);
				if (Configuration.IsEnabled (platform: testData.Platform))
					yield return [
						new GenerationTestData (
							Platform: testData.Platform,
							ClassName: testData.ClassName,
							InputFileName: testData.BindingFile,
							InputText: ReadFileAsString (file: testData.BindingFile),
							OutputFileName: testData.OutputFile,
							ExpectedOutputText: ReadFileAsString (file: testData.OutputFile),
							ExpectedLibraryText: libraryText,
							ExpectedTrampolineText: trampolineText
						)
					];
			}
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGenerator))]
	public void GenerationTests (GenerationTestData testData)
		=> CompareGeneratedCode (testData);

}
