// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using ObjCRuntime;

namespace InvalidRegistrarApp {
	public class Program {
		static void Main ()
		{
			using var obj = new InvalidDateTimeClass ();
		}
	}

	public class InvalidDateTimeClass : NSObject {
		[Export ("f:")]
		public void F (DateTime value)
		{
		}
	}
}
