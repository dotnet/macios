#if __MACOS__
#nullable enable

using System;
using System.IO;
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
			var path = Path.Combine (Path.GetTempPath (), Guid.NewGuid ().ToString (), "fake.keychain");
			var success = SecKeychain.TryOpen (path, out var status, out var keychain);
			using (keychain) {
				Assert.That (success, Is.False, "Success");
				Assert.That (keychain, Is.Null, "Keychain");
				Assert.That (status, Is.Not.EqualTo (0), "Status");
			}
		}

		[Test]
		public void TryOpen_DefaultKeychainPath ()
		{
			using var defaultKeychain = SecKeychain.GetDefault (out var defaultStatus);
			Assert.That (defaultStatus, Is.EqualTo (0), "GetDefault status");
			Assert.That (defaultKeychain, Is.Not.Null, "Default keychain");
			if (defaultKeychain is null)
				return;
			var path = defaultKeychain.GetPath (out var pathStatus);
			Assert.That (pathStatus, Is.EqualTo (0), "GetPath status");
			Assert.That (path, Is.Not.Null, "Path");
			if (path is null)
				return;

			var success = SecKeychain.TryOpen (path, out var status, out var keychain);
			using (keychain) {
				Assert.That (success, Is.True, "Success");
				Assert.That (status, Is.EqualTo (0), "Status");
				Assert.That (keychain, Is.Not.Null, "Keychain");
			}
		}

	}
}
#endif // __MACOS__
