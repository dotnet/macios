// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class ReturnInfoTests {

	[Fact]
	public void Constructor_WithProperty_SetsPropertiesCorrectly ()
	{
		var property = new Property (
			name: "FirstProperty",
			returnType: new ("string"),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			accessors: [
				new (
					accessorKind: AccessorKind.Getter,
					symbolAvailability: new (),
					exportPropertyData: null,
					attributes: [],
					modifiers: []
				)
			]) {
			ExportPropertyData = new ("name"),
			BindAs = new BindFromData (new TypeInfo { Name = "NSString" })
		};

		var returnInfo = new ReturnInfo (property);

		Assert.Equal (property.ReturnType, returnInfo.Type);
		Assert.Equal (property.BindAs, returnInfo.BindAs);
		Assert.Null (returnInfo.ReleaseHandle);
	}

	[Fact]
	public void ImplicitConversion_FromProperty_ConvertsCorrectly ()
	{
		var property = new Property (
			name: "FirstProperty",
			returnType: new ("string"),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
			],
			accessors: [
				new (
					accessorKind: AccessorKind.Getter,
					symbolAvailability: new (),
					exportPropertyData: null,
					attributes: [],
					modifiers: []
				)
			]) {
			ExportPropertyData = new ("name"),
			BindAs = new BindFromData (new TypeInfo { Name = "NSString" })
		};

		ReturnInfo returnInfo = property;

		Assert.Equal (property.ReturnType, returnInfo.Type);
		Assert.Equal (property.BindAs, returnInfo.BindAs);
		Assert.Null (returnInfo.ReleaseHandle);
	}

	[Fact]
	public void Constructor_WithDelegateInfo_SetsPropertiesCorrectly ()
	{
		var delegateInfo = new DelegateInfo (
			"TestDelegate",
			"System.Action",
			TypeInfo.Void,
			ImmutableArray<DelegateParameter>.Empty
		);

		var returnInfo = new ReturnInfo (delegateInfo);

		Assert.Equal (delegateInfo.ReturnType, returnInfo.Type);
		Assert.Null (returnInfo.BindAs);
		Assert.False (returnInfo.ReleaseHandle);
	}

	[Fact]
	public void ImplicitConversion_FromDelegateInfo_ConvertsCorrectly ()
	{
		var delegateInfo = new DelegateInfo (
			"TestDelegate",
			"System.Action",
			TestDataFactory.ReturnTypeForInt (),
			ImmutableArray<DelegateParameter>.Empty
		);

		ReturnInfo returnInfo = delegateInfo;

		Assert.Equal (delegateInfo.ReturnType, returnInfo.Type);
		Assert.Null (returnInfo.BindAs);
		Assert.False (returnInfo.ReleaseHandle);
	}

	[Fact]
	public void Constructor_WithMethod_SetsPropertiesCorrectly ()
	{

		var method = new Method (
			type: "MyType",
			name: "MyMethod",
			returnType: TestDataFactory.ReturnTypeForEnum ("MyEnum", isSmartEnum: true),
			symbolAvailability: new (),
			exportMethodData: new (selector: "myMethod"),
			attributes: [],
			modifiers: [],
			parameters: []
		) {
			BindAs = new BindFromData (TestDataFactory.ReturnTypeForNSString ()),
		};

		var returnInfo = new ReturnInfo (method);

		Assert.Equal (method.ReturnType, returnInfo.Type);
		Assert.Equal (method.BindAs, returnInfo.BindAs);
		Assert.Null (returnInfo.ReleaseHandle);
	}

	[Fact]
	public void ImplicitConversion_FromMethod_ConvertsCorrectly ()
	{

		var method = new Method (
			type: "MyType",
			name: "MyMethod",
			returnType: TestDataFactory.ReturnTypeForEnum ("MyEnum", isSmartEnum: true),
			symbolAvailability: new (),
			exportMethodData: new (selector: "myMethod"),
			attributes: [],
			modifiers: [],
			parameters: []
		) {
			BindAs = new BindFromData (TestDataFactory.ReturnTypeForNSString ()),
		};

		ReturnInfo returnInfo = method;

		Assert.Equal (method.ReturnType, returnInfo.Type);
		Assert.Equal (method.BindAs, returnInfo.BindAs);
		Assert.Null (returnInfo.ReleaseHandle);
	}
}
