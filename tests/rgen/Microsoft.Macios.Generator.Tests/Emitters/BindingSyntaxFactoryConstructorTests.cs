// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryConstructorTests : BaseGeneratorTestClass {
	class TestDataInitializeHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// no extra params
			yield return [
				ImmutableArray<ArgumentSyntax>.Empty,
				"InitializeHandle ()"
			];

			// one param extra
			ImmutableArray<ArgumentSyntax> args = ImmutableArray.Create (
				Argument (IdentifierName ("arg1"))
			);
			yield return [
				args,
				"InitializeHandle (arg1)"
			];

			// several params
			args = ImmutableArray.Create (
				Argument (IdentifierName ("arg1")),
				Argument (IdentifierName ("arg2")),
				Argument (IdentifierName ("arg3"))
			);
			yield return [
				args,
				"InitializeHandle (arg1, arg2, arg3)"
			];

			// out parameter
			args = ImmutableArray.Create (
				Argument (PrefixUnaryExpression (SyntaxKind.AddressOfExpression, IdentifierName ("errorValue")))
			);

			yield return [
				args,
				"InitializeHandle (&errorValue)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataInitializeHandleTests))]
	void InitializeHandleTests (ImmutableArray<ArgumentSyntax> parameters, string expectedDeclaration)
	{
		var declaration = InitializeHandle (parameters);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetInvocationsTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string defaultConstructor = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass : NSObject {
		[Export<Constructor>(""init"")]
		public MyClass () {}
	}
}
";

			yield return [
				defaultConstructor,
				$"InitializeHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"init\")), \"init\")",
				$"InitializeHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"init\")), \"init\")"
			];

			const string singleParameterConstructor = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {
	public class MyClass : NSObject {
		[Export<Constructor>(""initWithString:"")]
		public MyClass (string value) {}
	}
}
";

			yield return [
				singleParameterConstructor,
				$"InitializeHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSend_NativeHandle (this.Handle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"initWithString:\"), nsvalue), \"initWithString:\")",
				$"InitializeHandle ({Global ("ObjCRuntime")}.Messaging.NativeHandle_objc_msgSendSuper_NativeHandle (this.SuperHandle, {Global ("ObjCRuntime")}.Selector.GetHandle (\"initWithString:\"), nsvalue), \"initWithString:\")"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataGetInvocationsTests>]
	void GetInvocationsTests (ApplePlatform platform, string inputText, string expectedSend, string expectedSendSuper)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<ConstructorDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Constructor.TryCreate (declaration, semanticModel, out var constructor));
		Assert.NotNull (constructor);
		var invocations = GetInvocations (constructor.Value);
		Assert.Equal (expectedSend, invocations.Send.ToString ());
		Assert.Equal (expectedSendSuper, invocations.SendSuper.ToString ());
	}
}
