using NUnit.Framework;

using Xamarin.MacDev;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GeneratePlistTaskTests_tvOS : GeneratePlistTaskTests_Core {
		protected override ApplePlatform Platform => ApplePlatform.TVOS;

		protected override void ConfigureTask ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.TVOS);

			base.ConfigureTask ();
			Task.DefaultSdkVersion = Sdks.TVOS.GetClosestInstalledSdk (AppleSdkVersion.V9_0, true).ToString ();
			Task.TargetFrameworkMoniker = TargetFramework.DotNet_tvOS_String;
		}
	}
}
