// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using System.Diagnostics.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using ObjCRuntime;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class ArgumentInfoTests {
	[Fact]
	public void ExplicitConstructor_FromParameter ()
	{
		var typeInfo = new TypeInfo ("System.String");
		var parameter = new Parameter (0, typeInfo, "testParam") {
			ReferenceKind = ReferenceKind.In,
		};

		var argumentInfo = new ArgumentInfo (parameter);

		Assert.Equal (parameter.Name, argumentInfo.Name);
		Assert.Equal (parameter.Position, argumentInfo.Position);
		Assert.Equal (parameter.Type, argumentInfo.Type);
		Assert.Equal (parameter.BindAs, argumentInfo.BindAs);
		Assert.Equal (parameter.IsByRef, argumentInfo.IsByRef);
		Assert.False (argumentInfo.IsCCallback);
		Assert.False (argumentInfo.IsBlockCallback);
	}

	[Fact]
	public void ExplicitConstructor_FromDelegateParameter ()
	{
		var typeInfo = new TypeInfo ("System.Action");
		var delegateParameter = new DelegateParameter (0, typeInfo, "testDelegate") {
			IsBlockCallback = true,
		};

		var argumentInfo = new ArgumentInfo (delegateParameter);

		Assert.Equal (delegateParameter.Name, argumentInfo.Name);
		Assert.Equal (delegateParameter.Position, argumentInfo.Position);
		Assert.Equal (delegateParameter.Type, argumentInfo.Type);
		Assert.Equal (delegateParameter.BindAs, argumentInfo.BindAs);
		Assert.Equal (delegateParameter.IsByRef, argumentInfo.IsByRef);
		Assert.Equal (delegateParameter.IsCCallback, argumentInfo.IsCCallback);
		Assert.Equal (delegateParameter.IsBlockCallback, argumentInfo.IsBlockCallback);
	}

	[Fact]
	public void ImplicitConversion_FromParameter ()
	{
		var typeInfo = new TypeInfo ("System.Int32");
		var parameter = new Parameter (0, typeInfo, "testParam") {
			ReferenceKind = ReferenceKind.In,
		};

		ArgumentInfo argumentInfo = parameter;

		Assert.Equal (parameter.Name, argumentInfo.Name);
		Assert.Equal (parameter.Position, argumentInfo.Position);
		Assert.Equal (parameter.Type, argumentInfo.Type);
		Assert.Equal (parameter.BindAs, argumentInfo.BindAs);
		Assert.Equal (parameter.IsByRef, argumentInfo.IsByRef);
		Assert.False (argumentInfo.IsCCallback);
		Assert.False (argumentInfo.IsBlockCallback);
	}

	[Fact]
	public void ImplicitConversion_FromDelegateParameter ()
	{
		var typeInfo = new TypeInfo ("System.Func<int>");
		var delegateParameter = new DelegateParameter (0, typeInfo, "testDelegate") {
			IsBlockCallback = true,
		};

		ArgumentInfo argumentInfo = delegateParameter;

		Assert.Equal (delegateParameter.Name, argumentInfo.Name);
		Assert.Equal (delegateParameter.Position, argumentInfo.Position);
		Assert.Equal (delegateParameter.Type, argumentInfo.Type);
		Assert.Equal (delegateParameter.BindAs, argumentInfo.BindAs);
		Assert.Equal (delegateParameter.IsByRef, argumentInfo.IsByRef);
		Assert.Equal (delegateParameter.IsCCallback, argumentInfo.IsCCallback);
		Assert.Equal (delegateParameter.IsBlockCallback, argumentInfo.IsBlockCallback);
	}

	[Fact]
	public void ExplicitConstructor_FromProperty ()
	{
		var typeInfo = new TypeInfo ("System.String");
		var property = new Property (
			name: "MyProperty",
			returnType: typeInfo,
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			BindAs = new (),
			ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.Default)
		};

		var argumentInfo = new ArgumentInfo (property);

		Assert.Equal ("value", argumentInfo.Name);
		Assert.Equal (0, argumentInfo.Position);
		Assert.Equal (property.ReturnType, argumentInfo.Type);
		Assert.NotNull (argumentInfo.BindAs);
		Assert.Equal (property.BindAs, argumentInfo.BindAs);
		Assert.False (argumentInfo.IsByRef);
		Assert.Equal (ReferenceKind.None, argumentInfo.ReferenceKind);
		Assert.False (argumentInfo.IsCCallback);
		Assert.False (argumentInfo.IsBlockCallback);
	}

	[Fact]
	public void ImplicitConversion_FromProperty ()
	{
		var typeInfo = new TypeInfo ("System.String");
		var property = new Property (
			name: "MyProperty",
			returnType: typeInfo,
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			BindAs = new (),
			ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.Default)
		};

		ArgumentInfo argumentInfo = property;

		Assert.Equal ("value", argumentInfo.Name);
		Assert.Equal (0, argumentInfo.Position);
		Assert.Equal (property.ReturnType, argumentInfo.Type);
		Assert.NotNull (argumentInfo.BindAs);
		Assert.Equal (property.BindAs, argumentInfo.BindAs);
		Assert.False (argumentInfo.IsByRef);
		Assert.Equal (ReferenceKind.None, argumentInfo.ReferenceKind);
		Assert.False (argumentInfo.IsCCallback);
		Assert.False (argumentInfo.IsBlockCallback);
	}
}
