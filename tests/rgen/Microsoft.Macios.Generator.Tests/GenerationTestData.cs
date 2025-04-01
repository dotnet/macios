// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin.Utils;

namespace Microsoft.Macios.Generator.Tests;

public record GenerationTestData (
	ApplePlatform Platform,
	string ClassName,
	string InputFileName,
	string InputText,
	string OutputFileName,
	string ExpectedOutputText,
	string? ExpectedLibraryText = null,
	string? ExpectedTrampolineText = null) {
}
