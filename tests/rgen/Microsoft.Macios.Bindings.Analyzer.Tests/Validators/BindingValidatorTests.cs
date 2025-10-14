// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Bindings.Analyzer.Validators;
using Microsoft.Macios.Generator.DataModel;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

public class BindingValidatorTests {

	[Theory]
	[InlineData (BindingType.Class, BindingType.Class, true, 0)]
	[InlineData (BindingType.Class, BindingType.SmartEnum, false, 1)]
	void IsValidBindingTypeTests (BindingType actualType, BindingType expectedType, bool expectedResult, int expectedDiagnosticCount)
	{
		var binding = new Binding (
			bindingInfo: new (actualType, new ("Test")),
			name: "Class",
			@namespace: ["Test"],
			fullyQualifiedSymbol: "Test.Class",
			symbolAvailability: new ());

		var result = BindingValidator.IsValidBindingType (binding, expectedType, out var diagnostics);

		Assert.Equal (expectedResult, result);
		Assert.Equal (expectedDiagnosticCount, diagnostics.Length);

		if (!expectedResult) {
			Assert.Equal ("RBI0000", diagnostics [0].Id);
		}
	}
}
