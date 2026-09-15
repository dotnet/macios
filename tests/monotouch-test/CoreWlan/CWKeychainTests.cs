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
				Assert.That (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out var secIdentity), Is.False, "A");
				Assert.That (secIdentity, Is.Null, "A Identity");

				Assert.That (CWKeychain.TryFindWiFiEAPIdentity (domain, ssid, out secIdentity, out var status), Is.False, "B");
				Assert.That (secIdentity, Is.Null, "B Identity");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.ItemNotFound), "Status B");
			});
		}

		[Test]
		public void TryDeleteWiFiEAPUsernameAndPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), Is.False, "A");

				Assert.That (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out var status), Is.False);
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.ItemNotFound), "Status B");
			});
		}

		[Test]
		public void TryDeleteWiFiPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TryDeleteWiFiPassword (domain, ssid), Is.False, "A");

				Assert.That (CWKeychain.TryDeleteWiFiPassword (domain, ssid, out var status), Is.False, "B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Param), "Status B");
			});
		}

		[Test]
		public void TryFindWiFiEAPUsernameAndPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out string username, out string password), Is.False, "A");
				Assert.That (username, Is.Null, "A username");
				Assert.That (password, Is.Null, "A password");

				Assert.That (CWKeychain.TryFindWiFiEAPUsernameAndPassword (domain, ssid, out username, out password, out var status), Is.False, "B");
				Assert.That (username, Is.Null, "B username");
				Assert.That (password, Is.Null, "B password");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.ItemNotFound), "Status B");
			});
		}

		[Test]
		public void TryFindWiFiPasswordMissingTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TryFindWiFiPassword (domain, ssid, out string password), Is.False, "A");
				Assert.That (password, Is.Null, "A password");

				Assert.That (CWKeychain.TryFindWiFiPassword (domain, ssid, out password, out var status), Is.False, "B");
				Assert.That (password, Is.Null, "B password");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Param), "Status B");
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
				Assert.That (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid), Is.False, "C");

				Assert.That (CWKeychain.TryDeleteWiFiEAPUsernameAndPassword (domain, ssid, out status), Is.False, "D");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.ItemNotFound), "Status D");
			});
		}

		[Test]
		public void TrySetWiFiEAPUsernameAndPasswordTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test"), Is.True, "Both present A");
				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", "test", out var status), Is.True, "Both present B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Success), "Both present B Status");

				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null), Is.True, "Null pwd A");
				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, "mandel", null, out status), Is.True, "Null pwd B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Success), "Null pwd B Status");

				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test"), Is.False, "Null user A");
				Assert.That (CWKeychain.TrySetWiFiEAPUsernameAndPassword (domain, ssid, null, "test", out status), Is.False, "Null user B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Param), "Null user B Status");
			});
		}

		[Test]
		public void TrySetWiFiPasswordTest ()
		{
			RunOnBackgroundThread (() => {
				Assert.That (CWKeychain.TrySetWiFiPassword (domain, ssid, "password"), Is.False, "A");

				Assert.That (CWKeychain.TrySetWiFiPassword (domain, ssid, "password", out var status), Is.False, "B");
				Assert.That ((SecStatusCode) status, Is.EqualTo (SecStatusCode.Param), "Status B");
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
			Assert.That (ex, Is.Null, "No exception");
		}
	}
}
#endif
