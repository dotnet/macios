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
			(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			(ApplePlatform.TVOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			(ApplePlatform.MacCatalyst, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs", null, null),
			(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs", null, null),
			(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs", null, null),
			(ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs", null, null),
			(ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs", null, null),
			(ApplePlatform.iOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			(ApplePlatform.TVOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			(ApplePlatform.MacCatalyst, "CIImage", "CIImage.cs", "ExpectedCIImage.cs", null, null),
			(ApplePlatform.iOS, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs", null, null),
			(ApplePlatform.TVOS, "PropertyTests", "PropertyTests.cs", "tvOSExpectedPropertyTests.cs", null, null),
			(ApplePlatform.MacCatalyst, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs", null, null),
			(ApplePlatform.MacOSX, "PropertyTests", "PropertyTests.cs", "macOSExpectedPropertyTests.cs", null, null),
			(ApplePlatform.iOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			(ApplePlatform.TVOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			(ApplePlatform.MacCatalyst, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs", null, null),
			(ApplePlatform.iOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			(ApplePlatform.TVOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			(ApplePlatform.MacCatalyst, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs", null, null),
			(ApplePlatform.MacOSX, "AppKitPropertyTests", "AppKitPropertyTests.cs", "ExpectedAppKitPropertyTests.cs", null, null),
			(ApplePlatform.MacOSX, "ThreadSafeAppKitPropertyTests", "ThreadSafeAppKitPropertyTests.cs", "ExpectedThreadSafeAppKitPropertyTests.cs", null, null),

			(ApplePlatform.iOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			(ApplePlatform.TVOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			(ApplePlatform.MacCatalyst, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			(ApplePlatform.MacOSX, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs", null, null),
			
			// trampoline tests
			(ApplePlatform.iOS, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs", null, "ExpectedTrampolinePropertyTestsTrampolines.cs"),
			(ApplePlatform.TVOS, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs", null, "ExpectedTrampolinePropertyTestsTrampolines.cs"),
			(ApplePlatform.MacCatalyst, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs", null, "ExpectedTrampolinePropertyTestsTrampolines.cs"),
			(ApplePlatform.MacOSX, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs", null, "ExpectedTrampolinePropertyTestsTrampolines.cs"),
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
