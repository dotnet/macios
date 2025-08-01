// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.Tests.Classes;
using Xamarin.Tests;
using Xunit;
using Xamarin.Utils;

namespace Microsoft.Macios.Generator.Tests.SmartEnum;


/// <summary>
///  Test all the field generation code.
/// </summary>
public class SmartEnumDiagnosticsTests : BaseGeneratorTestClass {
	public class TestDataGenerator : BaseTestDataGenerator, IEnumerable<object []> {
		readonly List<(ApplePlatform Platform, string ClassName, string BindingFile, string OutputFile, string? LibraryText)> _data = new ()
		{
			(ApplePlatform.iOS, "AVCaptureDeviceTypeExtensions", "AVCaptureDeviceTypeEnum.cs", "ExpectedAVCaptureDeviceTypeEnum.cs", null),
			(ApplePlatform.iOS, "AVCaptureSystemPressureLevelExtensions", "AVCaptureSystemPressureLevel.cs", "ExpectedAVCaptureSystemPressureLevel.cs", null),
			(ApplePlatform.iOS, "AVMediaCharacteristicsExtensions", "AVMediaCharacteristics.cs", "ExpectediOSAVMediaCharacteristics.cs", null),
			(ApplePlatform.iOS, "NestedAVMediaCharacteristicsExtensions", "NestedAVMediaCharacteristics.cs", "ExpectedNestedAVMediaCharacteristics.cs", null),
			(ApplePlatform.iOS, "GKErrorExtensions", "GKError.cs", "ExpectedGKError.cs", null),
			(ApplePlatform.iOS, "NestedGKErrorExtensions", "NestedGKError.cs", "ExpectedNestedGKError.cs", null),
			(ApplePlatform.iOS, "GKCustomErrorExtensions", "GKErrorCustomLibrary.cs", "ExpectedGKErrorCustomLibrary.cs", null),
			(ApplePlatform.TVOS, "AVCaptureDeviceTypeExtensions", "AVCaptureDeviceTypeEnum.cs", "ExpectedAVCaptureDeviceTypeEnum.cs", null),
			(ApplePlatform.TVOS, "AVCaptureSystemPressureLevelExtensions", "AVCaptureSystemPressureLevel.cs", "ExpectedAVCaptureSystemPressureLevel.cs", null),
			(ApplePlatform.TVOS, "AVMediaCharacteristicsExtensions", "AVMediaCharacteristics.cs", "ExpectediOSAVMediaCharacteristics.cs", null),
			(ApplePlatform.TVOS, "NestedAVMediaCharacteristicsExtensions", "NestedAVMediaCharacteristics.cs", "ExpectedNestedAVMediaCharacteristics.cs", null),
			(ApplePlatform.TVOS, "GKErrorExtensions", "GKError.cs", "ExpectedGKError.cs", null),
			(ApplePlatform.TVOS, "NestedGKErrorExtensions", "NestedGKError.cs", "ExpectedNestedGKError.cs", null),
			(ApplePlatform.TVOS, "GKCustomErrorExtensions", "GKErrorCustomLibrary.cs", "ExpectedGKErrorCustomLibrary.cs", null),
			(ApplePlatform.MacCatalyst, "AVCaptureDeviceTypeExtensions", "AVCaptureDeviceTypeEnum.cs", "ExpectedAVCaptureDeviceTypeEnum.cs", null),
			(ApplePlatform.MacCatalyst, "AVCaptureSystemPressureLevelExtensions", "AVCaptureSystemPressureLevel.cs", "ExpectedAVCaptureSystemPressureLevel.cs", null),
			(ApplePlatform.MacCatalyst, "AVMediaCharacteristicsExtensions", "AVMediaCharacteristics.cs", "ExpectediOSAVMediaCharacteristics.cs", null),
			(ApplePlatform.MacCatalyst, "NestedAVMediaCharacteristicsExtensions", "NestedAVMediaCharacteristics.cs", "ExpectedNestedAVMediaCharacteristics.cs", null),
			(ApplePlatform.MacCatalyst, "GKErrorExtensions", "GKError.cs", "ExpectedGKError.cs", null),
			(ApplePlatform.MacCatalyst, "NestedGKErrorExtensions", "NestedGKError.cs", "ExpectedNestedGKError.cs", null),
			(ApplePlatform.MacCatalyst, "GKCustomErrorExtensions", "GKErrorCustomLibrary.cs", "ExpectedGKErrorCustomLibrary.cs", null),
			(ApplePlatform.MacOSX, "AVMediaCharacteristicsExtensions", "AVMediaCharacteristics.cs", "ExpectedMacOSAVMediaCharacteristics.cs", null),
			(ApplePlatform.MacOSX, "NestedAVMediaCharacteristicsExtensions", "NestedAVMediaCharacteristics.cs", "ExpectedNestedAVMediaCharacteristics.cs", null),
			(ApplePlatform.MacOSX, "CustomLibraryEnumExtensions", "CustomLibraryEnum.cs", "ExpectedCustomLibraryEnum.cs", "ExpectedCustomLibraryEnumLibrariesClass.cs"),
			(ApplePlatform.MacOSX, "CustomLibraryEnumInternalExtensions", "CustomLibraryEnumInternal.cs", "ExpectedCustomLibraryEnumInternal.cs", "ExpectedCustomLibraryEnumInternalLibrariesClass.cs"),
			(ApplePlatform.MacOSX, "GKErrorExtensions", "GKError.cs", "ExpectedGKError.cs", null),
			(ApplePlatform.MacOSX, "NestedGKErrorExtensions", "NestedGKError.cs", "ExpectedNestedGKError.cs", null),
			(ApplePlatform.MacOSX, "GKCustomErrorExtensions", "GKErrorCustomLibrary.cs", "ExpectedGKErrorCustomLibrary.cs", null),
		};

		public IEnumerator<object []> GetEnumerator ()
		{
			foreach (var testData in _data) {
				var libraryText = string.IsNullOrEmpty (testData.LibraryText) ?
					null : ReadFileAsString (testData.LibraryText);
				if (Configuration.IsEnabled (testData.Platform))
					yield return [
						new GenerationTestData (
							Platform: testData.Platform,
							ClassName: testData.ClassName,
							InputFileName: testData.BindingFile,
							InputText: ReadFileAsString (testData.BindingFile),
							OutputFileName: testData.OutputFile,
							ExpectedOutputText: ReadFileAsString (testData.OutputFile),
							ExpectedLibraryText: libraryText
						)
					];
			}
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGenerator))]
	public void ExtensionGenerationTests (GenerationTestData testData)
		=> CompareGeneratedCode (testData);

}
