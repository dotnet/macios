// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.InteropServices;

namespace CoreFoundation {
	// This struct is only used for P/Invokes.
	[StructLayout (LayoutKind.Sequential)]
	struct CFUuidBytes {
		public byte Byte0;
		public byte Byte1;
		public byte Byte2;
		public byte Byte3;
		public byte Byte4;
		public byte Byte5;
		public byte Byte6;
		public byte Byte7;
		public byte Byte8;
		public byte Byte9;
		public byte Byte10;
		public byte Byte11;
		public byte Byte12;
		public byte Byte13;
		public byte Byte14;
		public byte Byte15;
	}
}
