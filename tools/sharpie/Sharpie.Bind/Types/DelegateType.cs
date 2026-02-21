// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public abstract class DelegateType : MemberType, IDelegateType {
	internal DelegateType (string name, IEnumerable<AstType> typeArguments)
		: base (new SimpleType ("System"), name, typeArguments)
	{
	}
}
