using System;
using System.Runtime.InteropServices;

using Foundation;

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
			Console.WriteLine ($"Startup.Initialized: {StartupHook.Initialized}");
			Console.WriteLine ($"StartupStatus.Initialized: {StartupStatus.Initialized}");

			var rv = 0;

			if (!StartupHook.Initialized)
				rv += 1;

			if (!StartupStatus.Initialized)
				rv += 2;

			return rv;
		}
	}
}

class StartupHook {
	public static bool Initialized { get; private set; }
	public static void Initialize ()
	{
		Initialized = true;
	}
}
