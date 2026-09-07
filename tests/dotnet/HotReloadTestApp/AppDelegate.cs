using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using Foundation;

#nullable enable

namespace HotReloadTestApp;

public partial class Program {

	static string Variable = "Variable has not changed";
	static bool ContinueLooping = true;

	static partial void ChangeVariable ();


	static int Main (string [] args)
	{
		GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

#if (__IOS__ || __TVOS__) && !__MACCATALYST__
		// On iOS/tvOS devices the main thread must not be blocked (the watchdog kills
		// the app if FinishedLaunching doesn't return promptly). Run the test logic on
		// a background thread and let UIApplicationMain pump the run loop. Mac Catalyst
		// and macOS don't have this watchdog (and using UIApplicationMain there breaks
		// 'dotnet watch'), so they run the test logic directly on the main thread.
		var thread = new Thread (() => {
			var rv = RunTestLogic ();
			// On iOS/tvOS devices UIApplicationMain keeps the app alive after the test logic has
			// completed, so we have to explicitly exit the process once the test is done (the
			// variable changed or the 120s duration is up). This is modelled after Touch.Unit's
			// TerminateWithSuccess (see tests/common/Touch.Unit).
			TerminateWithExitCode (rv);
		}) {
			IsBackground = true,
			Name = "Test Logic Thread",
		};
		thread.Start ();

		UIKit.UIApplication.Main (args, null, typeof (AppDelegate));
		return 0;
#else
		return RunTestLogic ();
#endif
	}

#if (__IOS__ || __TVOS__) && !__MACCATALYST__
	[DllImport ("libc")]
	static extern void exit (int code);

	static void TerminateWithExitCode (int code)
	{
		Console.Out.Flush ();
		Console.Error.Flush ();
		exit (code);
	}
#endif

	static int RunTestLogic ()
	{
		Print (0);

		for (var i = 0; i < 120 && ContinueLooping; i++) {
			DoSomething (i + 1);
			Thread.Sleep (TimeSpan.FromSeconds (1));
		}

		return ContinueLooping ? 1 : 0;
	}

	static void DoSomething (int i)
	{
		ChangeVariable ();
		Print (i);
	}

	static string? LogPath = Environment.GetEnvironmentVariable ("HOTRELOAD_TEST_APP_LOGFILE");
	static StreamWriter? logStream;
	static void Print (int number)
	{
		var msg = $"{number} Variable={Variable}";
		if (!string.IsNullOrEmpty (LogPath)) {
			if (logStream is null) {
				var fs = new FileStream (LogPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
				logStream = new StreamWriter (fs);
				logStream.AutoFlush = true;
			}
			logStream.WriteLine (msg);
		}
		Console.WriteLine (msg);
	}
}

#if (__IOS__ || __TVOS__) && !__MACCATALYST__
[Foundation.Register ("AppDelegate")]
public class AppDelegate : UIKit.UIApplicationDelegate {
}
#endif
