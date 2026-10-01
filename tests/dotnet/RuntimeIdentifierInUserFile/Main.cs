// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;

namespace RuntimeIdentifierInUserFile {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			return args.Length;
		}
	}
}
