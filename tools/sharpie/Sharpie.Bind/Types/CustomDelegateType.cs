// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public class CustomDelegateType : SimpleType, IDelegateType {
	public CustomDelegateType (string name) : base (name)
	{
	}
}
