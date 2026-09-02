// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class FuncType : DelegateType {
	public FuncType (IEnumerable<AstType> typeArguments) : base ("Func", typeArguments)
	{
	}
}
