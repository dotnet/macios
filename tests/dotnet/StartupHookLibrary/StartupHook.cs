using System;

using Foundation;


class StartupHook {
	public static void Initialize ()
	{
		Console.WriteLine ("STARTUP");

		StartupStatus.Initialized = true;
	}
}

public static class StartupStatus {
	public static bool Initialized { get; internal set; }
}
