using System;
using System.Runtime.InteropServices;

using Foundation;

namespace StaticFrameworkFilterApp {
	public class Program {
		[DllImport ("XTest.framework/XTest")]
		static extern int theUltimateAnswer ();

		static int Main (string [] args)
		{
			Console.WriteLine ($"Framework: {theUltimateAnswer ()}");

			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			return 0;
		}
	}
}
