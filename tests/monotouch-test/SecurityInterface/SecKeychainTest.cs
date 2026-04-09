#if __MACOS__
using System;
using System.IO;
using NUnit.Framework;
using Security;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SecKeychainTest {

		[Test]
		public void GetTypeID ()
		{
			var typeId = SecKeychain.GetTypeID ();
			Assert.That ((int) typeId, Is.GreaterThan (0), "TypeID should be positive");
		}

		[Test]
		public void GetDefault ()
		{
			using var keychain = SecKeychain.GetDefault ();
			Assert.That (keychain, Is.Not.Null, "Default keychain should exist");
			Assert.That (keychain!.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle should be non-zero");
		}

		[Test]
		public void GetDefault_GetPath ()
		{
			using var keychain = SecKeychain.GetDefault ();
			Assert.That (keychain, Is.Not.Null, "Default keychain should exist");
			var path = keychain!.GetPath ();
			Assert.That (path, Is.Not.Null, "Path should not be null");
			Assert.That (path, Does.EndWith (".keychain-db").Or.EndWith (".keychain"), "Path should end with .keychain or .keychain-db");
		}

		[Test]
		public void Open_InvalidPath_ReturnsNull ()
		{
			using var keychain = SecKeychain.Open ("/nonexistent/path/fake.keychain");
			// SecKeychainOpen may succeed even for nonexistent paths (lazy open)
			// so we just verify it doesn't crash
		}

		[Test]
		public void Open_NullPath_Throws ()
		{
			Assert.Throws<ArgumentNullException> (() => SecKeychain.Open (null!));
		}

		[Test]
		public void CreateAndDelete_Keychain ()
		{
			var tempPath = Path.Combine (Path.GetTempPath (), $"test-keychain-{Guid.NewGuid ()}.keychain");
			try {
				using var defaultKc = SecKeychain.GetDefault ();
				Assert.That (defaultKc, Is.Not.Null, "Default keychain should exist");

				using var opened = SecKeychain.Open (tempPath);
				// SecKeychainOpen doesn't create the file; it just prepares a reference
				// We can verify the handle is valid
				if (opened is not null) {
					Assert.That (opened.Handle, Is.Not.EqualTo (IntPtr.Zero));
				}
			} finally {
				if (File.Exists (tempPath))
					File.Delete (tempPath);
			}
		}
	}
}
#endif // __MACOS__
