// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System;
using ObjCBindings;
using Xunit;

namespace Microsoft.Macios.Generator.Tests;

public class AttributesNamesTests {

	[Theory]
	[InlineData (StringComparison.Ordinal, null)]
	[InlineData (EnumValue.Default, null)]
	[InlineData (Property.Default, AttributesNames.ExportPropertyAttribute)]
	[InlineData (Method.Default, AttributesNames.ExportMethodAttribute)]
	public void GetExportAttributeName<T> (T @enum, string? expectedName) where T : Enum
	{
		Assert.NotNull (@enum);
		Assert.Equal (expectedName, AttributesNames.GetExportAttributeName<T> ());
	}

	[Theory]
	[InlineData (StringComparison.Ordinal, null)]
	[InlineData (Method.Default, null)]
	[InlineData (Property.Default, AttributesNames.FieldPropertyAttribute)]
	[InlineData (EnumValue.Default, AttributesNames.EnumFieldAttribute)]
	public void GetFieldAttributeName<T> (T @enum, string? expectedName) where T : Enum
	{
		Assert.NotNull (@enum);
		Assert.Equal (expectedName, AttributesNames.GetFieldAttributeName<T> ());
	}

	[Theory]
	[InlineData (StringComparison.Ordinal, null)]
	[InlineData (EnumValue.Default, null)]
	[InlineData (Category.Default, AttributesNames.CategoryAttribute)]
	[InlineData (Class.Default, AttributesNames.ClassAttribute)]
	[InlineData (Protocol.Default, AttributesNames.ProtocolAttribute)]
	[InlineData (StrongDictionary.Default, AttributesNames.StrongDictionaryAttribute)]
	public void GetBindingTypeAttributeName<T> (T @enum, string? expectedName) where T : Enum
	{
		Assert.NotNull (@enum);
		Assert.Equal (expectedName, AttributesNames.GetBindingTypeAttributeName<T> ());
	}
}
