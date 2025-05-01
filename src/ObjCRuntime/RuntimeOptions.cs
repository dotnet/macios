using System;
using System.IO;
using System.Text;

using System.Net.Http;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace ObjCRuntime {
	class RuntimeOptions {
		const string SocketsHandlerValue = "SocketsHttpHandler";
		const string CFNetworkHandlerValue = "CFNetworkHandler";
		const string NSUrlSessionHandlerValue = "NSUrlSessionHandler";

		// This is invoked by
		// System.Net.Http.dll!System.Net.Http.HttpClient.cctor
		// https://github.com/dotnet/runtime/blob/6be6c5de821e389c986b0926fb7334017decee54/src/libraries/System.Net.Http/src/System/Net/Http/HttpClientHandler.AnyMobile.InvokeNativeHandler.cs#L146-L152
		internal static HttpMessageHandler GetHttpMessageHandler ()
		{
			if (Runtime.UseNSUrlSessionHandler)
				return new NSUrlSessionHandler ();

			if (Runtime.UseCFNetworkHandler)
				return new CFNetworkHandler ();

			return new HttpClientHandler ();
		}
	}
}
