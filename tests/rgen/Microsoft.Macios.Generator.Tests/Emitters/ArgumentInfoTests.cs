// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
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
		Assert.Equal (delegateParameter.Type, argumentInfo.Type);
		Assert.Equal (delegateParameter.BindAs, argumentInfo.BindAs);
		Assert.Equal (delegateParameter.IsByRef, argumentInfo.IsByRef);
		Assert.Equal (delegateParameter.IsCCallback, argumentInfo.IsCCallback);
		Assert.Equal (delegateParameter.IsBlockCallback, argumentInfo.IsBlockCallback);
	}
}
