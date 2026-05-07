#if __MACOS__
using System.Reflection;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.Mac.Tests {
	public static class Asserts {
		public static bool SkipDueToAvailabilityAttribute (ICustomAttributeProvider member)
		{
			if (member is null)
				return false;
			return !member.IsAvailableOnHostPlatform ();
		}
	}
}
#endif // __MACOS__
