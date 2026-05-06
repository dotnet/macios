using System;
using System.Runtime.InteropServices;

using Foundation;

namespace EmbeddedFrameworkInBindingProjectApp {
	public class Program {
		static int Main (string [] args)
		{
			Console.WriteLine ($"Embedded framework: {BindingWithEmbeddedFramework.CFunctions.theUltimateAnswer ()}");

			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return 0;
		}
	}
}
