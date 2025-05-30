using System;
using System.Runtime.InteropServices;
using System.Net.Http;

using Foundation;

namespace MySimpleApp {
	public class Program {
		static int Main (string [] args)
		{
			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
#if HttpClientHandlerFeatureTrimmedAway
			var client = new System.Net.Http.HttpClient ();
			client.GetStringAsync ("https://microsoft.com");
#endif

			return args.Length;
		}
	}
}
