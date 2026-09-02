// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind.Attributes;

public sealed class StructLayoutAttribute : GroupedAttribute {
	public LayoutKind LayoutKind { get; private set; }

	public StructLayoutAttribute (LayoutKind layoutKind) : base (1)
	{
		Type = typeof (System.Runtime.InteropServices.StructLayoutAttribute).ToAstType ();
		LayoutKind = layoutKind;
		Arguments.Add (typeof (LayoutKind).ToAstType ().Member (layoutKind.ToString ()));
	}
}
