// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public sealed class NUIntType : PrimitiveType {
	public NUIntType () : base ("nuint")
	{
	}
}
