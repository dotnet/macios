// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using Microsoft.Macios.Bindings.Analyzer.Validators;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class ExportPropertyAttributeValidatorTests {

	[Fact]
	public void ResultTypeShouldFailIfNullSelector ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> (null);

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.Selector)));
		Assert.Single (errors [nameof (ExportData<Property>.Selector)]);
		Assert.Equal ("RBI0018", errors [nameof (ExportData<Property>.Selector)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void ResultTypeShouldFailIfSelectorHasWhitespace (string selector)
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> (selector);

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.Selector)));
		Assert.Single (errors [nameof (ExportData<Property>.Selector)]);
		Assert.Equal ("RBI0019", errors [nameof (ExportData<Property>.Selector)] [0].Id);
	}

	[Fact]
	public void ResultTypeShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			ResultType = ReturnTypeForInt ()
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.ResultType)));
		Assert.Single (errors [nameof (ExportData<Property>.ResultType)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.ResultType)] [0].Id);
	}

	[Fact]
	public void MethodNameShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			MethodName = "myMethod"
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.MethodName)));
		Assert.Single (errors [nameof (ExportData<Property>.MethodName)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.MethodName)] [0].Id);
	}

	[Fact]
	public void ResultTypeNameShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			ResultTypeName = "MyType"
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.ResultTypeName)));
		Assert.Single (errors [nameof (ExportData<Property>.ResultTypeName)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.ResultTypeName)] [0].Id);
	}

	[Fact]
	public void PostNonResultSnippetShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			PostNonResultSnippet = "return;"
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.PostNonResultSnippet)));
		Assert.Single (errors [nameof (ExportData<Property>.PostNonResultSnippet)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.PostNonResultSnippet)] [0].Id);
	}

	[Fact]
	public void StrongDictionaryKeyClassShouldFailIfNotStrongDictionary ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			StrongDictionaryKeyClass = ReturnTypeForClass ("MyKey")
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.StrongDictionaryKeyClass)));
		Assert.Single (errors [nameof (ExportData<Property>.StrongDictionaryKeyClass)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.StrongDictionaryKeyClass)] [0].Id);
	}

	[Fact]
	public void EventArgsTypeShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			EventArgsType = ReturnTypeForClass ("MyEventArgs")
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.EventArgsType)));
		Assert.Single (errors [nameof (ExportData<Property>.EventArgsType)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.EventArgsType)] [0].Id);
	}

	[Fact]
	public void EventArgsTypeNameShouldFailIfNotMethod ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			EventArgsTypeName = "MyEventArgs"
		};

		var errors = validator.ValidateAll (data);

		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.EventArgsTypeName)));
		Assert.Single (errors [nameof (ExportData<Property>.EventArgsTypeName)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Property>.EventArgsTypeName)] [0].Id);
	}

	[Fact]
	public void MultipleInvalidFieldsShouldReturnMultipleDiagnostics ()
	{
		var validator = new ExportPropertyAttributeValidator ();
		var data = new ExportData<Property> ("test") {
			ResultType = ReturnTypeForInt (),
			MethodName = "myMethod",
			StrongDictionaryKeyClass = ReturnTypeForClass ("MyKey")
		};

		var errors = validator.ValidateAll (data);

		Assert.Equal (3, errors.Count);
		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.ResultType)));
		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.MethodName)));
		Assert.True (errors.ContainsKey (nameof (ExportData<Property>.StrongDictionaryKeyClass)));
	}
}
