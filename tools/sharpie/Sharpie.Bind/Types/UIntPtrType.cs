// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public class UIntPtrType : MemberType {
	public UIntPtrType () : base (new SimpleType ("System"), "UIntPtr")
	{
	}
}
