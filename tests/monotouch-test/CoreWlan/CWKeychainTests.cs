#if MONOMAC

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using CoreWlan;
using Security;

using NUnit.Framework;
using MonoTouchFixtures.Security;

namespace MonoTouchFixtures.CoreWlan {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CWKeychainTests {

		CWKeychainDomain domain;
		NSData ssid;

		[SetUp]
		public void SetUp ()
		{
			domain = CWKeychainDomain.System;
			// always return a new one so that test do not mess with eachother
			ssid = NSData.FromString (Guid.NewGuid ().ToString ());
		}

		[Test]
		public void TryFindWiFiEAPIdentityMissingTest ()
		{
			Assert.False (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out var secIdentity), "A");
			Assert.IsNull (secIdentity, "A Identity");

			Assert.False (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out secIdentity, out var status), "B");
			Assert.IsNull (secIdentity, "B Identity");
			Assert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
		}

		[Test]
		public void TryDeleteWiFiEAPUsernameAndPasswordMissingTest ()
		{
			Assert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), "A");

			Assert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out var status));
			Assert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
		}

		[Test]
		public void TryDeleteWiFiPasswordMissingTest ()
		{
			Assert.True (CWKeychain.TryDeleteWiFiPassword (domain, ssid), "A");

			Assert.True (CWKeychain.TryDeleteWiFiPassword (domain, ssid, out var status), "B");
			Assert.AreEqual (SecStatusCode.Success, (SecStatusCode) status, "Status B");
		}

		[Test]
		public void TryFindWiFiEAPUsernameAndPasswordMissingTest ()
		{
			Assert.False (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out string username, out string password), "A");
			Assert.IsNull (username, "A username");
			Assert.IsNull (password, "A password");

			Assert.False (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out username, out password, out var status), "B");
			Assert.IsNull (username, "B username");
			Assert.IsNull (password, "B password");
			Assert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
		}

		[Test]
		public void TryFindWiFiPasswordMissingTest ()
		{
			Assert.False (CWKeychain.TryFindWiFiPassword (domain, ssid, out string password), "A");
			Assert.IsNull (password, "A password");

			Assert.False (CWKeychain.TryFindWiFiPassword (domain, ssid, out password, out var status), "B");
			Assert.IsNull (password, "B password");
			Assert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
		}

		[Test]
		public void TrySetWiFiEAPIdentityTest ()
		{
			// false because the ssid is not present
			var identity = IdentityTest.GetIdentity ();
			Assert.False (CWKeychain.TrySetWiFiEAPIdentity (domain, ssid, identity), "A");

			Assert.False (CWKeychain.TrySetWiFiEAPIdentity (domain, ssid, identity, out var status), "B");
			Assert.AreEqual (SecStatusCode.Allocate, (SecStatusCode) status, "Status B");

			// remove it to clean behind
			Assert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), "C");

			Assert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out status), "D");
			Assert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status D");
		}

		[Test]
		public void TrySetWiFiEAPUsernameAndPasswordTest ()
		{
			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test"), "Both present A");
			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test", out var status), "Both present B");
			Assert.AreEqual (SecStatusCode.Allocate, (SecStatusCode) status, "Both present B Status");

			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null), "Null pwd A");
			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null, out status), "Null pwd B");
			Assert.AreEqual (SecStatusCode.Allocate, (SecStatusCode) status, "Null pwd B Status");

			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test"), "Null user A");
			Assert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test", out status), "Null user B");
			Assert.AreEqual (SecStatusCode.Param, (SecStatusCode) status, "Null user B Status");
		}

		[Test]
		public void TrySetWiFiPasswordTest ()
		{
			Assert.True (CWKeychain.TrySetWiFiPassword (domain, ssid, "password"), "A");

			Assert.True (CWKeychain.TrySetWiFiPassword (domain, ssid, "password", out var status), "B");
			Assert.AreEqual (SecStatusCode.Success, (SecStatusCode) status, "Status B");
		}

	}
}
#endif
