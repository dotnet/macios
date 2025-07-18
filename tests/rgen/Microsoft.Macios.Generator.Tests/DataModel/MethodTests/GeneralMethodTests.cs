// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel.MethodTests;

public class GeneralMethodTests {

	[Fact]
	public void IsThreadSafe ()
	{
		var method = new Method (
			type: "NS.MyClass",
			name: "MyMethod",
			returnType: ReturnTypeForVoid (),
			symbolAvailability: new (),
			exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.IsThreadSafe),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			parameters: []
		);
		Assert.True (method.IsThreadSafe);

		method = new Method (
			type: "NS.MyClass",
			name: "MyMethod",
			returnType: ReturnTypeForVoid (),
			symbolAvailability: new (),
			exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			parameters: []
		);
		Assert.False (method.IsThreadSafe);
	}

	[Fact]
	public void IsExtension_IsTrue_WhenFirstParameterIsThis ()
	{
		var method = new Method (
			type: "NS.MyClass",
			name: "MyMethod",
			returnType: ReturnTypeForVoid (),
			symbolAvailability: new (),
			exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			parameters: [
				new Parameter (0, ReturnTypeForInt (), "self") { IsThis = true },
				new Parameter (1, ReturnTypeForInt (), "p1")
			]
		);
		Assert.True (method.IsExtension);
		Assert.Equal ("self", method.This);
	}

	[Fact]
	public void IsExtension_IsFalse_WhenNoParameterIsThis ()
	{
		var method = new Method (
			type: "NS.MyClass",
			name: "MyMethod",
			returnType: ReturnTypeForVoid (),
			symbolAvailability: new (),
			exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			parameters: [
				new Parameter (0, ReturnTypeForInt (), "p1")
			]
		);
		Assert.False (method.IsExtension);
		Assert.Equal ("this", method.This);

		// Not an extension method (has one parameter, but it is not marked as 'this')
		method = new Method (
			type: "NS.MyClass",
			name: "MyMethod",
			returnType: ReturnTypeForVoid (),
			symbolAvailability: new (),
			exportMethodData: new ("selector", ArgumentSemantic.None, ObjCBindings.Method.Default),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			parameters: [
				new Parameter (0, ReturnTypeForInt (), "p1")
			]
		);
		Assert.False (method.IsExtension);
		Assert.Equal ("this", method.This);
	}
}
