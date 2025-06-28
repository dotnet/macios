// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.Macios.Generator.Extensions;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Extensions;

public class StringExtensionsTests {

	[Theory]
	[InlineData ("", false)]
	[InlineData ("3Test", false)]
	[InlineData ("class", false)]
	[InlineData ("struct", false)]
	[InlineData ("if", false)]
	[InlineData ("else", false)]
	[InlineData ("!test", false)]
	[InlineData ("AVFoundation", true)]
	[InlineData ("AVFoundation Test", false)]
	[InlineData ("😁", false)]
	[InlineData (null, false)]
	public void IsValidIdentifier (string? identifier, bool expected)
		=> Assert.Equal (expected, identifier.IsValidIdentifier ());

	[Theory]
	[InlineData ("", "")]
	[InlineData ("a", "A")]
	[InlineData ("test", "Test")]
	[InlineData ("Test", "Test")]
	[InlineData ("TEST", "TEST")]
	[InlineData ("1test", "1test")]
	public void Capitalize (string s, string expected)
		=> Assert.Equal (expected, s.Capitalize ());
}
