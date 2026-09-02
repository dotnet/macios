// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class ActionType : DelegateType {
	public ActionType (IEnumerable<AstType> typeArguments) : base ("Action", typeArguments)
	{
	}
}
