using System;
using System.Runtime.InteropServices;

using Foundation;

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
			Console.WriteLine ($"Startup: {StartupHook.Initialized}");

			return StartupHook.Initialized ? 0 : 1;
		}
	}
}

class StartupHook {
	public static bool Initialized { get; private set; }
	public static void Initialize ()
	{
		Console.WriteLine ("STARTUP");

		Initialized = true;
	}
}
