// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Types;

public class NativeHandleType : MemberType {
	public NativeHandleType () : base (new SimpleType ("ObjCRuntime"), "NativeHandle")
	{
	}
}
