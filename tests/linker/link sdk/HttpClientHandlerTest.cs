// Tests to ensure consistency of our handlers across updates
//
// Copyright 2016 Xamarin Inc.

using System.Net;
using System.Net.Http;

namespace LinkSdk.Net.Http {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class HttpClientHandlerTest {

		[Test]
		public void HttpClient ()
		{
			using (var handler = new HttpClientHandler ()) {
				ClassicAssert.True (handler.AllowAutoRedirect, "AllowAutoRedirect");
				ClassicAssert.NotNull (handler.CookieContainer, "CookieContainer");
				ClassicAssert.Null (handler.Credentials, "Credentials");
				// (so far) not exposed in other, native handlers
				Assert.That (handler.AutomaticDecompression, Is.EqualTo (DecompressionMethods.None), "AutomaticDecompression");
				Assert.That (handler.ClientCertificateOptions, Is.EqualTo (ClientCertificateOption.Manual), "ClientCertificateOptions");
				Assert.That (handler.MaxAutomaticRedirections, Is.EqualTo (50), "MaxAutomaticRedirections");
				ClassicAssert.Null (handler.Proxy, "Proxy");
				ClassicAssert.True (handler.SupportsAutomaticDecompression, "SupportsAutomaticDecompression");
				ClassicAssert.True (handler.SupportsProxy, "SupportsProxy");
				ClassicAssert.True (handler.SupportsRedirectConfiguration, "SupportsRedirectConfiguration");
				ClassicAssert.True (handler.UseCookies, "UseCookies");
				ClassicAssert.False (handler.UseDefaultCredentials, "UseDefaultCredentials");
				ClassicAssert.True (handler.UseProxy, "UseProxy");
			}
		}

		[Test]
		public void CFNetwork ()
		{
			using (var handler = new CFNetworkHandler ()) {
				ClassicAssert.True (handler.AllowAutoRedirect, "AllowAutoRedirect");
				ClassicAssert.NotNull (handler.CookieContainer, "CookieContainer");
				// custom, not in HttpClientHandler
				ClassicAssert.False (handler.UseSystemProxy, "UseSystemProxy");
			}
		}

		[Test]
		public void NSUrlSession ()
		{
			using (var handler = new NSUrlSessionHandler ()) {
				ClassicAssert.True (handler.AllowAutoRedirect, "AllowAutoRedirect");
				ClassicAssert.Null (handler.Credentials, "Credentials");
				// custom, not in HttpClientHandler
				ClassicAssert.False (handler.DisableCaching, "DisableCaching");
			}
		}
	}
}
