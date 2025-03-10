// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Macios.Generator.Tests;

public class NomenclatorTests {

	[Theory]
	[InlineData ("AVCaptureDeviceType", "AVCaptureDeviceTypeExtensions")]
	[InlineData ("GKError", "GKErrorExtensions")]
	public void GetSmartEnumExtensionClassNameTests (string enumName, string expectedName)
		=> Assert.Equal (Nomenclator.GetSmartEnumExtensionClassName (enumName), expectedName);
}
