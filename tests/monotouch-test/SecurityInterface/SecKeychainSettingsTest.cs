#if __MACOS__
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
			// SecKeychainSettings has 4 fields: version(4) + lockOnSleep(1) + useLockInterval(1) + lockInterval(4) = 10
			// But with alignment it may be padded
			var size = Marshal.SizeOf<SecKeychainSettings> ();
			Assert.That (size, Is.GreaterThanOrEqualTo (10), "Struct should be at least 10 bytes");
			Assert.That (size, Is.LessThanOrEqualTo (16), "Struct should not exceed 16 bytes with padding");
		}
	}
}
#endif // __MACOS__
