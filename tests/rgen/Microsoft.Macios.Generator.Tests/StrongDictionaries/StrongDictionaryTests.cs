// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.StrongDictionaries;

public class StrongDictionaryTests : BaseGeneratorTestClass {

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
			new (ApplePlatform.iOS, "CARendererOptions", "CARendererOptions.cs", "ExpectedCARendererOptions.cs") {
				ExtraFiles = new () {
					{"CARendererOptions.Keys.g.cs", "ExpectedCARendererOptionsKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.TVOS, "CARendererOptions", "CARendererOptions.cs", "ExpectedCARendererOptions.cs") {
				ExtraFiles = new () {
					{"CARendererOptions.Keys.g.cs", "ExpectedCARendererOptionsKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.MacCatalyst, "CARendererOptions", "CARendererOptions.cs", "ExpectedCARendererOptions.cs") {
				ExtraFiles = new () {
					{"CARendererOptions.Keys.g.cs", "ExpectedCARendererOptionsKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.MacOSX, "CARendererOptions", "CARendererOptions.cs", "ExpectedCARendererOptions.cs") {
				ExtraFiles = new () {
					{"CARendererOptions.Keys.g.cs", "ExpectedCARendererOptionsKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedBGenKeys.cs"},
				}
			},

			new (ApplePlatform.iOS, "NestedStrongDictionary", "NestedStrongDictionary.cs", "ExpectedNestedStrongDictionary.cs") {
				ExtraFiles = new () {
					{"NestedStrongDictionary.Keys.g.cs", "ExpectedNestedNestedStrongDictionaryKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedNestedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.TVOS, "NestedStrongDictionary", "NestedStrongDictionary.cs", "ExpectedNestedStrongDictionary.cs") {
				ExtraFiles = new () {
					{"NestedStrongDictionary.Keys.g.cs", "ExpectedNestedNestedStrongDictionaryKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedNestedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.MacCatalyst, "NestedStrongDictionary", "NestedStrongDictionary.cs", "ExpectedNestedStrongDictionary.cs") {
				ExtraFiles = new () {
					{"NestedStrongDictionary.Keys.g.cs", "ExpectedNestedNestedStrongDictionaryKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedNestedBGenKeys.cs"},
				}
			},
			new (ApplePlatform.MacOSX, "NestedStrongDictionary", "NestedStrongDictionary.cs", "ExpectedNestedStrongDictionary.cs") {
				ExtraFiles = new () {
					{"NestedStrongDictionary.Keys.g.cs", "ExpectedNestedNestedStrongDictionaryKeys.cs"},
					{"CARendererOptionKeys.g.cs", "ExpectedNestedBGenKeys.cs"},
				}
			},
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
