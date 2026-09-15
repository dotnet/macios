#nullable enable

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Utilities;
using NUnit.Framework;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ReadAppManifestTaskTests : TestBase {
		ReadAppManifest CreateTask (ApplePlatform platform = ApplePlatform.iOS, Action<PDictionary>? createDictionary = null)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();

			var plistPath = Path.Combine (tmpdir, "TemporaryAppManifest.plist");
			var plist = new PDictionary ();
			if (createDictionary is not null)
				createDictionary (plist);
			plist.Save (plistPath);

			var task = CreateTask<ReadAppManifest> ();
			task.AppManifest = new TaskItem (plistPath);
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();

			return task;
		}

		[Test]
		public void ReadsApplicationMetadata ()
		{
			var task = CreateTask (createDictionary: (plist) => {
				plist ["CFBundleDisplayName"] = "$(PRODUCT_NAME)";
				plist ["CFBundleName"] = "Bundle Name";
				plist ["CFBundleIdentifier"] = "com.xamarin.custom";
				plist ["CFBundleShortVersionString"] = "2.3.4";
				plist ["CFBundleVersion"] = "42";
			});
			ExecuteTask (task);
			Assert.Multiple (() => {
				Assert.That (task.CFBundleDisplayName, Is.EqualTo ("$(PRODUCT_NAME)"), "CFBundleDisplayName");
				Assert.That (task.CFBundleName, Is.EqualTo ("Bundle Name"), "CFBundleName");
				Assert.That (task.CFBundleIdentifier, Is.EqualTo ("com.xamarin.custom"), "CFBundleIdentifier");
				Assert.That (task.CFBundleShortVersionString, Is.EqualTo ("2.3.4"), "CFBundleShortVersionString");
				Assert.That (task.CFBundleVersion, Is.EqualTo ("42"), "CFBundleVersion");
			});
		}

		[Test]
		public void MacCatalystVersionConversion ()
		{
			var task = CreateTask (platform: ApplePlatform.MacCatalyst, (plist) => {
				plist.SetMinimumSystemVersion ("10.15.2");
			});
			ExecuteTask (task);
			Assert.That (task.MinimumOSVersion, Is.EqualTo ("13.3"), "MinimumOSVersion");
		}

		[Test]
		public void MacCatalystVersionConversionError ()
		{
			var task = CreateTask (platform: ApplePlatform.MacCatalyst, (plist) => {
				plist.SetMinimumSystemVersion ("10.0");
			});
			ExecuteTask (task, expectedErrorCount: 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.StartWith ("Could not map the macOS version 10.0 to a corresponding Mac Catalyst version. Valid macOS versions are:"));
		}
	}
}
