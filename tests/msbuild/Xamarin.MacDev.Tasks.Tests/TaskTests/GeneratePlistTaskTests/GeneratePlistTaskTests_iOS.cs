using System.Linq;

using NUnit.Framework;
using Xamarin.MacDev;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GeneratePlistTaskTests_iOS : GeneratePlistTaskTests_Core {
		protected override ApplePlatform Platform => ApplePlatform.iOS;

		protected override void ConfigureTask ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);

			base.ConfigureTask ();
			Task.DefaultSdkVersion = Task.CurrentSdk.GetClosestInstalledSdk (AppleSdkVersion.V6_1, true).ToString () ?? "";
			Task.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			Task.TargetArchitectures = "ARM64";
		}

		[Test]
		public override void BundleExecutable ()
		{
			base.BundleExecutable ();
			// Adding ".app" to the assembly name isn't allowed because iOS may fail to launch the app.
			Task.BundleExecutable = "AssemblyName.app";
			Assert.That (Task.Execute (), Is.False, "#1");
		}

		[Test]
		public override void BundleName ()
		{
			Assert.That (CompiledPlist.ContainsKey (ManifestKeys.CFBundleName), "#1");
			Assert.That (appBundleName, Is.EqualTo (CompiledPlist.Get<PString> (ManifestKeys.CFBundleName)?.Value), "#2");
		}

		[Test]
		public void RequiredDeviceCapabilities ()
		{
			PArray? array;

			Assert.That (CompiledPlist.TryGetValue (ManifestKeys.UIRequiredDeviceCapabilities, out array), Is.True, "#1");
			Assert.That (array?.OfType<PString> ().Any (x => x.Value == "arm64") == true, Is.True, "#2");
			Assert.That (array?.OfType<PString> ().Any (x => x.Value == "armv6") == true, Is.False, "#3");
			Assert.That (array?.OfType<PString> ().Any (x => x.Value == "armv7") == true, Is.False, "#4");
		}
	}
}
