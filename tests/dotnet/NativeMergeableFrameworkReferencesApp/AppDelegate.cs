using System;
using System.Runtime.InteropServices;

using Foundation;

namespace NativeMergeableFrameworkReferencesApp {
	public class Program {
		[DllImport ("XMergeableTest.framework/XMergeableTest")]
		static extern int theUltimateAnswer ();

		static int Main (string [] args)
		{
			Console.WriteLine ($"Mergeable Framework: {theUltimateAnswer ()}");

			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return 0;
		}
	}
}
