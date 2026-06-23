//
// Unit tests for NSUrlCredential
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
using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlCredentialTest {

		SecTrust GetTrust ()
		{
			var x = X509CertificateLoader.LoadCertificate (CertificateTest.mail_google_com);
			using (var policy = SecPolicy.CreateBasicX509Policy ())
				return new SecTrust (x, policy);
		}

		[Test]
		public void Ctor_Trust ()
		{
			using (var trust = GetTrust ())
			using (var creds = new NSUrlCredential (trust)) {
				Assert.That (creds.Certificates, Is.Null, "Certificates");
				Assert.That (creds.HasPassword, Is.False, "HasPassword");
				Assert.That (creds.SecIdentity, Is.Null, "SecIdentity");
				Assert.That (creds.Password, Is.Null, "Password");
				Assert.That (creds.Persistence, Is.EqualTo (NSUrlCredentialPersistence.ForSession), "Persistence");
				Assert.That (creds.User, Is.Null, "User");
			}
		}

		[Test]
		public void FromTrust ()
		{
			using (var trust = GetTrust ())
			using (var creds = NSUrlCredential.FromTrust (trust)) {
				Assert.That (creds.Certificates, Is.Null, "Certificates");
				Assert.That (creds.HasPassword, Is.False, "HasPassword");
				Assert.That (creds.SecIdentity, Is.Null, "SecIdentity");
				Assert.That (creds.Password, Is.Null, "Password");
				Assert.That (creds.Persistence, Is.EqualTo (NSUrlCredentialPersistence.ForSession), "Persistence");
				Assert.That (creds.User, Is.Null, "User");
			}
		}
	}
}
