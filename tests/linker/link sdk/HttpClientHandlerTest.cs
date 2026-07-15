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
				Assert.That (handler.AllowAutoRedirect, Is.True, "AllowAutoRedirect");
				Assert.That (handler.CookieContainer, Is.Not.Null, "CookieContainer");
				Assert.That (handler.Credentials, Is.Null, "Credentials");
				// (so far) not exposed in other, native handlers
				Assert.That (handler.AutomaticDecompression, Is.EqualTo (DecompressionMethods.None), "AutomaticDecompression");
				Assert.That (handler.ClientCertificateOptions, Is.EqualTo (ClientCertificateOption.Manual), "ClientCertificateOptions");
				Assert.That (handler.MaxAutomaticRedirections, Is.EqualTo (50), "MaxAutomaticRedirections");
				Assert.That (handler.Proxy, Is.Null, "Proxy");
				Assert.That (handler.SupportsAutomaticDecompression, Is.True, "SupportsAutomaticDecompression");
				Assert.That (handler.SupportsProxy, Is.True, "SupportsProxy");
				Assert.That (handler.SupportsRedirectConfiguration, Is.True, "SupportsRedirectConfiguration");
				Assert.That (handler.UseCookies, Is.True, "UseCookies");
				Assert.That (handler.UseDefaultCredentials, Is.False, "UseDefaultCredentials");
				Assert.That (handler.UseProxy, Is.True, "UseProxy");
			}
		}

		[Test]
		public void CFNetwork ()
		{
			using (var handler = new CFNetworkHandler ()) {
				Assert.That (handler.AllowAutoRedirect, Is.True, "AllowAutoRedirect");
				Assert.That (handler.CookieContainer, Is.Not.Null, "CookieContainer");
				// custom, not in HttpClientHandler
				Assert.That (handler.UseSystemProxy, Is.False, "UseSystemProxy");
			}
		}

		[Test]
		public void NSUrlSession ()
		{
			using (var handler = new NSUrlSessionHandler ()) {
				Assert.That (handler.AllowAutoRedirect, Is.True, "AllowAutoRedirect");
				Assert.That (handler.Credentials, Is.Null, "Credentials");
				// custom, not in HttpClientHandler
				Assert.That (handler.DisableCaching, Is.False, "DisableCaching");
			}
		}
	}
}
