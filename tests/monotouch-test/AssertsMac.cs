#if __MACOS__
using System.Reflection;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.Mac.Tests {
	public static class Asserts {
		public static bool IsAtLeastYosemite {
			get {
				return true;
			}
		}

		public static bool IsAtLeastElCapitan {
			get {
				return true;
			}
		}

		public static void EnsureYosemite ()
		{
		}

		public static void EnsureMavericks ()
		{
		}

		public static void EnsureMountainLion ()
		{
			// We're always running on at least Mountain Lion
		}

		public static void Ensure64Bit ()
		{
			if (IntPtr.Size == 4)
				Assert.Pass ("This test requires 64-bit.  Skipping");
		}

		public static bool SkipDueToAvailabilityAttribute (ICustomAttributeProvider member)
		{
			if (member is null)
				return false;
			return !member.IsAvailableOnHostPlatform ();
		}
	}
}
#endif // __MACOS__
