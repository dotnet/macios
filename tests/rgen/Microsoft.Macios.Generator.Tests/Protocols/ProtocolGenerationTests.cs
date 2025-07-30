// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Xamarin.Tests;
using Xunit;
using Xamarin.Utils;

namespace Microsoft.Macios.Generator.Tests.Protocols;

public class ProtocolGenerationTests : BaseGeneratorTestClass {

	public class TestDataGenerator : BaseTestDataGenerator, IEnumerable<object []> {
		readonly List<(ApplePlatform Platform, string ClassName, string BindingFile, string OutputFile, string? LibraryText)> _data = new ()
		{
			(ApplePlatform.iOS, "IAVAudio3DMixing", "IAVAudio3DMixing.cs", "ExpectedIAVAudio3DMixing.cs", null),
			(ApplePlatform.TVOS, "IAVAudio3DMixing", "IAVAudio3DMixing.cs", "ExpectedIAVAudio3DMixing.cs", null),
			(ApplePlatform.MacCatalyst, "IAVAudio3DMixing", "IAVAudio3DMixing.cs", "ExpectedIAVAudio3DMixing.cs", null),
			(ApplePlatform.MacOSX, "IAVAudio3DMixing", "IAVAudio3DMixing.cs", "ExpectedIAVAudio3DMixing.cs", null),
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
