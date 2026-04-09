// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Text;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GetMlaunchArgumentsTaskTests : TestBase {

		[Test]
		public void SelectSimulatorDeviceSortsByRuntimeDeviceTypeNameAndUdid ()
		{
			AssertSelectedSimulator (
				CreateSimulatorXml (
					runtimes: [
						("com.apple.CoreSimulator.SimRuntime.iOS-26-2", 1704448),
						("com.apple.CoreSimulator.SimRuntime.iOS-26-3", 1704705),
					],
					deviceTypes: [
						("com.apple.CoreSimulator.SimDeviceType.iPhone-16", "iPhone", 1704448, 4294967295),
						("com.apple.CoreSimulator.SimDeviceType.iPhone-17", "iPhone", 1704705, 4294967295),
					],
					availableDevices: [
						("UDID-5", "A Older Runtime", "com.apple.CoreSimulator.SimRuntime.iOS-26-2", "com.apple.CoreSimulator.SimDeviceType.iPhone-17"),
						("UDID-4", "A Older Device Type", "com.apple.CoreSimulator.SimRuntime.iOS-26-3", "com.apple.CoreSimulator.SimDeviceType.iPhone-16"),
						("UDID-3", "B Name", "com.apple.CoreSimulator.SimRuntime.iOS-26-3", "com.apple.CoreSimulator.SimDeviceType.iPhone-17"),
						("UDID-2", "A Name", "com.apple.CoreSimulator.SimRuntime.iOS-26-3", "com.apple.CoreSimulator.SimDeviceType.iPhone-17"),
						("UDID-1", "A Name", "com.apple.CoreSimulator.SimRuntime.iOS-26-3", "com.apple.CoreSimulator.SimDeviceType.iPhone-17"),
					]
				),
				"UDID-1",
				[1, 2]);
		}

		[Test]
		public void SelectSimulatorDeviceFiltersNonApplicableDevices ()
		{
			AssertSelectedSimulator (
				CreateSimulatorXml (
					runtimes: [
						("com.apple.CoreSimulator.SimRuntime.iOS-26-2", 1704448),
						("com.apple.CoreSimulator.SimRuntime.iOS-26-3", 1704705),
						("com.apple.CoreSimulator.SimRuntime.tvOS-26-2", 1704448),
					],
					deviceTypes: [
						("com.apple.CoreSimulator.SimDeviceType.iPhone-17", "iPhone", 1704705, 4294967295),
						("com.apple.CoreSimulator.SimDeviceType.iPad-Pro", "iPad", 1704448, 4294967295),
						("com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K", "Apple TV", 1704448, 4294967295),
					],
					availableDevices: [
						("UDID-IPHONE", "Newest iPhone", "com.apple.CoreSimulator.SimRuntime.iOS-26-3", "com.apple.CoreSimulator.SimDeviceType.iPhone-17"),
						("UDID-TV", "Apple TV", "com.apple.CoreSimulator.SimRuntime.tvOS-26-2", "com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K"),
						("UDID-IPAD", "Newest iPad", "com.apple.CoreSimulator.SimRuntime.iOS-26-2", "com.apple.CoreSimulator.SimDeviceType.iPad-Pro"),
					]
				),
				"UDID-IPAD",
				[2]);
		}

		void AssertSelectedSimulator (string simulatorXml, string expectedUdid, int [] deviceFamilies)
		{
			var task = CreateTask<GetMlaunchArguments> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.AppManifestPath = CreateAppManifest (deviceFamilies);
			task.LaunchApp = "MySimpleApp.app";
			task.MlaunchPath = CreateMlaunch (simulatorXml);
			task.SdkIsSimulator = true;
			task.SdkVersion = "26.2";
			task.WaitForExit = true;

			ExecuteTask (task);

			Assert.That (task.MlaunchArguments, Does.Contain ($"--device :v2:udid={expectedUdid}"));
		}

		string CreateAppManifest (int [] deviceFamilies)
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

		string CreateMlaunch (string simulatorXml)
		{
			var mlaunchPath = Path.Combine (Cache.CreateTemporaryDirectory ("msbuild-tests"), "mlaunch");
			var script = new StringBuilder ();
			script.AppendLine ("#!/bin/sh");
			script.AppendLine ("if [ \"$1\" = \"--listsim\" ]; then");
			script.AppendLine ("cat <<'EOF' > \"$2\"");
			script.AppendLine (simulatorXml);
			script.AppendLine ("EOF");
			script.AppendLine ("exit 0");
			script.AppendLine ("fi");
			script.AppendLine ("exit 1");
			File.WriteAllText (mlaunchPath, script.ToString ());
			if (OperatingSystem.IsMacOS ())
				File.SetUnixFileMode (mlaunchPath, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute | UnixFileMode.GroupRead | UnixFileMode.GroupExecute | UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
			return mlaunchPath;
		}

		string CreateSimulatorXml (
			(string Identifier, long Version) [] runtimes,
			(string Identifier, string ProductFamilyId, long MinRuntimeVersion, long MaxRuntimeVersion) [] deviceTypes,
			(string Udid, string Name, string Runtime, string DeviceType) [] availableDevices)
		{
			var xml = new StringBuilder ();
			xml.AppendLine ("<MTouch>");
			xml.AppendLine ("  <Simulator>");
			xml.AppendLine ("    <SupportedRuntimes>");
			foreach (var runtime in runtimes) {
				xml.AppendLine ("      <SimRuntime>");
				xml.AppendLine ($"        <Identifier>{runtime.Identifier}</Identifier>");
				xml.AppendLine ($"        <Version>{runtime.Version}</Version>");
				xml.AppendLine ("      </SimRuntime>");
			}
			xml.AppendLine ("    </SupportedRuntimes>");
			xml.AppendLine ("    <SupportedDeviceTypes>");
			foreach (var deviceType in deviceTypes) {
				xml.AppendLine ("      <SimDeviceType>");
				xml.AppendLine ($"        <Identifier>{deviceType.Identifier}</Identifier>");
				xml.AppendLine ($"        <ProductFamilyId>{deviceType.ProductFamilyId}</ProductFamilyId>");
				xml.AppendLine ($"        <MinRuntimeVersion>{deviceType.MinRuntimeVersion}</MinRuntimeVersion>");
				xml.AppendLine ($"        <MaxRuntimeVersion>{deviceType.MaxRuntimeVersion}</MaxRuntimeVersion>");
				xml.AppendLine ("      </SimDeviceType>");
			}
			xml.AppendLine ("    </SupportedDeviceTypes>");
			xml.AppendLine ("    <AvailableDevices>");
			foreach (var device in availableDevices) {
				xml.AppendLine ($"      <SimDevice UDID=\"{device.Udid}\" Name=\"{device.Name}\">");
				xml.AppendLine ($"        <SimRuntime>{device.Runtime}</SimRuntime>");
				xml.AppendLine ($"        <SimDeviceType>{device.DeviceType}</SimDeviceType>");
				xml.AppendLine ("      </SimDevice>");
			}
			xml.AppendLine ("    </AvailableDevices>");
			xml.AppendLine ("  </Simulator>");
			xml.AppendLine ("</MTouch>");
			return xml.ToString ();
		}
	}
}
