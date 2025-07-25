// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Xamarin.Tests;
using Xunit;
using Xamarin.Utils;

namespace Microsoft.Macios.Generator.Tests.Categories;

public class CategoryGenerationTests : BaseGeneratorTestClass {

	public class TestDataGenerator : BaseTestDataGenerator, IEnumerable<object []> {
		readonly List<(ApplePlatform Platform, string ClassName, string BindingFile, string OutputFile, string? LibraryText)> _data = new ()
		{
			(ApplePlatform.iOS, "MethodTests", "MethodTests.cs", "ExpectedMethodTests.cs", null),
			(ApplePlatform.TVOS, "MethodTests", "MethodTests.cs", "tvOSExpectedMethodTests.cs", null),
			(ApplePlatform.MacCatalyst, "MethodTests", "MethodTests.cs", "ExpectedMethodTests.cs", null),
			(ApplePlatform.MacOSX, "MethodTests", "MethodTests.cs", "ExpectedMethodTests.cs", null),
			
			// nested tests
			(ApplePlatform.iOS, "NestedCategory", "NestedCategory.cs", "ExpectedNestedCategory.cs", null),
			(ApplePlatform.TVOS, "NestedCategory", "NestedCategory.cs", "ExpectedNestedCategory.cs", null),
			(ApplePlatform.MacCatalyst, "NestedCategory", "NestedCategory.cs", "ExpectedNestedCategory.cs", null),
			(ApplePlatform.MacOSX, "NestedCategory", "NestedCategory.cs", "ExpectedNestedCategory.cs", null),
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
