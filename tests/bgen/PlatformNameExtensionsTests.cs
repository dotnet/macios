#nullable enable
using System.Linq;
using System.IO;
using Xamarin.Utils;

namespace GeneratorTests {

	[TestFixture]
	public class PlatformNameExtensions {

		[TestCase (PlatformName.iOS, "UIApplication")]
		[TestCase (PlatformName.TvOS, "UIApplication")]
		[TestCase (PlatformName.MacCatalyst, "UIApplication")]
		[TestCase (PlatformName.MacOSX, "NSApplication")]
		public void GetApplicationClassNameTest (PlatformName platformName, string expected)
		{
			Assert.That (platformName.TryGetApplicationClassName (out var applicationClassName), Is.True);
			Assert.That (applicationClassName, Is.EqualTo (expected));
		}

		[TestCase (PlatformName.iOS, ApplePlatform.iOS)]
		[TestCase (PlatformName.TvOS, ApplePlatform.TVOS)]
		[TestCase (PlatformName.MacCatalyst, ApplePlatform.MacCatalyst)]
		[TestCase (PlatformName.MacOSX, ApplePlatform.MacOSX)]
		[TestCase (PlatformName.None, ApplePlatform.None)]
		public void AsApplePlatformTest (PlatformName platformName, ApplePlatform expected)
			=> Assert.That (platformName.AsApplePlatform (), Is.EqualTo (expected));
	}
}
