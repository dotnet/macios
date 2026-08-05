#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Security;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SecKeychainSettingsTest {

		[Test]
		public void Create_DefaultValues ()
		{
			var settings = SecKeychainSettings.Create ();
			Assert.That (settings.Version, Is.EqualTo (1), "Version should be 1");
			Assert.That (settings.LockOnSleep, Is.False, "LockOnSleep should default to false");
			Assert.That (settings.UseLockInterval, Is.False, "UseLockInterval should default to false");
			Assert.That (settings.LockInterval, Is.EqualTo (0), "LockInterval should default to 0");
		}

		[Test]
		public void Properties_RoundTrip ()
		{
			var settings = SecKeychainSettings.Create ();

			settings.LockOnSleep = true;
			Assert.That (settings.LockOnSleep, Is.True, "LockOnSleep");

			settings.UseLockInterval = true;
			Assert.That (settings.UseLockInterval, Is.True, "UseLockInterval");

			settings.LockInterval = 300;
			Assert.That (settings.LockInterval, Is.EqualTo (300), "LockInterval");

			settings.Version = 2;
			Assert.That (settings.Version, Is.EqualTo (2), "Version");
		}

		[Test]
		public void LockOnSleep_FalseRoundTrip ()
		{
			var settings = SecKeychainSettings.Create ();
			settings.LockOnSleep = true;
			settings.LockOnSleep = false;
			Assert.That (settings.LockOnSleep, Is.False, "LockOnSleep should be false after reset");
		}

		[Test]
		public void StructSize_IsBlittable ()
		{
			Assert.That (Marshal.SizeOf<SecKeychainSettings> (), Is.EqualTo (12), "Size");
			Assert.That (Marshal.OffsetOf<SecKeychainSettings> ("version").ToInt32 (), Is.EqualTo (0), "Version offset");
			Assert.That (Marshal.OffsetOf<SecKeychainSettings> ("lockOnSleep").ToInt32 (), Is.EqualTo (4), "LockOnSleep offset");
			Assert.That (Marshal.OffsetOf<SecKeychainSettings> ("useLockInterval").ToInt32 (), Is.EqualTo (5), "UseLockInterval offset");
			Assert.That (Marshal.OffsetOf<SecKeychainSettings> ("lockInterval").ToInt32 (), Is.EqualTo (8), "LockInterval offset");
		}
	}
}
#endif // __MACOS__
