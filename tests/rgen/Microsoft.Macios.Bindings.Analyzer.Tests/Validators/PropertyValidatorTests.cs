// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Bindings.Analyzer.Validators;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

public class PropertyValidatorTests {

	readonly PropertyValidatorTestLogic testLogic;

	public PropertyValidatorTests ()
	{
		testLogic = new PropertyValidatorTestLogic (new PropertyValidator ());
	}

	[Theory]
	[InlineData (true, 0)] // Valid property: partial
	[InlineData (false, 1)] // Invalid: not partial
	public void ValidatePropertyModifiersTests (bool isPartial, int expectedDiagnosticsCount)
		=> testLogic.ValidatePropertyModifiersTestsImpl (isPartial, expectedDiagnosticsCount);

	[Theory]
	[InlineData ("validSelector", true, 0)]
	[InlineData ("", false, 1)]
	[InlineData (null, false, 1)]
	[InlineData ("another_valid_selector", true, 0)]
	[InlineData ("valid123", true, 0)]
	public void PropertySelectorIsNotNullTests (string? selector, bool expectedResult, int expectedDiagnosticsCount)
		=> testLogic.PropertySelectorIsNotNullTestsImpl (selector, expectedResult, expectedDiagnosticsCount);

	[Theory]
	[InlineData ("validSelector", true, 0)]
	[InlineData ("invalid selector", false, 1)]
	[InlineData ("selector\twith\ttab", false, 1)]
	[InlineData ("selector\nwith\nnewline", false, 1)]
	[InlineData ("selector with space", false, 1)]
	[InlineData ("valid_selector", true, 0)]
	[InlineData ("validSelector123", true, 0)]
	[InlineData (" leadingSpace", false, 1)]
	[InlineData ("trailingSpace ", false, 1)]
	public void PropertySelectorHasNoWhitespaceTests (string selector, bool expectedResult, int expectedDiagnosticsCount)
		=> testLogic.PropertySelectorHasNoWhitespaceTestsImpl (selector, expectedResult, expectedDiagnosticsCount);

	[Theory]
	[InlineData ("validGetter", null, 0)] // Valid getter, no setter export
	[InlineData (null, "validSetter:", 0)] // No getter export, valid setter
	[InlineData ("validGetter", "validSetter:", 0)] // Both valid
	[InlineData ("", "validSetter:", 1)] // Invalid getter (empty)
	[InlineData ("validGetter", "", 1)] // Invalid setter (empty)
	[InlineData ("", "", 2)] // Both invalid
	public void AccessorSelectorValidationTests (string? getterSelector, string? setterSelector, int expectedDiagnosticsCount)
		=> testLogic.AccessorSelectorValidationTestsImpl (getterSelector, setterSelector, expectedDiagnosticsCount);

	[Theory]
	[InlineData ("validGetter", "validSetter:", 0)] // Valid selectors
	[InlineData ("invalid getter", "validSetter:", 1)] // Invalid getter (whitespace)
	[InlineData ("validGetter", "invalid setter:", 1)] // Invalid setter (whitespace)
	[InlineData ("invalid getter", "invalid setter:", 2)] // Both invalid
	[InlineData ("getter\twith\ttab", "setter\nwith\nnewline:", 2)] // Both have different whitespace
	public void AccessorSelectorWhitespaceTests (string getterSelector, string setterSelector, int expectedDiagnosticsCount)
		=> testLogic.AccessorSelectorWhitespaceTestsImpl (getterSelector, setterSelector, expectedDiagnosticsCount);

	[Theory]
	[InlineData ("getPropertyValue", "setPropertyValue:", 0)] // Correct arg count: 0 for getter, 1 for setter
	[InlineData ("getPropertyValue:", "setPropertyValue", 2)] // Wrong arg count: 1 for getter, 0 for setter
	[InlineData ("getPropertyValue:withParam:", "setPropertyValue:", 1)] // Wrong getter arg count
	[InlineData ("getPropertyValue", "setPropertyValue:withExtraParam:", 1)] // Wrong setter arg count
	public void AccessorSelectorArgCountTests (string getterSelector, string setterSelector, int expectedDiagnosticsCount)
		=> testLogic.AccessorSelectorArgCountTestsImpl (getterSelector, setterSelector, expectedDiagnosticsCount);

	[Fact]
	public void ValidateValidPropertyTests ()
		=> testLogic.ValidateValidPropertyTestsImpl ();

	[Theory]
	[InlineData (true, true, 0)] // Has getter and setter, both valid
	[InlineData (true, false, 0)] // Only getter, valid
	[InlineData (false, true, 0)] // Only setter, valid
	[InlineData (false, false, 0)] // No accessors, valid
	public void PropertyAccessorPresenceTests (bool hasGetter, bool hasSetter, int expectedDiagnosticsCount)
		=> testLogic.PropertyAccessorPresenceTestsImpl (hasGetter, hasSetter, expectedDiagnosticsCount);

	[Theory]
	[InlineData (false, "validGetter", null, 1)] // Not partial but has valid accessors
	[InlineData (false, "", "validSetter:", 2)] // Not partial and invalid getter
	[InlineData (false, "invalid getter", "invalid setter:", 3)] // Not partial and multiple issues
	public void CombinedValidationTests (bool isPartial, string? getterSelector, string? setterSelector, int expectedDiagnosticsCount)
		=> testLogic.CombinedPropertyValidationTestsImpl (isPartial, getterSelector, setterSelector, expectedDiagnosticsCount);
}
