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

		Print ($"Runtime={RuntimeInformation.FrameworkDescription}");
		Print ($"RuntimeVersion={Environment.Version}");
		Print ($"ProcessArchitecture={RuntimeInformation.ProcessArchitecture}");
		Print ($"OSDescription={RuntimeInformation.OSDescription}");
		Print ($"BaseDirectory={AppContext.BaseDirectory}");
		Print ($"CommandLine={Environment.CommandLine}");

		Print (0);

		try {
			for (var i = 0; i < 120 && ContinueLooping; i++) {
				DoSomething (i + 1);
				Thread.Sleep (TimeSpan.FromSeconds (1));
			}
		} catch (Exception e) {
			Print ($"Exception={e}");
			throw;
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
		=> Print ($"{number} Variable={Variable}");

	static void Print (string msg)
	{
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
