// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class NativeAttribute : GroupedAttribute {
	public NativeAttribute () : base (1)
	{
		Type = AstType.Create ("ObjCRuntime.NativeAttribute");
	}
}
