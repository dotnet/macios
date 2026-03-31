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
