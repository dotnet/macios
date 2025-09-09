// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using Microsoft.Macios.Bindings.Analyzer.Validators;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

public class FieldValidatorTests {

	readonly PropertyValidatorTestLogic testLogic;

	public FieldValidatorTests ()
	{
		testLogic = new PropertyValidatorTestLogic (new FieldValidator ());
	}

	[Theory]
	[InlineData (true, true, 0)] // Valid field: static and partial
	[InlineData (false, true, 1)] // Invalid: not static
	[InlineData (true, false, 1)] // Invalid: not partial
	[InlineData (false, false, 2)] // Invalid: neither static nor partial
	public void ValidateFieldModifiersTests (bool isStatic, bool isPartial, int expectedDiagnosticsCount)
		=> testLogic.ValidateFieldModifiersTestsImpl (isStatic, isPartial, expectedDiagnosticsCount);

	[Theory]
	[InlineData (ObjCBindings.Property.Default, 0)]
	[InlineData (ObjCBindings.Property.IsThreadStatic, 1)]
	[InlineData (ObjCBindings.Property.MarshalNativeExceptions, 1)]
	[InlineData (ObjCBindings.Property.CustomMarshalDirective, 1)]
	[InlineData (ObjCBindings.Property.DisableZeroCopy, 1)]
	[InlineData (ObjCBindings.Property.IsThreadSafe, 1)]
	[InlineData (ObjCBindings.Property.Transient, 1)]
	[InlineData (ObjCBindings.Property.PlainString, 1)]
	[InlineData (ObjCBindings.Property.CoreImageFilterProperty, 1)]
	[InlineData (ObjCBindings.Property.AutoRelease, 1)]
	[InlineData (ObjCBindings.Property.RetainReturnValue, 1)]
	[InlineData (ObjCBindings.Property.ReleaseReturnValue, 1)]
	[InlineData (ObjCBindings.Property.Proxy, 1)]
	[InlineData (ObjCBindings.Property.WeakDelegate, 1)]
	[InlineData (ObjCBindings.Property.Optional, 1)]
	[InlineData (ObjCBindings.Property.CreateEvents, 1)]
	public void ValidateIgnoredFlagsTests (ObjCBindings.Property flags, int expectedDiagnosticsCount)
		=> testLogic.ValidateIgnoredFlagsTestsImpl (flags, expectedDiagnosticsCount);

	[Theory]
	[InlineData (ObjCBindings.Property.IsThreadStatic | ObjCBindings.Property.MarshalNativeExceptions, 2)]
	[InlineData (ObjCBindings.Property.CustomMarshalDirective | ObjCBindings.Property.DisableZeroCopy | ObjCBindings.Property.IsThreadSafe, 3)]
	[InlineData (ObjCBindings.Property.Transient | ObjCBindings.Property.PlainString | ObjCBindings.Property.CoreImageFilterProperty | ObjCBindings.Property.AutoRelease, 4)]
	public void ValidateMultipleIgnoredFlagsTests (ObjCBindings.Property flags, int expectedDiagnosticsCount)
		=> testLogic.ValidateMultipleIgnoredFlagsTestsImpl (flags, expectedDiagnosticsCount);

	[Fact]
	public void ValidateValidFieldPropertyTests ()
		=> testLogic.ValidateValidFieldPropertyTestsImpl ();

	[Theory]
	[InlineData ("validSelector", true, 0)]
	[InlineData ("", false, 1)]
	[InlineData (null, false, 1)]
	[InlineData ("another_valid_selector", true, 0)]
	[InlineData ("valid123", true, 0)]
	public void SelectorIsNotNullOrEmptyTests (string? selector, bool expectedResult, int expectedDiagnosticsCount)
		=> testLogic.FieldSelectorIsNotNullOrEmptyTestsImpl (selector, expectedResult, expectedDiagnosticsCount);

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
	public void SelectorHasNoWhitespaceTests (string selector, bool expectedResult, int expectedDiagnosticsCount)
		=> testLogic.FieldSelectorHasNoWhitespaceTestsImpl (selector, expectedResult, expectedDiagnosticsCount);
}
