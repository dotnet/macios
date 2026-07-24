#if __MACOS__
#nullable enable

using System;
using NUnit.Framework;
using Security;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SecKeychainTest {

		[Test]
		public void GetTypeId ()
		{
			var typeId = SecKeychain.GetTypeId ();
			Assert.That ((int) typeId, Is.GreaterThan (0), "TypeID should be positive");
		}

		[Test]
		public void GetDefault ()
		{
			using var keychain = SecKeychain.GetDefault (out var status);
			if (status == 0)
				Assert.That (keychain, Is.Not.Null, "Default keychain");
			else
				Assert.That (keychain, Is.Null, "Default keychain on failure");
		}

		[Test]
		public void GetDefault_GetPath ()
		{
			using var keychain = SecKeychain.GetDefault (out var defaultStatus);
			Assert.That (defaultStatus, Is.EqualTo (0), "GetDefault status");
			Assert.That (keychain, Is.Not.Null, "Default keychain");
			if (keychain is null)
				return;
			var path = keychain.GetPath (out var pathStatus);
			Assert.That (pathStatus, Is.EqualTo (0), "GetPath status");
			Assert.That (path, Is.Not.Null, "Path should not be null");
			Assert.That (path, Does.EndWith (".keychain-db").Or.EndWith (".keychain"), "Path should end with .keychain or .keychain-db");
		}

		[Test]
		public void TryOpen_InvalidPath ()
		{
			var success = SecKeychain.TryOpen ("/nonexistent/path/fake.keychain", out var status, out var keychain);
			using (keychain) {
				Assert.That (success, Is.EqualTo (status == 0 && keychain is not null), "Success");
			}
		}

	}
}
#endif // __MACOS__
