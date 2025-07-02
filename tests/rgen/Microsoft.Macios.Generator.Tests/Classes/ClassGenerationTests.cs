// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Classes;

public class ClassGenerationTests : BaseGeneratorTestClass {

	public record ClassGenerationData (
		ApplePlatform Platform,
		string ClassName,
		string BindingFile,
		string OutputFile) {
		public string? LibrariesFile { get; set; } = null;
		public string? TrampolinesFile { get; set; } = null;
		public Dictionary<string, string>? ExtraFiles { get; set; } = null;
	}

	public class TestDataGenerator : BaseTestDataGenerator, IEnumerable<object []> {
		readonly List<ClassGenerationData> _data = new ()
		{
			new (ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs"),
			new (ApplePlatform.TVOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs"),
			new (ApplePlatform.MacCatalyst, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs"),
			new (ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoDefaultCtr.cs", "ExpectedAVAudioPcmBufferNoDefaultCtr.cs"),
			new (ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs"),
			new (ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferDefaultCtr.cs", "ExpectedAVAudioPcmBufferDefaultCtr.cs"),
			new (ApplePlatform.iOS, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs"),
			new (ApplePlatform.MacOSX, "AVAudioPcmBuffer", "AVAudioPcmBufferNoNativeName.cs", "ExpectedAVAudioPcmBufferNoNativeName.cs"),
			new (ApplePlatform.iOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs"),
			new (ApplePlatform.TVOS, "CIImage", "CIImage.cs", "ExpectedCIImage.cs"),
			new (ApplePlatform.MacCatalyst, "CIImage", "CIImage.cs", "ExpectedCIImage.cs"),
			new (ApplePlatform.iOS, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs"),
			new (ApplePlatform.TVOS, "PropertyTests", "PropertyTests.cs", "tvOSExpectedPropertyTests.cs"),
			new (ApplePlatform.MacCatalyst, "PropertyTests", "PropertyTests.cs", "iOSExpectedPropertyTests.cs"),
			new (ApplePlatform.MacOSX, "PropertyTests", "PropertyTests.cs", "macOSExpectedPropertyTests.cs"),
			new (ApplePlatform.iOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs"),
			new (ApplePlatform.TVOS, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs"),
			new (ApplePlatform.MacCatalyst, "UIKitPropertyTests", "UIKitPropertyTests.cs", "ExpectedUIKitPropertyTests.cs"),
			new (ApplePlatform.iOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs"),
			new (ApplePlatform.TVOS, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs"),
			new (ApplePlatform.MacCatalyst, "ThreadSafeUIKitPropertyTests", "ThreadSafeUIKitPropertyTests.cs", "ExpectedThreadSafeUIKitPropertyTests.cs"),
			new (ApplePlatform.MacOSX, "AppKitPropertyTests", "AppKitPropertyTests.cs", "ExpectedAppKitPropertyTests.cs"),
			new (ApplePlatform.MacOSX, "ThreadSafeAppKitPropertyTests", "ThreadSafeAppKitPropertyTests.cs", "ExpectedThreadSafeAppKitPropertyTests.cs"),

			new (ApplePlatform.iOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs"),
			new (ApplePlatform.TVOS, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs"),
			new (ApplePlatform.MacCatalyst, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs"),
			new (ApplePlatform.MacOSX, "NSUserDefaults", "NSUserDefaults.cs", "ExpectedNSUserDefaults.cs"),

			new (ApplePlatform.iOS, "MethodTests", "MethodTests.cs", "ExpectedMethodsTests.cs")
			{
				ExtraFiles = new () {
					{"NSLoadFromHtmlResult.g.cs", "ExpectedNSLoadFromHtmlResult.cs"},
				}
			},
			new (ApplePlatform.TVOS, "MethodTests", "MethodTests.cs", "tvOSExpectedMethodsTests.cs"),
			new (ApplePlatform.MacCatalyst, "MethodTests", "MethodTests.cs", "ExpectedMethodsTests.cs")
			{
				ExtraFiles = new () {
					{"NSLoadFromHtmlResult.g.cs", "ExpectedNSLoadFromHtmlResult.cs"},
				}
			},
			new (ApplePlatform.MacOSX, "MethodTests", "MethodTests.cs", "ExpectedMethodsTests.cs")
			{
				ExtraFiles = new () {
					{"NSLoadFromHtmlResult.g.cs", "ExpectedNSLoadFromHtmlResult.cs"},
				}
			},
			
			// trampoline tests
			new (ApplePlatform.iOS, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs"){ TrampolinesFile = "ExpectedTrampolinePropertyTestsTrampolines.cs"},
			new (ApplePlatform.TVOS, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs"){ TrampolinesFile = "ExpectedTrampolinePropertyTestsTrampolines.cs"},
			new (ApplePlatform.MacCatalyst, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs"){ TrampolinesFile = "ExpectedTrampolinePropertyTestsTrampolines.cs"},
			new (ApplePlatform.MacOSX, "TrampolinePropertyTests", "TrampolinePropertyTests.cs", "ExpectedTrampolinePropertyTests.cs"){ TrampolinesFile = "ExpectedTrampolinePropertyTestsTrampolines.cs"},
		};

		public IEnumerator<object []> GetEnumerator ()
		{
			foreach (var testData in _data) {
				var libraryText = string.IsNullOrEmpty (value: testData.LibrariesFile) ?
					null : ReadFileAsString (file: testData.LibrariesFile);
				var trampolineText = string.IsNullOrEmpty (value: testData.TrampolinesFile) ?
					null : ReadFileAsString (file: testData.TrampolinesFile);
				Dictionary<string, string>? extraFiles = null;
				if (testData.ExtraFiles is not null) {
					extraFiles = new ();
					foreach (var (key, filePath) in testData.ExtraFiles) {
						var extraFileText = ReadFileAsString (file: filePath);
						extraFiles.Add (key, extraFileText);
					}
				}
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
							ExpectedTrampolineText: trampolineText,
							ExtraFiles: extraFiles
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
