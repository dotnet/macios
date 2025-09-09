// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Bindings.Analyzer.Validators;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Tests;
using ObjCBindings;
using Xunit;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Bindings.Analyzer.Tests.Validators;

public class ExportMethodAttributeValidatorTests {

	readonly RootContext context;

	public ExportMethodAttributeValidatorTests ()
	{
		// Create a dummy compilation to get a semantic model and RootContext
		var syntaxTree = CSharpSyntaxTree.ParseText ("namespace Test { }");
		var compilation = CSharpCompilation.Create (
			"TestAssembly",
			[syntaxTree],
			references: [],
			options: new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary)
		);
		var semanticModel = compilation.GetSemanticModel (syntaxTree);
		context = new RootContext (semanticModel);
	}

	[Fact]
	public void SelectorShouldFailIfNull ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> (null);

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.Selector)));
		Assert.Single (errors [nameof (ExportData<Method>.Selector)]);
		Assert.Equal ("RBI0022", errors [nameof (ExportData<Method>.Selector)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void SelectorShouldFailIfHasWhitespace (string selector)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> (selector);

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.Selector)));
		Assert.Single (errors [nameof (ExportData<Method>.Selector)]);
		Assert.Equal ("RBI0023", errors [nameof (ExportData<Method>.Selector)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void NativePrefixShouldFailIfHasWhitespace (string prefix)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			NativePrefix = prefix
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.NativePrefix)));
		Assert.Single (errors [nameof (ExportData<Method>.NativePrefix)]);
		Assert.Equal ("RBI0024", errors [nameof (ExportData<Method>.NativePrefix)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void NativeSuffixShouldFailIfHasWhitespace (string suffix)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			NativeSuffix = suffix
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.NativeSuffix)));
		Assert.Single (errors [nameof (ExportData<Method>.NativeSuffix)]);
		Assert.Equal ("RBI0024", errors [nameof (ExportData<Method>.NativeSuffix)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void MethodNameShouldFailIfHasWhitespace (string methodName)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			MethodName = methodName
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.MethodName)));
		Assert.Single (errors [nameof (ExportData<Method>.MethodName)]);
		Assert.Equal ("RBI0026", errors [nameof (ExportData<Method>.MethodName)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void ResultTypeNameShouldFailIfHasWhitespace (string resultTypeName)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			ResultTypeName = resultTypeName
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.ResultTypeName)));
		Assert.Single (errors [nameof (ExportData<Method>.ResultTypeName)]);
		Assert.Equal ("RBI0025", errors [nameof (ExportData<Method>.ResultTypeName)] [0].Id);
	}

	[Theory]
	[InlineData (" ")]
	[InlineData (" test")]
	[InlineData ("test ")]
	[InlineData ("te st")]
	[InlineData ("\t")]
	public void EventArgsTypeNameShouldFailIfHasWhitespace (string eventArgsTypeName)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			EventArgsTypeName = eventArgsTypeName
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.EventArgsTypeName)));
		Assert.Single (errors [nameof (ExportData<Method>.EventArgsTypeName)]);
		Assert.Equal ("RBI0025", errors [nameof (ExportData<Method>.EventArgsTypeName)] [0].Id);
	}

	[Fact]
	public void FlagsShouldFailIfAsyncAndEvent ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			Flags = Method.Async | Method.Event
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0021", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void ResultTypeNameShouldFailIfNotAsync ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			ResultTypeName = "MyType"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void ResultTypeShouldFailIfNotAsync ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			ResultType = TestDataFactory.ReturnTypeForInt ()
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void MethodNameShouldFailIfNotAsync ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			MethodName = "MyMethod"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void PostNonResultSnippetShouldFailIfNotAsync ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			PostNonResultSnippet = "return;"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void EventArgsTypeShouldFailIfNotEvent ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			EventArgsType = TestDataFactory.ReturnTypeForClass ("MyEventArgs")
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void EventArgsTypeNameShouldFailIfNotEvent ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			EventArgsTypeName = "MyEventArgs"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0020", errors [string.Empty] [0].Id);
	}

	[Fact]
	public void StrongDelegateTypeShouldFail ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			StrongDelegateType = TestDataFactory.ReturnTypeForClass ("MyDelegate")
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.StrongDelegateType)));
		Assert.Single (errors [nameof (ExportData<Method>.StrongDelegateType)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Method>.StrongDelegateType)] [0].Id);
	}

	[Fact]
	public void StrongDelegateNameShouldFail ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			StrongDelegateName = "MyDelegate"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.StrongDelegateName)));
		Assert.Single (errors [nameof (ExportData<Method>.StrongDelegateName)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Method>.StrongDelegateName)] [0].Id);
	}

	[Fact]
	public void StrongDictionaryKeyClassShouldFail ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			StrongDictionaryKeyClass = TestDataFactory.ReturnTypeForClass ("MyKey")
		};

		var errors = validator.ValidateAll (data, context);

		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.StrongDictionaryKeyClass)));
		Assert.Single (errors [nameof (ExportData<Method>.StrongDictionaryKeyClass)]);
		Assert.Equal ("RBI0017", errors [nameof (ExportData<Method>.StrongDictionaryKeyClass)] [0].Id);
	}

	[Theory]
	[InlineData (true, true, true)] // Fails, we cannot have both ResultType and ResultTypeName set
	[InlineData (false, false, false)] // Succeeds, neither ResultType nor ResultTypeName set
	[InlineData (true, false, false)] // Succeeds, only ResultType set
	[InlineData (false, true, false)] // Succeeds, only ResultTypeName set
	public void AsyncResultFieldsShouldBeMutuallyExclusive (bool setResultType, bool setResultTypeName, bool shouldFail)
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test") {
			Flags = Method.Async,
			ResultType = setResultType ? TestDataFactory.ReturnTypeForInt () : TypeInfo.Default,
			ResultTypeName = setResultTypeName ? "MyType" : null
		};

		var errors = validator.ValidateAll (data, context);

		if (shouldFail) {
			Assert.True (errors.ContainsKey (string.Empty));
			Assert.Single (errors [string.Empty]);
			Assert.Equal ("RBI0016", errors [string.Empty] [0].Id);
		} else {
			Assert.Empty (errors);
		}
	}

	[Fact]
	public void MultipleInvalidFieldsShouldReturnMultipleDiagnostics ()
	{
		var validator = new ExportMethodAttributeValidator ();
		var data = new ExportData<Method> ("test with space") {
			Flags = Method.Async | Method.Event,
			ResultTypeName = "MyType",
			EventArgsTypeName = "MyEventArgs"
		};

		var errors = validator.ValidateAll (data, context);

		Assert.Equal (2, errors.Count);
		Assert.True (errors.ContainsKey (nameof (ExportData<Method>.Selector)));
		Assert.True (errors.ContainsKey (string.Empty));
		Assert.Single (errors [nameof (ExportData<Method>.Selector)]);
		Assert.Equal ("RBI0023", errors [nameof (ExportData<Method>.Selector)] [0].Id);
		Assert.Single (errors [string.Empty]);
		Assert.Equal ("RBI0021", errors [string.Empty] [0].Id);
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Async, false)]
	public void ResultTypeNameIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			ResultTypeName = "MyType",
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.ResultTypeNameIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Async, false)]
	public void ResultTypeIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			ResultType = TestDataFactory.ReturnTypeForInt (),
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.ResultTypeIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Async, false)]
	public void MethodNameIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			MethodName = "MyMethod",
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.MethodNameIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Async, false)]
	public void PostNonResultSnippetIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			PostNonResultSnippet = "return;",
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.PostNonResultSnippetIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Event, false)]
	public void EventArgsTypeIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			EventArgsType = TestDataFactory.ReturnTypeForClass ("MyEventArgs"),
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.EventArgsTypeIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Default, true)]
	[InlineData (Method.Event, false)]
	public void EventArgsTypeNameIsAllowedTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			EventArgsTypeName = "MyEventArgs",
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.EventArgsTypeNameIsAllowed (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0020", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}

	[Theory]
	[InlineData (Method.Async | Method.Event, true)]
	[InlineData (Method.Async, false)]
	[InlineData (Method.Event, false)]
	[InlineData (Method.Default, false)]
	public void FlagsAreValidTests (Method flags, bool shouldFail)
	{
		var exportData = new ExportData<Method> {
			Flags = flags
		};
		var result = ExportMethodAttributeValidator.FlagsAreValid (exportData, context, out var diagnostics);
		Assert.Equal (!shouldFail, result);
		if (shouldFail) {
			Assert.Single (diagnostics);
			Assert.Equal ("RBI0021", diagnostics [0].Id);
		} else {
			Assert.Empty (diagnostics);
		}
	}
}
