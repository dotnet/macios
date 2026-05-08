#if MONOMAC

using System.Drawing;
using System.Threading;
using CoreWlan;
using Security;

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
			domain = CWKeychainDomain.None;
			// always return a new one so that test do not mess with eachother
			ssid = NSData.FromString (Guid.NewGuid ().ToString ());
		}


		[Test]
		public void TryFindWiFiEAPIdentityMissingTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out var secIdentity), "A");
				ClassicAssert.IsNull (secIdentity, "A Identity");

				ClassicAssert.False (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out secIdentity, out var status), "B");
				ClassicAssert.IsNull (secIdentity, "B Identity");
				ClassicAssert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
			});
		}

		[Test]
		public void TryDeleteWiFiEAPUsernameAndPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), "A");

				ClassicAssert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out var status));
				ClassicAssert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
			});
		}

		[Test]
		public void TryDeleteWiFiPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TryDeleteWiFiPassword (domain, ssid), "A");

				ClassicAssert.False (CWKeychain.TryDeleteWiFiPassword (domain, ssid, out var status), "B");
				ClassicAssert.AreEqual (SecStatusCode.Param, (SecStatusCode) status, "Status B");
			});
		}

		[Test]
		public void TryFindWiFiEAPUsernameAndPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out string username, out string password), "A");
				ClassicAssert.IsNull (username, "A username");
				ClassicAssert.IsNull (password, "A password");

				ClassicAssert.False (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out username, out password, out var status), "B");
				ClassicAssert.IsNull (username, "B username");
				ClassicAssert.IsNull (password, "B password");
				ClassicAssert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status B");
			});
		}

		[Test]
		public void TryFindWiFiPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TryFindWiFiPassword (domain, ssid, out string password), "A");
				ClassicAssert.IsNull (password, "A password");

				ClassicAssert.False (CWKeychain.TryFindWiFiPassword (domain, ssid, out password, out var status), "B");
				ClassicAssert.IsNull (password, "B password");
				ClassicAssert.AreEqual (SecStatusCode.Param, (SecStatusCode) status, "Status B");
			});
		}

		[Test]
		public void TrySetWiFiEAPIdentityTest ()
		{
			var identity = IdentityTest.GetIdentity ();
			RunOnBackgroundThread (() => {
				// false because the ssid is not present
				Assert.That (CWKeychain.TrySetWiFiEAPIdentity (domain, ssid, identity), Is.True.Or.False, "A");

				Assert.That (CWKeychain.TrySetWiFiEAPIdentity (domain, ssid, identity, out var status), Is.True.Or.False, "B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Success).Or.EqualTo (SecStatusCode.Allocate), "Status B");

				// remove it to clean behind
				ClassicAssert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), "C");

				ClassicAssert.False (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out status), "D");
				ClassicAssert.AreEqual (SecStatusCode.ItemNotFound, (SecStatusCode) status, "Status D");
			});
		}

		[Test]
		public void TrySetWiFiEAPUsernameAndPasswordTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.True (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test"), "Both present A");
				ClassicAssert.True (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test", out var status), "Both present B");
				ClassicAssert.AreEqual (SecStatusCode.Success, (SecStatusCode) status, "Both present B Status");

				ClassicAssert.True (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null), "Null pwd A");
				ClassicAssert.True (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null, out status), "Null pwd B");
				ClassicAssert.AreEqual (SecStatusCode.Success, (SecStatusCode) status, "Null pwd B Status");

				ClassicAssert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test"), "Null user A");
				ClassicAssert.False (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test", out status), "Null user B");
				ClassicAssert.AreEqual (SecStatusCode.Param, (SecStatusCode) status, "Null user B Status");
			});
		}

		[Test]
		public void TrySetWiFiPasswordTest ()
		{
			RunOnBackgroundThread (() => {
				ClassicAssert.False (CWKeychain.TrySetWiFiPassword (domain, ssid, "password"), "A");

				ClassicAssert.False (CWKeychain.TrySetWiFiPassword (domain, ssid, "password", out var status), "B");
				ClassicAssert.AreEqual (SecStatusCode.Param, (SecStatusCode) status, "Status B");
			});
		}


		void RunOnBackgroundThread (Action action)
		{
			Exception ex = null;

			var thread = new Thread (() => {
				try {
					Assert.Multiple (() => action ());
				} catch (Exception e) {
					ex = e;
				}
			});
			thread.Start ();
			if (!thread.Join (TimeSpan.FromSeconds (10)))
				Assert.Fail ("Test timed out");
			ClassicAssert.IsNull (ex, "No exception");
		}
	}
}
#endif
