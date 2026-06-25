using System;
using Foundation;

#if !__MACOS__
using UIKit;
#endif

#nullable enable

namespace AppWithComposerIcon {
#if !(__MACCATALYST__ || __MACOS__)
	public class AppDelegate : UIApplicationDelegate {
		public override bool FinishedLaunching (UIApplication app, NSDictionary? options)
		{
			return true;
		}
	}
#endif

	public class Program {
		static int Main (string [] args)
		{
#if __MACCATALYST__ || __MACOS__
GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

return args.Length;
#else
			UIApplication.Main (args, null, typeof (AppDelegate));
			return 0;
#endif
		}
	}
}
