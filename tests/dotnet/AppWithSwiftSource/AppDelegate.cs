using System;
using System.Runtime.InteropServices;

using Foundation;

namespace AppWithSwiftSource {
	public class Program {
		[DllImport ("__Internal")]
		static extern int swift_add (int a, int b);

		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			var sum = swift_add (2, 3);
			if (sum != 5) {
				Console.WriteLine ($"Unexpected result from Swift: {sum}");
				return 1;
			}

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return args.Length;
		}
	}
}
