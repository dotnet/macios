#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Security {

	/// <summary>Represents keychain settings for lock behavior on macOS.</summary>
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct SecKeychainSettings {
		uint version;
		byte /* Boolean */ lockOnSleep;
		byte /* Boolean */ useLockInterval;
		uint lockInterval;

		/// <summary>Gets or sets the version of this settings structure.</summary>
		public uint Version {
			get => version;
			set => version = value;
		}

		/// <summary>Gets or sets whether the keychain locks when the system sleeps.</summary>
		public bool LockOnSleep {
			get => lockOnSleep != 0;
			set => lockOnSleep = value.AsByte ();
		}

		/// <summary>Gets or sets whether the lock interval is used.</summary>
		public bool UseLockInterval {
			get => useLockInterval != 0;
			set => useLockInterval = value.AsByte ();
		}

		/// <summary>Gets or sets the number of seconds before the keychain auto-locks.</summary>
		public uint LockInterval {
			get => lockInterval;
			set => lockInterval = value;
		}

		/// <summary>Creates a new <see cref="SecKeychainSettings" /> with the current version.</summary>
		/// <returns>A new <see cref="SecKeychainSettings" /> initialized with version 1.</returns>
		public static SecKeychainSettings Create ()
		{
			return new SecKeychainSettings { version = 1u };
		}
	}
}
#endif // __MACOS__
