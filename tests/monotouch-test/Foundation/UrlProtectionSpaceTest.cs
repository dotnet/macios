//
// Unit tests for NSUrlProtectionSpace
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.Security.Cryptography.X509Certificates;
using Security;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

using MonoTouchFixtures.Security;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlProtectionSpaceTest {

		[Test]
		public void Http ()
		{
			using (var ps = new NSUrlProtectionSpace ("www.xamarin.com", 80, NSUrlProtectionSpace.HTTP, null, null)) {
				Assert.That (ps.AuthenticationMethod, Is.EqualTo ("NSURLAuthenticationMethodDefault"), "AuthenticationMethod");
				Assert.That (ps.DistinguishedNames, Is.Null, "DistinguishedNames");
				Assert.That (ps.Host, Is.EqualTo ("www.xamarin.com"), "Host");
				Assert.That (ps.IsProxy, Is.False, "IsProxy");
				Assert.That (ps.Port, Is.EqualTo ((nint) 80), "Port");
				Assert.That (ps.Protocol, Is.EqualTo ("http"), "Protocol");
				Assert.That (ps.ProxyType, Is.Null, "ProxyType");
				Assert.That (ps.Realm, Is.Null, "Realm");
				Assert.That (ps.ReceivesCredentialSecurely, Is.False, "ReceivesCredentialSecurely");
				Assert.That (ps.ServerSecTrust, Is.Null, "ServerSecTrust");
			}
		}

		[Test]
		public void Https ()
		{
			using (var ps = new NSUrlProtectionSpace ("mail.google.com", 443, NSUrlProtectionSpace.HTTPS, null, NSUrlProtectionSpace.AuthenticationMethodHTTPBasic)) {
				if (TestRuntime.CheckXcodeVersion (7, 0)) {
					Assert.That (ps.AuthenticationMethod, Is.EqualTo ("NSURLAuthenticationMethodHTTPBasic"), "AuthenticationMethod");
				} else {
					Assert.That (ps.AuthenticationMethod, Is.EqualTo ("NSURLAuthenticationMethodDefault"), "AuthenticationMethod");
				}
				Assert.That (ps.DistinguishedNames, Is.Null, "DistinguishedNames");
				Assert.That (ps.Host, Is.EqualTo ("mail.google.com"), "Host");
				Assert.That (ps.IsProxy, Is.False, "IsProxy");
				Assert.That (ps.Port, Is.EqualTo ((nint) 443), "Port");
				Assert.That (ps.Protocol, Is.EqualTo ("https"), "Protocol");
				Assert.That (ps.ProxyType, Is.Null, "ProxyType");
				Assert.That (ps.Realm, Is.Null, "Realm");
				Assert.That (ps.ReceivesCredentialSecurely, Is.True, "ReceivesCredentialSecurely");
				Assert.That (ps.ServerSecTrust, Is.Null, "ServerSecTrust");
			}
		}

		[Test]
		public void HttpProxy ()
		{
			using (var ps = new NSUrlProtectionSpace ("www.xamarin.com", 80, NSUrlProtectionSpace.HTTPProxy, "default", NSUrlProtectionSpace.AuthenticationMethodHTTPDigest, false)) {
				Assert.That (ps.AuthenticationMethod, Is.EqualTo ("NSURLAuthenticationMethodHTTPDigest"), "AuthenticationMethod");
				Assert.That (ps.DistinguishedNames, Is.Null, "DistinguishedNames");
				Assert.That (ps.Host, Is.EqualTo ("www.xamarin.com"), "Host");
				Assert.That (ps.IsProxy, Is.False, "IsProxy");
				Assert.That (ps.Port, Is.EqualTo ((nint) 80), "Port");
				Assert.That (ps.Protocol, Is.EqualTo ("http"), "Protocol");
				Assert.That (ps.ProxyType, Is.Null, "ProxyType");
				Assert.That (ps.Realm, Is.EqualTo ("default"), "Realm");
				Assert.That (ps.ReceivesCredentialSecurely, Is.True, "ReceivesCredentialSecurely");
				Assert.That (ps.ServerSecTrust, Is.Null, "ServerSecTrust");
			}
		}

		[Test]
		public void HttpProxy_Proxy ()
		{
			using (var ps = new NSUrlProtectionSpace ("www.xamarin.com", 80, NSUrlProtectionSpace.HTTPProxy, "default", NSUrlProtectionSpace.AuthenticationMethodHTTPDigest, true)) {
				Assert.That (ps.AuthenticationMethod, Is.EqualTo ("NSURLAuthenticationMethodHTTPDigest"), "AuthenticationMethod");
				Assert.That (ps.DistinguishedNames, Is.Null, "DistinguishedNames");
				Assert.That (ps.Host, Is.EqualTo ("www.xamarin.com"), "Host");
				Assert.That (ps.IsProxy, Is.True, "IsProxy");
				Assert.That (ps.Port, Is.EqualTo ((nint) 80), "Port");
				Assert.That (ps.Protocol, Is.EqualTo ("http"), "Protocol");
				Assert.That (ps.ProxyType, Is.EqualTo ("http"), "ProxyType");
				Assert.That (ps.Realm, Is.Null, "Realm");
				Assert.That (ps.ReceivesCredentialSecurely, Is.True, "ReceivesCredentialSecurely");
				Assert.That (ps.ServerSecTrust, Is.Null, "ServerSecTrust");
			}
		}
	}
}
