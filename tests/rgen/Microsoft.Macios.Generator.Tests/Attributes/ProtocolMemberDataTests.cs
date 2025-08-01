// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
namespace Microsoft.Macios.Generator.Tests.Attributes;

public class ProtocolMemberDataTests : BaseGeneratorTestClass {

	ProtocolMemberCustomEqualityComparer comparer = new ();

	class TestDataFromPropertyDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string simpleProperty = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"")]
	public string Name { get; }
}
";
			yield return [
				simpleProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string optionalProperty = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"", Property.Optional)]
	public string Name { get; }
}
";
			yield return [
				optionalProperty,
				new ProtocolMemberData (
					isRequired: false,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string staticProperty = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"")]
	public static string Name { get; }
}
";
			yield return [
				staticProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: true,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = null,
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string getterSetterProperty = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"")]
	public string Name { get; set; }
}
";
			yield return [
				getterSetterProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = "setName:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string customSelectorsProperty = @"
using System;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"")]
	public string Name { 
		[Export<Property>(""myName"")]
		get; 
		[Export<Property>(""setMyName:"")]
		set; 
	}
}
";
			yield return [
				customSelectorsProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "myName",
					SetterSelector = "setMyName:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string argumentSemantic = @"
using System;
using ObjCRuntime;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""name"", ArgumentSemantic.Copy)]
	public string Name { get; set; }
}
";
			yield return [
				argumentSemantic,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "name"
				) {
					PropertyType = ReturnTypeForString (),
					GetterSelector = "name",
					SetterSelector = "setName:",
					ArgumentSemantic = ArgumentSemantic.Copy,
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [],
				}
			];

			const string actionProperty = @"
using System;
using ObjCRuntime;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""callback"")]
	public Action Name { get; set; }
}
";
			yield return [
				actionProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "callback"
				) {
					PropertyType = ReturnTypeForAction (),
					GetterSelector = "callback",
					SetterSelector = "setCallback:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction ()),
					ParameterBlockProxy = [TypeInfo.CreateDelegateProxy (ReturnTypeForAction ())],
				}
			];

			const string genericActionProperty = @"
using System;
using ObjCRuntime;
using ObjCBindings;

namespace Test;

public class TestClass {

	[Export<Property>(""callback"")]
	public Action<string, string> Name { get; set; }
}
";
			yield return [
				genericActionProperty,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: true,
					isStatic: false,
					name: "Name",
					selector: "callback"
				) {
					PropertyType = ReturnTypeForAction (null, "string", "string"),
					GetterSelector = "callback",
					SetterSelector = "setCallback:",
					ArgumentSemantic = ArgumentSemantic.None,
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction (null, "string", "string")),
					ParameterBlockProxy = [TypeInfo.CreateDelegateProxy (ReturnTypeForAction (null, "string", "string"))],
				}
			];
		}

		IEnumerator IEnumerable.GetEnumerator ()
			=> GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFromPropertyDeclaration>]
	void FromPropertyDeclaration (ApplePlatform platform, string inputText, ProtocolMemberData expectedAttributeData)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Property.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// get the extension methods from the property which we will use to construct the attr
		var (getter, setter) = changes.Value.ToExtensionMethods (ReturnTypeForInterface ("Test.IMyProtocol", isProtocol: true));
		var attr = new ProtocolMemberData (changes.Value, getter, setter);
		Assert.Equal (expectedAttributeData, attr, comparer);
	}

	class TestDataFromMethodDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string simpleVoidMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public void MyMethod (string input) { }
	}
}
";
			yield return [
				simpleVoidMethod,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				}
			];

			const string staticVoidMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public static void MyMethod (string input) { }
	}
}
";
			yield return [
				staticVoidMethod,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: true,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				}
			];

			const string simpleIntMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public int MyMethod (string input) { }
	}
}
";
			yield return [
				simpleIntMethod,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				}
			];

			const string intMethodMultiParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public int MyMethod (string input, string output) { }
	}
}
";
			yield return [
				intMethodMultiParameter,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForString ()],
					ParameterByRef = [false, false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, null],
				}
			];

			const string nullableParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public int MyMethod (string? input) { }
	}
}
";
			yield return [
				nullableParameter,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
				}
			];

			const string intMethodRefParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public int MyMethod (string input, out string output) { }
	}
}
";
			yield return [
				intMethodRefParameter,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForString ()],
					ParameterByRef = [false, true],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, null],
				}
			];

			const string variadicVoidMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"", Flags = Method.IsVariadic)] 
		public void MyMethod (string input) { }
	}
}
";
			yield return [
				variadicVoidMethod,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
					IsVariadic = true,
				}
			];

			const string simpleActionMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public Action MyMethod (string input) { }
	}
}
";
			yield return [
				simpleActionMethod,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForAction (),
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = TypeInfo.CreateDelegateProxy (ReturnTypeForAction ()),
					ParameterBlockProxy = [null],
				}
			];

			const string intMethodActionParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"")] 
		public int MyMethod (string input, Action callback) { }
	}
}
";
			yield return [
				intMethodActionParameter,
				new ProtocolMemberData (
					isRequired: true,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = ReturnTypeForInt (),
					ParameterType = [ReturnTypeForString (), ReturnTypeForAction ()],
					ParameterByRef = [false, false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null, TypeInfo.CreateDelegateProxy (ReturnTypeForAction ())],
				}
			];

			const string optionalVoidMethod = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	public class MyClass {

		[Export<Method> (""completeRequestReturningItems:completionHandler:"", Flags = Method.Optional)] 
		public void MyMethod (string input) { }
	}
}
";
			yield return [
				optionalVoidMethod,
				new ProtocolMemberData (
					isRequired: false,
					isProperty: false,
					isStatic: false,
					name: "MyMethod",
					selector: "completeRequestReturningItems:completionHandler:"
				) {
					ReturnType = TypeInfo.Void,
					ParameterType = [ReturnTypeForString ()],
					ParameterByRef = [false],
					ReturnTypeDelegateProxy = null,
					ParameterBlockProxy = [null],
					IsVariadic = false,
				}
			];
		}

		IEnumerator IEnumerable.GetEnumerator ()
			=> GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFromMethodDeclaration>]
	void FromMethodDeclaration (ApplePlatform platform, string inputText, ProtocolMemberData expectedAttributeData)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		var attr = new ProtocolMemberData (changes.Value);
		Assert.Equal (expectedAttributeData, attr, comparer);
	}
}
