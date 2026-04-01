// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class ObjCSelectorType : SimpleType {
	public ObjCSelectorType () : base ("SEL")
	{
	}
}
