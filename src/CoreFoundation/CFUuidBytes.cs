// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.InteropServices;

namespace CoreFoundation {
	// This struct is only used for P/Invokes.
	[StructLayout (LayoutKind.Sequential)]
	struct CFUuidBytes {
		internal byte Byte0;
		internal byte Byte1;
		internal byte Byte2;
		internal byte Byte3;
		internal byte Byte4;
		internal byte Byte5;
		internal byte Byte6;
		internal byte Byte7;
		internal byte Byte8;
		internal byte Byte9;
		internal byte Byte10;
		internal byte Byte11;
		internal byte Byte12;
		internal byte Byte13;
		internal byte Byte14;
		internal byte Byte15;
	}
}
