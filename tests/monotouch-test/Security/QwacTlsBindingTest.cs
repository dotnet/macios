// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Security.Cryptography.X509Certificates;

using Foundation;
using NUnit.Framework;
using ObjCRuntime;
using Security;

namespace MonoTouchFixtures.Security {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class QwacTlsBindingTest {

		[Test]
		public void ErrorDomain ()
		{
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 3);

			Assert.That (SecQwacTlsBindingError.Malformed.GetDomain (), Is.Not.Null, "Domain");
		}

		[Test]
		public void InvalidArguments ()
		{
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 3);

			using var certificate = new SecCertificate (CertificateTest.mail_google_com);
			using var policy = SecPolicy.CreateSslPolicy (true, "mail.google.com");
			using var trust = new SecTrust (certificate, policy);
			using var data = new NSData ();

			Assert.Throws<ArgumentNullException> (() => SecTrust.CreateAndVerifyQwacTlsBinding (null, trust, out _), "bindingData");
			Assert.Throws<ArgumentNullException> (() => SecTrust.CreateAndVerifyQwacTlsBinding (data, null, out _), "tlsTrust");

			data.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => SecTrust.CreateAndVerifyQwacTlsBinding (data, trust, out _), "Disposed bindingData");

			using var validData = new NSData ();
			trust.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => SecTrust.CreateAndVerifyQwacTlsBinding (validData, trust, out _), "Disposed tlsTrust");
		}

		[Test]
		public void RejectMalformedBinding ()
		{
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 3);

			using var certificate = X509CertificateLoader.LoadCertificate (CertificateTest.mail_google_com);
			using var intermediate = X509CertificateLoader.LoadCertificate (CertificateTest.gts_ca_1c3);
			using var root = X509CertificateLoader.LoadCertificate (CertificateTest.gts_root_r1);
			var certificates = new X509Certificate2Collection { certificate, intermediate, root };
			using var anchor = new SecCertificate (root);
			using var policy = SecPolicy.CreateSslPolicy (true, "mail.google.com");
			using var trust = new SecTrust (certificates, policy);
			trust.NetworkFetchAllowed = false;
			Assert.That (trust.SetAnchorCertificates (new [] { anchor }), Is.EqualTo (SecStatusCode.Success), "Anchor");
			// Use a date within the fixture certificate's validity period.
			trust.SetVerifyDate (new DateTime (2025, 10, 1, 0, 0, 0, DateTimeKind.Utc));
			var tlsValid = trust.Evaluate (out var tlsError);
			using (tlsError)
				Assert.That (tlsValid, Is.True, tlsError?.ToString ());

			using var data = NSData.FromString ("not-a-jws-binding");
			using var result = SecTrust.CreateAndVerifyQwacTlsBinding (data, trust, out var error);
			using (error) {
				Assert.That (result, Is.Null, "Result");
				Assert.That (error, Is.Not.Null, "Error");
				var domain = SecQwacTlsBindingError.Malformed.GetDomain ();
				Assert.That (domain, Is.Not.Null, "Domain");
				Assert.That (error?.Domain, Is.EqualTo (domain?.ToString ()), "Error domain");
				Assert.That (error?.Code, Is.EqualTo ((nint) SecQwacTlsBindingError.Malformed).Or.EqualTo ((nint) SecQwacTlsBindingError.Unavailable), "Error code");
			}
		}
	}
}
