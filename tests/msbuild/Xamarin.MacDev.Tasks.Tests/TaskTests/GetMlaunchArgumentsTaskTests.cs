// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GetMlaunchArgumentsTaskTests : TestBase {

		[Test]
		public void SelectSimulatorDeviceUsesFirstAvailableSimulator ()
		{
			var task = CreateTask<GetMlaunchArguments> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.AppManifestPath = CreateAppManifest (1, 2);
			task.Devices = CreateDevices (
				("DEVICE-1", "Connected iPhone", "Device"),
				("SIM-2", "Preferred Simulator", "Simulator"),
				("SIM-1", "Another Simulator", "Simulator")
			);
			task.LaunchApp = "MySimpleApp.app";
			task.MlaunchPath = "/usr/bin/false";
			task.SdkIsSimulator = true;
			task.SdkVersion = "26.2";
			task.WaitForExit = true;

			ExecuteTask (task);

			Assert.That (task.MlaunchArguments, Does.Contain ("--device :v2:udid=SIM-2"));
		}

		static TaskItem [] CreateDevices (params (string Udid, string Name, string Type) [] devices)
		{
			return devices.Select (v => {
				var item = new TaskItem (v.Udid);
				item.SetMetadata ("Description", v.Name);
				item.SetMetadata ("Name", v.Name);
				item.SetMetadata ("Type", v.Type);
				item.SetMetadata ("UDID", v.Udid);
				return item;
			}).ToArray ();
		}

		static string CreateAppManifest (params int [] deviceFamilies)
		{
			var appManifestPath = Path.Combine (Cache.CreateTemporaryDirectory ("msbuild-tests"), "Info.plist");
			var plist = new StringBuilder ();
			plist.AppendLine (@"<?xml version=""1.0"" encoding=""UTF-8""?>");
			plist.AppendLine (@"<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">");
			plist.AppendLine (@"<plist version=""1.0"">");
			plist.AppendLine ("<dict>");
			plist.AppendLine ("\t<key>UIDeviceFamily</key>");
			plist.AppendLine ("\t<array>");
			foreach (var family in deviceFamilies)
				plist.AppendLine ($"\t\t<integer>{family}</integer>");
			plist.AppendLine ("\t</array>");
			plist.AppendLine ("</dict>");
			plist.AppendLine ("</plist>");
			File.WriteAllText (appManifestPath, plist.ToString ());
			return appManifestPath;
		}
	}
}
