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
		public void SimulatorRuntimeSelection ()
		{
			AssertSimulatorRuntime (
				CreateSimulatorXml (
					("com.apple.CoreSimulator.SimRuntime.iOS-26-2", 1704448),
					("com.apple.CoreSimulator.SimRuntime.iOS-26-3", 1704705)
				),
				"com.apple.CoreSimulator.SimRuntime.iOS-26-2");

			AssertSimulatorRuntime (
				CreateSimulatorXml (
					("com.apple.CoreSimulator.SimRuntime.iOS-26-1", 1704192),
					("com.apple.CoreSimulator.SimRuntime.iOS-26-3", 1704705)
				),
				"com.apple.CoreSimulator.SimRuntime.iOS-26-3");
		}

		void AssertSimulatorRuntime (string simulatorXml, string expectedRuntime)
		{
			var task = CreateTask<GetMlaunchArguments> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.AppManifestPath = CreateAppManifest ();
			task.LaunchApp = "MySimpleApp.app";
			task.MlaunchPath = CreateMlaunch (simulatorXml);
			task.SdkIsSimulator = true;
			task.SdkVersion = "26.2";
			task.WaitForExit = true;

			ExecuteTask (task);

			Assert.That (task.MlaunchArguments, Does.Contain ($"--device :v2:runtime={expectedRuntime},devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-17"));
		}

		string CreateAppManifest ()
		{
			var appManifestPath = Path.Combine (Cache.CreateTemporaryDirectory ("msbuild-tests"), "Info.plist");
			File.WriteAllText (appManifestPath, @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>UIDeviceFamily</key>
	<array>
		<integer>1</integer>
	</array>
</dict>
</plist>
");
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

		string CreateSimulatorXml (params (string Identifier, long Version) [] runtimes)
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
			xml.AppendLine ("      <SimDeviceType>");
			xml.AppendLine ("        <Identifier>com.apple.CoreSimulator.SimDeviceType.iPhone-16</Identifier>");
			xml.AppendLine ("        <ProductFamilyId>iPhone</ProductFamilyId>");
			xml.AppendLine ("        <MinRuntimeVersion>1704448</MinRuntimeVersion>");
			xml.AppendLine ("        <MaxRuntimeVersion>4294967295</MaxRuntimeVersion>");
			xml.AppendLine ("      </SimDeviceType>");
			xml.AppendLine ("      <SimDeviceType>");
			xml.AppendLine ("        <Identifier>com.apple.CoreSimulator.SimDeviceType.iPhone-17</Identifier>");
			xml.AppendLine ("        <ProductFamilyId>iPhone</ProductFamilyId>");
			xml.AppendLine ("        <MinRuntimeVersion>1704705</MinRuntimeVersion>");
			xml.AppendLine ("        <MaxRuntimeVersion>4294967295</MaxRuntimeVersion>");
			xml.AppendLine ("      </SimDeviceType>");
			xml.AppendLine ("    </SupportedDeviceTypes>");
			xml.AppendLine ("  </Simulator>");
			xml.AppendLine ("</MTouch>");
			return xml.ToString ();
		}
	}
}
