#nullable enable
using System;
using System.Linq;
using System.IO;
using NUnit.Framework;
using ObjCRuntime;
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
			Assert.True (platformName.TryGetApplicationClassName (out var applicationClassName));
			Assert.AreEqual (expected, applicationClassName);
		}

		[TestCase (PlatformName.iOS, "CoreImage")]
		[TestCase (PlatformName.TvOS, "CoreImage")]
		[TestCase (PlatformName.MacCatalyst, "CoreImage")]
		[TestCase (PlatformName.MacOSX, "Quartz")]
		public void GetCoreImageMapTest (PlatformName platformName, string expected)
			=> Assert.AreEqual (expected, platformName.GetCoreImageMap ());

		[TestCase (PlatformName.iOS, "MobileCoreServices")]
		[TestCase (PlatformName.TvOS, "MobileCoreServices")]
		[TestCase (PlatformName.MacCatalyst, "MobileCoreServices")]
		[TestCase (PlatformName.MacOSX, "CoreServices")]
		public void GetCoreServicesMap (PlatformName platformName, string expected)
			=> Assert.AreEqual (expected, platformName.GetCoreServicesMap ());

		[TestCase (PlatformName.iOS, "PDFKit")]
		[TestCase (PlatformName.MacCatalyst, "PDFKit")]
		[TestCase (PlatformName.MacOSX, "Quartz")]
		public void GetPDFKitMapTest (PlatformName platformName, string expected)
			=> Assert.AreEqual (expected, platformName.GetPDFKitMap ());

		[TestCase (PlatformName.iOS, ApplePlatform.iOS)]
		[TestCase (PlatformName.TvOS, ApplePlatform.TVOS)]
		[TestCase (PlatformName.MacCatalyst, ApplePlatform.MacCatalyst)]
		[TestCase (PlatformName.MacOSX, ApplePlatform.MacOSX)]
		[TestCase (PlatformName.None, ApplePlatform.None)]
		public void AsApplePlatformTest (PlatformName platformName, ApplePlatform expected)
			=> Assert.AreEqual (expected, platformName.AsApplePlatform ());
	}
}
